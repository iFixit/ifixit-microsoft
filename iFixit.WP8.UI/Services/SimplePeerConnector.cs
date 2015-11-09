using iFixit.Domain.Code;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace iFixit.UI.Services
{
    public class SimplePeerConnector : Domain.Interfaces.IPeerConnector
    {
        public event EventHandler<Domain.Code.ConnectionStatusChangedEventArgs> ConnectionStatusChanged;

        public event EventHandler<Domain.Code.GuideReceivedEventArgs> GuideReceived;



        #region private members
        bool _peerFinderStarted = false;
        bool _socketClosed = true;
        DataWriter _dataWriter = null;
        StreamSocket _socket = null;
        #endregion



        public SimplePeerConnector()
        {
            // Note: Only tap & connect is currently supported
            if ((PeerFinder.SupportedDiscoveryTypes & PeerDiscoveryTypes.Triggered) != PeerDiscoveryTypes.Triggered)
            {
                UpdateConnectionStatus(ConnectionStatus.TapNotSupported);
                return;
            }

            PeerFinder.TriggeredConnectionStateChanged += TriggeredConnectionStateChanged;

            //foreach (var identity in alternateIdentities)
            //{
            //    // When setting the alternate identity, you only set the identity for the other
            //    // platform. For example, in the Windows Phone app, you only add the alternate identity for
            //    // the app on Windows. For Windows, you do the opposite.
            //    // If you attempt to add the same key an ArgumentException will be thrown, so check first
            //    if (!PeerFinder.AlternateIdentities.ContainsKey(identity.Key))
            //    {
            //        PeerFinder.AlternateIdentities.Add(identity.Key, identity.Value);
            //    }
            //}

            PeerFinder.AllowInfrastructure = true;
            PeerFinder.AllowBluetooth = true;

#if NETFX_CORE
                PeerFinder.AllowWiFiDirect = true;
#else
            PeerFinder.AllowWiFiDirect = false;
#endif
        }

        /// <summary>
        /// Start advertising for a peer connection
        /// </summary>
        public void StartConnect()
        {
            if (_peerFinderStarted) { return; }

            // Advertise ourselves for peer app discovery
            PeerFinder.Start();
            _peerFinderStarted = true;
            UpdateConnectionStatus(ConnectionStatus.ReadyForTap);
        }

        /// <summary>
        /// Stop advertising for a peer connection
        /// </summary>
        public void StopConnect()
        {
            PeerFinder.Stop();
            CloseSocket();
            _peerFinderStarted = false;

            UpdateConnectionStatus(ConnectionStatus.Disconnected);
        }

        /// <summary>
        /// Send picture or image bytearray to our peer over the connected socket
        /// </summary>
        /// <param name="imageBytes">The image to send</param>
        public async void SendGuideAsync([ReadOnlyArray] byte[] imageBytes)
        {
            if (!_socketClosed)
            {
                if (imageBytes.Length > 0)
                {
                    try
                    {
                        _dataWriter.WriteInt32(imageBytes.Length);
                        _dataWriter.WriteBytes(imageBytes);
                        uint numBytesWritten = await _dataWriter.StoreAsync();
                        if (numBytesWritten > 0)
                        {
                            Debug.WriteLine("Sent guide. Number of bytes written: {0} ", numBytesWritten);

                        }
                        else
                        {
                            SocketError("The remote side closed the socket");
                            UpdateConnectionStatus(ConnectionStatus.Disconnected);
                        }
                    }
                    catch (Exception err)
                    {
                        if (!_socketClosed)
                        {
                            SocketError("Failed to send message with error: " + err.Message);
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("BAD guide size:{0} ", imageBytes.Length);
                }
            }
            else
            {
                SocketError("The remote side closed the socket");
                UpdateConnectionStatus(ConnectionStatus.Disconnected);
            }
        }

        void TriggeredConnectionStateChanged(object sender, TriggeredConnectionStateChangedEventArgs e)
        {
            switch (e.State)
            {
                case TriggeredConnectState.PeerFound:
                    UpdateConnectionStatus(ConnectionStatus.PeerFound);
                    break;
                case TriggeredConnectState.Canceled:
                    UpdateConnectionStatus(ConnectionStatus.Canceled);
                    break;
                case TriggeredConnectState.Failed:
                    UpdateConnectionStatus(ConnectionStatus.Failed);
                    break;
                case TriggeredConnectState.Completed:
                    StartSendReceive(e.Socket);
                    UpdateConnectionStatus(ConnectionStatus.Completed);

                    // Stop advertising since we have connected
                    PeerFinder.Stop();
                    _peerFinderStarted = false;
                    break;
            }
        }

        void StartSendReceive(StreamSocket socket)
        {
            _socket = socket;

            if (!_peerFinderStarted)
            {
                CloseSocket();
                return;
            }

            _dataWriter = new DataWriter(_socket.OutputStream);
            _socketClosed = false;
            StartReceiveGuide(new DataReader(_socket.InputStream));
        }

        async void StartReceiveGuide(DataReader socketReader)
        {
            try
            {
                uint bytesRead = await socketReader.LoadAsync(sizeof(uint));
                if (bytesRead > 0)
                {
                    uint strLength = (uint)socketReader.ReadUInt32();
                    bytesRead = await socketReader.LoadAsync(strLength);
                    if (bytesRead > 0)
                    {
                        byte[] bytesIn = new byte[bytesRead];

                        Debug.WriteLine("ReadBytes");
                        socketReader.ReadBytes(bytesIn);
                        Debug.WriteLine("ReadBytes End");

                        // Raise PicReceived event
                        if (GuideReceived != null)
                        {
                            GuideReceivedEventArgs args = new GuideReceivedEventArgs();
                            args.Bytes = bytesIn;
                            GuideReceived(this, args);
                        }

                        StartReceiveGuide(socketReader); // Start another reader
                    }
                    else
                    {
                        SocketError("The remote side closed the socket");
                        socketReader.Dispose();
                        UpdateConnectionStatus(ConnectionStatus.Disconnected);
                    }
                }
                else
                {
                    SocketError("The remote side closed the socket");
                    socketReader.Dispose();
                    UpdateConnectionStatus(ConnectionStatus.Disconnected);
                }
            }
            catch (Exception e)
            {
                if (!_socketClosed)
                {
                    SocketError("Reading from socket failed: " + e.Message);
                }
                socketReader.Dispose();
            }
        }

        void CloseSocket()
        {
            if (_socket != null)
            {
                _socketClosed = true;
                _socket.Dispose();
                _socket = null;
            }

            if (_dataWriter != null)
            {
                _dataWriter.Dispose();
                _dataWriter = null;
            }
        }

        void SocketError(String errMessage)
        {
            Debug.WriteLine("SOCKET ERROR {0}", errMessage);
            CloseSocket();
        }

        void UpdateConnectionStatus(ConnectionStatus status)
        {
            var handler = ConnectionStatusChanged;
            if (handler == null)
                return;

            handler(this, new ConnectionStatusChangedEventArgs { Status = status });
        }
    }
}

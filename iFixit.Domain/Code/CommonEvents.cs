
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.Code
{
    public class GuideReceivedEventArgs : EventArgs
    {
        public byte[] Bytes { get;  set; }
    }

    public class ConnectionStatusChangedEventArgs : EventArgs
    {
        public ConnectionStatus Status { get; set; }
    }

    public enum ConnectionStatus
    {
        PeerFound,
        Completed,
        Canceled,
        Failed,
        ReadyForTap,
        Disconnected,
        TapNotSupported
    }
}

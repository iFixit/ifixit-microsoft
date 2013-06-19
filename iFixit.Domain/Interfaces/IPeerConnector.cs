using iFixit.Domain.Code;
using System;
namespace iFixit.Domain.Interfaces
{
   public interface IPeerConnector
    {
        event EventHandler<ConnectionStatusChangedEventArgs> ConnectionStatusChanged;
        event EventHandler<GuideReceivedEventArgs> GuideReceived;
        void SendGuideAsync(byte[] guideBytes);
        void StartConnect();
        void StopConnect();
    }
}

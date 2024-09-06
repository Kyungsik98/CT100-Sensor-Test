using System.IO.Ports;

namespace Robo_R.Domain.Driver
{
    public interface ICommunicator
    {
        bool Connect();
        void Disconnect();
        bool IsConnected();
        void ChangeSerialPort(string portName, int baudRate, StopBits stopBits);
    }
}

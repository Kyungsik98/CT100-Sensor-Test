using System.Diagnostics.Contracts;
using System.IO.Ports;
using System.Reflection.Metadata;
using Robo_R.CT100.Components;
using Robo_R.CT100.Driver;

namespace Communication
{
    class Program
    {
        private static SerialPort _serialPort = new SerialPort();
        private static CT100Driver _ct100Driver = new CT100Driver();
        private static Logger logger = new Logger("C:/Users/admin/Desktop/c# test/log.txt");
        static void Main(string[] args)
        {   
             var SenddataFrame = new List<byte>();
             var ReceivedDataFrame = new List<byte>();

            _serialPort.PortName = "COM13";
            _serialPort.BaudRate = 19200;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Parity = Parity.None;
            _serialPort.Handshake = Handshake.None;

            _serialPort.Open();
            Console.WriteLine("Serial Port Opened");

           
            
            for(int i = 0; i < 100; i++)
            {

                try 
                {   // 시리얼 버퍼에 남아있는 잔여 데이터 제거
                    while (_serialPort.BytesToRead > 0)
                    {
                        _= _serialPort.ReadByte();
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                finally
                {
                    _serialPort.Write(CT100Driver.MakeReadDataFrame(CT100Address.ObjectTemperature),0,8);
                    //_serialPort.Write(CT100Driver.MakeWriteDataFrame(CT100Address.ChangeEmissivity,80),0,8);
                    
                }

                //통신 응답 받기 위한 대기 (없으면 데이터 안받아옴)
                //Thread.Sleep(100);
                bool flag = false;
                Thread.Sleep(1000);
                
                while(flag == false)
                {   
                    
                    while(_serialPort.BytesToRead > 0)
                    {
                        var readByte = _serialPort.ReadByte();
                        ReceivedDataFrame.Add((byte)readByte);
                        Console.WriteLine("Received: " + readByte);
                    }

                    flag = true;

                }

                double temperature = (ReceivedDataFrame[3] * 256 + ReceivedDataFrame[4]) * 0.02 - 273.15;
                double PCBtemperature = (ReceivedDataFrame[5] * 256 + ReceivedDataFrame[6]) * 0.02 - 273.15;
                
                logger.Log(temperature.ToString());
                
                Console.Write(DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss "));
                Console.WriteLine(i+1 + "Temperature: " + Math.Round(temperature,3)+"°C");
                Console.WriteLine(i+1 + "PCB Temperature: " + Math.Round(PCBtemperature,3)+"°C");

                ReceivedDataFrame.Clear();
            }

            
            _serialPort.Close();
            Console.WriteLine("Serial Port Closed");
            
        }
        
    }
}
using System.Diagnostics.Contracts;
using System.IO.Ports;
using Robo_R.CT100.Components;
using Robo_R.CT100.Driver;

namespace Communication
{
    class Program
    {
        private static SerialPort _serialPort = new SerialPort();
        private static CT100Driver _ct100Driver = new CT100Driver();
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
            
            for(int i = 0; i < 5; i++)
            {
                //---set 방사율 data-----
                // SenddataFrame.Add(0x01);
                // SenddataFrame.Add(0x06);
                // SenddataFrame.Add(0x9C);
                // SenddataFrame.Add(0x41);
                // SenddataFrame.Add(0x00);
                // SenddataFrame.Add(0x50);

                //---get temperature---
                // SenddataFrame.Add(0x01);
                // SenddataFrame.Add(0x03);
                // SenddataFrame.Add(0x9C);
                // SenddataFrame.Add(0x42);
                // SenddataFrame.Add(0x00);
                // SenddataFrame.Add(0x02);

                


                // var CRC = CRC16.CalculateCRC(SenddataFrame.ToArray());
                // SenddataFrame.AddRange(CRC);          

                // for(int i = 0; i < 8; i++)
                // {
                //     Console.WriteLine("Send: "+SenddataFrame[i]);
                // }
                //_serialPort.Write(SenddataFrame.ToArray(),0,8);
                

                // //동작 확인
                 //_serialPort.Write(CT100Driver.MakeWriteDataFrame(CT100Address.ChangeEmissivity,80),0,8);

                //_serialPort.Write(CT100Driver.MakeWriteDataFrame(CT100Address.ChangeDeviceID,1),0,8);

                _serialPort.Write(CT100Driver.MakeReadDataFrame(CT100Address.ObjectTemperature),0,8);


                // for(int j = 0; j < 9 ; j++)
                // {   
                //     var readByte = _serialPort.ReadByte();
                //     ReceivedDataFrame.Add((byte)readByte);
                //     Console.WriteLine("Received: " + readByte);
                // }


                //통신 응답 받기 위한 대기 (없으면 데이터 안받아옴)
                Thread.Sleep(100);

                while(_serialPort.BytesToRead > 0)
                {   
                    var readByte = _serialPort.ReadByte();
                    ReceivedDataFrame.Add((byte)readByte);
                    Console.WriteLine("Received: " + readByte);
                }

                double temperature = (ReceivedDataFrame[3] * 256 + ReceivedDataFrame[4]) * 0.02 - 273.15;
                double PCBtemperature = (ReceivedDataFrame[5] * 256 + ReceivedDataFrame[6]) * 0.02 - 273.15;
                
                Console.WriteLine(i+1 + "Temperature: " + Math.Round(temperature,3)+"°C");
                Console.WriteLine(i+1 + "PCB Temperature: " + Math.Round(PCBtemperature,3)+"°C");

                ReceivedDataFrame.Clear();
                
                Thread.Sleep(500);
                
            }

            
            _serialPort.Close();
            Console.WriteLine("Serial Port Closed");

            

            
        }
        
    }
}
using System;
using System.Data;
using System.IO.Ports;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using Robo_R.CT100.Components;
using System.Text;

namespace Communication
{
    class Program
    {
        private static SerialPort _serialPort = new SerialPort();
        
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
            
            for(int i = 0; i < 50; i++)
            {
                //---set 방사율 data-----
                // SenddataFrame.Add(0x01);
                // SenddataFrame.Add(0x06);
                // SenddataFrame.Add(0x9C);
                // SenddataFrame.Add(0x41);
                // SenddataFrame.Add(0x00);
                // SenddataFrame.Add(0x50);

                //---get temperature---
                SenddataFrame.Add(0x01);
                SenddataFrame.Add(0x03);
                SenddataFrame.Add(0x9C);
                SenddataFrame.Add(0x42);
                SenddataFrame.Add(0x00);
                SenddataFrame.Add(0x02);

                var CRC = CRC16.CalculateCRC(SenddataFrame.ToArray());
                SenddataFrame.AddRange(CRC);          

                // for(int i = 0; i < 8; i++)
                // {
                //     Console.WriteLine("Send: "+SenddataFrame[i]);
                // }
                _serialPort.Write(SenddataFrame.ToArray(),0,8);


                for(int j = 0; j < 9; j++)
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
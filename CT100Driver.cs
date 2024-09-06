using Robo_R.Domain.Driver;
using Robo_R.CT100.Components;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;

namespace Robo_R.CT100.Driver
{
    public struct ChannelState
    {
        public double ObjectTemperature;
        public double PCBTemperature;
        public bool IsSenging;
    }
    public class CT100Driver

    {
        

        #region Helper Methods
        public static byte[] MakeReadDataFrame(CT100Address command)
        {
            var dataFrame = new List<byte>();
            dataFrame.Add((byte)CT100Address.ID1);  // ID Addres (Default : 1)
            dataFrame.Add(0x03);             // Read Register Function code 

            var commandCodes = BitConverter.GetBytes((int)command);
            dataFrame.Add(commandCodes[1]); // Register Address (High)
            dataFrame.Add(commandCodes[0]); // Register Address (Low)

            dataFrame.Add(0x00); // Register Count (High)
            //dataFrame.Add(0x02); // Register Count (Low)            
            dataFrame.Add((byte)(commandCodes[0] & 3)); // Register Count (Low)

            var crc = CRC16.CalculateCRC(dataFrame.ToArray());
            dataFrame.AddRange(crc);

            return dataFrame.ToArray();
        }

        public static byte[] MakeWriteDataFrame(CT100Address command,byte ChangeParameter)
        {
            var dataFrame = new List<byte>();
            dataFrame.Add((byte)CT100Address.ID1);  // ID Addres (Default : 1)
            dataFrame.Add(0x06);             // Read Register Function code 

            var commandCodes = BitConverter.GetBytes((int)command);
            dataFrame.Add(commandCodes[1]); // Register Address (High)
            dataFrame.Add(commandCodes[0]); // Register Address (Low)

            //var ChangeParameter16= Byte.Parse( Convert.ToString(ChangeParameter,16));

            dataFrame.Add(0x00); // Register Count (High)
            //dataFrame.Add(ChangeParameter16); // Register Count (Low) / ID : Using 1~200 / Emissivity : Using 10~100
            dataFrame.Add(ChangeParameter);

            var crc = CRC16.CalculateCRC(dataFrame.ToArray());
            dataFrame.AddRange(crc);

            return dataFrame.ToArray();
        }

        

        #endregion
    }
    
}

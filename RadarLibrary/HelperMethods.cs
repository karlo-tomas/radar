using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarLibrary
{
    public static class HelperMethods
    {
        public static string DecimalToIpAddress(long decimalAddress)
        {
            // Ensure the decimal address is within valid range for IPv4
            if (decimalAddress < 0 || decimalAddress > 0xFFFFFFFF)
                throw new ArgumentOutOfRangeException(nameof(decimalAddress), "The value must be between 0 and 4,294,967,295.");

            // Convert the decimal address to 4 bytes
            byte[] bytes = BitConverter.GetBytes(decimalAddress);

            // Reverse the byte array if system architecture is little-endian
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            // Take the last 4 bytes (IPv4 uses 4 bytes) for the IP address
            bytes = bytes.Skip(bytes.Length - 4).ToArray();

            // Convert to dot-decimal format
            return string.Join(".", bytes);
        }

    }
}

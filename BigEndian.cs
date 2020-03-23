using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace monhdk4
{
    public static class BigEndian
    {

        private static DateTime gps = new DateTime(1980, 1, 6);
        private static DateTime unix = new DateTime(1970, 1, 1);

        public static DateTime GetDateTime(BinaryReader rs)
        {
            long seconds = GetInt64(rs);
            return unix.AddSeconds(seconds); // new DateTime(gps.Ticks+(100*sec));
        }

        public static string GetString(BinaryReader rs)
        {
            var bytes = rs.ReadBytes(GetUint16(rs));
            return Encoding.UTF8.GetString(bytes);
        }

        public static ushort GetUint16(BinaryReader rs)
        {
            ushort val;
            var bytes = rs.ReadBytes(2);

            val = Convert.ToUInt16(bytes[0]);
            val <<= 8;
            val |= Convert.ToUInt16(bytes[1]);

            return val;
        }

        public static uint GetUint32(BinaryReader rs)
        {
            uint val = 0;
            var bytes = rs.ReadBytes(4);

            val |= Convert.ToUInt32(bytes[0]) << 24;
            val |= Convert.ToUInt32(bytes[1]) << 16;
            val |= Convert.ToUInt32(bytes[2]) << 8;
            val |= Convert.ToUInt32(bytes[3]);

            return val;
        }

        public static int GetInt32(BinaryReader rs)
        {
            int val = 0;
            var bytes = rs.ReadBytes(4);

            val |= Convert.ToInt32(bytes[0]) << 24;
            val |= Convert.ToInt32(bytes[1]) << 16;
            val |= Convert.ToInt32(bytes[2]) << 8;
            val |= Convert.ToInt32(bytes[3]);

            return val;
        }

        public static long GetInt64(BinaryReader rs)
        {
            long val = 0;
            var bytes = rs.ReadBytes(8);

            val |= Convert.ToInt64(bytes[0]) << 56;
            val |= Convert.ToInt64(bytes[1]) << 48;
            val |= Convert.ToInt64(bytes[2]) << 40;
            val |= Convert.ToInt64(bytes[3]) << 32;
            val |= Convert.ToInt64(bytes[4]) << 24;
            val |= Convert.ToInt64(bytes[5]) << 16;
            val |= Convert.ToInt64(bytes[6]) << 8;
            val |= Convert.ToInt64(bytes[7]);

            return val;
        }
    }
}

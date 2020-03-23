using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace monhdk4
{
    public static class AddrExtension
    {
        public static bool IsMulticast(this IPAddress addr)
        {
            var bytes = addr.GetAddressBytes();
            return bytes[0] >= 224 && bytes[0] <= 239;
        }
    }
}

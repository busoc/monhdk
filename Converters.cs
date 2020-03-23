using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Net;

namespace monhdk4
{
    class AddrPortConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                return "not yet connected";
            }
            return $"Connected to {values[0]?.ToString()}:{values[1]?.ToString()}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class SizeConverter : IValueConverter
    {
        private static readonly ulong Kilo = 1 << 10;
        private static readonly ulong Mega = 1 << 20;
        private static readonly ulong Giga = 1 << 30;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ulong size = (ulong)value;
            if (size >= Giga)
            {
                return $"{(double)size / Giga:F2}GB";
            } 
            else if (size >= Mega)
            {
                return $"{(double)size / Mega:F2}MB";
            } 
            else if (size >= Kilo)
            {
                return $"{(double)size / Kilo:F2}KB";
            }
            else
            {
                return $"{size}B";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class AddressConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return IPAddress.Parse(value.ToString());
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }
    }

    class ChannelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte val)
            {
                return val switch
                {
                    1 => "VIC1",
                    2 => "VIC2",
                    3 => "LRSD",
                    _ => "unknown",
                };
            }
            return "???";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class DatetimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dt)
            {
                return dt.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz");
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class ECConverter : IValueConverter
    {
        private const string RUBI = "RUBI";
        private const string SMD = "SMD";
        private const string VMU = "VMU";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ec;
            switch (value.ToString())
            {
                case "33":
                    ec = RUBI;
                    break;
                case "34":
                    ec = RUBI;
                    break;
                case "35":
                    ec = RUBI;
                    break;
                case "36":
                    ec = RUBI;
                    break;
                case "37":
                    ec = SMD;
                    break;
                case "38":
                    ec = SMD;
                    break;
                case "39":
                    ec = SMD;
                    break;
                case "40":
                    ec = SMD;
                    break;
                case "41":
                    ec = SMD;
                    break;
                case "51":
                    ec = VMU;
                    break;
                case "90":
                    ec = RUBI;
                    break;
                default:
                    return "unknown";
            }
            return ec;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class ModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                return val ? "realtime" : "playback";
            }
            return "???";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class InstanceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is uint val)
            {
                return val switch
                {
                    0 => "TEST",
                    1 => "SIM 1",
                    2 => "SIM 2",
                    255 => "OPS",
                    _ => "unknown",
                };
            }
            return "???";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

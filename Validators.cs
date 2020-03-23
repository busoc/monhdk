using System;
using System.Globalization;
using System.Net;
using System.Windows.Controls;

namespace monhdk4
{
    class DateValidatior : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                DateTime.Parse(value.ToString());
                return ValidationResult.ValidResult;
            }
            catch(Exception e)
            {
                return new ValidationResult(false, e.Message);
            }
        }
    }

    class AddressValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                IPAddress.Parse(value.ToString());
                return ValidationResult.ValidResult;
            }
            catch (Exception e)
            {
                return new ValidationResult(false, e.Message);
            }
        }
    }

    class PositionIntegerValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                int limit = Convert.ToInt32(value.ToString());
                if (limit <= 0)
                {
                    return new ValidationResult(false, "positive number required");
                }
                return ValidationResult.ValidResult;
            }
            catch(Exception e)
            {
                return new ValidationResult(false, e.Message);
            }
        }
    }

    class UriValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                new Uri(value.ToString());
                return ValidationResult.ValidResult;
            }
            catch (Exception e)
            {
                return new ValidationResult(false, e.Message);
            }
        }
    }

    class PortValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                int port = Convert.ToInt32(value.ToString());
                if (port >=1 && port <= 65535)
                {
                    return ValidationResult.ValidResult;
                }
                return new ValidationResult(false, $"port {port} out of range (1 - 65535)");
            }
            catch(Exception e)
            {
                return new ValidationResult(false, e.Message);
            }
        }
    }
}

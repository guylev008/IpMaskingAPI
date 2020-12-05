using System;
using System.Linq;
using System.Net;
using FluentValidation;
using MaskIp;

namespace IpMasking.Validators.IpValidators
{
    public class IpValidator : AbstractValidator<string>
    {
        private bool ValidateFormat(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                return false;
            }

            var splitValues = ip.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            return splitValues.All(r => byte.TryParse(r, out _));
        }

        private bool ExistInClass(string ip)
        {
            var ipAddress = IPAddress.Parse(ip);
            var x = Convert.ToString(ipAddress.GetAddressBytes()[0], 2);
            var firstOctetBits = Convert.ToString(ipAddress.GetAddressBytes()[0], 2).Substring(0, 3);
            var classCBits = ((int)IpAddressClass.ClassC).ToString();
            return firstOctetBits.Equals(classCBits);
        }
        public IpValidator()
        {
            RuleFor(d => d).Must(ValidateFormat);
            RuleFor(d => d).Must(ExistInClass);
        }

    }
}

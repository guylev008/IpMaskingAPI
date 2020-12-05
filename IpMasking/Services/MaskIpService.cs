using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using IpMasking.Validators;
using IpMasking.Validators.IpValidators;
using MaskIp;
using Microsoft.AspNetCore.Http;

namespace IpMasking.Services
{
    public interface IMaskIpService
    {
        MemoryStream HandleMasking(IFormFile i_file);
    }
    public class MaskIpService : IMaskIpService
    {
        private readonly IMaskIps _maskIps;

        public MaskIpService(IMaskIps maskIps)
        {
            _maskIps = maskIps;
        }

        public MemoryStream HandleMasking(IFormFile i_file)
        {
            var fileContent = i_file.ReadAsStringAsync();
            var ipList = GetIpsFromFileContent(fileContent);
            foreach (var ip in ipList)
            {
                if(!IsIpValid(ip))
                    continue;
                var maskedIp = _maskIps.Mask(ip);
                fileContent = fileContent.Replace(ip, maskedIp);
            }

            return fileContent.ConvertToStream();
        }

        private bool IsIpValid(string ip)
        {
            var ipValidator = ValidatorFactory<IpValidator>.GetValidator();
            return ipValidator.Validate(ip).IsValid;
        }

        private IEnumerable<string> GetIpsFromFileContent(string fileContent)
        {
            return Regex.Matches(fileContent, @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b").Select(m => m.Groups[0].Value).Distinct();
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            var ipList = GetClassCIpsFromFileContent(fileContent);
            foreach (var ip in ipList)
            {
                var maskedIp = _maskIps.Mask(ip);
                fileContent = fileContent.Replace(ip, maskedIp);
            }

            return fileContent.ConvertToStream();
        }

        private IEnumerable<string> GetClassCIpsFromFileContent(string fileContent)
        {
            return Regex.Matches(fileContent, @"(19[2-9]|2[0-2]\d|23[0-3])(\.([1-9]?\d|[12]\d\d)){3}").Select(m => m.Groups[0].Value).Distinct();
        }
    }
}

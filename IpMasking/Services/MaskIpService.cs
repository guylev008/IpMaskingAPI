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
            var content = _maskIps.Mask(fileContent);
            return content.ConvertToStream();
        }

       
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using IpMasking.Attributes;
using IpMasking.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IpMasking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IpController : ControllerBase
    {
        private readonly IMaskIpService _maskIpService;

        public IpController(IMaskIpService maskIpService)
        {
            _maskIpService = maskIpService;
        }

        [HttpPost]
        [Route("mask-ip")]
        public IActionResult MaskIp(
            [AllowedExtensions(new[] { ".log"})]
            [MaxFileSize(5 * 1024 * 1024)]
            [Required(ErrorMessage = "Please select a file.")]
            [DataType(DataType.Upload)]
            [FromForm]IFormFile i_file
            )
        {
            try
            {
                var stream = _maskIpService.HandleMasking(i_file);
                return File(stream, "application/octet-stream", $"{i_file.FileName}");
            }

            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}

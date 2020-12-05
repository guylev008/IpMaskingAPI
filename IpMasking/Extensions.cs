using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace IpMasking
{
    public static class Extensions
    {
        public static string ReadAsStringAsync(this IFormFile file)
        {
            var result = new StringBuilder();
            using var reader = new StreamReader(file.OpenReadStream());
            while (reader.Peek() >= 0)
                result.AppendLine(reader.ReadLine());
            return result.ToString();
        }

        public static MemoryStream ConvertToStream(this string content)
        {
            var byteArray = Encoding.UTF8.GetBytes(content);
            return new MemoryStream(byteArray);
        }
    }
}

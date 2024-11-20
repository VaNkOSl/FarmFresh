using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Infrastructure.TagHelpers
{
    public static class FileHelper
    {
        public static async Task<byte[]> ReadFileAsync(IFormFile file)
        {
            if (file.Length == 0) throw new ArgumentException("File is empty.");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }

        public static async Task SaveFileAsync(byte[] fileData, string path)
        {
            await File.WriteAllBytesAsync(path, fileData);
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path)) File.Delete(path);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neama.Core.Services.Contract;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly Client client;

        public AttachmentService( Supabase.Client client)
        {
            this.client = client;
        }
        public async Task<string?> ImageUrl(IFormFile Image )
        {
            using var memoryStream = new MemoryStream();
            await Image.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();

            var bucket = client.Storage.From("Photo");
            var fileName = $"{Guid.NewGuid()}_{Image.FileName}";

            await bucket.Upload(imageBytes, fileName);

            var Url = bucket.GetPublicUrl(fileName);

            return Url;
        }

        public async Task<bool> DeleteImageByUrl(List<string> imageUrls)
        {
            var fileNames = imageUrls
            .Where(url => !string.IsNullOrWhiteSpace(url)) 
            .Select(url => url.Split('/').Last()) 
            .ToList();

            if (!fileNames.Any())
                return false;
            var bucket = client.Storage.From("Photo");
            await bucket.Remove(fileNames);

            return true;
        }
    }
}

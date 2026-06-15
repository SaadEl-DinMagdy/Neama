using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Services.Contract
{
    public interface IAttachmentService
    {
        Task<string?> ImageUrl(IFormFile Image);
        Task<bool> DeleteImageByUrl(List<string> imageUrls);
    }
}

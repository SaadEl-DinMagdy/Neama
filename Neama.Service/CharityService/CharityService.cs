using Neama.Core;
using Neama.Core.Entities;
using Neama.Core.Services.Contract;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.CharityService
{
    
    public class CharityService : ICharityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CharityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<AllCharityDto>> GetAllAsync()
        {

            var charities = await _unitOfWork.Repository<Charity>().GetAllAsync();

            var result = charities.Select(c => new AllCharityDto
            {
                Id = c.Id,
                Name = c.Name,
                ImageURL = c.ImageURL
            }).ToList();

            return result;
        }

        public async Task<CharityDto?> GetByIdAsync(int id)
        {
            
            var charity = await _unitOfWork.Repository<Charity>().GetAsync(id);

            
            if (charity == null) return null;

            
            return new CharityDto
            {
                Id = charity.Id,
                Name = charity.Name,
                Description = charity.Description,
                ImageURL = charity.ImageURL
            };
        }
    }
}

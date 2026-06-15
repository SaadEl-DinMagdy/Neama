using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Neama.Core;
using Neama.Core.Entities;
using Neama.Core.Services.Contract;
using Neama.Core.Specifications;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Neama.Service.MainSectionService
{
    public class MainSectionService : IMainSectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachment;

        public MainSectionService(IUnitOfWork unitOfWork , IAttachmentService attachment)
        {
            _unitOfWork = unitOfWork;
            _attachment = attachment;
        }
        public async Task<IReadOnlyList<AllMainSectionDto>> GetAllAsync()
        {
            var spec = new BaseSpecifications<MainSection>(m => m.IsActive);
            var Sections = await _unitOfWork.Repository<MainSection>().GetAllWithSpecAsync(spec);


            IReadOnlyList<AllMainSectionDto> Result = Sections
                .Select(S => new AllMainSectionDto()
                {
                    Id = S.Id,
                    Name = S.Name,
                    IconURL = S.IconURL
                }).ToList();

            return Result;
        }
        
        public async Task<bool> Add(AddMainSectionDto section)
        {
            var iconUrl = await _attachment.ImageUrl(section.IconURL);
            var data = new MainSection()
            {
                Name = section.Name,
                IconURL = iconUrl,
                IsActive = true,
            };

            await _unitOfWork.Repository<MainSection>().AddAsync(data);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }
        public async Task<bool> Update(UpdateMainSection section)
        {

     
            var data = await _unitOfWork.Repository<MainSection>().GetAsync(section.Id);
            if (data == null)
            {
                return false;
            }
            var iconUrl = await _attachment.ImageUrl(section.IconURL);
            await _attachment.DeleteImageByUrl(new List<string>() { data.IconURL });

            data.IconURL = iconUrl;
            data.Name = section.Name;

            
             _unitOfWork.Repository<MainSection>().Update(data);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }
        public async Task<bool> Remove(int id)
        {
            var data = await _unitOfWork.Repository<MainSection>().GetAsync(id);

            data.IsActive=false;
           
            _unitOfWork.Repository<MainSection>().Update(data);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }

        
    }
}

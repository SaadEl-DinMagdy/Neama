using Neama.Core;
using Neama.Core.Entities;
using Neama.Core.Services.Contract;
using Neama.Core.Specifications;
using Neama.Core.Specifications.PartnerSpecifications;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.PartnerService
{
    public class PartnerService : IPartnerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PartnerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> AddAsync(AddPartnerDto model)
        {
            var partner = new Partner
            {
                Name = model.Name,
                ManagerId = model.ManagerId,
                Is_Active = true
            };

            await _unitOfWork.Repository<Partner>().AddAsync(partner);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }

        public async Task<IReadOnlyList<PartnerResponseDto>> GetAllAsync(string? search)
        {
            var spec = new PartnerWithBranchesSpecification(search);
            var partners = await _unitOfWork.Repository<Partner>().GetAllWithSpecAsync(spec);

            var data = partners.Select(p => new PartnerResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                WalledBalance = p.WalledBalance,
                Is_Active = p.Is_Active,
                Logo_URL = p.Logo_URL,
                Cover_URL = p.Cover_URL,
                CreationDate = p.CreationDate,
                ManagerEmail = p.Manager != null ? p.Manager.Email : null,
                MainSectionName = p.MainSection != null ? p.MainSection.Name : null
            }).ToList();

            return data;
        }

        public async Task<PartnerWithBranchesResponseDto?> GetPartnerWithBranchesAsync(int id)
        {
            var spec = new PartnerWithBranchesSpecification(id);
            var partner = await _unitOfWork.Repository<Partner>().GetWithSpecAsync(spec);

            if(partner == null)
            {
                return null;
            }
            var data = new PartnerWithBranchesResponseDto
            {
                Id = partner.Id,
                Name = partner.Name,
                WalledBalance = partner.WalledBalance,
                Is_Active = partner.Is_Active,
                Logo_URL = partner.Logo_URL,
                Cover_URL = partner.Cover_URL,
                CreationDate = partner.CreationDate,

                ManagerEmail = partner.Manager != null ? partner.Manager.Email : null,
                MainSectionName = partner.MainSection != null ? partner.MainSection.Name : null,

                Branches = partner.Branches.Select(b => new BranchResponseDto
                {
                    Id = b.Id,
                    BranchName = b.BranchName,
                    BranchPhone = b.BranchPhone,
                    Is_Active = b.Is_Active,
                    AverageRating = b.AverageRating,
                    ReviewCount = b.ReviewCount,
                    Latitude = b.Latitude,
                    Longitude = b.Longitude,
                    OpeningTime = b.OpeningTime,
                    ClosingTime = b.ClosingTime,
                    CreationDate = b.CreationDate
                }).ToList()
            };


            return data;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var spec = new PartnerWithBranchesSpecification(id);
            var partner = await _unitOfWork.Repository<Partner>().GetWithSpecAsync(spec);

            if (partner == null)
            {
                return false;
            }

            partner.Is_Active = false;
            foreach (var branch in partner.Branches)
            {
                branch.Is_Active = false;
            }

            _unitOfWork.Repository<Partner>().Update(partner);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }

        public async Task<bool> UpdatePartner(int id)
        {
            var spec = new PartnerWithBranchesSpecification(id);
            var partner = await _unitOfWork.Repository<Partner>().GetWithSpecAsync(spec);

            if (partner == null)
            {
                return false;
            }

            partner.Is_Active = true;
            foreach (var branch in partner.Branches)
            {
                branch.Is_Active = true;
            }

            _unitOfWork.Repository<Partner>().Update(partner);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }
    }
}

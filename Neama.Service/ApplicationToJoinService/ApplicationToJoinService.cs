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

namespace Neama.Service.ApplicationToJoinService
{
    public class ApplicationToJoinService : IApplicationToJoinService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationToJoinService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateApplicationAsync(CreateApplicationToJoinDto application)
        {
            var data = new ApplicationsToJoin()
            {
                FullName = application.FullName,
                Phone = application.Phone,
                Email = application.Email,
                Location = application.Location,
                PlaceName = application.PlaceName,
            };

            await _unitOfWork.Repository<ApplicationsToJoin>().AddAsync(data);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }

        public async Task<IReadOnlyList<ApplicationsToJoin>> GetAllApplicationsAsync(bool? ContactWasMade)
        {
            var spec = new BaseSpecifications<ApplicationsToJoin>(a => a.ContactWasMade==ContactWasMade);
            var applications = await _unitOfWork.Repository<ApplicationsToJoin>().GetAllWithSpecAsync(spec);
            return applications.OrderByDescending(a => a.CreatedAt).ToList();
        }

        public async Task<bool> MarkAsContactedAsync(int id)
        {
            var application = await _unitOfWork.Repository<ApplicationsToJoin>().GetAsync(id);

            if (application == null)
                return false;

            application.ContactWasMade = true;

            _unitOfWork.Repository<ApplicationsToJoin>().Update(application);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }
    }
}

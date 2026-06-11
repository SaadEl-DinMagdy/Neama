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

namespace Neama.Service.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<CategoryDto>> GetBranchCategoriesAsync(int branchId)
        {
            var spec = new BaseSpecifications<Category>(c => c.BranchId==branchId);
            var category = await _unitOfWork.Repository<Category>().GetAllWithSpecAsync(spec);

            IReadOnlyList<CategoryDto> Result = category
                .Select(C => new CategoryDto()
                {

                    Id = C.Id,
                    Name = C.Name
                }).ToList();

            return Result;
        }
        public Task<bool> Add(Category category)
        {
            throw new NotImplementedException();
        }
        public Task<bool> Update(Category category)
        {
            throw new NotImplementedException();
        }
        public Task<bool> Remove(Category category)
        {
            throw new NotImplementedException();
        }
    }
}

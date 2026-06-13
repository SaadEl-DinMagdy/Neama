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

namespace Neama.Service.ItemService
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<AllItemDto>> GetAllSpecAsync(int branchId, int? categoryId)
        {
            var spec = new BaseSpecifications<Item>(I => I.IsActive && I.BranchId == branchId && (!categoryId.HasValue || I.CategoryId == categoryId));

            var Items = await _unitOfWork.Repository<Item>().GetAllWithSpecAsync(spec);

            IReadOnlyList<AllItemDto> Result = Items
                .Select(I => new AllItemDto()
                {
                    Id = I.Id,
                    Name = I.Name,
                    ExpiryDate = I.ExpiryDate,
                    ImageURL = I.ImageURL,
                    OriginalPrice = I.OriginalPrice,
                    DiscountPrice = I.DiscountPrice,
                    Discount = I.DiscountPrice*100/I.OriginalPrice,
                    StockQuantity = I.StockQuantity,
                }).ToList();

            return Result;
        }

        public async Task<ItemDto?> GetItemAsync(int ItemId)
        {
            var Item = await _unitOfWork.Repository<Item>().GetAsync(ItemId);

            if(Item == null)
            {
                return null;
            }

            return new ItemDto()
            {
                Id = Item.Id,
                Name = Item.Name,
                ExpiryDate = Item.ExpiryDate,
                ImageURL = Item.ImageURL,
                OriginalPrice = Item.OriginalPrice,
                DiscountPrice = Item.DiscountPrice,
                Discount = Item.DiscountPrice * 100 / Item.OriginalPrice,
                StockQuantity = Item.StockQuantity,
                BranchId = Item.BranchId

            };
        }
        public Task<bool> Add(Item item)
        {
            throw new NotImplementedException();
        }
        public Task<bool> Update(Item item)
        {
            throw new NotImplementedException();
        }
        public Task<bool> Remove(Item item)
        {
            throw new NotImplementedException();
        }
    }
}

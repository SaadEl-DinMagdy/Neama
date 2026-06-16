using Neama.Core;
using Neama.Core.Entities;
using Neama.Core.Services.Contract;
using Neama.Core.Specifications;
using Neama.Core.Specifications.UserMarketSpecification;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.UserMarketService
{
    public class UserMarketService : IUserMarketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachment;

        public UserMarketService(IUnitOfWork unitOfWork , IAttachmentService attachment)
        {
            _unitOfWork = unitOfWork;
            _attachment = attachment;
        }
        public async Task<IReadOnlyList<ReturnUserProductDto>> GetUserMarketSpecAsync(MarketParam param)
        {
            var spec = new UserMarketSearchAndSortPaginationSpecifc(param);
            var products = await _unitOfWork.Repository<UserProduct>().GetAllWithSpecAsync(spec);


            IReadOnlyList<ReturnUserProductDto> Result = products
                .Select(p=> new ReturnUserProductDto()
                {
                    Photos = p.Photos,
                    Id = p.Id,
                    Name = p.Name,
                    Discription = p.Discription, 
                    Country = p.Country,
                    City = p.City,
                    Phone = p.Phone,
                    WhatsApp = p.WhatsApp,
                    Price = p.Price,
                    CreationDate = p.CreationDate
                }).ToList();

            return Result;
        }
        public async Task<IReadOnlyList<ReturnUserProductDto>> GetAllMyProductSpecAsync(string userId, string? Search)
        {
            var spec = new UserMarketSearchandSortOnlySpec(userId ,Search);
            var products = await _unitOfWork.Repository<UserProduct>().GetAllWithSpecAsync(spec);


            IReadOnlyList<ReturnUserProductDto> Result = products
                .Select(p => new ReturnUserProductDto()
                {
                    Photos = p.Photos, 
                    Id = p.Id,
                    Name = p.Name,
                    Discription = p.Discription,
                    Country = p.Country,
                    City = p.City,
                    Phone = p.Phone,
                    WhatsApp = p.WhatsApp,
                    Price = p.Price,
                    CreationDate = p.CreationDate
                }).ToList();

            return Result;
        }

        public async Task<ReturnUserProductDto?> GetMyProductSpecAsync(string userId, int Id)
        {
            var spec = new BaseSpecifications<UserProduct>(p => p.Id == Id&&p.AppUserId==userId);
            var p = await _unitOfWork.Repository<UserProduct>().GetWithSpecAsync(spec);
            if(p==null)return null;
            return new ReturnUserProductDto()
            {
                Photos = p.Photos, 
                Id = p.Id,
                Name = p.Name,
                Discription = p.Discription,
                Country = p.Country,
                City = p.City,
                Phone = p.Phone,
                WhatsApp = p.WhatsApp,
                Price = p.Price,
                CreationDate = p.CreationDate
            };
        }

        public async Task<bool> AddAsync(AddUserProduct userProduct , string userId)
        {
            List<string> images = new List<string>();
            foreach(var image in userProduct.Photos)
            {
              var data=await _attachment.ImageUrl(image);
                if(data == null)
                {
                    return false;
                }
                images.Add(data);
            }
            var product = new UserProduct()
            {
                Photos = images,
                Name = userProduct.Name,
                Discription = userProduct.Discription,
                Country = userProduct.Country,
                City = userProduct.City,
                Phone = userProduct.Phone,
                WhatsApp = userProduct.WhatsApp,
                Price = userProduct.Price,
                AppUserId = userId
            };

            await _unitOfWork.Repository<UserProduct>().AddAsync(product);
            await _unitOfWork.CompleteAsync();

            return true;
        }
        public async Task<ReturnUserProductDto?> Update(UserProductDto userProduct)
        {
            var product = await _unitOfWork.Repository<UserProduct>().GetAsync(userProduct.Id);
            if (product == null)
            {
                return null;
            }
            List<string> images = new List<string>();

            if (userProduct.Photos.Count > 0)
            {
                await _attachment.DeleteImageByUrl(product.Photos);
                
                foreach (var image in userProduct.Photos)
                {
                    var d = await _attachment.ImageUrl(image);
                    if (d == null)
                    {
                        return null;
                    }
                    images.Add(d);
                }

                product.Photos = images;
            }

           
            product.Name = userProduct.Name;
            product.Discription = userProduct.Discription;
            product.Country = userProduct.Country;
            product.City = userProduct.City;
            product.Phone = userProduct.Phone;
            product.WhatsApp = userProduct.WhatsApp;
            product.Price = userProduct.Price;
            product.CreationDate = DateOnly.FromDateTime(DateTime.Now); ;
            _unitOfWork.Repository<UserProduct>().Update(product);
            await _unitOfWork.CompleteAsync();

            var data = new ReturnUserProductDto()
            {
                Id = product.Id,
                Photos = images,
                Name = userProduct.Name,
                Discription = userProduct.Discription,
                Country = userProduct.Country,
                City = userProduct.City,
                Phone = userProduct.Phone,
                WhatsApp = userProduct.WhatsApp,
                Price = userProduct.Price,
                CreationDate = product.CreationDate
            };

            return data;
        }
        public async Task<bool> Remove(int id)
        {
            var p = await _unitOfWork.Repository<UserProduct>().GetAsync(id);

            if (p == null) return false;

            await _attachment.DeleteImageByUrl(p.Photos);

            

            _unitOfWork.Repository<UserProduct>().Delete(p);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}

using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.ImageQueries;
using JurayKV.Domain.Aggregates.ImageAggregate;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using TanvirArjel.Extensions.Microsoft.Caching;

namespace JurayKV.Persistence.Cache.Repositories
{
    public sealed class ImageCacheRepository : IImageCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IQueryRepository _repository;
        private readonly IImageRepository _imageRepository;
        public ImageCacheRepository(IDistributedCache distributedCache, IQueryRepository repository, IImageRepository imageRepository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
            _imageRepository = imageRepository;
        }

        public async Task<List<ImageDto>> GetListAsync()
        {
            string cacheKey = ImageCacheKeys.ListKey;
            List<ImageDto> list = await _distributedCache.GetAsync<List<ImageDto>>(cacheKey);

            if (list == null)
            {
                Expression<Func<ImageFile, ImageDto>> selectExp = d => new ImageDto
                {
                    Id = d.Id,
                    ImageUrl = d.ImageUrl,
                    ImageKey = d.ImageKey,
                    CreatedAtUtc = d.CreatedAtUtc,
                    ShowInDropdown = d.ShowInDropdown,
                    Name = d.Name,
                };

                list = await _repository.GetListAsync(selectExp);

                await _distributedCache.SetAsync(cacheKey, list);
            }

            return list;
        }

        public async Task<ImageDto> GetByIdAsync(Guid imageId)
        {
            string cacheKey = ImageCacheKeys.GetKey(imageId);
            ImageDto image = await _distributedCache.GetAsync<ImageDto>(cacheKey);

            if (image == null)
            {
                Expression<Func<ImageFile, ImageDto>> selectExp = d => new ImageDto
                {
                    Id = d.Id,
                    ImageUrl = d.ImageUrl,
                    ImageKey = d.ImageKey,
                    CreatedAtUtc = d.CreatedAtUtc,
                    ShowInDropdown = d.ShowInDropdown,
                    Name = d.Name,
                };

                image = await _repository.GetByIdAsync(imageId, selectExp);

                await _distributedCache.SetAsync(cacheKey, image);
            }
            return image;
        }

        public async Task<List<ImageDto>> GetListDropdownAsync()
        {

            var mainlist = await _imageRepository.ImageSHowInDropDown();
            var list = mainlist.Select(d => new ImageDto
            {
                Id = d.Id,
                Name = d.Name,

            }).ToList();


            return list;
        }
    }

}

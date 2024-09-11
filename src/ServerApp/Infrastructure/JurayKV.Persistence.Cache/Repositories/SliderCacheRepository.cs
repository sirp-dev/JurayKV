using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.SliderQueries;
using JurayKV.Domain.Aggregates.SliderAggregate;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using TanvirArjel.Extensions.Microsoft.Caching;

namespace JurayKV.Persistence.Cache.Repositories
{
    public sealed class SliderCacheRepository : ISliderCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IQueryRepository _repository;

        public SliderCacheRepository(IDistributedCache distributedCache, IQueryRepository repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }

        public async Task<List<SliderDetailsDto>> GetListAsync()
        {
            //string cacheKey = SliderCacheKeys.ListKey;
            //List<SliderDetailsDto> list = await _distributedCache.GetAsync<List<SliderDetailsDto>>(cacheKey);

            //if (list == null)
            //{
            Expression<Func<Slider, SliderDetailsDto>> selectExp = d => new SliderDetailsDto
            {
                Id = d.Id,
                Url = d.Url,
                Key = d.Key,
                SecondUrl = d.SecondUrl,
                SecondKey = d.SecondKey,
                YoutubeVideo = d.YoutubeVideo,
                IsVideo = d.IsVideo,
                SortOrder = d.SortOrder,
                Show = d.Show,
                Title = d.Title,
                MiniTitle = d.MiniTitle,
                Text = d.Text,
                ButtonText = d.ButtonText,
                ButtonLink = d.ButtonLink,
                DisableVideoButton = d.DisableVideoButton,
                VideoButtonText = d.VideoButtonText,
                VideoLink = d.VideoLink,

            };

            List<SliderDetailsDto> list = await _repository.GetListAsync(selectExp);

            //await _distributedCache.SetAsync(cacheKey, list);
            //}

            return list;
        }

        public async Task<SliderDetailsDto> GetByIdAsync(Guid sliderId)
        {
            string cacheKey = SliderCacheKeys.GetKey(sliderId);
            SliderDetailsDto slider = await _distributedCache.GetAsync<SliderDetailsDto>(cacheKey);
            if (slider == null)
            {
                Expression<Func<Slider, SliderDetailsDto>> selectExp = d => new SliderDetailsDto
                {
                    Id = d.Id,
                    Url = d.Url,
                    Key = d.Key,
                    SecondUrl = d.SecondUrl,
                    SecondKey = d.SecondKey,
                    YoutubeVideo = d.YoutubeVideo,
                    IsVideo = d.IsVideo,
                    SortOrder = d.SortOrder,
                    Show = d.Show,
                    Title = d.Title,
                    MiniTitle = d.MiniTitle,
                    Text = d.Text,
                    ButtonText = d.ButtonText,
                    ButtonLink = d.ButtonLink,
                    DisableVideoButton = d.DisableVideoButton,
                    VideoButtonText = d.VideoButtonText,
                    VideoLink = d.VideoLink,


                };

                slider = await _repository.GetByIdAsync(sliderId, selectExp);

                await _distributedCache.SetAsync(cacheKey, slider);
            }

            return slider;

        }

    }

}

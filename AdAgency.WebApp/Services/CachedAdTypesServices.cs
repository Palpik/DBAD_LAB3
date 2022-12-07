using AdAgency.Domain;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdAgency.WebApp.Services
{
    public class CachedAdTypesServices : ICachedAdTypesServices
    {
        private readonly AdAgencyContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedAdTypesServices(AdAgencyContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public void AddAdTypes(string cacheKey, int rowsNumbers = 20)
        {
            IEnumerable<AdType> adTypes = _dbContext.AdTypes.Take<AdType>(rowsNumbers).ToList<AdType>();
            if(adTypes != null)
            {
                _memoryCache.Set(cacheKey, adTypes, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(276)
                });
            }
        }

        public IEnumerable<AdType> GetAdTypes(int rowsNumbers = 20)
        {
            return _dbContext.AdTypes.Take(rowsNumbers).ToList();
        }

        public IEnumerable<AdType> GetAdTypes(string cacheKey, int rowsNumbers = 20)
        {
            IEnumerable<AdType> adTypes;
            if(!_memoryCache.TryGetValue(cacheKey, out adTypes))
            {
                adTypes = _dbContext.AdTypes.Take(rowsNumbers).ToList();
                this.AddAdTypes(cacheKey, rowsNumbers);
            }
            return adTypes;
        }
    }
}
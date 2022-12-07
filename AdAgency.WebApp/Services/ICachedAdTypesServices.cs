using AdAgency.Domain;
using System.Collections.Generic;

namespace AdAgency.WebApp.Services
{
    public interface ICachedAdTypesServices
    {
        public IEnumerable<AdType> GetAdTypes(int rowsNumbers = 20);

        public void AddAdTypes(string cacheKey, int rowsNumbers = 20);

        public IEnumerable<AdType> GetAdTypes(string cacheKey, int rowsNumbers = 20);
    }
}
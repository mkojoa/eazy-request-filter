using eazy.request.filter.Filter.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eazy.request.filter.EfCore.Services
{
    public interface IEfCoreService
    {
        public Task<IEnumerable<AuthCompanyDto>> FetchCompanies(Guid userId);
    }
}

using eazy.request.filter.Filter.Options;
using eazy.request.filter.Helper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace eazy.request.filter.EfCore.Services
{
    public class EfCoreService<TContext> : IEfCoreService
        where TContext : DbContext
    {
        private readonly TContext _context;

        public EfCoreService(TContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuthCompanyDto>> FetchCompanies(Guid userId)
        {
            try
            {
                var companies = new List<AuthCompanyDto>();

                _context.Database.OpenConnection();
                using (DbCommand cmdObj = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmdObj.CommandText = "spFetchUsersCompanyAccess";
                    cmdObj.CommandType = CommandType.StoredProcedure;
                    cmdObj.Parameters.Add(new SqlParameter("@uUserId", userId));
                    using var drObj2 = await cmdObj.ExecuteReaderAsync();
                    companies = drObj2.MapToList<AuthCompanyDto>();
                    drObj2.Close();
                }

                return companies;
            }
            catch (Exception ex)
            {
                throw new Exception($"Db Error : {ex.Message}");
            }
        }
    }
}

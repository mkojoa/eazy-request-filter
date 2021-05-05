using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eazy.request.filter.Cache.Store.Redis;
using eazy.request.filter.EfCore.Services;
using eazy.request.filter.Filter.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace eazy.request.filter.Filter
{
    public class ValidateCompanyOnRequest : TypeFilterAttribute
    {
        public ValidateCompanyOnRequest()
            : base(typeof(ValidateAuthorExistsFilterImpl)) { }

        private class ValidateAuthorExistsFilterImpl : IAsyncActionFilter
        {
            private readonly IEfCoreService _efCoreService;
            private readonly IRedisCacheService _redisCacheService;

            public ValidateAuthorExistsFilterImpl(IRedisCacheService redisCacheService, IEfCoreService efCoreService)
            {
                _redisCacheService = redisCacheService;
                _efCoreService = efCoreService; 
            }


            public async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
            {
                if (filterContext.ActionArguments.ContainsKey("CompanyId") == true)
                {
                    var companyId = filterContext.ActionArguments.ContainsKey("CompanyId")
                        ? filterContext.ActionArguments["CompanyId"]
                        : null;

                    var authId = Guid.Empty;

                    //if (filterContext.HttpContext.User.Claims != null)
                    //{

                    //    authId = Guid.Parse(filterContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value) != Guid.Empty
                    //    ? Guid.Parse(filterContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value)
                    //    : Guid.Empty;
                    //}

                    //make db call and re-cache db result for 30 sec
                    if (companyId != null)
                    {
                        var dbResult = await SpCheckIfAuthHasAccessToCompany(authId, companyId.ToString());

                        if (dbResult != true)
                            filterContext.Result = new NotFoundObjectResult($"Sorry!, you don't have access to company : {companyId}");
                        else
                            await next();
                    }
                }
                else
                {
                    // no key found
                    await next();
                    //filterContext.Result = new UnauthorizedResult();
                }
            }


            private async Task<bool> SpCheckIfAuthHasAccessToCompany(Guid authId, string companyId)
            {
                //- Algorithm
                if (await SpInMemoryCallToCheck(authId, Guid.Parse(companyId))) return true;
                return false;
            }


            /// <summary>
            ///  In memory check
            /// </summary>
            /// <param name="authId"></param>
            /// <param name="companyId"></param>
            /// <returns></returns>
            private async Task<bool> SpInMemoryCallToCheck(Guid authId, Guid companyId)
            {
                // "AuthIdAsKey" -> authId
                if (!_redisCacheService.TryGetValue(authId.ToString(), out IEnumerable<AuthCompanyDto> values)) //try get auth companies
                {

                    values = await _efCoreService.FetchCompanies(authId);

                    //cache it for 60 sec
                    await _redisCacheService.SetAsync(authId.ToString(), values, 60);


                    var authCachedRecord =
                        await _redisCacheService.GetAsync<IEnumerable<AuthCompanyDto>>(authId.ToString());

                    authCachedRecord = authCachedRecord.Where(x => x.Id == companyId);

                    if (!authCachedRecord.Any()) return false;

                    return true;
                }
                else
                {

                    var authCachedRecord =
                        await _redisCacheService.GetAsync<IEnumerable<AuthCompanyDto>>(authId.ToString());

                    authCachedRecord = authCachedRecord.Where(x => x.Id == companyId);

                    if (!authCachedRecord.Any()) return false;

                    return true;
                }
            }

            //private async Task<IEnumerable<AuthCompanyDto>> FetchCompanies(Guid authId)
            //{
            //    try
            //    {
            //        var companies = new List<AuthCompanyDto>();

            //        _context.Database.OpenConnection();
            //        using (DbCommand cmdObj = _context.Database.GetDbConnection().CreateCommand())
            //        {
            //            cmdObj.CommandText = "spFetchUsersCompanyAccess";
            //            cmdObj.CommandType = CommandType.StoredProcedure;
            //            cmdObj.Parameters.Add(new SqlParameter("@uUserId", authId));
            //            using var drObj2 = await cmdObj.ExecuteReaderAsync();
            //            companies = drObj2.MapToList<AuthCompanyDto>();
            //            drObj2.Close();
            //        }

            //        return companies;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception($"Db Error : {ex.Message}");
            //    }
            //}
        }
    }
}
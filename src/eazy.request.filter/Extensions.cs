using eazy.request.filter.Cache;
using eazy.request.filter.EfCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace eazy.request.filter
{
    public static class Extensions
    {

        public static IServiceCollection AddEazyRequestFilter<TContext>(this IServiceCollection services,
            IConfiguration configuration) 
            where TContext : DbContext
        {
            services.AddEfCore<TContext>(configuration);
            services.AddCacheable(configuration);

            return services;
        }
         
        public static IApplicationBuilder UseEazyRequestFilter(this IApplicationBuilder app, IConfiguration configuration)
        {
            configuration.GetSection("");

            return app;
        }
    }
}

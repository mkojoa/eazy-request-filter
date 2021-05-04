using eazy.request.filter.Cache.Option;
using eazy.request.filter.Cache.Store.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace eazy.request.filter.Cache
{
    public static class Extension
    {

        public static IServiceCollection AddCacheable(this IServiceCollection services, IConfiguration Configuration)
        {

            var options = new Cacheable();
            Configuration.GetSection(nameof(Cacheable)).Bind(options);

            var redisOptions = options.Redis ?? new RedisOptions();

            if (redisOptions.Enable == true)
            {
                services.RedisCache(redisOptions);
            }

            return services;
        }
    }
}

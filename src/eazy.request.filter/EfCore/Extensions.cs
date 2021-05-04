using eazy.request.filter.EfCore.Option;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eazy.request.filter.EfCore
{
    public static class Extensions
    {

        public static IServiceCollection AddEfCore<TContext>(this IServiceCollection services, IConfiguration Configuration)
            where TContext : DbContext
        {
            var option = new EfCoreOptions();
            Configuration.GetSection(nameof(EfCoreOptions)).Bind(option);

            services.AddDbContext<TContext>(
                options =>
                options.UseSqlServer(
                    option.ConnectionString
                ));

            return services;
        }
    }
}

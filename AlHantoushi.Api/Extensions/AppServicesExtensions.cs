
using AlHantoushi.Core.Interfaces;
using AlHantoushi.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace AlHantoushi.API.Extensions;

public static class AppServicesExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddDbContext<StoreContext>(opt =>
        {
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        });

        return services;
    }


}

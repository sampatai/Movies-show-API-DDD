using Microsoft.Extensions.DependencyInjection;
using MoviesTicket.Infrastructure.Repository;

namespace MoviesTicket.Infrastructure.DependencyExtensions;

public static class DepedencyExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IReadOnlyMovieRepository, ReadOnlyMovieRepository>();
        services.AddScoped<IEventLogRepository, EventLogRepository>();

        return services;
    }
}


using Microsoft.EntityFrameworkCore;
using MoviesTicket.Infrastructure;


namespace MoviesTicket.API.DependencyExtensions
{
    public static class DependencyExtension
    {
        public static IServiceCollection AddMovies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MoviesTicketDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("MoviesTicketDbContext"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            },
           ServiceLifetime.Scoped);


            return services;
        }

    }
}

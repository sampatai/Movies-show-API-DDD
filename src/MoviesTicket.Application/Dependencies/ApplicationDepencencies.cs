using Microsoft.Extensions.DependencyInjection;
using MoviesTicket.Application.Behaviors;
using System.Reflection;


namespace MoviesTicket.Application.Dependencies
{
    public static class ApplicationDepencencies
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
           
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                config.AddOpenBehavior(typeof(ValidatorBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));

            });
            return services;
        }

    }
}

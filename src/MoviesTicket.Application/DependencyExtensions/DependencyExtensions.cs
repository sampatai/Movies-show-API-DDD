﻿using Microsoft.Extensions.DependencyInjection;
using MoviesTicket.Application.Behaviors;
using System.Reflection;

namespace MoviesTicket.Application.DependencyExtensions
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            var applicationAssembly = Assembly.GetExecutingAssembly();

            
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(applicationAssembly);
                config.AddOpenBehavior(typeof(ValidatorBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));

            });

            // Register FluentValidation validators
            services.AddValidatorsFromAssembly(applicationAssembly, includeInternalTypes: true);
            return services;
        }

    }
}
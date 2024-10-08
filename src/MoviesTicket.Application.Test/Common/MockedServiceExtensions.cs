using Microsoft.Extensions.DependencyInjection;
using Moq.AutoMock;
using MoviesTicket.Shared.SeedWork;

namespace MoviesTicket.Application.Test.Common
{
    public static class MockedServiceExtensions
    {
        public static IServiceCollection ScopedMockedRepository<T, TAggregateRoot>(this IServiceCollection services, AutoMocker mocker)
       where T : class, IRepository<TAggregateRoot>
       where TAggregateRoot : IAggregateRoot
        {
            services.AddScoped<T>(_ => mocker.GetMock<T>().Object);
            mocker.GetMock<T>()
                .SetupGet(x => x.UnitOfWork)
                .Returns(mocker.GetMock<IUnitOfWork>().Object);

            return services;
        }

        public static IServiceCollection ScopedMockedReadOnlyRepository<T, TAggregateRoot>(this IServiceCollection services, AutoMocker mocker)
       where T : class, IReadOnlyRepository<TAggregateRoot>
       where TAggregateRoot : IAggregateRoot
        {
            services.AddScoped<T>(_ => mocker.GetMock<T>().Object);

            return services;
        }
        public static IServiceCollection MockedScoped<T>(this IServiceCollection services, AutoMocker mocker)
        where T : class
        {
            services.AddScoped<T>(_ => mocker.GetMock<T>().Object);
            return services;
        }
    }
}

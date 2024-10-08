using AutoFixture;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq.AutoMock;
using MoviesTicket.Application.Behaviors;
using MoviesTicket.Shared.SeedWork;


namespace MoviesTicket.Application.Test.Common;

public abstract class CommonTestBase
{
    protected AutoMocker Mocker = new AutoMocker();
    protected Fixture Fixture = new Fixture();
    protected IMediator Mediator;
    protected IServiceCollection Services;
    protected ServiceProvider Provider;

    [SetUp]
    public virtual void Setup()
    {
        Mocker = new AutoMocker();
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        Services = new ServiceCollection();

        Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        Services.AddMemoryCache();

        this.ServicesSetup();
        Provider = Services.BuildServiceProvider();

        HandlerSetup();
    }
    public virtual void HandlerSetup()
    { }

    public virtual void ServicesSetup()
    {
    }

    protected void ScopedMockedRepository<TRepository, TAggregate>()
       where TRepository : class, IRepository<TAggregate>
       where TAggregate : IAggregateRoot
       => Services.ScopedMockedRepository<TRepository, TAggregate>(Mocker);

    protected void ScopedMockedReadOnlyRepository<TRepository, TAggregate>()
        where TRepository : class, IReadOnlyRepository<TAggregate>
        where TAggregate : IAggregateRoot
        => Services.ScopedMockedReadOnlyRepository<TRepository, TAggregate>(Mocker);
    protected void MockedScoped<T>()
      where T : class
      => Services.MockedScoped<T>(Mocker);


}
public abstract class HandlerTestBase<THandler, TRequest> : CommonTestBase
    where THandler : IRequestHandler<TRequest>
    where TRequest : IRequest
{
    protected THandler Handler;
    protected TRequest Command;

    [SetUp]
    public new virtual void Setup()
    {
        Mocker = new AutoMocker();
        var assembly = typeof(THandler).Assembly;
        var validatorLogger = Mocker.GetMock<ILogger<ValidatorBehavior<TRequest, Unit>>>();

        Services = new ServiceCollection();

        
        Services.AddValidatorsFromAssemblyContaining<TRequest>(lifetime: ServiceLifetime.Scoped);
        Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

        Services.AddMediatR(x => x.RegisterServicesFromAssemblies(assembly));
        Services.AddScoped<ILogger<ValidatorBehavior<TRequest, Unit>>>(f => validatorLogger.Object);
        Services.AddTransient<ILogger<THandler>>(_ => Mocker.GetMock<ILogger<THandler>>().Object);
        Services.AddMemoryCache();

        this.ServicesSetup();


        Services.AddTransient<ILogger<THandler>>(_ =>
            Mocker.GetMock<ILogger<THandler>>().Object
        );
       
        Provider = Services.BuildServiceProvider();
        Mediator = Provider.GetRequiredService<IMediator>();


        HandlerSetup();
    }

    public new virtual void HandlerSetup()
    { }

    public new virtual void ServicesSetup()
    {
    }


    protected virtual TRequest CreateCommand()
        => default(TRequest);
}

public abstract class HandlerTestBase<THandlder, TRequest, TResponse> : CommonTestBase
    where THandlder : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected THandlder Handler;
    protected TRequest Query;
    protected TResponse Response;

    [SetUp]
    public new virtual void Setup()
    {
        var validatorLogger = Mocker.GetMock<ILogger<ValidatorBehavior<TRequest, TResponse>>>();

        var assembly = typeof(THandlder).Assembly;

        Services = new ServiceCollection();

      
        Services.AddValidatorsFromAssemblyContaining<TRequest>(ServiceLifetime.Scoped);
        Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        Services.AddMemoryCache();
        Services.AddMediatR(x => x.RegisterServicesFromAssemblies(assembly));
        Services.AddScoped<ILogger<ValidatorBehavior<TRequest, TResponse>>>(f => validatorLogger.Object);
        Services.AddTransient<ILogger<THandlder>>(_ => Mocker.GetMock<ILogger<THandlder>>().Object);        
        this.ServicesSetup();
        Provider = Services.BuildServiceProvider();
        Mediator = Provider.GetRequiredService<IMediator>();

        HandlerSetup();
    }

    public new virtual void HandlerSetup()
    { }

    public new virtual void ServicesSetup()
    {
    }
    protected virtual TRequest CreateQuery()
        => default(TRequest);
}

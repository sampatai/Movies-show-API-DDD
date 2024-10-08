using MoviesTicket.Domain.Aggregates.Enumerations;

namespace MoviesTicket.Application.Test.Command
{
    internal class CreateMovieTest : HandlerTestBase<Handler, CreateMovieCommand.Command>
    {
       
        public override void ServicesSetup()
        {
            base.ServicesSetup();
            ScopedMockedRepository<IMovieRepository, Movies>();
            Mocker.GetMock<IMovieRepository>()
                .Setup(x => x.AddAsync(It.IsAny<Movies>(), It.IsAny<CancellationToken>()));
            Command = CreateCommand();
        }
        [Test]
        public void Should_Have_Error_When_Title_Is_Empty()
        {

            Command.Title = string.Empty;

            var assertion = Assert.ThrowsAsync<ValidationException>(async () =>await Mediator.Send(Command))!;
            assertion.Errors.Should().Contain(x => x.PropertyName == nameof(CreateMovieCommand.Command.Title) && x.ErrorMessage == "Title is required.");
        }

        [Test]
        public void Should_Have_Error_When_ReleaseDate_Is_In_The_Future()
        {


            Command.ReleaseDate = DateTime.Now.AddDays(1);
            var assertion = Assert.ThrowsAsync<ValidationException>(async () =>await Mediator.Send(Command))!;
            assertion.Errors.Should().Contain(x => x.PropertyName == nameof(CreateMovieCommand.Command.ReleaseDate) && x.ErrorMessage == "Release date cannot be in the past.");
        }

        [Test]
        public void Should_Have_Error_When_Genre_Is_Invalid()
        {


            Command.Genres = new Domain.Aggregates.Enumerations.MovieGenres(0, "");
            var assertion = Assert.ThrowsAsync<ValidationException>(() => Mediator.Send(Command))!;
            assertion.Errors.Should().Contain(x => x.PropertyName == nameof(CreateMovieCommand.Command.Genres) && x.ErrorMessage == "Invalid genre.");
        }

        [Test]
        public void Should_Have_Error_When_Runtime_Is_Invalid()
        {
            Command.Runtime = "90 minutes";

            var assertion = Assert.ThrowsAsync<ValidationException>(() => Mediator.Send(Command))!;
            assertion.Errors.Should().Contain(x => x.PropertyName == nameof(CreateMovieCommand.Command.Runtime) && x.ErrorMessage == "Runtime should be in the format 'xxx min'.");
        }

        [Test]
        public void Should_Have_Error_When_Director_Is_Empty()
        {
            Command.Director = string.Empty;

            var assertion = Assert.ThrowsAsync<ValidationException>(() => Mediator.Send(Command))!;
            assertion.Errors.Should().Contain(x => x.PropertyName == nameof(CreateMovieCommand.Command.Director) && x.ErrorMessage == "Director is required.");
        }

        [Test]
        public void Should_Have_Error_When_Synopsis_Is_Too_Long()
        {
            Command.Synopsis = new string('a', 1001);

            var assertion = Assert.ThrowsAsync<ValidationException>(() => Mediator.Send(Command))!;
            assertion.Errors.Should().Contain(x => x.PropertyName == nameof(CreateMovieCommand.Command.Synopsis) && x.ErrorMessage == "Synopsis cannot be longer than 1000 characters.");
        }

        [Test]
        public async Task Handle_Should_Add_Valid_Movie()
        {
            // Arrange

            // Act
            await Mediator.Send(CreateCommand());

            // Assert
            Mocker.GetMock<IMovieRepository>().Verify(x => x.AddAsync(It.IsAny<Movies>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        protected override CreateMovieCommand.Command CreateCommand() => Fixture.Build<CreateMovieCommand.Command>()
            .With(c => c.Title, Fixture.Create<string>)
            .With(c => c.ReleaseDate, DateTime.Now.AddDays(-1)) 
            .With(c => c.Genres, MovieGenres.G) 
            .With(c => c.Runtime, "120 min")
            .With(c => c.Director, Fixture.Create<string>)
            .With(c => c.Synopsis, Fixture.Create<string>)
            .Create();

    }
}

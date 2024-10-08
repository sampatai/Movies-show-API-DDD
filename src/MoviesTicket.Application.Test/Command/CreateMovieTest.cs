using MoviesTicket.Domain.Aggregates.Enumerations;

namespace MoviesTicket.Application.Test.Command
{
    internal class CreateMovieTest : HandlerTestBase<Handler, CreateMovieCommand.Command>
    {
        CreateMovieCommand.Command _command;
        public override void ServicesSetup()
        {
            base.ServicesSetup();
            ScopedMockedRepository<IMovieRepository, Movies>();
            Mocker.GetMock<IMovieRepository>()
                .Setup(x => x.AddAsync(It.IsAny<Movies>(), It.IsAny<CancellationToken>()));
            _command = CreateCommand();
        }
        [Test]
        public async Task Should_Have_Error_When_Title_Is_Empty()
        {

            _command.Title = string.Empty;

            var assertion = Assert.ThrowsAsync<ValidationException>(async () =>await Mediator.Send(_command))!;
            assertion.Errors.Should().Contain(x => x.PropertyName == nameof(CreateMovieCommand.Command.Title) && x.ErrorMessage == "Title is required.");
        }

        [Test]
        public async Task Should_Have_Error_When_ReleaseDate_Is_In_The_Future()
        {


            _command.ReleaseDate = DateTime.Now.AddDays(1);
            var assertion = Assert.ThrowsAsync<ValidationException>(async () =>await Mediator.Send(_command))!;
            assertion.Errors.Should().Contain(x => x.PropertyName == nameof(CreateMovieCommand.Command.ReleaseDate) && x.ErrorMessage == "Release date cannot be in the past.");
        }

        [Test]
        public async Task Should_Have_Error_When_Genre_Is_Invalid()
        {


            _command.Genres = new Domain.Aggregates.Enumerations.MovieGenres(0, "");
            var assertion = Assert.ThrowsAsync<ValidationException>(() => Mediator.Send(_command))!;
            assertion.Errors.Should().Contain(x => x.PropertyName == nameof(CreateMovieCommand.Command.Genres) && x.ErrorMessage == "Invalid genre.");
        }

        [Test]
        public async Task Should_Have_Error_When_Runtime_Is_Invalid()
        {
            _command.Runtime = "90 minutes";

            var assertion = Assert.ThrowsAsync<ValidationException>(() => Mediator.Send(_command))!;
            assertion.Errors.Should().Contain(x => x.PropertyName == nameof(CreateMovieCommand.Command.Runtime) && x.ErrorMessage == "Runtime should be in the format 'xxx min'.");
        }

        [Test]
        public async Task Should_Have_Error_When_Director_Is_Empty()
        {
            _command.Director = string.Empty;

            var assertion = Assert.ThrowsAsync<ValidationException>(() => Mediator.Send(_command))!;
            assertion.Errors.Should().Contain(x => x.PropertyName == nameof(CreateMovieCommand.Command.Director) && x.ErrorMessage == "Director is required.");
        }

        [Test]
        public async Task Should_Have_Error_When_Synopsis_Is_Too_Long()
        {
            _command.Synopsis = new string('a', 1001);

            var assertion = Assert.ThrowsAsync<ValidationException>(() => Mediator.Send(_command))!;
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
            .With(c => c.Title, Fixture.Create<string>) // Invalid title for this test
            .With(c => c.ReleaseDate, DateTime.Now.AddDays(-1)) // Ensure the date is in the past
            .With(c => c.Genres, MovieGenres.G) // Assuming 1 is a valid genre ID
            .With(c => c.Runtime, "120 min")
            .With(c => c.Director, Fixture.Create<string>)
            .With(c => c.Synopsis, Fixture.Create<string>)
            .Create();

    }
}

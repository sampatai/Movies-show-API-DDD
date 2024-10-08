using MoviesTicket.Application.CommandQuery.Query;
using MoviesTicket.Application.Projections;
using MoviesTicket.Domain.Aggregates.Enumerations;

namespace MoviesTicket.Application.Test.Query
{
    internal class GetMoviesTest : HandlerTestBase<GetMovies.Handler, GetMovies.Query, ListMovie>
    {
        private readonly int _resultCount = 10;
        public override void ServicesSetup()
        {
            base.ServicesSetup();
            ScopedMockedReadOnlyRepository<IReadOnlyMovieRepository, Movies>();
            Mocker.GetMock<IReadOnlyMovieRepository>()
                .Setup(x => x.GetMovies(It.IsAny<FilterModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => (Fixture.CreateMany<Movies>(_resultCount), _resultCount));
            Query = CreateQuery();
        }
        [Test]
        public async Task Handle_Should_Return_ListMovie()
        {
            // Arrange


            // Act
            var result = await Mediator.Send(Query);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ListMovie>();
        }

        [Test]
        public void Should_Have_Error_When_PageNumber_Is_Less_Than_One()
        {
            // Arrange
            Query.PageNumber = 0;

            // Act
            AsyncTestDelegate sut = () => Mediator.Send(Query);
            // Assert
            var assertion = Assert.ThrowsAsync<ValidationException>(sut);

            assertion.Errors.Should().Contain(x => x.PropertyName == nameof(GetMovies.Query.PageNumber) && x.ErrorMessage == "Page number must be greater than 0.");
        }

        [Test]
        public void Should_Have_Error_When_PageSize_Is_Out_Of_Range()
        {
            // Arrange
            Query.PageSize = 101;

            // Act
            AsyncTestDelegate sut = () => Mediator.Send(Query);

            // Assert
            var assertion = Assert.ThrowsAsync<ValidationException>(sut);

            assertion.Errors.Should().Contain(x => x.PropertyName == nameof(GetMovies.Query.PageSize) && x.ErrorMessage == "Page size must be between 1 and 100.");
        }

        [Test]
        public void Should_Have_Error_When_Genre_Is_Invalid()
        {
            // Arrange
            Query.Genres = new MovieGenres(101, "Test");

            // Act
            AsyncTestDelegate sut = () => Mediator.Send(Query);

            // Assert
            var assertion = Assert.ThrowsAsync<ValidationException>(sut);
            assertion.Errors.Should().Contain(x => x.PropertyName == nameof(GetMovies.Query.Genres) && x.ErrorMessage == "Invalid genre.");
        }

        protected override GetMovies.Query CreateQuery() => Query = Fixture.Build<GetMovies.Query>()
           .With(q => q.PageNumber, 1)
           .With(q => q.PageSize, 10)
           .With(q => q.Genres, (MovieGenres)null)
            .With(q => q.ReleaseDate, (DateTime?)null)
             .With(q => q.Title, string.Empty)
             .With(q => q.ShowTime, string.Empty)
           .Create();
    }
}

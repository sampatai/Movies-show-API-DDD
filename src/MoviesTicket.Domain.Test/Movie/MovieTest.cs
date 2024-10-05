using AutoFixture;
using MoviesTicket.Domain.Aggregates.Enumerations;
using MoviesTicket.Domain.Aggregates.Root;
using Faker.Extensions;
using FluentAssertions;
using System.IO;

namespace MoviesTicket.Domain.Test.Movie;

public class MovieTest
{
    private Movies _movie;
    private string _title, _runtime, _director, _synopsis;
    private DateTime _releaseDate;
    MovieGenres _movieGenres;
    private Fixture _faker = new Fixture();

    [SetUp]
    public void Setup()
    {
        _title = _faker.Create<string>();
        _runtime = _faker.Create<string>();
        _director = _faker.Create<string>();
        _synopsis = _faker.Create<string>();
        _releaseDate = _faker.Create<DateTime>();
        _movieGenres = MovieGenres.G;
        _movie = _Init();

    }

    [TestCase("")]
    [TestCase(null)]
    public void validation_should_fail_if_title_null_or_empty(string title)
    {
        _title = title;
        if (_title == null)
            Assert.Throws<ArgumentNullException>(() => _Init());
        else Assert.Throws<ArgumentException>(() => _Init());
    }

    [Test]
    public void validation_should_pass_if_title_is_valid()
    {
        _title = "Valid Title";
        Assert.DoesNotThrow(() => _Init());
    }

    [TestCase("")]
    [TestCase(null)]
    public void validation_should_fail_if_runtime_null_or_empty(string runtime)
    {
        _runtime = runtime;
        if (_runtime == null)
            Assert.Throws<ArgumentNullException>(() => _Init());
        else Assert.Throws<ArgumentException>(() => _Init());
    }

    [Test]
    public void validation_should_pass_if_runtime_is_valid()
    {
        _runtime = "120 minutes";
        Assert.DoesNotThrow(() => _Init());
    }

    [TestCase("")]
    [TestCase(null)]

    public void validation_should_fail_if_director_null_or_empty(string director)
    {
        _director = director;
        if (_director == null)
            Assert.Throws<ArgumentNullException>(() => _Init());
        else Assert.Throws<ArgumentException>(() => _Init());
    }

    [Test]
    public void validation_should_pass_if_director_is_valid()
    {
        _director = "Valid Director";
        Assert.DoesNotThrow(() => _Init());
    }

    [TestCase("")]
    [TestCase(null)]
    public void validation_should_fail_if_synopsis_null_or_empty(string synopsis)
    {
        _synopsis = synopsis;
        if (_synopsis == null)
            Assert.Throws<ArgumentNullException>(() => _Init());
        else Assert.Throws<ArgumentException>(() => _Init());
    }

    [Test]
    public void validation_should_pass_if_synopsis_is_valid()
    {
        _synopsis = "Valid Synopsis";
        Assert.DoesNotThrow(() => _Init());
    }

    [Test]
    public void validation_should_pass_if_releasedate_is_valid()
    {
        _releaseDate = DateTime.Now;
        Assert.DoesNotThrow(() => _Init());
    }

    [Test]
    public void validation_should_pass_if_moviegenres_is_valid()
    {
        _movieGenres = MovieGenres.G;
        Assert.DoesNotThrow(() => _Init());
    }
    [Test]
    public void SetMovie_should_faild_if_invalid_input()
    {
        _title = string.Empty;
        _runtime = string.Empty;

        Assert.Throws<ArgumentException>(() => _movie.SetMovie(_title, _releaseDate, _movieGenres, _runtime, _director, _synopsis));

    }
    [Test]
    public void SetMovie_should_pass_if_valid_input()
    {
        _title = _faker.Create<string>();
        _runtime = _faker.Create<string>();
        _movieGenres = MovieGenres.PG13;
        Assert.DoesNotThrow(() => _movie.SetMovie(_title, _releaseDate, _movieGenres, _runtime, _director, _synopsis));
        _title.Should().Be(_title);
        _movieGenres.Should().Be(MovieGenres.PG13);
        _runtime.Should().Be(_runtime);


    }
    private Movies _Init() => new Movies(_title, _releaseDate, _movieGenres, _runtime, _director, _synopsis);
}


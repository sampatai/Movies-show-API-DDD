using AutoFixture;
using MoviesTicket.Domain.Aggregates.Enumerations;
using MoviesTicket.Domain.Aggregates.Root;
using Faker.Extensions;
using FluentAssertions;
using System.IO;
using MoviesTicket.Domain.Aggregates.Entities;
using MoviesTicket.Domain.Aggregates.ValueObjects;

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
  //  [TestCase(null)]
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
   // [TestCase(null)]
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
   // [TestCase(null)]

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
  //  [TestCase(null)]
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
    public void setmovie_should_faild_if_invalid_input()
    {
        _title = string.Empty;
        _runtime = string.Empty;

        Assert.Throws<ArgumentException>(() => _movie.SetMovie(_title, _releaseDate, _movieGenres, _runtime, _director, _synopsis));

    }
    [Test]
    public void setmovie_should_throwargumentexception_if_titleisnullorempty()
    {
        _title = string.Empty;
        Assert.Throws<ArgumentException>(() => _movie.SetMovie(_title, _releaseDate, _movieGenres, _runtime, _director, _synopsis));
    }

    

    [Test]
    public void setmovie_should_throwargumentexception_if_moviegenresisnull()
    {
        _movieGenres = null;
        Assert.Throws<ArgumentNullException>(() => _movie.SetMovie(_title, _releaseDate, _movieGenres, _runtime, _director, _synopsis));
    }

    [Test]
    public void setmovie_should_throwargumentexception_if_runtimeisnullorempty()
    {
        _runtime = string.Empty;
        Assert.Throws<ArgumentException>(() => _movie.SetMovie(_title, _releaseDate, _movieGenres, _runtime, _director, _synopsis));
    }

    [Test]
    public void setmovie_should_throwargumentexception_if_directorisnullorempty()
    {
        _director = string.Empty;
        Assert.Throws<ArgumentException>(() => _movie.SetMovie(_title, _releaseDate, _movieGenres, _runtime, _director, _synopsis));
    }

    [Test]
    public void setmovie_should_throwargumentexception_if_synopsisisnullorempty()
    {
        _synopsis = string.Empty;
        Assert.Throws<ArgumentException>(() => _movie.SetMovie(_title, _releaseDate, _movieGenres, _runtime, _director, _synopsis));
    }
    [Test]
    public void setinactive_should_set_isactive_to_false()
    {
        //Arrange
        //Act
        _movie.SetInactive();
        //Assert
        _movie.IsActive.Should().BeFalse();
    }

    [Test]
    public void addshowstimes_should_add_showstimes_to_list()
    {
        //Arrange
        var newShowsTimes = _faker.Create<List<ShowsTime>>();
        //Act
        _movie.AddShowsTimes(newShowsTimes);
        //Assert
        _movie.ShowsTimes.Should().Contain(newShowsTimes.First());
    }

    [Test]
    public void setshowtime_should_update_showtime()
    {
        //Arrange
        var newShowsTimes = _faker.Create<List<ShowsTime>>();
        _movie.AddShowsTimes(newShowsTimes);
        var showTimeGuid = _movie.ShowsTimes.First().ShowsTimeGUID;
        var newDate = DateTime.Now;
        var newTime = "10:00 AM";
        //Act
        _movie.SetShowTime(showTimeGuid, newDate, newTime);
        //Assert
        var updatedShowTime = _movie.ShowsTimes.Single(x => x.ShowsTimeGUID == showTimeGuid);
        newDate.Should().Be(updatedShowTime.ShowDate);
        newTime.Should().Be(updatedShowTime.Time);
    }

    [Test]
    public void addreservation_should_add_reservation_to_showtime()
    {
        //Arrange
        var newShowsTimes = _faker.Create<List<ShowsTime>>();
        _movie.AddShowsTimes(newShowsTimes);
        var showTimeGuid = _movie.ShowsTimes.First().ShowsTimeGUID;
        var reservation = _faker.Create<Reservation>();
        //act
        _movie.AddReservation(showTimeGuid, reservation);
        //assert
        var updatedShowTime = _movie.ShowsTimes.Single(x => x.ShowsTimeGUID == showTimeGuid);
        _movie.ShowsTimes.First().Reservation.Should().Contain(reservation);

    }

    [Test]
    public void deletereservation_should_remove_reservation_from_showtime()
    {
        //Arrange
        var newShowsTimes = _faker.Create<List<ShowsTime>>();
        _movie.AddShowsTimes(newShowsTimes);
        var showTimeGuid = _movie.ShowsTimes.First().ShowsTimeGUID;
        var reservation = _faker.Create<Reservation>();
        _movie.AddReservation(showTimeGuid, reservation);

        _movie.DeleteReservation(showTimeGuid, reservation);

        var updatedShowTime = _movie.ShowsTimes.Single(x => x.ShowsTimeGUID == showTimeGuid);
        _movie.ShowsTimes.First().Reservation.Should().NotContain(reservation);
    }


    private Movies _Init() => new Movies(_title, _releaseDate, _movieGenres, _runtime, _director, _synopsis);
}


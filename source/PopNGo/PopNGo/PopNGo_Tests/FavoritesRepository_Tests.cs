using Moq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PopNGo.DAL.Concrete;
using PopNGo.DAL.Abstract;
using PopNGo.Models;
using System.Collections.Generic;

namespace PopNGo_Tests;

public class FavoritesRepositoryTests
{
    private Mock<PopNGoDB> _mockContext;
    private Mock<DbSet<FavoriteEvent>> _mockSet;
    private FavoritesRepository _favoritesRepository;
    private Mock<DbSet<Event>> _mockEventSet;
    private List<Event> _eventsData;

    private Mock<DbSet<T>> GetMockDbSet<T>(List<T> list) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(list.AsQueryable().Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(list.AsQueryable().Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(list.AsQueryable().ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(list.AsQueryable().GetEnumerator());
        return mockSet;
    }

    [SetUp]
    public void Setup()
    {
        _mockContext = new Mock<PopNGoDB>();
        _mockSet = GetMockDbSet(new List<FavoriteEvent>());
        _mockContext.Setup(c => c.FavoriteEvents).Returns(_mockSet.Object);

        _eventsData = new List<Event>();
        _mockEventSet = GetMockDbSet(_eventsData);
        _mockContext.Setup(c => c.Events).Returns(_mockEventSet.Object);

        _favoritesRepository = new FavoritesRepository(_mockContext.Object);
    }

    [Test]
    public void AddFavorite_ShouldAddNewFavoriteEvent()
    {
        // Arrange
        var userId = 1;
        var eventId = "testEventId";
        var eventEntity = new Event { ApiEventId = eventId, Id = 2 };

        _eventsData.Add(eventEntity);

        // Act
        _favoritesRepository.AddFavorite(userId, eventId);

        // Assert
        _mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Test]
    public void AddFavorite_ShouldThrowArgumentException_WhenEventIdIsNull()
    {
        // Arrange
        var userId = 1;
        var eventId = "";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _favoritesRepository.AddFavorite(userId, eventId));
    }

    [Test]
    public void AddFavorite_ShouldThrowArgumentException_WhenEventNotFound()
    {
        // Arrange
        var userId = 1;
        var eventId = "testEventId";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _favoritesRepository.AddFavorite(userId, eventId));
    }

    [Test]
    public void RemoveFavorite_ShouldRemoveFavoriteEvent()
    {
        // Arrange
        var userId = 1;
        var apiEventId = "testEventId";
        var eventEntity = new Event { Id = 2, ApiEventId = apiEventId };
        var favoriteEvent = new FavoriteEvent { UserId = userId, EventId = eventEntity.Id, Event = eventEntity };

        var data = new List<FavoriteEvent> { favoriteEvent }.AsQueryable();

        _mockSet.As<IQueryable<FavoriteEvent>>().Setup(m => m.Provider).Returns(data.Provider);
        _mockSet.As<IQueryable<FavoriteEvent>>().Setup(m => m.Expression).Returns(data.Expression);
        _mockSet.As<IQueryable<FavoriteEvent>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _mockSet.As<IQueryable<FavoriteEvent>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _mockContext.Setup(c => c.Set<FavoriteEvent>()).Returns(_mockSet.Object);

        // Assuming _favoritesRepository is created like this:
        _favoritesRepository = new FavoritesRepository(_mockContext.Object);

        // Act
        _favoritesRepository.RemoveFavorite(userId, apiEventId);

        // Assert
        _mockSet.Verify(m => m.Remove(It.IsAny<FavoriteEvent>()), Times.Once);
        _mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Test]
    public void RemoveFavorite_ShouldThrowArgumentException_WhenApiEventIdIsNull()
    {
        // Arrange
        var userId = 1;
        var apiEventId = "";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _favoritesRepository.RemoveFavorite(userId, apiEventId));
    }

    [Test]
    public void RemoveFavorite_ShouldThrowArgumentException_WhenFavoriteEventNotFound()
    {
        // Arrange
        var userId = 1;
        var apiEventId = "testEventId";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _favoritesRepository.RemoveFavorite(userId, apiEventId));
    }

    [Test]
    public void GetUserFavorites_ShouldReturnUserFavorites()
    {
        // Arrange
        var userId = 1;
        var apiEventId = "testEventId";
        var eventEntity = new Event { Id = 2, ApiEventId = apiEventId };
        var favoriteEvent = new FavoriteEvent { UserId = userId, EventId = eventEntity.Id, Event = eventEntity };

        var data = new List<FavoriteEvent> { favoriteEvent }.AsQueryable();

        _mockSet.As<IQueryable<FavoriteEvent>>().Setup(m => m.Provider).Returns(data.Provider);
        _mockSet.As<IQueryable<FavoriteEvent>>().Setup(m => m.Expression).Returns(data.Expression);
        _mockSet.As<IQueryable<FavoriteEvent>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _mockSet.As<IQueryable<FavoriteEvent>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _mockContext.Setup(c => c.Set<FavoriteEvent>()).Returns(_mockSet.Object);

        // Assuming _favoritesRepository is created like this:
        _favoritesRepository = new FavoritesRepository(_mockContext.Object);

        // Act
        var result = _favoritesRepository.GetUserFavorites(userId);

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].ApiEventID, Is.EqualTo(apiEventId));
    }

    [Test]
    public void GetUserFavorites_ShouldReturnEmptyList_WhenNoFavoritesFound()
    {
        // Arrange
        var userId = 1;

        // Act
        var result = _favoritesRepository.GetUserFavorites(userId);

        // Assert
        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public void GetUserFavorites_ShouldReturnEmptyList_WhenUserIdIsInvalid()
    {
        // Arrange
        var userId = 0;

        // Act
        var result = _favoritesRepository.GetUserFavorites(userId);

        // Assert
        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public void IsFavorite_WhenApiEventIdIsNull_ShouldThrowArgumentException()
    {
        // Arrange
        var userId = 1;
        string? apiEventId = null;

        // Assuming _favoritesRepository is created like this:
        _favoritesRepository = new FavoritesRepository(_mockContext.Object);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _favoritesRepository.IsFavorite(userId, apiEventId));
    }

    [Test]
    public void IsFavorite_WhenApiEventIdIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var userId = 1;
        string apiEventId = "";

        // Assuming _favoritesRepository is created like this:
        _favoritesRepository = new FavoritesRepository(_mockContext.Object);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _favoritesRepository.IsFavorite(userId, apiEventId));
    }

    [Test]
    public void IsFavorite_WhenEventIsFavorite_ShouldReturnTrue()
    {
        // Arrange
        var userId = 1;
        var apiEventId = "testEventId";
        var eventEntity = new Event { Id = 2, ApiEventId = apiEventId };
        var favoriteEvent = new FavoriteEvent { UserId = userId, EventId = eventEntity.Id, Event = eventEntity };

        var data = new List<FavoriteEvent> { favoriteEvent }.AsQueryable();

        _mockSet.As<IQueryable<FavoriteEvent>>().Setup(m => m.Provider).Returns(data.Provider);
        _mockSet.As<IQueryable<FavoriteEvent>>().Setup(m => m.Expression).Returns(data.Expression);
        _mockSet.As<IQueryable<FavoriteEvent>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _mockSet.As<IQueryable<FavoriteEvent>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _mockContext.Setup(c => c.Set<FavoriteEvent>()).Returns(_mockSet.Object);

        // Assuming _favoritesRepository is created like this:
        _favoritesRepository = new FavoritesRepository(_mockContext.Object);

        // Act
        var result = _favoritesRepository.IsFavorite(userId, apiEventId);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void IsFavorite_WhenEventIsNotFavorite_ShouldReturnFalse()
    {
        // Arrange
        var userId = 1;
        var apiEventId = "testEventId";
        var eventEntity = new Event { Id = 2, ApiEventId = apiEventId };
        var favoriteEvent = new FavoriteEvent { UserId = userId + 1, EventId = eventEntity.Id, Event = eventEntity }; // Different user

        var data = new List<FavoriteEvent> { favoriteEvent }.AsQueryable();

        _mockSet.As<IQueryable<FavoriteEvent>>().Setup(m => m.Provider).Returns(data.Provider);
        _mockSet.As<IQueryable<FavoriteEvent>>().Setup(m => m.Expression).Returns(data.Expression);
        _mockSet.As<IQueryable<FavoriteEvent>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _mockSet.As<IQueryable<FavoriteEvent>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _mockContext.Setup(c => c.Set<FavoriteEvent>()).Returns(_mockSet.Object);

        // Assuming _favoritesRepository is created like this:
        _favoritesRepository = new FavoritesRepository(_mockContext.Object);

        // Act
        var result = _favoritesRepository.IsFavorite(userId, apiEventId);

        // Assert
        Assert.IsFalse(result);
    }

}
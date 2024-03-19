using Moq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PopNGo.DAL.Concrete;
using PopNGo.DAL.Abstract;
using PopNGo.Models;
using System.Collections.Generic;

namespace PopNGo_Tests;

public class EventRepositoryTests
{
    private Mock<PopNGoDB> _mockContext;
    private Mock<DbSet<Event>> _mockSet;
    private EventRepository _eventRepository;

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
        _mockSet = GetMockDbSet(new List<Event>());
        _mockContext.Setup(c => c.Events).Returns(_mockSet.Object);
        _eventRepository = new EventRepository(_mockContext.Object);

        //string EventId, DateTime EventDate, string EventName, string EventDescription, string EventLocation

    }

    [Test]
    public void AddEvent_ShouldAddNewEvent()
    {
        // Arrange
        var eventId = "1";
        var eventDate = DateTime.Now;
        var eventName = "Test Event";
        var eventDescription = "Test Description";
        var eventLocation = "Test Location";
        var eventImage = "";

        var events = new List<Event>();
        _mockContext.Setup(m => m.Update(It.IsAny<Event>())).Callback<Event>(e => events.Add(e));

        // Act
        _eventRepository.AddEvent(eventId, eventDate, eventName, eventDescription, eventLocation, eventImage);

        // Assert
        Assert.That(events.Count, Is.EqualTo(1));
        var addedEvent = events.First();
        Assert.That(addedEvent.ApiEventId, Is.EqualTo(eventId));
        Assert.That(addedEvent.EventDate, Is.EqualTo(eventDate));
        Assert.That(addedEvent.EventName, Is.EqualTo(eventName));
        Assert.That(addedEvent.EventDescription, Is.EqualTo(eventDescription));
        Assert.That(addedEvent.EventLocation, Is.EqualTo(eventLocation));
        Assert.That(addedEvent.EventImage, Is.EqualTo(eventImage));
    }

    [Test]
    public void AddEvent_ShouldErrorIfEventIdIsNull()
    {
        // Arrange
        var eventId = "";
        var eventDate = DateTime.Now;
        var eventName = "Test Event";
        var eventDescription = "Test Description";
        var eventLocation = "Test Location";
        var eventImage = "";

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _eventRepository.AddEvent(eventId, eventDate, eventName, eventDescription, eventLocation, eventImage));

        // Assert
        Assert.That(ex.Message, Is.EqualTo("EventId cannot be null or empty (Parameter 'eventId')"));
    }

    [Test]
    public void AddEvent_ShouldErrorIfEventDateIsDefault()
    {
        // Arrange
        var eventId = "1";
        var eventDate = default(DateTime);
        var eventName = "Test Event";
        var eventDescription = "Test Description";
        var eventLocation = "Test Location";
        var eventImage = "";

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _eventRepository.AddEvent(eventId, eventDate, eventName, eventDescription, eventLocation, eventImage));

        // Assert
        Assert.That(ex.Message, Is.EqualTo("EventDate cannot be default (Parameter 'eventDate')"));
    }

    [Test]
    public void AddEvent_ShouldErrorIfEventNameIsNull()
    {
        // Arrange
        var eventId = "1";
        var eventDate = DateTime.Now;
        var eventName = "";
        var eventDescription = "Test Description";
        var eventLocation = "Test Location";
        var eventImage = "";

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _eventRepository.AddEvent(eventId, eventDate, eventName, eventDescription, eventLocation, eventImage));

        // Assert
        Assert.That(ex.Message, Is.EqualTo("EventName cannot be null or empty (Parameter 'eventName')"));
    }

    [Test]
    public void AddEvent_ShouldErrorIfEventDescriptionIsNull()
    {
        // Arrange
        var eventId = "1";
        var eventDate = DateTime.Now;
        var eventName = "Test Event";
        var eventDescription = "";
        var eventLocation = "Test Location";
        var eventImage = "";

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _eventRepository.AddEvent(eventId, eventDate, eventName, eventDescription, eventLocation, eventImage));

        // Assert
        Assert.That(ex.Message, Is.EqualTo("EventDescription cannot be null or empty (Parameter 'eventDescription')"));
    }

    [Test]
    public void AddEvent_ShouldErrorIfEventLocationIsNull()
    {
        // Arrange
        var eventId = "1";
        var eventDate = DateTime.Now;
        var eventName = "Test Event";
        var eventDescription = "Test Description";
        var eventLocation = "";
        var eventImage = "";

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _eventRepository.AddEvent(eventId, eventDate, eventName, eventDescription, eventLocation, eventImage));

        // Assert
        Assert.That(ex.Message, Is.EqualTo("EventLocation cannot be null or empty (Parameter 'eventLocation')"));
    }

    [Test]
    public void IsEvent_ShouldReturnTrueIfEventExists()
    {
        // Arrange
        var eventId = "1";
        var events = new List<Event>
        {
            new Event { ApiEventId = eventId }
        };
        _mockSet = GetMockDbSet(events);
        _mockContext.Setup(c => c.Events).Returns(_mockSet.Object);
        _eventRepository = new EventRepository(_mockContext.Object);

        // Act
        var result = _eventRepository.IsEvent(eventId);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsEvent_ShouldReturnFalseIfEventDoesNotExist()
    {
        // Arrange
        var eventId = "1";
        var events = new List<Event>
        {
            new Event { ApiEventId = "2" }
        };
        _mockSet = GetMockDbSet(events);
        _mockContext.Setup(c => c.Events).Returns(_mockSet.Object);
        _eventRepository = new EventRepository(_mockContext.Object);

        // Act
        var result = _eventRepository.IsEvent(eventId);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsEvent_ShouldErrorIfEventIdIsNull()
    {
        // Arrange
        var eventId = "";

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _eventRepository.IsEvent(eventId));

        // Assert
        Assert.That(ex.Message, Is.EqualTo("ApiEventId cannot be null or empty (Parameter 'apiEventId')"));
    }
}
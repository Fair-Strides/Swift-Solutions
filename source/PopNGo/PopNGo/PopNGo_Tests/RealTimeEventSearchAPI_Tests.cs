using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PopNGo.Services;
using System.Collections.Generic;
using PopNGo.Models;
using System.Linq; // Add this for LINQ extension methods

namespace PopNGo_Tests;

public class RealTimeEventSearchServiceAPI_Tests
{
    private HttpClient _httpClient;
    private Mock<ILogger<RealTimeEventSearchService>> _mockLogger;
    private RealTimeEventSearchService _service;

    [SetUp]
    public void Setup()
    {
        // Create a Mock of the HttpMessageHandler
        var handlerMock = new Mock<HttpMessageHandler>();

        var responseBody = "{\"status\":\"OK\",\"request_id\":\"xyz\",\"parameters\":{\"query\":\"music\",\"start\":0},\"data\":[{\"event_id\":\"123\",\"name\":\"Test Event\",\"link\":\"http://example.com/event\",\"description\":\"Description\",\"start_time\":\"2023-01-01T00:00:00Z\",\"end_time\":\"2023-01-02T00:00:00Z\",\"is_virtual\":false,\"thumbnail\":\"http://example.com/thumb.jpg\",\"tags\":[\"music\",\"live\"],\"language\":\"en\",\"venue\":{\"full_address\":\"123 Main St, Anytown, USA\",\"latitude\":0,\"longitude\":0,\"phone_number\":\"123-456-7890\"}}]}";

        // Setup the protected method SendAsync on the mock to always return the expected HttpResponseMessage
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseBody, Encoding.UTF8, "application/json"),
            })
            .Verifiable();

        // Use the handler to construct an HttpClient
        _httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://real-time-events-search.p.rapidapi.com/")
        };

        _mockLogger = new Mock<ILogger<RealTimeEventSearchService>>();
        _service = new RealTimeEventSearchService(_httpClient, _mockLogger.Object);
    }

    [Test]
    public async Task SearchEventAsync_ShouldReturnEvents_WhenApiResponseIsSuccessful()
    {
        // Act
        var result = await _service.SearchEventAsync("music", 0);

        // Convert IEnumerable to List for easier handling, or directly access the first item
        var resultList = result.ToList();
        Assert.IsNotNull(resultList);
        Assert.That(resultList, Is.TypeOf<List<EventDetail>>());
        Assert.That(resultList.Count, Is.EqualTo(1));

        // Corrected the indexing issue
        var eventDetail = resultList.First(); // Or resultList[0], now that it's a List
        Assert.That(eventDetail.EventID, Is.EqualTo("123"));
        Assert.That(eventDetail.EventName, Is.EqualTo("Test Event"));
        Assert.That(eventDetail.Full_Address, Is.EqualTo("123 Main St, Anytown, USA"));
    }

    [Test]
    public async Task SearchEventAsync_ShouldReturnEmptyAndLogError_WhenApiResponseIsBadRequest()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        var badRequestResponse = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.BadRequest,
            Content = new StringContent("", Encoding.UTF8, "application/json"),
        };

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(badRequestResponse)
            .Verifiable();

        _httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://real-time-events-search.p.rapidapi.com/")
        };

        _service = new RealTimeEventSearchService(_httpClient, _mockLogger.Object);

        // Act
        var result = await _service.SearchEventAsync("music", 0);
        var resultList = result.ToList();

        // Assert
        Assert.IsNotNull(resultList);
        Assert.IsEmpty(resultList);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error: BadRequest")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            Times.Once);
    }
    [Test]
    public async Task SearchEventAsync_ShouldReturnEmptyAndLogError_WhenApiResponseIsInternalServerError()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        var internalServerErrorResponse = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Content = new StringContent("Internal Server Error", Encoding.UTF8, "application/json"),
        };

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(internalServerErrorResponse)
            .Verifiable();

        _httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://real-time-events-search.p.rapidapi.com/")
        };

        _service = new RealTimeEventSearchService(_httpClient, _mockLogger.Object);

        // Act
        var result = await _service.SearchEventAsync("failure", 0);
        var resultList = result.ToList();

        // Assert
        Assert.IsNotNull(resultList);
        Assert.IsEmpty(resultList);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error: InternalServerError")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            Times.Once);
    }
}

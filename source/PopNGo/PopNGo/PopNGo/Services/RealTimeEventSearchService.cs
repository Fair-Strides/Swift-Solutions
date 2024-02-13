using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PopNGo.Models;
namespace PopNGo.Services
{
    class Parameter
    {
        public string query { get; set; }
        public int start { get; set; }
    }

    class TicketLink
    {
        public string source { get; set; }
        public string link { get; set; }
    }

    class InfoLink
    {
        public string source { get; set; }
        public string link { get; set; }
    }

    class Venue
    {
        public string google_id { get; set; }
        public string name { get; set; }
        public string phone_number { get; set; }
        public string website { get; set; }
        public int review_count { get; set; }
        public double rating { get; set; }
        public string subtype { get; set; }
        public List<string> subtypes { get; set; }
        public string full_address { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string street_number { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string timezone { get; set; }
        public string google_mid { get; set; }
    }

    class Data
    {
        public string event_id { get; set; }
        public string event_mid { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public bool is_virtual { get; set; }
        public string thumbnail { get; set; }
        public List<TicketLink> ticket_links { get; set; }
        public List<InfoLink> info_links { get; set; }
        public Venue venue { get; set; }
        public List<string> tags { get; set; }
        public string language { get; set; }
        public string link { get; set; } // Add this property
    }


    class AllEventDetail
    {
        public string status { get; set; }
        public string request_id { get; set; }
        public Parameter parameters { get; set; }
        public List<Data> data { get; set; }
    }



    public class RealTimeEventSearchService : IRealTimeEventSearchService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RealTimeEventSearchService> _logger;

        public RealTimeEventSearchService(HttpClient httpClient, ILogger<RealTimeEventSearchService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<EventDetail>> SearchEventAsync(string query)
        {
            string endpoint = $"search-events?query={query}&start=0";
            _logger.LogInformation($"Calling Real Time Search Event API at {endpoint}");

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
                string responseBody;

                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    HandleErrorResponse(response.StatusCode);
                    return Enumerable.Empty<EventDetail>();
                }

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                AllEventDetail allEventDetail = JsonSerializer.Deserialize<AllEventDetail>(responseBody, options);
                return allEventDetail.data
                .Select(data => new EventDetail
                {
                    EventName = data.name.ToString(),
                    EventLink = data.link,
                    EventDescription = data.description,
                    EventStartTime = DateTime.TryParse(data.start_time, out DateTime startTime) ? startTime : DateTime.MinValue,
                    EventEndTime = DateTime.TryParse(data.end_time, out DateTime endTime) ? endTime : DateTime.MinValue,
                    EventIsVirtual = data.is_virtual,
                    EventThumbnail = data.thumbnail,
                    EventLanguage = data.language,
                    Full_Address = data.venue.full_address,
                    Longitude = data.venue.longitude,
                    Latitude = data.venue.latitude,
                    Phone_Number = data.venue.phone_number

                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return Enumerable.Empty<EventDetail>();
            }
        }

        private void HandleErrorResponse(HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogError("Authentication failed. Check your API credentials.");
            }
            else if (statusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError("Network failure. Check your API URL.");
            }
            else
            {
                _logger.LogError($"Error: {statusCode}");
            }
        }
    }
}

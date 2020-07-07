using Airport.Models;
using AngleSharp.Html.Dom;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Airport.Services
{
    public class LutonScraper : AbstractScraper
    {
        private readonly ILogger _logger;

        public LutonScraper(IServiceScopeFactory scopeFactory, ILogger<LutonScraper> logger) : base(scopeFactory, logger)
        {
            _logger = logger;
        }

        public override string GetUrl()
        {
            return "https://www.london-luton.co.uk/flights";
        }

        public override string GetSelector()
        {
            return "div.flight-details.saveflightList.arrivals > div.flight-info";
        }

        public override List<Flight> Transform(IHtmlDocument document)
        {
            var elements = document.QuerySelectorAll("div.flight-details.saveflightList.arrivals");
            _logger.LogInformation("Arriving fligths size:" + elements.Count());
            var flights = elements.ToList().ConvertAll(e => 
            new Flight()
            {
                Airport = "Luton",
                FlightId = e.QuerySelector("span.flightNumber").LastChild.TextContent,
                Scheduled = e.QuerySelector("span.flightTime").LastChild.TextContent,
                ArrivingFrom = e.QuerySelector("span.flightOrgDest").LastChild.TextContent,
                Status = e.QuerySelector("span.flightStatus").LastChild != null ? e.QuerySelector("span.flightStatus").LastChild.TextContent : "N/A",
                Terminal = "N/A"
            });
            return flights;
        }
    }
}

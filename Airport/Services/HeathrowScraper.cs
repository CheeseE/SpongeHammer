using Airport.Models;
using AngleSharp.Html.Dom;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airport.Services
{
    public class HeathrowScraper : AbstractScraper
    {
        private readonly ILogger _logger;

        public HeathrowScraper(IServiceScopeFactory scopeFactory, ILogger<HeathrowScraper> logger) : base(scopeFactory, logger)
        {
            _logger = logger;
        }

        public override string GetUrl()
        {
            return "https://www.heathrow.com/arrivals";
        }

        public override string GetSelector()
        {
            return "div.airline-listing-table > a";
        }

        public override List<Flight> Transform(IHtmlDocument document)
        {
            var elements = document.QuerySelectorAll("div.airline-listing-table > a");
            _logger.LogInformation("Arriving fligths size:" + elements.Count());
            var flights = elements.ToList().ConvertAll(e => new Flight()
            {
                Airport = "Heathrow",
                FlightId = e.QuerySelector("div.col-flight").LastChild.TextContent,
                Scheduled = e.QuerySelector("div.col-scheduled").LastChild.TextContent,
                ArrivingFrom = e.QuerySelector("div.col-arriving-from").LastChild.TextContent,
                Status = e.QuerySelector("div.col-status.sm-hide").LastChild.TextContent,
                Terminal = e.QuerySelector("div.col-terminal").LastChild.TextContent
            });
            return flights;
        }
    }
}

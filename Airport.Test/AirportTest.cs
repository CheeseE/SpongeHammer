using Airport.Services;
using AngleSharp;
using AngleSharp.Html.Parser;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Airport.Test
{
    [TestClass]
    public class AirportTest
    {
        [TestMethod]
        public void Test_Luton_Parser_Return_Flight()
        {

            //Source to be parsed
            var source = "<div id=\"ctl00_cphMainContent_plcZones_lt_FlightInfoZone1_FlightListing_rpArrivals_ctl00_hypFlightDetails\" class=\"flight-details saveflightList arrivals\">" + 
                            "<div class=\"logo-flightNo\"> " +
                                "<span class=\"flightNumber\">W95126</span>"+
                            "</div>"+
                            "<div class=\"flight-info\">" +
                                "<span class=\"flightOrgDest\">Palma De Mallorca</span>"+
                                "<span class=\"flightTime\">11:10</span>"+
                                "<span class=\"flightStatus\">Bags On Belt 2</span>"+
                            "</div>"+
                        "</div>";
            HtmlParser parser = new HtmlParser();

            var scraper = new LutonScraper(null, Mock.Of<ILogger<LutonScraper>>());
            var document = parser.ParseDocument(source);
            var elements = scraper.Transform(document);
            Assert.AreEqual(1, elements.Count);
            Assert.AreEqual(elements[0].Airport, "Luton");
            Assert.AreEqual(elements[0].ArrivingFrom, "Palma De Mallorca");
            Assert.AreEqual(elements[0].FlightId, "W95126");
            Assert.AreEqual(elements[0].Scheduled, "11:10");
            Assert.AreEqual(elements[0].Status, "Bags On Belt 2");
            Assert.AreEqual(elements[0].Terminal, "N/A");

        }

        [TestMethod]
        public void Test_Heatrow_Parser_Return_Flight()
        {

            //Source to be parsed
            var source = "<div class=\"airline-listing-table\">" +
                                "<a class=\"airline-listing-line-item sm-px3 lg-px3 py4 md-py3\" tabindex=\"0\" role=\"button\" href=\"/arrivals/terminal-5/flight-details/BA1307/07-07-2020\">" +
                                    "<div class=\"col-scheduled\">12:45</div>" +
                                    "<div class=\"col-flight\"><img class=\"pr2 sm-hide tail-fin-img\" src=\"/content/dam/heathrow/web/common/images/airline/tailfin/BA-tailfin.jpg\" alt=\"Logo\">BA1307</div>" +
                                    "<div class=\"col-arriving-from\">Aberdeen</div>" +
                                    "<div class=\"col-airline sm-hide\">British Airways</div>" +
                                    "<div class=\"col-terminal\"><p class=\"my0 tags flex-inline flex-justify-center\">T5</p></div>" +
                                    "<div class=\"col-status sm-hide\"><p class=\"my0 status flex-inline flex-justify-center green\">Landed</p></div>" +
                                    "<div class=\"col-save\"><div id=\"save-flight-button-BA1307\" role=\"button\" tabindex=\"0\" class=\"save-flight-button-wrapper  icon-not-selected\" aria-label=\"save-flight-button\"><svg class=\"icon\" height=\"24px\" width=\"24px\" focusable=\"false\" tabindex=\"-1\" aria-hidden=\"true\" aria-disabled=\"true\" data-locator=\"add-flight_icon\"><use xlink:href=\"#add-flight\"></use></svg></div></div>" +
                                "</a>" +
                        "</div>";
            HtmlParser parser = new HtmlParser();

            var scraper = new HeathrowScraper(null, Mock.Of<ILogger<HeathrowScraper>>());
            var document = parser.ParseDocument(source);
            var elements = scraper.Transform(document);
            Assert.AreEqual(1, elements.Count);
            Assert.AreEqual(elements[0].Airport, "Heathrow");
            Assert.AreEqual(elements[0].ArrivingFrom, "Aberdeen");
            Assert.AreEqual(elements[0].FlightId, "BA1307");
            Assert.AreEqual(elements[0].Scheduled, "12:45");
            Assert.AreEqual(elements[0].Status, "Landed");
            Assert.AreEqual(elements[0].Terminal, "T5");

        }
    }
}

using Airport.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PuppeteerSharp;
using AngleSharp.Html.Parser;
using AngleSharp.Html.Dom;
using Microsoft.Extensions.Logging;

namespace Airport.Services
{
    public abstract class AbstractScraper : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;
        private readonly ILogger _logger;

        protected AbstractScraper(IServiceScopeFactory scopeFactory, ILogger<AbstractScraper> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        private async Task<IHtmlDocument> GetHtmlAsync(string url)
        {
            HtmlParser parser = new HtmlParser();
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions{Headless = true});
            {
                _logger.LogInformation("Fetching page from url: " + url);
                var page = await browser.NewPageAsync();
                await page.GoToAsync(url);
                await page.WaitForSelectorAsync(GetSelector());
                var content = await page.GetContentAsync();
                await page.CloseAsync();
                await browser.CloseAsync();


                IHtmlDocument document = parser.ParseDocument(content);
                return document;
            }
        }

        public async void SaveFlightsAsync(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<FlightContext>();
                _logger.LogInformation("Saving flights...");
                var flights = await GetFlightsAsync();
                flights.ForEach(f =>
                {
                    if (dbContext.FlightExists(f.FlightId))
                    {
                        dbContext.Update(f);
                    }
                    else
                    {
                        dbContext.Add(f);
                    }
                });
                dbContext.SaveChanges();
                _logger.LogInformation("Flights saved.");
            }
        }

        private async Task<List<Flight>> GetFlightsAsync()
        {
            _logger.LogInformation("Getting flights...");
            var result = await GetHtmlAsync(GetUrl());
            var flights = Transform(result);
            _logger.LogInformation("Flights received:" + flights.Count);
            _logger.LogInformation("Flights received:" + flights.Count);
            return flights;
        }

        public abstract List<Flight> Transform(IHtmlDocument document);

        public abstract String GetUrl();

        public abstract String GetSelector();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(SaveFlightsAsync, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(600));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }


    }
}

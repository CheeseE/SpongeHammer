using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Airport.Models
{
    public class FlightContext : DbContext
    {
        public FlightContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Flight> Flights { get; set; }

        public bool FlightExists(string id)
        {
            return this.Flights.Any(e => e.FlightId == id);
        }

    }
}

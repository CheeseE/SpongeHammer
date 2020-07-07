using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Airport.Models
{
    public class Flight
    {
        [Key]
        public string FlightId { get; set; }
        public string Scheduled { get; set; }
        public string ArrivingFrom { get; set; }
        public string Airport { get; set; }
        public string Status { get; set; }
        public string Terminal { get; set; }
    }
}

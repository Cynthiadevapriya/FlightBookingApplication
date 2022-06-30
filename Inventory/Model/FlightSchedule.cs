using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Model
{
    public class FlightSchedule
    {
       
        public int Id { get; set; }
        public int FlightNumber { get; set; }
        public int AirlineId { get; set; }
        [ForeignKey("AirlineId")]
        public Airline Airline { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Instrument { get; set; }
        public int NoOfBusinessSeats { get; set; }
        public int NoOfNonBusinessSeats { get; set; }
        public float TicketCost { get; set; }
        public string Meal { get; set; }
    }
}

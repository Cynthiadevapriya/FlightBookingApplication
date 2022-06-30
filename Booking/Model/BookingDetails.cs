using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Model
{
    public class BookingDetails
    {
        [Key]
        public int BookingId { get; set; }
        public int FlightNumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public long PNRNumber { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NoOfSeatsBooked { get; set; }
        //public int PassengerId { get; set; }
        //public virtual Passengers  passeger{ get; set; }
        public string Meal { get; set; }
        public string SeatNumbers { get; set; }
    }
}

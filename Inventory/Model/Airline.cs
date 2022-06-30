using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Model
{
    public class Airline
    {
        
        public int Id { get; set; }
        public string AirlineName { get; set; }
        public bool IsEnable { get; set; }
        //public int  FlightId{ get; set; }
        //[ForeignKey("FlightId")]
        //public FlightSchedule FlightSchedule { get; set; }
    }
}

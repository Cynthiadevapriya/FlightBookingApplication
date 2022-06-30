using Inventory.Model;
using Inventory.RabbitMQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        public InventoryController(AppDbContext context)
        {
            this._context = context;
        }
        // GET: api/<InventoryController>
        [HttpGet("flight/search")]
        public IEnumerable<FlightSchedule> GetSchedules()
        {
            return _context.FlightSchedule.ToList();
        }
        [HttpGet("flightsearch")]
        public async Task<ActionResult<FlightResponse>> Get(string source, string destination)
        {
            var bookingDetails = (from f in _context.FlightSchedule join a in _context.Airline 
                                  on f.AirlineId equals a.Id
                                  where f.From == source && f.To==destination && a.IsEnable
                                  select new {
                                      FlightNumber = f.FlightNumber,
                                      From=f.From,
                                      To=f.To,
                                      AirlineName = a.AirlineName,
                                      StartTime = f.StartTime,
                                      EndTime = f.EndTime,
                                      NoOfBusinessSeats = f.NoOfBusinessSeats,
                                      NoOfNonBusinessSeats = f.NoOfNonBusinessSeats,
                                      TicketCost = f.TicketCost
                                  }).ToList();
            if (bookingDetails != null)
            {
                List<FlightResponse> details = new List<FlightResponse>();
                if (bookingDetails != null)
                {
                    bookingDetails.ForEach(x => details.Add(new FlightResponse
                    {
                        FlightNumber = x.FlightNumber,
                        From=x.From,
                        To=x.To,
                        AirlineName = x.AirlineName,
                        StartTime = x.StartTime,
                        EndTime = x.EndTime,
                        NoOfBusinessSeats = x.NoOfBusinessSeats,
                        NoOfNonBusinessSeats = x.NoOfNonBusinessSeats,
                        TicketCost = x.TicketCost
                    }));
                }
                return Ok(details);
            }
            else
            {
                return Ok(new { message = "No flights found for the searched source and destination" });
            }           
        }

        // GET api/<InventoryController>/5
        [HttpGet("flight/{id}")]
        public FlightSchedule GetFlights(int id)
        {
            return _context.FlightSchedule.Find(id);
        }
        [HttpGet("Airline/search")]
        public IEnumerable<Airline> GetAirline()
        {
            return _context.Airline.ToList();
        }
        // POST api/<InventoryController>
        [HttpPost("airline/inventory/add")]
        public async Task<IActionResult> FlightSchedule([FromBody] FlightSchedule flight)
        {
            try
            {
                FlightSchedule flightObj = new FlightSchedule();
                flightObj.FlightNumber = flight.FlightNumber;
                flightObj.AirlineId = flight.AirlineId;
                flightObj.From = flight.From;
                flightObj.To = flight.To;
                flightObj.StartTime = flight.StartTime;
                flightObj.EndTime = flight.EndTime;
                flightObj.Instrument = flight.Instrument;
                flightObj.NoOfBusinessSeats = flight.NoOfBusinessSeats;
                flightObj.NoOfNonBusinessSeats = flight.NoOfNonBusinessSeats;
                flightObj.TicketCost = flight.TicketCost;
                flightObj.Meal = flight.Meal;
                //_context.Flight.Add(flightObj);
                _context.FlightSchedule.Add(flightObj);
                _context.SaveChanges();
                return Ok(flight);
            }
            catch (Exception ex)
            {
                return BadRequest("Couldnt add the flight");
            }
        }
        [HttpPut("updateschedule")]
        public async Task<IActionResult> UpdateSchedule([FromBody] FlightSchedule schedule)
        {
            try
            {
                FlightSchedule flightObj = new FlightSchedule();
                flightObj.Id = schedule.Id;
                flightObj.FlightNumber = schedule.FlightNumber;
                flightObj.AirlineId = schedule.AirlineId;
                flightObj.From = schedule.From;
                flightObj.To = schedule.To;
                flightObj.StartTime = schedule.StartTime;
                flightObj.EndTime = schedule.EndTime;
                flightObj.Instrument = schedule.Instrument;
                flightObj.NoOfBusinessSeats = schedule.NoOfBusinessSeats;
                flightObj.NoOfNonBusinessSeats = schedule.NoOfNonBusinessSeats;
                flightObj.TicketCost = schedule.TicketCost;
                flightObj.Meal = schedule.Meal;
                _context.FlightSchedule.Update(flightObj);
                _context.SaveChanges();
                return Ok(new { message = "Flight details updated succesfully" });
            }
            catch (Exception ex)
            {
                return BadRequest("Couldnt Update the flight details");
            }

        }
        [HttpPost("flight/airline/register")]
        public IActionResult RegisterAirline([FromBody] Airline airline)
        {
            try
            {
                var airlin = (from f in _context.Airline
                                      where f.AirlineName == airline.AirlineName
                                      select f).FirstOrDefault<Airline>();
                if (airlin == null)
                {
                    _context.Airline.Add(airline);
                    _context.SaveChanges();
                    return Ok(new { message = "Airline Registration is Successful" });
                }
                else
                    return Ok(new { message = "Airline already exists" });              
            }
            catch (Exception ex)
            {
                return Ok(new { message = "Something went wrong try again" });
            }
        }
        [HttpPut("name")]
        public async Task<IActionResult> BlockAirline(string name)
        {
            var airlineDetails = (from f in _context.Airline
                                  where f.AirlineName == name
                                  select f).FirstOrDefault<Airline>();
            if (airlineDetails == null)
            {
                return Ok(new { message = "Airline doesnt exists" });
            }
            if (airlineDetails.IsEnable==true)
            {
                airlineDetails.IsEnable = false;
                _context.SaveChanges();
                AirlineProducer.Producer("The airline " + airlineDetails.AirlineName+" has been blocked" );
                return Ok(new { message = "The  Airline has been Blocked" });
            }
            else 
                return Ok(new { message = "Airline is already blocked" });

        }
        [HttpPut("airlinename")]
        public async Task<IActionResult> UnBlockAirline(string airlinename)
        {
            var airlineDetails = (from f in _context.Airline
                                  where f.AirlineName == airlinename
                                  select f).FirstOrDefault<Airline>();
            if (airlineDetails.IsEnable == false)
            {
                airlineDetails.IsEnable = true;
                _context.SaveChanges();
                return Ok(new { message = "The  Airline has been unblocked" });
            }
            else
                return Ok(new { message = "Airline is already unblocked" });
        }

        // PUT api/<InventoryController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<InventoryController>/5
        //[HttpDelete("{name}")]
        //public void Delete(string name)
        //{

        //    var airlineDetails = (from f in _context.Airline
        //                          where f.AirlineName == name
        //                          select f).FirstOrDefault<Airline>();
        //    _context.Airline.Remove(airlineDetails);
        //    _context.SaveChanges();

        //}
    }
}

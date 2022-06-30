using Booking.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BookingController(AppDbContext context)
        {
            this._context = context;
        }
        // GET: api/<BookingController>
        [HttpGet("email")]
        public async Task<ActionResult<BookingDetailsResponse>> History(string email)
        {
            
            var bookingDetails = (from f in _context.BookingDetails
                                  where f.Email == email
                                  select f).ToList();
            List<BookingDetailsResponse> details = new List<BookingDetailsResponse>();
            if(bookingDetails != null)
            {
                bookingDetails.ForEach(x => details.Add(new BookingDetailsResponse
                {
                    BookingId = x.BookingId,
                    FlightNumber=x.FlightNumber,
                    PNRNumber = x.PNRNumber,
                    From = x.From,
                    To = x.To,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    NoOfSeatsBooked = x.NoOfSeatsBooked,
                    Meal = x.Meal,
                    SeatNumbers = x.SeatNumbers
                }));
            }
            return Ok(details);
        }
        // GET api/<BookingController>/5
        [HttpGet("pnr")]
        public BookingDetails GetDetails(long pnr)
        {
            var bookingDetails = (from f in _context.BookingDetails
                                  where f.PNRNumber == pnr
                                  select f).FirstOrDefault<BookingDetails>();
            return bookingDetails;
        }

        // POST api/<BookingController>
        [HttpPost("bookingdetails")]
        public async Task<IActionResult> Post([FromBody] BookingDetails bookingDetails)
        {
            var random = new Random();
            var value = random.Next();
            try
            {
                BookingDetails bookingdts = new BookingDetails();
                bookingdts.UserName = bookingDetails.UserName;
                bookingdts.FlightNumber = bookingDetails.FlightNumber;
                bookingdts.Email = bookingDetails.Email;
                bookingdts.PNRNumber = value;
                bookingdts.From = bookingDetails.From;
                bookingdts.To = bookingDetails.To;
                bookingdts.StartTime = bookingDetails.StartTime;
                bookingdts.EndTime = bookingDetails.EndTime;
                bookingdts.NoOfSeatsBooked = bookingDetails.NoOfSeatsBooked;
                bookingdts.Meal = bookingDetails.Meal;
                bookingdts.SeatNumbers = bookingDetails.SeatNumbers;
                _context.BookingDetails.Add(bookingdts);
                _context.SaveChanges();
                return Ok(new { message = "Booking is successful" });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "Booking is not done, Please try again" });
            }
        }

        // PUT api/<BookingController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<BookingController>/217539183
        [HttpDelete("cpnr")] //Route param
        public IActionResult Delete(long pnr)
        {
            try
            {
                var bookingDetails = (from f in _context.BookingDetails
                              where f.PNRNumber == pnr
                              select f).FirstOrDefault();
                //if(DateTime.Now-bookingDetails.StartTime)
                _context.BookingDetails.Remove(bookingDetails);
                _context.SaveChanges();
                return Ok(new { message = "Ticket Cancellation is Successful" });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "Ticket Cancellation is not done, Please try again" });
            }
        }
    }
}

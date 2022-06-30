using Booking.Model;
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
    public class CouponController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CouponController(AppDbContext context)
        {
            this._context = context;
        }
        // GET: api/<CouponController>
        [HttpGet("getallcoupons")]
        public IEnumerable<Coupon> Get()
        {
            return _context.Coupon.ToList();
        }

        // GET api/<CouponController>/5
        [HttpGet("code")]
        public Coupon Get(string code)
        {
            return _context.Coupon.Find(code);
        }

        // POST api/<CouponController>
        [HttpPost("addcoupon")]
        public IActionResult Post([FromBody] Coupon coupon)
        {
            try
            {
                Coupon couponObj = new Coupon();
                couponObj.Code = coupon.Code;
                couponObj.Amount = coupon.Amount;
                _context.Coupon.Add(couponObj);
                _context.SaveChanges();
                return Ok(new { message = "Coupon Added Sucessfully" });
            }
            catch(Exception ex)
            {
                return Ok(new { message = "Something Went Wrong ,Please try again" });
            }
        }

        // PUT api/<CouponController>/5
        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Coupon coupon)
        {
            try
            {
                Coupon couponObj = new Coupon();
                couponObj.Code = coupon.Code;
                couponObj.Amount = coupon.Amount;
                _context.Coupon.Add(couponObj);
                _context.SaveChanges();
                return Ok(new { message = "Coupon Updated Sucessfully" });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "Something Went Wrong ,Please try again" });
            }
        }

        // DELETE api/<CouponController>/5
        [HttpDelete("deletecoupon")]
        public IActionResult Delete(string code)
        {
            try
            {
                var couponDetails = (from f in _context.Coupon
                                      where f.Code == code
                                      select f).FirstOrDefault();
                //if(DateTime.Now-bookingDetails.StartTime)
                _context.Coupon.Remove(couponDetails);
                _context.SaveChanges();
                return Ok(new { message = "coupon deleted  Successfully" });
            }
            catch(Exception ex)
            {
                return Ok(new { message = "Something Went Wrong ,Please try again" });
            }
        }
    }
}

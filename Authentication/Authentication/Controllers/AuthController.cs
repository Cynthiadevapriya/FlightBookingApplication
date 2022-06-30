using Authentication.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        public static User user = new User();
        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        
        [HttpPost("Register")]
        public async Task<ActionResult> Register(User request)
        {
            try
            {
                var login = _context.User.Where(x => (string.Equals(x.Email, request.Email) && string.Equals(x.IsAdmin, false))).FirstOrDefault();
                if (login != null)
                    return Ok(new { message = "User already Exists" });
                else
                {
                    //CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                    User usr = new User()
                    {
                        Email = request.Email,
                        Username = request.Username,
                        Password = request.Password,
                        Age = request.Age,
                        Gender = request.Gender,
                        IsAdmin = false
                    };
                    _context.Add(usr);
                    _context.SaveChanges();
                    return Ok(new { message = "User Registration succesful" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { message = "Something went wrong please try again" });
            }
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        [HttpPost("Login")]
        public  async Task<ActionResult> Login(User request)
        {
            try
            {
                var login = _context.User.Where(x => (string.Equals(x.Email, request.Email) && string.Equals(x.Password, request.Password))).FirstOrDefault();

                if (login != null)
                {
                    string tokenCreated = CreateToken(login);
                    var refreshToken = GenerateRefreshToken();
                        //SetRefreshToken(refreshToken);
                    User usr = new User()
                    {                      
                        Id=login.Id,
                        Email = login.Email,
                        Username=login.Username,
                        IsAdmin = login.IsAdmin
                    };
                    return Ok(new { token = tokenCreated,user=usr });
                }
                else
                {
                    return Ok(new { message = "User is not registered, Register and login again" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Something went wrong, please try again" });
            }
        }

        //[HttpPost("RefreshToken")]
        //public async Task<ActionResult<string>> RefreshToken()
        //{
        //    var refreshToken = Request.Cookies["refreshToken"];
        //    if (!user.RefreshToken.Equals(refreshToken))
        //    {
        //        return Unauthorized("Ivalid refresh token");
        //    }
        //    else if (user.TokenExpires < DateTime.Now)
        //        return Unauthorized("Token Expired");
        //    string token = CreateToken(user);
        //    var newRefreshToken = GenerateRefreshToken();
        //    SetRefreshToken(newRefreshToken);

        //    return Ok(token);
        //}
        //[HttpPost("RefreshToken")]
        //public async Task<ActionResult<string>> RefreshToken(User request)
        //{
        //    var login = _context.User.Where(x => (string.Equals(x.Email, request.Email) && string.Equals(x.IsAdmin, request.IsAdmin))).FirstOrDefault();

        //    if (login != null)
        //    {
        //        var refreshToken = Request.Cookies["refreshToken"];
        //        if (!login.RefreshToken.Equals(refreshToken))
        //        {
        //            return Unauthorized("Ivalid refresh token");
        //        }
        //        else if (login.TokenExpires < DateTime.Now)
        //            return Unauthorized("Token Expired");
        //        string token = CreateToken(login);
        //        var newRefreshToken = GenerateRefreshToken();
        //        SetRefreshToken(newRefreshToken);
        //    }
        //    else
        //        return BadRequest("login again and try");


        //}

        //private void SetRefreshToken(RefreshToken newRefreshToken)
        //{
        //    var cookieOptions = new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Expires = newRefreshToken.Expires
        //    };
        //    Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        //    user.RefreshToken = newRefreshToken.Token;
        //    user.TokenCreated = newRefreshToken.Created;
        //    user.TokenExpires = newRefreshToken.Expires;
        //}


        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                //Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Token = Convert.ToBase64String(BitConverter.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };
            return refreshToken;
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.Username)
            };
            if (user.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                claims.Add(new Claim("Role", "Admin"));
            }
            else
                claims.Add(new Claim(ClaimTypes.Role, "User"));
                claims.Add(new Claim("Role", "User"));

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using api.Models;
using Api.Dtos;
using Api.Utilities;
using System.Linq;
using api.Enums;
using Microsoft.AspNetCore.Cors;

namespace api.Controllers
{
    [Route("api/[controller]")]
    public partial class AccountController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _db;

        public AccountController(IConfiguration config, ApplicationDbContext db)
        {
            _config = config;
            _db = db;
        }


        [HttpGet, Authorize]
        [Route("IsTokenValid")]
        public IActionResult IsTokenValid()
        {

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [EnableCors("MyPolicy")]
        [Route("CreateToken")]
        public IActionResult CreateToken([FromBody]LoginDto login)
        {
            try
            {
                IActionResult response = Unauthorized();

                if (!ModelState.IsValid){return response;}
                                 

                var isExists = IsUserExists(login); // Admins only

                if (isExists)
                {
                    var tokenString = BuildToken();
                    response = Ok(new { token = tokenString });
                }

                return response;
            }
            catch (Exception ex)
            {

                return Json(new { status = "error", message = ex.ToString() });
            }
        }


        private string BuildToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private bool IsUserExists(LoginDto login)
        {
            var password = Encryption.Sha256(login.Password);

            var email = (
                        from u in _db.Users
                        join uir in _db.UserInRoles
                               on u.UserId equals uir.UserId
                        where
                               u.Password == password &&
                               u.Email == login.Email &&
                               uir.RoleId == (int)UserRoles.Administrator
                        select u.Email.FirstOrDefault()
                        );

            if (email.Any())
            {
                return true;
            }

            return false;
        }


    }
}
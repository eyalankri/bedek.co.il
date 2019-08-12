using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Enums;
using api.Models;
using Api.Dtos;
using Api.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Utilities;

namespace api.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HandymanController : ControllerBase
    {

        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;


        public HandymanController(IMapper mapper, ApplicationDbContext db)
        {
            _mapper = mapper;
            _db = db;
        }



        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] UserDto userDto)
        {


            var user = _mapper.Map<User>(userDto);
            var psw = RandomKey.GeneratePassword(5, 2, 2);
            user.Password = Encryption.Sha256(psw);

            if (!ModelState.IsValid) return BadRequest();

            var userId = Guid.NewGuid();
            user.UserId = userId;

            var userInRole = new UserInRole
            {
                RoleId = (int)UserRoles.Handyman,
                UserId = userId
            };

            _db.Add(user);
            _db.Add(userInRole);
            _db.SaveChanges();
            return Ok(user);

            //var body = "your password is: <strong>" + psw + "</strong>";

           // SendEmail.SendEmailWithGmail(body, handyman.Email, "החשבון שלך במערכת בדק", true);


          
        }

        [HttpGet]
        [Route("List")]
        [EnableCors("MyPolicy")]
        public IQueryable<User> List()
        {
            var list = (from u in _db.Users
                join ur in _db.UserInRoles
                    on u.UserId equals ur.UserId
                where (ur.RoleId == (int) UserRoles.Handyman)
                select new User()
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    IdentityCardId = u.IdentityCardId,
                    Phone1 = u.Phone1,
                    Phone2 = u.Phone2,
                    Password = null,
                    Company = u.Company
                });

            return list;
                //


        }


        [HttpGet]
        [Route("Get")]
        [EnableCors("MyPolicy")]
        public async Task<User> Get(Guid handymanId)
        {

            var handyman = await (from u in _db.Users
                                  where u.UserId == handymanId
                                  select new User
                                  {
                                      FirstName = u.FirstName,                                      
                                      LastName = u.LastName,
                                      IdentityCardId = u.IdentityCardId,
                                      Email = u.Email,
                                      Phone1 = u.Phone1,
                                      Phone2= u.Phone2,
                                      Company = u.Company
                                  }).FirstOrDefaultAsync();

            return handyman;
        }

        [HttpPost]
        [Route("Update")]
        [EnableCors("MyPolicy")]
        public IActionResult Update([FromBody] UserDto handymanDto)
        {
            var user = _mapper.Map<UserDto>(handymanDto);
            if (!ModelState.IsValid) return BadRequest();


            var entity = _db.Users.Find(user.UserId);

            if (entity == null)
            {
                return BadRequest();
            }
            _db.Entry(entity).CurrentValues.SetValues(handymanDto);

            _db.SaveChanges();
            return Ok(user);
        }



    }
}
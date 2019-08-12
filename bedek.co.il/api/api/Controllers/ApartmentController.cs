using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Enums;
using api.Models;
using Api.Dtos;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ApartmentController(IMapper mapper, ApplicationDbContext db)
        {
            _mapper = mapper;
            _db = db;
        }




        [HttpGet]
        [Route("Get")]
        [EnableCors("MyPolicy")]
        public ApartmentDto Get(int apartmentId)
        {
            var userDto = (from u in _db.Users
                           join ap in _db.Apartments
                            on u.UserId equals ap.User.UserId
                           where ap.ApartmentId == apartmentId
                           select new UserDto()
                           {
                               UserId = u.UserId,
                               FirstName = u.FirstName,
                               LastName = u.LastName,
                               Email = u.Email,
                               IdentityCardId = u.IdentityCardId,
                               Phone1 = u.Phone1,
                               Phone2 = u.Phone2,
                               Password = null

                           }).First();


            var building = (from ap in _db.Apartments
                            join b in _db.Buildings
                                on ap.BuildingId equals b.BuildingId
                            where (ap.ApartmentId == apartmentId)
                            select new ApartmentDto()
                            {
                                BuildingId = b.BuildingId,
                                ProjectName = b.ProjectName,
                                City = b.City,
                                Street = b.Street,
                                BuildingNumber = b.BuildingNumber,
                                ApartmentNumber = ap.ApartmentNumber,
                                DateOfEntrance = DateTime.Parse(ap.DateOfEntrance.ToString("d")),
                                Comment=ap.Comment,
                                User = userDto
                            }).First();

            return building;
        }


        [HttpPost]
        [Route("Add")]
        [EnableCors("MyPolicy")]
        public IActionResult Add([FromBody] ApartmentDto apartmentDto)
        {
            var apartment = _mapper.Map<Apartment>(apartmentDto);

            var userId = Guid.NewGuid();
            apartment.User.UserId = userId;

            //password using constructor

            if (!ModelState.IsValid) return BadRequest();
                       
            var userInRole = new UserInRole
            {
                RoleId = (int)UserRoles.Resident,
                UserId = userId
            };

            _db.Add(apartment);
            _db.Add(userInRole);
            _db.SaveChanges();
            return Ok(apartment);
        }


        [HttpPost]
        [Route("Update")]
        public IActionResult Update([FromBody] ApartmentDto apartmentDto)
        {

            var newAppartment = _mapper.Map<Apartment>(apartmentDto);
            var newUser = _mapper.Map<User>(apartmentDto.User);

            if (!ModelState.IsValid) return BadRequest();

            var entityAp = _db.Apartments.Find(newAppartment.ApartmentId);
            var entityUs = _db.Users.Find(newAppartment.User.UserId);

            newAppartment.BuildingId = entityAp.BuildingId;


            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (entityAp == null || entityUs == null)
            {
                return BadRequest();
            }

            _db.Entry(entityAp).CurrentValues.SetValues(newAppartment);
            _db.Entry(entityUs).CurrentValues.SetValues(newUser);


            _db.SaveChanges();

            return Ok(newAppartment);

        }





        [HttpGet]
        [Route("List")]
        public IEnumerable<ApartmentDto> List(int buildingId)
        {

            //var entity = _mapper.Map<Apartment>(apartmentDto);

            var list = from ap in _db.Apartments
                       orderby ap.ApartmentId
                       where
                           ap.BuildingId == buildingId
                       select new ApartmentDto
                       {
                           ApartmentId = ap.ApartmentId,
                           ApartmentNumber = ap.ApartmentNumber,
                           ApartmentDocs = ap.ApartmentDocs,
                           DateOfEntrance = DateTime.Parse(ap.DateOfEntrance.ToString("d")),
                           User = _mapper.Map<UserDto>(ap.User)
                       };


            return list;
        }


    }
}
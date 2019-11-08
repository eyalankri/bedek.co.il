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
using System.Collections.Generic;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceCallController : Controller
    {

        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _db;
        public ServiceCallController(IConfiguration config, ApplicationDbContext db)
        {
            _config = config;
            _db = db;
        }



        [HttpGet]
        [Route("List")]
       
        public IQueryable List()
        {

            var list = from sc in _db.ServiceCall.OrderByDescending(x => x.DateUpdated)
                       join a in _db.Apartments
                           on sc.ApartmentId equals a.ApartmentId
                       join b in _db.Buildings
                           on a.BuildingId equals b.BuildingId
                       join u in _db.Users
                           on a.User.UserId equals u.UserId
                       select new ServiceCallDto
                       {
                           ServiceCallId = sc.ServiceCallId,
                           DateCreated = sc.DateCreated,
                           DateUpdated = sc.DateUpdated,
                           Status = sc.Status,//sc.Status,
                           ApartmentNumber = a.ApartmentNumber,
                           BuildingNumber = b.BuildingNumber,
                           ApartmentId = a.ApartmentId,
                           City = b.City,
                           Description = sc.Description,
                           FirstName = u.FirstName,
                           LastName = u.LastName
                       };

            return list;
        }




    }

}
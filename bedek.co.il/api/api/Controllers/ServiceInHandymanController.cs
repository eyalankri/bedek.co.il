﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServiceInHandymanController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public ServiceInHandymanController(IMapper mapper, ApplicationDbContext db, IConfiguration configuration)
        {
            _mapper = mapper;
            _db = db;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("Get")]
        [EnableCors("MyPolicy")]
        public async Task<User> Get(Guid handymanId)
        {

            var handyman = await (from
                                      u in _db.Users
                                  join
                                  uis in _db.ServiceInUser
                                    on u.UserId equals uis.UserId into t
                                  from uis in t.DefaultIfEmpty()
                                  where u.UserId == handymanId
                                  select new User
                                  {
                                      FirstName = u.FirstName,
                                      LastName = u.LastName,
                                      IdentityCardId = u.IdentityCardId,
                                      Email = u.Email,
                                      Phone1 = u.Phone1,
                                      Phone2 = u.Phone2
                                  }).FirstOrDefaultAsync();

            return handyman;
        }

        [HttpGet]
        [Route("List")]
        public List<ServiceInHandymanDto> List(Guid handymanId)
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection");

            var list = new List<ServiceInHandymanDto>();

            using (var conn = new SqlConnection(connStr))
            {
                using (var cmd = new SqlCommand("[ServiceInUser_Select_UserId]", conn))
                {
                    conn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", handymanId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            list.Add(new ServiceInHandymanDto()
                            {
                                ServiceId = (int)reader["ServiceId"],
                                ServiceName = (string)reader["ServiceName"],
                                WarrantyPeriodInMonths = (int)reader["WarrantyPeriodInMonths"],
                                UserId = reader["UserId"].ToString() == "" ? null : (Guid?)reader["UserId"]
                            });

                        }
                    }
                }
            }




            return list;

        }



        [HttpPost]
        [Route("Update")]
        [EnableCors("MyPolicy")]
        public IActionResult Update([FromBody] ServiceInHandymanDto[] listDto)
        {

            try
            {
                if (!ModelState.IsValid) return BadRequest();

                _db.ServiceInUser.RemoveRange(_db.ServiceInUser.Where(x => x.UserId == listDto.FirstOrDefault().UserId));
                _db.SaveChanges();

                if (! listDto.FirstOrDefault().RemoveAll) // all the services removed from user. don't insert
                {
                    foreach (var dto in listDto)
                    {
                        var entity = _mapper.Map<ServiceInHandyman>(dto);
                        _db.Add(entity);
                        _db.SaveChanges();


                    }
                }
                

            }
            catch (Exception ex)
            {

                var x = ex.ToString();
                throw;
            }
            
             

            return Ok();
        }
    }
}

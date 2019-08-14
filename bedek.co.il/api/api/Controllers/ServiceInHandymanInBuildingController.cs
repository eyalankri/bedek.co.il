using System;
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
   
    public class ServiceInHandymanInBuildingController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ServiceInHandymanInBuildingController(ApplicationDbContext db, IMapper mapper, IConfiguration configuration)
        {
            _db = db;
            _mapper = mapper;
            _configuration = configuration;
        }



        [HttpGet]
        [Route("List")]
        [EnableCors("MyPolicy")]
        public List<ServiceInHandymanInBuildingDto> List(int buildingId)
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection");

            var list = new List<ServiceInHandymanInBuildingDto>();

            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    using (var cmd = new SqlCommand("[ServiceInHandymanInBuilding_Select_BuildingId]", conn))
                    {
                        conn.Open();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BuildingId", buildingId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                 
                                list.Add(new ServiceInHandymanInBuildingDto()
                                {
                                    UserId = (Guid)reader["UserId"],
                                    FirstName = (string)reader["FirstName"],
                                    LastName = (string)reader["LastName"],
                                    Company = reader["Company"] != DBNull.Value ? (string)reader["Company"] : "",
                                    ServiceName = (string)reader["ServiceName"],
                                    ServiceId = (int)reader["ServiceId"],                                    
                                    BuildingId = buildingId,
                                    IsAssociated = reader["BuildingId"] != DBNull.Value

                                    //WarrantyPeriodInMonths = (int)reader["WarrantyPeriodInMonths"],
                                    //UserId = reader["UserId"].ToString() == "" ? null : (Guid?)reader["UserId"]
                                });

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var x = ex.ToString();
                throw;
            }
      
            return list;

        }



        [HttpPost]
        [Route("Update")]
        [EnableCors("MyPolicy")]
        public IActionResult Update([FromBody] ServiceInHandymanInBuildingDto[] listDto )
        {

            if (!ModelState.IsValid) return BadRequest();

          


            foreach (var dto in listDto)
            {

                _db.ServiceInHandymanInBuilding.RemoveRange(_db.ServiceInHandymanInBuilding.Where(x => x.BuildingId == dto.BuildingId && x.ServiceId==dto.ServiceId && x.UserId==dto.UserId));
                _db.SaveChanges();

                var entity = _mapper.Map<ServiceInHandymanInBuilding>(dto);
                _db.Add(entity);
                _db.SaveChanges();
               

            }

            return Ok();

             

        }
    }
}
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
                                    BuildingId = reader["BuildingId"] != DBNull.Value ? (int?)reader["BuildingId"] : null,

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
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
namespace api.Controllers
{
    public class ServiceInHandymanInBuildingInServiceCallController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ServiceInHandymanInBuildingInServiceCallController(ApplicationDbContext db, IMapper mapper, IConfiguration configuration)
        {
            _db = db;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("List")]
        [EnableCors("MyPolicy")]
        public List<ServiceInHandymanInBuildingInServiceCallDto> List(int buildingId)
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection");

            var list = new List<ServiceInHandymanInBuildingInServiceCallDto>();

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

                                list.Add(new ServiceInHandymanInBuildingInServiceCallDto()
                                {
                                    UserId = (Guid)reader["UserId"],
                                    FirstName = (string)reader["FirstName"],
                                    LastName = (string)reader["LastName"],
                                    Company = reader["Company"] != DBNull.Value ? (string)reader["Company"] : "",
                                    ServiceName = (string)reader["ServiceName"],
                                    ServiceId = (int)reader["ServiceId"],
                                    BuildingId = (int)reader["BuildingId"],
                                    ServiceCallId =  reader["ServiceCallId"] == DBNull.Value ? null : (int?)reader["ServiceCallId"],
                                    IsAssociated = reader["ServiceCallId"] == DBNull.Value ? false : true,

                                    
                                }); ;

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
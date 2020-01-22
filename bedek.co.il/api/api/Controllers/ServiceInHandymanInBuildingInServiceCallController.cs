using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Enums;
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
    //[ApiController]
    //[Authorize]

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




        

        /// <summary>
        /// The list of Handymans in the service call page, where you can select to which handyman to attach this call
        /// </summary>

        [HttpGet]
        [Route("List")]
        [EnableCors("MyPolicy")]
        public List<ServiceInHandymanInBuildingInServiceCallDto> List(int apartmentId, Guid? serviceCallId)
        {
            //Guid? serviceCallId;
            var connStr = _configuration.GetConnectionString("DefaultConnection");

            var list = new List<ServiceInHandymanInBuildingInServiceCallDto>();
            DateTime today = DateTime.Today;
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    using (var cmd = new SqlCommand("[ServiceInHandymanInBuildingInServiceCall_Select_ApartmentId-ServiceCallId]", conn))
                    {
                        conn.Open();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ApartmentId", apartmentId);
                        cmd.Parameters.AddWithValue("@ServiceCallId", serviceCallId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                list.Add(new ServiceInHandymanInBuildingInServiceCallDto()
                                {
                                    ServiceInHandymanInBuildingId = (int)reader["ServiceInHandymanInBuildingId"],
                                    UserId = (Guid)reader["UserId"],
                                    FirstName = (string)reader["FirstName"],
                                    LastName = (string)reader["LastName"],
                                    Company = reader["Company"] != DBNull.Value ? (string)reader["Company"] : "",
                                    ServiceName = (string)reader["ServiceName"],
                                    ServiceId = (int)reader["ServiceId"],
                                    BuildingId = (int)reader["BuildingId"],
                                    ApartmentId = reader["ApartmentId"] == DBNull.Value ? null : (int?)reader["ApartmentId"],
                                    IsAssociated = reader["ApartmentId"] == DBNull.Value ? false : true,
                                    WarrantyPeriodInYears = (int)reader["WarrantyPeriodInMonths"] / 12,
                                    DateOfEntrance = (DateTime)reader["DateOfEntrance"],
                                    IsWarrantyExpired = (today - (DateTime)reader["DateOfEntrance"]).TotalDays / 365 > ((int)reader["WarrantyPeriodInMonths"] / 12) ? true : false,
                                    WarrantyDaysElpased = today.Subtract((DateTime)reader["DateOfEntrance"]).TotalDays - (((int)reader["WarrantyPeriodInMonths"] / 12 * 365))
                                    //WarrantyPeriodInMonths = (int)reader["WarrantyPeriodInMonths"],
                                    //UserId = reader["UserId"].ToString() == "" ? null : (Guid?)reader["UserId"]
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


        [HttpPost]
        [Route("Add")]
        [EnableCors("MyPolicy")]
        public IActionResult Add([FromBody] ServiceCallDto serviceCallDto)
        {
            serviceCallDto.Status = ServiceCallStatus.New.ToString();

            try
            {

                var entity = _mapper.Map<ServiceCall>(serviceCallDto);

                

                //password using constructor

                if (!ModelState.IsValid) return BadRequest();

                _db.Add(entity);
                _db.SaveChanges();

               
                var serviceCallId = entity.ServiceCallId;


                entity = _db.ServiceCall.Find(serviceCallId);

                //var listServiceInHandymanInBuildingInServiceCall = new List<ServiceInHandymanInBuildingInServiceCall>();
                foreach (var serviceInHandymanInBuildingId in serviceCallDto.ArrServiceInHandymanInBuildingId)
                {
                    var s = new ServiceInHandymanInBuildingInServiceCall
                    {
                        ServiceCallId = serviceCallId,
                        ServiceInHandymanInBuildingId = serviceInHandymanInBuildingId,
                    };

                    //listServiceInHandymanInBuildingInServiceCall.Add(s);
                    entity.ServiceInHandymanInBuildingInServiceCall.Add(s);


                }
                //entity.ServiceInHandymanInBuildingInServiceCall.Add(listServiceInHandymanInBuildingInServiceCall);

                //   _db.Entry(entity).CurrentValues.SetValues(service);


                _db.SaveChanges();
                return Ok(entity);


                //if (!ModelState.IsValid) return BadRequest();

                //_db.ServiceInUser.RemoveRange(_db.ServiceInUser.Where(x => x.UserId == listDto.FirstOrDefault().UserId));
                //_db.SaveChanges();

                //if (!listDto.FirstOrDefault().RemoveAll) // all the services removed from user. don't insert
                //{
                //    foreach (var dto in listDto)
                //    {
                //        var entity = _mapper.Map<ServiceInHandyman>(dto);
                //        _db.Add(entity);
                //        _db.SaveChanges();


                //    }
                //}


            }
            catch (Exception ex)
            {

                var x = ex.ToString();
                throw;
            }

        }
    }
}

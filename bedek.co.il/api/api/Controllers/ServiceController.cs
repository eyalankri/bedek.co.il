using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]


    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ServiceController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] ServiceDto dto)
        {
            
            var model = _mapper.Map<Service>(dto);
            if (!ModelState.IsValid) return BadRequest();

            model.WarrantyPeriodInMonths = dto.WarrantyPeriodInMonths;

            _db.Add(model);
            _db.SaveChanges();
            return Ok(model);
        }


        [HttpPost]
        [Route("Update")]
        public IActionResult Update([FromBody] ServiceDto dto)
        {

            var service = _mapper.Map<Service>(dto);
           
            if (!ModelState.IsValid) return BadRequest();

            var entity = _db.Service.Find(service.ServiceId);

           
            if (entity == null)
            {
                return BadRequest();
            }

            service.ServiceId = entity.ServiceId;
            
            _db.Entry(entity).CurrentValues.SetValues(service);
            _db.SaveChanges();

            return Ok(service);

        }




        [HttpGet]
        [Route("Get")]         
        public async Task<ServiceDto> Get(int serviceId)
        {

            var service = await (from s in _db.Service
                                  where s.ServiceId == serviceId
                                  select new ServiceDto
                                  {
                                      ServiceName = s.ServiceName,
                                      ServiceId = s.ServiceId,
                                      IsEnable = s.IsEnable,
                                      WarrantyPeriodInMonths = s.WarrantyPeriodInMonths,
                                      
                                  }).FirstOrDefaultAsync();

            return service;
        }



        [HttpGet]
        [Route("List")]

        public IQueryable List()
        {

            //var entity = _mapper.Map<Apartment>(apartmentDto);

            var list = from s in _db.Service.OrderByDescending(x => x.ServiceId)
                       select new ServiceDto
                       {
                           ServiceId = s.ServiceId,
                           ServiceName = s.ServiceName,
                           WarrantyPeriodInMonths = s.WarrantyPeriodInMonths,
                           IsEnable = s.IsEnable,
                       };


            return list;
        }


    }
}

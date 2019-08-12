using System.Collections.Generic;
using api.Dtos;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;


namespace api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class BuildingController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public BuildingController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }



        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] BuildingDto buildingDto)
        {
            var building = _mapper.Map<Building>(buildingDto);
            if (!ModelState.IsValid) return BadRequest();

            _db.Add(building);
            _db.SaveChanges();
            return Ok(building);
        }


        [HttpPost]
        [Route("Update")]
        public IActionResult Update([FromBody] BuildingDto buildingDto)
        {
            var building = _mapper.Map<Building>(buildingDto);
            if (!ModelState.IsValid) return BadRequest();


            var entity = _db.Buildings.Find(building.BuildingId);

            if (entity ==null)
            {
                return BadRequest();
            }
            _db.Entry(entity).CurrentValues.SetValues(buildingDto);
           
            _db.SaveChanges();
            return Ok(building);
        }


        [HttpGet]
        [Route("Get")]
        [EnableCors("MyPolicy")]
        public async Task<BuildingDto> Get(int buildingId)
        {

            var building = await (from b in _db.Buildings
                where b.BuildingId == buildingId
                                  select new BuildingDto
                {
                    Apartments = b.Apartments,
                    BuildingId = b.BuildingId,
                    City = b.City,
                    BuildingNumber = b.BuildingNumber,
                    ProjectName = b.ProjectName,
                    Street = b.Street
                }).FirstOrDefaultAsync(); 

            return building;
        }



        [HttpGet]
        [Route("List")]
        public IQueryable<BuildingDto> List()
        {

            var list = from b in _db.Buildings.OrderByDescending(x => x.BuildingId)
                       select new BuildingDto
                       {                           
                           Apartments = b.Apartments,
                           BuildingId = b.BuildingId,
                           City = b.City,
                           BuildingNumber = b.BuildingNumber,
                           ProjectName = b.ProjectName,
                           Street = b.Street
                       };

            return list;
        }



    }
}
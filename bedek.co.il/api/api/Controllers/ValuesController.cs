using System.Collections.Generic;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;


namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ValuesController(IMapper mapper, ApplicationDbContext db)
        {
            _mapper = mapper;
            _db = db;
        }


        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        //[Route("GetAllAsync1")]
        //public async Task<List<BuildingDto>> GetAllAsync()
        //{
        //    using (_db)
        //    {
        //        var list = await (from b in _db.Buildings.OrderByDescending(x => x.BuildingId)
        //            select new BuildingDto
        //            {
        //                Apartments = b.Apartments,
        //                BuildingId = b.BuildingId,
        //                City = b.City,
        //                Number = b.Number,
        //                ProjectName = b.ProjectName,
        //                Street = b.Street
        //            }).ToListAsync();

        //        return  list;
        //    }
        //}


        //[HttpGet]
        //[Route("Get1")]
        //public async Task<List<BuildingDto>> Get1(int buildingId)
        //{

        //    var list = await (from b in _db.Buildings.OrderByDescending(x => x.BuildingId)
        //        select new BuildingDto
        //        {
        //            Apartments = b.Apartments,
        //            BuildingId = b.BuildingId,
        //            City = b.City,
        //            Number = b.Number,
        //            ProjectName = b.ProjectName,
        //            Street = b.Street
        //        }).ToListAsync(); ;

        //    return list;
        //}






    }
}

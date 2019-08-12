using api.Models;
using Api.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [EnableCors("MyPolicy")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;


        public UserController(IMapper mapper, ApplicationDbContext db)
        {
            _mapper = mapper;
            _db = db;
        }



        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] UserDto userDto)
        {
            var entity = _mapper.Map<UserDto>(userDto);
            //password using constructor

            if (!ModelState.IsValid) return BadRequest();

            _db.Add(entity);
            _db.SaveChanges();
            return Ok(entity);
        }
    }
}
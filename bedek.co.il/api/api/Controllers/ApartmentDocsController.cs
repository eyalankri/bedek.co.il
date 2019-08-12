
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;
using api.Utilities;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;


namespace api.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    public class ApartmentDocsController : ControllerBase
    {
        
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ApartmentDocsController(ApplicationDbContext db, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            // var dir1 = _config["Jwt:Key"];
        }

       

        [HttpPost]
        [EnableCors("MyPolicy")]
        [Route("Upload")]        
        public async Task<IActionResult> Upload([FromForm] ApartmentDocDto apartmentDocDto)
        {

            if (apartmentDocDto.PostedFile.Length < 0) return BadRequest("לא נשלח קובץ");

            var appartment = _db.Apartments.Find(apartmentDocDto.ApartmentId);
            var buildingId = appartment.BuildingId;
            var appartmentId = apartmentDocDto.ApartmentId;


            var postedFile = apartmentDocDto.PostedFile;

        
                      
            var contentType = postedFile.ContentType;
            var fileExtension = FileExtension.GetExtension(contentType);

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            

            
            var path = _hostingEnvironment.ContentRootPath;
            // go one folder up to the virtual folder (parent folder)
            var loc = path.LastIndexOf('\\');
            path = path.Remove(loc);


            var dir = Path.Combine($"{path}\\Files\\AppartmentsDocs\\{buildingId}\\{appartmentId}");
            var fullPath = Path.Combine(dir, fileName);
            Directory.CreateDirectory(dir);
            try
            {
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {

                    await postedFile.CopyToAsync(fileStream);
                }
            }
            catch (Exception)
            {
                return BadRequest("הקובץ אינו נשמר!");
            }             

            apartmentDocDto.FileName = fileName;
            apartmentDocDto.FileContentType = contentType;


            var doc = _mapper.Map<ApartmentDoc>(apartmentDocDto);
            if (!ModelState.IsValid) return BadRequest();

            _db.Add(doc);
            _db.SaveChanges();
            return Ok(doc);


        }


        [HttpGet]
        [Route("List")]
        public IQueryable<ApartmentDocDto> List(int apartmentId)
        {
            //var entity = _mapper.Map<Apartment>(apartmentDto);

            var list = from ap in _db.Apartments
                join doc in _db.ApartmentDocs
                    on ap.ApartmentId equals doc.ApartmentId
                orderby doc.DateUploaded
                where ap.ApartmentId == apartmentId
                select new ApartmentDocDto
                {
                    ApartmentDocId = doc.ApartmentDocId,
                    DocDescription = doc.DocDescription,
                    FileName = doc.FileName,
                    BuildingId = ap.BuildingId,
                    ApartmentId = ap.ApartmentId,
                    DateUploaded = doc.DateUploaded                    
                };

            return list;
                           
                           
            
        }

    }
}
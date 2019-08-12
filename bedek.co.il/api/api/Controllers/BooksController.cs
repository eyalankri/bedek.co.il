﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Models;


namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        [HttpGet, Authorize]
        [Route("GetBooks")]
        public IEnumerable<Book> Get()
        {
            var currentUser = HttpContext.User;
            var resultBookList = new Book[] {
        new Book { Author = "Ray Bradbury",Title = "Fahrenheit 451" },
        new Book { Author = "Gabriel García Márquez", Title = "One Hundred years of Solitude" },
        new Book { Author = "George Orwell", Title = "1984" },
        new Book { Author = "Anais Nin", Title = "Delta of Venus" }
      };

            return resultBookList;
        }
    }
}
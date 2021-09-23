using ImplementCors.Base.Controllers;
using ImplementCors.Repositories.Data;
using Microsoft.AspNetCore.Mvc;
using NETCore1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImplementCors.Controllers
{
    [Route("[controller]")]
    public class PersonController : BaseController<Person, PersonRepository, string>
    {
        private readonly PersonRepository repository;

        public PersonController(PersonRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        [HttpGet("GetAllPersons")]
        public async Task<JsonResult> GetAllPersons()
        {
            var result = await repository.GetAllProfile();
            return Json(result);
        }
        [HttpGet("GetPersons/{nik}")]
        public async Task<JsonResult> GetPersons(string nik)
        {
            var result = await repository.GetByNik(nik);
            return Json(result);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

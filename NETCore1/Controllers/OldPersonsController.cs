using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore1.Models;
using NETCore1.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NETCore1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class OldPersonsController : ControllerBase
    {
        private readonly OldPersonRepository personRepository;
        public OldPersonsController(OldPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }
        [HttpPost]
        public ActionResult Insert(Person person)
        {

            try
            {
                
                return Ok(new
                {
                    data = personRepository.Insert(person),
                    statusCode = HttpStatusCode.OK,
                    message = "Success"
                });
            }
            catch
            {
                return BadRequest(new {
                    status = HttpStatusCode.BadRequest,
                    message = "Data Tidak Bisa dupllikat"
                });
                
            }
            
        }
        [HttpGet]
        public ActionResult Get()
        {

            try
            {
                return Ok(new
                {
                    data = personRepository.Get(),
                    /*statusCode = StatusCode(200),*/
                    status = HttpStatusCode.OK,
                    message = "Success"
                });
            }
            catch
            {
                return BadRequest(new
                {
                    status = HttpStatusCode.NoContent,
                    message = "No Data",
                    /*error = e*/
                });
            }

        }
        [HttpGet("{NIK}")]
        public ActionResult Get(string NIK)
        {
            try
            {
                return Ok(new
                {
                    data = personRepository.Get(NIK),
                    statusCode = HttpStatusCode.OK,
                    message = "Success"
                });
            }
            catch
            {
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = "Data tidak ditemukan"
                });
            }
        }

        [HttpPut]

        public ActionResult Update(Person person)
        {
            
            try
            {
                return Ok(personRepository.Update(person));
            }
            catch
            {

                return BadRequest("NIK Tidak ada");
            }
        }

        [HttpDelete("{NIK}")]
        public ActionResult Delete(string NIK)
        {
            try
            {
                personRepository.Delete(NIK);
                return Ok("data Berhasil dihapus");
            }
            catch
            {
                return BadRequest("NIK tidak ada");
            }
        }

    }
}

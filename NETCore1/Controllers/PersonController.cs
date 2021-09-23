using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore1.Base;
using NETCore1.Models;
using NETCore1.Repository.Data;
using System.Net;
    
namespace NETCore1.Controllers
{

    [EnableCors("AllowAllOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : BaseController<Person, PersonRepository, string>

    {

        private readonly PersonRepository personRepository;
      

        public PersonController(PersonRepository repository) : base(repository)
        {
            this.personRepository= repository;
          
        }
        //[Authorize]
        [HttpGet("getperson")]
        public ActionResult GetPerson()
        {
            
                var data = personRepository.GetPerson();
            try
            {
                return Ok(data);
            }
            catch
            {
                return Ok(data);
            }

        }
        [HttpGet("getperson/{nik}")]
        public ActionResult GetPerson(string nik)
        {

            try
            {
                return Ok(personRepository.GetPerson(nik));
                
            }
            catch
            {
                return BadRequest(new
                {
                    status = HttpStatusCode.NoContent,
                    message = "No Data",
                });
            }

        }
       
        [HttpPost("register")]
        public ActionResult Register(PersonViewModel personViewModel)
        {
            try
            {
                var status = personRepository.Register(personViewModel);
                if (status == 200)
                {
                    return Ok(new
                    {
                     
                        status = HttpStatusCode.BadRequest,
                        message = "Pendaftaran Berhasil"
                    });


                }else if(status == 201)
                {
                    return BadRequest(new
                    {
                        
                        status = HttpStatusCode.BadRequest,
                        message = "NIK Sudah Digunakan"
                    });
                }
                else if (status == 202)
                {
                    return BadRequest(new
                    {
                        
                        status = HttpStatusCode.BadRequest,
                        message = "Email Sudah Digunakan"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        
                        status = HttpStatusCode.BadRequest,
                        message = "Phone Sudah Digunakan"
                    });
                }
            }
            catch
            {
                return BadRequest(new
                {
                    status = HttpStatusCode.BadRequest,
                    message = "NIK Sudah Digunakan"
                });

            }

        }






    }
    
}

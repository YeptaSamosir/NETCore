using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore1.Base;
using NETCore1.Models;
using NETCore1.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniversityController : BaseController<University, UniversityRepository, string>

    {
        public UniversityController(UniversityRepository repository) : base(repository)
        {
        }
    }
}

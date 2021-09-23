using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore1.Base;
using NETCore1.Models;
using NETCore1.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NETCore1.Controllers
{
    //[Authorize(Roles = "Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController :BaseController<Role, RoleRepository, int>

    {
        private readonly RoleRepository roleRepository;
        public RoleController(RoleRepository repository) : base(repository)
        {
            this.roleRepository = repository;
        }

        [HttpPost("insert")]
        public ActionResult Insert(Role role)
        {
            try
            {
                var action = roleRepository.InsertRole(role);
                if (action == false)
                {
                    return BadRequest(new
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = "Role Sudah Terdaftar"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = "Role berhasil ditambahkan"
                    });
                }
               
            }
            catch
            {
                return BadRequest(new
                {
                    status = HttpStatusCode.BadRequest,
                    message = "Data Tidak Bisa dupllikat"
                });

            }
        }

    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore1.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NETCore1.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<Entity, Repository, Key> : ControllerBase
        where Entity : class
        where Repository : IRepository<Entity, Key>
    {
        private readonly Repository repository;
        public BaseController(Repository repository)
        {
            this.repository = repository;
        }

        // insert
        [HttpPost]
        public ActionResult Insert(Entity entity)
        {

            try
            {

                return Ok(new
                {
                    data = repository.Insert(entity),
                    statusCode = HttpStatusCode.OK,
                    message = "Success"
                });
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

        [HttpGet]
        public ActionResult Get()
        {

            try
            {
                return Ok(new
                {
                    data = repository.Get(),
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
        [HttpGet("{Key}")]
        public ActionResult Get(Key key)
        {
            try
            {
                return Ok(new
                {
                    data = repository.Get(key),
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

        public ActionResult Update(Entity entity)
        {

            try
            {
                return Ok(repository.Update(entity));
            }
            catch
            {

                return BadRequest("data Tidak ada");
            }
        }

        [HttpDelete("{key}")]
        public ActionResult Delete(Key key)
        {
            try
            {
                repository.Delete(key);
                return Ok("data Berhasil dihapus");
            }
            catch
            {
                return BadRequest("data tidak ada");
            }
        }

    }
}


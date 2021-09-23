
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NETCore1.Base;
using NETCore1.Models;
using NETCore1.Repository.Data;
using NETCore1.ViewModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NETCore1.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController<LoginVm, AccountRepository, string>

    {
        private readonly AccountRepository accountRepository;
        public IConfiguration configuration;
        public AccountController(IConfiguration config,AccountRepository repository) : base(repository)
        {
            this.accountRepository = repository;
            this.configuration = config;
        }

        [HttpPost("login")]
        public ActionResult Login(LoginModel loginModel)
        {
            var action = accountRepository.Login(loginModel);
            if (action == 1)
            {
                string[] roles = accountRepository.GetRole(accountRepository.GetEmail(loginModel.Email));
                var claims = new List<Claim>();

                claims.Add(new Claim("Email", loginModel.Email));
                foreach (string role in roles)
                {
                    claims.Add(new Claim("roles", role));
                }

                
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                var token_handler = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new JWTokenVM { Token = token_handler, Message = "Login Berhasil" });

            }
            else if (action == 0)
            {
                return BadRequest(new
                {
                    status = HttpStatusCode.OK,
                    message = "Wrong Password "
                });
            }
            else
            {
                return BadRequest(new
                {
                    status = HttpStatusCode.OK,
                    message = "Wrong Email"
                });
            }
        }
        [HttpPost("forgotpassword")]
        public ActionResult ForgotPassword(ForgotPassword forgotPassword)
        {
           
                accountRepository.ForgotPassword(forgotPassword);
                return StatusCode((int)HttpStatusCode.Created, new
                {
                    status = HttpStatusCode.OK,
                    message = "Success"
                });
            
           
                
        }

        [ HttpPost("changepassword")]
        public ActionResult ChangePassword(ChangePassword changePassword)
        {
            try
            {
                var action = accountRepository.ChangePassword(changePassword);
                if (action == 1)
                {
                    return Ok(new
                    {
                        data = action,
                        status = HttpStatusCode.OK,
                        message = "Password Berhasil Diubah"
                    });
                }
                else if (action == 0)
                {
                    return NotFound(new
                    {
                        data = action,
                        status = HttpStatusCode.OK,
                        message = "Password Anda Salah"
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        data = action,
                        status = HttpStatusCode.OK,
                        message = "Email tidak terdaftar"
                    });
                }
            }
            catch
            {

                return BadRequest(new
                {  
                    status = HttpStatusCode.OK,
                    message = "Password Confirmasi anda tidak sama"
                });
            }
           
        }
    }
}

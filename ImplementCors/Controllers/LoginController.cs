using ImplementCors.Base.Controllers;
using ImplementCors.Repositories.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ImplementCors.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginRepository loginRepository;

        public LoginController(LoginRepository loginRepository)
        {
            this.loginRepository = loginRepository;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "Admin");
            }
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(string Email,string Password)
        {
            var login = new LoginModel { Email = Email, Password = Password };
            var jwtToken = await loginRepository.Auth(login);
            var token = jwtToken.Token;

            if (token == null)
            {

                return RedirectToAction("index");
            }

            HttpContext.Session.SetString("JWToken", token);
            //HttpContext.Session.SetString("Name", jwtHandler.GetName(token));
            //HttpContext.Session.SetString("ProfilePicture", "assets/img/theme/user.png");
            
            return RedirectToAction("index", "Admin");
        }
        
        
    }
}

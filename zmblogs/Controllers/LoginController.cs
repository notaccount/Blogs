using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Power.Repository;
using Power.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace CommonPower.WebApp.Controllers
{
    public class LoginController : Controller
    {
        DataContext _db;
        public LoginController(DataContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginValidate(IFormCollection collection)
        {
            string uid = collection["UID"];
            string pwd = collection["PassWord"];
            var model = _db.PowerUser.Where(x => x.UID == uid).FirstOrDefault();
            if (model != null)
            {
                if (model.PassWord == pwd)
                {
                    //string userinfo = JsonConvert.SerializeObject(new PowerUser()
                    //{
                    //    Id = model.Id,
                    //    UID = model.UID,
                    //    Cn = model.Cn
                    //});

                    //HttpContext.Session.SetString("UserInfo", userinfo);


                    ////使用Form验证方式
                    //List<Claim> claims = new List<Claim>();
                    //claims.Add(new Claim(ClaimTypes.Name, model.Cn, ClaimValueTypes.String, model.Id.ToString()));
                    //var userIdentity = new ClaimsIdentity("管理员"); //角色
                    //userIdentity.AddClaims(claims);
                    //var userPrincipal = new ClaimsPrincipal(userIdentity);


                    //HttpContext.SignInAsync(CookieAuthenInfo.WebCookieInstance, userPrincipal,
                    //      new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                    //      {
                    //          ExpiresUtc = DateTime.UtcNow.AddMinutes(12),
                    //          IsPersistent = true,
                    //          AllowRefresh = false
                    //      });

                    SaveAuthen(model);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View("Index", new PowerUser() { UID = uid, PassWord = pwd });
        }


        private void SaveAuthen(PowerUser model)
        {
            string userinfo = JsonConvert.SerializeObject(new PowerUser()
            {
                Id = model.Id,
                UID = model.UID,
                Cn = model.Cn
            });

            HttpContext.Session.SetString("UserInfo", userinfo);


            //使用Form验证方式
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, model.Cn, ClaimValueTypes.String, model.Id.ToString()));
            var userIdentity = new ClaimsIdentity("管理员"); //角色
            userIdentity.AddClaims(claims);
            var userPrincipal = new ClaimsPrincipal(userIdentity);


            HttpContext.SignInAsync(CookieAuthenInfo.WebCookieInstance, userPrincipal,
                  new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                  {
                      ExpiresUtc = DateTime.UtcNow.AddDays(1),
                      IsPersistent = true,
                      AllowRefresh = false
                  });
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("UserInfo");
            HttpContext.SignOutAsync(CookieAuthenInfo.WebCookieInstance);
            return RedirectToAction("Index","Home");
        }
    }
}
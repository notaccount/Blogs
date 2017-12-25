using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Power.Repository;
using Power.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace CommonPower.WebApp.Controllers
{
    public class PowerUserController : Controller
    {

        DataContext _db;
        public PowerUserController(DataContext db)
        {
            _db = db;
        }


        // GET: PowerUser
        public ActionResult Index()
        {
            var list = _db.PowerUser.ToList();
            return View(list);
        }

        // GET: PowerUser/Details/5
        public ActionResult Details(string id)
        {
            var model = _db.PowerUser.Where(x => x.UID.ToString() == id).FirstOrDefault();
            return View(model);
        }

        // GET: PowerUser/Create
        public ActionResult Create()
        {
            PowerUser model = new PowerUser();
            ViewData["message"] = "";
            return View(model);
        }

        // POST: PowerUser/Create
        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            PowerUser model = new PowerUser();
            try
            {
                string uid =  collection["UID"];
                string password = collection["PassWord"];
                string cn = collection["Cn"];
                Guid id = Guid.NewGuid();
                model.Id = id;
                model.UID = uid;
                model.Cn = cn;
                model.PassWord = password;
                model.U_CreateDate = DateTime.Now;

                if (_db.PowerUser.Where(x => x.UID == uid.Trim()).Count() > 0)
                {
                    ViewData["message"] = "用户名已存在";
                    return View(model);
                }

                _db.PowerUser.Add(model);
                _db.SaveChanges();

                string userinfo = JsonConvert.SerializeObject(new PowerUser()
                {
                    Id = id,
                    UID = uid,
                    Cn = cn
                });

                HttpContext.Session.SetString("UserInfo", userinfo);

                SaveAuthen(model);

                return RedirectToAction("Index","Home");

                // TODO: Add insert logic her
            }
            catch(Exception ex)
            {
                ViewData["message"] = "账号注册异常，刷新后重试";
                return View(model);
            }
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
                      ExpiresUtc = DateTime.UtcNow.AddMinutes(12),
                      IsPersistent = true,
                      AllowRefresh = false
                  });
        }









        // GET: PowerUser/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PowerUser/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PowerUser/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PowerUser/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
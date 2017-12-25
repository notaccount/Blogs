using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Power.Repository;
using Power.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace CommonPower.WebApp.Controllers
{
    public class TagsController : Controller
    {
        DataContext _db;
        public TagsController(DataContext db)
        {
            _db = db;
        }


        // GET: PowerUser
        public ActionResult Index()
        {
            var list = _db.Tags.ToList();
            return View(list);
        }

        // GET: PowerUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PowerUser/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Tags tag)
        {
            try
            {
                if (string.IsNullOrEmpty(tag.Name))
                {
                    ModelState.AddModelError("Name", "请输入名次");
                }
                if (!ModelState.IsValid)
                {
                    return View(tag);
                }
                PowerUser puser = Helper.CurrentUser(User.Claims.FirstOrDefault().Issuer, _db);
                Guid id = Guid.NewGuid();
                tag.Id = id;
                tag.Creator = puser.Cn;
                tag.CreatorId = puser.Id;
                tag.U_CreateDate = DateTime.Now;
                _db.Tags.Add(tag);
                _db.SaveChanges();

                return RedirectToAction("Index", "Tags");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: PowerUser/Edit/5
        public ActionResult Edit(string id)
        {
            Tags tag = new Tags();
            if (!string.IsNullOrEmpty(id))
            {
                Guid gid = Guid.Parse(id);
                tag = _db.Tags.Find(gid);
            }
            return View(tag);
        }

        // POST: PowerUser/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    string name = collection["Name"];
                    string remark = collection["Remark"];

                    Guid gid = Guid.Parse(id);
                    Tags tag = _db.Tags.Find(gid);
                    tag.Name = name;
                    tag.Remark = remark;

                    _db.Entry(tag).State = EntityState.Modified;
                    _db.SaveChanges();

                    return RedirectToAction("Index");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }


        // GET: Tags/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Tags/Delete/5
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Power.Repository;
using Power.Models;
using Newtonsoft.Json;

namespace CommonPower.WebApp.Controllers
{
    public class CommentController : Controller
    {
        DataContext _db;
        public CommentController(DataContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string comment,string blogId)
        {
            try
            {
                var creatorId = User.Claims.FirstOrDefault().Issuer;
                PowerUser puser = Helper.CurrentUser(creatorId, _db);
                int sortno = _db.Comment.Where(x => x.BlogId.ToString() == blogId).DefaultIfEmpty().Max(x => x.SortNo);

                Comment model = new Comment();
                model.Id = Guid.NewGuid();
                model.MainContent = comment;
                model.Creator = puser.UID;
                model.CreatorId = puser.Id;
                model.Oppose = 0;
                model.Support = 0;
                model.BlogId = Guid.Parse(blogId);
                model.U_CreateDate = DateTime.Now;
                model.SortNo = sortno+1;

                _db.Comment.Add(model);
                _db.SaveChanges();
                return Content(JsonConvert.SerializeObject(model));
            }
            catch(Exception ex)
            {
                return View();
            }
        }


    }
}
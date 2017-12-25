using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Power.Models;
using Power.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CommonPower.WebApp
{
    [Authorize]
    public class BlogsController : Controller
    {
        private readonly DataContext _context;
        public BlogsController(DataContext context)
        {
            _context = context;
        }

        //[AllowAnonymous]
        //[Route("/Blogs/Index")]
        //public IActionResult Index()
        //{
        //    string id = User.Claims.FirstOrDefault() == null ? "" : User.Claims.FirstOrDefault().Issuer;
        //    PowerUser model = Helper.CurrentUser(id, _context);
        //    List<Blogs> list = (from c in _context.Blogs
        //                        join t in _context.BlogTag
        //                        on new { id = c.Id } equals new { id = t.BlogId }
        //                        into temp
        //                        from bb in temp.DefaultIfEmpty()
        //                        join p in _context.PowerUser
        //                        on new { id = c.PowerUserId } equals new { id = p.Id }
        //                        where c.IsOpen == true
        //                        orderby c.U_CreateDate descending
        //                        select new Blogs
        //                        {
        //                            Id = c.Id,
        //                            IsDelete = c.IsDelete,
        //                            IsOpen = c.IsOpen,
        //                            MainContent = c.MainContent,
        //                            Title = c.Title,
        //                            PageView = c.PageView,
        //                            PowerUserId = c.PowerUserId,
        //                            PowerUser = p,
        //                            ReCommend = c.ReCommend,
        //                            ShortContent = c.ShortContent,
        //                            U_CreateDate = c.U_CreateDate
        //                        }).ToList();
        //    ViewBag.UserInfo = model;
        //    ViewBag.TagList = _context.Tags.ToList();
        //    return View(list);
        //}

        //[AllowAnonymous]
        //[Route("/Blogs/Tag")]
        //public IActionResult Index(string tag)
        //{
        //    string id = User.Claims.FirstOrDefault() == null ? "" : User.Claims.FirstOrDefault().Issuer;
        //    PowerUser model = Helper.CurrentUser(id, _context);
        //    var tagModel = _context.Tags.Where(x => x.Name == tag).FirstOrDefault();

        //    List<Blogs> list = (from c in _context.Blogs
        //                        join t in _context.BlogTag
        //                        on new { id = c.Id } equals new { id = t.BlogId }
        //                          into temp
        //                        from bb in temp.DefaultIfEmpty()
        //                        join p in _context.PowerUser
        //                        on new { id = c.PowerUserId } equals new { id = p.Id }
        //                        where bb.TagId == tagModel.Id && c.IsOpen == true
        //                        orderby c.U_CreateDate descending
        //                        select new Blogs
        //                        {
        //                            Id = c.Id,
        //                            IsDelete = c.IsDelete,
        //                            IsOpen = c.IsOpen,
        //                            MainContent = c.MainContent,
        //                            Title = c.Title,
        //                            PageView = c.PageView,
        //                            PowerUserId = c.PowerUserId,
        //                            PowerUser = p,
        //                            ReCommend = c.ReCommend,
        //                            ShortContent = c.ShortContent,
        //                            U_CreateDate = c.U_CreateDate
        //                        }).ToList();


        //    ViewBag.UserInfo = model;
        //    ViewBag.TagList = _context.Tags.ToList();
        //    return View(list);
        //}


        [AllowAnonymous]
        public IActionResult Details(Guid id)
        {
            var model = _context.Blogs.Where(x => x.Id == id).FirstOrDefault();
            var a = _context.Database.BeginTransaction();
            model.PageView = model.PageView + 1;
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
            a.Commit();


            ViewBag.Entity = model;
            return View(model);
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            Guid id = Guid.NewGuid();
            ViewBag.TagList = _context.Tags.ToList();
            ViewBag.Id = id;
            return View();
        }


        public string UpFile()
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);

            var afile = files[0];
            var fileName = afile.FileName;
            string filePath = AppContext.BaseDirectory + $@"\wwwroot\Files\BlogImg\";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            fileName = Guid.NewGuid() + "." + fileName.Split('.')[1];
            string fileFullName = filePath + fileName;
            using (FileStream fs = System.IO.File.Create(fileFullName))
            {
                afile.CopyTo(fs);
                fs.Flush();
            }
            string path = "http://"+ Request.Host + $@"/Files/BlogImg/" + fileName;
            return path;
        }




        // POST: Blogs/Create
        [HttpPost]
        //[ValidateInput(false)]
        public IActionResult Create(IFormCollection collection)
        {
            try
            {
                Guid id = Guid.Parse(collection["Id"]);
                var title = collection["Title"];
                var content = collection["MainContent"];
                var strIsopen = collection["IsOPen"];
                bool IsOpen = strIsopen == "Tree" ? true : false;
                string checkedtag = collection["checkedtag"];

                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
                {
                    ViewBag.TagList = _context.Tags.ToList();
                    return View();
                }



                PowerUser model = Helper.CurrentUser(User.Claims.FirstOrDefault().Issuer, _context);
                //Guid id = Guid.NewGuid();

                _context.Blogs.Add(new Blogs() { Id = id, Title = title, U_CreateDate = DateTime.Now, PowerUser = model, ShortContent = Helper.ReplaceHtmlTag(content, 135), MainContent = content, IsOpen = IsOpen, IsDelete = false });

                List<BlogTag> taglist = new List<BlogTag>();
                string[] tags = checkedtag.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in tags)
                {
                    taglist.Add(new BlogTag() { Id = Guid.NewGuid(), BlogId = id, TagId = Guid.Parse(item), U_CreateDate = DateTime.Now });
                }
                _context.BlogTag.AddRange(taglist);

                var list = _context.Tags.Where(x => tags.Contains(x.Id.ToString())).ToList();
                foreach (var item in list)
                {
                    item.BlogNum++;
                }
                _context.Tags.UpdateRange(list);

                _context.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: Blogs/Edit/5
        public IActionResult Edit(string id)
        {

            var model = _context.Blogs.Where(x => x.Id.ToString() == id).FirstOrDefault();
            ViewBag.Entity = model;

            ViewBag.TagList = _context.Tags.ToList();

            ViewBag.CheckedTag = _context.BlogTag.Where(x => x.BlogId == model.Id).ToList();
            return View(model);
        }

        // POST: Blogs/Edit/5
        [HttpPost]
        //[ValidateInput(false)]
        public IActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                var title = collection["Title"];
                var content = collection["MainContent"];

                Blogs a = _context.Blogs.Find(Guid.Parse(id));
                a.Title = title;
                a.MainContent = content;

                //var bb = new Blogs() { Id = Guid.Parse(id), Title = title, MainContent = content };
                _context.Entry(a).State = EntityState.Modified;
                _context.SaveChanges();


                return RedirectToAction("Index","Home");
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        public IActionResult Manage()
        {
            string userId = User.Claims.FirstOrDefault().Issuer;
            IList<Blogs> list = _context.Blogs.Where(x => x.PowerUserId.ToString() == userId).ToList();
            return View(list);
        }


        // GET: Blogs/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: Blogs/Delete/5
        [HttpPost]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

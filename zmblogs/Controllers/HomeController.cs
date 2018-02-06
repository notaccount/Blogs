using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Power.Models;
using Power.Repository;

namespace CommonPower.WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DataContext _context;
        public HomeController(DataContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index(int id = 1)
        {
            int PageIndex = id;
            int PageSize = 15;
            string UserId = User.Claims.FirstOrDefault() == null ? "" : User.Claims.FirstOrDefault().Issuer;
            PowerUser model = Helper.CurrentUser(UserId, _context);

            var query = 
                         from c in _context.Blogs
                         join p in (from m in _context.PowerUser select new PowerUser { Cn = m.Cn,Id = m.Id,IsSuperAdmin = m.IsSuperAdmin,UID = m.UID})
                         on c.PowerUserId equals p.Id
                         where c.IsOpen == true
                         orderby c.U_CreateDate descending
                         select new Blogs
                         {
                             Id = c.Id,
                             IsDelete = c.IsDelete,
                             IsOpen = c.IsOpen,
                             MainContent = c.MainContent,
                             Title = c.Title,
                             PageView = c.PageView,
                             PowerUserId = c.PowerUserId,
                             PowerUser = p,
                             ReCommend = c.ReCommend,
                             ShortContent = c.ShortContent,
                             U_CreateDate = c.U_CreateDate
                         };
            IList<Blogs> list = query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            ViewBag.Pager = Helper.Pager(PageIndex, PageSize, query.Count(),true,Request.Path);

            ViewBag.UserInfo = model;
            ViewBag.TagList = _context.Tags.ToList();
            return View(list);
        }

        [AllowAnonymous]
        [Route("Home/Tag")]
        public IActionResult Tag(string tag)
        {
            string id = User.Claims.FirstOrDefault() == null ? "" : User.Claims.FirstOrDefault().Issuer;
            PowerUser model = Helper.CurrentUser(id, _context);
            var tagModel = _context.Tags.Where(x => x.Name == tag).FirstOrDefault();

            List<Blogs> list = (from c in _context.Blogs
                                join t in _context.BlogTag
                                on new { id = c.Id } equals new { id = t.BlogId }
                                  into temp
                                from bb in temp.DefaultIfEmpty()
                                join p in _context.PowerUser
                                on new { id = c.PowerUserId } equals new { id = p.Id }
                                where bb.TagId == tagModel.Id && c.IsOpen == true
                                orderby c.U_CreateDate descending
                                select new Blogs
                                {
                                    Id = c.Id,
                                    IsDelete = c.IsDelete,
                                    IsOpen = c.IsOpen,
                                    MainContent = c.MainContent,
                                    Title = c.Title,
                                    PageView = c.PageView,
                                    PowerUserId = c.PowerUserId,
                                    PowerUser = p,
                                    ReCommend = c.ReCommend,
                                    ShortContent = c.ShortContent,
                                    U_CreateDate = c.U_CreateDate
                                }).ToList();


            ViewBag.UserInfo = model;
            ViewBag.TagList = _context.Tags.ToList();
            return View("Index", list);
        }


        public Guid GenerateGuid()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();

            var baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            var days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = now.TimeOfDay;

            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }

    }
}
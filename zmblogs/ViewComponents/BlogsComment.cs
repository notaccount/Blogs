using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Power.Repository;

namespace CommonPower.WebApp.ViewComponents
{
    public class BlogsComment : ViewComponent
    {
        DataContext _db;
        public BlogsComment(DataContext db)
        {
            _db = db;
        }


        public IViewComponentResult Invoke(string id)
        {
            var list = _db.Comment.Where(x => x.BlogId.ToString() == id).OrderByDescending(x=>x.SortNo).ToList();
            ViewBag.list = list;
            return View();
        }

    
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace zmblogs.WebApp.ApiControllers
{
    public class FileUpDownController : Controller
    {
        //[Route("api/[controller]")]
        [HttpPost]
        public IActionResult UpFile(IFormCollection collection)
        {
            var oFile = collection["txt_file"];
         



            return View();
        }


        public IActionResult DownFile()
        {
            return View();
        }
    }
}
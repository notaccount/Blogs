using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Power.Models;
using Power.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CommonPower.WebApp
{
    public class Helper
    {
        public static PowerUser CurrentUser(string id, DataContext db)
        {
            if(string.IsNullOrEmpty(id))
                return new PowerUser();
            var model = db.PowerUser.Where(x => x.Id.ToString() == id).FirstOrDefault();
            if (model == null)
                return new PowerUser();
            return model;
        }

        public static string ReplaceHtmlTag(string html, int length = 0)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }

        public static string Pager(int PageIndex,int PageSize,int DataCount,bool IsFirstPage,string path)
        {
            int PageCount = (DataCount + PageSize - 1) / PageSize;
            path = string.Join("/",path.Split("/").Take(3).ToArray());
            StringBuilder sb = new StringBuilder();


            string prevUrl = "";
            string nextUrl = "";

            if (IsFirstPage)
            {
                if (PageIndex == 1)
                    prevUrl = "javascript:void(0)";
                else if (PageIndex == 2)
                    prevUrl = "/";
                else
                    prevUrl = "/Home/Index/" + (PageIndex - 1);
                nextUrl = PageIndex == PageCount ? "javascript:void(0)" : "/Home/Index/" + (PageIndex + 1);
            }
            else {
                if (PageIndex == 1)
                    prevUrl = "javascript:void(0)";
                else if (PageIndex == 2)
                    prevUrl = path;
                else
                    prevUrl = path + "/" + (PageIndex - 1);
                nextUrl = PageIndex == PageCount ? "javascript:void(0)" : path + "/" + (PageIndex + 1);
            }

            sb.AppendFormat("<li><a href='{0}'>Prev</a></li>",prevUrl);

            sb.AppendFormat("<li><a href='{0}'>Next</a></li>",nextUrl);
            // <li>
            //    <a href="#">Prev</a>
            //</li>
            //<li>
            //    <a href="#">1</a>
            //</li>
            //<li>
            //    <a href="#">2</a>
            //</li>
            //<li>
            //    <a href="#">3</a>
            //</li>
            //<li>
            //    <a href="#">4</a>
            //</li>
            //<li>
            //    <a href="#">5</a>
            //</li>
            //<li>
            //    <a href="#">Next</a>
            //</li>

            return sb.ToString();
        }

    }
}

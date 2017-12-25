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
    }
}

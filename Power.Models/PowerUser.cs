using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Power.Models
{
    public class PowerUser : BaseEntity
    {

        /// <summary>
        /// 账户
        /// </summary>
        [DisplayName("账户")]
        [StringLength(20)]
        public string UID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [DisplayName("姓名")]
        [StringLength(20)]
        public string Cn { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DisplayName("密码")]
        [StringLength(50)]
        public string PassWord { get; set; }

    }
}

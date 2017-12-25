using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Power.Models
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("发表时间")]
        public DateTime? U_CreateDate { get; set; }

    }
}

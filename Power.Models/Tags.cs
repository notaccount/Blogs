using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Power.Models
{
    public class Tags : BaseEntity
    {
        [Required]
        [StringLength(1000)]
        [DisplayName("标签名称")]
        public string Name { get; set; }

        [DisplayName("备注")]
        public string Remark { get; set; }
        
        [DisplayName("创建人ID")]
        public Guid CreatorId { get; set; }

        [DisplayName("创建人名称")]
        public string Creator { get; set; }

        [DisplayName("文章数量")]
        public int BlogNum { get; set; }
    }
}

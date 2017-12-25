using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Power.Models
{
    public class Blogs:BaseEntity
    {
        [DisplayName("标题")]
        [StringLength(100)]
        public string Title { get; set; }

        [DisplayName("摘要")]
        public string ShortContent { get; set; }

        [DisplayName("博文")]
        public string MainContent { get; set; }

        public Guid PowerUserId { get; set; }

        [ForeignKey("PowerUserId")]
        public PowerUser PowerUser { get; set; }

        public bool IsDelete { get; set; }

        [DisplayName("是否公开")]
        public bool IsOpen { get; set; }

        [DisplayName("浏览量")]
        public int PageView { get; set; }

        [DisplayName("推荐")]
        public int ReCommend { get; set; }
    }
}

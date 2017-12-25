using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Power.Models
{
    public class BlogTag:BaseEntity
    {
        [DisplayName("博客ID")]
        public Guid BlogId { get; set; }

        [DisplayName("标签ID")]
        public Guid TagId { get; set; }
    }
}

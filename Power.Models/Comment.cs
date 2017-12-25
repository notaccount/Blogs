using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Power.Models
{
    public class Comment: BaseEntity
    {
        [DisplayName("序号")]
        public int SortNo { get; set; }

        [DisplayName("博客ID")]
        public Guid BlogId { get; set; }

        [DisplayName("评论者Id")]
        public Guid CreatorId { get; set; }

        [DisplayName("评论者姓名")]
        public string Creator { get; set; }

        [DisplayName("评论内容")]
        public string MainContent { get; set; }

        [DisplayName("支持数量")]
        public int Support { get; set; }

        [DisplayName("反对数量")]
        public int Oppose { get; set; }
    }
}

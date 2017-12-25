using System;
using System.Collections.Generic;
using System.Text;

namespace Power.Models
{
    public class Attachment : BaseEntity
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileAddress { get; set; }
        public int AttachMentType { get; set; }
    }
}

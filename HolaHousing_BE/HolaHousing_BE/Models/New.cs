using System;
using System.Collections.Generic;

namespace HolaHousing_BE.Models
{
    public partial class New
    {
        public New()
        {
            PartContents = new HashSet<PartContent>();
            Tags = new HashSet<Tag>();
        }

        public int NewId { get; set; }
        public string? Title { get; set; }
        public string? Sumary { get; set; }
        public string? Author { get; set; }
        public DateTime? PostDate { get; set; }
        public int? CreatedBy { get; set; }

        public virtual User? CreatedByNavigation { get; set; }
        public virtual ICollection<PartContent> PartContents { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}

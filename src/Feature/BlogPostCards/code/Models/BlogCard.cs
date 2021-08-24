using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogHelix.Feature.BlogPostCard.Models
{
    public class BlogCard
    {
        public string BlogId { get; set; }

        public string BlogTitle { get; set; }

        public string BlogSImage { get; set; }

        public string date { get; set; }

        public string Category { get; set; }

        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public string Readonbtn { get; set; }

        public Sitecore.Data.Items.Item sitecoreItem { get; set; }

        public string BlogURL { get; set; }


    }
}
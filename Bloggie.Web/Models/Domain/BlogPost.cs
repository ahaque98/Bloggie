﻿using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Bloggie.Web.Models.Domain
{
    public class BlogPost
    {
        [Key]
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; } 
        public Boolean Visible { get; set; }
    
        // Navigation Property
        public ICollection<Tag> Tags { get; set; } 
        public ICollection<BlogPostLike> Likes { get; set; } 
    }
}

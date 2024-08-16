using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Bloggie.Web.Models.Domain
{
    public class Tag
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        //navigation property
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}

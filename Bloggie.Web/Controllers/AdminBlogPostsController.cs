using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Bloggie.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagInterface tagRepository;
        private readonly IBlogPostInterface blogPostRepository;
        public AdminBlogPostsController(ITagInterface tagRepository, IBlogPostInterface blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // Get all tags from repository
            var tags = await tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest blogPostRequest)
        {
            //map view model to domain model
            var model = new BlogPost
            {
                Heading = blogPostRequest.Heading,
                PageTitle = blogPostRequest.PageTitle,
                Content = blogPostRequest.Content,
                ShortDescription = blogPostRequest.ShortDescription,
                FeaturedImageUrl = blogPostRequest.FeaturedImageUrl,
                UrlHandle = blogPostRequest.UrlHandle,
                PublishedDate = blogPostRequest.PublishedDate,
                Author = blogPostRequest.Author,
                Visible = blogPostRequest.Visible,
            };

            //map tags from selected tags 
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in blogPostRequest.SelectedTag)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }

            //add blog post to repository
            model.Tags = selectedTags;

            await blogPostRepository.AddAsync(model);

            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            // Get all blog posts from repository
            var blogPosts = await blogPostRepository.GetAllAsync();

            return View(blogPosts);
        }
    }
}

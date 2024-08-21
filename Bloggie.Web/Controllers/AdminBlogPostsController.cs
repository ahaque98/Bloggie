using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")] 
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
            //convert publishedDate to UTC
            blogPostRequest.PublishedDate = DateTime.SpecifyKind(blogPostRequest.PublishedDate, DateTimeKind.Utc);

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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // retrieve the results from the repository
            var blogPost = await blogPostRepository.GetAsync(id);

            var tagsDomainModel = await tagRepository.GetAllAsync();

            //map the domain model to the view model
            if (blogPost != null)
            {
                var model = new EditBlogPostRequest
                {
                    Id = id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    Visible = blogPost.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTag = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };

                return View(model);
            }

            // pass data to view
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            //map the domain model to the view model
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Author = editBlogPostRequest.Author,
                Visible = editBlogPostRequest.Visible,
            };

            //map tags into domain model
            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTag)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);

                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }

            blogPostDomainModel.Tags = selectedTags;

            var updatedBlog = await blogPostRepository.UpdateAsync(blogPostDomainModel);

            if (updatedBlog != null)
            {
                return RedirectToAction("Edit");
            }

            return RedirectToAction("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

            if (deletedBlogPost != null)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new {id = editBlogPostRequest.Id});
        }
    }
}

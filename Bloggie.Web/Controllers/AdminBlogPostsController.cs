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
        public AdminBlogPostsController(ITagInterface tagRepository)
        {
            this.tagRepository = tagRepository;
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
    }
}

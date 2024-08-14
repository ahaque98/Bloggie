using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ILogger<AdminTagsController> _logger;
        private readonly BloggieDbContext _context;
        public AdminTagsController(ILogger<AdminTagsController> logger, BloggieDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public IActionResult SubmitTag(AddTagRequest addTagRequest)
        {
            if (ModelState.IsValid)
            {
                // Mapping the AddTagRequest viewModel to Tag model
                var tag = new Tag
                {
                    Name = addTagRequest.Name,
                    DisplayName = addTagRequest.DisplayName
                };

                _context.Tags.Add(tag);

                _logger.LogInformation($"----------Tag {tag.Name} added successfully to Db----------");

                _context.SaveChanges();

                _logger.LogInformation("----------DB savechanges successfull----------");
            }

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public IActionResult List()
        {
            IEnumerable<Tag> tags = _context.Tags.ToList();

            return View(tags);
        }
    }
}

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

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var tag = _context.Tags.Find(id);

            if (tag == null)
            {
                _logger.LogInformation($"Tag with ID:{tag} not found in Db");
                return NotFound();
            }

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };

                return View(editTagRequest);
            }

            return View(null);
        }

        [HttpPost]
        public IActionResult Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

            var existingTag = _context.Tags.Find(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                _context.SaveChanges();
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }

            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        [HttpPost]
        public IActionResult Delete(EditTagRequest editTagRequest)
        {
            var tag = _context.Tags.Find(editTagRequest.Id);

            if (tag != null)
            {
                _context.Tags.Remove(tag);
                _context.SaveChanges();
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ILogger<AdminTagsController> _logger;
        public AdminTagsController(ILogger<AdminTagsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
    }
}

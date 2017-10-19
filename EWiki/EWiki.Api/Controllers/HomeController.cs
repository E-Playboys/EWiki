using EWiki.Api.DataAccess;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EWiki.Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFeedInfoRepository _feedInfoRepo;

        public HomeController(IFeedInfoRepository feedInfoRepo)
        {
            _feedInfoRepo = feedInfoRepo;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            _feedInfoRepo.ExecuteFeeder();
            return View();
        }
    }
}

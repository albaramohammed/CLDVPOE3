using CLDV1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CLDV1.Controllers
{
    public class HomeController : Controller
    {
       

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult MyWork()
        {
            return View();
        }
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            return View();
        }
   
        public IActionResult Privacy()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
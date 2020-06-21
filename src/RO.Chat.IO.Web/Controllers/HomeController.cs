using System.Web.Mvc;

namespace RO.Chat.IO.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Current = "Home";
            return View();
        }

  
        public ActionResult About()
        {
            ViewBag.Current = "About";
            return View();
        }
    }
}
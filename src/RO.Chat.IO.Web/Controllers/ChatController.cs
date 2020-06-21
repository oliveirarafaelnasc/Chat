using System.Web.Mvc;

namespace RO.Chat.IO.Web.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        public ActionResult Index()
        {
            ViewBag.Current = "Chat";
            return View();
        }
    }
}
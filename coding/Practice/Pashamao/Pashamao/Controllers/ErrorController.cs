using System.Web.Mvc;

namespace Pashamao.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SuspendedInfo()
        {
            return View();
        }

    }
}
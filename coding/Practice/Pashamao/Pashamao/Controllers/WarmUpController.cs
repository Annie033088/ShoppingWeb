using System.Web.Mvc;

namespace Pashamao.Controllers
{
    public class WarmUpController : Controller
    {
        public ActionResult Index()
        {
            return Content("Application Warmed Up!");
        }
    }
}
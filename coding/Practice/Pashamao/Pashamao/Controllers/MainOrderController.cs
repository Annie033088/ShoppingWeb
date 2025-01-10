using Pashamao.Filters;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    [RequestLoggerFilter]
    public class MainOrderController : Controller
    {
        // GET: MainOrder
        public ActionResult Index()
        {
            return View();
        }
    }
}
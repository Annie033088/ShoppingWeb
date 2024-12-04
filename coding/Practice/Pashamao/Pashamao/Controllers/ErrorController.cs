using System.Web.Mvc;

namespace Pashamao.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            if (Session["UserVisitState"].ToString() == "Guest") ViewBag.User = "Guest";
            if (Session["UserVisitState"].ToString() == "User") ViewBag.User = "User";
            return View();
        }

    }
}
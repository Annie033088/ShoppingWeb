using Pashamao.Filters;
using System.Web.Mvc;
using System.Web.Security;

namespace Pashamao.Controllers
{
    [UserKickOutFilter]
    public class MainHomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session["UserSession"] = null;
            return RedirectToAction("Index", "Login");
        }
    }
}
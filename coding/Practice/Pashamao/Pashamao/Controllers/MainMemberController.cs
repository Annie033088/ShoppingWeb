using Pashamao.Filters;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    [UserKickOutFilter]
    public class MainMemberController : Controller
    {
        /// <summary>
        /// 主頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}
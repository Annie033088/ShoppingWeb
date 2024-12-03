using Pashamao.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly UserRoleService userRoleService;

        public UserRoleController()
        {
            userRoleService = new UserRoleService();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        /// <summary>
        /// 取得所有角色資料
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllRole()
        {
            return Json(userRoleService.GetAllRole(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditUserRole()
        {
            return View();
        }

    }
}
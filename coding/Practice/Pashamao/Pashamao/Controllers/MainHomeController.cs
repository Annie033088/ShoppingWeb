﻿using Pashamao.Filters;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    [UserKickOutFilter]
    public class MainHomeController : Controller
    {
        /// <summary>
        /// 主頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}
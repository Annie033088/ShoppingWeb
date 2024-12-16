using NLog;
using Pashamao.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pashamao.Models;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    public class MemberAddressController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private MemberAddressService MemberAddressService;
        
        public MemberAddressController()
        {
            MemberAddressService = new MemberAddressService();
        }
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 取得地址(無搜尋狀態)
        /// </summary>
        /// <param name="Column"></param>
        /// <param name="Page"></param>
        /// <param name="SortOrder"></param>
        /// <returns></returns>
        public ActionResult GetSortedAddress(string Column, string Page, string SortOrder)
        {
            try
            {
                (List<MemberAddress> addresses, int totalPages) = MemberAddressService.GetSortedAddress(Column, Page, SortOrder);

                if (addresses == null)
                {
                    string noAddress = "noAddress";
                    return Json(noAddress, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json((addresses, totalPages), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }
    }
}
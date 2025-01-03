using NLog;
using Pashamao.Models;
using Pashamao.Models.Dto.AddressDto;
using Pashamao.Service;
using System;
using System.Collections.Generic;
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
        public ActionResult GetSortedAddress(RequestGetSortedAddressDto getSortedAddressDto)
        {
            try
            {
                (List<MemberAddress> addresses, int totalPage) = MemberAddressService.GetSortedAddress(getSortedAddressDto);

                if (addresses == null)
                {
                    string errorMessage = "沒有地址";
                    return Json(new { errorMessage });
                }
                else
                {
                    return Json((new { addresses, totalPage}), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                string errorMessage = "發生錯誤，請再試一次";
                logger.Error(e);
                return Json(new { errorMessage });
                throw e;
            }
        }
    }
}
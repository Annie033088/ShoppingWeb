using NLog;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Models.Dto.AddressDto;
using Pashamao.Service;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    [UserKickOutFilter]
    [UserRoleAuthFilter(UserPermission.CreateMember | UserPermission.SelectMember | UserPermission.EditMemberPersonalData | UserPermission.EditMemberLevelAndStatus)]
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
        [HttpPost]
        public ActionResult GetSortedAddress(RequestGetSortedAddressDto getSortedAddressDto)
        {
            try
            {
                ErrorCodeDefine errorCode = 0;

                (List<MemberAddress> addresses, int totalPage) = MemberAddressService.GetSortedAddress(getSortedAddressDto);

                if (addresses == null)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json((new { addresses, totalPage, errorCode }));
                }
            }
            catch (Exception e)
            {
                ErrorCodeDefine errorCode = ErrorCodeDefine.ServerError;
                logger.Error(e);
                return Json(new { errorCode });
                throw e;
            }
        }
    }
}
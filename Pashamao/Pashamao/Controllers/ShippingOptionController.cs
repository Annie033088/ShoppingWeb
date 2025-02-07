using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using Pashamao.Filters;
using Pashamao.Models.Dto.OrderDto;
using Pashamao.Models;
using Pashamao.Service;

namespace Pashamao.Controllers
{
    [RequestLoggerFilter]
    [UserKickOutFilter]
    [UserRoleAuthFilter(UserPermission.EditShippingFee)]
    public class ShippingOptionController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private MainOrderService mainOrderService;

        public ShippingOptionController()
        {
            mainOrderService = new MainOrderService();
        }

        /// <summary>
        /// 跳轉運輸方式/運費設定頁面
        /// </summary>
        public ActionResult GetRedirectShippingOptionView()
        {
            try
            {
                return View("ShippingOption");
            }
            catch (Exception e)
            {
                ErrorCodeDefine errorCode = ErrorCodeDefine.ServerError;
                logger.Error(e);
                return Json(new { errorCode });
                throw e;
            }
        }

        /// <summary>
        /// 取得運費
        /// </summary>
        [HttpPost]
        public ActionResult GetShippingOption()
        {
            try
            {
                ErrorCodeDefine errorCode = 0;
                List<ShippingOption> shippingOptions = mainOrderService.GetShippingOption();

                if (shippingOptions == null)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json((new { shippingOptions, errorCode }));
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

        /// <summary>
        /// 修改運費
        /// </summary>
        [HttpPost]
        public ActionResult EditShippingOption(RequestEditShippingOptionDto editShippingOptionDto)
        {
            try
            {
                ErrorCodeDefine errorCode = 0;
                //檢查前端資料
                if (!ModelState.IsValid)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                if (editShippingOptionDto.ShippingFee < 0 || editShippingOptionDto.FreeShipping < 0 || editShippingOptionDto.ShippingOptionId < 0)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                bool successFlag = mainOrderService.EditShippingOption(editShippingOptionDto);

                if (successFlag)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.ModifiedFailed;
                    return Json((new { errorCode }));
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
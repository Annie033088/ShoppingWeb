using NLog;
using Pashamao.Filters;
using Pashamao.Models.Dto.ProductDto;
using Pashamao.Models;
using Pashamao.Service;
using System.Collections.Generic;
using System;
using System.Web.Mvc;
using Pashamao.Models.Dto.OrderDto;
using System.Linq;
using System.Security.Policy;
using System.Web.UI;
using Newtonsoft.Json;

namespace Pashamao.Controllers
{
    [RequestLoggerFilter]
    public class MainOrderController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private MainOrderService mainOrderService;

        public MainOrderController()
        {
            mainOrderService = new MainOrderService();
        }

        public ActionResult Index()
        {
            try
            {
                return View();
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
        /// 取得(搜尋)訂單
        /// </summary>
        [HttpPost]
        public ActionResult GetOrder(RequestGetSelectOrderDto selectOrderDto)
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

                if (selectOrderDto.OrderNumber != null)
                {
                    if (selectOrderDto.OrderNumber<0)
                    {
                        errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                        return Json(new { errorCode });
                    }
                }

                if (selectOrderDto.Phone != null)
                {
                    string phoneStr = selectOrderDto.Phone.ToString();
                    if (selectOrderDto.Phone<0 || phoneStr.Length != 3)
                    {
                        errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                        return Json(new { errorCode });
                    }
                }

                if (selectOrderDto.Status != null)
                {
                    if (selectOrderDto.Status > 8 || selectOrderDto.Status < 1) //目前只有 1~8狀態
                    {
                        errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                        return Json(new { errorCode });
                    }
                }

                //呼叫服務
                (List<Order> orders, int totalPage) = mainOrderService.GetOrder(selectOrderDto);

                if (orders == null)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    List<ResponseMainOrderDto> mainOrders = orders.Select(order => new ResponseMainOrderDto(order)).ToList();
                    errorCode = ErrorCodeDefine.Success;
                    return Json((new { orders = mainOrders, totalPage, errorCode }));
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
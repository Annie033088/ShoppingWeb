using MockPashamao.Models;
using MockPashamao.Models.Dto.OrderDto;
using MockPashamao.Service;
using System;
using System.Web.Mvc;

namespace MockPashamao.Controllers
{
    public class OrderController : Controller
    {
        private OrderService orderService;

        public OrderController()
        {
            orderService = new OrderService();
        }

        /// <summary>
        /// 創建訂單頁面
        /// </summary>
        public ActionResult GetCreateOrderView()
        {
            try
            {
                return View("CreateOrder");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 提交創建訂單
        /// </summary>
        [HttpPost]
        public ActionResult CreateOrder(RequestCreateOrderDto createOrderDto)
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

                foreach (var createOrderProductDto in createOrderDto.createOrderProductDtos)
                {
                    if (createOrderProductDto.ProductStyleId < 0)
                    {
                        errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                        return Json(new { errorCode });
                    }
                }

                bool successFlag = orderService.CreateOrder(createOrderDto);

                if (successFlag)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.CreateFailed;
                    return Json(new { errorCode });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
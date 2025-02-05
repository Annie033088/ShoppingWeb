using NLog;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Models.Dto.OrderDto;
using Pashamao.Service;
using Pashamao.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    [RequestLoggerFilter]
    [UserKickOutFilter]
    [UserRoleAuthFilter(UserPermission.SelectOrder | UserPermission.EditOrder | UserPermission.DelOrder)]
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
                    if (selectOrderDto.OrderNumber < 0)
                    {
                        errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                        return Json(new { errorCode });
                    }
                }

                if (selectOrderDto.MemberId != null)
                {
                    if (selectOrderDto.MemberId < 0)
                    {
                        errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                        return Json(new { errorCode });
                    }
                }

                if (selectOrderDto.Phone != null)
                {
                    string phoneStr = selectOrderDto.Phone.ToString();
                    if (selectOrderDto.Phone < 0 || phoneStr.Length != 3)
                    {
                        errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                        return Json(new { errorCode });
                    }
                }

                if (selectOrderDto.Status != null)
                {
                    if (selectOrderDto.Status > 10 || selectOrderDto.Status < 1) //目前只有 1~9狀態
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

        /// <summary>
        /// 修改訂單狀態
        /// </summary>
        [HttpPost]
        [UserRoleAuthFilter(UserPermission.EditOrder)]
        public ActionResult EditOrderState(RequestEditOrderStateDto editOrderStateDto)
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

                //判斷訂單狀態是否符合轉換規則
                ValidStateTransition validStateTransition = new ValidStateTransition();

                if (!validStateTransition.IsValidStateTransition(editOrderStateDto.OriginalState, editOrderStateDto.SelectedState))
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                bool successFlag = mainOrderService.EditOrderState(editOrderStateDto);

                if (successFlag)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.ModifiedFailed;
                    return Json(new { errorCode });
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
        /// 跳轉訂單詳情頁面
        /// </summary>
        public ActionResult GetRedirectOrderDetailView(int orderId)
        {
            try
            {
                ViewBag.OrderId = orderId;
                return View("OrderDetail");
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
        /// 請求訂單內頁資料
        /// </summary>
        [HttpPost]
        public ActionResult GetOrderDetail(int orderId)
        {
            try
            {
                ErrorCodeDefine errorCode = 0;
                //檢查前端資料
                if (orderId < 1)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                (Order order, List<OrderItem> orderItems, List<OrderState> orderStates) = mainOrderService.GetOrderDetail(orderId);

                ResponseOrderDetailDto orderDetail = new ResponseOrderDetailDto
                {
                    order = new ResponseSubOrderDto(order),
                    orderItemDtos = orderItems.Select(item => (new ResponseSelectOrderItemDto(item))).ToList(),
                    orderStateDtos = orderStates.Select(state => (new ResponseSelectOrderStateDto(state))).ToList()
                };

                errorCode = ErrorCodeDefine.Success;
                return Json(new { orderDetail, errorCode });
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
        /// 修改訂單備註
        /// </summary>
        [HttpPost]
        [UserRoleAuthFilter(UserPermission.EditOrder)]
        public ActionResult EditOrderRemark(RequestEditOrderRemarkDto editOrderRemarkDto)
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

                if (editOrderRemarkDto.OrderId < 0)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                if (editOrderRemarkDto.Remark == null)
                {
                    editOrderRemarkDto.Remark = string.Empty;
                }

                bool successFlag = mainOrderService.EditOrderRemark(editOrderRemarkDto);

                if (successFlag)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.ModifiedFailed;
                    return Json(new { errorCode });
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
        /// 修改物流編號
        /// </summary>
        [HttpPost]
        [UserRoleAuthFilter(UserPermission.EditOrder)]
        public ActionResult EditLogisticsNumber(RequestEditLogisticsNumberDto editLogisticsNumberDto)
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

                if (editLogisticsNumberDto.OrderId < 0)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                if (editLogisticsNumberDto.LogisticsNumber == null) editLogisticsNumberDto.LogisticsNumber = string.Empty;

                bool successFlag = mainOrderService.EditLogisticsNumber(editLogisticsNumberDto);

                if (successFlag)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.ModifiedFailed;
                    return Json(new { errorCode });
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
        /// 修改訂單狀態備註
        /// </summary>
        [HttpPost]
        [UserRoleAuthFilter(UserPermission.EditOrder)]
        public ActionResult EditOrderStateRemark(RequestEditOrderStateRemarkDto editOrderStateRemarkDto)
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

                if (editOrderStateRemarkDto.OrderStateId < 0)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                if (editOrderStateRemarkDto.Remark == null)
                {
                    editOrderStateRemarkDto.Remark = string.Empty;
                }

                bool successFlag = mainOrderService.EditOrderStateRemark(editOrderStateRemarkDto);

                if (successFlag)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.ModifiedFailed;
                    return Json(new { errorCode });
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
        /// 刪除訂單
        /// </summary>
        public ActionResult DeleteOrder(RequestOrderIdDto orderIdDto)
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

                if (orderIdDto.OrderId < 0)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                bool successFlag = mainOrderService.DeleteOrder(orderIdDto);

                if (successFlag)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.ModifiedFailed;
                    return Json(new { errorCode });
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
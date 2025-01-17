using NLog;
using Pashamao.Models;
using Pashamao.Models.Dto.OrderDto;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Service
{
    public class MainOrderService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private OrderRepository orderRepository;

        public MainOrderService()
        {
            orderRepository = new OrderRepository();
        }

        /// <summary>
        /// 取得(搜尋)訂單
        /// </summary>
        public (List<Order> orders, int totalPage) GetOrder(RequestGetSelectOrderDto selectOrderDto)
        {
            try
            {
                return orderRepository.GetOrder(selectOrderDto);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 取得訂單內頁
        /// </summary>
        public (Order order, List<OrderItem> orderItems, List<OrderState> orderStates) GetOrderDetail(int orderId)
        {
            try
            {
                return orderRepository.GetOrderDetail(orderId);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 修改訂單狀態
        /// </summary>
        public bool EditOrderState(RequestEditOrderStateDto editOrderStateDto)
        {
            try
            {
                return orderRepository.EditOrderState(editOrderStateDto);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 修改訂單備註
        /// </summary>
        public bool EditOrderRemark(RequestEditOrderRemarkDto editOrderRemarkDto)
        {
            try
            {
                return orderRepository.EditOrderRemark(editOrderRemarkDto);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 修改訂單狀態備註
        /// </summary>
        /// <param name="editOrderStateRemarkDto"></param>
        /// <returns></returns>
        public bool EditOrderStateRemark(RequestEditOrderStateRemarkDto editOrderStateRemarkDto)
        {
            try
            {
                return orderRepository.EditOrderStateRemark(editOrderStateRemarkDto);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 取得所有運輸方式
        /// </summary>
        public List<ShippingOption> GetShippingOption()
        {
            try
            {
                return orderRepository.GetShippingOption();
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 修改運輸價格
        /// </summary>
        public bool EditShippingOption(RequestEditShippingOptionDto editShippingOptionDto)
        {
            try
            {
                return orderRepository.EditShippingOption(editShippingOptionDto);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }
    }
}
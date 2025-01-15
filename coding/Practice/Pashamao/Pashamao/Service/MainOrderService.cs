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
            return orderRepository.GetOrder(selectOrderDto);
        }

        /// <summary>
        /// 取得所有運輸方式
        /// </summary>
        public List<ShippingOption> GetShippingOption()
        {
            return orderRepository.GetShippingOption();
        }

        /// <summary>
        /// 修改運輸價格
        /// </summary>
        public bool EditShippingOption(RequestEditShippingOptionDto editShippingOptionDto)
        {
            return orderRepository.EditShippingOption(editShippingOptionDto);
        }
    }
}
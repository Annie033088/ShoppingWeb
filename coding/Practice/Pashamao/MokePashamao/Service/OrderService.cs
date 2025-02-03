using MockPashamao.Models.Dto.OrderDto;
using MockPashamao.Repositories;
using System;

namespace MockPashamao.Service
{
    public class OrderService
    {
        private OrderRepository orderRepository;

        public OrderService()
        {
            orderRepository = new OrderRepository();
        }

        /// <summary>
        /// 創建訂單
        /// </summary>
        public bool CreateOrder(RequestCreateOrderDto createOrderDto)
        {
            try
            {
                string date = DateTime.Now.ToString("yyMMdd");
                string second = ((int)(DateTime.Now - DateTime.Today).TotalSeconds).ToString("D5"); //取得午夜至現在過的秒數並轉成5位

                string orderNumberStr = date + second + createOrderDto.MemberId.ToString("D7");
                long orderNumberLong = long.Parse(orderNumberStr);

                return orderRepository.CreateOrder(createOrderDto, orderNumberLong);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool EditOrderState(RequestEditOrderStateDto editOrderStateDto)
        {
            return orderRepository.EditOrderState(editOrderStateDto);
        }
    }
}
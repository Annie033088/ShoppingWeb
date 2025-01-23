using MockPashamao.Models;
using MockPashamao.Models.Dto.OrderDto;
using MockPashamao.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MockPashamao.Controllers
{
    public class EditOrderStateApiController : ApiController
    {
        private OrderService orderService;

        public EditOrderStateApiController()
        {
            orderService = new OrderService();
        }

        // 模擬資料庫：將狀態存儲在內存中的 List 中
        private static List<RequestEditOrderStateDto> editOrderStates = new List<RequestEditOrderStateDto>
        {
            new RequestEditOrderStateDto { OrderId = 12, State = OrderStateEnum.PackageArrive }
        };

        public IHttpActionResult Get()
        {
            return Ok("1");
        }

        // POST api/values
        public IHttpActionResult Post()
        {
            if (editOrderStates == null)
            {
                return BadRequest("Invalid product data.");
            }

            bool successFlag = orderService.EditOrderState(editOrderStates);

            if (successFlag)
            {
                return CreatedAtRoute("DefaultApi", new { id = editOrderStates[0].OrderId }, editOrderStates); // 返回創建的產品
            }
            else
            {
                return BadRequest("Invalid state data.");
            }
        }
    }
}

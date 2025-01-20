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

        // 模擬資料庫：將產品存儲在內存中的 List 中
        private static List<RequestEditOrderStateDto> editOrderStateDtos = new List<RequestEditOrderStateDto>
        {
            new RequestEditOrderStateDto { OrderId = 28, State = OrderStateEnum.PackageArrive },
            new RequestEditOrderStateDto { OrderId = 29, State = OrderStateEnum.NotPickedUp },
            new RequestEditOrderStateDto { OrderId = 30, State = OrderStateEnum.Finish },
            new RequestEditOrderStateDto { OrderId = 31, State = OrderStateEnum.ApplyForReturn }
        };

        public IHttpActionResult Get()
        {
            return Ok("1");
        }

        // POST api/values
        public IHttpActionResult Post([FromBody] RequestEditOrderStateDto editOrderStateDto)
        {
            if (editOrderStateDto == null)
            {
                return BadRequest("Invalid product data.");
            }

            bool successFlag = orderService.EditOrderState(editOrderStateDto);

            if (successFlag)
            {
                return CreatedAtRoute("DefaultApi", new { id = editOrderStateDto.OrderId }, editOrderStateDto); // 返回創建的產品
            }
            else
            {
                return BadRequest("Invalid product data.");
            }
        }
    }
}

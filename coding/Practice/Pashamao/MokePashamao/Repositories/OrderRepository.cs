
using MockPashamao.Models.Dto.OrderDto;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MockPashamao.Repositories
{
    public class OrderRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 新增訂單
        /// </summary>
        internal bool CreateOrder(RequestCreateOrderDto createOrderDto, long orderNumber)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_addOrder @orderNumber, @memberId, @name, @address, @phone, @shippingOptionId, @orderProductStyles";

                cmd.Parameters.Add("@orderNumber", SqlDbType.BigInt).Value = orderNumber;
                cmd.Parameters.Add("@memberId", SqlDbType.Int).Value = createOrderDto.MemberId;
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = createOrderDto.Name;
                cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = createOrderDto.PostalCode + createOrderDto.Address;
                cmd.Parameters.Add("@phone", SqlDbType.Int).Value = createOrderDto.Phone;
                cmd.Parameters.Add("@shippingOptionId", SqlDbType.Int).Value = createOrderDto.ShippingOptionId;

                DataTable OrderProductStyle = new DataTable();
                OrderProductStyle.Columns.Add("f_productStyleId", typeof(int));
                OrderProductStyle.Columns.Add("f_productId", typeof(Guid));
                OrderProductStyle.Columns.Add("f_quantity", typeof(int));

                foreach (RequestCreateOrderProductDto product in createOrderDto.createOrderProductDtos)
                {
                    OrderProductStyle.Rows.Add(product.ProductStyleId, product.ProductId, product.Quantity);
                }

                var stylesParam = new SqlParameter("@orderProductStyles", SqlDbType.Structured)
                {
                    TypeName = "dbo.type_pashamao_addOrderProductStyle",
                    Value = OrderProductStyle
                };
                cmd.Parameters.Add(stylesParam);

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                //受影響筆數>0代表成功
                if (ExeCnt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// 模擬訂單狀態
        /// </summary>
        public bool EditOrderState(RequestEditOrderStateDto editOrderStateDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editOrderStateMock @orderId, @logisticsNumber, @state";

                if (editOrderStateDto.OrderId == null)
                {
                    cmd.Parameters.Add("@OrderId", SqlDbType.Int).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@OrderId", SqlDbType.Int).Value = editOrderStateDto.OrderId;
                }

                if (editOrderStateDto.LogisticsNumber == null)
                {
                    cmd.Parameters.Add("@logisticsNumber", SqlDbType.VarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@logisticsNumber", SqlDbType.VarChar).Value = editOrderStateDto.LogisticsNumber;
                }

                cmd.Parameters.Add("@state", SqlDbType.TinyInt).Value = (int)editOrderStateDto.State;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                if (ExeCnt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Connection.Close();
            }
        }
    }
}
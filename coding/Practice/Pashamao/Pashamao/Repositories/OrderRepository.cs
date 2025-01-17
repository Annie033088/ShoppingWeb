using Pashamao.Models.Dto.OrderDto;
using Pashamao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using Pashamao.Models.Dto.ProductDto;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Optimization;

namespace Pashamao.Repositories
{
    public class OrderRepository
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 取得(搜尋)訂單
        /// </summary>
        public (List<Order> orders, int totalPage) GetOrder(RequestGetSelectOrderDto selectOrderDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<Order> orders = new List<Order>();
            int totalPages = 0;

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getOrder @orderNumber, @phone, @memberId, @dateStart, @dateEnd, @status, @page, @totalPages OUTPUT";

                if (selectOrderDto.OrderNumber == null)
                {
                    cmd.Parameters.Add("@orderNumber", SqlDbType.BigInt).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@orderNumber", SqlDbType.BigInt).Value = selectOrderDto.OrderNumber.Value;
                }

                if (selectOrderDto.Phone == null)
                {
                    cmd.Parameters.Add("@phone", SqlDbType.Int).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@phone", SqlDbType.Int).Value = selectOrderDto.Phone;
                }

                if (selectOrderDto.MemberId == null)
                {
                    cmd.Parameters.Add("@memberId", SqlDbType.Int).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@memberId", SqlDbType.Int).Value = selectOrderDto.MemberId;
                }

                if (selectOrderDto.StartDate == null)
                {
                    cmd.Parameters.Add("@dateStart", SqlDbType.DateTime).Value = DBNull.Value;
                    cmd.Parameters.Add("@dateEnd", SqlDbType.DateTime).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@dateStart", SqlDbType.DateTime).Value = selectOrderDto.StartDate;
                    cmd.Parameters.Add("@dateEnd", SqlDbType.DateTime).Value = selectOrderDto.EndDate;
                }

                if (selectOrderDto.Status == null)
                {
                    cmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = selectOrderDto.Status;
                }

                cmd.Parameters.Add("@page", SqlDbType.Int).Value = selectOrderDto.Page;
                SqlParameter totalPagesOutput = new SqlParameter("@totalPages", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(totalPagesOutput);

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);
                totalPages = (int)totalPagesOutput.Value;

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Order order = new Order();
                        order.OrderId = dt.Rows[i].IsNull("f_orderId") ? 0 : dt.Rows[i].Field<int>("f_orderId");
                        order.OrderNumber = dt.Rows[i].IsNull("f_orderNumber") ? 0 : dt.Rows[i].Field<long>("f_orderNumber");
                        order.MemberId = dt.Rows[i].IsNull("f_memberId") ? 0 : dt.Rows[i].Field<int>("f_memberId");
                        order.RecipientName = dt.Rows[i].IsNull("f_recipientName") ? string.Empty : dt.Rows[i].Field<string>("f_recipientName");
                        order.Phone = dt.Rows[i].IsNull("f_phone") ? 0 : dt.Rows[i].Field<int>("f_phone");
                        order.State = dt.Rows[i].IsNull("f_state") ? 0 : dt.Rows[i].Field<byte>("f_state");
                        order.TotalAmount = dt.Rows[i].IsNull("f_totalAmount") ? 0 : dt.Rows[i].Field<decimal>("f_totalAmount");
                        order.CreateTime = dt.Rows[i].IsNull("f_createTime") ? DateTime.MinValue : dt.Rows[i].Field<DateTime>("f_createTime");
                        order.UpdateTime = dt.Rows[i].IsNull("f_updateTime") ? DateTime.MinValue : dt.Rows[i].Field<DateTime>("f_updateTime");
                        orders.Add(order);
                    }

                    return (orders, totalPages);
                }
                else
                {
                    return (null, 0);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
            finally
            {
                cmd.Parameters.Clear();

                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
        }

        /// <summary>
        /// 取得訂單內容
        /// </summary>
        public (Order order, List<OrderItem> orderItems, List<OrderState> orderStates) GetOrderDetail(int orderId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            Order order = new Order();
            List<OrderItem> orderItems = new List<OrderItem>();
            List<OrderState> orderStates = new List<OrderState>();

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getOrderDetail @orderId";
                cmd.Parameters.Add("@orderId", SqlDbType.Int).Value = orderId;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Connection.Close();

                if (ds.Tables.Count > 0)
                {
                    //取得訂單資料
                    order.OrderId = orderId;
                    order.MemberId = ds.Tables[0].Rows[0].IsNull("f_memberId") ? 0 : ds.Tables[0].Rows[0].Field<int>("f_memberId");
                    order.OrderNumber = ds.Tables[0].Rows[0].IsNull("f_orderNumber") ? 0 : ds.Tables[0].Rows[0].Field<long>("f_orderNumber");
                    order.State = ds.Tables[0].Rows[0].IsNull("f_state") ? 0 : ds.Tables[0].Rows[0].Field<byte>("f_state");
                    order.RecipientName = ds.Tables[0].Rows[0].IsNull("f_recipientName") ? string.Empty : ds.Tables[0].Rows[0].Field<string>("f_recipientName");
                    order.Phone = ds.Tables[0].Rows[0].IsNull("f_phone") ? 0 : ds.Tables[0].Rows[0].Field<int>("f_phone");
                    order.Address = ds.Tables[0].Rows[0].IsNull("f_address") ? string.Empty : ds.Tables[0].Rows[0].Field<string>("f_address");
                    order.ShippingOption = ds.Tables[0].Rows[0].IsNull("f_shippingOption") ? string.Empty : ds.Tables[0].Rows[0].Field<string>("f_shippingOption");
                    order.ShippingFee = ds.Tables[0].Rows[0].IsNull("f_shippingFee") ? 0 : ds.Tables[0].Rows[0].Field<decimal>("f_shippingFee");
                    order.OriginalAmount = ds.Tables[0].Rows[0].IsNull("f_originalAmount") ? 0 : ds.Tables[0].Rows[0].Field<decimal>("f_originalAmount");
                    order.DiscountedAmount = ds.Tables[0].Rows[0].IsNull("f_discountedAmount") ? 0 : ds.Tables[0].Rows[0].Field<decimal>("f_discountedAmount");
                    order.TotalAmount = ds.Tables[0].Rows[0].IsNull("f_totalAmount") ? 0 : ds.Tables[0].Rows[0].Field<decimal>("f_totalAmount");
                    order.Remark = ds.Tables[0].Rows[0].IsNull("f_remark") ? string.Empty : ds.Tables[0].Rows[0].Field<string>("f_remark");
                    order.CreateTime = ds.Tables[0].Rows[0].IsNull("f_createTime") ? DateTime.MinValue : ds.Tables[0].Rows[0].Field<DateTime>("f_createTime");
                    order.UpdateTime = ds.Tables[0].Rows[0].IsNull("f_updateTime") ? DateTime.MinValue : ds.Tables[0].Rows[0].Field<DateTime>("f_updateTime");

                    //取得訂單商品
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        OrderItem orderItem = new OrderItem();
                        orderItem.ProductName = ds.Tables[1].Rows[i].IsNull("f_productName") ? string.Empty : ds.Tables[1].Rows[i].Field<string>("f_productName");
                        orderItem.Style = ds.Tables[1].Rows[i].IsNull("f_style") ? string.Empty : ds.Tables[1].Rows[i].Field<string>("f_style");
                        orderItem.Quantity = ds.Tables[1].Rows[i].IsNull("f_quantity") ? 0 : ds.Tables[1].Rows[i].Field<int>("f_quantity");
                        orderItem.Price = ds.Tables[1].Rows[i].IsNull("f_price") ? 0 : ds.Tables[1].Rows[i].Field<decimal>("f_price");

                        orderItems.Add(orderItem);
                    }

                    //取得訂單狀態
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        OrderState orderState = new OrderState();
                        orderState.OrderStateId = ds.Tables[2].Rows[i].IsNull("f_orderStateId") ? 0 : ds.Tables[2].Rows[i].Field<int>("f_orderStateId");
                        orderState.State = ds.Tables[2].Rows[i].IsNull("f_state") ? 0 : ds.Tables[2].Rows[i].Field<byte>("f_state");
                        orderState.Remark = ds.Tables[2].Rows[i].IsNull("f_remark") ? string.Empty : ds.Tables[2].Rows[i].Field<string>("f_remark");
                        orderState.CreateTime = ds.Tables[2].Rows[i].IsNull("f_createTime") ? DateTime.MinValue : ds.Tables[2].Rows[i].Field<DateTime>("f_createTime");
                        orderState.UpdateTime = ds.Tables[2].Rows[i].IsNull("f_updateTime") ? DateTime.MinValue : ds.Tables[2].Rows[i].Field<DateTime>("f_updateTime");

                        orderStates.Add(orderState);
                    }

                    return (order, orderItems, orderStates);
                }
                else
                {
                    return (null, null, null);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
            finally
            {
                cmd.Parameters.Clear();

                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
        }

        /// <summary>
        /// 修改訂單狀態
        /// </summary>
        public bool EditOrderState(RequestEditOrderStateDto editOrderStateDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editOrderState @orderId, @originalState, @selectedState, @updateTime";
                cmd.Parameters.Add("@orderId", SqlDbType.Int).Value = editOrderStateDto.OrderId;
                cmd.Parameters.Add("@originalState", SqlDbType.TinyInt).Value = editOrderStateDto.OriginalState;
                cmd.Parameters.Add("@selectedState", SqlDbType.TinyInt).Value = editOrderStateDto.SelectedState;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime).Value = editOrderStateDto.UpdateTime;

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
                logger.Error(e);
                throw e;
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// 修改訂單備註
        /// </summary>
        public bool EditOrderRemark(RequestEditOrderRemarkDto editOrderRemarkDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editOrderRemark @orderId, @remark";
                cmd.Parameters.Add("@orderId", SqlDbType.Int).Value = editOrderRemarkDto.OrderId;
                cmd.Parameters.Add("@remark", SqlDbType.NVarChar).Value = editOrderRemarkDto.Remark;

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
                logger.Error(e);
                throw e;
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// 修改訂單狀態備註
        /// </summary>
        public bool EditOrderStateRemark(RequestEditOrderStateRemarkDto editOrderStateRemarkDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editOrderStateRemark @orderStateId, @remark";
                cmd.Parameters.Add("@orderStateId", SqlDbType.Int).Value = editOrderStateRemarkDto.OrderStateId;
                cmd.Parameters.Add("@remark", SqlDbType.NVarChar).Value = editOrderStateRemarkDto.Remark;

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
                logger.Error(e);
                throw e;
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// 取得所有運輸方式
        /// </summary>
        public List<ShippingOption> GetShippingOption()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<ShippingOption> shippingOptions = new List<ShippingOption>();

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getShippingOption";

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ShippingOption shippingOption = new ShippingOption();
                        shippingOption.ShippingOptionId = dt.Rows[i].IsNull("f_shippingOptionId") ? 0 : dt.Rows[i].Field<int>("f_shippingOptionId");
                        shippingOption.Option = dt.Rows[i].IsNull("f_option") ? string.Empty : dt.Rows[i].Field<string>("f_option");
                        shippingOption.ShippingFee = dt.Rows[i].IsNull("f_shippingFee") ? 0 : dt.Rows[i].Field<decimal>("f_shippingFee");
                        shippingOption.FreeShipping = dt.Rows[i].IsNull("f_freeShipping") ? 0 : dt.Rows[i].Field<decimal>("f_freeShipping");
                        shippingOption.UpdateTime = dt.Rows[i].IsNull("f_updateTime") ? DateTime.MinValue : dt.Rows[i].Field<DateTime>("f_updateTime");
                        shippingOptions.Add(shippingOption);
                    }

                    return shippingOptions;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
            finally
            {
                cmd.Parameters.Clear();

                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
        }

        /// <summary>
        /// 修改運輸價格
        /// </summary>
        internal bool EditShippingOption(RequestEditShippingOptionDto editShippingOptionDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editShippingOption @shippingOptionId, @shippingFee, @freeShipping, @updateTime";
                cmd.Parameters.Add("@shippingOptionId", SqlDbType.Int).Value = editShippingOptionDto.ShippingOptionId;
                cmd.Parameters.Add("@shippingFee", SqlDbType.Decimal).Value = editShippingOptionDto.ShippingFee;
                cmd.Parameters.Add("@freeShipping", SqlDbType.Decimal).Value = editShippingOptionDto.FreeShipping;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime).Value = editShippingOptionDto.UpdateTime;

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
                logger.Error(e);
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
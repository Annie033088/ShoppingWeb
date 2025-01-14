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
                cmd.CommandText = "EXEC pro_pashamao_getOrder @orderNumber, @phone, @dateStart, @dateEnd, @status, @page, @totalPages OUTPUT";

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
                        order.CreateTime = dt.Rows[i].IsNull("f_createTime") ? DateTime.MinValue : dt.Rows[i].Field<DateTime>("f_createTime");
                        order.MemberId = dt.Rows[i].IsNull("f_memberId") ? 0 : dt.Rows[i].Field<int>("f_memberId");
                        order.RecipientName = dt.Rows[i].IsNull("f_recipientName") ? string.Empty : dt.Rows[i].Field<string>("f_recipientName");
                        order.Phone = dt.Rows[i].IsNull("f_phone") ? 0 : dt.Rows[i].Field<int>("f_phone");
                        order.State = dt.Rows[i].IsNull("f_state") ? 0 : dt.Rows[i].Field<byte>("f_state");
                        order.TotalPrice = dt.Rows[i].IsNull("f_totalPrice") ? 0 : dt.Rows[i].Field<decimal>("f_totalPrice");
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


    }
}
using NLog;
using Pashamao.Models.Dto.ImitateApiDto;
using Pashamao.Models.Dto.MemberDto;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Pashamao.Repositories
{
    public class ImitateApiRepository
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 新增會員
        /// </summary>
        internal bool CreateMember(RequestCreateMemberDto member)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_addMember @acct, @pwd, @email, @phone, @memberName, @nickname";

                cmd.Parameters.Add("@acct", SqlDbType.VarChar).Value = member.Account;
                cmd.Parameters.Add("@pwd", SqlDbType.VarChar).Value = member.Pwd;
                cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = member.Email;
                cmd.Parameters.Add("@memberName", SqlDbType.NVarChar).Value = member.MemberName;
                cmd.Parameters.Add("@nickname", SqlDbType.NVarChar).Value = member.Nickname;

                if (member.Phone == null)
                {
                    cmd.Parameters.Add("@phone", SqlDbType.Int).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@phone", SqlDbType.Int).Value = member.Phone.Value;
                }

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                //受影響筆數為1代表成功
                if (ExeCnt == 1)
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
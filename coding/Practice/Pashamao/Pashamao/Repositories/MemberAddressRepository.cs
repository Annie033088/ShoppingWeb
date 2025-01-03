using NLog;
using Pashamao.Models;
using Pashamao.Models.Dto.AddressDto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Pashamao.Repositories
{
    public class MemberAddressRepository
    {

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 排序地址並傳回
        /// </summary>
        internal (List<MemberAddress> addresses, int totalPage) GetSortedAddress(RequestGetSortedAddressDto getSortedAddressDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<MemberAddress> addresses = new List<MemberAddress>();
            int totalPages = 0;
            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getSortedMemberAddress @column, @page, @sortOrder, @totalPages OUTPUT";

                cmd.Parameters.Add("@column", SqlDbType.VarChar).Value = getSortedAddressDto.SortColumn;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getSortedAddressDto.Page;
                cmd.Parameters.Add("@sortOrder", SqlDbType.VarChar).Value = getSortedAddressDto.SortOrder;
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
                        MemberAddress address = new MemberAddress();
                        address.AddressId = dt.Rows[i].IsNull("f_addressId") ? 0 : dt.Rows[i].Field<int>("f_addressId");
                        address.MemberId = dt.Rows[i].IsNull("f_memberId") ? 0 : dt.Rows[i].Field<int>("f_memberId");
                        address.City = dt.Rows[i].IsNull("f_city") ? string.Empty : dt.Rows[i].Field<string>("f_city");
                        address.District = dt.Rows[i].IsNull("f_district") ? string.Empty : dt.Rows[i].Field<string>("f_district");
                        address.Detail = dt.Rows[i].IsNull("f_detail") ? string.Empty : dt.Rows[i].Field<string>("f_detail");
                        address.PostalCode = dt.Rows[i].IsNull("f_postalCode") ? string.Empty : dt.Rows[i].Field<string>("f_postalCode");
                        addresses.Add(address);
                    }
                    return (addresses, totalPages);
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
                //判斷是否已關閉
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
        }
    }
}
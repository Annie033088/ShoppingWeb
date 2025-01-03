using NLog;
using Pashamao.Models;
using Pashamao.Models.Dto.MemberDto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Pashamao.Repositories
{
    public class MemberRepository
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 新增會員
        /// </summary>
        internal bool Create(Member member)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_addMember @acct, @pwd, @email, @phone, @memberName, @nickname";

                cmd.Parameters.Add("@acct", SqlDbType.VarChar).Value = member.Account;
                cmd.Parameters.Add("@pwd", SqlDbType.VarChar).Value = member.Pwd;
                cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = member.Email;
                cmd.Parameters.Add("@phone", SqlDbType.Char).Value = member.Phone;
                cmd.Parameters.Add("@memberName", SqlDbType.NVarChar).Value = member.MemberName;
                cmd.Parameters.Add("@nickname", SqlDbType.NVarChar).Value = member.Nickname;

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
        /// 排序會員並傳回
        /// </summary>
        internal (List<Member> members, int totalPage) GetSortedMember(RequestGetSortedMemberDto getSortedMemberDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<Member> members = new List<Member>();
            int totalPages = 0;
            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getSortedMember @column, @page, @sortOrder, @totalPages OUTPUT";

                cmd.Parameters.Add("@column", SqlDbType.VarChar).Value = getSortedMemberDto.SortColumn;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getSortedMemberDto.Page;
                cmd.Parameters.Add("@sortOrder", SqlDbType.VarChar).Value = getSortedMemberDto.SortOrder;
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
                        Member member = new Member();
                        member.MemberId = dt.Rows[i].IsNull("f_memberId") ? 0 : dt.Rows[i].Field<int>("f_memberId");
                        member.Email = dt.Rows[i].IsNull("f_email") ? string.Empty : dt.Rows[i].Field<string>("f_email");
                        member.Phone = dt.Rows[i].IsNull("f_phone") ? string.Empty : dt.Rows[i].Field<string>("f_phone");
                        member.MemberName = dt.Rows[i].IsNull("f_memberName") ? string.Empty : dt.Rows[i].Field<string>("f_memberName");
                        member.Status = dt.Rows[i].IsNull("f_status") ? false : dt.Rows[i].Field<bool>("f_status");
                        member.Level = dt.Rows[i].IsNull("f_level") ? 0 : dt.Rows[i].Field<byte>("f_level");
                        members.Add(member);
                    }
                    return (members, totalPages);
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

        /// <summary>
        /// 取得搜尋且排序後的會員資料
        /// </summary>
        internal (List<Member> members, int totalPage) GetSelectMember(RequestGetSelectMemberDto getSelectMemberDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<Member> members = new List<Member>();
            int totalPages = 0;
            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getSelectMember @selectColumn, @value, @sortColumn, @page, @sortOrder, @totalPages OUTPUT";

                cmd.Parameters.Add("@selectColumn", SqlDbType.VarChar).Value = getSelectMemberDto.SelectColumn;
                cmd.Parameters.Add("@value", SqlDbType.VarChar).Value = getSelectMemberDto.Value;
                cmd.Parameters.Add("@sortColumn", SqlDbType.VarChar).Value = getSelectMemberDto.SortColumn;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getSelectMemberDto.Page;
                cmd.Parameters.Add("@sortOrder", SqlDbType.VarChar).Value = getSelectMemberDto.SortOrder;
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
                        Member member = new Member();
                        member.MemberId = dt.Rows[i].IsNull("f_memberId") ? 0 : dt.Rows[i].Field<int>("f_memberId");
                        member.Email = dt.Rows[i].IsNull("f_email") ? string.Empty : dt.Rows[i].Field<string>("f_email");
                        member.Phone = dt.Rows[i].IsNull("f_phone") ? string.Empty : dt.Rows[i].Field<string>("f_phone");
                        member.MemberName = dt.Rows[i].IsNull("f_memberName") ? string.Empty : dt.Rows[i].Field<string>("f_memberName");
                        member.Status = dt.Rows[i].IsNull("f_status") ? false : dt.Rows[i].Field<bool>("f_status");
                        member.Level = dt.Rows[i].IsNull("f_level") ? 0 : dt.Rows[i].Field<byte>("f_level");
                        members.Add(member);
                    }
                    return (members, totalPages);
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

        /// <summary>
        /// 更改會員等級跟狀態
        /// </summary>
        internal bool EditMemberLevelAndStatus(Member member)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editMemberLevelAndStatus @memberId, @status, @points, @level";

                cmd.Parameters.Add("@memberId", SqlDbType.Int).Value = member.MemberId;
                cmd.Parameters.Add("@status", SqlDbType.Bit).Value = member.Status;
                cmd.Parameters.Add("@points", SqlDbType.Int).Value = member.Points;
                cmd.Parameters.Add("@level", SqlDbType.TinyInt).Value = member.Level;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                if (ExeCnt == 0)
                {
                    return false;
                }
                else
                {
                    return true;
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
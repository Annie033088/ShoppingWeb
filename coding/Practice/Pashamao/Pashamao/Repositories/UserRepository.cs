using NLog;
using Pashamao.Models;
using Pashamao.Models.Dto.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Pashamao.Repositories
{
    public class UserRepository
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 取得使用者登入資料
        /// </summary>
        internal (User, long) VerifyAndGetUser(string acct, string pwd, string sessionId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getLoginUser @acct, @hash, @sessionId";
                cmd.Parameters.Add("@acct", SqlDbType.VarChar).Value = acct;
                cmd.Parameters.Add("@hash", SqlDbType.VarChar).Value = pwd;
                cmd.Parameters.Add("@sessionId", SqlDbType.VarChar).Value = sessionId;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    User user = new User();
                    DataRow dr = dt.Rows[0];
                    user.UserId = dr.IsNull("f_userId") ? 0 : dr.Field<int>("f_userId");
                    user.Account = dr.IsNull("f_account") ? string.Empty : dr.Field<string>("f_account");
                    user.Name = dr.IsNull("f_name") ? string.Empty : dr.Field<string>("f_name");
                    user.Status = dr.IsNull("f_status") ? false : dr.Field<bool>("f_status");

                    long rolePermission = dr.IsNull("f_rolePermission") ? 0 : dr.Field<long>("f_rolePermission");
                    return (user, rolePermission);
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
        /// 取得 sessionId, status 跟權限
        /// </summary>
        internal (bool, string, long) GetAtEveryRequest(UserSessionModel userSession)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getAtEveryRequest @userId";
                cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userSession.UserId;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    bool Status = dt.Rows[0].IsNull("f_status") ? false : dt.Rows[0].Field<bool>("f_status");
                    string SessionId = dt.Rows[0].IsNull("f_sessionId") ? string.Empty : dt.Rows[0].Field<string>("f_sessionId");
                    long Permissions = dt.Rows[0].IsNull("f_rolePermission") ? 0 : dt.Rows[0].Field<long>("f_rolePermission");
                    return (Status, SessionId, Permissions);
                }

                return (false, null, 0);
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
        /// 排序搜尋之前的使用者資料並傳回
        /// </summary>
        internal (List<User> user, int totalPage) GetSortedUser(RequestGetSortedUserDto sortedUserDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<User> users = new List<User>();
            int totalPages = 0;
            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getSortedUser @column, @page, @sortOrder, @totalPages OUTPUT";

                cmd.Parameters.Add("@column", SqlDbType.VarChar).Value = sortedUserDto.SortColumn;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = sortedUserDto.Page;
                cmd.Parameters.Add("@sortOrder", SqlDbType.VarChar).Value = sortedUserDto.SortOrder;
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
                        User user = new User();
                        user.UserId = dt.Rows[i].IsNull("f_userId") ? 0 : dt.Rows[i].Field<int>("f_userId");
                        user.Account = dt.Rows[i].IsNull("f_account") ? string.Empty : dt.Rows[i].Field<string>("f_account");
                        user.Name = dt.Rows[i].IsNull("f_name") ? string.Empty : dt.Rows[i].Field<string>("f_name");
                        user.Status = dt.Rows[i].IsNull("f_status") ? false : dt.Rows[i].Field<bool>("f_status");
                        user.RoleId = dt.Rows[i].IsNull("f_roleId") ? 0 : dt.Rows[i].Field<byte>("f_roleId");
                        users.Add(user);
                    }
                    return (users, totalPages);
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
        /// 取得查詢的使用者
        /// </summary>
        internal (List<User>, int) GetSelectUser(string selectColumn, string value, string sortColumn, string page, string sortOrder)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<User> users = new List<User>();
            int totalPages = 0;

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getSelectUser @selectColumn, @value, @sortColumn, @page, @sortOrder, @totalPages OUTPUT";

                cmd.Parameters.Add("@selectColumn", SqlDbType.VarChar).Value = selectColumn;
                cmd.Parameters.Add("@value", SqlDbType.VarChar).Value = value;
                cmd.Parameters.Add("@sortColumn", SqlDbType.VarChar).Value = sortColumn;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = page;
                cmd.Parameters.Add("@sortOrder", SqlDbType.VarChar).Value = sortOrder;
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
                        User user = new User();
                        user.UserId = dt.Rows[i].IsNull("f_userId") ? 0 : dt.Rows[i].Field<int>("f_userId");
                        user.Account = dt.Rows[i].IsNull("f_account") ? string.Empty : dt.Rows[i].Field<string>("f_account");
                        user.Name = dt.Rows[i].IsNull("f_name") ? string.Empty : dt.Rows[i].Field<string>("f_name");
                        user.Status = dt.Rows[i].IsNull("f_status") ? false : dt.Rows[i].Field<bool>("f_status");
                        user.RoleId = dt.Rows[i].IsNull("f_roleId") ? 0 : dt.Rows[i].Field<byte>("f_roleId");
                        users.Add(user);
                    }
                    return (users, totalPages);
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
        /// 新增使用者
        /// </summary>
        internal bool Create(RequestCreateUserDto user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_addUser @acct, @pwd, @name, @role";

                cmd.Parameters.Add("@acct", SqlDbType.VarChar).Value = user.Account;
                cmd.Parameters.Add("@pwd", SqlDbType.VarChar).Value = user.Password;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = user.Name;
                cmd.Parameters.Add("@role", SqlDbType.VarChar).Value = user.DropDownRole;

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
        /// 刪除使用者
        /// </summary>
        internal bool DeleteUser(int userId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_delUser @userId";

                cmd.Parameters.Add("@userId", SqlDbType.VarChar).Value = userId;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                return ExeCnt > 0;
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
        /// 取得角色名
        /// </summary>
        internal List<Role> GetRoleIdAndName()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<Role> roles = new List<Role>();
            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getRoleIdAndName";

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Role role = new Role();
                        role.RoleId = dt.Rows[i].IsNull("f_roleId") ? 0 : dt.Rows[i].Field<byte>("f_roleId");
                        role.Name = dt.Rows[i].IsNull("f_name") ? string.Empty : dt.Rows[i].Field<string>("f_name");
                        roles.Add(role);
                    }

                    return roles;
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
                //判斷是否已關閉
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
        }

        /// <summary>
        /// 更改使用者角色跟狀態
        /// </summary>
        internal bool EditUserRoleAndStatus(RequestEditUserRoleAndStatusDto editUserRoleAndStatus)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editUser @userId, @roleId, @status";

                cmd.Parameters.Add("@userId", SqlDbType.VarChar).Value = editUserRoleAndStatus.UserId;
                cmd.Parameters.Add("@roleId", SqlDbType.TinyInt).Value = editUserRoleAndStatus.RoleId;
                cmd.Parameters.Add("@status", SqlDbType.Bit).Value = editUserRoleAndStatus.Status;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();
                return ExeCnt > 0;
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
        /// 修改使用者密碼
        /// </summary>
        internal bool UpdatePwd(int userId, string oldPwd, string newPwd)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editUserPwd @userId, @oldPwd, @newPwd";

                cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                cmd.Parameters.Add("@oldPwd", SqlDbType.VarChar).Value = oldPwd;
                cmd.Parameters.Add("@newPwd", SqlDbType.VarChar).Value = newPwd;

                cmd.Connection.Open();

                int exeCnt = cmd.ExecuteNonQuery();

                if (exeCnt == 1)
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
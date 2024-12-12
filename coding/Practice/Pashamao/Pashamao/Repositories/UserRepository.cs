using NLog;
using Pashamao.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
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
        /// <param name="acct"></param>
        /// <returns></returns>
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
                    user.UserId = dr.IsNull("f_uId") ? 0 : dr.Field<int>("f_uId");
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
        /// <param name="userSession"></param>
        /// <returns></returns>
        internal (bool, string, long) GetAtEveryRequest(UserSessionModel userSession)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getAtEveryRequest @uId";
                cmd.Parameters.Add("@uId", SqlDbType.Int).Value = userSession.UserId;

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
        /// 新增使用者
        /// </summary>
        /// <param name="user"></param>
        internal bool Create(User user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_addUser @acct, @pwd, @name, @role";

                cmd.Parameters.Add("@acct", SqlDbType.VarChar).Value = user.Account;
                cmd.Parameters.Add("@pwd", SqlDbType.VarChar).Value = user.Pwd;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = user.Name;
                cmd.Parameters.Add("@role", SqlDbType.VarChar).Value = user.RoleName;

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
        /// <param name="user"></param>
        internal void Delete(User user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_delUser @uId";

                cmd.Parameters.Add("@uId", SqlDbType.VarChar).Value = user.UserId;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();
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
        /// <returns></returns>
        internal List<string> GetAllRoleName()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<string> rolesName = new List<string>();
            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getAllRole";

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string roleName = dt.Rows[i].IsNull("f_name") ? string.Empty : dt.Rows[i].Field<string>("f_name");
                        rolesName.Add(roleName);
                    }
                    return rolesName;
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
        /// 更改使用者角色跟權限
        /// </summary>
        /// <param name="user"></param>
        internal void UpdateRole(User user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editUser @uId, @roleId, @status";

                cmd.Parameters.Add("@uId", SqlDbType.VarChar).Value = user.UserId;
                cmd.Parameters.Add("@roleId", SqlDbType.TinyInt).Value = user.RoleId;
                cmd.Parameters.Add("@status", SqlDbType.Bit).Value = user.Status;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();
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
        /// 取得1個使用者
        /// </summary>
        /// <param name="primaryId"></param>
        /// <returns></returns>
        internal User Get(int primaryId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr); 
            SqlDataAdapter da = new SqlDataAdapter(); 
            DataTable dt = new DataTable(); 
            User user = new User();

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getUser @uId";
                cmd.Parameters.Add("@uId", SqlDbType.VarChar).Value = primaryId;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    user.UserId = dr.IsNull("f_uid") ? 0 : dr.Field<int>("f_uid");
                    user.Account = dr.IsNull("f_account") ? string.Empty : dr.Field<string>("f_account");
                    user.Name = dr.IsNull("f_name") ? string.Empty : dr.Field<string>("f_name");
                    user.Status = dr.IsNull("f_status") ? false : dr.Field<bool>("f_status");
                    user.RoleId = dr.IsNull("f_roleId") ? 0 : dr.Field<byte>("f_roleId");

                    Console.WriteLine(user.Name);
                    return user;
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
        /// 修改使用者密碼
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        internal bool UpdatePwd(int userId, string oldPwd, string newPwd)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editUserPwd @uId, @oldPwd, @newPwd";

                cmd.Parameters.Add("@uId", SqlDbType.Int).Value = userId;
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

        /// <summary>
        /// 排序user並傳回table
        /// </summary>
        /// <param name="column"></param>
        /// <param name="page"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        internal (List<User>, int) GetSortedUser(string column, int page, string sortOrder) {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr); 
            SqlDataAdapter da = new SqlDataAdapter(); 
            DataTable dt = new DataTable(); 
            List<User> users = new List<User>();
            int totalPages = 0;
            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getSortedUser @column, @page, @sortOrder, @totalPages OUTPUT";

                cmd.Parameters.Add("@column", SqlDbType.VarChar).Value = column;
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
                        user.UserId = dt.Rows[i].IsNull("f_uid") ? 0 : dt.Rows[i].Field<int>("f_uid");
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
    }
}
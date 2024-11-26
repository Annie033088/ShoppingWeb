﻿using NLog;
using Pashamao.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Pashamao.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 取得使用者登入資料
        /// </summary>
        /// <param name="acct"></param>
        /// <returns></returns>
        internal User VerifyAndGetUser(string acct, string pwd, string sessionId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr); //設定連線字串
            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            User user = new User();

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
                    DataRow dr = dt.Rows[0];
                    user.UID = dr.IsNull("f_uid") ? 0 : dr.Field<int>("f_uid");
                    user.Account = dr.IsNull("f_account") ? string.Empty : dr.Field<string>("f_account");
                    user.Status = dr.IsNull("f_status") ? false : dr.Field<bool>("f_status");
                    user.RoleId = dr.IsNull("f_roleId") ? 0 : dr.Field<byte>("f_roleId");

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
        /// update sessionId
        /// </summary>
        /// <param name="user"></param>
        internal void EditSessionId(User user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editSessionId @uid, @sessionId";

                cmd.Parameters.Add("@uid", SqlDbType.Int).Value = user.UID;
                cmd.Parameters.Add("@sessionId", SqlDbType.VarChar).Value = HttpContext.Current.Session.SessionID;

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
        /// 取得 sessionId
        /// </summary>
        /// <param name="userSession"></param>
        /// <returns></returns>
        internal string GetSessionId(SessionModel userSession)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getSessionId @uid";
                cmd.Parameters.Add("@uid", SqlDbType.Int).Value = userSession.UID;

                cmd.Connection.Open();


                string sessionId = cmd.ExecuteScalar().ToString();
                return sessionId;
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
        /// 取得所有User 
        /// </summary>
        /// <returns></returns>
        internal override IEnumerable<User> GetAll()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr); //設定連線字串
            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            List<User> users = new List<User>();
            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getAllUser";

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        User user = new User();
                        user.UID = dt.Rows[i].IsNull("f_uid") ? 0 : dt.Rows[i].Field<int>("f_uid");
                        user.Account = dt.Rows[i].IsNull("f_account") ? string.Empty : dt.Rows[i].Field<string>("f_account");
                        user.Name = dt.Rows[i].IsNull("f_name") ? string.Empty : dt.Rows[i].Field<string>("f_name");
                        user.Status = dt.Rows[i].IsNull("f_status") ? false : dt.Rows[i].Field<bool>("f_status");
                        user.RoleId = dt.Rows[i].IsNull("f_roleId") ? 0 : dt.Rows[i].Field<byte>("f_roleId");
                        users.Add(user);
                    }
                    return users;
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
        /// 新增User
        /// </summary>
        /// <param name="user"></param>
        internal override void Create(User user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_addUser @acct, @hash, @name, @role";

                cmd.Parameters.Add("@acct", SqlDbType.VarChar).Value = user.Account;
                cmd.Parameters.Add("@hash", SqlDbType.VarChar).Value = user.Hash;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = user.Name;
                cmd.Parameters.Add("@role", SqlDbType.TinyInt).Value = user.RoleId;

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
        /// 刪除User
        /// </summary>
        /// <param name="user"></param>
        internal override void Delete(User user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_delUser @uid";

                cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = user.UID;

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
        /// 更改User
        /// </summary>
        /// <param name="user"></param>
        internal override void Update(User user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editUser @uid, @roleId, @status";

                cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = user.UID;
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
        /// 取得1個User
        /// </summary>
        /// <param name="primaryId"></param>
        /// <returns></returns>
        internal override User Get(int primaryId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr); //設定連線字串
            SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable(); //宣告DataTable物件
            User user = new User();

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getUser @uid";
                cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = primaryId;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    user.UID = dr.IsNull("f_uid") ? 0 : dr.Field<int>("f_uid");
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
    }
}
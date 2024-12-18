﻿using NLog;
using Pashamao.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Pashamao.Repositories
{
    public class RoleRepository
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 取得所有角色
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Role> GetAllRole()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<Role> roles = new List<Role>();

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
                        Role role = new Role();
                        role.RoleId = dt.Rows[i].IsNull("f_roleId") ? 0 : dt.Rows[i].Field<byte>("f_roleId");
                        role.Name = dt.Rows[i].IsNull("f_name") ? string.Empty : dt.Rows[i].Field<string>("f_name");
                        role.Description = dt.Rows[i].IsNull("f_description") ? string.Empty : dt.Rows[i].Field<string>("f_description");
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
        /// 新增角色
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="rolePermission"></param>
        /// <returns></returns>
        public bool AddRole(Role role)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_addRole @name, @description, @rolePermission";

                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = role.Name;
                cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = role.Description;
                cmd.Parameters.Add("@rolePermission", SqlDbType.BigInt).Value = role.Permissions;

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
        /// 取得角色權限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string GetRolePermissions(int roleId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getRolePermission @roleId";
                cmd.Parameters.Add("@roleId", SqlDbType.TinyInt).Value = (byte)roleId;

                cmd.Connection.Open();

                var result = cmd.ExecuteScalar();
                string rolePermissions = result == null ? string.Empty : result.ToString();//當這個user被刪掉會是null
                return rolePermissions;
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
        /// 修改角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool EditRole(Role role)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editRole @roleId, @name, @description, @rolePermission";

                cmd.Parameters.Add("@roleId", SqlDbType.TinyInt).Value = role.RoleId;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = role.Name;
                cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = role.Description;
                cmd.Parameters.Add("@rolePermission", SqlDbType.BigInt).Value = role.Permissions;

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
        /// 刪除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool DeleteRole(int roleId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC [pro_pashamao_delRole] @roleId";

                cmd.Parameters.Add("@roleId", SqlDbType.TinyInt).Value = roleId;

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
        /// 取得角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Role GetRole(int roleId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getRole @roleId";
                cmd.Parameters.Add("@roleId", SqlDbType.TinyInt).Value = roleId;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    Role role = new Role();
                    role.RoleId = roleId;
                    role.Name = dt.Rows[0].IsNull("f_name") ? string.Empty : dt.Rows[0].Field<string>("f_name");
                    role.Description = dt.Rows[0].IsNull("f_description") ? string.Empty : dt.Rows[0].Field<string>("f_description");
                    return role;
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
    }
}
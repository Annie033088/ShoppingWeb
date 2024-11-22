using NLog;
using Pashamao.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Pashamao.Repositories
{
    public class UserRepository : SqlRepository
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger ();

        /// <summary>
        /// 取得使用者資料
        /// </summary>
        /// <param name="acct"></param>
        /// <returns></returns>
        internal User SqlGetUser( string acct )
        {
            SqlCommand cmd = new SqlCommand ();
            cmd.Connection = new SqlConnection ( this.ConnStr ); //設定連線字串
            SqlDataAdapter da = new SqlDataAdapter (); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable (); //宣告DataTable物件
            User user = new User ();

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getUser @Acct";
                cmd.Parameters.Add ( "@Acct", SqlDbType.VarChar ).Value = acct;

                cmd.Connection.Open ();

                da.SelectCommand = cmd;
                da.Fill ( dt );

                cmd.Connection.Close ();

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    user.UID = dr.IsNull ( "f_uid" ) ? 0 : dr.Field<int> ( "f_uid" );
                    user.Account = dr.IsNull ( "f_account" ) ? string.Empty : dr.Field<string> ( "f_account" );
                    user.Hash = dr.IsNull ( "f_hash" ) ? string.Empty : dr.Field<string> ( "f_hash" );
                    user.Name = dr.IsNull ( "f_name" ) ? string.Empty : dr.Field<string> ( "f_name" );
                    user.Status = dr.IsNull ( "f_status" ) ? false : dr.Field<bool> ( "f_status" );
                    user.RoleId = dr.IsNull ( "f_roleId" ) ? 0 : dr.Field<byte> ( "f_roleId" );

                    return user;
                } else
                {
                    return null;
                }

            } catch (Exception e)
            {
                logger.Error ( e );
                throw e;
            } finally
            {
                cmd.Parameters.Clear ();
                //判斷是否已關閉
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close ();
            }

        }

        /// <summary>
        /// update sessionId
        /// </summary>
        /// <param name="user"></param>
        internal void SqlEditSessionId( User user )
        {
            SqlCommand cmd = new SqlCommand ();
            cmd.Connection = new SqlConnection ( this.ConnStr );

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editSessionId @Uid, @SessionId";

                cmd.Parameters.Add ( "@Uid", SqlDbType.Int ).Value = user.UID;
                cmd.Parameters.Add ( "@SessionId", SqlDbType.VarChar ).Value = HttpContext.Current.Session.SessionID;

                cmd.Connection.Open ();

                int ExeCnt = cmd.ExecuteNonQuery ();
            } catch (Exception e)
            {
                logger.Error ( e );
                throw e;
            } finally
            {
                cmd.Parameters.Clear ();
                cmd.Connection.Close ();
            }

        }

        /// <summary>
        /// 取得 sessionId
        /// </summary>
        /// <param name="userSession"></param>
        /// <returns></returns>
        internal string SqlGetSessionId( SessionModel userSession )
        {

            SqlCommand cmd = new SqlCommand ();
            cmd.Connection = new SqlConnection ( this.ConnStr );

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getSessionId @Uid";
                cmd.Parameters.Add ( "@Uid", SqlDbType.Int ).Value = userSession.UID;

                cmd.Connection.Open ();


                string sessionId = cmd.ExecuteScalar ().ToString ();
                return sessionId;
            } catch (Exception e)
            {
                throw e;
            } finally
            {
                cmd.Parameters.Clear ();
                cmd.Connection.Close ();
            }
        }

    }
}
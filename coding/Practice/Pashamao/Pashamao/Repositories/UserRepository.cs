using NLog;
using Pashamao.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Pashamao.Repositories
{
    public class UserRepository : SqlRepository
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger ();

        internal User sqlUserPwd( string acct )
        {
            SqlCommand cmd = new SqlCommand ();
            cmd.Connection = new SqlConnection ( this.ConnStr ); //設定連線字串
            SqlDataAdapter da = new SqlDataAdapter (); //宣告一個配接器(DataTable與DataSet必須)
            DataTable dt = new DataTable (); //宣告DataTable物件
            User user = new User ();

            try
            {

                cmd.CommandText = "EXEC pro_pashamao_getUidPwd @Acct";

                cmd.Parameters.Add ( "@Acct", SqlDbType.VarChar ).Value = acct;

                cmd.Connection.Open ();

                da.SelectCommand = cmd;
                da.Fill ( dt );

                cmd.Connection.Close ();

                if ( dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    user.UID = int.Parse ( dr["f_uid"].ToString () );
                    user.Hash = dr["f_hash"].ToString ();

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

        internal void sqlEditSession( User user )
        {
            SqlCommand cmd = new SqlCommand ();
            cmd.Connection = new SqlConnection ( this.ConnStr ); //設定連線字串

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getAcctPwd @Uid, @SessionId";

                cmd.Parameters.Add ( "@Uid", SqlDbType.Int ).Value = user.UID;
                cmd.Parameters.Add ( "@Acct", SqlDbType.VarChar ).Value = user.SessionId;

                cmd.Connection.Open ();

                int iExecuteCount = cmd.ExecuteNonQuery ();

                cmd.Connection.Close ();

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

        internal void sqlGetSession(SessionModel sessionModel)
        {

        }
    }
}
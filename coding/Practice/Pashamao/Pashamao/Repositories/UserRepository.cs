using NLog;
using Pashamao.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

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

                cmd.CommandText = "EXEC pro_pashamao_getAcctPwd @Acct";

                cmd.Parameters.Add ( "@Acct", SqlDbType.VarChar ).Value = acct;

                cmd.Connection.Open ();

                da.SelectCommand = cmd;
                da.Fill ( dt );

                cmd.Connection.Close ();

                Console.WriteLine ( dt);
                if ( dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    user.Account = dr["f_account"].ToString ();
                    user.Hash = dr["f_hash"].ToString ();
                    Console.WriteLine ( user.Hash );

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


    }
}
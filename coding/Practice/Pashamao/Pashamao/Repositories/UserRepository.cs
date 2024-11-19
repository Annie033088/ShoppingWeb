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

        public User[] sqlUserTable()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = new SqlConnection ( this.ConnStr ); //設定連線字串
                SqlDataAdapter da = new SqlDataAdapter (); //宣告一個配接器(DataTable與DataSet必須)
                DataTable dt = new DataTable (); //宣告DataTable物件

                cmd.CommandText = "EXEC pro_XXXXX_XXX @xxx";

                //cmd.Parameters.Add ( "@xxx", SqlDbType.Int ).Value = Request["xxx"];




            } catch (Exception e)
            {

                throw e;
            } finally { }

            return null;
        }


    }
}
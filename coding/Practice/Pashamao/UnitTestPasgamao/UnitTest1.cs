using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SqlClient;

namespace UnitTestPasgamao
{
    [TestClass]
    public class UnitTest1
    {
        /* [TestMethod]
        public void TestMethod1()
        {
            //找到資料庫pwd_hash
            UserRepository userRepository = new UserRepository ();

             User user = userRepository.sqlUserPwd ( acct );

             //取得鹽值
             string salt = user.Hash.Substring ( 0, 24 );

             //用鹽值 hashPwd 之後, 比對
             HashPwd ( pwd, salt );

             Console.WriteLine ( HashPwd ( pwd, salt ), user.Hash );
             Console.WriteLine ( HashPwd ( pwd, salt ) == user.Hash );


             return (HashPwd ( pwd, salt ) == user.Hash);
        }
        [TestMethod]
        internal bool UpdatePwd(int uid, string oldPwd, string newPwd)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editUserPwd @uid, @oldPwd, @newPwd";

                cmd.Parameters.Add("@uid", SqlDbType.Int).Value = uid;
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
                throw e;
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Connection.Close();
            }
        }*/
    }
}

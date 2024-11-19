// See https://aka.ms/new-console-template for more information
using System.Data;
using System.Security.Cryptography;
using System.Text;


Console.WriteLine ( "Hello, World!" );











/* Pwd hash tst
//salt : 5xkeFUID9WBuVCTNMYYKGg==
//hash : 5xkeFUID9WBuVCTNMYYKGg==a1jyckR4KmZC+L/vF08VMlutOG0VGe2NLaTwienJeDM=

//BG6nNMVFp34q
//yXb8S9azKg7VPfTY
try
{
    string dbHash = "5xkeFUID9WBuVCTNMYYKGg==a1jyckR4KmZC+L/vF08VMlutOG0VGe2NLaTwienJeDM=";
    string salt = dbHash.Substring ( 0, 24 );
    string pwd = "yXb8S9azKg7VPfTY";
    //byte[] saltBytes = GenerateSalt ();
    byte[] pwdBytes = Encoding.UTF8.GetBytes ( pwd );
    //string salt = GenerateSalt ();
    byte[] saltBytes = Convert.FromBase64String ( salt );

    //string salt = GenerateSalt ();
    //string conbinedPwdStr = pwd + salt;
    //204 236 197 199 4 189 57 43 227 24 203 248 113 227 160 140 34 133 231 167 215 144 83 99 73 203 227 5 168 236 86 20
    //byte[] combinedPwdBytes = System.Text.Encoding.UTF8.GetBytes ( conbinedPwdStr );

    byte[] combinedPwdBytes = new byte[saltBytes.Length + pwdBytes.Length];
    //
    saltBytes.CopyTo ( combinedPwdBytes, 0 );

    pwdBytes.CopyTo ( combinedPwdBytes, saltBytes.Length );


    string inputHash;

    using (SHA256 sha256 = SHA256.Create ())
    {

        byte[] inputHashBytes = sha256.ComputeHash ( combinedPwdBytes );
        inputHash = salt + Convert.ToBase64String ( inputHashBytes );
        //轉成16進制後回傳 資料庫已鹽值+加密後pwd儲存

        foreach (byte b in inputHashBytes)
        {
            Console.Write ( b + " " );
        }
        Console.WriteLine ();

    }

    Console.WriteLine ( "salt：" + salt );
    Console.WriteLine ( "dbHash : " + dbHash );
    Console.WriteLine ( "pwdHash : " + inputHash );
    Console.WriteLine ( dbHash == inputHash );
} catch (Exception e)
{
    throw e;
}



/// <summary>
/// 驗證密碼是否正確
/// </summary>
/// <param name="pwd"></param>
/// <returns></returns>
bool VarifyPwd( string pwd )
{
    //找到資料庫pwd_hash

    //取得鹽值

    //用鹽值 hashPwd 之後, 比對


    return false;
}



/// <summary>
/// 雖機生成salt
/// </summary>
/// <returns></returns>
string GenerateSalt()
{
    try
    {
        int len = 16;
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create ())
        {
            byte[] saltBytes = new byte[len];
            rng.GetBytes ( saltBytes ); // 填充隨機數據

            // 將字節數組轉換為可讀的十六進制字符串 回傳(因為1個字節組數代表 1個16進位(xx)=>有32字元)
            return Convert.ToBase64String ( saltBytes );
        }
    } catch (Exception e)
    {
        throw e;
    }
    //16個字節組數


}
*/
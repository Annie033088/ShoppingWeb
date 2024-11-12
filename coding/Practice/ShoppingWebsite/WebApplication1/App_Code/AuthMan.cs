using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace LoginAuth
{
    public class AuthMan : BasePage
    {

        /// <summary>
        /// 無效字元驗證
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool LoginValidChar(string userName, string pwd )
        {
            string illigalChar = @"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,20}$";
            return ((Regex.IsMatch ( userName, illigalChar ) && Regex.IsMatch ( pwd, illigalChar )));
        }

        /// <summary>
        /// 帳密存在驗證
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool SiteSpecificAuthenticationMethod( string userName, string pwd )
        {


            return false;
        }

        /// <summary>
        /// 登入完成
        /// </summary>
        public void SiteSpecificUserLoggingMethod( string userName )
        {
           
            // 假設登錄成功 設置 Session 資料
            Session["username"] = userName;  // 存儲用戶名
            Session["loginTime"] = DateTime.Now;  // 存儲登錄時間

            Response.Redirect ( "ManPage.aspx" );
        }

    }
}
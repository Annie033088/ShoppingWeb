using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LoginAuth
{
    public partial class WebForm1 : BasePage
    {
        //軟體生命週期 
        AuthMan authMan = new AuthMan ();
        /// <summary>
        /// 送出登入, 驗證之前
        /// </summary>
        protected void OnLoggingIn( object sender, System.Web.UI.WebControls.LoginCancelEventArgs e )
        {

            if (!authMan.LoginValidChar ( UserLogin.UserName, UserLogin.Password))
            {
                UserLogin.InstructionText = "請輸入有效帳密";
                e.Cancel = true;//將 屬性LoginCancelEventArgs設定Cancel為 true，以取消登入嘗試
            } else
            {
                UserLogin.InstructionText = String.Empty;
            }

        }

        /// <summary>
        /// 登入進行中驗證
        /// </summary>
        protected void OnAuthenticate(object sender, AuthenticateEventArgs e) 
        {
            bool Authenticated = false;

            Authenticated = authMan.SiteSpecificAuthenticationMethod ( UserLogin.UserName, UserLogin.Password );
            //Authenticated = SiteSpecificAuthenticationMethod(UserLogin.UserName, UserLogin.Password);

            e.Authenticated = Authenticated;
        }



        /// <summary>
        /// 登入完成
        /// </summary>
        protected void OnLoggedIn(object sender, EventArgs e)
        {
            

            authMan.SiteSpecificUserLoggingMethod (UserLogin.UserName);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if ( Session["username"] != null )
            {
                Response.Redirect("ManPage.aspx");
            }
        }
    }
}
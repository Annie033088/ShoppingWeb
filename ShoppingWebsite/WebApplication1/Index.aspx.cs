using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        //軟體生命週期
     

        /// <summary>
        /// 登入進行中驗證
        /// </summary>
        private bool SiteSpecificAuthenticationMethod(string UserName, string Password)
        {
            // Insert code that implements a site-specific custom 
            // authentication method here.
            //
            // This example implementation always returns false.
            return false;
        }

        /// <summary>
        /// 登入進行中驗證
        /// </summary>
        protected void OnAuthenticate(object sender, AuthenticateEventArgs e) 
        {
            bool Authenticated = false;
            Authenticated = SiteSpecificAuthenticationMethod(UserLogin.UserName, UserLogin.Password);

            e.Authenticated = Authenticated;
        }

        /// <summary>
        /// 登入完成紀錄時間
        /// </summary>
        private void SiteSpecificUserLoggingMethod(string UserName)
        {
            // Insert code to record the current date and time
            // when this user was authenticated at the site.
            Server.Transfer("ManPage.aspx");
        }

        /// <summary>
        /// 登入完成紀錄時間
        /// </summary>
        protected void OnLoggedIn(object sender, EventArgs e)
        {
            SiteSpecificUserLoggingMethod(UserLogin.UserName);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            UserLogin.InstructionText = "你好";
        }
    }
}
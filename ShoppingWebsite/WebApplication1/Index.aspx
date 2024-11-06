<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="WebApplication1.WebForm1" %>

<!DOCTYPE html>
<script runat="server">    
        /// <summary>
        /// 送出登入, 驗證之前
        /// </summary>
        protected void OnLoggingIn(object sender, System.Web.UI.WebControls.LoginCancelEventArgs e)
        {

            if ( IsValidAcct(UserLogin.UserName) || IsValidPwd(UserLogin.Password) )
            {
                UserLogin.InstructionText = "請輸入有效帳密";
                e.Cancel = true;//將 屬性LoginCancelEventArgs設定Cancel為 true，以取消登入嘗試
            }
            else
            {
                UserLogin.InstructionText = String.Empty;
            }
        }
        /// <summary>
        /// 驗證帳號非法字元
        /// </summary>
        private bool IsValidAcct(string acct){
                string CharEmail = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";//^放外面是開頭
                return !Regex.IsMatch(acct, CharEmail);
            }
     
        /// <summary>
        /// 驗證密碼非法字元
        /// </summary>
        private bool IsValidPwd(string pwd){
                string illegalCharAcct = @"(^\w\d)";

                return Regex.IsMatch(pwd, illegalCharAcct);
            }
    
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="css/index.css" />
</head>
<body>
    <form id = "form1" runat="server">
        <div class = "LoginBlock">
            <asp:Login ID="UserLogin" runat="server"
                BorderStyle ="Solid" 
                BackColor ="#F0F0F0" 
                BorderWidth ="1px"
                BorderColor ="#CCCC99" 
                Font-Size ="10pt" 
                Font-Names ="Verdana" 
                CreateUserText ="新增使用者..."
                CreateUserUrl="AddNewAcct.aspx" 
                PasswordRecoveryText ="忘記密碼"
                UserNameLabelText ="請輸入信箱:"
                onloggingin ="OnLoggingIn"
                OnAuthenticate = "OnAuthenticate"
                OnLoggedIn = "OnLoggedIn">
                <TitleTextStyle Font-Bold="True" 
                    ForeColor="#FFFFFF" 
                    BackColor="#6B696B">
                </TitleTextStyle>
                <InstructionTextStyle Font-Bold="True"
                    ForeColor="#EA0000">
                </InstructionTextStyle>
            </asp:Login>
        </div>
    </form>
</body>
</html>

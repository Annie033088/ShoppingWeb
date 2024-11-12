<%@ Page Title="Login Page" Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="LoginAuth.WebForm1" %>

<!-- MasterPageFile="~/Site.Master" 使用contentplaceholder承接母網頁內容-->

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="css/index.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="LoginBlock"><!--Login控制項裡的頁面渲染由asp.net自動執行-->
            <asp:Login ID="UserLogin" runat="server"
                BorderStyle="Solid"
                BackColor="#F0F0F0"
                BorderWidth="1px"
                BorderColor="#CCCC99"
                Font-Size="10pt"
                Font-Names="Verdana"
                UserNameLabelText="請輸入信箱:"
                OnLoggingIn="OnLoggingIn"
                OnAuthenticate="OnAuthenticate"
                OnLoggedIn="OnLoggedIn">
                <TitleTextStyle Font-Bold="True"
                    ForeColor="#FFFFFF"
                    BackColor="#6B696B">
                </TitleTextStyle>
                <InstructionTextStyle Font-Bold="True"
                    ForeColor="#EA0000">
                </InstructionTextStyle>
            </asp:Login>
            
             <script src ="js/index.js" ></script>
        </div>

    </form>

    
</body>
</html>


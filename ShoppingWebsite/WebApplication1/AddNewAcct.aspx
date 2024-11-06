<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddNewAcct.aspx.cs" Inherits="WebApplication1.AddNewAccount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="css/AddNewAcct.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="AddBlock">
            <asp:createuserwizard id="Createuserwizard1" runat="server" >
            </asp:createuserwizard>
        </div>
    </form>
</body>
</html>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewSwitcher.ascx.cs" Inherits="WebApplication1.ViewSwitcher" %>
<div id="viewSwitcher">
    <%: CurrentView %> view | <a href="<%: SwitchUrl %>" data-ajax="false">Switch to <%: AlternateView %></a>
</div>

<!-- %>是一個動態資料插入的語法 CurrentView屬性執會插入到編碼中 防止跨站腳本攻擊-->
<!--ata-ajax="false"：這是 ASP.NET AJAX 的屬性，它告訴頁面在點擊這個超連結時不使用 Ajax 來更新頁面，而是進行完全的頁面重新載入-->
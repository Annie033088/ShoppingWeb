<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewSwitcher.ascx.cs" Inherits="WebApplication1.ViewSwitcher" %>
<div id="viewSwitcher">
    <%: CurrentView %> view | <a href="<%: SwitchUrl %>" data-ajax="false">Switch to <%: AlternateView %></a>
</div>

<!-- %>�O�@�ӰʺA��ƴ��J���y�k CurrentView�ݩʰ��|���J��s�X�� ����󯸸}������-->
<!--ata-ajax="false"�G�o�O ASP.NET AJAX ���ݩʡA���i�D�����b�I���o�ӶW�s���ɤ��ϥ� Ajax �ӧ�s�����A�ӬO�i�槹�����������s���J-->
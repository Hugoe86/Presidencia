<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Pager.ascx.cs" Inherits="Pager" %>

<div style="font-size:8pt; font-family:Verdana;">
    <div id="left" style="float:left;border-style:outset; color:White;background-color:#2F4E7D;">
        <span>Ir P&aacute;gina&nbsp;</span>
        <asp:DropDownList ID="ddlPageNumber" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPageNumber_SelectedIndexChanged"></asp:DropDownList>
        <span>&nbsp;de</span>
        <asp:Label ID="lblShowRecords" runat="server"></asp:Label>
        <span>P&aacute;ginas&nbsp;</span>
    </div>
    <div id="right" style="float:right;border-style:outset; color:White;background-color:#2F4E7D;">
        <span>Mostrar&nbsp;</span>
        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
            <asp:ListItem Text="1" Value="1"></asp:ListItem>
            <asp:ListItem Text="5" Value="5"></asp:ListItem>
            <asp:ListItem Text="10" Value="10" Selected="true"></asp:ListItem>
            <asp:ListItem Text="20" Value="20"></asp:ListItem>
            <asp:ListItem Text="25" Value="25"></asp:ListItem>
            <asp:ListItem Text="50" Value="50"></asp:ListItem>
        </asp:DropDownList>
        <span>&nbsp;Registros por P&aacute;gina</span>
    </div>
</div>
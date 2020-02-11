<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LinkPager.ascx.cs" Inherits="LinkPager" %>
<div style="font-size:8pt; font-family:Verdana;">
    <div id="left" style="float:left;"><span>Page</span>
        <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
        <span>of</span>
        <asp:Label ID="lblTotalRecords" runat="server"></asp:Label>
    </div>
    <div id="right" style="float:right;">
        <asp:Repeater ID="rptPages" runat="server" OnItemDataBound="rptPages_ItemDataBound">
        <ItemTemplate>
            [<asp:LinkButton ID="lnkPageNumbers" runat="server" OnClick="lnkPageNumbers_Click"></asp:LinkButton>
            <asp:Label ID="lblPageNumbers" runat="server"></asp:Label>]
        &nbsp;</ItemTemplate>
        </asp:Repeater>
    </div>
    <br />
    <div id="right2" style="float:right;">
        <asp:LinkButton ID="lnkFirstPage" runat="server" Text="First" OnClick="lnkGOFPage_Click"></asp:LinkButton>
        <asp:Label ID="lblFirstPage" runat="server" Text="First"></asp:Label>
        <asp:LinkButton ID="lnkPreviousPage" runat="server" Text="Prev" OnClick="lnkGOFPage_Click"></asp:LinkButton>
        <asp:Label ID="lblPreviousPage" runat="server" Text="Prev"></asp:Label>
        <asp:LinkButton ID="lnkNextPage" runat="server" Text="Next" OnClick="lnkGOFPage_Click"></asp:LinkButton>
        <asp:Label ID="lblNextPage" runat="server" Text="Next"></asp:Label>
        <asp:LinkButton ID="lnkLastPage" runat="server" Text="Last" OnClick="lnkGOFPage_Click"></asp:LinkButton>
        <asp:Label ID="lblLastPage" runat="server" Text="Last"></asp:Label>
    </div>
</div>

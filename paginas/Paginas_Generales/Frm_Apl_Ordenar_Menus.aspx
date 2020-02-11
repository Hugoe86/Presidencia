<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" EnableEventValidation="true"
    CodeFile="Frm_Apl_Ordenar_Menus.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Apl_Ordenar_Menus" Title="Catálogo de Ordenar Menús y Submenús" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Import Namespace="System.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script src="../../easyui/jquery-1.4.2.js" type="text/javascript"></script>
<script src="../../easyui/jquery-ui.min.js" type="text/javascript"></script>
<script src="../../javascript/Js_Ordenar_Menus.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="SM_Orden_Menus" runat="server" />
    <asp:UpdatePanel ID="UPnl_Orden_Menus" runat="server">
        <ContentTemplate>
            <center>
            <table style="width:90%;">
                <tr>
                    <td colspan="2" style="text-align:left;" class="label_titulo">
                        Ordenar Menus
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:left;">
                        <hr style="width:100%;"/>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:left; width:40%; cursor:default;">
                        <font style="color:#2F4E7D;font-size:14px; font-family:Comic Sans MS; font-weight:bold;">
                            Deseas ordenar los men&uacute;s o submen&uacute;s<b style="font-family:Comic Sans MS; font-size:22px;color:#FF7F24">?</b>
                        </font>
                    </td>
                    <td style="text-align:left; width:60%; cursor:default;">
                        <asp:DropDownList ID="Cmb_Tipo" runat="server" Width="40%">
                            <asp:ListItem Value="">&lt;- Seleccione - &gt;</asp:ListItem>
                            <asp:ListItem Value="menu">menu</asp:ListItem>
                            <asp:ListItem Value="submenu">submenu</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="Cmb_Menus" runat="server" Width="58%"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:left;">
                        <hr style="width:100%;"/>
                    </td>
                </tr>
            </table>
                
            <asp:Button ID="Btn_Actualizar_Menus" runat="server" Text="Actualizar Orden de los Menús" CssClass="button_autorizar" style="border-style:outset;width:90%;"/>
            <span id="Span_Orden_Menus"></span>
            </center>
            
                
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


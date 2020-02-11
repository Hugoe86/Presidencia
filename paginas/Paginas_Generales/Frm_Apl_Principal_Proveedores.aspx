<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Proveedores.master" AutoEventWireup="true" CodeFile="Frm_Apl_Principal_Proveedores.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Apl_Principal" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <table style="width: 100%;">
        <tr>
            <td>
                &nbsp;
            </td>
            <td align="center">            
                <img alt="" src="<%= Page.ResolveUrl("~/paginas/imagenes/paginas/img_portal_proveedor.PNG") %>"/>
            </td>
            <td>
                
            </td>
        </tr>      
    </table>
</asp:Content>


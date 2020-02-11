<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Menu_Pre_Tasas_Traslado_Dominio.aspx.cs" Inherits="paginas_Predial_Ventanas_Emergentes_Calculo_Impuestos_Frm_Menu_Pre_Tasas_Traslado_Dominio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TASAS</title>
    <%--<link href="../../estilos/estilo_masterpage.css" rel="stylesheet" type="text/css" />--%>
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <%--<link href="../../estilos/estilo_ajax.css" rel="stylesheet" type="text/css" />--%>
    <base target="_self" />
    <style type="text/css">
.GridView_1
{
    font-family:Verdana, Geneva, MS Sans Serif;
    font-size:8px;
    color:#2F4E7D;
    font-weight:normal;
    padding: 3px 6px 3px 6px;
    vertical-align:middle;
    white-space:nowrap;
    background-color:White;
    border-style:none;
    border-width:5px;
    width:98%;    
    text-align:left;
	margin-left: 0px;
}
*
{
	font-family:Arial;
	font-size:small;
}
.GridHeader
 {
     font-weight:bold;
     background-color:#2F4E7D;
     color:#ffffff;
     text-align:left;
     position:relative;
     height:23px;
 }

.GridItem
{
 background-color:white;
 color:#25406D;
}

.GridAltItem
{
 background-color:#E6E6E6;/*#E6E6E6 #E0F8F7*/
 color:#25406D;
}

.GridSelected
{
 background-color:#A4A4A4; /*#A9F5F2;*/
 color:#25406D;
}

.renglon_botones
{
   vertical-align:middle; 
   height:40px;
}

        .style1
        {
            width: 876px;
        }

        .style2
        {
            width: 4px;
        }

    </style>
</head>
<body>
    <form id="Frm_Menu_Pre_Tasas_Traslado_Dominio" method="post" runat="server">
      <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
        <table style="width:100%;">
          <tr>
            <td colspan="2" align="left">
              <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" 
                Width="24px" Height="24px"/>
                <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
            </td>            
          </tr>
          <tr>
            <td style="width:10%;">              
            </td>          
            <td style="width:90%;text-align:left;" valign="top" >
              <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text=""  CssClass="estilo_fuente_mensaje_error" />
            </td>
          </tr>          
        </table>                   
      </div>                          
    <div>
        <table  style="width:98%;" border="0" cellspacing="0" class="estilo_fuente">
            <tr>
                <td align="center" class="style2">
                    &nbsp;</td>
                <td align="center">
                    &nbsp;</td>
                <td align="center" class="style2">
                    &nbsp;</td>
            </tr>
            <tr class="barra_busqueda">
                <td align="center" class="style2">
                    &nbsp;</td>
                <td align="center">
                            <asp:ImageButton ID="Btn_Aceptar" runat="server" 
                                AlternateText="Aceptar" CssClass="Img_Button" 
                                ImageUrl="~/paginas/imagenes/paginas/accept.png" 
                                onclick="Btn_Aceptar_Click" Width="24px" Visible="false" />
                            <asp:ImageButton ID="Btn_Regresar" runat="server" 
                                AlternateText="Regresar" CssClass="Img_Button" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Regresar_Click" Width="24px" />
                        </td>
                <td align="center" class="style2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center" class="style2">
                    &nbsp;</td>
                <td align="center" style="background-color: #6699FF">
                    <asp:Label ID="Lbl_Title" runat="server" Text="Label" Font-Bold="True" 
                        ForeColor="White"></asp:Label>
                </td>
                <td align="center" class="style2">
                    &nbsp;</td>
            </tr>
            <tr >
                <td class="style2">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td class="style2">
                    &nbsp;</td>
            </tr>
            <tr >
                <td class="style2">
                    &nbsp;</td>
                <td>
                    <asp:GridView runat="server" AllowPaging="True" AutoGenerateColumns="False" 
                        GridLines="None" CssClass="GridView_1" Width="100%" ID="Grid_Tasas" 
                        onselectedindexchanged="Grid_Tasas_SelectedIndexChanged" 
                        onpageindexchanging="Grid_Tasas_PageIndexChanging" >
                        <AlternatingRowStyle CssClass="GridAltItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="TASA_ID" HeaderText="Tasa ID" Visible="false" >
                            <FooterStyle Font-Size="0pt" Width="0px" />
                            <HeaderStyle Font-Size="0pt" Width="0px" />
                            <ItemStyle Font-Size="0pt" Width="0px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Deducible Normal" 
                                HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Right" ItemStyle-Width="15%" >
                                <ItemTemplate>
                                    <asp:Label ID="Lbl_Descripcion_Deducible_10" runat="server" ItemStyle-HorizontalAlign="Right" 
                                        Text='<%# Eval("DEDUCIBLE_UNO", "{0:#,##0.00}") %>'></asp:Label>
                                    <asp:CheckBox ID="Chk_Deducible_10" runat="server" AutoPostBack="True" 
                                        oncheckedchanged="Chk_Deducible_10_CheckedChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Deducible <br />Interés Social" 
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" 
                                ItemStyle-HorizontalAlign="Right" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="Lbl_Descripcion_Deducible_15" runat="server"  ItemStyle-HorizontalAlign="Right" 
                                        Text='<%# Eval("DEDUCIBLE_DOS", "{0:#,##0.00}") %>'></asp:Label>
                                    <asp:CheckBox ID="Chk_Deducible_15" runat="server" AutoPostBack="True" 
                                        oncheckedchanged="Chk_Deducible_15_CheckedChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Deducible <br />20 Salarios" 
                                HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="7%" ItemStyle-Width="12%"
                                ItemStyle-HorizontalAlign="Right" >
                                <ItemTemplate>
                                    <asp:Label ID="Lbl_Descripcion_Deducible_20" runat="server" 
                                        Text='<%# Eval("DEDUCIBLE_TRES", "{0:#,##0.00}") %>'></asp:Label>
                                    <asp:CheckBox ID="Chk_Deducible_20" runat="server" AutoPostBack="True" 
                                        oncheckedchanged="Chk_Deducible_20_CheckedChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ANIO" HeaderText="Año" SortExpression="ANIO"
                             HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="COMENTARIOS" HeaderText="Comentarios" 
                            ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="TASA" HeaderText="Tasa" SortExpression="TASA" DataFormatString="{0:##0.00}" >
                            <ItemStyle Width="8%" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                        <PagerStyle CssClass="GridHeader"></PagerStyle>
                        <RowStyle CssClass="GridItem"></RowStyle>
                        <SelectedRowStyle CssClass="GridSelected"></SelectedRowStyle>
                    </asp:GridView>
                </td>
                <td class="style2">
                    &nbsp;</td>
            </tr>
            <tr >
                <td class="style2">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td class="style2">
                    &nbsp;</td>
            </tr>
            </table>
    
    </div>
    </form>
</body>
</html>

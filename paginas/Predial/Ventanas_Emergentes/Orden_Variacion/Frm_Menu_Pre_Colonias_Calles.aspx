<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Menu_Pre_Colonias_Calles.aspx.cs" Inherits="paginas_Predial_Ventanas_Emergentes_Orden_Variacion_Frm_Menu_Pre_Colonias_Calles" %>

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
	text-align: left;
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

        .style3
        {
            width: 238px;
        }

    </style>
</head>
<body>
    <form id="Frm_Menu_Pre_Colonias_Calles" method="post" runat="server">
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
                                onclick="Btn_Aceptar_Click" Width="24px" />
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
                <td style="font-weight: bold">
                    Colonias</td>
                <td class="style2">
                    &nbsp;</td>
            </tr>
            <tr >
                <td class="style2">
                    &nbsp;</td>
                <td>
                    <asp:GridView runat="server" AllowPaging="True" AutoGenerateColumns="False" 
                        GridLines="None" CssClass="GridView_1" Width="100%" ID="Grid_Colonias" 
                        PageSize="5" onpageindexchanging="Grid_Colonias_PageIndexChanging" 
                        onselectedindexchanged="Grid_Colonias_SelectedIndexChanged" >
                        <AlternatingRowStyle CssClass="GridAltItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:ButtonField CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png" ButtonType="Image">
                                <ItemStyle Width="30px"></ItemStyle>
                            </asp:ButtonField>
                            <asp:BoundField DataField="COLONIA_ID" HeaderText="Colonia_ID" 
                                SortExpression="COLONIA_ID">
                                <HeaderStyle Width="100px" />
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" 
                                SortExpression="NOMBRE" />
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
            <tr >
                <td class="style2">
                    &nbsp;</td>
                <td style="font-weight: bold">
                    Calles</td>
                <td class="style2">
                    &nbsp;</td>
            </tr>
            <tr >
                <td class="style2">
                    &nbsp;</td>
                <td>
                    <asp:GridView runat="server" AllowPaging="True" AutoGenerateColumns="False" 
                        GridLines="None" CssClass="GridView_1" Width="100%" ID="Grid_Calles" 
                        PageSize="5" onselectedindexchanged="Grid_Calles_SelectedIndexChanged" >
                        <AlternatingRowStyle CssClass="GridAltItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:ButtonField CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png" ButtonType="Image">
                                <ItemStyle Width="30px"></ItemStyle>
                            </asp:ButtonField>
                            <asp:BoundField DataField="CALLE_ID" HeaderText="Calle_ID" 
                                SortExpression="CALLE_ID">
                                <HeaderStyle Width="100px" />
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" 
                                SortExpression="NOMBRE" />
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

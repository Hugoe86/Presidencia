<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Desglose_Adeudos.aspx.cs" Inherits="paginas_Predial_Ventanas_Emergentes_Resumen_Predial_Frm_Desglose_Adeudos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Adeudos de la cuenta</title>
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
    <form id="Frm_Menu_Pre_Multas" method="post" runat="server">
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
                <td align="left">
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
                    <asp:GridView ID="Grid_Adeudos"  runat="server" 
                        AllowPaging="True" AutoGenerateColumns="False" 
                        GridLines="None" CssClass="GridView_1" Width="100%"
                        onpageindexchanging="Grid_Adeudos_PageIndexChanging" >
                        <AlternatingRowStyle CssClass="GridAltItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="ANIO" HeaderText="Año"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="9%" />
                            <asp:BoundField DataField="ADEUDO_BIMESTRE_1" HeaderText="Bimestre 1"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="12%" DataFormatString="{0:$#,##0.00}" />
                            <asp:BoundField DataField="ADEUDO_BIMESTRE_2" HeaderText="Bimestre 2"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="12%" DataFormatString="{0:$#,##0.00}" />
                            <asp:BoundField DataField="ADEUDO_BIMESTRE_3" HeaderText="Bimestre 3"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="12%" DataFormatString="{0:$#,##0.00}" />
                            <asp:BoundField DataField="ADEUDO_BIMESTRE_4" HeaderText="Bimestre 4"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="12%" DataFormatString="{0:$#,##0.00}" />
                            <asp:BoundField DataField="ADEUDO_BIMESTRE_5" HeaderText="Bimestre 5"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="12%" DataFormatString="{0:$#,##0.00}" />
                            <asp:BoundField DataField="ADEUDO_BIMESTRE_6" HeaderText="Bimestre 6"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="12%" DataFormatString="{0:$#,##0.00}" />
                            <asp:BoundField DataField="ADEUDO_TOTAL_ANIO" HeaderText="Total"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="15%" DataFormatString="{0:$#,##0.00}" />
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

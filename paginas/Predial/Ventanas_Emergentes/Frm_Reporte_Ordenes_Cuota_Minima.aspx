<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Reporte_Ordenes_Cuota_Minima.aspx.cs"
    Inherits="paginas_Predial_Ventanas_Emergentes_Frm_Reporte_Ordenes_Cuota_Minima" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Generación de adeudos - órdenes generadas</title>
    <link href="../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <base target="_self" />
    <style type="text/css">
        .GridView_1
        {
            font-family: Verdana, Geneva, MS Sans Serif;
            font-size: 8px;
            color: #2F4E7D;
            font-weight: normal;
            padding: 3px 6px 3px 6px;
            vertical-align: middle;
            white-space: nowrap;
            background-color: White;
            border-style: none;
            border-width: 5px;
            width: 98%;
            text-align: left;
            margin-left: 0px;
        }
        *
        {
            font-family: Arial;
            font-size: small;
        }
        .GridHeader
        {
            font-weight: bold;
            background-color: #2F4E7D;
            color: #ffffff;
            text-align: left;
            position: relative;
            height: 23px;
        }
        .GridItem
        {
            background-color: white;
            color: #25406D;
        }
        .GridAltItem
        {
            background-color: #E6E6E6; /*#E6E6E6 #E0F8F7*/
            color: #25406D;
        }
        .GridSelected
        {
            background-color: #A4A4A4; /*#A9F5F2;*/
            color: #25406D;
        }
        .renglon_botones
        {
            vertical-align: middle;
            height: 40px;
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
    <form id="Frm_Cuentas_Excluidas_Cierre_Anual" method="post" runat="server">
    <div>
        <table style="width: 98%;" border="0" cellspacing="0" class="estilo_fuente">
            <tr>
                <td align="center" class="style2">
                    &nbsp;
                </td>
                <td align="center">
                    &nbsp;
                </td>
                <td align="center" class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr class="barra_busqueda">
                <td align="center" class="style2">
                    &nbsp;
                </td>
                <td align="left">
                    <asp:ImageButton ID="Btn_Exportar_Excel" runat="server" ToolTip="Exportar a Excel"
                        CssClass="Img_Button" TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_xls.png"
                        OnClick="Btn_Exportar_Excel_Click" />
                    <asp:ImageButton ID="Btn_Regresar" runat="server" AlternateText="Regresar" CssClass="Img_Button"
                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Regresar_Click"
                        Width="24px" />
                </td>
                <td align="center" class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" class="style2">
                    &nbsp;
                </td>
                <td align="center" style="background-color: #6699FF">
                    <asp:Label ID="Lbl_Title" runat="server" Text="Órdenes de variación" Font-Bold="True" ForeColor="White"></asp:Label>
                </td>
                <td align="center" class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;
                </td>
                <td>
                    <asp:GridView ID="Grid_Ordenes_Generadas" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                        GridLines="None" CssClass="GridView_1" Width="100%">
                        <AlternatingRowStyle CssClass="GridAltItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta predial" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="25%" />
                            <asp:BoundField DataField="NO_ORDEN_VARIACION" HeaderText="Orden de variación" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="35%" />
                            <asp:BoundField DataField="NO_NOTA" HeaderText="Nota" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="25%" />
                        </Columns>
                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                        <PagerStyle CssClass="GridHeader"></PagerStyle>
                        <RowStyle CssClass="GridItem"></RowStyle>
                        <SelectedRowStyle CssClass="GridSelected"></SelectedRowStyle>
                    </asp:GridView>
                </td>
                <td class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td class="style2">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

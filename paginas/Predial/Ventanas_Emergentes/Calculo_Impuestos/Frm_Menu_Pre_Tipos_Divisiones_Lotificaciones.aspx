<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Menu_Pre_Tipos_Divisiones_Lotificaciones.aspx.cs"
    Inherits="paginas_Predial_Ventanas_Emergentes_Calculo_Impuestos_Frm_Menu_Pre_Tipos_Divisiones_Lotificaciones" %>

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
            text-align: left;
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
    <form id="Frm_Menu_Pre_Tipos_Divisiones_Lotificaciones" method="post" runat="server">
    <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
        <table style="width: 100%;">
            <tr>
                <td colspan="2" align="left">
                    <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                        Width="24px" Height="24px" />
                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                </td>
            </tr>
            <tr>
                <td style="width: 10%;">
                </td>
                <td style="width: 90%; text-align: left;" valign="top">
                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                </td>
            </tr>
        </table>
    </div>
    <div>
        <table style="width: 98%;" border="0" cellspacing="0" class="estilo_fuente">
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
            <tr class="barra_busqueda">
                <td class="style2">
                    &nbsp;
                </td>
                <td align="center" width="45%">
                    <asp:ImageButton ID="Btn_Regresar" runat="server" AlternateText="Regresar" CssClass="Img_Button"
                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Regresar_Click"
                        Width="24px" />
                </td>
                <td align="right" >
                    <table style="width: 100%; height: 28px;">
                        <tr>
                            <td style="vertical-align: middle; text-align: right; width: 20%;">
                                B&uacute;squeda:
                            </td>
                            <td style="width: 55%;">
                                <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="4" ToolTip="Buscar"
                                    Width="180px" />
                            </td>
                            <td style="vertical-align: middle; width: 5%;">
                                <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                    ToolTip="Buscar" OnClick="Btn_Buscar_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;
                </td>
                <td align="center" style="background-color: #6699FF" colspan="2">
                    <asp:Label ID="Lbl_Title" runat="server" Text="Label" Font-Bold="True" ForeColor="White"></asp:Label>
                </td>
                <td class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;
                </td>
                <td colspan="2">
                    <asp:GridView runat="server" AllowPaging="True" AutoGenerateColumns="False" GridLines="None"
                        CssClass="GridView_1" Width="100%" ID="Grid_Tasas" PageSize="5" OnSelectedIndexChanged="Grid_Tasas_SelectedIndexChanged"
                        OnPageIndexChanging="Grid_Tasas_PageIndexChanging" colspan="2">
                        <AlternatingRowStyle CssClass="GridAltItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:ButtonField CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png"
                                ButtonType="Image">
                                <ItemStyle Width="30px"></ItemStyle>
                            </asp:ButtonField>
                            <asp:BoundField DataField="ANIO" HeaderText="Año" SortExpression="ANIO" ItemStyle-Width="8%">
                            </asp:BoundField>
                            <asp:BoundField DataField="DESCRIPCION" HeaderText="Concepto" SortExpression="DESCRIPCION" />
                            <asp:BoundField DataField="TASA" HeaderText="Tasa" SortExpression="TASA" ItemStyle-Width="8%">
                            </asp:BoundField>
                            <asp:BoundField DataField="IMPUESTO_DIVISION_LOT_ID" HeaderText="Impuesto_Division_Lotificacion_ID"
                                Visible="false" />
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
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

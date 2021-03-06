﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ven_Busqueda_Avanzada_Peritos.aspx.cs"
    Inherits="paginas_Emergentes_Ventanilla_Frm_Ven_Busqueda_Avanzada_Peritos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>TASAS</title>
    <%--<link href="../../estilos/estilo_masterpage.css" rel="stylesheet" type="text/css" />--%>
    <link href="../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <%--<link href="../../estilos/estilo_ajax.css" rel="stylesheet" type="text/css" />--%>
    <base target="_self" />
    <style type="text/css">
        .Ventana
        {
            height: 420px;
            width: 700px;
        }
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
            font-size: x-small;
        }
        .GridAltItem
        {
            background-color: #E6E6E6; /*#E6E6E6 #E0F8F7*/
            color: #25406D;
            font-size: x-small;
        }
        .GridSelected
        {
            background-color: #A4A4A4; /*#A9F5F2;*/
            color: #25406D;
            font-size: x-small;
        }
        .style2
        {
            width: 4px;
        }
    </style>
</head>
<body>
    <form id="Frm_Busqueda_Avanzada_Peritos" method="post" runat="server">
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
        <asp:ScriptManager ID="ScptM_Busqueda_Avanzada" runat="server" />
        <%--    <asp:UpdatePanel ID="Upd_Parametros_Predial" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
--%>
        <table style="width: 98%;" border="0" cellspacing="0" class="estilo_fuente">
            <tr>
                <td colspan="5">
                    &nbsp;
                </td>
            </tr>
            <tr class="barra_busqueda">
                <td align="center" class="style2">
                    &nbsp;
                </td>
                <td align="center">
                    <asp:ImageButton ID="Btn_Regresar" runat="server" AlternateText="Regresar" CssClass="Img_Button"
                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Regresar_Click"
                        Width="24px" />
                </td>
                <td align="center">
                    &nbsp;
                </td>
                <td style="text-align: right;">
                    <asp:ImageButton ID="Btn_Limpiar" runat="server" AlternateText="Limpiar" CssClass="Img_Button"
                        ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" OnClick="Btn_Limpiar_Click"
                        Width="24px" />
                </td>
                <td align="center" class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;
                </td>
                <td>
                    Perito
                </td>
                <td style="text-align: left; font-size: 11px;">
                    <asp:TextBox ID="Txt_Busqueda_Perito" runat="server"  OnTextChanged="Txt_Busqueda_Perito_TextChanged"
                        AutoPostBack="True" Style="text-transform: uppercase; width: 500px;" />
                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Perito" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, CUSTOM"
                        ValidChars=" ÁáÉéÍíÓóÚúÑñ" TargetControlID="Txt_Busqueda_Perito" />
                    <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_Perito" runat="server" TargetControlID="Txt_Busqueda_Perito"
                        WatermarkText="Búsqueda de Peritos" WatermarkCssClass="watermarked" />
                </td>
                <td>
                    <asp:ImageButton ID="Btn_Buscar_Perito" runat="server" AlternateText="Buscar" CssClass="Img_Button"
                        ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Width="24px" OnClick="Btn_Buscar_Perito_Click" />
                </td>
                <td class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;
                </td>
                <td colspan="3">
                    <div style="overflow-y: scroll; overflow-x: hidden; height:250px; width:inherit;">
                        <asp:GridView ID="Grid_Perito" runat="server" AutoGenerateColumns="False" CssClass="GridView_1"
                            GridLines="None" OnPageIndexChanging="Grid_Perito_PageIndexChanging" OnSelectedIndexChanged="Grid_Perito_SelectedIndexChanged"
                            Style="white-space: normal; width: 97%;">
                            <RowStyle CssClass="GridItem" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="INSPECTOR_ID" Visible="False" />
                                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                    <HeaderStyle Width="25%" />
                                    <ItemStyle Width="25%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CALLE_OFICINA" HeaderText="Calle">
                                    <HeaderStyle Width="30%" />
                                    <ItemStyle Width="30%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="COLONIA_OFICINA" HeaderText="Colonia">
                                    <HeaderStyle Width="30%" />
                                    <ItemStyle Width="30%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NUMERO_OFICINA" HeaderText="Numero">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle Width="10%" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="GridHeader" />
                            <PagerStyle CssClass="GridHeader" />
                            <SelectedRowStyle CssClass="GridSelected" />
                        </asp:GridView>
                    </div>
                </td>
                <td class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Busqueda_Avanzada_Colonias.aspx.cs"
    Inherits="paginas_Predial_Ventanas_Emergentes_Frm_Busqueda_Avanzada_Colonias" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Búsqueda avanzada de Colonias</title>
    <link href="../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
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
        .style2
        {
            width: 4px;
        }
        .style3
        {
            width: 25%;
        }
        .style4
        {
            width: 25%;
        }
        .style6
        {
            width: 25%;
            text-align: right;
        }
        .style7
        {
            width: 10%;
        }
        .style8
        {
        }
    </style>
</head>
<body>
    <form id="Frm_Busqueda_Avanzada_Colonias" method="post" runat="server">
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
        <asp:UpdatePanel ID="Upd_Parametros_Predial" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table style="width: 98%;" border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td align="center" class="style2">
                            &nbsp;
                        </td>
                        <td align="center" class="style8">
                            &nbsp;
                        </td>
                        <td align="center" class="style3">
                            &nbsp;
                        </td>
                        <td align="center" class="style7">
                            &nbsp;
                        </td>
                        <td align="center" class="style3">
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
                        <td align="center" class="style8" colspan="2">
                            <asp:ImageButton ID="Btn_Regresar" runat="server" AlternateText="Regresar" CssClass="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Regresar_Click"
                                Width="24px" />
                            <asp:ImageButton ID="Btn_Buscar_Colonias" runat="server" AlternateText="Buscar" CssClass="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Width="24px" OnClick="Btn_Buscar_Colonias_Click" />
                        </td>
                        <td align="center" class="style7">
                            &nbsp;
                        </td>
                        <td align="center" class="style6">
                            <asp:ImageButton ID="Btn_Limpiar" runat="server" AlternateText="Limpiar" CssClass="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" OnClick="Btn_Limpiar_Click"
                                Width="24px" />
                        </td>
                        <td align="center" class="style2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            &nbsp;
                        </td>
                        <td class="style8">
                            &nbsp;
                        </td>
                        <td class="style3">
                            &nbsp;
                        </td>
                        <td class="style7">
                            &nbsp;
                        </td>
                        <td class="style3">
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
                        <td class="style8" style="text-align: left; font-size: 12px;">
                            Colonia
                        </td>
                        <td class="style3" style="width: 50%; text-align: left; font-size: 11px;">
                            <asp:HiddenField ID="Hdn_Colonia_ID" runat="server" />
                            <asp:TextBox ID="Txt_Busqueda_Colonia" runat="server" Width="80%" />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Colonia" runat="server"
                                FilterType="Numbers, LowercaseLetters, UppercaseLetters, CUSTOM" ValidChars=" ÁáÉéÍíÓóÚúÑñ"
                                TargetControlID="Txt_Busqueda_Colonia" />
                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Colonia"
                                runat="server" TargetControlID="Txt_Busqueda_Colonia" WatermarkText="Búsqueda de Colonias por Aproximación"
                                WatermarkCssClass="watermarked" />
                        </td>
                        <td class="style7">
                            Clave
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="Txt_Busqueda_Clave_Colonia" runat="server" Width="70%" />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Clave_Colonia"
                                runat="server" FilterType="Numbers"
                                TargetControlID="Txt_Busqueda_Clave_Colonia" />
                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Clave_Colonia"
                                runat="server" TargetControlID="Txt_Busqueda_Clave_Colonia" WatermarkText="Búsqueda por Clave"
                                WatermarkCssClass="watermarked" />
                        </td>
                        <td class="style2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            &nbsp;
                        </td>
                        <td class="style8" style="text-align: left; font-size: 12px;">
                            &nbsp;
                        </td>
                        <td class="style3" style="width: 40%; text-align: left; font-size: 11px;">
                            &nbsp;
                        </td>
                        <td class="style7" style="text-align: left; font-size: 12px;">
                            &nbsp;
                        </td>
                        <td class="style3" style="width: 40%; text-align: left; font-size: 11px;">
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
                        <td class="style8">
                            &nbsp;
                        </td>
                        <td class="style3">
                            &nbsp;
                        </td>
                        <td class="style7">
                            &nbsp;
                        </td>
                        <td class="style3">
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
                        <td colspan="4">
                            <asp:GridView ID="Grid_Colonias" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CssClass="GridView_1" GridLines="None" OnPageIndexChanging="Grid_Colonias_PageIndexChanging"
                                OnSelectedIndexChanged="Grid_Colonias_SelectedIndexChanged" PageSize="5" Style="white-space: normal"
                                Width="100%" DataKeyNames="COLONIA_ID">
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="COLONIA_ID" HeaderText="COLONIA_ID" Visible="False">
                                        <FooterStyle Font-Size="0pt" Width="0px" />
                                        <HeaderStyle Font-Size="0pt" Width="0px" />
                                        <ItemStyle Font-Size="0pt" Width="0px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE" HeaderText="Colonia">
                                        <HeaderStyle Width="30%" />
                                        <ItemStyle Width="30%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CLAVE_COLONIA" HeaderText="Clave Colonia">
                                        <HeaderStyle Width="10%" />
                                        <ItemStyle Width="10%" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
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
                        <td class="style8">
                            &nbsp;
                        </td>
                        <td class="style3">
                            &nbsp;
                        </td>
                        <td class="style7">
                            &nbsp;
                        </td>
                        <td class="style3">
                            &nbsp;
                        </td>
                        <td class="style2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>

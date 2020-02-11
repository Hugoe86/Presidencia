<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Estado_Cuenta.aspx.cs"
    Inherits="paginas_Predial_Ventanas_Emergentes_Resumen_Predial_Frm_Estado_Cuenta" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<style type="text/css">
    *
    {
        font-size: small;
        font-family: Arial;
    }
</style>
<head id="Head1" runat="server">
    <link href="../../../estilos/estilo_ajax.css" rel="stylesheet" type="text/css" />
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <title>Estado de Cuenta</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" class="estilo_fuente">
            <tr align="center">
                <td colspan="4" class="label_titulo">
                    Estado de cuenta
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;
                    <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                        Visible="false" />&nbsp;
                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                    <asp:Label ID="Lbl_Encabezado_Error" runat="server" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                </td>
            </tr>
        </table>
        <table width="98%" border="0" cellspacing="0">
        </table>
        <br />
        <table width="98%" class="estilo_fuente">
            <tr>
                <td style="text-align: left; width: 20%;">
                    Cuenta predial
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="96.4%" />
                </td>
                <td style="text-align: left; width: 20%;">
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
            <%------------------ Propietario ------------------%>
            <tr style="background-color: #36C;">
                <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="3">
                    Propietario
                </td>
                <td style="text-align: right; font-size: 15px; color: #FFF;" colspan="1">
                    <asp:ImageButton ID="Img_Btn_Imprimir_Estado_Cuenta" runat="server" ToolTip="Imprimir"
                        CssClass="Img_Button" TabIndex="4" OnClick="Img_Btn_Imprimir_Estado_Cuenta_Click"
                        ImageUrl="~/paginas/imagenes/gridview/grid_print.png" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Nombre
                </td>
                <td style="text-align: left; width: 30%;" colspan="3">
                    <asp:TextBox ID="Txt_Nombre_Propietario" runat="server" Width="96.4%" TextMode="MultiLine" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    RFC
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Rfc_Propietario" runat="server" Width="96.4%" />
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
            <%------------------ Bimestral ------------------%>
            <tr style="background-color: #36C;">
                <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                    Adeudos por bimestre
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Periodo inicial
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Periodo_Inicial" runat="server" Width="96.4%" />
                </td>
                <td style="text-align: right; width: 20%;">
                    Periodo final
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:DropDownList ID="Cmb_Bimestre" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Bimestre_SelectedIndexChanged">
                        <asp:ListItem Value="1">01</asp:ListItem>
                        <asp:ListItem Value="2">02</asp:ListItem>
                        <asp:ListItem Value="3">03</asp:ListItem>
                        <asp:ListItem Value="4">04</asp:ListItem>
                        <asp:ListItem Value="5">05</asp:ListItem>
                        <asp:ListItem Value="6" Selected="True">06</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="Cmb_Anio" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Anio_SelectedIndexChanged"
                        Width="60%">
                    </asp:DropDownList>
                </td>
            </tr>
            <caption>
                <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
            </caption>
            <tr align="center">
                <td colspan="4">
                    <asp:GridView ID="Grid_Estado_Cuenta" runat="server" AllowPaging="True" CssClass="GridView_1"
                        AutoGenerateColumns="False" PageSize="6" Width="100%" OnPageIndexChanging="Grid_Estado_Cuenta_PageIndexChanging"
                        OnRowDataBound="Grid_Estado_Cuenta_RowDataBound" AllowSorting="True" HeaderStyle-CssClass="tblHead"
                        Style="white-space: normal;">
                        <Columns>
                            <asp:BoundField DataField="Anio" HeaderText="Año">
                                <HeaderStyle HorizontalAlign="center" Width="10%" />
                                <ItemStyle HorizontalAlign="center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Adeudo_Bimestre_1" HeaderText="1" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" Width="10%" />
                                <ItemStyle HorizontalAlign="center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Adeudo_Bimestre_2" HeaderText="2" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" Width="10%" />
                                <ItemStyle HorizontalAlign="center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Adeudo_Bimestre_3" HeaderText="3" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" Width="10%" />
                                <ItemStyle HorizontalAlign="center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Adeudo_Bimestre_4" HeaderText="4" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" Width="10%" />
                                <ItemStyle HorizontalAlign="center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Adeudo_Bimestre_5" HeaderText="5" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" Width="10%" />
                                <ItemStyle HorizontalAlign="center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Adeudo_Bimestre_6" HeaderText="6" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" Width="10%" />
                                <ItemStyle HorizontalAlign="center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Adeudo_Total_Anio" HeaderText="Total" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" Width="25%" />
                                <ItemStyle HorizontalAlign="center" Width="25%" />
                            </asp:BoundField>
                        </Columns>
                        <SelectedRowStyle CssClass="GridSelected" />
                        <PagerStyle CssClass="GridHeader" />
                        <HeaderStyle CssClass="tblHead" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Periodo rezago
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Periodo_Rezago" runat="server" Width="96.4%" />
                </td>
                <td style="text-align: right; width: 20%;">
                    Adeudo rezago
                </td>
                <td>
                    <asp:TextBox ID="Txt_Adeudo_Rezago" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Periodo actual
                </td>
                <td style="text-align: left; width: 20%;">
                    <asp:TextBox ID="Txt_Periodo_Actual" runat="server" Width="96.4%" />
                </td>
                <td style="text-align: right; width: 20%;">
                    Adeudo actual
                </td>
                <td style="text-align: left; width: 20%;">
                    <asp:TextBox ID="Txt_Adeudo_Actual" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                </td>
                <td style="text-align: left; width: 30%;">
                </td>
                <td style="text-align: right; width: 20%;">
                    <%--Adeudo rezago--%>
                    Honorarios
                </td>
                <td style="text-align: left; width: 30%;">
                    <%--<asp:TextBox ID="Txt_Adeudo_Rezago" runat="server" Width="96.4%" />--%>
                    <asp:TextBox ID="Txt_Honorarios" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                </td>
                <td style="text-align: left; width: 30%;">
                </td>
                <td style="text-align: right; width: 20%;">
                    <%-- Adeudo actual--%>
                    Gastos de ejecución
                </td>
                <td style="text-align: left; width: 30%;">
                    <%--<asp:TextBox ID="Txt_Adeudo_Actual" runat="server" Width="96.4%" />--%>
                    <asp:TextBox ID="Txt_Gastos_Ejecucion" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td style="text-align: right; width: 30%;">
                    Total recargos ordinarios
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Total_Recargos_Ordinarios" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td style="text-align: right; width: 30%;">
                    Recargos moratorios
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Recargos_Moratorios" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td style="text-align: right; width: 20%;">
                    Subtotal
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Subtotal" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                </td>
                <td style="text-align: left; width: 30%;">
                </td>
                <td style="text-align: right; width: 20%;">
                    Descuento por pronto pago
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Descuento_Pronto_Pago" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;" colspan="2">
                    &nbsp;
                </td>
                <td style="text-align: right; width: 20%;">
                    Descuento recargos ordinarios
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Descuento_Recargos_Ordinarios" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    &nbsp;
                </td>
                <td>
                </td>
                <td style="width: 20%; text-align: right;">
                    Descuento recargos moratorios
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Descuento_Recargos_Moratorios" runat="server" Width="96.4%" />
                </td>
            </tr>
            <%--<tr>
                        <td style="text-align:left;width:20%;">
                            &nbsp;
                        </td>
                        <td>
                        </td>
                        <td style="text-align:right;width:20%;" >
                            Descuento honorarios 
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Descuento_Honorarios" runat="server" Width="96.4%" />
                        </td>
                    </tr>--%>
            <tr>
                <td style="text-align: left; width: 20%;">
                    &nbsp;
                </td>
                <td>
                </td>
                <td style="text-align: right; width: 20%; border-top: solid 1px BLACK">
                    Total
                </td>
                <td style="text-align: left; width: 30%; border-top: solid 1px BLACK">
                    <asp:TextBox ID="Txt_Total" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    &nbsp;
                </td>
                <td style="width: 20%; text-align: right;" colspan="2">
                </td>
                <td style="text-align: left; width: 30%;">
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="Hdn_Propietario_ID" runat="server" />
        <asp:HiddenField ID="Hdn_Tasa_ID" runat="server" />
        <asp:HiddenField ID="Hdn_Cuenta_Predial_ID" runat="server" />
        <asp:HiddenField ID="Hdn_Cuenta_Predial" runat="server" />
        <asp:HiddenField ID="Hdn_Cuota_Anual" runat="server" />
        <asp:TextBox ID="Txt_Tipo_Predio_General" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="Txt_Uso_Predio_General" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="Txt_Estado_Propietario" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="Txt_Tasa_General" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="Txt_Calle_Propietario" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="Txt_Numero_Exterior_Propietario" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="Txt_Ubicacion_General" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="Txt_Colonia_General" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="Txt_Numero_Exterior_General" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="Txt_Numero_Interior_General" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="Txt_Efectos_General" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="Txt_Valor_Fiscal_General" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="Txt_Cuota_Bimestral_General" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="Txt_Cod_Pos_Propietario" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="Txt_Numero_Interior_Propietario" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="Txt_Colonia_Propietario" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="Txt_Ciudad_Propietario" runat="server" Visible="false"></asp:TextBox>
    </div>
    </form>
</body>
</html>

<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="Frm_Convenios_Predial.aspx.cs"
    Inherits="paginas_Predial_Ventanas_Emergentes_Convenios_Frm_Convenios_Predial" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Convenios de Impuestos Predial</title>
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../../../estilos/estilo_ajax.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        *
        {
            font-size: small;
            font-family: Arial;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" EnablePartialRendering="true">
    </asp:ScriptManager>
    <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
        <table width="100%" class="estilo_fuente">
            <tr style="text-align:center">
                <td colspan="4" class="label_titulo">
                    Convenios de Impuesto Predial
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;
                    <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                        Visible="false" />&nbsp;
                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
        <asp:HiddenField ID="Hdf_Propietario_ID" runat="server" />
        <asp:HiddenField ID="Hdf_RFC_Propietario" runat="server" />
        <asp:HiddenField ID="Hdf_RFC_Solicitante" runat="server" />
        <asp:HiddenField ID="Hdf_Solicitante" runat="server" />
        <asp:HiddenField ID="Hdf_No_Convenio" runat="server" />
        <asp:HiddenField ID="Hdn_Tasa_ID" runat="server" />
        <asp:HiddenField ID="Hdn_Propietario_ID" runat="server" />
        <br />
        <asp:Panel ID="Panel_Datos" runat="server">
            <table width="98%" class="estilo_fuente">
                <tr>
                    <td style="text-align: left; width: 20%;">
                        *Cuenta Predial
                    </td>
                    <td colspan="3" style="width: 82%; text-align: left;">
                        <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" AutoPostBack="true" MaxLength="20"
                            TabIndex="7" Width="35.7%"></asp:TextBox>
                        &nbsp; &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        Propietario
                    </td>
                    <td style="text-align: left; width: 20%;">
                        <asp:TextBox ID="Txt_Propietario" runat="server" ReadOnly="True" Width="96.4%" TabIndex="11" />
                    </td>
                    <td style="text-align: right; width: 20%;">
                        Colonia
                    </td>
                    <td style="text-align: left; width: 20%;">
                        <asp:TextBox ID="Txt_Colonia" runat="server" ReadOnly="True" Width="96.4%" TabIndex="12" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        Calle
                    </td>
                    <td style="text-align: left; width: 20%;">
                        <asp:TextBox ID="Txt_Calle" runat="server" ReadOnly="True" Width="96.4%" TabIndex="13" />
                    </td>
                    <td style="text-align: right; width: 20%;">
                        No. exterior
                    </td>
                    <td style="text-align: left; width: 20%;">
                        <asp:TextBox ID="Txt_No_Exterior" runat="server" ReadOnly="True" Width="96.4%" TabIndex="14" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        No. interior
                    </td>
                    <td style="text-align: left; width: 20%;">
                        <asp:TextBox ID="Txt_No_Interior" runat="server" ReadOnly="True" Width="96.4%" TabIndex="15" />
                    </td>
                </tr>
                <!------------------------------ ADEUDOS ------------------------------>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        *Al periodo
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:DropDownList ID="Cmb_Hasta_Anio_Periodo" runat="server"
                            TabIndex="16" Style="width: 65px; text-align: center;">
                            <asp:ListItem Value="2011">2011</asp:ListItem>
                            <asp:ListItem Value="2010">2010</asp:ListItem>
                            <asp:ListItem Value="2009">2009</asp:ListItem>
                            <asp:ListItem Value="2008">2008</asp:ListItem>
                            <asp:ListItem Value="2007">2007</asp:ListItem>
                            <asp:ListItem Value="2006">2006</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="Cmb_Hasta_Bimestre_Periodo" runat="server"
                            TabIndex="17" Style="width: 40px; text-align: center;">
                            <asp:ListItem Value="1">1</asp:ListItem>
                            <asp:ListItem Value="2">2</asp:ListItem>
                            <asp:ListItem Value="3">3</asp:ListItem>
                            <asp:ListItem Value="4">4</asp:ListItem>
                            <asp:ListItem Value="5">5</asp:ListItem>
                            <asp:ListItem Value="6">6</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: left; width: 20%; text-align: right;">
                        Saldo predial
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Monto_Impuesto" runat="server" ReadOnly="True" Width="96.4%" TabIndex="18"
                            Style="text-align: right;" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        Adeudo recargos
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Monto_Recargos" runat="server" ReadOnly="True" Width="96.4%"
                            TabIndex="20" Style="text-align: right;" />
                    </td>
                    <td style="text-align: left; width: 20%; text-align: right;">
                        Recargos moratorios
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Monto_Moratorios" runat="server" ReadOnly="True" Enabled="false"
                            Width="96.4%" Style="text-align: right;" TabIndex="21" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        Adeudo honorarios
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Adeudo_Honorarios" runat="server" ReadOnly="True" Enabled="false"
                            Width="96.4%" Style="text-align: right;" TabIndex="22" />
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                
                <%------------------ Convenio ------------------%>
                <tr style="background-color: #3366CC">
                    <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                        Convenio
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        Número convenio
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Numero_Convenio" runat="server" ReadOnly="True" Width="96.4%"
                            TabIndex="23" />
                    </td>
                    <td style="text-align: left; width: 20%; text-align: right;">
                        Estatus
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%" TabIndex="24">
                            <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                <asp:ListItem Text="ACTIVO" Value="ACTIVO" />
                                <asp:ListItem Text="PENDIENTE" Value="PENDIENTE" />
                                <asp:ListItem Text="CANCELADO" Value="CANCELADO" />
                                <asp:ListItem Text="TERMINADO" Value="TERMINADO" /> 
                                <asp:ListItem Text="INCUMPLIDO" Value="INCUMPLIDO" />
                                <asp:ListItem Text="CUENTA CANCELADA" Value="CUENTA_CANCELADA" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        *Tipo solicitante
                    </td>
                    <td style="text-align: left; width: 30%;" colspan="3">
                        <asp:DropDownList ID="Cmb_Tipo_Solicitante" runat="server" Width="99.6%"
                            TabIndex="25">
                            <asp:ListItem>PROPIETARIO</asp:ListItem>
                            <asp:ListItem>DEUDOR SOLIDARIO</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%; vertical-align: top;">
                        *Solicitante
                    </td>
                    <td colspan="3" style="text-align: left; width: 80%; vertical-align: top;">
                        <asp:TextBox ID="Txt_Solicitante" runat="server" TabIndex="26" TextMode="SingleLine"
                            Width="98.6%" MaxLength="256" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        RFC
                    </td>
                    <td style="text-align: left; width: 30%;" colspan="3">
                        <asp:TextBox ID="Txt_RFC" runat="server" Width="98.6%" TabIndex="27" MaxLength="15" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        *Periodicidad de pago
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:DropDownList ID="Cmb_Periodicidad_Pago" runat="server" Width="99%" TabIndex="28" >
                            <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                            <asp:ListItem Value="7">Semanal</asp:ListItem>
                            <asp:ListItem Value="14">Catorcenal</asp:ListItem>
                            <asp:ListItem Value="15">Quincenal</asp:ListItem>
                            <asp:ListItem Value="30">Mensual</asp:ListItem>
                            <asp:ListItem Value="365">Anual</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: left; width: 20%; text-align: right;">
                        *Número de parcialidades
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Numero_Parcialidades" runat="server" Width="96.4%" 
                            TabIndex="29" MaxLength="3" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        Realizó
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Realizo" runat="server" ReadOnly="True" Width="96.4%" TabIndex="30" />
                    </td>
                    <td style="text-align: left; width: 20%; text-align: right;">
                        Fecha
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Fecha_Convenio" runat="server" ReadOnly="True" Width="96.4%"
                            TabIndex="31" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        &nbsp;
                    </td>
                    <td style="text-align: left; width: 30%;">
                        &nbsp;
                    </td>
                    <td style="text-align: left; width: 20%; text-align: right;">
                        Fecha vencimiento
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Fecha_Vencimiento" runat="server" ReadOnly="True" Width="96.4%"
                            TabIndex="32" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%; vertical-align: top;">
                        Observaciones
                    </td>
                    <td colspan="3" style="text-align: left; width: 80%;">
                        <asp:TextBox ID="Txt_Observaciones" runat="server" TabIndex="33" MaxLength="250"
                            TextMode="MultiLine" Width="98.6%" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr>
                <%------------------ Descuentos ------------------%>
                <tr style="background-color: #3366CC">
                    <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                        Descuentos
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        Descuento recargos ordinarios
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Descuento_Recargos_Ordinarios" runat="server" Width="96.4%"
                            MaxLength="10" TabIndex="34" Style="text-align: right;" />
                    </td>
                    <td style="text-align: right; width: 20%;">
                        Descuento recargos moratorios
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Descuento_Recargos_Moratorios" runat="server" Width="96.4%"
                            MaxLength="10" TabIndex="35" Style="text-align: right;" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        &nbsp;
                    </td>
                    <td style="text-align: right; width: 20%; border-top: solid 1px BLACK;" colspan="3">
                        Total Adeudo
                        <asp:TextBox ID="Txt_Total_Adeudo" runat="server" Width="100px" Style="text-align: right;"
                            TabIndex="36" />
                        - Total Descuento
                        <asp:TextBox ID="Txt_Total_Descuento" runat="server" Width="100px" Style="text-align: right;"
                            TabIndex="37" />
                        = Sub-Total
                        <asp:TextBox ID="Txt_Sub_Total" runat="server" Width="100px" Style="text-align: right;"
                            TabIndex="38" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr>
                <%------------------ Parcialidades ------------------%>
                <tr style="background-color: #3366CC">
                    <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                        Parcialidades
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;" colspan="4">
                        Porcentaje Anticipo
                        <asp:TextBox ID="Txt_Porcentaje_Anticipo" runat="server" Width="60px" TabIndex="39"
                            Style="text-align: right;" MaxLength="5" />
                        Total Anticipo
                        <asp:TextBox ID="Txt_Total_Anticipo" runat="server" Width="200px" TabIndex="40"
                            Style="text-align: right;" MaxLength="20" />
                        Saldo
                        <asp:TextBox ID="Txt_Total_Convenio" runat="server" TabIndex="41" Width="200px" Style="text-align: right;" />
                    </td>
                    <td style="text-align: right; width: 20%;">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                        <asp:GridView ID="Grid_Parcialidades" runat="server" AutoGenerateColumns="False"
                            CssClass="GridView_1" HeaderStyle-CssClass="tblHead" ShowFooter="True" Style="white-space: normal;"
                            Width="100%">
                            <Columns>
                                <asp:BoundField DataField="NO_PAGO" FooterText="Total" HeaderText="No. Pago">
                                    <FooterStyle Font-Bold="True" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PERIODO" HeaderText="Periodo" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="MONTO_HONORARIOS" HeaderText="Honorarios" DataFormatString="{0:c2}"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true"
                                    FooterStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="RECARGOS_ORDINARIOS" DataFormatString="{0:c2}" HeaderText="Recargos Ordinarios"
                                    HeaderStyle-Width="12%">
                                    <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RECARGOS_MORATORIOS" DataFormatString="{0:c2}" HeaderText="Recargos Moratorios"
                                    HeaderStyle-Width="12%">
                                    <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MONTO_IMPUESTO" DataFormatString="{0:c2}" HeaderText="Impuesto">
                                    <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MONTO_IMPORTE" DataFormatString="{0:c2}" HeaderText="Importe">
                                    <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FECHA_VENCIMIENTO" DataFormatString="{0:dd/MMM/yyyy}"
                                    HeaderText="Fecha Vencimiento">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                            </Columns>
                            <SelectedRowStyle CssClass="GridSelected" />
                            <PagerStyle CssClass="GridHeader" />
                            <HeaderStyle CssClass="tblHead" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                        </asp:GridView>
                        <br />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>

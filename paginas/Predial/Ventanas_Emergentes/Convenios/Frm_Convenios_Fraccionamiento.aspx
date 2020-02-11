<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="Frm_Convenios_Fraccionamiento.aspx.cs"
    Inherits="paginas_Predial_Ventanas_Emergentes_Convenios_Frm_Convenios_Fraccionamiento"
    Culture="es-MX" %>

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
    <title>Convenios de Impuestos de Fraccionamientos</title>
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../../../estilos/estilo_ajax.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" EnablePartialRendering="true">
    </asp:ScriptManager>
    <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
        <table width="100%" class="estilo_fuente">
            <tr align="center">
                <td class="label_titulo">
                    Convenios a Impuesto por Fraccionamiento
                </td>
            </tr>
            <tr>
                <td>
                    <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../../../imagenes/paginas/sias_warning.png"
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
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
        <asp:HiddenField ID="Hdf_Propietario_ID" runat="server" />
        <asp:HiddenField ID="Hdf_RFC" runat="server" />
        <asp:HiddenField ID="Hdf_No_Convenio" runat="server" />
        <asp:HiddenField ID="Hdf_No_Impuesto_Fraccionamiento" runat="server" />
        <br />
        <asp:Panel ID="Panel_Datos" runat="server">
            <table width="98%" class="estilo_fuente">
                <tr>
                    <td style="text-align: left; width: 20%;">
                        *Cuenta Predial
                    </td>
                    <td colspan="3" style="width: 82%; text-align: left;">
                        <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" AutoPostBack="true" MaxLength="20"
                            TabIndex="9" Width="35.7%"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cuenta_Predial" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers"
                            TargetControlID="Txt_Cuenta_Predial" />
                        &nbsp;
                        <asp:Button Style="background-color: transparent; border-style: none;" ID="Btn_Comodin_Close"
                            runat="server" Text="" />
                        <asp:Button Style="background-color: transparent; border-style: none;" ID="Btn_Comodin_Open"
                            runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        Propietario
                    </td>
                    <td style="text-align: left; width: 20%;">
                        <asp:TextBox ID="Txt_Propietario" runat="server" ReadOnly="True" Width="96.4%" />
                    </td>
                    <td style="text-align: right; width: 20%;">
                        Colonia
                    </td>
                    <td style="text-align: left; width: 20%;">
                        <asp:TextBox ID="Txt_Colonia" runat="server" ReadOnly="True" Width="96.4%" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        Calle
                    </td>
                    <td style="text-align: left; width: 20%;">
                        <asp:TextBox ID="Txt_Calle" runat="server" ReadOnly="True" Width="96.4%" />
                    </td>
                    <td style="text-align: right; width: 20%;">
                        No. exterior
                    </td>
                    <td style="text-align: left; width: 20%;">
                        <asp:TextBox ID="Txt_No_Exterior" runat="server" ReadOnly="True" Width="96.4%" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        No. interior
                    </td>
                    <td style="text-align: left; width: 20%;">
                        <asp:TextBox ID="Txt_No_Interior" runat="server" ReadOnly="True" Width="96.4%" />
                    </td>
                    <td style="text-align: left; width: 20%; text-align: right;">
                        Tipo Fraccionamiento
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Tipo_Fraccionamiento" runat="server" ReadOnly="True" Width="96.4%" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        Monto impuesto
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Monto_Impuesto" runat="server" ReadOnly="True" Width="96.4%" />
                        <cc1:MaskedEditExtender ID="Mee_Txt_Monto_Impuesto" runat="server" TargetControlID="Txt_Monto_Impuesto"
                            Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left"
                            ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                            CultureTimePlaceholder="" Enabled="True" />
                    </td>
                    <td style="text-align: left; width: 20%; text-align: right;">
                        Monto honorarios
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Honorarios" runat="server" ReadOnly="True" Width="96.4%" Enabled="false" />
                        <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="Txt_Honorarios"
                            Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left"
                            ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                            CultureTimePlaceholder="" Enabled="True" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%; text-align: left;">
                        Monto multas
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Monto_Multas" runat="server" ReadOnly="True" Width="96.4%" />
                        <cc1:MaskedEditExtender ID="Mee_Txt_Monto_Multas" runat="server" TargetControlID="Txt_Monto_Multas"
                            Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left"
                            ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                            CultureTimePlaceholder="" Enabled="True" />
                    </td>
                    <td style="text-align: right; width: 20%;">
                        Monto recargos
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Monto_Recargos" runat="server" ReadOnly="True" Width="96.4%" />
                        <cc1:MaskedEditExtender ID="Mee_Txt_Monto_Recargos" runat="server" TargetControlID="Txt_Monto_Recargos"
                            Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left"
                            ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                            CultureTimePlaceholder="" Enabled="True" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%; text-align: left;">
                        Fecha cálculo
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Fecha_Calculo" runat="server" ReadOnly="True" Width="96.4%" />
                        <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Fecha_Calculo" runat="server" TargetControlID="Txt_Fecha_Calculo"
                            WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                        <cc1:CalendarExtender ID="Dtp_Txt_Fecha_Calculo" runat="server" TargetControlID="Txt_Fecha_Calculo"
                            PopupButtonID="" Format="dd/MMM/yyyy">
                        </cc1:CalendarExtender>
                    </td>
                    <td style="text-align: right; width: 20%;">
                        Realizó cálculo
                    </td>
                    <td style="text-align: right; width: 30%;">
                        <asp:TextBox ID="Txt_Realizo_Calculo" runat="server" ReadOnly="True" Width="96.4%" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%; text-align: left;">
                        Fecha vencimiento
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Fecha_Vencimiento" runat="server" ReadOnly="True" Width="96.4%" />
                    </td>
                    <td colspan="3">
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
                        <asp:TextBox ID="Txt_Numero_Convenio" runat="server" ReadOnly="True" Width="96.4%" />
                    </td>
                    <td style="text-align: left; width: 20%; text-align: right;">
                        Estatus
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%" TabIndex="7">
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
                        <asp:DropDownList ID="Cmb_Tipo_Solicitante" runat="server" Width="99.6%" onChange="Validar_Tipo_Solicitante();"
                            TabIndex="7">
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
                        <asp:TextBox ID="Txt_Solicitante" runat="server" TabIndex="10" TextMode="SingleLine"
                            Width="98.6%" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        RFC
                    </td>
                    <td style="text-align: left; width: 30%;" colspan="3">
                        <asp:TextBox ID="Txt_RFC" runat="server" Width="98.6%" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        *Periodicidad de pago
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:DropDownList ID="Cmb_Periodicidad_Pago" runat="server" Width="99%" TabIndex="7"
                            AutoPostBack="True" OnSelectedIndexChanged="Cmb_Periodicidad_Pago_SelectedIndexChanged">
                            <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                            <asp:ListItem Value="7">Semanal</asp:ListItem>
                            <asp:ListItem Value="14">Catorcenal</asp:ListItem>
                            <asp:ListItem Value="15">Quincenal</asp:ListItem>
                            <asp:ListItem Value="Mensual">Mensual</asp:ListItem>
                            <asp:ListItem>Anual</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: left; width: 20%; text-align: right;">
                        *Número de parcialidades
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Numero_Parcialidades" runat="server" Width="96.4%" AutoPostBack="True"
                            OnTextChanged="Txt_Numero_Parcialidades_TextChanged" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        Realizó
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Realizo" runat="server" ReadOnly="True" Width="96.4%" />
                    </td>
                    <td style="text-align: left; width: 20%; text-align: right;">
                        Fecha convenio
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Fecha_Convenio" runat="server" ReadOnly="True" Width="96.4%" />
                        <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Fecha" runat="server" TargetControlID="Txt_Fecha_Convenio"
                            WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                        <cc1:CalendarExtender ID="DtpTxt_Fecha" runat="server" TargetControlID="Txt_Fecha_Convenio"
                            Format="dd/MMM/yyyy">
                        </cc1:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        &nbsp;
                    </td>
                    <td style="text-align: left; width: 30%;">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%; vertical-align: top;">
                        Observaciones
                    </td>
                    <td colspan="3" style="text-align: left; width: 80%;">
                        <asp:TextBox ID="Txt_Observaciones" runat="server" MaxLength="250" TabIndex="10"
                            TextMode="MultiLine" Width="98.6%" />
                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                            TargetControlID="Txt_Observaciones" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
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
                        Descuento recargos ordinarios (%)
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Descuento_Recargos_Ordinarios" runat="server" Width="96.4%"
                            OnTextChanged="Txt_Descuento_Recargos_Ordinarios_TextChanged" AutoPostBack="True" />
                    </td>
                    <td style="text-align: right; width: 20%;">
                        Descuento recargos moratorios (%)
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Descuento_Recargos_Moratorios" runat="server" Width="96.4%"
                            OnTextChanged="Txt_Descuento_Recargos_Moratorios_TextChanged" AutoPostBack="True" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        Descuento multas
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Descuento_Multas" runat="server" Width="96.4%" OnTextChanged="Txt_Descuento_Multas_TextChanged"
                            AutoPostBack="True" />
                    </td>
                    <td style="text-align: right; width: 20%;" colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        &nbsp;
                    </td>
                    <td style="text-align: right; width: 20%; border-top: solid 1px BLACK;" colspan="3">
                        Total Adeudo
                        <asp:TextBox ID="Txt_Total_Adeudo" runat="server" Width="100px" />
                        <cc1:MaskedEditExtender ID="Mee_Txt_Total_Adeudo" runat="server" TargetControlID="Txt_Total_Adeudo"
                            Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left"
                            ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                            CultureTimePlaceholder="" Enabled="True" />
                        - Total Descuento
                        <asp:TextBox ID="Txt_Total_Descuento" runat="server" Width="100px" />
                        <cc1:MaskedEditExtender ID="Mee_Txt_Total_Descuento" runat="server" TargetControlID="Txt_Total_Descuento"
                            Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left"
                            ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                            CultureTimePlaceholder="" Enabled="True" />
                        = Sub-Total
                        <asp:TextBox ID="Txt_Sub_Total" runat="server" Width="100px" />
                        <cc1:MaskedEditExtender ID="Mee_Txt_Sub_Total" runat="server" TargetControlID="Txt_Sub_Total"
                            Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left"
                            ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                            CultureTimePlaceholder="" Enabled="True" />
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
                        <asp:TextBox ID="Txt_Porcentaje_Anticipo" runat="server" Width="60px" AutoPostBack="True"
                            OnTextChanged="Txt_Porcentaje_Anticipo_TextChanged" />
                        Total Anticipo
                        <asp:TextBox ID="Txt_Total_Anticipo" runat="server" Width="200px" AutoPostBack="True"
                            OnTextChanged="Txt_Total_Anticipo_TextChanged" />
                        Saldo
                        <asp:TextBox ID="Txt_Total_Convenio" runat="server" Width="200px" />
                        <cc1:MaskedEditExtender ID="Mee_Txt_Total_Convenio" runat="server" TargetControlID="Txt_Total_Convenio"
                            Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left"
                            ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                            CultureTimePlaceholder="" Enabled="True" />
                    </td>
                    <td style="text-align: right; width: 20%;">
                        &nbsp;
                    </td>
                </tr>
                <tr align="center">
                    <td colspan="4">
                        <br />
                        <asp:GridView ID="Grid_Parcialidades" runat="server" AutoGenerateColumns="False"
                            HeaderStyle-CssClass="tblHead" Style="white-space: normal;" Width="100%" ShowFooter="True">
                            <Columns>
                                <asp:BoundField DataField="NO_PAGO" HeaderText="No. Pago" FooterText="Total">
                                    <FooterStyle Font-Bold="True" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="HONORARIOS" HeaderText="Honorarios" DataFormatString="{0:c2}">
                                    <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MONTO_MULTAS" HeaderText="Multas" DataFormatString="{0:c2}">
                                    <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RECARGOS_ORDINARIOS" DataFormatString="{0:c2}" HeaderText="Recargos Ordinarios">
                                    <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RECARGOS_MORATORIOS" DataFormatString="{0:c2}" HeaderText="Recargos Moratorios">
                                    <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MONTO_IMPUESTO" HeaderText="Impuesto" DataFormatString="{0:c2}">
                                    <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MONTO_IMPORTE" DataFormatString="{0:c2}" HeaderText="Importe">
                                    <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
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

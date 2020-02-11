<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="Frm_Convenios_Traslado.aspx.cs"
    Inherits="paginas_Predial_Ventanas_Emergentes_Convenios_Frm_Convenios_Traslado" %>

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
    <title>Convenios de Impuestos de Traslado</title>
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
                    Convenios al Impuesto por Traslado
                </td>
            </tr>
            <tr>
                <td>
                    <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../../imagenes/paginas/sias_warning.png"
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
        <br />
        <table width="98%" class="estilo_fuente">
            <%------------------ Contrarecibos ------------------%>
            <%--                    <tr style="background-color: #3366CC">
                        <td style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" >
                            Contrarecibos pendientes
                        </td>
                    </tr>
--%>
            <tr align="center">
                <td colspan="4">
                    <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                    <asp:HiddenField ID="Hdf_Propietario_ID" runat="server" />
                    <asp:HiddenField ID="Hdf_No_Convenio" runat="server" />
                    <asp:HiddenField ID="Hdf_No_Contrarecibo" runat="server" />
                    <asp:HiddenField ID="Hdf_RFC" runat="server" />
                    <asp:HiddenField ID="Hdf_No_Calculo" runat="server" />
                    <asp:HiddenField ID="Hdf_Anio_Calculo" runat="server" />
                    <%--                            <asp:GridView ID="Grid_Contrarecibos_Pendientes" runat="server" 
                                AllowPaging="True" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="5" Width="100%"
                                AllowSorting="True" HeaderStyle-CssClass="tblHead" 
                                style="white-space:normal;">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" HorizontalAlign="Center"/>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="c2" HeaderText="Contrarecibo" />
                                    <asp:BoundField DataField="c2" HeaderText="Cuenta predial" />
                                    <asp:BoundField DataField="c2" HeaderText="Fecha" />
                                    <asp:BoundField DataField="c2" HeaderText="Recargos" />
                                    <asp:BoundField DataField="c2" HeaderText="Descuento Recargos" 
                                        HeaderStyle-Width="13%" >
                                        <HeaderStyle Width="13%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="c2" HeaderText="Multas" />
                                    <asp:BoundField DataField="c2" HeaderText="Descuento multas" 
                                        HeaderStyle-Width="13%" >
                                        <HeaderStyle Width="13%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="c2" HeaderText="Importe" />
                                    <asp:BoundField DataField="c2" HeaderText="Estatus" />
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <HeaderStyle CssClass="tblHead" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
--%>
                    <br />
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel_Datos" runat="server">
            <table width="98%" border="0" cellspacing="0">
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
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        Propietario
                    </td>
                    <td colspan="3" style="text-align: left; width: 20%;">
                        <asp:TextBox ID="Txt_Propietario" runat="server" ReadOnly="True" Width="96.4%" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        Colonia
                    </td>
                    <td style="text-align: left; width: 20%;">
                        <asp:TextBox ID="Txt_Colonia" runat="server" ReadOnly="True" Width="96.4%" />
                    </td>
                    <td style="text-align: right; width: 20%;">
                        Calle
                    </td>
                    <td style="text-align: left; width: 20%;">
                        <asp:TextBox ID="Txt_Calle" runat="server" ReadOnly="True" Width="96.4%" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                        No. exterior
                    </td>
                    <td style="text-align: left; width: 20%;">
                        <asp:TextBox ID="Txt_No_Exterior" runat="server" ReadOnly="True" Width="96.4%" />
                    </td>
                    <td style="text-align: right; width: 20%;">
                        No. interior
                    </td>
                    <td style="text-align: left; width: 20%;">
                        <asp:TextBox ID="Txt_No_Interior" runat="server" ReadOnly="True" Width="96.4%" />
                    </td>
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
                            <asp:ImageButton ID="Btn_Detalles_Calculo" runat="server" ToolTip="Detalles del c&aacute;lculo"
                                TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Height="24px"
                                Width="24px" Style="float: left;" />
                            Estatus (Cálculo)
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Estatus_Calculo" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
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
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Monto multas
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Monto_Multas" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Constancia no adeudo
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Costo_Constancia" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Fecha cálculo
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Fecha_Calculo" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Realizó cálculo
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Realizo_Calculo" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
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
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            *Solicitante
                        </td>
                        <td colspan="3" style="text-align: left; width: 100%; vertical-align: top;">
                            <asp:TextBox ID="Txt_Solicitante" runat="server" TabIndex="10" TextMode="SingleLine"
                                Width="98.6%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            RFC
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_RFC" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            *Tipo solicitante
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:DropDownList ID="Cmb_Tipo_Solicitante" runat="server" Width="99%" onChange="Validar_Tipo_Solicitante();"
                                TabIndex="7">
                                <asp:ListItem>PROPIETARIO</asp:ListItem>
                                <asp:ListItem>DEUDOR SOLIDARIO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            *Número de parcialidades
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Numero_Parcialidades" runat="server" Width="96.4%" AutoPostBack="True"
                                OnTextChanged="Txt_Numero_Parcialidades_TextChanged" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            *Periodicidad de pago
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:DropDownList ID="Cmb_Periodicidad_Pago" runat="server" Width="99%" TabIndex="7"
                                OnSelectedIndexChanged="Cmb_Periodicidad_Pago_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                <asp:ListItem Value="7">Semanal</asp:ListItem>
                                <asp:ListItem Value="14">Catorcenal</asp:ListItem>
                                <asp:ListItem Value="15">Quincenal</asp:ListItem>
                                <asp:ListItem Value="Mensual">Mensual</asp:ListItem>
                                <asp:ListItem>Anual</asp:ListItem>
                            </asp:DropDownList>
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
                            Fecha
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Fecha" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Observaciones
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="Txt_Observaciones" runat="server" TabIndex="10" MaxLength="250"
                                TextMode="MultiLine" Width="98.6%" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios" runat="server" TargetControlID="Txt_Observaciones"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
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
                                AutoPostBack="True" OnTextChanged="Txt_Descuento_Recargos_Ordinarios_TextChanged" />
                        </td>
                        <td style="text-align: right; width: 20%;">
                            Descuento recargos moratorios (%)
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Descuento_Recargos_Moratorios" runat="server" Width="96.4%"
                                AutoPostBack="True" OnTextChanged="Txt_Descuento_Recargos_Moratorios_TextChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Descuento multas (%)
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Descuento_Multas" runat="server" Width="96.4%" AutoPostBack="True"
                                OnTextChanged="Txt_Descuento_Multas_TextChanged" />
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
                            <cc1:MaskedEditExtender ID="Mee_Txt_Total_Convenio" runat="server" TargetControlID="Txt_Sub_Total"
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
                        <td style="text-align: left;" colspan="4">
                            Porcentaje Anticipo
                            <asp:TextBox ID="Txt_Porcentaje_Anticipo" runat="server" Width="60px" AutoPostBack="True"
                                OnTextChanged="Txt_Porcentaje_Anticipo_TextChanged" />
                            Total Anticipo
                            <asp:TextBox ID="Txt_Total_Anticipo" runat="server" Width="200px" AutoPostBack="True"
                                OnTextChanged="Txt_Total_Anticipo_TextChanged" />
                            &nbsp;Total Convenio
                            <asp:TextBox ID="Txt_Total_Convenio" runat="server" Width="200px" />
                            <cc1:MaskedEditExtender ID="Mee_Txt_Total_Convenio0" runat="server" AcceptNegative="Left"
                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" InputDirection="RightToLeft"
                                Mask="9,999,999.99" MaskType="Number" TargetControlID="Txt_Total_Convenio" />
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="4">
                            <br />
                            <asp:GridView ID="Grid_Parcialidades" runat="server" AutoGenerateColumns="False"
                                CssClass="GridView_1" ShowFooter="True" HeaderStyle-CssClass="tblHead" Style="white-space: normal;"
                                Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="NO_PAGO" FooterText="Total" HeaderText="No. Pago">
                                        <FooterStyle Font-Bold="True" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="HONORARIOS" DataFormatString="{0:c2}" HeaderText="Honorarios">
                                        <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CONSTANCIA" DataFormatString="{0:c2}" HeaderText="Constancia">
                                        <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MONTO_MULTAS" DataFormatString="{0:c2}" HeaderText="Multas">
                                        <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RECARGOS_ORDINARIOS" DataFormatString="{0:c2}" HeaderText="Recargos Ordinarios">
                                        <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RECARGOS_MORATORIOS" DataFormatString="{0:c2}" HeaderText="Recargos Moratorios">
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
        <br />
    </div>
    </form>
</body>
</html>

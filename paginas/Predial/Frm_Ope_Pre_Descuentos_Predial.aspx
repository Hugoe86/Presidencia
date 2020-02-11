<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Descuentos_Predial.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Descuentos_Predial"
    UICulture="es-MX" Culture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType TypeName="Paginas_Generales_paginas_MasterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .Tabla_Comentarios
        {
            border-collapse: collapse;
            margin-left: 25px;
            color: #25406D;
            font-family: Verdana,Geneva,MS Sans Serif;
            font-size: small;
            text-align: left;
        }
        .Tabla_Comentarios, .Tabla_Comentarios th, .Tabla_Comentarios td
        {
            border: 1px solid #999999;
            padding: 2px 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type="text/javascript" language="javascript">
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_sesiones.ashx";

        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion() {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }

        //Temporizador para matener la sesión activa
        setInterval('MantenSesion()', "<%=(int)(0.9*(Session.Timeout * 60000))%>");

        //Metodos para abrir los Modal PopUp's de la página
        function Abrir_Busqueda_Cuentas_Predial() {
            $find('Busqueda_Cuentas_Predial').show();
            return false;
        }
        
        function Abrir_Ventana_Modal(Url, Propiedades)
		{
			window.showModalDialog(Url, null, Propiedades);
		}

	    function cancelBack() {
	        if (window.event) {
	            if (event.keyCode == 8 && (event.srcElement.form == null || event.srcElement.isTextEdit == false)) {
	                event.cancelBubble = true;
	                event.returnValue = false;
	        }
	    }
	}
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(initRequest);
        function initRequest(sender, args) {
            if (Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack())
                args.set_cancel(true);
        }   
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server" EnableScriptLocalization="true"
        EnableScriptGlobalization="true" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Descuentos Impuestos Predial
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />
                            &nbsp;
                            <asp:Label ID="Lbl_Encabezado_Error" runat="server" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td>
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click" />
                                            <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                                                TabIndex="3" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" Visible="false"
                                                OnClick="Btn_Imprimir_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            <table style="width: 100%; height: 28px;">
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                        B&uacute;squeda:
                                                    </td>
                                                    <td style="width: 55%;">
                                                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar"
                                                            Width="180px" />
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ/" />
                                                    </td>
                                                    <td style="vertical-align: middle; width: 5%;">
                                                        <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                            ToolTip="Buscar" OnClick="Btn_Buscar_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align: left;">
                            <asp:GridView ID="Grid_Descuentos_Predial" runat="server" Style="white-space: normal;"
                                AutoGenerateColumns="False" PageSize="10" Width="100%" AllowPaging="true" OnSelectedIndexChanged="Grid_Descuentos_Predial_SelectedIndexChanged"
                                OnPageIndexChanging="Grid_Descuentos_Predial_PageIndexChanging">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="NO_DESCUENTO_PREDIAL" HeaderText="No Descuento">
                                        <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                        <ItemStyle HorizontalAlign="Left" Width="12%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="Cuenta_ID" />
                                    <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial">
                                        <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                        <ItemStyle HorizontalAlign="Left" Width="12%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                        <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                        <ItemStyle HorizontalAlign="Center" Width="12%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESC_RECARGO" HeaderText="Desc Recargo" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                        <ItemStyle HorizontalAlign="Right" Width="12%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESC_RECARGO_MORATORIO" HeaderText="Desc Recargo Moratorio"
                                        DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                        <ItemStyle HorizontalAlign="Right" Width="12%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TOTAL_A_PAGAR" HeaderText="Total Pagar" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                        <ItemStyle HorizontalAlign="Right" Width="12%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}">
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                            </asp:GridView>
                            <br />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Pnl_Descuentos" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Cuenta_Predial" runat="server">*Cuenta Predial</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Enabled="false" MaxLength="20"
                                    TabIndex="7" Width="96.4%"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada" runat="server" ToolTip="Búsqueda Avanzada de Cuenta Predial"
                                    TabIndex="8" Height="24px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                    Width="24px" OnClick="Btn_Mostrar_Busqueda_Avanzada_Click" />
                                <asp:Label ID="Lbl_Mensaje_Cuenta_Predial" runat="server" Text="" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Nombre_Propietario" runat="server" Text="Propietario"></asp:Label>
                            </td>
                            <td style="text-align: left;" colspan="3">
                                <asp:TextBox ID="Txt_Nombre_Propietario" runat="server" Width="99%" TabIndex="9"
                                    Style="float: left" />
                                <asp:HiddenField ID="Hdn_Propietario_ID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Txt_Ubicacion" runat="server">Ubicación</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Ubicacion" runat="server" Width="99%" TabIndex="10" Enabled="false"
                                    TextMode="MultiLine" />
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
                                Estatus
                            </td>
                            <td style="text-align: right; width: 30%;">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%" TabIndex="11" Enabled="false">
                                    <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE" />
                                    <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                    <asp:ListItem Text="APLICADO" Value="APLICADO" />
                                    <asp:ListItem Text="PENDIENTE" Value="PENDIENTE" />
                                    <asp:ListItem Text="CANCELADO" Value="CANCELADO" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Periodo_Inicial" runat="server">Desde periodo</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Periodo_Inicial" runat="server" Width="96.4%" TabIndex="12"></asp:TextBox>
                            </td>
                            <td style="width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Periodo_Final" runat="server">Hasta periodo</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Hasta_Bimestre_Periodo" runat="server" OnSelectedIndexChanged="Cmb_Hasta_Periodo_SelectedIndexChanged"
                                    AutoPostBack="true" TabIndex="13" Style="width: 40px; text-align: center;">
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="Cmb_Hasta_Anio_Periodo" runat="server" OnSelectedIndexChanged="Cmb_Hasta_Periodo_SelectedIndexChanged"
                                    AutoPostBack="true" TabIndex="14" Style="width: 65px; text-align: center;">
                                </asp:DropDownList>
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
                                <asp:TextBox ID="Txt_Fecha_Vencimiento" runat="server" Width="80%" TabIndex="14"
                                    MaxLength="11" Height="18px" />
                                <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Vencimiento" runat="server" TargetControlID="Txt_Fecha_Vencimiento"
                                    WatermarkCssClass="watermarked" WatermarkText="Día/Mes/Año" Enabled="True" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Vencimiento" runat="server" TargetControlID="Txt_Fecha_Vencimiento"
                                    Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Fecha_Vencimiento" />
                                <asp:ImageButton ID="Btn_Fecha_Vencimiento" runat="server" ImageUrl="../imagenes/paginas/SmallCalendar.gif"
                                    Style="vertical-align: top;" Height="18px" CausesValidation="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Realizó
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Realizo" runat="server" TabIndex="16" MaxLength="250" TextMode="SingleLine"
                                    Width="96.4%" Enabled="false" />
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                Observaciones
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%;">
                                <asp:TextBox ID="Txt_Observaciones" runat="server" TabIndex="17" MaxLength="250"
                                    Style="text-transform: uppercase" TextMode="MultiLine" Width="98.6%" Enabled="false" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Observaciones" runat="server" TargetControlID="Txt_Observaciones"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                            </td>
                        </tr>
                        <tr runat="server" id="Td_Chk_Liquidacion_Temporal" visible="false">
                            <td style="text-align: left; width: 20%;">
                                Liquidación temporal
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:CheckBox ID="Chk_Liquidacion_Temporal" runat="server" TabIndex="18" AutoPostBack="True"
                                    Enabled="false" Text="" OnCheckedChanged="Chk_Liquidacion_Temporal_CheckedChanged" />
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <%------------------ Calculos ------------------%>
                        <tr>
                            <td style="text-align: center; font-size: 16px; font-weight: bold; color: #36C;"
                                colspan="4">
                                Cálculos para el pago
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Txt_Adeudo_Rezago" runat="server">Adeudo rezago</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Adeudo_Rezago" runat="server" Width="96.4%" Style="text-align: right;"
                                    TabIndex="20" />
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Txt_Adeudo_Corriente" runat="server">Adeudo corriente</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Adeudo_Corriente" runat="server" TabIndex="19" Width="96.4%"
                                    Style="text-align: right;" />
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="tr_descuento_pronto_pago" runat="server">
                            <td colspan="3" style="width: 20%; text-align: right;">
                                <asp:Label ID="Label3" runat="server">% descuento por pronto pago</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Porciento_Descuento_Pronto_Pago" runat="server" Width="46%"
                                    Style="text-align: right;" AutoPostBack="true" TabIndex="22" />
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Porciento_Descuento_Pronto_Pago" runat="server"
                                    TargetControlID="Txt_Porciento_Descuento_Pronto_Pago" FilterType="Custom, Numbers"
                                    ValidChars="." />
                                <asp:TextBox ID="Txt_Monto_Descuento_Pronto_Pago" runat="server" Width="46%" Style="text-align: right;"
                                    AutoPostBack="true" TabIndex="23" />
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Monto_Descuento_Pronto_Pago" runat="server"
                                    TargetControlID="Txt_Monto_Descuento_Pronto_Pago" FilterType="Custom, Numbers"
                                    ValidChars=".," />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Txt_Recargos_Ordinarios" runat="server">Recargos ordinarios</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Recargos_Ordinarios" runat="server" Width="96.4%" Style="text-align: right;"
                                    TabIndex="21" />
                            </td>
                            <td style="width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Descuento_" runat="server">% descuento</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Porciento_Descuento_Recargos" runat="server" Width="46%" Style="text-align: right;"
                                    AutoPostBack="true" TabIndex="22" OnTextChanged="Txt_Porciento_Descuento_Recargos_TextChanged" />
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Porciento_Descuento_Recargos" runat="server"
                                    TargetControlID="Txt_Porciento_Descuento_Recargos" FilterType="Custom, Numbers"
                                    ValidChars="." />
                                <asp:TextBox ID="Txt_Monto_Descuento_Recargos" runat="server" Width="46%" Style="text-align: right;"
                                    AutoPostBack="true" TabIndex="23" OnTextChanged="Txt_Monto_Descuento_Recargos_TextChanged" />
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Monto_Descuento_Recargos" runat="server"
                                    TargetControlID="Txt_Monto_Descuento_Recargos" FilterType="Custom, Numbers" ValidChars=".," />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Txt_Recargos_Moratorios" runat="server">Recargos moratorios</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Recargos_Moratorios" runat="server" Width="96.4%" Style="text-align: right;"
                                    TabIndex="24" />
                            </td>
                            <td style="width: 20%; text-align: right;">
                                <asp:Label ID="Label2" runat="server">% descuento</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Porciento_Descuento_Moratorios" runat="server" Width="46%" Style="text-align: right;"
                                    AutoPostBack="true" TabIndex="25" OnTextChanged="Txt_Porciento_Descuento_Moratorios_TextChanged" />
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Porciento_Descuento_Moratorios" runat="server"
                                    TargetControlID="Txt_Porciento_Descuento_Moratorios" FilterType="Custom, Numbers"
                                    ValidChars="." />
                                <asp:TextBox ID="Txt_Monto_Descuento_Moratorios" runat="server" Width="46%" Style="text-align: right;"
                                    AutoPostBack="true" TabIndex="26" OnTextChanged="Txt_Monto_Descuento_Moratorios_TextChanged" />
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Monto_Descuento_Moratorios" runat="server"
                                    TargetControlID="Txt_Monto_Descuento_Moratorios" FilterType="Custom, Numbers"
                                    ValidChars=".," />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Label1" runat="server">Honorarios</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Honorarios" runat="server" Width="96.4%" Style="text-align: right;"
                                    TabIndex="24" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; border-top: solid 1px BLACK">
                                Total adeudo
                            </td>
                            <td style="text-align: left; width: 30%; border-top: solid 1px BLACK">
                                <asp:TextBox ID="Txt_Total_Adeudo" runat="server" Width="96.4%" Style="text-align: right;"
                                    TabIndex="27" />
                            </td>
                            <td style="text-align: right; width: 20%; border-top: solid 1px BLACK">
                                Total descuento
                            </td>
                            <td style="text-align: left; width: 30%; border-top: solid 1px BLACK">
                                <asp:TextBox ID="Txt_Total_Descuento" runat="server" Width="96.4%" Style="text-align: right;"
                                    TabIndex="28" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; border-top: solid 1px BLACK">
                                Total a pagar
                            </td>
                            <td style="text-align: left; width: 30%; border-top: solid 1px BLACK">
                                <asp:TextBox ID="Txt_Total_Pagar" runat="server" Width="96.4%" Style="text-align: right;"
                                    TabIndex="29" />
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table width="98%" class="estilo_fuente">
                        <%------------------ Adeudos ------------------%>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Adeudos
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="4">
                                <asp:GridView ID="Grid_Adeudos" runat="server" CssClass="GridView_1" AutoGenerateColumns="false"
                                    PageSize="5" AllowPaging="false" Width="100%" HeaderStyle-CssClass="tblHead"
                                    Style="white-space: normal;">
                                    <Columns>
                                        <asp:BoundField DataField="ANIO" HeaderText="Año" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" />
                                        <asp:BoundField DataField="Bimestre1" HeaderText="Bimestre 1" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Right" NullDisplayText="0.00" />
                                        <asp:BoundField DataField="Bimestre2" HeaderText="Bimestre 2" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Right" NullDisplayText="0.00" />
                                        <asp:BoundField DataField="Bimestre3" HeaderText="Bimestre 3" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Right" NullDisplayText="0.00" />
                                        <asp:BoundField DataField="Bimestre4" HeaderText="Bimestre 4" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Right" NullDisplayText="0.00" />
                                        <asp:BoundField DataField="Bimestre5" HeaderText="Bimestre 5" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Right" NullDisplayText="0.00" />
                                        <asp:BoundField DataField="Bimestre6" HeaderText="Bimestre 6" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Right" NullDisplayText="0.00" />
                                        <asp:BoundField DataField="TOTAL" HeaderText="Total" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="14%" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <br />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <asp:HiddenField ID="Hdn_No_Orden_Variacion" runat="server" />
                <asp:HiddenField ID="Hdn_No_Descuento" runat="server" />
                <asp:HiddenField ID="Hdn_Cuenta_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Contrarecibo" runat="server" />
                <asp:HiddenField ID="Hdn_Busqueda" runat="server" />
                <asp:HiddenField ID="Hdn_Contribuyente_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Nuevo_Contribuyente_ID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Impuesto_Derechos_Supervision.aspx.cs"
    Inherits="paginas_Predial_Frm_Ope_Pre_Impuesto_Derechos_Supervision" Culture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type="text/javascript" language="javascript">
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_Session.ashx";
 
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
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);

        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }

		function getCookie(c_name)
		{
			if (document.cookie.length>0)
			{
			c_start=document.cookie.indexOf(c_name + "=");
			if (c_start!=-1)
				{
				c_start=c_start + c_name.length+1;
				c_end=document.cookie.indexOf(";",c_start);
				if (c_end==-1) c_end=document.cookie.length;
				return unescape(document.cookie.substring(c_start,c_end));
				}
			}
			return "";
		}

		function setCookie(c_name,value,expiredays)
		{
			var exdate=new Date();
			exdate.setDate(exdate.getDate()+expiredays);
			document.cookie=c_name+ "=" +escape(value)+
			((expiredays==null) ? "" : ";expires="+exdate.toUTCString());
		}
		//Abrir una ventana modal
		function Abrir_Ventana_Modal(Url, Propiedades)
		{
			window.showModalDialog(Url, null, Propiedades);
		}
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">

    <script type="text/javascript" language="javascript">

    //Metodos para abrir los Modal PopUp's de la página
    function Abrir_Busqueda_Tasas() {
        $find('Busqueda_Tasas').show();
        return false;
    }

    function Abrir_Busqueda_Multas() {
        $find('Busqueda_Multas').show();
        return false;
    }

    function Abrir_Detalle_Cuenta_Predial() {
        $find('Detalle_Cuenta_Predial').show();
        return false;
    }

    function Calcular_Recargos() {
        document.getElementById("<%=Txt_Recargos.ClientID%>").value="xxx";
        return false;
    }

    function Calcular_Importe() {
        var Valor_Estimado_Obra;
        Valor_Estimado_Obra=document.getElementById("<%=Txt_Valor_Estimado_Obra.ClientID%>").value;
        alert(Valor_Estimado_Obra);
        document.getElementById("<%=Txt_Importe.ClientID%>").value=Valor_Estimado_Obra;
        Txt_Valor_Estimado_Obra
        return false;
    }
    
    function Abrir_Resumen(Url, Propiedades) {
    window.open(Url, 'Resumen_Predio', Propiedades);
}

    </script>

    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server" AsyncPostBackTimeout="3600"
        EnableScriptGlobalization="true" EnableScriptLocalization="true" />
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
                            Derechos de Supervisión
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
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
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td>
                            <div align="right" style="width: 99%; background-color: #2F4E7D; color: #FFFFFF;
                                font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy;
                                height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" AlternateText="Nuevo"
                                                OnClick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" AlternateText="Modificar"
                                                OnClick="Btn_Modificar_Click" />
                                            <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                                                TabIndex="4" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" AlternateText="Imprimir"
                                                OnClick="Btn_Imprimir_Click" />
                                            <asp:ImageButton ID="Btn_Convenio" runat="server" ToolTip="Convenio" CssClass="Img_Button"
                                                TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/Listado.png" AlternateText="Convenio"
                                                PostBackUrl="~/paginas/Predial/Frm_Ope_Pre_Convenios_Derechos_Supervision.aspx" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" AlternateText="Salir"
                                                OnClick="Btn_Salir_Click" />
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
                                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. " />
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
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td align="center">
                            <asp:HiddenField ID="Hdf_No_Impuesto_Derecho_Supervision" runat="server" />
                            <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                            <asp:HiddenField ID="Hdf_Derecho_Supervision_ID" runat="server" />
                            <asp:HiddenField ID="Hdf_Derecho_Supervision_Tasa_ID" runat="server" />
                            <asp:HiddenField ID="Hdf_Estatus" runat="server" />
                            <asp:HiddenField ID="Hdf_Multa" runat="server" />
                            <asp:HiddenField ID="Hdf_Fecha_Ya_Asignada" runat="server" />
                            <asp:Label ID="Lbl_Mensaje_Adeudo_Impuesto_Predial" runat="server" CssClass="estilo_fuente_mensaje_error" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                            <br />
                            <asp:GridView ID="Grid_Impuestos_Derechos_Supervision" runat="server" AllowPaging="True"
                                AllowSorting="True" AutoGenerateColumns="False" CssClass="GridView_1" HeaderStyle-CssClass="tblHead"
                                PageSize="5" Style="white-space: normal;" Width="100%" OnPageIndexChanging="Grid_Impuestos_Derechos_Supervision_PageIndexChanging1"
                                OnSelectedIndexChanged="Grid_Impuestos_Derechos_Supervision_SelectedIndexChanged"
                                DataKeyNames="CUENTA_PREDIAL_ID">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png"
                                        Text="Button" CommandName="Select">
                                        <HeaderStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="NO_IMPUESTO_DERECHO_SUPERVISIO" HeaderText="No. Impuesto">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="CUENTA_PREDIAL_ID" Visible="False">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Cuenta_Predial" HeaderText="Cuenta Predial">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA_OFICIO" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Fecha Oficio">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA_VENCIMIENTO" DataFormatString="{0:dd-MMM-yyyy}"
                                        HeaderText="Fecha Vencimiento">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderStyle-Width="12%" HeaderText="Estatus">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA_CREO" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}"
                                        Visible="false">
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
                                &nbsp; Búsqueda:
                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial" runat="server"
                                    Height="24px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" OnClick="Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial_Click"
                                    TabIndex="10" ToolTip="Búsqueda Avanzada de Cuenta Predial" Width="24px" />
                                Detalles cuenta:
                                <asp:ImageButton ID="Btn_Detalles_Cuenta_Predial" runat="server" Height="24px" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png"
                                    TabIndex="10" ToolTip="Detalles de la cuenta" Width="24px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Propietario
                            </td>
                            <td style="text-align: left; width: 20%;">
                                <asp:TextBox ID="Txt_Propietario" runat="server" ReadOnly="True" Width="96.4%" />
                            </td>
                            <td style="text-align: left; width: 20%;">
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
                            <td style="text-align: left; width: 20%;">
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
                                Tipo Predio
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Tipo_Predio" runat="server" ReadOnly="True" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Superficie predio
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Superficie_Predio" runat="server" ReadOnly="True" Width="96.4%" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Fecha Oficio
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Oficio" runat="server" Width="84%" AutoPostBack="True"
                                    OnTextChanged="Txt_Fecha_Oficio_TextChanged" />
                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Fecha_Oficio" runat="server" TargetControlID="Txt_Fecha_Oficio"
                                    WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                <asp:ImageButton ID="Btn_Fecha_Oficio" runat="server" ImageUrl="../imagenes/paginas/SmallCalendar.gif"
                                    Style="vertical-align: top;" Height="18px" />
                                <cc1:CalendarExtender ID="Dtp_Fecha_Oficio" runat="server" TargetControlID="Txt_Fecha_Oficio"
                                    PopupButtonID="Btn_Fecha_Oficio" Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Estatus
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%" TabIndex="7">
                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                    <asp:ListItem Text="SIN PAGAR" Value="SIN PAGAR" />
                                    <asp:ListItem Text="POR PAGAR" Value="POR PAGAR" />
                                    <asp:ListItem Text="PAGADO" Value="PAGADO" />
                                    <asp:ListItem Text="IMPRESA" Value="IMPRESA" />
                                    <asp:ListItem Text="CANCELADA" Value="CANCELADA" />
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Fecha de vencimiento
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Vencimiento" runat="server" Width="94%" TabIndex="12"
                                    MaxLength="11" Height="18px" OnTextChanged="Txt_Fecha_Vencimiento_TextChanged" />
                                <cc1:TextBoxWatermarkExtender ID="Twe_Fecha_Vencimiento" runat="server" TargetControlID="Txt_Fecha_Vencimiento"
                                    WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                <cc1:CalendarExtender ID="Dtp_Fecha_Vencimiento" runat="server" TargetControlID="Txt_Fecha_Vencimiento"
                                    PopupButtonID="Btn_Fecha_Vencimiento" Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                Observaciones
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%;">
                                <asp:TextBox ID="Txt_Observaciones" runat="server" TabIndex="10" MaxLength="250"
                                    TextMode="MultiLine" Width="98.6%" Style="text-transform: uppercase" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios" runat="server" TargetControlID="Txt_Observaciones"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;" colspan="4">
                                <br />
                                <hr />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Presupuesto de Obra
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Valor_Estimado_Obra" runat="server" Width="96.4%" AutoPostBack="true"
                                    OnTextChanged="Txt_Valor_Estimado_Obra_TextChanged" />
                            </td>
                            <td style="width: 20%; text-align: right;">
                                *Tasa
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Tasa_Descripcion" runat="server" Width="64%" />
                                <asp:TextBox ID="Txt_Tasa" runat="server" AutoPostBack="True" OnTextChanged="Txt_Tasa_TextChanged"
                                    Width="15%" />
                                <cc1:MaskedEditExtender ID="MEE_Txt_Tasa" runat="server" TargetControlID="Txt_Tasa"
                                    Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                    CultureTimePlaceholder="" Enabled="True" />
                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Tasas" runat="server" Height="24px"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" OnClientClick="javascript:return Abrir_Busqueda_Tasas();"
                                    TabIndex="10" ToolTip="Búsqueda Avanzada de Tasas" Width="24px" />
                                <br>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Importe
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Importe" runat="server" ReadOnly="True" Width="96.4%" />
                                <cc1:MaskedEditExtender ID="MEE_Txt_Importe" runat="server" TargetControlID="Txt_Importe"
                                    Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                    CultureTimePlaceholder="" Enabled="True" />
                            </td>
                            <td style="width: 20%; text-align: right;">
                                Recargos
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Recargos" runat="server" Width="96.4%" AutoPostBack="True" />
                                <cc1:MaskedEditExtender ID="MEE_Txt_Recargos" runat="server" TargetControlID="Txt_Recargos"
                                    Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                    CultureTimePlaceholder="" Enabled="True" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                            <td style="text-align: left; width: 25%; text-align: right; padding-left: 30px;">
                                <span style="float: left;"></span>Multa
                            </td>
                            <td style="text-align: left; width: 25%;">
                                <asp:TextBox ID="Txt_Multa" runat="server" Width="80%" TabIndex="25" ReadOnly="true"
                                    Enabled="false" />
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Multa" runat="server" TargetControlID="Txt_Multa"
                                    FilterType="Custom, Numbers" ValidChars=",." />
                                <asp:ImageButton ID="Btn_Multas" runat="server" ToolTip="Seleccionar multa" TabIndex="19"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Height="24px" Width="24px"
                                    OnClientClick="javascript:return Abrir_Busqueda_Multas();" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                            <td style="width: 20%; text-align: right;">
                                Total
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Total" runat="server" ReadOnly="True" Width="96.4%" AutoPostBack="true" />
                                <cc1:MaskedEditExtender ID="MME_Txt_Total" runat="server" TargetControlID="Txt_Total"
                                    Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                    CultureTimePlaceholder="" Enabled="True" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: right;">
                                <asp:ImageButton ID="Btn_Agregar_Impuesto" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                    CssClass="Img_Button" ToolTip="Agregar" TabIndex="15" OnClick="Btn_Agregar_Impuesto_Click" />
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="4">
                                <asp:GridView ID="Grid_Detalle_Impuesto_Derechos_Supervision" runat="server" AllowPaging="True"
                                    CssClass="GridView_1" AutoGenerateColumns="False" PageSize="5" Width="100%" AllowSorting="True"
                                    HeaderStyle-CssClass="tblHead" Style="white-space: normal;" OnRowCommand="Grid_Detalles_Impuesto_Derechos_Supervision_RowCommand"
                                    OnPageIndexChanging="Grid_Detalles_Impuesto_Derechos_Supervision_PageIndexChanging"
                                    DataKeyNames="DERECHO_SUPERVISION_TASA_ID">
                                    <Columns>
                                        <asp:BoundField DataField="VALOR_ESTIMADO_OBRA" HeaderText="Valor estimado de obra "
                                            DataFormatString="{0:c2}">
                                            <ItemStyle HorizontalAlign="Right" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DERECHO_SUPERVISION_TASA_ID" HeaderText="DERECHO_SUPERVISION_TASA_ID"
                                            Visible="False"></asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPCION_TASA" HeaderText="Descripción Tasa">
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Tasa_Derecho_Supervision" HeaderText="Tasa" DataFormatString="{0:p2}">
                                            <ItemStyle HorizontalAlign="Right" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MULTAS" HeaderText="Multa" DataFormatString="{0:c2}">
                                            <ItemStyle HorizontalAlign="Right" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IMPORTE" HeaderText="Importe" DataFormatString="{0:c2}">
                                            <ItemStyle HorizontalAlign="Right" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RECARGOS" HeaderText="Recargos" HeaderStyle-Width="12%"
                                            DataFormatString="{0:c2}">
                                            <HeaderStyle Width="12%" />
                                            <ItemStyle HorizontalAlign="Right" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TOTAL" HeaderText="Total" DataFormatString="{0:c2}">
                                            <ItemStyle HorizontalAlign="Right" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DER_MULTA_ID" HeaderText="Multa id" Visible="false">
                                            <HeaderStyle Width="30%" />
                                            <ItemStyle HorizontalAlign="Right" Width="20%" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Eliminar_Detalle" runat="server" ImageUrl="~/paginas/imagenes/paginas/delete.png"
                                                    OnClientClick="return confirm('¿Está seguro de eliminar el presente registro?');"
                                                    CommandName="Eliminar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: right;">
                                Total
                                <asp:TextBox ID="Txt_Total_Impuestos_Grid" runat="server" Width="25%" ReadOnly="True" />
                                <cc1:MaskedEditExtender ID="MEE_Txt_Total_Impuestos_Grid" runat="server" TargetControlID="Txt_Total_Impuestos_Grid"
                                    Mask="999,999,999,999,999,999,999.99" MaskType="Number" InputDirection="RightToLeft"
                                    AcceptNegative="Left" ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                    CultureTimePlaceholder="" Enabled="True" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <%---------------------------------- Modal Popup Extender búsqueda de Contribuyentes ----------------------------------%>
                <cc1:ModalPopupExtender ID="Mpe_Busqueda_Tasas" runat="server" TargetControlID="Btn_Comodin_Open_Busqueda_Tasas"
                    PopupControlID="Pnl_Busqueda_Contenedor_Tasas" BackgroundCssClass="popUpStyle"
                    BehaviorID="Busqueda_Tasas" CancelControlID="Btn_Comodin_Close_Busqueda_Tasas"
                    DropShadow="true" DynamicServicePath="" Enabled="True" />
                <asp:Button ID="Btn_Comodin_Close_Busqueda_Tasas" runat="server" Style="background-color: transparent;
                    border-style: none; display: none;" Text="" />
                <asp:Button ID="Btn_Comodin_Open_Busqueda_Tasas" runat="server" Style="background-color: transparent;
                    border-style: none; display: none;" Text="" />
                <%---------------------------------- Modal Popup Extender búsqueda de Multas ----------------------------------%>
                <cc1:ModalPopupExtender ID="Mpe_Busqueda_Multas" runat="server" TargetControlID="Btn_Comodin_Open_Busqueda_Multas"
                    PopupControlID="Pnl_Busqueda_Contenedor" BackgroundCssClass="popUpStyle" BehaviorID="Busqueda_Multas"
                    CancelControlID="Btn_Comodin_Close_Busqueda_Multas" DropShadow="true" DynamicServicePath=""
                    Enabled="True" />
                <asp:Button ID="Btn_Comodin_Open_Busqueda_Multas" runat="server" Style="background-color: transparent;
                    border-style: none; display: none;" Text="" />
                <asp:Button ID="Btn_Comodin_Close_Busqueda_Multas" runat="server" Style="background-color: transparent;
                    border-style: none; display: none;" Text="" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <%---------------------------------- Panel búsqueda de Contribuyentes ----------------------------------%>
    <asp:Panel ID="Pnl_Busqueda_Contenedor_Tasas" runat="server" CssClass="drag" Style="display: none;
        width: 650px; border-style: outset; border-color: Silver; background-image: url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);
        background-repeat: repeat-y;">
        <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" Style="cursor: move; background-color: Silver;
            color: Black; font-size: 12; font-weight: bold; border-style: outset;">
            <table width="99%">
                <tr>
                    <td style="color: Black; text-align: center; font-size: 12; font-weight: bold;">
                        <asp:Image ID="Img_Icono_Busqueda_Tasas" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" />
                        B&uacute;squeda: Tasas
                    </td>
                    <td align="right" style="width: 5%;">
                        <asp:ImageButton ID="Btn_Cerrar_Busqueda_Tasas" runat="server" Style="cursor: pointer;"
                            ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"
                            OnClick="Btn_Cerrar_Busqueda_Tasas_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:UpdatePanel ID="Upd_Panel_Busqueda_Tasas" runat="server">
            <ContentTemplate>
                <div style="color: #5D7B9D">
                    <table width="100%">
                        <tr>
                            <td align="left" style="text-align: left;">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100%" colspan="4" align="right">
                                            <asp:ImageButton ID="Btn_Limpiar_Busqueda_Tasas" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_clear.png"
                                                ToolTip="Limpiar Controles de Busqueda" OnClick="Btn_Limpiar_Busqueda_Tasas_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left; font-size: 11px;">
                                            Identificador
                                        </td>
                                        <td style="width: 35%; text-align: left; font-size: 11px;">
                                            <asp:TextBox ID="Txt_Busqueda_Identificador" runat="server" Width="98%" MaxLength="20" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Identificador" runat="server" FilterType="CUSTOM, LowercaseLetters, UppercaseLetters"
                                                ValidChars=" " TargetControlID="Txt_Busqueda_Identificador" />
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Identificador" runat="server"
                                                TargetControlID="Txt_Busqueda_Identificador" WatermarkText="Búsqueda por Identificador"
                                                WatermarkCssClass="watermarked" />
                                        </td>
                                        <td style="width: 15%; text-align: left; font-size: 11px;">
                                            Año
                                        </td>
                                        <td style="width: 85%; text-align: left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Año" runat="server" Width="100%">
                                                <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left; font-size: 11px;">
                                            Descripción
                                        </td>
                                        <td style="width: 35%; text-align: left; font-size: 11px;" colspan="3">
                                            <asp:TextBox ID="Txt_Busqueda_Descripcion" runat="server" Width="98%" MaxLength="20" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Descripcion" runat="server" FilterType="CUSTOM, LowercaseLetters, UppercaseLetters"
                                                ValidChars=" " TargetControlID="Txt_Busqueda_Descripcion" />
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Descripcion" runat="server" TargetControlID="Txt_Busqueda_Descripcion"
                                                WatermarkText="Búsqueda por Descripción" WatermarkCssClass="watermarked" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; text-align: left;" colspan="4">
                                            <center>
                                                <asp:Button ID="Btn_Busqueda_Tasas" runat="server" Text="Buscar Tasas" CssClass="button"
                                                    CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Tasas_Click" />
                                            </center>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="Grid_Tasas" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                CssClass="GridView_1" GridLines="None" OnPageIndexChanging="Grid_Tasas_PageIndexChanging"
                                                OnSelectedIndexChanged="Grid_Tasas_SelectedIndexChanged" PageSize="5" Width="98%">
                                                <RowStyle CssClass="GridItem" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="DERECHO_SUPERVISION_ID" HeaderText="DERECHO_SUPERVISION_ID">
                                                        <FooterStyle Font-Size="0pt" Width="0px" />
                                                        <HeaderStyle Font-Size="0pt" Width="0px" />
                                                        <ItemStyle Font-Size="0pt" Width="0px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IDENTIFICADOR" HeaderText="Identificador">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="45%" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DERECHO_SUPERVISION_TASA_ID" HeaderText="DERECHO_SUPERVISION_TASA_ID">
                                                        <FooterStyle Font-Size="0pt" Width="0px" />
                                                        <HeaderStyle Font-Size="0pt" Width="0px" />
                                                        <ItemStyle Font-Size="0pt" Width="0px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ANIO" HeaderText="Año">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="15px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TASA" HeaderText="Monto">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <HeaderStyle CssClass="GridHeader" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                            </asp:GridView>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag" HorizontalAlign="Center"
        Width="650px" Style="display: none; border-style: outset; border-color: Silver;
        background-image: url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG); background-repeat: repeat-y;">
        <asp:Panel ID="Panel1" runat="server" Style="cursor: move; background-color: Silver;
            color: Black; font-size: 12; font-weight: bold; border-style: outset;">
            <table width="99%">
                <tr>
                    <td style="color: Black; font-size: 12; font-weight: bold;">
                        <asp:Image ID="Img_Informatcion_Autorizacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                        B&uacute;squeda: Multas
                    </td>
                    <td align="right" style="width: 10%;">
                        <asp:ImageButton ID="Btn_Cerrar_Ventana" CausesValidation="false" runat="server"
                            Style="cursor: pointer;" ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"
                            OnClick="Btn_Cerrar_Ventana_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Prestamos" runat="server">
            <ContentTemplate>
                <div style="color: #5D7B9D">
                    <table width="100%">
                        <tr>
                            <td align="left" style="text-align: left;">
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server"
                                    AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Prestamos" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter1" class="progressBackgroundFilter">
                                        </div>
                                        <div style="background-color: Transparent; position: fixed; top: 50%; left: 47%;
                                            padding: 10px; z-index: 1002;" id="div_progress1">
                                            <img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100%" colspan="4" align="right">
                                            <asp:ImageButton ID="Btn_Limpiar_Ctlr_Busqueda" runat="server" OnClientClick="javascript:return Limpiar_Ctlr();"
                                                ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" ToolTip="Limpiar Controles de Busqueda" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left; font-size: 11px;">
                                            Año
                                        </td>
                                        <td style="width: 30%; text-align: right; font-size: 11px;">
                                            <asp:TextBox ID="Txt_Anio" runat="server" Width="98%" MaxLength="4" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Impuesto" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Anio" />
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_No_Impuesto" runat="server" TargetControlID="Txt_Anio"
                                                WatermarkText="Busqueda por año" WatermarkCssClass="watermarked" />
                                        </td>
                                        <td style="width: 20%; text-align: right; font-size: 11px;">
                                            Identificador
                                        </td>
                                        <td style="width: 30%; text-align: left; font-size: 11px;">
                                            <asp:TextBox ID="Txt_Busqueda_Identificador_Multa" runat="server" Width="98%" MaxLength="10" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Cuenta_Predial" runat="server"
                                                FilterType="Numbers, UppercaseLetters" TargetControlID="Txt_Busqueda_Identificador_Multa" />
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_Cuenta_Predial" runat="server"
                                                TargetControlID="Txt_Busqueda_Identificador_Multa" WatermarkText="Busqueda por Identificador"
                                                WatermarkCssClass="watermarked" />
                                        </td>
                                    </tr>
                                    <asp:GridView ID="Grid_Multas" runat="server" AllowPaging="True" AllowSorting="True"
                                        AutoGenerateColumns="False" CssClass="GridView_1" HeaderStyle-CssClass="tblHead"
                                        PageSize="5" OnPageIndexChanging="Grid_Multas_PageIndexChanging" OnSelectedIndexChanged="Grid_Multas_SelectedIndexChanged"
                                        Style="white-space: normal;" Width="100%" EmptyDataText="No se encontraron Multas.">
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png"
                                                Text="Button" CommandName="Select">
                                                <HeaderStyle Width="5%" />
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="MULTA_CUOTA_ID" HeaderText="Multa cuota" Visible="false">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IDENTIFICADOR" HeaderText="Identificador">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ANIO" HeaderText="Año" Visible="False"></asp:BoundField>
                                            <asp:BoundField DataField="MONTO" HeaderText="Tasa">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <HeaderStyle CssClass="tblHead" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                    <tr>
                                        <td style="width: 100%; text-align: left;" colspan="4">
                                            <center>
                                                <asp:Button ID="Btn_Busqueda_Empleados" runat="server" Text="Busqueda de Multas"
                                                    CssClass="button" CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Multas_Click" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--                    </td>
                </tr>
                <tr>
                    <td>                                                      
                    </td>
                </tr>
             </table>                                                   
           </div>                 --%>
    </asp:Panel>
</asp:Content>

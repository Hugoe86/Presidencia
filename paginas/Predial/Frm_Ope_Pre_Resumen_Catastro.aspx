<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Resumen_Catastro.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Resumen_Catastro"
    Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script runat="server">

</script>

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
        function MantenSesion()
        {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }

        //Temporizador para matener la sesión activa
        setInterval('MantenSesion()', "<%=(int)(0.9*(Session.Timeout * 60000))%>");

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
    function Abrir_Busqueda_Cuentas_Predial() {
        $find('Busqueda_Cuentas_Predial').show();
        return false;
    }
    function Abrir_Detalle_Cuenta_Predial() {
        $find('Detalle_Cuenta_Predial').show();
        return false;
    }

    </script>

    <asp:ScriptManager ID="ScriptManager" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
            </asp:UpdateProgress>
            <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Resumen de catastro
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
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="Lbl_Estatus" Style="color: Red" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                            Cuenta predial
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="96.4%" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            <asp:ImageButton ID="Img_Btn_Imprimir_Cuenta_Predial" runat="server" ToolTip="Imprimir"
                                OnClick="Img_Btn_Imprimir_Cuenta_Predial_Click" TabIndex="10" ImageUrl="~/paginas/imagenes/gridview/grid_print.png"
                                Height="22px" Width="22px" Style="float: left" />
                            <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada" runat="server" ToolTip="Búsqueda Avanzada de Cuenta Predial"
                                TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px"
                                Width="24px" OnClick="Btn_Mostrar_Busqueda_Avanzada_Click" />
                            Cuenta origen
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Cuenta_Origen" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                    </tr>
                    <%------------------ Datos generales ------------------%>
                    <tr style="background-color: #3366CC">
                        <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                            <asp:ImageButton ID="Img_Btn_Imprimir_Generales" runat="server" ImageUrl="../imagenes/gridview/grid_print.png"
                                Style="vertical-align: top; float: right;" Height="18px" CausesValidation="false"
                                OnClick="Img_Btn_Imprimir_Generales_Click" />
                            Generales
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Tipo predio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Tipo_Predio_General" runat="server" Width="96.4%" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            Uso predio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Uso_Predio_General" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Estado de predio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Estado_Predio_General" runat="server" Width="96.4%" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            Estatus
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Estatus_General" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Supe. construida (m&sup2;)
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Supe_Construida_General" runat="server" Width="96.4%" Text="0" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            Superficie Total (m&sup2;)
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Super_Total_General" runat="server" Width="96.4%" Text="0" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Ubicación
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Ubicacion_General" runat="server" Width="96.4%" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            Colonia
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Colonia_General" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Número exterior
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Numero_Exterior_General" runat="server" Width="96.4%" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            Número interior
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Numero_Interior_General" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Clave catastral
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Clave_Catastral_General" runat="server" Width="96.4%" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            Efectos
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Efectos_General" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Ultimo movimiento
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Ultimo_Movimiento_General" runat="server" Width="96.4%" />
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <%------------------ Propietario ------------------%>
                    <tr style="background-color: #36C;">
                        <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                            <asp:ImageButton ID="Img_Btn_Imprimir_Propietario" runat="server" ImageUrl="../imagenes/gridview/grid_print.png"
                                Style="vertical-align: top; float: right;" Height="18px" CausesValidation="false"
                                OnClick="Img_Btn_Imprimir_Propietario_Click" />
                            Propietario
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Nombre
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Nombre_Propietario" runat="server" Width="96.4%" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            Propietario/Poseedor
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Propietario_Poseedor_Propietario" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            RFC
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Rfc_Propietario" runat="server" Width="96.4%" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            Colonia
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Colonia_Propietario" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Calle
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Calle_Propietario" runat="server" Width="96.4%" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            Número exterior
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Numero_Exterior_Propietario" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Número interior
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Numero_Interior_Propietario" runat="server" Width="96.4%" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            Estado
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Estado_Propietario" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Ciudad
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Ciudad_Propietario" runat="server" Width="96.4%" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            C.P.
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Cod_Postal_Propietario" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Copropietarios
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="Txt_Copropietarios_Propietario" runat="server" TabIndex="10" MaxLength="250"
                                TextMode="MultiLine" Width="98.6%" AutoPostBack="True" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <%------------------ Impuestos ------------------%>
                    <tr style="background-color: #36C;">
                        <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                            <asp:ImageButton ID="Img_Btn_Imprimir_Impuestos" runat="server" ImageUrl="../imagenes/gridview/grid_print.png"
                                Style="vertical-align: top; float: right;" Height="18px" CausesValidation="false"
                                OnClick="Img_Btn_Imprimir_Impuestos_Click" />
                            Impuestos
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Valor fiscal
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Valor_Fiscal_Impuestos" runat="server" Width="96.4%" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            Tasa
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Tasa_Impuestos" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Periodo corriente
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Periodo_Corriente_Impuestos" runat="server" Width="96.4%" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            Tipo predio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Tipo_Periodo_Impuestos" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Cuota anual
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Cuota_Anual_Impuestos" runat="server" Width="96.4%" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            Cuota bimestral
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Cuota_Bimestral_Impuestos" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Dif. de construcción
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Dif_Construccion_Impuestos" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            % Exención
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Porciento_Exencion_Impuestos" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Término exención
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Fecha_Termino_Extencion" runat="server" Width="96.4%" TabIndex="12"
                                MaxLength="11" Height="18px" />
                            <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Termino_Extencion" runat="server"
                                TargetControlID="Txt_Fecha_Termino_Extencion" WatermarkCssClass="watermarked"
                                WatermarkText="Dia/Mes/Año" Enabled="True" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Fecha avalúo
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Fecha_Avaluo_Impuestos" runat="server" Width="96.4%" TabIndex="12"
                                MaxLength="11" Height="18px" />
                            <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Avaluo_Impuestos" runat="server"
                                TargetControlID="Txt_Fecha_Avaluo_Impuestos" WatermarkCssClass="watermarked"
                                WatermarkText="Dia/Mes/Año" Enabled="True" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Cuota fija
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:CheckBox ID="Chk_Cuota_Fija" runat="server" Text="" />
                            <asp:TextBox ID="Txt_Cuota_Fija_Impuestos" runat="server" Width="85%" />
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <%------------------ Detalles cuota fija ------------------%>
                    <tr>
                        <td style="text-align: left; font-size: 14px; border-bottom: 1px solid #3366CC;"
                            colspan="4">
                            Detalles Cuota fija
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Cuota fija por:
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Cuota_Fija" runat="server" Width="96.4%" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            Plazo financiamiento
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Plazo_Financiamiento" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; text-align: left;">
                            Total impuesto
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Total_Impuesto" runat="server" Enabled="false" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Fundamento legal
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="Txt_Fundamento_Legal" runat="server" TabIndex="10" MaxLength="250"
                                TextMode="SingleLine" Width="98.6%" AutoPostBack="True" />
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="Txt_Fundamento_Legal"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <%------------------ Historial de movimientos ------------------%>
                    <tr style="background-color: #36C;">
                        <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                            <asp:ImageButton ID="Img_Btn_Imprimir_Historial_Movimientos" runat="server" ImageUrl="../imagenes/gridview/grid_print.png"
                                Style="vertical-align: top; float: right;" Height="18px" CausesValidation="false"
                                OnClick="Img_Btn_Imprimir_Historial_Movimientos_Click" />
                            Historial de movimientos
                            <br />
                            <asp:GridView ID="Grid_Historial_Movimientos" runat="server" CssClass="GridView_1"
                                AutoGenerateColumns="false" Width="100%" OnPageIndexChanging="Grid_Historial_Movimientos_PageIndexChanging"
                                HeaderStyle-CssClass="tblHead" Style="white-space: normal;" PageSize="5" AllowPaging="true">
                                <Columns>
                                    <asp:BoundField DataField="No_Nota" HeaderText="No. Nota">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Identificador" HeaderText="Clave ">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <br />
                <asp:HiddenField ID="Hdn_Propietario_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Cuenta_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Tasa_ID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

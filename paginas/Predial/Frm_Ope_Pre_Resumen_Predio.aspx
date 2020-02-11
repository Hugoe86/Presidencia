<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Resumen_Predio.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Resumen_Predio" %>

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

        function Abrir_Resumen(Url, Propiedades)
        {
            window.open(Url, 'Resumen_Predio', Propiedades);
        }

		function Abrir_Ventana_Estado_Cuenta(Url, Propiedades)
		{
			window.open(Url, 'Estado_Cuenta', Propiedades);
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
                        <td colspan="4" class="label_titulo">
                            Resumen de predio
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
                            <asp:ImageButton ID="Img_Btn_Documento_Cuenta_Predial" runat="server" ToolTip="Listado de movimientos pendientes de aplicar"
                                TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Listado.png" Height="22px"
                                Width="22px" Style="float: left" OnClick="Img_Btn_Documento_Cuenta_Predial_Click" />
                            <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada" runat="server" ToolTip="Búsqueda Avanzada de Cuenta Predial"
                                TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px"
                                Width="24px" OnClick="Btn_Mostrar_Busqueda_Avanzada_Click" />
                            Cuenta origen
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Cuenta_Origen" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                    </tr>
                    <div id="Div_Detalles_Cuenta" runat="server">
                        <tr style="background-color: #3366CC">
                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                <asp:ImageButton ID="Img_Btn_Quitar_Grid" runat="server" ImageUrl="~/paginas/imagenes/paginas/quitar.png"
                                    Style="vertical-align: top; float: right;" Height="18px" CausesValidation="false"
                                    OnClick="Img_Btn_Quitar_Grid_Click" />
                                Movimientos Pendientes Por Aplicar
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="Grid_Movimientos_Pendientes" runat="server" AllowPaging="True"
                                    GridLines="None" AutoGenerateColumns="False" CssClass="GridView_1" HeaderStyle-CssClass="tblHead"
                                    OnPageIndexChanging="Grid_Movimientos_Pendientes_PageIndexChanging" PageSize="5"
                                    OnSelectedIndexChanged="Grid_Movimientos_Pendientes_SelectedIndexChanged" Style="white-space: normal;"
                                    Width="100%" OnRowDataBound="Grid_Movimientos_Pendientes_RowDataBound">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="NO_ORDEN_VARIACION" HeaderText="No. Movimiento" Visible="True">
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA_CREO" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}">
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IDENTIFICADOR" HeaderText="Movimiento" Visible="True">
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_PROPIETARIO" HeaderText="Propietario" Visible="True">
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader " />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </div>
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
                            Estado de predio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Estado_Predio_General" runat="server" Width="96.4%" />
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
                            Ultimo movimiento
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Ultimo_Movimiento_General" runat="server" Width="96.4%" />
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
                            Calle
                        </td>
                        <td style="text-align: left; width: 30%;" colspan="3">
                            <asp:TextBox ID="Txt_Ubicacion_General" runat="server" Width="98.6%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; text-align: left;">
                            Colonia
                        </td>
                        <td style="text-align: left; width: 30%;" colspan="3">
                            <asp:TextBox ID="Txt_Colonia_General" runat="server" Width="98.6%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%;">
                            Número exterior
                        </td>
                        <td style="width: 80%;" colspan="3">
                            <asp:TextBox ID="Txt_Numero_Exterior_General" runat="server" Width="98.6%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%;">
                            Número interior
                        </td>
                        <td style="width: 80%;" colspan="3">
                            <asp:TextBox ID="Txt_Numero_Interior_General" runat="server" Width="98.6%" />
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
                        </td>
                        <td style="text-align: left; width: 30%;">
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
                        <td style="text-align: left; width: 30%;" colspan="3">
                            <asp:TextBox ID="Txt_Nombre_Propietario" runat="server" Width="98.6%" TextMode="MultiLine" />
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
                            Propietario/Poseedor
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Propietario_Poseedor_Propietario" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Calle
                        </td>
                        <td style="text-align: left; width: 30%;" colspan="3">
                            <asp:TextBox ID="Txt_Calle_Propietario" runat="server" Width="98.6%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%;">
                            Número exterior
                        </td>
                        <td style="width: 80%;" colspan="3">
                            <asp:TextBox ID="Txt_Numero_Exterior_Propietario" runat="server" Width="98.6%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%;">
                            Número interior
                        </td>
                        <td style="width: 80%;" colspan="3">
                            <asp:TextBox ID="Txt_Numero_Interior_Propietario" runat="server" Width="98.6%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; text-align: left;">
                            Colonia
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Colonia_Propietario" runat="server" Width="96.4%" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            C.P.
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Cod_Postal_Propietario" runat="server" Width="96.4%" />
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
                            Estado
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Estado_Propietario" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Copropietarios
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="Txt_Copropietarios_Propietario" runat="server" TabIndex="10" MaxLength="250"
                                TextMode="MultiLine" Width="98.6%" AutoPostBack="True" Enabled="true" ForeColor="Silver" />
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
                            <asp:TextBox ID="Txt_Cuota_Fija_Impuestos" runat="server" Width="85%" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Beneficio
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Cuota_Fija" runat="server" Width="98.6%" />
                        </td>
                    </tr>
                    <%------------------ Detalles cuota fija ------------------%>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <%------------------ Historial de pagos ------------------%>
                    <tr style="background-color: #36C;">
                        <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                            <asp:DropDownList ID="Cmb_Consultar_Tipo_Pago" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="Cmb_Consultar_Tipo_Pago_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="PREDIAL">PREDIAL</asp:ListItem>
                                <asp:ListItem Value="TRASLADO">TRASLADO</asp:ListItem>
                                <asp:ListItem Value="IMPUESTOS FRACCIONAMIENTO">IMPUESTOS FRACCIONAMIENTO</asp:ListItem>
                                <asp:ListItem Value="DERECHOS SUPERVISION">DERECHOS SUPERVISION</asp:ListItem>
                                <asp:ListItem Value="CONSTANCIAS">CONSTANCIAS</asp:ListItem>
                            </asp:DropDownList>
                            <asp:ImageButton ID="Img_Btn_Imprimir_Histotial_Pagos" runat="server" ImageUrl="../imagenes/gridview/grid_print.png"
                                Style="vertical-align: top; float: right;" Height="18px" CausesValidation="false"
                                OnClick="Img_Btn_Imprimir_Histotial_Pagos_Click" />
                            Historial de pagos
                            <br />
                            <asp:GridView ID="Grid_Historial_Pagos" runat="server" CssClass="GridView_1" AutoGenerateColumns="false"
                                Width="100%" OnPageIndexChanging="Grid_Historial_Pagos_PageIndexChanging" HeaderStyle-CssClass="tblHead"
                                Style="white-space: normal;" PageSize="5" AllowPaging="true">
                                <Columns>
                                    <asp:BoundField DataField="PREDIAL" HeaderText="TIPO">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="No_Recibo" HeaderText="No. Recibo">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yy}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="No_Operacion" HeaderText="No. Operación">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Numero_Caja" HeaderText="No. Caja ">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Clave_Banco" HeaderText="No. Banco">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Periodo" HeaderText="Periodo">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Monto_Rezago" HeaderText="Monto Rezago" DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Monto_Corriente" HeaderText="Monto Corriente" DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Recargos_Ordinarios" HeaderText="Recargos Ordinarios"
                                        DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Recargos_Moratorios" HeaderText="Recargos Moratorios"
                                        DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Honorarios" HeaderText="Honorarios" DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Gastos_Ejecucion" HeaderText="G. Ejec." DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descuento_Recargos" HeaderText="Desc. Rec." DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descuento_Moratorios" HeaderText="Desc. Mor." DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descuento_Honorarios" HeaderText="Desc. Hon." DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descuento_Pronto_Pago" HeaderText="Desc. Pronto Pago"
                                        DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                            <asp:GridView ID="Grid_Traslado" runat="server" CssClass="GridView_1" AutoGenerateColumns="false"
                                Width="100%" OnPageIndexChanging="Grid_Traslado_PageIndexChanging" HeaderStyle-CssClass="tblHead"
                                Style="white-space: normal;" PageSize="5" AllowPaging="true" AllowSorting="true">
                                <Columns>
                                    <asp:BoundField DataField="TIPO_PAGO" HeaderText="TIPO">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="No_Recibo" HeaderText="No. Recibo">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Numero_Caja" HeaderText="No. Caja ">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yy}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Impuesto_Traslado" HeaderText="Impuesto Traslado" DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Impuesto_Division" HeaderText="Impuesto Division" DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Constancia_No_Adeudo" HeaderText="Constancia no Adeudo"
                                        DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Multas" HeaderText="Multas" DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descuento_Multas" HeaderText="Desc. Multas" DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="recargos" HeaderText="Recargos" DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descuento_Recargos" HeaderText="Desc. Recargos" DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:c}">
                                        <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                        <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                            <asp:GridView ID="Grid_Impuesto_Fraccionamiento" runat="server" CssClass="GridView_1"
                                AutoGenerateColumns="false" Width="100%" OnPageIndexChanging="Grid_Impuesto_Fraccionamiento_PageIndexChanging"
                                HeaderStyle-CssClass="tblHead" Style="white-space: normal;" PageSize="5" AllowPaging="true">
                                <Columns>
                                    <asp:BoundField DataField="TIPO_PAGO" HeaderText="FRACC.">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="No_Recibo" HeaderText="No. Recibo">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Numero_Caja" HeaderText="No. Caja ">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yy}">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Recargos" HeaderText="Recargos" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descuento_Recargos" HeaderText="Desc. Recargos" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Multas" HeaderText="Multas" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descuento_Multas" HeaderText="Desc. Multas" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                            <asp:GridView ID="Grid_Derechos_Supervision" runat="server" CssClass="GridView_1"
                                AutoGenerateColumns="false" Width="100%" OnPageIndexChanging="Grid_Derechos_Supervision_PageIndexChanging"
                                HeaderStyle-CssClass="tblHead" Style="white-space: normal;" PageSize="5" AllowPaging="true">
                                <Columns>
                                    <asp:BoundField DataField="TIPO_PAGO" HeaderText="DER. SUP.">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="No_Recibo" HeaderText="No. Recibo">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Numero_Caja" HeaderText="No. Caja ">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yy}">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Recargos" HeaderText="Recargos" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descuento_Recargos" HeaderText="Desc. Recargos" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Multas" HeaderText="Multas" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descuento_Multas" HeaderText="Desc. Multas" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                            <asp:GridView ID="Grid_Constancias" runat="server" CssClass="GridView_1" AutoGenerateColumns="false"
                                Width="100%" OnPageIndexChanging="Grid_Constancias_PageIndexChanging" HeaderStyle-CssClass="tblHead"
                                Style="white-space: normal;" PageSize="5" AllowPaging="true">
                                <Columns>
                                    <asp:BoundField DataField="'CONSTANCIAS'" HeaderText="CONST">
                                        <HeaderStyle HorizontalAlign="center" Width="5%" />
                                        <ItemStyle HorizontalAlign="center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="No_Recibo" HeaderText="No. Recibo">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Numero_Caja" HeaderText="No. Caja ">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yy}">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Tipo_Constancia" HeaderText="Tipo Constancia">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="No_Constancia" HeaderText="No Constancia" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
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
                                HeaderStyle-CssClass="tblHead" Style="white-space: normal;" OnSelectedIndexChanged="Grid_Historial_Movimientos_SelectedIndexChanged"
                                PageSize="5" AllowPaging="true">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="No_Nota" HeaderText="No. Nota">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Grupo_Movimiento" HeaderText="Grupo Movimiento">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yy}">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Movimiento_ID" HeaderText="No. Movimiento">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Identificador" HeaderText="Clave ">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus_Orden" HeaderText="Estatus">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
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
                    <%------------------ Historial de convenios ------------------%>
                    <tr style="background-color: #36C;">
                        <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                            <asp:ImageButton ID="Img_Btn_Imprimir_Historial_Convenios" runat="server" ImageUrl="../imagenes/gridview/grid_print.png"
                                Style="vertical-align: top; float: right;" Height="18px" CausesValidation="false"
                                OnClick="Img_Btn_Imprimir_Historial_Convenios_Click" />
                            Convenios de la cuenta
                            <br />
                            <asp:GridView ID="Grid_Convenios_Cuenta" runat="server" AllowPaging="true" AutoGenerateColumns="false"
                                CssClass="GridView_1" HeaderStyle-CssClass="tblHead" OnSelectedIndexChanged="Grid_Convenios_Cuenta_SelectedIndexChanged"
                                PageSize="5" Style="white-space: normal;" Width="100%">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="No_Convenio" HeaderText="No. Convenio">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Total_Convenio" HeaderText="Total convenido" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                    </asp:BoundField>
                                    <%--<asp:BoundField DataField="Fecha" HeaderText="Tipo convenio" />--%>
                                    <asp:BoundField DataField="Fecha" DataFormatString="{0:dd/MM/yy}" HeaderText="Fecha">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Numero_Parcialidades" HeaderText="Parcialidades">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="No_Impuesto" HeaderText="No. Impuesto">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="center" Width="15%" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="4">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <%------------------ Estado de cuenta ------------------%>
                    <tr style="background-color: #36C;">
                        <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                            Estado de cuenta
                            <asp:ImageButton ID="Btn_Estado_Cuenta" runat="server" ToolTip="Detalles Estado de Cuenta"
                                TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Listado.png" Height="24px"
                                Width="24px" />
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
                            <asp:TextBox ID="Txt_Periodo_Final" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_Estado_Cuenta" runat="server" AllowPaging="True" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="5" Width="100%" OnPageIndexChanging="Grid_Estado_Cuenta_PageIndexChanging"
                                AllowSorting="True" HeaderStyle-CssClass="tblHead" Style="white-space: normal;">
                                <Columns>
                                    <asp:BoundField DataField="NO_ADEUDO" HeaderText="NO_ADEUDO">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CONCEPTO" HeaderText="Concepto">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ANIO" HeaderText="Año">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BIMESTRE" HeaderText="Bim.">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESCUENTO" HeaderText="Desc." DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CORRIENTE" HeaderText="Corriente" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="REZAGOS" HeaderText="Rezagos" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RECARGOS_ORDINARIOS" HeaderText="Recargos Ordinarios"
                                        DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RECARGOS_MORATORIOS" HeaderText="Recargos Moratorios"
                                        DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="HONORARIOS" HeaderText="Honorarios" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MULTAS" HeaderText="Multas" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESCUENTOS" HeaderText="Descuentos" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MONTO" HeaderText="Monto" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
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
                        <td style="width: 20%; text-align: right;">
                            Adeudo rezago
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Adeudo_Rezago" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Periodo corriente
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Periodo_Actual" runat="server" Width="96.4%" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            Adeudo corriente
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Adeudo_Actual" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Total recargos ordinarios
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Total_Recargos_Ordinarios" runat="server" Width="96.4%" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            Honorarios
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Honorarios" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Recargos moratorios
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Recargos_Moratorios" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: right; width: 20%;">
                            Gastos de ejecución
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Gastos_Ejecucion" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;" colspan="2">
                            &nbsp;
                        </td>
                        <td style="width: 20%; text-align: right; border-top: solid 1px BLACK">
                            Subtotal
                        </td>
                        <td style="text-align: left; width: 30%; border-top: solid 1px BLACK">
                            <asp:TextBox ID="Txt_Subtotal" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            &nbsp;
                        </td>
                        <td style="width: 20%; text-align: right;" colspan="2">
                            Descuento por pronto pago
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Descuento_Pronto_Pago" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            &nbsp;
                        </td>
                        <td style="width: 20%; text-align: right;" colspan="2">
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
                        <td style="width: 20%; text-align: right;" colspan="2">
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
                        <td style="width:20%;text-align:right;" colspan="2">
                            Descuento honorarios (%)
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Descuento_Honorarios" runat="server" Width="96.4%" />
                        </td>
                    </tr>--%>
                    <tr>
                        <td style="text-align: left; width: 20%;" colspan="2">
                            &nbsp;
                        </td>
                        <td style="width: 20%; text-align: right; border-top: solid 1px BLACK">
                            Total
                        </td>
                        <td style="text-align: left; width: 30%; border-top: solid 1px BLACK">
                            <asp:TextBox ID="Txt_Total" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:HiddenField ID="Hdn_Propietario_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Cuenta_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Tasa_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Estatus_Convenio" runat="server" />
                <asp:HiddenField ID="Hdn_Tipo_Convenio" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

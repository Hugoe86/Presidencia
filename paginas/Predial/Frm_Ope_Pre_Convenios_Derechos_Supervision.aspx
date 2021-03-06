﻿<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Convenios_Derechos_Supervision.aspx.cs"
    Inherits="paginas_Predial_Frm_Ope_Pre_Convenios_Derechos_Supervision" Culture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ PreviousPageType VirtualPath="~/paginas/Predial/Frm_Ope_Pre_Impuesto_Derechos_Supervision.aspx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
		
		function Validar_Tipo_Solicitante()
		{
		    var Tipo_Solicitante_Activo;
		    if (document.getElementById("<%=Cmb_Tipo_Solicitante.ClientID%>").value == "DEUDOR SOLIDARIO")
		    {
		        Tipo_Solicitante_Activo=true;
		        document.getElementById("<%=Txt_Solicitante.ClientID%>").value="";
		        document.getElementById("<%=Txt_RFC.ClientID%>").value="";
		    }
		    else
		    {
		        Tipo_Solicitante_Activo=false;
		        document.getElementById("<%=Txt_Solicitante.ClientID%>").value=document.getElementById("<%=Txt_Propietario.ClientID%>").value;
		        document.getElementById("<%=Txt_RFC.ClientID%>").value=document.getElementById("<%=Hdf_RFC.ClientID%>").value;
		    }
	        document.getElementById("<%=Txt_Solicitante.ClientID%>").disabled=!Tipo_Solicitante_Activo;
	        document.getElementById("<%=Txt_RFC.ClientID%>").disabled=!Tipo_Solicitante_Activo;
	        if (document.getElementById("<%=Txt_Solicitante.ClientID%>").disabled == false)
	        {
    	        document.getElementById("<%=Txt_Solicitante.ClientID%>").focus();
	        }
		}
		
		function Replace_Modificado(Cadena_Origen, Cadena_Busqueda)
		{
            var Patron;
            
            Cadena_Busqueda || ( Cadena_Busqueda = '\\s|\\&nbsp;' );
            Patron = new RegExp('^(' + Cadena_Busqueda + ')*', 'g');
            Cadena_Origen = Cadena_Origen.replace(Patron, "");
            Patron = new RegExp('(' + Cadena_Busqueda + ')*$', 'g');
            Cadena_Origen = Cadena_Origen.replace(Patron, "");

		    return Cadena_Origen;
		}
		
		function Replace_Patron(Cadena_Origen, Cadena_Busqueda)
		{
            var Patron;
            
            Cadena_Busqueda || ( Cadena_Busqueda = '\\_|\\,' );
            Patron = new RegExp('(' + Cadena_Busqueda + ')*', 'g');
            Cadena_Origen = Cadena_Origen.replace(Patron, "");
            
//            Cadena_Busqueda || ( Cadena_Busqueda = '\\_|\\,' );
//            Patron = new RegExp('^(' + Cadena_Busqueda + ')*', 'g');
//            Cadena_Origen = Cadena_Origen.replace(Patron, "");
//            alert(Patron + '-' + Cadena_Origen);
//            Patron = new RegExp('(' + Cadena_Busqueda + ')*$', 'g');
//            Cadena_Origen = Cadena_Origen.replace(Patron, "");
//            alert(Patron + '-' + Cadena_Origen);

		    return Cadena_Origen;
		}
		
		function Calcular_Total_Descuento()
		{
	        var Descuento_Recargos_Ordinarios = 0.0;
	        var Descuento_Recargos_Moratorios = 0.0;
	        var Descuento_Recargos_Multa = 0.0;
	        var Total_Descuento = 0.0;
	        var Total_Adeudo = 0.0;
	        var Total_Convenio = 0.0;
            
            if(Replace_Modificado(Replace_Patron(document.getElementById("<%=Txt_Descuento_Recargos_Ordinarios.ClientID%>").value)) != ""
            && Replace_Modificado(Replace_Patron(document.getElementById("<%=Txt_Descuento_Recargos_Ordinarios.ClientID%>").value)) != ".")
            {
    	        Descuento_Recargos_Ordinarios = parseFloat(Replace_Patron(document.getElementById("<%=Txt_Descuento_Recargos_Ordinarios.ClientID%>").value));
            }
            else
            {
                document.getElementById("<%=Txt_Descuento_Recargos_Ordinarios.ClientID%>").value = "0.0";
            }
            if(Replace_Modificado(Replace_Patron(document.getElementById("<%=Txt_Descuento_Recargos_Moratorios.ClientID%>").value)) != ""
            && Replace_Modificado(Replace_Patron(document.getElementById("<%=Txt_Descuento_Recargos_Moratorios.ClientID%>").value)) != ".")
            {
    	        Descuento_Recargos_Moratorios = parseFloat(Replace_Patron(document.getElementById("<%=Txt_Descuento_Recargos_Moratorios.ClientID%>").value));
            }
            else
            {
                document.getElementById("<%=Txt_Descuento_Recargos_Moratorios.ClientID%>").value = "0.0";
            }
            if(Replace_Modificado(Replace_Patron(document.getElementById("<%=Txt_Descuento_Multas.ClientID%>").value)) != ""
            && Replace_Modificado(Replace_Patron(document.getElementById("<%=Txt_Descuento_Multas.ClientID%>").value)) != ".")
            {
    	        Descuento_Recargos_Multa = parseFloat(Replace_Patron(document.getElementById("<%=Txt_Descuento_Multas.ClientID%>").value));
            }
            else
            {
                document.getElementById("<%=Txt_Descuento_Multas.ClientID%>").value = "0.0";
            }

            Total_Descuento = Descuento_Recargos_Ordinarios + Descuento_Recargos_Moratorios + Descuento_Recargos_Multa;
	        document.getElementById("<%=Txt_Total_Descuento.ClientID%>").value = Total_Descuento.toFixed(2);
	        
	        if(document.getElementById("<%=Txt_Total_Adeudo.ClientID%>").value != "")
	        {
	            Total_Adeudo = parseFloat(document.getElementById("<%=Txt_Total_Adeudo.ClientID%>").value);
	        }
	        else
	        {
	            document.getElementById("<%=Txt_Total_Adeudo.ClientID%>").value = "0.0";
	        }
	        Total_Convenio = Total_Adeudo + Total_Descuento;
	        document.getElementById("<%=Txt_Total_Convenio.ClientID%>").value = Total_Convenio.toFixed(2);
	        
//	        Frm_Ope_Pre_Convenios_Derechos_Supervision.Calcular_Parcialidades(alert);
//	        Calcular_Parcialidades();
		}
        
        function Calcular_Parcialidades()
        {
            PageMethods.WM_Calcular_Parcialidades(OnSucceeded, OnFailed); 
        }

        function OnSucceeded(result)
        {
            return true; 
        }

        function OnFailed(error)
        {
            return false;
        }
        
        function Abrir_Modal_Popup() {
            $find('Busqueda_Empleados').show();
            return false;
        }
        function Limpiar_Ctlr(){
            return false;
        }

        function Abrir_Resumen(Url, Propiedades) {
            window.open(Url, 'Resumen_Predio', Propiedades);
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
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
                        <td class="label_titulo">
                            Convenios de Derechos de Supervisión
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
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
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
                                                            ToolTip="Buscar" AlternateText="Buscar" OnClick="Btn_Buscar_Click" />
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
                <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                <asp:HiddenField ID="Hdf_Propietario_ID" runat="server" />
                <asp:HiddenField ID="Hdf_RFC" runat="server" />
                <asp:HiddenField ID="Hdf_No_Convenio" runat="server" />
                <asp:HiddenField ID="Hdf_No_Descuento" runat="server" />
                <asp:HiddenField ID="Hdf_No_Impuesto_Derechos_Supervision" runat="server" />
                <asp:HiddenField ID="Hdf_Desc_Multas" runat="server" />
                <asp:HiddenField ID="Hdf_Desc_Recargos" runat="server" />
                <br />
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align: left;">
                            <asp:GridView ID="Grid_Convenios_Impuestos_Derechos_Supervision" runat="server" AutoGenerateColumns="False"
                                CssClass="GridView_1" DataKeyNames="CUENTA_PREDIAL_ID,PROPIETARIO_ID" HeaderStyle-CssClass="tblHead"
                                OnPageIndexChanging="Grid_Convenios_Impuestos_Derechos_Supervision_PageIndexChanging"
                                OnSelectedIndexChanged="Grid_Convenios_Impuestos_Derechos_Supervision_SelectedIndexChanged"
                                Style="white-space: normal;" Width="100%">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png"
                                        Text="Button">
                                        <HeaderStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="NO_CONVENIO" HeaderText="No. Convenio">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="CUENTA_PREDIAL_ID" Visible="False">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Cuenta_Predial" HeaderText="Cuenta Predial">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PROPIETARIO_ID" HeaderText="PROPIETARIO_ID" Visible="False">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Nombre_Propietario" HeaderText="Propietario" />
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA" DataFormatString="{0:dd/MMM/yyyy}" HeaderText="Fecha">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ANTICIPO" HeaderText="Anticipo">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_IMPUESTO_DERECHO_SUPERVISIO" HeaderText="Impuesto Id"
                                        Visible="false">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_REESTRUCTURA" HeaderText="Reestructura10" Visible="false">
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
                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Tramites" runat="server" Height="24px"
                                    OnClientClick="javascript:return Abrir_Modal_Popup();" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                    TabIndex="10" ToolTip="Búsqueda Avanzada de Impuestos" Width="24px" />
                                Detalles cuenta:
                                <asp:ImageButton ID="Btn_Detalles_Cuenta_Predial" runat="server" Height="24px" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png"
                                    TabIndex="10" ToolTip="Detalles de la cuenta" Width="24px" />
                                <asp:ImageButton ID="Btn_Convenio_Escaneado" runat="server" Height="24px" ImageUrl="~/paginas/imagenes/gridview/grid_docto.png"
                                    TabIndex="10" ToolTip="Subir convenio escaneado" Width="24px" />
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
                                <asp:TextBox ID="Txt_Tipo_Fraccionamiento" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Derechos de Supervisión
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Monto_Impuesto" runat="server" ReadOnly="True" Width="96.4%" />
                                <cc1:MaskedEditExtender ID="Mee_Txt_Monto_Impuesto" runat="server" TargetControlID="Txt_Monto_Impuesto"
                                    Mask="999,999,999,999,999,999,999.99" MaskType="Number" InputDirection="RightToLeft"
                                    AcceptNegative="Left" ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                    CultureTimePlaceholder="" Enabled="True" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Monto honorarios
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Honorarios" runat="server" ReadOnly="True" Width="96.4%" Enabled="false" />
                                <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="Txt_Honorarios"
                                    Mask="999,999,999,999,999,999,999.99" MaskType="Number" InputDirection="RightToLeft"
                                    AcceptNegative="Left" ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
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
                                <cc1:MaskedEditExtender ID="Mee_Txt_Monto_Multa" runat="server" TargetControlID="Txt_Monto_Multas"
                                    Mask="999,999,999,999,999,999,999.99" MaskType="Number" InputDirection="RightToLeft"
                                    AcceptNegative="Left" ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                    CultureTimePlaceholder="" Enabled="True" />
                            </td>
                            <td style="text-align: right; width: 20%;">
                                Monto recargos
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Monto_Recargos" runat="server" ReadOnly="True" Width="96.4%" />
                                <cc1:MaskedEditExtender ID="Mee_Txt_Monto_Recargos" runat="server" TargetControlID="Txt_Monto_Recargos"
                                    Mask="999,999,999,999,999,999,999.99" MaskType="Number" InputDirection="RightToLeft"
                                    AcceptNegative="Left" ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
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
                            <td style="text-align: left; width: 30%;">
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
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%" TabIndex="7" OnSelectedIndexChanged="Cmb_Estatus_Selected_Index_Changed"
                                    AutoPostBack="true">
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
                                    Width="98.6%" Style="text-transform: uppercase" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                RFC
                            </td>
                            <td style="text-align: left; width: 30%;" colspan="3">
                                <asp:TextBox ID="Txt_RFC" runat="server" Width="98.6%" Style="text-transform: uppercase" />
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
                                Fecha
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
                                Descuento Multas
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
                                    Mask="999,999,999,999,999,999,999.99" MaskType="Number" InputDirection="RightToLeft"
                                    AcceptNegative="Left" ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                    CultureTimePlaceholder="" Enabled="True" />
                                - Total Descuento
                                <asp:TextBox ID="Txt_Total_Descuento" runat="server" Width="100px" />
                                <cc1:MaskedEditExtender ID="Mee_Txt_Total_Descuento" runat="server" TargetControlID="Txt_Total_Descuento"
                                    Mask="999,999,999,999,999,999,999.99" MaskType="Number" InputDirection="RightToLeft"
                                    AcceptNegative="Left" ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                    CultureTimePlaceholder="" Enabled="True" />
                                = Sub-Total
                                <asp:TextBox ID="Txt_Sub_Total" runat="server" Width="100px" />
                                <cc1:MaskedEditExtender ID="Mee_Txt_Sub_Total" runat="server" TargetControlID="Txt_Sub_Total"
                                    Mask="999,999,999,999,999,999,999.99" MaskType="Number" InputDirection="RightToLeft"
                                    AcceptNegative="Left" ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
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
                                    Mask="999,999,999,999,999,999,999.99" MaskType="Number" InputDirection="RightToLeft"
                                    AcceptNegative="Left" ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
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
                                    CssClass="GridView_1" HeaderStyle-CssClass="tblHead" ShowFooter="True" Style="white-space: normal;"
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
                                        <asp:BoundField DataField="MONTO_IMPUESTO" DataFormatString="{0:c2}" HeaderText="Derechos de Supervisión">
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
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="Mpe_Busqueda_Empleados" runat="server" BackgroundCssClass="popUpStyle"
        BehaviorID="Busqueda_Empleados" PopupControlID="Pnl_Busqueda_Contenedor" TargetControlID="Btn_Comodin_Open"
        PopupDragHandleControlID="Pnl_Busqueda_Cabecera" CancelControlID="Btn_Comodin_Close"
        DropShadow="True" DynamicServicePath="" Enabled="True" />
    <asp:Button Style="background-color: transparent; border-style: none;" ID="Btn_Comodin_Close"
        runat="server" Text="" />
    <asp:Button Style="background-color: transparent; border-style: none;" ID="Btn_Comodin_Open"
        runat="server" Text="" />
    <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag" HorizontalAlign="Center"
        Width="650px" Style="display: none; border-style: outset; border-color: Silver;
        background-image: url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG); background-repeat: repeat-y;">
        <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" Style="cursor: move; background-color: Silver;
            color: Black; font-size: 12; font-weight: bold; border-style: outset;">
            <table width="99%">
                <tr>
                    <td style="color: Black; font-size: 12; font-weight: bold;">
                        <asp:Image ID="Img_Informatcion_Autorizacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                        B&uacute;squeda: Impuestos
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
                                            No Impuesto
                                        </td>
                                        <td style="width: 30%; text-align: right; font-size: 11px;">
                                            <asp:TextBox ID="Txt_Busqueda_No_Impuesto" runat="server" Width="98%" MaxLength="10" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Impuesto" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Busqueda_No_Impuesto" />
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_No_Impuesto" runat="server" TargetControlID="Txt_Busqueda_No_Impuesto"
                                                WatermarkText="Busqueda por No de Impuesto" WatermarkCssClass="watermarked" />
                                        </td>
                                        <td style="width: 20%; text-align: right; font-size: 11px;">
                                            Cuenta predial
                                        </td>
                                        <td style="width: 30%; text-align: left; font-size: 11px;">
                                            <asp:TextBox ID="Txt_Busqueda_Cuenta_Predial" runat="server" Width="98%" MaxLength="20" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Cuenta_Predial" runat="server"
                                                FilterType="Numbers, UppercaseLetters" TargetControlID="Txt_Busqueda_Cuenta_Predial" />
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_Cuenta_Predial" runat="server"
                                                TargetControlID="Txt_Busqueda_Cuenta_Predial" WatermarkText="Busqueda por Cuenta predial"
                                                WatermarkCssClass="watermarked" />
                                        </td>
                                    </tr>
                                    <asp:GridView ID="Grid_Impuestos_Derechos_Supervision" runat="server" AllowPaging="True"
                                        AllowSorting="True" AutoGenerateColumns="False" CssClass="GridView_1" HeaderStyle-CssClass="tblHead"
                                        PageSize="5" Style="white-space: normal;" Width="100%" OnPageIndexChanging="Grid_Impuestos_Derechos_Supervision_PageIndexChanging1"
                                        OnSelectedIndexChanged="Grid_Impuestos_Derechos_Supervision_SelectedIndexChanged"
                                        DataKeyNames="CUENTA_PREDIAL_ID" EmptyDataText="No se encontraron impuestos para esta cuenta predial o búsqueda.">
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
                                            <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_VENCIMIENTO" DataFormatString="{0:dd-MMM-yyyy}"
                                                HeaderText="Fecha Vencimiento">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ESTATUS" HeaderStyle-Width="12%" HeaderText="Estatus">
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
                                                <asp:Button ID="Btn_Busqueda_Empleados" runat="server" Text="Busqueda de Impuestos"
                                                    CssClass="button" CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Impuestos_Click" />
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
    </asp:Panel>
</asp:Content>

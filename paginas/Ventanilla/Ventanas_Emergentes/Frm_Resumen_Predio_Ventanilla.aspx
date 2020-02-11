<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Resumen_Predio_Ventanilla.aspx.cs"
    Inherits="paginas_Ventanilla_Ventanas_Emergentes_Frm_Resumen_Predio_Ventanilla" %>

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
<script type="text/javascript" language="javascript">
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../../Mantenedor_sesiones.ashx";

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
		    window.open(Url, null, Propiedades);
		}

        function Abrir_Resumen(Url, Nombre, Propiedades)
        {
            window.open(Url, Nombre, Propiedades);
        }

		function Abrir_Ventana_Estado_Cuenta(Url, Propiedades)
		{
		    window.open(Url, 'Estado_Cuenta', Propiedades);
		}
</script>

<head runat="server">
    <title>Resumen de Predio</title>
    <link href="../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../../estilos/estilo_ajax.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
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
                    <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada" runat="server" ToolTip="Búsqueda Avanzada de Cuenta Predial"
                        TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px"
                        Width="24px" OnClick="Btn_Mostrar_Busqueda_Avanzada_Click" Visible="false" />
                    Cuenta origen
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Cuenta_Origen" runat="server" ReadOnly="True" Width="96.4%" />
                </td>
            </tr>
            <tr id="Tr_Contenedor_Detalles_Cuenta" runat="server" style="background-color: #3366CC">
                <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                    <asp:ImageButton ID="Img_Btn_Quitar_Grid" runat="server" ImageUrl="~/paginas/imagenes/paginas/quitar.png"
                        Style="vertical-align: top; float: right;" Height="18px" CausesValidation="false"
                        OnClick="Img_Btn_Quitar_Grid_Click" />
                    Cuentas Pendientes Por Aplicar
                </td>
            </tr>
            <%------------------ Datos generales ------------------%>
            <tr style="background-color: #3366CC">
                <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
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
                    Calle notifiacion
                </td>
                <td style="text-align: left; width: 30%;" colspan="3">
                    <asp:TextBox ID="Txt_Ubicacion_Notificacion" runat="server" Width="98.6%" Enabled="false"/>
                </td>
            </tr>
            <tr>
                <td style="width: 20%; text-align: left;">
                    Colonia notifiacion
                </td>
                <td style="text-align: left; width: 30%;" colspan="3">
                    <asp:TextBox ID="Txt_Colonia_Notificacion" runat="server" Width="98.6%" Enabled="false"/>
                </td>
            </tr>
             <tr>
                <td style="width: 20%;">
                    Número exterior notifiacion
                </td>
                <td style="width: 80%;" colspan="3">
                    <asp:TextBox ID="Txt_Numero_Interior_Notificacion" runat="server" Width="98.6%" Enabled="false" />
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
        </table>
        <br />
        <asp:HiddenField ID="Hdn_Propietario_ID" runat="server" />
        <asp:HiddenField ID="Hdn_Cuenta_ID" runat="server" />
        <asp:HiddenField ID="Hdn_Tasa_ID" runat="server" />
        <asp:HiddenField ID="Hdn_Estatus_Convenio" runat="server" />
        <asp:HiddenField ID="Hdn_Tipo_Convenio" runat="server" />
    </div>
    </form>
</body>
</html>

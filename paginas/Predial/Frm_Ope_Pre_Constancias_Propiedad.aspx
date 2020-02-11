<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Constancias_Propiedad.aspx.cs" Inherits="paginas_predial_Frm_Ope_Pre_Constanicas_Propiedad"
    Title="Catalogo de Tipos de Colonias" Culture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
    
    function Abrir_Resumen(Url, Propiedades) {
    window.open(Url, 'Resumen_Predio', Propiedades);
}


    </script>

    <asp:ScriptManager ID="ScptM_Contribuyentes" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="2">
                            Constancias de Propiedad
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
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Nuevo" OnClick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Modificar" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click" />
                        </td>
                        <td>
                            Busqueda
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="150px" Style="text-transform: uppercase"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar" runat="server" CausesValidation="false" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                ToolTip="Buscar" OnClick="Btn_Buscar_Click" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkText="<Descripción>"
                                TargetControlID="Txt_Busqueda" WatermarkCssClass="watermarked">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            *Cuenta Predial
                        </td>
                        <td style="width: 82%; text-align: left;" colspan="3">
                            <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="35.7%" AutoPostBack="true"
                                TabIndex="9" MaxLength="20"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cuenta_Predial" runat="server" TargetControlID="Txt_Cuenta_Predial"
                                FilterType="UppercaseLetters, LowercaseLetters, Numbers" />
                            &nbsp; B&uacute;squeda:
                            <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada" runat="server" ToolTip="Búsqueda Avanzada de Cuenta Predial"
                                TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px"
                                Width="24px" OnClick="Btn_Mostrar_Busqueda_Avanzada_Click" />
                            Detalles cuenta:
                            <asp:ImageButton ID="Btn_Detalles_Cuentas_Predial" runat="server" ToolTip="Detalles de la cuenta"
                                TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Height="24px"
                                Width="24px" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Propietario
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="Txt_Propietario" runat="server" TabIndex="10" MaxLength="250" TextMode="SingleLine"
                                Width="98.6%" AutoPostBack="True" />
                        </td>
                    </tr>
                    <%--<tr>
                        <td style="text-align:left;width:20%; vertical-align:top;">
                            Ubicación del predio
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Domicilio" runat="server" TabIndex="10" MaxLength="250"
                                TextMode="SingleLine" Width="98.6%" AutoPostBack="True"/>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            Colonia
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Colonia" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                        <td align="right">
                            Calle
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Calle" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            No. exterior
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_No_Exterior" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                        <td align="right">
                            No. interior
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_No_Interior" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Realizó
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="Txt_Realizo" runat="server" TabIndex="10" MaxLength="250" TextMode="SingleLine"
                                Width="98.6%" AutoPostBack="True" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Folio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Folio" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                        <td style="text-align: right; width: 20%;">
                            Fecha
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Fecha" runat="server" Width="96.4%" Enabled="false" />
                            <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha" runat="server" TargetControlID="Txt_Fecha"
                                WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                            <cc1:CalendarExtender runat="server" ID="CE_Txt_Fecha" Format="dd/MM/yyyy" TargetControlID="Txt_Fecha"
                                Enabled="True" PopupButtonID="Img_Calendario" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            No. Recibo de pago
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_No_Recibo_Pago" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Estatus
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE" />
                                <asp:ListItem Text="POR PAGAR" Value="POR PAGAR" />
                                <asp:ListItem Text="PAGADA" Value="PAGADA" />
                                <asp:ListItem Text="IMPRESA" Value="IMPRESA" />
                                <asp:ListItem Text="CANCELADA" Value="CANCELADA" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Fecha Vencimiento
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Fecha_Vencimiento" runat="server" Width="96.4%"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="Txt_Fecha_Vencimiento"
                                WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Observaciones
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="Txt_Observaciones" runat="server" TabIndex="10" MaxLength="250"
                                TextMode="MultiLine" Width="98.6%" Style="text-transform: uppercase" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios" runat="server" TargetControlID="Txt_Observaciones"
                                WatermarkText="Límite de Caracteres 250" WatermarkCssClass="watermarked" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios" runat="server" TargetControlID="Txt_Observaciones"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                        </td>
                    </tr>
                    <caption>
                        <asp:HiddenField ID="Hdf_No_Constancia" runat="server" />
                        <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                        <asp:HiddenField ID="Hdf_Propietario_ID" runat="server" />
                        <tr align="center">
                            <td colspan="4">
                                <br />
                                <asp:GridView ID="Grid_Constancias_Propiedad" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    CssClass="GridView_1" HeaderStyle-CssClass="tblHead" PageSize="5" Style="white-space: normal;"
                                    Width="100%" OnPageIndexChanging="Grid_Constancias_Propiedad_PageIndexChanging"
                                    OnSelectedIndexChanged="Grid_Constancias_Propiedad_SelectedIndexChanged" OnRowCommand="Grid_Constancias_Propiedad_RowCommand"
                                    OnRowDataBound="Grid_Constancias_Propiedad_RowDataBound">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial" SortExpression="CUENTA_PREDIAL">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_PROPIETARIO" HeaderText="Propietario" SortExpression="NOMBRE_PROPIETARIO" />
                                        <asp:BoundField DataField="FOLIO" HeaderText="Folio" SortExpression="FOLIO" />
                                        <asp:BoundField DataField="FECHA" HeaderText="Fecha" SortExpression="FECHA" DataFormatString="{0:dd/MMM/yyyy}" />
                                        <asp:BoundField DataField="FECHA_VENCIMIENTO" HeaderText="Fecha Vencimiento" SortExpression="FECHA"
                                            DataFormatString="{0:dd/MMM/yyyy}" />
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS" />
                                        <asp:ButtonField ButtonType="Image" CommandName="Print" ImageUrl="~/paginas/imagenes/gridview/grid_print.png">
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:ButtonField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </caption>
                </table>
                <%---------------------------------- Modal Popup Extender búsqueda de Cuentas Predial -----------------------------%>
                <%--                <cc1:ModalPopupExtender ID="Mpe_Busqueda_Cuentas_Predial" runat="server" 
                    TargetControlID="Btn_Comodin_Open_Busqueda_Cuentas_Predial"
                    PopupControlID="Pnl_Busqueda_Contenedor_Cuentas_Predial" 
                    BackgroundCssClass="popUpStyle"
                    BehaviorID="Busqueda_Cuentas_Predial"
                    CancelControlID="Btn_Comodin_Close_Busqueda_Cuentas_Predial" 
                    DropShadow="true" 
                    DynamicServicePath="" 
                    Enabled="True" />
                <asp:Button ID="Btn_Comodin_Close_Busqueda_Cuentas_Predial" runat="server"
                    Style="background-color: transparent; border-style:none;display:none;"  Text="" />
                <asp:Button  ID="Btn_Comodin_Open_Busqueda_Cuentas_Predial" runat="server"
                    Style="background-color:transparent; border-style:none;display:none;" Text="" />
--%>
                <%----------------------------- Mpe Detalle de Cuenta Predial -----------------------------%>
                <cc1:ModalPopupExtender ID="Mpe_Detalles_Cuenta_Predial" runat="server" TargetControlID="Btn_Comodin_Open_Detalle_Cuenta_Predial"
                    PopupControlID="Pnl_Contenedor_Detalle_Cuenta_Predial" BackgroundCssClass="popUpStyle"
                    BehaviorID="Detalle_Cuenta_Predial" CancelControlID="Btn_Comodin_Close_Detalle_Cuenta_Predial"
                    DropShadow="true" DynamicServicePath="" Enabled="True" />
                <asp:Button ID="Btn_Comodin_Close_Detalle_Cuenta_Predial" runat="server" Style="background-color: transparent;
                    border-style: none; display: none;" Text="" />
                <asp:Button ID="Btn_Comodin_Open_Detalle_Cuenta_Predial" runat="server" Style="background-color: transparent;
                    border-style: none; display: none;" Text="" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <%---------------------------------- Panel búsqueda de Cuentas Predial ----------------------------------%>
    <%--    <asp:Panel ID="Pnl_Busqueda_Contenedor_Cuentas_Predial" runat="server" CssClass="drag" 
        style="display:none;width:650px;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">
            <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" 
                style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                <table width="99%">
                    <tr>
                        <td style="color:Black; text-align:center; font-size:12; font-weight:bold;">
                            <asp:Image ID="Img_Icono_Busqueda_Cuentas_Predial" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" />
                            B&uacute;squeda: Cuentas Predial
                        </td>
                        <td align="right" style="width:5%;">
                           <asp:ImageButton ID="Btn_Cerrar_Busqueda_Cuentas_Predial" runat="server" 
                                style="cursor:pointer;" ToolTip="Cerrar Ventana"
                                ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" 
                                onclick="Btn_Cerrar_Busqueda_Cuentas_Predial_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:UpdatePanel ID="Upd_Panel_Busqueda_Cuentas_Predial" runat="server">
                <ContentTemplate>
                    <div style="color: #5D7B9D">
                     <table width="100%">
                        <tr>
                            <td align="left" style="text-align: left;" >

                                <table width="100%">
                                    <tr>
                                        <td style="width:100%" colspan="4" align="right">
                                            <asp:ImageButton ID="Btn_Limpiar_Busqueda_Cuentas_Predial" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" 
                                                ToolTip="Limpiar Controles de Busqueda" 
                                                onclick="Btn_Limpiar_Busqueda_Cuentas_Predial_Click"/>
                                        </td>
                                    </tr>     
                                    <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td style="width:15%;text-align:left;font-size:11px;">
                                           Cuenta Predial 
                                        </td>
                                        <td style="width:35%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Busqueda_Cuenta_Predial" runat="server" Width="98%" MaxLength="20" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Cuenta_Predial" runat="server" 
                                                FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                                TargetControlID="Txt_Busqueda_Cuenta_Predial"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Cuenta_Predial" runat="server" 
                                                TargetControlID ="Txt_Busqueda_Cuenta_Predial" WatermarkText="Búsqueda por Cuenta Predial" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                        <td style="width:15%; text-align:right; font-size:11px;">
                                            Estatus
                                        </td>
                                        <td style="width:35%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="100%">
                                                <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>
                                                <asp:ListItem Value="ACTIVA">ACTIVA</asp:ListItem>
                                                <asp:ListItem Value="INACTIVA">INACTIVA</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%;text-align:left;font-size:11px;">
                                            Propietario
                                        </td>     
                                        <td style="width:85%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Busqueda_Propietatio" runat="server" Width="99.5%" MaxLength="80" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Propietatio" runat="server" 
                                                FilterType="LowercaseLetters, UppercaseLetters, Custom" 
                                                ValidChars="ÑñáéíóúÁÉÍÓÚ "
                                                TargetControlID="Txt_Busqueda_Propietatio" />
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Propietatio" runat="server" 
                                                TargetControlID ="Txt_Busqueda_Propietatio" 
                                                WatermarkText="Búsqueda por Nombre de Propietario o Copropietario" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%; text-align:left; font-size:11px;">
                                            Colonia
                                        </td>
                                        <td style="width:35%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Colonia" runat="server" Width="100%" 
                                                OnSelectedIndexChanged="Cmb_Busqueda_Colonia_SelectedIndexChanged" AutoPostBack=true>
                                                <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="Lse_Cmb_Busqueda_Colonia" runat="server" 
                                                TargetControlID="Cmb_Busqueda_Colonia" IsSorted="true" ></cc1:ListSearchExtender>
                                        </td>
                                        <td style="width:15%; text-align:right; font-size:11px;">
                                            Calle
                                        </td>
                                        <td style="width:35%;text-align:right;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Calle" runat="server" Width="100%">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="Lse_Cmb_Busqueda_Calle" runat="server" 
                                                TargetControlID="Cmb_Busqueda_Calle" IsSorted="true" ></cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                   <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <center>
                                               <asp:Button ID="Btn_Busqueda_Cuentas_Predial" runat="server"  Text="Buscar Cuenta Predial" CssClass="button"  
                                                CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Cuentas_Predial_Click" /> 
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
                                            <asp:GridView ID="Grid_Cuentas_Predial" runat="server" AllowPaging="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                                OnPageIndexChanging="Grid_Cuentas_Predial_PageIndexChanging" 
                                                OnSelectedIndexChanged="Grid_Cuentas_Predial_SelectedIndexChanged" PageSize="5" 
                                                Width="98%">
                                                <RowStyle CssClass="GridItem" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle Width="5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="CUENTA_PREDIAL_ID">
                                                        <FooterStyle Font-Size="0pt" Width="0px" />
                                                        <HeaderStyle Font-Size="0pt" Width="0px" />
                                                        <ItemStyle Font-Size="0pt" Width="0px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial">
                                                        <ItemStyle Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PROPIETARIO_ID" HeaderText="PROPIETARIO_ID">
                                                        <FooterStyle Font-Size="0pt" Width="0px" />
                                                        <HeaderStyle Font-Size="0pt" Width="0px" />
                                                        <ItemStyle Font-Size="0pt" Width="0px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NOMBRE_PROPIETARIO" HeaderText="Propietario">
                                                        <ItemStyle Width="65%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="COLONIA_ID" HeaderText="COLONIA_ID">
                                                        <FooterStyle Font-Size="0pt" Width="0px" />
                                                        <HeaderStyle Font-Size="0pt" Width="0px" />
                                                        <ItemStyle Font-Size="0pt" Width="0px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NOMBRE_COLONIA" HeaderText="Colonia">
                                                        <ItemStyle Width="65%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CALLE_ID" HeaderText="CALLE_ID">
                                                        <FooterStyle Font-Size="0pt" Width="0px" />
                                                        <HeaderStyle Font-Size="0pt" Width="0px" />
                                                        <ItemStyle Font-Size="0pt" Width="0px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NOMBRE_CALLE" HeaderText="Calle">
                                                        <ItemStyle Width="65%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                        <ItemStyle Width="5%" />
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
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <center>
                                               <asp:Button ID="Btn_Busqueda_Aceptar" runat="server"  
                                                    Text="Aceptar" CssClass="button"  CausesValidation="false" Width="200px" onclick="Btn_Busqueda_Aceptar_Click" /> 
                                            </center>
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
--%>
    <%----------------------------- Panel detalle de Cuenta Predial -----------------------------%>
    <asp:Panel ID="Pnl_Contenedor_Detalle_Cuenta_Predial" runat="server" CssClass="drag"
        Style="display: none; width: 650px; border-style: outset; border-color: Silver;
        background-image: url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG); background-repeat: repeat-y;">
        <asp:Panel ID="Pnl_Cuentas_Cabacera" runat="server" Style="cursor: move; background-color: Silver;
            color: Black; font-size: 12; font-weight: bold; border-style: outset;">
            <table width="99%">
                <tr>
                    <td style="color: Black; text-align: center; font-size: 12; font-weight: bold;">
                        <asp:Image ID="Img_Icono_Detalle_Cuenta_Predial" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" />
                        Detalle: Cuenta Predial
                    </td>
                    <td align="right" style="width: 5%;">
                        <asp:ImageButton ID="Btn_Cerrar_Detalle_Cuenta_Predial" runat="server" Style="cursor: pointer;"
                            ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"
                            OnClick="Btn_Cerrar_Detalle_Cuenta_Predial_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:UpdatePanel ID="Upd_Panel_Detalle_Cuenta_Predial" runat="server">
            <ContentTemplate>
                <div style="color: #5D7B9D">
                    <table width="100%">
                        <tr>
                            <td align="left" style="text-align: left;">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100%" colspan="4" align="right">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left; font-size: 11px;">
                                            Cuenta Predial
                                        </td>
                                        <td style="width: 35%; text-align: left; font-size: 11px;">
                                            <asp:TextBox ID="Txt_Detalle_Cuenta_Predial" runat="server" Width="98%" MaxLength="20"
                                                Enabled="False" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Cuenta_Predial" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                                TargetControlID="Txt_Detalle_Cuenta_Predial" />
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Detalle_Cuenta_Predial" runat="server"
                                                TargetControlID="Txt_Detalle_Cuenta_Predial" WatermarkText="Detalle de Cuenta Predial"
                                                WatermarkCssClass="watermarked" />
                                        </td>
                                        <td style="width: 15%; text-align: right; font-size: 11px;">
                                            Estatus
                                        </td>
                                        <td style="width: 35%; text-align: left;">
                                            <asp:TextBox ID="Txt_Detalle_Estatus" runat="server" Width="98%" MaxLength="20" Enabled="False" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Estatus" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                                TargetControlID="Txt_Detalle_Estatus" />
                                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="Txt_Detalle_Estatus"
                                                WatermarkText="Detalle de Estatus" WatermarkCssClass="watermarked" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left; font-size: 11px;">
                                            Propietario
                                        </td>
                                        <td style="width: 85%; text-align: left;" colspan="3">
                                            <asp:TextBox ID="Txt_Detalle_Propietatio" runat="server" Width="99.5%" MaxLength="80"
                                                Enabled="False" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Propietatio" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom"
                                                ValidChars="ÑñáéíóúÁÉÍÓÚ " TargetControlID="Txt_Detalle_Propietatio" />
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Detalle_Propietatio" runat="server" TargetControlID="Txt_Detalle_Propietatio"
                                                WatermarkText="Detalle de Nombre de Propietario o Copropietario" WatermarkCssClass="watermarked" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left; font-size: 11px;">
                                            Colonia
                                        </td>
                                        <td style="width: 35%; text-align: left;">
                                            <asp:TextBox ID="Txt_Detalle_Colonia" runat="server" Width="98%" MaxLength="20" Enabled="False" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Colonia" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                                TargetControlID="Txt_Detalle_Colonia" />
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Detalle_Colonia" runat="server" TargetControlID="Txt_Detalle_Colonia"
                                                WatermarkText="Detalle de Colonia" WatermarkCssClass="watermarked" />
                                        </td>
                                        <td style="width: 15%; text-align: right; font-size: 11px;">
                                            Calle
                                        </td>
                                        <td style="width: 35%; text-align: right;">
                                            <asp:TextBox ID="Txt_Detalle_Calle" runat="server" Width="98%" MaxLength="20" Enabled="False" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Calle" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                                TargetControlID="Txt_Detalle_Calle" />
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Detalle_Calle" runat="server" TargetControlID="Txt_Detalle_Calle"
                                                WatermarkText="Detalle de Calle" WatermarkCssClass="watermarked" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            &nbsp;
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
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Constancias_Reimpresion.aspx.cs"
    Inherits="paginas_predial_Frm_Ope_Pre_Constancias_Reimpresion" Title="Reimpresiones"
    Culture="auto" UICulture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">

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
                            Reimpresiones de Constancias
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
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click" />
                        </td>
                        <td>
                            Busqueda
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="150px"></asp:TextBox>
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
                        <td style="width: 30%; text-align: left;">
                            <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="96.4%" AutoPostBack="true"
                                TabIndex="9" MaxLength="20"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cuenta_Predial" runat="server" TargetControlID="Txt_Cuenta_Predial"
                                FilterType="UppercaseLetters, LowercaseLetters, Numbers" />
                        </td>
                        <td style="text-align: left; width: 5%; text-align: left; position: fixed;">
                            <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada" runat="server" Height="24px"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" TabIndex="10" ToolTip="Búsqueda Avanzada de Cuenta Predial"
                                Width="24px" OnClick="Btn_Mostrar_Busqueda_Avanzada_Click" />
                        </td>
                        <td style="text-align: left; width: 15%; text-align: right;">
                            Folio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Folio" runat="server" Width="98%" AutoPostBack="True" OnTextChanged="Txt_Folio_TextChanged"
                                Style="text-transform: uppercase"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Observaciones
                        </td>
                        <td colspan="4" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="Txt_Observaciones" runat="server" AutoPostBack="True" MaxLength="250"
                                TabIndex="10" TextMode="MultiLine" Width="98.6%" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios" runat="server" TargetControlID="Txt_Observaciones"
                                WatermarkCssClass="watermarked" WatermarkText="Límite de Caracteres 250" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                TargetControlID="Txt_Observaciones" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                        </td>
                    </tr>
                    <caption>
                        <asp:HiddenField ID="Hdf_No_Constancia" runat="server" />
                        <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                        <asp:HiddenField ID="Hdf_Propietario_ID" runat="server" />
                        <tr align="center">
                            <td colspan="5">
                                <br />
                                <asp:GridView ID="Grid_Constancias" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    CssClass="GridView_1" HeaderStyle-CssClass="tblHead" PageSize="5" Style="white-space: normal;"
                                    Width="100%" OnPageIndexChanging="Grid_Constancias_PageIndexChanging" OnSelectedIndexChanged="Grid_Constancias_SelectedIndexChanged"
                                    OnRowCommand="Grid_Constancias_RowCommand" DataKeyNames="TIPO_CONSTANCIA_ID,SOLICITANTE,RFC,Nombre_Documento,Periodo"
                                    OnRowDataBound="Grid_Constancias_RowDataBound">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta predial" SortExpression="CUENTA_PREDIAL">
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_PROPIETARIO" HeaderText="Propietario/Solidario"
                                            SortExpression="NOMBRE_PROPIETARIO">
                                            <ItemStyle Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SOLICITANTE" HeaderText="Solicitante" Visible="False">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RFC" HeaderText="RFC" Visible="False">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TIPO_CONSTANCIA_ID" HeaderText="TIPO_CONSTANCIA_ID" Visible="False" />
                                        <asp:BoundField DataField="TIPO_CONSTANCIA" HeaderText="Tipo Constancia">
                                            <ItemStyle Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Nombre_Documento" HeaderText="Nombre documento" Visible="False" />
                                        <asp:BoundField DataField="Periodo" HeaderText="Periodo" Visible="False" />
                                        <asp:BoundField DataField="FOLIO" HeaderText="Folio" SortExpression="FOLIO">
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA" HeaderText="Fecha" SortExpression="FECHA" DataFormatString="{0:dd/MMM/yyyy}">
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS">
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:BoundField>
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
                <%--                <cc1:ModalPopupExtender ID="Mpe_Detalles_Cuenta_Predial" runat="server" 
                    TargetControlID="Btn_Comodin_Open_Detalle_Cuenta_Predial"
                    PopupControlID="Pnl_Contenedor_Detalle_Cuenta_Predial" 
                    BackgroundCssClass="popUpStyle"
                    BehaviorID="Detalle_Cuenta_Predial"
                    CancelControlID="Btn_Comodin_Close_Detalle_Cuenta_Predial"
                    DropShadow="true" 
                    DynamicServicePath="" 
                    Enabled="True" />
                <asp:Button ID="Btn_Comodin_Close_Detalle_Cuenta_Predial" runat="server"
                    Style="background-color: transparent; border-style:none;display:none;"  Text="" />
                <asp:Button  ID="Btn_Comodin_Open_Detalle_Cuenta_Predial" runat="server"
                    Style="background-color:transparent; border-style:none;display:none;" Text="" />
--%>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--        <asp:Panel ID="Pnl_Contenedor_Detalle_Cuenta_Predial" runat="server" CssClass="drag" 
        style="display:none;width:650px;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">
            <asp:Panel ID="Pnl_Cuentas_Cabacera" runat="server" 
                style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                <table width="99%">
                    <tr>
                        <td style="color:Black; text-align:center; font-size:12; font-weight:bold;">
                            <asp:Image ID="Img_Icono_Detalle_Cuenta_Predial" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" />
                            Detalle: Cuenta Predial
                        </td>
                        <td align="right" style="width:5%;">
                           <asp:ImageButton ID="Btn_Cerrar_Busqueda_Cuentas" runat="server" 
                                style="cursor:pointer;" ToolTip="Cerrar Ventana"
                                ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" 
                                onclick="Btn_Cerrar_Detalle_Cuenta_Predial_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:UpdatePanel ID="Upd_Panel_Detalle_Cuenta_Predial" runat="server">
                <ContentTemplate>
                    <div style="color: #5D7B9D">
                     <table width="100%">
                        <tr>
                            <td align="left" style="text-align: left;" >
                               
                                <table width="100%">
                                    <tr>
                                        <td style="width:100%" colspan="4" align="right">
                                        <br />
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
                                           <asp:TextBox ID="Txt_Detalle_Cuenta_Predial" runat="server" Width="98%" MaxLength="20" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Cuenta_Predial" runat="server" 
                                                FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                                TargetControlID="Txt_Detalle_Cuenta_Predial"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Detalle_Cuenta_Predial" runat="server" 
                                                TargetControlID ="Txt_Detalle_Cuenta_Predial" WatermarkText="Detalle de Cuenta Predial" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                        <td style="width:15%; text-align:right; font-size:11px;">
                                            Estatus
                                        </td>
                                        <td style="width:35%;text-align:left;">
                                           <asp:TextBox ID="Txt_Detalle_Estatus" runat="server" Width="98%" MaxLength="20" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Estatus" runat="server" 
                                                FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                                TargetControlID="Txt_Detalle_Estatus"/>
                                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                                                TargetControlID ="Txt_Detalle_Estatus" WatermarkText="Detalle de Estatus" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%;text-align:left;font-size:11px;">
                                            Propietario
                                        </td>     
                                        <td style="width:85%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Detalle_Propietatio" runat="server" Width="99.5%" MaxLength="80" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Propietatio" runat="server" 
                                                FilterType="LowercaseLetters, UppercaseLetters, Custom" 
                                                ValidChars="ÑñáéíóúÁÉÍÓÚ "
                                                TargetControlID="Txt_Detalle_Propietatio" />
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Detalle_Propietatio" runat="server" 
                                                TargetControlID ="Txt_Detalle_Propietatio" 
                                                WatermarkText="Detalle de Nombre de Propietario o Copropietario" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%; text-align:left; font-size:11px;">
                                            Colonia
                                        </td>
                                        <td style="width:35%;text-align:left;">
                                           <asp:TextBox ID="Txt_Detalle_Colonia" runat="server" Width="98%" MaxLength="20" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Colonia" runat="server" 
                                                FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                                TargetControlID="Txt_Detalle_Colonia"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Detalle_Colonia" runat="server" 
                                                TargetControlID ="Txt_Detalle_Colonia" WatermarkText="Detalle de Colonia" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                        <td style="width:15%; text-align:right; font-size:11px;">
                                            Calle
                                        </td>
                                        <td style="width:35%;text-align:right;">
                                           <asp:TextBox ID="Txt_Detalle_Calle" runat="server" Width="98%" MaxLength="20" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Calle" runat="server" 
                                                FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                                TargetControlID="Txt_Detalle_Calle"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Detalle_Calle" runat="server" 
                                                TargetControlID ="Txt_Detalle_Calle" WatermarkText="Detalle de Calle" 
                                                WatermarkCssClass="watermarked"/>
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
                                               <asp:Button ID="Btn_Refrescar_Detalle_Cuenta_Predial" runat="server"  Text="Refrescar" CssClass="button"  
                                                CausesValidation="false" Width="200px" OnClick="Btn_Refrescar_Detalle_Cuenta_Predial_Click" /> 
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
--%>
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
    <%--        <asp:Panel ID="Pnl_Contenedor_Detalle_Cuenta_Predial" runat="server" CssClass="drag" 
        style="display:none;width:650px;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">
            <asp:Panel ID="Pnl_Cuentas_Cabacera" runat="server" 
                style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                <table width="99%">
                    <tr>
                        <td style="color:Black; text-align:center; font-size:12; font-weight:bold;">
                            <asp:Image ID="Img_Icono_Detalle_Cuenta_Predial" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" />
                            Detalle: Cuenta Predial
                        </td>
                        <td align="right" style="width:5%;">
                           <asp:ImageButton ID="Btn_Cerrar_Busqueda_Cuentas" runat="server" 
                                style="cursor:pointer;" ToolTip="Cerrar Ventana"
                                ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" 
                                onclick="Btn_Cerrar_Detalle_Cuenta_Predial_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:UpdatePanel ID="Upd_Panel_Detalle_Cuenta_Predial" runat="server">
                <ContentTemplate>
                    <div style="color: #5D7B9D">
                     <table width="100%">
                        <tr>
                            <td align="left" style="text-align: left;" >
                               
                                <table width="100%">
                                    <tr>
                                        <td style="width:100%" colspan="4" align="right">
                                        <br />
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
                                           <asp:TextBox ID="Txt_Detalle_Cuenta_Predial" runat="server" Width="98%" MaxLength="20" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Cuenta_Predial" runat="server" 
                                                FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                                TargetControlID="Txt_Detalle_Cuenta_Predial"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Detalle_Cuenta_Predial" runat="server" 
                                                TargetControlID ="Txt_Detalle_Cuenta_Predial" WatermarkText="Detalle de Cuenta Predial" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                        <td style="width:15%; text-align:right; font-size:11px;">
                                            Estatus
                                        </td>
                                        <td style="width:35%;text-align:left;">
                                           <asp:TextBox ID="Txt_Detalle_Estatus" runat="server" Width="98%" MaxLength="20" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Estatus" runat="server" 
                                                FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                                TargetControlID="Txt_Detalle_Estatus"/>
                                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                                                TargetControlID ="Txt_Detalle_Estatus" WatermarkText="Detalle de Estatus" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%;text-align:left;font-size:11px;">
                                            Propietario
                                        </td>     
                                        <td style="width:85%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Detalle_Propietatio" runat="server" Width="99.5%" MaxLength="80" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Propietatio" runat="server" 
                                                FilterType="LowercaseLetters, UppercaseLetters, Custom" 
                                                ValidChars="ÑñáéíóúÁÉÍÓÚ "
                                                TargetControlID="Txt_Detalle_Propietatio" />
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Detalle_Propietatio" runat="server" 
                                                TargetControlID ="Txt_Detalle_Propietatio" 
                                                WatermarkText="Detalle de Nombre de Propietario o Copropietario" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%; text-align:left; font-size:11px;">
                                            Colonia
                                        </td>
                                        <td style="width:35%;text-align:left;">
                                           <asp:TextBox ID="Txt_Detalle_Colonia" runat="server" Width="98%" MaxLength="20" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Colonia" runat="server" 
                                                FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                                TargetControlID="Txt_Detalle_Colonia"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Detalle_Colonia" runat="server" 
                                                TargetControlID ="Txt_Detalle_Colonia" WatermarkText="Detalle de Colonia" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                        <td style="width:15%; text-align:right; font-size:11px;">
                                            Calle
                                        </td>
                                        <td style="width:35%;text-align:right;">
                                           <asp:TextBox ID="Txt_Detalle_Calle" runat="server" Width="98%" MaxLength="20" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Calle" runat="server" 
                                                FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                                TargetControlID="Txt_Detalle_Calle"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Detalle_Calle" runat="server" 
                                                TargetControlID ="Txt_Detalle_Calle" WatermarkText="Detalle de Calle" 
                                                WatermarkCssClass="watermarked"/>
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
                                               <asp:Button ID="Btn_Refrescar_Detalle_Cuenta_Predial" runat="server"  Text="Refrescar" CssClass="button"  
                                                CausesValidation="false" Width="200px" OnClick="Btn_Refrescar_Detalle_Cuenta_Predial_Click" /> 
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
--%>
</asp:Content>

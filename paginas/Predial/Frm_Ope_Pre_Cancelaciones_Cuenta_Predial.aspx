<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Cancelaciones_Cuenta_Predial.aspx.cs"
    Inherits="paginas_Predial_Frm_Ope_Pre_Cancelaciones_Cuenta_Predial" Culture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
        .style1
        {
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

        function Abrir_Detalle_Cuenta_Predial() {
        $find('Detalle_Cuenta_Predial').show();
        return false;
        }
        
        //Abrir una ventana modal
		function Abrir_Ventana_Modal(Url, Propiedades)
		{
			window.showModalDialog(Url, null, Propiedades);
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
                            Cancelación de cuentas predial
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
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
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                                                TabIndex="4" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" OnClick="Btn_Imprimir_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            <table style="width: 100%; height: 28px;">
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                        B&uacute;squeda:
                                                    </td>
                                                    <td style="width: 55%;">
                                                        <asp:TextBox ID="Txt_Busqueda_Cuenta" runat="server" MaxLength="100" TabIndex="5"
                                                            ToolTip="Buscar por Nombre" Width="180px" />
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Producto" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Cuenta Predial>" TargetControlID="Txt_Busqueda_Cuenta" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Producto" runat="server" TargetControlID="Txt_Busqueda_Cuenta"
                                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. " />
                                                    </td>
                                                    <td style="vertical-align: middle; width: 5%;">
                                                        <asp:ImageButton ID="Btn_Buscar_Cancelacion" runat="server" TabIndex="6" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                            ToolTip="Buscar" OnClick="Btn_Buscar_Cancelacion_Click" />
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
                        <td style="width: 15%; text-align: left; font-size: 11px;">
                            *Cuenta Predial
                        </td>
                        <td style="width: 35%; text-align: left; font-size: 11px;">
                            <asp:TextBox ID="Txt_Detalle_Cuenta_Predial" runat="server" Width="98%" MaxLength="20"
                                Enabled="False" />
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                TargetControlID="Txt_Detalle_Cuenta_Predial" />
                        </td>
                        <td style="text-align: left; font-size: 11px;" class="style1">
                            <asp:ImageButton ID="Btn_Lanzar_Mpe_Busqueda_Avanzada" runat="server" Height="24px"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" OnClick="Btn_Lanzar_Mpe_Busqueda_Avanzada_Click"
                                TabIndex="10" ToolTip="Búsqueda Avanzada de Cuenta Predial" Width="24px" />
                        </td>
                        <td style="width: 35%; text-align: left;">
                            <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%; text-align: left; font-size: 11px;">
                            Propietario
                        </td>
                        <td style="width: 85%; text-align: left;" colspan="3">
                            <asp:TextBox ID="Txt_Detalle_Propietatio" runat="server" Width="99.5%" MaxLength="80"
                                Enabled="False" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%; text-align: left; font-size: 11px;">
                            Ubicación
                        </td>
                        <td style="text-align: left;" colspan="3">
                            <asp:TextBox ID="Txt_Detalle_Colonia" runat="server" Width="99.5%" MaxLength="20"
                                Enabled="False" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                </table>
                <hr />
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="width: 15%; text-align: left; font-size: 11px;">
                            *Movimiento
                        </td>
                        <td style="text-align: left; width: 85%;" colspan="2">
                            <asp:DropDownList ID="Cmb_Movimiento" runat="server" Width="100%" TabIndex="7">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%; text-align: left; font-size: 11px;">
                            *Observaciones Cancelación
                        </td>
                        <td colspan="2" style="text-align: left; width: 85%;">
                            <asp:TextBox ID="Txt_Observaciones_Cancelacion" runat="server" TabIndex="10" MaxLength="250"
                                Style="text-transform: uppercase" TextMode="MultiLine" Width="99.3%" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Area" runat="server" TargetControlID="Txt_Observaciones_Cancelacion"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;(){}[]áéíóúÁÉÍÓÚ@¿?¡!_|°#$%&/*-+¨~^ " />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%; text-align: left; font-size: 11px;">
                            *Observaciones Validación
                        </td>
                        <td colspan="2" style="text-align: left; width: 85%;">
                            <asp:TextBox ID="Txt_Observaciones_Validacion" runat="server" TabIndex="10" MaxLength="250"
                                Style="text-transform: uppercase" TextMode="MultiLine" Width="99.3%" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Observaciones_Validacion" runat="server"
                                TargetControlID="Txt_Observaciones_Validacion" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                ValidChars="Ññ.,:;(){}[]áéíóúÁÉÍÓÚ@¿?¡!_|°#$%&/*-+¨~^ " />
                        </td>
                    </tr>
                </table>
                <br />
                <hr />
                <br />
                <div style="color: #25406D;">
                    Filtrar cancelaciones por fecha:</div>
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Fecha inicial
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="84%" TabIndex="12" MaxLength="11"
                                Height="18px" OnTextChanged="Txt_Fecha_Inicial_TextChanged" />
                            <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Inicial" runat="server" TargetControlID="Txt_Fecha_Inicial"
                                WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Inicial" runat="server" TargetControlID="Txt_Fecha_Inicial"
                                Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Txt_Fecha_Inicial" />
                            <asp:ImageButton ID="Btn_Txt_Fecha_Inicial" runat="server" ImageUrl="../imagenes/paginas/SmallCalendar.gif"
                                Style="vertical-align: top;" Height="18px" CausesValidation="false" />
                            <cc1:MaskedEditExtender ID="Msk_Txt_Fecha_Inicial" Mask="99/LLL/9999" runat="server"
                                MaskType="None" UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/"
                                TargetControlID="Txt_Fecha_Inicial" Enabled="True" CultureName="es-MX" ClearMaskOnLostFocus="false" />
                            <cc1:MaskedEditValidator ID="Mev_Txt_Fecha_Inicial" runat="server" ControlToValidate="Txt_Fecha_Inicial"
                                ControlExtender="Msk_Txt_Fecha_Inicial" EmptyValueMessage="Fecha Requerida" InvalidValueMessage="Fecha Inicial Inválida"
                                IsValidEmpty="false" TooltipMessage="Ingrese o Seleccione la Fecha Inicial" Enabled="true"
                                Style="font-size: 10px; background-color: #F0F8FF; color: Black; font-weight: bold;" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Fecha final
                        </td>
                        <td style="width: 31.5%; text-align: left;">
                            <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="84%" TabIndex="12" MaxLength="11"
                                Height="18px" OnTextChanged="Txt_Fecha_Final_TextChanged" />
                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Fecha_Final" runat="server" TargetControlID="Txt_Fecha_Final"
                                WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Final" runat="server" TargetControlID="Txt_Fecha_Final"
                                Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Txt_Fecha_Final" />
                            <asp:ImageButton ID="Btn_Txt_Fecha_Final" runat="server" ImageUrl="../imagenes/paginas/SmallCalendar.gif"
                                Style="vertical-align: top;" Height="18px" CausesValidation="false" />
                            <cc1:MaskedEditExtender ID="Msk_Txt_Fecha_Final" Mask="99/LLL/9999" runat="server"
                                MaskType="None" UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/"
                                TargetControlID="Txt_Fecha_Final" Enabled="True" ClearMaskOnLostFocus="false" />
                            <cc1:MaskedEditValidator ID="Mev_Txt_Fecha_Final" runat="server" ControlToValidate="Txt_Fecha_Final"
                                ControlExtender="Msk_Txt_Fecha_Final" EmptyValueMessage="Fecha Requerida" InvalidValueMessage="Fecha Final Inválida"
                                IsValidEmpty="false" TooltipMessage="Ingrese o Seleccione la Fecha Final" Enabled="true"
                                Style="font-size: 10px; background-color: #F0F8FF; color: Black; font-weight: bold;" />
                        </td>
                        <td>
                            <asp:ImageButton ID="Btn_Consultar_Por_Fechas" runat="server" Height="24px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                OnClick="Btn_Consultar_Por_Fechas_Click" TabIndex="10" ToolTip="Consultar Cuentas por Rango de Fecha"
                                Width="24px" />
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="5">
                            <br />
                            <asp:GridView ID="Grid_Cancelacion" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CssClass="GridView_1" GridLines="None" Style="white-space: normal" Width="100%"
                                OnSelectedIndexChanged="Grid_Cancelacion_SelectedIndexChanged" OnPageIndexChanging="Grid_Cancelaciones_PageIndexChanging"
                                DataKeyNames="MOVIMIENTO_ID,NO_ORDEN,ANIO_ORDEN,CUENTA_PREDIAL_ID,OBSERVACIONES_VALIDACION">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <HeaderStyle Width="2%" HorizontalAlign="Left" />
                                        <ItemStyle Width="2%" HorizontalAlign="Center" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="Id" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial" SortExpression="Cuenta Predial">
                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA_CREO" HeaderText="Fecha de Cancelación" DataFormatString="{0:dd/MMM/yyyy}">
                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TOTAL_RECARGOS" HeaderText="Total Recargos" DataFormatString="{0:C2}">
                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemStyle HorizontalAlign="Right" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TOTAL_CORRIENTE" HeaderText="Total Corriente" DataFormatString="{0:C2}">
                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemStyle HorizontalAlign="Right" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TOTAL_REZAGO" HeaderText="Total Rezago" DataFormatString="{0:C2}">
                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemStyle HorizontalAlign="Right" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ADEUDO_CANCELADO" HeaderText="Adeudo Cancelado" DataFormatString="{0:C2}">
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Right" Width="8%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TIPO_MOVIMIENTO" HeaderText="Tipo de Movimiento">
                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="OBSERVACIONES_CANCELACION" HeaderText="Motivo Cancelación">
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="OBSERVACIONES_VALIDACION" HeaderText="Motivo Cancelación"
                                        Visible="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MOVIMIENTO_ID" HeaderText="Movimiento_Id" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_ORDEN" HeaderText="No Orden" Visible="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ANIO_ORDEN" HeaderText="Año" Visible="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="Txt_Adeudo_Id" runat="server" Width="96.4%" Visible="false" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Adeudo total cancelado
                        </td>
                        <td colspan="2" style="width: 31.5%; text-align: left;">
                            <asp:TextBox ID="Txt_Adeudo_Total_Cancelado" runat="server" Width="96.4%" ReadOnly="true"
                                Enabled="False" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <td>
                                <asp:TextBox ID="Txt_Fecha_Inicial_Filtro" runat="server" Width="20%" Visible="false" />
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Fecha_Final_Filtro" runat="server" Width="99.5%" Visible="false" />
                            </td>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="Hdf_Orden_Variacion" runat="server" />
            <asp:HiddenField ID="Hdf_Anio_Orden" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <%----------------------------- Panel detalle de Cuenta Predial -----------------------------%>
    <%--<asp:Panel ID="Pnl_Contenedor_Detalle_Cuenta_Predial" runat="server" CssClass="drag" 
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
                           <asp:ImageButton ID="Btn_Cerrar_Detalle_Cuenta_Predial" runat="server" 
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
                                           <asp:TextBox ID="Txt_Detalle_Cuenta_Predial" runat="server" Width="98%" 
                                                MaxLength="20" Enabled="False" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Cuenta_Predial" runat="server" 
                                                FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                                TargetControlID="Txt_Detalle_Cuenta_Predial"/>
                                            
                                        </td>
                                        <td style="width:15%; text-align:right; font-size:11px;">
                                            Estatus
                                        </td>
                                        <td style="width:35%;text-align:left;">
                                           <asp:TextBox ID="Txt_Detalle_Estatus" runat="server" Width="98%" MaxLength="20" 
                                                Enabled="False" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Estatus" runat="server" 
                                                FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                                TargetControlID="Txt_Detalle_Estatus"/>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%;text-align:left;font-size:11px;">
                                            Propietario
                                        </td>     
                                        <td style="width:85%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Detalle_Propietatio" runat="server" Width="99.5%" 
                                                MaxLength="80" Enabled="False" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Propietatio" runat="server" 
                                                FilterType="LowercaseLetters, UppercaseLetters, Custom" 
                                                ValidChars="ÑñáéíóúÁÉÍÓÚ "
                                                TargetControlID="Txt_Detalle_Propietatio" />
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%; text-align:left; font-size:11px;">
                                            Colonia
                                        </td>
                                        <td style="width:35%;text-align:left;">
                                           <asp:TextBox ID="Txt_Detalle_Colonia" runat="server" Width="98%" MaxLength="20" 
                                                Enabled="False" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Colonia" runat="server" 
                                                FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                                TargetControlID="Txt_Detalle_Colonia"/>
                                            
                                        </td>
                                        <td style="width:15%; text-align:right; font-size:11px;">
                                            Calle
                                        </td>
                                        <td style="width:35%;text-align:right;">
                                           <asp:TextBox ID="Txt_Detalle_Calle" runat="server" Width="98%" MaxLength="20" 
                                                Enabled="False" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Calle" runat="server" 
                                                FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                                TargetControlID="Txt_Detalle_Calle"/>
                                            
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan="4">
                                            <hr />
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
        </asp:Panel--%>
</asp:Content>

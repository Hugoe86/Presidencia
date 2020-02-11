<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Rpt_Cajas_General.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Rpt_Cajas_General" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type="text/javascript" language="javascript">
        //Metodo para mantener los calendarios en una capa mas alat.
         function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
        //Metodos para limpiar los controles de la busqueda.
        function Limpiar_Ctlr(){
            $("input[id$=Txt_Busqueda_No_Empleado]").val("");
            $("input[id$=Txt_Busqueda_Nombre_Empleado]").val("");
            $("[id$=Lbl_Numero_Registros]").text("");
            $('[id$=Lbl_Error_Busqueda]').text("");
            $('[id$=Lbl_Error_Busqueda]').css("display", "none");
            $('[id$=Img_Error_Busqueda]').hide();
            $("#grid").remove();
            return false;
        }  
        function Abrir_Modal_Popup() {
            $find('Busqueda_Empleados').show();
            return false;
        }
        function Cerrar_Modal_Popup() {
            $find('Busqueda_Empleados').hide();
            Limpiar_Ctlr();
            return false;
        } 
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="True" />
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
                            Reportes Generales
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
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="imprimir" AlternateText="Imprimir"
                                                CssClass="Img_Button" TabIndex="1" OnClick="Btn_Nuevo_Click" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" OnClick="Btn_Salir_Click" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" />
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
                        <td colspan="4">
                            <asp:RadioButtonList ID="Rdb_Tipo_Reporte" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Rdb_Tipo_Reporte_Click">
                                <asp:ListItem Value="1">Reporte General</asp:ListItem>
                                <asp:ListItem Value="2">Reporte Detallado</asp:ListItem>
                                <asp:ListItem Value="3">Reporte Cancelaciones</asp:ListItem>
                                <asp:ListItem Value="15">Reporte Cancelaciones por Empleado</asp:ListItem>
                                <asp:ListItem Value="4">Reporte Folios Inutilizados</asp:ListItem>
                                <asp:ListItem Value="5">Concentración Monetaria por Caja</asp:ListItem>
                                <asp:ListItem Value="6">Reporte Concentrado Tarjeta Bancaria</asp:ListItem>
                                <asp:ListItem Value="7">Reporte Desglosado Tarjeta Bancaria</asp:ListItem>
                                <asp:ListItem Value="8">Concentración Monetaria del Ingreso</asp:ListItem>
                                <asp:ListItem Value="9">Corte de Caja</asp:ListItem>
                                <asp:ListItem Value="10">Resumen Diario de Ingresos</asp:ListItem>
                                <asp:ListItem Value="11">Reporte de análisis de entrega por día</asp:ListItem>
                                <asp:ListItem Value="12">Reporte Detallado de Pagos con Tarjeta</asp:ListItem>
                                <asp:ListItem Value="13">Reporte Detallado de Pagos con Cheque</asp:ListItem>
                                <asp:ListItem Value="14">Reporte Detallado de Pagos con Transferencia</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="text-align: left; width: 10%;">
                            Modulo
                        </td>
                        <td style="text-align: left; width: 14%;" colspan="2">
                            <asp:DropDownList ID="Cmb_Modulos" runat="server" Width="80%" AutoPostBack="true"
                                OnSelectedIndexChanged="Cmb_Modulos_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 10%;">
                            Caja
                        </td>
                        <td style="text-align: left; width: 14%;" colspan="2">
                            <asp:DropDownList ID="Cmb_Cajas" runat="server" Width="80%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 10%;">
                            Fecha Inicial
                        </td>
                        <td align="left">
                            <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Enabled="false" Width="75%" />
                            <cc1:CalendarExtender ID="Ce_Txt_Fecha" runat="server" TargetControlID="Txt_Fecha_Inicial"
                                Format="dd/MMM/yyyy" PopupButtonID="Btn_Fecha" />
                            <asp:ImageButton ID="Btn_Fecha" runat="server" Width="15%" ImageUrl="~/paginas/imagenes/gridview/grid_calendar.png"
                                Style="vertical-align: top;" Height="18px" CausesValidation="false" />
                            <cc1:MaskedEditExtender ID="Mee_Txt_Fecha" Mask="99/LLL/9999" runat="server" MaskType="None"
                                UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Inicial"
                                Enabled="True" ClearMaskOnLostFocus="false" />
                            <cc1:MaskedEditValidator ID="Mev_Mee_Txt_Fecha" runat="server" ControlToValidate="Txt_Fecha_Inicial"
                                ControlExtender="Mee_Txt_Fecha" EmptyValueMessage="Fecha Inicio Vacía" InvalidValueMessage="Fecha Inicio Invalida"
                                IsValidEmpty="true" TooltipMessage="Ingrese o Seleccione la Fecha Inicio" Enabled="true"
                                Style="font-size: 10px; background-color: #F0F8FF; color: Black; font-weight: bold;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 10%;">
                            Fecha Final
                        </td>
                        <td align="left">
                            <asp:TextBox ID="Txt_Fecha_Final" runat="server" Enabled="false" Width="75%" />
                            <cc1:CalendarExtender ID="Ce_Txt_Fecha_Final" runat="server" TargetControlID="Txt_Fecha_Final"
                                Format="dd/MMM/yyyy" PopupButtonID="Btn_Fecha_final" />
                            <asp:ImageButton ID="Btn_fecha_Final" runat="server" Width="15%" ImageUrl="~/paginas/imagenes/gridview/grid_calendar.png"
                                Style="vertical-align: top;" Height="18px" CausesValidation="false" />
                            <cc1:MaskedEditExtender ID="Mee_Txt_Fecha_Final" Mask="99/LLL/9999" runat="server"
                                MaskType="None" UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/"
                                TargetControlID="Txt_Fecha_Final" Enabled="True" ClearMaskOnLostFocus="false" />
                            <cc1:MaskedEditValidator ID="Mev_Mee_Txt_Fecha_final" runat="server" ControlToValidate="Txt_Fecha_Final"
                                ControlExtender="Mee_Txt_Fecha_Final" EmptyValueMessage="Fecha Final Vacía" InvalidValueMessage="Fecha Final Invalida"
                                IsValidEmpty="true" TooltipMessage="Ingrese o Seleccione la Fecha Final" Enabled="true"
                                Style="font-size: 10px; background-color: #F0F8FF; color: Black; font-weight: bold;" />
                        </td>
                    </tr>
                    <tr>
                        <div id="Div_Contenedor_Empleado" style="width: 98%; visibility: hidden;" runat="server">
                            <table width="100%">
                                <tr>
                                    <td style="width: 5%">
                                        Empleado
                                    </td>
                                    <td style="width: 95%">
                                        <asp:TextBox ID="Txt_Empleado" runat="server" Width="80%"></asp:TextBox>
                                        &nbsp;
                                        <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Buscar Empleado" CssClass="Img_Button"
                                            TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" OnClientClick="javascript:return Abrir_Modal_Popup();" />
                                        <asp:HiddenField ID="HF_Empleado_ID" runat="server" />
                                        <cc1:ModalPopupExtender ID="Mpe_Busqueda_Empleados" runat="server" BackgroundCssClass="popUpStyle"
                                            BehaviorID="Busqueda_Empleados" PopupControlID="Pnl_Busqueda_Contenedor" TargetControlID="Btn_Comodin_Open"
                                            PopupDragHandleControlID="Pnl_Busqueda_Cabecera" CancelControlID="Btn_Comodin_Close"
                                            DropShadow="True" DynamicServicePath="" Enabled="True" />
                                        <asp:Button Style="background-color: transparent; border-style: none;" ID="Btn_Comodin_Close"
                                            runat="server" Text="" />
                                        <asp:Button Style="background-color: transparent; border-style: none;" ID="Btn_Comodin_Open"
                                            runat="server" Text="" />
                                    </td>
                                </tr>
                        </div>
                    </tr>
                </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Grid_Empleados" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag" HorizontalAlign="Center"
        Width="650px" Style="display: none; border-style: outset; border-color: Silver;
        background-image: url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG); background-repeat: repeat-y;">
        <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" Style="cursor: move; background-color: Silver;
            color: Black; font-size: 12; font-weight: bold; border-style: outset;">
            <table width="99%">
                <tr>
                    <td style="color: Black; font-size: 12; font-weight: bold;">
                        <asp:Image ID="Img_Informatcion_Autorizacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                        B&uacute;squeda: Empleados
                    </td>
                    <td align="right" style="width: 10%;">
                        <asp:ImageButton ID="Btn_Cerrar_Ventana" CausesValidation="false" runat="server"
                            Style="cursor: pointer;" ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"
                            OnClientClick="javascript:return Cerrar_Modal_Popup();" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <div style="color: #5D7B9D">
            <table width="100%">
                <tr>
                    <td align="left" style="text-align: left;">
                        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Prestamos" runat="server">
                            <ContentTemplate>
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server"
                                    AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Prestamos" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                                        </div>
                                        <div style="background-color: Transparent; position: fixed; top: 50%; left: 47%;
                                            padding: 10px; z-index: 1002;" id="div_progress">
                                            <img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table width="100%">
                                    <tr>
                                        <td colspan="2">
                                            <table style="width: 80%;">
                                                <tr>
                                                    <td align="left">
                                                        <asp:ImageButton ID="Img_Error_Busqueda" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
                                                            Width="24px" Height="24px" Style="display: none" />
                                                        <asp:Label ID="Lbl_Error_Busqueda" runat="server" Text="" CssClass="estilo_fuente_mensaje_error"
                                                            Style="display: none" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 100%" colspan="2" align="right">
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
                                            No Empleado
                                        </td>
                                        <td style="width: 30%; text-align: left; font-size: 11px;">
                                            <asp:TextBox ID="Txt_Busqueda_No_Empleado" runat="server" Width="98%" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Empleado" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Busqueda_No_Empleado" />
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_No_Empleado" runat="server" TargetControlID="Txt_Busqueda_No_Empleado"
                                                WatermarkText="Busqueda por No Empleado" WatermarkCssClass="watermarked" />
                                        </td>
                                        <td style="width: 20%; text-align: left; font-size: 11px;">
                                        </td>
                                        <td style="width: 30%; text-align: left; font-size: 11px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left; font-size: 11px;">
                                            Nombre
                                        </td>
                                        <td style="width: 30%; text-align: left;" colspan="3">
                                            <asp:TextBox ID="Txt_Busqueda_Nombre_Empleado" runat="server" Width="99.5%" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Nombre_Empleado" runat="server"
                                                FilterType="Custom, LowercaseLetters, Numbers, UppercaseLetters" TargetControlID="Txt_Busqueda_Nombre_Empleado"
                                                ValidChars="áéíóúÁÉÍÓÚ " />
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Nombre_Empleado" runat="server" TargetControlID="Txt_Busqueda_Nombre_Empleado"
                                                WatermarkText="Busqueda por Nombre" WatermarkCssClass="watermarked" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" id="grid">
                                            <asp:GridView ID="Grid_Empleados" runat="server" AllowPaging="True" CssClass="GridView_1"
                                                AutoGenerateColumns="False" GridLines="None" Width="96.5%" OnSelectedIndexChanged="Grid_Empleados_SelectedIndexChanged"
                                                HeaderStyle-CssClass="tblHead">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle Width="7%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="Empleado_ID" HeaderText="Empleado ID" Visible="True" SortExpression="Empleado_ID">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="No_Empleado" HeaderText="No Empleado" Visible="True" SortExpression="No_Empleado">
                                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Empleado" HeaderText="Nombre" Visible="True" SortExpression="Empleado">
                                                        <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                                        <ItemStyle HorizontalAlign="left" Width="60%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            <div style="text-align: center;">
                                                <asp:Label ID="Lbl_Numero_Registros" runat="server" Text="" />
                                            </div>
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
                                                <asp:Button ID="Btn_Busqueda_Empleados" runat="server" Text="Busqueda de Empleados"
                                                    CssClass="button" CausesValidation="false" OnClick="Btn_Busqueda_Empleados_Click"
                                                    Width="200px" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Btn_Busqueda_Empleados" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>

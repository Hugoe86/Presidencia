<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Reactivaciones_Cuenta_Predial.aspx.cs"
    Inherits="paginas_Predial_Frm_Ope_Pre_Reactivaciones_Cuenta_Predial" %>

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

        function Abrir_Modal_Popup_Historial()
        {
            $find('Historial_Cuenta_Predial').show();
            return false;
        }

        function Abrir_Detalle_Cuenta_Predial()
        {
            $find('Detalle_Cuenta_Predial').show();
            return false;
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
                            Reactivaciones de cuenta predial
                        </td>
                    </tr>
                    <tr>
                        <td>
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
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click" />
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
                                                        <asp:TextBox ID="Txt_Busqueda_Reactivacion" runat="server" MaxLength="100" TabIndex="5"
                                                            ToolTip="Buscar por Nombre" Width="180px" />
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Producto" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda_Reactivacion" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Producto" runat="server" TargetControlID="Txt_Busqueda_Reactivacion"
                                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. " />
                                                    </td>
                                                    <td style="vertical-align: middle; width: 5%;">
                                                        <asp:ImageButton ID="Btn_Buscar_Reactivacion" runat="server" TabIndex="6" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
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
                        <td style="text-align: left; width: 20%;">
                            *Cuenta Predial
                        </td>
                        <td style="width: 82%; text-align: left;" colspan="2">
                            <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="35.7%" AutoPostBack="true"
                                TabIndex="9" MaxLength="20" ReadOnly="true" OnTextChanged="Txt_Detalle_Cuenta_TextChanged"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cuenta_Predial" runat="server" TargetControlID="Txt_Cuenta_Predial"
                                FilterType="UppercaseLetters, LowercaseLetters, Numbers" />
                            &nbsp;
                        </td>
                        <td>
                            <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                            <asp:HiddenField ID="Hdf_No_Orden" runat="server" />
                            <asp:HiddenField ID="Hdf_Anio_Orden" runat="server" />
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
                        </td>
                        <td style="width: 15%; text-align: right; font-size: 11px;">
                            Estatus
                        </td>
                        <td style="width: 35%; text-align: left;">
                            <asp:TextBox ID="Txt_Detalle_Estatus" runat="server" Width="98%" MaxLength="20" Enabled="False" />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Estatus" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                TargetControlID="Txt_Detalle_Estatus" />
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
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%; text-align: left; font-size: 11px;">
                            Colonia
                        </td>
                        <td style="width: 35%; text-align: left;">
                            <asp:TextBox ID="Txt_Detalle_Colonia" runat="server" Width="98%" MaxLength="20" Enabled="False" />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Colonia" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom"
                                ValidChars="ÑñáéíóúÁÉÍÓÚ " TargetControlID="Txt_Detalle_Colonia" />
                        </td>
                        <td style="width: 15%; text-align: right; font-size: 11px;">
                            Calle
                        </td>
                        <td style="width: 35%; text-align: right;">
                            <asp:TextBox ID="Txt_Detalle_Calle" runat="server" Width="98%" MaxLength="20" Enabled="False" />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Detalle_Calle" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, Custom"
                                ValidChars="ÑñáéíóúÁÉÍÓÚ " TargetControlID="Txt_Detalle_Calle" />
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
                <table width="100%" class="estilo_fuente">
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            *Movimiento
                        </td>
                        <td style="text-align: left; width: 80%;" colspan="2">
                            <asp:DropDownList ID="Cmb_Movimiento" runat="server" Width="99%" TabIndex="7">
                            </asp:DropDownList>
                            <asp:TextBox ID="Txt_Adeudo_Id" runat="server" Width="96.4%" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            *Observaciones
                        </td>
                        <td colspan="2" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="Txt_Observaciones" runat="server" TabIndex="10" MaxLength="250"
                                Style="text-transform: uppercase" TextMode="MultiLine" Width="98.6%" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Area" runat="server" TargetControlID="Txt_Observaciones"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;(){}[]áéíóúÁÉÍÓÚ@¿?¡!_|°#$%&/*-+¨~^ " />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Vista previa de adeudos
                        </td>
                        <td colspan="2" style="text-align: left; width: 80%;">
                            <asp:ImageButton ID="Btn_Vista_Previa" runat="server" ToolTip="Vista Previa de Adeudos"
                                TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Height="22px"
                                Width="22px" Style="float: left" />
                            <cc1:ModalPopupExtender ID="Mpe_Historial_Cuenta" runat="server" BackgroundCssClass="popUpStyle"
                                BehaviorID="Historial_Cuenta_Predial" PopupControlID="Pnl_Historial_Contenedor"
                                TargetControlID="Btn_Comodin_Open" PopupDragHandleControlID="Pnl_Historial_Cabecera"
                                CancelControlID="Btn_Comodin_Close" DropShadow="True" DynamicServicePath="" Enabled="True" />
                            <asp:Button ID="Btn_Comodin_Close" runat="server" Style="background-color: transparent;
                                border-style: none; display: none;" Text="" />
                            <asp:Button ID="Btn_Comodin_Open" runat="server" Style="background-color: transparent;
                                border-style: none; display: none;" Text="" />
                        </td>
                    </tr>
                </table>
                <br />
                <hr />
                <br />
                <div style="color: #25406D;">
                    Filtrar reactivaciones por fecha:</div>
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
                                TargetControlID="Txt_Fecha_Inicial" Enabled="True" ClearMaskOnLostFocus="false" />
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
                            <asp:GridView ID="Grid_Adeudos" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                GridLines="none" PageSize="5" Style="white-space: normal" Width="96%">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="ANIO" HeaderText="Año" Visible="true">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ADEUDO_BIMESTRE_1" HeaderText="Saldo Bimestre 1" Visible="true">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ADEUDO_BIMESTRE_2" HeaderText="Saldo Bimestre 2" Visible="true">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ADEUDO_BIMESTRE_3" HeaderText="Saldo Bimestre 3" Visible="true">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ADEUDO_BIMESTRE_4" HeaderText="Saldo Bimestre 4" Visible="true">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ADEUDO_BIMESTRE_5" HeaderText="Saldo Bimestre 5" Visible="true">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ADEUDO_BIMESTRE_6" HeaderText="Saldo Bimestre 6" Visible="true">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MONTO_POR_PAGAR" HeaderText="Saldo Total" Visible="true">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                            <asp:GridView ID="Grid_Cancelacion" runat="server" AllowPaging="True" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="5" Width="100%" AllowSorting="True" HeaderStyle-CssClass="tblHead"
                                Style="white-space: normal;" OnPageIndexChanging="Grid_Cancelaciones_PageIndexChanging"
                                OnSelectedIndexChanged="Grid_Reactivaciones_SelectedIndexChanged" DataKeyNames="NO_ORDEN,ANIO_ORDEN,CUENTA_PREDIAL_ID">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="Id" SortExpression="Cuenta Predial"
                                        Visible="false">
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial" SortExpression="Cuenta Predial">
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA_CREO" HeaderText="Fecha de Reactivación" DataFormatString="{0:dd/MMM/yyyy}">
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ADEUDO_CANCELADO" HeaderText="Adeudo Cancelado" DataFormatString="{0:C2}">
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TIPO_MOVIMIENTO" HeaderText="Tipo de Movimiento">
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="OBSERVACIONES_CANCELACION" HeaderText="Motivo Cancelación">
                                        <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_ORDEN" HeaderText="NO_ORDEN_VARIACION" Visible="False">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ANIO_ORDEN" HeaderText="ANIO_ORDEN_VARIACION" Visible="False">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <HeaderStyle CssClass="tblHead" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="Pnl_Historial_Contenedor" runat="server" CssClass="drag" HorizontalAlign="Center"
        Width="650px" Style="display: none; border-style: outset; border-color: Silver;
        background-image: url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG); background-repeat: repeat-y;">
        <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: Silver;
            color: Black; font-size: 12; font-weight: bold; border-style: outset;">
            <table width="99%">
                <tr>
                    <td style="color: Black; font-size: 12; font-weight: bold;">
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                        Detalle: Cuenta Predial
                    </td>
                    <td align="right" style="width: 10%;">
                        <asp:ImageButton ID="Btn_Cerrar_Historial" CausesValidation="false" runat="server"
                            Style="cursor: pointer;" ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"
                            OnClick="Btn_Cerrar_Ventana_Historial_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <div style="color: #5D7B9D">
            <table width="100%">
                <tr>
                    <td align="left" style="text-align: left;">
                        <asp:UpdatePanel ID="Udp_Historial_Cuenta_Predial" runat="server">
                            <ContentTemplate>
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Historial" runat="server" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter1" class="progressBackgroundFilter">
                                        </div>
                                        <div style="background-color: Transparent; position: fixed; top: 50%; left: 47%;
                                            padding: 10px; z-index: 1002;" id="div_progress">
                                            <img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table width="100%">
                                    <tr>
                                        <td colspan="2" style="width: 100%">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left; font-size: 11px;">
                                            Adeudo Total Bimestre(s) 1
                                        </td>
                                        <td style="width: 30%; text-align: left;">
                                            <asp:TextBox ID="Txt_Adeudo_1" runat="server" Width="99.5%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left; font-size: 11px;">
                                            Adeudo Total Bimestre(s) 2
                                        </td>
                                        <td style="width: 30%; text-align: left;">
                                            <asp:TextBox ID="Txt_Adeudo_2" runat="server" Width="99.5%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left; font-size: 11px;">
                                            Adeudo Total Bimestre(s) 3
                                        </td>
                                        <td style="width: 30%; text-align: left;">
                                            <asp:TextBox ID="Txt_Adeudo_3" runat="server" Width="99.5%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left; font-size: 11px;">
                                            Adeudo Total Bimestre(s) 4
                                        </td>
                                        <td style="width: 30%; text-align: left;">
                                            <asp:TextBox ID="Txt_Adeudo_4" runat="server" Width="99.5%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left; font-size: 11px;">
                                            Adeudo Total Bimestre(s) 5
                                        </td>
                                        <td style="width: 30%; text-align: left;">
                                            <asp:TextBox ID="Txt_Adeudo_5" runat="server" Width="99.5%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left; font-size: 11px;">
                                            Adeudo Total Bimestre(s) 6
                                        </td>
                                        <td style="width: 30%; text-align: left;">
                                            <asp:TextBox ID="Txt_Adeudo_6" runat="server" Width="99.5%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; text-align: left;" colspan="2">
                                            <center>
                                            </center>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="Txt_Fecha_Inicial_Filtro" runat="server" Width="20%" Visible="false" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txt_Fecha_Final_Filtro" runat="server" Width="99.5%" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>

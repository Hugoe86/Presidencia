<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Estados_Cuenta.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Estados_Cuenta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .TooltipAdeudos
        {
            width: 240px;
            padding: 30px 30px 25px 18px;
            text-align: left;
            border: none;
            line-height: 30px;
            background: transparent url(../imagenes/TooltipAdeudos500.png) no-repeat 0 0;
        }
        .Tabla_Comentarios
        {
            border-collapse: collapse;
            margin-left: 25px;
            color: #25406D;
            font-family: Verdana,Geneva,MS Sans Serif;
            font-size: small;
            text-align: left;
            font-size: 11px;
            line-height: 15px;
        }
        .Tabla_Comentarios, .Tabla_Comentarios th, .Tabla_Comentarios td
        {
            border: 1px solid #999999;
            padding: 0;
            font-size: 11px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

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
            <%--<asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>--%>
            <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Estado de cuenta
                        </td>
                    </tr>
                    <div id="Div_Mensaje_Error" runat="server">
                        <tr>
                            <td colspan="4">
                                &nbsp;
                                <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                    Visible="false" />&nbsp;
                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                                <asp:Label ID="Lbl_Encabezado_Error" runat="server" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            </td>
                        </tr>
                    </div>
                </table>
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td colspan="2">
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Img_Btn_Imprimir_Estado_Cuenta" runat="server" ToolTip="Imprimir"
                                                CssClass="Img_Button" TabIndex="4" OnClick="Img_Btn_Imprimir_Estado_Cuenta_Click"
                                                ImageUrl="~/paginas/imagenes/gridview/grid_print.png" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" />
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            <table style="width: 100%; height: 28px;">
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                    </td>
                                                    <td style="width: 55%;">
                                                        <%--<asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5"  ToolTip = "Buscar" Width="180px"/>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                        WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" 
                                                        runat="server" TargetControlID="Txt_Busqueda" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>--%>
                                                    </td>
                                                    <td style="vertical-align: middle; width: 5%;">
                                                        <%--<asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6"
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" />--%>
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
                        <td style="text-align: left; width: 20%;">
                            Cuenta predial
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%;">
                            <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada" runat="server" ToolTip="Búsqueda Avanzada de Cuenta Predial"
                                TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px"
                                Width="24px" OnClick="Btn_Mostrar_Busqueda_Avanzada_Click" />
                        </td>
                        <td>
                            <asp:Label ID="Lbl_Estado" CssClass="estilo_fuente_mensaje_error" Visible="false"
                                runat="server" Text="Cuenta sin Adeudos"></asp:Label>
                        </td>
                    </tr>
                    <%------------------ Datos generales ------------------%>
                    <tr style="background-color: #3366CC">
                        <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                            Ubicación / Datos de predio
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Tipo predio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Tipo_Predio_General" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: right; width: 20%;">
                            Uso predio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Uso_Predio_General" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Ubicación
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Ubicacion_General" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: right; width: 20%;">
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
                        <td style="text-align: left; width: 30%;" colspan="3">
                            <asp:TextBox ID="Txt_Numero_Exterior_General" runat="server" Width="99%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Número interior
                        </td>
                        <td style="text-align: left; width: 30%;" colspan="3">
                            <asp:TextBox ID="Txt_Numero_Interior_General" runat="server" Width="99%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Valor fiscal
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Valor_Fiscal_General" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: right; width: 20%;">
                            Efectos
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Efectos_General" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Tasa
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Tasa_General" runat="server" Width="96.4%" Text="0" />
                        </td>
                        <td style="text-align: right; width: 20%;">
                            Cuota bimestral
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Cuota_Bimestral_General" runat="server" Width="96.4%" Text="0" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Ultimo movimiento
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Ultimo_Movimiento" runat="server" Width="96.4%" />
                        </td>
                        <td colspan="2">
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
                            <asp:TextBox ID="Txt_Nombre_Propietario" runat="server" Width="99%" TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            RFC
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Rfc_Propietario" runat="server" Width="96.4%" />
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Calle
                        </td>
                        <td style="text-align: left; width: 30%;" colspan="3">
                            <asp:TextBox ID="Txt_Calle_Propietario" runat="server" Width="99%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Número exterior
                        </td>
                        <td style="text-align: left; width: 30%;" colspan="3">
                            <asp:TextBox ID="Txt_Numero_Exterior_Propietario" runat="server" Width="99%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Número interior
                        </td>
                        <td style="text-align: left; width: 30%;" colspan="3">
                            <asp:TextBox ID="Txt_Numero_Interior_Propietario" runat="server" Width="99%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Colonia
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Colonia_Propietario" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: right; width: 20%;">
                            C.P.
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Cod_Pos_Propietario" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Ciudad
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Ciudad_Propietario" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: right; width: 20%;">
                            Estado
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Estado_Propietario" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table width="98%" class="estilo_fuente">
                    <%------------------ Bimestral ------------------%>
                    <tr style="background-color: #36C;">
                        <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                            Adeudos
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
                            <asp:DropDownList ID="Cmb_Bimestre" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Bimestre_SelectedIndexChanged">
                                <asp:ListItem Value="1">01</asp:ListItem>
                                <asp:ListItem Value="2">02</asp:ListItem>
                                <asp:ListItem Value="3">03</asp:ListItem>
                                <asp:ListItem Value="4">04</asp:ListItem>
                                <asp:ListItem Value="5">05</asp:ListItem>
                                <asp:ListItem Selected="True" Value="6">06</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="Cmb_Anio" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Anio_SelectedIndexChanged"
                                Width="60%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <caption>
                        <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                    </caption>
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_Estado_Cuenta" runat="server" AllowPaging="True" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="6" Width="100%" OnPageIndexChanging="Grid_Estado_Cuenta_PageIndexChanging"
                                AllowSorting="True" HeaderStyle-CssClass="tblHead" Style="white-space: normal;"
                                OnRowDataBound="Grid_Estado_Cuenta_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="Anio" HeaderText="Año">
                                        <HeaderStyle HorizontalAlign="center" Width="10%" />
                                        <ItemStyle HorizontalAlign="center" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Adeudo_Bimestre_1" HeaderText="1" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" Width="10%" />
                                        <ItemStyle HorizontalAlign="center" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Adeudo_Bimestre_2" HeaderText="2" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" Width="10%" />
                                        <ItemStyle HorizontalAlign="center" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Adeudo_Bimestre_3" HeaderText="3" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" Width="10%" />
                                        <ItemStyle HorizontalAlign="center" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Adeudo_Bimestre_4" HeaderText="4" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" Width="10%" />
                                        <ItemStyle HorizontalAlign="center" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Adeudo_Bimestre_5" HeaderText="5" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" Width="10%" />
                                        <ItemStyle HorizontalAlign="center" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Adeudo_Bimestre_6" HeaderText="6" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" Width="10%" />
                                        <ItemStyle HorizontalAlign="center" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Adeudo_Total_Anio" HeaderText="Total" DataFormatString="{0:c}">
                                        <HeaderStyle HorizontalAlign="center" Width="25%" />
                                        <ItemStyle HorizontalAlign="center" Width="25%" />
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
                        <td style="text-align: left; width: 20%;">
                            Periodo rezago
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Periodo_Rezago" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: right; width: 20%;">
                            Adeudo rezago
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Adeudo_Rezago" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Periodo corriente
                        </td>
                        <td style="text-align: left; width: 20%;">
                            <asp:TextBox ID="Txt_Periodo_Actual" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: right; width: 20%;">
                            Adeudo corriente
                        </td>
                        <td style="text-align: left; width: 20%;">
                            <asp:TextBox ID="Txt_Adeudo_Actual" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                        </td>
                        <td style="text-align: left; width: 30%;">
                        </td>
                        <td style="text-align: right; width: 20%;">
                            <%--Adeudo rezago--%>
                            Honorarios
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Honorarios" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                        </td>
                        <td style="text-align: left; width: 30%;">
                        </td>
                        <td style="text-align: right; width: 20%;">
                            <%-- Adeudo actual--%>
                            Gastos de ejecución
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <%--<asp:TextBox ID="Txt_Adeudo_Actual" runat="server" Width="96.4%" />--%>
                            <asp:TextBox ID="Txt_Gastos_Ejecucion" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td style="text-align: right; width: 30%;">
                            Total recargos ordinarios
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Total_Recargos_Ordinarios" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td style="text-align: right; width: 30%;">
                            Recargos moratorios
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Recargos_Moratorios" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td style="text-align: right; width: 20%;">
                            Subtotal
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Subtotal" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                        </td>
                        <td style="text-align: left; width: 30%;">
                        </td>
                        <td style="text-align: right; width: 20%;">
                            Descuento por pronto pago
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Descuento_Pronto_Pago" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;" colspan="2">
                            &nbsp;
                        </td>
                        <td style="text-align: right; width: 20%;">
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
                        <td>
                        </td>
                        <td style="width: 20%; text-align: right;">
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
                        <td>
                        </td>
                        <td style="text-align:right;width:20%;" >
                            Descuento honorarios 
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Descuento_Honorarios" runat="server" Width="96.4%" />
                        </td>
                    </tr>--%>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            &nbsp;
                        </td>
                        <td>
                        </td>
                        <td style="text-align: right; width: 20%; border-top: solid 1px BLACK">
                            Total
                        </td>
                        <td style="text-align: left; width: 30%; border-top: solid 1px BLACK">
                            <asp:TextBox ID="Txt_Total" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            &nbsp;
                        </td>
                        <td style="width: 20%; text-align: right;" colspan="2">
                        </td>
                        <td style="text-align: left; width: 30%;">
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="Hdn_Propietario_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Tasa_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Cuenta_Predial_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Cuota_Anual" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

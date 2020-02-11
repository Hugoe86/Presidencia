<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Resolucion.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Resolucion_Calculo_Traslado" %>

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

    //Abrir una ventana modal
    function Abrir_Ventana_Modal(Url, Propiedades) {
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
                        <td colspan="4" class="label_titulo">
                            Resolución
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td colspan="2">
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Btn_Convenio" runat="server" ToolTip="Convenio" CssClass="Img_Button"
                                                TabIndex="1" PostBackUrl="~/paginas/Predial/Frm_Ope_Pre_Convenios_Traslado.aspx"
                                                ImageUrl="~/paginas/imagenes/paginas/Listado.png" OnClick="Btn_Convenio_Click" />
                                            <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" OnClick="Btn_Imprimir_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            <table style="width: 100%; height: 28px;">
                                                <caption>
                                                    <tr>
                                                        <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                            Búsqueda:
                                                        </td>
                                                        <td style="width: 55%;">
                                                            <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="4" ToolTip="Buscar"
                                                                Width="180px" />
                                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                                                WatermarkCssClass="watermarked" WatermarkText="&lt;Buscar&gt;" />
                                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ/" />
                                                        </td>
                                                        <td style="vertical-align: middle; width: 5%;">
                                                            <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                                OnClick="Btn_Buscar_Click" TabIndex="5" ToolTip="Buscar" />
                                                        </td>
                                                    </tr>
                                                </caption>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:HiddenField ID="Hdn_No_Calculo" runat="server" />
                <asp:HiddenField ID="Hdn_No_Orden" runat="server" />
                <asp:HiddenField ID="Hdn_Anio_Orden" runat="server" />
                <asp:HiddenField ID="Hdn_Anio_Calculo" runat="server" />
                <asp:HiddenField ID="Hdn_Tasa_Traslado_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Tasa_Div_Lotif_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Cuenta_Predial_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Realizo_Calculo" runat="server" />
                <asp:Panel ID="Pnl_Grid" runat="server" Visible="true">
                    <table width="98%" class="estilo_fuente">
                        <%------------------ Cálculos pendientes ------------------%>
                        <tr id="Tr_Titulo_Pendientes" style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Cálculos pendientes
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="4">
                                <asp:GridView ID="Grid_Calculos" runat="server" AllowPaging="True" CssClass="GridView_1"
                                    AutoGenerateColumns="False" PageSize="5" Width="100%" TabIndex="6" AllowSorting="True"
                                    HeaderStyle-CssClass="tblHead" Style="white-space: normal;" OnPageIndexChanging="Grid_Calculos_PageIndexChanging"
                                    OnSelectedIndexChanged="Grid_Calculos_SelectedIndexChanged" OnRowDataBound="Grid_Calculos_RowDataBound">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="CONTRARECIBO" HeaderText="Contrarecibo" />
                                        <asp:BoundField DataField="FOLIO_ORDEN" HeaderText="Movimiento" />
                                        <asp:BoundField DataField="FOLIO_CALCULO" HeaderText="No. cálculo" Visible="false" />
                                        <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cta. Predial" />
                                        <asp:BoundField DataField="MONTO_TOTAL_CALCULO" HeaderText="Importe" DataFormatString="{0:c}"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="FECHA_CALCULO" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yy}"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="ESTATUS_CONTRARECIBO_CALCULO" HeaderText="Estatus" />
                                        <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="Anio orden" Visible="false" />
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
                <asp:Panel ID="Pnl_Controles" runat="server" Visible="false">
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td style="text-align: left; width: 25%;">
                                Estatus
                            </td>
                            <td style="text-align: left; width: 25%;">
                                <asp:TextBox ID="Txt_Estatus" runat="server" Width="96.4%" TabIndex="7" ReadOnly="true" />
                            </td>
                            <td style="width: 25%; text-align: right; padding-left: 35px;">
                                Folio pago
                            </td>
                            <td style="text-align: left; width: 25%;">
                                <asp:TextBox ID="Txt_Folio_Pago" runat="server" Width="96.4%" TabIndex="8" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 25%;">
                                Cuenta Predial
                            </td>
                            <td style="text-align: left; width: 25%;">
                                <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="96.4%" AutoPostBack="true"
                                    TabIndex="9" MaxLength="20"></asp:TextBox>
                            </td>
                            <td colspan="2" style="text-align: right; padding-right: 35px; vertical-align: bottom;">
                                <asp:CheckBox ID="Chk_Predio_Colindante" runat="server" Text="Predio colindante"
                                    TabIndex="10" OnCheckedChanged="Chk_Predio_Colindante_CheckedChanged" AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 25%;">
                                *Base del impuesto
                            </td>
                            <td style="text-align: left; width: 25%;">
                                <asp:TextBox ID="Txt_Base_Impuesto" runat="server" Width="96.4%" TabIndex="11" />
                            </td>
                            <td style="width: 25%; text-align: right; padding-left: 35px;">
                                <span style="float: left;">-</span>Mínimo elevado al año
                            </td>
                            <td style="text-align: left; width: 25%;">
                                <asp:TextBox ID="Txt_Minimo_Elevado_Anio" runat="server" Width="96.4%" TabIndex="12" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 25%;">
                                Base gravable T. de dominio
                            </td>
                            <td style="text-align: left; width: 25%;">
                                <asp:TextBox ID="Txt_Base_Gravable_Traslado" runat="server" Width="96.4%" TabIndex="13" />
                            </td>
                            <td style="width: 25%; text-align: right; padding-left: 35px;">
                                <span style="float: left;">x</span>Tasa T. de dominio
                            </td>
                            <td style="text-align: left; width: 25%;">
                                <asp:TextBox ID="Txt_Tasa_Traslado_Dominio" runat="server" Width="96.4%" TabIndex="14" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                            <td style="width: 25%; text-align: right; padding-left: 35px;">
                                <span style="float: left;">=</span>Impuesto Traslado dom.
                            </td>
                            <td style="text-align: left; width: 25%;">
                                <asp:TextBox ID="Txt_Impuesto_Traslado_Dominio" runat="server" Width="96.4%" TabIndex="15" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 25%;">
                                Fecha escritura
                            </td>
                            <td style="text-align: left; width: 25%;">
                                <asp:TextBox ID="Txt_Fecha_Escritura" runat="server" Width="96.4%" TabIndex="16" />
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 25%;">
                                Tipo División o lotificación
                            </td>
                            <td style="text-align: left; width: 25%;" colspan="3">
                                <asp:TextBox ID="Txt_Tipo_Division_Lotificacion" runat="server" Width="98%" TabIndex="17"
                                    TextMode="SingleLine" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 25%;">
                                *Base del impuesto
                            </td>
                            <td style="text-align: left; width: 25%;">
                                <asp:TextBox ID="Txt_Base_Impuesto_Div_Lotif" runat="server" Width="96.4%" TabIndex="18" />
                            </td>
                            <td style="width: 25%; text-align: right; padding-left: 35px;">
                                <span style="float: left;">x</span>Tasa Divi. o Lotif. (%)
                            </td>
                            <td style="text-align: left; width: 25%;">
                                <asp:TextBox ID="Txt_Tasa_Division_Lotificacion" runat="server" Width="96.4%" TabIndex="19" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                            <td style="width: 25%; text-align: right; padding-left: 35px;">
                                <span style="float: left;">=</span>Impuesto Div./Lotif.
                            </td>
                            <td style="text-align: left; width: 25%;">
                                <asp:TextBox ID="Txt_Impuesto_Division_Lotificacion" runat="server" Width="96.4%"
                                    TabIndex="20" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                            <td style="text-align: left; width: 25%; text-align: right; padding-left: 35px;">
                                <span style="float: left;">+</span>Constancia No adeudo
                            </td>
                            <td style="text-align: left; width: 25%;">
                                <asp:TextBox ID="Txt_Costo_Constancia_No_Adeudo" runat="server" Width="79%" TabIndex="21" />
                                <asp:CheckBox ID="Chk_Constancia_No_Adeudo" runat="server" Text="" Checked="true"
                                    TabIndex="22" OnCheckedChanged="Chk_Constancia_No_Adeudo_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                            <td style="text-align: left; width: 25%; text-align: right; padding-left: 35px;">
                                <span style="float: left;">+</span>Multa
                            </td>
                            <td style="text-align: left; width: 25%;">
                                <asp:TextBox ID="Txt_Multa" runat="server" Width="96.4%" TabIndex="23" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                            <td style="text-align: left; width: 25%; text-align: right; padding-left: 35px;">
                                <span style="float: left;">+</span>Recargos
                            </td>
                            <td style="text-align: left; width: 25%;">
                                <asp:TextBox ID="Txt_Recargos" runat="server" Width="96.4%" TabIndex="24" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                            <td style="text-align: left; width: 25%; text-align: right; padding-left: 35px;">
                                <span style="float: left;">=</span>Total
                            </td>
                            <td style="text-align: left; width: 25%;">
                                <asp:TextBox ID="Txt_Total" runat="server" Width="96.4%" TabIndex="25" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 25%;">
                                Tipo
                            </td>
                            <td style="text-align: left; width: 25%;" colspan="3">
                                <asp:RadioButton ID="Opt_Tipo_Valor_Fiscal" GroupName="Opt_Tipo" runat="server" Text="Valor fiscal"
                                    TabIndex="26" />
                                &nbsp; &nbsp;
                                <asp:RadioButton ID="Opt_Tipo_Valor_Operacion" GroupName="Opt_Tipo" runat="server"
                                    Text="Valor operación" TabIndex="27" />
                                &nbsp; &nbsp;
                                <asp:RadioButton ID="Opt_Tipo_Avaluo_Predial" GroupName="Opt_Tipo" runat="server"
                                    Text="Valor registrado" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 25%;">
                                Imprimir formato:
                            </td>
                            <td style="text-align: left; width: 25%;" colspan="3">
                                <asp:RadioButton ID="Opt_Formato_Oficial" GroupName="Opt_Formato" runat="server"
                                    Text="Formato oficial" TabIndex="28" />
                                &nbsp; &nbsp;
                                <asp:RadioButton ID="Opt_Formato_Notario" GroupName="Opt_Formato" runat="server"
                                    Text="Formato notario" TabIndex="29" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 25%;">
                                *Fundamento
                            </td>
                            <td style="text-align: left; width: 25%;" colspan="3">
                                <asp:TextBox ID="Txt_Fundamento" runat="server" Width="98%" TabIndex="30" TextMode="SingleLine"
                                    Style="text-transform: uppercase" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fundamento" runat="server" TargetControlID="Txt_Fundamento"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 25%; vertical-align: top;">
                                <asp:Label ID="Lbl_Campo_Observaciones" runat="server" Text="Observaciones"></asp:Label>
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%;">
                                <asp:TextBox ID="Txt_Comentarios_Area" runat="server" TabIndex="31" MaxLength="250"
                                    Style="text-transform: uppercase" TextMode="MultiLine" Width="98.6%" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Area" runat="server" TargetControlID="Txt_Comentarios_Area"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                            </td>
                        </tr>
                    </table>
                    <div id="Encabezado_Observaciones_Anteriores" runat="server" style="color: #25406D;
                        display: none;">
                        Observaciones anteriores:</div>
                    <div id="Contenedor_Observaciones_Anteriores" runat="server" style="color: #25406D;
                        display: none;">
                    </div>
                    <br />
                </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

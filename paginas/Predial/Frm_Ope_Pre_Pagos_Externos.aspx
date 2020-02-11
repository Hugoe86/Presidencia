<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Pagos_Externos.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Pagos_Externos"
    Culture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type="text/javascript" language="javascript">
    //Abrir una ventana modal
    function Abrir_Ventana_Modal(Url, Propiedades) {
        window.showModalDialog(Url, null, Propiedades);
    }
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_Session.ashx";
 
        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion() {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }
 
        //Temporizador para matener la sesión activa
        setInterval("MantenSesion()", "<%=(int)(0.9*(Session.Timeout * 60000))%>");
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <%------------------ Bimestral ------------------%>
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" AsyncPostBackTimeout="3600" />
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
                            Pagos externos
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
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Salir" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
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
                                                            ToolTip="Buscar" OnClick="Btn_Buscar_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                    <asp:HiddenField ID="Hfd_Tipo_Predio" runat="server" />
                    <asp:HiddenField ID="Hfd_Cuota_Fija" runat="server" />
                    <asp:HiddenField ID="Hdf_Convenio" runat="server" />
                    <asp:HiddenField ID="Hfd_No_Descuento_Recargos" runat="server" />
                    <asp:HiddenField ID="Hfd_Adeudo_Actual" runat="server" />
                    <asp:HiddenField ID="Hfd_Adeudo_Rezago" runat="server" />
                </table>
                <br />
                <table width="98%" class="estilo_fuente">
                    <div id="Div_Grid_Pagos_Externos" runat="server" visible="true">
                        <tr align="center">
                            <td colspan="4">
                                <asp:GridView ID="Grid_Pagos_Externos" runat="server" AllowPaging="True" CssClass="GridView_1"
                                    AutoGenerateColumns="False" PageSize="5" Width="100%" AllowSorting="True" HeaderStyle-CssClass="tblHead"
                                    Style="white-space: normal;" OnPageIndexChanging="Grid_Pagos_Externos_Page_Index_Changing"
                                    OnSelectedIndexChanged="Grid_Pagos_Externos_Selected_Index_Changed">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="Folio" HeaderText="No.Operación" Visible="True" />
                                        <asp:BoundField DataField="Periodo" HeaderText="Periodo" Visible="True" />
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" Visible="True" />
                                        <asp:BoundField DataField="Recibo" HeaderText="Recibo" Visible="True" />
                                        <asp:BoundField DataField="Caja" HeaderText="Caja" Visible="True" />
                                        <asp:BoundField DataField="Clave Banco" HeaderText="Institución Externa" Visible="True" />
                                        <asp:BoundField DataField="Corriente" HeaderText="Corriente" Visible="True" />
                                        <asp:BoundField DataField="Descuento" HeaderText="Descuento" Visible="True" />
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                                <br />
                            </td>
                        </tr>
                    </div>
                    <div id="Div_Datos" runat="server" visible="true">
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *No. Folio
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_No_Folio" runat="server" Width="96.4%" OnTextChanged="Txt_No_Folio_TextChanged"
                                    Style="text-transform: uppercase" AutoPostBack="True" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                *Fecha Pago
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Pago" runat="server" Width="88.4%" TabIndex="12" MaxLength="11"
                                    Height="18px" />
                                <cc1:CalendarExtender ID="CE_Fecha_Pago" runat="server" TargetControlID="Txt_Fecha_Pago"
                                    Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Fecha_Pago" />
                                <asp:ImageButton ID="Btn_Fecha_Pago" runat="server" CausesValidation="false" Height="18px"
                                    ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" />
                                <cc1:TextBoxWatermarkExtender ID="TBWE_Fecha_Pago" runat="server" TargetControlID="Txt_Fecha_Pago"
                                    WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Cuenta Predial
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="96.4%" AutoPostBack="True"
                                    TabIndex="9" MaxLength="20" Style="text-transform: uppercase" OnTextChanged="Txt_Cuenta_Predial_TextChanged"></asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:ImageButton ID="Btn_Busqueda_Avanzada_Cuentas_Predial" runat="server" ToolTip="Búsqueda Avanzada"
                                    TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Height="24px"
                                    Width="24px" Style="float: left" OnClick="Btn_Busqueda_Avanzada_Cuentas_Predial_Click" />
                                *Institución Externa
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Lugar_Pago" runat="server" TabIndex="7" Width="99%">
                                    <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Desde Periodo
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Desde_Periodo" runat="server" Width="96.4%"></asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Hasta Periodo
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Hasta_Periodo_Bimestre" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="Cmb_Bimestre_Final_SelectedIndexChanged">
                                    <asp:ListItem Value="1">01</asp:ListItem>
                                    <asp:ListItem Value="2">02</asp:ListItem>
                                    <asp:ListItem Value="3">03</asp:ListItem>
                                    <asp:ListItem Value="4">04</asp:ListItem>
                                    <asp:ListItem Value="5">05</asp:ListItem>
                                    <asp:ListItem Value="6">06</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="Cmb_Hasta_Periodo_Año" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Anio_Final_SelectedIndexChanged"
                                    Width="60px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Periodo Rezago
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Periodo_Rezago" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Adeudo Rezago
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Adeudo_Rezago" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Periodo Corriente
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Periodo_Corriente" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Adeudo Corriente
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Adeudo_Corriente" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Honorarios
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Honorarios" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Gastos de Ejecución
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Gastos_Ejecucion" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Total Recargos Ordinarios
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Total_Recargos_Ordinarios" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Recargos Moratorios
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Recargos_Moratorios" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                &nbsp;
                            </td>
                            <td style="text-align: left; width: 30%;">
                                &nbsp;
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <b>Subtotal</b>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Subtotal" runat="server" Font-Bold="True" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Descuento por Pronto Pago
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Descuento_Pronto_Pago" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Descuento Recargos Ordinarios
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Descuento_Recargos_Ordinarios" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Descuento Recargos Moratorios
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Descuento_Recargos_Moratorios" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <b>Total</b>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Total" runat="server" Font-Bold="True" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <%------------------ Cuenta ------------------%>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Cuenta
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                Propietario
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%;">
                                <asp:TextBox ID="Txt_Propietario" runat="server" TabIndex="10" MaxLength="250" TextMode="SingleLine"
                                    Width="98.6%" AutoPostBack="True" Enabled="False" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                Ubicación
                            </td>
                            <td style="text-align: left;" colspan="3">
                                <asp:TextBox ID="Txt_Ubicacion" runat="server" TabIndex="10" MaxLength="250" TextMode="SingleLine"
                                    Width="98.6%" AutoPostBack="True" Enabled="False" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                    </div>
                    <div id="Div1" runat="server" visible="true">
                        <%------------------ Bimestral ------------------%>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Adeudos Pendientes
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="4">
                                <div id="Div_Listado_Adeudos_Predial" runat="server" style="width: 99%; overflow: auto;
                                    border-width: thick; border-color: Black;">
                                    <asp:GridView ID="Grid_Listado_Adeudos" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                        CssClass="GridView_1" HeaderStyle-CssClass="tblHead" OnRowDataBound="Grid_Listado_Adeudos_RowDataBound"
                                        Style="white-space: normal;" Width="100%">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chk_Seleccion_Adeudo" runat="server" AutoPostBack="True" Checked="true"
                                                        Enabled="False" OnCheckedChanged="Chk_Seleccion_Adeudo_CheckedChanged" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="NO_ADEUDO" HeaderText="NO_ADEUDO">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CONCEPTO" HeaderText="Concepto">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ANIO" HeaderText="Año">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="BIMESTRE" HeaderText="Bim.">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DESCUENTO" DataFormatString="{0:c}" HeaderText="Desc.">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CORRIENTE" DataFormatString="{0:c}" HeaderText="Corriente">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="REZAGOS" DataFormatString="{0:c}" HeaderText="Rezagos">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RECARGOS_ORDINARIOS" DataFormatString="{0:c}" HeaderText="Recargos Ordinarios">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RECARGOS_MORATORIOS" DataFormatString="{0:c}" HeaderText="Recargos Moratorios">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="HONORARIOS" DataFormatString="{0:c}" HeaderText="Honorarios">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MULTAS" DataFormatString="{0:c}" HeaderText="Multas">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DESCUENTOS" DataFormatString="{0:c}" HeaderText="Descuentos">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MONTO" DataFormatString="{0:c}" HeaderText="Monto">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <HeaderStyle CssClass="tblHead" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </div>
                                <div id="Div_Listado_Adeudos_Convenio" runat="server" style="width: 99%; overflow: auto;
                                    border-width: thick; border-color: Black;">
                                    <asp:GridView ID="Grid_Listado_Adeudos_Convenio" runat="server" AllowSorting="True"
                                        AutoGenerateColumns="False" CssClass="GridView_1" HeaderStyle-CssClass="tblHead"
                                        OnRowDataBound="Grid_Listado_Adeudos_Convenio_RowDataBound" Style="white-space: normal;"
                                        Width="100%">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chk_Seleccion_Parcialidad" runat="server" AutoPostBack="True" Checked="false"
                                                        OnCheckedChanged="Chk_Seleccion_Parcialidad_CheckedChanged" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="NO_PAGO" HeaderText="NO_PAGO">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ESTADO_PAGO" HeaderText="ESTADO_PAGO">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PARCIALIDAD" HeaderText="Parcialidad">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ANIO" HeaderText="Año">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PERIODO" HeaderText="Bimestres">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CORRIENTE" DataFormatString="{0:c}" HeaderText="Corriente">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="REZAGOS" DataFormatString="{0:c}" HeaderText="Rezagos">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RECARGOS_ORDINARIOS" DataFormatString="{0:c}" HeaderText="Recargos Ordinarios">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RECARGOS_MORATORIOS" DataFormatString="{0:c}" HeaderText="Recargos Moratorios">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="HONORARIOS" DataFormatString="{0:c}" HeaderText="Honorarios">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MONTO" DataFormatString="{0:c}" HeaderText="Monto">
                                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                            </asp:BoundField>
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <HeaderStyle CssClass="tblHead" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </div>
                                <br />
                            </td>
                        </tr>
                    </div>
                    <tr align="center">
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Descuentos_Traslado.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Descuentos_Traslado"
    UICulture="es" Culture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script language="javascript" type="text/javascript">
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
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);
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
                        <td colspan="4" class="label_titulo">
                            Descuentos Traslado de Dominio
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                            <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:ImageButton ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
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
                            <div align="right" style="width: 99%; background-color: #2F4E7D; color: #FFFFFF;
                                font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy;
                                height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 30%;">
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
                                                    <td>
                                                        <asp:DropDownList ID="Cmb_Busqueda_General" runat="server">
                                                            <asp:ListItem>CONTRARECIBO</asp:ListItem>
                                                            <asp:ListItem>CUENTA</asp:ListItem>
                                                            <asp:ListItem>FOLIO</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar"
                                                            Width="180px" />
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. " />
                                                    </td>
                                                    <td style="vertical-align: middle;">
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
            </div>
            </td> </tr> </table>
            <br />
            <table width="98%" class="estilo_fuente">
                <%------------------ Contrarecibos ------------------%>
                <div id="titulo" runat="server">
                    <tr style="background-color: #3366CC">
                        <td style="text-align: left; font-size: 15px; color: #FFFFFF;">
                            Contrarecibos
                        </td>
                    </tr>
                </div>
                <tr align="center">
                    <td>
                        <asp:GridView ID="Grid_Descuentos_Traslado" runat="server" AllowPaging="True" CssClass="GridView_1"
                            AutoGenerateColumns="False" PageSize="5" Width="100%" EmptyDataText="&quot;No se encontraron registros&quot;"
                            AllowSorting="True" HeaderStyle-CssClass="tblHead" Style="white-space: normal;"
                            OnPageIndexChanging="Grid_Descuentos_Traslado_PageIndexChanging" OnSelectedIndexChanged="Grid_Descuentos_Traslado_SelectedIndexChanged">
                            <Columns>
                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="NO_CONTRARECIBO" HeaderText="Contrarecibo">
                                    <HeaderStyle HorizontalAlign="center" Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial">
                                    <HeaderStyle HorizontalAlign="center" Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FECHA_INICIAL" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}">
                                    <HeaderStyle HorizontalAlign="center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MONTO_RECARGOS" HeaderText="Recargos" DataFormatString="{0:$#,##0.00}">
                                    <HeaderStyle HorizontalAlign="center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DESC_RECARGO" HeaderText="Descuento Recargos" DataFormatString="{0:0.##}">
                                    <HeaderStyle HorizontalAlign="center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MONTO_MULTA" HeaderText="Multas" DataFormatString="{0:$#,##0.00}">
                                    <HeaderStyle HorizontalAlign="center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DESC_MULTA" HeaderText="Descuento Multas" DataFormatString="{0:0.##}">
                                    <HeaderStyle HorizontalAlign="center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TOTAL" HeaderText="Importe" DataFormatString="{0:$#,##0.00}">
                                    <HeaderStyle HorizontalAlign="center" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                    <HeaderStyle HorizontalAlign="center" Width="10%" />
                                    <ItemStyle HorizontalAlign="center" Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FOLIO" HeaderText="Folio">
                                    <HeaderStyle HorizontalAlign="center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NO_DESCUENTO" HeaderText="No. Descuento">
                                    <HeaderStyle HorizontalAlign="center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NO_CALCULO" HeaderText="No. Calculo">
                                    <HeaderStyle HorizontalAlign="center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ANIO_CALCULO" HeaderText="Año Calculo">
                                    <HeaderStyle HorizontalAlign="center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NO_ORDEN_VARIACION" HeaderText="No. Orden Variacion">
                                    <HeaderStyle HorizontalAlign="center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ANIO_ORDEN" HeaderText="Año Orden">
                                    <HeaderStyle HorizontalAlign="center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NO_ADEUDO" HeaderText="No. Adeudo">
                                    <HeaderStyle HorizontalAlign="center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="REALIZO" HeaderText="Realizo">
                                    <HeaderStyle HorizontalAlign="center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FECHA_VENCIMIENTO" HeaderText="Fecha Vencimiento" DataFormatString="{0:dd/MMM/yyyy}">
                                    <HeaderStyle HorizontalAlign="center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OBSERVACIONES" HeaderText="Observaciones">
                                    <HeaderStyle HorizontalAlign="center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FUNDAMENTO_LEGAL" HeaderText="Fundamento Legal">
                                    <HeaderStyle HorizontalAlign="center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MONTO_TRASLADO" HeaderText="Impuesto Traslado" DataFormatString="{0:$#,##0.00}">
                                    <HeaderStyle HorizontalAlign="center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MONTO_DIVISION" HeaderText="Impuesto Division" DataFormatString="{0:$#,##0.00}">
                                    <HeaderStyle HorizontalAlign="center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="COSTO_CONSTANCIA" HeaderText="Costo Constancia" DataFormatString="{0:$#,##0.00}">
                                    <HeaderStyle HorizontalAlign="center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="ID Cuenta Predial" />
                            </Columns>
                            <SelectedRowStyle CssClass="GridSelected" />
                            <PagerStyle CssClass="GridHeader" />
                            <HeaderStyle CssClass="tblHead" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                        </asp:GridView>
                        <br />
                    </td>
                </tr>
                <tr>
                    <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                    <asp:HiddenField ID="Hdf_Propietario_ID" runat="server" />
                    <asp:HiddenField ID="Hdf_RFC" runat="server" />
                </tr>
                <div id="Div_Descuentos_Traslado" runat="server">
                    <table>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:TextBox ID="Txt_Folio" runat="server" Visible="false">
                                </asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 20%;">
                                <asp:TextBox ID="Txt_No_Descuento" runat="server" Visible="false">
                                </asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 20%;">
                                <asp:TextBox ID="Txt_No_Calculo" runat="server" Visible="false">
                                </asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 20%;">
                                <asp:TextBox ID="Txt_Anio_Calculo" runat="server" Visible="false">
                                </asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 20%;">
                                <asp:TextBox ID="Txt_No_Adeudo" runat="server" Visible="false">
                                </asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 20%;">
                                <asp:TextBox ID="Txt_Total_Suma_Rec_Mult" runat="server" Visible="false">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Cuenta Predial
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="96.4%" AutoPostBack="true"
                                    TabIndex="9" MaxLength="20"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cuenta_Predial" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers"
                                    TargetControlID="Txt_Cuenta_Predial" />
                            </td>
                            <td style="text-align: left; width: 30%; text-align: right;">
                                Contrarecibo
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Contrarecibo" runat="server" Width="96.4%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Fecha
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" ReadOnly="True" Width="84%" />
                                <cc1:TextBoxWatermarkExtender ID="TBE_Fecha_Inicial" runat="server" TargetControlID="Txt_Fecha_Inicial"
                                    WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                <cc1:CalendarExtender ID="CE_Fecha_Inicial" runat="server" TargetControlID="Txt_Fecha_Inicial"
                                    Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Txt_Fecha_Inicial" />
                                <asp:ImageButton ID="Btn_Txt_Fecha_Inicial" runat="server" ImageUrl="../imagenes/paginas/SmallCalendar.gif"
                                    Style="vertical-align: top;" Height="18px" CausesValidation="false" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Estatus
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%" TabIndex="7">
                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                    <asp:ListItem Text="VIGENTE" Value="Vigente" />
                                    <asp:ListItem Text="BAJA" Value="Baja" />
                                    <asp:ListItem Text="CANCELADO" Value="Cancelado" />
                                    <asp:ListItem Text="VENCIDO" Value="Vencido" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="background-color: #3366CC">
                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                Cálculos
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="TextBox1" runat="server" ReadOnly="True" Width="120%" Visible="false" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Fecha vencimiento
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Vencimiento" runat="server" Width="84%" TabIndex="12"
                                    MaxLength="11" Height="18px" />
                                <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Inicial" runat="server" TargetControlID="Txt_Fecha_Vencimiento"
                                    WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Inicial" runat="server" TargetControlID="Txt_Fecha_Vencimiento"
                                    Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Txt_Fecha_Vencimiento" />
                                <asp:ImageButton ID="Btn_Txt_Fecha_Vencimiento" runat="server" ImageUrl="../imagenes/paginas/SmallCalendar.gif"
                                    Style="vertical-align: top;" Height="18px" CausesValidation="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="TextBox2" runat="server" ReadOnly="True" Width="120%" Visible="false" />
                            </td>
                            <td style="text-align: right; width: 20%; text-align: right;">
                                Realizó
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Realizo" runat="server" ReadOnly="True" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="TextBox3" runat="server" ReadOnly="True" Width="120%" Visible="false" />
                            </td>
                            <td style="text-align: right; width: 20%; vertical-align: top;">
                                Observaciones
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%;">
                                <asp:TextBox ID="Txt_Observaciones" runat="server" TabIndex="10" MaxLength="250"
                                    Style="text-transform: uppercase" TextMode="MultiLine" Width="71%" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Observaciones" runat="server" TargetControlID="Txt_Observaciones"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Constancia No Adeudo
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Costo_Constancia" runat="server" ReadOnly="True" Width="96.4%" />
                            </td>
                            <td style="width: 25%; text-align: right; padding-left: 35px;">
                            </td>
                            <td style="text-align: left; width: 25%;">
                                <asp:TextBox ID="Txt_Folio_Pago" runat="server" Width="96.4%" TabIndex="8" Visible="false"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Imp. Traslado de Dominio
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Impuesto_Traslado" runat="server" ReadOnly="True" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Imp. División y Lotificación
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Impuesto_Division" runat="server" ReadOnly="True" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; text-align: right;" visible="false">
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Total_Impuesto" runat="server" ReadOnly="True" Width="96.4%"
                                    Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Recargos
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Recargos" runat="server" ReadOnly="True" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; text-align: left;">
                                % Descuento Recargos
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Desc_Recargo" runat="server" Width="96.4%" OnTextChanged="Txt_Desc_Recargo_TextChanged"
                                    AutoPostBack="true" MaxLength="6" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Desc_Recargo"
                                    FilterType="Custom , Numbers" ValidChars="0123456789." />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Descuento Recargos
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Monto_Recargos" runat="server" Width="96.4%" OnTextChanged="Txt_Monto_Recargos_TextChanged"
                                    AutoPostBack="true" MaxLength="20" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="Txt_Monto_Recargos"
                                    FilterType="Custom , Numbers" ValidChars="0123456789.$," />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Multas
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Multas" runat="server" ReadOnly="True" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                % Descuento Multas
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Desc_Multas" runat="server" Width="96.4%" OnTextChanged="Txt_Desc_Multas_TextChanged"
                                    AutoPostBack="true" MaxLength="6" />
                                <cc1:FilteredTextBoxExtender ID="FTBE_Multas" runat="server" TargetControlID="Txt_Desc_Multas"
                                    FilterType="Custom , Numbers" ValidChars="0123456789." />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Descuento Multas
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Monto_Multas" runat="server" Width="96.4%" OnTextChanged="Txt_Monto_Multas_TextChanged"
                                    AutoPostBack="true" MaxLength="20" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="Txt_Monto_Multas"
                                    FilterType="Custom , Numbers" ValidChars="0123456789.$," />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; border-top: solid 2px BLACK;">
                                Total a pagar
                            </td>
                            <td style="text-align: left; width: 30%; border-top: solid 2px BLACK;">
                                <asp:TextBox ID="Txt_Total_Por_Pagar" runat="server" ReadOnly="True" Width="96.4%"
                                    BorderWidth="1" Font-Bold="true" Font-Size="Large" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <asp:HiddenField ID="Hdn_Estatus" runat="server" />
                    </table>
                </div>
            </table>
            <div id="Div_OV" runat="server">
                <table>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Propietario
                        </td>
                        <td colspan="3" style="text-align: left; width: 20%;">
                            <asp:TextBox ID="Txt_Propietario" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Colonia
                        </td>
                        <td style="text-align: left; width: 20%;">
                            <asp:TextBox ID="Txt_Colonia" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                        <td style="text-align: right; width: 20%;">
                            Calle
                        </td>
                        <td style="text-align: left; width: 20%;">
                            <asp:TextBox ID="Txt_Calle" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            No. exterior
                        </td>
                        <td style="text-align: left; width: 20%;">
                            <asp:TextBox ID="Txt_No_Exterior" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                        <td style="text-align: right; width: 20%;">
                            No. interior
                        </td>
                        <td style="text-align: left; width: 20%;">
                            <asp:TextBox ID="Txt_No_Interior" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Descuentos_Impuestos_Fracc.aspx.cs"
    Inherits="paginas_Predial_Frm_Ope_Pre_Descuentos_Impuestos_Fracc" Culture="es-MX"
    Title="Descuentos de Impuestos de Fraccionamiento" UICulture="es-MX" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" %>

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

    function Abrir_Modal_Popup() {
            $find('Busqueda_Empleados').show();
            return false;
        }

        function Limpiar_Ctlr(){
        document.getElementById("<%=Txt_Busqueda_No_Impuesto.ClientID%>").value="";
        document.getElementById("<%=Txt_Busqueda_Cuenta_Predial.ClientID%>").value="";
            return false;
        }


    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server" AsyncPostBackTimeout="3600"
        EnableScriptGlobalization="true" EnableScriptLocalization="true" />
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
                            Descuentos de Fraccionamientos
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
                                                            <asp:ListItem>CUENTA</asp:ListItem>
                                                            <asp:ListItem>NO. DESCUENTO</asp:ListItem>
                                                            <asp:ListItem>NO. IMPUESTO</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar"
                                                            Width="180px" Style="text-transform: uppercase" />
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
                <div id="Div_Grid_Descuentos" runat="server">
                    <tr style="background-color: #3366CC">
                        <td style="text-align: left; font-size: 15px; color: #FFFFFF;">
                            Descuentos
                        </td>
                    </tr>
                    <%--</div>--%>
                    <tr align="center">
                        <td>
                            <asp:GridView ID="Grid_Descuentos_Der_Sup" runat="server" AllowPaging="True" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="5" Width="100%" EmptyDataText="&quot;No se encontraron registros&quot;"
                                AllowSorting="True" HeaderStyle-CssClass="tblHead" Style="white-space: normal;"
                                OnPageIndexChanging="Grid_Descuentos_Der_Sup_Page_Index_Changing" OnSelectedIndexChanged="Grid_Descuentos_Der_Sup_Selected_Index_Changed">
                                <SelectedRowStyle CssClass="GridSelected" Font-Size="XX-Small" />
                                <PagerStyle CssClass="GridHeader" />
                                <HeaderStyle CssClass="tblHead" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="NO_DESCUENTO" HeaderText="Contrarecibo" Visible="false">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="Cuenta Predial ID" Visible="false">
                                        <HeaderStyle HorizontalAlign="center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial">
                                        <HeaderStyle HorizontalAlign="center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}">
                                        <HeaderStyle HorizontalAlign="center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MONTO_RECARGOS" HeaderText="Recargos" DataFormatString="{0:$#,###,###,##0.00}">
                                        <HeaderStyle HorizontalAlign="center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Right" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESC_RECARGO" HeaderText="Descuento Recargos">
                                        <HeaderStyle HorizontalAlign="center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Right" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MONTO_MULTAS" HeaderText="Multas" DataFormatString="{0:$#,###,###,##0.00}">
                                        <HeaderStyle HorizontalAlign="center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Right" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESC_MULTA" HeaderText="Descuento Multas">
                                        <HeaderStyle HorizontalAlign="center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Right" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TOTAL_POR_PAGAR" HeaderText="Importe" DataFormatString="{0:$#,###,###,##0.00}">
                                        <HeaderStyle HorizontalAlign="center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Right" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                        <HeaderStyle HorizontalAlign="center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="REALIZO" HeaderText="Realizo">
                                        <HeaderStyle HorizontalAlign="center" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA_VENCIMIENTO" HeaderText="Fecha Vencimiento" DataFormatString="{0:dd/MM/yyyy}">
                                        <HeaderStyle HorizontalAlign="center" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="REFERENCIA" HeaderText="Folio">
                                        <HeaderStyle HorizontalAlign="center" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                        <asp:HiddenField ID="Hdf_No_Convenio" runat="server" />
                    </tr>
                </div>
                <div id="Div_Datos" runat="server">
                    <table width="98%">
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Cuenta Predial
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="96.4%" TabIndex="9" MaxLength="20"
                                    Enabled="false"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cuenta_Predial" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers"
                                    TargetControlID="Txt_Cuenta_Predial" />
                                <asp:ImageButton ID="Btn_Busqueda_Avanzada_Impuestos" runat="server" Height="24px"
                                    OnClientClick="javascript:return Abrir_Modal_Popup();" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                    TabIndex="10" ToolTip="Búsqueda Avanzada de Impuestos" Width="24px" />
                            </td>
                            <td style="text-align: left; width: 30%; text-align: right;">
                            </td>
                            <td style="text-align: left; width: 30%;">
                                &nbsp &nbsp &nbsp
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Propietario
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%;">
                                <asp:TextBox ID="Txt_Propietario" runat="server" ReadOnly="True" Width="99.6%" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Colonia
                            </td>
                            <td style="text-align: left; width: 20%;">
                                <asp:TextBox ID="Txt_Colonia" runat="server" ReadOnly="True" Width="96.4%" Enabled="false" />
                            </td>
                            <td style="text-align: right; width: 20%;">
                                Calle
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Calle" runat="server" ReadOnly="True" Width="200px" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                No. exterior
                            </td>
                            <td style="text-align: left; width: 20%;">
                                <asp:TextBox ID="Txt_No_Exterior" runat="server" ReadOnly="True" Width="96.4%" Enabled="false" />
                            </td>
                            <td style="text-align: right; width: 20%;">
                                No. interior
                            </td>
                            <td style="text-align: left; width: 20%;">
                                <asp:TextBox ID="Txt_No_Interior" runat="server" ReadOnly="True" Width="96.4%" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Fecha
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" ReadOnly="True" Width="84%" Enabled="false" />
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
                                    <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                    <asp:ListItem Text="BAJA" Value="BAJA" />
                                    <asp:ListItem Text="CANCELADO" Value="CANCELADO" />
                                    <asp:ListItem Text="VENCIDO" Value="VENCIDO" />
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
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Fecha vencimiento
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Vencimiento" runat="server" Width="84%" TabIndex="12"
                                    MaxLength="11" Height="18px" Enabled="false" />
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
                            </td>
                            <td style="text-align: right; width: 20%; text-align: right;">
                                Realizó
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Realizo" runat="server" ReadOnly="True" Width="96.4%" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 30%;">
                            </td>
                            <td style="text-align: right; width: 20%; vertical-align: top;">
                                Observaciones
                            </td>
                            <td colspan="2" style="text-align: left; width: 80%;">
                                <asp:TextBox ID="Txt_Observaciones" runat="server" TabIndex="10" MaxLength="250"
                                    Style="text-transform: uppercase" TextMode="MultiLine" Width="71%" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Observaciones" runat="server" TargetControlID="Txt_Observaciones"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Imp. Fraccionamiento
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Monto_Impuesto_Der_Sup" runat="server" ReadOnly="True" Width="96.4%"
                                    Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Recargos
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Monto_Recargos" runat="server" ReadOnly="True" Width="96.4%"
                                    Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; text-align: left;">
                                % Descuento Recargos
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Prcentaje_Desc_Recargos" runat="server" Width="96.4%" AutoPostBack="true"
                                    OnTextChanged="Txt_Porcentaje_Desc_Recargos_Text_Changed" MaxLength="6" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="Txt_Prcentaje_Desc_Recargos"
                                    FilterType="Custom , Numbers" ValidChars="0123456789." />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Monto Descuento Recargos
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Monto_Desc_Recargo" runat="server" Width="96.4%" AutoPostBack="true"
                                    OnTextChanged="Txt_Monto_Desc_Recargo_Text_Changed" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Monto_Desc_Recargo"
                                    FilterType="Custom , Numbers" ValidChars="0123456789.," />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Multas
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Monto_Multas" runat="server" ReadOnly="True" Width="96.4%" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                % Descuento Multas
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Porcentaje_Desc_Multas" runat="server" Width="96.4%" AutoPostBack="true"
                                    MaxLength="6" OnTextChanged="Txt_Porcentaje_Desc_Multas_Text_Changed" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="Txt_Porcentaje_Desc_Multas"
                                    FilterType="Custom , Numbers" ValidChars="0123456789." />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Monto Descuento Multas
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Monto_Desc_Multas" runat="server" Width="96.4%" AutoPostBack="true"
                                    OnTextChanged="Txt_Monto_Desc_Multas_Text_Changed" />
                                <cc1:FilteredTextBoxExtender ID="FTBE_Multas" runat="server" TargetControlID="Txt_Monto_Desc_Multas"
                                    FilterType="Custom , Numbers" ValidChars="0123456789.," />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; border-top: solid 2px BLACK;">
                                Total a pagar
                            </td>
                            <td style="text-align: left; width: 30%; border-top: solid 2px BLACK;">
                                <asp:TextBox ID="Txt_Total_Por_Pagar" runat="server" ReadOnly="True" Width="96.4%"
                                    BorderWidth="1" Font-Bold="true" Font-Size="Large" Enabled="false" />
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
                        <asp:HiddenField ID="Hdf_No_Descuento" runat="server" />
                        <asp:HiddenField ID="Hdf_No_Impuesto_Der_Sup" runat="server" />
                    </table>
                </div>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--Ventana Modal --%>
    <cc1:ModalPopupExtender ID="Mpe_Busqueda_Empleados" runat="server" BackgroundCssClass="popUpStyle"
        BehaviorID="Busqueda_Empleados" PopupControlID="Pnl_Busqueda_Contenedor" TargetControlID="Btn_Comodin_Open"
        PopupDragHandleControlID="Pnl_Busqueda_Cabecera" CancelControlID="Btn_Comodin_Close"
        DropShadow="True" DynamicServicePath="" Enabled="True" />
    <asp:Button Style="background-color: transparent; border-style: none;" ID="Btn_Comodin_Close"
        runat="server" Text="" />
    <asp:Button Style="background-color: transparent; border-style: none;" ID="Btn_Comodin_Open"
        runat="server" Text="" />
    <%--Panel de la ventana Modal--%>
    <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag" HorizontalAlign="Center"
        Width="650px" Style="display: none; border-style: outset; border-color: Silver;
        background-image: url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG); background-repeat: repeat-y;">
        <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" Style="cursor: move; background-color: Silver;
            color: Black; font-size: 12; font-weight: bold; border-style: outset;">
            <table width="99%">
                <tr>
                    <td style="color: Black; font-size: 12; font-weight: bold;">
                        <asp:Image ID="Img_Informatcion_Autorizacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                        B&uacute;squeda: Impuestos
                    </td>
                    <td align="right" style="width: 10%;">
                        <asp:ImageButton ID="Btn_Cerrar_Ventana" CausesValidation="false" runat="server"
                            Style="cursor: pointer;" ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"
                            OnClick="Btn_Cerrar_Ventana_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Prestamos" runat="server">
            <ContentTemplate>
                <div style="color: #5D7B9D">
                    <table width="100%">
                        <tr>
                            <td align="left" style="text-align: left;">
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server"
                                    AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Prestamos" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter1" class="progressBackgroundFilter">
                                        </div>
                                        <div style="background-color: Transparent; position: fixed; top: 50%; left: 47%;
                                            padding: 10px; z-index: 1002;" id="div_progress1">
                                            <img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100%" colspan="4" align="right">
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
                                            No Impuesto
                                        </td>
                                        <td style="width: 30%; text-align: right; font-size: 11px;">
                                            <asp:TextBox ID="Txt_Busqueda_No_Impuesto" runat="server" Width="98%" MaxLength="10" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Impuesto" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Busqueda_No_Impuesto" />
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_No_Impuesto" runat="server" TargetControlID="Txt_Busqueda_No_Impuesto"
                                                WatermarkText="Busqueda por No de Impuesto" WatermarkCssClass="watermarked" />
                                        </td>
                                        <td style="width: 20%; text-align: right; font-size: 11px;">
                                            Cuenta predial
                                        </td>
                                        <td style="width: 30%; text-align: left; font-size: 11px;">
                                            <asp:TextBox ID="Txt_Busqueda_Cuenta_Predial" runat="server" Width="98%" MaxLength="20" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Cuenta_Predial" runat="server"
                                                FilterType="Numbers, UppercaseLetters" TargetControlID="Txt_Busqueda_Cuenta_Predial" />
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_Cuenta_Predial" runat="server"
                                                TargetControlID="Txt_Busqueda_Cuenta_Predial" WatermarkText="Busqueda por Cuenta predial"
                                                WatermarkCssClass="watermarked" />
                                        </td>
                                    </tr>
                                    <asp:GridView ID="Grid_Impuestos_Derechos_Supervision" runat="server" AllowPaging="True"
                                        AllowSorting="True" AutoGenerateColumns="False" CssClass="GridView_1" HeaderStyle-CssClass="tblHead"
                                        PageSize="5" Style="white-space: normal;" Width="100%" OnPageIndexChanging="Grid_Impuestos_Derechos_Supervision_PageIndexChanging1"
                                        OnSelectedIndexChanged="Grid_Impuestos_Derechos_Supervision_SelectedIndexChanged"
                                        DataKeyNames="CUENTA_PREDIAL_ID" EmptyDataText="No se encontraron impuestos para esta cuenta predial o búsqueda.">
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png"
                                                Text="Button" CommandName="Select">
                                                <HeaderStyle Width="5%" />
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="NO_IMPUESTO_FRACCIONAMIENTO" HeaderText="No. Impuesto">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="CUENTA_PREDIAL_ID" Visible="False">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_VENCIMIENTO" DataFormatString="{0:dd-MMM-yyyy}"
                                                HeaderText="Fecha Vencimiento">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ESTATUS" HeaderStyle-Width="12%" HeaderText="Estatus">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <HeaderStyle CssClass="tblHead" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                    <tr>
                                        <td style="width: 100%; text-align: left;" colspan="4">
                                            <center>
                                                <asp:Button ID="Btn_Busqueda_Empleados" runat="server" Text="Busqueda de Impuestos"
                                                    CssClass="button" CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Impuestos_Click" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>

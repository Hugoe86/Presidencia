<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Pagos_Instit_Externas.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Pagos_Instit_Externas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType TypeName="Paginas_Generales_paginas_MasterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
    <!--SCRIPT PARA LA VALIDACION QUE NO EXPERE LA SESSION-->

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
    <cc1:ToolkitScriptManager ID="Tsm_Script_Manager" runat="server" AsyncPostBackTimeout="3600" />
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
                            Pagos en instituciones externas
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
                                        <td align="left" style="width: 30%;">
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" OnClick="Btn_Imprimir_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td align="right">
                                            <div id="Div_Contenedor_Busqueda" runat="server">
                                                <table style="width: 71%; height: 28px;">
                                                    <tr>
                                                        <td>
                                                            B&uacute;squeda: &nbsp;
                                                            <asp:DropDownList ID="Cmb_Filtrar_Institucion" runat="server" TabIndex="4">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="Cmb_Filtrar_Anio" runat="server" TabIndex="5">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="vertical-align: middle; width: 5%;">
                                                            <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                                TabIndex="6" ToolTip="Buscar" OnClick="Btn_Buscar_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="Pnl_Contenedor_Controles" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Archivo
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Nombre_Archivo" runat="server" Width="86.5%" Enabled="false"
                                    Visible="true" />
                                <asp:FileUpload ID="Fup_Archivo_Adeudos" runat="server" Width="86.5%" Visible="false"
                                    TabIndex="7" />
                                <asp:ImageButton ID="Btn_Subir_Archivo" runat="server" ToolTip="Enviar archivo" Style="border: 0 none;
                                    width: 20px; height: 20px; padding: 0;" TabIndex="8" ImageUrl="~/paginas/imagenes/paginas/sias_upload.png"
                                    OnClick="Btn_Subir_Archivo_Click" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Fecha
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha" runat="server" Width="96.4%" TabIndex="9" MaxLength="11"
                                    Height="18px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Institución
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Banco" runat="server" Width="99%" TabIndex="10" AutoPostBack="true"
                                    OnSelectedIndexChanged="Cmb_Banco_SelectedIndexChanged">
                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Caja
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:HiddenField ID="Hdn_Caja_Id" runat="server" Value="" />
                                <asp:TextBox ID="Txt_Caja" runat="server" Width="96.4%" TabIndex="11" MaxLength="11"
                                    Height="18px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Número de captura
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Numero_Captura" runat="server" Width="96.4%" TabIndex="12" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Cantidad de movimientos
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Cantidad_Movimientos" runat="server" Width="96.4%" TabIndex="13"
                                    MaxLength="13" Height="18px" />
                            </td>
                        </tr>
                        <%------------------ Incluidos ------------------%>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                <div id="Div_Encabezado_Pagos_Incluidos" runat="server">
                                    Pagos incluidos</div>
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="4">
                                <asp:GridView ID="Grid_Pagos_Incluidos" runat="server" AllowPaging="True" CssClass="GridView_1"
                                    AutoGenerateColumns="False" PageSize="5" Width="100%" AllowSorting="True" HeaderStyle-CssClass="tblHead"
                                    Style="white-space: normal;" OnPageIndexChanging="Grid_Pagos_Incluidos_PageIndexChanging"
                                    OnRowDataBound="Grid_Pagos_Incluidos_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="LINEA_CAPTURA" HeaderText="Línea captura" />
                                        <asp:BoundField DataField="FECHA_PAGO" HeaderText="Fecha" DataFormatString="{0:dd-MMM-yy}" />
                                        <asp:BoundField DataField="ADEUDO_REZAGO" HeaderText="Rezago" DataFormatString="{0:c2}"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="ADEUDO_CORRIENTE" HeaderText="Corriente" DataFormatString="{0:c2}"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="ADEUDO_RECARGOS" HeaderText="Recargos" DataFormatString="{0:c2}"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="ADEUDO_HONORARIOS" HeaderText="Honorarios" DataFormatString="{0:c2}"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="DESCUENTO" HeaderText="Descuento" DataFormatString="{0:c2}"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="MONTO_TOTAL_ADEUDO" HeaderText="Adeudo total" DataFormatString="{0:c2}"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="MONTO_PAGADO" HeaderText="Pagado" DataFormatString="{0:c2}"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="DIFERENCIA" HeaderText="Diferencia" DataFormatString="{0:c2}"
                                            ItemStyle-HorizontalAlign="Right" Visible="true" />
                                        <asp:BoundField DataField="SUCURSAL" HeaderText="Sucursal" />
                                        <asp:BoundField DataField="CAJERO" HeaderText="Cajero" Visible="false" />
                                        <asp:BoundField DataField="GUIA_CIE" HeaderText="Guía CIE" Visible="false" />
                                        <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta predial" />
                                        <asp:BoundField DataField="CONSECUTIVO" HeaderText="Consecutivo" Visible="false" />
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Excluir_Pago" runat="server" ImageUrl="~/paginas/imagenes/paginas/delete.png"
                                                    CssClass="Img_Button" ToolTip="Excluir pago" OnClick="Btn_Excluir_Pago_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
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
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <%------------------ Excluidos ------------------%>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                <div id="Div_Encabezado_Pagos_Excluidos" runat="server">
                                    Pagos excluidos</div>
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="4">
                                <asp:GridView ID="Grid_Pagos_Excluidos" runat="server" AllowPaging="True" CssClass="GridView_1"
                                    AutoGenerateColumns="False" PageSize="5" Width="100%" AllowSorting="True" HeaderStyle-CssClass="tblHead"
                                    Style="white-space: normal;" OnPageIndexChanging="Grid_Pagos_Excluidos_PageIndexChanging"
                                    OnRowDataBound="Grid_Pagos_Excluidos_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Editar_Pago_Excluido" runat="server" ImageUrl="~/paginas/imagenes/gridview/grid_update.png"
                                                    CssClass="Img_Button" ToolTip="Editar pago" OnClick="Btn_Editar_Pago_Excluido_Click" />
                                                <asp:ImageButton ID="Btn_Aplicar_Cambios_Pago_Excluido" runat="server" ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png"
                                                    Style="width: 18px; height: 18px;" CssClass="Img_Button" ToolTip="Aceptar cambios"
                                                    Visible="false" OnClick="Btn_Aplicar_Cambios_Pago_Excluido_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="LINEA_CAPTURA" HeaderText="Línea captura" />
                                        <asp:BoundField DataField="FECHA_PAGO" HeaderText="Fecha" DataFormatString="{0:dd-MMM-yy}" />
                                        <asp:BoundField DataField="ADEUDO_REZAGO" HeaderText="Rezago" DataFormatString="{0:c2}"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="ADEUDO_CORRIENTE" HeaderText="Corriente" DataFormatString="{0:c2}"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="ADEUDO_RECARGOS" HeaderText="Recargos" DataFormatString="{0:c2}"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="ADEUDO_HONORARIOS" HeaderText="Honorarios" DataFormatString="{0:c2}"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="DESCUENTO" HeaderText="Descuento" DataFormatString="{0:c2}"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="MONTO_TOTAL_ADEUDO" HeaderText="Adeudo" DataFormatString="{0:c2}"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:TemplateField HeaderText="Pagado" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txt_Grid_Monto_Pagado" runat="server" Style="width: 96%; text-align: right;"
                                                    Visible="false" Text='<%# Bind("MONTO_PAGADO") %>'>
                                                </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Monto_Pagado" runat="server" FilterType=" Numbers,Custom"
                                                    TargetControlID="Txt_Grid_Monto_Pagado" ValidChars=".,$" />
                                                <asp:Label ID="Lbl_Grid_Monto_Pagado" runat="server" Style="width: 96%; text-align: right;"
                                                    Visible="true" Text='<%# Bind("MONTO_PAGADO", "{0:c2}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DIFERENCIA" HeaderText="Diferencia" DataFormatString="{0:c2}"
                                            ItemStyle-HorizontalAlign="Right" Visible="true" />
                                        <asp:BoundField DataField="SUCURSAL" HeaderText="Sucursal" />
                                        <asp:BoundField DataField="CAJERO" HeaderText="Cajero" Visible="false" />
                                        <asp:BoundField DataField="GUIA_CIE" HeaderText="Guía CIE" Visible="false" />
                                        <asp:TemplateField HeaderText="Cuenta predial" ItemStyle-HorizontalAlign="center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txt_Grid_Cuenta_Predial" runat="server" Style="width: 96%; text-align: right;
                                                    text-transform: uppercase;" Visible="false" Text='<%# Bind("CUENTA_PREDIAL") %>'>
                                                </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Cuenta_Predial" runat="server" FilterType="Numbers,LowercaseLetters,UppercaseLetters"
                                                    TargetControlID="Txt_Grid_Cuenta_Predial" />
                                                <asp:Label ID="Lbl_Grid_Cuenta_Predial" runat="server" Style="width: 96%; text-align: right;"
                                                    Visible="true" Text='<%# Bind("CUENTA_PREDIAL") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CONSECUTIVO" HeaderText="Consecutivo" Visible="false" />
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Incluir_Pago" runat="server" ImageUrl="~/paginas/imagenes/paginas/accept.png"
                                                    CssClass="Img_Button" ToolTip="Incluir pago" OnClick="Btn_Incluir_Pago_Click" />
                                                <asp:ImageButton ID="Btn_Deshacer_Cambios_Pago_Excluido" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_circle_red.png"
                                                    Style="width: 16px; height: 16px;" CssClass="Img_Button" ToolTip="Descartar cambios"
                                                    Visible="false" OnClick="Btn_Deshacer_Cambios_Pago_Excluido_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
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
                <asp:Panel ID="Pnl_Pagos_Registrados" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <%------------------ Registros ------------------%>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Registros
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="4">
                                <asp:GridView ID="Grid_Capturas" runat="server" AllowPaging="True" CssClass="GridView_1"
                                    AutoGenerateColumns="False" PageSize="5" Width="100%" AllowSorting="True" HeaderStyle-CssClass="tblHead"
                                    Style="white-space: normal;" EmptyDataText="SIN DATOS" OnPageIndexChanging="Grid_Capturas_PageIndexChanging"
                                    OnSelectedIndexChanged="Grid_Capturas_SelectedIndexChanged">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button_.png">
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="NO_CAPTURA" HeaderText="CAPTURA" Visible="false" />
                                        <asp:BoundField DataField="INSTITUCION" HeaderText="Institución" HeaderStyle-Width="15%" />
                                        <asp:BoundField DataField="NO_CAJA" HeaderText="Caja" />
                                        <asp:BoundField DataField="FECHA_CAPTURA" HeaderText="Fecha captura" DataFormatString="{0:dd-MMM-yyyy}"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="FECHA_CORTE" HeaderText="Fecha corte" DataFormatString="{0:dd-MMM-yyyy}"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="NOMBRE_ARCHIVO" HeaderText="Archivo" />
                                        <asp:BoundField DataField="MOVIMIENTOS" HeaderText="Movimientos" DataFormatString="{0:#,##0}"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="INCLUIDOS" HeaderText="Incluidos" DataFormatString="{0:#,##0}"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="EXCLUIDOS" HeaderText="Excluidos" DataFormatString="{0:#,##0}"
                                            ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="INSTITUCION_ID" HeaderText="Institución Id" Visible="false" />
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

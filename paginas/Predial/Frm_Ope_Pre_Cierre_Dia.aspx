<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Cierre_Dia.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Cierre_Dia" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
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
                            Cierre de Día Definitivo
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
                <table width="100%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr class="barra_busqueda" align="right">
                        <td style="width: 50%" align="left">
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Guardar" CssClass="Img_Button"
                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                        </td>
                        <td style="width: 50%">
                            Busqueda
                            <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar Fecha Cierre"
                                Width="180px" Enabled="false" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Buscar Fecha Cierre>" TargetControlID="Txt_Busqueda" />
                            <cc1:CalendarExtender ID="DTP_Fecha_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Fecha_Busqueda" />
                            <asp:ImageButton ID="Btn_Fecha_Busqueda" runat="server" ImageUrl="../imagenes/paginas/SmallCalendar.gif"
                                Style="vertical-align: top;" Height="18px" CausesValidation="false" Enabled="false" />
                            <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                ToolTip="Buscar por Fecha" OnClick="Btn_Buscar_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="Pnl_Datos_Generales" runat="server" GroupingText="Datos Generales de Apertura y Cierre de Turno"
                    Width="97%" BackColor="White">
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                No Cierre Día
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_No_Cierre_Dia" runat="server" ReadOnly="true" Width="98%" BorderStyle="Solid"
                                    BorderWidth="1" />
                            </td>
                            <td style="text-align: left; width: 20%;">
                                Estatus
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Estatus_Cierre_Dia" runat="server" ReadOnly="true" Width="98%"
                                    BorderStyle="Solid" BorderWidth="1" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Fecha Apertura
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Apertura_Dia" runat="server" ReadOnly="true" Width="98%"
                                    BorderStyle="Solid" BorderWidth="1" />
                            </td>
                            <td style="text-align: left; width: 20%;">
                                Hora Apertura
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Hora_Apertura_Dia" runat="server" ReadOnly="true" Width="98%"
                                    BorderStyle="Solid" BorderWidth="1" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Fecha Cierre
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Cierre_Dia" runat="server" ReadOnly="true" Width="98%"
                                    BorderStyle="Solid" BorderWidth="1" />
                            </td>
                            <td style="text-align: left; width: 20%;">
                                Hora Cierre
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Hora_Cierre_Dia" runat="server" ReadOnly="true" Width="98%"
                                    BorderStyle="Solid" BorderWidth="1" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table width="97%" class="estilo_fuente">
                    <%------------------ Cajas abiertas ------------------%>
                    <tr style="background-color: #36C;">
                        <td style="width: 100%; text-align: left; font-size: 15px; color: #FFF;">
                            Cajas Abiertas
                        </td>
                    </tr>
                    <tr align="center">
                        <td style="width: 100%;">
                            <asp:GridView ID="Grid_Cajas_Abiertas" runat="server" AllowPaging="True" Width="100%"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" AllowSorting="True"
                                HeaderStyle-CssClass="tblHead" OnPageIndexChanging="Grid_Cajas_Abiertas_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="Modulo" HeaderText="Módulo" />
                                    <asp:BoundField DataField="Caja" HeaderText="Caja" />
                                    <asp:BoundField DataField="Cajero" HeaderText="Cajero" />
                                    <asp:BoundField DataField="Fecha_Turno" HeaderText="Apertura" DataFormatString="{0:dd/MMM/yyyy}" />
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr align="center">
                        <td style="width: 100%;">
                            <asp:Label ID="Lbl_Cajas_Abiertas" Visible="false" runat="server" Font-Bold="true"
                                Text="Ya se puede realizar el cierre de turno de día definitivo ya que no exisen cajas abiertas"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div id="Div_Totales_Turno" style="width: 97%;" visible="false">
                    <table width="100%" border="0" cellspacing="0" class="estilo_fuente">
                        <tr>
                            <td style="width: 50%; text-align: left; vertical-align: top;">
                            </td>
                            <td style="width: 100%; background-color: #36C; text-align: left; font-size: 15px;
                                color: #FFF;">
                                Total Registrado
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 50%; text-align: left; vertical-align: top;">
                            </td>
                            <td style="width: 50%; text-align: left; vertical-align: top;">
                                <asp:UpdatePanel ID="Upnl_Totales_Turno" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="Pnl_Totales_Turno" runat="server" GroupingText="Total Pagado" Width="97%"
                                            BackColor="White">
                                            <table width="98%" class="estilo_fuente">
                                                <tr>
                                                    <td style="text-align: left; width: 50%; font-family: Tahoma; font-size: 13px; font-weight: bold;">
                                                        Total Efectivo
                                                    </td>
                                                    <td style="text-align: left; width: 50%;">
                                                        <asp:TextBox ID="Txt_Total_Efectivo" Text="0.0" runat="server" Width="98%" ReadOnly="true"
                                                            Style="font-weight: bolder; font-size: medium; text-align: right;" BorderWidth="1" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 50%; font-family: Tahoma; font-size: 13px; font-weight: bold;">
                                                        Total Bancos
                                                    </td>
                                                    <td style="text-align: left; width: 50%;">
                                                        <asp:TextBox ID="Txt_Total_Bancos" Text="0.0" runat="server" Width="98%" ReadOnly="true"
                                                            Style="font-weight: bolder; font-size: medium; text-align: right;" BorderWidth="1" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 50%; font-family: Tahoma; font-size: 13px; font-weight: bold;">
                                                        Total Cheques
                                                    </td>
                                                    <td style="text-align: left; width: 50%;">
                                                        <asp:TextBox ID="Txt_Total_Cheques" Text="0.0" runat="server" ReadOnly="true" Width="98%"
                                                            Style="font-weight: bolder; font-size: medium; text-align: right;" BorderWidth="1" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 50%; font-family: Tahoma; font-size: 13px; font-weight: bold;">
                                                        Total Transferencias
                                                    </td>
                                                    <td style="text-align: left; width: 50%;">
                                                        <asp:TextBox ID="Txt_Total_Transferencia" Text="0.0" runat="server" ReadOnly="true"
                                                            Width="98%" Style="font-weight: bolder; font-size: medium; text-align: right;"
                                                            BorderWidth="1" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 50%; font-family: Tahoma; font-size: 13px; font-weight: bold;">
                                                        Total Ajuste Tarifario
                                                    </td>
                                                    <td style="text-align: left; width: 50%;">
                                                        <asp:TextBox ID="Txt_Total_Ajuste_Tarifario" Text="0.0" runat="server" Width="98%"
                                                            ReadOnly="true" Style="font-weight: bolder; font-size: medium; text-align: right;"
                                                            BorderWidth="1" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 50%; font-family: Tahoma; font-size: 13px; font-weight: bold;">
                                                        Monto Total
                                                    </td>
                                                    <td style="text-align: left; width: 50%;">
                                                        <asp:TextBox ID="Txt_Monto_Total" runat="server" Text="0.0" ReadOnly="true" Width="98%"
                                                            Style="font-weight: bolder; font-size: medium; text-align: right;" BorderWidth="1"
                                                            ForeColor="Red" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

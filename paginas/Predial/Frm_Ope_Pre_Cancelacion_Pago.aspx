<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Cancelacion_Pago.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Cancelacion_Pago" %>

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
                            Cancelación de pagos
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
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                                            <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender_Btn_Nuevo" runat="server" ConfirmText="¿Esta seguro de cancelar el recibo?"
                                                TargetControlID="Btn_Nuevo">
                                            </cc1:ConfirmButtonExtender>
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            <table style="width: 100%; height: 28px;">
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                        B&uacute;squeda:
                                                    </td>
                                                    <td style="width: 55%;">
                                                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="3" ToolTip="Buscar"
                                                            Width="180px" />
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. " />
                                                    </td>
                                                    <td style="vertical-align: middle; width: 5%;">
                                                        <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
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
                            &nbsp;
                        </td>
                        <td style="text-align: left; width: 30%;">
                            &nbsp;
                        </td>
                        <td style="text-align: left; width: 20%;">
                            # Acumulado Cancelados
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_No_Acumulado_Recibos_Cancelados" runat="server" Width="96.4%"
                                ReadOnly="True" BorderStyle="Solid" BorderWidth="1" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Caja
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Caja" runat="server" Width="96.4%" ReadOnly="True" BorderStyle="Solid"
                                BorderWidth="1" />
                        </td>
                        <td style="text-align: left; width: 20%;">
                            Fecha
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Fecha" runat="server" Width="96.4%" ReadOnly="True" BorderStyle="Solid"
                                BorderWidth="1" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Módulo
                        </td>
                        <td style="text-align: left; width: 30%;" colspan="3">
                            <asp:TextBox ID="Txt_Modulo" runat="server" Width="99.2%" ReadOnly="True" BorderStyle="Solid"
                                BorderWidth="1" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Cajero
                        </td>
                        <td style="text-align: left; width: 30%;" colspan="3">
                            <asp:TextBox ID="Txt_Cajero" runat="server" Width="99.2%" ReadOnly="True" BorderStyle="Solid"
                                BorderWidth="1" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            No. de Recibo
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_No_Recibo" runat="server" Width="96.4%" ReadOnly="True" BorderStyle="Solid"
                                BorderWidth="1" />
                        </td>
                        <td style="text-align: left; width: 20%;">
                            No. de Operación
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_No_Operacion" runat="server" Width="96.4%" ReadOnly="True" BorderStyle="Solid"
                                BorderWidth="1" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Monto pagado
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Monto" runat="server" Width="96.4%" ReadOnly="True" BorderStyle="Solid"
                                BorderWidth="1" />
                        </td>
                        <td style="text-align: left; width: 20%;">
                            Fecha Pago
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Fecha_Pago" runat="server" Width="96.4%" ReadOnly="True" BorderStyle="Solid"
                                BorderWidth="1" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Contribuyente
                        </td>
                        <td style="text-align: left; width: 30%;" colspan="3">
                            <asp:TextBox ID="Txt_Contribuyente" runat="server" Width="99.2%" ReadOnly="True"
                                BorderStyle="Solid" BorderWidth="1" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            *Motivo
                        </td>
                        <td style="text-align: left; width: 30%;" colspan="3">
                            <asp:DropDownList ID="Cmb_Motivo" runat="server" Width="99.2%" TabIndex="5">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Observaciones
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%; vertical-align: top;">
                            <asp:TextBox ID="Txt_Observaciones" runat="server" TabIndex="6" MaxLength="250" TextMode="MultiLine"
                                Width="99.2%" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios" runat="server" TargetControlID="Txt_Observaciones"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_Cancelacion" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" CssClass="GridView_1" EnableModelValidation="True"
                                HeaderStyle-CssClass="tblHead" OnPageIndexChanging="Grid_Cancelaciones_PageIndexChanging"
                                OnSelectedIndexChanged="Grid_Cancelacion_SelectedIndexChanged" Style="white-space: normal;"
                                Width="100%">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="NO_PAGO" HeaderText="No. pago" Visible="false" />
                                    <asp:BoundField DataField="NO_RECIBO" HeaderText="No. Recibo" ItemStyle-HorizontalAlign="Center"
                                        Visible="True" />
                                    <asp:BoundField DataField="No_Operacion" HeaderText="No. Operación" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="FECHA" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Fecha"
                                        ItemStyle-HorizontalAlign="Center" Visible="True" />
                                    <asp:BoundField DataField="CAJA" HeaderText="Caja" Visible="True" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Documento" HeaderText="Documento" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Contribuyente" HeaderText="Contribuyente" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="MONTO" DataFormatString="{0:c}" HeaderText="Monto" ItemStyle-HorizontalAlign="Right"
                                        Visible="True" />
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
                        <td>
                            <asp:TextBox ID="Txt_No_Turno" runat="server" Visible="false" Width="96.4%" />
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Id" runat="server" Width="96.4%" Visible="false" />
                        </td>
                    </tr>
                </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

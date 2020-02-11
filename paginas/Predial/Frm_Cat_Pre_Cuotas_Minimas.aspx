<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Cuotas_Minimas.aspx.cs" Inherits="paginas_predial_Frm_Cat_Cuotas_Minimas"
    Title="Catalogo de Cuotas Minimas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScptM_Cuotas_Minimas" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="2">
                            Catálogo de Cuotas Minimas
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
                                        <td style="font-size: 10px; width: 90%; text-align: left;" valign="top">
                                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Nuevo" OnClick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Modificar" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Eliminar" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro de la Base de Datos?');"
                                OnClick="Btn_Eliminar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click" />
                        </td>
                        <td>
                            Búsqueda:
                            <asp:TextBox ID="Txt_Busqueda_Cuotas_Minimas" runat="server" Width="130px"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Cuotas_Minimas" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" OnClick="Btn_Buscar_Cuotas_Minimas_Click"
                                Height="20px" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Cuotas_Minimas" runat="server"
                                WatermarkText="<Año>" TargetControlID="Txt_Busqueda_Cuotas_Minimas" WatermarkCssClass="watermarked">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Cuotas_Minimas" runat="server"
                                FilterType="Numbers" TargetControlID="Txt_Busqueda_Cuotas_Minimas">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
                <br />
                <center>
                    <table width="96%">
                        <tr>
                            <td colspan="4">
                                <asp:HiddenField ID="Hdf_Cuota_Minima_ID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 18%; text-align: left;">
                                <asp:Label ID="Lbl_ID_Cuota_Minima" runat="server" Text="Cuota Minima ID" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 32%">
                                <asp:TextBox ID="Txt_ID_Cuota_Minima" runat="server" Width="98%" MaxLength="10" Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%; text-align: left;">
                                <asp:Label ID="Lbl_Anio" runat="server" Text="* Año" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 35%; text-align: left;">
                                <asp:TextBox ID="Txt_Anio" runat="server" Width="98%" MaxLength="4"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Anio" runat="server" TargetControlID="Txt_Anio"
                                    FilterType="Numbers" Enabled="True">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="width: 15%; text-align: left;">
                                <asp:Label ID="Lbl_Cuota" runat="server" Text="* Cuota" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 35%; text-align: left;">
                                <asp:TextBox ID="Txt_Cuota" runat="server" Width="98%" OnTextChanged="Txt_Cuota_TextChanged"
                                    AutoPostBack="true"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Txt_Cuota_FTBE" runat="server" TargetControlID="Txt_Cuota"
                                    ValidChars="0123456789.," FilterType="Custom, Numbers">
                                </cc1:FilteredTextBoxExtender>
                                <%--
                                <cc1:MaskedEditExtender ID="MEE_Txt_Cuota" runat="server" TargetControlID="Txt_Cuota"
                                    Mask="9,999,999.99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                    OnInvalidCssClass="MaskedEditError" MaskType="Number" InputDirection="RightToLeft"
                                    AcceptNegative="Left" DisplayMoney="None" ErrorTooltipEnabled="True" />--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 18%; text-align: left;">
                                <asp:Label ID="Lbl_Bimestre" runat="server" Text="Bimestre" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 32%">
                                <asp:TextBox ID="Txt_Bimestre" runat="server" Width="98%" MaxLength="10" Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:GridView ID="Grid_Cuotas_Minimas" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                        AllowPaging="True" PageSize="5" Width="96%" OnSelectedIndexChanged="Grid_Cuotas_Minimas_SelectedIndexChanged"
                        GridLines="None" OnPageIndexChanging="Grid_Cuotas_Minimas_PageIndexChanging">
                        <RowStyle CssClass="GridItem" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                <ItemStyle Width="30px" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="CUOTA_MINIMA_ID" HeaderText="Cuota Minima ID" SortExpression="CUOTA_MINIMA_ID" />
                            <asp:BoundField DataField="ANIO" HeaderText="Año" SortExpression="Año" />
                            <asp:BoundField DataField="CUOTA" HeaderText="Cuota" SortExpression="Cuota" DataFormatString="{0:c2}" />
                            <asp:BoundField DataField="BIMESTRE" HeaderText="Bimestre" DataFormatString="{0:c2}">
                                <HeaderStyle Width="15%" />
                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                            </asp:BoundField>
                        </Columns>
                        <PagerStyle CssClass="GridHeader" />
                        <SelectedRowStyle CssClass="GridSelected" />
                        <HeaderStyle CssClass="GridHeader" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                    </asp:GridView>
                </center>
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

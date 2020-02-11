<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Descuentos_Predial.aspx.cs" Inherits="paginas_predial_Cls_Cat_Pre_Descuentos_Predial"
    Title="Catalogo de Descuentos Predial" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScptM_Descuentos" runat="server" />
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
                        <td colspan="2" class="label_titulo">
                            Catálogo de Descuentos Pronto Pago Predial
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
                                        <td style="width: 90%; text-align: left;" valign="top">
                                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
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
                            <asp:TextBox ID="Txt_Busqueda_Descuento" runat="server" MaxLength="4" Width="130px"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Descuento" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" OnClick="Btn_Buscar_Descuento_Click" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Descuento" runat="server" TargetControlID="Txt_Busqueda_Descuento"
                                FilterType="Numbers" Enabled="True">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Descuento" runat="server" WatermarkText="<Año>"
                                TargetControlID="Txt_Busqueda_Descuento" WatermarkCssClass="watermarked">
                            </cc1:TextBoxWatermarkExtender>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="96%">
                    <tr>
                        <td colspan="4">
                            <asp:HiddenField ID="Hdf_Descuento_ID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 18%; text-align: left;">
                            <asp:Label ID="Lbl_ID_Descuento" runat="server" Text="Descuento ID" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width: 82%">
                            <asp:TextBox ID="Txt_ID_Descuento" runat="server" Width="48%" MaxLength="10" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 18%; text-align: left;">
                            <asp:Label ID="Lbl_Anio" runat="server" Text="* Año" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width: 82%">
                            <asp:TextBox ID="Txt_Anio" runat="server" Width="48%" MaxLength="4"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Anio" runat="server" TargetControlID="Txt_Anio"
                                FilterType="Numbers" Enabled="True">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Panel ID="Pnl_Tasas__Por_Mes" runat="server" Width="98%" Style="border-style: outset;">
                                <table width="99%">
                                    <tr>
                                        <td style="width: 13%; text-align: left;">
                                            <asp:Label ID="Lbl_Enero" runat="server" Text="Enero (%)" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 20%; text-align: left;">
                                            <asp:TextBox ID="Txt_Enero" runat="server" Width="98%" MaxLength="6"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="Txt_Enero" ValidChars="0123456789." Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td style="width: 13%; text-align: left;">
                                            <asp:Label ID="Lbl_Febrero" runat="server" Text="Febrero (%)" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 20%; text-align: left;">
                                            <asp:TextBox ID="Txt_Febrero" runat="server" Width="98%" MaxLength="6"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="Txt_Febrero" ValidChars="0123456789." Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td style="width: 13%; text-align: left;">
                                            <asp:Label ID="Lbl_Marzo" runat="server" Text="Marzo (%)" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 21%; text-align: left;">
                                            <asp:TextBox ID="Txt_Marzo" runat="server" Width="98%" MaxLength="6"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="Txt_Marzo" ValidChars="0123456789." Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 13%; text-align: left;">
                                            <asp:Label ID="Lbl_Abril" runat="server" Text="Abril (%)" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 20%; text-align: left;">
                                            <asp:TextBox ID="Txt_Abril" runat="server" Width="98%" MaxLength="6"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="Txt_Abril" ValidChars="0123456789." Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td style="width: 13%; text-align: left;">
                                            <asp:Label ID="Lbl_Mayo" runat="server" Text="Mayo (%)" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 20%; text-align: left;">
                                            <asp:TextBox ID="Txt_Mayo" runat="server" Width="98%" MaxLength="6"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="Txt_Mayo" ValidChars="0123456789." Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td style="width: 13%; text-align: left;">
                                            <asp:Label ID="Lbl_Junio" runat="server" Text="Junio (%)" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 21%; text-align: left;">
                                            <asp:TextBox ID="Txt_Junio" runat="server" Width="98%" MaxLength="6"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="Txt_Junio" ValidChars="0123456789." Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 13%; text-align: left;">
                                            <asp:Label ID="Lbl_Julio" runat="server" Text="Julio (%)" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 20%; text-align: left;">
                                            <asp:TextBox ID="Txt_Julio" runat="server" Width="98%" MaxLength="6"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="Txt_Julio" ValidChars="0123456789." Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td style="width: 13%; text-align: left;">
                                            <asp:Label ID="Lbl_Agosto" runat="server" Text="Agosto (%)" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 20%; text-align: left;">
                                            <asp:TextBox ID="Txt_Agosto" runat="server" Width="98%" MaxLength="6"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="Txt_Agosto" ValidChars="0123456789." Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td style="width: 17%; text-align: left;">
                                            <asp:Label ID="Lbl_Septiembre" runat="server" Text="Septiembre (%)" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 25%; text-align: left;">
                                            <asp:TextBox ID="Txt_Septiembre" runat="server" Width="98%" MaxLength="6"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="Txt_Septiembre" ValidChars="0123456789." Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 13%; text-align: left;">
                                            <asp:Label ID="Lbl_Octubre" runat="server" Text="Octubre (%)" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 20%; text-align: left;">
                                            <asp:TextBox ID="Txt_Octubre" runat="server" Width="98%" MaxLength="6"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="Txt_Octubre" ValidChars="0123456789." Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td style="width: 17%; text-align: left;">
                                            <asp:Label ID="Lbl_Noviembre" runat="server" Text="Noviembre (%)" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 20%; text-align: left;">
                                            <asp:TextBox ID="Txt_Noviembre" runat="server" Width="98%" MaxLength="6"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="Txt_Noviembre" ValidChars="0123456789." Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td style="width: 13%; text-align: left;">
                                            <asp:Label ID="Lbl_Diciembre" runat="server" Text="Diciembre (%)" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 21%; text-align: left;">
                                            <asp:TextBox ID="Txt_Diciembre" runat="server" Width="98%" MaxLength="6"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="Txt_Diciembre" ValidChars="0123456789." Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="Grid_Descuentos" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                    AllowPaging="True" PageSize="5" Width="96%" EmptyDataText="&quot;No se encontraron registros&quot;"
                    OnSelectedIndexChanged="Grid_Descuentos_SelectedIndexChanged" GridLines="None"
                    OnPageIndexChanging="Grid_Descuentos_PageIndexChanging">
                    <RowStyle CssClass="GridItem" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                            <ItemStyle Width="30px" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="DESCUENTO_ID" HeaderText="Descuento ID" SortExpression="DESCUENTO_ID" />
                        <asp:BoundField DataField="ANIO" HeaderText="Año" SortExpression="Anio" />
                    </Columns>
                    <PagerStyle CssClass="GridHeader" />
                    <SelectedRowStyle CssClass="GridSelected" />
                    <HeaderStyle CssClass="GridHeader" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                </asp:GridView>
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

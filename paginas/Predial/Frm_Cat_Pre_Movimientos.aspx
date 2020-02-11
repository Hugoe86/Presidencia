<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Movimientos.aspx.cs" Inherits="paginas_predial_Frm_Cat_Pre_Movimientos"
    Title="Catalogo de Movimientos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScptM_Movimientos" runat="server" />
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
                            Catálogo de Movimientos
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
                            <asp:TextBox ID="Txt_Busqueda_Movimiento" runat="server" Width="130px"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Movimiento" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" OnClick="Btn_Buscar_Movimiento_Click" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Movimiento" runat="server" WatermarkText="<Identificador>"
                                TargetControlID="Txt_Busqueda_Movimiento" WatermarkCssClass="watermarked">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Movimiento" runat="server" TargetControlID="Txt_Busqueda_Movimiento"
                                InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
                <br />
                <center>
                    <table width="98%">
                        <tr>
                            <td colspan="4">
                                <asp:HiddenField ID="Hdf_Movimiento_ID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 18%; text-align: left;">
                                <asp:Label ID="Lbl_ID_Movimiento" runat="server" Text="Movimiento ID" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 32%">
                                <asp:TextBox ID="Txt_ID_Movimiento" runat="server" Width="98%" MaxLength="10" Enabled="False"></asp:TextBox>
                            </td>
                            <td style="width: 18%; text-align: left;">
                                <asp:Label ID="Lbl_Grupo" runat="server" Text="* Grupo" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 32%">
                                <asp:DropDownList ID="Cmb_Grupo" runat="server" Width="100%">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%; text-align: left;">
                                <asp:Label ID="Lbl_Identificador" runat="server" Text="* Identificador" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 35%">
                                <asp:TextBox ID="Txt_Identificador" runat="server" Width="98%" MaxLength="10"></asp:TextBox>
                            </td>
                            <td style="width: 15%; text-align: left;">
                                <asp:Label ID="Lbl_Estatus" runat="server" Text="* Estatus" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 35%">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                    <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                    <asp:ListItem Text="BAJA" Value="BAJA" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%; text-align: left;">
                                <asp:Label ID="Lbl_Traslacion" runat="server" Text="* Aplica" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 35%; text-align: left;">
                                <asp:DropDownList ID="Cmb_Aplica" runat="server" Width="100%">
                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                    <asp:ListItem Text="PREDIAL" Value="PREDIAL" />
                                    <asp:ListItem Text="TRASLADO" Value="TRASLADO" />
                                </asp:DropDownList>
                            </td>
                            <td style="width: 15%; text-align: left;">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%; text-align: left;">
                                <asp:Label ID="Label2" runat="server" CssClass="estilo_fuente" Text="* Cargar en Modulos"></asp:Label>
                            </td>
                            <td colspan="3" style="text-align: left;">
                                <asp:CheckBoxList ID="Chk_Lst_Cargar_Modulos" runat="server" RepeatColumns="3" Width="98%">
                                    <asp:ListItem Value="BAJAS_DIRECTAS">Bajas Directas</asp:ListItem>
                                    <asp:ListItem Value="CANCELACION_CUENTAS">Cancelación de Cuentas</asp:ListItem>
                                    <asp:ListItem Value="REACTIVACION_CUENTAS">Reactivación de Cuentas</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%; text-align: left; vertical-align: top;">
                                <asp:Label ID="Lbl_Descripcion" runat="server" Text="* Descripci&oacute;n" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Descripcion" runat="server" Rows="3" TextMode="MultiLine" Width="98%"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Descripcion" runat="server" TargetControlID="Txt_Descripcion"
                                    WatermarkText="Límite de Caractes 100" WatermarkCssClass="watermarked" Enabled="True" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:GridView ID="Grid_Movimientos" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                        AllowPaging="True" PageSize="10" Width="96%" Style="white-space: normal" GridLines="None"
                        OnSelectedIndexChanged="Grid_Movimientos_SelectedIndexChanged" OnPageIndexChanging="Grid_Movimientos_PageIndexChanging"
                        DataKeyNames="CARGAR_MODULOS">
                        <RowStyle CssClass="GridItem" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                <ItemStyle Width="30px" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="MOVIMIENTO_ID" HeaderText="Movimiento ID" SortExpression="MOVIMIENTO_ID"
                                Visible="false" />
                            <asp:BoundField DataField="IDENTIFICADOR" HeaderText="Identificador" SortExpression="Identificador" />
                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="APLICA" HeaderText="Aplica" SortExpression="Aplica" />
                            <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" SortExpression="descripción" />
                            <asp:BoundField DataField="CARGAR_MODULOS" HeaderText="CARGAR_MODULOS" Visible="False" />
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

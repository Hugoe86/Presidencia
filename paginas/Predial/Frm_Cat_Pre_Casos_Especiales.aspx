<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Casos_Especiales.aspx.cs" Inherits="paginas_predial_Frm_Cat_Pre_Casos_Especiales"
    Title="Catalogo de Casos Especiales" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
<script language="javascript" type="text/javascript">

    function ValidarCaracteres(textareaControl, maxlength) { if (textareaControl.value.length > maxlength) { textareaControl.value = textareaControl.value.substring(0, maxlength); alert("Debe ingresar hasta un máximo de " + maxlength + " caracteres"); } }

</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScptM_Casos_Especiales" runat="server" />
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
                            Catálogo de Casos Especiales
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
                            <asp:TextBox ID="Txt_Busqueda_Caso_Especial" runat="server" Width="130px" style="text-transform:uppercase"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Caso_Especial" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" OnClick="Btn_Buscar_Caso_Especial_Click" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Caso_Especial" runat="server"
                                WatermarkText="<Descripción>" TargetControlID="Txt_Busqueda_Caso_Especial" WatermarkCssClass="watermarked">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Caso_Especial" runat="server" TargetControlID="Txt_Busqueda_Caso_Especial"
                                InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
                <br />
                <center>
                    <table width="96%">
                        <tr>
                            <td colspan="4">
                                <asp:HiddenField ID="Hdf_Caso_Especial_ID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 18%; text-align: left;">
                                <asp:Label ID="Lbl_ID_Caso_Especial" runat="server" Text="Caso Especial ID" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 32%">
                                <asp:TextBox ID="Txt_ID_Caso_Especial" runat="server" Width="98%" MaxLength="10"
                                    Enabled="False"></asp:TextBox>
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
                            </td>
                            <td style="width: 35%; text-align: left;">
                                &nbsp;
                            </td>
                            <td style="width: 15%; text-align: left;">
                                <asp:Label ID="Lbl_Tipo" runat="server" Text="* Tipo" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 35%">
                                <asp:DropDownList ID="Cmb_Tipo" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Tipo_SelectedIndexChanged">
                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                    <asp:ListItem Text="FINANCIAMIENTO" Value="FINANCIAMIENTO" />
                                    <asp:ListItem Text="SOLICITANTE" Value="SOLICITANTE" />
                                    <asp:ListItem Text="ESCUELAS" Value="ESCUELAS" />
                                    <asp:ListItem Text="ASOCIACIONES" Value="ASOCIACIONES" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <asp:Panel ID="Pnl_Porcentaje" runat="server" Visible="false">
                        <tr>
                        <td style="width: 15%; text-align: left;">
                            </td>
                            <td style="width: 35%; text-align: left;">
                                &nbsp;
                            </td>
                            <td style="width: 15%; text-align: left;">
                                <asp:Label ID="Lbl_Porcentaje" runat="server" Text="* Porcentaje" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 35%">
                                <asp:TextBox ID="Txt_Porcentaje" runat="server" Width="98%" MaxLength="6" AutoPostBack="true" OnTextChanged="Txt_Porcentaje_TextChanged" Enabled="false"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTBE_Txt_Porcentaje" TargetControlID="Txt_Porcentaje" runat="server" FilterType="Numbers, Custom" ValidChars="0123456789." />
                            </td>
                        </tr>
                        </asp:Panel>
                        <tr>
                            <td style="width: 15%; text-align: left; vertical-align: top;">
                                <asp:Label ID="Lbl_Descripcion" runat="server" Text="* Descripci&oacute;n" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Descripcion" runat="server" Rows="2" TextMode="MultiLine" Width="98%" MaxLength="45" style="text-transform:uppercase"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Descripcion" runat="server" TargetControlID="Txt_Descripcion"
                                    WatermarkText="Límite de Caractes 45" WatermarkCssClass="watermarked" Enabled="True" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%; text-align: left;">
                                <asp:Label ID="Lbl_Articulo" runat="server" Text="* Artículo" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 35%">
                                <asp:TextBox ID="Txt_Articulo" runat="server" Width="98%" MaxLength="10"></asp:TextBox>
                            </td>
                            <td style="width: 15%; text-align: left;">
                                <asp:Label ID="Lbl_Inciso" runat="server" Text="* Inciso" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 35%">
                                <asp:TextBox ID="Txt_Inciso" runat="server" Width="98%" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%; text-align: left; vertical-align: top;">
                                <asp:Label ID="Lbl_Observaciones" runat="server" Text="* Observaciones" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Observaciones" runat="server" Rows="2" TextMode="MultiLine"
                                    Width="98%" MaxLength="45"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observaciones" runat="server" TargetControlID="Txt_Observaciones"
                                    WatermarkText="Límite de Caractes 45" WatermarkCssClass="watermarked" Enabled="True" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:GridView ID="Grid_Casos_Especiales" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                        AllowPaging="True" PageSize="5" Width="96%" OnSelectedIndexChanged="Grid_Casos_Especiales_SelectedIndexChanged"
                        GridLines="None" OnPageIndexChanging="Grid_Casos_Especiales_PageIndexChanging">
                        <RowStyle CssClass="GridItem" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                <ItemStyle Width="30px" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="CASO_ESPECIAL_ID" HeaderText="Caso Especial ID" SortExpression="CASO_ESPECIAL_ID" />
                            <asp:BoundField DataField="IDENTIFICADOR" HeaderText="Identificador" SortExpression="Identificador" />
                            <asp:BoundField DataField="DESCRIPCION" HeaderText="Nombre" SortExpression="Nombre" />
                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="TIPO" HeaderText="Tipo" SortExpression="Tipo" />
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

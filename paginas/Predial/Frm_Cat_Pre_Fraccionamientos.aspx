<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Fraccionamientos.aspx.cs" Inherits="paginas_predial_Frm_Cat_Pre_Fraccionamientos"
    Title="Catalogo de Fraccionamientos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScptM_Fraccionamientos" runat="server" />
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
                        <td colspan="4" class="label_titulo">
                            Cat&aacute;logo de Fraccionamientos
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
                        <td colspan="4">
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
                                Width="24px" CssClass="Img_Button" AlternateText="Eliminar" 
                                OnClientClick="return confirm('Sólo se cambiará el estatus a BAJA. ¿Confirma que desea proceder?');"
                                OnClick="Btn_Eliminar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click" />
                        </td>
                        <td>
                            Búsqueda:
                            <asp:TextBox ID="Txt_Busqueda_Fraccionamiento" runat="server" Width="130px"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Fraccionamiento" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" OnClick="Btn_Buscar_Fraccionamiento_Click" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Fraccionamiento" runat="server"
                                WatermarkText="<Identificador>" TargetControlID="Txt_Busqueda_Fraccionamiento"
                                WatermarkCssClass="watermarked">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Fraccionamiento" runat="server"
                                TargetControlID="Txt_Busqueda_Fraccionamiento" InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
                <br />
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="98%" ActiveTabIndex="0">
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1" Width="100%">
                        <HeaderTemplate>
                            Fraccionamientos</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table width="100%">
                                    <tr>
                                        <td colspan="4">
                                            <asp:HiddenField ID="Hdf_Fraccionamiento_ID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 18%; text-align: left;">
                                            <asp:Label ID="Lbl_ID_Fraccionamiento" runat="server" Text="Fraccionamiento ID" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 32%">
                                            <asp:TextBox ID="Txt_ID_Fraccionamiento" runat="server" Width="98%" MaxLength="10"
                                                Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left;">
                                            <asp:Label ID="Lbl_Nombre" runat="server" Text="* Identificador" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 35%">
                                            <asp:TextBox ID="Txt_Identificador" runat="server" Width="98%" MaxLength="10" Style="text-transform: uppercase;"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Identificador" runat="server" TargetControlID="Txt_Identificador"
                                                InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                            </cc1:FilteredTextBoxExtender>
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
                                        <td style="width: 15%; text-align: left; vertical-align: top;">
                                            <asp:Label ID="Lbl_Descripcion" runat="server" Text="* Descripci&oacute;n" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Descripcion" runat="server" Rows="3" TextMode="MultiLine" Width="98%"
                                                Style="text-transform: uppercase;"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Descripcion" runat="server" TargetControlID="Txt_Descripcion"
                                                WatermarkText="Límite de Caractes 100" WatermarkCssClass="watermarked" Enabled="True" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Descripcion" runat="server" TargetControlID="Txt_Descripcion"
                                                InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel2" ID="TabPanel2" Width="100%">
                        <HeaderTemplate>
                            Fraccionamientos - Impuestos</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table width="100%">
                                    <tr>
                                        <td colspan="4">
                                            <asp:HiddenField ID="Hdf_Fraccionamiento_Impuesto_ID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 18%; text-align: left;">
                                            <asp:Label ID="Lbl_Impuesto_ID" runat="server" Text="Fracc. Impuesto ID" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 32%">
                                            <asp:TextBox ID="Txt_Impuesto_ID" runat="server" Width="98%" MaxLength="10" Enabled="False"></asp:TextBox>
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
                                            <asp:Label ID="Lbl_Monto" runat="server" Text="* Tarifa" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 35%; text-align: left;">
                                            <asp:TextBox ID="Txt_Monto" runat="server" Width="98%" AutoPostBack="true" OnTextChanged="Txt_Monto_TextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Monto" runat="server" TargetControlID="Txt_Monto" FilterType="Custom, Numbers"
                                                ValidChars="0123456789.,">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            &nbsp;
                                        </td>
                                        <td style="width: 15%;">
                                            <asp:ImageButton ID="Btn_Agregar_Impuesto" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                                AlternateText="Agregar" OnClick="Btn_Agregar_Impuesto_Click" />
                                            <asp:ImageButton ID="Btn_Modificar_Impuesto" runat="server" ImageUrl="~/paginas/imagenes/paginas/actualizar_detalle.png"
                                                AlternateText="Modificar" OnClick="Btn_Modificar_Impuesto_Click" />
                                            <asp:ImageButton ID="Btn_Quitar_Impuesto" runat="server" ImageUrl="~/paginas/imagenes/paginas/quitar.png"
                                                AlternateText="Quitar" OnClick="Btn_Quitar_Impuesto_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:GridView ID="Grid_Fraccionamientos_Impuesto" runat="server" AutoGenerateColumns="False"
                                    CellPadding="4" ForeColor="#333333" GridLines="None" AllowPaging="True" Width="98%"
                                    OnPageIndexChanging="Grid_Fraccionamientos_Impuesto_PageIndexChanging" PageSize="5"
                                    CssClass="GridView_1" OnSelectedIndexChanged="Grid_Fraccionamientos_Impuesto_SelectedIndexChanged">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="30px" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="IMPUESTO_FRACCIONAMIENTO_ID" HeaderText="IMPUESTO_FRACCIONAMIENTO_ID"
                                            SortExpression="Id_Impuesto" />
                                        <asp:BoundField DataField="ANIO" HeaderText="Año" />
                                        <asp:BoundField DataField="MONTO" HeaderText="Monto" />
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
                <table id="Tbl_Fracccionamientos" border="0" cellspacing="0" class="estilo_fuente"
                    style="width: 100%;">
                    <tr>
                        <asp:GridView ID="Grid_Fraccionamientos_Generales" runat="server" CssClass="GridView_1"
                            AutoGenerateColumns="False" AllowPaging="True" PageSize="5" Width="98%" GridLines="None"
                            OnSelectedIndexChanged="Grid_Fraccionamientos_Generales_SelectedIndexChanged"
                            OnPageIndexChanging="Grid_Fraccionamientos_Generales_PageIndexChanging" EnableModelValidation="True">
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle Width="30px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="FRACCIONAMIENTO_ID" HeaderText="Fraccionamiento ID" SortExpression="FRACCIONAMIENTO_ID">
                                    <HeaderStyle Width="8%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" SortExpression="descripcion">
                                    <HeaderStyle Width="80%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="Estatus" />
                            </Columns>
                            <PagerStyle CssClass="GridHeader" />
                            <SelectedRowStyle CssClass="GridSelected" />
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                        </asp:GridView>
                    </tr>
                </table>
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

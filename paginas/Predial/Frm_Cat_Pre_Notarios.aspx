<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Notarios.aspx.cs" Inherits="paginas_predial_Frm_Cat_Pre_Notarios"
    Title="Catalogo de Notarios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScptM_Contribuyentes" runat="server" />
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
                            Catálogo de Notarios
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
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="130px" ToolTip="Por Nombre o No. de Notaría"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar" runat="server" CausesValidation="false" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                ToolTip="Buscar" OnClick="Btn_Buscar_Click" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                WatermarkCssClass="watermarked" WatermarkText="&lt;Nombre|No. Notaría&gt;">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
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
                                <asp:HiddenField ID="Hdf_Notario" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Notario_ID" runat="server" Text="Notario ID" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 27%">
                                <asp:TextBox ID="Txt_Notario_ID" runat="server" Width="98%" MaxLength="10" Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Estatus" runat="server" CssClass="estilo_fuente" Text="* Estatus"></asp:Label>
                            </td>
                            <td style="width: 27%">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                    <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE" />
                                    <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                    <asp:ListItem Text="OBSOLETO" Value="OBSOLETO" />
                                </asp:DropDownList>
                            </td>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Numero_Notaria" runat="server" Text="* Número de notaría" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 27%">
                                <asp:TextBox ID="Txt_Numero_Notaria" runat="server" MaxLength="20" Width="98%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Apellido_Paterno" runat="server" Text="* Apellido Paterno" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 27%">
                                <asp:TextBox ID="Txt_Apellido_Paterno" runat="server" Width="98%" MaxLength="25"
                                    Style="text-transform: uppercase"></asp:TextBox>
                            </td>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Apellido_Materno" runat="server" Text="Apellido Materno" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 27%">
                                <asp:TextBox ID="Txt_Apellido_Materno" runat="server" Width="98%" MaxLength="25"
                                    Style="text-transform: uppercase"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Nombre" runat="server" Text="* Nombre" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3" style="width: 35%">
                                <asp:TextBox ID="Txt_Nombre" runat="server" Width="98%" MaxLength="45" Style="text-transform: uppercase"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_RFC" runat="server" CssClass="estilo_fuente" Text="RFC"></asp:Label>
                            </td>
                            <td style="width: 27%; text-align: left;">
                                <asp:TextBox ID="Txt_RFC" runat="server" MaxLength="13" Width="98%" Style="text-transform: uppercase"></asp:TextBox>
                            </td>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_CURP" runat="server" CssClass="estilo_fuente" Text="CURP"></asp:Label>
                            </td>
                            <td style="width: 27%">
                                <asp:TextBox ID="Txt_CURP" runat="server" MaxLength="18" Width="98%" Style="text-transform: uppercase"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Sexo" runat="server" Text="Sexo" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 27%">
                                <asp:DropDownList ID="Cmb_Sexo" runat="server" Width="100%">
                                    <asp:ListItem Text="<SELECCIONE>" Value="" />
                                    <asp:ListItem Text="FEMENINO" Value="FEMENINO" />
                                    <asp:ListItem Text="MASCULINO" Value="MASCULINO" />
                                </asp:DropDownList>
                            </td>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Estado" runat="server" Text="Estado" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 27%">
                                <asp:TextBox ID="Txt_Estado" runat="server" MaxLength="20" Width="98%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Ciudad" runat="server" CssClass="estilo_fuente" Text="Ciudad"></asp:Label>
                            </td>
                            <td style="width: 27%; margin-left: 280px;">
                                <asp:TextBox ID="Txt_Ciudad" runat="server" MaxLength="50" Width="98%"></asp:TextBox>
                            </td>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Colonia" runat="server" CssClass="estilo_fuente" Text="Colonia"></asp:Label>
                            </td>
                            <td style="width: 27%">
                                <asp:TextBox ID="Txt_Colonia" runat="server" MaxLength="100" Width="98%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Calle" runat="server" CssClass="estilo_fuente" Text="Calle"></asp:Label>
                            </td>
                            <td style="width: 27%; margin-left: 40px;">
                                <asp:TextBox ID="Txt_Calle" runat="server" MaxLength="100" Width="98%"></asp:TextBox>
                            </td>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_CP" runat="server" CssClass="estilo_fuente" Text="C.P."></asp:Label>
                            </td>
                            <td style="width: 27%">
                                <asp:TextBox ID="Txt_Codigo_Postal" runat="server" MaxLength="5" Width="98%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Codigo_Postal" runat="server" TargetControlID="Txt_Codigo_Postal"
                                    FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%; text-align: left;">
                                <asp:Label ID="Lbl_No_Exterior" runat="server" CssClass="estilo_fuente" Text="No. Exterior"></asp:Label>
                            </td>
                            <td style="width: 27%">
                                <asp:TextBox ID="Txt_No_Exterior" runat="server" MaxLength="5" Width="98%"></asp:TextBox>
                            </td>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Ciudad2" runat="server" CssClass="estilo_fuente" Text="No. Interior"></asp:Label>
                            </td>
                            <td style="width: 27%">
                                <asp:TextBox ID="Txt_No_Interior" runat="server" MaxLength="5" Width="98%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Telefono" runat="server" CssClass="estilo_fuente" Text="Teléfono"></asp:Label>
                            </td>
                            <td style="width: 27%">
                                <asp:TextBox ID="Txt_Telefono" runat="server" MaxLength="20" Width="98%"></asp:TextBox>
                            </td>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Fax" runat="server" CssClass="estilo_fuente" Text="FAX"></asp:Label>
                            </td>
                            <td style="width: 27%">
                                <asp:TextBox ID="Txt_Fax" runat="server" MaxLength="20" Width="98%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Celular" runat="server" CssClass="estilo_fuente" Text="Celular"></asp:Label>
                            </td>
                            <td style="width: 27%">
                                <asp:TextBox ID="Txt_Celular" runat="server" MaxLength="20" Width="98%"></asp:TextBox>
                            </td>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_EMail" runat="server" CssClass="estilo_fuente" Text="E-Mail"></asp:Label>
                            </td>
                            <td style="width: 27%">
                                <asp:TextBox ID="Txt_EMail" runat="server" MaxLength="100" Width="98%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:GridView ID="Grid_Notarios" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                        AllowPaging="True" PageSize="10" Width="96%" OnSelectedIndexChanged="Grid_Notarios_SelectedIndexChanged"
                        GridLines="None" OnPageIndexChanging="Grid_Notarios_PageIndexChanging">
                        <RowStyle CssClass="GridItem" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png" />
                            <asp:BoundField DataField="NOTARIO_ID" HeaderText="Notario ID" Visible="false" />
                            <asp:BoundField DataField="NUMERO_NOTARIA" HeaderText="Número de Notaría" ItemStyle-Width="11%" />
                            <asp:BoundField DataField="RFC" HeaderText="RFC" SortExpression="rfc" ItemStyle-Width="18%" />
                            <asp:BoundField DataField="NOMBRE_COMPLETO" HeaderText="NOMBRE COMPLETO" SortExpression="nombre" />
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

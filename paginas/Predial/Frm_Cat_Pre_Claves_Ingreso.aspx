<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Claves_Ingreso.aspx.cs" Inherits="paginas_Predial_Frm_Cat_Pre_Claves_Ingreso" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel_Claves_Ingreso" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Claves_Ingreso" runat="server" AssociatedUpdatePanelID="Upd_Panel_Claves_Ingreso"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="General" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table id="Tbl_Comandos" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                    <tr>
                        <td class="label_titulo" colspan="2">
                            Catálogo de Claves de Ingreso
                        </td>
                    </tr>
                    <tr>
                        <div id="Div_Contenedor_Msj_Error" runat="server">
                            <td colspan="2">
                                <asp:Image ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" />
                                <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                <br />
                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" TabIndex="0" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            </td>
                        </div>
                    </tr>
                    <tr class="barra_busqueda">
                        <td style="width: 50%">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                CssClass="Img_Button" OnClick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                CssClass="Img_Button" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                CssClass="Img_Button" OnClientClick="return confirm('¿Esta seguro de Eliminar el presente registro?');"
                                OnClick="Btn_Eliminar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                CssClass="Img_Button" OnClick="Btn_Salir_Click" />
                        </td>
                        <td align="right" style="width: 50%">
                            Búsqueda:
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180" ToolTip="Buscar" TabIndex="1"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                TabIndex="2" OnClick="Btn_Buscar_Click" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkText="<Buscar por Clave>"
                                TargetControlID="Txt_Busqueda" WatermarkCssClass="watermarked">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
                <cc1:TabContainer ID="Tab_Claves_Ingreso" runat="server" Width="98%" ActiveTabIndex="0">
                    <cc1:TabPanel ID="Tab_Claves" runat="server" HeaderText="Claves de Ingreso" ActiveTabIndex="1">
                        <HeaderTemplate>
                            Claves de Ingreso</HeaderTemplate>
                        <ContentTemplate>
                            <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                                <tr>
                                    <td style="width: 18%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 32%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 18%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 32%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="Txt_Clave_Ingreso_ID" runat="server" ReadOnly="True" Visible="False"
                                            Width="92%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        Estatus
                                    </td>
                                    <td style="width: 32%">
                                        <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="94%">
                                            <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE"></asp:ListItem>
                                            <asp:ListItem Text="VIGENTE" Value="VIGENTE"></asp:ListItem>
                                            <asp:ListItem Text="BAJA" Value="BAJA"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 18%; text-align: right;">
                                        Rama
                                    </td>
                                    <td style="width: 32%">
                                        <asp:DropDownList ID="Cmb_Rama" Width="94%" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Cmb_Rama_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        Grupo
                                    </td>
                                    <td style="width: 32%">
                                        <asp:DropDownList ID="Cmb_Grupo" runat="server" Width="94%" AutoPostBack="True" OnSelectedIndexChanged="Cmb_Grupo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 18%; text-align: right;">
                                        Clave Ingreso
                                    </td>
                                    <td style="width: 32%">
                                        <asp:TextBox ID="Txt_Clave" runat="server" MaxLength="8" Width="92%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        Unidad Responsable
                                    </td>
                                    <td style="width: 32%">
                                        <asp:DropDownList ID="Cmb_Unidad_Responsable" runat="server" Width="94%">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 18%; text-align: right">
                                        Cuenta Contable
                                    </td>
                                    <td style="width: 32%">
                                        <asp:DropDownList ID="Cmb_Cuenta_Contable" runat="server" Width="94%">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        Descripción
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Descripcion" runat="server" Width="97%" Height="60px" TextMode="MultiLine"></asp:TextBox>
                                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkCssClass="watermarked"
                                            WatermarkText="Límite de Caractes 250" TargetControlID="Txt_Descripcion" Enabled="True">
                                        </cc1:TextBoxWatermarkExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                            TargetControlID="Txt_Descripcion" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        Fundamento
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Fundamento" runat="server" Width="97%" Height="60px" TextMode="MultiLine"></asp:TextBox>
                                        <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Comentarios" runat="server" WatermarkCssClass="watermarked"
                                            WatermarkText="Límite de Caractes 250" TargetControlID="Txt_Fundamento" Enabled="True">
                                        </cc1:TextBoxWatermarkExtender>
                                        <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" runat="server"
                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="Txt_Fundamento"
                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="Tab_Grupos_Movimientos" runat="server" HeaderText="Grupos de Movimientos"
                        ActiveTabIndex="2">
                        <HeaderTemplate>
                            Costos de Claves</HeaderTemplate>
                        <ContentTemplate>
                            <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                                <tr>
                                    <td style="width: 145px" colspan="3">
                                        Año &nbsp
                                        <asp:TextBox ID="Txt_Anio" runat="server" Width="130px" Visible="true" MaxLength="4"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Ftb_Txt_Anio" runat="server" TargetControlID="Txt_Anio"
                                            FilterType="Numbers" />
                                        &nbsp &nbsp &nbsp Importe &nbsp
                                        <asp:TextBox ID="Txt_Costo" runat="server" Width="130px" Visible="true" MaxLength="16"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Ftb_Txt_Costo" runat="server" TargetControlID="Txt_Costo"
                                            FilterType="Numbers, Custom" ValidChars=".," />
                                        &nbsp &nbsp &nbsp &nbsp
                                        <asp:ImageButton ID="Img_Movimiento" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                            OnClick="Btn_Movimiento_Click" />
                                    </td>
                                </tr>
                                <asp:HiddenField ID="Hdf_Costo_Clave_Id" runat="server" />
                                <tr>
                                    &nbsp &nbsp
                                </tr>
                                <tr>
                                    <td colspan="2" align="center" style="width: 300px">
                                        <asp:GridView ID="Grid_Movimientos" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                            GridLines="None" PageSize="5" Style="white-space: normal" Width="96%" OnSelectedIndexChanged="Grid_Movimientos_SelectedIndexChanged"
                                            OnPageIndexChanging="Grid_Movimientos_PageIndexChanging" OnRowCommand="Borrar_Registro"
                                            EnableModelValidation="True">
                                            <RowStyle CssClass="GridItem" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="5%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="COSTO_CLAVE_ID" HeaderText="Costo Clave ID" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ANIO" HeaderText="Anio">
                                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="COSTO" HeaderText="Costo" DataFormatString="{0:c2}">
                                                    <HeaderStyle HorizontalAlign="Center" Width="80%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="80%" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Btn_Eliminar_Movimiento" runat="server" Height="20px" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png"
                                                            TabIndex="10" OnClientClick="return confirm('Se eliminará de forma permanente, ¿Esta seguro que desea eliminar el registro?')"
                                                            ToolTip="Borrar Costo de Clave" Width="20px" CommandName="Erase_Movimiento" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="GridHeader" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 77%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 5%">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="Tab_Otros_Pagos" runat="server" HeaderText="" ActiveTabIndex="3">
                        <HeaderTemplate>
                            Claves de Otros Pagos</HeaderTemplate>
                        <ContentTemplate>
                            <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                                <tr>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="Txt_Detalle_ID_2" runat="server" Width="92%" Visible="False"></asp:TextBox>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="Txt_Descripcion_OP" runat="server" Width="92%" Visible="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        Clave de Otros Pagos
                                    </td>
                                    <td style="width: 77%">
                                        <asp:DropDownList ID="Cmb_Otros_Pagos" runat="server" Width="94%" AutoPostBack="True"
                                            OnSelectedIndexChanged="Llenar_Campos_Otro_Pago">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5%">
                                        <asp:ImageButton ID="Img_Otros_Pagos" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                            OnClick="Btn_Otros_Pagos_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="Txt_Clave_Otro_Pago" runat="server" Width="92%" Visible="False"></asp:TextBox>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="Txt_Descripcion_Otros_Pagos" runat="server" Width="92%" Visible="False"></asp:TextBox>
                                    </td>
                                    <td style="width: 5%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:GridView ID="Grid_Otros_Pagos" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                            GridLines="None" PageSize="5" Style="white-space: normal" Width="96%" OnSelectedIndexChanged="Grid_Otros_Pagos_SelectedIndexChanged"
                                            OnPageIndexChanging="Grid_Otros_Pagos_PageIndexChanging" OnRowCommand="Borrar_Registro"
                                            EnableModelValidation="True">
                                            <RowStyle CssClass="GridItem" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="5%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="CLAVE_INGRESO_ID" HeaderText="Clave Ingreso ID">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CLAVE" HeaderText="Clave">
                                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción">
                                                    <HeaderStyle HorizontalAlign="Center" Width="80%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="80%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CLAVE_OTRO_PAGO" HeaderText="Clave Otro Pago">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Btn_Eliminar_Otro_Pago" runat="server" Height="20px" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png"
                                                            TabIndex="10" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro?')"
                                                            ToolTip="Borrar Otro Pago" Width="20px" CommandName="Erase_Otro_Pago" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="GridHeader" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 77%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 5%">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="Tab_Documentos" runat="server" HeaderText="Claves de Documentos"
                        ActiveTabIndex="4">
                        <HeaderTemplate>
                            Tipos de Constancias</HeaderTemplate>
                        <ContentTemplate>
                            <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                                <tr>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="Txt_Detalle_ID_3" runat="server" Width="92%" Visible="False"></asp:TextBox>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="Txt_Descripcion_Doc" runat="server" Width="92%" Visible="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        Tipo de Constancia
                                    </td>
                                    <td style="width: 77%">
                                        <asp:DropDownList ID="Cmb_Documentos" runat="server" Width="94%" AutoPostBack="True"
                                            OnSelectedIndexChanged="Llenar_Campos_Documento">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5%">
                                        <asp:ImageButton ID="Img_Documentos" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                            OnClick="Btn_Documentos_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="Txt_Clave_Documento" runat="server" Width="92%" Visible="False"></asp:TextBox>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="Txt_Descripcion_Documento" runat="server" Width="92%" Visible="False"></asp:TextBox>
                                    </td>
                                    <td style="width: 5%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:GridView ID="Grid_Documentos" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                            GridLines="None" PageSize="5" Style="white-space: normal" Width="96%" OnSelectedIndexChanged="Grid_Documentos_SelectedIndexChanged"
                                            OnPageIndexChanging="Grid_Documentos_PageIndexChanging" OnRowCommand="Borrar_Registro"
                                            EnableModelValidation="True">
                                            <RowStyle CssClass="GridItem" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="5%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="CLAVE_INGRESO_ID" HeaderText="Clave Ingreso ID">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CLAVE" HeaderText="Clave">
                                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción">
                                                    <HeaderStyle HorizontalAlign="Center" Width="80%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="80%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CLAVE_DOCUMENTO" HeaderText="Clave Documento">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Btn_Eliminar_Documento" runat="server" Height="20px" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png"
                                                            TabIndex="10" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro?')"
                                                            ToolTip="Borrar Documento" Width="20px" CommandName="Erase_Documento" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="GridHeader" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 77%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 5%">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="Tab_Gastos_Ejecucion" runat="server" HeaderText="Gastos de ejecución"
                        ActiveTabIndex="5">
                        <HeaderTemplate>
                            Gastos de ejecución</HeaderTemplate>
                        <ContentTemplate>
                            <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                                <tr>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="Txt_Detalle_ID_4" runat="server" Width="92%" Visible="False"></asp:TextBox>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="Txt_Descripcion_Gas" runat="server" Width="92%" Visible="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        Gastos de ejecución
                                    </td>
                                    <td style="width: 77%">
                                        <asp:DropDownList ID="Cmb_Gastos_Ejecucion" runat="server" Width="94%" AutoPostBack="True"
                                            OnSelectedIndexChanged="Llenar_Campos_Gasto_Ejecucion">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5%">
                                        <asp:ImageButton ID="Img_Gastos_Ejecucion" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                            OnClick="Btn_Gastos_Ejecucion" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="Txt_Clave_Gastos" runat="server" Width="92%" Visible="False"></asp:TextBox>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="Txt_Descripcion_Gastos" runat="server" Width="92%" Visible="False"></asp:TextBox>
                                    </td>
                                    <td style="width: 5%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:GridView ID="Grid_Gastos_Ejecucion" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                            GridLines="None" PageSize="5" Style="white-space: normal" Width="96%" OnSelectedIndexChanged="Grid_Gastos_Ejecucion_SelectedIndexChanged"
                                            OnPageIndexChanging="Grid_Gastos_Ejecucion_PageIndexChanging" OnRowCommand="Borrar_Registro">
                                            <RowStyle CssClass="GridItem" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="5%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="CLAVE_INGRESO_ID" HeaderText="Clave Ingreso ID">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="GASTO_EJECUCION_ID" HeaderText="Gasto Ejecucion ID">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CLAVE" HeaderText="Clave">
                                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción">
                                                    <HeaderStyle HorizontalAlign="Center" Width="80%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="80%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CLAVE_GASTOS" HeaderText="Clave Gastos">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Btn_Eliminar_Gasto" runat="server" Height="20px" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png"
                                                            TabIndex="10" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro?')"
                                                            ToolTip="Borrar Gasto" Width="20px" CommandName="Erase_Gasto" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="GridHeader" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 77%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 5%">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="Tab_Predial_Traslado" runat="server" HeaderText="Gastos de ejecución"
                        ActiveTabIndex="6">
                        <HeaderTemplate>
                            Predial - Traslado</HeaderTemplate>
                        <ContentTemplate>
                            <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                                <tr>
                                    <td style="width: 15%" />
                                    <td style="width: 35%" />
                                    <td style="width: 15%" />
                                    <td style="width: 35%" />
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        Tipo:
                                    </td>
                                    <td style="width: 35%">
                                        <asp:DropDownList ID="Cmb_Tipo" runat="server" Width="70%">
                                            <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE"></asp:ListItem>
                                            <asp:ListItem Text="TRASLADO" Value="TRASLADO"></asp:ListItem>
                                            <asp:ListItem Text="PREDIAL" Value="PREDIAL"></asp:ListItem>
                                            <asp:ListItem Text="DIVISION" Value="DIVISION"></asp:ListItem>
                                            <asp:ListItem Text="FRACCIONAMIENTOS" Value="FRACCIONAMIENTOS"></asp:ListItem>
                                            <asp:ListItem Text="DERECHOS DE SUPERVISION" Value="DERECHOS DE SUPERVISION"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 16%">
                                        Tipo Predial Traslado:
                                    </td>
                                    <td style="width: 35%">
                                        <asp:DropDownList ID="Cmb_Tipo_Predial_Traslado" runat="server" Width="70%">
                                            <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE"></asp:ListItem>
                                            <asp:ListItem Text="IMPUESTO" Value="IMPUESTO"></asp:ListItem>
                                            <asp:ListItem Text="IMPUESTO URBANO" Value="IMPUESTO_URBANO"></asp:ListItem>
                                            <asp:ListItem Text="IMPUESTO RUSTICO" Value="IMPUESTO_RUSTICO"></asp:ListItem>
                                            <asp:ListItem Text="REZAGO URBANO" Value="REZAGO_URBANO"></asp:ListItem>
                                            <asp:ListItem Text="REZAGO RUSTICO" Value="REZAGO_RUSTICO"></asp:ListItem>
                                            <asp:ListItem Text="RECARGOS ORDINARIOS" Value="RECARGOS_ORDINARIOS"></asp:ListItem>
                                            <asp:ListItem Text="RECARGOS MORATORIOS" Value="RECARGOS_MORATORIOS"></asp:ListItem>
                                            <asp:ListItem Text="HONORARIOS" Value="HONORARIOS"></asp:ListItem>
                                            <asp:ListItem Text="MULTAS" Value="MULTAS"></asp:ListItem>
                                            <asp:ListItem Text="DESCUENTO PP URBANO" Value="DESCUENTO_PP_URBANO"></asp:ListItem>
                                            <asp:ListItem Text="DESCUENTO PP RUSTICO" Value="DESCUENTO_PP_RUSTICO"></asp:ListItem>
                                            <asp:ListItem Text="DESCUENTO RECARGOS" Value="DESCUENTO_RECARGOS"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5%">
                                        <asp:ImageButton ID="Img_Predial_Traslado" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                            OnClick="Btn_Predial_Traslado" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 35%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 15%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 35%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5" align="center">
                                        <asp:GridView ID="Grid_Predial_Traslado" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                            GridLines="None" PageSize="5" Style="white-space: normal" Width="96%" OnPageIndexChanging="Grid_Predial_Traslado_PageIndexChanging"
                                            OnSelectedIndexChanged="Grid_Predial_Traslado_SelectedIndexChanged" OnRowCommand="Borrar_Registro">
                                            <RowStyle CssClass="GridItem" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="5%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="CLAVE_INGRESO_ID" HeaderText="Clave Ingreso">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TIPO" HeaderText="Tipo">
                                                    <HeaderStyle HorizontalAlign="Center" Width="50%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="50%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TIPO_PREDIAL_TRASLADO" HeaderText="Tipo Predial Traslado">
                                                    <HeaderStyle HorizontalAlign="Center" Width="50%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="50%" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Btn_Eliminar_Predial_Traslado" runat="server" Height="20px"
                                                            ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" TabIndex="10" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro?')"
                                                            ToolTip="Borrar Predial - Traslado" Width="20px" CommandName="Erase_Predial"
                                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="GridHeader" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 35%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 15%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 35%">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
                <table id="Tbl_Claves_Ingreso" border="0" cellspacing="0" class="estilo_fuente" style="width: 100%;">
                    <tr>
                        <td align="center">
                            <asp:GridView ID="Grid_Claves_Ingreso" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                GridLines="None" PageSize="5" Style="white-space: normal" Width="96%" OnSelectedIndexChanged="Grid_Claves_Ingreso_SelectedIndexChanged"
                                OnPageIndexChanging="Grid_Claves_Ingreso_PageIndexChanging">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="CLAVE_INGRESO_ID" HeaderText="Clave Ingreso ID">
                                        <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CLAVE" HeaderText="Clave">
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE_RAMA" HeaderText="Nombre Rama">
                                        <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RAMA_ID" HeaderText="Rama ID">
                                        <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RAMA" HeaderText="Rama">
                                        <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE_GRUPO" HeaderText="Nombre Grupo">
                                        <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="GRUPO_ID" HeaderText="Grupo ID">
                                        <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="GRUPO" HeaderText="Grupo">
                                        <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion">
                                        <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FUNDAMENTO" HeaderText="Fundamentos">
                                        <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CUENTA_CONTABLE_ID" HeaderText="Cuenta Contable">
                                        <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DEPENDENCIA_ID" HeaderText="Dependencia">
                                        <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

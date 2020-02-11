<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Recargos_Traslado.aspx.cs" Inherits="paginas_Predial_Frm_Cat_Pre_Recargos_Translado" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel_Padron_Predios" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Padrones" runat="server" AssociatedUpdatePanelID="Upd_Panel_Padron_Predios" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
            </asp:UpdateProgress>
                <div id="General" style="background-color:#ffffff; width:100%; height:100%;">
                    <table id="Tbl_Comandos" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                                <tr>
                                    <td class="label_titulo" colspan="2">
                                        Catálogo de Recargos de Traslado</td>
                                </tr>
                                <tr>
                                    <div id="Div_Contenedor_error" runat="server">
                                    <td colspan="2">
                                        <asp:Image ID="Img_Error" runat="server" 
                                            ImageUrl = "../imagenes/paginas/sias_warning.png"/>
                                        <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                        <br />
                                        <asp:Label ID="Lbl_Error" runat="server" Text="" TabIndex="0" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                                    </td>
                                    </div>
                                </tr>
                                <tr class="barra_busqueda">
                                    <td style="width:50%">
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                        CssClass="Img_Button" onclick="Btn_Nuevo_Click" TabIndex="1"/>
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                        CssClass="Img_Button" onclick="Btn_Modificar_Click" TabIndex="2"/>
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                        CssClass="Img_Button" onclick="Btn_Salir_Click" TabIndex="3"/>
                                    </td>
                                    <td align="right" style="width:50%">
                                        Búsqueda:
                                        <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"
                                        ToolTip="Buscar" TabIndex="4" ></asp:TextBox>
                                        <asp:ImageButton ID="Btn_Busqueda" runat="server" 
                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                            TabIndex="5" onclick="Btn_Buscar_Recargo_Click"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                            WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda"/>
                                    </td>
                                </tr>
                            </table>
                    <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                        <tr>
                            <td style="width:18%">&nbsp;</td>
                            <td style="width:32%">&nbsp;</td>
                            <td style="width:18%">&nbsp;</td>
                            <td style="width:32%">&nbsp;</td>
                        </tr>
                        <tr>
                        <td style="width:18%">
                                *Año</td>
                            <td style="width:32%">
                                <asp:TextBox ID="Txt_Anio" runat="server" Width="92%" Text="" TabIndex="6" MaxLength="4"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Txt_Anio_FilteredTextBoxExtender" FilterType="Numbers"
                                    runat="server" Enabled="True" TargetControlID="Txt_Anio" ValidChars="1234567890">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="width:18%">
                                *Porcentaje</td>
                            <td style="width:32%">
                                <asp:TextBox ID="Txt_Cuota" runat="server" Text="" Width="92%" TabIndex="7" MaxLength="6"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Txt_Cuota_FilteredTextBoxExtender" 
                                    runat="server" Enabled="True" TargetControlID="Txt_Cuota" FilterType="Numbers, Custom" ValidChars="1234567890.">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                                <asp:TextBox ID="Txt_Id" runat="server" Width="92%" Text="" Visible="false"></asp:TextBox>
                            </td>
                            <td style="width:18%"></td>
                            <td style="width:32%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width:18%">
                                &nbsp;</td>
                            <td colspan="3">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <asp:GridView ID="Grid_Recargos_Translado" runat="server" AllowPaging="true" 
                                    AutoGenerateColumns="False" CssClass="GridView_1" 
                                    EmptyDataText="&quot;No se encontraron registros&quot;" GridLines="none" 
                                    PageSize="5" Style="white-space:normal" Width="96%" 
                                    onpageindexchanging="Grid_Recargos_PageIndexChanging" 
                                    onselectedindexchanged="Grid_Recargos_SelectedIndexChanged">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="RECARGO_TRASLADO_ID" HeaderText="Id" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ANIO" HeaderText="Año">
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TASA" HeaderText="Cuota" DataFormatString="{0:#,##0.00}">
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                                                <tr>
                            <td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                                &nbsp;</td>
                            <td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                                &nbsp;</td>
                            <td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                                &nbsp;</td>
                        </tr>
                    </table>
                    
                </div>
            </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
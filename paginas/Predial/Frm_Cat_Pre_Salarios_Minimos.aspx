<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Salarios_Minimos.aspx.cs" Inherits="paginas_Predial_Cat_Pre_Salarios_Minimos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Contenido" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Catálogo de Salarios Mínimos
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
               </table>
               <table width="98%"  border="0" cellspacing="0">
                <tr align="center">
                    <td>
                        <div style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td align="left" style="width:59%;">
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                            CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                            onclick="Btn_Nuevo_Click" TabIndex="1"/>
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                            CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                            onclick="Btn_Modificar_Click" TabIndex="2"/>
                                        <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                            CssClass="Img_Button" 
                                            OnClientClick="return confirm('¿Desea eliminar el presente registro?');" 
                                            onclick="Btn_Eliminar_Click" ToolTip="Eliminar" TabIndex="3"/>
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                            CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            onclick="Btn_Salir_Click" TabIndex="4"/>
                                    </td>
                                    <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                <td style="width:55%;">
                                                    <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5"  ToolTip = "Buscar" Width="180px"/>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                        WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" 
                                                        runat="server" TargetControlID="Txt_Busqueda" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                </td>
                                                <td style="vertical-align:middle;width:5%;" >
                                                    <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6"
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                                        onclick="Btn_Buscar_Salarios_Click" />
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
                        <td style="text-align:left;width:20%;">
                            *Año
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Anio" runat="server" Width="96.4%" MaxLength="4" TabIndex="7"/>
                            <cc1:FilteredTextBoxExtender ID="Txt_Anio_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" TargetControlID="Txt_Anio" FilterType="Numbers" ValidChars="1234567890">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            *Estatus
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%" TabIndex="8">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                <asp:ListItem Text="BAJA" Value="BAJA" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Monto
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Monto" runat="server" Width="96.4%" MaxLength="9" />
                            <cc1:FilteredTextBoxExtender ID="Txt_Monto_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" TargetControlID="Txt_Monto" FilterType="Numbers, Custom" ValidChars="1234567890.">
                            </cc1:FilteredTextBoxExtender>
                            
                        </td>
                        <td style="text-align:right;width:20%;">
                            &nbsp;</td>
                        <td style="text-align:left;width:20%;text-align:right;" colspan="2">
                            &nbsp;</td>
                    </tr>
                    <tr>
                    <td><asp:TextBox ID="Txt_Id" Visible="false" runat="server" Width="96.4%"/></td>
                    </tr>
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_Salarios" runat="server" AllowPaging="True" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="5" Width="100%"
                                AllowSorting="True" HeaderStyle-CssClass="tblHead" 
                                style="white-space:normal;" 
                                onpageindexchanging="Grid_Salarios_PageIndexChanging" 
                                onselectedindexchanged="Grid_Salarios_SelectedIndexChanged">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="3.5%" HorizontalAlign="Center"/>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="SALARIO_MINIMO_ID" HeaderText="Id" Visible="false"/>
                                    <asp:BoundField DataField="ANIO" HeaderText="Año" />
                                    <asp:BoundField DataField="MONTO" HeaderText="Salario mínimo" />
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" HeaderStyle-Width="15%" />
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <HeaderStyle CssClass="tblHead" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                            <br />
                        </td>
                    </tr>
                </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


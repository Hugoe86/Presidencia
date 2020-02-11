<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Tipos_Documento.aspx.cs" Inherits="paginas_Predial_Frm_Cat_Pre_Tipos_Documento" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div style="width: 95%;">
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                            
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">Cat&aacute;logo de Tipos de Documentos</td>
                    </tr>
                    <tr align="left">
                        <td colspan="4" >
                            <asp:Image ID="Img_Error" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" /> &nbsp; 
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" 
                                ForeColor="#990000"></asp:Label>                        
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td colspan="4" align="left" valign="middle">
                           <table width = "100%">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                        CssClass="Img_Button" ToolTip="Nuevo" TabIndex="1" 
                                        onclick="Btn_Nuevo_Click" />
                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                        CssClass="Img_Button" ToolTip="Modificar" TabIndex="2" 
                                        onclick="Btn_Modificar_Click" />
                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                        CssClass="Img_Button" ToolTip="Inactivar" TabIndex="3" 
                                        
                                        OnClientClick="return confirm('S�lo se cambiar� el estatus a BAJA. �Confirma que desea proceder?');" onclick="Btn_Eliminar_Click" 
                                        />
                                    <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" 
                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" 
                                        TabIndex="4" onclick="Btn_Salir_Click" />
                                </td>
                                <td align = "right">
                                    B&uacute;squeda:
                                    <asp:TextBox ID="Txt_Busqueda" runat="server" TabIndex="5" MaxLength="100" Width="180"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Rol_ID" runat="server" WatermarkCssClass="watermarked"
                                        WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                    <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" TabIndex="5" 
                                        onclick="Btn_Buscar_Click" />
                                </td>
                            </tr>
                           </table>
                        </td>
                    </tr>
                    </table>
                    
                    <table width="98%" class="estilo_fuente">
                    
                    <tr>
                        <td style="text-align:left;width:20%;">Documento_ID</td>
                        <td style="text-align:left;width:30%;"><asp:TextBox ID="Txt_Documento_ID" runat="server" Enabled="false" 
                        ReadOnly="true" Width="97%" ></asp:TextBox></td>
                        
                        <td style="text-align:right;width:20%;">*Estatus</td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%" TabIndex="6">
                                <asp:ListItem Value="VIGENTE" Text = "VIGENTE"></asp:ListItem>
                                <asp:ListItem Value="BAJA" Text = "BAJA"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="text-align:left;width:20%;">*Nombre</td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Nombre" runat="server" MaxLength="100" Width="99%" TabIndex="7" style="text-transform:uppercase"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre" runat="server" TargetControlID="Txt_Nombre" 
                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="������������. "></cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>Descripci�n</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Descripcion" runat="server" MaxLength="250" Width="99%" TextMode="MultiLine" TabIndex="8" style="text-transform:uppercase"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Descripcion" runat="server" TargetControlID="Txt_Descripcion" 
                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="������������., "></cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="TWM_Txt_Descripcion" runat="server" TargetControlID="Txt_Descripcion" 
                            WatermarkCssClass="watermarked" WatermarkText="<L&iacute;mite de Caracteres 250>" />
                        </td>
                    </tr>
                    
                    <tr><td colspan="4"><hr /></td></tr>
                    
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>
                    <tr align="center">
                        <td colspan="4" align="center">
                            <asp:GridView ID="Grid_Tipos_Documento" runat="server" AutoGenerateColumns="False" 
                                CssClass="GridView_1" AllowPaging="True" PageSize="5" GridLines="None" Style="white-space:normal"
                                Width="100%" onpageindexchanging="Grid_Tipos_Documento_PageIndexChanging" 
                                onselectedindexchanged="Grid_Tipos_Documento_SelectedIndexChanged" >
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Documento_ID" HeaderText="ID" Visible="True">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Nombre_Documento" HeaderText="Nombre" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripci�n" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                        <ItemStyle HorizontalAlign="Left" Width="50%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

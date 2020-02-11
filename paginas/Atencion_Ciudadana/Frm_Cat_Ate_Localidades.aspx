<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Ate_Localidades.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Cat_Localidades" Title="Untitled Page" %>
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
                        <div id="progressBackgroundFilter" class="progressTemplate">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                            
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">Catálogo de Localidades</td>
                    </tr>
                    <tr align="left">
                        <td colspan="4" >
                            <asp:Image ID="Img_Warning" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />
                            <asp:Label ID="Lbl_Informacion" runat="server" 
                                ForeColor="#990000"></asp:Label>                        
                        </td>
                    </tr>                    
                    <tr class="barra_busqueda" align="right">
                        <td colspan="4" align="left" valign="middle">
                           <table width = "100%">
                                <tr>
                                <td>
                                <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                    CssClass="Img_Button" onclick="Btn_Nuevo_Click" ToolTip="Nuevo" />
                                <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                    CssClass="Img_Button" onclick="Btn_Modificar_Click" ToolTip="Modificar" />
                                <asp:ImageButton ID="Bnt_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                    CssClass="Img_Button" onclick="Bnt_Eliminar_Click" ToolTip="Eliminar" 
                                        OnClientClick="return confirm('¿Está seguro de eliminar la localidad seleccionada?');"/>
                                <asp:ImageButton ID="Btn_Salir" runat="server" 
                                    CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click" ToolTip="Inicio"  />
                                    </td>
                                 <td align = "right">
                                    Búsqueda por:
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Rol_ID" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese nombre de localidad>" TargetControlID="Txt_Busqueda" />
                            <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" TabIndex="1" 
                                            onclick="Btn_Buscar_Click" />
                                  </td>
                                    </tr>
                                    </table>
                           
                        </td>
                        
                    </tr>
                    <tr>
                        <td>Localidad ID</td>
                        <td>
                            <asp:TextBox ID="Txt_ID" runat="server" ReadOnly="True" Width="150px" Enabled="false"></asp:TextBox></td>
                        
                    </tr>
                    <tr>
                        <td>*Nombre</td>
                        <td>
                            <asp:TextBox ID="Txt_Nombre" runat="server" MaxLength="100" TabIndex="1" 
                                Width="600px" Enabled="False"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Txt_Nombre_Dependencia_FilteredTextBoxExtender" 
                                runat="server" TargetControlID="Txt_Nombre" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>*Descripción</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Descripcion" runat="server" TabIndex="4" MaxLength="250"
                                TextMode="MultiLine" Width="605px" Enabled="False"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Límite de Caracteress 120>" 
                                TargetControlID="Txt_Descripcion" />
                            <cc1:FilteredTextBoxExtender ID="Txt_Descripcion_FilteredTextBoxExtender" 
                                runat="server" TargetControlID="Txt_Descripcion" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_Localidades" runat="server" AutoGenerateColumns="False" 
                                CssClass="GridView_1" AllowPaging="True" PageSize="5" 
                                GridLines="None" onpageindexchanging="Grid_Localidades_PageIndexChanging" 
                                onselectedindexchanged="Grid_Localidades_SelectedIndexChanged" Width = "98%">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Localidad_ID" HeaderText="Localidad ID" 
                                        Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre de la localidad" 
                                        Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" Visible="True">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                        <ItemStyle HorizontalAlign="Left" Width="50%" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />                                
                                <AlternatingRowStyle CssClass="GridAltItem" />                                
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr style ="height:"50px">
                        <td colspan = "3"></td>
                    </tr>
                    <%--<tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="Btn_Nuevo" runat="server" Text="Nuevo" Height="26px" 
                                CssClass="button" CausesValidation="False" TabIndex="5" 
                                onclick="Btn_Nuevo_Click1" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Btn_Modificar" runat="server" Text="Modificar" Height="26px" 
                                CssClass="button" CausesValidation="False" TabIndex="6" 
                                onclick="Btn_Modificar_Click1" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Btn_Eliminar" runat="server" Text="Eliminar" Height="26px" 
                                CssClass="button" CausesValidation="False" TabIndex="7" 
                                onclick="Btn_Eliminar_Click1" 
                                OnClientClick="return confirm('¿Esta seguro de eliminar el registro seleccionado?');"/>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Btn_Salir" runat="server" Text="Salir" Height="26px"
                                CssClass="button" CausesValidation="False" TabIndex="8" 
                                onclick="Btn_Salir_Click1" />
                        </td>
                    </tr>--%>
                </table>
                </ContentTemplate>
  </asp:UpdatePanel>
  </div>

</asp:Content>


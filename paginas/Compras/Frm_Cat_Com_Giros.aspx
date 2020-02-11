<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Com_Giros.aspx.cs" Inherits="paginas_Compras_Frm_Cat_Com_Giros" Title="Catálogo de Giros" %>
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
                        <td colspan="2" class="label_titulo">Cat&aacute;logo de Giros</td>
                    </tr>
                    <tr align="left">
                        <td colspan="2" >
                            <asp:Image ID="Img_Warning" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />
                            <asp:Label ID="Lbl_Informacion" runat="server" 
                                ForeColor="#990000"></asp:Label>                        
                        </td>
                    </tr>                    
                    <tr class="barra_busqueda" align="right">
                        <td colspan="2" align="left" valign="middle">
                           <table width = "100%">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                        CssClass="Img_Button" ToolTip="Nuevo" onclick="Btn_Nuevo_Click" />
                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                        CssClass="Img_Button" ToolTip="Modificar" onclick="Btn_Modificar_Click" />
                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                        CssClass="Img_Button" ToolTip="Eliminar"                                             
                                            
                                        OnClientClick="return confirm('¿Está seguro de eliminar el giro seleccionado?');" 
                                        onclick="Btn_Eliminar_Click" />
                                    <asp:ImageButton ID="Btn_Salir" runat="server"  
                                        CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            ToolTip="Inicio" onclick="Btn_Salir_Click" />
                                </td>
                                <td align = "right">
                                    Búsqueda por:
                                    <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Impuesto_ID" runat="server" WatermarkCssClass="watermarked"
                                        WatermarkText="<Ingrese nombre del giro>" TargetControlID="Txt_Busqueda" />
                                    <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Consultar"
                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" TabIndex="1" 
                                        onclick="Btn_Buscar_Click" />
                                </td>
                            </tr>
                           </table>                           
                        </td>                        
                    </tr>
                    <tr>
                        <td align="left">Giro ID</td>
                        <td align="left"><asp:TextBox ID="Txt_Giro_ID" runat="server" ReadOnly="true" Enabled="false" Width="150px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="left">*Nombre</td>
                        <td align="left">
                            <asp:TextBox ID="Txt_Nombre" runat="server" Width="600px" MaxLength="100"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Txt_Nombre_FilteredTextBoxExtender" runat="server" TargetControlID="Txt_Nombre" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "></cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">*Estatus</td>
                        <td align="left">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="150px">
                                <asp:ListItem Value="">Seleccione</asp:ListItem>
                                <asp:ListItem Value="ACTIVO">Activo</asp:ListItem>
                                <asp:ListItem Value="OBSOLETO">Obsoleto</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Comentarios</td>
                        <td align="left">
                            <asp:TextBox ID="Txt_Comentarios" runat="server" Width="600px" MaxLength="250" TextMode="MultiLine"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="Txt_Comentarios_TextBoxWatermarkExtender" runat="server" 
                                TargetControlID="Txt_Comentarios" WatermarkCssClass="watermarked" 
                                WatermarkText="&lt;Límite de Caracteres 250&gt;" />
                            <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                TargetControlID="Txt_Comentarios" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr align="center">
                        <td colspan="2">
                            <asp:GridView ID="Grid_Giros" runat="server" AutoGenerateColumns="False" 
                                CssClass="GridView_1" AllowPaging="True" PageSize="5" 
                                GridLines="None" Width = "98%" 
                                onpageindexchanging="Grid_Giros_PageIndexChanging" 
                                onselectedindexchanged="Grid_Giros_SelectedIndexChanged"> 
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="GIRO_ID" HeaderText="Giro ID">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />                                        
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                        <ItemStyle HorizontalAlign="Left" Width="30%" />                                                                            
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />                                                                            
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Comentarios">
                                        <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                        <ItemStyle HorizontalAlign="Left" Width="40%" />                                                                                                                
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


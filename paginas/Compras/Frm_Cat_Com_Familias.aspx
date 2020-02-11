<%@ Page Title="Catálogo de Familias" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Com_Familias.aspx.cs" Inherits="paginas_Compras_Frm_Cat_Com_Familias" %>
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
                
                    <div id="Div_Busqueda" style="width:100%; height:100%; background-color:#ffffff">
                        <table id="Tbl_Comandos" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">                
                            <tr>
                                <td colspan="4" class="label_titulo">
                                    Cátalogo de Familias
                                </td>                        
                            </tr>
                            
                            <tr>
                                <td colspan="4">
                                    <asp:Image ID="Img_Error" runat="server" ImageUrl = "../imagenes/paginas/sias_warning.png"/>
                                    <br />
                                    <asp:Label ID="Lbl_Error" runat="server" ForeColor="Red" Text="" TabIndex="0"></asp:Label>
                                </td> 
                            </tr>
                            
                            <tr class="barra_busqueda">
                                <td colspan="2" style="width:50%">
                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                    CssClass="Img_Button" onclick="Btn_Nuevo_Click"/>
                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                    CssClass="Img_Button" onclick="Btn_Modificar_Click"/>                            
                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                    CssClass="Img_Button" OnClick="Btn_Eliminar_Click" OnClientClick="return confirm('¿Esta seguro de Eliminar el presente registro?');" />
                                    <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                    CssClass="Img_Button" onclick="Btn_Salir_Click"/>
                                </td>
                                <td colspan="2" align="right" style="width:50%">
                                    Búsqueda
                                    <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"
                                    ToolTip="Buscar" TabIndex="1" ></asp:TextBox>
                                    <asp:ImageButton ID="Btn_Busqueda" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                        TabIndex="2" onclick="Btn_Busqueda_Click"/>
                                        <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                        WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                </td>                        
                            </tr>                    
                        </table>
        </div>
        
                    <div id="Div_Datos_Generales" runat="server" style="background-color:#ffffff; width:100%; height:100%;">
                        <table id="Tbl_Datos_Generales" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                            <tr>
                                <td style="width:18%">
                                    <asp:Label ID="Lbl_Familia_ID" runat="server" Text="Familia ID"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_Familia_ID" runat="server" Width="90%"></asp:TextBox>
                                </td>
                                <td style="width:18%">
                                </td>
                                <td style="width:32%">
                                </td>
                            </tr>
                            <tr>
                                <td style="width:18%">
                                    <asp:Label ID="Lbl_Nombre" runat="server" Text="*Nombre"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_Nombre" runat="server" Width="90%" MaxLength="100"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Txt_Nombre_FilteredTextBoxExtender" runat="server" 
                                            TargetControlID="Txt_Nombre" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                            ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                        </cc1:FilteredTextBoxExtender>
                                </td>
                                <td style="width:18%">
                                    <asp:Label ID="Lbl_Abreviatura" runat="server" Text="*Abreviatura"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_Abreviatura" runat="server" Width="90%" MaxLength="2"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="Ftxt_Abreviatura" runat="server"
                                        TargetControlID="Txt_Abreviatura" FilterType="UppercaseLetters,Numbers">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:18%">
                                    <asp:Label ID="Lbl_Estaus" runat="server" Text="*Estatus"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:DropDownList ID="Cmb_Estatus" Width="92%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="width:18%">
                                </td>
                                <td style="width:32%">
                                </td>
                            </tr>
                            <tr>
                                <td style="width:18%">
                                <asp:Label ID="Lbl_Comentarios" runat="server" Text="Comentarios"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Comentarios" runat="server" Width="96%" Height="65" 
                                            TextMode="MultiLine"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Comentarios" runat="server" WatermarkCssClass="watermarked"
                                                     WatermarkText="Límite de Caractes 250" TargetControlID="Txt_Comentarios">
                                                </cc1:TextBoxWatermarkExtender>
                                                <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                                runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                TargetControlID="Txt_Comentarios" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                                </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:18%">
                                </td>
                                <td style="width:32%">
                                </td>
                                <td style="width:18%">
                                </td>
                                <td style="width:32%">
                                </td>
                            </tr>
                        </table>
        </div>
                
                    <div id="Div_Grid" runat="server" style="background-color:#ffffff; width:100%; height:100%;">
                    <table id="Tbl_Grid" border="0" cellspacing="0" class="estilo_fuente" style="width:98%;">
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:GridView ID="Grid_Familias" runat="server" AllowPaging="true" 
                                        AutoGenerateColumns="False" CssClass="GridView_1"
                                        EmptyDataText="&quot;No se encontraron registros&quot;" GridLines="none"                                        
                                        PageSize="5" Style="white-space:normal" Width="96%" 
                                        onpageindexchanging="Grid_Familias_PageIndexChanging" 
                                        onselectedindexchanged="Grid_Familias_SelectedIndexChanged">
                                        <RowStyle CssClass="GridItem"/>
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                <ItemStyle Width="5%"/>
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="FAMILIA_ID"
                                                HeaderText="ID" SortExpression="FAMILIA_ID">
                                                <HeaderStyle HorizontalAlign="Left" Width="15%"/>
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NOMBRE"
                                                HeaderText="Nombre" SortExpression="NOMBRE">
                                                <HeaderStyle HorizontalAlign="Left" Width="55%"/>
                                                <ItemStyle HorizontalAlign="Left" Width="55%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ABREVIATURA"
                                                HeaderText="Abreviatura" SortExpression="ABREVIATURA">
                                                <HeaderStyle HorizontalAlign="Left" Width="10%"/>
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ESTATUS"
                                                HeaderText="Estatus" SortExpression="ESTATUS">
                                                <HeaderStyle HorizontalAlign="Left" Width="15%"/>
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
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
            
                        </td>
                    </tr>
                </table>                
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
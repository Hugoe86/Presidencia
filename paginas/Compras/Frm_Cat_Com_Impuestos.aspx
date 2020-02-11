<%@ Page Title="Catálogo de Impuestos" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Com_Impuestos.aspx.cs" Inherits="paginas_Compras_Frm_Cat_Com_Impuestos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Impuestos" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Impuestos" style="background-color:#ffffff; width:100%; height:100%;">            
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="4">
                            Catálogo de Impuestos
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
                        <td colspan="2"> 
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                            CssClass="Img_Button" 
                            ToolTip="Nuevo" onclick="Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                            CssClass="Img_Button" 
                            AlternateText="Modificar" ToolTip="Modificar" onclick="Btn_Modificar_Click"/>
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                            CssClass="Img_Button" 
                            AlternateText="Eliminar" ToolTip="Eliminar" 
                                OnClientClick="return confirm('¿Esta seguro de Eliminar el presente registro?');" 
                                onclick="Btn_Eliminar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" 
                            CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" 
                                onclick="Btn_Salir_Click"/>
                        </td>
                        <td colspan="2" align="right">
                            Búsqueda
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"
                            ToolTip="Buscar" TabIndex="1" ></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                            WatermarkText="<Nombre>" TargetControlID="Txt_Busqueda" />
                            <asp:ImageButton ID="Btn_Buscar_Servicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                            TabIndex="2" onclick="Btn_Buscar_Servicio_Click"/>
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
                    <tr>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_ID" runat="server" Text="Impuesto ID"></asp:Label>
                        </td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_ID" runat="server" Width="90%"></asp:TextBox>
                        </td>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td style="width:32%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Nombre" runat="server" Text="*Nombre"></asp:Label>
                        </td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Nombre" runat="server" Width="90%"></asp:TextBox>
                        </td>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Txt_Porcentaje_Impuesto" runat="server" Text="*Porcentaje"></asp:Label>
                        </td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Porcentaje_Impuesto" runat="server" Width="90%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Costo" 
                                runat="server" Enabled="True" TargetControlID="Txt_Porcentaje_Impuesto"
                                InvalidChars="<,>,&,',!," 
                                FilterType="Custom, Numbers" 
                                ValidChars=".12345676890">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>                    
                    <tr>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Comentarios" runat="server" Text="Comentarios"></asp:Label>
                            </td>
                            <td colspan="3" style="width:82%">
                            <asp:TextBox ID="Txt_Comentarios" runat="server" Width="96%" Height="65" 
                            TextMode="MultiLine"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Comentarios" runat="server" WatermarkCssClass="watermarked"
                                     WatermarkText="Límite de Caractes 250" TargetControlID="Txt_Comentarios">
                                </cc1:TextBoxWatermarkExtender>
                        </td>                        
                    </tr>
                    <tr>
                        <td style="width:18%">                            
                            </td>
                                <td style="width:32%">
                                    
                                <td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:GridView ID="Grid_Impuestos" runat="server" CssClass="GridView_1" Style="white-space:normal"
                            AutoGenerateColumns="False" AllowPaging="true" PageSize="5" Width="96%" 
                                GridLines="none" onpageindexchanging="Grid_Impuestos_PageIndexChanging" 
                                onselectedindexchanged="Grid_Impuestos_SelectedIndexChanged">
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png"  >
                                <ItemStyle Width="3%" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="IMPUESTO_ID" HeaderText="Impuesto ID"
                                SortExpression="IMPUESTO_ID" >
                                <ItemStyle Width="14%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" 
                                SortExpression="NOMBRE" >
                                <ItemStyle Width="30%" />
                            </asp:BoundField>                                
                            <asp:BoundField DataField="PORCENTAJE_IMPUESTO" HeaderText="Porcentaje Impuesto" 
                                SortExpression="PORCENTAJE_IMPUESTO" >
                                <ItemStyle Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="COMENTARIOS" HeaderText="Comentarios" 
                                SortExpression="COMENTARIOS" >
                                <ItemStyle Width="43%" />
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
                            &nbsp;<td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                                &nbsp;</td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td style="width:32%">
                            &nbsp;<td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                                &nbsp;</td>
                        </td>
                    </tr>
        </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


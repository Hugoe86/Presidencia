<%@ Page Title="Catálogo de Presupuestos" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Com_Dep_Presupuesto.aspx.cs" Inherits="paginas_Compras_Frm_Cat_Com_Dep_Presupuesto" %>
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
            <asp:UpdateProgress ID="Uprg_Presupuestos" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Presupuestos" style="overflow:auto; background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="4">
                            Catálogo de Presupuestos</td>            
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
                            ToolTip="Buscar por nombre de Dependencia o por Año" TabIndex="1" ></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                            WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                            <asp:ImageButton ID="Btn_Busqueda" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                            TabIndex="2" onclick="Btn_Buscar_Servicio_Click" ToolTip="Consultar"/>
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
                            <asp:Label ID="Lbl_Dependencia" runat="server" Text="*Dependencia"></asp:Label>
                        </td>
                        <td style="width:32%">
                            <asp:DropDownList ID="Cmb_Dependencias" runat="server" Width="92%">
                            </asp:DropDownList>
                        </td>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Anio_Presupuesto" runat="server" Text="*Año Presupuesto"></asp:Label></td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Anio_Presupuesto" runat="server" Width="90%" MaxLength="4"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                runat="server" Enabled="True" TargetControlID="Txt_Anio_Presupuesto"
                                InvalidChars="<,>,&,',!," 
                                FilterType="Numbers" 
                                ValidChars="12345676890">
                            </cc1:FilteredTextBoxExtender></td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Monto_Comprometido" runat="server" Text="*Monto Comprometido"></asp:Label>
                        </td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Monto_Comprometido" runat="server" Width="90%"></asp:TextBox>
                        </td>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                runat="server" Enabled="True" TargetControlID="Txt_Monto_Comprometido"
                                InvalidChars="<,>,&,',!," 
                                FilterType="Custom, Numbers" 
                                ValidChars=".12345676890">
                            </cc1:FilteredTextBoxExtender>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Monto_Presupuestal" runat="server" Text="*Monto Presupuestal"></asp:Label>
                        </td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Monto_Presupuestal" runat="server" Width="90%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Costo" 
                                runat="server" Enabled="True" TargetControlID="Txt_Monto_Presupuestal"
                                InvalidChars="<,>,&,',!," 
                                FilterType="Custom, Numbers" 
                                ValidChars=".12345676890">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>                    
                    <tr>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Monto_Disponible" runat="server" Text="*Monto Disponible"></asp:Label></td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Monto_Disponible" runat="server" Width="90%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                                runat="server" Enabled="True" TargetControlID="Txt_Monto_Disponible"
                                InvalidChars="<,>,&,',!," 
                                FilterType="Custom, Numbers" 
                                ValidChars=".12345676890">
                            </cc1:FilteredTextBoxExtender></td>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td style="width:32%">
                            &nbsp;</td>
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

                            <asp:GridView ID="Grid_Presupuestos" runat="server" CssClass="GridView_1" Style="white-space:normal"
                            AutoGenerateColumns="False" AllowPaging="true" PageSize="5" 
                                GridLines="none" onpageindexchanging="Grid_Presupuestos_PageIndexChanging" 
                                onselectedindexchanged="Grid_Presupuestos_SelectedIndexChanged" 
                                Width="96%" EmptyDataText="No se encontraron registros">
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png"  >
                                <ItemStyle Width="3%" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="PRESUPUESTO_ID" HeaderText="ID"
                                SortExpression="PRESUPUESTO_ID" HeaderStyle-Wrap="true">
                                <HeaderStyle Wrap="True" />
                                <ItemStyle HorizontalAlign="Center" Width="8%"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="DEPENDENCIA" HeaderText="Dependencia"
                                SortExpression="DEPENDENCIA" HeaderStyle-Wrap="true">
                                <HeaderStyle Wrap="True" />
                                <ItemStyle Width="20%" Wrap="true"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="ANIO_PRESUPUESTO" HeaderText="Año Presupuesto" 
                                SortExpression="ANIO_PRESUPUESTO" FooterStyle-Wrap="False">
                                <FooterStyle Wrap="False" />
                                <ItemStyle HorizontalAlign="Center" Width="10%" Wrap="true"/>
                            </asp:BoundField>                                
                            <asp:BoundField DataField="MONTO_COMPROMETIDO" HeaderText="Monto Comprometido" HeaderStyle-Wrap="true"
                                SortExpression="MONTO_COMPROMETIDO" >
                                <HeaderStyle Wrap="True" />
                                <ItemStyle HorizontalAlign="Center" Width="10%" Wrap="true"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="MONTO_PRESUPUESTAL" HeaderText="Monto Presupuestal" 
                                SortExpression="MONTO_PRESUPUESTAL" >
                                <ItemStyle HorizontalAlign="Center" Width="10%" Wrap="true"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="MONTO_DISPONIBLE" HeaderText="Monto Disponible" 
                                SortExpression="MONTO_DISPONIBLE" >
                                <ItemStyle HorizontalAlign="Center" Width="10%" Wrap="true"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="COMENTARIOS" HeaderText="Comentarios" 
                                SortExpression="COMENTARIOS" >                                
                                <ItemStyle Width="29%" Wrap="true"/>
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
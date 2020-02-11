<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Com_Proyectos_Programas.aspx.cs" Inherits="paginas_Compras_Frm_Cat_Com_Proyectos_Programas"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    
    
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
    
        <ContentTemplate>
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
                
            <div id="Div_SAP_Proyectos_Programas_Busqueda" style="background-color:#ffffff; width:100%; height:100%;">                
                <table id="Tbl_Comandos" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                    
                
                    <tr>
                        <td colspan="4" class="label_titulo">
                            Cat&aacute;logo de Proyectos y Programas
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
                            CssClass="Img_Button" onclick="Btn_Eliminar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                            CssClass="Img_Button" onclick="Btn_Salir_Click"/>
                        </td>
                        <td colspan="2" align="right" style="width:50%">
                            Búsqueda
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"
                            ToolTip="Buscar" TabIndex="1" ></asp:TextBox>
                            <asp:ImageButton ID="Btn_Busqueda" runat="server" ToolTip="Consultar"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                TabIndex="2" onclick="Btn_Busqueda_Click"/>
                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                        </td>                        
                    </tr>
                    
                </table>
            </div>
            
            <div id="Div_SAP_Proyectos_Programas" runat="server" style="background-color:#ffffff; width:100%; height:100%;">        
               <table id="Datos Generales_Inner" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                    
                    <tr>
                        <td style="width:18%">
                            &nbsp;
                        </td>
                        <td style="width:32%">
                            &nbsp;
                        </td>
                        <td style="width:18%">
                            &nbsp;
                        </td>
                        <td style="width:32%">
                            &nbsp;
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_ID" runat="server" Text="Proyecto Programa ID"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_ID" runat="server" Width="90%"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Nombre" runat="server" Text="*Nombre"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Nombre" runat="server" Width="90%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                TargetControlID="Txt_Nombre" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </cc1:FilteredTextBoxExtender>
                            </td>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Clave" runat="server" Text="*Clave"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Clave" runat="server" MaxLength="5" Width="90%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Txt_Nombre_FilteredTextBoxExtender" 
                                runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                TargetControlID="Txt_Clave" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    
                    <tr>                        
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Estatus" runat="server" Text="*Estatus"></asp:Label>
                        </td>
                        <td style="width:32%">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="92%">
                            </asp:DropDownList>
                        </td>
                        <td style="width:18%">
                        &nbsp;<asp:Label ID="Lbl_Elemento_Pep" runat="server" Text="*Elemento PEP"></asp:Label>
                        </td>                        
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Elemento_Pep" runat="server" MaxLength="10" Width="90%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                TargetControlID="Txt_Elemento_Pep" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Comentarios" runat="server" Text="Descripcion"></asp:Label>
                            </td>
                            <td colspan="3" style="width:82%">
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
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    
               </table>
                    
            </div>
                    
            <div id="Div_Grid_Programas_Proyectos" runat="server" style="background-color:#ffffff; width:100%; height:100%;">
                    <table id="Tbl_Grid_Proyectos" border="0" cellspacing="0" class="estilo_fuente" style="width:98%;">                        
                            <tr>
                                <td align="center" colspan="4">
                                    <div style="overflow:auto;height:320px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" >
                                        <asp:GridView ID="Grid_Proyectos_Programas" runat="server" AllowPaging="false" 
                                            AutoGenerateColumns="False" CssClass="GridView_1" AllowSorting="true"
                                            EmptyDataText="&quot;No se encontraron registros&quot;" GridLines="none" 
                                            onpageindexchanging="Grid_Proyectos_Programas_PageIndexChanging" 
                                            OnSorting="Grid_Proyectos_Programas_Sorting"
                                            onselectedindexchanged="Grid_Proyectos_Programas_SelectedIndexChanged" 
                                            PageSize="5" Style="white-space:normal" Width="96%">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="3%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE">
                                                    <HeaderStyle HorizontalAlign="Left"  />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CLAVE" HeaderText="Clave" SortExpression="CLAVE">
                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ELEMENTO_PEP" HeaderText="Elemento PEP" 
                                                    SortExpression="ELEMENTO_PEP">
                                                    <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="12%" />
                                                </asp:BoundField>                                            
                                                <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" 
                                                    SortExpression="ESTATUS">
                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                </asp:BoundField>
    <%--                                            <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" 
                                                    SortExpression="DESCRIPCION">
                                                    <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                </asp:BoundField>--%>
                                            </Columns>
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>                        
                    </table>
            </div>      
                                                    
        </ContentTemplate> 
                 
    </asp:UpdatePanel>
     
     
    <asp:UpdatePanel ID="Upd_Partidas" runat="server">
                        <ContentTemplate>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Upd_Partidas"
                        DisplayAfter="0">
                        <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressTemplate">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />                           
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                            <div id="Div_Partidas" runat="server" style="background-color:#ffffff; width:100%; height:100%;">
                            <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
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
                                    <td colspan="4" style = "font-size:medium; color:#18468C; font-weight:bold " align="left" >
                                        Partidas</td>
                                </tr>
                                <tr>
                                    <td align="left" class="label_titulo" colspan="4">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width:18%">
                                        <asp:Label ID="Lbl_Capitulo" runat="server" Text="Cápitulo"></asp:Label>
                                    </td>
                                    <td style="width:82%" colspan="3">
                                        <asp:DropDownList ID="Cmb_Capitulo" runat="server" Width="97%" 
                                            AutoPostBack="True" onselectedindexchanged="Cmb_Capitulo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td style="width:18%">
                                        <asp:Label ID="Lbl_Concepto" runat="server" Text="Concepto"></asp:Label></td>
                                    <td style="width:82%" colspan="3">
                                        <asp:DropDownList ID="Cmb_Conceptos" runat="server" Width="97%" 
                                            AutoPostBack="True" onselectedindexchanged="Cmb_Conceptos_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:18%">
                                        <asp:Label ID="Lbl_Partida_General" runat="server" Text="Partida Genérica"></asp:Label></td>
                                    <td style="width:82%" colspan="3">
                                        <asp:DropDownList ID="Cmb_Partida_General" runat="server" Width="97%" 
                                            AutoPostBack="True" 
                                            onselectedindexchanged="Cmb_Partida_General_SelectedIndexChanged">
                                        </asp:DropDownList></td>
                                 </tr>
                                 <tr>
                                    <td style="width:18%">
                                        <asp:Label ID="Lbl_Partida_Especifica" runat="server" Text="*Partida Específica"></asp:Label></td>
                                    <td style="width:82%" colspan="3">
                                        <asp:DropDownList ID="Cmb_Partida_Especifica" runat="server" Width="97%" 
                                            AutoPostBack="True" 
                                            onselectedindexchanged="Cmb_Partida_Especifica_SelectedIndexChanged">
                                        </asp:DropDownList></td>
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
                                    <td align="right">
                                        <asp:ImageButton ID="Btn_Agregar" Width="7%" Height="7%" runat="server" 
                                        ImageUrl="~/paginas/imagenes/gridview/add_grid.png"
                                        CssClass="Img_Button"  onclick="Btn_Agregar_Click" ToolTip="Agregar Partida"/>
                                        <asp:ImageButton ID="Btn_Quitar" runat="server"  Width="7%" Height="7%"
                                        ImageUrl="~/paginas/imagenes/gridview/minus_grid.png"
                                        CssClass="Img_Button" onclick="Btn_Quitar_Click"
                                        
                                            OnClientClick="return confirm('¿Esta seguro de quitar la partida del Programa seleccionado?');" 
                                            ToolTip="Quitar Partida" />
                                    </td>
                                </tr>                            
                        <tr>
                            <td align="center" colspan="4">
                            <div style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" >
                                    <asp:GridView ID="Grid_Partidas" runat="server" 
                                        CssClass="GridView_1" Style="white-space:normal"
                                    AutoGenerateColumns="False"  Width="96%" 
                                    GridLines="none" 
                                        EmptyDataText="&quot;No se encontraron partidas registradas&quot;" 
                                        OnSorting="Grid_Partidas_Sorting" AllowSorting="true"
                                        onpageindexchanging="Grid_Partidas_PageIndexChanging">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png"  >
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>                                                                  
                                    <asp:BoundField DataField="PARTIDA_CLAVE" HeaderText="Clave" 
                                        SortExpression="PARTIDA_CLAVE" >
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>                                    
                                    <asp:BoundField DataField="PARTIDA_NOMBRE" HeaderText="Partida" 
                                        SortExpression="PARTIDA_NOMBRE" >
                                        <HeaderStyle HorizontalAlign="Left" Width="80%" />
                                        <ItemStyle HorizontalAlign="Left" Width="80%" />
                                    </asp:BoundField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />                                
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </div>
                            </div>                                
                           </td>
                        </tr>
              </table>
              </div>
              </ContentTemplate>
         </asp:UpdatePanel>
         
</asp:Content>


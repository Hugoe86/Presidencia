<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Alm_Inventario_Stock.aspx.cs" Inherits="paginas_Almacen_Frm_Ope_Alm_Inventario_Stock" Title="Inventarios" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true">
    </asp:ScriptManager>
    
    <div id="Div_General" style="width: 98%;" visible="true" runat="server">
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
                <%--Div Encabezado--%>
                
               <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                     <ContentTemplate>                 
                <div id="Div_Encabezado" runat="server">
                    <table style="width: 100%;" border="0" cellspacing="0">
                        <tr align="center">
                            <td colspan="4" class="label_titulo">
                                Inventario de Stock
                            </td>
                        </tr>
                        <tr align="left">
                            <td colspan="4">
                                <asp:Image ID="Img_Warning" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />
                                <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="#990000"></asp:Label>
                            </td>
                        </tr>
                        <tr class="barra_busqueda" align="right">
                            <td align="left" valign="middle" colspan="2">
                                <asp:Button ID="Button1" runat="server" Text="Button" 
                                    onclick="Button1_Click1" Visible="false" />
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" 
                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" 
                                    onclick="Btn_Salir_Click"  />
                                <asp:ImageButton ID="Btn_Guardar" runat="server" CssClass="Img_Button" 
                                    ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" ToolTip="Guardar" 
                                    onclick="Btn_Guardar_Click"  />
                                <asp:ImageButton ID="Btn_Imprimir" runat="server" 
                                    ImageUrl="~/paginas/imagenes/gridview/grid_print.png" CssClass="Img_Button"  
                                    ToolTip="Imprimir" onclick="Btn_Imprimir_Click"  />                                    
                                <asp:HiddenField ID="Hdf_Producto_ID" runat="server" />
                                 <asp:ImageButton ID="Btn_Imprimir_Excel" runat="server" 
                                     ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" Width="24px" CssClass="Img_Button" 
                                     AlternateText="Imprimir Excel" 
                                     ToolTip="Exportar Excel"
                                     OnClick="Btn_Imprimir_Excel_Click" Visible="true"/>                                 
                            </td>
                            <td colspan="2">
                            </td>
                        </tr>
                    </table>
                </div>
               </ContentTemplate>
              </asp:UpdatePanel>                 
                <%--Div listado de requisiciones--%>
                <div id="Div_Listado" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 15%;">
                                Tipo 
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="Cmb_Tipo" runat="server" Width="25.5%" 
                                    AutoPostBack="true" onselectedindexchanged="Cmb_Tipo_SelectedIndexChanged"/>                                                                 
                            </td>
                        </tr>                      
                        <tr>
                            <td style="width: 15%;">
                                Clave
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Clave" runat="server" Width="25%"></asp:TextBox>                                
                            </td>
                        </tr>                    
                        <tr>
                            <td style="width: 15%;">
                                Producto
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="Txt_Producto" runat="server" Width="90%"></asp:TextBox>                                
                            </td>
                        </tr>  
                        <tr>
                            <td style="width: 15%;">
                                Partida
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="Cmb_Partida" runat="server" Width="90%">
                                </asp:DropDownList>                                                               
                            </td>
                            <td align="right">
                                <asp:Button ID="Btn_Buscar" runat="server" Text="Buscar" CssClass="button" 
                                    onclick="Btn_Buscar_Click"/>                                                           
                            </td>                            
                        </tr>
                        <tr>
                            <td colspan="2" style="width: 50%;">
                                <asp:Label ID="Lbl_Registros" runat="server" Text="" ForeColor="DarkBlue"></asp:Label>
                            </td>
                            <td colspan="2">
                                                                                             
                            </td>
                            <td align="right">
                                                          
                            </td>                            
                        </tr>                          
                        <tr>
                            <td colspan="4" style="height: 4px;">
                                <hr class="linea" />
                            </td>
                        </tr>                        
                     </table>  
                     <table style="width: 100%;" border="0" cellspacing="0">
                        <tr class="barra_busqueda" >                            
                            <td style="width:3%;" align="left"></td>
                            <td style="width:10%;" align="left">Clave</td>
                            <td style="width:13%;" align="left">Producto</td>
                            <td>Descripción</td>
                            <td style="width:5%" align="right" runat="server" id="Td_Existencia">Exist.</td>
                            <td style="width:5%" align="right" runat="server" id="Td_Disponible">Dis.</td>
                            <td style="width:5%" align="right" runat="server" id="Td_Comprometido">Com.</td>
                            <td style="width:5%" align="right" runat="server" id="Td_Minimo">Min.</td>
                            <td style="width:5%" align="right" runat="server" id="Td_Maximo">Max.</td>
                            <td style="width:5%" align="right" runat="server" id="Td_Reorden">P.R.</td>
                            <td style="width:8%" align="right" runat="server" id="Td_Promedio" visible="false">$ Prom.</td>
                            <td style="width:8%" align="right" runat="server" id="Td_Acumulado" visible="false">$ Acum.</td>                            
                        </tr>
                     </table>
                     <div style="overflow:auto;height:320px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" >                          
                     <table style="width: 100%;">
                        <tr>
                            <td style="width: 99%" align="center" colspan="4">
                                <asp:GridView ID="Grid_Inventario" runat="server" 
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" Width="100%" 
                                EmptyDataText="No se encontraron productos" ShowHeader="false">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Seleccionar_Requisicion" runat="server" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png"  
                                                    OnClick="Btn_Seleccionar_Producto_Click"                                                                                                      
                                                    CommandArgument='<%# Eval("PRODUCTO_ID") %>'/>
                                            </ItemTemplate >
                                            <HeaderStyle HorizontalAlign="Center" Width="3%" />
                                            <ItemStyle HorizontalAlign="Center" Width="3%" />
                                        </asp:TemplateField>                                    
                                        <asp:BoundField DataField="CLAVE" HeaderText="Clave" Visible="True" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Producto" 
                                             Visible="True" SortExpression="NOMBRE">
                                            <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" Visible="True" >
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                        </asp:BoundField>                                                                             
                                        <asp:BoundField DataField="EXISTENCIA" HeaderText="Exist." Visible="True" >
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" Width="5%" Font-Size="X-Small"/>
                                        </asp:BoundField>                                                                                    
                                        <asp:BoundField DataField="DISPONIBLE" HeaderText="Disp." >
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" Width="5%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="COMPROMETIDO" HeaderText="Comp."  >
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" Width="5%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MINIMO" HeaderText="Min." >
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" Width="5%" Font-Size="X-Small"/>
                                        </asp:BoundField>    
                                        <asp:BoundField DataField="MAXIMO" HeaderText="Max." >
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" Width="5%" Font-Size="X-Small"/>
                                        </asp:BoundField>                                                                             
                                        <asp:BoundField DataField="REORDEN" HeaderText="Reorden" >
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" Width="5%" Font-Size="X-Small" />
                                        </asp:BoundField>     
                                        <asp:BoundField DataField="COSTO_PROMEDIO" HeaderText="P. Promedio" DataFormatString="{0:n}" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Right" Width="8%" Font-Size="X-Small" />
                                        </asp:BoundField>                                                                                                                   
                                        <asp:BoundField DataField="ACUMULADO" HeaderText="P.Acumulado" DataFormatString="{0:n}" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Right" Width="8%" Font-Size="X-Small"/>
                                        </asp:BoundField>     
                                        <asp:BoundField DataField="PRODUCTO_ID" HeaderText="ID" Visible="false" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="5%" Font-Size="X-Small"/>
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
                </div>
                <div>
                    <table>
                        <tr align="right">
                            <td style="width:75%"></td>
                            <td align="right" style="width:25%; text-align:right;">
                                <asp:Label Style="text-align:right;" Width="100%" ID="Lbl_Total_Acumulado" runat="server"
                                    Text="" Visible="true" ForeColor="Blue"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <%--Div Contenido--%>
                <div id="Div_Contenido" runat="server" visible="false">

                    <table style="width: 100%;">
                        <tr>
                            <td style="width:15%">
                                Clave
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Clave_Modificar" runat="server" Width="50%" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width:15%">
                                Nombre
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Nombre_Modificar" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                            </td>                            
                        </tr>
                     
                        <tr>
                            <td style="width:15%">
                                Descripción
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Descripcion_Modificar" runat="server" Enabled="false" TextMode="MultiLine" Width="98%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="height: 4px;">
                                <hr class="linea" />
                            </td>
                        </tr>                         
                        <tr>
                            <td style="width:15%">
                                Existencia
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Existencia_Modificar" runat="server" Width="50%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%">
                                Disponible
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Disponible_Modificar" runat="server" Width="50%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%">
                                Comprometido
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Comprometido_Modificar" runat="server" Width="50%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="height: 4px;">
                                <hr class="linea" />
                            </td>
                        </tr>                                        
                        <tr>
                            <td style="width:15%">
                                Minimo
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Minimo_Modificar" runat="server" Width="50%"></asp:TextBox>
                            </td>
                        </tr>         
                        <tr>
                            <td style="width:15%">
                                Máximo
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Maximo_Modificar" runat="server" Width="50%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%">
                                Reorden
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Reorden_Modificar" runat="server" Width="50%"></asp:TextBox>
                            </td>
                        </tr>                                                                                                
                    </table>
                </div>
            </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger  ControlID="Btn_Imprimir_Excel"/>
                </Triggers>              
        </asp:UpdatePanel>
    </div>    
</asp:Content>


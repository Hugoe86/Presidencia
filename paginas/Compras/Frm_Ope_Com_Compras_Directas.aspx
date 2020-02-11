<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Com_Compras_Directas.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Compras_Directas" Title="Compras Directas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

 <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True">
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
                <div id="Div_Encabezado" runat="server">  
                    <table style="width: 100%;" border="0" cellspacing="0">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Compras Directas</td>
                    </tr>
                    <tr align="left">
                        <td colspan="2">
                            <asp:Image ID="Img_Warning" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />
                            <asp:Label ID="Lbl_Informacion" runat="server"
                                ForeColor="#990000"></asp:Label>              
                        </td>
                    </tr>                    
                    <tr>
                        <td colspan ="4">
                            <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                            <table style="width:100%;">
                            <tr>
                                <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                                <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                Width="24px" Height="24px"/>
                                    <asp:HiddenField ID="HDF_No_Requisicion" runat="server" />
                                </td>            
                                <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />
                                </td>
                            </tr> 
                            </table>                   
                            </div>
                        </td>
                    </tr>                              
                    <tr class="barra_busqueda" align="right">
                        <td align="left" valign="middle">                         
                                <asp:ImageButton ID="Btn_Guardar" runat="server" ToolTip="Guardar Consolidación" CssClass="Img_Button"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" onclick="Btn_Guardar_Click" 
                                    AlternateText="Modificar"/>
                                <asp:ImageButton ID="Btn_Salir" runat="server"
                                    CssClass="Img_Button"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" onclick="Btn_Salir_Click"/>                                    
                        </td>
                        <td>
                        </td>                         
                    </tr> 

                    </table>                    
                </div>      
                <div id="Div_Filtros" runat="server">
                    <table  style="width: 100%;" border="0" cellspacing="0">
                        <tr>
                            <td>
                                Requisiciones:&nbsp;&nbsp;
                                <asp:DropDownList ID="Cmb_Tipo_Articulo" runat="server" 
                                    onselectedindexchanged="Cmb_Tipo_Articulo_SelectedIndexChanged"
                                    AutoPostBack = "true">                                    
                                </asp:DropDownList>                                
                            </td>
                            <td>
                                Estatus:&nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Estatus" runat="server" Enabled = "false"></asp:TextBox>
                            </td>                                                        
                            <td>
                                Tipo:&nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Tipo" runat="server" Enabled = "false"></asp:TextBox>                            
                            </td>        
                        </tr>    
                    </table>                                        
                </div> 
                <%--Div listado de Consolidaciones--%>
                <div id="Div_Listado_Requisiciones" runat="server">                    
                    <table style="width:100%;">
                        <tr>
                            <td style="width:99%" align="center">
                                 <div>   
                                 <asp:GridView ID="Grid_Requisiciones" runat="server" 
                                    AutoGenerateColumns="False" 
                                    CssClass="GridView_1" GridLines="None" 
                                    Width="99%"
                                    AllowPaging="true"
                                    DataKeyNames="NO_REQUISICION,TOTAL" 
                                    onpageindexchanging="Grid_Requisiciones_PageIndexChanging" 
                                    onselectedindexchanged="Grid_Requisiciones_SelectedIndexChanged" 
                                    PageSize="10" AllowSorting="true" OnSorting="Grid_Requisiciones_Sorting">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>                                    
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="5%" />
                                        </asp:ButtonField>                                                                                                 
                                        <asp:BoundField DataField="NO_REQUISICION" HeaderText="No. Requisición" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left"/>
                                        </asp:BoundField>                                        
                                        <asp:BoundField DataField="FOLIO" HeaderText="Folio" Visible="True" SortExpression="NO_REQUISICION">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_DEPENDENCIA" HeaderText="Unidad Responsable" Visible="True" SortExpression="NOMBRE_DEPENDENCIA">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="40%"/>
                                        </asp:BoundField>                                        
                                        <asp:BoundField DataField="FECHA_AUTORIZACION" HeaderText="F.Autorizada"  SortExpression="FECHA_AUTORIZACION"
                                            Visible="True" DataFormatString="{0:dd/MMM/yyyy}">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%"/>
                                        </asp:BoundField>                                                                                      
                                        <asp:BoundField DataField="FECHA_FILTRADO" HeaderText="F.Filtrado" SortExpression="FECHA_FILTRADO"
                                            Visible="True" DataFormatString="{0:dd/MMM/yyyy}">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%"/>
                                        </asp:BoundField>                                                                                
                                        <asp:BoundField DataField="TOTAL" HeaderText="Total" Visible="True" SortExpression="TOTAL">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Right" Width="14%"/>
                                        </asp:BoundField>
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
                
                <div id="Div_Orden_Compra" runat="server" visible = "false">                          
                    <table style="width: 100%;">
                        <tr align="center">
                            <td class="label_titulo">Cotizar</td>
                        </tr>                        
                    </table>                                        
                    <table style="width: 100%;">
                        <tr>
                            <td align="left" style="width:15%;">
                                Unidad Responsable
                            </td>
                            <td align="left" style="width:35%;">
                                <asp:TextBox ID="Txt_Dependencia" runat="server" Width="95%" Enabled="false"></asp:TextBox>
                                <asp:TextBox ID="Txt_No_Orden_Compra" runat="server" Width="95%" Enabled="false" Visible="false"></asp:TextBox>
                            </td>
                            <td align="left" style="width:15%;">
                                Fecha 
                            </td>
                            <td align="left" style="width:35%;">
                                <asp:TextBox ID="Txt_Fecha" runat="server" Width="95%" Enabled="false"></asp:TextBox>
                            </td>                            
                        </tr>                    
                        <tr>                        
                            <td align="left" style="width:15%;">
                                No. Requisición
                            </td>
                            <td align="left" style="width:35%;">
                                <asp:TextBox ID="Txt_No_Requisicion" runat="server" Width="95%" Enabled="false"></asp:TextBox>
                            </td>                                                
                            <td align="left" style="width:15%;">
                                Partidas
                            </td>
                            <td align="left" style="width:35%;">
                                <asp:DropDownList ID="Cmb_Partidas" runat="server" Width="98%" Enabled="false">
                                </asp:DropDownList>
                            </td>                            
                        </tr>    
                        <tr>
                            <td style="width: 50%;" align="left" colspan="2">
                            </td>
                            <td style="width: 15%;" align="left">
                                Disponible
                            </td>                        
                            <td style="width: 35%;" align="left" >
                                <asp:Label ID="Lbl_Disponible_Partida" runat="server" Text=" $ 0.00" 
                                ForeColor="Blue" BorderColor="Blue" BorderWidth="2px" Width="98%">
                                </asp:Label>
                            </td>
                        </tr>                        
                        <tr align="center">
                            <td  colspan="4">
                                <hr class="linea" />
                            </td>
                        </tr>  
                        <tr>
                            <td align="left" style="width:15%;">
                                Giros ó Conceptos
                            </td>
                            <td align="left" style="width:35%;">                               
                                <asp:DropDownList ID="Cmb_Giros" runat="server" Width="98%" 
                                onselectedindexchanged="Cmb_Giros_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>                                                        
                            <td align="left" style="width:15%;">
                                Proveedores
                            </td>                            
                            <td align="left" style="width:35%;">
                                <table>          
                                  <tr>
                                    <td align="left" style="width:85%;">
                                        <asp:DropDownList ID="Cmb_Proveedor" runat="server" Width="98%">
                                        </asp:DropDownList>
                                    </td>    
                                    <td align="left" style="width:15%;">
                                        <asp:ImageButton ID="Btn_Actualizar_Proveedor" runat="server" 
                                            onclick="Btn_Actualizar_Proveedor_Click" CssClass="Img_Button" 
                                            ImageUrl="~/paginas/imagenes/paginas/accept.png"/>
                                    </td>
                                  </tr> 
                                </table>            
                            </td>                            
                        </tr>       
                      </table>
                      <div style="overflow:inherit; height:35px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" 
                            runat="server" id="Div_Modificar_Productos" visible="false">                      
                      <table width="100%">                                               
                        <tr>
                            <td align="left" style="width:15%;">
                                Producto
                            </td>
                            <td align="left" style="width:35%;">
                                <asp:TextBox ID="Txt_Producto" runat="server" Width="95%" Enabled="false"></asp:TextBox>
                            </td>
                            <td align="left" style="width:15%;">
                                Precio Unitario
                            </td>
                            <td align="left" style="width:35%;">                                                                                                    
                                <table>          
                                  <tr>
                                    <td align="left" style="width:85%;">
                                        <asp:TextBox ID="Txt_Precio_Unitario" runat="server" Width="98%" MaxLength="20"></asp:TextBox>                                      
                                        <cc1:FilteredTextBoxExtender ID="FTxb_PU" runat="server" 
                                            FilterType="Custom,Numbers"
                                            TargetControlID="Txt_Precio_Unitario" ValidChars=".,">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>    
                                    <td align="left" style="width:15%;">
                                        <asp:ImageButton ID="Btn_Actualizar_Precio" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/accept.png"
                                        ToolTip="Actualizar Precio" onclick="Btn_Actualizar_Precio_Click" 
                                        CssClass="Img_Button"/>
                                    </td>
                                  </tr> 
                                </table>                                           
                            </td>                                                        
                        </tr>         

                    </table>
                    </div>
                                        
                    <table style="width: 100%;">                        
                        <tr>
                            <td style="width:99%" align="center" colspan="2">
                                 <asp:GridView ID="Grid_Productos_Requisiciones" runat="server"
                                    AutoGenerateColumns="false" DataKeyNames="PROD_SERV_ID,NOMBRE_PRODUCTO_SERVICIO,MONTO,PRECIO_UNITARIO"
                                    CssClass="GridView_1" GridLines="None"
                                    Width="100%" AllowPaging="True" PageSize="5" onpageindexchanging="Grid_Productos_Requisiciones_PageIndexChanging" 
                                     onselectedindexchanged="Grid_Productos_Requisiciones_SelectedIndexChanged" >
                                    <RowStyle CssClass="" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="5%" />
                                        </asp:ButtonField>                                                                          
                                        <asp:BoundField DataField="NOMBRE_PRODUCTO_SERVICIO" HeaderText="Nombre" Visible="true" >
                                            <HeaderStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                            <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small" />  
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_PROVEEDOR" HeaderText="Proveedor" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" ForeColor="Blue"/>
                                        </asp:BoundField>                                         
                                        <asp:BoundField DataField="CANTIDAD" HeaderText="Ctd" Visible="true">
                                            <HeaderStyle HorizontalAlign="Center" Font-Size="X-Small"/>
                                            <ItemStyle HorizontalAlign="Center" Width="5%" Font-Size="X-Small"/>
                                        </asp:BoundField>                                  
                                        <asp:BoundField DataField="PRECIO_UNITARIO" HeaderText="PU S/I" Visible="true">
                                            <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                            <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small"/>   
                                        </asp:BoundField>                         
                                        <asp:BoundField DataField="MONTO_TOTAL" HeaderText="Total" Visible="true">
                                            <HeaderStyle HorizontalAlign="Center" Font-Size="X-Small"/>
                                            <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small"/>
                                        </asp:BoundField>                                                                                
                                        <asp:BoundField DataField="PRECIO_U_SIN_IMP_COTIZADO" HeaderText="PU Cotizado" Visible="true">
                                            <HeaderStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                            <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small" ForeColor="Blue"/>
                                        </asp:BoundField>                                                                                                 
                                        <asp:BoundField DataField="TOTAL_COTIZADO" HeaderText="T. Cotizado" Visible="true">
                                            <HeaderStyle HorizontalAlign="Center" Font-Size="X-Small"/>
                                            <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small" ForeColor="Blue"/>
                                        </asp:BoundField>                                                                                
                                        <asp:BoundField DataField="PROD_SERV_ID" HeaderText="ID" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                            <ItemStyle HorizontalAlign="Center" Font-Size="X-Small"/>
                                        </asp:BoundField>                                         
                                    </Columns>
                                    <RowStyle CssClass="GridItem" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                        </tr>
                        <tr>
                            <td colspan="2">
                              <hr class="linea" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" >
                                Subtotal
                                <asp:TextBox ID="Txt_Subtotal" runat="server" Style="text-align:right;" Enabled="false" Width="95px"></asp:TextBox>
                            </td>
                            <td align="right" >                                
                                <asp:Label ID="Lbl_Subtotal_Cotizado" runat="server" Text="Subtotal" ForeColor="Blue"></asp:Label>
                                <asp:TextBox ID="Txt_Subtotal_Cotizado" runat="server" Style="text-align:right;" 
                                Enabled="false" Width="95px" ForeColor="Blue"></asp:TextBox>
                            </td>
                            
                        </tr>                                                     
                        <tr>
                            <td align="right" >
                                IEPS
                                <asp:TextBox ID="Txt_IEPS" runat="server" Style="text-align:right;" Enabled="false" Width="95px"></asp:TextBox>
                            </td>
                            <td align="right" >
                                <asp:Label ID="Lbl__IEPS_Cotizado" runat="server" Text="IEPS" ForeColor="Blue"></asp:Label>
                                <asp:TextBox ID="Txt_IEPS_Cotizado" runat="server" Style="text-align:right;" 
                                Enabled="false" Width="95px" ForeColor="Blue"></asp:TextBox>
                            </td>                            
                        </tr>                             
                        <tr>
                            <td align="right" >
                                IVA
                                <asp:TextBox ID="Txt_IVA" runat="server" Style="text-align:right;" Enabled="false" Width="95px"></asp:TextBox>
                            </td>
                            <td align="right" >
                                <asp:Label ID="Lbl_IVA_Cotizado" runat="server" Text="IVA" ForeColor="Blue"></asp:Label>
                                <asp:TextBox ID="Txt_IVA_Cotizado" runat="server" Style="text-align:right;" 
                                Enabled="false" Width="95px" ForeColor="Blue"></asp:TextBox>
                            </td>                            
                        </tr>                                                                                                      
                                                
                        <tr>
                            <td align="right" >
                                Total
                                <asp:TextBox ID="Txt_Total_Requisicion" runat="server" Style="text-align:right;" Enabled="false" Width="95px"></asp:TextBox>
                            </td>
                            <td align="right" >
                                <asp:Label ID="Lbl_Total_Cotzacion" runat="server" Text="Total Cotización" ForeColor="Blue"></asp:Label>
                                <asp:TextBox ID="Txt_Total_Cotizado" runat="server" Style="text-align:right;" 
                                Enabled="false" Width="95px" ForeColor="Blue"></asp:TextBox>
                            </td>                            
                        </tr>                             
                        <tr>
                            <td>
                                <br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/>
                                <br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/>
                            </td>
                        </tr>                        
                    </table>
                </div>                
           </ContentTemplate>           
      </asp:UpdatePanel>
  </div>
</asp:Content>


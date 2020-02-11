<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Com_Consolidar_Requisicion.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Consolidar_Requisicion" Title="Consolidar Requisiciones" %>

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
                        <td colspan="2" class="label_titulo">Consolidar Requisiciones</td>
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
                                <asp:ImageButton ID="Btn_Consolidar" runat="server" ImageUrl="~/paginas/imagenes/paginas/Listado.png"
                                    CssClass="Img_Button"
                                    ToolTip="Consolidar" onclick="Btn_Consolidar_Click"/>
                                <asp:ImageButton ID="Btn_Guardar" runat="server" ToolTip="Guardar Consolidación" CssClass="Img_Button"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" onclick="Btn_Guardar_Click" />
                                <asp:ImageButton ID="Btn_Ver_Consolidaciones" runat="server"
                                    CssClass="Img_Button"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" ToolTip="Ver Consolidaciones" onclick="Btn_Ver_Consolidaciones_Click"/> 
                                <asp:ImageButton ID="Btn_Salir" runat="server"
                                    CssClass="Img_Button"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" onclick="Btn_Salir_Click"/>                                                                        
                        </td>
                        <td>
                        </td>                         
                    </tr> 

                    </table>                    
                </div>      
                <%--Div listado de requisiciones FILTRADAS--%>
                <div id="Div_Listado_Requisiciones" runat="server">

                    <table  style="width: 100%;" border="0" cellspacing="0">
                        <tr>
                            <td>
                                Requisiciones:&nbsp;&nbsp;
                                <asp:DropDownList ID="Cmb_Tipo_Articulo" runat="server" 
                                    onselectedindexchanged="Cmb_Tipo_Articulo_SelectedIndexChanged" AutoPostBack="true">
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
                    <table style="width:100%;">
                        <tr>
                            <td style="width:99%" align="center">
                                 <div style="overflow:auto;height:250px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" >   
                                 <asp:GridView ID="Grid_Requisiciones" runat="server" 
                                    AutoGenerateColumns="False" 
                                    CssClass="GridView_1" GridLines="None" 
                                    Width="96%"
                                    DataKeyNames="NO_REQUISICION,TOTAL">                                                  
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>                                    
                                        <asp:BoundField DataField="GRUPO" HeaderText="Grupo" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%"/>
                                        </asp:BoundField>                                                                            
                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate >
                                                <asp:CheckBox ID="Chk_Requisa" runat="server" />                                                
                                            </ItemTemplate >
                                            <ControlStyle Width="8%"/>
                                        </asp:TemplateField>                   
                                        <asp:BoundField DataField="NO_REQUISICION" HeaderText="No. Requisición" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left"/>
                                        </asp:BoundField>                                        
                                        <asp:BoundField DataField="FOLIO" HeaderText="Folio" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_DEPENDENCIA" HeaderText="Dependencia" Visible="True">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="38%"/>
                                        </asp:BoundField>                                        
                                        <asp:BoundField DataField="FECHA_FILTRADO" HeaderText="Fecha" Visible="True"
                                            DataFormatString="{0:dd/MMM/yyyy}">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%"/>
                                        </asp:BoundField>                                                                                
                                        <asp:BoundField DataField="TOTAL" HeaderText="Total" Visible="True" DataFormatString="{0:0.00}">
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
                <div>                            
                    <table style="width: 100%;">
                        <tr>
                            <td>
                              <hr class="linea" />
                            </td>
                        </tr>
                        <tr align="center">
                            <td class="label_titulo">Productos de Requisiciones Consolidadas</td>
                        </tr>                        
                    </table>                                        
                </div>                            
                <%--Div Contenido--%>
                <div id="Div_Contenido" runat="server">  
                    <table style="width: 100%;">
                        <tr>
                            <td style="width:99%" align="center" colspan="2">
                                 <asp:GridView ID="Grid_Requisas_Consolidadas" runat="server"
                                    AutoGenerateColumns="false" DataKeyNames="ID"
                                    CssClass="GridView_1" GridLines="None"
                                    Width="100%" AllowPaging="True" onpageindexchanging="Grid_Requisas_Consolidadas_PageIndexChanging" PageSize="5" >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="Clave" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>                                    
                                        <asp:BoundField DataField="CLAVE" HeaderText="Clave" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" Visible="true">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="10%"/>
                                        </asp:BoundField>                                                             
<%--                                        <asp:BoundField DataField="TIPO" HeaderText="Tipo" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="5%"/>
                                        </asp:BoundField>                                                                 
                                        <asp:BoundField DataField="NOMBRE_GIRO" HeaderText="Giro" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="5%"/>
                                        </asp:BoundField>--%>                                
                                        <asp:BoundField DataField="IMPORTE_TOTAL_CON_IMP" HeaderText="Costo" Visible="true"
                                            DataFormatString="{0:0.00}">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" Width="10%"/>
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
                            <td align="left">
                                Número de Consolidación:&nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Num_Consolidacion" runat="server" Style="text-align:right;" Enabled="false" Width="120px"></asp:TextBox>
                            </td>                        
                            <td align="right">
                                Total de la Consolidación:&nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Total" runat="server" Style="text-align:right;" Enabled="false" Width="120px"></asp:TextBox>
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


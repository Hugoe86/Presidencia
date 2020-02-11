<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Com_Listado_Consolidaciones.aspx.cs" Inherits="paginas_Listado_Consolidaciones" Title="Listado Consolidaciones" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

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
<%--               <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>                   
                </asp:UpdateProgress>     --%>
                <%--Div Encabezado--%>
                
                <div id="Div_Encabezado" runat="server">  
                    <table style="width: 100%;" border="0" cellspacing="0">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Listado Consolidaciones</td>
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
                        </td>
                    </tr>                              
                    <tr class="barra_busqueda" align="right">
                        <td align="left" valign="middle">                         
                                <asp:ImageButton ID="Btn_Salir" runat="server"
                                    CssClass="Img_Button"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Regresar" onclick="Btn_Salir_Click"/>                                    
                                <asp:ImageButton ID="Btn_Modificar" runat="server"
                                    CssClass="Img_Button"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" ToolTip="Modificar" onclick="Btn_Modificar_Click"/>
                        </td>
                        <td>
                        </td> 
                    </tr> 

                    </table>                    
                </div>      
                <%--Div listado de Consolidaciones--%>
                <div id="Div_Listado_Requisiciones" runat="server">
                    <table  style="width: 100%;" border="0" cellspacing="0">
                        <tr>
                            <td>
                                Fecha Inicial:&nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="90px" Enabled="false"></asp:TextBox>           
                                <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Inicio_FilteredTextBoxExtender" 
                                    runat="server" TargetControlID="Txt_Fecha_Inicio" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                                <cc1:CalendarExtender ID="Txt_Fecha_Inicio_CalendarExtender" runat="server" 
                                    TargetControlID="Txt_Fecha_Inicio" PopupButtonID="Btn_Fecha_Inicio" Format="dd/MMM/yyyy"/>
                                <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha Inicial"/>
                            </td>                                                        
                            <td>
                                Fecha Final:&nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="90px" Enabled="false"></asp:TextBox>                            
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                                    TargetControlID="Txt_Fecha_Final" PopupButtonID="Btn_Fecha_Final" Format="dd/MMM/yyyy"/>
                                <asp:ImageButton ID="Btn_Fecha_Final" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha Final"/>
                            </td>        
                            <td>
                                No. Órden Compra:&nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Consolidacion_Busqueda" runat="server"></asp:TextBox>                                
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" 
                                    runat="server" WatermarkCssClass="watermarked"
                                    WatermarkText="<CN-0>" 
                                    TargetControlID="Txt_Consolidacion_Busqueda" />
                               <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Consultar"
                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                    OnClick="Btn_Buscar_Click"/>
                            </td>                            
                        </tr>    
                    </table>                                        
                    
                    <table style="width:100%;">
                        <tr>
                            <td style="width:99%" align="center">
                                 <%--<div style="overflow:auto;height:180px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" >   --%>
                                 <div>   
                                 <asp:GridView ID="Grid_Consolidaciones" runat="server" 
                                    AutoGenerateColumns="False" 
                                    CssClass="GridView_1" GridLines="None" 
                                    Width="99%"
                                    DataKeyNames="LISTA_REQUISICIONES,NO_CONSOLIDACION,MONTO,FOLIO,TIPO" AllowPaging="True" 
                                    onpageindexchanging="Grid_Consolidaciones_PageIndexChanging" 
                                         onselectedindexchanged="Grid_Consolidaciones_SelectedIndexChanged" PageSize="5">                                                  
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="NO_CONSOLIDACION" HeaderText="Folio" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center"/>
                                        </asp:BoundField>                                    
                                        <asp:BoundField DataField="FOLIO" HeaderText="Folio" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%"/>
                                        </asp:BoundField>                                                                            
                                        <asp:BoundField DataField="FECHA_CREO" HeaderText="Fecha" Visible="True" DataFormatString="{0:dd/MMM/yyyy}" >
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%"/>
                                        </asp:BoundField>                                                                                                                                                                
                                        <asp:BoundField DataField="USUARIO_CREO" HeaderText="Usuario" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left"/>
                                        </asp:BoundField>                                        
                                        <asp:BoundField DataField="MONTO" HeaderText="Monto" Visible="True">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" Width="20%"/>
                                        </asp:BoundField>                                        
                                        <asp:BoundField DataField="LISTA_REQUISICIONES" HeaderText="Lista" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TIPO" HeaderText="Tipo_Articulo" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
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
                <div  id="Div_Informacion" runat="server" visible="false">                
                <div>                            
                    <table style="width: 100%;">
                        <tr>
                            <td>
                              <hr class="linea" />
                            </td>
                        </tr>
                        <tr align="center">
                            <td class="label_titulo">Requisiciones de Consolidación</td>
                        </tr>                        
                    </table>                                        
                </div>                            
                <%--Div Contenido--%>
                <div id="Div1" runat="server">  
                    <table style="width: 100%;">
                        <tr>
                            <td style="width:99%" align="center" colspan="2">
                                 <asp:GridView ID="Grid_Requisiciones" runat="server"
                                    AutoGenerateColumns="false" DataKeyNames="TOTAL"
                                    CssClass="GridView_1" GridLines="None"
                                    Width="100%" AllowPaging="True"  PageSize="5" 
                                    onpageindexchanging="Grid_Requisiciones_PageIndexChanging" >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:BoundField DataField="NO_REQUISICION" HeaderText="ID" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%"/>
                                        </asp:BoundField>                                    
                                        <asp:BoundField DataField="FOLIO" HeaderText="Folio" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="5%"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_DEPENDENCIA" HeaderText="Nombre" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="5%"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TOTAL" HeaderText="Costo" Visible="true">
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" Width="5%"/>
                                        </asp:BoundField>                                                                                                                                                                                                                                                             
                                    </Columns>
                                    <RowStyle CssClass="GridItem" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
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
                            <td class="label_titulo">Productos de Consolidación</td>
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
                                    Width="100%" AllowPaging="True" onpageindexchanging="Grid_Requisas_Consolidadas_PageIndexChanging" 
                                    PageSize="5" >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="Clave" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>                                    
                                        <asp:BoundField DataField="CLAVE" HeaderText="Clave" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CANTIDAD" HeaderText="Ctd." Visible="true">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="8%"/>
                                        </asp:BoundField>                                                             
                                        <asp:BoundField DataField="TIPO" HeaderText="Tipo" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="8%"/>
                                        </asp:BoundField>                                                                 
<%--                                        <asp:BoundField DataField="NOMBRE_GIRO" HeaderText="Giro" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="5%"/>
                                        </asp:BoundField>                                                         --%>        
                                        <asp:BoundField DataField="IMPORTE_TOTAL_CON_IMP" HeaderText="Costo" Visible="true">
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" Width="8%"/>
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
                </div>
           </ContentTemplate>           
      </asp:UpdatePanel>
  </div>
</asp:Content>


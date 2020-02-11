<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" 
AutoEventWireup="true" 
CodeFile="Frm_Ope_Com_Autorizar_Orden_Compra_Especiales.aspx.cs" 
Inherits="paginas_Compras_Frm_Ope_Com_Autorizar_Orden_Compra_Especiales" 
Title="Asignación de reserva" %>

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
                        <td colspan="2" class="label_titulo">Asignar reserva a órden de compra de programas especiales y ramo 33</td>
                    </tr>
                  
                    <tr>
                        <td colspan ="4">
                            <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                            <table style="width:100%;">
                            <tr>
                                <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                                <asp:ImageButton ID="Img_Warning" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                Width="24px" Height="24px"/>
                                </td>            
                                <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                                <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="Red" />
                                </td>
                            </tr> 
                            </table>                   
                            </div>
                        </td>
                    </tr>
                        <tr class="barra_busqueda" align="right">
                            <td align="left" valign="middle">
                                <asp:ImageButton ID="Btn_Guardar" runat="server" ToolTip="Guardar Órden de Compra" AlternateText="Modificar" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" OnClick="Btn_Guardar_Click" />
                                <asp:ImageButton ID="Btn_Imprimir_Orden_Compra" runat="server" 
                                                    ImageUrl="~/paginas/imagenes/gridview/grid_print.png" 
                                                    OnClick="Btn_Imprimir_Orden_Compra_Click"/>
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" OnClick="Btn_Salir_Click" />
                            
                            </td>
                        </tr>
                    </table>
                </div>      
                <div id="Div_Filtros" runat="server" visible = "true">
                    <table  style="width: 100%;" border="0" cellspacing="0">
                        <tr>
                            <td>
                                De&nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="80px" Enabled="false"></asp:TextBox>           
                                <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Inicio_FilteredTextBoxExtender" 
                                    runat="server" TargetControlID="Txt_Fecha_Inicio" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                                <cc1:CalendarExtender ID="Txt_Fecha_Inicio_CalendarExtender" runat="server" 
                                    TargetControlID="Txt_Fecha_Inicio" PopupButtonID="Btn_Fecha_Inicio" Format="dd/MMM/yyyy"/>
                                <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha Inicial"/>
                            </td>                                                        
                            <td>
                                Al&nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="80px" Enabled="false"></asp:TextBox>                            
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                                    TargetControlID="Txt_Fecha_Final" PopupButtonID="Btn_Fecha_Final" Format="dd/MMM/yyyy"/>
                                <asp:ImageButton ID="Btn_Fecha_Final" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha Final"/>
                            </td>        
                            <td>
                                Estatus&nbsp;&nbsp;
                                <asp:DropDownList ID="Cmb_Estatus_Busqueda" runat="server">
                                </asp:DropDownList>
                            </td>                                                          
                            <td>
                                Folio&nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Orden_Compra_Busqueda" runat="server"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked" WatermarkText="<OC-0>" TargetControlID="Txt_Orden_Compra_Busqueda" />
                                <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                    OnClick="Btn_Buscar_Click" AlternateText="Consultar"/>
                            </td>                                                      
                        </tr>    
                        <tr>
                            <td>
                            &nbsp;
                            </td>
                        </tr>
                        
                    </table>                                          
                </div> 
                <div id="Div_Ordenes_Compra" runat="server" visible="true">    <%--Area de Resultado--%>   
                   <table style="width: 100%;">
                       <tr>
                           <td>
                                 <asp:GridView ID="Grid_Ordenes_Compra" runat="server"
                                    AutoGenerateColumns="false"
                                    CssClass="GridView_1" GridLines="None"
                                    Width="100%" AllowPaging="True"  
                                    DataKeyNames="NO_ORDEN_COMPRA,LISTA_REQUISICIONES,TIPO_ARTICULO,TOTAL,TIPO_PROCESO,FOLIO,ESTATUS,NO_RESERVA,CODIGO,JUSTIFICACION_COMPRA"
                                    PageSize="10" 
                                    onpageindexchanging="Grid_Ordenes_Compra_PageIndexChanging" 
                                    onselectedindexchanged="Grid_Ordenes_Compra_SelectedIndexChanged">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>          
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="5%" />
                                        </asp:ButtonField>                                                                
                                        <asp:BoundField DataField="NO_ORDEN_COMPRA" HeaderText="numero" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%"/>
                                        </asp:BoundField>                                    
                                        <asp:BoundField DataField="FOLIO" HeaderText="Folio" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="9%"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LISTA_REQUISICIONES" HeaderText="Req." Visible="true" DataFormatString="{0:RQ-0}">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="9%"/>
                                        </asp:BoundField>                                          
                                        <asp:BoundField DataField="FECHA_CREO" HeaderText="Fecha" Visible="true" DataFormatString="{0:dd/MMM/yyy}">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%"/>
                                        </asp:BoundField>                                        
                                        <asp:BoundField DataField="NOMBRE_PROVEEDOR" HeaderText="Proveedor" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SUBTOTAL" HeaderText="Subtotal" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="8%"/>
                                        </asp:BoundField>                                                    
                                        <asp:BoundField DataField="TOTAL_IEPS" HeaderText="IEPS" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="8%"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TOTAL_IVA" HeaderText="IVA" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="8%"/>
                                        </asp:BoundField>              
                                        <asp:BoundField DataField="TOTAL" HeaderText="Total" Visible="true" DataFormatString="{0:C}">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="8%"/>
                                        </asp:BoundField>            
                                        <asp:BoundField DataField="NO_RESERVA" HeaderText="No. Reserva" Visible="true">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="12%"/>
                                        </asp:BoundField>                                                              
                                                                                                                                                                                                                                                                                                                                 
                                        <asp:BoundField DataField="TIPO_ARTICULO" HeaderText="Tipo" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>                           
                                        <asp:BoundField DataField="TIPO_PROCESO" HeaderText="Tipo Proceso" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>                                                                   
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CODIGO" HeaderText="Codigo" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>    
                                        <asp:BoundField DataField="JUSTIFICACION_COMPRA" HeaderText="Justify" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>                                                                               
                                    </Columns>
                                    <RowStyle CssClass="GridItem" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>                                                                
                           </td>
                           <td>
                               &nbsp;
                           </td>
                       </tr>

                   </table>
                </div>    <%--Area de trabajo--%>                    
                
                
                <div id="Div_Articulos" runat="server" visible="true">  
                    <table style="width: 100%;">
                        <tr align="left">
                            <td colspan="2" class="label_titulo"></td>
                        </tr>      
                        <tr>
                            <td colspan="2">
                            <table style="width: 100%;">
                                <tr>
                                    <td align="left" style="width:15%">
                                        No. Orden Compra
                                    </td>                                                                    
                                    <td align="left" >
                                        <asp:TextBox ID="Txt_Proceso_Compra" runat="server" Width="95%" Enabled="false" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="Txt_Identificador_Compra" runat="server" Width="95%" ReadOnly="true"></asp:TextBox>
                                    <td align="left" style="width:15%;" >
                                        <asp:Label ID="Lbl_No_Reserva" runat="server" Text="*No. de Reserva" ForeColor="Blue"></asp:Label>
                                    </td>                                                                    
                                    <td align="left" >
                                        <asp:TextBox ID="Txt_No_Reserva" runat="server" Width="95%" ></asp:TextBox>
                                    </td>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td align="left" style="width:15%">
                                         No. Requisición
                                    </td>
                                    <td align="left" style="width:35%">                                        
                                        <asp:TextBox ID="Txt_Listado_Requisiciones" runat="server" Width="95%" Wrap="true"
                                         ReadOnly="true"></asp:TextBox>                                        
                                    </td>                                
                                    <td align="left" style="width:15%">
                                        <asp:Label ID="Lbl_Estatus" runat="server" Text="*Estatus" ForeColor="Blue"></asp:Label>
                                    </td>                                                                                                        
                                    <td align="left" style="width:35%">
                                         <asp:DropDownList ID="Cmb_Estatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Estatus_SelectedIndexChanged"
                                             Width="98%">
                                         </asp:DropDownList>
                                    </td>                                                                    
                                </tr>                              
                                <tr>
                                    <td align="left">
                                        Código Programatico
                                    </td>                                                                                            
                                    <td align="left" colspan="3">  
                                        <asp:TextBox ID="Txt_Codigo_Programatico" runat="server" Width="98%" Wrap="true"
                                        ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>                              
                                <tr>
                                    <td align="left">
                                         Justificación
                                    </td>                                                                                            
                                    <td align="left" colspan="3">  
                                        <asp:TextBox ID="Txt_Comentarios" runat="server" Width="98%" Height="52px" Wrap="true"
                                            ReadOnly="true" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="Lbl_Motivo_Rechazo" runat="server" Text="*Ingrese el motivo por el cual rechaza la requisición"
                                            ForeColor="Blue" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:TextBox ID="Txt_Motivo_Rechazo" runat="server" Height="90px" Width="100%" Visible="false"
                                            BorderStyle="Solid"></asp:TextBox>
                                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="Txt_Motivo_Rechazo"
                                            WatermarkText="&lt;Límite de Caracteres 3000&gt;" WatermarkCssClass="watermarked">
                                        </cc1:TextBoxWatermarkExtender>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Justificacion" runat="server" TargetControlID="Txt_Motivo_Rechazo"
                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ./*-!$%&()=,[]{}+<>@?¡?¿# ">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                            </td>
                        </tr>
                                                      
                        <tr>
                            <td style="width:99%" align="center" colspan="2">
                                 <asp:GridView ID="Grid_Detalles_Compra" runat="server"
                                    AutoGenerateColumns="false" DataKeyNames="ID"
                                    CssClass="GridView_1" GridLines="None"
                                    Width="100%" AllowPaging="True" 
                                    onpageindexchanging="Grid_Detalles_Compra_PageIndexChanging"
                                    PageSize="5" Visible="false">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="Clave" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%"/>
                                        </asp:BoundField>                                    
                                        <asp:BoundField DataField="CLAVE" HeaderText="Clave" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="5%"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Descripción" Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="5%"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" Visible="true">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%"/>
                                        </asp:BoundField>            
                                        <asp:BoundField DataField="NOMBRE_PROVEEDOR" HeaderText="Proveedor" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%"/>
                                        </asp:BoundField>                                                                                                     
                                        <asp:BoundField DataField="IMPORTE_TOTAL_CON_IMP_COT" HeaderText="Costo" Visible="true" DataFormatString="{0:0.00}">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" Width="5%"/>
                                        </asp:BoundField>                                                                                                                                                                                                                                                             
                                    </Columns>
                                    <RowStyle CssClass="GridItem" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                             </td>
                        </tr>
                        <tr>
                            <td align="right" colspan = "2">
                                Total Orden de Compra
                                <asp:TextBox ID="Txt_Total" runat="server" ReadOnly="true" Style="text-align:right"></asp:TextBox>
                            </td>
                        </tr>                        
                        <tr>
                            <td colspan="2">
                              <hr class="linea" />
                            </td>
                        </tr>                        

                    </table>
                </div>                  
                
                
                                                                              
           </ContentTemplate>           
      </asp:UpdatePanel>
  </div>

</asp:Content>


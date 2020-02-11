<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Alm_Requisiciones_Parciales_Stock.aspx.cs" Inherits="paginas_Almacen_Frm_Ope_Alm_Requisiciones_Parciales_Stock" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   <script type="text/javascript" language="javascript">
       function calendarShown(sender, args) {
           sender._popupBehavior._element.style.zIndex = 10000005;
       }
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
    <ContentTemplate>
             <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                  <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" />
                    </div>
                </ProgressTemplate>
           </asp:UpdateProgress>
           <div id="Div_General" style="background-color:#ffffff; width:98%; height:100%;"> <%--Fin del div General--%>
            <table  border="0" cellspacing="0" class="estilo_fuente" frame="border" width="100%"> <%--Tabla Generar Inventario--%>
                    <tr style="width:98%" align="center">
                         <td colspan ="2" class="label_titulo"> <asp:Label ID="Lbl_Titulo" runat="server" Text="Cancelar Requisiciones Stock" class="label_titulo"></asp:Label> </td>
                    </tr>
                    <tr style="width:98%">
                        <td colspan="2">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%">
                              <tr>
                                <td colspan="2" align="left" width="80%">
                                    <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top">
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                   
                                </td>
                              </tr>          
                            </table>                   
                          </div>                          
                        </td>
                    </tr> 
                    <tr style="width:98%">
                        <td style="width:63%" >&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" style="width:98%" >
                            <td align="left" style="width:63%" colspan="2" >
                                    <asp:ImageButton ID="Btn_Cancelar_Requisicion" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" Width="24px" CssClass="Img_Button" 
                                    AlternateText="NUEVO" ToolTip="Cancelar Requisición" 
                                    OnClientClick="return confirm('¿Está Seguro de Cancelar Requisiciones?');"
                                    OnClick="Btn_Cancelar_Requisicion_Click" Visible="true"/>
                                <asp:ImageButton ID="Btn_Salir" runat="server"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                    ToolTip="Salir" onclick="Btn_Salir_Click"/>                    
                            </td>    
                        </tr> 
                         <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                         TargetControlID="Txt_Req_Buscar" InvalidChars="<,>,&,',!," 
                         FilterType="Numbers"
                         Enabled="True">
                        </cc1:FilteredTextBoxExtender>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                        WatermarkCssClass="watermarked"
                        WatermarkText="<No. Requisición>"
                        TargetControlID="Txt_Req_Buscar" />
                        <tr>
                            <td>
                                
                                 <asp:HiddenField ID="Txt_Dependencia_ID" runat="server" />
                                 <asp:HiddenField ID="Txt_Proy_Prog_ID" runat="server" />
                                 
                            </td>
                        </tr>
                        
                  <div id="Div_Busqueda_Av" runat="server" style="width: 98%; height: 100%;"> 
                      <tr>
                         <td colspan="2">             
                                 <table border="0" cellspacing="0" class="estilo_fuente" frame="border" width="98%">
                                    <tr>                         
                                        <td align="left" >
                                            <asp:CheckBox ID="Chk_Fecha_B" runat="server" Text="Fecha" 
                                            oncheckedchanged="Chk_Fecha_B_CheckedChanged" AutoPostBack="true"/>
                                                &nbsp;&nbsp;   
                                         </td>
                                         <td align="left" >
                                             &nbsp;<asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="110px" 
                                            Enabled="False"></asp:TextBox>
                                            <asp:ImageButton ID="Img_Btn_Fecha_Inicio" runat="server" 
                                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" Enabled="False" 
                                                 ToolTip="Seleccione la Fecha Inicial" />
                                             &nbsp;&nbsp;&nbsp;&nbsp;
                                            <cc1:CalendarExtender ID="CalendarExtender1" OnClientShown="calendarShown" runat="server" TargetControlID="Txt_Fecha_Inicio" PopupButtonID="Img_Btn_Fecha_Inicio" Format="dd/MMM/yyyy">
                                            </cc1:CalendarExtender>
                                        <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="110px" Enabled="False"></asp:TextBox>
                                        <asp:ImageButton ID="Img_Btn_Fecha_Fin" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" Enabled="False" 
                                                ToolTip="Seleccione la Fecha Final" />
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="Txt_Fecha_Fin" PopupButtonID="Img_Btn_Fecha_Fin" OnClientShown="calendarShown" Format="dd/MMM/yyyy">
                                        </cc1:CalendarExtender>
                                             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                         <asp:TextBox ID="Txt_Req_Buscar" runat="server"  MaxLength="10" Width="120px"></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                            onclick="Btn_Buscar_Click" ToolTip="Buscar" />
                                            &nbsp;<asp:ImageButton ID="Btn_Limpiar" runat="server" Width="20px" 
                                            ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" 
                                            onclick="Btn_Limpiar_Click" ToolTip="Limpiar"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                 </table>
                         </td>
                    </tr>
                </div >
                           
                <div id="Div_Requisiciones_Parciales_Stock" runat="server" style="width: 97%; height: 100%;"> <!--Div_Requisiciones_Parciales_Stock--> 
                    <tr>
                        <td colspan="2">
                          <table width="100%">       
                            <tr>
                                <td>
                                <div style="overflow: auto; height: 380px; width: 99%; vertical-align: top; border-style: outset;
                                    border-color: Silver;">                                  
                                    <asp:GridView ID="Grid_Requisiciones_Parciales_Stock" runat="server" 
                                        AutoGenerateColumns="False" CellPadding="1" CssClass="GridView_1" 
                                        ForeColor="#333333" GridLines="None"  style="white-space:normal;" 
                                        Width="98%" DataKeyNames="ESTATUS">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:TemplateField>
                                                 <ItemTemplate>
                                                    <asp:ImageButton ID="Btn_Seleccionar_Requisicion" runat="server" 
                                                        CommandArgument='<%# Eval("NO_REQUISICION") %>' 
                                                        CommandName="Seleccionar_Requisicion" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png" 
                                                        OnClick="Btn_Seleccionar_Requisicion_Click" ToolTip="Ver" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" Font-Size="X-Small" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="FOLIO" 
                                                HeaderText="Requisición">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="110px" Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UNIDAD_RESPONSABLE" 
                                                HeaderText="Unidad Responsable" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA" DataFormatString="{0:dd/MMM/yyyy}" 
                                                HeaderText="Fecha">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MONTO" DataFormatString="{0:c}" HeaderText="Monto">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NO_REQUISICION" HeaderText="No. Requisición" Visible="False" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="110px" Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="true" >
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <ItemStyle Width="8%" Font-Size="X-Small" HorizontalAlign="Right"/>
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
                               
                               
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                     <tr>
                         <td colspan="2">
                              <table  style="width: 100%;" class="estilo_fuente"> 
                                  <tr>
                                      
                                      <td >
                                        <%--<asp:TextBox ID="Txt_Observaciones" runat="server"
                                          Width="96%" Height="50px" TextMode="MultiLine" MaxLength="240"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Txt_Observaciones_FilteredTextBoxExtender" 
                                        runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                        TargetControlID="Txt_Observaciones" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/ ">
                                        </cc1:FilteredTextBoxExtender>--%>

                                      </td>
                                  </tr>
                             </table>
                        </td>
                        </tr>
                           <caption>
                               <br />
                               </tr>
                           </caption>
                    </div> <!--Fin Div_Requisiciones_Stock-->

                    <div id="Div_Detalles_Requisicion_Parcial" runat="server" style="width: 97%; height: 100%;" visible="false"> <!--Div_Detalles_Requisicion--> 
                    <tr>
                        <td colspan= "2">
                            <table style="width: 99%;" class="estilo_fuente">
                                <tr>
                                    <td style="width:10%;">
                                       Folio
                                    </td>
                                    <td align="left" style="width:35%;">
                                        <asp:TextBox ID="Txt_Folio" runat="server" Enabled="false" Width="96%" TabIndex="1"></asp:TextBox>
                                    </td>
                                    <td style="width:10%;">
                                        Fecha
                                    </td>
                                    <td align="left" style="width:30%;">
                                        <asp:TextBox ID="Txt_Fecha_Surtido" runat="server" DataFormatString="{0:dd/MMM/yyyy}" 
                                        Enabled="false" Width="97%" TabIndex="2"></asp:TextBox>
                                    </td>
                                  </tr>
                                  <tr>
                                    <td >
                                      Unidad Responsable
                                    </td>
                                    <td align="left" >
                                        <asp:TextBox ID="Txt_Unidad_Responsable" runat="server" Width="96%" Enabled="False" 
                                        MaxLength="20" TabIndex="3"></asp:TextBox>
                                    </td>
                                        <td >
                                                Área
                                        </td>
                                        <td align="left"  >
                                            <asp:TextBox ID="Txt_Area" runat="server" Enabled="false" 
                                             Width="97%" TabIndex="4"></asp:TextBox>
                                        </td>
                                  </tr>
                                  <tr>   
                                         <td >
                                                Programa
                                        </td>
                                        <td align="left" >
                                            <asp:TextBox ID="Txt_Programa" runat="server" Enabled="false" 
                                             Width="96%" TabIndex="4"></asp:TextBox>
                                        </td>
                                        <td >
                                                Partida
                                        </td>
                                        <td align="left" >
                                            <asp:TextBox ID="Txt_Partida" runat="server" Enabled="false" 
                                             Width="97%" TabIndex="4"></asp:TextBox>
                                        </td>
                                  </tr>
                                  <tr>
                                        <td >          
                                            F. Financiamiento
                                        </td>
                                        <td align="left" colspan="3" >
                                            <asp:TextBox ID="Txt_Financiamiento" runat="server" Enabled="false" Width="99%"></asp:TextBox>
                                        </td>
                                  </tr>
                                
                                <tr >
                                    <td align="left" > Observaciones
                                    </td>
                                    <td align="left" colspan="3" >
                                         <asp:TextBox ID="Txt_Justificacion" runat="server"
                                          Width="99%" Height="55px" TextMode="MultiLine" MaxLength="249" 
                                          ReadOnly="True" Enabled="False"></asp:TextBox>
                                    </td>
                                 </tr>
                                 <tr>
                                      <td >
                                        <br />
                                   </td>
                                 </tr>
                                <tr align="right" class="barra_delgada">
                                    <td colspan="4" align="center">
                                    </td>
                                </tr> 
                                
                                
                             <div id="Div_Productos_Requisicion" runat="server" style="width: 97%; height: 100%;"> <!--Div_Detalles_Requisicion--> 
                                <tr>
                                   <td colspan="3">
                                        Productos Entregados<br />
                                   </td>
                                </tr> 
                                <tr>
                                    <td colspan="4">
                                            <asp:GridView ID="Grid_Productos_Requisicion" runat="server"  
                                            style="white-space:normal;"                                        
                                            AutoGenerateColumns="False" CellPadding="1" CssClass="GridView_1" 
                                            GridLines="None" Width="100%" >
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:BoundField DataField="CLAVE" HeaderText="Clave">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Center"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PRODUCTO" HeaderText="Producto"  
                                                    Visible="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Left"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Left"/>                                                </asp:BoundField>
                                                <asp:BoundField DataField="CANTIDAD_SOLICITADA" HeaderText="Ctd. Solicitada" 
                                                    Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Center"/>                                                </asp:BoundField>
                                                <asp:BoundField DataField="CANTIDAD_ENTREGADA" HeaderText="Ctd. Entregada" 
                                                    Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Center"/>                                                </asp:BoundField>
                                                <asp:BoundField DataField="UNIDADES" HeaderText="Unidad" >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Center"/>                                                </asp:BoundField>
                                                <asp:BoundField DataField="PRECIO" HeaderText="Precio">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Right"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SUB_TOTAL" HeaderText="Total" >
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
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
                                   <td colspan="4">
                                          <table>
                                                <tr>
                                                     <td style="width:93%;" align="right"> Subtotal</td>
                                                      <td  align="right">
                                                              <asp:Label ID="Lbl_SubTotal" runat="server" 
                                                              Text=" $ 0.00" ForeColor="Blue" BorderColor="Blue" 
                                                              BorderWidth="2px" Width="90px">
                                                                </asp:Label>
                                                      </td>
                                                 </tr>
                                                <tr>
                                                    <td align="right">IVA&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                    <td align="right">
                                                        <asp:Label ID="Lbl_IVA" runat="server" 
                                                        Text=" $ 0.00" ForeColor="Blue" BorderColor="Blue" 
                                                        BorderWidth="2px" Width="90px" >
                                                        </asp:Label>
                                                    </td>
                                                  </tr>
                                                <tr>
                                                     <td align="right"> Total&nbsp;&nbsp;&nbsp;&nbsp;
                                                     </td>
                                                      <td align="right">
                                                          <asp:Label ID="Lbl_Total" runat="server" 
                                                          Text=" $ 0.00" ForeColor="Blue" BorderColor="Blue" 
                                                          BorderWidth="2px" Width="90px" >
                                                          </asp:Label>
                                                       </td>
                                                   </tr>
                                           </table>
                                    </td>
                                  </tr>
                             </div>          
                                  
                             <div id="Div_Productos_A_Cancelar" runat="server" style="width: 97%; height: 100%;"> <!--Div_Detalles_Requisicion--> 
                                <tr>
                                   <td colspan="3">
                                        Productos a Cancelar<br />
                                   </td>
                                </tr> 
                                <tr>
                                    <td colspan="4">
                                            <asp:GridView ID="Grid_Productos_A_Cancelar" runat="server"  
                                            style="white-space:normal;"                                        
                                            AutoGenerateColumns="False" CellPadding="1" CssClass="GridView_1" 
                                            GridLines="None" Width="100%" >
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:BoundField DataField="CLAVE" HeaderText="Clave">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Center"/>                                                </asp:BoundField>
                                                <asp:BoundField DataField="PRODUCTO" HeaderText="Producto" 
                                                    Visible="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Left"/>                                                </asp:BoundField>
                                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Left"/>                                                </asp:BoundField>
                                                <asp:BoundField DataField="CANTIDAD_CANCELAR" HeaderText="Ctd. Cancelar" 
                                                    Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Center"/>                                                </asp:BoundField>
                                                <asp:BoundField DataField="UNIDADES" HeaderText="Unidad" >
                                                    <ItemStyle Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PRECIO" HeaderText="Precio">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Right"/>                                                </asp:BoundField>
                                                <asp:BoundField DataField="SUB_TOTAL" HeaderText="Total" >
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
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
                                   <td colspan="4">
                                          <table>
                                                <tr>
                                                     <td style="width:93%;" align="right"> Subtotal</td>
                                                      <td  align="right">
                                                              <asp:Label ID="Lbl_Subtotal_PC" runat="server" 
                                                              Text=" $ 0.00" ForeColor="Blue" BorderColor="Blue" 
                                                              BorderWidth="2px" Width="90px">
                                                                </asp:Label>
                                                      </td>
                                                 </tr>
                                                <tr>
                                                    <td align="right">IVA&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                    <td align="right">
                                                        <asp:Label ID="Lbl_Iva_PC" runat="server" 
                                                        Text=" $ 0.00" ForeColor="Blue" BorderColor="Blue" 
                                                        BorderWidth="2px" Width="90px" >
                                                        </asp:Label>
                                                    </td>
                                                  </tr>
                                                <tr>
                                                     <td align="right"> Total&nbsp;&nbsp;&nbsp;&nbsp;
                                                     </td>
                                                      <td align="right">
                                                          <asp:Label ID="Lbl_Total_PC" runat="server" 
                                                          Text=" $ 0.00" ForeColor="Blue" BorderColor="Blue" 
                                                          BorderWidth="2px" Width="90px" >
                                                          </asp:Label>
                                                       </td>
                                                   </tr>
                                           </table>
                                    </td>
                                  </tr>
                             </div>   
                              </table>
                              
                               <div id="Div_Justificacion" visible="false" runat="server" style="overflow:auto;vertical-align:top;border-style:none;border-color:Silver;" >    
                                       <table width ="98%">
                                        <tr>
                                            <td> 
                                           
                                            </td> 
                                        </tr>     
                                          <tr align="center" class="barra_delgada">
                                               <td  align="center" colspan="4"> 
                                               <asp:Label ID="lbl_Justificacion" runat="server"  Text="Motivo de Cancelación" />
                                                </td>
                                          </tr>
                                          <tr>
                                            <td align="left">
                                                <asp:TextBox ID="Txt_Observaciones" runat="server" Height="133px" 
                                                    MaxLength="200" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                            
                                                    <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                                    runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                    TargetControlID="Txt_Observaciones" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/ ">
                                                    </cc1:FilteredTextBoxExtender>
                                            
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observaciones" runat="server" TargetControlID ="Txt_Observaciones" 
                                                    WatermarkText="Límite de Caracteres 250" WatermarkCssClass="watermarked" Enabled="True"/> 
                                            </td>
                                          </tr>
                                         </table>            
                               </div>
                              
                       </td>
                
                    </tr>
                   </div> <!--Fin Div_Detalles_Requisicion--> 
                 

            </table>
            </div>
  </ContentTemplate>
     <%--<Triggers>
             <asp:AsyncPostBackTrigger  ControlID="Btn_Aceptar" EventName="Click"/>
     </Triggers>--%>
 </asp:UpdatePanel>
 
          <%-- <asp:UpdatePanel ID="Udp_Modal_Popup" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <cc1:ModalPopupExtender ID="Modal_Busqueda" runat="server" BackgroundCssClass="popUpStyle" 
                    PopupControlID="Pnl_Busqueda" TargetControlID="Btn_Comodin_Open" 
                    CancelControlID="Btn_Cancelar_Panel" DropShadow="True" DynamicServicePath="" Enabled="True" /> 
                   <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Close" runat="server" Text="" />
                   <asp:Button  Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Open" runat="server" Text="" />  
                </ContentTemplate>          
            </asp:UpdatePanel>  --%>
</asp:Content>


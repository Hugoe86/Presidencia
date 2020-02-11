<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" 
AutoEventWireup="true" 
CodeFile="Frm_Ope_Alm_Imprimir_Contra_Recibo.aspx.cs" 
Inherits="paginas_Almacen_Frm_Ope_Alm_Imprimir_Contra_Recibo" 
Title="Mostrar Contra Recibos"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" language="javascript">
        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True" />
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
          <div id="Div_Contenido" style="width: 99%; height: 100%;">
               <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                 <tr>
                   <td class="label_titulo">Reimprimir Contrarecibos de Almacén
                   </td>
                 </tr>
                <!--Bloque del mensaje de error-->
                <tr>
                    <td>
                        <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                            <table style="width:100%;">
                                <tr>
                                    <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                                        <asp:Image ID="Img_Warning" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                        Width="24px" Height="24px" />
                                    </td>            
                                    <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                                        <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="Red" />
                                    </td>
                                </tr> 
                            </table>                   
                        </div>
                    </td>
                </tr>
                <!--Bloque de la busqueda-->
                <tr class="barra_busqueda">
                    <td style="width:20%;">
                        <asp:ImageButton ID="Btn_Imprimir" runat="server" 
                         ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" CssClass="Img_Button" 
                         AlternateText="NUEVO" 
                         ToolTip="Exportar PDF"  OnClick="Btn_Imprimir_Click" Visible="false"/>  
                         <asp:ImageButton ID="Btn_Imprimir_Excel" runat="server" 
                         ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" Width="24px" CssClass="Img_Button" 
                         AlternateText="Imprimir Excel" 
                         ToolTip="Exportar Excel"
                         OnClick="Btn_Imprimir_Excel_Click" Visible="false"/>  
                        <asp:ImageButton ID="Btn_Salir" runat="server"  
                            CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                            onclick="Btn_Salir_Click" 
                            ToolTip="Salir"
                            AlternateText="Salir"/>
                    </td>                                 
                </tr>
                
                 <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                 TargetControlID="Txt_Busqueda" InvalidChars="<,>,&,',!," 
                                 FilterType="Numbers" 
                                 Enabled="True">   
                            </cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                WatermarkCssClass="watermarked"
                                 WatermarkText="<No. Orden Compra>"
                                TargetControlID="Txt_Busqueda" />
                                
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                             TargetControlID="Txt_Req_Buscar" InvalidChars="<,>,&,',!," 
                             FilterType="Numbers"
                             Enabled="True">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<No. Requisición>"
                                TargetControlID="Txt_Req_Buscar" />
                                
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                             TargetControlID="Txt_Contra_Recibo" InvalidChars="<,>,&,',!," 
                             FilterType="Numbers"
                             Enabled="True">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<No. Recibo>"
                                TargetControlID="Txt_Contra_Recibo" />
                                
              <div id="Div_Busqueda_Av" runat="server" style="width: 97%; height: 100%;"> 
                <tr>
                   <%-- <td colspan="2">&nbsp;</td>--%>
                     <table width="96%">
                     <tr>
                            <td align="left" style="width:10%;">
                                <asp:CheckBox ID="Chk_Proveedor" runat="server" Text="Proveedor" 
                                oncheckedchanged="Chk_Proveedor_CheckedChanged" AutoPostBack="true"/>
                                &nbsp;&nbsp;&nbsp;
                           </td>
                            <td colspan="2" align="left" style="width:60%;">
                                <asp:DropDownList ID="Cmb_Proveedores" runat="server" 
                                 Width="69%" Enabled="False"></asp:DropDownList>
                                 &nbsp;&nbsp;&nbsp;&nbsp;
                                 <asp:TextBox ID="Txt_Contra_Recibo" runat="server"  MaxLength="10" Width="120px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>                         
                            <td align="left">
                                <asp:CheckBox ID="Chk_Fecha_B" runat="server" Text="Fecha" 
                                oncheckedchanged="Chk_Fecha_B_CheckedChanged" AutoPostBack="true"/>
                                    &nbsp;&nbsp;   </td>
                                <td align="left" style="width:30%;">
                                 &nbsp;<asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="110px" 
                                Enabled="False"></asp:TextBox>
                                <asp:ImageButton ID="Img_Btn_Fecha_Inicio" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" Enabled="False" 
                                     ToolTip="Seleccione la Fecha Inicial" />
                                 &nbsp;&nbsp;&nbsp;&nbsp;
                                <cc1:CalendarExtender ID="Img_Btn_Fecha_Inicio_CalendarExtender" 
                                OnClientShown="calendarShown" runat="server" 
                                TargetControlID="Txt_Fecha_Inicio" 
                                PopupButtonID="Img_Btn_Fecha_Inicio" 
                                Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender>
                            <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="110px" Enabled="False"></asp:TextBox>
                            <asp:ImageButton ID="Img_Btn_Fecha_Fin" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" Enabled="False" 
                                    ToolTip="Seleccione la Fecha Final" />
                            <cc1:CalendarExtender ID="Img_Btn_Fecha_Fin_CalendarExtender" 
                            runat="server" TargetControlID="Txt_Fecha_Fin" 
                            PopupButtonID="Img_Btn_Fecha_Fin" 
                            OnClientShown="calendarShown" 
                            Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                            </td>
                            <td>
                            <asp:TextBox ID="Txt_Busqueda" runat="server"  MaxLength="10" Width="120px"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:TextBox ID="Txt_Req_Buscar" runat="server"  MaxLength="10" Width="120px"></asp:TextBox>
                                &nbsp;
                               <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                onclick="Btn_Buscar_Click" ToolTip="Buscar" AlternateText="CONSULTAR" />
                                <asp:ImageButton ID="Btn_Limpiar" runat="server" Width="20px" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" 
                                onclick="Btn_Limpiar_Click" ToolTip="Limpiar"  />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                        </tr>
                     </table>
                </tr>
              </div >
                
                
             <!-- <div id="Div_ContraRecibos" runat="server" style="width: 97%; height: 100%;"> <!--Div Ordenes de Compra--> 
             <div id="Div_Contra_Recibos" 
                style="overflow:auto;height:320px;width:97%;vertical-align:top;border-style:outset;border-color: Silver;" 
                visible="true" runat="server"> 
                <tr>
                    <td>                       
                        &nbsp;
                    </td>
                </tr>
                <tr>
                      <td align="center">
                            <asp:GridView ID="Grid_Contra_Recibos" runat="server" style="white-space:normal;"
                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                            onselectedindexchanged="Grid_Contra_Recibos_SelectedIndexChanged" 
                            PageSize="1" Width="100%" >
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png" Text="Ver" />
                                <asp:BoundField DataField="NO_CONTRA_RECIBO" HeaderText="Folio" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Font-Size="X-Small" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FECHA_CREO" DataFormatString="{0:dd/MMM/yyyy}" 
                                    HeaderStyle-HorizontalAlign="Left" HeaderText="Fecha  Elaboración" 
                                    ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FOLIO" HeaderText="O. Compra">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NO_REQUISICION" HeaderText="Requisición" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Font-Size="X-Small" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PROVEEDOR" HeaderStyle-HorizontalAlign="Left" 
                                    HeaderText="Proveedor">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NO_ORDEN_COMPRA" HeaderText="No_Orden_Compra" 
                                    Visible="False">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Font-Size="X-Small" />
                                </asp:BoundField>
                                <asp:BoundField DataField="USUARIO_CREO" HeaderText="Usuario Creo" Visible="False" >
                                    <ItemStyle Font-Size="X-Small" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TOTAL" HeaderText="Total" DataFormatString="{0:C}">
                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Right"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="TIPO_ARTICULO" HeaderText="Tipo Articulo" Visible="False" />
                                <asp:BoundField DataField="LISTADO_ALMACEN" HeaderText="Listado Almacen" Visible="False" />
                            </Columns>
                            <PagerStyle CssClass="GridHeader" />
                            <SelectedRowStyle CssClass="GridSelected" />
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                        </asp:GridView>
                      </td>
                </tr> 
             </div> <!-- Fin div contra Recibos-->
               
             <div id="Div_Detalles_CR" runat="server"  style="width: 97%; height: 100%;" visible = "true"> <!-- Div Detalles contra Recibos--> 
                <tr>
                  <td>
                      <asp:Label ID="Lbl_Detalles" runat="server" Text="Detalles"></asp:Label>
                  </td>
                </tr>
                <tr align="right" class="barra_delgada">
                    <td align="center">
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 100%;" class="estilo_fuente">
                            <tr>
                                <td>
                                    No. Contra Recibo
                                </td>
                                <td align="left" style="width:20%;">
                                    <asp:TextBox ID="Txt_No_Contra_Recibo" runat="server" Enabled="false" Width="89%"></asp:TextBox>
                                </td>
                                <td align="left" style="width:15%;">
                                   <asp:Label ID="Label2" runat="server" Text="Fecha Elaboración"></asp:Label>
                                 </td>
                                <td align="left" style="width:20%;">
                                     <asp:TextBox ID="Txt_Fecha_Resepcion" runat="server" 
                                     Enabled="false" Width="89%"></asp:TextBox>
                                 </td>
                              </tr>
                              <tr>
                                <td>
                                    Requisición
                                </td>
                                <td align="left" style="width:20%;">
                                    <asp:TextBox ID="Txt_No_Requisicion" runat="server" Enabled="false" Width="89%"></asp:TextBox>
                                </td>
                                <td align="left" style="width:15%;">
                                   <asp:Label ID="Lbl_Dependencia" runat="server" Text="Unidad Responsable "></asp:Label>
                                 </td>
                                <td align="left" style="width:20%;">
                                     <asp:TextBox ID="Txt_Unidad_Responsable" runat="server" 
                                     Enabled="false" Width="89%"></asp:TextBox>
                                 </td>
                              </tr>
                              <tr>
                                <td style="width:15%;">
                                   <asp:Label ID="Lbl_Usuario_Elaboro" runat="server" Text="Usuario Elaboró"></asp:Label>
                                </td>
                                <td align="left" style="width:20%;" colspan="3"  >
                                    <asp:TextBox ID="Txt_Usuario_Elaboro" runat="server" Width="96%" Enabled="False" 
                                    MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   <asp:Label ID="lbl_Proveedor" runat="server" Text="Proveedor"></asp:Label>
                                </td >
                                <td align="left" style="width:20%;" colspan="3">
                                    <asp:TextBox ID="Txt_Proveedor" runat="server" Width="96%" Enabled="False"></asp:TextBox>
                                </td>
                            </tr>
                            <tr >
                                <td align="left" > Observaciones
                                </td>
                                <td align="left" colspan="3" >
                                     <asp:TextBox ID="Txt_Observaciones" runat="server"
                                      Width="96%" Height="50px" TextMode="MultiLine" MaxLength="249" 
                                      ReadOnly="True" Enabled="False"></asp:TextBox>
                                </td>
                             </tr>
                            <tr>
                               <td colspan="4">
                                    <br />
                               </td>
                             </tr>
                            <tr align="right" class="barra_delgada">
                                <td colspan="4" align="center">
                                </td>
                            </tr> 
                            <tr>
                               <td colspan="4">
                                    Productos Orden de Compra<br />
                               </td>
                             </tr> 
                            <tr>
                                <td colspan="4">
                                    <asp:GridView ID="Grid_Productos_Orden_Compra" runat="server" 
                                    style="white-space:normal;"
                                    AutoGenerateColumns="False" CssClass="GridView_1" 
                                    GridLines="None"   Width="100%" PageSize="5" >
                                    <RowStyle CssClass="GridItem" Font-Size="Smaller" />
                                    <EmptyDataRowStyle Font-Size="Smaller" />
                                    <Columns>                                 
                                        <asp:BoundField DataField="PRODUCTO" HeaderStyle-HorizontalAlign="Left" 
                                            HeaderText="Producto">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPCION" 
                                            HeaderStyle-HorizontalAlign="Left" HeaderText="Descripción" 
                                            ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PRECIO" HeaderText="Precio U." DataFormatString="{0:c}">
                                            <ItemStyle HorizontalAlign="Right"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CANTIDAD" HeaderText="Ctd.">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UNIDAD" HeaderText="Unidad" />
                                        <asp:BoundField DataField="PRECIO_AC" HeaderText="Precio Ac." DataFormatString="{0:c}" >
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                    </Columns>
                                    <FooterStyle Font-Size="X-Small" />
                                    <PagerStyle CssClass="GridHeader" Font-Size="Smaller" />
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
                              
                            <tr>
                               <td colspan="2">
                                    <br />
                               </td>
                            </tr>
                            
                      
                                 <tr align="right" class="barra_delgada">
                                    <td colspan="4" align="center">
                                    </td>
                                 </tr>
                                 <tr>
                                     <td colspan="4">
                                        Facturas Proveedor
                                    </td>
                                 </tr>
                                  <tr>
                                     <td colspan="4">
                                         <asp:GridView ID="Grid_Facturas" runat="server" 
                                           AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                           PageSize="1" 
                                           style="white-space:normal;" Width="70%">
                                           <RowStyle CssClass="GridItem" />
                                           <Columns>
                                               <asp:BoundField HeaderText="No. Factura Proveedor" DataField="NO_FACTURA_PROVEEDOR" >
                                                   <HeaderStyle HorizontalAlign="Left" />
                                               </asp:BoundField>
                                               <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:c}" >
                                                   <HeaderStyle HorizontalAlign="Left" />
                                               </asp:BoundField>
                                               <asp:BoundField DataField="FECHA_FACTURA" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}" >
                                                   <HeaderStyle HorizontalAlign="Left" />
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
                                   <td colspan="2">
                                        <br />
                                   </td>
                                 </tr>
                      
                       
                            <tr align="right" class="barra_delgada">
                                <td colspan="4" align="center">
                                </td>
                            </tr> 
                            <tr>
                                <td colspan="4">
                                    Documentos Soporte
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:GridView ID="Grid_Doc_Soporte" runat="server" style="white-space:normal;" 
                                        AutoGenerateColumns="False" CellPadding="1" CssClass="GridView_1" 
                                        GridLines="None" 
                                         PageSize="5" 
                                        Width="100%" >
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:BoundField DataField="NOMBRE" HeaderText="Documento" 
                                                Visible="true">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="COMENTARIOS" HeaderText="Descripción">
                                                <HeaderStyle HorizontalAlign="Left" />
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
                   </td>
                </tr>
              </div> <!-- Fin del div Detalles contra recibos-->
            </table> <!--Tabla General-->
           </div>  <!--Div General-->
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


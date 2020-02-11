<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Alm_Imprimir_Recibo_Transitorio.aspx.cs" Inherits="paginas_Almacen_Frm_Ope_Alm_Imprimir_Recibo_Transitorio" Title="Mostrar Recibos Transitorios" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
 <%--       <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
               <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress">
                    <img alt="" src="../Imagenes/paginas/Updating.gif" />
                </div>
            </ProgressTemplate>
           </asp:UpdateProgress>--%>
           
          <div id="Div_Contenido" style="width: 97%; height: 100%;">
               <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
                 <tr>
                   <td class="label_titulo">Recibos Transitorios
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
                    <td style="width:20%;" colspan="2">
                        <asp:ImageButton ID="Btn_Imprimir" runat="server" 
                         ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" CssClass="Img_Button" 
                         AlternateText="NUEVO" 
                         ToolTip="Exportar PDF" 
                         OnClick="Btn_Imprimir_Click" Visible="true"/>  
                         <asp:ImageButton ID="Btn_Imprimir_Excel" runat="server" 
                         ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" Width="24px" CssClass="Img_Button" 
                         AlternateText="Imprimir Excel"
                         ToolTip="Exportar Excel"       
                         OnClick="Btn_Imprimir_Excel_Click" 
                         Visible="true"/>  
                        <asp:ImageButton ID="Btn_Salir"
                         runat="server" 
                         AlternateText="Salir"  
                         ToolTip="Salir"
                            CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                            onclick="Btn_Salir_Click"/>
                    </td>                                     
                </tr>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                 TargetControlID="Txt_OrdenC_Buscar" InvalidChars="<,>,&,',!," 
                                 FilterType="Numbers" 
                                 Enabled="True">   
                            </cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                WatermarkCssClass="watermarked"
                                 WatermarkText="<No. Orden Compra>"
                                TargetControlID="Txt_OrdenC_Buscar" />
                                
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
                             TargetControlID="Txt_ReciboT_Buscar" InvalidChars="<,>,&,',!," 
                             FilterType="Numbers"
                             Enabled="True">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<No. Recibo>"
                                TargetControlID="Txt_ReciboT_Buscar" />
                
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
                                 <asp:TextBox ID="Txt_ReciboT_Buscar" runat="server"  MaxLength="10" Width="120px"></asp:TextBox>
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
                            <asp:TextBox ID="Txt_OrdenC_Buscar" runat="server"  MaxLength="10" Width="120px"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:TextBox ID="Txt_Req_Buscar" runat="server"  MaxLength="10" Width="120px"></asp:TextBox>
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
                                Tipo 
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Tipo" runat="server" 
                                    >
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                        </tr>
                     </table>
                </tr>
              </div >
                
               <div id="Div_Recibos_Transitorios"  visible="false" runat="server" style="width: 97%; height: 100%;"> <!--Div Recibos Transitorios--> 
                <tr>
                </tr>
                <tr>
                     <td align="center" colspan="2">
                        <asp:GridView ID="Grid_Recibos_Transitorios" runat="server" style="white-space:normal;"
                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                            onselectedindexchanged="Grid_Recibos_Transitorios_SelectedIndexChanged" 
                            PageSize="1" Width="99%" >
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png" Text="Ver" />
                                <asp:BoundField DataField="NO_RECIBO" HeaderText="No. Recibo" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FECHA_CREO" DataFormatString="{0:dd/MMM/yyyy}" 
                                    HeaderStyle-HorizontalAlign="Left" HeaderText="Fecha" 
                                    ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FOLIO" HeaderText="O. Compra">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NO_REQUISICION" HeaderText="Requisición" >
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
                                <asp:BoundField DataField="NO_CONTRA_RECIBO" HeaderText="Contra Recibo" Visible="False" >
                                    <ItemStyle Font-Size="X-Small" />
                                </asp:BoundField>
                                <asp:BoundField DataField="USUARIO_CREO" HeaderText="Usuario Creo" Visible="False" >
                                    <ItemStyle Font-Size="X-Small" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TOTAL" HeaderText="Total" Visible="False" >
                                    <ItemStyle Font-Size="X-Small" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TIPO" HeaderText="Tipo" >
                                    <ItemStyle Font-Size="X-Small" />
                                </asp:BoundField>
                            </Columns>
                            <PagerStyle CssClass="GridHeader" />
                            <SelectedRowStyle CssClass="GridSelected" />
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                        </asp:GridView>
                      </td>
                </tr> 
                </div> <!-- Fin Div Recibos Transitorios-->     
                
                <div id="Div_Detalles_Recibo_Transitorio" runat="server" visible="false" style="width: 97%; height: 100%;"> <!-- Div Detalles Recibos Transitorios--> 
                <tr>
                      <td colspan ="2">
                        <br />
                      </td>
                </tr>
                <tr>
                      <td colspan ="2">
                          <asp:Label ID="Lbl_Detalles" runat="server" Text="Detalles"></asp:Label>
                      </td>
                </tr>
                <tr align="right" class="barra_delgada">
                    <td colspan="2" align="center">
                    </td>
                </tr>
                     <tr>
                        <td colspan="2">
                            <table style="width: 99%;" class="estilo_fuente">
                            <tr>
                                <td>
                                    No. Recibo
                                </td>
                                <td align="left" style="width:20%;">
                                    <asp:TextBox ID="Txt_No_Recibo" runat="server" Enabled="false" Width="89%"></asp:TextBox>
                                </td>
                                <td align="left" style="width:15%;">
                                   <asp:Label ID="Label2" runat="server" Text="Fecha Elaboró"></asp:Label>
                                 </td>
                                <td align="left" style="width:20%;">
                                     <asp:TextBox ID="Txt_Fecha_Elaboro" runat="server" 
                                     Enabled="false" Width="89%"></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                <td style="width:15%;">
                                   <asp:Label ID="Lbl_Usuario_Elaboro" runat="server" Text="Usuario Elaboró"></asp:Label>
                                </td>
                                <td align="left" style="width:20%;"  colspan="3">
                                    <asp:TextBox ID="Txt_Usuario_Elaboro" runat="server" Width="96%" Enabled="False" 
                                        MaxLength="20"></asp:TextBox>
                                </td>
                             </tr>
                             <tr>
                                <td>
                                    Orden Compra
                                </td>
                                <td align="left" style="width:20%;" colspan="1">
                                    <asp:TextBox ID="Txt_Folio_OrdenC" runat="server" Enabled="false" Width="89%"></asp:TextBox>
                                </td>
                                <td align="left" style="width:15%;">
                                   <asp:Label ID="lbl_Requisicion" runat="server" Text="Requisición"></asp:Label>
                                 </td>
                                <td align="left" style="width:20%;">
                                     <asp:TextBox ID="Txt_No_Requisicion" runat="server" 
                                     Enabled="false" Width="89%"></asp:TextBox>
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
                                <%--<td align="left" > Observaciones
                                </td>--%>
                              <%--  <td align="left" colspan="3" >
                                     <asp:TextBox ID="Txt_Observaciones" runat="server"
                                      Width="96%" Height="50px" TextMode="MultiLine" MaxLength="249" 
                                         Enabled="False"></asp:TextBox>
                                </td>--%>
                                     <%-- <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observaciones" runat="server" TargetControlID ="Txt_Observaciones" 
                                      WatermarkText="Límite de Caracteres 250" WatermarkCssClass="watermarked" Enabled="True"/> --%>
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
                                        Productos <br />
                                   </td>
                                 </tr>                                  
                                  <tr>
                                    <td colspan="4">
                                           <asp:GridView ID="Grid_Productos_RT_Unidad" runat="server" style="white-space:normal;" AutoGenerateColumns="False" CellPadding="1" 
                                                    CssClass="GridView_1"  GridLines="None" 
                                                    PageSize="1" 
                                                    Visible="False"
                                                    Width="99%">
                                                    <RowStyle CssClass="GridItem" />
                                                    <Columns>                                                     
                                                        <asp:BoundField DataField="PRODUCTO_ID" HeaderText="Producto_ID" 
                                                            Visible="false">
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="NO_SERIE" HeaderText="No. Serie" >
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="PRODUCTO" HeaderText="Producto" Visible="True">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" >
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="MATERIAL" HeaderText="Material" >
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="COLOR" HeaderText="Color" >
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="UNIDAD" HeaderText="Unidad">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="MONTO" DataFormatString="{0:c}" 
                                                            HeaderText="Precio" Visible="True">
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="NO_INVENTARIO" HeaderText="No Inventario" >
                                                            <ItemStyle Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>
                                           <asp:GridView ID="Grid_Productos_RT_Totalidad" runat="server" AutoGenerateColumns="False" CellPadding="1" CssClass="GridView_1" GridLines="None" 
                                               onselectedindexchanged="Grid_Productos_RT_Totalidad_SelectedIndexChanged" PageSize="1" style="white-space:normal;" Visible="False" Width="99%">
                                               <RowStyle CssClass="GridItem" />
                                               <Columns>
                                                   <asp:BoundField DataField="PRODUCTO_ID" HeaderText="Producto_ID" Visible="false">
                                                       <FooterStyle HorizontalAlign="Right" />
                                                       <HeaderStyle HorizontalAlign="Center" />
                                                       <ItemStyle HorizontalAlign="Right" />
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="PRODUCTO" HeaderText="Producto" Visible="True">
                                                       <HeaderStyle HorizontalAlign="Left" />
                                                       <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" />
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción">
                                                       <HeaderStyle HorizontalAlign="Left" />
                                                       <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" />
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="MODELO" HeaderText="Modelo" Visible="True">
                                                       <HeaderStyle HorizontalAlign="Left" />
                                                       <ItemStyle Font-Size="X-Small" />
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="CANTIDAD" HeaderText="C. Solicitada" Visible="true">
                                                       <HeaderStyle HorizontalAlign="Center" />
                                                       <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" />
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="EXISTENCIA" HeaderText="C. Existente">
                                                       <HeaderStyle HorizontalAlign="Center" />
                                                       <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" />
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="UNIDAD" HeaderText="Unidad">
                                                       <HeaderStyle HorizontalAlign="Left" />
                                                       <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" />
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="PRECIO" DataFormatString="{0:c}" HeaderText="Precio">
                                                       <FooterStyle HorizontalAlign="Right" />
                                                       <HeaderStyle HorizontalAlign="Center" />
                                                       <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" />
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
                                        &nbsp;</td>
                                  </tr>
                                 <tr>
                                   <td colspan="2">
                                        <br />
                                   </td>
                                 </tr>
                                 </div> <!-- Fin del div Detalles  Recibos Transitorios-->
                             </table>
                         </td>   
                     </tr>  
                </table> <!--Tabla General-->
           </div>  <!--Div General-->
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


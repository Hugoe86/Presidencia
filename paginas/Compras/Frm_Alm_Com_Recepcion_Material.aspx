<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Alm_Com_Recepcion_Material.aspx.cs" Inherits="paginas_Compras_Frm_Alm_Com_Recepcion_Material" Title="Recepción de Materiales" %>
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
           <div id="Div_Contenido" style="width: 97%; height: 100%;"> <!--Div Principal -->
             <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
                <tr>
                    <td colspan ="2" class="label_titulo">Recepci&oacute;n de Material del Proveedor</td>
                </tr>
                <!--Bloque del mensaje de error-->
                <tr>
                    <td colspan ="2">
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
                         AlternateText="Imprimir PDF" OnClick="Btn_Imprimir_Click" Visible="false" 
                         ToolTip="Exporta PDF Orden de Compra"/>  
                         <asp:ImageButton ID="Btn_Imprimir_Excel" runat="server" 
                         ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" Width="24px" CssClass="Img_Button" 
                         AlternateText="Imprimir Excel"
                         ToolTip="Exportar Excel Orden de Compra" 
                         OnClick="Btn_Imprimir_Excel_Click" Visible="false" />  
                         <asp:ImageButton ID="Btn_Salir" runat="server"  
                            CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                            onclick="Btn_Salir_Click" 
                            AlternateText="Salir"
                            ToolTip="Salir" />
                    </td>                    
                    <td align="right" style="width:80%;">
                        <div id="Div_Busqueda" runat="server">
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                             TargetControlID="Txt_Busqueda" InvalidChars="<,>,&,',!," 
                             FilterType="Numbers"
                             Enabled="True">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<No. Orden Compra>"
                                TargetControlID="Txt_Busqueda" />
                                
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                             TargetControlID="Txt_Req_Buscar" InvalidChars="<,>,&,',!," 
                             FilterType="Numbers"
                             Enabled="True">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<No. Requisición>"
                                TargetControlID="Txt_Req_Buscar" />
                        </div>
                    </td>
                </tr>
                
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
                                    Width="91%" Enabled="False"></asp:DropDownList></td>
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
                                <cc1:CalendarExtender ID="Img_Btn_Fecha_Inicio_CalendarExtender" OnClientShown="calendarShown" runat="server" TargetControlID="Txt_Fecha_Inicio" PopupButtonID="Img_Btn_Fecha_Inicio" Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender>
                            
                            <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="110px" Enabled="False"></asp:TextBox>
                            <asp:ImageButton ID="Img_Btn_Fecha_Fin" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" Enabled="False" 
                                    ToolTip="Seleccione la Fecha Final" />
                            <cc1:CalendarExtender ID="Img_Btn_Fecha_Fin_CalendarExtender" runat="server" TargetControlID="Txt_Fecha_Fin" PopupButtonID="Img_Btn_Fecha_Fin" OnClientShown="calendarShown" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                            </td>
                            <td>
                            <asp:TextBox ID="Txt_Busqueda" runat="server"  MaxLength="10" Width="120px"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;&nbsp;
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
                            </td>
                        </tr>
                     </table>
                </tr>
              </div >
                
              <div id="Div_Ordenes_Compra" runat="server" style="width: 97%; height: 100%;"> 
              <caption>
                    <br />
                <tr>
                    <td colspan="2">
                        <asp:Label ID="Lbl_Ordenes_Compra" Text="Órdenes de Compra" runat="server" CssClass="estilo_fuente" 
                            Visible="False" />
                    </td>
                </tr>
                    <!--Grid con las ordenes de compra-->
                 <tr>
                      <td align="center" colspan="2">
                            <div style="overflow: auto; height: 380px; width: 99%; vertical-align: top; border-style: outset;
                                border-color: Silver;">                      
                            <asp:GridView ID="Grid_Ordenes_Compra" runat="server" style="white-space:normal;" 
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                onpageindexchanging="Grid_Ordenes_Compra_PageIndexChanging" PageSize="20" 
                                Width="100%" onselectedindexchanged="Grid_Ordenes_Compra_SelectedIndexChanged"
                                DataKeyNames="TIPO_ARTICULO,REQUISICION,FOLIO">
                                <RowStyle CssClass="GridItem" Font-Size="Larger" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png" Text="Ver" />
                                    <asp:BoundField DataField="FOLIO" HeaderText="O. Compra" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FOLIO_REQ" HeaderText="Requisición" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PROVEEDOR" HeaderStyle-HorizontalAlign="Left" 
                                        HeaderText="Proveedor">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA" DataFormatString="{0:dd/MMM/yyyy}" 
                                        HeaderStyle-HorizontalAlign="Left" HeaderText="Fecha" 
                                        ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TOTAL" HeaderText="Total" DataFormatString="{0:c}">
                                        <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderStyle-HorizontalAlign="Left" 
                                        HeaderText="Estatus" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_ORDEN_COMPRA" HeaderText="No_Orden_Compra" 
                                        Visible="False" />
                                    <asp:BoundField DataField="TIPO_ARTICULO" HeaderStyle-HorizontalAlign="Left" 
                                        HeaderText="Articulo" ItemStyle-HorizontalAlign="Center" Visible="false">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>                                        
                                    <asp:BoundField DataField="REQUISICION" HeaderText="Rquisicion" Visible="False" />
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                          </div>  
                        </td>
                 </tr>
                </caption>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
            </div>

            <div id="Div_Detalles_Orden_C" runat="server" style="width: 97%; height: 100%;"> 
                <table style="width: 97%;" class="estilo_fuente">
                <!--Datos de la factura-->
                <tr>
                    <td align="left" style="width:15%;"> Folio
                    </td>
                    <td align="left" style="width:35%;">
                        <asp:TextBox ID="Txt_Folio" runat="server" 
                         Width="90%" MaxLength="10" Enabled="False" ></asp:TextBox>
                    </td>
                    <td style="width:15%;" align="left">Fecha
                    </td>
                    <td style="width:35%;">
                        <asp:TextBox ID="Txt_Fecha_Construccion" runat="server" MaxLength="20" 
                         Width="90%" Enabled="False"></asp:TextBox>
                    </td>
                </tr>
                   <tr>
                    <td align="left">Requisición
                    </td>
                    <td align="left" >
                        <asp:TextBox ID="Txt_Requisicion" runat="server" Enabled="false" Width="90%"></asp:TextBox>
                    </td>
                    <td align="left">Estatus
                    </td>
                    <td align="left" >
                        <asp:TextBox ID="Txt_Estatus" runat="server" Enabled="false" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left"> Proveedor
                    </td>
                    <td align="left" colspan="3">
                         <asp:TextBox ID="Txt_Proveedor" runat="server" Width="96%" 
                          Enabled="false"></asp:TextBox></td>
                    
                </tr>
             
                <tr>
                    <td align="left"> Observaciones
                    </td>
                    <td align="left" colspan="3">
                        <asp:TextBox ID="Txt_Observaciones" runat="server" 
                        Width="97%" Height="50px" TextMode="MultiLine" MaxLength="249"></asp:TextBox>
                        
                        <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                         runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                        TargetControlID="Txt_Observaciones" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/ ">
                        </cc1:FilteredTextBoxExtender>
                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observaciones" runat="server" TargetControlID ="Txt_Observaciones" 
                        WatermarkText="Límite de Caracteres 250" WatermarkCssClass="watermarked" Enabled="True"/> 
                    </td>
                        
                </tr>                                
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                 <tr align="right" class="barra_delgada">
                     <td align="center" colspan="4"></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                <tr>
                 <td colspan="4">
                     <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
                           <tr>

                             <td style="width:95%;" align="right" >
                                <asp:Label ID="lbl_Resepcion" CssClass="estilo_fuente" runat="server" Text="Recepción Completa"></asp:Label>         
                             </td> 
                             <td align="left">
                                <asp:ImageButton ID="Ibtn_Recepcion_Completa" 
                                runat="server" CssClass="Img_Button" 
                                onclick="Ibtn_Recepcion_Completa_Click" 
                                AlternateText="NUEVO" 
                                ToolTip="Aceptar Orden de Compra" 
                                OnClientClick="return confirm('¿Acepta la Orden de Compra?');"
                                ImageUrl="~/paginas/imagenes/paginas/accept.png"/>  
                             </td>
                           </tr>
                       </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                       <asp:Label ID="Lbl_productos" Text="Productos Orden de Compra" runat="server" CssClass="estilo_fuente"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                      <asp:GridView ID="Grid_Orden_Compra_Detalles" runat="server" 
                                style="white-space:normal;"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                           
                                Width="100%" PageSize="5">
                                <RowStyle CssClass="GridItem"  />
                                <Columns>
                                    <asp:BoundField DataField="CLAVE" HeaderText="Clave" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Font-Size="X-Small" Width="8%"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PRODUCTO" HeaderStyle-HorizontalAlign="Left" 
                                        HeaderText="Producto">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Font-Size="X-Small" Width="10%"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Font-Size="X-Small" Width="20%"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MODELO" HeaderStyle-HorizontalAlign="Left" 
                                        HeaderText="Modelo" Visible="False">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MARCA" HeaderStyle-HorizontalAlign="Left" 
                                        HeaderText="Marca" Visible="False">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CANTIDAD" HeaderText="Ctd." 
                                        ItemStyle-HorizontalAlign="Right">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="5%" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UNIDAD" HeaderStyle-HorizontalAlign="Left" 
                                        HeaderText="Unidad" >
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle Width="10%" Font-Size="X-Small" HorizontalAlign="Center"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="COSTO_REAL" DataFormatString="{0:c}" 
                                        HeaderStyle-HorizontalAlign="Left" HeaderText="P. Unitario" 
                                        ItemStyle-HorizontalAlign="Right">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" Width="8%" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MONTO" DataFormatString="{0:c}" 
                                        HeaderStyle-HorizontalAlign="Left" HeaderText="Importe" 
                                        ItemStyle-HorizontalAlign="Right">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" Width="8%" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TIPO" HeaderStyle-HorizontalAlign="Left" 
                                        HeaderText="Tipo" ItemStyle-HorizontalAlign="Center" Visible="False">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Font-Size="X-Small" />
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
                      <asp:GridView ID="Grid_Servicios" runat="server" 
                                style="white-space:normal;"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                Visible="false"
                                Width="100%" PageSize="5">
                                <RowStyle CssClass="GridItem"  />
                                <Columns>
   
                                    <asp:BoundField DataField="NOMBRE_PRODUCTO_SERVICIO" HeaderStyle-HorizontalAlign="Left" 
                                        HeaderText="Servicio">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Font-Size="X-Small" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="CANTIDAD" HeaderText="Ctd." 
                                        ItemStyle-HorizontalAlign="Right">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="5%" Font-Size="X-Small" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="PRECIO_U_SIN_IMP_COTIZADO" DataFormatString="{0:c}" 
                                        HeaderStyle-HorizontalAlign="Left" HeaderText="P. Unitario" 
                                        ItemStyle-HorizontalAlign="Right">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" Width="8%" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SUBTOTAL_COTIZADO" DataFormatString="{0:c}" 
                                        HeaderStyle-HorizontalAlign="Left" HeaderText="Importe" 
                                        ItemStyle-HorizontalAlign="Right">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" Width="8%" Font-Size="X-Small" />
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
                    <td align="right"> IEPS&nbsp;&nbsp;&nbsp;&nbsp; 
                    </td>
                     <td align="right">
                        <asp:Label ID="Lbl_IEPS" runat="server" align="right"
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
                </table>
            </div>
            </table>          
           </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


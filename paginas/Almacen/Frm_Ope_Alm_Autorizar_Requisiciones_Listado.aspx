<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Frm_Ope_Alm_Autorizar_Requisiciones_Listado.aspx.cs" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Inherits="paginas_Almacen_Frm_Ope_Alm_Autorizar_Requisiciones_Listado" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<script runat="server">

</script>

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
    <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
           <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                 <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                 <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
           </asp:UpdateProgress>
           <%--Div de Contenido --%>
           <div id="Div_Contenido" style="width: 97%; height: 100%;">
           <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
           
            <tr>
                <td colspan ="4" class="label_titulo">Autorizar Requisiciones Listado Stock</td>
            </tr>
             <%--Fila de div de Mensaje de Error --%>
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
            <%--Fila 3 Renglon de barra de Busqueda--%>
            <tr class="barra_busqueda">
                <td style="width:20%;">
                <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                onclick="Btn_Salir_Click"/>
                
                </td>
                
                <td align="right" colspan="3" style="width:80%;">
                        <div id="Div_Busqueda" runat="server" style="display:none">
                        <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                        ToolTip="Avanzada">Busqueda</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese un Folio>"
                                TargetControlID="Txt_Busqueda" />
                        <asp:ImageButton ID="Btn_Buscar" runat="server" AlternateText="Consultar"
                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                        />
                        </div>
                </td> 
                
            </tr>
            <tr>
            
           
                <td colspan="4">
                    <div ID="Div_Requisiciones" runat="server" style="width:100%;font-size:9px;" 
                        visible="true">
                        <table style="width:100%;">
                            <tr align="center">
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:GridView ID="Grid_Requisiciones" runat="server" 
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                            onselectedindexchanged="Grid_Requisiciones_SelectedIndexChanged" Width="98%"
                                            AllowSorting="True" OnSorting="Grid_Requisiciones_Sorting" HeaderStyle-CssClass="tblHead">
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                    HeaderText="Selecciona" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="FOLIO" HeaderText="Requisicion" Visible="True" SortExpression="FOLIO">
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FECHA_GENERACION" HeaderText="Fecha Generacion" 
                                                    Visible="True" SortExpression="FECHA_GENERACION">
                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TIPO" HeaderText="Tipo" Visible="True" SortExpression="TIPO">
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TIPO_ARTICULO" HeaderText="Tipo Articulo" 
                                                    Visible="True" SortExpression="TIPO_ARTICULO">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="True" SortExpression="ESTATUS">
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TOTAL" HeaderText="Monto Total" Visible="True" SortExpression="TOTAL">
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Right" Width="15%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                            </Columns>
                                           <SelectedRowStyle CssClass="GridSelected" />
                                           <PagerStyle CssClass="GridHeader" />
                                           <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
               <tr>
                   <td colspan="4">
                       <div ID="Div_Productos" runat="server" style="width:100%;font-size:9px;" 
                           visible="false">
                           <table style="width:100%;">
                               <tr>
                                   <td align="center" colspan="4">
                                       Datos Generales</td>
                               </tr>
                               <tr align="right" class="barra_delgada">
                                   <td align="center" colspan="4">
                                   </td>
                               </tr>
                               <tr>
                                   <td align="center" colspan="4">
                                   </td>
                               </tr>
                               <tr>
                                  
                                   <td>
                                       Folio</td>
                                   <td>
                                       <asp:TextBox ID="Txt_Folio" runat="server" Enabled="False" Width="250px"></asp:TextBox>
                                   </td>
                               </tr>
                               <tr>
                                   <td>
                                       Dependencia</td>
                                   <td>
                                       <asp:TextBox ID="Txt_Dependencia" runat="server" Enabled="False" Width="250px"></asp:TextBox>
                                   </td>
                                   <td>
                                       Fecha Generacion</td>
                                   <td>
                                       <asp:TextBox ID="Txt_Fecha_Generacion" runat="server" Enabled="False" 
                                           Width="250px"></asp:TextBox>
                                   </td>
                               </tr>
                               <tr>
                                   <td>
                                       Tipo</td>
                                   <td>
                                       <asp:TextBox ID="Txt_Tipo" runat="server" Enabled="False" Width="250px"></asp:TextBox>
                                   </td>
                                   <td>
                                       Tipo Articulo
                                   </td>
                                   <td>
                                       <asp:TextBox ID="Txt_Tipo_Articulo" runat="server" Enabled="false" 
                                           Width="250px"></asp:TextBox>
                                   </td>
                               </tr>
                               <tr>
                                   <td colspan="2">
                                   </td>
                                   <td colspan="2">
                                       <asp:CheckBox ID="Chk_Verificacion" runat="server" Enabled="false" 
                                           Text="Verificar las características, garantías y pólizas" />
                                   </td>
                               </tr>
                               <tr>
                                   <td>
                                       Justificación
                                       <br />
                                       de la Compra</td>
                                   <td colspan="3">
                                       <asp:TextBox ID="Txt_Justificacion" runat="server" Enabled="False" 
                                           TabIndex="10" TextMode="MultiLine" Width="95%"></asp:TextBox>
                                       <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" 
                                           TargetControlID="Txt_Justificacion" WatermarkCssClass="watermarked" 
                                           WatermarkText="&lt;Indica el motivo de realizar la requisición&gt;" />
                                   </td>
                               </tr>
                               <tr>
                                   <td>
                                       Especificaciones
                                       <br />
                                       Adicionales</td>
                                   <td colspan="3">
                                       <asp:TextBox ID="Txt_Especificacion" runat="server" Enabled="False" 
                                           TabIndex="10" TextMode="MultiLine" Width="95%"></asp:TextBox>
                                       <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" 
                                           TargetControlID="Txt_Especificacion" WatermarkCssClass="watermarked" 
                                           WatermarkText="&lt;Especificaciones de los productos&gt;" />
                                   </td>
                               </tr>
                               <tr>
                                   <td align="center" colspan="4">
                                       Productos y Servicios</td>
                               </tr>
                               <tr align="right" class="barra_delgada">
                                   <td align="center" colspan="4">
                                   </td>
                               </tr>
                               <tr>
                                   <td align="center" colspan="4">
                                       <div ID="Div_1" runat="server" 
                                           style="overflow:auto;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">
                                           <asp:GridView ID="Grid_Productos" runat="server" AutoGenerateColumns="False" 
                                               CssClass="GridView_1" GridLines="None" Width="95%"
                                               AllowSorting="True" OnSorting="Grid_Productos_Sorting" HeaderStyle-CssClass="tblHead">
                                               <Columns>
                                                   <asp:BoundField DataField="Clave" HeaderText="Clave" 
                                                       Visible="True" SortExpression="Clave" >
                                                       <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                       <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small"/>
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="Producto" HeaderText="Producto/Servicio" 
                                                       Visible="True" SortExpression="Producto">
                                                       <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                       <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small"/>
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" 
                                                       Visible="True" SortExpression="Producto">
                                                       <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                       <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small"/>
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" Visible="True" SortExpression="Cantidad">
                                                       <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                       <ItemStyle HorizontalAlign="Center" Width="10%" Font-Size="X-Small"/>
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="Precio_Unitario" HeaderText="Costo Promedio" DataFormatString="{0:C}"
                                                       Visible="True" SortExpression="Precio_Unitario">
                                                       <HeaderStyle HorizontalAlign="Right" Width="20%" />
                                                       <ItemStyle HorizontalAlign="Right" Width="20%" Font-Size="X-Small"/>
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="Importe_S_I" HeaderText="Importe" Visible="True" DataFormatString="{0:C}" SortExpression="Importe">
                                                       <HeaderStyle HorizontalAlign="Right" Width="20%" />
                                                       <ItemStyle HorizontalAlign="Right" Width="20%" Font-Size="X-Small"/>
                                                   </asp:BoundField>
                                               </Columns>
                                               <SelectedRowStyle CssClass="GridSelected" />
                                               <PagerStyle CssClass="GridHeader" />
                                               <AlternatingRowStyle CssClass="GridAltItem" />
                                           </asp:GridView>
                                       </div>
                                   </td>
                               </tr>
                               <tr>
                                   <td>
                                   </td>
                                   <td>
                                   </td>
                                   <td>
                                       &nbsp;</td>
                                   <td align="right">
                                       SubTotal&nbsp;
                                       <asp:TextBox ID="Txt_Subtotal" runat="server" Enabled="False" 
                                           style="text-align:right" Width="150px"></asp:TextBox>
                                       &nbsp;&nbsp;&nbsp;&nbsp;
                                   </td>
                               </tr>
                               <tr>
                                   <td>
                                   </td>
                                   <td>
                                   </td>
                                   <td>
                                       &nbsp;</td>
                                   <td align="right">
                                       IVA&nbsp;
                                       <asp:TextBox ID="Txt_IVA" runat="server" Enabled="False" 
                                           style="text-align:right" Width="150px"></asp:TextBox>
                                       &nbsp;&nbsp;&nbsp;&nbsp;
                                   </td>
                               </tr>
                               <tr>
                                   <td>
                                   </td>
                                   <td>
                                   </td>
                                   <td>
                                       &nbsp;</td>
                                   <td align="right">
                                       Total&nbsp;
                                       <asp:TextBox ID="Txt_Total" runat="server" Enabled="False" 
                                           style="text-align:right" Width="150px"></asp:TextBox>
                                       &nbsp;&nbsp;&nbsp;&nbsp;
                                   </td>
                               </tr>
                               <tr>
                                   <td align="right" colspan="4">
                                       <div ID="Div_Productos_Cotizados" runat="server" visible="false">
                                           <center>
                                               Productos/Servicios Cotizados
                                               <div ID="Div_2" runat="server" 
                                                   style="overflow:auto;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">
                                                   <asp:GridView ID="Gri_Productos_Cotizados" runat="server" 
                                                       AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" Width="95%"
                                                       AllowSorting="True" OnSorting="Grid_Productos_Cotizados_Sorting" HeaderStyle-CssClass="tblHead">
                                                       <Columns>
                                                           <asp:BoundField DataField="Clave" HeaderText="Clave" 
                                                                Visible="True" SortExpression="Clave">
                                                                <FooterStyle HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                                           </asp:BoundField>
                                                           
                                                           <asp:BoundField DataField="Nombre" HeaderText="Producto/Servicio" SortExpression="Nombre">
                                                               <FooterStyle HorizontalAlign="Left" />
                                                               <HeaderStyle HorizontalAlign="Left" />
                                                               <ItemStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                                           </asp:BoundField>
                                                           <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True" SortExpression="Descripcion">
                                                               <FooterStyle HorizontalAlign="Left" />
                                                               <HeaderStyle HorizontalAlign="Left" />
                                                               <ItemStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                                           </asp:BoundField>
                                                           <asp:BoundField DataField="Nombre_Proveedor" HeaderText="Proveedor" SortExpression="Nombre_Proveedor">
                                                               <FooterStyle HorizontalAlign="Left" />
                                                               <HeaderStyle HorizontalAlign="Left" />
                                                               <ItemStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                                           </asp:BoundField>
                                                           <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad">
                                                               <FooterStyle HorizontalAlign="Center" />
                                                               <HeaderStyle HorizontalAlign="Center" />
                                                               <ItemStyle HorizontalAlign="Center" Font-Size="X-Small"/>
                                                           </asp:BoundField>
                                                           <asp:BoundField DataField="Precio_U_Sin_Imp_Cotizado" HeaderText="Costo S/I" DataFormatString="{0:C}" SortExpression="Precio_U_Sin_Imp_Cotizado">
                                                               <FooterStyle HorizontalAlign="Right" />
                                                               <HeaderStyle HorizontalAlign="Right" />
                                                               <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                           </asp:BoundField>
                                                           <asp:BoundField DataField="Precio_U_Con_Imp_Cotizado" HeaderText="Costo C/I" DataFormatString="{0:C}" SortExpression="Precio_U_Con_Imp_Cotizado" Visible="false">
                                                               <FooterStyle HorizontalAlign="Right" />
                                                               <HeaderStyle HorizontalAlign="Right" />
                                                               <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                           </asp:BoundField>
                                                           <asp:BoundField DataField="Subtotal_Cotizado" HeaderText="SubTotal Cotizado" DataFormatString="{0:C}" SortExpression="Subtotal_Cotizado">
                                                               <FooterStyle HorizontalAlign="Right" />
                                                               <HeaderStyle HorizontalAlign="Right" />
                                                               <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                           </asp:BoundField>
                                                           <asp:BoundField DataField="Total_Cotizado" HeaderText="Total Cotizado" DataFormatString="{0:C}" SortExpression="Total_Cotizado">
                                                               <FooterStyle HorizontalAlign="Right" />
                                                               <HeaderStyle HorizontalAlign="Right" />
                                                               <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                           </asp:BoundField>
                                                       </Columns>
                                                       <SelectedRowStyle CssClass="GridSelected" />
                                                       <PagerStyle CssClass="GridHeader" />
                                                       <AlternatingRowStyle CssClass="GridAltItem" />
                                                   </asp:GridView>
                                               </div>
                                           </center>
                                           
                                       </div>
                                   </td>
                               </tr>
                               <tr>
                                   <td>
                                   </td>
                                   <td>
                                   </td>
                                   <td>
                                       &nbsp;</td>
                                   <td align="right">
                                       Subtotal Cotizado&nbsp;
                                       <asp:TextBox ID="Txt_Subtotal_Cotizado" runat="server" Enabled="False" 
                                           style="text-align:right" Width="150px"></asp:TextBox>
                                       &nbsp;&nbsp;&nbsp;&nbsp;
                                   </td>
                               </tr>
                               <tr>
                                   <td>
                                   </td>
                                   <td>
                                   </td>
                                   <td>
                                       &nbsp;</td>
                                   <td align="right">
                                       IVA Cotizado&nbsp;
                                       <asp:TextBox ID="Txt_IVA_Cotizado" runat="server" Enabled="False" 
                                           style="text-align:right" Width="150px"></asp:TextBox>
                                       &nbsp;&nbsp;&nbsp;&nbsp;
                                   </td>
                               </tr>
                               <tr>
                                   <td>
                                   </td>
                                   <td>
                                   </td>
                                   <td>
                                       &nbsp;</td>
                                   <td align="right">
                                      Total Cotizado&nbsp;
                                           <asp:TextBox ID="Txt_Total_Cotizado" runat="server" Enabled="False" 
                                               style="text-align:right" Width="150px"></asp:TextBox>
                                           &nbsp;&nbsp;&nbsp;&nbsp;
                                   </td>
                               </tr>
                           </table>
                       </div>
                   </td>
               </tr>
               <tr>
                   <td colspan="4">
                       <%-- Div que contiene el agregar comentarios--%>
                       <div ID="Div_Comentarios" runat="server" style="width:100%;font-size:9px;" 
                           visible="false">
                           <table border="0" style="width:100%">
                               <tr align="right" class="barra_delgada">
                                   <td align="center" colspan="3">
                                   </td>
                               </tr>
                               <tr>
                                   <td align="center" colspan="3">
                                       Observaciones
                                   </td>
                               </tr>
                               <tr>
                                   <td style="width:10%;">
                                       Estatus</td>
                                   <td style="width:70%;">
                                       <asp:DropDownList ID="Cmb_Estatus" runat="server" Enabled="False" Width="255px">
                                       </asp:DropDownList>
                                   </td>
                                   <td align="right">
                                       <asp:ImageButton ID="Btn_Alta_Observacion" runat="server" Height="24px" 
                                           ImageUrl="~/paginas/imagenes/paginas/accept.png" AlternateText="Modificar"
                                           onclick="Btn_Alta_Observacion_Click" ToolTip="Evaluar" Width="24px" />
                                       <asp:ImageButton ID="Btn_Cancelar_Observacion" runat="server" Height="24px" 
                                           ImageUrl="../imagenes/paginas/icono_cancelar.png" 
                                           onclick="Btn_Cancelar_Observacion_Click" Width="24px" />
                                   </td>
                               </tr>
                               <tr>
                                   <td>
                                       Comentario&nbsp;
                                   </td>
                                   <td colspan="2">
                                       <asp:TextBox ID="Txt_Comentario" runat="server" MaxLength="250" TabIndex="10" 
                                           TextMode="MultiLine" Width="95%"></asp:TextBox>
                                       <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                                           TargetControlID="Txt_Comentario" WatermarkCssClass="watermarked" 
                                           WatermarkText="&lt;Límite de Caracteres 250&gt;" />
                                   </td>
                               </tr>
                               <tr>
                                   <td align="center" colspan="3">
                                       <asp:GridView ID="Grid_Comentarios" runat="server"
                                           AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                          onselectedindexchanged="Grid_Comentarios_SelectedIndexChanged" 
                                           Width="95%">
                                           <RowStyle CssClass="GridItem" />
                                           <Columns>
                                               <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                   ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                   <ItemStyle Width="5%" />
                                               </asp:ButtonField>
                                               <asp:BoundField DataField="Observacion_ID" Visible="True">
                                                   <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                   <ItemStyle Font-Size="0pt" ForeColor="White" HorizontalAlign="Left" 
                                                       Width="0%" />
                                               </asp:BoundField>
                                               <asp:BoundField DataField="Comentario" HeaderText="Comentarios" Visible="True">
                                                   <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                                   <ItemStyle HorizontalAlign="Left" Width="50%" Font-Size="X-Small"/>
                                               </asp:BoundField>
                                               <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True">
                                                   <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                   <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                               </asp:BoundField>
                                               <asp:BoundField DataField="Fecha_Creo" HeaderText="Fecha" Visible="True">
                                                   <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                   <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                               </asp:BoundField>
                                               <asp:BoundField DataField="Usuario_Creo" HeaderText="Usuario" Visible="True">
                                                   <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                   <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
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
                   </td>
               </tr>
            </table>
           </div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Ope_Com_Distribuir_Requisiciones_Prov.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Distribuir_Requisiciones_Prov" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
                <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
        </asp:UpdateProgress>
        <%--Div de Contenido --%>
        <div id="Div_Contenido" style="width:97%;height:100%;">
        <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
            <tr>
                <td colspan ="4" class="label_titulo">Distribuir Requisiciones a Proveedores</td>
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
            <%--Fila de Busqueda y Botones Generales --%>
            <tr class="barra_busqueda">
                    <td style="width:20%;" colspan="4">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                            ToolTip="Nuevo" onclick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Salir_Click"/>
                            <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/gridview/grid_print.png" 
                                onclick="Btn_Imprimir_Click"/>                              
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>   
            </tr>
            <tr>
                <td colspan="4">
                    <div id="Div_Grid_Requisiciones" style="width:100%;height:100%;" runat="server">
                        <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                            <tr>
                                <td style="width: 10%;">
                                    Cotizador(a)
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="Cmb_Cotizadores" runat="server" Width="100%" OnSelectedIndexChanged="Cmb_Cotizadores_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>  
                        </table>
                        <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                            <tr>
                                <td colspan="4">
                                <div ID="Div_1" runat="server" style="overflow:auto;height:500px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">
                                    <asp:GridView ID="Grid_Requisiciones" runat="server"
                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="Both" 
                                    onselectedindexchanged="Grid_Requisiciones_SelectedIndexChanged" 
                                    Width="99%" Enabled ="False" DataKeyNames="No_Requisicion"
                                    AllowSorting="True" OnSorting="Grid_Requisiciones_Sorting" OnRowDataBound="Grid_Requisiciones_RowDataBound"
                                    HeaderStyle-CssClass="tblHead" >
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select" Text="Ver Requisicion" HeaderText="Ver"
                                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                                <ItemStyle Width="5%" />
                                            </asp:ButtonField>
                                            
                                            <asp:BoundField DataField="No_Requisicion" HeaderText="No_Requisicion" Visible="false">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="true" SortExpression="Folio" ItemStyle-Wrap="true">
                                                <HeaderStyle HorizontalAlign="Left" Width="10%" Wrap="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="true" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Dependencia" HeaderText="U.Responsable" SortExpression="Dependencia" ItemStyle-Wrap="true"
                                                Visible="True">
                                                <HeaderStyle HorizontalAlign="Left" Width="25%" Wrap="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="25%" Wrap="true" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Concepto" HeaderText="Concepto" Visible="True" SortExpression="Concepto" 
                                            ItemStyle-Wrap="true">
                                                <FooterStyle HorizontalAlign="Left" Width="25%" Wrap="true"/>
                                                <HeaderStyle HorizontalAlign="Left" Width="25%" Wrap="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="25%" Wrap="true" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Tipo_Articulo" HeaderText="Tipo Articulo" Visible="True" SortExpression="Tipo_Articulo" 
                                            ItemStyle-Wrap="true">
                                                <FooterStyle HorizontalAlign="Left" Width="10%" Wrap="true"/>
                                                <HeaderStyle HorizontalAlign="Left" Width="10%" Wrap="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="10%"  Wrap="true" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            
                                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus" 
                                            ItemStyle-Wrap="true">
                                                <FooterStyle HorizontalAlign="Left" Width="10%" Wrap="true"/>
                                                <HeaderStyle HorizontalAlign="Left" Width="10%" Wrap="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="10%"  Wrap="true" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:C}"
                                                Visible="True" SortExpression="Total" ItemStyle-Wrap="true">
                                                <FooterStyle HorizontalAlign="Right" Width="10%" Wrap="true"/>
                                                <HeaderStyle HorizontalAlign="Right" Width="10%" Wrap="true"/>
                                                <ItemStyle HorizontalAlign="Right" Width="10%" Wrap="true" Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ALERTA" HeaderText="Alerta" Visible="false" SortExpression="Estatus">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>

                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Btn_Alerta" runat="server" ImageUrl="~/paginas/imagenes/gridview/circle_grey.png"/>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="3%" />
                                                <ItemStyle HorizontalAlign="Center" Width="3%" />
                                            </asp:TemplateField> 
                                            <asp:BoundField DataField="LEIDO" HeaderText="Leido" Visible="false" >
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>                                                                                      
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                <div ID="Div_Detalle_Requisicion" runat="server" style="width:100%;font-size:9px;" 
                    visible="false">
                    <table width="99%">
                        <tr>
                            <td align="center" colspan="4">
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%">
                                U.Responsable</td>
                            <td style="width:85%" colspan="3">
                                <asp:TextBox ID="Txt_Dependencia" runat="server" Enabled="False" Width="99%"></asp:TextBox>
                            </td>

                        </tr>
                        <tr>
                            <td>
                                Concepto</td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Concepto" runat="server" Enabled="False" Width="99%"></asp:TextBox>
                            </td>                        
                        </tr>
                        <tr>
                            <td style="width:15%">
                                Folio</td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Folio" runat="server" Enabled="False" Width="99%"></asp:TextBox>
                            </td>                            
                            <td style="text-align:right;">
                                Fecha Autorización</td>
                            <td>
                                <asp:TextBox ID="Txt_Fecha_Generacion" runat="server" Enabled="False" 
                                Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Tipo</td>
                            <td>
                                <asp:TextBox ID="Txt_Tipo" runat="server" Enabled="False" Width="99%"></asp:TextBox>
                            </td>
                            <td style="text-align:right;">
                                Tipo Artículo
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Tipo_Articulo" runat="server" Enabled="false" 
                                Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Estatus
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Estatus" runat="server" Enabled="false" 
                                Width="99%"></asp:TextBox>
                            </td>
                            <td style="text-align:right;">
                                Compra especial
                            </td>                            
                            <td >
                                <asp:CheckBox ID="Chk_Verificacion" runat="server" Enabled="false" Visible="false"
                                Text="Verificar las características, garantías y pólizas" />
                                <asp:TextBox ID="Txt_Compra_Especial" runat="server" Enabled="false" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Justificación
                            <br />
                                de la Compra</td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Justificacion" runat="server" Enabled="False" Height="80px"
                                TabIndex="10" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" 
                                TargetControlID="Txt_Justificacion" WatermarkCssClass="watermarked" 
                                WatermarkText="&lt;Indica el motivo de realizar la requisición&gt;" />
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td>
                                Especificaciones
                            <br />
                                Adicionales</td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Especificacion" runat="server" Enabled="False" 
                                TabIndex="10" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" 
                                TargetControlID="Txt_Especificacion" WatermarkCssClass="watermarked" 
                                WatermarkText="&lt;Especificaciones de los productos&gt;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                 <cc1:TabContainer ID="Tab_Detalle_Requisicion" runat="server" 
                                    ActiveTabIndex="0" Width="100%">
                                    <cc1:TabPanel ID="TabPnl_Productos" runat="server" Visible="true" Width="100%">
                                    <HeaderTemplate>Productos/Servicios</HeaderTemplate>
                                    <ContentTemplate>
                                    <table width="99%">
                                        <tr> 
                                             <td align="center" colspan="4">
                                                 <asp:GridView ID="Grid_Productos" runat="server"
                                                     AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                                     Width="99%"
                                                     AllowSorting="True" OnSorting="Grid_Productos_Sorting" >
                                                     <Columns>
                                                         <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" >
                                                             <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                             <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small"/>
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="Nombre" HeaderText="Producto/Servicio" 
                                                             SortExpression="Nombre">
                                                             <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                             <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small"/>
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" 
                                                             SortExpression="Descripcion">
                                                             <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                             <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small"/>
                                                         </asp:BoundField>
                                                          <asp:BoundField DataField="Unidad" HeaderText="Unidad" 
                                                             SortExpression="Unidad">
                                                             <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                             <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small"/>
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" 
                                                             SortExpression="Cantidad">
                                                             <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                             <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small"/>
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="Monto_Total" HeaderText="Precio Acumulado" DataFormatString="{0:C}"
                                                             SortExpression="Monto_Total">
                                                             <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                             <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small"/>
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="Monto" HeaderText="Importe S/I" DataFormatString="{0:C}"
                                                             SortExpression="Monto">
                                                             <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                             <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small"/>
                                                         </asp:BoundField>
                                                     </Columns>
                                                     <SelectedRowStyle CssClass="GridSelected" />
                                                     <HeaderStyle CssClass="tblHead" />
                                                     <PagerStyle CssClass="GridHeader" />
                                                     <AlternatingRowStyle CssClass="GridAltItem" />
                                                 </asp:GridView>
                                             </td>
                                        </tr>
                                        <tr>
                                             <td align="right" colspan="4">
                                                 Subtotal
                                                 <asp:TextBox ID="Txt_Subtotal" runat="server" Enabled="False" 
                                                     Style="text-align: right"></asp:TextBox>
                                             </td>
                                        </tr>
                                        <tr>
                                             <td align="right" colspan="4">
                                                 IEPS
                                                 <asp:TextBox ID="Txt_IEPS" runat="server" Enabled="False" 
                                                     Style="text-align: right"></asp:TextBox>
                                             </td>
                                        </tr>
                                        <tr>
                                             <td align="right" colspan="4">
                                                 IVA
                                                 <asp:TextBox ID="Txt_IVA" runat="server" Enabled="False" 
                                                     Style="text-align: right"></asp:TextBox>
                                             </td>
                                        </tr>
                                        <tr>
                                             <td align="right" colspan="4">
                                                 Total
                                                 <asp:TextBox ID="Txt_Total" runat="server" Style="text-align: right" 
                                                     Enabled="False"></asp:TextBox>
                                             </td>
                                        </tr>
                                        <tr>
                                             <td align="left" colspan="4">
                                                 <hr class="linea" />
                                             </td>
                                        </tr>
                                    
                                    </table>
                                    </ContentTemplate>
                                    </cc1:TabPanel>
                                    <cc1:TabPanel ID="TabPnl_Proveedores" runat="server" Visible="true" Width="99%">
                                    <HeaderTemplate>Proveedores</HeaderTemplate>
                                    <ContentTemplate>
                                    <table width="99%">
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:DropDownList ID="Cmb_Proveedores" runat="server" Width="99%">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td colspan="2"><asp:CheckBox ID="Chk_Enviar_Correo" runat="server" Text="Enviar Correo"/>
                                            </td>
                                            <td colspan="2" align="right">
                                                <asp:ImageButton ID="Btn_Agregar_Proveedor" runat="server" 
                                                ToolTip="Agregar Proveedor" CssClass="Img_Button" 
                                                ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                                    onclick="Btn_Agregar_Proveedor_Click" /> 
                                            </td>
                                        </tr>
                                        <tr> 
                                             <td align="center" colspan="4">
                                                 <asp:GridView ID="Grid_Proveedores" runat="server"
                                                     AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                                     Width="95%" DataKeyNames="Proveedor_ID" onselectedindexchanged="Grid_Proveedores_SelectedIndexChanged"
                                                     AllowSorting="True" OnSorting="Grid_Proveedores_Sorting" >
                                                     <Columns>
                                                        <asp:ButtonField ButtonType="Image" CommandName="Select" Text="Ver Requisicion" HeaderText="Ver"
                                                            ImageUrl="~/paginas/imagenes/paginas/delete.png">
                                                            <ItemStyle Width="5%" />
                                                        </asp:ButtonField>
                                                        <asp:BoundField DataField="Proveedor_ID" HeaderText="PROVEEDOR_ID" 
                                                             Visible="False">
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Nombre" HeaderText="Razón Social" SortExpression="Nombre" >
                                                             <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                                             <ItemStyle HorizontalAlign="Left" Width="25%" Font-Size="X-Small"/>
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="Compania" HeaderText="Nombre Comercial" 
                                                             SortExpression="Compania">
                                                             <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                                             <ItemStyle HorizontalAlign="Left" Width="25%" Font-Size="X-Small"/>
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="Telefonos" HeaderText="Telefonos" 
                                                             SortExpression="Telefonos">
                                                             <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                             <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small"/>
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="E_MAIL" HeaderText="Correo" 
                                                             SortExpression="E_MAIL">
                                                             <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                             <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small"/>
                                                         </asp:BoundField>
                                                     </Columns>
                                                     <SelectedRowStyle CssClass="GridSelected" />
                                                     <HeaderStyle CssClass="tblHead" />
                                                     <PagerStyle CssClass="GridHeader" />
                                                     <AlternatingRowStyle CssClass="GridAltItem" />
                                                 </asp:GridView>
                                             </td>
                                        </tr>
                                       
                                        
                                    </table>
                                    
                                    </ContentTemplate>
                                    </cc1:TabPanel>
                                    
                                     <cc1:TabPanel ID="Tab_Comentarios" runat="server" Visible="true" Width="99%">
                                    <HeaderTemplate>Comentarios</HeaderTemplate>
                                    <ContentTemplate>
                                    <table  width="99%">
                                    <tr>
                                        <td>
                                        <asp:GridView ID="Grid_Comentarios" runat="server"
                                           AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                           Width="95%">
                                           <RowStyle CssClass="GridItem" />
                                           <Columns>
                                               <asp:BoundField DataField="Comentario" HeaderText="Comentarios" Visible="True">
                                                   <HeaderStyle HorizontalAlign="Left" Width="50%" Font-Size="X-Small"/>
                                                   <ItemStyle HorizontalAlign="Left" Width="50%" Font-Size="X-Small"/>
                                               </asp:BoundField>
                                               <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True">
                                                   <HeaderStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                                   <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                               </asp:BoundField>
                                               <asp:BoundField DataField="Fecha_Creo" HeaderText="Fecha" Visible="True">
                                                   <HeaderStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                                   <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                               </asp:BoundField>
                                               <asp:BoundField DataField="Usuario_Creo" HeaderText="Usuario" Visible="True">
                                                   <HeaderStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
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
                                    
                                    </ContentTemplate>
                                    </cc1:TabPanel>
                                    
                                </cc1:TabContainer>
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

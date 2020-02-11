<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Ope_Com_Propuesta_Ganadora.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Propuesta_Ganadora" %>

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
                <td  colspan="4" class="label_titulo">Propuesta Ganadora</td>
            </tr>
            <%--Fila de div de Mensaje de Error --%>
            <tr>
                <td>
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
                            <asp:ImageButton ID="Btn_Modificar" runat="server" CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                            ToolTip="Modificar" onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Salir_Click"/>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>

            </tr>
            <tr>
                <td colspan="4">
                    <div id="Div_Grid_Requisiciones" style="width:100%;height:100%;" runat="server">
                    <asp:GridView ID="Grid_Requisiciones" runat="server"
                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                    onselectedindexchanged="Grid_Requisiciones_SelectedIndexChanged" 
                    Width="99%" Enabled ="False" DataKeyNames="No_Requisicion"
                    AllowSorting="True" OnSorting="Grid_Requisiciones_Sorting" 
                    HeaderStyle-CssClass="tblHead" >
                    <RowStyle CssClass="GridItem" />
                    <Columns>
                    <asp:ButtonField ButtonType="Image" CommandName="Select" Text="Ver Requisicion" HeaderText="Ver"
                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                    <ItemStyle Width="5%" />
                    </asp:ButtonField>
                    
                    <asp:BoundField DataField="No_Requisicion" HeaderText="No_Requisicion" Visible="false">
                    <FooterStyle HorizontalAlign="Right" />
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                    </asp:BoundField>
                    <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="true" SortExpression="Folio" ItemStyle-Wrap="true">
                    <HeaderStyle HorizontalAlign="Left" Width="10%" Wrap="true"/>
                    <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="true" Font-Size="X-Small"/>
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
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha Solicitud" 
                    Visible="True" SortExpression="Cotizador" ItemStyle-Wrap="true">
                    <FooterStyle HorizontalAlign="Left" Width="10%" Wrap="true"/>
                    <HeaderStyle HorizontalAlign="Left" Width="10%" Wrap="true"/>
                    <ItemStyle HorizontalAlign="Left" Width="10%"  Wrap="true" Font-Size="X-Small"/>
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
                            Unidad responsable</td>
                        <td style="width:35%">
                            <asp:TextBox ID="Txt_Dependencia" runat="server" Enabled="False" Width="99%"></asp:TextBox>
                        </td>
                        <td style="width:15%">
                            Folio</td>
                        <td style="width:35%">
                            <asp:TextBox ID="Txt_Folio" runat="server" Enabled="False" Width="99%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Concepto</td>
                        <td>
                            <asp:TextBox ID="Txt_Concepto" runat="server" Enabled="False" Width="99%"></asp:TextBox>
                        </td>
                        <td>
                            Fecha Generacion</td>
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
                        <td>
                            Tipo Articulo
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
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Enabled="false">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Compra especial
                        </td>                        
                        <td colspan="2">
                            <asp:CheckBox ID="Chk_Verificacion" runat="server" Enabled="false"  Visible="false"
                            Text="Verificar las características, garantías y pólizas" />
                            <asp:TextBox ID="Txt_Compra_Especial" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Justificación
                        <br />
                            de la Compra</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Justificacion" runat="server" Enabled="False" 
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
                        <td>
                            Proveedor
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="Cmb_Proveedores_Principal" runat="server" AutoPostBack="true" Width="100%"
                                OnSelectedIndexChanged="Cmb_Proveedores_Principal_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>                    
                    <tr>
                        <td colspan = "4">
                            <asp:GridView ID="Grid_Productos" runat="server"
                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"  HeaderStyle-CssClass="tblHead"
                            Width="99%" DataKeyNames="Ope_Com_Req_Producto_ID">
                            <Columns>
                            <asp:BoundField DataField="Ope_Com_Req_Producto_ID" HeaderText="Ope_Com_Req_Producto_ID" Visible="false">
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Porcentaje_Impuesto" HeaderText="Porcentaje_Impuesto" Visible="false">
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Subtotal_Cotizado" HeaderText="Subtotal_Cotizado" Visible="false">
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="IVA_Cotizado" HeaderText="IVA_Cotizado" Visible="false">
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="IEPS_Cotizado" HeaderText="IVA_Cotizado" Visible="false">
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Precio_U_Con_Imp_Cotizado" HeaderText="Precio_U_Con_Imp_Cotizado" Visible="false">
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            </asp:BoundField>

                            <asp:BoundField DataField="Nombre_Prod_Serv" HeaderText="Producto/Servicio" 
                            SortExpression="Nombre_Prod_Serv" Visible="True">
                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                            <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small"/>
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" 
                            SortExpression="Descripcion" Visible="True">
                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                            <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Cantidad" HeaderText="Ctd" 
                            SortExpression="Cantidad" Visible="True">
                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            <ItemStyle HorizontalAlign="Center" Width="10%" Font-Size="X-Small"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Total_Cotizado" HeaderText="Importe" 
                            SortExpression="Monto_Total" Visible="True">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Proveedor_ID" HeaderText="Proveedor_ID" Visible="false">
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Nombre_Proveedor" HeaderText="Nombre_Proveedor" Visible="false">
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Precio_U_Sin_Imp_Cotizado" HeaderText="Precio Unitario S/I" Visible="true">
                            <FooterStyle HorizontalAlign="Right" Width="10%"/>
                            <HeaderStyle HorizontalAlign="Right" Width="10%"/>
                            <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small"/>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Proveedor">
                            <ItemTemplate>
                                <asp:DropDownList ID="Cmb_Proveedor" runat="server" Width="99%">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ItemStyle Width="30%" />
                            </asp:TemplateField>
                            </Columns>
                            <SelectedRowStyle CssClass="GridSelected" />
                            <PagerStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            
                            <td colspan="2" align="right">
                                <asp:Button ID="Btn_Calcular_Precios_Cotizados" runat="server" 
                                    Text="Calcular Precios Cotizados" CssClass="button" Width="200px" 
                                    onclick="Btn_Calcular_Precios_Cotizados_Click"/>
                            </td>
                            <td align="right" colspan="2">
                                <table width="100%">
                                <tr>
                                    <td align="right" style="width:50%">
                                        Subtotal Cotizado
                                    </td>
                                
                                    <td align="right" style="width:50%">
                                        <asp:TextBox ID="Txt_SubTotal_Cotizado_Requisicion" runat="server" Style="text-align:right;" Enabled="false" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            
                            <td colspan="2">
                                
                                Comentarios:</td>
                            <td colspan="2">
                                <table width="100%">
                                <tr>
                                    <td align="right" style="width:50%">
                                        IVA Cotizado
                                    </td>
                                    <td align="right" style="width:50%">
                                        <asp:TextBox ID="Txt_IVA_Cotizado" runat="server" Enabled="false"  Style="text-align:right;" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            
                            <td rowspan="2" colspan="2">
                                
                                <asp:TextBox ID="Txt_Comentarios" runat="server" TextMode="MultiLine" Width="95%" Height="95%"></asp:TextBox>
                                
                            </td>
                            <td colspan="2">
                                <table width="100%">
                                <tr>
                                    <td align="right" style="width:50%">
                                        IEPS Cotizado
                                    </td>
                                    <td align="right" style="width:50%">
                                        <asp:TextBox ID="Txt_IEPS_Cotizado" runat="server" Enabled="false"  Style="text-align:right;" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            
                           
                            <td colspan="2">
                                <table width="100%">
                                <tr>
                                    <td align="right" style="width:50%">
                                        Total Cotizado
                                    </td>
                                    <td align="right" style="width:50%">
                                        <asp:TextBox ID="Txt_Total_Cotizado_Requisicion" runat="server"  Style="text-align:right;" Enabled="false" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                                </table>
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
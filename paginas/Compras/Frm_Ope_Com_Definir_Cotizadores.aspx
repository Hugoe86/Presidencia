<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Ope_Com_Definir_Cotizadores.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Definir_Cotizadores" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<script runat="server">

</script>

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
                <td colspan ="4" class="label_titulo">Distribuir Requisiciones a Cotizadores</td>
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
                    <td style="width:20%;">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" 
                            ToolTip="Agregar Cotizadores" CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/sias_new.png" 
                                onclick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                            onclick="Btn_Salir_Click"/>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                   <td align="right" colspan="3" style="width:99%;">
                       <asp:LinkButton ID="Btn_Reasignar" runat="server" 
                           onclick="Btn_Reasignar_Click" Text="Reasignar"></asp:LinkButton>
                    </td>
            </tr>
            <tr>
                <td colspan="4">
                    <div id="Div_Cotizadores" style="width:100%;height:100%;" runat="server">
                        <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                            <tr>
                                <td style="width:20%">
                                    Partida Especifica
                                </td>
                                <td style="width:70%" colspan="2">
                                    <asp:DropDownList ID="Cmb_Partidas" runat="server" Width="99%">
                                    </asp:DropDownList>
                                </td>
                                
                            </tr>
                            <tr>
                                <td>
                                    Cotizador
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="Cmb_Cotizador" runat="server" Width="99%" 
                                        onselectedindexchanged="Cmb_Cotizador_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                  </td>
                                 <%-- <td style="width:10%" align="right">
                                    <asp:ImageButton ID="Btn_Asignar_Cotizador" runat="server" Height="24px" 
                                    ImageUrl="~/paginas/imagenes/paginas/accept.png" AlternateText="Asignar Cotizador"
                                    ToolTip="Asignar Cotizador" Width="24px" 
                                        onclick="Btn_Asignar_Cotizador_Click"/>
                                </td>--%>
                            </tr>
                            <tr align="right" class="barra_delgada">
                                <td align="center" colspan="4">
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4">
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
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="true" SortExpression="Folio" ItemStyle-Wrap="true">
                                                <HeaderStyle HorizontalAlign="Left" Width="10%" Wrap="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="true"  Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Dependencia" HeaderText="U.Responsable" SortExpression="Dependencia" ItemStyle-Wrap="true"
                                                Visible="True">
                                                <HeaderStyle HorizontalAlign="Left" Width="15%" Wrap="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="true"  Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Partida_ID" HeaderText="Partida_ID" Visible="false">
                                                <FooterStyle HorizontalAlign="Right"/>
                                                <HeaderStyle HorizontalAlign="Right"/>
                                                <ItemStyle HorizontalAlign="Right"  Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Partida_Especifica" HeaderText="Partida Especifica" Visible="True" SortExpression="Partida_Especifica" 
                                            ItemStyle-Wrap="true">
                                                <FooterStyle HorizontalAlign="Left" Width="25%" Wrap="true"/>
                                                <HeaderStyle HorizontalAlign="Left" Width="25%" Wrap="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="25%" Wrap="true"  Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Producto" HeaderText="Producto" Visible="True" SortExpression="Producto" 
                                            ItemStyle-Wrap="true">
                                                <FooterStyle HorizontalAlign="Left" Width="25%" Wrap="true"/>
                                                <HeaderStyle HorizontalAlign="Left" Width="25%" Wrap="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="25%" Wrap="true"  Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:c}"
                                                Visible="True" SortExpression="Total" ItemStyle-Wrap="true">
                                                <FooterStyle HorizontalAlign="Center" Width="15%" Wrap="true"/>
                                                <HeaderStyle HorizontalAlign="Center" Width="15%" Wrap="true"/>
                                                <ItemStyle HorizontalAlign="Center" Width="15%" Wrap="true"  Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Cotizador_ID" HeaderText="Cotizador_ID" Visible="false">
                                                <FooterStyle HorizontalAlign="Right"/>
                                                <HeaderStyle HorizontalAlign="Right"/>
                                                <ItemStyle HorizontalAlign="Right"  Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Nombre_Cotizador" HeaderText="Cotizador" Visible="True" SortExpression="Nombre_Cotizador" 
                                            ItemStyle-Wrap="true">
                                                <FooterStyle HorizontalAlign="Left" Width="25%" Wrap="true"/>
                                                <HeaderStyle HorizontalAlign="Left" Width="25%" Wrap="true"/>
                                                <ItemStyle HorizontalAlign="Left" Width="25%"  Wrap="true"  Font-Size="X-Small"/>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="" >
                                                <ItemTemplate>
                                                <center>
                                                <asp:CheckBox ID="Chk_Requisicion" runat="server"/>
                                                </center>
                                                </ItemTemplate>
                                                <ControlStyle Width="5%" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                <div ID="Div_Detalle_Requisicion" runat="server" style="width:100%;font-size:9px;" 
                           visible="false">
                    <table width="99%">
                            <tr>
                                <td colspan="4" style="background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                                    <table width="99%">
                                        <tr>
                                            <td align="center">
                                                Datos Generales Requisicion 
                                            </td>
                                            <td align="right">
                                                <asp:ImageButton ID="ImageButton1" CausesValidation="false" runat="server" 
                                                ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClick="Btn_Cerrar_Click"/>  
                                            </td>
                                        </tr>
                                    </table>
                                </td>
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
                                 <td style="width:15%">
                                     Dependencia</td>
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
                                     <asp:TextBox ID="Txt_Estatus" runat="server" Enabled="false" 
                                         Width="99%"></asp:TextBox>
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
                                         TabIndex="10" TextMode="MultiLine" Width="99%"></asp:TextBox>
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
                                         TabIndex="10" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                     <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" 
                                         TargetControlID="Txt_Especificacion" WatermarkCssClass="watermarked" 
                                         WatermarkText="&lt;Especificaciones de los productos&gt;" />
                                 </td>
                             </tr>
                             <tr>
                                 <td align="center" colspan="4">
                                     Productos ó Servicios</td>
                             </tr>
                             <tr align="right" class="barra_delgada">
                                 <td align="center" colspan="4">
                                 </td>
                             </tr>
                             <tr> <%-- HeaderStyle-CssClass="tblHead" OnSorting="Grid_Productos_Sorting"--%>
                                 <td align="center" colspan="4">
                                     <asp:GridView ID="Grid_Productos" runat="server"
                                         AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                         Width="95%"
                                         AllowSorting="True" OnSorting="Grid_Productos_Sorting" 
                                         HeaderStyle-CssClass="tblHead" >
                                         <Columns>
                                             <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" 
                                                 Visible="True" >
                                                 <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                 <ItemStyle HorizontalAlign="Left" Width="10%"  Font-Size="X-Small"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="Nombre" HeaderText="Producto/Servicio" 
                                                 SortExpression="Nombre" Visible="True">
                                                 <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                 <ItemStyle HorizontalAlign="Left" Width="20%"  Font-Size="X-Small"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" 
                                                 SortExpression="Descripcion" Visible="True">
                                                 <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                 <ItemStyle HorizontalAlign="Left" Width="20%"  Font-Size="X-Small"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" 
                                                 SortExpression="Cantidad" Visible="True">
                                                 <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                 <ItemStyle HorizontalAlign="Left" Width="10%"  Font-Size="X-Small"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="Monto_Total" HeaderText="Precio Acumulado" 
                                                 SortExpression="Monto_Total" Visible="True">
                                                 <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                 <ItemStyle HorizontalAlign="Right" Width="20%" Font-Size="X-Small"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="Monto" HeaderText="Importe S/I" 
                                                 SortExpression="Monto" Visible="True">
                                                 <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                 <ItemStyle HorizontalAlign="Right" Width="20%" Font-Size="X-Small"/>
                                             </asp:BoundField>
                                         </Columns>
                                         <SelectedRowStyle CssClass="GridSelected" />
                                         <PagerStyle CssClass="GridHeader" />
                                         <AlternatingRowStyle CssClass="GridAltItem" />
                                     </asp:GridView>
                                 </td>
                             </tr>
                             <tr>
                                 <td align="right" colspan="4">
                                     Subtotal
                                     <asp:TextBox ID="Txt_Subtotal" runat="server" Enabled="false" 
                                         Style="text-align: right"></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td align="right" colspan="4">
                                     IEPS
                                     <asp:TextBox ID="Txt_IEPS" runat="server" Enabled="false" 
                                         Style="text-align: right"></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td align="right" colspan="4">
                                     IVA
                                     <asp:TextBox ID="Txt_IVA" runat="server" Enabled="false" 
                                         Style="text-align: right"></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td align="right" colspan="4">
                                     Total
                                     <asp:TextBox ID="Txt_Total" runat="server" Style="text-align: right" Enabled="false"></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td align="left" colspan="4">
                                     <hr class="linea" />
                                 </td>
                             </tr>
                         </caption>
                    </table>
                </div>
                </td>
            </tr>
        </table>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
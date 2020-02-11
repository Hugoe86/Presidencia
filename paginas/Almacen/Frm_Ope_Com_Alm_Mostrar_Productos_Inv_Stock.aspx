<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Com_Alm_Mostrar_Productos_Inv_Stock.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Alm_Mostrar_Productos_Inv_Stock" Title="Productos Inventario Stock"  ValidateRequest="false"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
          <div id="Div_General" style="background-color:#ffffff; width:98%; height:99%;"> <%--Div General--%>
            <table  border="0" cellspacing="0" class="estilo_fuente" frame="border" 
               width="100%"> <%--Tabla General--%>
                    <tr style="width:100%" align="center">
                        <td class="label_titulo">Productos del Inventario</td>
                    </tr>
                    <tr style="width:100%">
                        <td >
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
                    <tr style="width:100%">
                        <td style="width:70%" >&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda"  >
                            <td align="left" style="width:70%" >
                                 <asp:ImageButton ID="Btn_Imprimir" runat="server" 
                                 ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" CssClass="Img_Button" 
                                 AlternateText="Imprimir PDF" 
                                 ToolTip="Exportar PDF" 
                                 OnClick="Btn_Imprimir_Click" Visible="true"/> 
                                 <asp:ImageButton ID="Btn_Imprimir_Excel" runat="server" 
                                 ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" Width="24px" CssClass="Img_Button" 
                                 AlternateText="Exportar Excel" 
                                 ToolTip="Exportar Excel" 
                                 OnClick="Btn_Imprimir_Excel_Click" Visible="true"/> 
                                 <asp:ImageButton ID="Btn_Cancelar_Inventario" runat="server"
                                  ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" Width="24px" CssClass="Img_Button"
                                  AlternateText="Cancelar Inventario"
                                  ToolTip= "Cancelar Inventario"
                                  OnClientClick="return confirm('¿Está seguro de cancelar el inventario?');"
                                  onclick="Btn_Cancelar_Inventario_Click"/>
                                 <asp:ImageButton ID="Btn_Modificar_Captura" runat="server" 
                                 ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button" 
                                 AlternateText="Modificar Captura" 
                                 ToolTip="Modificar Captura"
                                 OnClientClick="return confirm('¿Está seguro de modificar la captura?');"
                                 OnClick="Btn_Modificar_Captura_Click" Visible="false"/> 
                                 <asp:ImageButton ID="Btn_Atras" runat="server"
                                  ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                  AlternateText="Atrás" 
                                  ToolTip="Atrás" 
                                  onclick="Btn_Atras_Click"/>
                            </td>
                        </tr>  
                    <tr>
                       <td>
                           <div id="Div_Generar_Inventario" visible="true" runat="server" style="overflow:auto;height:100%;width:99%;vertical-align:top;border-style:none;border-color:Silver;" >    
                                <table  border="0" cellspacing="0" class="estilo_fuente" frame="border"  width="100%">
                                    <tr>
                                        <td>
                                             <br />
                                        </td>
                                    </tr>
                                    <tr>
                                      <td >
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Fecha<br />
                                      </td>
                                      <td>
                                        <asp:TextBox ID="Txt_Fecha" runat="server" Width="400px" ReadOnly="True" 
                                              DataFormatString="{0:dd/MMM/yyyy}" Enabled="False"></asp:TextBox>
                                      </td>
                                    </tr>
                                    <tr>
                                         <td width="187px" > 
                                             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Estatus</td>
                                         <td>
                                                 <asp:TextBox ID="Txt_Estatus" runat="server" Width="400px" ReadOnly="True" 
                                                     Enabled="False"></asp:TextBox>
                                         </td>
                                    </tr>
                                    <tr>
                                         <td width="187px" > 
                                             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Usuario Creó</td>
                                         <td>
                                                 <asp:TextBox ID="Txt_Usuario_Creo" runat="server" Width="400px" ReadOnly="True" 
                                                     Enabled="False"></asp:TextBox>
                                         </td>
                                     </tr>
                                    <tr>
                                          <td width="187px">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Observaciones
                                            </td>
                                          <td >
                                                <asp:TextBox ID="Txt_Observaciones" runat="server" Height="48px" 
                                                    Width="400px" ReadOnly="True"  Enabled="False" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                     </tr>
                                    <tr>
                                           <td width="187px"  >
                                               &nbsp;&nbsp;&nbsp; 
                                            </td>
                                           <td>
                                                &nbsp;</td>
                                      </tr>
                                </table>
                            </div>  
                       </td>
                    </tr>
                    <tr>
                        <td>
                          <br />
                        </td>
                    </tr>
                    <tr align="right" class="barra_delgada" >
                         <td align="center"></td>
                    </tr>   
                    <tr>
                          <td>
                             <asp:Label ID="Lbl_No_Inventario" runat="server" CssClass="estilo_fuente" />
                             <br />
                          </td>
                    </tr>
                    <tr>
                         <td >
                             <table  border="0" cellspacing="0" class="estilo_fuente" frame="border" width="100%">
                                <tr>
                                        <td>
                                            <asp:GridView ID="Grid_Productos_Inventario" runat="server" 
                                    AutoGenerateColumns="False" CellPadding="1" ForeColor="#333333" 
                                    GridLines="None" AllowPaging="True" Width="98%" Height="98%" 
                                    onpageindexchanging="Grid_Productos_Inventario_PageIndexChanging" 
                                    CssClass="GridView_1" style="white-space:normal;" PageSize="20">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:BoundField DataField="PRODUCTO_ID" HeaderText="Producto_Id" 
                                            SortExpression="PRODUCTO_ID" Visible="False">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CLAVE" HeaderText="Clave" 
                                            SortExpression="CLAVE" >
                                            <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                            <ItemStyle Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PRODUCTO" 
                                            HeaderText="Producto">
                                            <HeaderStyle HorizontalAlign="Center" Width="150px" />
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CONTADOS_SISTEMA" HeaderText="C. Sistema" 
                                            SortExpression="CONTADOS_SISTEMA">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Width="50px" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CONTADOS_USUARIO" HeaderText="C. Usuario" 
                                            SortExpression="CONTADOS_USUARIO" Visible="False" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Width="50px" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DIFERENCIA" HeaderText="Diferencia" 
                                            SortExpression="DIFERENCIA" Visible="False">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Width="50px" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UNIDAD" HeaderText="Unidad" >
                                            <ItemStyle Width="20px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SUBFAMILIA" HeaderText="Partida Especifica" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Width="100px" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />                                
                                    <AlternatingRowStyle CssClass="GridAltItem" />                                
                                </asp:GridView>
                                        </td>
                                    </tr>
                             </table> <%--tabla--%> 
                          </td>
                    </tr>
                    <tr>
                       <td>
                       
                       <table  border="0" cellspacing="0" class="estilo_fuente" frame="border" width="100%">
                            <tr>
                                 <td>
                          <asp:Panel ID="Pnl_Modificar_Capturar" runat="server" 
                                    ScrollBars="Auto" Width="100%" Height="250px" Visible="False" 
                                    BorderStyle="None">
                                    <asp:GridView ID="Grid_Modificar_Captura" runat="server" 
                                        AutoGenerateColumns="False" CellPadding="1" ForeColor="#333333" 
                                        GridLines="None" Width="97%" PageSize="1" style="white-space:normal;">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:BoundField DataField="PRODUCTO_ID" HeaderText="Producto_Id" 
                                                SortExpression="PRODUCTO_ID" Visible="False">
                                                <ItemStyle Width="110px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CLAVE" HeaderText="Clave" 
                                                SortExpression="CLAVE" >
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PRODUCTO" 
                                                HeaderText="Producto">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="150px" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Productos Contados" 
                                                SortExpression="CONTADOS_USUARIO">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Txt_Cantidad"  runat="server" Width="74px"
                                                        CssClass="text_cantidades_grid" MaxLength="8" ValidationGroup="1234567890"  
                                                        ></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                                     TargetControlID="Txt_Cantidad" InvalidChars="<,>,&,',!,"
                                                                     FilterType="Numbers" 
                                                                     Enabled="True">   
                                                                </cc1:FilteredTextBoxExtender> 
                                                        <%--<cc1:MaskedEditExtender ID="MEE_Txt_Cantidad" runat="server" 
                                                    TargetControlID="Txt_Cantidad" Mask="999999999" MaskType="Number" 
                                                    InputDirection="RightToLeft" AcceptNegative="None" ErrorTooltipEnabled="True" 
                                                     CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                    CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" AcceptAMPM="True" 
                                                    ClearTextOnInvalid="True" UserDateFormat="None" /> --%> 
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="100px" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="UNIDAD" HeaderText="Unidad" >
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="20px" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SUBFAMILIA" HeaderText="Partida Especifica">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="100px" HorizontalAlign="Right" />
                                            </asp:BoundField>
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <HeaderStyle CssClass="GridHeader" />                                
                                        <AlternatingRowStyle CssClass="GridAltItem" />                                
                                    </asp:GridView>
                             </asp:Panel>
                                </td>
                             </tr>
                       </table>
                         
                          <div id="Div_Justificación" visible="false" runat="server" style="overflow:auto;height:100%;width:99%;vertical-align:top;border-style:none;border-color:Silver;" >    
                             <table width ="100%">
                                <tr>
                                     <td>
                                          <br />
                                    </td>
                                </tr>
                                <tr align="center" class="barra_delgada">
                                    <td  align="center" width="100%" height="16px"> 
                                        <asp:Label ID="lbl_Justificacion" runat="server"  Text="*Observaciones" />
                                    </td>
                               </tr>
                                <tr>
                                 <td align="left">
                                    <asp:TextBox ID="Txt_Justificacion" runat="server" Height="100px" 
                                        MaxLength="2000" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observaciones" runat="server" TargetControlID ="Txt_Justificacion" 
                                    WatermarkText="Límite de Caracteres 2000" WatermarkCssClass="watermarked" Enabled="True"/> 
                                 </td>
                               </tr>
                            </table>            
                         </div> <%--Fin del Div Justificación--%>
                       </td>
                    </tr>
               </table> <%--Fin de la tabla General --%>
           </div> <%--Fin del div General--%>
</ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>


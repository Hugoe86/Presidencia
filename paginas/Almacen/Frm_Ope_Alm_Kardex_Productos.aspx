<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" 
AutoEventWireup="true" CodeFile="Frm_Ope_Alm_Kardex_Productos.aspx.cs" 
Inherits="paginas_Almacen_Frm_Ope_Alm_Kardex_Productos" Title="Kardex Productos" %>
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
         <%-- 
           <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
               <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" />
                    </div>
                </ProgressTemplate>
               </asp:UpdateProgress>                       
               --%>
                  <div id="Div_Contenido" style="width: 95%; height: 100%;">  <!--Div General-->
                        <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                         <tr>
                           <td class="label_titulo">Kardex Productos Stock 
                           </td>
                         </tr>
                         <tr> <!--Bloque del mensaje de error-->
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
                         <tr class="barra_busqueda">
                                <td >
                                     <asp:ImageButton ID="Btn_Imprimir_PDF" runat="server" 
                                     ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" CssClass="Img_Button" 
                                     AlternateText="NUEVO" 
                                     ToolTip="Exportar PDF"  OnClick="Btn_Imprimir_PDF_Click" Visible="false"/>  
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
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<Clave>"
                                TargetControlID="Txt_Clave" />
                         
                       <div id="Div_Busqueda_Av" runat="server" style="width: 97%; height: 100%;"> 
                          <table width="98%">
                              <tr>
                                  <td align="left" style="width:10%;">
                                      <asp:CheckBox ID="Chk_Proveedor" runat="server" AutoPostBack="true"  Text="Almacén" Visible="false" />
                                      &nbsp;&nbsp;&nbsp;
                                  </td>
                                  <td align="left" colspan="2" style="width:60%;">
                                      <asp:DropDownList ID="Cmb_Almacen" runat="server" Enabled="False" Visible="false"  Width="92%">
                                      </asp:DropDownList>
                                  </td>
                              </tr>
                              <tr>
                                  <td align="left">
                                      <asp:CheckBox ID="Chk_Fecha_B" runat="server" AutoPostBack="true" oncheckedchanged="Chk_Fecha_B_CheckedChanged" Text="Fecha" />
                                      &nbsp;&nbsp;
                                  </td>
                                  <td align="left" style="width:30%;">
                                      &nbsp;<asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Enabled="False" Width="110px"></asp:TextBox>
                                      <asp:ImageButton ID="Img_Btn_Fecha_Inicio" runat="server" Enabled="False" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                                      &nbsp;&nbsp;&nbsp;&nbsp;
                                      <cc1:CalendarExtender ID="Img_Btn_Fecha_Inicio_CalendarExtender" runat="server" Format="dd/MMM/yyyy" OnClientShown="calendarShown" PopupButtonID="Img_Btn_Fecha_Inicio" 
                                          TargetControlID="Txt_Fecha_Inicio">
                                      </cc1:CalendarExtender>
                                      <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Enabled="False" Width="110px"></asp:TextBox>
                                      <asp:ImageButton ID="Img_Btn_Fecha_Fin" runat="server" Enabled="False" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Final" />
                                      <cc1:CalendarExtender ID="Img_Btn_Fecha_Fin_CalendarExtender" runat="server" Format="dd/MMM/yyyy" OnClientShown="calendarShown" PopupButtonID="Img_Btn_Fecha_Fin" 
                                          TargetControlID="Txt_Fecha_Fin">
                                      </cc1:CalendarExtender>
                                  </td>
                                  <td>
                                      <asp:TextBox ID="Txt_Clave" runat="server" MaxLength="10" Width="120px"></asp:TextBox>
                                      <asp:ImageButton ID="Btn_Buscar" runat="server" AlternateText="CONSULTAR" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Click" ToolTip="Buscar" />
                                      <asp:ImageButton ID="Btn_Limpiar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" onclick="Btn_Limpiar_Click" ToolTip="Limpiar" Width="20px" />
                                  </td>
                              </tr>
                              <tr>
                                  <td>
                                      &nbsp;</td>
                              </tr>
                              <tr align="right" class="barra_delgada">
                                  <td align="center" colspan="3">
                                  
                                  </td>
                              </tr>
                          </table>
                        </div >
                        
                    <div id="Div_Detalles" visible="false" runat="server" style="width: 97%; height: 100%;"> <!--DIV DETALLES-->  
                        <table width="96%">
                            <tr>
                                 <td style="width:15%">
                                     <asp:Label ID="Lbl_Producto" runat="server" Text="Producto"></asp:Label>
                                 </td>
                                  <td colspan="3">
                                      <asp:TextBox ID="Txt_Producto" Width="99%" runat="server" Enabled="False" ></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td style="width:15%">
                                     <asp:Label ID="Lbl_Descripcion" runat="server" Text="Descripción"></asp:Label>
                                 </td>
                                  <td colspan="3">
                                      <asp:TextBox ID="Txt_Descripcion" Width="99%" runat="server" Enabled="False" ></asp:TextBox>
                                 </td>
                             </tr>
                              <tr>
                                 <td style="width:15%">
                                     <asp:Label ID="Lbl_Clave_Producto" runat="server" Text="Clave"></asp:Label>
                                 </td>
                                  <td >
                                      <asp:TextBox ID="Txt_Clave_Producto" Width="99%" runat="server" Enabled="False" ></asp:TextBox>
                                 </td>
                                  <td style="width:15%">
                                     <asp:Label ID="Lbl_Unidad" runat="server" Text="Unidad"></asp:Label>
                                 </td>
                                  <td >
                                      <asp:TextBox ID="Txt_Unidad" Width="98%" runat="server" Enabled="False" ></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td style="width:15%">
                                     <asp:Label ID="Lbl_Marca" runat="server" Text="Marca"></asp:Label>
                                 </td>
                                  <td >
                                      <asp:TextBox ID="Txt_Marca" Width="99%" runat="server" Enabled="False" ></asp:TextBox>
                                 </td>
                                  <td style="width:15%">
                                     <asp:Label ID="Lbl_Modelo" runat="server" Text="Modelo"></asp:Label>
                                 </td>
                                  <td >
                                      <asp:TextBox ID="Txt_Modelo" Width="98%" runat="server" Enabled="False" ></asp:TextBox>
                                 </td>
                             </tr>
                               <tr>
                                 <td style="width:15%">
                                     <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus"></asp:Label>
                                 </td>
                                  <td >
                                      <asp:TextBox ID="Txt_Estatus" Width="99%" runat="server" Enabled="False" ></asp:TextBox>
                                 </td>
                                  <td style="width:15%">
                                     <asp:Label ID="Lbl_Comprometido" runat="server" Text="Comprometido"></asp:Label>
                                 </td>
                                  <td >
                                      <asp:TextBox ID="Txt_Comprometido" Width="98%" runat="server" Enabled="False" ></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td> &nbsp;</td>
                             </tr>
                          </table>
                          
                        <table width="96%">
                             <tr>
                                <td  style="width:390px;" align="center">
                                    <asp:Label ID="Lbl_Entradas" runat="server" Visible="false"  style="white-space:normal;" Text="Entradas"></asp:Label>
                                    <br />
                                    <asp:GridView ID="Grid_Entradas" runat="server"  Visible="False"   style="white-space:normal;"
                                    AutoGenerateColumns="False" CssClass="GridView_1" 
                                    GridLines="None" PageSize="1"  
                                    Width="93%" 
                                    Height="99%">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                    <asp:BoundField DataField="FECHA" DataFormatString="{0:dd/MMM/yyyy}" 
                                        HeaderStyle-HorizontalAlign="Left" HeaderText="Fecha" 
                                        ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ORDEN_COMPRA" HeaderText="O. Compra">
                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="REQUISICION" HeaderStyle-HorizontalAlign="Left" 
                                        HeaderText="Requisición" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" />
                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CANTIDAD" HeaderStyle-HorizontalAlign="Left" 
                                        HeaderText="Cantidad">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <EditRowStyle Font-Size="Smaller" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                                </td>
                                <td  style="width: 390px;" align="center">
                                    <asp:Label ID="Lbl_Salidas" runat="server" Visible="false" Text="Salidas"></asp:Label>
                                    <br />
                                    <asp:GridView ID="Grid_Salidas" runat="server" 
                                    Visible="False" 
                                    style="white-space:normal;"
                                    AutoGenerateColumns="False" 
                                    CssClass="GridView_1" 
                                    GridLines="None" PageSize="1"  
                                    Width="93%" Height="99%" >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                    <asp:BoundField DataField="FECHA" DataFormatString="{0:dd/MMM/yyyy}" 
                                        HeaderStyle-HorizontalAlign="Left" HeaderText="Fecha" 
                                        ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="REQUISICION" HeaderStyle-HorizontalAlign="Left" 
                                        HeaderText="Requisición" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CANTIDAD" HeaderStyle-HorizontalAlign="Left" 
                                        HeaderText="Cantidad">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_SALIDA" HeaderText="O. Salida">
                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <EditRowStyle Font-Size="Smaller" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                                 </td>
                            </tr>
                        </table>
                    </div >
                        </table> <!--Tabla General-->
                   </div>
            </ContentTemplate>
     </asp:UpdatePanel>
</asp:Content>


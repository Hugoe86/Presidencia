﻿ <%@ Page Language="C#" 
 MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" 
 AutoEventWireup="true" 
 CodeFile="Frm_Ope_Alm_Elaborar_Recibo_Transitorio.aspx.cs" 
 Inherits="paginas_Almacen_Frm_Ope_Alm_Elaborar_Recibo_Transitorio" 
 Title="Elaborar Recibo Transitorio" %>
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
       <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" />
                    </div>
                </ProgressTemplate>
                
           </asp:UpdateProgress>
             <div id="Div_Contenido" style="width: 97%; height: 100%;">
                <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
                 <tr>
                   <td colspan ="2" class="label_titulo">Recibo Transitorio
                   </td>
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
                        <asp:ImageButton ID="Btn_Generar_Recibo_T" runat="server" 
                         ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" 
                         Width="24px" CssClass="Img_Button" 
                         AlternateText="NUEVO" 
                         ToolTip="Generar Recibo Transitorio"
                         OnClick="Btn_Generar_Recibo_T_Click"
                         OnClientClick="return confirm('¿Desea Generar el Recibo Transitorio?');"
                          Visible="false"/>  
                        <asp:ImageButton ID="Btn_Salir" runat="server"  
                            CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                            onclick="Btn_Salir_Click"
                            ToolTip="Salir" 
                            AlternateText="Salir" />
                    </td>                    
                    <td align="right" style="width:90%;">
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
                    </td>                    
                </tr>
            <div id="Div_Busqueda_Av" runat="server" style="width: 98%; height: 100%;"> 
                <tr>
                     <td  colspan="2">             
                             <table border="0" cellspacing="0" class="estilo_fuente" frame="border" width="98%">
                                <tr>                         
                                    <td align="left">
                                        <asp:CheckBox ID="Chk_Fecha_B" runat="server" Text="Fecha" 
                                        oncheckedchanged="Chk_Fecha_B_CheckedChanged" AutoPostBack="true"/>
                                            &nbsp;&nbsp;   
                                     </td>
                                     <td align="left" style="width:40%;">
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
                     </td>
                </tr>
            </div >
                 <tr>
                    <td colspan="2">&nbsp;
                    </td>
                 </tr>
                 <div id="Div_Ordenes_Compra" runat="server" style="width: 97%;"> <!--Div Ordenes de Compra--> 
                    <tr>
                         <td colspan="2">
                           <asp:GridView ID="Grid_Ordenes_Compra" 
                                    runat="server" 
                                    style="white-space:normal;" AutoGenerateColumns="False" CellPadding="1" 
                                    CssClass="GridView_1"  GridLines="None" 
                               
                                    PageSize="1" 
                                    Width="99%">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="Btn_Seleccionar_Orden_Compra" runat="server" 
                                                CommandArgument='<%# Eval("NO_ORDEN_COMPRA") %>' 
                                                CommandName="Seleccionar_Orden_Compra" 
                                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png" 
                                                OnClick="Btn_Seleccionar_Orden_Compra_Click" ToolTip="Ver" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FOLIO" HeaderText="O. Compra">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_REQUISICION" 
                                        HeaderStyle-HorizontalAlign="Left" HeaderText="Requisición" 
                                        ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA" DataFormatString="{0:dd/MMM/yyyy}" HeaderText="Fecha">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PROVEEDOR" HeaderStyle-HorizontalAlign="Left" 
                                        HeaderText="Proveedor">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TOTAL" HeaderText="Total" DataFormatString="{0:c}">
                                        <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderStyle-HorizontalAlign="Left" 
                                        HeaderText="Estatus" ItemStyle-HorizontalAlign="Right">
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_ORDEN_COMPRA" HeaderText="No_Orden_Compra" 
                                        Visible="False" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PROVEEDOR_ID" HeaderText="Proveedor ID" 
                                        Visible="False" />
                                    <asp:BoundField DataField="NO_CONTRA_RECIBO" HeaderText="Contra Recibo" Visible="False" />
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <EditRowStyle Font-Size="Smaller" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                         </td>
                    </tr> 
                </div> <!-- Fin div Ordenes de Compra-->   
                
                  <div id="Div_Detalles_Orden_Compra" runat="server"  style="width: 97%; height: 100%;">
                    <tr>
                        <td colspan="2">
                          <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">  
                            <tr>
                              <td >
                                  <asp:Label ID="Lbl_Detalles" runat="server" Text="Detalles"></asp:Label>
                              </td>
                           </tr>
                            <tr align="right" class="barra_delgada">
                                <td align="center">
                                </td>
                            </tr>
                           <tr>
                                <td>
                                   <table style="width: 99%;" class="estilo_fuente">
                                        <tr>
                                            <td>
                                                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                                                    <tr>
                                                            <td style="width:20%;">
                                                               <asp:Label ID="Lbl_Unidad_Responsable" runat="server" Text="Unidad Responsable"></asp:Label>
                                                            
                                                            </td >
                                                            <td align="left" style="width:20%;" colspan="3">
                                                                <asp:TextBox ID="Txt_Unidad_Responsable" runat="server" Width="99%" Enabled="False"></asp:TextBox>
                                                            </td>
                                                      </tr>
                                                    <tr>
                                                        <td style="width:20%;">
                                                            <asp:Label ID="Lbl_Orden_Compra" runat="server" Width="80%" Text="Orden de Compra"></asp:Label> 
                                                        </td>
                                                        <td align="left" style="width:30%;">
                                                            <asp:TextBox ID="Txt_Orden_Compra" runat="server" 
                                                             Enabled="false" Width="98%"></asp:TextBox>
                                                        </td>
                                                          <td align="left" style="width:15%;">
                                                          Importe
                                                         </td>
                                                         <td align="left" style="width:20%;">
                                                             <asp:TextBox ID="Txt_Importe" runat="server" Enabled="false" 
                                                             Width="98%"></asp:TextBox>
                                                         </td>
                                                     </tr>
                                                         <tr>
                                                            <td style="width:20%;">
                                                                <asp:Label ID="Lbl_Requisición" runat="server" Width="80%" Text="Requisición"></asp:Label> 
                                                            </td>
                                                            <td align="left" style="width:20%;">
                                                                <asp:TextBox ID="Txt_Requisicion" runat="server" 
                                                                 Enabled="false" Width="98%"></asp:TextBox>
                                                            </td>
                                                            
                                                            <td align="left" style="width:15%;">
                                                              Fecha 
                                                             </td>
                                                             <td align="left" style="width:20%;">
                                                                 <asp:TextBox ID="Txt_Fecha_Surtido" runat="server" Enabled="false" 
                                                                 Width="98%"></asp:TextBox>
                                                             </td>

                                                         </tr>
                                                        <tr>
                                                            <td style="width:20%;">
                                                               <asp:Label ID="lbl_Proveedor" runat="server" Text="Proveedor"></asp:Label>
                                                            
                                                            </td >
                                                            <td align="left" style="width:20%;" colspan="3">
                                                                <asp:TextBox ID="Txt_Proveedor" runat="server" Width="99%" Enabled="False"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                                                                                <tr>
                                                           <td style="width:15%;">
                                                                <asp:Label ID="Lbl_Factura" runat="server" Text="Factura"></asp:Label>
                                                            </td>
                                                            <td >
                                                                <asp:TextBox ID="Txt_Factura" runat="server" Width="98%" Enabled="False"></asp:TextBox>
                                                            </td>                                                         
                                                         </tr>
                                                    <tr>
                                                        <td style="width: 20%;">
                                                            <asp:Label ID="Lbl_Responsable" runat="server" Text="Responsable"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="Cmb_Responsable" Width="98%" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" colspan="2">
                                                            <asp:TextBox ID="Txt_Numero_Empleado" runat="server" Width="150px" MaxLength="20"></asp:TextBox>
                                                            <asp:ImageButton ID="Img_Buscar_Empleado" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                                OnClick="Btn_Buscar_Empleado_Click" ToolTip="Buscar Empleado" />
                                                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkTxt_Numero_Empleado" runat="server"
                                                                WatermarkCssClass="watermarked" WatermarkText="<No. Empleado>" TargetControlID="Txt_Numero_Empleado" />
                                                            <cc1:FilteredTextBoxExtender ID="FTBE_Txt_Numero_Empleado" runat="server" FilterType="Custom"
                                                                TargetControlID="Txt_Numero_Empleado" ValidChars="0123456789">
                                                            </cc1:FilteredTextBoxExtender>
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
                                         <tr align="right" class="barra_delgada">
                                            <td align="center">
                                            </td>
                                         </tr> 
                                         <tr>
                                             <td>
                                                 <asp:Label ID="Lbl_Productos_Por_Unidad" runat="server" Text="Registro por Unidad"></asp:Label>
                                                <br />
                                                <asp:GridView ID="Grid_Productos_RT_Unidad" runat="server" 
                                                 style="white-space:normal;" AutoGenerateColumns="False" CellPadding="1" 
                                                    CssClass="GridView_1"  GridLines="None" 
                                                    Visible="False"
                                                    PageSize="1" 
                                                    Width="99%">
                                                    <RowStyle CssClass="GridItem" />
                                                    <Columns>                                                     
                                                        <asp:BoundField DataField="PRODUCTO_ID" HeaderText="Producto_ID" 
                                                            Visible="false">
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="NO_SERIE" HeaderText="No. Serie" 
                                                            Visible="true">
                                                            <ItemStyle Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="PRODUCTO" HeaderText="Producto" Visible="True">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" >
                                                            <ItemStyle Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="MATERIAL" 
                                                            HeaderText="Material">
                                                            <ItemStyle Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="COLOR" HeaderText="Color" >
                                                            <ItemStyle Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="UNIDAD" HeaderText="Unidad">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="MONTO" DataFormatString="{0:c}" 
                                                            HeaderText="Precio" Visible="True">
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="NO_INVENTARIO" HeaderText="No Inventario" >
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
                                            <td>
                                                 <asp:Label ID="Lbl_Productos_Por_Totalidad" runat="server" Text="Registro por Totalidad"></asp:Label>
                                            </td>
                                         </tr>
                                           <tr>
                                             <td>
                                                <asp:GridView ID="Grid_Productos_RT_Totalidad" runat="server" style="white-space:normal;" AutoGenerateColumns="False" CellPadding="1" 
                                                    CssClass="GridView_1"  GridLines="None" 
                                                    Visible="False"
                                                    PageSize="1" 
                                                    Width="99%">
                                                    <RowStyle CssClass="GridItem" />
                                                    <Columns>                                                     
                                                        <asp:BoundField DataField="PRODUCTO_ID" HeaderText="Producto_ID" 
                                                            Visible="false">
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="PRODUCTO" HeaderText="Producto" Visible="True">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" >
                                                            <ItemStyle Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="MODELO" 
                                                            HeaderText="Modelo" Visible="True">
                                                            <ItemStyle Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="UNIDAD" HeaderText="Unidad">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CANTIDAD" HeaderText="C. Solicitada" 
                                                            Visible="true">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="EXISTENCIA" HeaderText="C. Existente">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="MONTO_TOTAL" DataFormatString="{0:c}" 
                                                            HeaderText="Precio">
                                                            <FooterStyle HorizontalAlign="Right" />
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
                                            <td>
                                                &nbsp;</td>
                                          </tr>
                                </table>
                               </td>
                        </tr>
                   </table>
                        </td>
                   </tr>
                 </div> <!-- Fin del div Detalles Orden Compra-->            
               </table>
           </div>  <!--Div General-->
        </ContentTemplate>      
    </asp:UpdatePanel> 
</asp:Content>


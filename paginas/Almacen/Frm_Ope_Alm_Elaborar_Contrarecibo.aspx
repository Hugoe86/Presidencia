<%@ Page Language="C#" 
MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" 
AutoEventWireup="true" 
CodeFile="Frm_Ope_Alm_Elaborar_Contrarecibo.aspx.cs" 
Inherits="paginas_Almacen_Frm_Ope_Alm_Elaborar_Contrarecibo" 
Title="Elaborar Contra Recibo" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript" language="javascript">
        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
    </script> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <!--SCRIPT PARA LA VALIDACION QUE NO EXPERE LA SESSION-->  
   <script language="javascript" type="text/javascript">
    <!--
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_sesiones.ashx";

        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion() {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }
        //Temporizador para matener la sesión activa
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);
    //-->
   </script>
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
            <!--Div de Contenido -->
            <div id="Div_Contenido" style="width:98%; height: 100%;">
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                 <tr>
                   <td colspan ="2" class="label_titulo">Elaborar Contra Recibo
                   </td>
                 </tr>
                <tr> <!--Bloque del mensaje de error-->
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
                    <td >
                        <asp:ImageButton ID="Btn_Guardar_Contra_Recibo" runat="server" 
                         ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" Width="24px" CssClass="Img_Button" 
                         AlternateText="NUEVO" 
                         ToolTip="Guardar Contra Recibo"
                         OnClick="Btn_Guardar_Contra_Recibo_Click" 
                         Visible="false"
                         OnClientClick="return confirm('¿Desea Guardar el Contra Recibo?');"/>  
                        <asp:ImageButton ID="Btn_Salir" runat="server" 
                        CssClass="Img_Button" 
                        ToolTip="Salir"
                        AlternateText="Salir";
                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                        onclick="Btn_Salir_Click"/>
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
                                
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                             TargetControlID="Txt_Req_Buscar" InvalidChars="<,>,&,',!," 
                             FilterType="Numbers"
                             Enabled="True">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<No. Requisición>"
                                TargetControlID="Txt_Req_Buscar" />
                            
                        </div>
                    </td>                    
                </tr>
                <tr>
                    <td colspan="2">
                      <asp:HiddenField ID="Txt_Proveedor_ID" runat="server" />
                    </td>
                </tr>
                  
                  
                  
              <div id="Div_Busqueda_Av" runat="server" style="width: 97%; height: 100%;"> 
                <tr>
                   <%-- <td colspan="2">&nbsp;</td>--%>
                     <table width="100%">
                     <tr>
                            <td align="left" style="width:10%;">
                                <asp:CheckBox ID="Chk_Proveedor" runat="server" Text="Proveedor" 
                                oncheckedchanged="Chk_Proveedor_CheckedChanged" AutoPostBack="true"/>
                                &nbsp;&nbsp;&nbsp;
                           </td>
                            <td colspan="2" align="left" style="width:60%;">
                                <asp:DropDownList ID="Cmb_Proveedores" runat="server" 
                                    Width="92%" Enabled="False"></asp:DropDownList></td>
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
              
              <div id="Div_Ordenes_Compra" runat="server" style="overflow:auto;height:320px;width:98%;vertical-align:top;border-style:outset;border-color: Silver;" > 
                <tr>
                      <td align="left" colspan="2">
                           <%-- <asp:Panel ID="Pnl_Ordenes_Compra" runat="server" Width="100%" BorderColor="#3366FF" Height="99%">--%>
                                <asp:GridView ID="Grid_Ordenes_Compra" runat="server" style="white-space:normal;"
                                    AutoGenerateColumns="False" CssClass="GridView_1" 
                                    GridLines="None" PageSize="1"  Width="100%" >
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
                                        <asp:BoundField DataField="NO_REQUISICION" HeaderText="Requisición" >
                                            <ItemStyle Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA" DataFormatString="{0:dd/MMM/yyyy}" 
                                            HeaderStyle-HorizontalAlign="Left" HeaderText="Fecha" 
                                            ItemStyle-HorizontalAlign="Center">
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
                                            HeaderText="Estatus" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NO_ORDEN_COMPRA" HeaderText="No_Orden_Compra" 
                                            Visible="False" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PROVEEDOR_ID" HeaderText="Proveedor ID" 
                                        Visible="False" />
                                        <asp:BoundField DataField="LISTADO_ALMACEN" HeaderText="Listado Almacen" Visible="False" />
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <EditRowStyle Font-Size="Smaller" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            <%--</asp:Panel>--%>  
                      </td>
                </tr>   
                <tr>
                      <td colspan ="2">
                        <br />
                      </td>
                </tr>
               </div> <!-- Fin div Ordenes de Compra--> 
                
               <div id="Div_Detalles" runat="server"  style="width: 97%; height: 100%;" visible="false"> <!--Div Detalles--> 
                 <tr>
                      <td colspan ="2" align="center">
                          <asp:Label ID="Lbl_Detalles" runat="server" Text="Detalles"></asp:Label>
                      </td>
                </tr>
                <tr align="right" class="barra_delgada">
                    <td colspan="2" align="center">
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                                <tr>
                                    <td style="width:20%;">
                                        <asp:Label ID="Lbl_Orden_Compra" runat="server" Width="80%" Text="Orden de Compra"></asp:Label> 
                                    </td>
                                    <td align="left" style="width:20%;">
                                        <asp:TextBox ID="Txt_Orden_Compra" runat="server" 
                                         Enabled="false" Width="90%"></asp:TextBox>
                                    </td>
                                     <td align="left" style="width:20%;">
                                      Fecha elaboró orden de compra
                                     </td>
                                     <td align="left" style="width:20%;">
                                         <asp:TextBox ID="Txt_Fecha_Generacion" runat="server" Enabled="false" 
                                         Width="88%"></asp:TextBox>
                                     </td>
                                 </tr>
                                  <tr>
                                    <td style="width:20%;">
                                        <asp:Label ID="Lbl_Req" runat="server" Width="80%" Text="Requisición"></asp:Label> 
                                    </td>
                                    <td align="left" style="width:20%;" colspan ="3">
                                        <asp:TextBox ID="Txt_Requisicion" runat="server" 
                                         Enabled="false" Width="96%"></asp:TextBox>
                                    </td>
                                  </tr>
                                  <tr>
                                    <td style="width:20%;">
                                       <asp:Label ID="lbl_Tipo_Req" runat="server" Text="Tipo Requisición"></asp:Label>
                                    </td >
                                    <td align="left" style="width:20%;" colspan="3">
                                        <asp:TextBox ID="Txt_Tipo_Requisicion" runat="server" Width="96%" Enabled="False"></asp:TextBox>
                                    </td>
                                 </tr>
                                <tr>
                                    <td style="width:20%;">
                                       <asp:Label ID="lbl_Proveedor" runat="server" Text="Proveedor"></asp:Label>
                                    </td >
                                    <td align="left" style="width:20%;" colspan="3">
                                        <asp:TextBox ID="Txt_Proveedor" runat="server" Width="96%" Enabled="False"></asp:TextBox>
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
                <tr>
                      <td align="left" colspan="2">                         
                            <asp:GridView ID="Grid_Productos_Orden_Compra" runat="server" 
                                style="white-space:normal;"
                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                GridLines="None"   Width="100%" PageSize="5" >
                                <RowStyle CssClass="GridItem"  />
                                
                                <Columns>                                 
                                    <asp:BoundField DataField="PRODUCTO_ID" HeaderText="Producto ID" 
                                        Visible="False" />
                                    <asp:BoundField DataField="CLAVE" HeaderText="Clave" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE" HeaderStyle-HorizontalAlign="Left" 
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
                                    <asp:BoundField DataField="PRECIO_U" HeaderText="Precio U." DataFormatString="{0:c}">
                                        <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CANTIDAD" HeaderText="Ctd.">
                                        <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UNIDAD" HeaderText="Unidad" >
                                        <ItemStyle Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PRECIO_AC" HeaderText="Precio Ac." DataFormatString="{0:c}" >
                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Almacén">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="Cmb_Almacen" AutoPostBack ="true" runat="server" onselectedindexchanged="Cmb_Almacen_SelectedIndexChanged" Font-Size="X-Small">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="X-Small" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Registro">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="Cmb_RegistroX"  runat="server" Font-Size="X-Small">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="X-Small" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="R. T.">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Chk_R_Transitorio" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
                   <td colspan="2">
                      <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                       <tr>
                            <td>
                             </td>
                        </tr>
                            <tr>
                                 <td style="width:93%;" align="right"> Subtotal</td>
                                  <td  align="right">
                                          <asp:Label ID="Lbl_SubTotal" runat="server" 
                                          Text="0.00" ForeColor="Blue" BorderColor="Blue" 
                                          BorderWidth="2px" Width="90px">
                                          </asp:Label>
                                  </td>
                             </tr>
                            <tr>
                                <td align="right">IVA&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td align="right">
                                    <asp:Label ID="Lbl_IVA" runat="server" 
                                    Text="0.00" ForeColor="Blue" BorderColor="Blue" 
                                    BorderWidth="2px" Width="90px" Height="16px" ></asp:Label>
                                </td>
                              </tr>
                            <tr>
                                 <td align="right"> Total&nbsp;&nbsp;&nbsp;&nbsp;
                                 </td>
                                  <td align="right">
                                      <asp:Label ID="Lbl_Total" runat="server" 
                                      Text="0.00" ForeColor="Blue" BorderColor="Blue" 
                                      BorderWidth="2px" Width="90px" >
                                      </asp:Label>
                                   </td>
                            </tr>
                       </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table style="width: 100%;" class="estilo_fuente">
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                     <asp:Label ID="Lbl_Facturas_Proveedor" runat="server" Text="Facturas Proveedor"></asp:Label>
                                </td>
                            </tr>
                            <tr align="right" class="barra_delgada">
                                    <td align="center">
                                    </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                             <tr>
                                 <td>
                                       <table class="estilo_fuente" style="width: 97%;">
                                           <tr>
                                              <td align="left" style="width:15%;" colspan="4">
                                               <asp:Label ID="Lbl_No_Facturas" runat="server" Text="Facturas"></asp:Label>
                                                                                      
                                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                             
                                                  <asp:TextBox ID="Txt_No_Facturas" runat="server" Width="170px" 
                                                     MaxLength="2"></asp:TextBox>
                                                      &nbsp;
                                                      <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                                                       TargetControlID="Txt_No_Facturas" InvalidChars="<,>,&,',!," 
                                                       FilterType="Numbers" 
                                                       Enabled="True">   
                                                        </cc1:FilteredTextBoxExtender>
                                                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                                                       WatermarkCssClass="watermarked"
                                                       WatermarkText="<Facturas>"
                                                       TargetControlID="Txt_No_Facturas" />
                                                    <asp:ImageButton ID="Btn_Agregar_Facturas" runat="server" 
                                                     AlternateText="Agregar"
                                                     ToolTip="Agregar Facturas" 
                                                     ImageUrl="~/paginas/imagenes/gridview/add_grid.png" 
                                                     OnClick="Btn_Agregar_Facturas_Click" Enabled="true" />
                                              </td>
                                          </tr>
                                           
                                           <tr>
                                               <td colspan="3">
                                                   <asp:GridView ID="Grid_Facturas" runat="server" 
                                                       AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                                       PageSize="1" 
                                                       style="white-space:normal;"  Width="70%">
                                                       <RowStyle CssClass="GridItem" />
                                                       <Columns>
                                                           <asp:BoundField DataField="NO_REGISTRO" HeaderText="No Registro" 
                                                               Visible="False" >
                                                               <ItemStyle Width="10%" />
                                                           </asp:BoundField>
                                                           <asp:TemplateField HeaderText="No. Factura">
                                                               <ItemTemplate>
                                                                   <asp:TextBox ID="Txt_No_Factura" runat="server" Width="80%" MaxLength="49" ></asp:TextBox>

                                                                    <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                                                     runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                                     TargetControlID="Txt_No_Factura" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/ " InvalidChars="'">
                                                                     </cc1:FilteredTextBoxExtender>
                                                               </ItemTemplate>
                                                               <HeaderStyle HorizontalAlign="Center" />
                                                               <ItemStyle Width="30%" />
                                                           </asp:TemplateField>
                                                           <asp:TemplateField HeaderText="Importe">
                                                               <ItemTemplate>
                                                                   <asp:TextBox ID="Txt_Importe_Factura" runat="server"
                                                                    Width="80%" MaxLength="10" AutoPostBack="true" 
                                                                    ReadOnly="False" ontextchanged="Txt_Importe_Factura_TextChanged"
                                                                    CssClass="text_cantidades_grid" ></asp:TextBox>
                                                                   <cc1:FilteredTextBoxExtender ID="FTE_Txt_Letra_Inicial" runat="server" 
                                                                    Enabled="True" FilterType="Custom" InvalidChars="&lt;,&gt;,',!," 
                                                                    TargetControlID="Txt_Importe_Factura" 
                                                                    ValidChars="0123456789.">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                    
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                               </ItemTemplate>
                                                               <HeaderStyle HorizontalAlign="Center" />
                                                               <ItemStyle HorizontalAlign="Right" Width="30%"  />
                                                           </asp:TemplateField>
                                                           <asp:TemplateField HeaderText="Fecha Factura">
                                                               <ItemTemplate>
                                                                   <asp:TextBox ID="Txt_Fecha_Factura" runat="server" Width="60%" Enabled="False"></asp:TextBox>
                                                                       <asp:ImageButton ID="Btn_Fecha_Factura" runat="server" Enabled="true" 
                                                                       ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                                                       ToolTip="Seleccionar Fecha de Pago" />
                                                                       <cc1:CalendarExtender ID="CalendarExtender1" OnClientShown="calendarShown" runat="server" 
                                                                       TargetControlID="Txt_Fecha_Factura" 
                                                                       PopupButtonID="Btn_Fecha_Factura" Format="dd/MMM/yyyy">
                                                                       </cc1:CalendarExtender>
                                                               </ItemTemplate>
                                                               <HeaderStyle HorizontalAlign="Center" />
                                                               <ItemStyle Width="30%"  />
                                                           </asp:TemplateField>
                                                       </Columns>
                                                       <PagerStyle CssClass="GridHeader" />
                                                       <SelectedRowStyle CssClass="GridSelected" />
                                                       <HeaderStyle CssClass="GridHeader" />
                                                       <AlternatingRowStyle CssClass="GridAltItem" />
                                                   </asp:GridView>
                                                     
                                               </td>
                                              
                                               <td style= "width:20%;">

                                               </td>
                                           </tr>
                                           <tr>
                                                 <td colspan ="3">
                                                    <asp:Label ID="Lbl_Msg_Importe_Fact" runat="server" 
                                                      ForeColor="Blue" BorderColor="Blue" 
                                                      BorderWidth="2px" Width="340px" Visible="false">
                                                    </asp:Label> 
                                                 </td>
                                               </tr>
                                           <tr>
                                               <td colspan="3">
                                                 <br />
                                               </td>
                                           </tr>
                                  <tr>
                                    <td style="width:20%;" colspan="3">
                                        <asp:Label ID="Lbl_Fecha_Pago" runat="server" Text="Fecha de Pago"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="Txt_Fecha_Pago"  Width="20%" runat="server" AutoPostBack="true" 
                                        Enabled="false" ontextchanged="Txt_Fecha_Pago_TextChanged"></asp:TextBox>
                                        <asp:ImageButton ID="Btn_Fecha_Pago" runat="server"
                                        ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" Enabled="true"  
                                        ToolTip="Seleccionar Fecha de Pago" onclick="Btn_Fecha_Pago_Click" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" OnClientShown="calendarShown" runat="server" 
                                        TargetControlID="Txt_Fecha_Pago" PopupButtonID="Btn_Fecha_Pago" Format="dd/MMM/yyyy">
                                        </cc1:CalendarExtender>
                                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" 
                                        WatermarkCssClass="watermarked"
                                        WatermarkText="<Fecha de Pago>"
                                        TargetControlID="Txt_Fecha_Pago" />
                                    </td>
                                 </tr>
                                <tr>
                                  <td colspan ="2">
                                    <asp:Label ID="Lbl_Msg_Fecha_Pago" runat="server" 
                                      ForeColor="Blue" BorderColor="Blue" 
                                      BorderWidth="2px" Width="340px" Visible="false">
                                    </asp:Label> 
                                  </td>
                              </tr>
                                       </table>
                                 </td>
                            </tr>
                           </table>
                      </td>
                 </tr>
                 <tr>
                     <td >
                       
                     </td>
                 </tr>         
                <tr>
                    <td colspan ="2" align="center">
                         <asp:Label ID="Lbl_Documentos_Soporte" runat="server" Text="Documentos Soporte"></asp:Label>
                     </td>
                </tr>
                
                <tr align="right" class="barra_delgada">
                    <td colspan="2" align="center">
                    </td>
                </tr>
                <tr>
                    <td style="width:15%;">
                        <br />
                    </td>
                </tr> 
                <tr>
                      <td align="left" style="width:50%;" colspan ="3">
                        <asp:Label ID="Lbl_Doc_Soporte" runat="server" Text="Documentos"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="Cmb_Doc_Soporte" runat="server" AutoPostBack="true" 
                            Width="22%" onselectedindexchanged="Cmb_Doc_Soporte_SelectedIndexChanged">
                             </asp:DropDownList>
                             &nbsp;&nbsp;<asp:ImageButton ID="Btn_Agregar_Doc_Soporte" runat="server" 
                             AlternateText="Agregar"
                             ToolTip="Agregar Documento" 
                             ImageUrl="~/paginas/imagenes/gridview/add_grid.png" 
                             OnClick="Btn_Agregar_Doc_Soporte_Click" Enabled="False" />
                             &nbsp;<asp:ImageButton ID="Btn_Quitar_Doc_Soporte" runat="server" 
                             AlternateText="Quitar" 
                             ToolTip="Quitar Documento" 
                             ImageUrl="~/paginas/imagenes/gridview/minus_grid.png" 
                             OnClick="Btn_Quitar_Doc_Soporte_Click" Enabled="False" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                      </td>
                  </tr>                           
                <tr>
                    <td colspan="2" >
                       <asp:GridView ID="Grid_Doc_Soporte" runat="server" style="white-space:normal;"
                        AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" PageSize="1" 
                        Width="98%" onpageindexchanging="Grid_Doc_Soporte_PageIndexChanging">
                        <RowStyle CssClass="GridItem" />
                        <Columns>
                        <asp:TemplateField FooterText="Select">
                         <ItemTemplate>
                            <asp:ImageButton ID="Btn_Seleccionar_Documento" runat="server" 
                                CommandArgument='<%# Eval("DOCUMENTO_ID") %>' 
                                CommandName="Select" 
                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png" 
                                OnClick="Btn_Seleccionar_Documento_Click" ToolTip="Ver" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="DOCUMENTO_ID" HeaderText="Documento ID  " Visible="False">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NOMBRE" 
                            HeaderStyle-HorizontalAlign="Left" HeaderText="Nombre " 
                            ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DESCRIPCION" HeaderStyle-HorizontalAlign="Left" 
                        HeaderText="Descripción">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
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
                    <td colspan= "2">
                        <table style="width: 100%;" class="estilo_fuente">
                            <tr>
                                 <td style="width: 10%;">
                                    <br />  
                                 </td>
                            <tr>
                                <td align="center">
                                         <asp:Label ID="Lbl_Observaciones" runat="server" Text="Observaciones"></asp:Label>
                                 </td>
                            </tr>
                            <tr align="right" class="barra_delgada">
                                    <td align="center">
                                    </td>
                            </tr>
                            <tr >
                                <td align="left"  >
                                     <asp:TextBox ID="Txt_Observaciones" runat="server"
                                      Width="98%" Height="50px" TextMode="MultiLine" MaxLength="240"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                    runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    TargetControlID="Txt_Observaciones" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/ ">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                      <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observaciones" runat="server" TargetControlID ="Txt_Observaciones" 
                                      WatermarkText="Límite de Caracteres 250" WatermarkCssClass="watermarked" Enabled="True"/> 
                             </tr>
                            <tr>
                               <td>
                                    <br />
                               </td>
                             </tr>
                         </table>
                     </td>   
                </tr>  
               </div> <!-- Fin del div Detalles-->
                 </table> <!--Tabla General-->
           </div>  <!--Div General-->
         </ContentTemplate>
         
     </asp:UpdatePanel>    
</asp:Content>


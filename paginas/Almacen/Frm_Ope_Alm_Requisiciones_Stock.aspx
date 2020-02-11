<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Alm_Requisiciones_Stock.aspx.cs" Inherits="paginas_Almacen_Frm_Ope_Alm_Requisiciones_Stock" Title="Requisiciones Stock" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" language="javascript">
        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
   <!--SCRIPT PARA LA VALIDACION QUE NO EXPiRE LA SESSION-->  
   <script language="javascript" type="text/javascript">
    <!--
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_Session.ashx";
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
            <div id="Div_Contenido" style="width: 97%; height: 100%;">
                <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
                   <tr>
                      <td colspan ="2" class="label_titulo"><asp:Label ID="Lbl_Titulo"  Text="Requisiciones Stock"  runat="server"></asp:Label></td>
                   </tr>
                    <!--Bloque del mensaje de error-->
                    <tr>
                        <td colspan ="2">
                            <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                            <table style="width:99%;">
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
                            <asp:ImageButton ID="Btn_Orden_Salida" runat="server" ImageUrl="~/paginas/imagenes/gridview/grid_docto.png"
                            CssClass="Img_Button" ToolTip="Orden de Salida" 
                            onclick="Btn_Orden_Salida_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" AlternateText="Salir" ToolTip="Salir" 
                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Salir_Click" />
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
                         <td colspan="2">             
                                 <table border="0" cellspacing="0" class="estilo_fuente" frame="border" width="98%">
                                    <tr>                         
                                        <td align="left" >
                                            <asp:CheckBox ID="Chk_Fecha_B" runat="server" Text="Fecha" 
                                            oncheckedchanged="Chk_Fecha_B_CheckedChanged" AutoPostBack="true"/>
                                                &nbsp;&nbsp;   
                                         </td>
                                         <td align="left" >
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
                                             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
                                            &nbsp;</td>
                                    </tr>
                                 </table>
                         </td>
                    </tr>
                </div >
                   
                    <div id="Div_Requisiciones_Stock" runat="server" style="width: 97%; height: 100%;"> <!--Div_Requisiciones_Stock--> 
                    <tr>
                        <td>
                        <asp:HiddenField ID="Txt_Dependencia_ID" runat="server" />
                        <asp:HiddenField ID="Txt_Proyecto_Programa_ID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">   
                          <table width="100%">       
                            <tr>
                                <td>
                                <div style="overflow: auto; height: 380px; width: 99%; vertical-align: top; border-style: outset;
                                    border-color: Silver;">                                  
                                    <asp:GridView ID="Grid_Requisiciones_Stock" runat="server" 
                                        AutoGenerateColumns="False" CellPadding="1" CssClass="GridView_1" 
                                        ForeColor="#333333" GridLines="None" Height="98%" style="white-space:normal;" 
                                        Width="98%" PageSize="1">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:TemplateField>
                                             <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Seleccionar_Requisicion" runat="server" 
                                                    CommandArgument='<%# Eval("NO_REQUISICION") %>' 
                                                    CommandName="Seleccionar_Requisicion" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png" 
                                                    OnClick="Btn_Seleccionar_Requisicion_Click" ToolTip="Ver" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:TemplateField>
                                            <asp:BoundField DataField="NO_REQUISICION" HeaderText=" Requisición" Visible="False" >
                                                <FooterStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="110px" Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FOLIO" 
                                                HeaderText="Requisición">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="110px" Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UNIDAD_RESPONSABLE" 
                                                HeaderText="Unidad Responsable" >
                                                <FooterStyle HorizontalAlign="Left" />
                                                <ItemStyle Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FECHA_AUTORIZACION" HeaderText="Fecha Autorización" 
                                                DataFormatString="{0:dd/MMM/yyyy}" >
                                                <FooterStyle HorizontalAlign="Left" />
                                                <ItemStyle Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TOTAL" HeaderText="Total" >
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" >
                                                <FooterStyle HorizontalAlign="Left" />
                                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" />
                                            </asp:BoundField>
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                   </div>
                                </td>
                            </tr>
                           </table>
                        </td>
                    </tr>
                    </div> <!--Fin Div_Requisiciones_Stock--> 
                   <div id="Div_Detalles_Requisicion" runat="server" style="width: 97%; height: 100%;" visible="false"> <!--Div_Detalles_Requisicion--> 
                    <tr>
                        <td colspan= "2">
                            <table style="width: 99%;" class="estilo_fuente">
                                <tr>
                                    <td style="width:20%;">
                                       Folio
                                    </td>
                                    <td align="left" style="width:30%;">
                                        <asp:TextBox ID="Txt_Folio" runat="server" Enabled="false" Width="89%"></asp:TextBox>
                                    </td>
                                    <td style="width:20%;">
                                        Fecha Autorización
                                    </td>
                                    <td align="left" style="width:30%;">
                                        <asp:TextBox ID="Txt_Fecha_Autorizacion" runat="server" DataFormatString="{0:dd/MMM/yyyy}" Enabled="false" Width="89%"></asp:TextBox>
                                    </td>
                                  </tr>
                                  <tr>
                                    <td >
                                      Unidad Responsable
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:TextBox ID="Txt_Unidad_Responsable" runat="server" Width="96%" Enabled="False" 
                                        MaxLength="20"></asp:TextBox>
                                    </td>
                                  </tr>
                                  <tr>   
                                         <td >
                                                Programa
                                        </td>
                                        <td align="left" colspan="3"  >
                                            <asp:TextBox ID="Txt_Programa" runat="server" Enabled="false" 
                                             Width="96%"></asp:TextBox>
                                        </td>
                                  </tr>
                                  <tr>
                                        <td >          
                                            F. Financiamiento
                                        </td>
                                        <td align="left" colspan="3" >
                                            <asp:TextBox ID="Txt_Financiamiento" runat="server" Enabled="false" Width="96%"></asp:TextBox>
                                        </td>
                                  </tr>
                                
                                <tr >
                                    <td align="left" > Justificación
                                    </td>
                                    <td align="left" colspan="3" >
                                         <asp:TextBox ID="Txt_Justificacion" runat="server"
                                          Width="96%" Height="50px" TextMode="MultiLine" MaxLength="249" 
                                          ReadOnly="True" Enabled="False"></asp:TextBox>
                                    </td>
                                 </tr>
                                <tr>
                                   <td colspan="4">
                                        <br />
                                   </td>
                                 </tr>
                                 
                                 <tr>
                                   <td >
                                       <asp:Label ID="Lbl_Empleado" runat="server" Text="Recibe Requisición"></asp:Label> 
                                   </td>
                                   <td >
                                          <asp:DropDownList ID="Cmb_Empleados_UR" runat="server" Width="400px">
                                          </asp:DropDownList>
                                        <br />
                                   </td>
                                   <td align="left" colspan="2">
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="Txt_Numero_Empleado" runat="server"  Width="150px" MaxLength="20"></asp:TextBox>
                                    
                                     <asp:ImageButton ID="Img_Buscar_Empleado" runat="server" 
                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                            onclick="Btn_Buscar_Empleado_Click" ToolTip="Buscar Empleado" />
                                </td>
                                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                        WatermarkCssClass="watermarked"
                                        WatermarkText="<No. Empleado>"
                                        TargetControlID="Txt_Numero_Empleado" />
                                        
                                        <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                         TargetControlID="Txt_Numero_Empleado" InvalidChars="<,>,&,',!," 
                                         FilterType="Custom"
                                         Enabled="True">
                                        </cc1:FilteredTextBoxExtender>--%>
                                 </tr>
                                <tr align="right" class="barra_delgada">
                                    <td colspan="4" align="center">
                                    </td>
                                </tr> 
                                <tr>
                                   <td colspan="3">
                                        Productos de la Requisición<br />
                                   </td>
                                 
                                </tr> 
                                <tr>
                                    <td colspan="4">
                                      <%-- <asp:Panel ID="Pnl_Detalles_Requisicion" runat="server" ScrollBars="Vertical" style="white-space:normal;" Width="99%" BorderColor="#3366FF" Height="300px">--%>
                                        <asp:GridView ID="Grid_Productos_Requisicion" runat="server"  
                                            style="white-space:normal;"                                        
                                            AutoGenerateColumns="False" CellPadding="1" CssClass="GridView_1" 
                                            GridLines="None" Width="100%" >
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:BoundField DataField="PRODUCTO_ID" HeaderText="Producto ID" 
                                                    Visible="False">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CLAVE" HeaderText="Clave">
                                                    <ItemStyle Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PRODUCTO" HeaderText="Producto" 
                                                    Visible="true">
                                                    <ItemStyle Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" >
                                                    <ItemStyle Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CANTIDAD_SOLICITADA" HeaderText="Ctd. Solicitada" 
                                                    Visible="True">
                                                    <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CANTIDAD_ENTREGADA" HeaderText="Ctd. Entregada">
                                                    <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Ctd.  A Entregar">
                                                    <ItemTemplate>
                                                            <asp:TextBox ID="Txt_Cantidad_A_Entregar"  runat="server" Width="74px"
                                                                CssClass="text_cantidades_grid" MaxLength="10" 
                                                                ValidationGroup="1234567890"  
                                                                ></asp:TextBox> 
                                                               <%-- <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                                     TargetControlID="Txt_Cantidad_Entregada" InvalidChars="<,>,&,',!,"
                                                                     FilterType="Numbers" 
                                                                     Enabled="True">   
                                                                </cc1:FilteredTextBoxExtender>--%> 
                                                            <%--<cc1:MaskedEditExtender ID="MEE_Txt_Cantidad" runat="server" 
                                                            TargetControlID="Txt_Cantidad_Entregada" Mask="99999" MaskType="Number" 
                                                            InputDirection="RightToLeft" AcceptNegative="None" ErrorTooltipEnabled="True" 
                                                             CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                            CultureThousandsPlaceholder="" CultureTimePlaceholder="" 
                                                            Enabled="True" AcceptAMPM="True" ClearTextOnInvalid="True" UserDateFormat="None" />--%>
                                                        </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="UNIDAD" HeaderText="Unidad" >
                                                    <ItemStyle Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PRECIO" HeaderText="Precio">
                                                    <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SUBTOTAL" HeaderText="SubTotal" Visible="False" />
                                                <asp:BoundField DataField="MONTO_IVA" HeaderText="IVA" Visible="False">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SUBTOTAL" HeaderText="Total" >
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PORCENTAJE_IVA" HeaderText="Porcentaje IVA" 
                                                    Visible="False" />
                                                <asp:BoundField DataField="PARTIDA_ID" HeaderText="Partida ID" 
                                                    Visible="False" />
                                            </Columns>
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                      <%-- </asp:Panel>--%>
                                     </td>
                                 </tr>
                                 <tr>
                                   <td colspan="4">
                                          <table>
                                              <tr>
                                                    <td>
                                                    </td>
                                              </tr>
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
                       </td>
                
                    </tr>
                   </div> <!--Fin Div_Detalles_Requisicion--> 
                   <tr>
                       <td colspan="2">
                       </td>
                   </tr>
               </table> <!--Fin Tabla Generar -->
             </div> <!-- Fin Div de Contenido -->
         </ContentTemplate>
        <%--<Triggers>
            <asp:AsyncPostBackTrigger ControlID="Btn_Aceptar_Busqueda" EventName="Click" />
        </Triggers>--%>
    </asp:UpdatePanel>
    
  <%--<!--Modal para la Busqueda Avanzada-->
            <asp:UpdatePanel ID="Udp_Modal_Popup" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <cc1:ModalPopupExtender ID="Modal_Busqueda" runat="server" BackgroundCssClass="popUpStyle" 
                    PopupControlID="Pnl_Busqueda" TargetControlID="Btn_Comodin_Open" 
                    CancelControlID="Btn_Cancelar_Panel" DropShadow="True" DynamicServicePath="" Enabled="True" />  
                   <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Close" runat="server" Text="" />
                   <asp:Button  Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Open" runat="server" Text="" />  
                </ContentTemplate>          
            </asp:UpdatePanel>  --%>
    
    <!--Panel para realizar la Busqueda Avanzada-->
   <%-- <asp:Panel ID="Pnl_Busqueda" runat="server" Width="490px" Height="100%" style="border-style:outset;border-color:Silver;background-repeat:repeat-y;background-color:White;color:White; display:none;">
        <center>
            <asp:UpdatePanel ID="pnlPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table width="100%" class="estilo_fuente">
                        <tr>
                            <td colspan="4">
                                <asp:Image ID="Img_Error_Busqueda" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px" />
                                <asp:Label ID="Lbl_Error_Busqueda" runat="server" ForeColor="Red" Width="100%" />
                            </td>                        
                        </tr>
                        <tr>
                            <td colspan="4" class="barra_busqueda" align="center">B&uacute;squeda Avanzada</td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:CheckBox ID="Chk_Fecha" runat="server" AutoPostBack="true" 
                                    Text="Rango Fechas" oncheckedchanged="Chk_Fecha_CheckedChanged" />
                            </td>
                            <td align="left">
                                <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="130px" Enabled="False"></asp:TextBox>
                                <asp:ImageButton ID="Img_Btn_Fecha_Inicial" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                    ToolTip="Seleccione la Fecha Inicial" />
                                <cc1:CalendarExtender ID="Img_Btn_Fecha_Inicial_CalendarExtender" runat="server" OnClientShown="calendarShown"
                                    TargetControlID="Txt_Fecha_Inicial" Format ="dd/MMM/yyyy" PopupButtonID="Img_Btn_Fecha_Inicial">
                                </cc1:CalendarExtender>
                            </td>
                            <td align="left"></td>
                            <td align="left">
                                <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="130px" Enabled="False"></asp:TextBox>
                                <asp:ImageButton ID="Img_Btn_Fecha_Final" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                    ToolTip="Seleccione la Fecha Final" />
                                <cc1:CalendarExtender ID="Img_Btn_Fecha_Final_CalendarExtender" runat="server" OnClientShown="calendarShown"
                                    TargetControlID="Txt_Fecha_Final" Format ="dd/MMM/yyyy" PopupButtonID="Img_Btn_Fecha_Final">
                                </cc1:CalendarExtender>
                            </td>                        
                        </tr>
                        
                        <tr>
                            <td align="left">
                                <asp:CheckBox ID="Chk_Dependencia" runat="server" Text="U. Responsable" 
                                    AutoPostBack="true" oncheckedchanged="Chk_Dependencia_CheckedChanged" />
                            </td>
                            <td align="left" colspan="3">
                                <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="355px"  Enabled="False"></asp:DropDownList>
                            </td>                        
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <table>
                                    <tr>
                                        <td  align="center">
                                            <asp:Button ID="Btn_Aceptar_Busqueda" runat="server" Text="Aceptar" Width="100px" CssClass="button" OnClick="Btn_Aceptar_Busqueda_Click" ToolTip="Buscar"/>       
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;       
                                            <asp:Button ID="Btn_Cancelar_Busqueda" runat="server" CssClass="button" onclick="Btn_Cancelar_Busqueda_Click" Text="Cancelar" Width="100px" ToolTip="Cancelar" />
                                         </td>  
                                     </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Btn_Busqueda_Avanzada" EventName="Click" />                    
                </Triggers>
            </asp:UpdatePanel>
        </center>
    </asp:Panel>--%>
    
</asp:Content>


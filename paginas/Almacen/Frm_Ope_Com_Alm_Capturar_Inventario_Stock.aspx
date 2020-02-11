<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Com_Alm_Capturar_Inventario_Stock.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Alm_Capturar_Inventario_Stock" Title="Capturar Inventario Stock" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1"%>
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
     <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization ="true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
  <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
        <ProgressTemplate>
            <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
            <div  class="processMessage" id="div_progress">
                <img alt="" src="../Imagenes/paginas/Updating.gif" />
            </div>
        </ProgressTemplate> 
       </asp:UpdateProgress>
    <div id="Div_Inventarios_Pendientes" style="background-color:#ffffff; width:98%; height:100%;"> <%-- Div Geenral--%>
                <table  border="0" cellspacing="0" class="estilo_fuente" frame="border"  
                    width="100%"> <%-- Tabla Capturar inventario--%>
                    <tr align="center">
                        <td colspan ="2" class="label_titulo">Capturar Inventario</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td style="font-size:9px;width:70%;text-align:left;" valign="top">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error"/>
                                </td>            
                              </tr>
                              <tr>
                                 <td style="font-size:9px;width:70%;text-align:left;" valign="top">             
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error"/>
                                </td>          
                                <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                                </td>
                              </tr>          
                            </table>                   
                          </div>                          
                        </td>
                    </tr> 
                    <tr>
                        <td>&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" >
                           <td style="width:20%;">
                                 <asp:ImageButton ID="Btn_Guardar" runat="server" 
                                 ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" Width="24px" CssClass="Img_Button"
                                 AlternateText="NUEVO" ToolTip="Guardar Inventario Capturado"
                                 OnClientClick="return confirm('¿Está seguro de guardar el inventario capturado?');"                                
                                 OnClick="Btn_Guardar_Click" Visible="false"/>
                                <asp:ImageButton ID="Btn_Salir" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" ToolTip="Salir" onclick="Btn_Salir_Click"/>
                        </td>
                           <td align="right" style="width:80%;">
                             <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                              ToolTip="AVANZADA" onclick="Btn_Busqueda_Avanzada_Click">B&uacute;squeda</asp:LinkButton>
                              &nbsp;&nbsp;
                              <asp:TextBox ID="Txt_Busqueda" runat="server"  MaxLength="10" Width="150px"></asp:TextBox>
                              <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                         TargetControlID="Txt_Busqueda" InvalidChars="<,>,&,',!," 
                                         FilterType="Numbers" 
                                        Enabled="True">   
                               </cc1:FilteredTextBoxExtender>
                               <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese el No. Inventario>"
                                TargetControlID="Txt_Busqueda" />
                               <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                onclick="Btn_Buscar_Click" ToolTip="Buscar" />
                           </td>
                    </tr>
                    <tr>
                      <td colspan="2">
                          <div id="Div_Inventario_Pendientes" visible="true" runat="server" style="overflow:auto;height:100%;width:99%;vertical-align:top;border-style:none;border-color:Silver;">    <%-- Div inventarios pendientes--%>
                            <table width="99%">
                                 <tr>
                                     <td>
                                         &nbsp;
                                     </td>
                                 </tr>
                                  <tr>
                                    <td>
                                     &nbsp;<asp:Label ID="Lbl_Inventarios" runat="server" CssClass="estilo_fuente" 
                                     Visible="False">Inventarios</asp:Label>
                                     &nbsp;
                                     <asp:GridView ID="Grid_Inventario_Pendientes" runat="server"  style="white-space:normal;"
                                     AllowPaging="True" AutoGenerateColumns="False" CellPadding="1" 
                                     CssClass="GridView_1" ForeColor="#333333" GridLines="None" Height="99%" 
                                     onpageindexchanging="Grid_Inventario_Pendientes_PageIndexChanging" 
                                     OnSelectedIndexChanged="Grid_Inventario_Pendientes_SelectedIndexChanged" 
                                     Width="98%">
                                     <RowStyle CssClass="GridItem" />
                                     <Columns>
                                         <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                             ImageUrl="~/paginas/imagenes/gridview/blue_button.png" Text="Ver" />
                                         <asp:BoundField DataField="NO_INVENTARIO" HeaderText="No. Inventario" 
                                             SortExpression="NO_INVENTARIO">
                                             <ItemStyle Width="110px" />
                                         </asp:BoundField>
                                         <asp:BoundField DataField="FECHA" DataFormatString="{0:dd/MMM/yyyy}" 
                                             HeaderText="Fecha" SortExpression="FECHA">
                                             <ItemStyle Width="110px" />
                                         </asp:BoundField>
                                         <asp:BoundField DataField="USUARIO_CREO" HeaderText="Usuario Creó" 
                                             SortExpression="USUARIO_CREO" />
                                         <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" 
                                             SortExpression="ESTATUS" />
                                         <asp:BoundField DataField="OBSERVACIONES" HeaderText="Observaciones" 
                                             SortExpression="OBSERVACIONES" Visible="False" />
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
                                 &nbsp;
                                 </td>
                                  </tr>
                                    <tr>
                                    <td>
                                 &nbsp;</td>
                                 </tr>
                     </table>
                          </div> <%-- Fin div inventarios pendientes--%>
                       </td>
                     </tr>
                    <div id="Div_Capturar_Inventario" runat="server" visible="false">  <%-- Fin de la Tabla que contiene el DataGrid--%>
                      <tr>
                         <td colspan="2">
                             <table  border="0" width="99%" cellspacing="0" class="estilo_fuente"> <%-- Tabla Capturar Inventario--%>
                                     <tr>
                                        <td>
                                         &nbsp;
                                        </td>
                                    </tr>
                                     <tr>
                                        <td  style="width:191px">
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Fecha
                                        </td>
                                        <td >
                                            <asp:TextBox ID="Txt_Fecha_Creo" runat="server" Width="400px" ReadOnly="True" 
                                                Enabled="False"></asp:TextBox>
                                        </td>
                                     </tr>
                                     <tr>
                                        <td style="width:191px" >
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Usuario Creó
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtUsuario_Creo" runat="server" Width="400px" ReadOnly="True" 
                                                Enabled="False"></asp:TextBox>
                                        </td>
                                     </tr>
                                     <tr>
                                        <td style="width:191px" >
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            Estatus&nbsp;
                                        </td>
                                        <td >
                                            <asp:TextBox ID="Txt_Estatus" runat="server" Width="400px" ReadOnly="True" 
                                                Enabled="False"></asp:TextBox>
                                        </td>
                                     </tr>
                                     <tr>
                                        <td style="width:191px" >
                                       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                       Observaciones
                                        </td>
                                         <td>
                                                <asp:TextBox ID="Txt_Observaciones" runat="server" Width="400px" Rows="2" 
                                                TextMode="MultiLine" ReadOnly="True" Height="48px" Enabled="False"></asp:TextBox>
                                         </td>
                                    </tr>
                                        <table width="100%"> <%--Tabla Productos--%>
                                            <tr>
                                                <td>
                                                &nbsp
                                                </td>
                                            </tr>
                                            <tr align="right" class="barra_delgada">
                                                <td align="center"></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                &nbsp
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Lbl_No_Inventario" runat="server" CssClass="estilo_fuente" 
                                                        Visible="False" />
                                                    <br />
                                                </td>
                                        </tr>
                                        
                                        <tr>
                                            <td>
                                                  <asp:Panel ID="Pnl_Productos_Inventario" runat="server" ScrollBars="Vertical" style="white-space:normal;"
                                                      Width="99%" BorderColor="#3366FF" Height="300px">
                                                    <asp:GridView ID="Grid_Capturar_Inventario" runat="server" 
                                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                                    GridLines="None" Width="96%" 
                                                        PageSize="1" >
                                                    <RowStyle CssClass="GridItem" />
                                                    <Columns>
                                                    <asp:BoundField DataField="PRODUCTO_ID" HeaderText="Producto_Id" 
                                                        SortExpression="PRODUCTO_ID" Visible="False">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="110px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CLAVE" HeaderText="Clave" 
                                                        SortExpression="CLAVE" >
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:BoundField>
                                                    <asp:BoundField DataField="PRODUCTO" 
                                                        HeaderText="Producto">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Width="200px" />
                                                    </asp:BoundField>
                                                        <asp:BoundField DataField="EXISTENCIA" HeaderText="Existencia">
                                                            <ItemStyle HorizontalAlign="Right" Width="50px" />
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
                                                            ClearTextOnInvalid="True" UserDateFormat="None" />--%>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="100px" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="UNIDAD" HeaderText="Unidad" >
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20px" />
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
                                    </table> <%--Fin de la tabla productos--%>
                                   </table> <%-- Fin del tabla--%>
                          </td>
                       </tr>
                    </div> <%--Fin Div Capturar inventario--%>
                </table>    <%--Fin de la tabla General--%>
    </div> <%-- Fin del Div General--%>
</ContentTemplate>
      <Triggers>
         <asp:AsyncPostBackTrigger  ControlID="Btn_Aceptar_Busqueda" EventName="Click"/>
      </Triggers> 
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="Udp_Modal_Popup" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <cc1:ModalPopupExtender ID="Modal_Busqueda" runat="server" BackgroundCssClass="popUpStyle" 
                    PopupControlID="Pnl_Busqueda" TargetControlID="Btn_Comodin_Open" 
                    CancelControlID="Btn_Cancelar_Busqueda" DropShadow="True" DynamicServicePath="" Enabled="True" />  
                   <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Close" runat="server" Text="" />
                   <asp:Button  Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Open" runat="server" Text="" />  
                </ContentTemplate>          
     </asp:UpdatePanel>  
     
                  <%-- Panel del ModalPopUp--%>
    <asp:Panel ID="Pnl_Busqueda" runat="server" Width="500px" Height="200px"
            style="border-style:outset;border-color:Silver;background-repeat:repeat-y;background-color:White;color:White; display:none; ">
           <center>
           <asp:UpdatePanel ID="pnlPanel" runat="server" UpdateMode="Conditional">
           <ContentTemplate>
              <table width="95%" class="estilo_fuente">
                  <tr>
                     <td colspan="4">
                            <asp:ImageButton ID="Img_Error_Busqueda" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                            Width="24px" Height="24px"/>
                            <asp:Label ID="Lbl_Error_Busqueda" runat="server" ForeColor="Red" Width="100%" />
                      </td>
                  </tr>
                  <tr align="center" >
                      <td colspan="4" class="barra_busqueda" align="center" width="100%"> Búsqueda Avanzada</td>
                  </tr>
                  <tr>
                      <td colspan="4"></td>
                  </tr>
                  <tr>
                        <td align="left">
                            <asp:CheckBox ID="Chk_Fecha_Abanzada" runat="server" Text="Fecha" 
                            oncheckedchanged="Chk_Fecha_B_CheckedChanged" AutoPostBack="true"/>
                            &nbsp;&nbsp; &nbsp;Del</td>
                        <td align="left">
                            <asp:TextBox ID="Txt_Fecha_Inicial_B" runat="server" Width="150px" 
                            Enabled="False"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Calendar_Fecha_Inicial" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                ToolTip="Seleccione la Fecha Inicial" />
                            <cc1:CalendarExtender ID="CalendarExtender1" OnClientShown="calendarShown" runat="server" TargetControlID="Txt_Fecha_Inicial_B" PopupButtonID="Btn_Calendar_Fecha_Inicial" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                        <td align="left">
                            Al</td>
                        <td align="left">
                            <asp:TextBox ID="Txt_Fecha_Final_B" runat="server" Width="150px" Enabled="False"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Calendar_Fecha_Final" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                ToolTip="Seleccione la Fecha Final" />
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="Txt_Fecha_Final_B" PopupButtonID="Btn_Calendar_Fecha_Final" OnClientShown="calendarShown" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            &nbsp;</td>
                        <td align="left" colspan="3">
                            &nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td colspan="4">
                            <center>
                                <asp:Button ID="Btn_Aceptar_Busqueda" runat="server" Text="Aceptar" Width="100px" 
                                onclick="Btn_Aceptar_Busqueda_Click" CssClass="button" Enabled="False"/>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Btn_Cancelar_Busqueda" runat="server" Text="Cancelar" Width="100px" 
                                CssClass="button" onclick="Btn_Cancelar_Busqueda_Click"/>
                           </center>
                        </td>
                    </tr>
                </table>
           </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger  ControlID="Btn_Busqueda_Avanzada" EventName="Click"/>            
          </Triggers>    
        </asp:UpdatePanel> 
        </center>
        </asp:Panel> 
</asp:Content>
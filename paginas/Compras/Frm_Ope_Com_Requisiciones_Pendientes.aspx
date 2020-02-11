<%@ Page Language="C#" 
MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" 
AutoEventWireup="true" 
CodeFile="Frm_Ope_Com_Requisiciones_Pendientes.aspx.cs" 
Inherits="paginas_Compras_Frm_Ope_Com_Requisiciones_Pendientes"
Title="Requisiciones" %>

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
         <%--<asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress">
                    <img alt="" src="../Imagenes/paginas/Updating.gif" />
                </div>
            </ProgressTemplate>
           </asp:UpdateProgress>--%>
           <!--Div de Contenido -->
             <div id="Div_Contenido" style="width: 97%; height: 100%;">
               <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
                <tr>
                    <td colspan ="2" class="label_titulo"><asp:Label ID="Lbl_Titulo" runat="server"></asp:Label></td>
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
                    <td style="width:20%;">
                        <asp:ImageButton ID="Btn_Salir" runat="server" AlternateText="NUEVO" ToolTip="Salir" 
                            CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                            onclick="Btn_Salir_Click" />
                    </td>                    
                    <td align="right" style="width:80%;">
                            <div id="Div_Busqueda" runat="server">
                                 <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                                 ToolTip="Avanzada" onclick="Btn_Busqueda_Avanzada_Click">B&uacute;squeda</asp:LinkButton>
                                 &nbsp;&nbsp;
                                 <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="10" Width="150px"></asp:TextBox>
                                 <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                 TargetControlID="Txt_Busqueda" InvalidChars="<,>,&,',!," 
                                 FilterType="Numbers" 
                                 Enabled="True">   
                                 </cc1:FilteredTextBoxExtender>
                                 <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                 WatermarkCssClass="watermarked"
                                 WatermarkText="<Ingrese No. Requisición>"
                                 TargetControlID="Txt_Busqueda" />
                                 <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                 ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Click" 
                                 ToolTip="Consultar" />
                            </div>
                    </td>
                </tr>
                
                <!--Grid con las requisiciones pendientes-->
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                
                <div id="Div_Requisiciones_Pendientes" runat="server" style="width: 97%; height: 100%;"> <!--Div_Requisiciones_Pendientes--> 
                <tr>
                    <td colspan="2">
                          
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                                  <tr>
                                      <td align="center">
                                        <asp:GridView ID="Grid_Requisiciones_Pendientes" runat="server" AllowPaging="True" 
                                        AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                        PageSize="5" Width="99%" style="white-space:normal;"
                                        onpageindexchanging="Grid_Requisiciones_Pendientes_PageIndexChanging" 
                                        onrowdatabound="Grid_Requisiciones_Pendientes_RowDataBound">
                                       <Columns>
                                        <asp:TemplateField FooterText="Select">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Seleccionar_Requisicion" runat="server" 
                                                    CommandArgument='<%# Eval("NO_REQUISICION") %>' 
                                                    CommandName="Select" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png" 
                                                    OnClick="Btn_Seleccionar_Requisicion_Click" ToolTip="Ver" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FOLIO" HeaderText="Folio" 
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NO_REQUISICION" HeaderText="No Requisici&oacute;n" 
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA" HeaderText="Fecha" 
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center" 
                                            DataFormatString="{0:dd/MMM/yyyy}" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AREA" HeaderText="&Aacute;rea" 
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Operaci&oacute;n" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Img_Btn_Revisar" runat="server" ImageUrl="~/paginas/imagenes/paginas/accept.png" ToolTip="Revisar" OnClientClick="return confirm('¿Est&aacute; seguro de revisar la requisici&oacute;n seleccionada?');" OnClick="Img_Btn_Revisar_Click" Visible="false"/>
                                                <asp:ImageButton ID="Img_Btn_Filtrado" runat="server" ImageUrl="~/paginas/imagenes/gridview/Filter2HS.png" ToolTip="Filtrado" OnClientClick="return confirm('¿Est&aacute; seguro de filtrar la requisici&oacute;n seleccionada?');" OnClick="Img_Btn_Filtrado_Click"/>
                                                <asp:HyperLink ID="Hyp_Lnk_Operacion" runat="server"></asp:HyperLink>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MOSTRAR" HeaderText="Mostrar" />
                                        <asp:BoundField DataField="TIPO_PRODUCTO" HeaderText="Tipo_Producto" />
                                        <asp:BoundField DataField="PRODUCTO_ID" HeaderText="Producto_ID" />
                                        <asp:BoundField DataField="TIPO_RESGUARDO" HeaderText="Tipo_Resguardo" />
                                        <asp:BoundField DataField="FECHA_ADQUISICION" HeaderText="Fecha_Adquisicion" />
                                        <asp:BoundField DataField="PRODUCTO" HeaderText="Producto" Visible="False" />
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                                      </td>
                                  </tr> 
                            </table>
                    </td>
                </tr>
                </div>
                
                  <div id="Div_Productos_Requisicion" runat="server" style="width: 97%; height: 100%;"> <!--Div_Productos_Requisicion--> 
                 <tr>
                     <td colspan ="2">
                          <table width="97%" class="estilo_fuente">
                            <tr>
                                <td style="width:15%">
                                    <asp:Label ID="Lbl_Folio" runat="server" Text="Folio"></asp:Label>
                                </td>
                                <td style="width:40%">
                                    <asp:TextBox ID="Txt_Folio" runat="server" Enabled="false" Width="90%"></asp:TextBox>
                                </td>
                                <td style="width:15%">
                                   <asp:Label ID="Lbl_Area" runat="server" Text="Área"></asp:Label>
                                </td>
                                <td style="width:40%">
                                    <asp:TextBox ID="Txt_Area" runat="server" Enabled="false" Width="90%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:15%">
                                    <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus"></asp:Label>
                                </td>
                                <td style="width:40%">
                                    <asp:TextBox ID="Txt_Estatus" runat="server" Enabled="false" Width="90%"></asp:TextBox>
                                </td>
                                <td style="width:15%">
                                    <asp:Label ID="Lbl_Dependencia" runat="server" Text="Unidad Responsable"></asp:Label>
                                </td>
                                <td style="width:40%">
                                    <asp:TextBox ID="Txt_Dependencia" runat="server" Enabled="false" Width="90%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:15%">
                                    <asp:Label ID="Lbl_Usuario_Genero" runat="server" Text="Usuario Creó"></asp:Label>
                                </td>
                                <td style="width:40%">
                                    <asp:TextBox ID="Txt_Usuario_Creo" runat="server" Enabled="false" Width="90%"></asp:TextBox>
                                </td>
                                <td style="width:15%">
                                     <asp:Label ID="Lbl_Fecha" runat="server" Text="Fecha Generación"></asp:Label>
                                </td>
                                <td style="width:40%">
                                    <asp:TextBox ID="Txt_Fecha_Generación" runat="server" Enabled="false" Width="90%" DataFormatString="{0:dd/MMM/yyyy}"></asp:TextBox>
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
                     <td colspan="2" align="center">
                     </td>
                 </tr>
                 <tr>
                     <td colspan="2">
                        <asp:Label ID="Lbl_Productos_Requisicion" runat="server" Text="Productos de la Requisición" CssClass="estilo_fuente"> </asp:Label>
                     </td>
                 </tr>
                 <tr>
                    <td colspan="2" align="center">
                           <asp:GridView ID="Grid_Productos_Requisicion" runat="server" 
                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                            Width="100%"  AllowPaging="True" 
                            onselectedindexchanging="Grid_Productos_Requisicion_SelectedIndexChanging">
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                                <asp:BoundField DataField="PRODUCTO" HeaderText="Producto">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CANTIDAD" HeaderStyle-HorizontalAlign="Left" 
                                    HeaderText="Cantidad">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MONTO_TOTAL" HeaderText="Total " >
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                            </Columns>
                            <PagerStyle CssClass="GridHeader" />
                            <SelectedRowStyle CssClass="GridSelected" />
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                        </asp:GridView>
                    </td>
                </tr> 
                 </div>                
            </table>
           </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Btn_Aceptar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    
           <asp:UpdatePanel ID="Udp_Modal_Popup" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <cc1:ModalPopupExtender ID="Modal_Busqueda" runat="server" BackgroundCssClass="popUpStyle" 
                    PopupControlID="Pnl_Busqueda" 
                    TargetControlID="Btn_Comodin_Open" 
                    CancelControlID="Btn_Cancelar" 
                    DropShadow="True" DynamicServicePath="" Enabled="True" />  
                   <asp:Button  Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Open" runat="server" Text="" />  
                </ContentTemplate>          
            </asp:UpdatePanel>      
    
           <asp:UpdatePanel ID="Udp_Modal_Popup_2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <cc1:ModalPopupExtender ID="Modal_Filtrado" runat="server" BackgroundCssClass="popUpStyle" 
                    PopupControlID="Pnl_Comentarios_Filtrado" 
                    TargetControlID="Btn_Comodin_Op" 
                    CancelControlID="Btn_Cancelar_Filtrado" 
                    DropShadow="True" DynamicServicePath="" Enabled="True" />  
                   <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Op" runat="server" Text="" />
                </ContentTemplate>          
            </asp:UpdatePanel>  
    
    <asp:Panel ID="Pnl_Busqueda" runat="server" Width="490px" Height="270px"
        style="border-style:outset;border-color:Silver;background-repeat:repeat-y;background-color:White;color:White;display:none; ">
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
                                    Text="Rengo Fechas" oncheckedchanged="Chk_Fecha_CheckedChanged" />
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
                                <asp:CheckBox ID="Chk_Estatus" runat="server" Text="Estatus" 
                                    AutoPostBack="true" oncheckedchanged="Chk_Estatus_CheckedChanged" />
                            </td>
                            <td align="left" colspan="3">
                                <asp:DropDownList ID="Cmb_Estatus_Busqueda" runat="server" Width="355px" Enabled="False">
                                    <asp:ListItem Text="&lt;SELECCIONE&gt;" Value=""></asp:ListItem>
                                    <asp:ListItem Text="AUTORIZADA" Value="AUTORIZADA"></asp:ListItem>
                                    <asp:ListItem Text="ALMACEN" Value="ALMACEN"></asp:ListItem>
                                    <asp:ListItem Text="CONFIRMADA" Value="CONFIRMADA"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:CheckBox ID="Chk_Dependencia" runat="server" Text="U. Responsable" 
                                    AutoPostBack="true" oncheckedchanged="Chk_Dependencia_CheckedChanged" />
                            </td>
                            <td align="left" colspan="3">
                                <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="355px" AutoPostBack="true" Enabled="False" OnSelectedIndexChanged="Cmb_Dependencia_SelectedIndexChanged"></asp:DropDownList>
                            </td>                        
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:CheckBox ID="Chk_Area" runat="server" Text="Area" AutoPostBack="true" 
                                    oncheckedchanged="Chk_Area_CheckedChanged" />
                            </td>
                            <td align="left" colspan="3">
                                <asp:DropDownList ID="Cmb_Area" runat="server" Width="355px" Enabled="False"></asp:DropDownList>
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
                                            <asp:Button ID="Btn_Aceptar" runat="server" Text="Aceptar" Width="100px" CssClass="button" OnClick="Btn_Aceptar_Click" />       
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;       
                                            <asp:Button ID="Btn_Cancelar" runat="server" CssClass="button" onclick="Btn_Cancelar_Click" Text="Cancelar" Width="100px" />
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
    </asp:Panel>
    
    <!--Panel para los comentarios del filtrado-->
    <asp:Panel ID="Pnl_Comentarios_Filtrado" runat="server" Width="450px" Height="100%"
       style="border-style:outset;border-color:Silver;background-repeat:repeat-y;background-color:White;color:White;display:none;">
       <asp:UpdatePanel ID="Upd_Filtrado" runat="server" UpdateMode="Conditional">
           <ContentTemplate>
               <center>
                   <table width="95%" class="estilo_fuente">
                       <tr>
                            <td colspan="2" class="barra_busqueda" align="center">
                             <asp:Label ID="Lbl_Titulo_Operacion" runat="server" Text="" Width="100%"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            <br /> 
                            </td>
                        </tr>
                       <tr>
                            <td colspan="2">
                                <asp:Image ID="Img_Error_Filtrado" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                Width="24px" Height="24px" Visible="false"/>
                                <asp:Label ID="Lbl_Error_Filtrado" runat="server" ForeColor="Red" Width="90%" Visible="false" />
                            </td>                       
                       </tr>
                       <tr>
                            <td colspan="2">
                            </td>
                       </tr>
                       <tr>
                           <td align="left" >
                               <asp:Label ID="lbl_No_Requisicion" runat="server" Text="No. Requisición" Width="120px"></asp:Label>
                           </td>
                           <td align="left">
                           <asp:TextBox ID="Txt_No_Requisicion_Filtrado" runat="server" Width="280px"></asp:TextBox>
                           </td>
                       </tr>
                       <tr>
                           <td align="left">
                               <asp:Label ID="Lbl_Comentario" runat="server" Text="Comentarios"></asp:Label>
                           </td>
                           <td>
                               <asp:TextBox ID="Txt_Comentarios_Filtrado" runat="server" Width="280px" 
                                   TextMode="MultiLine" Height="45px"></asp:TextBox>
                                   
                                   <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                    runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    TargetControlID="Txt_Comentarios_Filtrado" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/ ">
                                    </cc1:FilteredTextBoxExtender>
                                    
                               <cc1:TextBoxWatermarkExtender ID="Txt_Comentarios_Filtrado_TextBoxWatermarkExtender" runat="server" WatermarkCssClass="watermarked"
                                   WatermarkText="<Límite de Caracteres 250>" 
                                   TargetControlID="Txt_Comentarios_Filtrado" />                    
                           </td>
                       </tr>
                       <tr>
                           <td align="left"  >
                           <asp:Button ID="Btn_Aceptar_Filtrado" runat="server" Text="Aceptar" Width="100px" CssClass="button" OnClick="Btn_Aceptar_Filtrado_Click" />
                           </td>
                           <td align="right"><asp:Button ID="Btn_Cancelar_Filtrado" runat="server" 
                            Text="Cancelar" Width="100px" CssClass="button" 
                            onclick="Btn_Cancelar_Filtrado_Click" /></td>
                       </tr>
                   </table>
               </center>
           </ContentTemplate>
       </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
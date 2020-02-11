<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Caj_Folios_Inutilizados_General.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Caj_Folios_Inutilizados_General" Title="Untitled Page" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
 <script type="text/javascript" language="javascript">
        //Metodo para mantener los calendarios en una capa mas alat.
         function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
        //Metodos para limpiar los controles de la busqueda.
        function Limpiar_Ctlr(){
            $("input[id$=Txt_Busqueda_No_Empleado]").val("");
            $("input[id$=Txt_Busqueda_Nombre_Empleado]").val("");
            $("[id$=Lbl_Numero_Registros]").text("");
            $('[id$=Lbl_Error_Busqueda]').text("");
            $('[id$=Lbl_Error_Busqueda]').css("display", "none");
            $('[id$=Img_Error_Busqueda]').hide();
            $("#grid").remove();
            return false;
        }  
        function Abrir_Modal_Popup() {
            $find('Busqueda_Empleados').show();
            return false;
        }
        function Cerrar_Modal_Popup() {
            $find('Busqueda_Empleados').hide();
            Limpiar_Ctlr();
            return false;
        } 
</script>  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <div id="Div_Contenido" style="background-color:#ffffff; width:100%; height:100%;">
                    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                           Reporte de Folios Inutilizados
                        </td>
                    </tr>
                    <tr>
                         <td class="style1">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top" >
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text=""  CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                          
                        </td>
                    </tr>
               </table>             
               <table width="98%"  border="0" cellspacing="0">
                <tr align="center">
                    <td colspan="2">                
                        <div style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td align="left" style="width:59%;">
                                        <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button" TabIndex="4"
                                            ImageUrl="~/paginas/imagenes/gridview/grid_print.png" OnClick="Btn_Imprimir_Click" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="5"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            OnClick = "Btn_Salir_Click"/>
                                    </td>
                                    <td align="right" style="width:41%;">&nbsp;</td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                </table>  
                <br />
                <table width="98%" class="estilo_fuente"> 
                    <tr>
                        <td style="width:12%">Modulo</td>
                        <td colspan="3">
                            <asp:DropDownList ID="Cmb_Modulo" runat="server" Width="95%" 
                                onselectedindexchanged="Cmb_Modulo_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Empleado</td>
                        <td style="width:55%">
                            <asp:TextBox ID="Txt_Empleado" runat="server" Width="80%"></asp:TextBox>
                            &nbsp;
                            <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Buscar Empleado" CssClass="Img_Button" TabIndex="5"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" OnClientClick="javascript:return Abrir_Modal_Popup();"/>
                            <asp:HiddenField ID="HF_Empleado_ID" runat="server" />
                            <cc1:ModalPopupExtender ID="Mpe_Busqueda_Empleados" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="Busqueda_Empleados"
                                PopupControlID="Pnl_Busqueda_Contenedor" TargetControlID="Btn_Comodin_Open" PopupDragHandleControlID="Pnl_Busqueda_Cabecera" 
                                CancelControlID="Btn_Comodin_Close" DropShadow="True" DynamicServicePath="" Enabled="True"/>  
                            <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Close" runat="server" Text="" />
                            <asp:Button  Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Open" runat="server" Text="" />
                        </td>
                        <td style="width:6%">Caja</td>
                        <td style="width:30%">
                            <asp:DropDownList ID="Cmb_Caja" runat="server" Width="84%"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                       <td>Fecha</td>
                       <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="79%" Enabled = "false" MaxLength="10"/>
                                         <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Fecha_Inicio" runat="server" 
                                            TargetControlID="Txt_Fecha_Inicio" WatermarkCssClass="watermarked" 
                                            WatermarkText="Dia/Mes/Año" Enabled="True" />
                                        <cc1:CalendarExtender ID="CE_Txt_Fecha_Inicio" runat="server" 
                                            TargetControlID="Txt_Fecha_Inicio" Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Txt_Fecha_Inicio"/>
                                        <asp:ImageButton ID="Btn_Txt_Fecha_Inicio" runat="server"
                                            ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                            Height="18px" CausesValidation="false"/>
                                   </td>
                                   <td >&nbsp;&nbsp;al &nbsp;&nbsp;&nbsp;</td>
                                   <td>
                                         <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="79%" Enabled = "false" MaxLength="10"/>
                                         <cc1:TextBoxWatermarkExtender ID="TBE_Txt_Fecha_Fin" runat="server" 
                                            TargetControlID="Txt_Fecha_Fin" WatermarkCssClass="watermarked" 
                                            WatermarkText="Dia/Mes/Año" Enabled="True" />
                                        <cc1:CalendarExtender ID="CE_Txt_Fecha_Fin" runat="server" 
                                            TargetControlID="Txt_Fecha_Fin" Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Txt_Fecha_Fin"/>
                                        <asp:ImageButton ID="Btn_Txt_Fecha_Fin" runat="server"
                                            ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                            Height="18px" CausesValidation="false"/>
                                   </td>
                                </tr>
                            </table>
                       </td>
                       <td colspan="2">&nbsp;</td>
                    </tr>
                </table>
             </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="Btn_Busqueda_Empleados" EventName="Click"/>--%>
            <asp:PostBackTrigger   ControlID="Grid_Empleados"/>
        </Triggers>
    </asp:UpdatePanel>
    
     <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="650px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" 
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Img_Informatcion_Autorizacion" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     B&uacute;squeda: Empleados
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClientClick="javascript:return Cerrar_Modal_Popup();"/>  
                </td>
            </tr>
        </table>            
    </asp:Panel>                                                                          
           <div style="color: #5D7B9D">
             <table width="100%">
                <tr>
                    <td align="left" style="text-align: left;" >                                    
                        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Prestamos" runat="server">
                            <ContentTemplate>
                            
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server" AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Prestamos" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                                        <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress"><img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress> 
                                                             
                                  <table width="100%">
                                   <tr>
                                        <td colspan="2">
                                            <table style="width:80%;">
                                              <tr>
                                                <td align="left">
                                                  <asp:ImageButton ID="Img_Error_Busqueda" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                                    Width="24px" Height="24px" style="display:none" />
                                                    <asp:Label ID="Lbl_Error_Busqueda" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" style="display:none"/>
                                                </td>            
                                              </tr>         
                                            </table>  
                                        </td>
                                        <td style="width:100%" colspan="2" align="right">
                                            <asp:ImageButton ID="Btn_Limpiar_Ctlr_Busqueda" runat="server" OnClientClick="javascript:return Limpiar_Ctlr();"
                                                ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" ToolTip="Limpiar Controles de Busqueda"/>
                                        </td>
                                    </tr>     
                                   <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                           No Empleado 
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Busqueda_No_Empleado" runat="server" Width="98%" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Empleado" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Busqueda_No_Empleado"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_No_Empleado" runat="server" 
                                                TargetControlID ="Txt_Busqueda_No_Empleado" WatermarkText="Busqueda por No Empleado" 
                                                WatermarkCssClass="watermarked"/>                                                                                                                                          
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;">                                            
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">                                         
                                        </td>                                         
                                    </tr>                                                                                                   
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Nombre
                                        </td>              
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Busqueda_Nombre_Empleado" runat="server" Width="99.5%" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Nombre_Empleado" runat="server" FilterType="Custom, LowercaseLetters, Numbers, UppercaseLetters"
                                                TargetControlID="Txt_Busqueda_Nombre_Empleado" ValidChars="áéíóúÁÉÍÓÚ "/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Nombre_Empleado" runat="server" 
                                                TargetControlID ="Txt_Busqueda_Nombre_Empleado" WatermarkText="Busqueda por Nombre" 
                                                WatermarkCssClass="watermarked"/>                                                                                               
                                        </td>                                         
                                    </tr>
                                    <tr><td colspan="4">&nbsp;</td></tr>
                                    <tr>
                                        <td colspan="4" id="grid">
                                             <asp:GridView ID="Grid_Empleados" runat="server" AllowPaging="True" CssClass="GridView_1" 
                                                AutoGenerateColumns="False" GridLines="None" Width="96.5%"
                                                onselectedindexchanged="Grid_Empleados_SelectedIndexChanged" HeaderStyle-CssClass="tblHead">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle Width="7%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="Empleado_ID" HeaderText="Empleado ID" 
                                                        Visible="True" SortExpression="Empleado_ID">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="No_Empleado" HeaderText="No Empleado" 
                                                        Visible="True" SortExpression="No_Empleado">
                                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Empleado" HeaderText="Nombre" 
                                                        Visible="True" SortExpression="Empleado">
                                                        <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                                        <ItemStyle HorizontalAlign="left" Width="60%" />
                                                    </asp:BoundField>                                    
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            <div style="text-align:center;">
                                                <asp:Label ID="Lbl_Numero_Registros" runat="server" Text=""/>
                                            </div>
                                        </td>
                                    </tr>
                                   <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <center>
                                               <asp:Button ID="Btn_Busqueda_Empleados" runat="server"  Text="Busqueda de Empleados" CssClass="button"  
                                                CausesValidation="false" OnClick="Btn_Busqueda_Empleados_Click" Width="200px"/> 
                                            </center>
                                        </td>                                                     
                                    </tr>                                                                        
                                  </table>                                                                                                                                                              
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Btn_Busqueda_Empleados" EventName="Click"/>
                            </Triggers>                                                                   
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>                                                      
                    </td>
                </tr>
             </table>                                                   
           </div>                 
    </asp:Panel>
    
</asp:Content>


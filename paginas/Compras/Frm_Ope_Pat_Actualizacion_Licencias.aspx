<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pat_Actualizacion_Licencias.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Pat_Actualizacion_Licencias" Title="Actualización de Licencias" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" language="javascript">
        function Mostrar_Calendar(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
          //Metodos para limpiar los controles de la busqueda.
        function Limpiar_Ctlr(){
            document.getElementById("<%=Txt_Busqueda_No_Empleado.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_RFC.ClientID%>").value="";  
            document.getElementById("<%=Cmb_Busqueda_Dependencia.ClientID%>").value="";  
            document.getElementById("<%=Txt_Busqueda_Nombre_Empleado.ClientID%>").value="";                            
            return false;
        }  
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="ScptM_Licencias" runat="server" EnableScriptGlobalization ="true" EnableScriptLocalization = "True" /> 
     
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Licencias" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Licencias" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="4">Actualización de Licencias</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
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
                    <tr>
                        <td colspan="2">&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left">
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button"
                                             AlternateText="Modificar" ToolTip="Modificar" OnClick="Btn_Modificar_Click"/>
                            <asp:ImageButton ID="Btn_Limpiar_Campos" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Width="24px" CssClass="Img_Button"
                                             AlternateText="Limpiar Campos" ToolTip="Limpiar Campos" OnClick="Btn_Limpiar_Campos_Click"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                             AlternateText="Salir" ToolTip="Salir" OnClick="Btn_Salir_Click"/>
                        </td>
                        <td align="right" style="width:80%;">
                           <div id="Div_Busqueda" runat="server">
                                <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White" ToolTip="Busqueda y Selección de Empleado" OnClick="Btn_Busqueda_Avanzada_Click" >Búsqueda</asp:LinkButton>
                                <asp:TextBox ID="Txt_Busqueda_Directa" runat="server" Width="150px" MaxLength="6" style="text-align:center;"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Directa" runat="server" TargetControlID="Txt_Busqueda_Directa" FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Directa" runat="server" TargetControlID="Txt_Busqueda_Directa" WatermarkText="<- No. Empleado ->" WatermarkCssClass="watermarked">
                                </cc1:TextBoxWatermarkExtender>
                                <asp:ImageButton ID="Btn_Busqueda_Directa" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                    onclick="Btn_Busqueda_Directa_Click" />
                           </div>
                        </td>                       
                    </tr>
                </table>   
                <br />
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td colspan="4">
                            <asp:HiddenField ID="Hdf_Empleado_ID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%">
                            <asp:Label ID="Lbl_Dependencia_Empleado" runat="server" Text="U. Responsable"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Dependencia_Empleado" runat="server" ReadOnly="true" Width="99%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%">
                            <asp:Label ID="Lbl_No_Empledo" runat="server" Text="No. Empleado"></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:TextBox ID="Txt_No_Empledo" runat="server" ReadOnly="true" Width="97%"></asp:TextBox>
                        </td>
                        <td style="width:20%">
                            &nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Lbl_RFC_Empleado" runat="server" Text="RFC"></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:TextBox ID="Txt_RFC_Empleado" runat="server" ReadOnly="true" Width="96%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%">
                            <asp:Label ID="Lbl_Nombre_Empleado" runat="server" Text="Nombre"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" ReadOnly="true" Width="99%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4"><hr /></td>
                    </tr>
                    <tr>
                        <td style="width:20%">
                            <asp:Label ID="Lbl_No_Licencia" runat="server" style="font-weight:bolder;" Text="No. Licencia"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_No_Licencia" runat="server" Width="98.5%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Licencia" runat="server" TargetControlID="Txt_No_Licencia"
                                InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_!%&/" Enabled="True">
                            </cc1:FilteredTextBoxExtender>
                            
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%">
                            <asp:Label ID="Lbl_Fecha_Vencimiento_Licencia" runat="server" style="font-weight:bolder;" Text="Fecha Vencimiento"></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:TextBox ID="Txt_Fecha_Vencimiento_Licencia" runat="server" Width="85%" Enabled="false" ></asp:TextBox>
                            <asp:ImageButton ID="Btn_Fecha_Vencimiento_Licencia" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Vencimiento_Licencia" runat="server" TargetControlID="Txt_Fecha_Vencimiento_Licencia" PopupButtonID="Btn_Fecha_Vencimiento_Licencia" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                        <td style="width:20%">
                            <asp:Label ID="Lbl_Tipo_Licencia" runat="server" style="font-weight:bolder;" Text="Tipo"></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:TextBox ID="Txt_Tipo_Licencia" runat="server" Width="96%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Tipo_Licencia" runat="server" TargetControlID="Txt_Tipo_Licencia"
                                InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_!%&/" Enabled="True">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>  
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Grid_Listado_Empleados" EventName="SelectedIndexChanged" />
        </Triggers>         
    </asp:UpdatePanel>  
    
    <asp:UpdatePanel ID="UpPnl_aux_Busqueda_Resguardante" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_MPE_Resguardante" runat="server" Text="" style="display:none;"/>
                <cc1:ModalPopupExtender ID="MPE_Busqueda_Empleados" runat="server" 
                TargetControlID="Btn_Comodin_MPE_Resguardante" PopupControlID="Pnl_Busqueda_Contenedor" 
                CancelControlID="Btn_Cerrar_Ventana" PopupDragHandleControlID="Pnl_Busqueda_Resguardante_Cabecera"
                DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>  
        </ContentTemplate>
    </asp:UpdatePanel>  
         
    <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="850px" 
            style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
            <asp:Panel ID="Pnl_Busqueda_Resguardante_Cabecera" runat="server" 
                style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                <table width="99%">
                    <tr>
                        <td style="color:Black;font-size:12;font-weight:bold;">
                           <asp:Image ID="Img_Informatcion_Autorizacion" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                             B&uacute;squeda: Empleados
                        </td>
                        <td align="right" style="width:10%;">
                           <asp:ImageButton ID="Btn_Cerrar_Ventana" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                                ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
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
                                        <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress"><img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress> 
                                                             
                                  <table width="100%">
                                   <tr>
                                        <td style="width:100%" colspan="4" align="right">
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
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Empleado" runat="server" FilterType="Numbers" TargetControlID="Txt_Busqueda_No_Empleado"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_No_Empleado" runat="server" TargetControlID ="Txt_Busqueda_No_Empleado" WatermarkText="Busqueda por No Empleado" 
                                                WatermarkCssClass="watermarked"/>                                                                                                                                          
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            RFC
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Busqueda_RFC" runat="server" Width="98%" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_RFC" runat="server" FilterType="Numbers, UppercaseLetters"
                                                TargetControlID="Txt_Busqueda_RFC"/>  
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_RFC" runat="server" 
                                                TargetControlID ="Txt_Busqueda_RFC" WatermarkText="Busqueda por RFC" 
                                                WatermarkCssClass="watermarked"/>                                                                                                                                     
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
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Unidad Responsable
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;" colspan="3">
                                           <asp:DropDownList ID="Cmb_Busqueda_Dependencia" runat="server" Width="100%" />                                          
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
                                                CausesValidation="false"  Width="200px" OnClick="Btn_Busqueda_Empleados_Click" /> 
                                            </center>
                                        </td>                                                     
                                    </tr>                                                                        
                                  </table>   
                                  <br />
                                  <div id="Div_Resultados_Busqueda_Resguardantes" runat="server" style="border-style:outset; width:99%; height: 250px; overflow:auto;">
                                      <asp:GridView ID="Grid_Listado_Empleados" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            ForeColor="#333333" GridLines="None" AllowPaging="True" Width="100%" 
                                            PageSize="100" EmptyDataText="No se encontrarón resultados para los filtros de la busqueda" 
                                            OnSelectedIndexChanged="Grid_Listado_Empleados_SelectedIndexChanged"
                                            OnPageIndexChanging="Grid_Listado_Empleados_PageIndexChanging"
                                            >
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="30px" Font-Size="X-Small" HorizontalAlign="Center" />
                                                    <HeaderStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="EMPLEADO_ID" HeaderText="EMPLEADO_ID" SortExpression="EMPLEADO_ID">
                                                    <ItemStyle Width="3px" Font-Size="X-Small" HorizontalAlign="Center" />
                                                    <HeaderStyle Width="3px" Font-Size="X-Small" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="NO_EMPLEADO" HeaderText="No. Empleado" SortExpression="NO_EMPLEADO" >
                                                    <ItemStyle Width="70px" Font-Size="X-Small" HorizontalAlign="Center" />
                                                    <HeaderStyle Width="70px" Font-Size="X-Small" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE" NullDisplayText="-" >
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" />
                                                    <HeaderStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DEPENDENCIA" HeaderText="Unidad Responsable" SortExpression="DEPENDENCIA" NullDisplayText="-" >
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" />
                                                    <HeaderStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                            </Columns>
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView> 
                                </div>                                                                                                                                                          
                            </ContentTemplate>                                                                   
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


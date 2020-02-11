<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Vacaciones_Recursos_Humanos.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Vacaciones_Empleado" Title="Autorizacion Vacaciones Recursos Humanos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">

<script src="../../javascript/Js_Ope_Nom_Vacaciones_UR.js" type="text/javascript"></script>

 <script type="text/javascript" language="javascript">
        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
        function Limpiar_Ctlr(){
            document.getElementById("<%=Txt_Busqueda_No_Vacacion.ClientID %>").value="";
            document.getElementById("<%=Cmb_Busqueda_Estatus.ClientID %>").value = "";
            document.getElementById("<%=Cmb_Busqueda_Calendario_Nomina.ClientID %>").value = "";
            document.getElementById("<%=Cmb_Busqueda_Periodos_Catorcenales_Nomina.ClientID %>").value = "";            
            document.getElementById("<%=Cmb_Busqueda_Dependencia.ClientID %>").value="";
            document.getElementById("<%=Txt_Busqueda_Fecha_Inicio.ClientID %>").value="";
            document.getElementById("<%=Txt_Busqueda_Fecha_Fin.ClientID %>").value="";
            return false;
        }
        
        function Autorizar(){
           if( document.getElementById("<%=Chk_Autorizar.ClientID %>").checked){
                document.getElementById("<%=Txt_Autorizacion_Comentarios.ClientID %>").disabled = true;
           }else{
                document.getElementById("<%=Txt_Autorizacion_Comentarios.ClientID %>").disabled = false;
           }
        }
        
        function Abrir_Modal_Popup() {
            $find('MPE_Autoarizacion').show();
            document.getElementById("<%=Txt_Autorizacion_No_Empleado.ClientID %>").value = document.getElementById("<%=Txt_No_Empleado.ClientID %>").value;
            document.getElementById("<%=Txt_Autorizacion_Nombre_Empleado.ClientID %>").value = document.getElementById("<%=Cmb_Empleado.ClientID %>").options[document.getElementById("<%=Cmb_Empleado.ClientID %>").selectedIndex].text;
            return false;
        }
        
        function pageLoad(){
            inicializarEventosVacaciones();
            $('input[id$=Txt_No_Empleado]').keydown(function(event){
              if (event.keyCode == 13) {
                return false;
              }              
            });
        }
        
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
 </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Vacaciones_Empleados" runat="server"  AsyncPostBackTimeout="36000"  EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>

            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
             <div id="Div_Vacaciones_Empleado" style="background-color:#ffffff; width:100%; height:100%;">
                 
                    <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                        <tr align="center">
                            <td class="label_titulo">Recursos Humanos Seguimiento Vacaciones</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false"  />&nbsp;
                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                            </td>
                        </tr>
                    </table>                                                             
                    
                    <table width="98%"  border="0" cellspacing="0">
                             <tr align="center">
                                 <td colspan="2">                
                                     <div align="right" class="barra_busqueda">                        
                                          <table style="width:100%;height:28px;">
                                            <tr>
                                              <td align="left" style="width:59%;">                                                  
                                                   <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" 
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" 
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" 
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                                        OnClientClick="return confirm('¿Está seguro de eliminar la vacacion seleccionado?');"/>
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" 
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                              </td>
                                              <td align="right" style="width:41%;">
                                                <table style="width:100%;height:28px;">
                                                    <tr>
                                                        <td style="vertical-align:middle;width:5%;" >
                                                            B&uacute;squeda:                                                        
                                                            <asp:ImageButton ID="Btn_Mostrar_Busqueda" runat="server" ToolTip="Avanzada" 
                                                               ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px" Width="24px"
                                                               OnClientClick="javascript:$find('Busqueda_Empleados').show();return false;" CausesValidation="false"/>                                        
                                                        </td>
                                                    </tr>                                                                          
                                                </table>                                    
                                               </td>       
                                             </tr>         
                                          </table>                      
                                        </div>
                                 </td>
                             </tr>
                    </table>                      

                <br />
                <asp:Panel ID="Pnl_Busqueda_Dias_Vacaciones" runat="server" Width="97%" 
                    style="background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset; color:White;">               
                    <table width="98%">
                        <tr>
                            <td  style="text-align:left;width:50%; color:Black;">
                                <table style="width:100%;">
                                    <tr>
                                        <td style="width:40%;">
                                            <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="98%" MaxLength="6" Enabled="false"
                                                AutoPostBack="true" OnTextChanged="Txt_Consultar_Dias_Vacaciones_Click"/>
                                            <cc1:TextBoxWatermarkExtender ID="TBW_Txt_Empleados" runat="server" TargetControlID="Txt_No_Empleado"
                                                WatermarkCssClass="watermarked2" WatermarkText="No Empleado"/>
                                            <cc1:FilteredTextBoxExtender ID="FTxt_No_Empleado" 
                                                 runat="server" TargetControlID="Txt_No_Empleado" FilterType="Numbers"/>  
                                        </td>
                                        <td style="width:60%; vertical-align:middle; text-align:left;">
                                             <asp:ImageButton ID="Btn_Consultar_Dias_Vacaciones" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                 OnClick="Btn_Consultar_Dias_Vacaciones_Click"/>  
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="text-align:left;width:50%; color:Black;" colspan="2">   
                                 <center>
                                     <div id="Div_Mensajes_Dias_Vacaciones" runat="server" visible="false" style="width:90%;z-index:1000;background-color:#F0F8FF;color:Black;font-weight:bold;">
                                        <center>
                                            Dias de Vacaciones Disponibles:&nbsp;&nbsp; 
                                            <asp:Label ID="Lbl_Dias_Vacaciones" runat="server" Font-Size="12px" Font-Bold="true" ForeColor="Black"/>  
                                        </center>
                                     </div>
                                 </center>                                                                                   
                            </td>                    
                        </tr>                       
                     </table>      
                 </asp:Panel>

                 <br />
                 
                 <asp:Panel ID="Pnl_Datos_Vacaciones_Empleado" runat="server" Width="100%">
                     <table width="98%">
                        <tr style="display :none;">
                            <td style="text-align:left;width:20%;">
                                No Vacaci&oacute;n
                            </td>
                            <td style="text-align:left;width:30%;">     
                                <asp:TextBox ID="Txt_No_Vacacion" runat="server" Width="98%"/>
                            </td>
                            <td style="text-align:left;width:20%;">
                                
                            </td>
                            <td style="text-align:left;width:30%;">     

                            </td>                        
                        </tr>
                        <tr id="Tr_Periodos_Fiscales" runat="server">
                            <td style="text-align:left;width:20%;">
                                N&oacute;mina
                            </td>
                            <td  style="text-align:left;width:30%;" >
                                <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" AutoPostBack="true" 
                                    OnSelectedIndexChanged="Cmb_Calendario_Nomina_SelectedIndexChanged"/>
                            </td> 
                            <td style="text-align:left;width:20%;">
                               &nbsp;&nbsp;Periodo
                            </td>
                            <td  style="text-align:left;width:30%;">   
                                <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" Width="100%"/>
                            </td>                                                                       
                        </tr>                           
                       <tr>
                            <td style="text-align:left;width:20%;">                          
                                *U. Responsable
                            </td>
                            <td style="text-align:left;width:80%;" colspan="3">     
                                <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="100%" />
                            </td>
                       </tr>                           
                       <tr>                            
                            <td style="text-align:left;width:20%;">
                               *Empleado
                            </td>
                            <td style="text-align:left;width:80%;" colspan="3">
                                <asp:DropDownList ID="Cmb_Empleado" runat="server" Width="100%"/>                          
                            </td>       
                        </tr>       
                        <tr>
                            <td style="text-align:left;width:20%;">
                                *Dias de Vacaciones   
                            </td>
                            <td  style="text-align:left;width:30%;">
                                <asp:TextBox ID="Txt_Dias_Vacaciones_Empleado" runat="server" Width="98%"  MaxLength="10"
                                    OnTextChanged="Txt_Dias_Vacaciones_Empleado_TextChanged" AutoPostBack="true"/>
                                <cc1:FilteredTextBoxExtender ID="FTxt_Dias_Vacaciones_Empleado" 
                                     runat="server" TargetControlID="Txt_Dias_Vacaciones_Empleado" FilterType="Numbers"/>                                                 
                            </td>
                            <td style="text-align:left;width:20%;">
                                &nbsp;&nbsp;Estatus                 
                            </td>
                            <td  style="text-align:left;width:30%;">                            
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%" >
                                    <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                    <asp:ListItem>Aceptado</asp:ListItem>
                                    <asp:ListItem>Autorizado</asp:ListItem>
                                    <asp:ListItem>Rechazado</asp:ListItem>
                                </asp:DropDownList>                                                          
                            </td>                        
                        </tr>                                               
                        <tr>
                            <td style="text-align:left;width:20%;">
                                *Fecha Inicio
                            </td>
                            <td  style="text-align:left;width:30%;">
                                <asp:TextBox ID="Txt_Fecha_Inicio_Vacaciones" runat="server" Width="85%" MaxLength="1" 
                                    AutoPostBack="true" ontextchanged="Txt_Fecha_Inicio_Vacaciones_TextChanged"
                                    />      
                                <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Inicio_Vacaciones_FilteredTextBoxExtender" 
                                    runat="server" TargetControlID="Txt_Fecha_Inicio_Vacaciones" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                    ValidChars="/"/>
                                <cc1:CalendarExtender ID="Txt_Fecha_Inicio_Vacaciones_CalendarExtender" runat="server" 
                                    TargetControlID="Txt_Fecha_Inicio_Vacaciones" PopupButtonID="Btn_Fecha_Inicio_Vacaciones" Format="dd/MMM/yyyy"/>
                                <asp:ImageButton ID="Btn_Fecha_Inicio_Vacaciones" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha Final"/>                                                    
                            </td>
                            <td style="text-align:left;width:20%;">
                                &nbsp;&nbsp;*Fecha Termino                          
                            </td>
                            <td  style="text-align:left;width:30%;">                            
                                <asp:TextBox ID="Txt_Fecha_Termino_Vacaciones" runat="server" Width="85%"/>     
                                <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Termino_Vacaciones_FilteredTextBoxExtender" 
                                    runat="server" TargetControlID="Txt_Fecha_Termino_Vacaciones" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"  
                                    ValidChars="/"/>
                                <cc1:CalendarExtender ID="Txt_Fecha_Termino_Vacaciones_CalendarExtender" runat="server" 
                                    TargetControlID="Txt_Fecha_Termino_Vacaciones" PopupButtonID="Btn_Fecha_Termino_Vacaciones" Format="dd/MMM/yyyy"/>
                                <asp:ImageButton ID="Btn_Fecha_Termino_Vacaciones" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha Final"/>                                                         
                            </td>                        
                        </tr>
                        <tr>
                            <td style="text-align:left;width:20%;">
                                
                            </td>
                            <td  style="text-align:left;width:30%;">
                                                  
                            </td>
                            <td style="text-align:left;width:20%;">
                                &nbsp;&nbsp;*Regreso Vacaciones                     
                            </td>
                            <td  style="text-align:left;width:30%;"> 
                                <asp:TextBox ID="Txt_Fecha_Regreso_Vacaciones" runat="server" Width="85%" MaxLength="1"/>      
                                <cc1:FilteredTextBoxExtender ID="Fte_Fecha_Regreso_Vacaciones" 
                                    runat="server" TargetControlID="Txt_Fecha_Regreso_Vacaciones" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                    ValidChars="/"/>
                                <cc1:CalendarExtender ID="Ce_Fecha_Regreso_Vacaciones" runat="server" 
                                    TargetControlID="Txt_Fecha_Regreso_Vacaciones" PopupButtonID="Btn_Fecha_Regreso_Vacaciones" Format="dd/MMM/yyyy"/>
                                <asp:ImageButton ID="Btn_Fecha_Regreso_Vacaciones" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha Regreso de Vacaciones"/>                                                                                                                
                            </td>                        
                        </tr>                                                 
                        <tr>
                            <td style="text-align:left;width:20%;vertical-align:top;">
                                Comentarios
                            </td>
                            <td  style="text-align:left;width:80%;" colspan="3">
                                <asp:TextBox ID="Txt_Comentarios" runat="server" Width="99%" MaxLength="100" TextMode="MultiLine" Wrap="true" Height="60px"/>
                                <cc1:FilteredTextBoxExtender ID="FTxt_Comentarios" runat="server"  TargetControlID="Txt_Comentarios"
                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>    
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios" runat="server" TargetControlID ="Txt_Comentarios" 
                                    WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/>    
                                <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>                                                    
                            </td>
                        </tr>                                                                                                                                                         
                     </table>
                 </asp:Panel>   
                 
                 <table width="98%">
                        <tr>
                            <td  style="width:100%;" colspan="4" align="center">
                                <asp:Button ID="Btn_Autorizacion_Vacaciones" runat="server" Text="Autorizacion de Vacaciones" Visible="false" 
                                   OnClientClick="javascript:return Abrir_Modal_Popup();" 
                                   style="border-style:outset;border-color:White;background-color:#F0F8FF;color:Black;font-weight:bold;cursor:hand;"/>    
                                     
                                <asp:Button ID="Btn_Cancelar_Vacaciones" runat="server" Text="Cancelar de Vacaciones" Visible="false" 
                                   style="border-style:outset;border-color:White;background-color:#F0F8FF;color:Black;font-weight:bold;cursor:hand;"
                                   OnClick="Btn_Cancelar_Vacaciones_Click"/>                                                                             
                            </td>
                        </tr>  
                 </table>                                  
                 
                 <br />
                 
                 <div style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style: outset;color:White;" >
                     <table width="98%">
                        <tr align="center">
                            <td colspan="4">
                                <asp:GridView ID="Grid_Vacaciones_Empleados" runat="server" CssClass="GridView_1"
                                     AutoGenerateColumns="False"  GridLines="Both"
                                    onpageindexchanging="Grid_Vacaciones_Empleados_PageIndexChanging"
                                    onselectedindexchanged="Grid_Vacaciones_Empleados_SelectedIndexChanged"
                                    OnRowDataBound="Grid_Vacaciones_Empleados_RowDataBound">
                                         <Columns>
                                             <asp:ButtonField ButtonType="Image" CommandName="Select"  HeaderText=""
                                                 ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                 <ItemStyle Width="5%" />
                                             </asp:ButtonField>
                                             <asp:BoundField DataField="NO_VACACION" HeaderText="No Vacacion">
                                                 <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                 <ItemStyle HorizontalAlign="Left" Width="0%" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="EMPLEADO" HeaderText="Empleado">
                                                 <HeaderStyle HorizontalAlign="Left" Width="200px" Font-Size="X-Small" Font-Bold="true"/>
                                                 <ItemStyle HorizontalAlign="Left" Width="200px" Font-Size="X-Small" Font-Bold="true"/>
                                             </asp:BoundField>                                         
                                             <asp:BoundField DataField="FECHA_INICIO" HeaderText="Fecha Inicio" DataFormatString="{0:dd/MMM/yyyy}">
                                                 <HeaderStyle HorizontalAlign="Center" Width="50px" Font-Size="X-Small" Font-Bold="true"/>
                                                 <ItemStyle HorizontalAlign="Center" Width="50px" Font-Size="X-Small" Font-Bold="true"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="FECHA_TERMINO" HeaderText="Fecha Fin" DataFormatString="{0:dd/MMM/yyyy}">
                                                 <HeaderStyle HorizontalAlign="Center" Width="50px" Font-Size="X-Small" Font-Bold="true"/>
                                                 <ItemStyle HorizontalAlign="Center" Width="50px" Font-Size="X-Small"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="CANTIDAD_DIAS" HeaderText="Dias" >
                                                 <HeaderStyle HorizontalAlign="Center" Width="5%" Font-Size="X-Small" Font-Bold="true"/>
                                                 <ItemStyle HorizontalAlign="Center" Width="5%" Font-Size="X-Small" Font-Bold="true"/>
                                             </asp:BoundField>                                                                                                                           
                                             <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                  <HeaderStyle HorizontalAlign="Center" Width="50px" Font-Size="X-Small" Font-Bold="true"/>
                                                  <ItemStyle HorizontalAlign="Center" Width="50px" Font-Size="X-Small" Font-Bold="true"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="COMENTARIOS_ESTATUS" HeaderText="Estatus">
                                                  <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                  <ItemStyle HorizontalAlign="Left" Width="0%" />
                                             </asp:BoundField>                                           
                                         </Columns>
                                         <SelectedRowStyle CssClass="GridSelected" />
                                         <PagerStyle CssClass="GridHeader" />
                                         <HeaderStyle CssClass="GridHeader" />
                                         <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>

            <%--<cc1:ModalPopupExtender 
                ID="MPE_Msj" 
                BehaviorID="Mpe_Busqueda_Vacaciones"
                runat="server" 
                BackgroundCssClass="popUpStyle" 
                PopupControlID="Pnl_Mensaje" 
                TargetControlID="Btn_Mostrar_Busqueda" 
                PopupDragHandleControlID="Pnl_Interno" 
                CancelControlID="Btn_Comodin" 
                DropShadow="True"
                DynamicServicePath=""
                Enabled="True" />
            <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin" runat="server" Text="" />   
            --%>
            
<%--            <cc1:ModalPopupExtender ID="MPE_Autorizacion_Vacaciones" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="MPE_Autoarizacion"
                PopupControlID="Pnl_Autorizacion_Contenedor" TargetControlID="Btn_Comodin_autorizacion2" PopupDragHandleControlID="Pnl_Autorizacion_Cabecera" 
                CancelControlID="Btn_Comodin_autorizacion1" DropShadow="True" DynamicServicePath="" Enabled="True" />  
            <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_autorizacion1" runat="server" Text="" />            
             <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_autorizacion2" runat="server" Text="" />--%>            
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Btn_Busqueda_Vacaciones_Empleados" EventName="Click"/>
        </Triggers>
    </asp:UpdatePanel>  
    
        <asp:UpdatePanel ID="Pnl_Modal" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
                <cc1:ModalPopupExtender ID="MPE_Msj" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="Busqueda_Empleados"
                    PopupControlID="Pnl_Mensaje" TargetControlID="Btn_Axuliar" PopupDragHandleControlID="Pnl_Interno" 
                    CancelControlID="Btn_Comodin" DropShadow="True" DynamicServicePath="" Enabled="True" />  
                <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin" runat="server" Text="" />  
                <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Axuliar" runat="server" Text="" />   
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Btn_Busqueda_Vacaciones_Empleados" EventName="Click"/>
        </Triggers>        
        </asp:UpdatePanel>
        
        <asp:UpdatePanel ID="Pnl_Autoriza" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
              <cc1:ModalPopupExtender ID="MPE_Autorizacion_Vacaciones" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="MPE_Autoarizacion"
                    PopupControlID="Pnl_Autorizacion_Contenedor" TargetControlID="Btn_Comodin_autorizacion2" PopupDragHandleControlID="Pnl_Autorizacion_Cabecera" 
                    CancelControlID="Btn_Comodin_autorizacion1" DropShadow="True" DynamicServicePath="" Enabled="True" />  
                <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_autorizacion1" runat="server" Text="" />            
                <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_autorizacion2" runat="server" Text="" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Btn_Guardar_Autorizacion_Vacacion" EventName="Click"/>
        </Triggers>          
        </asp:UpdatePanel>
    
    
    
<asp:Panel ID="Pnl_Mensaje" runat="server" CssClass="drag" HorizontalAlign="Center" Width="650px"  
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Interno" runat="server" 
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Image1" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Informacion: B&uacute;squeda de Vacaciones de los Empleados
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana_Emergente" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClientClick="javascript: $find('Busqueda_Empleados').hide(); return false;"/>  
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <asp:UpdatePanel ID="a" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>
            <div style="cursor:default;width:100%">
                <table width="100%" style="background-color:#ffffff;">
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
                        <td style="width:20%;text-align:left;">
                            No Empleado
                        </td>
                        <td style="width:30%;text-align:left;">
                            <asp:TextBox ID="Txt_No_Empleado_Busqueda" runat="server" Width="98%" MaxLength="6" />
                            <cc1:FilteredTextBoxExtender ID="Fte_No_Empleado_Busqueda" 
                                 runat="server" TargetControlID="Txt_No_Empleado_Busqueda" FilterType="Numbers"/>
                        </td>
                        <td style="width:20%;text-align:left;">
                        </td>
                        <td style="width:30%;text-align:left;">
                        </td>
                    </tr> 
                    <tr>
                        <td style="width:20%;text-align:left;">
                            No Vacacion
                        </td>
                        <td style="width:30%;text-align:left;">
                            <asp:TextBox ID="Txt_Busqueda_No_Vacacion" runat="server" Width="98%" MaxLength="10"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Busqueda_No_Vacacion" 
                                 runat="server" TargetControlID="Txt_Busqueda_No_Vacacion" FilterType="Numbers"/>
                        </td>
                        <td style="width:20%;text-align:left;">
                            Estatus
                        </td>
                        <td style="width:30%;text-align:left;">
                            <asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="100%">
                                    <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                    <asp:ListItem>Aceptado</asp:ListItem>
                                    <asp:ListItem>Autorizado</asp:ListItem>
                                    <asp:ListItem>Rechazado</asp:ListItem>
                                    <asp:ListItem>Cancelado</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>  
                    
                    <tr>
                        <td  style="width:20%; text-align:left; cursor:default;">
                            N&oacute;mina
                        </td>
                        
                        <td style="width:30%; text-align:left; cursor:default;">
                            <asp:DropDownList ID="Cmb_Busqueda_Calendario_Nomina" runat="server" Width="95%" AutoPostBack="true" 
                                TabIndex="3" onselectedindexchanged="Cmb_Busqueda_Calendario_Nomina_SelectedIndexChanged"/>
                        </td> 
                        
                        <td style="width:20%; text-align:left; cursor:default;">
                            Periodo
                        </td>
                        
                        <td style="width:30%; text-align:left; cursor:default;">
                            <asp:DropDownList ID="Cmb_Busqueda_Periodos_Catorcenales_Nomina" runat="server" Width="95%" TabIndex="4" 
                                AutoPostBack="true" 
                                onselectedindexchanged="Cmb_Busqueda_Periodos_Catorcenales_Nomina_SelectedIndexChanged" />
                        </td> 
                    </tr>
                    
                    <tr>
                        <td style="width:20%;text-align:left;">
                            U. Responsable
                        </td>
                        <td style="width:80%;text-align:left;" colspan="3">
                            <asp:DropDownList ID="Cmb_Busqueda_Dependencia" runat="server" Width="100%"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%;text-align:left;">
                            Fecha Inicio
                        </td>
                        <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_Busqueda_Fecha_Inicio" runat="server" Width="85%" MaxLength="1"/>
                                <cc1:FilteredTextBoxExtender ID="FTxt_Busqueda_Fecha_Inicio" 
                                    runat="server" TargetControlID="Txt_Busqueda_Fecha_Inicio" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                    ValidChars="/"/>
                                <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Inicio" runat="server" 
                                    TargetControlID="Txt_Busqueda_Fecha_Inicio" PopupButtonID="Btn_Busqueda_Fecha_Inicio" Format="dd/MMM/yyyy" 
                                    OnClientShown="calendarShown"/>
                                <asp:ImageButton ID="Btn_Busqueda_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha"/>
                        </td>
                        <td style="width:20%;text-align:left;">
                            Fecha Fin
                        </td>
                        <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_Busqueda_Fecha_Fin" runat="server" Width="85%" MaxLength="1"/>
                                <cc1:FilteredTextBoxExtender ID="FTxt_Busqueda_Fecha_Fin" 
                                    runat="server" TargetControlID="Txt_Busqueda_Fecha_Fin" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                    ValidChars="/"/>
                                <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Fin" runat="server" 
                                    TargetControlID="Txt_Busqueda_Fecha_Fin" PopupButtonID="Btn_Busqueda_Fecha_Fin" Format="dd/MMM/yyyy" 
                                    OnClientShown="calendarShown"/>
                                <asp:ImageButton ID="Btn_Busqueda_Fecha_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha"/>
                        </td>
                    </tr>  
                   <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>
                   <tr>
                        <td style="width:100%" colspan="4" align="center">
                            <asp:Button ID="Btn_Busqueda_Vacaciones_Empleados" runat="server" Text="Buscar Vacaciones" OnClick="Btn_Busqueda_Vacaciones_Empleados_Click"
                                style="border-style:none;background-color:White;font-weight:bold;"  ToolTip="Ejecuatar la búsqueda de vacaciones en el sistema"/>
                        </td>
                    </tr>
                   <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>
                </table>
            </div>        
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Panel>

<asp:Panel ID="Pnl_Autorizacion_Contenedor" runat="server" CssClass="drag" HorizontalAlign="Center" Width="650px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">
    <asp:Panel ID="Pnl_Autorizacion_Cabecera" runat="server" 
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Img_Informatcion_Autorizacion" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Recursos Humanos: Autorizar Vacaciones
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana_Autorizacion" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClientClick="javascript: $find('MPE_Autoarizacion').hide(); return false;"/>  
                </td>
            </tr>
        </table>
    </asp:Panel>
        <div style="cursor:default;width:100%">
            <table width="100%" style="background-color:#ffffff;">
                <tr>
                    <td style="width:100%" colspan="4">
                        &nbsp;
                    </td>
                </tr>    
               <tr>
                    <td style="width:100%" colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;text-align:left;">
                        No Empleado
                    </td>
                    <td style="width:30%;text-align:left;">
                        <asp:TextBox ID="Txt_Autorizacion_No_Empleado" runat="server" Width="98%" MaxLength="10"
                            Enabled="false"/>
                        <cc1:FilteredTextBoxExtender ID="Txt_Autorizacion_No_Empleado_FilteredTextBoxExtender" 
                             runat="server" TargetControlID="Txt_Autorizacion_No_Empleado" FilterType="Numbers"/>
                    </td>
                    <td style="width:20%;text-align:left;">
                    </td>
                    <td style="width:30%;text-align:left;">
                    </td>
                </tr> 
                <tr>
                    <td style="width:20%;text-align:left;">
                        Nombre
                    </td>
                    <td style="width:80%;text-align:left;" colspan="3">
                        <asp:TextBox ID="Txt_Autorizacion_Nombre_Empleado" runat="server" Width="98%"
                            Enabled="false"/>
                    </td>
                </tr>
               
                <tr>
                   <td style="text-align:left;width:20%;vertical-align:top;">
                       Comentarios
                   </td>
                   <td  style="text-align:left;width:80%;" colspan="3">
                        <asp:TextBox ID="Txt_Autorizacion_Comentarios" runat="server" Width="99%" MaxLength="100" TextMode="MultiLine" Enabled="false"/>
                        <cc1:FilteredTextBoxExtender ID="Txt_Autorizacion_Comentarios_FilteredTextBoxExtender" runat="server"  
                            TargetControlID="Txt_Autorizacion_Comentarios" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" 
                            ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>    
                        <cc1:TextBoxWatermarkExtender ID="Txt_Autorizacion_Comentarios_TextBoxWatermarkExtender" runat="server" 
                            TargetControlID ="Txt_Autorizacion_Comentarios" WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/>
                  </td>
               </tr>   
               <tr>
                    <td style="width:100%" colspan="4">
                        <hr />
                    </td>
                </tr>  
                <tr> 
                   <td style="text-align:left;width:100%;vertical-align:top;" align="center" colspan="4">
                        <center>
                           Autorizar
                           <asp:CheckBox ID="Chk_Autorizar"  onclick="javascript:Autorizar();" runat="server" Checked="true"/>
                       </center>
                   </td>
               </tr>
               <tr>
                    <td style="width:100%" colspan="4" align="right">
                        <asp:ImageButton ID="Btn_Guardar_Autorizacion_Vacacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png"
                           CssClass="button_autorizar" style="cursor:hand; width:32px; height:32px;"  ToolTip="Autorizar"
                            OnClick="Btn_Guardar_Autorizacion_Vacacion_Click"/>   
                    </td>
                </tr>
               <tr>
                    <td style="width:100%" colspan="4">
                        <hr />
                    </td>
                </tr>
            </table>
        </div>
</asp:Panel>
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Vacaciones_Empleados.aspx.cs" Inherits="paginas_Nomina_Frm_Nom_Vacaciones_Empleados" Title="Solicitud de Vacaciones" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script src="../../javascript/Js_Vacaciones_Empleados.js" type="text/javascript">    
    </script>
       <!--SCRIPT PARA LA VALIDACION QUE NO EXPERE LA SESSION-->  
   <script language="javascript" type="text/javascript">
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
                            <td class="label_titulo">Empleado Solicitud de Vacaciones</td>
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
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                              </td>
                                              <td align="right" style="width:41%;">                                  
                                               </td>       
                                             </tr>         
                                          </table>                      
                                        </div>
                                 </td>
                             </tr>
                    </table>                      

                <br />
                <asp:Panel ID="Pnl_Busqueda_Dias_Vacaciones" runat="server" Width="97%" 
                    style="background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;color:White;">               
                    <table width="98%">
                        <tr>
                            <td  style="text-align:left;width:50%; color:Black;">
                                <table style="width:100%;">
                                    <tr>
                                        <td style="width:40%;">
                                            <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="98%" MaxLength="10" Enabled="false"/>
                                            <cc1:TextBoxWatermarkExtender ID="TBW_Txt_Empleados" runat="server" TargetControlID="Txt_No_Empleado"
                                                WatermarkCssClass="watermarked2" WatermarkText="No Empleado"/>
                                            <cc1:FilteredTextBoxExtender ID="FTxt_No_Empleado" 
                                                 runat="server" TargetControlID="Txt_No_Empleado" FilterType="Numbers"/>      
                                        </td>
                                        <td style="width:60%; vertical-align:middle; text-align:left;">
                                            <asp:ImageButton ID="Btn_Consultar_Dias_Vacaciones" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                    ToolTip="Seleccione la Fecha Final" OnClick="Btn_Consultar_Dias_Vacaciones_Click"/>     
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
                        <tr style="display:none">
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
                                <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="100%"/>
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
                                <asp:TextBox ID="Txt_Dias_Vacaciones_Empleado" runat="server" Width="98%"  MaxLength="10" onKeyDown="if(event.keyCode==13){event.keyCode=9; return event.keyCode;}"
                                    OnTextChanged="Txt_Dias_Vacaciones_Empleado_TextChanged" AutoPostBack="true"/>
                                <cc1:FilteredTextBoxExtender ID="FTxt_Dias_Vacaciones_Empleado" 
                                     runat="server" TargetControlID="Txt_Dias_Vacaciones_Empleado" FilterType="Numbers"/>                                                 
                            </td>
                            <td style="text-align:left;width:20%;">
                                &nbsp;&nbsp;Estatus                 
                            </td>
                            <td  style="text-align:left;width:30%;">                            
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                    <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                    <asp:ListItem>Pendiente</asp:ListItem>
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
            </div>          
        </ContentTemplate>
    </asp:UpdatePanel>  
</asp:Content>


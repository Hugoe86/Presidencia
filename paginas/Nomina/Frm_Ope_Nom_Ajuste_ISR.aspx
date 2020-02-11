<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Ajuste_ISR.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Ajuste_ISR" Title="Ajuste de ISR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
            document.getElementById("<%=Txt_Busqueda_No_Ajuste_ISR.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_Empleado.ClientID%>").value=""; 
            document.getElementById("<%=Cmb_Busqueda_Estatus.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_RFC.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_Fecha_Inicio.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_Fecha_Fin.ClientID%>").value="";
            return false;
        }
 </script>
 
 <script type="text/javascript" language="javascript">
    function pageLoad(sender, args) {
        $('textarea[id*=Txt_Comen]').keyup(function() {var Caracteres =  $(this).val().length;if (Caracteres > 250) {this.value = this.value.substring(0, 250);$(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});
        
        $('input[id$=Txt_No_Empleado]').live("blur", function() {
            if (isNumber($(this).val())) {
                var Ceros = "";
                if ($(this).val() != undefined) {
                    if ($(this).val() != '') {
                        for (i = 0; i < (6 - $(this).val().length); i++) {
                            Ceros += '0';
                        }
                        $(this).val(Ceros + $(this).val());
                        Ceros = "";
                    } else $(this).val('');
                }
            }
        }); 
        
        $('input[id$=Txt_Busqueda_Empleado]').live("blur", function() {
            if (isNumber($(this).val())) {
                var Ceros = "";
                if ($(this).val() != undefined) {
                    if ($(this).val() != '') {
                        for (i = 0; i < (6 - $(this).val().length); i++) {
                            Ceros += '0';
                        }
                        $(this).val(Ceros + $(this).val());
                        Ceros = "";
                    } else $(this).val('');
                }
            }
        });         
        
        Contar_Caracteres();
    }
    
    function Contar_Caracteres(){
        $('input[id$=Txt_No_Empleado]').keyup(function() {
            var Caracteres =  $(this).val().length;
            
            if (Caracteres > 6) {
                this.value = this.value.substring(0, 6);
                $(this).css("background-color", "Yellow");
                $(this).css("color", "Red");
            }else{
                $(this).css("background-color", "White");
                $(this).css("color", "Black");
            }
        });
    }
    function isNumber(n) {   return !isNaN(parseFloat(n)) && isFinite(n); }        
</script>

    <script src="../../easyui/jquery.formatCurrency-1.4.0.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency.all.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Ajuste_ISR" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <asp:Button ID="Btn_Comodin_Perder_Foco" runat="server" style="background-color:Transparent;border-style:none;" OnClientClick="javascript:return false;"/>
            
            <div id="Div_Ajuste_ISR" style="background-color:#ffffff; width:99%; height:100%;">
            
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Ajustes de ISR</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false"  />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                        </td>
                    </tr>
                </table>                                                             
                
                <table width="100%"  border="0" cellspacing="0">
                         <tr align="center">
                             <td colspan="2">                
                                 <div align="right" class="barra_busqueda" >                        
                                      <table style="width:100%;height:28px;">
                                        <tr>
                                          <td align="left" style="width:59%;">                                                  
                                               <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" 
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click"/>
                                                <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" 
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"  OnClick="Btn_Modificar_Click"/>
                                                <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" 
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" OnClick="Btn_Eliminar_Click"
                                                    OnClientClick="return confirm('¿Está Seguro de Eliminar el Ajuste ISR Seleccionado?');"/>
                                                <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" 
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click"
                                                    CausesValidation="false"/>
                                          </td>
                                          <td align="right" style="width:41%;">
                                            <table style="width:100%;height:28px;">
                                                <tr>
                                                    <td style="width:60%;vertical-align:top;">
                                                        B&uacute;squeda
                                                        <asp:ImageButton ID="Btn_Busqueda_Avanzada" runat="server" ToolTip="Avanzada"  
                                                            ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px" Width="24px"
                                                            OnClick="Btn_Busqueda_Avanzada_Click" CausesValidation="false"/>                                        
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
                        
                        
                <asp:Panel ID="Pnl_Empleado" runat="server" GroupingText="Datos Empleado">
                    <table width="98%">
                        <tr style="display:none">
                            <td style="width:20%;text-align:left;">
                                No Ajuste ISR
                            </td>
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_Ajuste_ISR" runat="server" Width="80%" Enabled="false"/>
                            </td>    
                            <td style="width:20%;text-align:left;">
                            </td>
                            <td style="width:30%;text-align:left;">                                                                                                                         
                            </td>                                                  
                        </tr>
                        <tr>
                            <td style="width:20%;text-align:left;">
                                *No Empleado
                            </td>  
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="80%"  MaxLength="10"
                                    OnTextChanged="Txt_No_Empleado_TextChanged" AutoPostBack="true"/>
                                <asp:ImageButton ID="Btn_Buscar_Empleado" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                    onclick="Btn_Buscar_Empleado_Click" CausesValidation="false"
                                    ToolTip="Buscar Empleado a Ajustar el ISR"/>                                
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Empleado" runat="server" FilterType="Numbers"
                                    TargetControlID="Txt_No_Empleado"/> 
                                <cc1:TextBoxWatermarkExtender ID="Twm_Txt_No_Empleado" runat="server" 
                                    TargetControlID ="Txt_No_Empleado" 
                                    WatermarkText="No Empleado" WatermarkCssClass="watermarked"/>                                                                          
                            </td>                            
                            <td style="width:20%;text-align:left;">
                                Clase Nomina
                            </td>
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_Clase_Nomina_Empleado" runat="server" Width="97.5%" Enabled="false"/>
                            </td>                                                                                                         
                        </tr>
                        <tr>                          
                            <td style="width:20%;text-align:left;">
                                Nombre Empleado
                            </td>
                            <td style="width:80%;text-align:left;" colspan="3">
                                <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" Width="99.5%" Enabled="false"/>
                            </td>                                         
                        </tr>                        
                    </table>
                </asp:Panel>      
                        
                <asp:Panel ID="Pnl_Datos_Empleado" runat="server" GroupingText="Datos Personales Empleado">
                    <table width="100%">
                        <tr>
                            <td style="width:10%;vertical-align:top ;" align="center">
                                <asp:Image ID="Img_Foto_Empleado_Solicitante" runat="server"  Width="70px" Height="85px"
                                    ImageUrl="~/paginas/imagenes/paginas/Sias_No_Disponible.JPG" BorderWidth="1px" />
                            </td>
                            <td style="width:90%;">                
                                <table width="100%">
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            RFC
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_RFC_Empleado" runat="server" Width="98%" Enabled="false"/>
                                        </td>   
                                        <td style="width:20%;text-align:left;">
                                            Fecha Ingreso
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Fecha_Ingreso_Empleado" runat="server" Width="98%" Enabled="false"/>
                                        </td>                                                      
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            Sindicato
                                        </td>
                                        <td style="width:80%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Sindicato_Empleado" runat="server" Width="99.5%" Enabled="false"/>
                                        </td>                                                                   
                                    </tr>   
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            U. Responsable
                                        </td>
                                        <td style="width:80%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Dependencia_Empelado" runat="server" Width="99.5%" Enabled="false"/>
                                        </td>                                                                   
                                    </tr>       
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            Direcci&oacute;n Empleado
                                        </td>
                                        <td style="width:80%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Direccion_Empleado" runat="server" Width="99.5%" Enabled="false"/>
                                        </td>                                 
                                    </tr>     
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                           Cuenta Bancaria
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Cuenta_Bancaria_Empleado" runat="server" Width="98%" Enabled="false"/>
                                        </td>   
                                        <td style="width:20%;text-align:left;">
                                            Sueldo Mensual
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Sueldo_Mensual_Empleado" runat="server" Width="98%" Enabled="false"/>
                                        </td>                                                      
                                    </tr>                                                          
                                </table>
                            </td>                        
                        </tr>
                    </table>                        
                </asp:Panel>                                                          
                        
                <asp:Panel ID="Pnl_Datos_Ajuste_ISR" runat="server" GroupingText="Datos Ajuste ISR">
                    <table style="width:100%;">
                        <tr>
                            <td style="width:20%;text-align:left;">
                                *Fecha Solicitud
                            </td>
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_Fecha_Alta_Ajuste_ISR" runat="server" Width="85%" MaxLength="10"/>  
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Fecha_Alta_Ajuste_ISR" 
                                    runat="server" TargetControlID="Txt_Fecha_Alta_Ajuste_ISR" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/"/>
                                <cc1:CalendarExtender ID="Txt_Fecha_Alta_Ajuste_ISR_CalendarExtender" runat="server"
                                    TargetControlID="Txt_Fecha_Alta_Ajuste_ISR" PopupButtonID="Btn_Fecha_Ajuste_ISR" Format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Fecha_Ajuste_ISR" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha" CausesValidation="false"/>                          
                            </td>  
                            <td style="width:20%;text-align:left;">
                                *Estatus
                            </td>
                            <td style="width:30%;text-align:left;">
                                <asp:DropDownList ID="Cmb_Estatus_Ajuste_ISR" runat="server" Width="100%">
                                    <asp:ListItem>&lt;-- Seleccione --&gt;</asp:ListItem>
                                    <asp:ListItem>Pendiente</asp:ListItem>
                                    <asp:ListItem>Proceso</asp:ListItem>
                                    <asp:ListItem>Pagado</asp:ListItem>                                    
                                </asp:DropDownList>                               
                            </td>                                                                                                                          
                        </tr>
                        <tr>
                            <td style="width:100%;" colspan="4" align="center">
                                <div id="Div_Periodo-Pago" style="background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;">
                                    <table width="100%">
                                        <tr>
                                            <td style="width:20%;text-align:left;">
                                                *Nomina
                                            </td>
                                            <td style="width:30%;text-align:left;">
                                                <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" AutoPostBack="true" 
                                                   onselectedindexchanged="Cmb_Calendario_Nomina_SelectedIndexChanged"/>
                                            </td>             
                                            <td style="width:20%;text-align:left;">
                                                *Periodo
                                            </td>
                                            <td style="width:30%;text-align:left;">
                                                <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" 
                                                    Width="100%" AutoPostBack="true" 
                                                    onselectedindexchanged="Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged"/>
                                            </td>                                                                                        
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:20%;text-align:left;">
                                *Fecha Inicio Pago
                            </td>
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_Fecha_Inicio_Ajuste_ISR" runat="server" Width="98%" Enabled="false"/>                                
                            </td>     
                            <td style="width:20%;text-align:left;">
                                *Fecha Termino Pago
                            </td>              
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_Fecha_Termino_Ajuste_ISR" runat="server" Width="98%"  Enabled="false"/> 
                            </td>                                                                                             
                        </tr>                        
                        <tr>
                            <td style="width:20%;text-align:left;vertical-align:top;">
                                *Comentarios Ajuste ISR
                            </td>
                            <td style="width:80%;text-align:left;" colspan="3">
                                <asp:TextBox ID="Txt_Comentarios_Ajuste_ISR" runat="server" Width="660px" TextMode="MultiLine"
                                    Height="60px" Wrap="true" MaxLength="100" />  
                                <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Comentarios_Ajuste_ISR" runat="server"  TargetControlID="Txt_Comentarios_Ajuste_ISR"
                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>    
                                <cc1:TextBoxWatermarkExtender ID="Twe_Fte_Txt_Comentarios_Ajuste_ISR" runat="server" TargetControlID ="Txt_Comentarios_Ajuste_ISR" 
                                    WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/>                                                                  
                            </td>                                                                                               
                        </tr> 
                        <tr>
                            <td style="width:100%;" colspan="4" align="center">
                                <div id="Div_Total_Prestamo" style="background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;width:40%;">
                                    <table width="98%">
                                        <tr>
                                            <td style="width:50%;text-align:left;">
                                                Total ISR Ajustar
                                            </td>
                                            <td style="width:50%;text-align:left;">
                                                <asp:TextBox ID="Txt_Total_ISR_Ajustar" runat="server" Width="98%" MaxLength="10"
                                                    ontextchanged="Txt_Total_ISR_Ajustar_TextChanged" AutoPostBack="true"
                                                    onblur="$('input[id$=Txt_Total_ISR_Ajustar]').formatCurrency({colorize:true, region: 'es-MX'});"
                                                    style="text-align:right;"/>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Total_ISR_Ajustar" runat="server" 
                                                    TargetControlID="Txt_Total_ISR_Ajustar" FilterType="Custom, Numbers" ValidChars="-,."/>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>                        
                        <tr>
                            <td style="width:20%;text-align:left;">
                                *No Catorcenas
                            </td>
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_No_Catorcenas" runat="server" Width="98%" MaxLength="10"
                                    OnTextChanged="Txt_No_Catorcenas_TextChanged" AutoPostBack="true"/>    
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Catorcenas" runat="server" FilterType="Custom, Numbers"
                                    TargetControlID="Txt_No_Catorcenas" ValidChars="."/>                           
                            </td>     
                            <td style="width:20%;text-align:left;">
                                *Pago Catorcenal
                            </td>              
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_Pago_Catorcenal_ISR" runat="server" Width="98%" Enabled="false"/>  
                            </td> 
                        </tr>    
                        <tr>
                            <td style="width:20%;text-align:left;">
                                *No Pago 
                            </td>
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_No_Pago" runat="server" Width="98%" Enabled="false"/>                                
                            </td>     
                            <td style="width:20%;text-align:left;">
                                *Total ISR Ajustado
                            </td>              
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_Total_ISR_Ajustado" runat="server" Width="98%" Enabled="false"/> 
                            </td> 
                        </tr>       
                        <tr>   
                            <td style="width:20%;text-align:left;">
                                *Concepto Percepci&oacute;n
                            </td>              
                            <td style="width:80%;text-align:left;" colspan="3">
                                <asp:DropDownList ID="Cmb_Percepcion" runat="server" Width="100%"/>
                            </td> 
                        </tr>                                                                                                                                                  
                    </table>
                </asp:Panel>       
                <br /><br /><br />                              
            </div>         
        </ContentTemplate>
    </asp:UpdatePanel>   

<asp:UpdatePanel ID="Udp_Modal_Popup" runat="server" UpdateMode="Conditional">
   <ContentTemplate>    
        <cc1:ModalPopupExtender ID="Mpe_Busqueda_Ajuste_ISR" runat="server" BackgroundCssClass="popUpStyle" 
            PopupControlID="Pnl_Busqueda_Contenedor" TargetControlID="Btn_Comodin_Open" PopupDragHandleControlID="Pnl_Busqueda_Cabecera" 
            CancelControlID="Btn_Comodin_Close" DropShadow="True" DynamicServicePath="" Enabled="True" />  
        <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Close" runat="server" Text="" />
        <asp:Button  Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Open" runat="server" Text="" />  
    </ContentTemplate>          
</asp:UpdatePanel> 

<asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="650px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" 
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Img_Informatcion_Autorizacion" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     B&uacute;squeda: Ajustes ISR
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana_Autorizacion" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClick="Btn_Cerrar_Ventana_Busqueda_Click"/>  
                </td>
            </tr>
        </table>            
    </asp:Panel>                                                                          
           <div style="color: #5D7B9D">
             <table width="100%">
                <tr>
                    <td align="left" style="text-align: left;" >                                    
                        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Prestamos" runat="server" >
                            <ContentTemplate>
                            
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server" AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Prestamos" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter" class="progressTemplateInner"></div>
                                        <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress"><img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
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
                                            No Solicitud
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Busqueda_No_Ajuste_ISR" runat="server" Width="98%" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Ajuste_ISR" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Busqueda_No_Ajuste_ISR"/>                                             
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Estatus
                                        </td>              
                                        <td style="width:30%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="100%"> 
                                                <asp:ListItem Value="">&lt; -- Seleccione -- &gt;</asp:ListItem>                                               
                                                <asp:ListItem>Pendiente</asp:ListItem>
                                                <asp:ListItem>Proceso</asp:ListItem>
                                                <asp:ListItem>Pagado</asp:ListItem>                                    
                                            </asp:DropDownList>                                          
                                        </td>                                         
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            No Empleado
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Busqueda_Empleado" runat="server" Width="98%" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Empleado" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Busqueda_Empleado"/>                                            
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;"> 
                                            RFC Empleado                                   
                                        </td>              
                                        <td style="width:30%;text-align:left;">    
                                            <asp:TextBox ID="Txt_Busqueda_RFC" runat="server" Width="98%" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_RFC" runat="server" FilterType="Numbers, UppercaseLetters"
                                                TargetControlID="Txt_Busqueda_RFC"/>                                  
                                        </td>                                         
                                    </tr>  
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Fecha Inicio
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                                <asp:TextBox ID="Txt_Busqueda_Fecha_Inicio" runat="server" Width="85%" MaxLength="1"/>
                                                <cc1:FilteredTextBoxExtender ID="FTxt_Busqueda_Fecha_Inicio" 
                                                    runat="server" TargetControlID="Txt_Busqueda_Fecha_Inicio" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                                    ValidChars="/_"/>
                                                <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Inicio" runat="server" 
                                                    TargetControlID="Txt_Busqueda_Fecha_Inicio" PopupButtonID="Btn_Busqueda_Fecha_Inicio" Format="dd/MMM/yyyy" 
                                                    OnClientShown="calendarShown"/>
                                                <asp:ImageButton ID="Btn_Busqueda_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                                    ToolTip="Seleccione la Fecha"/> 
                                                <cc1:MaskedEditExtender 
                                                    ID="Mee_Txt_Busqueda_Fecha_Inicio" 
                                                    Mask="99/LLL/9999" 
                                                    runat="server"
                                                    MaskType="None" 
                                                    UserDateFormat="DayMonthYear" 
                                                    UserTimeFormat="None" Filtered="/"
                                                    TargetControlID="Txt_Busqueda_Fecha_Inicio" 
                                                    Enabled="True" 
                                                    ClearMaskOnLostFocus="false"/>  
                                                <cc1:MaskedEditValidator 
                                                    ID="Mev_Txt_Busqueda_Fecha_Inicio" 
                                                    runat="server" 
                                                    ControlToValidate="Txt_Busqueda_Fecha_Inicio"
                                                    ControlExtender="Mee_Txt_Busqueda_Fecha_Inicio" 
                                                    EmptyValueMessage="Es valido no ingresar fecha inicial"
                                                    InvalidValueMessage="Fecha Inicial Invalida" 
                                                    IsValidEmpty="true" 
                                                    TooltipMessage="Ingrese o Seleccione la Fecha Inicial"
                                                    Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                                                                               
                                        </td>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Fecha Fin
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                                <asp:TextBox ID="Txt_Busqueda_Fecha_Fin" runat="server" Width="85%" MaxLength="1"/>
                                                <cc1:FilteredTextBoxExtender ID="FTxt_Busqueda_Fecha_Fin" 
                                                    runat="server" TargetControlID="Txt_Busqueda_Fecha_Fin" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                                                    ValidChars="/_"/>                                                    
                                                <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Fin" runat="server" 
                                                    TargetControlID="Txt_Busqueda_Fecha_Fin" PopupButtonID="Btn_Busqueda_Fecha_Fin" Format="dd/MMM/yyyy" 
                                                    OnClientShown="calendarShown"/>
                                                <asp:ImageButton ID="Btn_Busqueda_Fecha_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                                    ToolTip="Seleccione la Fecha"/> 
                                                <cc1:MaskedEditExtender 
                                                    ID="Mee_Txt_Busqueda_Fecha_Fin" 
                                                    Mask="99/LLL/9999" 
                                                    runat="server"
                                                    MaskType="None" 
                                                    UserDateFormat="DayMonthYear" 
                                                    UserTimeFormat="None" Filtered="/"
                                                    TargetControlID="Txt_Busqueda_Fecha_Fin" 
                                                    Enabled="True" 
                                                    ClearMaskOnLostFocus="false"/>  
                                                <cc1:MaskedEditValidator 
                                                    ID="Mev_Mee_Txt_Busqueda_Fecha_Fin" 
                                                    runat="server" 
                                                    ControlToValidate="Txt_Busqueda_Fecha_Fin"
                                                    ControlExtender="Mee_Txt_Busqueda_Fecha_Fin" 
                                                    EmptyValueMessage="Es valido no ingresar fecha final"
                                                    InvalidValueMessage="Fecha Final Invalida" 
                                                    IsValidEmpty="true" 
                                                    TooltipMessage="Ingrese o Seleccione la Fecha Final"
                                                    Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                        
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
                                               <asp:Button ID="Btn_Buscar_Ajustes_ISR" runat="server"  Text="Buscar" CssClass="button"  CausesValidation="false"  
                                                    OnClick="Btn_Buscar_Ajustes_ISR_Click" Width="60px" /> 
                                            </center>
                                        </td>                                                     
                                    </tr>                                                                        
                                  </table>                                                                                                                                                              
                            </ContentTemplate>                                        
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="Upnl_Grid_Ajuste_ISR" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                           <asp:GridView ID="Grid_Ajuste_ISR" runat="server" AutoGenerateColumns="False" CellPadding="4" Width="100%"                                             
                                AllowPaging ="True" PageSize="5" ForeColor="#333333" GridLines="None" CssClass="GridView_1"
                                OnSelectedIndexChanged="Grid_Ajuste_ISR_OnSelectedIndexChanged"
                                OnRowDataBound="Grid_Ajuste_ISR_RowDataBound"
                                OnPageIndexChanging="Grid_Ajuste_ISR_PageIndexChanging">                                
                                       <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" HeaderText="Seleccione"
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%"/>
                                                </asp:ButtonField>    
                                                <asp:BoundField DataField="NO_AJUSTE_ISR" HeaderText="No Ajuste ISR" Visible="True" >
                                                        <ItemStyle Width="20%"  HorizontalAlign="Left"/>
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%"/>                                                    
                                                </asp:BoundField>
                                                <asp:BoundField DataField="NOMBRE" HeaderText="Empleado" Visible="True" >
                                                        <ItemStyle Width="40%"  HorizontalAlign="Left"/>
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%"/>                                                    
                                                </asp:BoundField>                                                  
                                                <asp:BoundField DataField="ESTATUS_AJUSTE_ISR" HeaderText="Estatus" Visible="True">
                                                        <ItemStyle Width="20%"  HorizontalAlign="Left"/>
                                                        <HeaderStyle HorizontalAlign="Left" Width="45%"/>                                                    
                                                </asp:BoundField>                                                                                                                                                            
                                       </Columns>
                                      <SelectedRowStyle CssClass="GridSelected" />
                                      <PagerStyle CssClass="GridHeader" />
                                      <HeaderStyle CssClass="GridHeader" />
                                      <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>                            
                            </ContentTemplate>
                            <Triggers >
                                <asp:AsyncPostBackTrigger ControlID="Btn_Buscar_Ajustes_ISR"  EventName="Click"/>
                            </Triggers>                                        
                        </asp:UpdatePanel>                                    
                    </td>
                </tr>
             </table>                                                   
           </div>                 
    </asp:Panel>     
</asp:Content>


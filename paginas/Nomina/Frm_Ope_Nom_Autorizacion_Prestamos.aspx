<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Autorizacion_Prestamos.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Autorizacion_Prestamos" Title="Autorización Prestamos Internos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">

    <script src="../../javascript/Js_Ope_Nom_Prestamos_Internos.js" type="text/javascript"></script>

    <script src="../../easyui/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency-1.4.0.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency.all.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
 
        //Metodo para mantener los calendarios en una capa mas alat.
         function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
        //Metodos para limpiar los controles de la busqueda.
        function Limpiar_Ctlr(){
            document.getElementById("<%=Txt_Busqueda_No_Solicitud.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_Empleado_Solicitante.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_Empleado_Aval.ClientID%>").value="";            
            document.getElementById("<%=Txt_Busqueda_RFC_Solicita.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_RFC_Aval.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_Fecha_Inicio.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_Fecha_Fin.ClientID%>").value="";                        
            return false;
        }                
       //Metodos para la autorizacion del prestamo.
       function Autorizar(){
           if( document.getElementById("<%=Chk_Autorizar.ClientID %>").checked){
                document.getElementById("<%=Txt_Autorizacion_Comentarios.ClientID %>").disabled = true;
           }else{
                document.getElementById("<%=Txt_Autorizacion_Comentarios.ClientID %>").disabled = false;
           }
        }        
        function Abrir_Modal_Popup() {
            $find('MPE_Autoarizacion').show();
            document.getElementById("<%=Txt_Autorizacion_No_Empleado.ClientID %>").value = document.getElementById("<%=Txt_No_Empleado_Solicitante_Prestamo.ClientID %>").value;
            document.getElementById("<%=Txt_Autorizacion_Nombre_Empleado.ClientID %>").value = document.getElementById("<%=Txt_Nombre_Empleado_Solicitante.ClientID %>").value;
            return false;
        }              
        //Metodos para la operacion de cancelacion de prestamo.
        function Cancelar(){
           if( document.getElementById("<%=Chk_Cancelar_Prestamo.ClientID %>").checked){
                document.getElementById("<%=Txt_Cancelacion_Referencia_Pago.ClientID %>").disabled = false;
                document.getElementById("<%=Txt_Cancelacion_Comentarios.ClientID %>").disabled = false;  
                document.getElementById("<%=Btn_Cancelar_Prestamo.ClientID %>").disabled=false;              
           }else{
                document.getElementById("<%=Txt_Cancelacion_Referencia_Pago.ClientID %>").disabled = true;
                document.getElementById("<%=Txt_Cancelacion_Comentarios.ClientID %>").disabled = true;
                document.getElementById("<%=Btn_Cancelar_Prestamo.ClientID %>").disabled=true;   
           }
        }                       
        function Abrir_Modal_Popup_Cancelar() {
            $find('MPE_Cancelacion').show();
            document.getElementById("<%=Txt_Cancelacion_No_Empleado.ClientID %>").value = document.getElementById("<%=Txt_No_Empleado_Solicitante_Prestamo.ClientID %>").value;
            document.getElementById("<%=Txt_Cancelacion_Nombre_Empleado.ClientID %>").value = document.getElementById("<%=Txt_Nombre_Empleado_Solicitante.ClientID %>").value;
            return false;
        }
        
    function pageLoad(sender, args) {
        $('textarea[id*=Txt_Finalidad]').keyup(function() {var Caracteres =  $(this).val().length; if (Caracteres > 250) {this.value = this.value.substring(0, 250); Caracteres =  $(this).val().length;  $(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});
        
        Formato_No_Empleado_Solicita();
        Contar_Caracteres_Solicita();
        
        Formato_No_Empleado_Aval();
        Contar_Caracteres_Aval();
        
          $('input[id$=Txt_Busqueda_Empleado_Solicitante]').live("blur", function(){
                if(isNumber($(this).val())){
                    var Ceros = "";
                    if($(this).val() != undefined){
                        if($(this).val() != ''){
                            for(i=0; i<(6-$(this).val().length); i++){
                                Ceros += '0';
                            }
                            $(this).val(Ceros + $(this).val());
                            Ceros = "";
                        }else $(this).val('');
                    }
                }
            });      
          $('input[id$=Txt_Busqueda_Empleado_Aval]').live("blur", function(){
                if(isNumber($(this).val())){
                    var Ceros = "";
                    if($(this).val() != undefined){
                        if($(this).val() != ''){
                            for(i=0; i<(6-$(this).val().length); i++){
                                Ceros += '0';
                            }
                            $(this).val(Ceros + $(this).val());
                            Ceros = "";
                        }else $(this).val('');
                    }
                }
            }); 
    }
    
    function Formato_No_Empleado_Solicita(){
        $('input[id$=Txt_No_Empleado_Solicitante_Prestamo]').bind("blur", function(){
            var Ceros = "";
            if($(this).val() != undefined){
                for(i=0;    i<(6-$(this).val().length);    i++){
                    Ceros += '0';
                }
                $(this).val(Ceros + $(this).val());
                Ceros = "";
            }
        });
    }
    
    function Formato_No_Empleado_Aval(){
        $('input[id$=Txt_No_Empleado_Aval]').bind("blur", function(){
            var Ceros = "";
            if($(this).val() != undefined){
                for(i=0;    i<(6-$(this).val().length);    i++){
                    Ceros += '0';
                }
                $(this).val(Ceros + $(this).val());
                Ceros = "";
            }
        });
    }
    
    function Contar_Caracteres_Solicita(){
        $('input[id$=Txt_No_Empleado_Solicitante_Prestamo]').keyup(function() {
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
    
    function Contar_Caracteres_Aval(){
        $('input[id$=Txt_No_Empleado_Aval]').keyup(function() {
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
    <cc1:ToolkitScriptManager ID="Tsm_Solicitud_Prestamo" runat="server"  AsyncPostBackTimeout="36000"  EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <asp:Button ID="Btn_Comodin_Perder_Foco" runat="server" style="background-color:Transparent;border-style:none;" OnClientClick="javascript:return false;"/>
            
            <div id="Div_Solicitud_Prestamos" style="background-color:#ffffff; width:99%; height:100%;">
            
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Autorizaci&oacute;n de Solicitudes de Prestamos</td>
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
                                               <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="19" 
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click"/>
                                                <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="20"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"  OnClick="Btn_Modificar_Click"/>
                                                <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" TabIndex="21"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" OnClick="Btn_Eliminar_Click"
                                                    OnClientClick="return confirm('¿Está Seguro de Eliminar el Prestamo Seleccionado?');"/>
                                                <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="22"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click"
                                                    CausesValidation="false"/>
                                          </td>
                                          <td align="right" style="width:41%;">
                                            <table style="width:100%;height:28px;">
                                                <tr>
                                                    <td style="width:60%;vertical-align:top;">
                                                        B&uacute;squeda
                                                        <asp:ImageButton ID="Btn_Busqueda_Prestamos" runat="server" ToolTip="Avanzada" TabIndex="23" 
                                                            ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px" Width="24px"
                                                            OnClick="Btn_Busqueda_Prestamos_Click" CausesValidation="false"/>                                        
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
                        
                        
                <asp:Panel ID="Pnl_Buscar_Empleado_Solicitante" runat="server" GroupingText="Solicitud Prestamo">
                    <table width="98%">
                        <tr>
                            <td style="width:20%;text-align:left;">
                                No Solicitud
                            </td>
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_No_Solocitud" runat="server" Width="80%" Enabled="false"/>
                            </td>    
                            <td style="width:20%;text-align:left; cursor:auto;border-style:outset;vertical-align:middle;height:15px;" align="center">
                                <center>
                                    <asp:CheckBox ID="Chk_Omitir_Validaciones" runat="server" Text="Validar" Width="100%" TextAlign="Left" 
                                        ToolTip="Indica si aplicaran las validaciones para generar un prestamo o si las mismas se omitiran."/> &nbsp;&nbsp;&nbsp;
                                </center>    
                            </td>
                            <td style="width:30%;text-align:left;">
                                <asp:Button ID="Btn_Autorizacion_Prestamos" runat="server" Text="Autorizacion de Prestamos" Visible="false" 
                                   OnClientClick="javascript:return Abrir_Modal_Popup();" CssClass="button_autorizar" ToolTip="Modificar"
                                   style="border-style:outset;border-color:White;background-color:#F0F8FF;color:Black;font-weight:bold;cursor:hand;"/> 
                                   
                                <asp:Button ID="Btn_Cancelacion_Prestamo" runat="server" Text="Cancelacion de Prestamo" Visible="false" 
                                   OnClientClick="javascript:return Abrir_Modal_Popup_Cancelar();" CssClass="button_autorizar" ToolTip="Modificar"
                                   style="border-style:outset;border-color:White;background-color:#F0F8FF;color:Black;font-weight:bold;cursor:hand;"/>                                                                                                                          
                            </td>                                                  
                        </tr>                                             
                    </table>
                    
                    <div id="Div_Comentarios" style="background-color:White;color:White; font-size:12;font-weight:bold;border-style:none;" runat="server">
                        <table width="98%">
                            <tr>
                                <td style="width:20%;text-align:left;vertical-align:top;color:Black;">
                                    <asp:Label ID="Lbl_Motivo_Cancelacion_Rechazo_Prestamo" runat="server" Text=""/> 
                                </td>
                                <td style="width:80%;text-align:left;">
                                    <asp:TextBox ID="Txt_Motivo_Cancelacion_Rechazo" runat="server" Width="99.5%" Enabled="false"
                                        Height="40px" Wrap="true" TextMode="MultiLine"/>  
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"  TargetControlID="Txt_Motivo_Cancelacion_Rechazo"
                                        FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>    
                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID ="Txt_Motivo_Cancelacion_Rechazo" 
                                        WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/>  
                                </td>                                                    
                            </tr>                                              
                        </table>                            
                    </div>                    
                </asp:Panel>      
                        
                <asp:Panel ID="Pnl_Datos_Empleado_Solicitante" runat="server" GroupingText="Empleado Solicitante">
                    <div id="Div1" style="background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;">
                        <table width="100%">
                            <tr>
                                <td style="width:5%;text-align:left;">
                                   *Solicita
                                </td>
                                <td style="width:20%;text-align:left;">
                                    <asp:TextBox ID="Txt_No_Empleado_Solicitante_Prestamo" runat="server" Width="70%" MaxLength="10" TabIndex="0"
                                        AutoPostBack="true" OnTextChanged="Txt_No_Empleado_Solicitante_Prestamo_TextChanged" Enabled="false"/>
                                    <asp:ImageButton ID="Btn_Buscar_Empleado_Solicitante" runat="server" 
                                        TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                        onclick="Btn_Buscar_Empleado_Solicitante_Click" CausesValidation="false"
                                        ToolTip="Buscar Empleado que Solicita el Prestamo"/>
                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Empleado_Solicitante_Prestamo" runat="server" FilterType="Numbers"
                                        TargetControlID="Txt_No_Empleado_Solicitante_Prestamo"/> 
                                    <cc1:TextBoxWatermarkExtender ID="Twm_Txt_No_Empleado_Solicitante_Prestamo" runat="server" 
                                        TargetControlID ="Txt_No_Empleado_Solicitante_Prestamo" 
                                        WatermarkText="No Empleado" WatermarkCssClass="watermarked"/>                                                                             
                                </td>                            
                                <td style="width:10%;text-align:left;">
                                    Nombre
                                </td>
                                <td style="width:65%;text-align:left;">
                                    <asp:TextBox ID="Txt_Nombre_Empleado_Solicitante" runat="server" Width="98%" Enabled="false"/>
                                </td>                                         
                            </tr>                        
                        </table>
                    </div>                 
                
                
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
                                            Clase Nomina
                                        </td>  
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Clase_Nomina_Empleado" runat="server" Width="97.5%" Enabled="false"/>                          
                                        </td>                               
                                        <td style="width:20%;text-align:left;">
                                            RFC
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_RFC_Empleado_Solicitante" runat="server" Width="98%" Enabled="false"/>
                                        </td>                                                                                                         
                                    </tr>                                
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            Sindicato
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Sindicato_Empleado_Solicitante" runat="server" Width="99.5%" Enabled="false"/>
                                        </td>                                                                                           
                                    </tr>                                    
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            U. Responsable
                                        </td>
                                        <td style="width:80%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Dependencia_Empelado_Solicitante" runat="server" Width="99.5%" Enabled="false"/>
                                        </td>                                                                   
                                    </tr>       
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            Direcci&oacute;n Empleado
                                        </td>
                                        <td style="width:80%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Direccion_Empleado_Solicitante" runat="server" Width="99.5%" Enabled="false"/>
                                        </td>                                 
                                    </tr> 
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                           Cuenta Bancaria
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Cuenta_Bancaria_Empleado_Solicitante" runat="server" Width="98%" Enabled="false"/>
                                        </td>   
                                        <td style="width:20%;text-align:left;">
                                            Banco
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Banco_Empleado_Solicitante" runat="server" Width="98%" Enabled="false"/>
                                        </td>                                                     
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            Fecha Ingreso
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Fecha_Ingreso_Empleado_Solicitante" runat="server" Width="98%" Enabled="false"/>
                                        </td>
                                        <td style="width:20%;text-align:left;">
                                            Sueldo Mensual
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Sueldo_Mensual_Empleado_Solicitante" runat="server" Width="98%" Enabled="false" CssClass="text_cantidades_grid"/>
                                        </td>                                                                  
                                    </tr>                                                          
                                </table>
                            </td>                        
                        </tr>
                    </table>                        
                </asp:Panel>          
                                                
                <asp:Panel ID="Pnl_Datos_Empleado_Aval" runat="server" GroupingText="Empleado Aval">
                
                    <div id="Div_Buscar_Empleado_Aval" style="background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;">
                        <table width="100%">
                            <tr>
                                <td style="width:5%;text-align:left;">
                                    *Aval
                                </td>
                                <td style="width:20%;text-align:left;">
                                    <asp:TextBox ID="Txt_No_Empleado_Aval" runat="server" Width="70%" TabIndex="2" MaxLength="10"
                                        AutoPostBack="true" OnTextChanged="Txt_No_Empleado_Aval_TextChanged"/>
                                    <asp:ImageButton ID="Btn_Buscar_Empleado_Aval" runat="server" Text="Buscar" TabIndex="3"
                                         ImageUrl="~/paginas/imagenes/paginas/busqueda.png" CausesValidation="false"
                                         OnClick="Btn_Buscar_Empleado_Aval_Click" ToolTip="Buscar Empleado Aval del Prestamo"/>
                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Empleado_Aval" runat="server" FilterType="Numbers"
                                        TargetControlID="Txt_No_Empleado_Aval"/>     
                                    <cc1:TextBoxWatermarkExtender ID="Twm_Txt_No_Empleado_Aval" runat="server" TargetControlID ="Txt_No_Empleado_Aval" 
                                        WatermarkText="No Empleado" WatermarkCssClass="watermarked"/>                                                                               
                                </td>                            
                                <td style="width:10%;text-align:left;">
                                    Nombre
                                </td>
                                <td style="width:65%;text-align:left;">
                                    <asp:TextBox ID="Txt_Nombre_Empleado_Aval" runat="server" Width="98%" Enabled="false"/>
                                </td>                                         
                            </tr>                        
                        </table>
                    </div>          
                            
                    <table width="100%">
                        <tr>
                            <td style="width:10%;vertical-align:top;" align="center">
                                <asp:Image ID="Img_Empleado_Aval" runat="server"  Width="70px" Height="85px"
                                    ImageUrl="~/paginas/imagenes/paginas/Sias_No_Disponible.JPG" BorderWidth="1px" />
                            </td>
                            <td style="width:90%;">
                                <table width="100%">
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            RFC
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_RFC_Aval" runat="server" Width="98%" Enabled="false"/>
                                        </td>                                                    
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            Sindicato
                                        </td>
                                        <td style="width:80%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Sindicato_Aval" runat="server" Width="99.5%" Enabled="false"/>
                                        </td>                                                                   
                                    </tr>   
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            U. Responsable
                                        </td>
                                        <td style="width:80%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Dependencia_Aval" runat="server" Width="99.5%" Enabled="false"/>
                                        </td>                                                                   
                                    </tr>       
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            Direcci&oacute;n Empleado
                                        </td>
                                        <td style="width:80%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Direccion_Aval" runat="server" Width="99.5%" Enabled="false"/>
                                        </td>                                 
                                    </tr>     
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            Fecha Ingreso
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Fecha_Ingreso_Aval" runat="server" Width="98%" Enabled="false"/>
                                        </td>      
                                        <td style="width:20%;text-align:left;">
                                            Sueldo Mensual
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Sueldo_Mensual_Aval" runat="server" Width="98%" Enabled="false" CssClass="text_cantidades_grid"/>
                                        </td>                                                      
                                    </tr>                                                          
                                </table>  
                            </td>                        
                        </tr>
                    </table>                                      
                </asp:Panel>
                        
                <asp:Panel ID="Pnl_Datos_Prestamo" runat="server" GroupingText="Datos del Prestamo">
                    <table style="width:100%;">
                        <tr style="width:100%;">
                            <td colspan="4">
                                <div id="Div_Prestamo_Datos_Empleado" style="background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;text-align:center;">
                                    <table width="100%">
                                        <tr style="width:100%;">
                                            <td style="width:100%;color:Black;">
                                                Datos del Empleado Solicitante
                                            </td>
                                        </tr>
                                    </table>                                    
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:20%;text-align:left;">
                                *Importe Prestamo
                            </td>
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_Importe_Prestamo" runat="server" MaxLength="10" TabIndex="10" 
                                    Width="98%" onblur="$('input[id$=Txt_Importe_Prestamo]').formatCurrency({colorize:true, region: 'es-MX'});"
                                    ontextchanged="Txt_Importe_Prestamo_TextChanged" AutoPostBack="true"/>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Importe_Prestamo" runat="server" 
                                    TargetControlID="Txt_Importe_Prestamo" FilterType="Custom, Numbers" ValidChars="-,."/>
                            </td>     
                           <td style="width:20%;text-align:left;">
                            </td>
                            <td style="width:30%;text-align:left;">
                            </td>                            
                        </tr>                           
                        <tr>
                            <td style="width:20%;text-align:left;">
                                *Proveedor 
                            </td>
                            <td style="width:80%;text-align:left;" colspan="3">
                                <asp:DropDownList ID="Cmb_Proveedor" runat="server" Width="100%" TabIndex="17" AutoPostBack="true" 
                                    OnSelectedIndexChanged="Cmb_Proveedor_SelectedIndexChanged"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:20%;text-align:left;">
                                *Concepto Deducci&oacute;n
                            </td>              
                            <td style="width:80%;text-align:left;" colspan="3">
                                <asp:DropDownList ID="Cmb_Deduccion" runat="server" Width="100%" TabIndex="18"/>
                            </td> 
                        </tr>          
                        <tr>
                            <td style="width:20%;text-align:left;vertical-align:top;">
                                *Finalidad Prestamo
                            </td>
                            <td style="width:80%;text-align:left;" colspan="3">
                                <asp:TextBox ID="Txt_Finalidad_Prestamo" runat="server" Width="99%" TabIndex="9" TextMode="MultiLine"
                                    Height="60px" MaxLength="1" Wrap="true"/>  
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Finalidad_Prestamo" runat="server"  TargetControlID="Txt_Finalidad_Prestamo"
                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>    
                                <cc1:TextBoxWatermarkExtender ID="Twe_Fte_Txt_Finalidad_Prestamo" runat="server" TargetControlID ="Txt_Finalidad_Prestamo" 
                                    WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/>      
                                <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>                                                                 
                            </td>                                                                                               
                        </tr>                                         
                        <tr style="width:100%;">
                            <td colspan="4">
                                <div id="Div_Prestamo_RH" style="background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;text-align:center;">
                                    <table width="100%">
                                        <tr style="width:100%;">
                                            <td style="width:100%;color:Black;">
                                                Datos Recursos Humanos
                                            </td>
                                        </tr>
                                    </table>                                    
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:97%;" colspan="4" align="center">
                                <div id="Div_Periodo-Pago"  style="font-size:12;color:White; font-weight:bold;border-style:outset; cursor:auto;width:98%;">
                                    <table width="98%">
                                        <tr>
                                            <td style="width:20%;text-align:left;color:Black;">
                                                *Nomina
                                            </td>
                                            <td style="width:30%;text-align:left;">
                                                <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" AutoPostBack="true" 
                                                    TabIndex="5" onselectedindexchanged="Cmb_Calendario_Nomina_SelectedIndexChanged"/>
                                            </td>             
                                            <td style="width:20%;text-align:left; color:Black;"">
                                                *Periodo
                                            </td>
                                            <td style="width:30%;text-align:left;">
                                                <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" 
                                                    Width="100%" TabIndex="6" AutoPostBack="true" 
                                                    onselectedindexchanged="Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged"/>
                                            </td>                                                                                        
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>                                                
                        <tr>
                            <td style="width:20%;text-align:left;">
                                *Fecha Solicitud
                            </td>
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_Fecha_Solicitud_Prestamo" runat="server" Width="85%" TabIndex="4" MaxLength="10"/>  
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Fecha_Solicitud_Prestamo" 
                                    runat="server" TargetControlID="Txt_Fecha_Solicitud_Prestamo" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/"/>
                                <cc1:CalendarExtender ID="Txt_Fecha_Solicitud_Prestamo_CalendarExtender" runat="server"
                                    TargetControlID="Txt_Fecha_Solicitud_Prestamo" PopupButtonID="Btn_Fecha_Solicitud_Prestamo" Format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Fecha_Solicitud_Prestamo" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha" CausesValidation="false"/>                          
                            </td>  
                            <td style="width:20%;text-align:left;">
                                *Estatus
                            </td>
                            <td style="width:30%;text-align:left;">
                                <asp:DropDownList ID="Cmb_Estatus_Solicitud_Prestamo" runat="server" Width="100%" TabIndex="5" Enabled="false">
                                    <asp:ListItem>&lt;Seleccione&gt;</asp:ListItem>
                                    <asp:ListItem Selected="True">Pendiente</asp:ListItem>
                                    <asp:ListItem>Autorizado</asp:ListItem>
                                    <asp:ListItem>Rechazado</asp:ListItem>                                  
                                </asp:DropDownList>                               
                            </td>                                                                                                                          
                        </tr>
                        <tr>
                            <td style="width:20%;text-align:left;">
                                *Fecha Inicio Pago
                            </td>
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_Fecha_Inicio_Pago_Prestamo" runat="server" Width="98%" TabIndex="7" Enabled="false"/>                                
                            </td>     
                            <td style="width:20%;text-align:left;">
                                *Fecha Termino Pago
                            </td>              
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_Fecha_Termino_Pago_Prestamo" runat="server" Width="98%" TabIndex="8" Enabled="false"/> 
                            </td>                                                                                             
                        </tr>                                   
                        <tr>
                            <td style="width:20%;text-align:left;">
                                *No Pagos
                            </td>
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_No_Pagos" runat="server" Width="98%" TabIndex="13" MaxLength="10"
                                    OnTextChanged="Txt_No_Pagos_TextChanged" AutoPostBack="true" onblur="if(parseInt($('input[id$=Txt_No_Pagos]').val()) == 0 || $('input[id$=Txt_No_Pagos]').val() == ''){ $('input[id$=Txt_No_Pagos]').val(''); $('input[id$=Txt_Abono]').val('');}"/>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Pagos" runat="server" FilterType="Custom, Numbers"
                                    TargetControlID="Txt_No_Pagos" ValidChars="."/>                           
                            </td>     
                            <td style="width:20%;text-align:left;">
                                *Abono
                            </td>              
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_Abono" runat="server" Width="98%" TabIndex="14" Enabled="false" CssClass="text_cantidades_grid"/>  
                            </td> 
                        </tr>    
                        <tr>
                            <td style="width:20%;text-align:left;">
                                *No Abonos 
                            </td>
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_No_Abonos" runat="server" Width="98%" TabIndex="15" Enabled="false"/>                                
                            </td>     
                            <td style="width:20%;text-align:left;">
                                *Saldo Actual
                            </td>              
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_Saldo_Actual" runat="server" Width="98%" TabIndex="16" Enabled="false" CssClass="text_cantidades_grid"/> 
                            </td> 
                        </tr>                
                        <tr style="cursor:auto; background-color:#2F4E7D;  color:White; font-weight:bold; filter:alpha(opacity=40);-moz-opacity: 0.40;opacity: 0.40;">
                            <td style="width:20%;text-align:left;">
                                <asp:Label ID="Lbl_Importe_Interes" runat="server" Text="Importe Interes"/>
                            </td>              
                            <td style="width:30%;text-align:right;">
                                <asp:CheckBox ID="Chk_Aplica_Porcentaje" runat="server" ToolTip="El Importe de interes es una cantidad fija o un porcentaje."
                                    OnCheckedChanged="Chk_Aplica_Porcentaje_CheckedChanged" AutoPostBack="true" Width="30px"/>
                                <asp:TextBox ID="Txt_Importe_Interes" runat="server" MaxLength="10" TabIndex="11" 
                                    Width="200px" onblur="$('input[id$=Txt_Importe_Interes]').formatCurrency({colorize:true, region: 'es-MX'});" CssClass="text_cantidades_grid"
                                    OnTextChanged="Txt_Importe_Interes_TextChanged" AutoPostBack="true"/>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Importe_Interes" runat="server" 
                                    TargetControlID="Txt_Importe_Interes" FilterType="Custom, Numbers" ValidChars="-,."/>
                            </td>                         
                            <td style="width:20%;text-align:left;">
                                Total Prestamo
                            </td>
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_Total_Prestamo" runat="server" Width="98%" Enabled="false" TabIndex="12" CssClass="text_cantidades_grid"/>
                            </td>  
                        </tr>                                                                                                                                                               
                    </table>
                </asp:Panel>       
                <br /><br /><br />                              
            </div>
            
            <cc1:ModalPopupExtender ID="MPE_Autorizacion_Prestamos" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="MPE_Autoarizacion"
                PopupControlID="Pnl_Autorizacion_Contenedor" TargetControlID="Btn_Comodin_autorizacion2" PopupDragHandleControlID="Pnl_Autorizacion_Cabecera" 
                CancelControlID="Btn_Comodin_autorizacion1" DropShadow="True" DynamicServicePath="" Enabled="True" />  
            <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_autorizacion1" runat="server" Text="" />            
            <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_autorizacion2" runat="server" Text="" />             
            
            
            <cc1:ModalPopupExtender ID="Mpe_Cancelar_Prestamo" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="MPE_Cancelacion"
                PopupControlID="Pnl_Cancelacion_Prestamo" TargetControlID="Btn_Comodin_Cancelacion_Open" PopupDragHandleControlID="Pnl_Cabecera_Cancelacion" 
                CancelControlID="Btn_Comodin_Cancelacion_Close" DropShadow="True" DynamicServicePath="" Enabled="True" />  
            <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Cancelacion_Open" runat="server" Text="" />            
            <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Cancelacion_Close" runat="server" Text="" />             
        </ContentTemplate>
    </asp:UpdatePanel>   


<asp:UpdatePanel ID="Udp_Modal_Popup" runat="server" UpdateMode="Conditional">
   <ContentTemplate>    
        <cc1:ModalPopupExtender ID="Mpe_Busqueda_Prestamos" runat="server" BackgroundCssClass="popUpStyle" 
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
                     B&uacute;squeda: Prestamos
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana_Autorizacion" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClick="Btn_Cerrar_Ventana_Autorizacion_Click"/>  
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
                                           <asp:TextBox ID="Txt_Busqueda_No_Solicitud" runat="server" Width="98%" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Solicitud" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Busqueda_No_Solicitud"/>                                             
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;">
                 
                                        </td>              
                                        <td style="width:30%;text-align:left;">
                                        
                                        </td>                                         
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                           Estatus
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                          <asp:DropDownList ID="Cmb_Busqueda_Estatus_Solicitud" runat="server" Width="100%">   
                                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>                                               
                                                <asp:ListItem>Pendiente</asp:ListItem>
                                                <asp:ListItem>Autorizado</asp:ListItem>
                                                <asp:ListItem>Rechazado</asp:ListItem>                              
                                            </asp:DropDownList>
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                           Estado
                                        </td>              
                                        <td style="width:30%;text-align:left;">
                                          <asp:DropDownList ID="Cmb_Busqueda_Estado_Prestamo" runat="server" Width="100%">   
                                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>                                                                                             
                                                <asp:ListItem>Pendiente</asp:ListItem>
                                                <asp:ListItem>Proceso</asp:ListItem>
                                                <asp:ListItem>Pagado</asp:ListItem>                               
                                            </asp:DropDownList>                                        
                                        </td>                                         
                                    </tr>                                      
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            No Empleado Solicita
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Busqueda_Empleado_Solicitante" runat="server" Width="98%" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Empleado_Solicitante" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Busqueda_Empleado_Solicitante"/>                                            
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            No Empleado Aval
                                        </td>              
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Empleado_Aval" runat="server" Width="98%" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Empleado_Aval" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Busqueda_Empleado_Aval"/>                                               
                                        </td>                                         
                                    </tr>  
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            RFC Solicita
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Busqueda_RFC_Solicita" runat="server" Width="98%" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_RFC_Solicita" runat="server" FilterType="Numbers, UppercaseLetters"
                                                TargetControlID="Txt_Busqueda_RFC_Solicita"/>                                            
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            RFC Aval
                                        </td>              
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_RFC_Aval" runat="server" Width="98%" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_RFC_Aval" runat="server" FilterType="Numbers, UppercaseLetters"
                                                TargetControlID="Txt_Busqueda_RFC_Aval"/>                                               
                                        </td>                                         
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Fecha Inicio
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                                <asp:TextBox ID="Txt_Busqueda_Fecha_Inicio" runat="server" Width="85%" MaxLength="1" TabIndex="14"/>
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
                                                <asp:TextBox ID="Txt_Busqueda_Fecha_Fin" runat="server" Width="85%" MaxLength="1" TabIndex="15"/>
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
                                               <asp:Button ID="Btn_Buscar_Solicitudes_Prestamos" runat="server"  Text="Buscar" CssClass="button"  CausesValidation="false"  
                                                    OnClick="Btn_Buscar_Solicitudes_Prestamos_Click" Width="60px" /> 
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
                        <asp:UpdatePanel ID="Upnl_Grid_Prestamos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                           <asp:GridView ID="Grid_Prestamos" runat="server" AutoGenerateColumns="False" CellPadding="4" Width="100%"                                             
                                AllowPaging ="True" PageSize="5" ForeColor="#333333" GridLines="None" CssClass="GridView_1"
                                OnSelectedIndexChanged="Grid_Prestamos_OnSelectedIndexChanged"
                                OnRowDataBound="Grid_Prestamos_RowDataBound"
                                OnPageIndexChanging="Grid_Prestamos_PageIndexChanging">                                
                                       <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" HeaderText="Seleccione"
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%"/>
                                                </asp:ButtonField>    
                                                <asp:BoundField DataField="NO_SOLICITUD" HeaderText="No Solicitud" Visible="True" >
                                                        <ItemStyle Width="20%"  HorizontalAlign="Left"/>
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%"/>                                                    
                                                </asp:BoundField>
                                                <asp:BoundField DataField="NOMBRE" HeaderText="Empleado Solicita" Visible="True" >
                                                        <ItemStyle Width="40%"  HorizontalAlign="Left"/>
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%"/>                                                    
                                                </asp:BoundField>                                                  
                                                <asp:BoundField DataField="ESTATUS_SOLICITUD" HeaderText="Estatus" Visible="True">
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
                                <asp:AsyncPostBackTrigger ControlID="Btn_Buscar_Solicitudes_Prestamos"  EventName="Click"/>
                            </Triggers>                                        
                        </asp:UpdatePanel>                                    
                    </td>
                </tr>
             </table>                                                   
           </div>                 
    </asp:Panel>     
    
    
    
<asp:Panel ID="Pnl_Autorizacion_Contenedor" runat="server" CssClass="drag" HorizontalAlign="Center" Width="650px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Autorizacion_Cabecera" runat="server" 
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Image1" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Recursos Humanos: Autorizar Prestamos
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
    </asp:Panel>                                                                          
        <div style="cursor:default;width:100%">
            <table width="100%" style="background-color:#ffffff;">     
                <tr>
                    <td style="width:100%" colspan="4">
                        &nbsp;<asp:Label ID="Lbl_Observaciones_Autorizacion" runat="server" />
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
                        <asp:TextBox ID="Txt_Autorizacion_No_Empleado" runat="server" Width="98%" MaxLength="10" TabIndex="11"
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
                        <asp:TextBox ID="Txt_Autorizacion_Nombre_Empleado" runat="server" Width="98%" TabIndex="13"
                            Enabled="false"/>
                    </td>                                                            
                </tr>                        
                <tr>
                   <td style="text-align:left;width:20%;vertical-align:top;">
                       *Motivo Rechazo
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
                        <asp:ImageButton ID="Btn_Guardar_Autorizacion_Prestamos" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png"
                            style="border-style:none;background-color:White;font-weight:bold;cursor:hand;"  TabIndex="16" ToolTip="Autorizar"
                            OnClick="Btn_Guardar_Autorizacion_Prestamos_Click"/>   
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



<asp:Panel ID="Pnl_Cancelacion_Prestamo" runat="server" CssClass="drag" HorizontalAlign="Center" Width="650px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Cabecera_Cancelacion" runat="server" 
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Img_Inf_Cancelar_Prestamo" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Recursos Humanos: Cancelar Prestamo
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana_Cancelacion" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
    </asp:Panel>                                                                          
        <div style="cursor:default;width:100%">
            <table width="100%" style="background-color:#ffffff;">     
                <tr>
                    <td style="width:100%" colspan="4">
                        &nbsp;<asp:Label ID="Lbl_Observaciones_Cancelacion" runat="server" />
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
                        <asp:TextBox ID="Txt_Cancelacion_No_Empleado" runat="server" Width="98%" MaxLength="10" TabIndex="11"
                            Enabled="false"/>
                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cancelacion_No_Empleado" 
                             runat="server" TargetControlID="Txt_Cancelacion_No_Empleado" FilterType="Numbers"/>                          
                    </td>
                    <td style="width:20%;text-align:left;">   
                        *Referencia Recibo                                            
                    </td>
                    <td style="width:30%;text-align:left;">  
                        <asp:TextBox ID="Txt_Cancelacion_Referencia_Pago" runat="server" Width="98%" TabIndex="13"
                            Enabled="false"/>    
                        <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Cancelacion_Referencia_Pago" runat="server" 
                            TargetControlID ="Txt_Cancelacion_Referencia_Pago" WatermarkText="Requerido" WatermarkCssClass="watermarked"/>                                                                
                    </td>                                                            
                </tr> 
                <tr>
                    <td style="width:20%;text-align:left;">
                        Nombre
                    </td>
                    <td style="width:80%;text-align:left;" colspan="3">
                        <asp:TextBox ID="Txt_Cancelacion_Nombre_Empleado" runat="server" Width="98%" TabIndex="13"
                            Enabled="false"/>
                    </td>                                                            
                </tr>                        
                <tr>
                   <td style="text-align:left;width:20%;vertical-align:top;">
                       *Motivo Cancelaci&oacute;n
                   </td>
                   <td  style="text-align:left;width:80%;" colspan="3">
                        <asp:TextBox ID="Txt_Cancelacion_Comentarios" runat="server" Width="99%" MaxLength="100" TextMode="MultiLine" Enabled="false"/>
                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cancelacion_Comentarios" runat="server"  
                            TargetControlID="Txt_Autorizacion_Comentarios" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" 
                            ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>    
                        <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Cancelacion_Comentarios" runat="server" 
                            TargetControlID ="Txt_Cancelacion_Comentarios" WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/>                                                   
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
                           Cancelar Prestamo
                           <asp:CheckBox ID="Chk_Cancelar_Prestamo"  onclick="javascript:Cancelar();" runat="server" Checked="false"/>
                       </center>
                   </td>
               </tr>                                   
               <tr>
                    <td style="width:100%" colspan="4" align="right">
                        <asp:ImageButton ID="Btn_Cancelar_Prestamo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png"
                            style="border-style:none;background-color:White;font-weight:bold;cursor:hand;"  TabIndex="16" ToolTip="Cancelar Prestamos"
                            Enabled = "false" OnClick="Btn_Cancelar_Prestamo_Click"/>   
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


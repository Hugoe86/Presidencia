<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Modificar_Empleados.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Empleados" Title="Modificar Empleado" %>
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
            document.getElementById("<%=Txt_Busqueda_No_Empleado.ClientID%>").value="";
            //document.getElementById("<%=Cmb_Busqueda_Estatus.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_Nombre_Empleado.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_RFC.ClientID%>").value="";  
            document.getElementById("<%=Cmb_Busqueda_Rol.ClientID%>").value="";            
            document.getElementById("<%=Cmb_Busqueda_Dependencia.ClientID%>").value="";          
            document.getElementById("<%=Cmb_Busqueda_Tipo_Contrato.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Areas.ClientID%>").value="";            
            document.getElementById("<%=Cmb_Busqueda_Puesto.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Escolaridad.ClientID%>").value="";            
            document.getElementById("<%=Cmb_Busqueda_Sindicato.ClientID%>").value="";                     
            document.getElementById("<%=Cmb_Busqueda_Turno.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Zona.ClientID%>").value="";            
            document.getElementById("<%=Cmb_Busqueda_Tipo_Trabajador.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Tipo_Nomina.ClientID%>").value="";                                                     
            document.getElementById("<%=Txt_Busqueda_Fecha_Inicio.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_Fecha_Fin.ClientID%>").value="";                        
            return false;
        }  
        
        function Abrir_Modal_Popup() {
            $find('Busqueda_Empleados').show();
            return false;
        }

        function pageLoad(sender, args) {
            $("input[id$=Txt_Salario_Diario]").bind('keyup', function() {
                $("input[id$=Txt_Salario_Diario_Integrado]").val(Math.round((parseFloat($(this).val()) * 1.126) * 100) / 100);
            });
        
            $('textarea[id*=Txt_Comentarios_Empleado]').keyup(function() {var Caracteres =  $(this).val().length; if (Caracteres > 250) {this.value = this.value.substring(0, 250); Caracteres =  $(this).val().length;  $(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});

            $('input[id*=Txt_Busqueda_No_Empleado]').live("blur", function() {
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
            });

            $('input[id*=Txt_No_Empleado]').live("blur", function() {
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
            });
            
            $('input[id$=Txt_Empleado_Confronto]').css("display", "none");
            Generar_CONFRONTO();
        }
        
        function Generar_CONFRONTO(){
            var CONFRONTO = "";
            var FULL_NAME = "";
            var i = 0;

            try{
                FULL_NAME = $('input[id$=Txt_Apellido_Paterno_Empleado]').val() + " " + 
                            $('input[id$=Txt_Apellido_Materno_Empleado]').val() + " " +
                            $('input[id$=Txt_Nombre_Empleado]').val();
                
                if(FULL_NAME.replace(/^\s*|\s*$/g,"") != ""){
                    FULL_NAME = FULL_NAME.toUpperCase().toString().replace(/^\s*|\s*$/g,"");
                    var Palabras = FULL_NAME.split(' ');
                    
                    for(i=0; i<Palabras.length; i++){
                        CONFRONTO += Palabras[i].substring(0,1);
                    }
                    
                    $('input[id$=Txt_Empleado_Confronto]').css("display", "Block");
                    $('input[id$=Txt_Empleado_Confronto]').val(CONFRONTO);
                    $('input[id$=Txt_Empleado_Confronto]').css("color", "#000000");
                    $('input[id$=Txt_Empleado_Confronto]').css("font-size", "15px");
                    $('input[id$=Txt_Empleado_Confronto]').css("font-family", "Elephant");
                    $('input[id$=Txt_Empleado_Confronto]').css("text-align", "Center");
                    $('input[id$=Txt_Empleado_Confronto]').css("background-color", "#f5f5f5");
                    $('input[id$=Txt_Empleado_Confronto]').css('border-style', 'solid');
                    $('input[id$=Txt_Empleado_Confronto]').css('border-color', 'Black');
                    $('input[id$=Txt_Empleado_Confronto]').css('height', 20);
                    
                }else{
                    $('input[id$=Txt_Empleado_Confronto]').val('');
                    $('input[id$=Txt_Empleado_Confronto]').css("display", "none");
                }
            }catch(e){
                alert("Error al generar el CONFRONTO del empleado. Error: " + e + "]");
            }
        }
</script>

   <!--SCRIPT PARA LA VALIDACION QUE NO EXPERE LA SESSION-->  
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

    <asp:ScriptManager ID="ScriptManager_Empleados" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>  
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>          
            
            <div id="Div_Empleados" style="background-color:#ffffff; width:100%; height:100%;font-size:10px;" >
            <div id="tooltip"></div>
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Modificar Empleado
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" >&nbsp;
                            <asp:UpdatePanel ID="Upnl_Mensajes_Error" runat="server" UpdateMode="Always" RenderMode="Inline">
                                <ContentTemplate>                         
                                    <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                                </ContentTemplate>                                
                            </asp:UpdatePanel>                                      
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
                                            <asp:UpdatePanel ID="Upnl_Botones_Operacion" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
                                                <ContentTemplate> 
                                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                                    <asp:ImageButton ID="Btn_Eliminar_Mostrar_Popup" runat="server" ToolTip="Eliminar" CssClass="Img_Button"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                                        OnClick="Btn_Eliminar_Mostrar_Popup_Click"/>
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>                                                
                                          </td>
                                          <td align="right" style="width:41%;">
                                            <table style="width:100%;height:28px;">
                                                <tr>
                                                    <td style="width:100%;vertical-align:top;" align="right">
                                                        B&uacute;squeda 
                                                        <asp:UpdatePanel ID="Udp_Modal_Popup" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
                                                            <ContentTemplate> 
                                                                    <asp:ImageButton ID="Btn_Mostrar_Popup_Busqueda" runat="server" ToolTip="Busqueda Avanzada"
                                                                        ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px" Width="24px"
                                                                        OnClientClick="javascript:return Abrir_Modal_Popup();" CausesValidation="false" />
                                                                        
                                                                    <cc1:ModalPopupExtender ID="Mpe_Busqueda_Empleados" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="Busqueda_Empleados"
                                                                        PopupControlID="Pnl_Busqueda_Contenedor" TargetControlID="Btn_Comodin_Open" PopupDragHandleControlID="Pnl_Busqueda_Cabecera" 
                                                                        CancelControlID="Btn_Comodin_Close" DropShadow="True" DynamicServicePath="" Enabled="True"/>  
                                                                    <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Close" runat="server" Text="" />
                                                                    <asp:Button  Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Open" runat="server" Text="" />                                                                                                    
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
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
              
            <asp:UpdatePanel ID="Upnl_Catalogo_Empleados" runat="server" UpdateMode="Conditional">
                <ContentTemplate>             
                <asp:Panel ID="Pnl_Datos_Generales" runat="server"  GroupingText="Datos Generales" Width="97%">
                    <table>
                        <tr>
                            <td style="width:10%;vertical-align:top;" align="center">
                                <asp:Image ID="Img_Foto_Empleado" runat="server"  Width="70px" Height="85px"
                                    ImageUrl="~/paginas/imagenes/paginas/Sias_No_Disponible.JPG" BorderWidth="1px"/>           
                                <asp:HiddenField ID="Txt_Ruta_Foto" runat="server" />         
                                <asp:ImageButton ID="Btn_Subir_Foto" runat="server" ToolTip="Mostrar Foto del Empleado"  OnClick="Subir_Foto_Click" 
                                     ImageUrl="../imagenes/paginas/Sias_Actualizar.png" style="cursor:hand;"/>
                            </td>
                            <td style="width:90%;">
                                <table width="100%" class="estilo_fuente">                                                                           
                                    <tr style="display:none;">                      
                                        <td style="width:20%;text-align:left;">                                
                                            Empleado ID
                                        </td>                                                        
                                        <td style="width:30%;text-align:left;">                                
                                            <asp:TextBox ID="Txt_Empleado_ID" runat="server" ReadOnly="True" Width="98%" TabIndex="1"/>                                                                     
                                        </td>                            
                                        <td style="width:20%;text-align:left;">    
                                            
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Empleado_Confronto" runat="server" Width="98%" Enabled="false"/>
                                        </td>                    
                                    </tr>                       
                                    <tr>                    
                                        <td style="width:20%;text-align:left;">
                                            *No Empleado
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="98%" TabIndex="2" MaxLength="6" onkeyup='this.value = this.value.toUpperCase();'/> 
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Empleadoo" runat="server"
                                                TargetControlID="Txt_No_Empleado" FilterType="Numbers"/>                       
                                        </td>
                                        <td style="width:20%;text-align:left;">
                                            &nbsp;&nbsp;Estatus
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Estatus_Empleado" runat="server" Width="101%" TabIndex="3">                                                                  
                                                <asp:ListItem>&lt; - Seleccione - &gt;</asp:ListItem>
                                                <asp:ListItem>ACTIVO</asp:ListItem>
                                                <asp:ListItem>INACTIVO</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            *Nombre
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" TabIndex="4" MaxLength="100" Width="98%" onkeyup="javascript:Generar_CONFRONTO();this.value = this.value.toUpperCase();"/>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Empleado" runat="server" TargetControlID="Txt_Nombre_Empleado" 
                                                FilterType="Custom, UppercaseLetters, LowercaseLetters" ValidChars="ÑñáéíóúÁÉÍÓÚ "/>
                                        </td>
                                        <td style="width:20%;text-align:left;" style="display:none;">
                                            &nbsp;&nbsp;*Rol
                                        </td>
                                        <td colspan="3" style="width:30%;text-align:left;" style="display:none;">
                                            <asp:DropDownList ID="Cmb_Roles_Empleado" runat="server" Width="100%" TabIndex="5"/>
                                        </td>                            
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;">
                                            *Apellido Paterno
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Apellido_Paterno_Empleado" runat="server" TabIndex="6" MaxLength="100" Width="98%" onkeyup="javascript:Generar_CONFRONTO();this.value = this.value.toUpperCase();"/>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Apellido_Paterno_Empleado" runat="server" 
                                                TargetControlID="Txt_Apellido_Paterno_Empleado" FilterType="Custom, UppercaseLetters, LowercaseLetters" 
                                                ValidChars="ÑñáéíóúÁÉÍÓÚ "/>
                                        </td>
                                        <td style="width:20%;text-align:left;">
                                            &nbsp;&nbsp;Apellido Materno
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Apellido_Materno_Empleado" runat="server" TabIndex="7" MaxLength="100" Width="98%" onkeyup="javascript:Generar_CONFRONTO();this.value = this.value.toUpperCase();"/>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Apellido_Materno_Empleado" runat="server" 
                                                TargetControlID="Txt_Apellido_Materno_Empleado" FilterType="Custom, UppercaseLetters, LowercaseLetters" 
                                                ValidChars="ÑñáéíóúÁÉÍÓÚ "/>
                                        </td>
                                    </tr>
                                    <tr style="display:none;">
                                        <td style="width:20%;text-align:left;">
                                            Password
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Password_Empleado" runat="server" TabIndex="8" MaxLength="100" 
                                                Width="98%" TextMode="Password"/>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Password_Empleado" runat="server" 
                                                TargetControlID="Txt_Password_Empleado" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                ValidChars="ÑñáéíóúÁÉÍÓÚ!¡#$%&/()=¿?.,:;-+* "/>
                                        </td>
                                        <td style="width:20%;text-align:left;">
                                            &nbsp;&nbsp;Confirmar Password
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Confirma_Password_Empleado" runat="server" TabIndex="9" MaxLength="100" 
                                                Width="98%" TextMode="Password"/>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Confirma_Password_Empleado" runat="server" 
                                                TargetControlID="Txt_Confirma_Password_Empleado" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                ValidChars="ÑñáéíóúÁÉÍÓÚ!¡#$%&/()=¿?.,:;-+* " />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;vertical-align:top;">
                                            Comentarios
                                        </td>
                                        <td colspan="3" style="width:80%;text-align:left;">
                                            <asp:TextBox ID="Txt_Comentarios_Empleado" runat="server" TabIndex="10" MaxLength="250" onkeyup='this.value = this.value.toUpperCase();'
                                                TextMode="MultiLine" Width="100%" Height="60px" />
                                            <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Empleado" runat="server" 
                                                TargetControlID ="Txt_Comentarios_Empleado" WatermarkText="Límite de Caractes 250" 
                                                WatermarkCssClass="watermarked"/>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Empleado" runat="server" 
                                                TargetControlID="Txt_Comentarios_Empleado" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ "/>
                                        </td>
                                    </tr>    
                                    <tr>                    
                                        <td style="width:20%;text-align:left;">
                                            Buscar Foto
                                        </td>
                                        <td style="width:80%;text-align:left;" colspan="3">
                                            <cc1:AsyncFileUpload ID="Async_Foto_Empleado" runat="server"  Width="550px" />                       
                                        </td>
                                    </tr>                                         
                                </table>                        
                            </td>                        
                        </tr>
                    </table>
                </asp:Panel>
                

                <cc1:TabContainer ID="Contenedor_Datos_Personales" runat="server" Width="97%" 
                    ActiveTabIndex="0" CssClass="Tab">
                    <cc1:TabPanel HeaderText="Datos Empleados" ID="Tab_Datos_Personales" runat="server">
                        <HeaderTemplate>Personales</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:20%;text-align:left;">
                                        *F. Nacimiento
                                    </td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Fecha_Nacimiento_Empleado" runat="server" Width="84%" TabIndex="11" MaxLength="11" Height="18px" onblur='this.value = this.value.toLowerCase();'/>
                                        <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Nacimiento_Empleado" runat="server" 
                                            TargetControlID="Txt_Fecha_Nacimiento_Empleado" WatermarkCssClass="watermarked" 
                                            WatermarkText="Dia/Mes/Año" Enabled="True" />
                                        <cc1:CalendarExtender ID="CE_Txt_Fecha_Nacimiento_Empleado" runat="server" 
                                            TargetControlID="Txt_Fecha_Nacimiento_Empleado" Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Fecha_Nacimiento"/>
                                         <asp:ImageButton ID="Btn_Fecha_Nacimiento" runat="server"
                                            ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                            Height="18px" CausesValidation="false"/>           
                                        <cc1:MaskedEditExtender 
                                            ID="Mee_Txt_Fecha_Nacimiento_Empleado" 
                                            Mask="99/LLL/9999" 
                                            runat="server"
                                            MaskType="None" 
                                            UserDateFormat="DayMonthYear" 
                                            UserTimeFormat="None" Filtered="/"
                                            TargetControlID="Txt_Fecha_Nacimiento_Empleado" 
                                            Enabled="True" 
                                            ClearMaskOnLostFocus="false"/>  
                                        <cc1:MaskedEditValidator 
                                            ID="Mev_Txt_Fecha_Nacimiento_Empleado" 
                                            runat="server" 
                                            ControlToValidate="Txt_Fecha_Nacimiento_Empleado"
                                            ControlExtender="Mee_Txt_Fecha_Nacimiento_Empleado" 
                                            EmptyValueMessage="Fecha Requerida"
                                            InvalidValueMessage="Fecha Nacimiento Invalida" 
                                            IsValidEmpty="false" 
                                            TooltipMessage="Ingrese o Seleccione la Fecha Nacimiento"
                                            Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                                                                                            
                                    </td>
                                    <td style="width:20%;text-align:left;">
                                        &nbsp;&nbsp;*Sexo
                                    </td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:DropDownList ID="Cmb_Sexo_Empleado" runat="server" Width="100%" TabIndex="12">
                                            <asp:ListItem><- Seleccionar -></asp:ListItem>
                                            <asp:ListItem>MASCULINO</asp:ListItem>
                                            <asp:ListItem>FEMENINO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">
                                        RFC
                                    </td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_RFC_Empleado" runat="server" Width="98%" TabIndex="13"
                                            onkeyup='this.value = this.value.toUpperCase();' MaxLength="13"
                                            onblur="$('input[id$=Txt_RFC_Empleado]').filter(function(){if(!this.value.match(/^[a-zA-Z]{3,4}(\d{6})((\D|\d){3})?$/))$(this).val('');});"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_RFC_Empleado" runat="server"
                                            TargetControlID="Txt_RFC_Empleado" FilterType="Numbers, UppercaseLetters, LowercaseLetters" 
                                            ValidChars=" " Enabled="True"/>
                                    </td>
                                    <td style="width:20%;text-align:left;">
                                        &nbsp;&nbsp;C.U.R.P.
                                    </td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_CURP_Empleado" runat="server" Width="98%" TabIndex="14"
                                            onkeyup='this.value = this.value.toUpperCase();' MaxLength="18"
                                            onblur="$('input[id$=Txt_CURP_Empleado]').filter(function(){if(!this.value.match(/^[a-zA-Z]{4}(\d{6})([a-zA-Z]{6})(\d{2})?$/))$(this).val('');});"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_CURP_Empleado" runat="server"
                                            TargetControlID="Txt_CURP_Empleado" FilterType="Numbers, UppercaseLetters, LowercaseLetters" 
                                            ValidChars=" " Enabled="True"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">
                                        *Domicilio
                                    </td>
                                    <td colspan="3" style="width:80%;text-align:left;">
                                        <asp:TextBox ID="Txt_Domicilio_Empleado" runat="server" Width="99.5%" TabIndex="15" MaxLength="100" onkeyup='this.value = this.value.toUpperCase();'/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Domicilio_Empleado" runat="server"
                                            TargetControlID="Txt_Domicilio_Empleado" 
                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ# " Enabled="True"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">
                                        *Colonia
                                    </td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Colonia_Empleado" runat="server" Width="98%" TabIndex="16" MaxLength="100" onkeyup='this.value = this.value.toUpperCase();'/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Colonia_Empleado" runat="server"
                                            TargetControlID="Txt_Colonia_Empleado" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True"/>
                                    </td>
                                    <td style="width:20%;text-align:left;">
                                        &nbsp;&nbsp;CP
                                    </td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Codigo_Postal_Empleado" runat="server" Width="98%" TabIndex="17" MaxLength="5"
                                            onblur="$('input[id$=Txt_Codigo_Postal_Empleado]').filter(function(){if(!this.value.match(/^([1-9]{2}|[0-9][1-9]|[1-9][0-9])[0-9]{3}$/))$(this).val('');});"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Codigo_Postal_Empleado" runat="server"
                                            TargetControlID="Txt_Codigo_Postal_Empleado" FilterType="Numbers" Enabled="True"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">
                                        *Ciudad
                                    </td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Ciudad_Empleado" runat="server" Width="98%" TabIndex="18" MaxLength="50" onkeyup='this.value = this.value.toUpperCase();'/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Ciudad_Empleado" runat="server"
                                            TargetControlID="Txt_Ciudad_Empleado" FilterType="UppercaseLetters, LowercaseLetters" 
                                            ValidChars=" " Enabled="True"/>
                                    </td>
                                    <td style="width:20%;text-align:left;">
                                        &nbsp;&nbsp;*Estado
                                    </td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Estado_Empleado" runat="server" Width="98%" TabIndex="19" MaxLength="50" onkeyup='this.value = this.value.toUpperCase();'/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Estado_Empleado" runat="server"
                                            TargetControlID="Txt_Estado_Empleado" FilterType="UppercaseLetters, LowercaseLetters" 
                                            ValidChars=" " Enabled="True"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">
                                        Tel. Casa
                                    </td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Telefono_Casa_Empleado" runat="server" Width="98%" TabIndex="20" MaxLength="20"
                                            onblur="$('input[id$=Txt_Telefono_Casa_Empleado]').filter(function(){if(!this.value.match(/^[0-9]{0,3}-? ?[0-9]{6,7}$/))$(this).val('');});"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Telefono_Casa_Empleado" runat="server"
                                            TargetControlID="Txt_Telefono_Casa_Empleado" FilterType="Custom, Numbers" 
                                            ValidChars="- " Enabled="True"/>
                                    </td>
                                    <td style="width:20%;text-align:left;">
                                        &nbsp;&nbsp;Celular
                                    </td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Celular_Empleado" runat="server" Width="98%" TabIndex="21" MaxLength="20"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Celular_Empleado" runat="server"
                                            TargetControlID="Txt_Celular_Empleado" FilterType="Custom, Numbers" 
                                            ValidChars="- " Enabled="True"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">
                                        Nextel
                                    </td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Nextel_Empleado" runat="server" Width="98%" TabIndex="22" MaxLength="20"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nextel_Empleado" runat="server"
                                            TargetControlID="Txt_Nextel_Empleado" FilterType="Custom, Numbers" 
                                            ValidChars="*- " Enabled="True"/>
                                    </td>
                                    <td style="width:20%;text-align:left;">
                                        &nbsp;&nbsp;Tel. Oficina
                                    </td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Telefono_Oficina_Empleado" runat="server" Width="98%" TabIndex="23" MaxLength="20"
                                            onblur="$('input[id$=Txt_Telefono_Oficina_Empleado]').filter(function(){if(!this.value.match(/^[0-9]{0,3}-? ?[0-9]{6,7}$/))$(this).val('');});"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Telefono_Oficina_Empleado" runat="server"
                                            TargetControlID="Txt_Telefono_Oficina_Empleado" FilterType="Custom, Numbers" 
                                            ValidChars="- " Enabled="True"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">
                                        Extensión
                                    </td>
                                    <td style="width:20%;text-align:left;">
                                        <asp:TextBox ID="Txt_Extension_Empleado" runat="server" Width="98%" TabIndex="24" MaxLength="10"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Extension_Empleado" runat="server"
                                            TargetControlID="Txt_Extension_Empleado" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" Enabled="True" ValidChars="- "/>
                                    </td>
                                    <td style="width:20%;text-align:left;">
                                        &nbsp;&nbsp;Fax
                                    </td>
                                    <td style="width:20%;text-align:left;">
                                        <asp:TextBox ID="Txt_Fax_Empleado" runat="server" Width="98%" TabIndex="25" MaxLength="20"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fax_Empleado" runat="server"
                                            TargetControlID="Txt_Fax_Empleado" FilterType="Numbers" ValidChars="- " Enabled="True"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">
                                        E-Mail
                                    </td>
                                    <td colspan="3" style="width:80%;text-align:left;">
                                        <asp:TextBox ID="Txt_Correo_Electronico_Empleado" runat="server" Width="99.5%" TabIndex="26" MaxLength="100"
                                            onblur="$('input[id$=Txt_Correo_Electronico_Empleado]').filter(function(){if(!this.value.match(/^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/))$(this).val('');});"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Correo_Electronico_Empleado" runat="server"
                                            TargetControlID="Txt_Correo_Electronico_Empleado" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                            ValidChars=".@-_" Enabled="True"/>
                                    </td>
                                </tr>
                            </table>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TPnl_Codigo_Programatico" HeaderText="Codigo Programatico" runat="server">
                        <HeaderTemplate>Codigo Programatico</HeaderTemplate>
                        <ContentTemplate>
                            <table style="width:100%;">
                               <tr>
                                    <td class="button_autorizar" style="width:100%; text-align:center;cursor:default;" colspan="4">   
                                         Generación Código Programático
                                    </td>
                              </tr>
                               <tr>
                                    <td class="button_autorizar" style="width:100%; text-align:left;cursor:default;" colspan="4">
                                        <hr />
                                    </td>
                              </tr>
                                <tr>
                                    <td class="button_autorizar" style="width:30%; text-align:left;cursor:default;">
                                        Unidad Responsable
                                    </td>
                                    <td class="button_autorizar" style="width:70%; text-align:left;cursor:default;" colspan="3">   
                                        <asp:DropDownList ID="Cmb_SAP_Unidad_Responsable" runat="server" Width="100%" AutoPostBack="true"
                                            OnSelectedIndexChanged="Cmb_SAP_Unidad_Responsable_SelectedIndexChanged" TabIndex="26"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="button_autorizar" style="width:30%; text-align:left;cursor:default;">
                                        Fuente Financciamiento
                                    </td>
                                    <td class="button_autorizar" style="width:70%; text-align:left;cursor:default;" colspan="3">
                                        <asp:DropDownList ID="Cmb_SAP_Fuente_Financiamiento" runat="server" Width="100%" AutoPostBack="true"
                                            OnSelectedIndexChanged="Cmb_SAP_Fuente_Financiamiento_SelectedIndexChanged" TabIndex="27"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td class="button_autorizar" style="width:30%; text-align:left;cursor:default;">
                                        Área Funcional
                                    </td>          
                                    <td class="button_autorizar" style="width:70%; text-align:left;cursor:default;" colspan="3">
                                        <asp:DropDownList ID="Cmb_SAP_Area_Funcional" runat="server" Width="100%" TabIndex="28"/>
                                    </td>
                                </tr> 
                                <tr>
                                    <td class="button_autorizar" style="width:30%; text-align:left;cursor:default;">
                                        Programa
                                    </td>          
                                    <td class="button_autorizar" style="width:70%; text-align:left;cursor:default;" colspan="3">
                                        <asp:DropDownList ID="Cmb_SAP_Programas" runat="server" Width="100%" AutoPostBack="true"
                                            OnSelectedIndexChanged="Cmb_SAP_Programas_SelectedIndexChanged" TabIndex="29"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="button_autorizar" style="width:30%; text-align:left;cursor:default;">
                                        Partida
                                    </td>
                                    <td class="button_autorizar" style="width:70%; text-align:left;cursor:default;" colspan="3">
                                        <asp:DropDownList ID="Cmb_SAP_Partida" runat="server" Width="100%" AutoPostBack="true"
                                            OnSelectedIndexChanged="Cmb_SAP_Partida_SelectedIndexChanged" TabIndex="30"/>
                                    </td>
                                </tr>  
                                <tr>
                                    <td class="button_autorizar" style="width:30%; text-align:left;cursor:default;">
                                        Puesto
                                    </td>
                                    <td class="button_autorizar" style="width:70%; text-align:left;cursor:default;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Puestos" runat="server" TabIndex="33" Width="100%"
                                            AutoPostBack="True" 
                                            OnSelectedIndexChanged="Cmb_Puestos_SelectedIndexChanged"/>
                                    </td>
                                </tr>
                               <tr>
                                    <td class="button_autorizar" style="width:100%; text-align:left;cursor:default;" colspan="4">
                                        <hr />
                                    </td>
                              </tr>
                               <tr>
                                    <td class="button_autorizar" style="width:100%; text-align:left;cursor:default;" colspan="4">
                                        Código Programático
                                    </td>
                              </tr>
                               <tr>
                                    <td style="width:100%; text-align:left;" colspan="4">
                                        <hr />
                                    </td>
                              </tr>
                              <tr>
                                    <td class="button_autorizar" style="width:100%; text-align:left;cursor:default;" colspan="4">
                                        <table width="100%">
                                            <tr>
                                                <td style="width:20%;text-align:center;border-style:outset;font-size:11px;cursor:default;">
                                                   F. Financiamiento
                                                </td>
                                                <td style="width:20%;text-align:center;border-style:outset;font-size:11px;cursor:default;">
                                                   Área Funcional
                                                </td>
                                                <td style="width:20%;text-align:center;border-style:outset;font-size:11px;cursor:default;">
                                                   Programa
                                                </td>
                                                <td style="width:20%;text-align:center;border-style:outset;font-size:11px;cursor:default;">
                                                   Unidad Responsable
                                                </td>
                                                <td style="width:20%;text-align:center;border-style:outset;font-size:11px;cursor:default;">
                                                   Partida
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td style="width:20%;">
                                                    <asp:TextBox ID="Txt_SAP_Fuente_Financiamiento" runat="server" Width="100%" CssClass="watermarked2" Enabled="false"/>
                                                </td>
                                                <td style="width:20%;">
                                                   <asp:TextBox ID="Txt_SAP_Area_Responsable" runat="server" Width="100%" CssClass="watermarked2" Enabled="false"/>
                                                </td>
                                                <td style="width:20%;"> 
                                                   <asp:TextBox ID="Txt_SAP_Programa" runat="server" Width="100%" CssClass="watermarked2" Enabled="false"/>
                                                </td>
                                                <td style="width:20%;"> 
                                                   <asp:TextBox ID="Txt_SAP_Unidad_Responsable" runat="server" Width="100%" CssClass="watermarked2" Enabled="false"/>
                                                </td>
                                                <td style="width:20%;">   
                                                   <asp:TextBox ID="Txt_SAP_Partida" runat="server" Width="100%" CssClass="watermarked2" Enabled="false"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>                       
                   </cc1:TabPanel>                                                       
                    <cc1:TabPanel ID="Tab_Datos_Presidencia" HeaderText="Presidencia" runat="server">
                        <ContentTemplate>
                            <center>
                                <table style="width:100%;">
                                   <tr style="display:none;"> 
                                        <td style="text-align:left;width:20%;" colspan="1">
                                            *Area
                                        </td>
                                        <td style="text-align:left;width:80%;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Areas_Empleado" runat="server" TabIndex="31" Width="100%"/>
                                        </td>
                                    </tr>
                                    <tr style="display:none;"> 
                                        <td style="text-align:left;width:20%;" colspan="1">
                                            *Tipo de Contrato
                                        </td>
                                        <td style="text-align:left;width:80%;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Tipo_Contrato" runat="server" TabIndex="32" Width="100%"  
                                                onmouseover="this.title=$('select[id$=Cmb_Tipo_Contrato]').text();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:left;width:20%;" colspan="1">
                                            *Escolaridad
                                        </td>
                                        <td style="text-align:left;width:80%;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Escolaridad" runat="server" TabIndex="34" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:left;width:20%;" colspan="1">
                                            Sindicato
                                        </td>
                                        <td style="text-align:left;width:80%;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Sindicato" runat="server" TabIndex="35" Width="100%"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:left;width:20%;" colspan="1">
                                            *Turno
                                        </td>
                                        <td style="text-align:left;width:80%;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Turno" runat="server" TabIndex="36" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:left;width:20%;" colspan="1">
                                           *Zona 
                                        </td>
                                        <td style="text-align:left;width:80%;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Zona" runat="server" TabIndex="37" Width="100%"/>
                                        </td>
                                   </tr>
                                   <tr style="display:none;"> 
                                        <td style="text-align:left;width:20%;" colspan="1">
                                            *Tipo de Trabajador
                                        </td>
                                        <td style="text-align:left;width:80%;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Tipo_Trabajador" runat="server" TabIndex="38" Width="100%" />
                                        </td>
                                   </tr>
                                   <tr>
                                        <td style="text-align:left;width:20%;" colspan="1">
                                            Terceros
                                        </td>
                                        <td style="text-align:left;width:80%;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Terceros" runat="server" TabIndex="39" Width="100%" />
                                        </td> 
                                   </tr>        
                                   <tr style="display:none;"> 
                                        <td style="text-align:left;width:20%;" colspan="1">
                                            Cuenta Contable
                                        </td>
                                        <td style="text-align:left;width:80%;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Cuentas_Contables" runat="server" TabIndex="39" Width="100%" />
                                        </td> 
                                   </tr>
                                </table>
                            </center>    
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="Tab_Recursos_Humanos" HeaderText="RH" runat="server">
                        <HeaderTemplate>
                            RH
                        </HeaderTemplate>
                        <ContentTemplate>
                             <table style="width:100%;">
                                <tr>
                                    <td colspan="4" style="width:100%;">
                                        <table style="width:100%; border-style:outset; color:White; background-color: Silver;">
                                            <tr>
                                                <td style="text-align:left; width:50%; color:Black; font-weight:bold; font-family:Comic Sans MS;">
                                                    ¿El empleado aplicara para el cálculo de ISSEG? 
                                                    <asp:CheckBox ID="Chk_Aplica_Esquema_ISSEG" runat="server"/>
                                                </td>
                                                <td style="text-align:center; width:50%; font-size:10px; font-weight:bold; color:Black;">
                                                    
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>    
                                    <td style="text-align:left;width:20%;">
                                        *No de I.M.S.S
                                    </td>
                                    <td style="text-align:left;width:30%;">  
                                        <asp:TextBox ID="Txt_No_IMSS" runat="server" Width="98%" onkeyup='this.value = this.value.toUpperCase();'
                                            TabIndex="36" MaxLength="20"/> 
                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_IMSS" runat="server"
                                            TargetControlID="Txt_No_IMSS" FilterType="Numbers, UppercaseLetters, LowercaseLetters" 
                                            ValidChars=" " Enabled="True"/>                                      
                                    </td>          
                                    <td style="text-align:left;width:20%;">
                                        &nbsp;&nbsp;Reloj Checador
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:DropDownList ID="Cmb_Reloj_Checador" runat="server" Width="100%"
                                            TabIndex="37">
                                            <asp:ListItem>&lt;- Seleccione -&gt;</asp:ListItem>
                                            <asp:ListItem>SI</asp:ListItem>
                                            <asp:ListItem>NO</asp:ListItem>
                                        </asp:DropDownList>                                          
                                    </td>                                                                                                                                                                        
                                </tr>
                                <tr>    
                                    <td style="text-align:left;width:20%;">
                                        *Tipo de Nomina
                                    </td>
                                    <td style="text-align:left;width:30%;" colspan="3">  
                                        <asp:DropDownList ID="Cmb_Tipo_Nomina" runat="server" Width="100%" AutoPostBack="true" 
                                            OnSelectedIndexChanged="Cmb_Tipo_Nomina_SelectedIndexChanged" TabIndex="38">
                                            <asp:ListItem>&lt;- Seleccione -&gt;</asp:ListItem>
                                            <asp:ListItem>SEMANAL</asp:ListItem>
                                            <asp:ListItem>CATORCENAL</asp:ListItem>
                                            <asp:ListItem>QUINCENAL</asp:ListItem>
                                            <asp:ListItem>MENSUAL</asp:ListItem>
                                        </asp:DropDownList>                                           
                                    </td>                                                                                                                                                                                                
                                </tr>
                                <tr>    
                                    <td style="text-align:left;width:20%;">
                                        *No Tarjeta
                                    </td>
                                    <td style="text-align:left;width:30%;">  
                                        <asp:TextBox ID="Txt_No_Tarjeta" runat="server" Width="98%" onkeyup='this.value = this.value.toUpperCase();'
                                            TabIndex="39" MaxLength="30"/> 
                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Tarjeta" runat="server"
                                            TargetControlID="Txt_No_Tarjeta" FilterType="Numbers, UppercaseLetters, LowercaseLetters" 
                                            ValidChars=" " Enabled="True"/>                                                                                
                                    </td>             
                                    <td style="text-align:left;width:20%;">
                                       &nbsp;&nbsp;Forma de Pago 
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:DropDownList ID="Cmb_Forma_Pago" runat="server" Width="100%" TabIndex="44"/>                       
                                    </td>                                                                                                                                                                     
                                </tr>
                                <tr>    
                                    <td style="text-align:left;width:20%;">
                                        *Banco
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:DropDownList ID="Cmb_Bancos" runat="server" Width="100%" TabIndex="41"/>                                                   
                                    </td>
                                    <td style="text-align:left;width:20%;">
                                        &nbsp;&nbsp;No Cuenta Bancaria
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Cuenta_Bancaria" runat="server" Width="98%" 
                                            onkeyup='this.value = this.value.toUpperCase();' TabIndex="42" MaxLength="20"/>
                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cuenta_Bancaria" runat="server"
                                            TargetControlID="Txt_Cuenta_Bancaria" FilterType="Numbers, UppercaseLetters, LowercaseLetters" 
                                            ValidChars=" " Enabled="True"/>                                           
                                    </td>                                                                                                                                                
                                </tr>
                                <tr>    
                                    <td style="text-align:left;width:20%;">
                                        *Fecha de Inicio
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="85%" TabIndex="43" onblur='this.value = this.value.toLowerCase();'/>
                                         <cc1:CalendarExtender ID="Txt_Fecha_Nacimiento_CalendarExtender" 
                                            runat="server" TargetControlID="Txt_Fecha_Inicio" 
                                            Format="dd/MMM/yyyy"  PopupButtonID="Btn_Txt_Fecha_Inicio"  />
                                         <asp:ImageButton ID="Btn_Txt_Fecha_Inicio" runat="server" Width="10%" 
                                            ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                            Height="18px" CausesValidation="false"/>      
                                        <cc1:MaskedEditExtender 
                                            ID="Mee_Txt_Fecha_Inicio" 
                                            Mask="99/LLL/9999" 
                                            runat="server"
                                            MaskType="None" 
                                            UserDateFormat="DayMonthYear" 
                                            UserTimeFormat="None" Filtered="/"
                                            TargetControlID="Txt_Fecha_Inicio" 
                                            Enabled="True" 
                                            ClearMaskOnLostFocus="false"/>  
                                        <cc1:MaskedEditValidator 
                                            ID="Mev_Txt_Fecha_Inicio" 
                                            runat="server" 
                                            ControlToValidate="Txt_Fecha_Inicio"
                                            ControlExtender="Mee_Txt_Fecha_Inicio" 
                                            EmptyValueMessage="Fecha Requerida"
                                            InvalidValueMessage="Fecha Inicio Invalida" 
                                            IsValidEmpty="false" 
                                            TooltipMessage="Ingrese o Seleccione la Fecha Inicio"
                                            Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                                                                                             
                                    </td>
                                    <td style="text-align:left;width:20%;">
                                        &nbsp;&nbsp;Fecha de Termino
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Fecha_Termino" runat="server" Width="85%" TabIndex="44" onblur='this.value = this.value.toLowerCase();'/>
                                         <cc1:CalendarExtender ID="Txt_Fecha_Termino_CalendarExtender" 
                                            runat="server" TargetControlID="Txt_Fecha_Termino" 
                                            Format="dd/MMM/yyyy"  PopupButtonID="Btn_Txt_Fecha_Termino_CalendarExtender"  />
                                         <asp:ImageButton ID="Btn_Txt_Fecha_Termino_CalendarExtender" runat="server" Width="10%" 
                                            ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                            Height="18px" CausesValidation="false"/>
                                        <cc1:MaskedEditExtender 
                                            ID="Mee_Txt_Fecha_Termino" 
                                            Mask="99/LLL/9999" 
                                            runat="server"
                                            MaskType="None" 
                                            UserDateFormat="DayMonthYear" 
                                            UserTimeFormat="None" Filtered="/"
                                            TargetControlID="Txt_Fecha_Termino" 
                                            Enabled="True" 
                                            ClearMaskOnLostFocus="false"/>  
                                        <cc1:MaskedEditValidator 
                                            ID="Mev_Txt_Fecha_Termino" 
                                            runat="server" 
                                            ControlToValidate="Txt_Fecha_Termino"
                                            ControlExtender="Mee_Txt_Fecha_Termino" 
                                            EmptyValueMessage="Fecha Requerida"
                                            InvalidValueMessage="Fecha Termino Invalida" 
                                            IsValidEmpty="true" 
                                            TooltipMessage="Ingrese o Seleccione la Fecha Termino"
                                            Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                                                                                                      
                                    </td>                                                                                                                                                
                                </tr>
                                <tr>    
                                    <td style="text-align:left;width:20%;">
                                        Tipo de Finiquito
                                    </td>
                                    <td style="text-align:left;width:30%;">  
                                        <asp:DropDownList ID="Cmb_Tipo_Finiquito" runat="server" Width="100%" TabIndex="45"/>                                                                                                          
                                    </td>
                                    <td style="text-align:left;width:20%;">
                                        &nbsp;&nbsp;Fecha de Baja
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Fecha_Baja" runat="server" Width="85%" onblur='this.value = this.value.toLowerCase();'/>
                                         <cc1:CalendarExtender ID="Ce_Txt_Fecha_Baja" 
                                            runat="server" TargetControlID="Txt_Fecha_Baja" 
                                            Format="dd/MMM/yyyy"  PopupButtonID="Btn_Fecha_Baja"  />
                                         <asp:ImageButton ID="Btn_Fecha_Baja" runat="server" Width="10%" 
                                            ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                            Height="18px" CausesValidation="false"/> 
                                        <cc1:MaskedEditExtender 
                                            ID="Mee_Txt_Fecha_Baja" 
                                            Mask="99/LLL/9999" 
                                            runat="server"
                                            MaskType="None" 
                                            UserDateFormat="DayMonthYear" 
                                            UserTimeFormat="None" Filtered="/"
                                            TargetControlID="Txt_Fecha_Baja" 
                                            Enabled="True" 
                                            ClearMaskOnLostFocus="false"/>  
                                        <cc1:MaskedEditValidator 
                                            ID="Mev_Mee_Txt_Fecha_Baja" 
                                            runat="server" 
                                            ControlToValidate="Txt_Fecha_Baja"
                                            ControlExtender="Mee_Txt_Fecha_Baja" 
                                            EmptyValueMessage=""
                                            InvalidValueMessage="Fecha Baja Invalida" 
                                            IsValidEmpty="true" 
                                            TooltipMessage="Ingrese o Seleccione la Fecha Baja"
                                            Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                                                                                                  
                                    </td>                                                                                                                                                
                                </tr>   
                                <tr>    
                                    <td style="text-align:left;width:20%;">
                                        Salario Diario
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Salario_Diario" runat="server" Width="81%" TabIndex="47" onblur="$('input[id$=Txt_Salario_Diario]').formatCurrency({colorize:true, region: 'es-MX'});"/>
                                        <asp:ImageButton ID="Btn_Actualizar_Salario" runat="server" ImageUrl="../imagenes/paginas/Sias_Actualizar.png"
                                            OnClick="Btn_Actualizar_Salario_Click" Height="24px" Width="24px" ToolTip="Actualizar el Salario Diario del Empleado"/>
                                        <%--<cc1:MaskedEditExtender ID="MEE_Txt_Salario_Diario" runat="server" 
                                            TargetControlID="Txt_Salario_Diario"                                          
                                            Mask="9,999,999.99"  
                                            MessageValidatorTip="true"    
                                            OnFocusCssClass="MaskedEditFocus"    
                                            OnInvalidCssClass="MaskedEditError"  
                                            MaskType="Number"    
                                            InputDirection="RightToLeft"    
                                            AcceptNegative="Left"    
                                            DisplayMoney="Left"  
                                            ErrorTooltipEnabled="True"  
                                            AutoComplete="true"
                                            AutoCompleteValue="0"
                                            ClearTextOnInvalid ="true"
                                            /> 
                                        <cc1:MaskedEditValidator
                                            ID="MEV_Txt_Salario_Diario" 
                                            runat="server"
                                            ControlExtender="MEE_Txt_Salario_Diario"
                                            ControlToValidate="Txt_Salario_Diario" 
                                            IsValidEmpty="true" 
                                            MaximumValue="1000000" 
                                            EmptyValueMessage="El salario diario no puede ser $0.00 Pestaña 3/5"
                                            InvalidValueMessage="Formato del salario diario es inválido. Pestaña 3/5"
                                            MaximumValueMessage="Cantidad > $1,000,000.00"
                                            MinimumValueMessage="Cantidad < $0.00"
                                            MinimumValue="0" 
                                            EmptyValueBlurredText="Cantidad Requerida" 
                                            InvalidValueBlurredMessage="Formato Incorrecto" 
                                            MaximumValueBlurredMessage="Cantidad > $1,000,000.00" 
                                            MinimumValueBlurredText="Cantidad < $0.00"
                                            Display="Dynamic" 
                                            TooltipMessage="Monto entre $0.00 y $1,000,000.00"
                                            style="font-size:9px;"/>--%>                                                    
                                    </td>
                                    <td style="text-align:left;width:20%;">
                                        &nbsp;&nbsp;Salario D. Integrado
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Salario_Diario_Integrado" runat="server" Width="98%" TabIndex="49" onblur="$('input[id$=Txt_Salario_Diario_Integrado]').formatCurrency({colorize:true, region: 'es-MX'});"/>
                                        <%--<cc1:MaskedEditExtender ID="MEE_Txt_Salario_Diario_Integrado" runat="server" 
                                            TargetControlID="Txt_Salario_Diario_Integrado"                                          
                                            Mask="9,999,999.99"  
                                            MessageValidatorTip="true"    
                                            OnFocusCssClass="MaskedEditFocus"    
                                            OnInvalidCssClass="MaskedEditError"  
                                            MaskType="Number"    
                                            InputDirection="RightToLeft"    
                                            AcceptNegative="Left"    
                                            DisplayMoney="Left"  
                                            ErrorTooltipEnabled="True"  
                                            AutoComplete="true"
                                            AutoCompleteValue="0"
                                            ClearTextOnInvalid ="true"
                                            /> 
                                        <cc1:MaskedEditValidator
                                            ID="MEV_Txt_Salario_Diario_Integrado" 
                                            runat="server"
                                            ControlExtender="MEE_Txt_Salario_Diario_Integrado"
                                            ControlToValidate="Txt_Salario_Diario_Integrado" 
                                            IsValidEmpty="true" 
                                            MaximumValue="1000000" 
                                            EmptyValueMessage="El salario diario integrado no puede ser $0.00 Pestaña 3/5"
                                            InvalidValueMessage="Formato del salario diario integrado es inválido. Pestaña 3/5"
                                            MaximumValueMessage="Cantidad > $1,000,000.00"
                                            MinimumValueMessage="Cantidad < $0.00"
                                            MinimumValue="0" 
                                            EmptyValueBlurredText="Cantidad Requerida" 
                                            InvalidValueBlurredMessage="Formato Incorrecto" 
                                            MaximumValueBlurredMessage="Cantidad > $1,000,000.00" 
                                            MinimumValueBlurredText="Cantidad < $0.00"
                                            Display="Dynamic" 
                                            TooltipMessage="Monto entre $0.00 y $1,000,000.00"
                                            style="font-size:9px;"/>--%>                                                         
                                    </td>                                                                                                                                                
                                </tr> 
                                <tr>    
                                    <td style="text-align:left;width:20%;">
                                        No Licencia Manejo
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                       <asp:TextBox ID="Txt_No_Licencia" runat="server" Width="98%" onkeyup='this.value = this.value.toUpperCase();'
                                            TabIndex="50" MaxLength="20"/> 
                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Licencia" runat="server"
                                            TargetControlID="Txt_No_Licencia" FilterType="Numbers, UppercaseLetters, LowercaseLetters" 
                                            ValidChars=" " Enabled="True"/>                                            
                                    </td>
                                    <td style="text-align:left;width:20%;">
                                       &nbsp;&nbsp;*Fecha Vencimiento 
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Fecha_Vencimiento_Licencia" runat="server" Width="85%" TabIndex="51" onblur='this.value = this.value.toLowerCase();'/>                                      
                                         <cc1:CalendarExtender ID="Ce_Txt_Fecha_Vencimiento_Licencia" 
                                            runat="server" TargetControlID="Txt_Fecha_Vencimiento_Licencia" 
                                            Format="dd/MMM/yyyy"  PopupButtonID="Btn_Fecha_Vencimiento_Licencia"  />
                                         <asp:ImageButton ID="Btn_Fecha_Vencimiento_Licencia" runat="server" Width="10%" 
                                            ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                            Height="18px" CausesValidation="false"/> 
                                        <cc1:MaskedEditExtender 
                                            ID="Mee_Txt_Fecha_Vencimiento_Licencia" 
                                            Mask="99/LLL/9999" 
                                            runat="server"
                                            MaskType="None" 
                                            UserDateFormat="DayMonthYear" 
                                            UserTimeFormat="None" Filtered="/"
                                            TargetControlID="Txt_Fecha_Vencimiento_Licencia" 
                                            Enabled="True" 
                                            ClearMaskOnLostFocus="false"/>  
                                        <cc1:MaskedEditValidator 
                                            ID="Mev_Txt_Fecha_Vencimiento_Licencia" 
                                            runat="server" 
                                            ControlToValidate="Txt_Fecha_Vencimiento_Licencia"
                                            ControlExtender="Mee_Txt_Fecha_Vencimiento_Licencia" 
                                            EmptyValueMessage="Fecha Requerida"
                                            InvalidValueMessage="Fecha Vencimiento Licencia Invalida" 
                                            IsValidEmpty="true" 
                                            TooltipMessage="Ingrese o Seleccione la Fecha Vencimiento de Licencia"
                                            Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                                            
                                    </td>                                                                                                                                                
                                </tr>                                      
                                <tr>    
                                    <td style="text-align:left;width:20%;">
                                        No Seguro Poliza
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_No_Seguro_Poliza_Empleado" MaxLength="20" runat="server" Width="98%" 
                                            onkeyup='this.value = this.value.toUpperCase();' TabIndex="52"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Seguro_Poliza_Empleado" runat="server" ValidChars=" " Enabled="True" 
                                            TargetControlID="Txt_No_Seguro_Poliza_Empleado" FilterType="Numbers, UppercaseLetters, LowercaseLetters" />                                                                                 
                                    </td>
                                    <td style="text-align:left;width:20%;">
                                        &nbsp;&nbsp;Nombre Beneficiario
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Beneficiario_Empleado" MaxLength="100" runat="server" Width="98%" 
                                            onkeyup='this.value = this.value.toUpperCase();' TabIndex="53"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Beneficiario_Empleado" runat="server" ValidChars=" " Enabled="True"
                                            TargetControlID="Txt_Beneficiario_Empleado" FilterType="Custom, UppercaseLetters, LowercaseLetters" />                                     
                                    </td>                                                                                                                                               
                                </tr>                
                                <tr>    
                                    <td style="text-align:left;width:20%;">
                                        
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                                                                             
                                    </td>
                                    <td style="text-align:left;width:20%;">
                                        
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                                                                             
                                    </td>                                                                                                                                                
                                </tr>                                                                                                                                                                                                                                                                                                                                                                                     
                             </table>   
                        </ContentTemplate>
                    </cc1:TabPanel>     
                    <cc1:TabPanel  ID="Tab_Dias_Trabaja" HeaderText="Dias Trabaja" runat="server">
                        <HeaderTemplate>
                            Dias Laborales
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" border="0">
                                <tr style="background:#2F4E7D url(../imagenes/paginas/titleBackground.png) repeat-x top;color:White;font-weight:bold;">
                                    <td style="text-align:center;width:14px;">
                                        Lunes
                                        <asp:CheckBox ID="Chk_Lunes" runat="server" />
                                    </td>
                                    <td style="text-align:center;width:14px">
                                        Martes
                                        <asp:CheckBox ID="Chk_Martes" runat="server" />
                                    </td>
                                    <td style="text-align:center;width:14px">
                                        Miercoles
                                        <asp:CheckBox ID="Chk_Miercoles" runat="server" />
                                    </td>
                                    <td style="text-align:center;width:14px">
                                        Jueves
                                        <asp:CheckBox ID="Chk_Jueves" runat="server" />
                                    </td> 
                                    <td style="text-align:center;width:14px">
                                        Viernes
                                        <asp:CheckBox ID="Chk_Viernes" runat="server" />
                                    </td> 
                                    <td style="text-align:center;width:14px">
                                        Sabado
                                        <asp:CheckBox ID="Chk_Sabado" runat="server" />
                                    </td> 
                                    <td style="text-align:center;width:14px">
                                        Domingo
                                        <asp:CheckBox ID="Chk_Domingo" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>                                                                                          
                    <cc1:TabPanel ID="Tab_Doc_Empleado" HeaderText="Documentos" runat="server">
                        <HeaderTemplate>
                            Requisitos
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table style="width:100%;">
                                <tr>    
                                    <td style="width:100%;">  
                                        <asp:GridView ID="Grid_Documentos_Empleado" runat="server" CssClass="GridView_1" 
                                            AutoGenerateColumns="False" GridLines="None" Width="100%"
                                            OnRowDataBound="Grid_Documentos_Empleado_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="REQUISITO_ID" HeaderText="Clave" 
                                                    Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" 
                                                    Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" Width="30%"  Wrap="true"/>
                                                    <ItemStyle HorizontalAlign="Left" Width="30%"  Wrap="true" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Cargar Archivos">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Throbber" Text="wait" runat="server"  Width="30px">                                                                     
                                                            <div id="progressBackgroundFilter" class="progressTemplate"></div>
                                                            <div  class="processMessage" id="div_progress">
                                                                <img alt="" src="../Imagenes/paginas/Updating.gif" />
                                                            </div>
                                                        </asp:Label>
                                                        <cc1:AsyncFileUpload ID="Async_Requisito_Empleado" runat="server" Width="300px" ThrobberID="Throbber" 
                                                            ForeColor="White" Font-Bold="true"  CompleteBackColor="LightBlue" ErrorBackColor="Red" 
                                                            UploadingBackColor="LightGray" PersistedStoreType="Session"
                                                            />                                                                       
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                                    <HeaderStyle  HorizontalAlign="Left"/>
                                                    <ItemStyle HorizontalAlign="Left" Width="40%" />                                                                                                                                
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Entrego">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Chk_Requisito_Empleado" runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="10%"/>                                                                
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ARCHIVO" HeaderText="Archivo" 
                                                    Visible="True">
                                                    <HeaderStyle HorizontalAlign="Center" Width="10%"/>
                                                    <ItemStyle HorizontalAlign="Center" Width="10%"/>
                                                </asp:BoundField>
                                            </Columns>
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <HeaderStyle  CssClass="GridHeader"/>
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </td>
                                </tr>                                            
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel> 
                    <cc1:TabPanel ID="Tab_Percepciones_Tipo_Nomina" HeaderText="Percepciones" runat="server">
                        <HeaderTemplate>Percepciones</HeaderTemplate>
                        <ContentTemplate>
                            <center><b>Percepciones Tipo Nomina</b></center>
                            <table width="100%">
                                <tr style="width:100%;">
                                    <td style="width:20%;" >
                                        Percepciones
                                    </td>
                                    <td style="width:50%;">
                                        <asp:DropDownList ID="Cmb_Percepciones_All" runat="server" Width="100%"/>
                                    </td>   
                                    <td style="width:30%;">
                                        <asp:Button ID="Btn_Agregar_Percepcion" runat="server" Text="Agregar Percepción" CssClass="button_autorizar"
                                            OnClick="Btn_Agregar_Percepcion_Click" Width="100%"/>
                                    </td>                                           
                                </tr>
                            </table>
                            <br />
                            <div style="overflow:auto;height:250px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">                                
                                <asp:GridView ID="Grid_Tipo_Nomina_Percepciones" runat="server" CssClass="GridView_1"
                                     AutoGenerateColumns="False"  GridLines="None" 
                                     OnRowDataBound="Grid_Tipo_Nomina_Percepciones_RowDataBound">
                                        <Columns>                                              
                                            <asp:TemplateField HeaderText="Aplica" ControlStyle-BackColor="ActiveBorder">
                                                <ItemTemplate>
                                                        <asp:CheckBox ID="Chk_Aplica" runat="server" Checked="false" AutoPostBack="true" OnCheckedChanged="Chk_Aplica_Percepciones_CheckedChanged"/>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />                                                    
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PERCEPCION_DEDUCCION_ID" HeaderText="Clabe">
                                                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                                <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                <ItemStyle HorizontalAlign="Left" Width="30%" />
                                            </asp:BoundField>                 
                                            <asp:BoundField DataField="TIPO_ASIGNACION" HeaderText="Tipo">
                                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>                                             
                                            <asp:BoundField DataField="CANTIDAD" HeaderText="$ Tipo Nomina" DataFormatString="{0:c}">
                                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>                                             
                                           <asp:TemplateField HeaderText="$ Empleado">
                                                <ItemTemplate>
                                                          <asp:TextBox ID="Txt_Cantidad_Percepcion" runat="server" Width="70%" />
                                                            <cc1:MaskedEditExtender ID="MEE_Txt_Cantidad_Percepcion" runat="server" 
                                                                                    TargetControlID="Txt_Cantidad_Percepcion"                                          
                                                                                    Mask="9,999,999.99"  
                                                                                    MessageValidatorTip="true"    
                                                                                    OnFocusCssClass="MaskedEditFocus"    
                                                                                    OnInvalidCssClass="MaskedEditError"  
                                                                                    MaskType="Number"    
                                                                                    InputDirection="RightToLeft"    
                                                                                    AcceptNegative="Left"    
                                                                                    DisplayMoney="Left"  
                                                                                    ErrorTooltipEnabled="True"  
                                                                                    AutoComplete="true"
                                                                                    AutoCompleteValue="0"
                                                                                    ClearTextOnInvalid ="true"
                                                                                    />                                                                                                                                                                                                                            
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                <ItemStyle HorizontalAlign="Left" Width="20%" />                                                                                   
                                           </asp:TemplateField>    
                                            <asp:TemplateField HeaderText="Eliminar">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Btn_Quitar_Percepcion" runat="server" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png"  CommandArgument='<%# Eval("PERCEPCION_DEDUCCION_ID") %>'
                                                       OnClick="Btn_Quitar_Percepcion_Click" OnClientClick="return confirm('¿Está seguro de eliminar la percepcion seleccionado?');"/>                                                    
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                <ItemStyle HorizontalAlign="Center" Width="20%" />                                                        
                                           </asp:TemplateField>                                                                                                                                                                                                                                                                      
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView> 
                             </div>                                 
                        </ContentTemplate>
                    </cc1:TabPanel>       
                    <cc1:TabPanel ID="Tab_Deducciones_Tipo_Nomina" HeaderText="Deducciones" runat="server">
                        <HeaderTemplate>Deducciones</HeaderTemplate>
                        <ContentTemplate>
                            <center><b>Deducciones Tipo Nomina</b></center>
                            <table width="100%">
                                <tr style="width:100%;">
                                    <td style="width:20%;" >
                                        Deducciones
                                    </td>
                                    <td style="width:50%;">
                                        <asp:DropDownList ID="Cmb_Deducciones_All" runat="server" Width="100%"/>
                                    </td>   
                                    <td style="width:30%;">
                                        <asp:Button ID="Btn_Agregar_Deduccion" runat="server" Text="Agregar Deducción" CssClass="button_autorizar"
                                            OnClick="Btn_Agregar_Deduccion_Click" Width="100%"/>
                                    </td>                                           
                                </tr>
                            </table>                            
                            <br />
                            <div style="overflow:auto;height:250px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" >
                                <asp:GridView ID="Grid_Tipo_Nomina_Deducciones" runat="server" CssClass="GridView_1"
                                     AutoGenerateColumns="False"  GridLines="None" 
                                     OnRowDataBound="Grid_Tipo_Nomina_Deduccion_RowDataBound">
                                        <Columns>                                              
                                            <asp:TemplateField HeaderText="Aplica" ControlStyle-BackColor="ActiveBorder">
                                                <ItemTemplate>
                                                        <asp:CheckBox ID="Chk_Aplica" runat="server" Checked="false" AutoPostBack="true" OnCheckedChanged="Chk_Aplica_Deducciones_CheckedChanged"/>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />                                                    
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PERCEPCION_DEDUCCION_ID" HeaderText="Clabe">
                                                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                                <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                <ItemStyle HorizontalAlign="Left" Width="30%" />
                                            </asp:BoundField>    
                                            <asp:BoundField DataField="TIPO_ASIGNACION" HeaderText="Tipo">
                                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>                                             
                                            <asp:BoundField DataField="CANTIDAD" HeaderText="$ Tipo Nomina" DataFormatString="{0:c}">
                                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>                                             
                                           <asp:TemplateField HeaderText="$ Empleado">
                                                <ItemTemplate>
                                                          <asp:TextBox ID="Txt_Cantidad_Deduccion" runat="server" Width="70%" />
                                                            <cc1:MaskedEditExtender ID="MEE_Txt_Cantidad_Deduccion" runat="server" 
                                                                                    TargetControlID="Txt_Cantidad_Deduccion"                                          
                                                                                    Mask="9,999,999.99"  
                                                                                    MessageValidatorTip="true"    
                                                                                    OnFocusCssClass="MaskedEditFocus"    
                                                                                    OnInvalidCssClass="MaskedEditError"  
                                                                                    MaskType="Number"    
                                                                                    InputDirection="RightToLeft"    
                                                                                    AcceptNegative="Left"    
                                                                                    DisplayMoney="Left"  
                                                                                    ErrorTooltipEnabled="True"  
                                                                                    AutoComplete="true"
                                                                                    AutoCompleteValue="0"
                                                                                    ClearTextOnInvalid ="true"
                                                                                    />                                                                                                                                                                                                                            
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                <ItemStyle HorizontalAlign="Left" Width="20%" />                                                                                   
                                                </asp:TemplateField>       
                                                <asp:TemplateField HeaderText="Eliminar">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Btn_Quitar_Deduccion" runat="server" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png"  CommandArgument='<%# Eval("PERCEPCION_DEDUCCION_ID") %>'
                                                           OnClick="Btn_Quitar_Deduccion_Click" OnClientClick="return confirm('¿Está seguro de eliminar la deducción seleccionado?');"/>                                                    
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />                                                        
                                               </asp:TemplateField>                                                                                                                                                                                                                                                                                                          
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>              
                            </div>                  
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
                
                <br />
                <hr style="width:97%;"/>
                <div style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style: outset;color:White;" >
                    <asp:GridView ID="Grid_Empleados" runat="server" CssClass="GridView_1" 
                        AutoGenerateColumns="False" GridLines="None" Width="96.5%"
                        onpageindexchanging="Grid_Empleados_PageIndexChanging"  
                        onselectedindexchanged="Grid_Empleados_SelectedIndexChanged"
                        AllowSorting="True" OnSorting="Grid_Empleados_Sorting" HeaderStyle-CssClass="tblHead">
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
                            <asp:BoundField DataField="RFC" HeaderText="RFC" SortExpression="RFC">
                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Empleado" HeaderText="Nombre" 
                                Visible="True" SortExpression="Empleado">
                                <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                <ItemStyle HorizontalAlign="left" Width="60%" />
                            </asp:BoundField>                                    
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus">
                                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                            </asp:BoundField>
                        </Columns>
                        <SelectedRowStyle CssClass="GridSelected" />
                        <PagerStyle CssClass="GridHeader" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                    </asp:GridView>
                </div>                               
                                                                 
            <cc1:ModalPopupExtender ID="Mpe_Baja_Empleado" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="MPE_Baja"
                PopupControlID="Pnl_Baja_Empleado" TargetControlID="Btn_Comodin_Cancelacion_Open" PopupDragHandleControlID="Pnl_Cabecera" 
                CancelControlID="Btn_Comodin_Cancelacion_Close" DropShadow="True" DynamicServicePath="" Enabled="True" />  
            <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Cancelacion_Open" runat="server" Text="" />            
            <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Cancelacion_Close" runat="server" Text="" />  
             
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Btn_Nuevo" EventName="Click"/>
                <asp:AsyncPostBackTrigger ControlID="Btn_Modificar" EventName="Click"/>
                <asp:AsyncPostBackTrigger ControlID="Btn_Eliminar_Mostrar_Popup" EventName="Click"/>
                <asp:AsyncPostBackTrigger ControlID="Btn_Salir" EventName="Click"/>
                <asp:AsyncPostBackTrigger ControlID="Btn_Busqueda_Empleados" EventName="Click"/>
                <asp:AsyncPostBackTrigger ControlID="Btn_Eliminar" EventName="Click"/>
            </Triggers>
        </asp:UpdatePanel>                   
                                                              
        </ContentTemplate>        
    </asp:UpdatePanel>

                               
<asp:Panel ID="Pnl_Baja_Empleado" runat="server" CssClass="drag" HorizontalAlign="Center" Width="650px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Cabecera" runat="server" 
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Img_Inf_Baja_Empleado" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Baja de Empleados
                </td>
                <td align="right" style="width:10%;">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana_Cancelacion" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
    </asp:Panel> 
        <asp:UpdatePanel ID="Up_Popup_Baja" runat="server" UpdateMode="Conditional">
            <ContentTemplate>       
            
                <asp:UpdateProgress ID="Up_Baja_Empleado" runat="server" AssociatedUpdatePanelID="Up_Popup_Baja" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress"><img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                    </ProgressTemplate>
                </asp:UpdateProgress>     
                                                                                         
                <div style="cursor:default;width:100%">
                <table width="100%">
                    <tr>
                        <td style="width:13%;vertical-align:top;top:20px;" align="center">
                            <br />
                            <asp:Image ID="Img_Empleado_Eliminar" runat="server"  Width="75px" Height="90px"
                                         ImageUrl="~/paginas/imagenes/paginas/Sias_No_Disponible.JPG" BorderWidth="1px"/> 
                        </td>
                        <td style="width:87%;">        
                            <table width="100%" style="background-color:#ffffff;">       
                                <tr>
                                    <td style="width:100%" colspan="4" align="left">
                                        
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
                                        <asp:TextBox ID="Txt_Baja_No_Empleado" runat="server" Width="98%" MaxLength="10" TabIndex="11"
                                            Enabled="false"/>
                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Baja_No_Empleado" 
                                             runat="server" TargetControlID="Txt_Baja_No_Empleado" FilterType="Numbers"/>                          
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
                                        <asp:TextBox ID="Txt_Baja_Nombre_Empleado" runat="server" Width="98%" TabIndex="13" onkeyup='this.value = this.value.toUpperCase();'
                                            Enabled="false"/>
                                    </td>                                                            
                                </tr>                        
                                <tr>
                                   <td style="text-align:left;width:20%;vertical-align:top;">
                                       *Motivo Baja
                                   </td>
                                   <td  style="text-align:left;width:80%;" colspan="3">
                                        <asp:TextBox ID="Txt_Motivo_Baja" runat="server" Width="99%" MaxLength="100" TextMode="MultiLine" Enabled="true" onkeyup='this.value = this.value.toUpperCase();'/>
                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Motivo_Baja" runat="server"  
                                            TargetControlID="Txt_Motivo_Baja" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" 
                                            ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>    
                                        <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Motivo_Baja" runat="server" 
                                            TargetControlID ="Txt_Motivo_Baja" WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/>                                                   
                                  </td>
                               </tr>   
                               <tr>
                                    <td style="width:100%" colspan="4">
                                        <hr />
                                    </td>
                                </tr>                                   
                               <tr>
                                    <td style="width:100%" colspan="4" align="right">
                                        <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                            style="border-style:none;background-color:White;font-weight:bold;cursor:hand;"  TabIndex="16" 
                                            OnClick="Btn_Eliminar_Click" CausesValidation="false"/>   
                                    </td>
                                </tr>                  
                               <tr>
                                    <td style="width:100%" colspan="4">
                                        <hr />
                                    </td>
                                </tr>                                            
                            </table>
                        </td>                
                    </tr>
                </table>            
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Btn_Eliminar_Mostrar_Popup" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>   
</asp:Panel>
 
        
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
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClick="Btn_Cerrar_Ventana_Click"/>  
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
                                           <asp:TextBox ID="Txt_Busqueda_No_Empleado" runat="server" Width="98%" MaxLength="6"/>
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
                                           C&oacute;digo Progr&aacute;matico
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;" colspan="3">
                                           <asp:TextBox ID="Txt_Busqueda_Codigo_Programatico" runat="server" Width="99%" />     
                                           <cc1:TextBoxWatermarkExtender ID="Twm_Codigo_Programatico" runat="server" 
                                                TargetControlID ="Txt_Busqueda_Codigo_Programatico" WatermarkText="Busqueda por Codigo Programatico" 
                                                WatermarkCssClass="watermarked"/>                                                                                                                                                                                    
                                        </td>                                       
                                    </tr>                                                                                                                                  
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            RFC
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Busqueda_RFC" runat="server" Width="98%" onkeyup='this.value = this.value.toUpperCase();'/>
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_RFC" runat="server" FilterType="Numbers, UppercaseLetters, LowercaseLetters"
                                                TargetControlID="Txt_Busqueda_RFC"/>  
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_RFC" runat="server" 
                                                TargetControlID ="Txt_Busqueda_RFC" WatermarkText="Busqueda por RFC" 
                                                WatermarkCssClass="watermarked"/>                                                                                                                                     
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Estatus
                                        </td>              
                                        <td style="width:30%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="100%">   
                                                <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>                                         
                                                <asp:ListItem>ACTIVO</asp:ListItem>
                                                <asp:ListItem>INACTIVO</asp:ListItem>                                   
                                            </asp:DropDownList>                                          
                                        </td>                                         
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Nombre
                                        </td>              
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Busqueda_Nombre_Empleado" runat="server" Width="99.5%" onkeyup='this.value = this.value.toUpperCase();'/>
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Nombre_Empleado" runat="server" FilterType="Custom, LowercaseLetters, Numbers, UppercaseLetters"
                                                TargetControlID="Txt_Busqueda_Nombre_Empleado" ValidChars="áéíóúÁÉÍÓÚÑñ "/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Nombre_Empleado" runat="server" 
                                                TargetControlID ="Txt_Busqueda_Nombre_Empleado" WatermarkText="Busqueda por Nombre" 
                                                WatermarkCssClass="watermarked"/>                                                                                               
                                        </td>                                         
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Rol
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;" colspan="3">
                                           <asp:DropDownList ID="Cmb_Busqueda_Rol" runat="server" Width="100%" />                                          
                                        </td> 
                                    </tr>                                      
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Unidad Responsable
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;" colspan="3">
                                           <asp:DropDownList ID="Cmb_Busqueda_Dependencia" runat="server" Width="100%" 
                                                AutoPostBack="true" OnSelectedIndexChanged="Cmb_Busqueda_Dependencia_SelectedIndexChanged"/>                                          
                                        </td> 
                                    </tr>                                    
                                    <tr style="display:none;"> 
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Tipo Contrato
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;" colspan="3">
                                           <asp:DropDownList ID="Cmb_Busqueda_Tipo_Contrato" runat="server" Width="100%" />                                          
                                        </td> 
                                    </tr>                                    
                                    <tr style="display:none;"> 
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Area
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;" colspan="3">
                                           <asp:DropDownList ID="Cmb_Busqueda_Areas" runat="server" Width="100%" />                                          
                                        </td> 
                                    </tr> 
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Puesto
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;" colspan="3">
                                           <asp:DropDownList ID="Cmb_Busqueda_Puesto" runat="server" Width="100%" />                                          
                                        </td> 
                                    </tr>                                                                                                            
                                    <tr>                                    
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Escolariadad
                                        </td>              
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:DropDownList  ID="Cmb_Busqueda_Escolaridad" runat="server" Width="100%" />                                           
                                        </td>                                         
                                    </tr>            
                                    <tr>                                    
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Sindicato
                                        </td>              
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:DropDownList  ID="Cmb_Busqueda_Sindicato" runat="server" Width="100%" />                                           
                                        </td>                                         
                                    </tr>      
                                    <tr>                                    
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Turno
                                        </td>              
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:DropDownList  ID="Cmb_Busqueda_Turno" runat="server" Width="100%" />                                           
                                        </td>                                         
                                    </tr> 
                                    <tr>                                    
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Zona
                                        </td>              
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:DropDownList  ID="Cmb_Busqueda_Zona" runat="server" Width="100%" />                                           
                                        </td>                                         
                                    </tr>
                                    <tr style="display:none;">                                     
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Tipo Trabajador
                                        </td>              
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:DropDownList  ID="Cmb_Busqueda_Tipo_Trabajador" runat="server" Width="100%" />                                           
                                        </td>                                         
                                    </tr>   
                                    <tr>                                    
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Tipo Nomina
                                        </td>              
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:DropDownList  ID="Cmb_Busqueda_Tipo_Nomina" runat="server" Width="100%" />                                           
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
                                               <asp:Button ID="Btn_Busqueda_Empleados" runat="server"  Text="Busqueda de Empleados" CssClass="button"  
                                                CausesValidation="false" OnClick="Btn_Busqueda_Empleados_Click" Width="200px"/> 
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
                    </td>
                </tr>
             </table>                                                   
           </div>                 
    </asp:Panel>

</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Incapacidades.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Incapacidades" Title="Incapacidades" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <link href="../../easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../javascript/Js_Ope_Nom_Faltas_Empleados.js" type="text/javascript"></script>

<script type="text/javascript" language="javascript">
    function isNumber(n) {   return !isNaN(parseFloat(n)) && isFinite(n); } 

    function pageLoad(sender, args) {
        $('textarea[id*=Txt_Comentarios]').keyup(function() {var Caracteres =  $(this).val().length; if (Caracteres > 250) {this.value = this.value.substring(0, 250); Caracteres =  $(this).val().length;  $(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});
        $('input[id$=Txt_No_Empleado]').focus().select();
        
       $('input[id$=Txt_No_Empleado]').bind("blur", function(){
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

            $('input[id$=Txt_No_Empleado]').bind('dblclick', function(){
                $.ajax({
                    url: "Frm_Informacion_Empleado.aspx?tabla=EMPLEADOS&opcion=consultar_empleados&no_empleado=" + $(this).val(),
                    type:'POST',
                    async: false,
                    cache: false,
                    dataType:'json',
                    success: function (Datos) {
                         if (Datos != null) {  
                            var texto = "";
                             $.each(Datos.EMPLEADOS, function (Contador, Elemento) {                                  
                                texto = texto + "Empleado: " + Elemento.NO_EMPLEADO + "<br />" +
                                        "Nombre: " + Elemento.EMPLEADO + "<br />" +
                                        "Unidad Responsable: " + Elemento.UR + "<br />" +
                                        "Salario Diario Integrado: $" + Elemento.SALARIO_DIARIO_INTEGRADO + "<br />";

                                 texto = texto + "<br /><br />";
                             });
		                        $.messager.show({
			                        title:'Información Empleado',
			                        msg:texto,
			                        showType:'fade',
			                        width:400,
			                        height:180
		                        });   
                         }
                         else {
                             alert("El empleado no existe en el sistema.");
                         }      
                    }        
                });
            });
            
            $("select[id$=Cmb_Tipo_Incapacidad]").bind('change', function(){
                switch($(this).val()){
                    case 'Enfermedad General':
                        $('input[id$=Txt_Porcentaje_Incapacidad]').attr('disabled', false);
                        $('input[id$=Txt_Porcentaje_Incapacidad]').val('');
                    break;
                    case 'Maternidad':
                        $('input[id$=Txt_Porcentaje_Incapacidad]').attr('disabled', false);
                        $('input[id$=Txt_Porcentaje_Incapacidad]').val('');
                        $('input[id$=Txt_Dias_Incapacidad]').val('');
                    break;
                    case 'Riesgo Laboral':
                        $('input[id$=Txt_Porcentaje_Incapacidad]').attr('disabled', true);
                        $('input[id$=Txt_Porcentaje_Incapacidad]').val('100');
                    break;

                    default:
                        $('input[id$=Txt_Porcentaje_Incapacidad]').val('');
                        $('input[id$=Txt_Dias_Incapacidad]').val('');                    
                    break;
                }
            });
            
            $('input[id$=Txt_Dias_Incapacidad]').keydown(function(event){
              if (event.keyCode == 13) {
                return false;
              }              
            });
            
            $('input[id$=Txt_Dias_Incapacidad]').bind('blur', function(){
                 var Tipo_Incapacidad_Seleccionado = $("select[id$=Cmb_Tipo_Incapacidad] :selected").val();
                 
                 if(Tipo_Incapacidad_Seleccionado != ""){
                    if(($(this).val() < 0 || $(this).val() > 100) != 0){
                        $(this).val('');
                    }else if($(this).val() != '') {
                        switch(Tipo_Incapacidad_Seleccionado){
                            case 'Enfermedad General':
                                if($(this).val() <= 28){
                                    //Mostrar_Msj("Los días de incapacidad por " + $("select[id$=Cmb_Tipo_Incapacidad]").val() + " son " + $('input[id$=Txt_Dias_Incapacidad]').val() + " días.");
                                }else{
                                    $('input[id$=Txt_Dias_Incapacidad]').val('');
                                    $.messager.show({
                                        title:'Infomación',
                                        msg:'Maximo de Dias por Enfermedad General es de 28',
                                        width:500,
                                        height:100,
                                        showSpeed:300,
                                        showType:'slide',
                                        timeout:5000});
                                }
                            break;
                            case 'Maternidad':
                                if($(this).val() <= 42){
                                    //Mostrar_Msj("Los días de incapacidad por " + $("select[id$=Cmb_Tipo_Incapacidad]").val() + " son " + $('input[id$=Txt_Dias_Incapacidad]').val() + " días.");
                                }else{
                                    $('input[id$=Txt_Dias_Incapacidad]').val('');
                                    $.messager.show({
                                        title:'Infomación',
                                        msg:'Los dias de incapacidad por maternidad deben de ser menor de 42 días',
                                        width:500,
                                        height:100,
                                        showSpeed:300,
                                        showType:'slide',
                                        timeout:5000});                                    
                                }                               
                            break;
                            case 'Riesgo Laboral':
                                if($(this).val() <= 28){
                                    //Mostrar_Msj("Los días de incapacidad por " + $("select[id$=Cmb_Tipo_Incapacidad]").val() + " son " + $('input[id$=Txt_Dias_Incapacidad]').val() + " días.");
                                }else{
                                    $('input[id$=Txt_Dias_Incapacidad]').val('');
                                    $.messager.show({
                                        title:'Infomación',
                                        msg:'Maximo de Dias por Riesgo Laboral es de 28',
                                        width:500,
                                        height:100,
                                        showSpeed:300,
                                        showType:'slide',
                                        timeout:5000});
                                }
                            break;
                            
                            default:
                            break;
                        }
                    }  
                 }else{
                    $('input[id$=Txt_Dias_Incapacidad]').val('');
                    $.messager.show({
                        title:'Infomación',
                        msg:'Es necesario seleccionar el tipo de incapacidad antes de ingresar los dias de incapacidad.',
                        width:500,
                        height:100,
                        showSpeed:300,
                        showType:'slide',
                        timeout:5000});
                 }            
            });  
            
            $('input[id$=Txt_Porcentaje_Incapacidad]').bind('blur', function(){
                 var Tipo_Incapacidad_Seleccionado = $("select[id$=Cmb_Tipo_Incapacidad] :selected").val();
                 
                 if(Tipo_Incapacidad_Seleccionado != ""){
                    if(($(this).val() < 0 || $(this).val() > 100) != 0){
                        $(this).val('');
                    }else if($(this).val() != '') {
                        $.messager.confirm("Información", "El porcentaje de incapacidad por " + $("select[id$=Cmb_Tipo_Incapacidad]").val() + " es de " + $('input[id$=Txt_Porcentaje_Incapacidad]').val() + " %.", function(opcion){
                            if(opcion){
                                $('input[id$=Txt_Porcentaje_Incapacidad]').val($('input[id$=Txt_Porcentaje_Incapacidad]').val());
                            }
                            else{
                                $('input[id$=Txt_Porcentaje_Incapacidad]').val('');
                            }
                        });  
                    }  
                 }else{
                    $('input[id$=Txt_Porcentaje_Incapacidad]').val('');
                    $.messager.show({
                        title:'Infomación',
                        msg:'Es necesario seleccionar el tipo de incapacidad antes de ingresar el porcentaje de incapacidad.',
                        width:500,
                        height:100,
                        showSpeed:300,
                        showType:'slide',
                        timeout:5000});
                 }            
            });            
    }

    function Aplica_Cuarto_Pago_Dia(){
        var DIAS = 0.0;

        if($('input:checkbox[id$=Chk_Aplica_Pago_Cuarto_Dia]').is(':checked')){
            DIAS = parseFloat(($('input[id$=Txt_Dias_Incapacidad]').val()=='')?'0':$('input[id$=Txt_Dias_Incapacidad]').val());
            if(DIAS < 4){
                $('input[id$=Txt_Dias_Incapacidad]').val('');
                $('input:checkbox[id$=Chk_Aplica_Pago_Cuarto_Dia]').attr("checked", false);
                alert('Si aplica pago cuarto día los días de incapacidad no pueden ser menor a 4');
            }else{
                 $('input:checkbox[id$=Chk_Aplica_Pago_Cuarto_Dia]').attr("checked", true);
            }
        }
    }
    
    function Mostrar_Msj(msj){
        $.messager.confirm("Información", msj, function(opcion){
            if(opcion){
                $('input[id$=Txt_Dias_Incapacidad]').val($('input[id$=Txt_Dias_Incapacidad]').val());
            }
            else{
                $('input[id$=Txt_Dias_Incapacidad]').val('');
            }
        });    
    }

    function _alert() { $.messager.alert("Información", "Operación Completa."); }  
    
       
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
    <cc1:ToolkitScriptManager ID="Tsm_Incapacidades" runat="server"  AsyncPostBackTimeout="36000"  EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>    
    <asp:UpdatePanel ID="UPnl_Incapacidades" runat="server">
        <ContentTemplate>  
               
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Incapacidades" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <asp:Button ID="Btn_Comodin" runat="server" style="background-color:Transparent;border-style:none;" 
                OnClientClick="javascript:return false;"/>
            
             <div id="Div_Incapacidades" style="background-color:#ffffff; width:100%; height:100%;">
            
                    <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                        <tr align="center">
                            <td class="label_titulo">Captura de Incapacidades</td>
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
                                     <div align="right" class="barra_busqueda" style="padding:0px 0px 6px 0px;">                        
                                          <table style="width:100%;height:28px;">
                                            <tr>
                                              <td align="left" style="width:59%;">                                                  
                                                   <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" 
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"  OnClick="Btn_Nuevo_Click"/>
                                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" 
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" 
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                                        OnClientClick="return confirm('¿Está seguro de eliminar la incapacidad seleccionado?');"/>
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" 
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                              </td>
                                              <td align="right" style="width:41%;">
                                                    <table style="width:100%;">
                                                        <tr>
                                                            <td style="width:95%; padding-bottom:6px;" align="right">
                                                                <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="250px" MaxLength="100" 
                                                                    onkeyup='this.value = this.value.toUpperCase();' />
                                                                <cc1:TextBoxWatermarkExtender ID="TBW_Txt_Empleados" runat="server" TargetControlID="Txt_No_Empleado"
                                                                    WatermarkCssClass="watermarked2" WatermarkText="No Empleado "/>
                                                                <cc1:FilteredTextBoxExtender ID="FTxt_No_Empleado" 
                                                                     runat="server" TargetControlID="Txt_No_Empleado" FilterType="LowercaseLetters, Numbers, UppercaseLetters, Custom" ValidChars="ÁÉÍÓÚ ñÑ.áéíóú"/>   
                                                            </td>
                                                            <td style="width:5%; vertical-align:middle; text-align:left;">
                                                                <asp:ImageButton ID="Btn_Consultar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                                        ToolTip="Buscar al Empleado" OnClick="Btn_Consultar_Click"/>    
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
                
                    <table width="98%" class="">
                        <tr id="Tr_Periodos_Fiscales" runat="server">
                            <td colspan="4" style="width:100%">
                                <asp:Panel id="Pnl_Nomina_Periodo_Incapacidad" runat="server" GroupingText="Calendario Nomina">
                                    <table width="98%">
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
                                                    Width="100%" />
                                            </td>                                                                                        
                                        </tr>
                                    </table>
                                </asp:Panel>      
                                <br />                     
                            </td>
                        </tr>                   
                        <tr style="display:none;">
                            <td style="width:20%; text-align:left;">
                                No Incapacidad
                            </td>
                            <td style="width:30%; text-align:left;">
                                <asp:TextBox ID="Txt_No_Incapacidad" runat="server" Width="98%" />
                            </td>              
                            <td style="width:20%; text-align:left;">
                                
                            </td>
                            <td style="width:30%; text-align:left;">

                            </td>                                                        
                        </tr>
                        <tr>
                            <td style="width:20%; text-align:left;">
                                *U. Responsable
                            </td>
                            <td style="width:80%; text-align:left;" colspan="3">
                                <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="100%" AutoPostBack="true"
                                    OnSelectedIndexChanged="Cmb_Dependencia_SelectedIndexChanged"/>
                            </td>                              
                        </tr>    
                        <tr>              
                            <td style="width:20%; text-align:left;">
                                *Empleados
                            </td>
                            <td style="width:80%; text-align:left;" colspan="3">
                                <asp:DropDownList ID="Cmb_Empleados" runat="server" Width="95.5%"/>
                                <asp:ImageButton ID="Btn_Busqueda_Incapacidad" runat="server" ToolTip="Consultar"
                                                 ImageUrl="~/paginas/imagenes/paginas/busqueda.png" Height="16px" Width="18px"
                                                 OnClick="Btn_Busqueda_Incapacidad_Click"/>                                 
                            </td>                                                        
                        </tr>         
                        <tr>
                            <td style="width:20%; text-align:left;">
                                *Tipo Incapacidad
                            </td>
                            <td style="width:30%; text-align:left;">
                                <asp:DropDownList ID="Cmb_Tipo_Incapacidad" runat="server" Width="100%"
                                    >
                                    <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>
                                    <asp:ListItem>Enfermedad General</asp:ListItem>
                                    <asp:ListItem>Maternidad</asp:ListItem>
                                    <asp:ListItem>Riesgo Laboral</asp:ListItem>                                    
                                </asp:DropDownList>
                            </td>              
                            <td style="width:20%; text-align:left;">
                                &nbsp;&nbsp;*Porcentaje
                            </td>
                            <td style="width:30%; text-align:left;">
                                <asp:TextBox ID="Txt_Porcentaje_Incapacidad" runat="server" Width="98%" />
                                <cc1:FilteredTextBoxExtender ID="FTxt_Porcentaje_Incapacidad" runat="server"  
                                    TargetControlID="Txt_Porcentaje_Incapacidad" FilterType="Numbers"/>  
                                <asp:CustomValidator ID="Cv_Txt_Porcentaje_Incapacidad" runat="server"  Display="None"
                                     EnableClientScript="true" ErrorMessage="El Porcentaje [%] debe ser entre [0-100]"
                                     Enabled="true"
                                     ClientValidationFunction="TextBox_Txt_Porcentaje_Incapacidad"
                                     HighlightCssClass="highlight" 
                                     ControlToValidate="Txt_Porcentaje_Incapacidad"/>
                                <cc1:ValidatorCalloutExtender ID="Vce_Txt_Porcentaje_Incapacidad" runat="server" TargetControlID="Cv_Txt_Porcentaje_Incapacidad" 
                                    PopupPosition="TopRight"/>    
                                <script type="text/javascript" >
                                    function TextBox_Txt_Porcentaje_Incapacidad(sender, args) {     
                                         var Porcentaje = document.getElementById("<%=Txt_Porcentaje_Incapacidad.ClientID%>").value;   
                                         if (Porcentaje < 0 || Porcentaje > 100){  
                                            document.getElementById("<%=Txt_Porcentaje_Incapacidad.ClientID%>").value ="";       
                                            args.IsValid = false;     
                                         }
                                      } 
                                </script>                                  
                            </td>                                                        
                        </tr>                                                                                    
                        <tr>
                            <td style="width:20%; text-align:left;">
                                *Fecha Inicio
                            </td>
                            <td style="width:30%; text-align:left;">
                                <asp:TextBox ID="Txt_Fecha_Inicio" runat="server"  Width="85%" AutoPostBack="true" OnTextChanged="Txt_Fecha_Inicio_TextChanged"/>
                                <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Inicio_FilteredTextBoxExtender" 
                                    runat="server" TargetControlID="Txt_Fecha_Inicio" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                                <cc1:CalendarExtender ID="Txt_Fecha_Inicio_CalendarExtender" runat="server" 
                                    TargetControlID="Txt_Fecha_Inicio" PopupButtonID="Btn_Fecha_Inicio" Format="dd/MMM/yyyy"/>
                                <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha Final"/>
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
                                    ID="Mev_Mee_Txt_Fecha_Inicio" 
                                    runat="server" 
                                    ControlToValidate="Txt_Fecha_Inicio"
                                    ControlExtender="Mee_Txt_Fecha_Inicio" 
                                    EmptyValueMessage="La Fecha Inicial es obligatoria"
                                    InvalidValueMessage="Fecha Inicial Invalida" 
                                    IsValidEmpty="true" 
                                    TooltipMessage="Ingrese o Seleccione la Fecha Inicial"
                                    Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                                  
                            </td>              
                            <td style="width:20%; text-align:left;">
                                &nbsp;&nbsp;*Fecha Fin
                            </td>
                            <td style="width:30%; text-align:left;">
                                <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="85%"/>
                                <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Fin_FilteredTextBoxExtender" 
                                    runat="server" TargetControlID="Txt_Fecha_Fin" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                                <cc1:CalendarExtender ID="Txt_Fecha_Fin_CalendarExtender" runat="server" 
                                    TargetControlID="Txt_Fecha_Fin" PopupButtonID="Btn_Fecha_Fin" Format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Fecha_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha Inicial"/>   
                                <cc1:MaskedEditExtender 
                                    ID="Mee_Txt_Fecha_Fin" 
                                    Mask="99/LLL/9999" 
                                    runat="server"
                                    MaskType="None" 
                                    UserDateFormat="DayMonthYear" 
                                    UserTimeFormat="None" Filtered="/"
                                    TargetControlID="Txt_Fecha_Fin" 
                                    Enabled="True" 
                                    ClearMaskOnLostFocus="false"/>  
                                <cc1:MaskedEditValidator 
                                    ID="Mev_Mee_Txt_Fecha_Fin" 
                                    runat="server" 
                                    ControlToValidate="Txt_Fecha_Fin"
                                    ControlExtender="Mee_Txt_Fecha_Fin" 
                                    EmptyValueMessage="La Fecha Final es obligatoria"
                                    InvalidValueMessage="Fecha Final Invalida" 
                                    IsValidEmpty="true" 
                                    TooltipMessage="Ingrese o Seleccione la Fecha Final"
                                    Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                                        
                            </td>   
                        </tr>   
                        <tr>
                            <td style="width:20%; text-align:left;">
                                *Estatus
                            </td>
                            <td style="width:30%; text-align:left;">
                               <asp:DropDownList ID="Cmb_Estatis" runat="server" Width="100%">
                                    <asp:ListItem>&lt;- Seleccione -&gt;</asp:ListItem>
                                    <asp:ListItem>Pendiente</asp:ListItem>
                                    <asp:ListItem>Rechazado</asp:ListItem>
                                    <asp:ListItem>Autorizado</asp:ListItem>
                                </asp:DropDownList>
                            </td>              
                            <td style="width:20%; text-align:left;">
                                &nbsp;&nbsp;Dias
                            </td>
                            <td style="width:30%; text-align:left;">
                                <asp:TextBox ID="Txt_Dias_Incapacidad" runat="server" Width="98%" AutoPostBack="true" 
                                    OnTextChanged="Txt_Dias_Incapacidad_TextChanged" onblur="javascript:Aplica_Cuarto_Pago_Dia();"/>
                            </td>                                                        
                        </tr>                                     
                        <tr>
                            <td colspan="4" style="width:100%;">
                                <br />
                                <div id="Div_Aplica_Pago_Cuarto_Dia_Aplica_Incapacidad" runat="server" style="font-size:10;color:White; font-weight:bold;border-style:outset; cursor:auto;width:98%;" class="watermarked">
                                    <center style="color:Black; font-size:10px;">                                        
                                        <asp:Label ID="Lbl_Chk_Aplica_Pago_Cuarto_Dia" runat="server" Text="Aplica Pago Cuarto Día" CssClass=""/>
                                        &nbsp;&nbsp;
                                        <asp:CheckBox ID="Chk_Aplica_Pago_Cuarto_Dia" runat="server" onclick="javascript:Aplica_Cuarto_Pago_Dia();"
                                            ToolTip="Indica si el pago de incapacidad se realizara hasta el cuerto día."/>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="Chk_Aplica_Extencion_Incapacidad" runat="server"
                                            ToolTip="Indica si la incapacidad a capturar es una extención de alguna incapacidad previa."/>    
                                        &nbsp;&nbsp;    
                                        <asp:Label ID="Lbl_Chk_Aplica_Extencion_Incapacidad" runat="server" Text="Extención Incapacidad "/>                                                                       
                                    </center>
                                </div>
                                <br />
                            </td>
                        </tr> 
                        <tr>
                            <td style="width:20%; text-align:left; vertical-align:top;">
                                *Comentarios
                            </td>
                            <td style="width:30%; text-align:left;" colspan="3">
                                <asp:TextBox ID="Txt_Comentarios" runat="server" Width="99.5%" MaxLength="100" TextMode="MultiLine" 
                                    Wrap="true" Height="60px"/>
                                <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>
                                <cc1:FilteredTextBoxExtender ID="FTxt_Comentarios" runat="server"  TargetControlID="Txt_Comentarios"
                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>    
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios" runat="server" TargetControlID ="Txt_Comentarios" 
                                    WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/>   
                            </td>              
                        </tr>                                                                                                           
                    </table>
                    
                    <br />
                    <div style="overflow:auto;height:400px;width:97%;vertical-align:top;border-style: outset;color:White;" >
                        <table width="98%">
                            <tr>
                                <td style="width:100%;">
                                <asp:GridView ID="Grid_Incapacidades" runat="server" CssClass="GridView_1"
                                     AutoGenerateColumns="False"  GridLines="None"
                                     HeaderStyle-CssClass="tblHead" OnSelectedIndexChanged="Grid_Incapacidades_SelectedIndexChanged">
                                         <Columns>
                                             <asp:ButtonField ButtonType="Image" CommandName="Select"  HeaderText=""
                                                 ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                 <ItemStyle Width="5%" HorizontalAlign="Left"/>
                                             </asp:ButtonField>
                                             <asp:BoundField DataField="NO_INCAPACIDAD" HeaderText="No Incapacidad" SortExpression="NO_INCAPACIDAD">
                                                 <HeaderStyle HorizontalAlign="Left" Width="15%"  />
                                                 <ItemStyle HorizontalAlign="Left" Width="15%" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="EMPLEADO_ID" HeaderText="Empleado" SortExpression="EMPLEADO_ID">
                                                 <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                 <ItemStyle HorizontalAlign="Left" Width="0%" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="DEPENDENCIA" HeaderText="U. Responsable" SortExpression="DEPENDENCIA">
                                                 <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                                 <ItemStyle HorizontalAlign="Left" Width="25%" />
                                             </asp:BoundField>                                                                                                                      
                                             <asp:BoundField DataField="TIPO_INCAPACIDAD" HeaderText="Tipo Falta" SortExpression="TIPO_INCAPACIDAD">
                                                  <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                  <ItemStyle HorizontalAlign="Left" Width="20%" />
                                             </asp:BoundField>                                             
                                         </Columns>
                                         <SelectedRowStyle CssClass="GridSelected" />
                                         <PagerStyle CssClass="GridHeader" />                                     
                                         <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>                                
                                </td>
                            </tr>
                        </table>
                    </div>
            </div>        
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


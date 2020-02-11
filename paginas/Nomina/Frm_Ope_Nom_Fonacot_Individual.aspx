<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" 
    CodeFile="Frm_Ope_Nom_Fonacot_Individual.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Fonacot_Individual" Title="Captura de Créditos Fonacot" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <link href="../../easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency-1.4.0.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency.all.js" type="text/javascript"></script>    
    
    <script type="text/javascript">
    function pageLoad(){
        $('#contenedor_empleados table > tbody > tr:not(:has(table, th))').click(function(e){
            var $row = $(this);
            var texto = "Empleado: " + $('td', $row).eq(2).text() + "<br />" +
                        "Deduccion: " + $('td', $row).eq(3).text() + "<br />" +
                        "No Fonacot: " + $('td', $row).eq(0).text() + "<br />" +
                        "No Crédito: " + $('td', $row).eq(1).text() + "<br />" +
                        "Cantidad: " + $('td', $row).eq(6).text() + "<br />" +
                        "Periodo: " + $('td', $row).eq(5).text() + "<br />";

			    $.messager.show({
				    title:'Información',
				    msg:texto,
				    showType:'fade',
				    width:400,
				    height:180
			    });
            });
            
            $('select[id$=Cmb_Proveedores]').bind("change", function(){
                var Opcion_Seleccionada = $('select[id$=Cmb_Proveedores] :selected').val();
                if(Opcion_Seleccionada == ""){
                    $('#archivo').attr('disabled', 'disabled');
                }else{
                    $('#archivo').removeAttr('disabled');
                }
                
                $('#contenedor_empleados table > tbody > tr').remove();
            });
            
            var Opcion_Seleccionada = $('select[id$=Cmb_Proveedores] :selected').val();
            if(Opcion_Seleccionada == ""){
                $('#archivo').attr('disabled', 'disabled');
            }else {
                $('#archivo').removeAttr('disabled');
            }                 
    }
    </script>

    <script type="text/javascript" language="javascript">
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

 <script type="text/javascript" language="javascript">
    function Limpiar_Ctlr(){
        document.getElementById("<%=Txt_Busqueda_No_Empleado.ClientID%>").value="";
        document.getElementById("<%=Txt_Busqueda_RFC.ClientID%>").value="";  
        document.getElementById("<%=Cmb_Busqueda_Dependencia.ClientID%>").value="";  
        document.getElementById("<%=Txt_Busqueda_Nombre_Empleado.ClientID%>").value="";                            
        return false;
    }  
    
    function Calcular_Retension_Mensual(){
        var Plazo = parseFloat( ($('input[id$=Txt_Plazo]').val() == '') ? '0' : $('input[id$=Txt_Plazo]').val());
        var Importe = parseFloat( ($('input[id$=Txt_Importe]').val() == '') ? '0' : $('input[id$=Txt_Importe]').val().replace(',', ''));
        var Periodos_Mes = parseFloat( ($('input[id$=Txt_No_Periodos]').val() == '') ? '0' : $('input[id$=Txt_No_Periodos]').val());
        var Retencion_Mensual = 0.0;
        var Retencion_Catorcenal = parseFloat( ($('input[id$=Txt_Retension_Real]').val() == '') ? '0' : $('input[id$=Txt_Retension_Real]').val());
       
        if(((!isNaN(Importe)) && (Importe != undefined) && (Importe != 'Requerido'))
            &&
          ((!isNaN(Plazo)) && (Plazo != undefined)) && (Plazo != 'Requerido'))
        {
            $('input[id$=Txt_Retension_Mensual]').val(roundNumber((Importe/Plazo), 2));        
            Retencion_Mensual = ($('input[id$=Txt_Retension_Mensual]').val() == '') ? '0' : $('input[id$=Txt_Retension_Mensual]').val();
        }
        
        if((!isNaN(Periodos_Mes)) && (Periodos_Mes != undefined) && (Periodos_Mes != 'Requerido'))
            $('input[id$=Txt_Retension_Real]').val(roundNumber((parseFloat(Retencion_Mensual) / Periodos_Mes ), 2));
    }
    function roundNumber(num, dec) {
	    var result = Math.round(num*Math.pow(10,dec))/Math.pow(10,dec);
	    return result;
    }    
</script> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server"  AsyncPostBackTimeout="3600" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="UPnl_Captura_Masiva_Proveedores_Fijas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Captura_Masiva_Proveedores_Fijas" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        
        <div id="Div_Captura_Masiva_Proveedores_Fijas" style="background-color:White; width:98%;">
            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                <tr align="center">
                    <td class="label_titulo" >
                       <table style="width:98%;">
                            <tr>
                                <td style="height:55px;background-image:url(../imagenes/paginas/Escudo_icon.jpg); background-repeat: no-repeat; background-position:left;width:20%;">
                                
                                </td>
                                <td style="width:60%;">
                                    <span style="font-family:Arial; font-size:x-large; font-weight:normal; font-style:italic; vertical-align:bottom;">Captura de Cr&eacute;ditos FONACOT</span>
                                </td>
                                <td style="height:55px;background-image:url(../imagenes/paginas/Fonacot_V1.png); background-repeat: no-repeat; background-position:right;width:20%;">
                                    
                                </td>                                                                
                            </tr>
                       </table>
                    </td>
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
                                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" CausesValidation="false"/>
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click" CausesValidation="false"/>
                                      </td>
                                      <td align="right" style="width:41%;">  
                                        <asp:ImageButton ID="Btn_Mostrar_Popup_Busqueda" runat="server" ToolTip="Busqueda Avanzada"
                                            ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px" Width="24px"
                                            OnClientClick="javascript:$find('Busqueda_Empleados').show();return false;" CausesValidation="false" />                                                                   
                                       </td>       
                                     </tr>         
                                  </table>                      
                                </div>
                         </td>
                     </tr>
            </table>
        
            <table style="width:98%;">
               <tr>
                    <td colspan="4" style="width:100%;">
                        <hr />
                    </td>
                </tr>
                <tr id="Tr_Periodos_Fiscales" runat="server">
                    <td style="text-align:left;width:20%;">
                        *N&oacute;mina
                    </td>
                    <td  style="text-align:left;width:30%;" >
                        <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" AutoPostBack="true" 
                            OnSelectedIndexChanged="Cmb_Calendario_Nomina_SelectedIndexChanged"/>
                    </td> 
                    <td style="text-align:left;width:20%;">
                       &nbsp;&nbsp;*Periodo Inicia
                    </td>
                    <td  style="text-align:left;width:30%;">   
                        <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" Width="100%" AutoPostBack="true" 
                            OnSelectedIndexChanged="Cmb_Periodos_Selected"/>
                    </td>                                                                       
                </tr> 
                <tr>
                    <td style="width:20%; text-align:left;">
                        *Proveedor
                    </td>
                    <td style="width:80%; text-align:left;" colspan="3">
                        <asp:DropDownList ID="Cmb_Proveedores" runat="server" Width="100%"/>
                    </td>
                </tr>                
                <tr>
                    <td colspan="4" style="width:100%;">
                        <hr />
                    </td>
                </tr> 
                <tr >
                    <td style="text-align:left;width:20%;">
                        
                    </td>
                    <td  style="text-align:left;width:30%;" >

                    </td>                       
                    <td style="width:20%; text-align:left; cursor:default;">
                       &nbsp;&nbsp;*Fecha Autorización
                    </td>
                    <td style="width:30%; cursor:default;">
                        <asp:TextBox ID="Txt_Fecha_Autorizacion" runat="server"  Width="88%"/>                        
                        <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Autorizacion_FilteredTextBoxExtender" 
                            runat="server" TargetControlID="Txt_Fecha_Autorizacion" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                        <cc1:CalendarExtender ID="Txt_Fecha_Autorizacion_CalendarExtender" runat="server" 
                            TargetControlID="Txt_Fecha_Autorizacion" PopupButtonID="Btn_Fecha_Autorizacion" Format="dd/MMM/yyyy"/>
                        <asp:ImageButton ID="Btn_Fecha_Autorizacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                            ToolTip="Seleccione la Fecha Final"/>
                        <cc1:MaskedEditExtender 
                            ID="Mee_Txt_Fecha_Autorizacion" 
                            Mask="99/LLL/9999" 
                            runat="server"
                            MaskType="None" 
                            UserDateFormat="DayMonthYear" 
                            UserTimeFormat="None" Filtered="/"
                            TargetControlID="Txt_Fecha_Autorizacion" 
                            Enabled="True" 
                            ClearMaskOnLostFocus="false"/>  
                        <cc1:MaskedEditValidator 
                            ID="Mev_Mee_Txt_Fecha_Autorizacion" 
                            runat="server" 
                            ControlToValidate="Txt_Fecha_Autorizacion"
                            ControlExtender="Mee_Txt_Fecha_Autorizacion" 
                            EmptyValueMessage="La Fecha Inicial es obligatoria"
                            InvalidValueMessage="Fecha Inicial Invalida" 
                            IsValidEmpty="true" 
                            TooltipMessage="Ingrese o Seleccione la Fecha Inicial"
                            Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>       
                    </td>                                                                                        
                </tr>                 
            </table>    
            
            <table style="width:98%;">
                <tr>
                    <td style="width:20%; text-align:left; cursor:default;">
                        No Empleado
                    </td>
                    <td style="width:30%; cursor:default;">
                        <asp:TextBox ID="Txt_No_Empleado_Fonacot" runat="server" Width="98%" Enabled="false"/>                    
                    </td>                 
                    <td style="width:20%; text-align:left; cursor:default;">
                        <asp:HiddenField ID="HTxt_Empleado_ID" runat="server"/>
                    </td>
                    <td style="width:30%; cursor:default;">
                    
                    </td>                                        
                </tr>
                <tr>
                    <td style="width:20%; text-align:left; cursor:default;">
                        Empleado
                    </td>
                    <td style="width:30%; cursor:default;" colspan="3">
                        <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" Width="99.5%" Enabled="false"/>
                    </td>                                                      
                </tr>
                <tr>
                    <td style="width:20%; text-align:left; cursor:default;">
                        *No Crédito
                    </td>
                    <td style="width:30%; cursor:default;">
                        <asp:TextBox ID="Txt_No_Credito" runat="server" Width="98%" />
                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_No_Credito" runat="server" 
                            TargetControlID ="Txt_No_Credito" WatermarkText="Requerido" 
                            WatermarkCssClass="watermarked"/>
                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Credito" runat="server" 
                            TargetControlID="Txt_No_Credito" FilterType="Numbers" />                            
                    </td>                 
                    <td style="width:20%; text-align:left; cursor:default;">
                        &nbsp;&nbsp;*No Fonacot
                    </td>
                    <td style="width:30%; cursor:default;">
                        <asp:TextBox ID="Txt_No_Fonacot" runat="server" Width="98%" />
                        <cc1:TextBoxWatermarkExtender ID="Twm_Txt_No_Fonacot" runat="server" 
                            TargetControlID ="Txt_No_Fonacot" WatermarkText="Requerido" 
                            WatermarkCssClass="watermarked"/>
                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Fonacot" runat="server" 
                            TargetControlID="Txt_No_Fonacot" FilterType="Numbers" />                           
                    </td>                                        
                </tr>
                
                <tr>
                    <td style="width:20%; text-align:left; cursor:default;">
                        *Importe Cr&eacute;dito
                    </td>
                    <td style="width:30%; cursor:default;">
                        <asp:TextBox ID="Txt_Importe" runat="server" Width="98%" onkeyup="javascript:Calcular_Retension_Mensual();"
                            onblur="$('input[id$=Txt_Importe]').formatCurrency({colorize:true, region: 'es-MX'});"/>
                        <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Importe" runat="server" 
                            TargetControlID ="Txt_Importe" WatermarkText="Requerido" 
                            WatermarkCssClass="watermarked"/>    
                    </td>                 
                    <td style="width:20%; text-align:left; cursor:default;">
                        &nbsp;&nbsp;*Plazo
                    </td>
                    <td style="width:30%; cursor:default;">
                        <asp:TextBox ID="Txt_Plazo" runat="server" Width="98%" onkeyup="javascript:Calcular_Retension_Mensual();"/>
                        <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Plazo" runat="server" 
                            TargetControlID ="Txt_Plazo" WatermarkText="Requerido" 
                            WatermarkCssClass="watermarked"/>
                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Plazo" runat="server" 
                            TargetControlID="Txt_Plazo" FilterType="Numbers" />     
                    </td>                                        
                </tr>                
                
                <tr>
                    <td style="width:20%; text-align:left; cursor:default;">
                        *Periodos Mes
                    </td>
                    <td style="width:30%; cursor:default;">
                        <asp:TextBox ID="Txt_No_Periodos" runat="server" Width="98%" MaxLength="1" onkeyup="javascript:Calcular_Retension_Mensual();"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="Twm_Txt_No_Periodos" runat="server" 
                            TargetControlID ="Txt_No_Periodos" WatermarkText="Requerido" 
                            WatermarkCssClass="watermarked"/>
                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Periodos" runat="server" 
                            TargetControlID="Txt_No_Periodos" FilterType="Numbers" />   
                    </td>                 
                    <td style="text-align:left;width:20%;">
                        &nbsp;&nbsp;*Cuotas Pagadas
                    </td>
                    <td>
                        <asp:TextBox ID="Txt_Cuotas_Pagadas" runat="server" Width="98%" />
                        <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Cuotas_Pagadas" runat="server" 
                            TargetControlID ="Txt_Cuotas_Pagadas" WatermarkText="Opcional"
                            WatermarkCssClass="watermarked"/>
                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cuotas_Pagadas" runat="server" 
                            TargetControlID="Txt_Cuotas_Pagadas" FilterType="Numbers" />   
                    </td>                   
                </tr>      
                <tr>
                    <td style="width:20%; text-align:left; cursor:default;">
                        Retención Mensual
                    </td>
                    <td style="width:30%; cursor:default;">
                        <asp:TextBox ID="Txt_Retension_Mensual" runat="server" Width="98%" onkeyup="javascript:Calcular_Retension_Mensual();"/>
                    </td>                 
                    <td style="width:20%; text-align:left; cursor:default;">
                        &nbsp;&nbsp;Retension Catorcenal
                    </td>
                    <td style="width:30%; cursor:default;">
                        <asp:TextBox ID="Txt_Retension_Real" runat="server" Width="98%" onkeyup="javascript:Calcular_Retension_Mensual();"/>
                    </td>                                        
                </tr>     
                <tr>
                    <td style="width:20%; text-align:left; cursor:default;">
                        RFC Proveedor
                    </td>
                    <td style="width:30%; cursor:default;">
                        <asp:TextBox ID="Txt_RFC_Proveedor" runat="server" Width="98%" />
                    </td>                 
                    <td style="width:20%; text-align:left; cursor:default;">
                        &nbsp;&nbsp;Fonacot
                    </td>
                    <td style="width:30%; cursor:default;">
                        <asp:DropDownList ID="Cmb_Deducciones_Fonacot" runat ="server" Width="100%" Enabled="false"/>
                    </td>                                        
                </tr>                                                                                                               
            </table>                   
        </div>
       
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_MPE_Empleados" runat="server" Text="" style="display:none;"/>
            <cc1:ModalPopupExtender ID="MPE_Empleados" runat="server"  BehaviorID="Busqueda_Empleados"
            TargetControlID="Btn_Comodin_MPE_Empleados" PopupControlID="Pnl_Busqueda_Contenedor" 
            CancelControlID="Btn_Cerrar_Ventana" PopupDragHandleControlID="Pnl_Busqueda_Empleado"
            DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>              
            </ContentTemplate>
    </asp:UpdatePanel>
    
<asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="850px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Busqueda_Empleado" runat="server" 
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
                          <div id="Div_Resultados_Busqueda" runat="server" style="border-style:outset; width:99%; height: 250px; overflow:auto;">
                              <asp:GridView ID="Grid_Busqueda_Empleados" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                    ForeColor="#333333" GridLines="None" AllowPaging="True" Width="100%" 
                                    PageSize="100" EmptyDataText="No se encontrarón resultados para los filtros de la busqueda" 
                                    OnSelectedIndexChanged="Grid_Busqueda_Empleados_SelectedIndexChanged"
                                    OnPageIndexChanging="Grid_Busqueda_Empleados_PageIndexChanging"
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


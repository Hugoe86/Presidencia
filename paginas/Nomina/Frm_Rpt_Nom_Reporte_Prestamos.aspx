<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Reporte_Prestamos.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Reporte_Prestamos" Title="Untitled Page" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">

<script src="../../easyui/jquery.formatCurrency-1.4.0.min.js" type="text/javascript"></script>
<script src="../../easyui/jquery.formatCurrency.all.js" type="text/javascript"></script>

<script type="text/javascript" language="javascript">
function pageLoad(){
     $('input[id$=Txt_No_Empleado]').css('background-image', 'url(../imagenes/paginas/empleado.png)');
     $('input[id$=Txt_No_Empleado]').css('background-repeat', 'no-repeat');
     $('input[id$=Txt_No_Empleado]').css('background-position', 'left');    
     $('input[id$=Txt_No_Empleado]').css('border-style', 'Solid');
     $('input[id$=Txt_No_Empleado]').css('border-color', '#2F4E7D');
     $('input[id$=Txt_No_Empleado]').css('font-family', 'Tahoma');
     $('input[id$=Txt_No_Empleado]').css('font-size', '12px');
     $('input[id$=Txt_No_Empleado]').css('font-weight', 'bold');
     $('input[id$=Txt_No_Empleado]').css('color', '#2F4E7D');
     $('input[id$=Txt_No_Empleado]').css('height', '24px');
     $('input[id$=Txt_No_Empleado]').css('text-align', 'center');   
     $('input[id$=Txt_No_Empleado]').click(function(){ $(this).focus().select(); });
     $('input[id$=Txt_No_Empleado]').keyup(function(){$(this).css('background-color', '#FFFF99');});
     $('input[id$=Txt_No_Empleado]').blur(function(){$(this).css('background-color', 'White');});
     
     $('input[id$=Txt_Aval]').css('background-image', 'url(../imagenes/paginas/empleado.png)');
     $('input[id$=Txt_Aval]').css('background-repeat', 'no-repeat');
     $('input[id$=Txt_Aval]').css('background-position', 'left');    
     $('input[id$=Txt_Aval]').css('border-style', 'Solid');
     $('input[id$=Txt_Aval]').css('border-color', '#2F4E7D');
     $('input[id$=Txt_Aval]').css('font-family', 'Tahoma');
     $('input[id$=Txt_Aval]').css('font-size', '12px');
     $('input[id$=Txt_Aval]').css('font-weight', 'bold');
     $('input[id$=Txt_Aval]').css('color', '#2F4E7D');
     $('input[id$=Txt_Aval]').css('height', '24px');
     $('input[id$=Txt_Aval]').css('text-align', 'center');   
     $('input[id$=Txt_Aval]').click(function(){ $(this).focus().select(); });
     $('input[id$=Txt_Aval]').keyup(function(){$(this).css('background-color', '#FFFF99');});
     $('input[id$=Txt_Aval]').blur(function(){$(this).css('background-color', 'White');});     
   
    
     $('input[id$=Txt_Cantidad_Prestamo]').css('background-image', 'url(../imagenes/gridview/economico.png)');
     $('input[id$=Txt_Cantidad_Prestamo]').css('background-repeat', 'no-repeat');
     $('input[id$=Txt_Cantidad_Prestamo]').css('background-position', 'left');    
     $('input[id$=Txt_Cantidad_Prestamo]').css('border-style', 'Solid');
     $('input[id$=Txt_Cantidad_Prestamo]').css('border-color', '#2F4E7D');
     $('input[id$=Txt_Cantidad_Prestamo]').css('font-family', 'Courier');
     $('input[id$=Txt_Cantidad_Prestamo]').css('font-size', '24px');
     $('input[id$=Txt_Cantidad_Prestamo]').css('font-weight', 'bold');
     $('input[id$=Txt_Cantidad_Prestamo]').css('color', '#2F4E7D');
     $('input[id$=Txt_Cantidad_Prestamo]').css('height', '24px');
     $('input[id$=Txt_Cantidad_Prestamo]').css('text-align', 'right'); 
     $('input[id$=Txt_Cantidad_Prestamo]').click(function(){ $(this).focus().select(); });
     $('input[id$=Txt_Cantidad_Prestamo]').keyup(function(){$(this).css('background-color', '#FFFF99');});
     $('input[id$=Txt_Cantidad_Prestamo]').blur(function(){$(this).css('background-color', 'White');});

   $('input[id$=Txt_No_Empleado]').live("blur", function(){
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
    
   $('input[id$=Txt_Aval]').live("blur", function(){
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
    
function isNumber(n) {   return !isNaN(parseFloat(n)) && isFinite(n); }   
</script> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

<asp:ScriptManager ID="Sm_Reporte_Prestamos" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"></asp:ScriptManager>

<asp:UpdatePanel ID="UPnl_Reporte_Prestamos" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdateProgress ID="UPgrs_Reporte_Prestamos" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UPnl_Reporte_Prestamos">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress">
                    <img alt="" src="../Imagenes/paginas/Updating.gif" />
                </div>    
            </ProgressTemplate>
        </asp:UpdateProgress>
    
        <div id="Div_Contenedor" style="width:98%; background-color:White;">
            <table width="100%" title="Control_Errores"> 
                <tr>
                    <td class="button_autorizar" style="width:100%; text-align:center; cursor:default; font-size:12px;">
                        Reporte Pr&eacute;stamos
                    </td>               
                </tr>            
                <tr>
                    <td class="button_autorizar"  style="width:100%; text-align:left; cursor:default;">
                        <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false"  />&nbsp;
                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                    </td>               
                </tr>   
            </table>           
        
            <table id="Tbl_Contenedor_Principal" style="width:100%;">
                <tr>
                    <td style="width:100%;" colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width:20%; text-align:left; cursor:default;">
                        &nbsp;&nbsp;Estatus
                    </td>
                    <td style="width:30%; text-align:left; cursor:default;">
                        <asp:DropDownList ID="Cmb_Estatus_Prestamo" runat="server" Width="100%">
                            <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                            <asp:ListItem>Pendiente</asp:ListItem>
                            <asp:ListItem>Autorizado</asp:ListItem>
                            <asp:ListItem>Rechazado</asp:ListItem>                                  
                        </asp:DropDownList>                           
                    </td>        
                    <td style="width:20%; text-align:left; cursor:default;">     
                        &nbsp;&nbsp;Estado                   
                    </td>
                    <td style="width:30%; text-align:left; cursor:default;">
                          <asp:DropDownList ID="Cmb_Estado_Prestamo" runat="server" Width="100%">   
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>                                                                                             
                                <asp:ListItem>Pendiente</asp:ListItem>
                                <asp:ListItem>Proceso</asp:ListItem>
                                <asp:ListItem>Pagado</asp:ListItem>                               
                           </asp:DropDownList>    
                    </td>                
                </tr>   
                <tr>
                    <td style="width:100%;" colspan="4">
                        <hr />
                    </td>
                </tr>                             
                <tr>
                    <td style="width:20%; text-align:left; cursor:default;">
                        &nbsp;&nbsp;Solicitante
                    </td>
                    <td style="width:30%; text-align:left; cursor:default;">
                        <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="97%" MaxLength="6"/>
                    </td>        
                    <td style="width:20%; text-align:left; cursor:default;">     
                        &nbsp;&nbsp;Aval                 
                    </td>
                    <td style="width:30%; text-align:left; cursor:default;">
                        <asp:TextBox ID="Txt_Aval" runat="server" Width="97%" MaxLength="6"/>
                    </td>                
                </tr>
                <tr>
                    <td style="width:20%; text-align:left; cursor:default;">
                        &nbsp;&nbsp;
                    </td>
                    <td style="width:30%; text-align:left; cursor:default;">
                        
                    </td>        
                    <td style="width:20%; text-align:left; cursor:default;">     
                        &nbsp;&nbsp;Cantidad              
                    </td>
                    <td style="width:30%; text-align:left; cursor:default;">
                        <asp:TextBox ID="Txt_Cantidad_Prestamo" runat="server" Width="97%" MaxLength="6" 
                            onblur="$('input[id$=Txt_Cantidad_Prestamo]').formatCurrency({colorize:true, region: 'es-MX'});"/>
                    </td>                
                </tr>
                <tr>
                    <td style="width:100%;" colspan="4">
                        <hr />
                    </td>
                </tr>                
                <tr>
                    <td style="text-align:left;width:20%;">
                       &nbsp;&nbsp;Fecha Inicio
                    </td>
                    <td style="text-align:left;width:30%;">
                        <asp:TextBox ID="Txt_Fecha_Inicio" runat="server"  Width="88%"/>
                        <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Inicio_FilteredTextBoxExtender" 
                            runat="server" TargetControlID="Txt_Fecha_Inicio" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                        <cc1:CalendarExtender ID="Txt_Fecha_Inicio_CalendarExtender" runat="server" 
                            TargetControlID="Txt_Fecha_Inicio" PopupButtonID="Btn_Fecha_Inicio" Format="dd/MM/yyyy"/>
                        <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                            ToolTip="Seleccione la Fecha Final"/>
                        <cc1:MaskedEditExtender 
                            ID="Mee_Txt_Fecha_Inicio" 
                            Mask="99/99/9999" 
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
                    <td style="text-align:left;width:20%;">
                       &nbsp;&nbsp;Fecha Fin
                    </td>
                    <td style="text-align:left;width:30%;">
                        <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="88%" />
                        <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Fin_FilteredTextBoxExtender" 
                            runat="server" TargetControlID="Txt_Fecha_Fin" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                        <cc1:CalendarExtender ID="Txt_Fecha_Fin_CalendarExtender" runat="server" 
                            TargetControlID="Txt_Fecha_Fin" PopupButtonID="Btn_Fecha_Fin" Format="dd/MM/yyyy" />
                        <asp:ImageButton ID="Btn_Fecha_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                            ToolTip="Seleccione la Fecha Inicial"/>   
                        <cc1:MaskedEditExtender 
                            ID="Mee_Txt_Fecha_Fin" 
                            Mask="99/99/9999" 
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
                    <td style="width:100%;" colspan="4">
                        <hr />
                    </td>
                </tr>                 
            </table>
        </div>    
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Btn_Generar_Reporte_PDF" EventName="Click" />
        <asp:PostBackTrigger  ControlID="Btn_Generar_Reporte_Excel"/>
    </Triggers>    
</asp:UpdatePanel>

<table style="width:98%;">       
    <tr>
        <td class="button_autorizar" style="width:100%; text-align:right; cursor:default;" colspan="4">
            <asp:UpdatePanel ID="Upnl_Export_PDF" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                        <asp:ImageButton ID="Btn_Generar_Reporte_PDF" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" 
                            OnClick="Btn_Generar_Reporte_PDF_Click" ToolTip="Generar Reporte Prestamos en PDF" Width="32px" Height="32px" style="cursor:hand; display:none;"/>
                </ContentTemplate>
            </asp:UpdatePanel>
            
            <asp:UpdatePanel ID="Upnl_Export_EXCEL" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <asp:ImageButton ID="Btn_Generar_Reporte_Excel" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                          OnClick="Btn_Generar_Reporte_Excel_Click" ToolTip="Generar Reporte Prestamos en EXCEL" Width="32px" Height="32px" style="cursor:hand;"/>
                 </ContentTemplate>
            </asp:UpdatePanel>
        </td>                
    </tr>   
    <tr>
        <td style="width:100%; text-align:left; cursor:default;" colspan="4">
            <hr />
        </td>                
    </tr>                                                                         
</table>
    
</asp:Content>


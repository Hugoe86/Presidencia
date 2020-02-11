<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Empleados.aspx.cs" Inherits="paginas_Nomina_Reportes_Nomina_Frm_Rpt_Nom_Empleados" Title="Reporte de Empleados" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" language="javascript">
function pageLoad(){
   $('input[id$=Txt_Busqueda_No_Empleado]').live("blur", function(){
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

<asp:ScriptManager ID="SM_Rpt_Empleados" runat="server"  EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
<asp:UpdatePanel ID="UPnl_Rpt_Empleados" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Rpt_Empleados" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>              

        <div style="background-color:White; width:98%;">
            <table style="width:99%;">
                <tr>
                    <td style="width:100%;" align="center">
                        <div id="Contenedor_Titulo" style="color:White;font-size:12;font-weight:bold;border-style:outset;background:url(../imagenes/paginas/titleBackground.png) repeat-x top;background-color:Silver;">
                            <table width="100%">
                                <tr>
                                    <td></td>
                                </tr>            
                                <tr>
                                    <td width="100%" align="left">
                                        <font style="color: Black; font-weight: bold;">Reporte de Empleados</font>
                                    </td>    
                                </tr>  
                                <tr>
                                    <td></td>
                                </tr>                                      
                            </table>    
                        </div>
                    </td>
                </tr>
            </table>
            
            
            <table width="100%" class="estilo_fuente">
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
        
            <table style="width:100%;">
                <tr>
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        No Empleado
                    </td>
                    <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                        <asp:TextBox ID="Txt_Busqueda_No_Empleado" runat="server" Width="98%" TabIndex="0"/>
                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Empleado" runat="server" 
                            TargetControlID="Txt_Busqueda_No_Empleado" FilterType="Numbers"/>                    
                    </td>        
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        Estatus
                    </td>
                    <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                        <asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="100%">   
                            <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>                                         
                            <asp:ListItem>ACTIVO</asp:ListItem>
                            <asp:ListItem>INACTIVO</asp:ListItem>                                   
                        </asp:DropDownList>                     
                    </td>                
                </tr>
                <tr>
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        Nombre
                    </td>
                    <td class="button_autorizar" style="width:80%; text-align:left; cursor:default;" colspan="3">
                        <asp:TextBox ID="Txt_Busqueda_Nombre_Empleado" runat="server" Width="98%" TabIndex="0"
                            onkeyup='this.value = this.value.toUpperCase();'/>
                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Nombre_Empleado" runat="server" 
                            TargetControlID="Txt_Busqueda_Nombre_Empleado" FilterType="Custom, UppercaseLetters, LowercaseLetters"
                            ValidChars="ÑñáéíóúÁÉÍÓÚ "/>                    
                    </td>                   
                </tr>  
                <tr>
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        RFC
                    </td>
                    <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                        <asp:TextBox ID="Txt_Busqueda_RFC_Empleado" runat="server" Width="98%" TabIndex="0"
                            onkeyup='this.value = this.value.toUpperCase();'/>
                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_RFC_Empleado" runat="server" 
                            TargetControlID="Txt_Busqueda_RFC_Empleado" ValidChars=" "
                            FilterType="Numbers, UppercaseLetters, LowercaseLetters"/>                    
                    </td>        
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        CURP
                    </td>
                    <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                        <asp:TextBox ID="Txt_Busqueda_CURP_Empleado" runat="server" Width="98%" TabIndex="0"
                            onkeyup='this.value = this.value.toUpperCase();'/>
                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_CURP_Empleado" runat="server" 
                            TargetControlID="Txt_Busqueda_CURP_Empleado" ValidChars=" "
                            FilterType="Numbers, UppercaseLetters, LowercaseLetters"/>                      
                    </td>                
                </tr>   
                <tr>
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        Tipo Nómina
                    </td>
                    <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                        <asp:DropDownList ID="Cmb_Busqueda_Tipo_Nomina" runat="server" Width="100%"/>                   
                    </td>        
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        Sindicato
                    </td>
                    <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                        <asp:DropDownList ID="Cmb_Busqueda_Sindicato" runat="server" Width="100%"/>                      
                    </td>                
                </tr>    
                <tr>
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        U. Responsable
                    </td>
                    <td class="button_autorizar" style="width:80%; text-align:left; cursor:default;" colspan="3">
                        <asp:DropDownList ID="Cmb_Busqueda_Unidad_Responsable" runat="server" Width="100%"
                            AutoPostBack="true" OnSelectedIndexChanged="Cmb_Busqueda_Unidad_Responsable_SelectedIndexChanged"/>              
                    </td>                   
                </tr>   
                <tr>
                    <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                        Area
                    </td>              
                    <td class="button_autorizar" style="width:30%;text-align:left;cursor:default;" colspan="3">
                       <asp:DropDownList ID="Cmb_Busqueda_Areas" runat="server" Width="100%" />                                          
                    </td> 
                </tr>                 
                <tr>
                    <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                        Fecha Inicio
                    </td>
                    <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                        <asp:TextBox ID="Txt_Busqueda_Fecha_Inicio" runat="server" Width="85%" MaxLength="1" TabIndex="14"/>
                        <cc1:FilteredTextBoxExtender ID="FTxt_Busqueda_Fecha_Inicio" 
                            runat="server" TargetControlID="Txt_Busqueda_Fecha_Inicio" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                            ValidChars="/_"/>
                        <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Inicio" runat="server" 
                            TargetControlID="Txt_Busqueda_Fecha_Inicio" PopupButtonID="Btn_Busqueda_Fecha_Inicio" Format="dd/MMM/yyyy" 
                            />
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
                    <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                        Fecha Fin
                    </td>
                    <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                        <asp:TextBox ID="Txt_Busqueda_Fecha_Fin" runat="server" Width="85%" MaxLength="1" TabIndex="15"/>
                        <cc1:FilteredTextBoxExtender ID="FTxt_Busqueda_Fecha_Fin" 
                            runat="server" TargetControlID="Txt_Busqueda_Fecha_Fin" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                            ValidChars="/_"/>                                                    
                        <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Fin" runat="server" 
                            TargetControlID="Txt_Busqueda_Fecha_Fin" PopupButtonID="Btn_Busqueda_Fecha_Fin" Format="dd/MMM/yyyy" 
                            />
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
                    <td style="width:100%; text-align:left; cursor:default;" colspan="4">
                       <hr />
                    </td>                
                </tr>                                                                                       
            </table>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Btn_Generar_Reporte" EventName="Click" />
        <asp:PostBackTrigger  ControlID="Btn_Generar_Reporte_Minimo"/>
    </Triggers>
</asp:UpdatePanel>

<table style="width:98%;">       
    <tr>
        <td class="button_autorizar" style="width:100%; text-align:right; cursor:default;" colspan="4">
            <asp:UpdatePanel ID="Upnl_Export_PDF" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                        <asp:ImageButton ID="Btn_Generar_Reporte" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" 
                             OnClick="Btn_Generar_Reporte_Click" ToolTip="Generar Reporte Completo Empleados en PDF" Width="32px" Height="32px" style="cursor:hand;"/>
                </ContentTemplate>
            </asp:UpdatePanel>
            
            <asp:UpdatePanel ID="Upnl_Export_EXCEL" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <asp:ImageButton ID="Btn_Generar_Reporte_Minimo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                         OnClick="Btn_Generar_Reporte_Minimo_Click" ToolTip="Generar Reporte Completo Empleados en EXCEL" Width="32px" Height="32px" style="cursor:hand;"/>
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


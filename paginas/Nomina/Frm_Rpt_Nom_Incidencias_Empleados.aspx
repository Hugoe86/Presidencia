<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Incidencias_Empleados.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Incidencias_Empleados" Title="Reporte de Incidencias de los Empleados" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

<asp:ScriptManager ID="SM_Rpt_Incidencias_Empleados" runat="server"  EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
<asp:UpdatePanel ID="UPnl_Rpt_Incidencias_Empleados" runat="server" UpdateMode="Always">
    <ContentTemplate>
    
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Rpt_Incidencias_Empleados" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>  
        
        <div style="width:98%; background-color:White;">
            <table width="100%" title="Control_Errores"> 
                <tr>
                    <td class="button_autorizar" style="width:100%; text-align:center; cursor:default; font-size:14px;">
                        Reportes Incidencias de los Empleados
                    </td>               
                </tr>            
                <tr>
                    <td style="width:100%; text-align:left; cursor:default;">
                        <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />
                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>        
                    </td>               
                </tr>
            </table>
            <asp:Panel ID="Pnl_Datos_Incidencia" runat="server" Width="100%" GroupingText="Filtrar por Incidencia">
                <table width="100%">
                    <tr>
                        <td class="button_autorizar" style="width:20%;text-align:left;">
                            Nomina
                        </td>
                        <td class="button_autorizar" style="width:30%;text-align:left;">
                            <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" AutoPostBack="true" 
                               onselectedindexchanged="Cmb_Calendario_Nomina_SelectedIndexChanged"/>
                        </td>             
                        <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                            &nbsp;&nbsp;Periodo
                        </td>
                        <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                            <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" 
                                Width="100%"/>
                        </td>                                                                                        
                    </tr>         
                <tr>
                    <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                        Fecha Inicio
                    </td>
                    <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                        <asp:TextBox ID="Txt_Incidencia_Fecha_Inicio" runat="server" Width="85%" MaxLength="1"/>
                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Incidencia_Fecha_Inicio" 
                            runat="server" TargetControlID="Txt_Incidencia_Fecha_Inicio" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                            ValidChars="/_"/>
                        <cc1:CalendarExtender ID="Ce_Incidencia_Fecha_Inicio" runat="server" 
                            TargetControlID="Txt_Incidencia_Fecha_Inicio" PopupButtonID="IBtn_Incidencia_Fecha_Inicio" Format="dd/MMM/yyyy" 
                            />
                        <asp:ImageButton ID="IBtn_Incidencia_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                            ToolTip="Seleccione la Fecha"/> 
                        <cc1:MaskedEditExtender 
                            ID="Mee_Incidencia_Fecha_Inicio" 
                            Mask="99/LLL/9999" 
                            runat="server"
                            MaskType="None" 
                            UserDateFormat="DayMonthYear" 
                            UserTimeFormat="None" Filtered="/"
                            TargetControlID="Txt_Incidencia_Fecha_Inicio" 
                            Enabled="True" 
                            ClearMaskOnLostFocus="false"/>  
                        <cc1:MaskedEditValidator 
                            ID="Mev_Incidencia_Fecha_Inicio" 
                            runat="server" 
                            ControlToValidate="Txt_Incidencia_Fecha_Inicio"
                            ControlExtender="Mee_Incidencia_Fecha_Inicio" 
                            EmptyValueMessage="Es valido no ingresar fecha inicial"
                            InvalidValueMessage="Fecha Inicial Invalida" 
                            IsValidEmpty="true" 
                            TooltipMessage="Ingrese o Seleccione la Fecha Inicial"
                            Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                                                                               
                    </td>
                    <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                        &nbsp;&nbsp;Fecha Fin
                    </td>
                    <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                        <asp:TextBox ID="Txt_Incidencia_Fecha_Fin" runat="server" Width="85%" MaxLength="1"/>
                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Incidencia_Fecha_Fin" 
                            runat="server" TargetControlID="Txt_Incidencia_Fecha_Fin" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" 
                            ValidChars="/_"/>                                                    
                        <cc1:CalendarExtender ID="Ce_Incidencia_Fecha_Fin" runat="server" 
                            TargetControlID="Txt_Incidencia_Fecha_Fin" PopupButtonID="IBtn_Incidencia_Fecha_Fin" Format="dd/MMM/yyyy" 
                            />
                        <asp:ImageButton ID="IBtn_Incidencia_Fecha_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                            ToolTip="Seleccione la Fecha"/> 
                        <cc1:MaskedEditExtender 
                            ID="Mee_Incidencia_Fecha_Fin" 
                            Mask="99/LLL/9999" 
                            runat="server"
                            MaskType="None" 
                            UserDateFormat="DayMonthYear" 
                            UserTimeFormat="None" Filtered="/"
                            TargetControlID="Txt_Incidencia_Fecha_Fin" 
                            Enabled="True" 
                            ClearMaskOnLostFocus="false"/>  
                        <cc1:MaskedEditValidator 
                            ID="Mev_Incidencia_Fecha_Fin" 
                            runat="server" 
                            ControlToValidate="Txt_Incidencia_Fecha_Fin"
                            ControlExtender="Mee_Incidencia_Fecha_Fin" 
                            EmptyValueMessage="Es valido no ingresar fecha final"
                            InvalidValueMessage="Fecha Final Invalida" 
                            IsValidEmpty="true" 
                            TooltipMessage="Ingrese o Seleccione la Fecha Final"
                            Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                        
                    </td>                                                            
                </tr>                              
                </table>
            </asp:Panel>
      
            <asp:Panel ID="Pnl_Filtros_Empleado" runat="server" Width="100%" GroupingText="Filtrar por Empleados">        
                <table width="100%">              
                    <tr>
                        <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                            Número Empleado
                        </td>
                        <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                            <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="98%"/>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Empleado" runat="server" 
                                TargetControlID="Txt_No_Empleado" FilterType="Numbers"/>                         
                        </td>        
                        <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                            &nbsp;&nbsp;Estatus
                        </td>
                        <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                <asp:ListItem>&lt;- Seleccione - &gt;</asp:ListItem>
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
                            <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" Width="99.5%"/>
                        </td>                        
                    </tr>    
                    <tr>
                        <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                            RFC
                        </td>
                        <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                            <asp:TextBox ID="Txt_Busqueda_RFC_Empleado" runat="server" Width="98%"
                                onkeyup='this.value = this.value.toUpperCase();'/>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_RFC_Empleado" runat="server" 
                                TargetControlID="Txt_Busqueda_RFC_Empleado" ValidChars=" "
                                FilterType="Numbers, UppercaseLetters, LowercaseLetters"/>                    
                        </td>        
                        <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                            &nbsp;&nbsp;CURP
                        </td>
                        <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                            <asp:TextBox ID="Txt_Busqueda_CURP_Empleado" runat="server" Width="98%"
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
                            <asp:DropDownList ID="Cmb_Tipo_Nomina" runat="server" Width="100%"/>
                        </td>        
                        <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                            &nbsp;&nbsp;Sindicato
                        </td>
                        <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                            <asp:DropDownList ID="Cmb_Sindicato" runat="server" Width="100%"/>
                        </td>                
                    </tr>    
                    <tr>
                        <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                            U. Responsable
                        </td>
                        <td class="button_autorizar" style="width:80%; text-align:left; cursor:default;" colspan="3">
                            <asp:DropDownList ID="Cmb_Unidad_Responsable" runat="server" Width="100%"
                                AutoPostBack="true" OnSelectedIndexChanged="Cmb_Busqueda_Unidad_Responsable_SelectedIndexChanged"/>
                        </td>   
                    </tr>    
                    <tr>                             
                        <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                            Área
                        </td>
                        <td class="button_autorizar" style="width:80%; text-align:left; cursor:default;" colspan="3">
                            <asp:DropDownList ID="Cmb_Area" runat="server" Width="100%"/>
                        </td>                
                    </tr>       
                    <tr>
                        <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                            Fecha Inicio
                        </td>
                        <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                            <asp:TextBox ID="Txt_Busqueda_Fecha_Inicio" runat="server" Width="85%" MaxLength="1"/>
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
                            &nbsp;&nbsp;Fecha Fin
                        </td>
                        <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                            <asp:TextBox ID="Txt_Busqueda_Fecha_Fin" runat="server" Width="85%" MaxLength="1"/>
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
                        <td class="button_autorizar" style="width:100%; text-align:center; cursor:default;" colspan="4">
                            <asp:RadioButtonList ID="RBtn_Tipo_Incidencia" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem>Faltas</asp:ListItem>
                                <asp:ListItem>Retardos</asp:ListItem>
                                <asp:ListItem>Vacaciones</asp:ListItem>
                                <asp:ListItem>Tiempo Extra</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>                
                    </tr> 
                </table>
            </asp:Panel>
                
            <table width="100%">                                    
                <tr>
                    <td class="button_autorizar" style="width:100%; text-align:left; cursor:default;" colspan="4">
                       <hr />
                    </td>                
                </tr>                
                <tr>
                    <td style="width:100%; text-align:left; cursor:default;" colspan="4">
                        <asp:Button ID="Btn_Generar_Reporte" runat="server" Text="Generar Reporte" 
                            Width="100%" CssClass="button_autorizar" OnClick="Btn_Generar_Reporte_Click"/>
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
</asp:UpdatePanel>
</asp:Content>


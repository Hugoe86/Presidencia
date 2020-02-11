<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Asistencias.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Asistencias" Title="Asistencias de Empleados" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Src="~/paginas/Paginas_Generales/Pager.ascx" TagPrefix="custom" TagName="Pager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script type="text/javascript" language="javascript">
        //Metodo para mantener los calendarios en una capa mas alat.
        function calendarShown(sender, args)
        {
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
        //Metodos para limpiar los controles de la busqueda.
        function Limpiar_Ctlr()
        {
            document.getElementById("<%=Txt_Busqueda_No_Empleado.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_Nombre_Empleado.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_Fecha_Inicio.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_Fecha_Fin.ClientID%>").value="";                        
            return false;
        }  
        function Abrir_Modal_Popup() 
        {
            $find('Busqueda_Empleados').show();
            return false;
        } 
    </script>
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
    <asp:ScriptManager ID="ScriptManager_Asistencias_Personal" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Asistencias_Empleados" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Asistencias_Personal" style="background-color:#ffffff; width:98%; height:100%;">
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Asistencias de Empleado</td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="98%"  border="0" cellspacing="0">
                    <tr align="center">
                        <td>                
                            <div align="right" class="barra_busqueda">                        
                                <table style="width:100%;height:28px;">
                                    <tr>
                                        <td align="left" style="width:59%;"> 
                                            <asp:UpdatePanel ID="Upnl_Botones_Operacion" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
                                                <ContentTemplate> 
                                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                                        CssClass="Img_Button" TabIndex="2"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                                        CssClass="Img_Button" TabIndex="3"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" 
                                                        CssClass="Img_Button" TabIndex="4"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"                                                        
                                                        OnClientClick="return confirm('¿Está seguro de eliminar la asistencia seleccionada?');" 
                                                        onclick="Btn_Eliminar_Click"/>
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                                        CssClass="Img_Button" TabIndex="5"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click" />
                                                    <asp:TextBox ID="Txt_Empleado_ID" runat="server" Visible="false" ReadOnly="true"></asp:TextBox>    
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
                                                                    <asp:ImageButton ID="Btn_Mostrar_Popup_Busqueda" runat="server" 
                                                                        ToolTip="Busqueda Avanzada" TabIndex="1"
                                                                        ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px" Width="24px"
                                                                        OnClientClick="javascript:return Abrir_Modal_Popup();" CausesValidation="false" />
                                                                    <cc1:ModalPopupExtender ID="Mpe_Busqueda_Empleados" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="Busqueda_Empleados"
                                                                        PopupControlID="Pnl_Busqueda_Contenedor" TargetControlID="Btn_Comodin_Open" PopupDragHandleControlID="Pnl_Busqueda_Cabecera" 
                                                                        CancelControlID="Btn_Comodin_Close" DropShadow="True" DynamicServicePath="" Enabled="True"/>  
                                                                    <asp:Button Style="background-color: transparent; border-style:none;" 
                                                                        ID="Btn_Comodin_Close" runat="server" Text="" />
                                                                    <asp:Button  Style="background-color: transparent; border-style:none;" 
                                                                        ID="Btn_Comodin_Open" runat="server" Text="" />                                                                                                    
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
                
                <asp:UpdatePanel ID="Upnl_Datos_Generales_Empleado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>          
                        <asp:Panel ID="Pnl_Datos_Generales" runat="server" GroupingText="Datos Generales" Width="97%" BackColor="White">
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:20%;text-align:left;">No. Empleado</td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_No_Empleado_Asistencia" runat="server" ReadOnly="true" BorderStyle="Solid" BorderWidth="1" Width="200px"/>
                                    </td>
                                    <td style="width:20%;text-align:left;">Estatus</td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Estatus_Empleado_Asistencia" runat="server" ReadOnly="True" BorderStyle="Solid" BorderWidth="1" Width="200px"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">Nombre</td>
                                    <td colspan="3" style="width:80%;text-align:left;">
                                        <asp:TextBox ID="Txt_Nombre_Empleado_Asistencia" runat="server" ReadOnly="true" BorderStyle="Solid" BorderWidth="1" Width="98%"></asp:TextBox> 
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">Unidad Responsable</td>
                                    <td colspan="3" style="width:80%;text-align:left;">
                                        <asp:TextBox ID="Txt_Unidad_Responsable_Empleado_Asistencia" runat="server" ReadOnly="true" BorderStyle="Solid" BorderWidth="1" Width="98%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:UpdatePanel ID="Upnl_Datos_Asistencia" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="Pnl_Datos_Asistencia" runat="server" GroupingText="Datos Asistencia" Width="97%" BackColor="White">
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:20%;text-align:left;">No. Asistencia</td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_No_Asistencia" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="width:20%;text-align:left;">Reloj Checador</td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:DropDownList ID="Cmb_Reloj_Checador_Asistencia" runat="server" Width="200px" TabIndex="6"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">Fecha Entrada</td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Fecha_Entrada_Asistencia" runat="server" Width="83%" TabIndex="7"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Fecha_Entrada_Asistencia" runat="server" TargetControlID="Txt_Fecha_Entrada_Asistencia"
                                            FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                                        <cc1:CalendarExtender ID="DTP_Fecha_Entrada_Asistencia" runat="server" PopupButtonID="Btn_Fecha_Entrada_Asistencia"
                                            TargetControlID="Txt_Fecha_Entrada_Asistencia" Format="dd/MMM/yyyy" OnClientShown="calendarShown"/>
                                        <asp:ImageButton ID="Btn_Fecha_Entrada_Asistencia" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                            ToolTip="Seleccione la Fecha de Entrada"/> 
                                        <cc1:MaskedEditExtender 
                                            ID="MEE_Txt_Fecha_Entrada_Asistencia" 
                                            Mask="99/LLL/9999" 
                                            runat="server"
                                            MaskType="None" 
                                            UserDateFormat="DayMonthYear" 
                                            UserTimeFormat="None" Filtered="/"
                                            TargetControlID="Txt_Fecha_Entrada_Asistencia" 
                                            Enabled="True" 
                                            ClearMaskOnLostFocus="false"/>  
                                    </td>
                                    <td style="width:20%;text-align:left;">Hora Entrada</td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Hora_Entrada_Asistencia" runat="server" TabIndex="8"  Text="12:12:12 a.m."
                                            ToolTip="[ Horas:0-23  Minutos:0-59  Segundos:0-59 ]"  Width="200px"/>
                                        <cc1:MaskedEditExtender 
                                             AutoCompleteValue="true"
                                             ID="MEE_Txt_Hora_Entrada_Asistencia"
                                             runat="server" 
                                             TargetControlID="Txt_Hora_Entrada_Asistencia"
                                             PromptCharacter="_" 
                                             AcceptAMPM="true" 
                                             Mask="99:99:99" 
                                             MaskType="Time"
                                             Century="2000"
                                             UserTimeFormat="TwentyFourHour"
                                             InputDirection="LeftToRight"
                                             ClearMaskOnLostFocus="false"
                                             AcceptNegative="None"
                                             MessageValidatorTip="true" AutoComplete="true" ClipboardEnabled="False"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">Fecha Salida</td>
                                    <td style="width:30%;text-align:left;">
                                         <asp:TextBox ID="Txt_Fecha_Salida_Asistencia" runat="server" Width="83%" TabIndex="9"></asp:TextBox>
                                         <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fecha_Salida_Asistencia" runat="server" TargetControlID="Txt_Fecha_Salida_Asistencia"
                                            FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                                        <cc1:CalendarExtender ID="DTP_Fecha_Salida" runat="server" PopupButtonID="Btn_Fecha_Salida_Asistencia"
                                            TargetControlID="Txt_Fecha_Salida_Asistencia" Format="dd/MMM/yyyy" OnClientShown="calendarShown"/>
                                        <asp:ImageButton ID="Btn_Fecha_Salida_Asistencia" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                            ToolTip="Seleccione la Fecha de Salida"/> 
                                        <cc1:MaskedEditExtender 
                                            ID="MEE_Fecha_Salida_Asistencia" 
                                            Mask="99/LLL/9999"
                                            runat="server"
                                            MaskType="None" 
                                            UserDateFormat="DayMonthYear" 
                                            UserTimeFormat="None" Filtered="/"
                                            TargetControlID="Txt_Fecha_Salida_Asistencia" 
                                            Enabled="True" 
                                            ClearMaskOnLostFocus="false"/>
                                    </td>
                                    <td style="width:20%;text-align:left;">Hora Salida</td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Hora_Salida_Asistencia" runat="server" TabIndex="10"  Text="12:12:12 a.m."
                                            ToolTip="[ Horas:0-23  Minutos:0-59  Segundos:0-59 ]"  Width="200px"/>
                                        <cc1:MaskedEditExtender 
                                             AutoCompleteValue="true"
                                             ID="MEE_Txt_Hora_Salida_Asistencia"
                                             runat="server" 
                                             TargetControlID="Txt_Hora_Salida_Asistencia"
                                             PromptCharacter="_"
                                             AcceptAMPM="true" 
                                             Mask="99:99:99" 
                                             MaskType="Time"
                                             Century="2000"
                                             UserTimeFormat="TwentyFourHour"
                                             InputDirection="LeftToRight"
                                             ClearMaskOnLostFocus="false"
                                             AcceptNegative="None"
                                             MessageValidatorTip="true" AutoComplete="true" ClipboardEnabled="False"/>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table style="width:100%;">
                                <tr align="center">
                                    <td>
                                        <div style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" >  
                                            <asp:GridView ID="Grid_Asistencias_Empleados" runat="server"  Width="100%"
                                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                                HeaderStyle-CssClass="tblHead" OnSelectedIndexChanged="Grid_Asistencias_Empleados_SelectedIndexChanged" >
                                                <RowStyle CssClass="GridItem" />
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle Width="7%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="NO_ASISTENCIA" HeaderText="No. Asistencia" Visible="True" SortExpression="NO_ASISTENCIA">
                                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Reloj_Checador_ID" HeaderText="Reloj ID" Visible="True" SortExpression="Reloj_Checador_ID">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0px" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Reloj" HeaderText="Reloj" Visible="True" SortExpression="Reloj">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FECHA_HORA_ENTRADA" HeaderText="Fecha Hora Entrada" 
                                                        Visible="True" SortExpression="FECHA_HORA_ENTRADA" 
                                                        DataFormatString="{0:dd/MMM/yyyy HH:mm:ss}">
                                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FECHA_HORA_SALIDA" HeaderText="Fecha Hora Salida" 
                                                        Visible="True" SortExpression="FECHA_HORA_SALIDA" 
                                                        DataFormatString="{0:dd/MMM/yyyy HH:mm:ss}">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <RowStyle CssClass="GridItem" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
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
                         B&uacute;squeda: Empleado
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
                        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Asistencia" runat="server">
                            <ContentTemplate>
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Asistencia" runat="server" AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Asistencia" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                                        <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress">
                                            <img alt="" src="../Imagenes/paginas/Sias_Roler.gif" />
                                        </div>
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
                                        <td style="width:100%" colspan="4"><hr /></td>
                                    </tr>   
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">No Empleado</td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                            <asp:TextBox ID="Txt_Busqueda_No_Empleado" runat="server" Width="98%" TabIndex="11"/>
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Empleado" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Busqueda_No_Empleado" />
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_No_Empleado" runat="server" WatermarkCssClass="watermarked"
                                                TargetControlID ="Txt_Busqueda_No_Empleado" WatermarkText="Busqueda por No Empleado" />                                                                                                                                          
                                        </td> 
                                        <td colspan="2" style="width:50%;text-align:left;font-size:11px;"></td>                     
                                    </tr>                                                                                                   
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">Nombre</td>              
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Busqueda_Nombre_Empleado" runat="server" Width="99.5%" TabIndex="12"/>
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Nombre_Empleado" runat="server" FilterType="Custom, LowercaseLetters, Numbers, UppercaseLetters"
                                                TargetControlID="Txt_Busqueda_Nombre_Empleado" ValidChars="áéíóúÁÉÍÓÚ "/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Nombre_Empleado" runat="server" WatermarkCssClass="watermarked"
                                                TargetControlID ="Txt_Busqueda_Nombre_Empleado" WatermarkText="Busqueda por Nombre" />                                                                                               
                                        </td>                                         
                                    </tr>                                                                                                                                                                                                      
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">Fecha Inicio</td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Fecha_Inicio" runat="server" Width="85%" MaxLength="1" TabIndex="13"/>
                                            <cc1:FilteredTextBoxExtender ID="FTxt_Busqueda_Fecha_Inicio" runat="server" TargetControlID="Txt_Busqueda_Fecha_Inicio"
                                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                                            <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Inicio" runat="server" PopupButtonID="Btn_Busqueda_Fecha_Inicio"
                                                TargetControlID="Txt_Busqueda_Fecha_Inicio" Format="dd/MMM/yyyy" OnClientShown="calendarShown"/>
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
                                        <td style="width:20%;text-align:left;font-size:11px;">Fecha Fin</td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Fecha_Fin" runat="server" Width="85%" MaxLength="1" TabIndex="14"/>
                                            <cc1:FilteredTextBoxExtender ID="FTxt_Busqueda_Fecha_Fin" runat="server" TargetControlID="Txt_Busqueda_Fecha_Fin"
                                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>                                                    
                                            <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Fin" runat="server" OnClientShown="calendarShown"
                                                TargetControlID="Txt_Busqueda_Fecha_Fin" PopupButtonID="Btn_Busqueda_Fecha_Fin" Format="dd/MMM/yyyy"/>
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
                                        <td style="width:100%" colspan="4"><hr /></td>
                                    </tr>                                    
                                    <tr>
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <center>
                                                <asp:Button ID="Btn_Busqueda_Empleados" runat="server"  Text="Busqueda de Empleado" CssClass="button"  
                                                    CausesValidation="false" Width="200px" TabIndex="15" OnClick="Btn_Busqueda_Empleados_Click"/> 
                                            </center>
                                        </td>                                                     
                                    </tr>
                                    <tr align="center">
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <asp:GridView ID="Grid_Busqueda_Empleados" runat="server" AllowPaging="True" Width="100%"
                                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" AllowSorting="true"
                                                HeaderStyle-CssClass="tblHead" onselectedindexchanged="Grid_Busqueda_Empleados_SelectedIndexChanged">
                                                <RowStyle CssClass="GridItem" />
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle Width="7%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="EMPLEADO_ID" HeaderText="Empleado ID" Visible="True" >
                                                        <HeaderStyle HorizontalAlign="Left" Width="0px" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NO_EMPLEADO" HeaderText="No Empleado" Visible="True" >
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Empleado" HeaderText="Nombre" Visible="True" >
                                                        <HeaderStyle HorizontalAlign="Left" Width="70%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="70%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <RowStyle CssClass="GridItem" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                        </td>
                                    </tr>                                                          
                                </table>                                                                                                                                                              
                            </ContentTemplate>                                                                   
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
            </table>                                                   
        </div>
    </asp:Panel>
</asp:Content>


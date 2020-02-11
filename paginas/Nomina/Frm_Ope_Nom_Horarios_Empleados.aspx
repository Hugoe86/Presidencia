<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Horarios_Empleados.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Horarios_Empleados" Title="Horarios de Empleados" %>
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
            return false;
        }  
        function Abrir_Modal_Popup() 
        {
            $find('Busqueda_Empleados').show();
            return false;
        } 
    </script>s
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Asistencias_Personal" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Horarios_Empleados" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Horarios_Personal" style="background-color:#ffffff; width:98%; height:100%;">
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Horarios de Empleado</td>
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
                                                        OnClientClick="return confirm('¿Está seguro de eliminar el horario seleccionado?');" 
                                                        onclick="Btn_Eliminar_Click"/>
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                                        CssClass="Img_Button" TabIndex="5"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click" />
                                                    <asp:TextBox ID="Txt_Empleado_ID" runat="server" Visible=false ReadOnly="true"></asp:TextBox>    
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
                                                                        ToolTip="Busqueda Avanzada"
                                                                        ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px" Width="24px"
                                                                        OnClientClick="javascript:return Abrir_Modal_Popup();" CausesValidation="false" TabIndex="1"/>
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
                                        <asp:TextBox ID="Txt_No_Empleado_Horario" runat="server" ReadOnly="True" BorderStyle="Solid" BorderWidth="1" Width="200px"/>
                                    </td>
                                    <td style="width:20%;text-align:left;">Estatus</td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Estatus_Empleado_Horario" runat="server" ReadOnly="True" BorderStyle="Solid" BorderWidth="1" Width="200px"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">Nombre</td>
                                    <td colspan="3" style="width:80%;text-align:left;">
                                        <asp:TextBox ID="Txt_Nombre_Empleado_Horario" runat="server" ReadOnly="true" BorderStyle="Solid" BorderWidth="1" Width="98%"></asp:TextBox> 
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">Unidad Responsable</td>
                                    <td colspan="3" style="width:80%;text-align:left;">
                                        <asp:TextBox ID="Txt_Unidad_Responsable_Empleado_Horario" runat="server" ReadOnly="true" BorderStyle="Solid" BorderWidth="1" Width="98%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:UpdatePanel ID="Upnl_Datos_Asistencia" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="Pnl_Datos_Horario_Empleado" runat="server" GroupingText="Datos Horarios del Empleado" Width="97%" BackColor="White">
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:20%;text-align:left;">No. Horario</td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_No_Horario_Empleado" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">Fecha Inicio</td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Fecha_Entrada_Horario" runat="server" Width="83%" TabIndex="6"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Fecha_Entrada_Horario" runat="server" TargetControlID="Txt_Fecha_Entrada_Horario"
                                            FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                                        <cc1:CalendarExtender ID="DTP_Fecha_Entrada_Horario" runat="server" PopupButtonID="Btn_Fecha_Entrada_Horario"
                                            TargetControlID="Txt_Fecha_Entrada_Horario" Format="dd/MMM/yyyy" OnClientShown="calendarShown"/>
                                        <asp:ImageButton ID="Btn_Fecha_Entrada_Horario" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                            ToolTip="Seleccione la Fecha de Horario de Entrada"/> 
                                        <cc1:MaskedEditExtender 
                                            ID="MEE_Txt_Fecha_Entrada_Horario" 
                                            Mask="99/LLL/9999" 
                                            runat="server"
                                            MaskType="None" 
                                            UserDateFormat="DayMonthYear" 
                                            UserTimeFormat="None" Filtered="/"
                                            TargetControlID="Txt_Fecha_Entrada_Horario" 
                                            Enabled="True" 
                                            ClearMaskOnLostFocus="false"/>
                                    </td>
                                    <td style="width:20%;text-align:left;">Fecha Termino</td>
                                    <td style="width:30%;text-align:left;">
                                         <asp:TextBox ID="Txt_Fecha_Termino_Horario" runat="server" Width="83%" TabIndex="7"></asp:TextBox>
                                         <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fecha_Termino_Horario" runat="server" TargetControlID="Txt_Fecha_Termino_Horario"
                                            FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                                        <cc1:CalendarExtender ID="DTP_Fecha_Termino_Horario" runat="server" PopupButtonID="Btn_Fecha_Termino_Horario"
                                            TargetControlID="Txt_Fecha_Termino_Horario" Format="dd/MMM/yyyy" OnClientShown="calendarShown"/>
                                        <asp:ImageButton ID="Btn_Fecha_Termino_Horario" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                            ToolTip="Seleccione la Fecha de Termino"/> 
                                        <cc1:MaskedEditExtender 
                                            ID="MEE_Fecha_Termino_Horario" 
                                            Mask="99/LLL/9999"
                                            runat="server"
                                            MaskType="None" 
                                            UserDateFormat="DayMonthYear" 
                                            UserTimeFormat="None" Filtered="/"
                                            TargetControlID="Txt_Fecha_Termino_Horario" 
                                            Enabled="True" 
                                            ClearMaskOnLostFocus="false"/>
                                    </td>                                   
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">Hora Entrada</td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Hora_Entrada" runat="server" TabIndex="8"  Text="12:12:12 a.m."
                                            ToolTip="[ Horas:0-23  Minutos:0-59  Segundos:0-59 ]"  Width="200px"/>
                                        <cc1:MaskedEditExtender 
                                             AutoCompleteValue="true"
                                             ID="MEE_Txt_Hora_Entrada"
                                             runat="server" 
                                             TargetControlID="Txt_Hora_Entrada"
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
                                    <td style="width:20%;text-align:left;">Hora Salida</td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Hora_Salida" runat="server" TabIndex="9"  Text="12:12:12 a.m."
                                            ToolTip="[ Horas:0-23  Minutos:0-59  Segundos:0-59 ]"  Width="200px"/>
                                        <cc1:MaskedEditExtender 
                                             AutoCompleteValue="true"
                                             ID="MEE_Txt_Hora_Salida"
                                             runat="server" 
                                             TargetControlID="Txt_Hora_Salida"
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
                                        <asp:GridView ID="Grid_Horarios_Empleado" runat="server" AllowPaging="True" Width="100%"
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                            PageSize="5" AllowSorting="True" HeaderStyle-CssClass="tblHead" 
                                            onpageindexchanging="Grid_Horarios_Empleado_PageIndexChanging" 
                                            onselectedindexchanged="Grid_Horarios_Empleado_SelectedIndexChanged" 
                                            onsorting="Grid_Horarios_Empleado_Sorting">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="5%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="NO_HORARIO_EMPLEADO" HeaderText="No. Horario" Visible="True" SortExpression="NO_HORARIO_EMPLEADO">
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FECHA_INICIO" HeaderText="Fecha Inicio" 
                                                    Visible="True" SortExpression="FECHA_INICIO" 
                                                    DataFormatString="{0:dd/MMM/yyyy}">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FECHA_TERMINO" HeaderText="Fecha Termino" 
                                                    Visible="True" SortExpression="FECHA_TERMINO" 
                                                    DataFormatString="{0:dd/MMM/yyyy}">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="HORA_ENTRADA" HeaderText="Hora Entrada" 
                                                    Visible="True" SortExpression="HORA_ENTRADA" 
                                                    DataFormatString="{0:HH:mm:ss}">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="HORA_SALIDA" HeaderText="Hora Salida" 
                                                    Visible="True" SortExpression="HORA_SALIDA"
                                                    DataFormatString="{0:HH:mm:ss}">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                            </Columns>
                                            <RowStyle CssClass="GridItem" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <HeaderStyle CssClass="tblHead" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag" HorizontalAlign="Center" Width="650px" 
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
                                            <asp:TextBox ID="Txt_Busqueda_No_Empleado" runat="server" Width="98%" TabIndex="10" />
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
                                            <asp:TextBox ID="Txt_Busqueda_Nombre_Empleado" runat="server" Width="99.5%" TabIndex="11"/>
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Nombre_Empleado" runat="server" FilterType="Custom, LowercaseLetters, Numbers, UppercaseLetters"
                                                TargetControlID="Txt_Busqueda_Nombre_Empleado" ValidChars="áéíóúÁÉÍÓÚ "/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Nombre_Empleado" runat="server" WatermarkCssClass="watermarked"
                                                TargetControlID ="Txt_Busqueda_Nombre_Empleado" WatermarkText="Busqueda por Nombre" />                                                                                               
                                        </td>                                         
                                    </tr>                                                                                          
                                    <tr>
                                        <td style="width:100%" colspan="4"><hr /></td>
                                    </tr>                                    
                                    <tr>
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <center>
                                                <asp:Button ID="Btn_Busqueda_Empleados" runat="server"  Text="Busqueda de Empleado" CssClass="button"  
                                                    CausesValidation="false" Width="200px" TabIndex="12" OnClick="Btn_Busqueda_Empleados_Click"/> 
                                            </center>
                                        </td>                                                     
                                    </tr>
                                    <tr align="center">
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <asp:GridView ID="Grid_Busqueda_Empleados" runat="server" AllowPaging="True" Width="100%"
                                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" PageSize="5" AllowSorting="true"
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


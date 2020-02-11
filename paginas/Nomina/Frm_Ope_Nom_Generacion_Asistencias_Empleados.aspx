<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Generacion_Asistencias_Empleados.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Generacion_Asistencias_Empleados" Title="Generación de Asistencias de Empleados" %>
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
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Areas" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>        
            <asp:UpdateProgress ID="Uprg_Reloj_Checador" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Reloj_Checador" style="background-color:#ffffff; width:100%; height:100%;">    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Generacion de Asistencias de Empleados</td>
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
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                                        CssClass="Img_Button" TabIndex="4"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click" /> 
                                                </ContentTemplate>
                                            </asp:UpdatePanel>                                                
                                        </td>
                                        <td align="right" style="width:41%;"></td>       
                                    </tr>         
                                </table>                      
                            </div>
                        </td>
                    </tr>
                </table>           
                <br />            
                <table width="98%" class="estilo_fuente">     
                    <tr>
                        <tr>
                            <td style="width:20%;text-align:left;">Fecha Inicio</td>
                            <td style="width:30%;text-align:left;">
                                <asp:TextBox ID="Txt_Fecha_Entrada_Asistencia" runat="server" Width="83%" TabIndex="7"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Fecha_Entrada_Asistencia" runat="server" TargetControlID="Txt_Fecha_Entrada_Asistencia"
                                    FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                                <cc1:CalendarExtender ID="DTP_Fecha_Entrada_Asistencia" runat="server" PopupButtonID="Btn_Fecha_Entrada_Asistencia"
                                    TargetControlID="Txt_Fecha_Entrada_Asistencia" Format="dd/MMM/yyyy" OnClientShown="calendarShown"/>
                                <asp:ImageButton ID="Btn_Fecha_Entrada_Asistencia" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha de Inicio"/> 
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
                            <td style="width:20%;text-align:left;">Fecha Termino</td>
                            <td style="width:30%;text-align:left;">
                                 <asp:TextBox ID="Txt_Fecha_Salida_Asistencia" runat="server" Width="83%" TabIndex="9"></asp:TextBox>
                                 <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fecha_Salida_Asistencia" runat="server" TargetControlID="Txt_Fecha_Salida_Asistencia"
                                    FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                                <cc1:CalendarExtender ID="DTP_Fecha_Salida" runat="server" PopupButtonID="Btn_Fecha_Salida_Asistencia"
                                    TargetControlID="Txt_Fecha_Salida_Asistencia" Format="dd/MMM/yyyy" OnClientShown="calendarShown"/>
                                <asp:ImageButton ID="Btn_Fecha_Salida_Asistencia" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                    ToolTip="Seleccione la Fecha de Termino"/> 
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

                    </tr>               
                    <tr>
                        <td colspan="2" style="text-align:center;width:50%;">
                            <asp:ImageButton ID="Btn_Generar_Asistencias_Empleados" runat="server" 
                                ToolTip="Sincronizar" Width="200px" Height="30px" TabIndex="2"
                                ImageUrl="~/paginas/imagenes/paginas/Generacion_Asistencias.jpg" 
                                onclick="Btn_Generar_Asistencias_Empleados_Click"/>
                        </td>
                        <td colspan="2" style="text-align:center;width:50%;">
                            <asp:ImageButton ID="Btn_Registrar_Asistencias" runat="server" ToolTip="Registrar" 
                                Width="200px" Height="30px" TabIndex="1" onclick="Btn_Registrar_Asistencias_Click"
                                ImageUrl="~/paginas/imagenes/paginas/Registro_Asistencias.jpg" />
                        </td>
                    </tr>
                </table>
                <br />                
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">                                      
                    <tr align="center">
                        <td>
                            <asp:GridView ID="Grid_Lista_Asistencias" runat="server" AllowPaging="True" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="5" GridLines="None" Width="100%"
                                onpageindexchanging="Grid_Lista_Asistencias_PageIndexChanging"
                                AllowSorting="True" HeaderStyle-CssClass="tblHead">
                                <Columns>
                                    <asp:BoundField DataField="EMPLEADO_ID" HeaderText="Empleado ID" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RELOJ_CHECADOR_ID" HeaderText="Dependencia_ID" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_EMPLEADO" HeaderText="No Empleado" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="23%" />
                                        <ItemStyle HorizontalAlign="Left" Width="23%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CLAVE" HeaderText="Retardo" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="23%" />
                                        <ItemStyle HorizontalAlign="Left" Width="23" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA_HORA_ENTRADA" HeaderText="Fecha" Visible="True"
                                        DataFormatString="{0:dd/MMM/yyyy HH:mm:ss}">
                                        <HeaderStyle HorizontalAlign="Left" Width="23%" />
                                        <ItemStyle HorizontalAlign="Left" Width="23%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA_HORA_SALIDA" HeaderText="Fecha" Visible="True"
                                        DataFormatString="{0:dd/MMM/yyyy HH:mm:ss}">
                                        <HeaderStyle HorizontalAlign="Left" Width="23%" />
                                        <ItemStyle HorizontalAlign="Left" Width="23%" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


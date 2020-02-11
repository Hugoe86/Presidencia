<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Empleado_Reloj_Checador.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Empleado_Reloj_Checador" Title="Empleados Datos Reloj Checador" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
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
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Empleado_Reloj_Checador" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <%--<asp:UpdateProgress ID="Uprg_Empleado_Reloj_Checador" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>--%>
            <div id="Div_Asistencias_Personal" style="background-color:#ffffff; width:98%; height:100%;">
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Empleado Datos Reloj Checador</td>
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
                                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                                        CssClass="Img_Button" TabIndex="1"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                                        onclick="Btn_Modificar_Click"/>
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                                        CssClass="Img_Button" TabIndex="2"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                                        onclick="Btn_Salir_Click"/>
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
                                                                        ToolTip="Busqueda Avanzada" TabIndex="3"
                                                                        ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px" Width="24px"
                                                                        OnClientClick="javascript:return Abrir_Modal_Popup();" 
                                                                        CausesValidation="false"/>
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
                                        <asp:TextBox ID="Txt_No_Empleado" runat="server" ReadOnly="True" Width="90%" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                    <td style="width:20%;text-align:left;">Estatus</td>
                                    <td style="width:30%;text-align:left;">&nbsp;
                                        <asp:TextBox ID="Txt_Estatus_Empleado" Width="90%" runat="server" ReadOnly="True" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">Nombre</td>
                                    <td colspan="3" style="width:80%;text-align:left;">
                                        <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" ReadOnly="true" Width="98%" BorderStyle="Solid" BorderWidth="1"></asp:TextBox> 
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">Unidad Responsable</td>
                                    <td colspan="3" style="width:80%;text-align:left;">
                                        <asp:TextBox ID="Txt_Unidad_Responsable" runat="server" ReadOnly="true" Width="98%" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:UpdatePanel ID="Upnl_Datos_Reloj_Checador" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="Pnl_Datos_Reloj_Checador" runat="server" GroupingText="Datos Reloj Checador" Width="97%" BackColor="White">
                            <table style="width:100%;">
                                <tr>
                                   <td style="text-align:left;width:20%;">*Checa Asistencia</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:DropDownList ID="Cmb_Checa_Asistencia" runat="server" Width="93%" AutoPostBack="true"
                                            TabIndex="4" onselectedindexchanged="Cmb_Checa_Asistencia_SelectedIndexChanged">
                                            <asp:ListItem>&lt;- Seleccione -&gt;</asp:ListItem>
                                            <asp:ListItem>SI</asp:ListItem>
                                            <asp:ListItem>NO</asp:ListItem>
                                        </asp:DropDownList>                                          
                                    </td>                                    
                                    <td style="width:20%;text-align:left;">Fecha Inicio Checada</td>
                                    <td style="width:30%;text-align:left;">
                                        <asp:TextBox ID="Txt_Fecha_Inicio_Reloj_Checador" runat="server" Width="83%" TabIndex="5"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fecha_Inicio_Reloj_Checador" runat="server" TargetControlID="Txt_Fecha_Inicio_Reloj_Checador"
                                            FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                                        <cc1:CalendarExtender ID="DTP_Fecha_Inicio_Reloj_Checador" runat="server" PopupButtonID="Btn_Fecha_Inicio_Reloj_Checador"
                                            TargetControlID="Txt_Fecha_Inicio_Reloj_Checador" Format="dd/MMM/yyyy" OnClientShown="calendarShown"/>
                                        <asp:ImageButton ID="Btn_Fecha_Inicio_Reloj_Checador" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                            ToolTip="Seleccione la Fecha de Inicio de Checada"/> 
                                        <cc1:MaskedEditExtender 
                                            ID="MEE_Txt_Fecha_Inicio_Reloj_Checador"
                                            Mask="99/LLL/9999" 
                                            runat="server"
                                            MaskType="None" 
                                            UserDateFormat="DayMonthYear" 
                                            UserTimeFormat="None" Filtered="/"
                                            TargetControlID="Txt_Fecha_Inicio_Reloj_Checador" 
                                            Enabled="True" 
                                            ClearMaskOnLostFocus="false"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">Reloj Checador</td>
                                    <td>
                                        <asp:TextBox ID="Txt_Clave_Reloj_Checador" MaxLength="20" runat="server" Width="80%" TabIndex="6" AutoPostBack="true"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Clave_Reloj_Checador" runat="server" 
                                            FilterType="Custom, LowercaseLetters, Numbers, UppercaseLetters"
                                            TargetControlID="Txt_Clave_Reloj_Checador" ValidChars="áéíóúÁÉÍÓÚ "/>
                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Clave_Reloj_Checador" runat="server" WatermarkCssClass="watermarked"
                                            TargetControlID ="Txt_Clave_Reloj_Checador" WatermarkText="Busqueda por Clave" />
                                        <asp:ImageButton ID="Btn_Buscar_Reloj_Checador" runat="server" TabIndex="6" ToolTip="Consultar Reloj"
                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Reloj_Checador_Click"/>        
                                    </td>
                                    <td colspan="2" style="width:50%;text-align:left;">
                                        <asp:DropDownList ID="Cmb_Reloj_Checador_Asistencia" runat="server" Width="98%" 
                                            TabIndex="7" AutoPostBack="true" 
                                            onselectedindexchanged="Cmb_Reloj_Checador_Asistencia_SelectedIndexChanged"></asp:DropDownList>
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
                        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Empleado_Reloj_Checador" runat="server">
                            <ContentTemplate>
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Empleado_Reloj_Checador" runat="server" AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Empleado_Reloj_Checador" DisplayAfter="0">
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
                                                HeaderStyle-CssClass="tblHead" onselectedindexchanged="Grid_Busqueda_Empleados_SelectedIndexChanged" >
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


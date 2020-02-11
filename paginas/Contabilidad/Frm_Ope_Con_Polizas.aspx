<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Con_Polizas.aspx.cs" Inherits="paginas_Contabilidad_Frm_Ope_Con_Polizas" Title="Polizas" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

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
            document.getElementById("<%=Txt_No_Poliza_PopUp.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Anio_Poliza.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Tipo_Poliza.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Mes_Poliza.ClientID%>").value="";
            return false;
        }  
        function Limpiar_Carga()
        {
            document.getElementById("<%=Txt_Num_Polizas.ClientID%>").value="";
        }
        function Limpiar_Autorizar_Password()
        {
            document.getElementById("<%=Txt_No_Empleado_Popup.ClientID%>").value="";
            document.getElementById("<%=Txt_Password_Popup.ClientID%>").value="";
        }
        function Abrir_Modal_Popup() 
        {
            Limpiar_Ctlr();
            $find('Busqueda_Polizas').show();
            return false;
        }
        function Abrir_Carga_PopUp() 
        {
            Limpiar_Carga();
            $find('Carga_Masiva').show();
            return false;
        }
        function Abrir_Autorizar_Password() 
        {
            Limpiar_Autorizar_Password();
            $find('Autorizar_Password').show();
            return false;
        }
        function pageLoad() { $('[id*=Txt_Comen').keyup(function() {var Caracteres =  $(this).val().length;if (Caracteres > 250) {this.value = this.value.substring(0, 250);$(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});}
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Polizas" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>        
           <asp:UpdateProgress ID="Uprg_Polizas" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Polizas" style="background-color:#ffffff; width:98%; height:100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Pólizas</td>
                    </tr>
                    <tr>
                        <td >&nbsp;
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
                        <td>                
                            <div align="right" class="barra_busqueda">                        
                                <table style="width:100%;height:28px;">
                                    <tr>
                                        <td align="left" style="width:59%;"> 
                                            <asp:UpdatePanel ID="Upnl_Botones_Operacion" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
                                                    <ContentTemplate> 
                                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                                        CssClass="Img_Button" TabIndex="1"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                                        onclick="Btn_Nuevo_Click" />
                                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                                        CssClass="Img_Button" TabIndex="2"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                                        onclick="Btn_Modificar_Click" />
                                                    <asp:ImageButton ID="Btn_Copiar" runat="server" CssClass="Img_Button" 
                                                        ImageUrl="~/paginas/imagenes/paginas/subir.png" TabIndex="3" 
                                                        ToolTip="Carga Masiva" OnClientClick="javascript:return Abrir_Carga_PopUp();" CausesValidation="false"/>
                                                        <cc1:ModalPopupExtender ID="Mpe_Carga_Masiva" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="Carga_Masiva"
                                                            PopupControlID="Pnl_Carga_Masiva" TargetControlID="Btn_Open" PopupDragHandleControlID="Pnl_Carga_Masiva_Cabecera" 
                                                            CancelControlID="Btn_Cerrar" DropShadow="True" DynamicServicePath="" Enabled="True"/>  
                                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" 
                                                        CssClass="Img_Button" TabIndex="3"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                                        OnClientClick="return confirm('¿Está seguro de eliminar la Póliza seleccionada?');" 
                                                        onclick="Btn_Eliminar_Click"/>
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                                        CssClass="Img_Button" TabIndex="4"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                                        onclick="Btn_Salir_Click" />
                                                    <asp:Button Style="background-color: transparent; border-style:none; visibility:hidden" ID="Btn_Cerrar" runat="server" Text="" />
                                                    <asp:Button  Style="background-color: transparent; border-style:none; visibility:hidden" ID="Btn_Open" runat="server" Text="" />
                                                    <asp:Button  Style="border-style:none; visibility:visible" ID="Btn_Password" 
                                                        runat="server" Text="Password" 
                                                        OnClientClick="javascript:return Abrir_Autorizar_Password();" 
                                                        CausesValidation="false" onclick="Btn_Password_Click"/>
                                                    <cc1:ModalPopupExtender ID="Mpe_Autorizar_Password" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="Autorizar_Password"
                                                            PopupControlID="Pnl_Password" TargetControlID="Btn_Open" PopupDragHandleControlID="Pnl_Password_Cabecera" 
                                                            CancelControlID="Btn_Cerrar" DropShadow="True" DynamicServicePath="" Enabled="True"/>  
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
                                                                    <asp:ImageButton ID="Btn_Mostrar_Popup_Busqueda" runat="server" ToolTip="Busqueda Avanzada" TabIndex="23" 
                                                                        ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px" Width="24px"
                                                                        OnClientClick="javascript:return Abrir_Modal_Popup();" CausesValidation="false" />
                                                                    <cc1:ModalPopupExtender ID="Mpe_Busqueda_Polizas" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="Busqueda_Polizas"
                                                                        PopupControlID="Pnl_Busqueda_Contenedor" TargetControlID="Btn_Comodin_Open" PopupDragHandleControlID="Pnl_Busqueda_Cabecera" 
                                                                        CancelControlID="Btn_Comodin_Close" DropShadow="True" DynamicServicePath="" Enabled="True"/>  
                                                                    <asp:Button Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Close" runat="server" Text="" />
                                                                    <asp:Button  Style="background-color: transparent; border-style:none;" ID="Btn_Comodin_Open" runat="server" Text="" />                                                                                                    
                                                            </ContentTemplate>
                                                            <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="Btn_Mostrar_Popup_Busqueda" EventName ="Click" />
                                                            </Triggers>
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
                <asp:UpdatePanel ID="Upnl_Generales_Poliza" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>             
                        <asp:Panel ID="Pnl_Datos_Generales" runat="server" GroupingText="Datos de la Póliza" Width="98%" BackColor="white">
                            <table width="100%" class="estilo_fuente">
                                <tr>
                                    <td width="70px">*Tipo Poliza</td>
                                    <td width="200px">
                                        <asp:DropDownList ID="Cmb_Tipo_Poliza" runat="server" TabIndex="5" Width="98%">
                                        </asp:DropDownList>
                                    </td>
                                    <td width="70px">No Poliza</td>
                                    <td width ="100px">
                                        <asp:TextBox ID="Txt_No_Poliza" runat="server" Enabled="false"></asp:TextBox></td>
                                    <td width="70px">*Fecha</td>
                                    <td width ="200px">
                                        <asp:TextBox ID="Txt_Fecha_Poliza" runat="server" Width="80%" TabIndex="6" MaxLength="11" Height="18px" />
                                        <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Poliza" runat="server" 
                                            TargetControlID="Txt_Fecha_Poliza" WatermarkCssClass="watermarked" 
                                            WatermarkText="Dia/Mes/Año" Enabled="True" />
                                        <cc1:CalendarExtender ID="CE_Txt_Fecha_Poliza" runat="server" 
                                            TargetControlID="Txt_Fecha_Poliza" Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Fecha_Poliza"/>
                                         <asp:ImageButton ID="Btn_Fecha_Poliza" runat="server"
                                            ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                            Height="18px" CausesValidation="false"/>           
                                        <cc1:MaskedEditExtender 
                                            ID="Mee_Txt_Fecha_Poliza" 
                                            Mask="99/LLL/9999" 
                                            runat="server"
                                            MaskType="None" 
                                            UserDateFormat="DayMonthYear" 
                                            UserTimeFormat="None" Filtered="/"
                                            TargetControlID="Txt_Fecha_Poliza" 
                                            Enabled="True" 
                                            ClearMaskOnLostFocus="false"/>  
                                        <cc1:MaskedEditValidator 
                                            ID="Mev_Txt_Fecha_Poliza" 
                                            runat="server" 
                                            ControlToValidate="Txt_Fecha_Poliza"
                                            ControlExtender="Mee_Txt_Fecha_Poliza" 
                                            EmptyValueMessage="Fecha Requerida"
                                            InvalidValueMessage="Fecha Póliza Invalida" 
                                            IsValidEmpty="false" 
                                            TooltipMessage="Ingrese o Seleccione la Fecha de Póliza"
                                            Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>  
                                    </td>
                                </tr>               
                                <tr>
                                    <td width="70px">*Concepto</td>
                                    <td colspan="5">
                                        <asp:TextBox ID="Txt_Concepto_Poliza" runat="server" Width="98%" MaxLength="80"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>        
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="Upnl_Partidas_Polizas" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Txt_Cuenta_Contable"  EventName="Textchanged"/>
                    <asp:AsyncPostBackTrigger ControlID="Cmb_Descripcion" EventName="Selectedindexchanged"/>
                    <%--<asp:AsyncPostBackTrigger ControlID="Cmb_Unidad_Responsable" EventName="Selectedindexchanged" />
                    <asp:AsyncPostBackTrigger ControlID="Cmb_Programa" EventName="Selectedindexchanged" />
                    <asp:AsyncPostBackTrigger ControlID="Cmb_Fuente_Financiamiento" EventName="Selectedindexchanged" />
                    <asp:AsyncPostBackTrigger ControlID="Cmb_Area_Funcional"  EventName="Selectedindexchanged" />
                    <asp:AsyncPostBackTrigger ControlID="Cmb_Afectable"  EventName="Selectedindexchanged" />
                    <asp:AsyncPostBackTrigger ControlID="Cmb_No_Compromiso" EventName="Selectedindexchanged" />--%>
                    <asp:AsyncPostBackTrigger ControlID="Btn_Salir"  EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="Btn_Nuevo" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="Btn_Modificar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="Btn_Copiar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="Btn_Eliminar"  EventName="Click" />
                    <asp:AsyncPostBackTrigger controlID="Btn_Agregar_Partida" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="Txt_Empleado_Autorizo" EventName="Textchanged" />
                    <asp:AsyncPostBackTrigger  ControlID="Grid_Polizas" EventName="Selectedindexchanged"/>
                    <asp:AsyncPostBackTrigger ControlID="Grid_Detalles_Poliza" EventName="Selectedindexchanged" />
                </Triggers>
                    <ContentTemplate>
                        <asp:Panel ID="Pnl_Partidas_Polizas" runat="server" GroupingText="Partidas Contables" Width="98%" BackColor="White">
                            <table width="100%" class="estilo_fuente">
                                <tr>
                                    <td width="30%">Descripción</td>
                                    <td width="20%">Cuenta</td>
                                    <td width="25%">Concepto</td>
                                    <td width="10%">Debe</td>
                                    <td width="10%">Haber</td>
                                    <td width="5%"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="Cmb_Descripcion" runat="server" width="98%" 
                                            onselectedindexchanged="Cmb_Descripcion_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Cuenta_Contable" runat="server" width="98%" 
                                            ontextchanged="Txt_Cuenta_Contable_TextChanged" AutoPostBack ="true"></asp:TextBox> 
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Cuenta_Contable" FilterType="Custom" ValidChars="1234567890-"></cc1:FilteredTextBoxExtender>                                       
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Concepto_Partida" runat="server" width="98%"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Debe_Partida" runat="server" width="98%" ontextchanged="Txt_Debe_Partida_TextChanged" AutoPostBack ="true" Enabled="false"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="Txt_Debe_Partida" FilterType="Custom" ValidChars="1234567890."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Haber_Partida" runat="server" width="98%" ontextchanged="Txt_Haber_Partida_TextChanged" AutoPostBack ="true" Enabled="false"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="Txt_Haber_Partida" FilterType="Custom" ValidChars="1234567890."></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td align="center">
                                        <asp:ImageButton ID="Btn_Agregar_Partida" runat="server" ToolTip="Agregar" 
                                            CssClass="Img_Button" TabIndex="16"
                                            ImageUrl="~/paginas/imagenes/gridview/add_grid.png" 
                                            onclick="Btn_Agregar_Partida_Click" />                                            
                                    </td>
                                </tr>
                                </table>
<%--                            <asp:Panel ID="Panel1" runat="server" GroupingText="Afectacion Presupuestal" Width="98%" BackColor="White">
                                <table width="100%" class="estilo_fuente">
                                <tr>
                                    <td width="25%">Unidad Responsable</td>
                                    <td width="25%">Programa</td>
                                    <td width="25%" colspan="2">Fuente de Financiamiento</td>
                                    <td width="25%" colspan="2">Area Funcional</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="Cmb_Unidad_Responsable" runat="server" width="100%" 
                                            onselectedindexchanged="Cmb_Unidad_Responsable_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Cmb_Programa" runat="server" width="100%" 
                                            onselectedindexchanged="Cmb_Programa_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="Cmb_Fuente_Financiamiento" runat="server" width="100%" 
                                            onselectedindexchanged="Cmb_Fuente_Financiamiento_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="Cmb_Area_Funcional" runat="server" width="100%" 
                                            AutoPostBack="true" 
                                            onselectedindexchanged="Cmb_Area_Funcional_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Clave de Partida Presupuestal </td>
                                    <td> Partida Presupuestal</td>
                                    <td width="15%">Afectar</td>
                                    <td width="15%" colspan="2"><asp:Label ID="Lbl_compromiso" runat="server" Text="No Compromiso"></asp:Label></td>
                                    <td width="15%">Monto</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="Txt_Clave_Presupuestal" runat="server" width="98%" AutoPostBack ="true" ReadOnly="true" Enabled="false"></asp:TextBox> 
                                        <asp:TextBox ID="Txt_Partida_ID" runat="server" width="98%" AutoPostBack ="true" ReadOnly="true" Enabled="false" Visible="false"></asp:TextBox> 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Nombre_Presupuestal" runat="server" width="98%" AutoPostBack ="true" ReadOnly="true" Enabled="false"></asp:TextBox> 
                                    </td> 
                                    <td>
                                        <asp:DropDownList ID="Cmb_Afectable" runat="server" width="98%" 
                                            AutoPostBack="true" 
                                            onselectedindexchanged="Cmb_Afectable_SelectedIndexChanged" Enabled="False">
                                            <asp:ListItem>&lt;- Seleccione -&gt;</asp:ListItem>
                                            <asp:ListItem>Disponible</asp:ListItem>
                                            <asp:ListItem>Compromiso</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="Txt_No_Compromiso" runat="server" width="98%" AutoPostBack ="true" ReadOnly="true" Enabled="false" Visible="true"></asp:TextBox> 
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="Cmb_No_Compromiso" runat="server" width="98%" 
                                            AutoPostBack="true" Enabled="false" 
                                            onselectedindexchanged="Cmb_No_Compromiso_SelectedIndexChanged"/>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="Cbx_Comprometido" runat="server" AutoPostBack="true" Enabled="false"
                                            oncheckedchanged="Cbx_Comprometido_CheckedChanged" Width="10%" TextAlign="Left"/>
                                        <asp:TextBox ID="Txt_Monto_Comprometido" runat="server" width="98%" ReadOnly="true" Enabled="false" Visible="true"></asp:TextBox> 
                                        <asp:TextBox ID="Txt_Monto_Disponible" runat="server" width="98%" ReadOnly="true" Enabled="false" Visible="false"></asp:TextBox> 
                                    </td>
                                </tr>
                                </table>
                                </asp:Panel>--%>
                            <table width="100%" class="estilo_fuente">
                                <tr>
                                    <td style="width:100%;text-align:center;vertical-align:top;"> 
                                        <center>
                                            <div style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" >
                                                <asp:GridView ID="Grid_Detalles_Poliza" runat="server" 
                                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                                    OnRowDataBound="Grid_Detalles_Poliza_RowDataBound" Width="100%">
                                                    <Columns>
                                                        <asp:BoundField DataField="PARTIDA" HeaderText="No">
                                                            <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CUENTA_CONTABLE_ID" HeaderText="Cuenta Contable ID">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                       <%-- <asp:BoundField DataField="DEPENDENCIA_ID" HeaderText="Dependencia">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="FUENTE_FINANCIAMIENTO_ID" 
                                                            HeaderText="Financiamiento">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="AREA_FUNCIONAL_ID" HeaderText="Area Funcional">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="PROYECTO_PROGRAMA_ID" HeaderText="Programa">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="PARTIDA_ID" HeaderText="Partida">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="COMPROMISO_ID" HeaderText="Compromiso">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CODIGO_PROGRAMATICO" 
                                                            HeaderText="Codigo Programatico">
                                                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                        </asp:BoundField>--%>
                                                        <asp:BoundField DataField="CUENTA" HeaderText="Cuenta">
                                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CONCEPTO" HeaderText="Concepto">
                                                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DEBE" HeaderText="Debe">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="HABER" HeaderText="Haber">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:ImageButton ID="Btn_Eliminar_Partida" runat="server" 
                                                                        CausesValidation="false" 
                                                                        ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" 
                                                                        OnClick="Btn_Eliminar_Partida" 
                                                                        OnClientClick="return confirm('¿Está seguro de eliminar de la tabla la partida de la poliza seleccionada?');" />
                                                                </center>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>
                                            </div>
                                        </center>                                       
                                    </td>
                                </tr>
                            </table>
                            <table width="100%" class="estilo_fuente">
                                <tr>
                                    <td width="100px">No. Partidas</td>
                                    <td width="100px">
                                        <asp:TextBox ID="Txt_No_Partidas" runat="server" Enabled="false" Width="98%"></asp:TextBox></td>
                                    <td width="100px">Acumulado</td>
                                    <td width="100px">
                                        <asp:TextBox ID="Txt_Total_Debe" runat="server" Enabled="false" Width="98%" CssClass=""></asp:TextBox></td>
                                    <td width="100px">
                                        <asp:TextBox ID="Txt_Total_Haber" runat="server" Enabled="false" Width="98%"></asp:TextBox></td>
                                    <td width="30px"></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Panel ID="Panel2" runat="server" GroupingText="Empleado elabora" 
                                            Width="98%" BackColor="White">
                                            <asp:TextBox ID="Txt_Empleado_Creo" runat="server"  Width="100%"></asp:TextBox>
                                            <%--<asp:DropDownList ID="Cmb_Empleado_Creo" runat="server" AutoPostBack="true" width="100%"></asp:DropDownList>--%></asp:Panel>
                                    </td>
                                    <td colspan="3">
                                        <asp:Panel ID="Panel3" runat="server" GroupingText="Empleado autoriza" Width="100%" BackColor="White">
                                            <asp:TextBox ID="Txt_Empleado_Autorizo" runat="server" Width="48%" CssClass="" ontextchanged="Txt_Empleado_Autorizo_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Empleado_Autorizo" runat="server" WatermarkCssClass="watermarked"
                                                WatermarkText="No. Empleado o Nombre" TargetControlID="Txt_Empleado_Autorizo" />
                                            <asp:DropDownList ID="Cmb_Nombre_Empleado" runat="server" AutoPostBack="true" width="48%"></asp:DropDownList></asp:Panel>
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                            
                            <%--********************************************************
                            ********************************************************
                            ********************************************************--%></asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>      
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <%--*************BUSQUEDA****************************************
    *************************************************************--%>
    <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="650px" 
    style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" 
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table width="99%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;">
                   <asp:Image ID="Img_Informatcion_Autorizacion" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     B&uacute;squeda: Polizas
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
                        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Polizas" runat="server" UpdateMode="Conditional" >                                                           
                            <ContentTemplate> 
                         <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Polizas" DisplayAfter="0" >
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                                        <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress"><img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>                                                           
                                  <table width="100%">
                                   <tr>
                                        <td colspan="4">

                                            <table style="width:80%;">

                                              <tr>

                                                <td align="left">

                                                  <asp:ImageButton ID="Img_Error_Busqueda" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 

                                                    Width="24px" Height="24px" Visible=false />

                                                    <asp:Label ID="Lbl_Error_Busqueda" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" Visible=false />

                                                </td>            

                                              </tr>         

                                            </table>  

                                        </td>
                                   </tr>
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
                                           Numero de Poliza:
                                        </td>
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_No_Poliza_PopUp" runat="server" Width="98%" MaxLength="5"/>
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Poliza_PopUp" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_No_Poliza_PopUp"/>  
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_No_Poliza_PopUp" runat="server" 
                                                TargetControlID ="Txt_No_Poliza_PopUp" WatermarkText="Busqueda por No. de Poliza" 
                                                WatermarkCssClass="watermarked"/>                                                                                                                                    
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                           Tipo de Poliza 
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Tipo_Poliza" runat="server" Width="100%">   
                                            </asp:DropDownList>                                          
                                        </td>             
                                    </tr>                                                                                                   
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                           *Mes 
                                        </td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Mes_Poliza" runat="server" Width="100%">   
                                                <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>                                         
                                                <asp:ListItem>ENERO</asp:ListItem>
                                                <asp:ListItem>FEBRERO</asp:ListItem>
                                                <asp:ListItem>MARZO</asp:ListItem>
                                                <asp:ListItem>ABRIL</asp:ListItem>
                                                <asp:ListItem>MAYO</asp:ListItem>
                                                <asp:ListItem>JUNIO</asp:ListItem>
                                                <asp:ListItem>JULIO</asp:ListItem>
                                                <asp:ListItem>AGOSTO</asp:ListItem>
                                                <asp:ListItem>SEPTIEMBRE</asp:ListItem>
                                                <asp:ListItem>OCTUBRE</asp:ListItem>
                                                <asp:ListItem>NOVIEMBRE</asp:ListItem>
                                                <asp:ListItem>DICIEMBRE</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            *Año
                                        </td>              
                                        <td style="width:30%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Anio_Poliza" runat="server" Width="100%">   
                                                <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>                                         
                                                <asp:ListItem>2010</asp:ListItem>
                                                <asp:ListItem>2011</asp:ListItem>
                                                <asp:ListItem>2012</asp:ListItem>
                                                <asp:ListItem>2013</asp:ListItem>
                                                <asp:ListItem>2014</asp:ListItem>
                                                <asp:ListItem>2015</asp:ListItem>
                                                <asp:ListItem>2016</asp:ListItem>
                                                <asp:ListItem>2017</asp:ListItem>
                                                <asp:ListItem>2018</asp:ListItem>
                                                <asp:ListItem>2019</asp:ListItem>
                                                <asp:ListItem>2020</asp:ListItem>
                                                <asp:ListItem>2021</asp:ListItem>
                                                <asp:ListItem>2022</asp:ListItem>
                                                <asp:ListItem>2023</asp:ListItem>
                                                <asp:ListItem>2024</asp:ListItem>
                                                <asp:ListItem>2025</asp:ListItem>
                                                <asp:ListItem>2026</asp:ListItem>
                                                <asp:ListItem>2027</asp:ListItem>
                                                <asp:ListItem>2028</asp:ListItem>
                                                <asp:ListItem>2029</asp:ListItem>
                                                <asp:ListItem>2030</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>                                                                         
                                   <tr>
                                        <td style="width:100%" colspan ="4">
                                        <asp:GridView ID="Grid_Polizas" runat="server" AllowPaging="True" CssClass="GridView_1" 
                                                    AutoGenerateColumns="False" PageSize="5" GridLines="None" Width="99.9%"
                                                    onpageindexchanging="Grid_Polizas_PageIndexChanging"  
                                                    onselectedindexchanged="Grid_Polizas_SelectedIndexChanged"
                                                    AllowSorting="True" HeaderStyle-CssClass="tblHead">
                                                    <Columns>         
                                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                            <ItemStyle Width="7%" />
                                                        </asp:ButtonField>                       
                                                        <asp:BoundField DataField="No_Poliza" HeaderText="No Poliza" 
                                                             SortExpression="No_Poliza">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Tipo_Poliza_ID" HeaderText="Tipo Poliza" SortExpression="Tipo_Poliza">
                                                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Descripcion" HeaderText="Tipo Poliza" SortExpression="Tipo_Poliza">
                                                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Fecha_Poliza" HeaderText="Fecha" 
                                                            SortExpression="Fecha_Poliza" DataFormatString="{0:dd/MMM/yyyy}">
                                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Concepto" HeaderText="Concepto" 
                                                            Visible="True" SortExpression="Concepto">
                                                            <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                                            <ItemStyle HorizontalAlign="left" Width="40%" />
                                                        </asp:BoundField>                                    
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <HeaderStyle CssClass="tblHead" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>
                                            <hr />
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <center>
                                               <asp:Button ID="Btn_Busqueda_Poliza_Popup" runat="server"  Text="Busqueda de Polizas" CssClass="button"  
                                                CausesValidation="false" OnClick="Btn_Busqueda_Poliza_Popup_Click" Width="200px"/> 
                                            </center>
                                        </td>                                                     
                                    </tr>                                                                        
                                  </table>                                                                                                                                                                 
                            </ContentTemplate>   
                            <Triggers>
                                 <asp:AsyncPostBackTrigger  ControlID="Btn_Busqueda_Poliza_Popup"  EventName="Click" />
                            </Triggers>                                                         
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
    
    <%--*************CARGA*MASIVA************************************
    *************************************************************--%>
    
    <asp:Panel ID="Pnl_Carga_Masiva" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="650px" 
        style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
        <asp:Panel ID="Pnl_Carga_Masiva_Cabecera" runat="server" 
            style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
            <table width="99%">
                <tr>
                    <td style="color:Black;font-size:12;font-weight:bold;">
                       <asp:Image ID="Image1" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                         Carga Masiva Polizas
                    </td>
                    <td align="right" style="width:10%;">
                       <asp:ImageButton ID="ImageButton1" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                            ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClick="Btn_Cerrar_Ventana_Carga_Click"/>  
                    </td>
                </tr>
            </table>            
        </asp:Panel>                                                                          
        <div style="color: #5D7B9D">
             <table width="100%">
                <tr>
                    <td align="left" style="text-align: left;" >                                    
                        <asp:UpdatePanel ID="Upnl_Carga_Masiva" runat="server">
                            <ContentTemplate>                            
                                <asp:UpdateProgress ID="Progress_Upnl_Carga_Masiva" runat="server" AssociatedUpdatePanelID="Upnl_Carga_Masiva" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                                        <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress"><img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>                 
                                <table width="100%">
                                    <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                           Ruta del Archivo
                                        </td>
                                        <td style="width:80%;text-align:left;font-size:11px;">
                                           <cc1:AsyncFileUpload ID="AFU_Archivo_Excel" runat="server" size="80"
                                                ThrobberID="Throbber" onuploadedcomplete="AFU_Archivo_Excel_UploadedComplete"/>
                                        <asp:Label ID="Throbber" Text="wait" runat="server" Width="30px">                                                                     
                                            <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                                            <div  class="processMessage" id="div_progress">
                                                <center>
                                                    <img alt="" src="../Imagenes/paginas/Sias_Roler.gif" />
                                                    <br /><br />
                                                    <span id="spanUploading" runat="server" style="color:White;font-size:30px;font-weight:bold;font-family:Lucida Calligraphy;font-style:italic;">
                                                        Cargando...
                                                    </span>
                                                </center>
                                            </div>
                                        </asp:Label>  
                                        </td> 
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                           Numero de Movimientos
                                        </td>
                                        <td style="width:80%;text-align:left;">
                                            <asp:TextBox ID="Txt_Num_Polizas" runat="server" width="21%" Height="22px"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="Txt_Num_Polizas" FilterType="Custom" ValidChars="1234567890"></cc1:FilteredTextBoxExtender>
                                        </td>             
                                    </tr>                                                                                                   
                                    <tr>
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <center>
                                                <asp:Button ID="Btn_Carga_Masiva_Popup" runat="server"  Text="Capturar Polizas" CssClass="button"  
                                                CausesValidation="false" OnClick="Btn_Carga_Masiva_Popup_Click" Width="200px"/> 
                                            </center>
                                        </td>                                                     
                                    </tr>                                                                        
                                  </table>                                                                                                                                                              
                            </ContentTemplate>   
                            <Triggers>
                            <asp:AsyncPostBackTrigger EventName="Click"  ControlID ="Btn_Carga_Masiva_Popup" />
                            </Triggers>                                                                
                        </asp:UpdatePanel>
                    </td>
                </tr>
             </table>                                                   
        </div>                 
    </asp:Panel>
    
    
    <%--*************PASSWORD****************************************
    *************************************************************--%>
    <asp:Panel ID="Pnl_Password" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="400px" 
        style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
        <asp:Panel ID="Pnl_Password_Cabecera" runat="server" 
            style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
            <table width="99%">
                <tr>
                    <td style="color:Black;font-size:12;font-weight:bold;">
                       <asp:Image ID="Image2" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                         Autorizacion del Administrador
                    </td>
                    <td align="right" style="width:10%;">
                       <asp:ImageButton ID="ImageButton2" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                            ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClick="Btn_Cerrar_Ventana_Password_Click"/>  
                    </td>
                </tr>
                <tr>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" RenderMode="Inline">
                        <ContentTemplate>                         
                            <asp:Image ID="Img_Error_Password" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Error_Password" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                        </ContentTemplate>                                
                    </asp:UpdatePanel>
                </tr>
            </table>            
        </asp:Panel>                                                                          
        <div style="color: #5D7B9D">
             <table width="100%">
                <tr>
                    <td align="left" style="text-align: left;" >                                    
                        <asp:UpdatePanel ID="Upnl_Password" runat="server">
                            <ContentTemplate>                            
                                <asp:UpdateProgress ID="Progress_Upnl_Password" runat="server" AssociatedUpdatePanelID="Upnl_Password" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                                        <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress"><img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>                 
                                <table width="100%">
                                    <tr>
                                        <td width="40%" visible="false"></td>
                                        <td width="60%" visible="false"></td>
                                    </tr>
                                    <tr>
                                        <td style="width:100%" colspan="3">
                                            <hr />
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td>*Clave de Empleado</td>
                                        <td>
                                            <asp:TextBox ID="Txt_No_Empleado_Popup" runat="server" Width="98%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="Txt_No_Empleado_Popup" FilterType="Custom" ValidChars="1234567890"></cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>                                                              
                                    <tr>
                                        <td>*Contraseña</td>
                                        <td>
                                            <asp:TextBox ID="Txt_Password_Popup" runat="server" Width="98%" TextMode="Password"></asp:TextBox>
                                        </td>
                                    </tr>                                   
                                    <tr>
                                        <td style="width:100%;text-align:left;" colspan="2">
                                            <center>
                                                <asp:Button ID="Btn_Autorizar_Poliza_Popup" runat="server"  Text="Capturar Polizas" CssClass="button"  
                                                CausesValidation="false" OnClick="Btn_Autorizar_Poliza_Popup_Click" Width="200px"/> 
                                            </center>
                                        </td>                                                     
                                    </tr>                                                                        
                                  </table>                                                                                                                                                              
                            </ContentTemplate>  
                            <Triggers>
                                <asp:AsyncPostBackTrigger EventName="Click" ControlID ="Btn_Autorizar_Poliza_Popup" />
                            </Triggers>                                                              
                        </asp:UpdatePanel>
                    </td>
                </tr>
             </table>                                                   
        </div>                 
    </asp:Panel>
</asp:Content>


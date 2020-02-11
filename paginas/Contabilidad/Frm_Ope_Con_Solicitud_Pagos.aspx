<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Con_Solicitud_Pagos.aspx.cs" Inherits="paginas_Contabilidad_Frm_Ope_Con_Solicitud_Pagos" Title="Solicitud de Pagos" %>
<%@ Register Assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script src="../../easyui/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency-1.4.0.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency.all.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        //Metodo para mantener los calendarios en una capa mas alat.
        function calendarShown(sender, args)
        {
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
        //Metodos para limpiar los controles de la busqueda.
        function Limpiar_Ctlr()
        {
            document.getElementById("<%=Txt_Busqueda_No_Reserva.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_No_Solicitud_Pago.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Tipo_Solicitud.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Dependencia.ClientID%>").value="";
            document.getElementById("<%=Cmb_Busqueda_Estatus_Solicitud_Pago.ClientID%>").values="";
            document.getElementById("<%=Txt_Busqueda_Fecha_Inicio.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_Fecha_Fin.ClientID%>").value="";
            return false;
        }
        function Abrir_Modal_Popup() 
        {
            $find('Busqueda_Solicitud_Pago').show();
            return false;
        } 
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
        <ProgressTemplate>
            <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
            <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:ScriptManager ID="ScriptManager_Tipo_Solicitud_Pagos" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <div id="Div_Solicitud_Pagos" >
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Solicitud de Pagos</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>               
                    <tr class="barra_busqueda" align="right">
                        <td style="width:50%" align="left">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" 
                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                onclick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" 
                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                onclick="Btn_Modificar_Click" />
                            <%--<asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" 
                                TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"  
                                OnClientClick="return confirm('¿Está seguro de eliminar la Solicitud seleccionado?');" 
                                onclick="Btn_Eliminar_Click"/>--%>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" 
                                TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Salir_Click"/>
                        </td>
                        <td style="width:50%">
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
                                                    <cc1:ModalPopupExtender ID="Mpe_Busqueda_Solicitud_Pago" runat="server" BackgroundCssClass="popUpStyle"  BehaviorID="Busqueda_Solicitud_Pago"
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
                <asp:UpdatePanel ID="Upnl_Datos_Generales_Solicitud_Pagos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate> 
                        <asp:Panel ID="Pnl_Datos_Generales_Solicitud_Pagos" runat="server" GroupingText="Datos Generales" Width="98%" BackColor="White">
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                                <tr>
                                    <td style="width:20%">No. Solicitud</td>
                                    <td style="width:30%">
                                        <asp:TextBox ID="Txt_No_Solicitud_Pago" runat="server" ReadOnly="True" Width="236px" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>
                                    <td style="width:20%">Estatus</td>
                                    <td style="width:30%">
                                        <asp:TextBox ID="Txt_Estatus_solicitud_Pago" runat="server" ReadOnly="True" Width="236px" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%">Fecha Solicitud</td>
                                    <td style="width:30%">
                                        <asp:TextBox ID="Txt_Fecha_Solicitud_Pago" runat="server" ReadOnly="True" Width="236px" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>
                                    <td style="width:20%">*Tipo de Solicitud</td>
                                    <td style="width:30%">
                                        <asp:DropDownList ID="Cmb_Tipo_Solicitud_Pago" runat="server" TabIndex="6" Width="240px"/> 
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate> 
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="Upnl_Datos_Reserva" runat="server" UpdateMode="Conditional">
                    <ContentTemplate> 
                        <asp:Panel ID="Pnl_Datos_Reserva" runat="server" GroupingText="Datos Reserva" Width="98%" BackColor="White">
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                                <tr>
                                    <td style="width:20%">*No. Reserva</td>
                                    <td style="width:30%">
                                        <asp:DropDownList ID="Cmb_Reserva_Pago" runat="server" TabIndex="7" AutoPostBack="true"
                                            Width="240px" onselectedindexchanged="Cmb_Reserva_Pago_SelectedIndexChanged"/> 
                                    </td>
                                    <td style="width:20%">Saldo</td>
                                    <td style="width:30%">
                                        <asp:TextBox ID="Txt_Saldo_Reserva" runat="server" ReadOnly="true" Width="236px" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>
                                </tr>                            
                                <tr>
                                    <td style="width:20%">Reservado</td>
                                    <td colspan="3" style="width:80%">
                                        <asp:TextBox ID="Txt_Concepto_Reserva" runat="server" Width="99%" ReadOnly="true" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                                <tr>                                    
                                    <td style="width:33%">Fuente de Financiamiento</td>
                                    <td style="width:33%">Area Funcional</td>
                                    <td style="width:33%">Proyecto Programa</td>
                                </tr>
                                <tr>
                                    <td style="width:33%">
                                        <asp:TextBox ID="Txt_Fuente_Financiamiento_Reserva" runat="server" ReadOnly="true" Width="97%" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>
                                    <td style="width:33%">                                        
                                        <asp:TextBox ID="Txt_Area_Funcional_Reserva" runat="server" ReadOnly="true" Width="97%" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>
                                    <td style="width:33%">
                                        <asp:TextBox ID="Txt_Proyecto_Programa_Reserva" runat="server" ReadOnly="true" Width="97%" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    
                                    <td style="width:33%">Dependencia</td>
                                    <td style="width:33%">Paritda</td>
                                    <td style="width:33%">Codigo Programatico</td>
                                </tr>
                                <tr>
                                    <td style="width:33%">
                                        <asp:TextBox ID="Txt_Dependencia_Reserva" runat="server" ReadOnly="true" Width="97%" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>
                                    <td style="width:33%">
                                        <asp:TextBox ID="Txt_Partida_Reserva" runat="server" ReadOnly="true" Width="97%" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>
                                    <td style="width:33%">
                                        <asp:TextBox ID="Txt_Codigo_Programatico_Reserva" runat="server" ReadOnly="true" Width="97%" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>
                                </tr>
                                <asp:HiddenField ID="Txt_No_Reserva_Anterior" runat="server" />
                                <asp:HiddenField ID="Txt_Monto_Solicitud_Anterior" runat="server" />
                                <asp:HiddenField ID="Txt_Cuenta_Contable_ID_Proveedor" runat="server" />
                                <asp:HiddenField ID="Txt_Cuenta_Contable_ID_Proveedor_Anterior" runat="server" />
                                <asp:HiddenField ID="Txt_Cuenta_Contable_ID" runat="server" />
                                <asp:HiddenField ID="Txt_Cuenta_Contable_ID_Anterior" runat="server" />
                            </table>
                        </asp:Panel>
                    </ContentTemplate> 
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="Upnl_Datos_Solicutd_Pago" runat="server" UpdateMode="Conditional">
                    <ContentTemplate> 
                        <asp:Panel ID="Pnl_Datos_Solicutd_Pago" runat="server" GroupingText="Solicitud de Pago" Width="98%" BackColor="White">
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                                <tr>
                                    <td style="width:33%">*No. Documento</td>
                                    <td style="width:33%">*Fecha Documento</td>
                                    <td style="width:33%">*Monto Solicitado</td>
                                </tr>
                                <tr>                                    
                                    <td style="width:33%">
                                        <asp:TextBox ID="Txt_No_Factura_Solicitud_Pago" runat="server" Width="97%" TabIndex="8"></asp:TextBox>
                                    </td>
                                    <td style="width:33%">
                                        <asp:TextBox ID="Txt_Fecha_Factura_Solicitud_Pago" runat="server" MaxLength="100" Width="88%" Enabled="false"/>
                                        <cc1:CalendarExtender ID="DTP_Fecha_Factura_Solicitud_Pago" runat="server" 
                                            TargetControlID="Txt_Fecha_Factura_Solicitud_Pago" Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Fecha_Factura_Solicitud_Pago"/>
                                         <asp:ImageButton ID="Btn_Fecha_Factura_Solicitud_Pago" runat="server" TabIndex="9" 
                                            ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                            Height="18px" CausesValidation="false" Enabled="true"/> 
                                    </td>
                                    <td style="width:33%">
                                        <asp:TextBox ID="Txt_Monto_Solicitud_Pago" runat="server" Width="97%" CssClass="text_cantidades_grid" TabIndex="10"
                                            onblur="$('input[id$=Txt_Monto_Solicitud_Pago]').formatCurrency({colorize:true, region: 'es-MX'});">                                          
                                        </asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Monto_Solicitud_Pago" runat="server" 
                                            TargetControlID="Txt_Monto_Solicitud_Pago" FilterType="Custom, Numbers" ValidChars="-,."/>
                                    </td>
                                </tr>
                            </table>
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                                <tr>
                                    <td style="width:20%">*Proveedor</td>
                                    <td style="width:30%">
                                        <asp:TextBox ID="Txt_Nombre_Proveedor_Solicitud_Pago" runat="server" MaxLength="100" TabIndex="11" Width="80%"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Proveedor_Solicitud_Pago" runat="server" TargetControlID="Txt_Nombre_Proveedor_Solicitud_Pago"
                                            FilterType="Custom, UppercaseLetters, LowercaseLetters" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:ImageButton ID="Btn_Buscar_Proveedor_Solicitud_Pagos" 
                                            runat="server" ToolTip="Consultar"
                                            TabIndex="12" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                            onclick="Btn_Buscar_Proveedor_Solicitud_Pagos_Click"/>
                                    </td>
                                    <td style="width:50%">
                                        <asp:DropDownList ID="Cmb_Proveedor_Solicitud_Pago" runat="server" 
                                            TabIndex="13" AutoPostBack="true"
                                            Width="100%" 
                                            onselectedindexchanged="Cmb_Proveedor_Solicitud_Pago_SelectedIndexChanged"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%">*Concepto</td>
                                    <td colspan="2" style="width:80%">
                                        <asp:TextBox ID="Txt_Concepto_Solicitud_Pago" runat="server" MaxLength="100" TabIndex="14" Width="99%"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Concepto_Solicitud_Pago" runat="server" TargetControlID="Txt_Concepto_Solicitud_Pago"
                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>                                
                            </table>
                        </asp:Panel>
                    </ContentTemplate> 
                </asp:UpdatePanel>
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr align="center">
                        <td style="width:100%">
                            <asp:GridView ID="Grid_Solicitud_Pagos" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                PageSize="5" AllowSorting="True" HeaderStyle-CssClass="tblHead" 
                                onpageindexchanging="Grid_Solicitud_Pagos_PageIndexChanging" 
                                onselectedindexchanged="Grid_Solicitud_Pagos_SelectedIndexChanged" 
                                onsorting="Grid_Solicitud_Pagos_Sorting">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="No_Solicitud_Pago" HeaderText="No Solicitud" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Solicitud" HeaderText="Solicitud" Visible="True" SortExpression="Descripcion">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="No_Factura" HeaderText="Documento" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha_Factura" HeaderText="F. Documento" 
                                        Visible="True" DataFormatString="{0:dd/MMM/yyyy}">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Beneficiario" HeaderText="Beneficiario" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="18%" />
                                        <ItemStyle HorizontalAlign="Left" Width="18%" />
                                    </asp:BoundField>                             
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <HeaderStyle CssClass="tblHead" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>                            
                        </td>
                    </tr>
                </table>
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
                         B&uacute;squeda: Solicitud de Pagos
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
                        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Solicitud_Pagos" runat="server">
                            <ContentTemplate>
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Solicitud_Pagos" runat="server" AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Solicitud_Pagos" DisplayAfter="0">
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
                                        <td style="width:20%;text-align:left;font-size:11px;">No Reserva</td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                            <asp:TextBox ID="Txt_Busqueda_No_Reserva" runat="server" Width="98%" TabIndex="11" MaxLength="50"/>
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Reserva" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Busqueda_No_Reserva" />
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_No_Reserva" runat="server" WatermarkCssClass="watermarked"
                                                TargetControlID ="Txt_Busqueda_No_Reserva" WatermarkText="Busqueda por No Reserva" />                                                                                                                                          
                                        </td> 
                                        <td colspan="2" style="width:50%;text-align:left;font-size:11px;"></td>                     
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">No Solicitud</td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                            <asp:TextBox ID="Txt_Busqueda_No_Solicitud_Pago" runat="server" Width="98%" TabIndex="11" MaxLength="10"/>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_No_Solicitud_Pago" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Busqueda_No_Solicitud_Pago" />
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_No_Solicitud_Pago" runat="server" WatermarkCssClass="watermarked"
                                                TargetControlID ="Txt_Busqueda_No_Solicitud_Pago" WatermarkText="Busqueda por No Solicitud" />                                                                                                                                          
                                        </td> 
                                        <td colspan="2" style="width:50%;text-align:left;font-size:11px;"></td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px">Estatus</td>
                                        <td style="width:30%; text-align:left; font-size:11px">
                                            <asp:DropDownList ID="Cmb_Busqueda_Estatus_Solicitud_Pago" runat="server" Width="101%" TabIndex="2">
                                                <asp:ListItem>&lt; - Seleccione - &gt;</asp:ListItem>
                                                <asp:ListItem>PENDIENTE</asp:ListItem>
                                                <asp:ListItem>PREAUTORIZADO</asp:ListItem>
                                                <asp:ListItem>AUTORIZADO</asp:ListItem>
                                                <asp:ListItem>PAGADO</asp:ListItem>
                                                <asp:ListItem>RECHAZADO</asp:ListItem>                                                
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2" style="width:50%;text-align:left;font-size:11px;"></td>
                                    </tr>                                                                                                   
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">Tipo Solicitud</td>              
                                        <td colspan="3" style="width:80%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Tipo_Solicitud" runat="server" Width="100%" />                                                                                
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">Depedencia</td>
                                        <td colspan="3" style="width:80%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Dependencia" runat="server" Width="100%" />                                                                                
                                        </td>
                                    </tr>                                                                                                          
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">Fecha Inicio</td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Fecha_Inicio" runat="server" Width="85%" MaxLength="1" TabIndex="13" Enabled="false"/>
                                            <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Inicio" runat="server" PopupButtonID="Btn_Busqueda_Fecha_Inicio"
                                                TargetControlID="Txt_Busqueda_Fecha_Inicio" Format="dd/MMM/yyyy" OnClientShown="calendarShown"/>
                                            <asp:ImageButton ID="Btn_Busqueda_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                                ToolTip="Seleccione la Fecha"/>
                                        </td>
                                        <td style="width:20%;text-align:left;font-size:11px;">Fecha Fin</td>
                                        <td style="width:30%;text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Fecha_Fin" runat="server" Width="85%" MaxLength="1" TabIndex="14" Enabled="false"/>
                                            <cc1:CalendarExtender ID="CE_Txt_Busqueda_Fecha_Fin" runat="server" OnClientShown="calendarShown"
                                                TargetControlID="Txt_Busqueda_Fecha_Fin" PopupButtonID="Btn_Busqueda_Fecha_Fin" Format="dd/MMM/yyyy"/>
                                            <asp:ImageButton ID="Btn_Busqueda_Fecha_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                                ToolTip="Seleccione la Fecha"/> 
                                        </td>                                                            
                                    </tr>                                                                                                      
                                    <tr>
                                        <td style="width:100%" colspan="4"><hr /></td>
                                    </tr>                                    
                                    <tr>
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <center>
                                                <asp:Button ID="Btn_Busqueda_Solicitud_Pago" runat="server"  Text="Busqueda de Solicitud de Pago" CssClass="button"  
                                                    CausesValidation="false" Width="300px" TabIndex="15" OnClick="Btn_Busqueda_Solicitud_Pago_Click"/> 
                                            </center>
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


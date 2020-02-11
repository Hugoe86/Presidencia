<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Apertura_Turno.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Apertura_Turno" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script src="../../easyui/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency-1.4.0.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency.all.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server"  EnableScriptGlobalization="true" EnableScriptLocalization ="true"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Contenido" style="background-color:#ffffff; width:100%; height:100%;">                    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">Apertura de Turno</td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
               </table>            
               <table width="98%"  border="0" cellspacing="0">
                <tr align="center">
                    <td colspan="2">                
                        <div style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td align="left" style="width:50%;">
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                            CssClass="Img_Button" TabIndex="1"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                            onclick="Btn_Nuevo_Click" />
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                            OnClick="Btn_Modificar_Click" TabIndex="2"/>
                                        <asp:ImageButton ID="Btn_Reapertura_Turno" runat="server" TabIndex="3"
                                            ImageUrl="~/paginas/imagenes/paginas/sias_revisarplan.png" ToolTip="Reapertura de Turno"
                                            OnClientClick="return confirm('¿Está seguro de Abrir nuevamente el Turno?');" 
                                            onclick="Btn_Reapertura_Turno_Click" />&nbsp;
                                        <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" 
                                            CssClass="Img_Button" TabIndex="4"
                                            ImageUrl="~/paginas/imagenes/gridview/grid_print.png" 
                                            onclick="Btn_Imprimir_Click" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                            CssClass="Img_Button" TabIndex="5" 
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            onclick="Btn_Salir_Click" />
                                        <asp:TextBox ID="Txt_Caja_ID" runat="server" Visible="false" ReadOnly="True" Width="20%" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                    <td align="right" style="width:50%;">
                                        <asp:TextBox ID="Txt_Busqueda" runat="server" ToolTip = "Buscar Fecha Apertura" Width="180px" Enabled="false" 
                                            BorderStyle="Solid" BorderWidth="1"/>
                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                            WatermarkText="<Buscar Fecha Apertura>" TargetControlID="Txt_Busqueda" />
                                        <cc1:CalendarExtender ID="DTP_Fecha_Busqueda" runat="server" Enabled="true"
                                            TargetControlID="Txt_Busqueda" Format="dd/MMM/yyyy" PopupButtonID="Btn_Fecha_Busqueda"/>
                                         <asp:ImageButton ID="Btn_Fecha_Busqueda" runat="server"
                                            ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                            Height="18px" CausesValidation="false" Enabled="false"/>
                                        <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6"
                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                            ToolTip="Buscar por Fecha" onclick="Btn_Buscar_Click"/>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                </table>
                <br />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>          
                        <asp:Panel ID="Panel1" runat="server" GroupingText="Datos Generales del Cajero" Width="97%" BackColor="White">              
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">  
                                <tr>
                                    <td style="text-align:left;width:20%;">No Empleado</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_No_Empleado" runat="server" ReadOnly="True" Width="98%" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                    <td colspan="2" style="text-align:left;width:50%;">
                                        <asp:HiddenField ID="Hfd_No_Turno" runat="server" />
                                        <asp:HiddenField ID="Hfd_Fondo_Inicial" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">Nombre</td>
                                    <td colspan="3" style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" ReadOnly="True" Width="99%" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:UpdatePanel ID="Upnl_Datos_Generales_Empleado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>          
                        <asp:Panel ID="Pnl_Datos_Generales" runat="server" GroupingText="Datos Generales del Turno" Width="97%" BackColor="White"> 
                            <table width="98%" class="estilo_fuente">
                                <tr>
                                    <td style="text-align:left;width:20%;">Estatus</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Estatus_Turno" runat="server" Width="98%" ReadOnly="true" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                    <td colspan="2" style="text-align:left;width:50%;"></td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">Fecha Movimiento</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Fecha_Movimiento_Turno" runat="server" Width="98%" ReadOnly="true" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                    <td style="text-align:left;width:20%;">Fecha Aplicación</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Fecha_Aplicacion_Turno" runat="server" Width="98%" ReadOnly="true" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">Hora Apertura</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Hora_Apertura_Turno" runat="server" Width="98%" ReadOnly="true" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                    <td style="text-align:left;width:20%;">Hora Cierre</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Hora_Cierre_Turno" runat="server" Width="98%" ReadOnly="true" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:UpdatePanel ID="Upnl_Datos_Caja" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="Pnl_Datos_Caja" runat="server" GroupingText="Datos de la Caja" Width="97%" BackColor="White">
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">  
                                <tr>
                                    <td style="text-align:left;width:20%;">Módulo</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Modulo_Caja_Empleado" runat="server" Width="98%" ReadOnly="true" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                    <td style="text-align:left;width:20%;">Caja</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Caja_Empleado" runat="server" Width="98%" ReadOnly="true" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                </tr>
                                <tr>                                    
                                    <td style="text-align:left;width:20%;">*Fondo Inicial</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Fondo_Inicial_Turno" runat="server" Width="98%" MaxLength="8" TabIndex="4" CssClass="text_cantidades_grid"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fondo_Inicial_Turno" runat="server"
                                            TargetControlID="Txt_Fondo_Inicial_Turno" FilterType="Custom, Numbers" ValidChars=",."/>
                                    </td>
                                    <td style="text-align:left;width:20%;">*Recibo Inicial</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Recibo_Inicial_Turno" runat="server" Width="98%" MaxLength="10" TabIndex="5"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Recibo_Inicial_Turno" 
                                            runat="server" Enabled="True" TargetControlID="Txt_Recibo_Inicial_Turno" FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>             
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


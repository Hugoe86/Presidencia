<%@ Page Title="Reportes de Constancias y Certificaciones" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Rpt_Impresion_Constancias.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Rpt_Impresion_Constancias" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
 <script type="text/javascript" language="javascript">
         <!--
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_Session.ashx";
 
        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion() {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }
 
        //Temporizador para matener la sesión activa
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);
        
    //-->
</script>  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server"  AsyncPostBackTimeout="3600"  EnableScriptGlobalization="true" EnableScriptLocalization="true" />
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
                        <td class="label_titulo">
                           Reportes Constancias y Certificaciones
                        </td>
                    </tr>
                    <tr>
                        <td>
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top">
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                 
                        </td>
                    </tr>
               </table>             
            
               <table width="98%"  border="0" cellspacing="0">
                <tr align="center">
                    <td>                
                        <div style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td align="left" style="width:59%;">
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="imprimir" AlternateText="Imprimir" CssClass="Img_Button" TabIndex="1"
                                            OnClick="Btn_Nuevo_Click" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="5"
                                            OnClick="Btn_Salir_Click" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                </table>  
                    
                <br />
                    
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td colspan="4">
                            <asp:RadioButtonList ID="Rdb_Tipo_Reporte" runat="server" AutoPostBack= "true">
                            <asp:ListItem Value="CP">Constancia de Propiedad</asp:ListItem>
                            <asp:ListItem Value="CNP">Constancia de No Propiedad</asp:ListItem>
                            <asp:ListItem Value="CNA">Constancia de No Adeudo</asp:ListItem>
                            <asp:ListItem Value="COD">Certificaciones</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                     <td style="text-align:left;width:10%;">
                            Fecha Inicial</td>
                        <td align="left">
                        <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Enabled="false" Width ="75%"/>
                        <cc1:CalendarExtender ID="Ce_Txt_Fecha"  
                        runat="server" TargetControlID="Txt_Fecha_Inicial" 
                        Format="dd/MMM/yyyy"  PopupButtonID="Btn_Fecha"  />
                        <asp:ImageButton ID="Btn_Fecha" runat="server" Width="15%" 
                        ImageUrl="~/paginas/imagenes/gridview/grid_calendar.png" style="vertical-align:top;"
                        Height="18px" CausesValidation="false"/> 
                        <cc1:MaskedEditExtender 
                        ID="Mee_Txt_Fecha" 
                        Mask="99/LLL/9999" 
                        runat="server"
                        MaskType="None" 
                        UserDateFormat="DayMonthYear" 
                        UserTimeFormat="None" Filtered="/"
                        TargetControlID="Txt_Fecha_Inicial" 
                        Enabled="True" 
                        ClearMaskOnLostFocus="false"/>  
                        <cc1:MaskedEditValidator 
                        ID="Mev_Mee_Txt_Fecha" 
                        runat="server" 
                        ControlToValidate="Txt_Fecha_Inicial"
                        ControlExtender="Mee_Txt_Fecha" 
                        EmptyValueMessage="Fecha Inicio Vacía"
                        InvalidValueMessage="Fecha Inicio Invalida" 
                        IsValidEmpty="true" 
                        TooltipMessage="Ingrese o Seleccione la Fecha Inicio"
                        Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                    </td>
                   </tr>
                   <tr>
                     <td style="text-align:left;width:10%;">
                            Fecha Final</td>
                        <td   align="left">
                        <asp:TextBox ID="Txt_Fecha_Final" runat="server" Enabled="false" Width ="75%"/>
                        <cc1:CalendarExtender ID="Ce_Txt_Fecha_Final"  
                        runat="server" TargetControlID="Txt_Fecha_Final" 
                        Format="dd/MMM/yyyy"  PopupButtonID="Btn_Fecha_final"  />
                        <asp:ImageButton ID="Btn_fecha_Final"  runat="server" Width="15%" 
                        ImageUrl="~/paginas/imagenes/gridview/grid_calendar.png" style="vertical-align:top;"
                        Height="18px" CausesValidation="false"/> 
                        <cc1:MaskedEditExtender 
                        ID="Mee_Txt_Fecha_Final" 
                        Mask="99/LLL/9999" 
                        runat="server"
                        MaskType="None" 
                        UserDateFormat="DayMonthYear" 
                        UserTimeFormat="None" Filtered="/"
                        TargetControlID="Txt_Fecha_Final" 
                        Enabled="True" 
                        ClearMaskOnLostFocus="false"/>  
                        <cc1:MaskedEditValidator 
                        ID="Mev_Mee_Txt_Fecha_final" 
                        runat="server" 
                        ControlToValidate="Txt_Fecha_Final"
                        ControlExtender="Mee_Txt_Fecha_Final" 
                        EmptyValueMessage="Fecha Final Vacía"
                        InvalidValueMessage="Fecha Final Invalida" 
                        IsValidEmpty="true" 
                        TooltipMessage="Ingrese o Seleccione la Fecha Final"
                        Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                    </td>
                    </tr>
                    <tr>
                    <td align="left">
                        Estatus
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="Cmb_Estatus" runat="server">
                        <asp:ListItem Value="('PAGADA','IMPRESA','POR PAGAR','CANCELADA')" Text="TODOS"/>
                        <asp:ListItem Value="('PAGADA','IMPRESA')" Text="PAGADAS"/>
                        <asp:ListItem Value="('POR PAGAR')" Text="POR PAGAR"/>
                        <asp:ListItem Value="('CANCELADA')" Text="CANCELADAS"/>
                        </asp:DropDownList>
                    </td>
                    </tr>
                </table>                
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
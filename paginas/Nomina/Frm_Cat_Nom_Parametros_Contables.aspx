<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Parametros_Contables.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Parametros_Contables" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
   <!--SCRIPT PARA LA VALIDACION QUE NO EXPERE LA SESSION-->  
   <script language="javascript" type="text/javascript">
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
<cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server"  AsyncPostBackTimeout="36000" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>

<asp:UpdatePanel ID="UPnl_Parametros_Contables" runat="server">
    <ContentTemplate>
    
    <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Parametros_Contables" DisplayAfter="0">
        <ProgressTemplate>
            <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
            <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
    <div id="Div_Parametros_Contables" style="background-color:#ffffff; width:100%; height:100%;">
        <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
            <tr align="center">
                <td class="label_titulo">Par&aacute;metros Contables</td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false"  />&nbsp;
                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                </td>
            </tr>
        </table>
        
        <table width="98%"  border="0" cellspacing="0">
                 <tr align="center">
                     <td colspan="2">                
                         <div align="right" class="barra_busqueda" style="padding:0px 0px 6px 0px;">                        
                              <table style="width:100%;height:28px;">
                                <tr>
                                  <td align="left" style="width:59%;">                                                  
                                       <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="17" 
                                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"  OnClick="Btn_Nuevo_Click"/>
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="18"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                        <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" TabIndex="19"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                            OnClientClick="return confirm('¿Está seguro de eliminar el Parámetro Contable?');"/>
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="20"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                  </td>
                                  <td align="right" style="width:41%;">
                                        
                                   </td>       
                                 </tr>         
                              </table>                      
                            </div>
                     </td>
                 </tr>
        </table>          
        
        <table style="width:98%; font-family: Tahoma; font-size:11px;" >
            <tr style="height:1%; min-height: 24px; overflow: hidden;">
                <td style="width:30%; text-align:left; cursor:default;" >
                    Dietas
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Dietas" runat="server" Width="100%"/>
                </td>
            </tr>        
            <tr style="height:1%; min-height: 24px; overflow: hidden;">
                <td style="width:30%; text-align:left; cursor:default;" >
                    Sueldos Base
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Sueldos_Base" runat="server" Width="100%"/>
                </td>
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">                            
                <td style="width:30%; text-align:left; cursor:default;" >
                    Honorarios Asimilados
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Honorarios_Asimilados" runat="server" Width="100%"/>
                </td>                
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">
                <td style="width:30%; text-align:left; cursor:default;" >
                    Remuneracíones Eventuales
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Remuneraciones_Eventuales" runat="server" Width="100%"/>
                </td>
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">                            
                <td style="width:30%; text-align:left; cursor:default;" >
                    Prima Vacacional
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Prima_Vacacional" runat="server" Width="100%"/>
                </td>                
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">
                <td style="width:30%; text-align:left; cursor:default;" >
                    Gratificaciones Fin de Año
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Gratificaciones_Fin_Anio" runat="server" Width="100%"/>
                </td>
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">                            
                <td style="width:30%; text-align:left; cursor:default;" >
                    Previsión Social Múltiple
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Prevision_Social_Multiple" runat="server" Width="100%"/>
                </td>                
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">
                <td style="width:30%; text-align:left; cursor:default;" >
                    Prima Dominical
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Prima_Dominical" runat="server" Width="100%"/>
                </td>
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">                            
                <td style="width:30%; text-align:left; cursor:default;" >
                    Horas Extra
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Horas_Extra" runat="server" Width="100%"/>
                </td>                
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">
                <td style="width:30%; text-align:left; cursor:default;" >
                    Participaciones por Vigilancia
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Participaciones_Vigilancia" runat="server" Width="100%"/>
                </td>
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">                            
                <td style="width:30%; text-align:left; cursor:default;" >
                    Aportaciones al ISSEG
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Aportaciones_ISSEG" runat="server" Width="100%"/>
                </td>                
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">
                <td style="width:30%; text-align:left; cursor:default;" >
                    Aportaciones IMSS
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Aportaciones_IMSS" runat="server" Width="100%"/>
                </td>
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">                            
                <td style="width:30%; text-align:left; cursor:default;" >
                    Cuotas para el Fondo de Ahorro 
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Cuotas_Fondo_Ahorro" runat="server" Width="100%"/>
                </td>                
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">
                <td style="width:30%; text-align:left; cursor:default;" >
                    Prestaciones
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Prestaciones" runat="server" Width="100%"/>
                </td>
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">
                <td style="width:30%; text-align:left; cursor:default;" >
                    Prestaciones Establecidas Por Condiciones Generales de Trabajo
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Prestaciones_Generales_Establecidas_Condiciones_Trabajo" runat="server" Width="100%"/>
                </td>
            </tr>            
            <tr style="height:1%; min-height: 24px; overflow: hidden;">                            
                <td style="width:30%; text-align:left; cursor:default;" >
                    Estimulos Productividad Eficiencia
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Estimulos_Productividad_Eficiencia" runat="server" Width="100%"/>
                </td>                
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">
                <td style="width:30%; text-align:left; cursor:default;" >
                    Impuestos sobre Nóminas
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Impuestos_Sobre_Nominas" runat="server" Width="100%"/>
                </td>
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">                            
                <td style="width:30%; text-align:left; cursor:default;" >
                    Pensiones
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Pensiones" runat="server" Width="100%"/>
                </td>                
            </tr>                  
            <tr style="height:1%; min-height: 24px; overflow: hidden;">                            
                <td style="width:30%; text-align:left; cursor:default;" >
                    Honorarios
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Honorarios" runat="server" Width="100%"/>
                </td>                
            </tr>  
            <tr style="height:1%; min-height: 24px; overflow: hidden;">                            
                <td style="width:30%; text-align:left; cursor:default;" >
                    Seguros
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Seguros" runat="server" Width="100%"/>
                </td>                
            </tr>  
            <tr style="height:1%; min-height: 24px; overflow: hidden;">                            
                <td style="width:30%; text-align:left; cursor:default;" >
                    Liquidaciones por Indemnización
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Liq_Indemnizacion" runat="server" Width="100%"/>
                </td>                
            </tr>  
            <tr style="height:1%; min-height: 24px; overflow: hidden;">                            
                <td style="width:30%; text-align:left; cursor:default;" >
                    Prestaciones por Retiro
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Prestaciones_Retiro" runat="server" Width="100%"/>
                </td>                
            </tr>                                                                                                       
        </table>
        <asp:HiddenField ID="HTxt_Clave_Primaria_Parametro_ID" runat="server" />
    </div>    
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" 
CodeFile="Cls_Cat_Nom_Parametros_Desc.aspx.cs" Inherits="paginas_Nomina_Cls_Cat_Nom_Parametros_Desc" Title="Parámetros Descuentos" %>
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
        
    //-->z
   </script> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<cc1:ToolkitScriptManager ID="Tsm_Parametros_Desc" runat="server"  AsyncPostBackTimeout="36000" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>

<asp:UpdatePanel ID="UPnl_Parametros_Desc" runat="server">
    <ContentTemplate>
    
    <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Parametros_Desc" DisplayAfter="0">
        <ProgressTemplate>
            <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
            <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
    <div id="Div_Parametros_Contables" style="background-color:#ffffff; width:100%; height:100%;">
        <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
            <tr align="center">
                <td class="label_titulo">Par&aacute;metros Descuentos</td>
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
                                            OnClientClick="return confirm('¿Está seguro de eliminar el Parámetro Descuentos?');"/>
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
                    Descuento PMO Mercados
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Concepto_PMO_Mercados" runat="server" Width="100%"/>
                </td>
            </tr>        
            <tr style="height:1%; min-height: 24px; overflow: hidden;">
                <td style="width:30%; text-align:left; cursor:default;" >
                    Descuento PMO Tesoreria
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Concepto_PMO_Tesoreria" runat="server" Width="100%"/>
                </td>
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">                            
                <td style="width:30%; text-align:left; cursor:default;" >
                    Descuento PMO Corto Plazo
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Concepto_PMO_Corto_Plazo" runat="server" Width="100%"/>
                </td>                
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">
                <td style="width:30%; text-align:left; cursor:default;" >
                    Descuento PMO Pago Aval
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Concepto_PMO_Pago_Aval" runat="server" Width="100%"/>
                </td>
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">                            
                <td style="width:30%; text-align:left; cursor:default;" >
                    Descuento PMO IMUVI
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Concepto_PMO_IMUVI" runat="server" Width="100%"/>
                </td>                
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">
                <td style="width:30%; text-align:left; cursor:default;" >
                    Descuento Llamadas Tel.
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Concepto_Llamadas_Tel" runat="server" Width="100%"/>
                </td>
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">                            
                <td style="width:30%; text-align:left; cursor:default;" >
                    Descuento Perdida Equipo
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Concepto_Perdida_Equipo" runat="server" Width="100%"/>
                </td>                
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">
                <td style="width:30%; text-align:left; cursor:default;" >
                    Otros Descuentos Fijos
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Concepto_Otros_Descuentos_Fijos" runat="server" Width="100%"/>
                </td>
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">                            
                <td style="width:30%; text-align:left; cursor:default;" >
                    Otros Descuentos Variables
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Concepto_Otros_Descuentos_Variables" runat="server" Width="100%"/>
                </td>                
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">
                <td style="width:30%; text-align:left; cursor:default;" >
                    Descuento Agua
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Concepto_Pago_Agua" runat="server" Width="100%"/>
                </td>
            </tr>
            <tr style="height:1%; min-height: 24px; overflow: hidden;">                            
                <td style="width:30%; text-align:left; cursor:default;" >
                    Descuento Predial
                </td>
                <td style="width:70%; text-align:left; cursor:default;" >
                    <asp:DropDownList ID="Cmb_Concepto_Pago_Predial" runat="server" Width="100%"/>
                </td>                
            </tr>           
        </table>
        <asp:HiddenField ID="HTxt_Clave_Primaria_Parametro_ID" runat="server" />
    </div>    
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>


<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Rpt_Cat_Reportes_Pre_Estrategicos.aspx.cs" Inherits="paginas_Catastro_Frm_Rpt_Cat_Reportes_Pre_Estrategicos" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Title="Reportes de Predios Estrategicos"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script type='text/javascript' >
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

    function ValidarCaracteres(textareaControl, maxlength) { if (textareaControl.value.length > maxlength) { textareaControl.value = textareaControl.value.substring(0, maxlength); alert("Debe ingresar hasta un máximo de " + maxlength + " caracteres"); } }
    
    function Validar_Tipo_Solicitante()
		{
		    var Tipo_Solicitante_Activo;
		    if (document.getElementById("<%=Cmb_Tipo_Reporte.ClientID%>").value == "ACV")
		    {
		        Tipo_Solicitante_Activo=true;
		        document.getElementById("<%=Cmb_Movimiento.ClientID%>").value = "ACV";
		        document.getElementById("<%=Cmb_Movimiento.ClientID%>").disabled = true;
		    }
		    else
		    {
		        document.getElementById("<%=Cmb_Movimiento.ClientID%>").disabled = false;
		    }
		}
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
                           Reportes Predios Estrategicos
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
                        <div align="right" style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td align="left" style="width:59%;">
                                        <asp:ImageButton ID="Btn_Exportar_Predios" runat="server" TabIndex="5" ImageUrl="~/paginas/imagenes/gridview/pdf.png"
                                            Width="24px" CssClass="Img_Button" AlternateText="Reporte Predios Estrategicos" 
                                            onclick="Btn_Exportar_Predios_Click" />    
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                            CssClass="Img_Button" TabIndex="3"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            AlternateText="Salir" onclick="Btn_Salir_Click" />
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
                        <td style="text-align:left;width:20%;">
                            Reporte
                        </td>
                        <td style="width:30%; text-align:left;">
                            <asp:DropDownList ID="Cmb_Tipo_Reporte" runat="server" Enabled="true" Width="98%" onchange="Validar_Tipo_Solicitante();">
                            <asp:ListItem Text="Predios Estratégicos" Value="ESTRATEGICO" />
                            <asp:ListItem Text="Predios Urbanos" Value="URBANO" />
                            <asp:ListItem Text="Predios Rusticos" Value="RUSTICO" />
                            <asp:ListItem Text="Predios de Municipio" Value="MUNICIPIO" />
                            <asp:ListItem Text="Predios a la Baja" Value="BAJA" />
                            <asp:ListItem Text="Predios con ACV" Value="ACV" />
                            <asp:ListItem Text="Ejercicio fiscal" Value="AEF" />
                             <asp:ListItem Text="Totales de Cuotas Asisnadas-Atendidas" Value="TCAA" />
                             <asp:ListItem Text="Veces de Rechazo" Value="VR" />
                            </asp:DropDownList>
                        </td>
                        <td style="text-align:left;width:20%;">
                            Actualizados en B.D.
                            </td>
                            <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Movimiento" runat="server" Enabled="true" Width="98%" >
                            <asp:ListItem Text="No" Value="NO" />
                            <asp:ListItem Text="Movimiento ACV" Value="ACV" />
                            </asp:DropDownList>
                            </td>
                    <td>
                    </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Fecha Inicio
                        </td>
                        <td style="width:30%; text-align:left;">
                            <asp:TextBox  runat="server" ID="Txt_Fecha_Inicio"  Width="92%" TabIndex="6"
                                Style="float: left"/>
                                <cc1:TextBoxWatermarkExtender ID="Twe_Fecha_Inicio" runat="server" 
                                TargetControlID="Txt_Fecha_Inicio" WatermarkCssClass="watermarked" 
                                WatermarkText="Dia/Mes/Año" Enabled="True" />
                                <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server"
                                ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                Height="18px"/>
                            <cc1:CalendarExtender ID="Dtp_Fecha_Inicio" runat="server" TargetControlID="Txt_Fecha_Inicio" 
                                PopupButtonID="Btn_Fecha_Inicio" Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender>
                        </td>
                        <td style="text-align:left;width:20%;">
                            * Fecha_Fin</td>
                    <td>
                        <asp:TextBox  runat="server" ID="Txt_Fecha_Fin"  Width="92%" TabIndex="6"
                                Style="float: left"/>
                                <cc1:TextBoxWatermarkExtender ID="Twe_Fecha_Fin" runat="server" 
                                TargetControlID="Txt_Fecha_Fin" WatermarkCssClass="watermarked" 
                                WatermarkText="Dia/Mes/Año" Enabled="True" />
                                <asp:ImageButton ID="Btn_Fecha_Fin" runat="server"
                                ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                Height="18px"/>
                            <cc1:CalendarExtender ID="Dtp_Fecha_Fin" runat="server" TargetControlID="Txt_Fecha_Fin" 
                                PopupButtonID="Btn_Fecha_Fin" Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender>
                    </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Anio
                        </td>
                        <td style="width:30%; text-align:left;">
                            <asp:TextBox  runat="server" ID="Txt_Anio"  Width="92%" TabIndex="6"
                                Style="float: left" MaxLength="4"/>
                                 <cc1:FilteredTextBoxExtender FilterType="Numbers" runat="server" ID="FTB_Txt_Anio"
                                            TargetControlID="Txt_Anio" />
                                </tr>
                    </table>        
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
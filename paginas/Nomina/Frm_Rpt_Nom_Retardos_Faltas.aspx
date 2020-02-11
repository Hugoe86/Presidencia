<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Retardos_Faltas.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Retardos_Faltas" Title="Reporte de retardos y faltas"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

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
<cc1:ToolkitScriptManager ID="Tsm_Rpt_Incidencias_Empleados" runat="server"  AsyncPostBackTimeout="36000" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>

<asp:UpdatePanel ID="UPnl_Rpt_Incidencias_Empleados" runat="server" UpdateMode="Always">
    <ContentTemplate>
    
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Rpt_Incidencias_Empleados" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>  
        
                <div style="width:98%; background-color:White;">
            <table width="100%" title="Control_Errores"> 
                <tr>
                    <td style="width:100%; text-align:center; cursor:default; font-size:14px;">
                        Reportes de reloj checador
                    </td>               
                </tr>            
                <tr>
                    <td style="width:100%; text-align:left; cursor:default;">
                        <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />
                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>        
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
                                                <asp:ImageButton ID="Btn_Reporte_Pdf" runat="server" ToolTip="Generar Reporte" 
                                                    CssClass="Img_Button" TabIndex="1"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" 
                                                    onclick="Btn_Reporte_Pdf_Click"/>
                                                <asp:ImageButton ID="Btn_Reporte_Excel" runat="server" 
                                                     ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png"
                                                      CssClass="Img_Button"  AlternateText="Imprimir Excel" 
                                                     ToolTip="Exportar Excel"
                                                     OnClick="Btn_Reporte_Excel_Click" Visible="true"/> 
                                                <%--<asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                                    CssClass="Img_Button" TabIndex="2"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                                    onclick="Btn_Salir_Click"/>--%>
                                            </td>
                                            <td align="right" style="width:41%;">&nbsp;</td>       
                                        </tr>         
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                   
                   
                     <asp:Panel ID="Pnl_Filtros_Empleado" runat="server" Width="100%" GroupingText="Filtrar por Empleados">        
                        <table width="100%">              
                            <tr>
                                <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                                    N&uacute;mero de Empleado 
                                </td>
                                <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                                    <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="98%" TabIndex="0" MaxLength="20"/>
                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Empleado" runat="server" 
                                        TargetControlID="Txt_No_Empleado" FilterType="Numbers" ValidChars="0123456789" />                        
                                </td> 
                                <td  style="width:50%; text-align:left; cursor:default;"></td>
                           </tr>  
                           
                        </table>
                         <table width="100%"> 
                             
                             <tr>
                                <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                                    Nombre
                                </td>
                                <td class="button_autorizar" style="width:37%; text-align:left; cursor:default;" colspan="3">
                                    <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" Width="98%" TabIndex="1" />
                                </td>
                                 <td class="button_autorizar" style="width:3%; text-align:left; cursor:default;" colspan="3">
                                    <asp:ImageButton ID="Btn_Buscar_Empleado" 
                                            runat="server" ToolTip="Consultar"
                                            TabIndex="12" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                            onclick="Btn_Buscar_Empleado_Click"/>
                                </td>                        
                                <td class="button_autorizar" style="width:40%; text-align:left; cursor:default;" colspan="3">
                                        <asp:DropDownList ID="Cmb_Nombre_Empleado" runat="server" TabIndex="2" Width="97%" 
                                        AutoPostBack="true" OnSelectedIndexChanged="Cmb_Nombre_Empleado_OnSelectedIndexChanged"/>
                                </td>
                            </tr> 
                        </table>  
                    </asp:Panel>
                        
                    <asp:Panel ID="Pnl_Dependencia" runat="server" Width="100%" GroupingText="">   
                        <table width="100%"> 
                           <%-- <tr>
                                <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                                    Tipo Nómina
                                </td>
                                <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                                    <asp:DropDownList ID="Cmb_Tipo_Nomina" runat="server" Width="100%" TabIndex="3"/>
                                </td>  --%>
                                   
                            <tr>   
                                <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                                    U. Responsable
                                </td>
                                <td class="button_autorizar" style="width:80%; text-align:left; cursor:default;">
                                    <asp:DropDownList ID="Cmb_Unidad_Responsable" runat="server" Width="100%" TabIndex="3" />
                                </td>             
                            </tr>  
                             
                        </table>
                    </asp:Panel>
                    
                     <asp:Panel ID="Pnl_Tipo_Reporte" runat="server" Width="100%" GroupingText="Reporte">   
                        <table width="100%"> 
                             <tr>
                                <td  class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                                    Tipo de reporte
                                </td>
                                
                                <td class="button_autorizar" style="width:80%; text-align:left; cursor:default;">
                                    <asp:DropDownList ID="Cmb_Tipo_Reporte" runat="server" Width="100%" TabIndex="3" 
                                        AutoPostBack="true" OnSelectedIndexChanged="Cmb_Tipo_Reporte_OnSelectedIndexChanged">
                                        <asp:ListItem Value ="RETARDOS Y FALTAS RELOJ CHECADOR">
                                            RETARDOS Y FALTAS RELOJ CHECADOR</asp:ListItem>
                                        <asp:ListItem Value="FALTAS RELOJ CHECADOR">
                                            FALTAS RELOJ CHECADOR</asp:ListItem>
                                        <asp:ListItem Value="SALIDAS ANTICIPADAS RELOJ CHECADOR">
                                            SALIDAS ANTICIPADAS RELOJ CHECADOR</asp:ListItem>
                                        <asp:ListItem Value="REPORTE HISTORICO RELOJ CHECADOR">
                                            REPORTE HISTORICO RELOJ CHECADOR</asp:ListItem>
                                        <asp:ListItem Value="REPORTE PERSONAL QUE CHECA">
                                            REPORTE PERSONAL QUE CHECA</asp:ListItem>
                                    </asp:DropDownList>
                                </td> 
                            </tr>
                        </table>
                        
                         <table width="100%"> 
                         <tr>
                                <td  class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                                    Personal Checa
                                </td>
                                
                                <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                                    <asp:DropDownList ID="Cmb_Checa" runat="server" Width="100%" TabIndex="3" >
                                        <asp:ListItem Value ="SI">SI</asp:ListItem>
                                        <asp:ListItem Value ="NO">NO</asp:ListItem>
                                    </asp:DropDownList>
                                </td> 
                                <td  style="width:50%; text-align:left; cursor:default;"></td>
                            </tr>
                         </table>     
                    </asp:Panel>
                    
                    <asp:Panel ID="Pnl_Fechas" runat="server" Width="100%" GroupingText="Fechas">  
                         <table width="100%"> 
                             <tr>
                                <td  class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                                    Nomina
                                </td>
                                
                                <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                                    <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="95%" AutoPostBack="true" 
                                        TabIndex="3" onselectedindexchanged="Cmb_Calendario_Nomina_SelectedIndexChanged"/>
                                </td> 
                                
                                <td  class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                                    Periodo
                                </td>
                                
                                <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                                    <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" Width="95%" TabIndex="4" 
                                        AutoPostBack="true" onselectedindexchanged="Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged" />
                                </td> 
                            </tr>
                        </table>
                      <%--  --%>
                        
                        <table width="100%"> 
                             <tr>
                                <td  class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                                    Fecha Inicial(DDMMAAAA)
                                </td>
                                <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                                    <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="98%" MaxLength="8" ToolTip="ddmmaaaa"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Inicial_FilteredTextBoxExtender" runat="server" TargetControlID="Txt_Fecha_Inicial" FilterType="Custom, Numbers"
                                    ValidChars="0123456789" />
                                </td>
                                
                                <td  class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                                    Fecha Final(DDMMAAAA)
                                </td>
                                <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                                    <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="98%" MaxLength="8" ToolTip="ddmmaaaa"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Final_FilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Fecha_Final" FilterType="Custom, Numbers"
                                    ValidChars="0123456789" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>   
                    
                </div>
             </ContentTemplate>
      </asp:UpdatePanel>
</asp:Content>
<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Est_Pat_Siniestros.aspx.cs" Inherits="paginas_Compras_Frm_Est_Pat_Siniestros" Title="Estadisticas Siniestros" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" language="javascript">
        //Metodos para limpiar los controles de la busqueda.
        function Limpiar_Ctlr_Campos(){ 
            document.getElementById("<%=Txt_Fecha_Inicial.ClientID%>").value="__/___/____"; 
            document.getElementById("<%=Txt_Fecha_Final.ClientID%>").value="__/___/____";  
            document.getElementById("<%=Chk_Responsabilidad.ClientID%>").checked = false;  
            document.getElementById("<%=Chk_Unidad_Responsable.ClientID%>").checked = false;   
            document.getElementById("<%=Chk_Tipo_Siniestro.ClientID%>").checked = false;  
            return false;
        }  
    </script>  
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
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"  AsyncPostBackTimeout="36000" EnableScriptLocalization="true" EnableScriptGlobalization="true"/> 
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <center>
                    <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">              
                        <tr align="center">
                            <td class="label_titulo" colspan="2">Estadisticas de Siniestros</td>
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
                        <tr>
                            <td>&nbsp;</td>                        
                        </tr>
                        <tr class="barra_busqueda" align="right">
                            <td align="left">
                                <asp:ImageButton ID="Btn_Generar_Reporte_PDF" runat="server" 
                                    ToolTip="Generar (Pdf)" Width="24px" 
                                    ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" 
                                    onclick="Btn_Generar_Reporte_PDF_Click" />
                                <asp:ImageButton ID="Btn_Generar_Reporte_Excel" runat="server" ToolTip="Generar (Excel)" Width="24px" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png"
                                    onclick="Btn_Generar_Reporte_Excel_Click" />
                                <asp:ImageButton ID="Btn_Limpiar_Filtros" runat="server" ToolTip="Limpiar Filtros" Width="24px" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" OnClientClick="javascript:return Limpiar_Ctlr_Campos();" />
                            </td>
                            <td>&nbsp;</td>                        
                        </tr>
                    </table>   
                </center>
                <br />
                <center>
                    <div id="Div_Fechas" style="border-style:solid; width:96%;" >
                        <br />
                        <table width="98%" border="0" cellspacing="0" class="estilo_fuente">  
                            <tr>
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Fecha_Inicial" runat="server" Text="Fecha Inicial" ></asp:Label>
                                </td>
                                <td style="width:32%; text-align:left;">
                                    <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="80%" MaxLength="20" AutoPostBack="true" CausesValidation="true"></asp:TextBox>
                                    <asp:ImageButton ID="Btn_Fecha_Inicial" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                    <cc1:CalendarExtender ID="CE_Txt_Fecha_Inicial" runat="server" TargetControlID="Txt_Fecha_Inicial" PopupButtonID="Btn_Fecha_Inicial" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender>
                                    <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Inicial_FilteredTextBoxExtender" runat="server" TargetControlID="Txt_Fecha_Inicial" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                                    <cc1:MaskedEditExtender ID="Mee_Txt_Fecha_Inicial" Mask="99/LLL/9999" runat="server" MaskType="None" UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Inicial" Enabled="True" ClearMaskOnLostFocus="false"/>
                                    <cc1:MaskedEditValidator  
                                    ID="Mev_Mee_Txt_Fecha_Inicial" 
                                    runat="server" 
                                    ControlToValidate="Txt_Fecha_Inicial"
                                    ControlExtender="Mee_Txt_Fecha_Inicial" 
                                    EmptyValueMessage="La Fecha Inicial es obligatoria"
                                     InvalidValueMessage="Fecha Inicial Invalida" 
                                    IsValidEmpty="true" 
                                    TooltipMessage="Ingrese o Seleccione la Fecha Inicial"
                                    Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>  
                                </td>
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Fecha_Final" runat="server" Text="Fecha Final" ></asp:Label>
                                </td>
                                <td style="width:32%; text-align:left;">
                                    <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="80%" MaxLength="20" AutoPostBack="true"></asp:TextBox>
                                    <asp:ImageButton ID="Btn_Fecha_Final" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                    <cc1:CalendarExtender ID="CE_Txt_Fecha_Final" runat="server" TargetControlID="Txt_Fecha_Final" PopupButtonID="Btn_Fecha_Final" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender>
                                    <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Final_FilteredTextBoxExtender" runat="server" TargetControlID="Txt_Fecha_Final" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_"/>
                                    <cc1:MaskedEditExtender ID="Mee_Txt_Fecha_Final" Mask="99/LLL/9999" runat="server" MaskType="None" UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Final" Enabled="True" ClearMaskOnLostFocus="false"/>
                                    <cc1:MaskedEditValidator  
                                    ID="Mev_Txt_Fecha_Final" 
                                    runat="server" 
                                    ControlToValidate="Txt_Fecha_Final"
                                    ControlExtender="Mee_Txt_Fecha_Final" 
                                    EmptyValueMessage="La Fecha Final es obligatoria"
                                    InvalidValueMessage="Fecha Final Invalida" 
                                    IsValidEmpty="true" 
                                    TooltipMessage="Ingrese o Seleccione la Fecha Final"
                                    Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>  
                                </td>
                            </tr>     
                                  
                        </table>
                        <br />
                    </div>
                </center>
                <br />    
                <center>
                    <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                        <tr>
                            <td style="width:100%; text-align:center; font-weight:bolder;">
                                <asp:Label ID="Lbl_Leyenda_Estadisticas" runat="server" Text="SELECCION DE ESTADISTICAS:"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:100%; text-align:center;">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:100%; text-align:left;">
                                <asp:CheckBox ID="Chk_Responsabilidad" runat="server" Text="Por Responsabilidad de Siniestro."></asp:CheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:100%; text-align:left;">
                                <asp:CheckBox ID="Chk_Unidad_Responsable" runat="server" Text="Por Unidad Responsable."></asp:CheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:100%; text-align:left;">
                                <asp:CheckBox ID="Chk_Tipo_Siniestro" runat="server" Text="Por Tipo Siniestro."></asp:CheckBox>
                            </td>
                        </tr>
                    </table>
                </center>                      
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


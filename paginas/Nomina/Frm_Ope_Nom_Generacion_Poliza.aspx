<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Generacion_Poliza.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Generacion_Poliza" Title="Untitled Page" %>
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

<script src="../../easyui/jquery.tools.min.js" type="text/javascript"></script>

<script>
    function pageLoad() {
        init();
    }

    $(document).ready(init);
    
    function init() {
        $('input[id$=Btn_Generar_Poliza]').tooltip({
            position: ["top", "center"],
            // tweak the position
            offset: [10, 2],
            relative: 1,
            effect: "slide",
            fadeOutSpeed: "fast"
        });

        // select all desired input fields and attach tooltips to them
        $("select[id*=Cmb_]").tooltip({
            tipClass: "tooltip_fields",
            // place tooltip on the right edge
            position: "center right",
            // a little tweaking of the position
            offset: [-2, 10],
            // use the built-in fadeIn/fadeOut effect
            effect: "fade",
            // custom opacity setting
            opacity: 0.7
        });
    }
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

<cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server"  AsyncPostBackTimeout="36000"/>
<asp:UpdatePanel ID="UPnl_Nominas_Negativas" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Nominas_Negativas" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>  
         
        <div style="width:98%; background-color:White;">
        
            <table width="100%" title="Control_Errores"> 
                <tr>
                    <td class="button_autorizar" style="width:100%; text-align:center; cursor:default; font-size:12px;">
                        Generación Poliza Nómina
                    </td>
                </tr>
                <tr>
                    <td class="button_autorizar"  style="width:100%; text-align:left; cursor:default;">
                        <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" 
                            style="display:none;" Width="24px" Height="24px"/>
                        <asp:Label id="Lbl_Mensaje_Error" runat="server" CssClass="estilo_fuente_mensaje_error" />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                        Nomina
                    </td>
                    <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                        <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" OnSelectedIndexChanged="Cmb_Calendario_Nomina_SelectedIndexChanged" 
                            AutoPostBack="true" title="Dato requerido Año del Calendario"/>
                    </td>
                    <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                        Periodo
                    </td>
                    <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                        <asp:DropDownList ID="Cmb_Periodo" runat="server" title="Dato requerido Periodo de Nómina" 
                            Width="100%" TabIndex="6"/>
                    </td>
                </tr>
            </table> 
            
            <table width="100%">
                <tr>
                    <td class="button_autorizar" style="width:100%; text-align:left; cursor:default;" colspan="4">
                       <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width:100%;text-align:right;" colspan="4">
                        <asp:UpdatePanel ID="Upnl_Reporte" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="Btn_Generar_Poliza" runat="server" Text="Generar Poliza" 
                                    Width="20%" CssClass="button_autorizar" OnClick="Btn_Generar_Poliza_Click"
                                    style="background-image:url(../imagenes/paginas/icono_rep_excel.png); background-repeat: no-repeat; background-position:right; cursor:hand;"/>
                                <div class="tooltip" style="text-align:justify;">
                                  Información:<br />
                                  Generar la Póliza de Nómina
                                  Este Proceso puede tardar varios minutos
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width:100%; text-align:left; cursor:default;" colspan="4">
                        <hr />
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger  ControlID="Btn_Generar_Poliza"/>
    </Triggers>
</asp:UpdatePanel>
</asp:Content>


<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Rpt_Cat_Imprimir_Reporte_Avaluos_Realizados_Valuador.aspx.cs" Inherits="paginas_Catastro_Frm_Imprimir_Reporte_Avaluos_Realizados_Valuador" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    Title="Catálogo de Impresiones De registro de cuotas por valuador"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 277px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type='text/javascript'>
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }
         function Abrir_Busqueda_Peritos_Internos() {
        $find('Busqueda_Peritos_Internos').show();
        return false;
    }
       

       // <!--
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
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server" AsyncPostBackTimeout="3600"
        EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <asp:UpdatePanel ID="Upd_Otros_Pagos" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Otros_Pagos"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Calles" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Reporte&nbsp; Impresion de Avaluos Realizados por Valuador y Entregas</td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:ImageButton ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
                                                Width="24px" Height="24px" />
                                            <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%;">
                                        </td>
                                        <td style="width: 90%; text-align: left;" valign="top">
                                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" class="style1">
                         <asp:ImageButton ID="Btn_Imprimir_Cuentas" runat="server" TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Imprimir" OnClick="Btn_Imprimir_Cuentas_Click"
                                ToolTip="Imprimir cuentas asignadas" />
                        <asp:ImageButton ID="Btn_Imprimir_Avaluos_Fiscales" runat="server" TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png"
                            Width="24px" CssClass="Img_Button" AlternateText="Imprimir" OnClick="Btn_Imprimir_Avaluos_Fiscales_Click"
                            ToolTip="Imprimir Avaluos Fiscales" />
                            <asp:ImageButton ID="Btn_Imprimir_Metas_Autorizacion" runat="server" TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png"
                            Width="24px" CssClass="Img_Button" AlternateText="Imprimir Metas Autorizacion" OnClick="Btn_Imprimir_Metas_Autorizacion_Click"
                            ToolTip="Imprimir Metas Autorizacion" />
                             </td>
                    </tr>
                </table>
                <br />
                <center>
                    <table width="98%" class="estilo_fuente">
                        <div id="Div_Grid_Cuotas_Perito" runat="server" visible="true">
              
                        </div>
                    </table>
            </div>
            </table> </center>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

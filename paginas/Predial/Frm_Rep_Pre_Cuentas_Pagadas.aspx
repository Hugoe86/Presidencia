<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Rep_Pre_Cuentas_Pagadas.aspx.cs" Inherits="paginas_Predial_Frm_Rep_Pre_Cuentas_Pagadas"
    UICulture="es-MX" Culture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <!--SCRIPT PARA LA VALIDACION QUE NO EXPIRE LA SESSION-->
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
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="ScriptManager" runat="server" AsyncPostBackTimeout="3600" EnableScriptGlobalization="true" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Reporte de cuentas pagadas
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td>
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Btn_Exportar_pdf" runat="server" ToolTip="Exportar a pdf" CssClass="Img_Button"
                                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" AlternateText="Exportar a pdf"
                                                OnClick="Btn_Exportar_pdf_Click" />
                                            <asp:ImageButton ID="Btn_Exportar_Excel" runat="server" ToolTip="Exportar a Excel" CssClass="Img_Button"
                                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" AlternateText="Exportar a Excel"
                                                OnClick="Btn_Exportar_Excel_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <asp:HiddenField ID="Hdf_Cuenta_Predial_Id" runat="server" />
                </table>
                <br />
                <table width="98%" class="estilo_fuente">
                    <tr style="background-color: #3366CC">
                        <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                            Filtrar por fecha y tipo de pago
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Desde el día
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="84%" 
                                MaxLength="11" TabIndex="5" />
                            <cc1:TextBoxWatermarkExtender ID="TBE_Fecha_Inicial" runat="server" TargetControlID="Txt_Fecha_Inicial"
                                WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                            <cc1:CalendarExtender ID="CE_Fecha_Inicial" runat="server" TargetControlID="Txt_Fecha_Inicial"
                                Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Txt_Fecha_Inicial" />
                            <asp:ImageButton ID="Btn_Txt_Fecha_Inicial" runat="server" ImageUrl="../imagenes/paginas/SmallCalendar.gif"
                                Style="vertical-align: top;" Height="18px" CausesValidation="false" />
                        </td>
                        <td style="width: 20%; text-align: right;">
                            Hasta el día
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="84%" 
                                MaxLength="11" Height="18px" TabIndex="6" />
                            <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Final" runat="server" TargetControlID="Txt_Fecha_Final"
                                WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Final" runat="server" TargetControlID="Txt_Fecha_Final"
                                Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Txt_Fecha_Final" />
                            <asp:ImageButton ID="Btn_Txt_Fecha_Final" runat="server" ImageUrl="../imagenes/paginas/SmallCalendar.gif"
                                Style="vertical-align: top;" Height="18px" CausesValidation="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:RadioButton ID="Opt_Pagos_Traslado" runat="server" Text="Traslado" 
                                GroupName="Reporte_Pagos" /><br />
                            <asp:RadioButton ID="Opt_Pagos_Constancias" runat="server" Text="Constancias" 
                                GroupName="Reporte_Pagos" /><br />
                            <asp:RadioButton ID="Opt_Pagos_Fraccionamientos" runat="server" Text="Fraccionamientos" 
                                GroupName="Reporte_Pagos" /><br />
                            <asp:RadioButton ID="Opt_Pagos_Derechos_Supervision" runat="server" Text="Derechos de supervisión"
                                GroupName="Reporte_Pagos" /><br />
                            <asp:RadioButton ID="Opt_Pagos_Predial" runat="server" Text="Predial" 
                                GroupName="Reporte_Pagos" /><br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

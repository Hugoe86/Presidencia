<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Rpt_Pre_Movimientos_En_Proceso.aspx.cs" Inherits="paginas_Predial_Frm_Rpt_Pre_Movimientos_En_Proceso"
    Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type="text/javascript" language="javascript">
        //Abrir una ventana modal
		function Abrir_Ventana_Modal(Url, Propiedades)
		{
			window.showModalDialog(Url, null, Propiedades);
		}
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
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
                        <td class="label_titulo">
                            Reporte de Movimientos en Proceso
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />
                            <asp:Label ID="Lbl_Encabezado_Error" runat="server" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" CssClass="estilo_fuente_mensaje_error"></asp:Label>
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
                                        <td align="left" style="width: 35%;">
                                            <asp:ImageButton ID="Btn_Exportar_pdf" runat="server" AlternateText="Exportar a pdf"
                                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png"
                                                OnClick="Btn_Exportar_pdf_Click" TabIndex="1" ToolTip="Exportar a pdf" />
                                            <asp:ImageButton ID="Btn_Exportar_Excel" runat="server" AlternateText="Exportar a Excel"
                                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png"
                                                OnClick="Btn_Exportar_Excel_Click" TabIndex="1" ToolTip="Exportar a Excel" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click"
                                                AlternateText="Salir" />
                                        </td>
                                        <td align="right">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="98%" class="estilo_fuente">
                    <tr style="background-color: #3366CC">
                        <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                            Filtros
                        </td>
                        <td style="text-align: right; font-size: 15px; color: #FFFFFF;">
                            <asp:ImageButton ID="Btn_Limpiar_Filtros" runat="server" Height="22px"
                                ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" OnClick="Btn_Limpiar_Filtros_Click"
                                TabIndex="10" ToolTip="Limpiar Análisis de Rezago" Width="22px" />
                        </td>
                    </tr>
                </table>
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align: left; width: 25%">
                            Fecha Inicio del Movimiento</td>
                        <td style="text-align: left; width: 25%">
                            <asp:TextBox ID="Txt_Fecha_Inicio_Movimiento" runat="server" Width="84%" 
                                TabIndex="12" MaxLength="11"
                                Height="18px" />
                            <cc1:CalendarExtender ID="CE_Fecha_Inicio_Movimiento" runat="server" TargetControlID="Txt_Fecha_Inicio_Movimiento"
                                Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Fecha_Inicio_Movimiento" />
                            <asp:ImageButton ID="Btn_Fecha_Inicio_Movimiento" runat="server" ImageUrl="../imagenes/paginas/SmallCalendar.gif"
                                Style="vertical-align: top;" Height="18px" CausesValidation="false" />
                        </td>
                        <td style="text-align: right; width: 25%">
                            Fecha Término del Movimiento
                        </td>
                        <td style="text-align: left; width: 25%">
                            <asp:TextBox ID="Txt_Fecha_Termino_Movimiento" runat="server" Width="84%" 
                                TabIndex="12" MaxLength="11"
                                Height="18px" />
                            <cc1:CalendarExtender ID="CE_Fecha_Termino_Movimiento" runat="server" TargetControlID="Txt_Fecha_Termino_Movimiento"
                                Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Fecha_Termino_Movimiento" />
                            <asp:ImageButton ID="Btn_Fecha_Termino_Movimiento" runat="server" ImageUrl="../imagenes/paginas/SmallCalendar.gif"
                                Style="vertical-align: top;" Height="18px" CausesValidation="false" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 25%">
                            Fecha Inicio de Rechazo</td>
                        <td style="text-align: left; width: 25%">
                            <asp:TextBox ID="Txt_Fecha_Inicio_Rechazo" runat="server" Width="84%" 
                                TabIndex="12" MaxLength="11"
                                Height="18px" />
                            <cc1:CalendarExtender ID="CE_Fecha_Inicio_Rechazo" runat="server" TargetControlID="Txt_Fecha_Inicio_Rechazo"
                                Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Fecha_Inicio_Rechazo" />
                            <asp:ImageButton ID="Btn_Fecha_Inicio_Rechazo" runat="server" ImageUrl="../imagenes/paginas/SmallCalendar.gif"
                                Style="vertical-align: top;" Height="18px" CausesValidation="false" />
                        </td>
                        <td style="text-align: right; width: 25%">
                            Fecha Término de Rechazo</td>
                        <td style="text-align: left; width: 25%">
                            <asp:TextBox ID="Txt_Fecha_Termino_Rechazo" runat="server" Width="84%" 
                                TabIndex="12" MaxLength="11"
                                Height="18px" />
                            <cc1:CalendarExtender ID="CE_Fecha_Termino_Rechazo" runat="server" TargetControlID="Txt_Fecha_Termino_Rechazo"
                                Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Fecha_Termino_Rechazo" />
                            <asp:ImageButton ID="Btn_Fecha_Termino_Rechazo" runat="server" ImageUrl="../imagenes/paginas/SmallCalendar.gif"
                                Style="vertical-align: top;" Height="18px" CausesValidation="false" />
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

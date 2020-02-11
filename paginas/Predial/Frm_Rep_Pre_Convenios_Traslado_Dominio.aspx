<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Rep_Pre_Convenios_Traslado_Dominio.aspx.cs" Inherits="paginas_Predial_Frm_Rep_Pre_Convenios_Traslado_Dominio"
    UICulture="es-MX" Culture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server" EnableScriptGlobalization="true" />
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
                            Reporte de convenios de traslado de dominio
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
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Estatus
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="97%" TabIndex="3">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                <asp:ListItem Text="ACTIVO" Value="ACTIVO" />
                                <asp:ListItem Text="TERMINADO" Value="TERMINADO" />
                                <asp:ListItem Text="INCUMPLIDO" Value="INCUMPLIDO" />
                                <asp:ListItem Text="CANCELADO" Value="CANCELADO" />
                            </asp:DropDownList>
                        </td>
                        <td style="width:20%;text-align:right;">
                            Incluir
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Incluir_Reestructuras" runat="server" Width="97%" TabIndex="4">
                                <asp:ListItem Text="CONVENIOS Y REESTRUCTURAS" Value="" />
                                <asp:ListItem Text="SOLO CONVENIOS" Value="CONVENIOS" />
                                <asp:ListItem Text="SOLO REESTRUCTURAS" Value="REESTRUCTURAS" />
                            </asp:DropDownList>
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
                            &nbsp;
                        </td>
                    </tr>
                </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

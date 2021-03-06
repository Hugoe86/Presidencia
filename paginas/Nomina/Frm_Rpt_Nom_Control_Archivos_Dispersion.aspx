﻿<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Control_Archivos_Dispersion.aspx.cs"
    Inherits="paginas_Nomina_Reporte_Frm_Rpt_Nom_Control_Archivos_Dispersion" Title="Reporte de Control de archivos de dispersión" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="SM_Rpt_Archivos_Dispersion" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="True" />
    <asp:UpdatePanel ID="UPnl_Rpt_Archivos_Dispersion" runat="server" >
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Rpt_Archivos_Dispersion"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div style="background-color: White; width: 98%;">
                <table style="width: 99%;">
                    <tr>
                        <td style="width: 100%;" align="center">
                            <div id="Contenedor_Titulo" style="color: White; font-size: 12; font-weight: bold;
                                border-style: outset; background: url(../imagenes/paginas/titleBackground.png) repeat-x top;
                                background-color: Silver;">
                                <table width="100%">
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100%" align="left">
                                            <font style="color: Black; font-weight: bold;">Reporte de Nómina por Dependencia</font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <table width="100%" class="estilo_fuente">
                    <tr>
                        <td colspan="4">
                            &nbsp;
                            <asp:UpdatePanel ID="Upnl_Mensajes_Error" runat="server" UpdateMode="Always" RenderMode="Inline">
                                <ContentTemplate>
                                    <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                        Visible="false" />&nbsp;
                                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%;">
                    <tr>
                        <td class="button_autorizar" style="width: 20%; text-align: left;">
                            *Nómina
                        </td>
                        <td class="button_autorizar" style="width: 30%; text-align: left;">
                            <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" AutoPostBack="true"
                                TabIndex="5" OnSelectedIndexChanged="Cmb_Calendario_Nomina_SelectedIndexChanged" />
                        </td>
                        <td class="button_autorizar" style="width: 20%; text-align: left; cursor: default;">
                            &nbsp;&nbsp;*Periodo
                        </td>
                        <td class="button_autorizar" style="width: 30%; text-align: left; cursor: default;">
                            <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" Width="100%"
                                TabIndex="6" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Periodo_SelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td class="button_autorizar" style="width: 20%; font-size: 12px;">
                            <asp:Label ID="Lbl_Inicia_Catorcena" runat="server" Text="Inicia Catorcena" Width="100%" />
                        </td>
                        <td class="button_autorizar" style="width: 30%; font-size: 12px;">
                            <asp:TextBox ID="Txt_Inicia_Catorcena" runat="server" Width="98%" Enabled="false" />
                        </td>
                        <td class="button_autorizar" style="width: 20%; font-size: 12px;">
                            <asp:Label ID="Lbl_Fin_Catorcena" runat="server" Text="Fin Catorcena" Width="100%" />
                        </td>
                        <td class="button_autorizar" style="width: 30%; font-size: 12px;">
                            <asp:TextBox ID="Txt_Fin_Catorcena" runat="server" Width="98%" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="button_autorizar" style="width: 20%; font-size: 12px;">
                            <asp:Label ID="Lbl_Fecha" runat="server" Text="*Fecha" Width="100%" />
                        </td>
                        <td class="button_autorizar" style="width: 30%; font-size: 12px;">
                            <asp:TextBox ID="Txt_Fecha" runat="server" Width="85%" MaxLength="1" Wrap="true" />
                            <cc1:FilteredTextBoxExtender ID="Txt_Fecha_FilteredTextBoxExtender" runat="server"
                                TargetControlID="Txt_Fecha" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                ValidChars="/" />
                            <cc1:CalendarExtender ID="Txt_Fecha_CalendarExtender" runat="server" TargetControlID="Txt_Fecha"
                                PopupButtonID="Btn_Fecha" Format="dd/MMM/yyyy" />
                            <asp:ImageButton ID="Btn_Fecha" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                ToolTip="Seleccione la Fecha" />
                        </td>
                        <td class="button_autorizar" colspan="2" >
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; text-align: left; cursor: default;" colspan="4">
                            <hr />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Btn_Generar_Reporte_Excel"/>
        </Triggers>
    </asp:UpdatePanel>
    <table style="width: 98%;">
        <tr>
            <td class="button_autorizar" style="width: 100%; text-align: right; cursor: default;"
                colspan="4">
                <asp:UpdatePanel ID="Upnl_Export_EXCEL" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:ImageButton ID="Btn_Generar_Reporte_Excel" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png"
                            OnClick="Btn_Generar_Reporte_Excel_Click" ToolTip="Generar Reporte en EXCEL"
                            Width="32px" Height="32px" Style="cursor: hand;" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="width: 100%; text-align: left; cursor: default;" colspan="4">
                <hr />
            </td>
        </tr>
    </table>
</asp:Content>

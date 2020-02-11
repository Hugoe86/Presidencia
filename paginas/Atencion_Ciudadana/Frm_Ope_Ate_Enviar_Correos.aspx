<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Ate_Enviar_Correos.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Ope_Ate_Enviar_Correos"
    Culture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 78px;
        }
        .style2
        {
            width: 497px;
        }
        .tamanio_combos
        {
            width: 400px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization="true" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color: #ffffff; width: 99%; height: 100%;">
                <table border="0" cellspacing="0" class="estilo_fuente" style="height: auto; width: 100%;">
                    <tr>
                        <td colspan="4" align="center" class="label_titulo">
                            Envío de correos a contribuyentes
                        </td>
                    </tr>
                    <tr class="barra_busqueda">
                        <td colspan="4">
                            <asp:ImageButton ID="Btn_Enviar_Correos" runat="server" CausesValidation="false"
                                ToolTip="Iniciar envío de correos" Height="18px" ImageUrl="../imagenes/paginas/sias_send.png"
                                Style="vertical-align: top;" OnClick="Btn_Enviar_Correos_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                            <asp:Image ID="Img_Advertencia" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="False" />
                            <asp:Label ID="Lbl_Informacion" runat="server" Enabled="False" ForeColor="#990000"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            &nbsp;
                        </td>
                        <td>
                            Seleccione fecha de nacimiento
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="145px"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="Fte_Txt_Fecha_Inicio" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                TargetControlID="Txt_Fecha_Inicio" ValidChars="-/">
                            </ajax:FilteredTextBoxExtender>
                            <ajax:CalendarExtender ID="Cln_Txt_Fecha_Inicio" runat="server" TargetControlID="Txt_Fecha_Inicio"
                                Format="dd/MMM/yyyy" PopupButtonID="Btn_Cln_Txt_Fecha_Inicio">
                            </ajax:CalendarExtender>
                            <asp:ImageButton ID="Btn_Cln_Txt_Fecha_Inicio" runat="server" CausesValidation="false"
                                Height="18px" ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

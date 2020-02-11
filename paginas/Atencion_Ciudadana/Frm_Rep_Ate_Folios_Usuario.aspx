<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Rep_Ate_Folios_Usuario.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Rep_Ate_Folios_Usuario"
    Title="Reporte Peticiones-Usuario" Culture="es-MX" %>

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

    <script type="text/javascript" language="javascript">
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades)
        {
            window.showModalDialog(Url, null, Propiedades);
        }
    </script>

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
            <div id="Div_Requisitos" style="background-color: #ffffff; width: 97%; height: 100%;">
                <table border="0" cellspacing="0" class="estilo_fuente" style="height: auto;">
                    <tr>
                        <td colspan="4" align="center" class="label_titulo">
                            Reporte folios por usuario
                        </td>
                    </tr>
                    <tr class="barra_busqueda">
                        <td colspan="4">
                            &nbsp;
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
                    <tr id="Tr_Contenedor_Tipo_Reporte" runat="server" visible="false">
                        <td align="center" colspan="4">
                            Tipo de reporte
                            <asp:DropDownList ID="Cmb_Tipo_Reporte" runat="server" Style="width: 500px;" Visible="false">
                                <asp:ListItem Value="FOLIOS_POR_USUARIO">Folios por usuario</asp:ListItem>
                            </asp:DropDownList>
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
                            <asp:CheckBox ID="Chk_Fecha" runat="server" Text="Por Fecha de Petición" AutoPostBack="True"
                                Enabled="False" />
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="145px"></asp:TextBox>
                            <ajax:CalendarExtender ID="Cln_Txt_Fecha_Inicio" runat="server" TargetControlID="Txt_Fecha_Inicio"
                                Format="dd/MMM/yyyy" PopupButtonID="Btn_Cln_Txt_Fecha_Inicio">
                            </ajax:CalendarExtender>
                            <asp:ImageButton ID="Btn_Cln_Txt_Fecha_Inicio" runat="server" CausesValidation="false"
                                Height="18px" ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" />
                            &nbsp; a &nbsp;
                            <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="145px"></asp:TextBox>
                            <ajax:CalendarExtender ID="Cln_Txt_Fecha_Fin" runat="server" TargetControlID="Txt_Fecha_Fin"
                                Format="dd/MMM/yyyy" PopupButtonID="Btn_Cln_Txt_Fecha_Fin">
                            </ajax:CalendarExtender>
                            <asp:ImageButton ID="Btn_Cln_Txt_Fecha_Fin" runat="server" CausesValidation="false"
                                Height="18px" ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="Chk_Origen" runat="server" AutoPostBack="True" Text="Origen" OnCheckedChanged="Chk_Origen_CheckedChanged" />
                        </td>
                        <td>
                            <asp:DropDownList ID="Cmb_Origen" runat="server" Enabled="False" CssClass="tamanio_combos">
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td class="style2" align="center" colspan="3">
                            <asp:Button ID="Btn_Generar_Reporte" runat="server" Text="Generar Reporte" Width="145px"
                                class="button" OnClick="Btn_Generar_Reporte_Click" />
                            <asp:Button ID="Btn_Exportar_Excel" runat="server" Text="Exportar a  Excel" Width="145px"
                                class="button" OnClick="Btn_Exportar_Excel_Click" />
                            <asp:Button ID="Btn_Salir" runat="server" class="button" OnClick="Btn_Salir_Click"
                                Text="Salir" Width="145px" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

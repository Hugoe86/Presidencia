<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Reportes.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Reportes"
    Title="Reportes" Culture="es-MX" %>

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
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization="true" AsyncPostBackTimeout="216000" />
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
                            Reportes
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
                    <tr>
                        <td align="center" colspan="4">
                            Tipo de reporte
                            <asp:DropDownList ID="Cmb_Tipo_Reporte" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Tipo_Reporte_OnSelectedIndexChanged"
                                Style="width: 500px;">
                                <asp:ListItem Value="REPORTE_DETALLADO">Reportes detallados de peticiones</asp:ListItem>
                                <asp:ListItem Value="REPORTE_ACUMULADO">Reportes acumulados de peticiones</asp:ListItem>
                                <asp:ListItem Value="ESTADISTICAS_DEPENDENCIA">Estadísticas de peticiones por Unidad Responsable</asp:ListItem>
                                <asp:ListItem Value="ESTADISTICAS_ASUNTO">Estadísticas de peticiones por Asunto</asp:ListItem>
                                <asp:ListItem Value="TIEMPOS_RESPUESTA">Tiempos de Respuesta por Unidad Responsable</asp:ListItem>
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
                            <asp:CheckBox ID="Chk_Dependencias" runat="server" Text="Por Unidad Responsable"
                                AutoPostBack="True" OnCheckedChanged="Chk_Dependencias_CheckedChanged" />
                        </td>
                        <td class="style2">
                            <asp:DropDownList ID="Cmb_Dependencias" runat="server" CssClass="tamanio_combos"
                                AutoPostBack="True" Enabled="False" OnSelectedIndexChanged="Cmb_Dependencias_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:ImageButton ID="Btn_Buscar_Dependencia" runat="server" ToolTip="Seleccionar Unidad responsable"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                OnClick="Btn_Buscar_Dependencia_Click" Enabled="false" />
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
                            <asp:CheckBox ID="Chk_Asunto" runat="server" AutoPostBack="True" Text="Asunto" OnCheckedChanged="Chk_Asunto_CheckedChanged" />
                        </td>
                        <td>
                            <asp:DropDownList ID="Cmb_Asunto" runat="server" Enabled="false" CssClass="tamanio_combos">
                            </asp:DropDownList>
                            <asp:ImageButton ID="Btn_Buscar_Asunto" runat="server" ToolTip="Seleccionar Asunto"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                Enabled="false" OnClick="Btn_Buscar_Asunto_Click" />
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
                        <td class="style1">
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="Chk_Colonias" runat="server" Text="Por Colonia" AutoPostBack="True"
                                OnCheckedChanged="Chk_Colonias_CheckedChanged" /><br />
                            <asp:CheckBox ID="Chk_Totales_Colonia" runat="server" Text="Totales por colonia" />
                        </td>
                        <td class="style2">
                            <asp:DropDownList ID="Cmb_Colonias" runat="server" CssClass="tamanio_combos" Enabled="False">
                            </asp:DropDownList>
                            <asp:ImageButton ID="Btn_Buscar_Colonia" runat="server" ToolTip="Seleccionar Colonia"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                OnClick="Btn_Buscar_Colonia_Click" Enabled="false" />
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
                            <asp:CheckBox ID="Chk_Estatus" runat="server" Text="Por estatus" AutoPostBack="True"
                                OnCheckedChanged="Chk_Estatus_CheckedChanged" />
                        </td>
                        <td class="style2">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" CssClass="tamanio_combos" Enabled="False">
                                <asp:ListItem Value="0">&lt;Seleccione&gt;</asp:ListItem>
                                <asp:ListItem Value="TERMINADA">TERMINADA</asp:ListItem>
                                <asp:ListItem Value="EN PROCESO">EN PROCESO</asp:ListItem>
                                <asp:ListItem Value="GENERADA">GENERADA</asp:ListItem>
                            </asp:DropDownList>
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
                            <asp:CheckBox ID="Chk_Tipo_Solucion" runat="server" Text="Por Tipo de Solución" AutoPostBack="True"
                                OnCheckedChanged="Chk_Tipo_Solucion_CheckedChanged" />
                        </td>
                        <td class="style2">
                            <asp:DropDownList ID="Cmb_Tipo_Solucion" runat="server" CssClass="tamanio_combos"
                                Enabled="False">
                                <asp:ListItem Value="0">&lt;Seleccione&gt;</asp:ListItem>
                                <asp:ListItem Value="POSITIVA">POSITIVA</asp:ListItem>
                                <asp:ListItem Value="NEGATIVA">NEGATIVA</asp:ListItem>
                            </asp:DropDownList>
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
                            <asp:CheckBox ID="Chk_Sexo" runat="server" Text="Por sexo" AutoPostBack="True" OnCheckedChanged="Chk_Sexo_CheckedChanged" />
                        </td>
                        <td class="style2">
                            <asp:DropDownList ID="Cmb_Sexo" runat="server" CssClass="tamanio_combos" Enabled="False">
                                <asp:ListItem Value="0">&lt;Seleccione&gt;</asp:ListItem>
                                <asp:ListItem Value="MASCULINO">MASCULINO</asp:ListItem>
                                <asp:ListItem Value="FEMENINO">FEMENINO</asp:ListItem>
                            </asp:DropDownList>
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
                            <asp:CheckBox ID="Chk_Folios_Vencidos" runat="server" Text="Folios Vencidos" />
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="Chk_Con_Telefono" runat="server" Text="Peticiones con número de teléfono" />
                        </td>
                        <td colspan="2">
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

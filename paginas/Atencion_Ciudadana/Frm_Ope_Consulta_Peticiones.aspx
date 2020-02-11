<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Consulta_Peticiones.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Ope_Consulta_Peticiones"
    Title="Consulta de Peticiones" Culture="es-MX" %>

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
    <asp:ScriptManager ID="ScriptManager_Consulta_Peticiones" runat="server" EnableScriptGlobalization="true" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
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
                <table border="0" cellspacing="0" class="estilo_fuente" width="100%">
                    <tr>
                        <td colspan="3" align="center" class="label_titulo">
                            Consulta de Peticiones
                        </td>
                    </tr>
                    <tr class="barra_busqueda">
                        <td colspan="3" align="left">
                            <asp:ImageButton ID="Btn_Consultar" runat="server" ToolTip="Consultar" CssClass="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" OnClick="Btn_Consultar_Click" />
                            <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir Listado" CssClass="Img_Button"
                                ImageUrl="~/paginas/imagenes/gridview/grid_print.png" OnClick="Btn_Imprimir_Click" />
                            <asp:ImageButton ID="Btn_Exportar_Excel" runat="server" ToolTip="Exportar listado a Excel" CssClass="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" OnClick="Btn_Exportar_Excel_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Image ID="Img_Advertencia" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="False" />
                            <asp:Label ID="Lbl_Informacion" runat="server" Enabled="False" ForeColor="#990000"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="Ckb_Fecha" runat="server" Checked="True" />
                        </td>
                        <td>
                            Fecha
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="175px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            a &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="175px"></asp:TextBox>
                            <cc1:CalendarExtender ID="Calendar_Fecha_Inicio" runat="server" TargetControlID="Txt_Fecha_Inicio"
                                Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="Calendar_Fecha_Fin" runat="server" TargetControlID="Txt_Fecha_Fin"
                                Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                        <tr>
                            <%--                        <td colspan="3" align="right">
                            &nbsp;</td>--%>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="Chk_Dependencia" runat="server" OnCheckedChanged="Chk_Dependencia_CheckedChanged"
                                    AutoPostBack="True" />
                            </td>
                            <td>
                                Unidad Responsable
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="95%" Enabled="False"
                                    OnSelectedIndexChanged="Cmb_Dependencia_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:ImageButton ID="Btn_Buscar_Dependencia" runat="server" ToolTip="Seleccionar Unidad responsable"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                    OnClick="Btn_Buscar_Dependencia_Click" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="Chk_Asunto" runat="server" AutoPostBack="True" OnCheckedChanged="Chk_Asunto_CheckedChanged" />
                            </td>
                            <td>
                                Asunto
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Asunto" runat="server" Width="95%" Enabled="false">
                                </asp:DropDownList>
                                <asp:ImageButton ID="Btn_Buscar_Asunto" runat="server" ToolTip="Seleccionar Asunto"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                    OnClick="Btn_Buscar_Asunto_Click" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="Chk_Origen" runat="server" AutoPostBack="True" OnCheckedChanged="Chk_Origen_CheckedChanged" />
                            </td>
                            <td>
                                Origen
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Origen" runat="server" Width="100%" Enabled="False">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="Chk_Colonia" runat="server" AutoPostBack="True" OnCheckedChanged="Chk_Colonia_CheckedChanged" />
                            </td>
                            <td>
                                Colonia
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Colonia" runat="server" Width="95%" Enabled="false" />
                                <asp:ImageButton ID="Btn_Buscar_Colonia" runat="server" ToolTip="Seleccionar Colonia"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                    OnClick="Btn_Buscar_Colonia_Click" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="Chk_Nombre_Solicitante" runat="server" AutoPostBack="True" OnCheckedChanged="Chk_Nombre_Solicitante_CheckedChanged" />
                            </td>
                            <td>
                                Nombre Solicitante
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Nombre_Solicitante" runat="server" Width="99%" Enabled="False"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Nombre_Solicitante" runat="server" TargetControlID="Txt_Nombre_Solicitante"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars=" .ÑñáéíóúÁÉÍÓÚ">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="Chk_Folio" runat="server" OnCheckedChanged="Chk_Folio_CheckedChanged"
                                    AutoPostBack="True" />
                            </td>
                            <td>
                                Folio
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Folio" runat="server" Width="99%" Enabled="False"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Folio" runat="server" TargetControlID="Txt_Folio"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="-">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="Chk_Estatus" runat="server" AutoPostBack="True" OnCheckedChanged="Chk_Estatus_CheckedChanged" />
                            </td>
                            <td>
                                Estatus
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="175px" Enabled="False">
                                    <asp:ListItem Value="0">&lt;Seleccione&gt;</asp:ListItem>
                                    <asp:ListItem Value="TERMINADA">TERMINADA</asp:ListItem>
                                    <asp:ListItem Value="EN PROCESO">EN PROCESO</asp:ListItem>
                                    <asp:ListItem Value="GENERADA">GENERADA</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="right">
                                <hr class="linea" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="right">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="right">
                                &nbsp;
                            </td>
                        </tr>
                </table>
                <div style="max-height: 300px; overflow-x: hidden; overflow-y: auto;">
                    <table border="0" cellspacing="0" class="estilo_fuente" width="100%">
                        <tr>
                            <td align="center" colspan="4">
                                <asp:GridView ID="Grid_Consulta_Peticiones" runat="server" AutoGenerateColumns="False"
                                    AllowPaging="false" CssClass="GridView_1" OnSelectedIndexChanged="Grid_Consulta_Peticiones_SelectedIndexChanged"
                                    AllowSorting="true" OnSorting="Grid_Consulta_Peticiones_Sorting" Width="100%"
                                    GridLines="None">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/grid_print.png">
                                            <ItemStyle Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="True" SortExpression="Folio">
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Asunto" HeaderText="Asunto" Visible="True" SortExpression="Asunto">
                                            <HeaderStyle HorizontalAlign="Left" Width="18%" />
                                            <ItemStyle HorizontalAlign="Left" Width="18%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Peticion" HeaderText="Petición" Visible="True" SortExpression="Peticion">
                                            <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                            <ItemStyle HorizontalAlign="Left" Width="25%" Wrap="true" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Nombre_Solicitante" HeaderText="Nombre Solicitante" Visible="True"
                                            SortExpression="Nombre_Solicitante">
                                            <HeaderStyle HorizontalAlign="Left" Width="18%" />
                                            <ItemStyle HorizontalAlign="Left" Width="18%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DEPENDENCIA" HeaderText="Unidad Responsable" Visible="True"
                                            SortExpression="DEPENDENCIA">
                                            <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                            <ItemStyle HorizontalAlign="Left" Width="25%" />
                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

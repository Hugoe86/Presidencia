<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Ate_Listado_Vacantes.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Ope_Ate_Listado_Vacantes"
    Title="Listado de vacantes" Culture="en-Us" %>

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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="background-color: #ffffff; width: 95%; height: 75%;">
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
                <div id="Div_Datos_Peticion" runat="server">
                    <asp:Panel ID="Pnl_Datos_Peticion" runat="server">
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="label_titulo">
                                    Listado de vacantes
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Image ID="Img_Warning" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" />
                                    <br />
                                    <asp:Label ID="Lbl_Warning" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%;">
                            <tr class="barra_busqueda" align="right">
                                <td colspan="4" align="left" valign="middle">
                                    <div>
                                        &nbsp;
                                        <asp:ImageButton ID="Btn_Consultar" runat="server" ToolTip="Consultar" CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" OnClick="Btn_Consultar_Click" />
                                        <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir listado de vacantes"
                                            CssClass="Img_Button" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" OnClick="Btn_Imprimir_Click" />
                                        <asp:ImageButton ID="Btn_Importar_Archivo_Excel" runat="server" ToolTip="Importar archivo de vacantes"
                                            CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/sias_upload.png" OnClick="Btn_Importar_Archivo_Excel_Click" />
                                        <a id="Btn_Descarga_Plantilla" runat="server" href="../../Archivos/Atencion_Ciudadana/Plantillas/Listado%20de%20vacantes%20de%20la%20Bolsa%20Municipal%20de%20Empleo.xlsx">
                                            <img alt="Descargar plantilla para listado de vacantes" src="../imagenes/paginas/sias_download.png" width="22px" height="22px" style="border:none;" /></a>
                                        <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                            ToolTip="Inicio" OnClick="Btn_Salir_Click" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div runat="server" id="Contenedor_Vacantes">
                            <table style="width: 100%;">
                                <tr>
                                    <td colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%;">
                                        Vacante
                                    </td>
                                    <td style="width: 35%;">
                                        <asp:TextBox ID="Txt_Vacante" runat="server" MaxLength="50" Width="90%"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Vacante" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                            TargetControlID="Txt_Vacante" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td style="width: 15%; text-align: right;">
                                        Sexo
                                    </td>
                                    <td style="width: 35%;">
                                        <asp:DropDownList ID="Cmb_Sexo" runat="server" Width="99%">
                                            <asp:ListItem Value="0">&lt;Seleccione&gt;</asp:ListItem>
                                            <asp:ListItem Value="MASCULINO">MASCULINO</asp:ListItem>
                                            <asp:ListItem Value="FEMENINO">FEMENINO</asp:ListItem>
                                            <asp:ListItem Value="INDISTINTO">INDISTINTO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%;">
                                        Escolaridad
                                    </td>
                                    <td style="width: 35%;">
                                        <asp:TextBox ID="Txt_Escolaridad" runat="server" MaxLength="50" Width="90%"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Escolaridad" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                            TargetControlID="Txt_Escolaridad" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td style="width: 15%; text-align: right;">
                                        Experiencia
                                    </td>
                                    <td style="width: 35%;">
                                        <asp:TextBox ID="Txt_Experiencia" runat="server" MaxLength="50" Width="98%"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Experiencia" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                            TargetControlID="Txt_Experiencia" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%;">
                                        Número de vacante
                                    </td>
                                    <td style="width: 35%;">
                                        <asp:TextBox ID="Txt_Numero_Vacante" runat="server" MaxLength="15" Width="90%"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Numero_Vacante" runat="server" FilterType="Numbers,Custom"
                                            TargetControlID="Txt_Numero_Vacante" ValidChars=", ">
                                        </cc1:FilteredTextBoxExtender>
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
                            </table>
                            <div runat="server" id="Contenedor_Grid_Vacantes" style="clear: both; width: 100%;">
                                <asp:GridView ID="Grid_Vacantes" runat="server" Width="100%" AutoGenerateColumns="False"
                                    CssClass="GridView_1" AllowPaging="false" GridLines="None" Font-Underline="False"
                                    EmptyDataText="No se encontraron vacantes" AllowSorting="true" Style="font-size: xx-small;
                                    white-space: normal" OnSorting="Grid_Vacantes_Sorting" OnRowDataBound="Grid_Vacantes_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="NO_VACANTE" HeaderText="No." SortExpression="NO_VACANTE" />
                                        <asp:BoundField DataField="CONTACTO" Visible="false" />
                                        <asp:BoundField DataField="NOMBRE_VACANTE" HeaderText="Vacante" SortExpression="NOMBRE_VACANTE">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EDAD" HeaderText="Edad" SortExpression="EDAD">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="6%" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SEXO" HeaderText="Sexo" SortExpression="SEXO">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESCOLARIDAD" HeaderText="Escolaridad" SortExpression="ESCOLARIDAD">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="11%" Wrap="true" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EXPERIENCIA" HeaderText="Experiencia" SortExpression="EXPERIENCIA">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="25%" Wrap="true" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SUELDO" HeaderText="Sueldo" SortExpression="SUELDO">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="14%" Wrap="true" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="9%">
                                            <HeaderTemplate>
                                                <asp:ImageButton ID="Btn_Grid_Imprimir_Varias_Vacantes" runat="server" ImageUrl="~/paginas/imagenes/gridview/grid_print.png"
                                                    Style="width: 20px; height: 20px;" CssClass="Img_Button" ToolTip="Imprimir vacantes (máximo 3)"
                                                    OnClick="Btn_Grid_Imprimir_Varias_Vacantes_Click" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Grid_Contacto_Vacante" runat="server" ImageUrl="~/paginas/imagenes/paginas/empleado.png"
                                                    Style="width: 20px; height: 20px;" CssClass="Img_Button" ToolTip="Contacto" OnClick="Btn_Grid_Contacto_Vacante_Click" />
                                                <asp:ImageButton ID="Btn_Grid_Imprimir_Vacante" runat="server" ImageUrl="~/paginas/imagenes/gridview/grid_print.png"
                                                    Style="width: 20px; height: 20px;" CssClass="Img_Button" ToolTip="Imprimir vacante"
                                                    OnClick="Btn_Grid_Imprimir_Vacante_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" Wrap="True" />
                                </asp:GridView>
                            </div>
                        </div>
                        <div runat="server" id="Contenedor_Subir_Archivo">
                            <table width="100%">
                                <tr>
                                    <td style="text-align: left; width: 20%;">
                                        *Archivo
                                    </td>
                                    <td style="text-align: left; width: 80%;" colspan="3">
                                        <asp:FileUpload ID="Fup_Archivo" runat="server" Width="95%" size="84%" />
                                        <asp:ImageButton ID="Btn_Subir_Archivo" runat="server" ToolTip="Enviar archivo" Style="border: 0 none;
                                            width: 20px; height: 20px; padding: 0;" ImageUrl="~/paginas/imagenes/paginas/sias_upload.png"
                                            OnClick="Btn_Subir_Archivo_Click" />
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label_titulo" colspan="4">
                                        <asp:GridView ID="Grid_Vacantes_Archivo" runat="server" Height="100%" AutoGenerateColumns="False"
                                            CssClass="GridView_1" AllowPaging="false" GridLines="None" Font-Underline="False"
                                            AllowSorting="true" Style="font-size: xx-small; white-space: normal" OnSorting="Grid_Vacantes_Archivo_Sorting">
                                            <Columns>
                                                <asp:BoundField DataField="NO_VACANTE" HeaderText="No." SortExpression="NO_VACANTE" />
                                                <asp:BoundField DataField="NOMBRE_VACANTE" HeaderText="Vacante" Visible="True" SortExpression="NOMBRE_VACANTE">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="EDAD" HeaderText="Edad" Visible="True" SortExpression="EDAD">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SEXO" HeaderText="Sexo" Visible="True" SortExpression="SEXO">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" Width="12%" Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ESCOLARIDAD" HeaderText="Escolaridad" Visible="True" SortExpression="ESCOLARIDAD">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" Width="12%" Wrap="true" Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="EXPERIENCIA" HeaderText="Experiencia" Visible="True" SortExpression="EXPERIENCIA">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="true" Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SUELDO" HeaderText="Sueldo" Visible="True" SortExpression="SUELDO">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="true" Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CONTACTO" HeaderText="Contacto" Visible="True" SortExpression="CONTACTO">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" Wrap="true" Font-Size="X-Small" />
                                                </asp:BoundField>
                                            </Columns>
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" Wrap="True" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

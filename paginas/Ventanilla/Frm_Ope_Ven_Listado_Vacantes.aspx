<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Ventanilla.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Ven_Listado_Vacantes.aspx.cs" Inherits="paginas_Ventanilla_Frm_Ope_Ven_Listado_Vacantes"
    Title="Listado de vacantes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="True" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
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
            <div id="Div_General" runat="server" style="background-color: #ffffff; width: 98%;
                height: 100%;">
                <%--Fin del div General--%>
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente" frame="border">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">
                            Listado de vacantes
                        </td>
                    </tr>
                    <tr>
                        <!--Bloque del mensaje de error-->
                        <td colspan="2">
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left">
                            <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ToolTip="Salir"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                        </td>
                        <td>
                            Búsqueda
                            <asp:DropDownList ID="Cmb_Filtro" runat="server" Width="180px">
                                <asp:ListItem Value="0">&lt;Seleccione&gt;</asp:ListItem>
                                <asp:ListItem Value="VACANTE">VACANTE</asp:ListItem>
                                <asp:ListItem Value="SEXO">SEXO</asp:ListItem>
                                <asp:ListItem Value="ESCOLARIDAD">ESCOLARIDAD</asp:ListItem>
                                <asp:ListItem Value="EXPERIENCIA">EXPERIENCIA</asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180px" MaxLength="13" ToolTip="Buscar por Folio."></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </cc1:FilteredTextBoxExtender>
                            <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                OnClick="Btn_Buscar_Click" />
                        </td>
                    </tr>
                </table>
                <div runat="server" id="Contenedor_Grid_Vacantes" style="width: 98%; margin-top:10px;">
                    <asp:GridView ID="Grid_Vacantes" runat="server" Width="100%" AutoGenerateColumns="False"
                        CssClass="GridView_1" AllowPaging="false" GridLines="None" Font-Underline="False"
                        EmptyDataText="No se encontraron vacantes" AllowSorting="false" Style="font-size: small;
                        white-space: normal" OnRowDataBound="Grid_Vacantes_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="NO_VACANTE" HeaderText="No." SortExpression="NO_VACANTE" />
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
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="9%">
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
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

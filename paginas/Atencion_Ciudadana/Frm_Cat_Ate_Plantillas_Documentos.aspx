<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Cat_Ate_Plantillas_Documentos.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Cat_Ate_Plantillas_Documentos"
    Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .Enlace_Archivo:link
        {
            color: #3F68B6;
            font-weight: normal;
        }
        .Enlace_Archivo:hover
        {
            color: #00388C;
            font-weight: bold;
        }
        .Enlace_Archivo:active
        {
            color: #00388C;
        }
        .Enlace_Archivo:visited
        {
            color: #3F68B6;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="background-color: #ffffff; width: 95%; height: 75%;">
        <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Conditional">
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
                <table style="width: 100%;">
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="label_titulo">
                            Plantillas de documentos de Atencion_Ciudadana
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="Img_Warning" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
                                Visible="false" />
                            <br />
                            <asp:Label ID="Lbl_Warning" runat="server" ForeColor="Red" Text="" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%; background-color: #2F4E7D;">
                    <tr class="barra_busqueda" align="right">
                        <td colspan="2" align="left" valign="middle">
                            &nbsp;
                            <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                ToolTip="Inicio" OnClick="Btn_Salir_Click" />
                        </td>
                        <td colspan="2">
                            Búsqueda
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180" MaxLength="13" ToolTip="Buscar nombre de archivo."></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="< Nombre archivo >" TargetControlID="Txt_Busqueda" />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="_-.">
                            </cc1:FilteredTextBoxExtender>
                            <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                OnClick="Btn_Buscar_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <div style="text-align: center;">
                    <asp:GridView ID="Grid_Archivos" runat="server" Height="100%" AutoGenerateColumns="False"
                        CssClass="GridView_1" AllowPaging="false" GridLines="None" Font-Underline="False"
                        EmptyDataText="No se encontraron archivos" AllowSorting="false" Style="font-size: xx-small;
                        white-space: normal" OnRowDataBound="Grid_Archivos_OnRowDataBound">
                        <Columns>
                            <asp:BoundField DataField="RUTA_ARCHIVO" Visible="false" />
                            <asp:BoundField DataField="NOMBRE_ARCHIVO" HeaderText="Archivo" Visible="True">
                                <FooterStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                            </asp:BoundField>
                            <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Right"/>
                                <ItemTemplate>
                                    <asp:FileUpload ID="Fup_Archivo_Plantilla" runat="server" />
                                    <asp:ImageButton ID="Btn_Actualizar_Archivo" runat="server" ImageUrl="~/paginas/imagenes/paginas/actualizar_detalle.png"
                                        CssClass="Img_Button" ToolTip="Actualizar archivo" OnClick="Btn_Actualizar_Archivo_Click"
                                        OnClientClick="return confirm('El archivo en el servidor será sustituido ¿Desea continuar?');" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

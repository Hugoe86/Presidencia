<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Ate_Seguimiento_Peticiones.aspx.cs"
    Inherits="paginas_Atencion_Ciudadana_Frm_Ope_Atencion_Seguimiento_Peticiones" %>

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
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600"></cc1:ToolkitScriptManager>
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
                                    Seguimiento a Peticiones
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
                        <table style="width: 100%; background-color: #2F4E7D;">
                            <tr class="barra_busqueda" align="right">
                                <td colspan="2" align="left" valign="middle">
                                    <div>
                                        &nbsp;
                                        <asp:ImageButton ID="Btn_Reasignar" runat="server" AlternateText="Modificar" CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Reasignar_Click" />
                                        <asp:ImageButton ID="Btn_Solucion" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_respuesta_peticion.png"
                                            OnClick="Btn_Solucion_Click" ToolTip="Marcar como revisada" />
                                        <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir petición" CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/gridview/grid_print.png" OnClick="Btn_Imprimir_Peticion_Click" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                            ToolTip="Inicio" OnClick="Btn_Salir_Click" />
                                    </div>
                                </td>
                                <td colspan="2">
                                    Búsqueda
                                    <asp:TextBox ID="Txt_Busqueda_Registro_Peticion" runat="server" Width="180" MaxLength="20"
                                        ToolTip="Buscar por Folio."></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Registro_Peticion" runat="server"
                                        WatermarkCssClass="watermarked" WatermarkText="< Folio de petición >" TargetControlID="Txt_Busqueda_Registro_Peticion" />
                                    <cc1:FilteredTextBoxExtender ID="Fte_Busqueda_Registro_Petision" runat="server" TargetControlID="Txt_Busqueda_Registro_Peticion"
                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="-">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:ImageButton ID="Btn_Buscar_Registro_Peticion" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                        OnClick="Btn_Buscar_Registro_Peticion_Click" />
                                </td>
                            </tr>
                        </table>
                        <div runat="server" id="Contenedor_Controles_Seguimiento">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 25%">
                                        Folio
                                    </td>
                                    <td style="width: 25%">
                                        <asp:TextBox ID="Txt_Folio" runat="server" Width="200px"></asp:TextBox>
                                    </td>
                                    <td style="width: 25%; text-align: right;">
                                        Fecha de petición
                                    </td>
                                    <td style="width: 25%">
                                        <asp:TextBox ID="Txt_Fecha" runat="server" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 25%">
                                        Nombre de Ciudadano
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Nombre" runat="server" Width="98.8%"></asp:TextBox>
                                        <asp:HiddenField ID="Hdn_Email" runat="server" />
                                        <asp:HiddenField ID="Hdn_No_Peticion" runat="server" />
                                        <asp:HiddenField ID="Hdn_Anio" runat="server" />
                                        <asp:HiddenField ID="Hdn_Programa_Id" runat="server" />
                                        <asp:HiddenField ID="Hdn_Contribuyente_Id" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 25%">
                                        Domicilio
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Domicilio" runat="server" Width="98.8%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 25%">
                                        Referencia
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Referencia" runat="server" Width="98.8%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%;">
                                        <asp:Label ID="Lbl_Cmb_Asunto" runat="server" Text="Asunto"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="Cmb_Asunto" runat="server" Width="95%" AutoPostBack="true"
                                            OnSelectedIndexChanged="Cmb_Asunto_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="Btn_Buscar_Asunto" runat="server" ToolTip="Seleccionar Asunto"
                                            ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                            OnClick="Btn_Buscar_Asunto_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%;">
                                        <asp:Label ID="Lbl_Cmb_Dependencia" runat="server" Text="Unidad Responsable"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="95%" AutoPostBack="true"
                                            OnSelectedIndexChanged="Cmb_Dependencia_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="Btn_Buscar_Dependencia" runat="server" ToolTip="Seleccionar Unidad responsable"
                                            ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                            OnClick="Btn_Buscar_Dependencia_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%">
                                        Estatus
                                    </td>
                                    <td style="width: 26%">
                                        <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="205px">
                                            <asp:ListItem Value="">&lt;Seleccione&gt;</asp:ListItem>
                                            <asp:ListItem Value="TERMINADA">Terminada</asp:ListItem>
                                            <asp:ListItem Value="EN PROCESO">En proceso</asp:ListItem>
                                            <asp:ListItem Value="GENERADA">Generada</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 25%" valign="top">
                                        <asp:Label ID="Lbl_Motivo" runat="server" Text="Motivo de cambio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Motivo" runat="server" Height="50px" TextMode="MultiLine" Width="99%"
                                            MaxLength="200"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Motivo"
                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ.,;:()#¿?!¡@$+-/* ">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkCssClass="watermarked"
                                            WatermarkText="<Límite de Caracteres - 200>" TargetControlID="Txt_Motivo" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 25%" valign="top">
                                        Descripción
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Descripcion" runat="server" Height="50px" TextMode="MultiLine"
                                            Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div id="Div_Seguimiento_Consecutivo" runat="server" style="text-align: right; color: #5D7B9D;
                                display: none;">
                                <div style="float: left; margin-top: 8px; width: 193px; text-align: left;">
                                    *Seguimiento consecutivo (Para generar formato)</div>
                                <asp:DropDownList ID="Cmb_Seguimiento_Consecutivo" runat="server" Style="width: 75%;
                                    position: relative; right: 4px;">
                                    <asp:ListItem Value=""> &lt;Seleccione&gt; </asp:ListItem>
                                    <asp:ListItem Value="RESOLUCION">Analizar la petición y brindar resolución.</asp:ListItem>
                                    <asp:ListItem Value="CONOCIMIENTO">Para conocimiento.</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div id="Div_Cargar_Archivo" runat="server" style="text-align: right; color: #5D7B9D;
                                display: none;">
                                <fieldset>
                                    <legend>Adjuntar documento</legend>
                                    <div style="float: left; margin-top: 8px;">
                                        Seleccione el archivo</div>
                                    <asp:FileUpload ID="Fup_Subir_Archivo" runat="server" size="74" ToolTip="Formatos: .jpg .png .pdf .doc .pps" />
                                    <asp:ImageButton ID="Btn_Subir_Archivo" runat="server" Height="20px" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                        OnClick="Btn_Subir_Archivo_Click" ToolTip="Agregar Costo" Width="20px" />
                                    <asp:GridView ID="Grid_Archivos" runat="server" AutoGenerateColumns="False" HeaderStyle-CssClass="tblHead"
                                        OnSelectedIndexChanged="Grid_Archivos_SelectedIndexChanged" Style="white-space: normal;
                                        margin: 8px auto;" Width="98%">
                                        <Columns>
                                            <asp:BoundField DataField="RUTA_ARCHIVO" HeaderText="Archivos">
                                                <ItemStyle HorizontalAlign="left" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Btn_Eliminar_Archivo" runat="server" CommandName="Select" Height="20px"
                                                        ImageUrl="~/paginas/imagenes/paginas/delete.png" ToolTip="Eliminar" Width="20px" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="2%" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="tblHead" />
                                    </asp:GridView>
                                </fieldset>
                            </div>
                            <table width="100%">
                                <tr>
                                    <td colspan="3">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 25%" valign="top">
                                        <asp:Label ID="Lbl_Txt_Solucion" runat="server" Text="Solución"></asp:Label>
                                    </td>
                                    <td style="width: 75%" colspan="2">
                                        <asp:DropDownList ID="Cmb_Solucion" runat="server" Width="205px" Style="margin-bottom: 6px;">
                                            <asp:ListItem Value="">&lt;Seleccione&gt;</asp:ListItem>
                                            <asp:ListItem Value="POSITIVA">POSITIVA</asp:ListItem>
                                            <asp:ListItem Value="NEGATIVA">NEGATIVA</asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                        <asp:TextBox ID="Txt_Solucion" runat="server" Height="50px" TextMode="MultiLine"
                                            Width="100%" MaxLength="3000"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="Txt_Solucion"
                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ.,;:()#¿?!¡@$+-/* ">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" WatermarkCssClass="watermarked"
                                            WatermarkText="<Límite de Caracteres - 3000>" TargetControlID="Txt_Solucion" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 25%">
                                        <asp:Label ID="Lbl_Fecha_Probable" runat="server" Text="Fecha de Solución Probable"></asp:Label>
                                    </td>
                                    <td colspan="2" style="width: 75%">
                                        <asp:TextBox ID="Txt_Fecha_Probable" runat="server" Width="200px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="Txt_Fecha_Probable_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="Txt_Fecha_Probable" DaysModeTitleFormat="MMMM,yyyy" Format="dd/MMM/yyyy">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                            <div runat="server" id="Contenedor_Grid_Seguimiento" style="clear: both;">
                                <table width="100%">
                                    <tr>
                                        <td class="label_titulo" colspan="3">
                                            <asp:Label ID="Lbl_Seguimiento" runat="server" Text="Seguimiento"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label_titulo" colspan="3">
                                            <asp:GridView ID="Grid_Seguimiento" runat="server" Height="100%" AutoGenerateColumns="False"
                                                CssClass="GridView_1" AllowPaging="false" GridLines="None" Font-Underline="False"
                                                EmptyDataText="No se encontraron registros" AllowSorting="true" OnSorting="Grid_Seguimiento_Sorting"
                                                Style="font-size: xx-small; white-space: normal">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="No">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FECHA_ASIGNACION" HeaderText="Fecha" Visible="True" DataFormatString="{0:dd/MMM/yyyy}"
                                                        SortExpression="FECHA_ASIGNACION">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DEPENDENCIA" HeaderText="Dependencia" Visible="True" SortExpression="DEPENDENCIA">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PROGRAMA" HeaderText="Programa" Visible="True" SortExpression="PROGRAMA">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OBSERVACIONES" HeaderText="Observaciones" Visible="True"
                                                        SortExpression="OBSERVACIONES">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="true" Font-Size="X-Small" />
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
                            <div runat="server" id="Contenedor_Grid_Historial" style="clear: both; margin-top: 10px;">
                                <table width="100%">
                                    <tr>
                                        <td class="label_titulo" colspan="3">
                                            <asp:Label ID="Lbl_Historial" runat="server" Text="Historial"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label_titulo" colspan="3">
                                            <asp:GridView ID="Grid_Observaciones" runat="server" Height="100%" AutoGenerateColumns="False"
                                                CssClass="GridView_1" AllowPaging="false" GridLines="None" Font-Underline="False"
                                                AllowSorting="true" OnSorting="Grid_Observaciones_Sorting" Style="font-size: xx-small;
                                                white-space: normal">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="No">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Width="6%" />
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha" Visible="True" DataFormatString="{0:dd/MMM/yyyy}"
                                                        SortExpression="FECHA">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Left" Width="11%" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OBSERVACION" HeaderText="Observación" Visible="True" SortExpression="OBSERVACION">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="True" SortExpression="ESTATUS">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="true" Font-Size="X-Small" />
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
                            <div runat="server" id="Contenedor_Grid_Archivos" style="clear: both; margin-top: 10px;">
                                <table width="100%">
                                    <tr>
                                        <td class="label_titulo" colspan="3">
                                            <asp:Label ID="Lbl_Archivos" runat="server" Text="Documentos adjuntos"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label_titulo" colspan="3">
                                            <asp:GridView ID="Grid_Archivos_Peticiones" runat="server" Height="100%" AutoGenerateColumns="False"
                                                CssClass="GridView_1" AllowPaging="false" GridLines="None" Font-Underline="False"
                                                AllowSorting="false" OnSelectedIndexChanged="Grid_Archivos_Peticiones_OnSelectedIndexChanged"
                                                Style="font-size: xx-small; white-space: normal" OnRowDataBound="Grid_Archivos_Peticiones_OnRowDataBound">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle Width="5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="RUTA_ARCHIVO" Visible="false" />
                                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha" Visible="True" DataFormatString="{0:dd/MMM/yyyy}">
                                                        <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RUTA_ARCHIVO" HeaderText="Archivo" Visible="True">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ESTATUS_PETICION" HeaderText="Estatus" Visible="True">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="true" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="Btn_Baja_Archivo" runat="server" CommandArgument='<%# Eval("NO_ARCHIVO") %>'
                                                                ToolTip="Eliminar archivo" Height="20px" ImageUrl="~/paginas/imagenes/paginas/delete.png"
                                                                Width="20px" OnClick="Btn_Baja_Archivo_Click" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="2%" />
                                                    </asp:TemplateField>
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
                        </div>
                        <div runat="server" id="Contenedor_Grid_Peticiones" style="clear: both;">
                            <table width="100%">
                                <tr>
                                    <td style="text-align: left;">
                                        Unidad Responsable
                                        <asp:DropDownList ID="Cmb_Dependencia_Peticiones" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="Cmb_Dependencia_Peticiones_SelectedIndexChanged" Style="width: 77%;
                                            margin-left: 10px;">
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="Btn_Buscar_Dependencia_Filtro_Inicial" runat="server" ToolTip="Seleccionar Unidad responsable"
                                            ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                            OnClick="Btn_Buscar_Dependencia_Filtro_Inicial_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label_titulo">
                                        <asp:Label ID="Lbl_Peticiones" runat="server" Text="Peticiones"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label_titulo">
                                        <asp:GridView ID="Grid_Peticiones" runat="server" Height="100%" AutoGenerateColumns="False"
                                            CssClass="GridView_1" AllowPaging="false" GridLines="None" Font-Underline="False"
                                            EmptyDataText="No se encontraron registros" AllowSorting="true" OnSorting="Grid_Peticiones_Sorting"
                                            OnSelectedIndexChanged="Grid_Peticiones_SelectedIndexChanged" OnRowDataBound="Grid_Peticiones_OnRowDataBound"
                                            Style="font-size: xx-small; white-space: normal">
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="5%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="NO_PETICION" Visible="false" />
                                                <asp:BoundField DataField="ANIO_PETICION" Visible="false" />
                                                <asp:BoundField DataField="PROGRAMA_ID" Visible="false" />
                                                <asp:BoundField DataField="FECHA_SOLUCION_PROBABLE" Visible="false" />
                                                <asp:BoundField DataField="FECHA_PETICION" HeaderText="Fecha" Visible="True" DataFormatString="{0:dd/MMM/yyyy}"
                                                    SortExpression="FECHA_PETICION">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FOLIO" HeaderText="Folio" Visible="True" SortExpression="FOLIO">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DEPENDENCIA" HeaderText="Dependencia" Visible="True" SortExpression="DEPENDENCIA">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ASUNTO" HeaderText="Asunto" Visible="True" SortExpression="ASUNTO">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="True" SortExpression="ESTATUS">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="12%" Wrap="true" Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <%# (Eval("POR_VALIDAR")).ToString() != "SI" ? 
                                                        "" : "<img border='0' src='../imagenes/gridview/tecnico.png' alt='Petici&oacute;n modificada' />" %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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
                        <br />
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Ventanilla.master"
    AutoEventWireup="true" CodeFile="Frm_Rpt_Ven_Consultar_Peticion.aspx.cs" Inherits="paginas_Ventanilla_Frm_Rpt_Ven_Consultar_Peticion"
    Title="Consultar Peticiones" %>

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
                            Consultar Peticiones
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
                            <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir listado de vacantes"
                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" OnClick="Btn_Imprimir_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ToolTip="Salir"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                        </td>
                        <td>
                            <td>
                    </tr>
                </table>
                <table class="estilo_fuente" width="100%">
                    <tr>
                        <td style="width: 20%">
                            <asp:Label ID="Lbl_Folio_Peticion" runat="server" Text="Folio de la petición"></asp:Label>
                        </td>
                        <td style="width: 40%">
                            <asp:TextBox ID="Txt_Folio_Peticion" runat="server" Width="70%" MaxLength="12"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Folio_Peticion" runat="server" TargetControlID="Txt_Folio_Peticion"
                                FilterType="Numbers, Custom, UppercaseLetters, LowercaseLetters" ValidChars="-ñÑ">
                            </cc1:FilteredTextBoxExtender>
                            <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                OnClick="Btn_Buscar_Click" />
                        </td>
                        <td style="width: 40%">
                        </td>
                    </tr>
                </table>
                <br />
                <div id="Contenedor_Datos_Peticion" runat="server">
                    <fieldset>
                        <legend>Información personal</legend>
                        <table width="100%" cellspacing="0">
                            <tr>
                                <td style="width: 15%;">
                                    Solicitante
                                </td>
                                <td style="width: 85%;" colspan="3">
                                    <asp:TextBox ID="Txt_Solicitante" runat="server" Width="97%" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%;">
                                    Sexo
                                </td>
                                <td style="width: 35%;">
                                    <asp:TextBox ID="Txt_Sexo" runat="server" Width="90%" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 15%; text-align: right;">
                                    Edad
                                </td>
                                <td style="width: 35%;">
                                    <asp:TextBox ID="Txt_Edad" runat="server" Width="92%" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Domicilio
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Domicilio" runat="server" MaxLength="100" Width="97%" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Referencia
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Referencia" runat="server" MaxLength="100" Width="97%" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Código Postal
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Codigo_Postal" runat="server" Width="90%" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="text-align: right;">
                                    E-mail
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Email" runat="server" Width="92%" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Teléfono
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Telefono" runat="server" Width="90%" Enabled="false"></asp:TextBox>
                                </td>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <table width="100%" cellspacing="0">
                        <tr>
                            <td colspan="4">
                                <hr class="linea" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;" valign="top">
                                Petición
                            </td>
                            <td style="width: 85%;" colspan="3">
                                <asp:TextBox ID="Txt_Peticion" runat="server" TextMode="MultiLine" Width="97%" Height="65"
                                    Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;">
                                Estatus
                            </td>
                            <td style="width: 35%;">
                                <asp:TextBox ID="Txt_Estatus" runat="server" Width="90%" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width: 15%;">
                                Fecha petición
                            </td>
                            <td style="width: 35%;">
                                <asp:TextBox ID="Txt_Fecha_Peticion" runat="server" Width="90%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server" id="Tr_Fecha_Solucion" visible="false">
                            <td style="width: 15%;">
                                Fecha de Solución
                            </td>
                            <td style="width: 35%;">
                                <asp:TextBox ID="Txt_Fecha_Solucion" runat="server" Width="90%" Enabled="false"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr runat="server" id="Tr_Txt_Solucion" visible="false">
                            <td style="width: 15%;" valign="top">
                                Solución
                            </td>
                            <td style="width: 85%;" colspan="3">
                                <asp:TextBox ID="Txt_Solucion" runat="server" TextMode="MultiLine" Width="97%" Enabled="false"
                                    Height="65"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <hr class="linea" />
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
                                        EmptyDataText="No se encontraron registros" Style="font-size: x-small; white-space: normal">
                                        <Columns>
                                            <asp:TemplateField HeaderText="No">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="FECHA_ASIGNACION" HeaderText="Fecha" Visible="True" DataFormatString="{0:dd/MMM/yyyy hh:mm tt}">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Font-Size="Small" Width="140px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DEPENDENCIA" HeaderText="Dependencia" Visible="True">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OBSERVACIONES" HeaderText="Observaciones" Visible="True">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="10%" Wrap="true" Font-Size="Small" />
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
                                        Style="font-size: x-small; white-space: normal">
                                        <Columns>
                                            <asp:TemplateField HeaderText="No">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="FECHA" HeaderText="Fecha" Visible="True" DataFormatString="{0:dd/MMM/yyyy hh:mm tt}">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Left" Font-Size="Small" Width="140px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OBSERVACION" HeaderText="Observación" Visible="True">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Font-Size="Small" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="True">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" Width="12%" Wrap="true" Font-Size="Small" />
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
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

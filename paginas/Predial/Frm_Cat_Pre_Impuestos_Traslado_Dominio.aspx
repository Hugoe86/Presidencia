<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Impuestos_Traslado_Dominio.aspx.cs"
    Inherits="paginas_predial_Frm_Cat_Pre_Impuestos_Traslado_Dominio" Title="Catalogo de Impuestos de Traslado de Dominio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 838px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScptM_Impuestos_Traslado_Dominio" runat="server" />
    <asp:UpdatePanel ID="Upd_Impuestos_Traslado_Dominio" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Impuestos_Traslado_Dominio"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Impuestos_Traslado_Dominio" style="background-color: #ffffff; width: 100%;
                height: 100%;">
                <table border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">
                            Cat&aacute;logo de Impuestos de Traslado de Dominio
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:ImageButton ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
                                                Width="24px" Height="24px" />
                                            <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%;">
                                        </td>
                                        <td style="width: 90%; text-align: left;" valign="top">
                                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" class="style1">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Nuevo" OnClick="Btn_Nuevo_Click"
                                TabIndex="1" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Modificar" OnClick="Btn_Modificar_Click"
                                TabIndex="2" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Eliminar" OnClientClick="return confirm('¿Desea eliminar el presente registro?');"
                                OnClick="Btn_Eliminar_Click" TabIndex="3" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click"
                                TabIndex="4" />
                        </td>
                        <td align="right" class="style1">
                            Búsqueda:
                            <asp:TextBox ID="Txt_Busqueda_Impuestos_Traslado_Dominio" runat="server" Width="130px"
                                TabIndex="5"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Impuestos" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                OnClick="Btn_Buscar_Click" TabIndex="6" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Impuestos_Traslado_Dominio" runat="server"
                                WatermarkText="<Buscar por Año>" TargetControlID="Txt_Busqueda_Impuestos_Traslado_Dominio"
                                WatermarkCssClass="watermarked">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Impuestos_Traslado_Dominio" runat="server"
                                TargetControlID="Txt_Busqueda_Impuestos_Traslado_Dominio" InvalidChars="<,>,&,',!,"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
                <br />
                <center>
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td style="text-align: left; width: 10%;">
                                <asp:TextBox ID="Txt_Tasa_ID" runat="server" Width="40%" Visible="false" />
                            </td>
                            <tr>
                                <td style="text-align: left; width: 20%; text-align: left;">
                                    *Año
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Anio" runat="server" Width="96.4%" MaxLength="4" TabIndex="7" />
                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_Anio" runat="server" TargetControlID="Txt_Anio"
                                        FilterType="Custom, Numbers" ValidChars=",." />
                                </td>
                                <td style="text-align: left; width: 10%; text-align: left;">
                                    *Estatus
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%" TabIndex="8">
                                        <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE"></asp:ListItem>
                                        <asp:ListItem Text="VIGENTE" Value="VIGENTE"></asp:ListItem>
                                        <asp:ListItem Text="BAJA" Value="BAJA"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%; text-align: left;">
                                    *Valor fiscal inicial
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Valor_Inicial" runat="server" Width="96.4%" MaxLength="13" TabIndex="9" AutoPostBack="true" OnTextChanged="Txt_Valor_Inicial_TextChanged"/>
                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_Valor_Inicial" runat="server" TargetControlID="Txt_Valor_Inicial"
                                        FilterType="Custom, Numbers" ValidChars=",." />
                                </td>
                                <td style="text-align: left; width: 20%; text-align: left;">
                                    *Valor fiscal final
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Valor_Final" runat="server" Width="96.4%" MaxLength="13" TabIndex="10"  AutoPostBack="true" OnTextChanged="Txt_Valor_Final_TextChanged"/>
                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_Valor_Final" runat="server" TargetControlID="Txt_Valor_Final"
                                        FilterType="Custom, Numbers" ValidChars=",." />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%; text-align: left;">
                                    *Tasa
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Tasa" runat="server" Width="96.4%" MaxLength="13" TabIndex="11"  AutoPostBack="true" OnTextChanged="Txt_Tasa_TextChanged"/>
                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_Tasa" runat="server" TargetControlID="Txt_Tasa"
                                        FilterType="Custom, Numbers" ValidChars=",." />
                                </td>
                                <td style="text-align: left; width: 20%; text-align: left;">
                                    *Deducible (Fracción) 1
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Deducible_Uno" runat="server" Width="96.4%" MaxLength="13" TabIndex="12" AutoPostBack="true" OnTextChanged="Txt_Deducible_Uno_TextChanged"/>
                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_Deducible_Uno" runat="server" TargetControlID="Txt_Deducible_Uno"
                                        FilterType="Custom, Numbers" ValidChars=",." />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%; text-align: left;">
                                    *Deducible (Fracción) 2
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Deducible_Dos" runat="server" Width="96.4%" MaxLength="13" TabIndex="13" OnTextChanged="Txt_Deducible_Dos_TextChanged" AutoPostBack="true"/>
                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_Deducible_Dos" runat="server" TargetControlID="Txt_Deducible_Dos"
                                        FilterType="Custom, Numbers" ValidChars=",." />
                                </td>
                                <td style="text-align: left; width: 20%; text-align: left;">
                                    *Deducible (Fracción) 3
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Deducible_Tres" runat="server" Width="96.4%" MaxLength="13" TabIndex="14" OnTextChanged="Txt_Deducible_Tres_TextChanged" AutoPostBack="true"/>
                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_Deducible_Tres" runat="server" TargetControlID="Txt_Deducible_Tres"
                                        FilterType="Custom, Numbers" ValidChars=",." />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%; vertical-align: top;">
                                    Comentarios
                                </td>
                                <td style="text-align: left; width: 80%; vertical-align: top;" colspan="3">
                                    <asp:TextBox ID="Txt_Comentarios" runat="server" TabIndex="15" MaxLength="250" TextMode="MultiLine"
                                        Width="98.6%" Style="text-transform: uppercase;" />
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios" runat="server" TargetControlID="Txt_Comentarios"
                                        WatermarkText="Límite de Caracteres 250" WatermarkCssClass="watermarked" />
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios" runat="server" TargetControlID="Txt_Comentarios"
                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <tr align="center">
                                        <td colspan="4">
                                            <asp:GridView ID="Grid_Impuestos" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="5" Style="white-space: normal;" Width="100%"
                                                OnSelectedIndexChanged="Grid_Impuestos_SelectedIndexChanged" OnPageIndexChanging="Grid_Impuestos_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="TASA_ID" HeaderStyle-Width="15%" HeaderText="Tasa ID">
                                                        <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Right" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ANIO" HeaderStyle-Width="5%" HeaderText="Año">
                                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="VALOR_INICIAL" HeaderStyle-Width="15%" HeaderText="Valor fiscal inicial"
                                                        DataFormatString="{0:c2}">
                                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="VALOR_FINAL" HeaderStyle-Width="15%" HeaderText="Valor fiscal final"
                                                        DataFormatString="{0:c2}">
                                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TASA" HeaderText="Tasa">
                                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DEDUCIBLE_UNO" HeaderStyle-Width="15%" HeaderText="Deducible (Fracción) 1"
                                                        DataFormatString="{0:c2}">
                                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DEDUCIBLE_DOS" HeaderText="Deducible (Fracción) 2" DataFormatString="{0:c2}">
                                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DEDUCIBLE_TRES" HeaderText="Deducible (Fracción) 3" DataFormatString="{0:c2}">
                                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ESTATUS" HeaderStyle-Width="15%" HeaderText="Estatus">
                                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="COMENTARIOS" HeaderText="Comentarios">
                                                        <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            <br />
                                        </td>
                                    </tr>
                                </td>
                            </tr>
                    </table>
                </center>
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Ate_Reinicio_Folios.aspx.cs" Inherits="paginas_Operacion_Atencion_Ciudadana_Frm_Ope_Ate_Reinicio_Folios"
    Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div style="width: 95%;">
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table width="100%" cellspacing="0">
                    <tr>
                        <td colspan="4" align="center" class="label_titulo">
                            Reiniciar folios
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Image ID="Img_Informacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />
                            <asp:Label ID="Lbl_Mensaje" runat="server" Text="" Visible="false" CssClass="estilo_fuente_mensaje_error">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td colspan="2" align="left" valign="middle">
                            <div>
                                &nbsp;
                                <asp:ImageButton ID="Btn_Reiniciar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                    CssClass="Img_Button" OnClick="Btn_Reiniciar_Click" ToolTip="Reiniciar folios"
                                    CausesValidation="False" />
                                <asp:ImageButton ID="Btn_Salir" runat="server" OnClick="Btn_Salir_Click" CssClass="Img_Button"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" />
                            </div>
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
                        <td colspan="4">
                            <hr class="linea" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%;">
                            *Prefijo
                        </td>
                        <td style="width: 35%;">
                            <asp:TextBox ID="Txt_Prefijo" runat="server" Width="90%" MaxLength="6" ToolTip="texto que se antepondrá a los folios anteriores"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Prefijo" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                TargetControlID="Txt_Prefijo" ValidChars="-">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="width: 15%; text-align: right;">
                            *Programa
                        </td>
                        <td style="width: 35%;">
                            <asp:DropDownList ID="Cmb_Origen" runat="server" Width="92%" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <br />
</asp:Content>

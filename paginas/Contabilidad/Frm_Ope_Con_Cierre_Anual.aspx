<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Con_Cierre_Anual.aspx.cs" Inherits="paginas_Contabilidad_Frm_Ope_Con_Cierre_Anual" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:UpdateProgress ID="Uprg_Reporte" runat="server" 
        AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
        <ProgressTemplate>
            <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
            <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:ScriptManager ID="ScriptManager_Cierre_Anual" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <div id="Div_Cierre_Anual" >
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Cierre Anual</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>               
                    <tr class="barra_busqueda" align="right">
                        <td align="left">
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" 
                                TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                        </td>
                        <td style="width:50%">
                        </td> 
                    </tr>
                </table>
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td width="15%"></td>
                        <td width="20%"></td>
                        <td width="10%"></td>
                        <td width="10%"></td>
                        <td width="45%"></td>
                    </tr>
                    <tr>
                        <td>*Cuenta Inicial</td>
                        <td colspan="3">
                            <asp:DropDownList ID="Cmb_Cuenta_Inicial" runat="server" Width="99%" 
                                AutoPostBack="True" onselectedindexchanged="Cmb_Cuenta_Inicial_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>*Cuenta Final</td>
                        <td colspan="3">
                            <asp:DropDownList ID="Cmb_Cuenta_Final" runat="server" Width="99%" AutoPostBack="True"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>*Nueva Cuenta</td>
                        <td>
                            <asp:TextBox ID="Txt_Nueva_Cuenta" runat="server" Width="98%" 
                                AutoPostBack="True" ontextchanged="Txt_Nueva_Cuenta_TextChanged"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nueva_Cuenta" runat="server" TargetControlID="Txt_Nueva_Cuenta"
                                FilterType="Custom" ValidChars="1234567890">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="text-align: center">*Año</td>
                        <td>
                            <asp:TextBox ID="Txt_Anio" runat="server" Width="98%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Anio" runat="server" TargetControlID="Txt_Anio"
                                FilterType="Custom" ValidChars="1234567890">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>Descripcion</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Descripcion" runat="server" Width="99%" 
                                AutoPostBack="True" MaxLength="400" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr><td></td></tr>
                    <tr>
                        <td colspan="4" style="text-align: center">
                            <asp:Button ID="Btn_Cierre_Anual" runat="server" Text="Generar Cierre" 
                                onclick="Btn_Cierre_Anual_Click"/>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
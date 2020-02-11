<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Com_Parametros.aspx.cs" Inherits="paginas_Compras_Frm_Cat_Com_Parametros" Title="Catálogo de parámetros" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 13px;
        }
    </style>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div style="width: 95%;">
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
            <ContentTemplate>
              <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressTemplate">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />                           
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Cat&aacute;logo de Parametros</td>
                    </tr>
                    <tr align="left">
                        <td colspan="2" >
                            <asp:Image ID="Img_Warning" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />
                            <asp:Label ID="Lbl_Informacion" runat="server" 
                                ForeColor="#990000"></asp:Label>                        
                        </td>
                    </tr>                    
                    <tr class="barra_busqueda" align="right">
                        <td colspan="2" align="left" valign="middle">
                           <table width = "100%">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                        CssClass="Img_Button" ToolTip="Modificar" onclick="Btn_Modificar_Click" />
                                    <asp:ImageButton ID="Btn_Salir" runat="server" 
                                        CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            ToolTip="Inicio" onclick="Btn_Salir_Click" />
                                </td>
                            </tr>
                           </table>                           
                        </td>                        
                    </tr>
                    <tr>
                        <td align="left" style="width:30%">*Salario Mínimo Resguardado</td>
                        <td align="left">
                            <asp:TextBox ID="Txt_Salario_Minimo_Resguardado" runat="server" Width="150px"></asp:TextBox>
                          <%-- <cc1:FilteredTextBoxExtender ID="Txt_Salario_Minimo_Resguardado_FilteredTextBoxExtender" 
                                runat="server" TargetControlID="Txt_Salario_Minimo_Resguardado" 
                                FilterType="Numbers" 
                                ValidChars="."></cc1:FilteredTextBoxExtender>--%>
                                <cc1:MaskedEditExtender ID="MEE_Txt_Salario_Minimo_Resguardado" runat="server" 
                                 TargetControlID="Txt_Salario_Minimo_Resguardado" Mask="9999999.99" MaskType="Number" 
                                 InputDirection="RightToLeft" AcceptNegative="Left" ErrorTooltipEnabled="True" 
                                 CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                 CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                 CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" AcceptAMPM="True" ClearTextOnInvalid="True" UserDateFormat="None" />
                        </td>
                    </tr>
                    <tr>
                        <td>*Plazo Surtir Orden Compras</td>
                        <td align="left">
                            <asp:TextBox ID="Txt_Plazo_Surtir_Orden_Compra" runat="server" Width="150px" MaxLength="1000"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Txt_Plazo_Surtir_Orden_Compra_FilteredTextBoxExtender"
                                runat="server" FilterType="Numbers" 
                                TargetControlID="Txt_Plazo_Surtir_Orden_Compra" ValidChars="0123456789 ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="2">
                            &nbsp;</td>
                    </tr>
                </table> 
                <table style="width: 98%;">
                        <tr>
                            <td colspan="3" >
                              <hr class="linea" />
                                <br />
                            </td>   
                            <tr>
                            <td align="left" style="width:3%" >Partida Generica Listados Almacen</td>
                            <td align="right" class="style1">
                            <%--<tr>--%>
                                <asp:DropDownList ID="Cmb_Partida_Generica_Listados" runat="server" 
                                    AutoPostBack="true" Enabled="TRUE"
                                    onselectedindexchanged="Cmb_Partida_Generica_Listados_SelectedIndexChanged" 
                                    Width="400px" />
                                <td align="left"colspan="2">
                                    &nbsp;</td>
                                </td>
                        <%--</tr>--%>
                            <tr>
                                <td align="left" style="width:35%">Partida Especifica Listados Almacen</td>
                                <td align="left" class="style1">
                                <%--<tr>--%>
                                   <%-- <td align="left" colspan="2">--%>
                                        <asp:DropDownList ID="Cmb_Partida_Especifica_Listados" runat="server" AutoPostBack="true" Enabled="TRUE" Width="400px" />
                                    </td>
                                <%--</tr>--%>
                        </tr>
                        
                            </tr>
                            
                        </tr>
                        </table>            
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Balanza_Mensual.aspx.cs" Inherits="paginas_Contabilidad_Frm_Rpt_Balanza_Mensual" Title="Reporte Balanza Mensual" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Areas" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>        
            <asp:UpdateProgress ID="Uprg_Reloj_Checador" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Reporte_Balance_Mensual" style="background-color:#ffffff; width:100%; height:100%;">    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Reporte Balance Mensual</td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="98%"  border="0" cellspacing="0">
                    <tr align="center">
                        <td colspan="2">                
                            <div align="right" class="barra_busqueda">                        
                                <table style="width:100%;height:28px;">
                                    <tr>
                                        <td align="left" style="width:59%;">
                                            <asp:ImageButton ID="Btn_Reporte_Balance" runat="server" ToolTip="Nuevo" 
                                                CssClass="Img_Button" TabIndex="1"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" 
                                                onclick="Btn_Reporte_Balance_Click"/>
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                                CssClass="Img_Button" TabIndex="2"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                                onclick="Btn_Salir_Click"/>
                                        </td>
                                      <td align="right" style="width:41%;">&nbsp;</td>       
                                    </tr>         
                                </table>                      
                            </div>
                        </td>
                    </tr>
                </table>   
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="width:20%;text-align:left;">*Tipo de Balance</td>
                        <td style="width:30%;text-align:left;">
                            <asp:DropDownList ID="Cmb_Tipo_Balance" runat="server" Width="80%" TabIndex="3">
                                <asp:ListItem>&lt; -- Seleccione -- &gt;</asp:ListItem>
                                <asp:ListItem>MES NORMAL</asp:ListItem>
                                <asp:ListItem>MES 13</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%;text-align:left;">*Mes</td>
                        <td style="width:30%;text-align:left;">
                            <asp:DropDownList ID="Cmb_Meses" runat="server" Width="80%" TabIndex="4">
                                <asp:ListItem>&lt; -- Seleccione -- &gt;</asp:ListItem>
                                    <asp:ListItem>01 ENERO</asp:ListItem>
                                    <asp:ListItem>02 FEBRERO</asp:ListItem>
                                    <asp:ListItem>03 MARZO</asp:ListItem>
                                    <asp:ListItem>04 ABRIL</asp:ListItem>
                                    <asp:ListItem>05 MAYO</asp:ListItem>
                                    <asp:ListItem>06 JUNIO</asp:ListItem>
                                    <asp:ListItem>07 JULIO</asp:ListItem>
                                    <asp:ListItem>08 AGOSTO</asp:ListItem>
                                    <asp:ListItem>09 SEPTIEMBRE</asp:ListItem>
                                    <asp:ListItem>10 OCTUBRE</asp:ListItem>
                                    <asp:ListItem>11 NOVIEMBRE</asp:ListItem>
                                    <asp:ListItem>12 DICIEMBRE</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:50%;text-align:left;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width:20%;text-align:left;">*Año</td>
                        <td style="width:30%;text-align:left;">
                            <asp:DropDownList ID="Cmb_Anio" runat="server" Width="80%" TabIndex="5">
                                <asp:ListItem>&lt; -- Seleccione -- &gt;</asp:ListItem>
                                <asp:ListItem>2011</asp:ListItem>
                                <asp:ListItem>2012</asp:ListItem>
                                <asp:ListItem>2013</asp:ListItem>
                                <asp:ListItem>2014</asp:ListItem>
                                <asp:ListItem>2015</asp:ListItem>
                                <asp:ListItem>2016</asp:ListItem>
                                <asp:ListItem>2017</asp:ListItem>
                                <asp:ListItem>2018</asp:ListItem>
                                <asp:ListItem>2019</asp:ListItem>
                                <asp:ListItem>2020</asp:ListItem>
                                <asp:ListItem>2021</asp:ListItem>
                                <asp:ListItem>2022</asp:ListItem>
                                <asp:ListItem>2023</asp:ListItem>
                                <asp:ListItem>2024</asp:ListItem>
                                <asp:ListItem>2025</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:50%;text-align:left;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width:20%;text-align:left;">Cuenta Inicial</td>
                        <td style="width:30%;text-align:left;">
                            <asp:TextBox ID="Txt_Cuenta_Inicial" runat="server" Width="80%" MaxLength="20" 
                                TabIndex="6" ontextchanged="Txt_Cuenta_Inicial_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cuenta_Inicial" runat="server" TargetControlID="Txt_Cuenta_Inicial" 
                                FilterType="Custom, Numbers" ValidChars="-"/>                              
                        </td>
                        <td style="width:50%;text-align:left;">
                            <asp:DropDownList ID="Cmb_Cuenta_Inicial" runat="server" Width="90%"  AutoPostBack ="true" TabIndex="8" 
                                onselectedindexchanged="Cmb_Cuenta_Inicial_SelectedIndexChanged"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%;text-align:left;">Cuenta Final</td>
                        <td style="width:30%;text-align:left;">
                            <asp:TextBox ID="Txt_Cuenta_Final" runat="server" Width="80%" MaxLength="20" 
                                TabIndex="9" ontextchanged="Txt_Cuenta_Final_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cuenta_Final" runat="server" TargetControlID="Txt_Cuenta_Final" 
                                FilterType="Custom, Numbers" ValidChars="-"/>
                        </td>
                        <td style="width:50%;text-align:left;">
                            <asp:DropDownList ID="Cmb_Cuenta_Contable_Final" runat="server" Width="90%"  AutoPostBack ="true" TabIndex="11" 
                                onselectedindexchanged="Cmb_Cuenta_Contable_Final_SelectedIndexChanged"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="width:100%;text-align:center;">
                            <asp:CheckBox ID="Chk_Movimientos_Saldo_No_Cero" Text="Monvimientos con Saldo no Ceros" runat="server" TabIndex="12"/>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


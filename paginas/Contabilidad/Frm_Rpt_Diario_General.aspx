<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Diario_General.aspx.cs" Inherits="paginas_Contabilidad_Frm_Rpt_Diario_General" Title="Reporte Diario General" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Areas" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>        
            <asp:UpdateProgress ID="Uprg_Diario_General" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Reporte_Diario_General" style="background-color:#ffffff; width:100%; height:100%;">    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Reporte Libro Diario</td>
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
                                                onclick="Btn_Reporte_Balance_Click" />
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
                        <td style="width:20%;text-align:left;">*Libro Diario</td>
                        <td style="width:30%;text-align:left;">
                            <asp:DropDownList ID="Cmb_Libro_Diario" runat ="server" Width="200px" TabIndex="5">
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                <asp:ListItem>GENERAL</asp:ListItem>
                                <asp:ListItem>CONAC</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:50%;text-align:left;"></td>
                    </tr>
                    <tr>
                        <td style="width:20%;text-align:left;">*Fecha Inicio</td>
                        <td style="width:30%;text-align:left;">
                            <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" MaxLength="100" Width="180px" Enabled="false"/>
                            <cc1:CalendarExtender ID="DTP_Fecha_Inicio" runat="server" 
                                TargetControlID="Txt_Fecha_Inicio" Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Fecha_Inicio"/>
                             <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server" TabIndex="6" 
                                ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                Height="18px" CausesValidation="false" Enabled="true"/>
                        </td>
                        <td style="width:50%;text-align:left;"></td>
                    </tr>                    
                    <tr>
                        <td style="width:20%;text-align:left;">*Fecha Final</td>
                        <td style="width:30%;text-align:left;">
                            <asp:TextBox ID="Txt_Fecha_Final" runat="server" MaxLength="100" Width="180px" Enabled="false"/>
                            <cc1:CalendarExtender ID="DTP_Fecha_Final" runat="server" 
                                TargetControlID="Txt_Fecha_Final" Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Fecha_Final"/>
                             <asp:ImageButton ID="Btn_Fecha_Final" runat="server" TabIndex="7"
                                ImageUrl="../imagenes/paginas/SmallCalendar.gif" style="vertical-align:top;"
                                Height="18px" CausesValidation="false" Enabled="true"/>
                        </td>
                        <td style="width:50%;text-align:left;"></td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


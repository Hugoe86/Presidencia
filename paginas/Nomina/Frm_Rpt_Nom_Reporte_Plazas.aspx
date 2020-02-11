<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Reporte_Plazas.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Reporte_Plazas" Title="Reporte de Plazas"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Polizas" runat="server" />
                <div id="Div_Reporte_Balance_Mensual" style="background-color:#ffffff; width:100%; height:100%;">    
                    <table width="100%" class="estilo_fuente">
                        <tr align="center">
                            <td class="label_titulo">"Reporte de Plazas</td>
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
                            <td>                
                                <div align="right" class="barra_busqueda">                        
                                    <table style="width:100%;height:28px;">
                                        <tr>
                                            <td >
                                            <asp:UpdatePanel ID="Upd_Panel" runat="server">
                                                <ContentTemplate>
                                                <asp:UpdateProgress ID="Uprg_Polizas" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                                                    <ProgressTemplate>
                                                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                                                        <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                                <asp:ImageButton ID="Btn_Reporte" runat="server" ToolTip="Generar Reporte" 
                                                    CssClass="Img_Button" TabIndex="1"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" 
                                                    onclick="Btn_Reporte_Click"/>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                               </td>
                                               <td align="left" style="width:50%;">
                                                    <asp:ImageButton ID="Btn_Imprimir_Excel" runat="server" 
                                                     ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png"
                                                      CssClass="Img_Button"  AlternateText="Imprimir Excel" 
                                                     ToolTip="Exportar Excel"
                                                     OnClick="Btn_Excel_Click" Visible="true"/> 
                                                <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                                    CssClass="Img_Button" TabIndex="2"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                                    onclick="Btn_Salir_Click"/>
                                            </td>
                                            <td align="right" style="width:50%;">&nbsp;</td>       
                                        </tr>         
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table width="98%" class="estilo_fuente"> 
                        <tr >
                              <td>
                                &nbsp;&nbsp;
                                <asp:Label ID="Lbl_Unidad_Responsable" runat="server" 
                                    Text="*Unidad Responsable"></asp:Label>
                              </td>
                              <td colspan="4">
                                    <asp:DropDownList ID="Cmb_Unidad_Responsable_Busqueda" runat="server" 
                                        AutoPostBack="true" 
                                        OnSelectedIndexChanged="Cmb_Unidad_Responsable_Busqueda_SelectedIndexChanged" 
                                        Width="97%">
                                    </asp:DropDownList>
                              </td>
                        </tr>
                        <tr >
                            <td style=" width:20%">&nbsp;&nbsp;
                                        <asp:Label ID="Lbl_Puesto" runat="server" Text="Puesto"></asp:Label>
                                    </td>
                                    <td style=" width:30%">
                                        <asp:DropDownList ID="Cmb_Puesto" runat="server" Width="96%">
                                        </asp:DropDownList>
                                    </td>
                                    <td style=" width:20%">
                                        <asp:Label ID="Lbl_Tipo_Nomina" runat="server" Text="Tipo Nomina"/>
                                    </td>                                    
                                    <td  style=" width:30%">
                                        <asp:DropDownList ID="Cmb_Tipo_Nomina" runat="server"  Width="95%">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                        </tr>
                    </table>                       
               </div>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Plazas.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Plazas" Title="Reporte de Plazas" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="Sm_Rpt_Nom_Plazas" runat="server"/>
<asp:UpdatePanel ID="Upnl_Rpt_Nom_Plazas" runat="server">
    <ContentTemplate>
    
        <asp:UpdateProgress ID="Uprs_Rpt_Nom_Plazas" runat="server" DisplayAfter="0" DynamicLayout="true">
            <ProgressTemplate>
                <div id="Div_Fondo" class="progressBackgroundFilter"></div>
                <div id="Div_Imagen" style="background-color:Transparent; position:fixed; z-index:1001; top:30%; left:43%;">
                    <img src="../imagenes/paginas/Updating.gif"  alt=""/>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    
        <div style="width:98%;background-color:White;">
        
        
            <table style="width:99%;">
                <tr>
                    <td style="width:100%;" align="center">
                        <div id="Contenedor_Titulo" style="color:White;font-size:12;font-weight:bold;border-style:outset;background:url(../imagenes/paginas/titleBackground.png) repeat-x top;background-color:Silver;">
                            <table width="100%">
                                <tr>
                                    <td></td>
                                </tr>            
                                <tr>
                                    <td width="100%">
                                        <font style="color: Black; font-weight: bold;">Reporte de Puestos por Unidad Responsable</font>
                                    </td>    
                                </tr>  
                                <tr>
                                    <td></td>
                                </tr>                                      
                            </table>    
                        </div>
                    </td>
                </tr>
            </table>       
             
            <table width="98%">           
                <tr>
                    <td>
                        <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false"  />&nbsp;
                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                    </td>
                </tr>
            </table>
        
            <table width="99%">
                <tr>
                    <td colspan="4" style="width:100%;">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:left; width:20%;">
                        Estatus
                    </td>
                    <td style="text-align:left; width:30%;">
                        <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                            <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                            <asp:ListItem>DISPONIBLE</asp:ListItem>
                            <asp:ListItem>OCUPADO</asp:ListItem>
                        </asp:DropDownList>                    
                    </td>
                    <td style="text-align:left; width:20%;">
                    </td>
                    <td style="text-align:left; width:30%;">

                    </td>
                </tr>
                <tr>
                    <td style="text-align:left; width:20%;">
                        Unidad Responsable
                    </td>
                    <td style="text-align:left; width:80%;" colspan="3">
                        <asp:DropDownList ID="Cmb_Unidades_Responsables" runat="server" Width="99%"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="width:100%;">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:left; width:100%;" colspan="4">
                        <table width="98%">
                            <tr>
                                <td style="text-align:right;">
                                    <asp:ImageButton ID="Btn_Generar_Reporte" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" ToolTip="Reporte de Plazas en PDF"
                                        CssClass="button_autorizar" Width="32px" Height="32px"  style="cursor:hand;display:inline;" OnClick="Btn_Generar_Reporte_Click" />
                                    <asp:ImageButton ID="Btn_Excel" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" ToolTip="Reporte de Plazas en Excel"
                                        CssClass="button_autorizar" OnClick="Btn_Excel_Click"  Width="32px" Height="32px" style="cursor:hand;display:inline;" />                                        
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="width:100%;">
                        <hr />
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Btn_Excel"/>
        </Triggers>
</asp:UpdatePanel>
</asp:Content>


<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"  CodeFile="Frm_Rpt_Nom_Relacion_Recibos_Nomina.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Relacion_Recibos_Nomina" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
        <ContentTemplate>
        <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
        </asp:UpdateProgress>
        <%--Div de Contenido --%>
        <div id="Div_Contenido" style="width:97%;height:100%;">
        <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
            <tr>
                <td colspan ="4" class="label_titulo">Relación de Recibos de Nomina</td>
            </tr>
            <%--Fila de div de Mensaje de Error --%>
            <tr>
                <td colspan ="4">
                    <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                    <table style="width:100%;">
                        <tr>
                            <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                            <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                            Width="24px" Height="24px"/>
                            </td>            
                            <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />
                            </td>
                        </tr> 
                    </table>                   
                    </div>
                </td>
            </tr>
            <%--Fila de Busqueda y Botones Generales --%>
            <tr class="barra_busqueda">
                    <td style="width:20%;" colspan="4">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ImageButton ID="Btn_PDF" runat="server" CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" 
                            ToolTip="Exportar a PDF" onclick="Btn_PDF_Click" />
                           
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
            </tr>
            <tr>
                <td colspan ="4">
                    <table width="97%">
                        <tr>
                            <td width="15%">
                                Unidad Responsable
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="Cmb_Unidad_Responsable" runat="server" Width="80%" AutoPostBack="true"
                                    onselectedindexchanged="Cmb_Unidad_Responsable_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            
                            
                        </tr>
                        <tr>
                            <td>
                                Nomina
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Nomina" runat="server" Width="90%" AutoPostBack="true"
                                    onselectedindexchanged="Cmb_Nomina_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            
                            
                            <td width="15%">
                                Periodo
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Periodo" runat="server" Width="90%" AutoPostBack="true"
                                    onselectedindexchanged="Cmb_Periodo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            
                            
                        </tr>
                        
                        <tr>
                            <td>
                                Inicia Catorcena</td>
                            <td>
                                <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="80%" Enabled="false"></asp:TextBox>
                                
                            </td>
                            <td>
                                Fin Catorcena
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="80%" Enabled="false"></asp:TextBox>
                            </td>
                            
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </div>
        
        
        </ContentTemplate>
        </asp:UpdatePanel>
        </asp:Content>
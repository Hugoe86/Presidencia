<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado" %>

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
                <%--<div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>--%>
                </ProgressTemplate>
        </asp:UpdateProgress>
        <%--Div de Contenido --%>
        <div id="Div_Contenido" style="width:97%;height:100%;">
        <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
            <tr>
                <td class="label_titulo">Reporte de Vacaciones Acumulado por Empleado</td>
            </tr>
            <%--Fila de div de Mensaje de Error --%>
            <tr>
                <td>
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
                    <td style="width:20%;">
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
                <td>
                    <table width="97%">
                        <tr>
                            <td width="20%">
                                No. Empleado
                            </td>
                            <td width="30%">
                                <asp:DropDownList ID="Cmb_Empleado" runat="server" Width="100%">
                                </asp:DropDownList>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="Txt_Buscar_Empleado" runat="server"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkCssClass="watermarked"
                                                WatermarkText="<Num. Empleado>" TargetControlID="Txt_Buscar_Empleado" />
                                           <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" 
                                            TargetControlID="Txt_Buscar_Empleado"  
                                            FilterType="Custom" ValidChars="0,1,2,3,4,5,6,7,8,9"
                                            Enabled="True" InvalidChars="<,>,&,',!,">   
                                            </cc1:FilteredTextBoxExtender>
                                
                                <asp:ImageButton ID="Btn_Buscar_Empleado" runat="server" CssClass="Img_Button" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                ToolTip="Buscar Empleado" onclick="Btn_Buscar_Empleado_Click"  />
                           
                        
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Unidad Responsable
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="Cmb_Unidad_Responsable" runat="server" Width="100%">
                                </asp:DropDownList>
                            </td>
                            
                            <td>
                            
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Tipo de Nomina
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="Cmb_Tipo_Nomina" runat="server" Width="100%">
                                </asp:DropDownList>
                            </td>
                            
                            <td>
                            
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                Año
                            </td>
                            <td width="30%">
                                <asp:DropDownList ID="Cmb_Anio" runat="server" Width="100%">
                                </asp:DropDownList>
                            </td>
                            <td width="20%">
                                Periodo
                            </td>
                            <td width="30%">
                                <asp:DropDownList ID="Cmb_Periodo" runat="server" Width="100%">
                                    <asp:ListItem >--SELECCIONAR--</asp:ListItem>
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                </asp:DropDownList>
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
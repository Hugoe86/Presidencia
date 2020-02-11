<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Pat_Detalle_Bien.aspx.cs" Inherits="paginas_Compras_Frm_Rpt_Pat_Detalle_Bien" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Zonas" runat="server"  EnableScriptGlobalization ="true" EnableScriptLocalization = "True" />  
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="2">Reporte Detallado de Bien</td>
                    </tr>
                    <tr>
                        <td>
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top">
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left">&nbsp;</td>
                        <td>&nbsp;</td>                        
                    </tr>
                </table>      
                <br />
                <table width="98%">
                    <tr>
                        <td style="width:20%">
                            <asp:Label ID="Lbl_No_Inventario" runat="server" Text="No. Inventario"></asp:Label>
                        </td>
                        <td style="width:80%">
                            <asp:TextBox ID="Txt_No_Inventario" runat="server" Width="85%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Inventario" runat="server" TargetControlID="Txt_No_Inventario" FilterType="Numbers">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                        <tr>
                            <td style="text-align:center;" colspan="2">&nbsp;</td>
                        </tr>  
                        <tr>
                            <td style="text-align:center;" colspan="2">
                                <asp:ImageButton ID="Btn_Generar_Reporte_PDF" runat="server"  OnClick="Btn_Generar_Reporte_PDF_Click"
                                 ToolTip="Generar Reporte (Pdf)" 
                                    ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png"  />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="Btn_Generar_Reporte_Excel" runat="server" 
                                 ToolTip="Generar Reporte (Excel)" 
                                    ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                                    onclick="Btn_Generar_Reporte_Excel_Click"  />&nbsp;
                            </td>
                        </tr>  
                </table>  
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


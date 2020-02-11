<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Con_Reporte_Polizas.aspx.cs" Inherits="paginas_contabilidad_Frm_Ope_Con_Reporte_Polizas" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Rpt_Tipos_Polizas" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
       <ContentTemplate>
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>        
        <div style="width:98%; background-color:White;">
            <table width="100%" title="Control_Errores"> 
                <tr align="center">
                    <td class="label_titulo">Reporte de Tipos de Pólizas</td>               
                </tr>            
                <tr>
                    <td style="width:100%; text-align:left; cursor:default;">
                        <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />
                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>        
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
                                            <asp:ImageButton ID="Btn_Generar_Reporte" runat="server" ToolTip="Reporte" 
                                                CssClass="Img_Button" TabIndex="1" onclick="Btn_Generar_Reporte_Click"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                                CssClass="Img_Button" TabIndex="2"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                                onclick="Btn_Salir_Click" />
                                        </td>
                                      <td align="right" style="width:41%;">&nbsp;</td>       
                                    </tr>         
                                </table>                      
                            </div>
                        </td>
                    </tr>
                </table>   
            <asp:Panel ID="Pnl_Tipos_Polizas" runat="server" Width="98%" GroupingText="Filtrar por Tipos de Póliza" BackColor="White">
                <table width="100%">
                    <tr>
                        <td style="width:20%;text-align:left;">
                            <asp:CheckBoxList ID="Chk_Tipos_Poliza" runat="server" RepeatColumns="8" 
                                RepeatDirection="Horizontal">
                            </asp:CheckBoxList>
                        </td>
                    </tr>                               
                </table>
            </asp:Panel>
            <table width="98%">              
                <tr>
                    <td style="width:20%; text-align:left; cursor:default;">Números de Póliza del</td>
                    <td style="width:30%; text-align:left; cursor:default;">
                        <asp:TextBox ID="Txt_No_Poliza_Inicio" runat="server" Width="200px" TabIndex="0" />
                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Poliza_Inicio" runat="server" 
                            TargetControlID="Txt_No_Poliza_Inicio" FilterType="Numbers"/>                         
                    </td>        
                    <td style="width:20%; text-align:left; cursor:default;">&nbsp;&nbsp;Al</td>
                    <td style="width:30%; text-align:left; cursor:default;">
                        <asp:TextBox ID="Txt_No_Poliza_Termino" runat="server" Width="200px" TabIndex="0" />
                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Poliza_Termino" runat="server" 
                            TargetControlID="Txt_No_Poliza_Termino" FilterType="Numbers"/>                         
                    </td>                
                </tr>  
                <tr>
                    <td style="width:20%;text-align:left; cursor:default;">Fecha Inicio</td>
                    <td style="width:30%;text-align:left; cursor:default;">
                        <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="180px" TabIndex="14" Enabled="false"/>
                        <cc1:CalendarExtender ID="Cal_Txt_Fecha_Inicio" runat="server" 
                            TargetControlID="Txt_Fecha_Inicio" PopupButtonID="Btn_Fecha_Inicio" Format="dd/MMM/yyyy"/>
                        <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                            ToolTip="Seleccione la Fecha"/>                                                                              
                    </td>
                    <td style="width:20%;text-align:left; cursor:default;">&nbsp;&nbsp;Fecha Final</td>
                    <td style="width:30%;text-align:left; cursor:default;">
                        <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="180px" TabIndex="15" Enabled="false"/>                                                 
                        <cc1:CalendarExtender ID="Cal_Txt_Fecha_Final" runat="server" 
                            TargetControlID="Txt_Fecha_Final" PopupButtonID="Btn_Fecha_Final" Format="dd/MMM/yyyy" />
                        <asp:ImageButton ID="Btn_Fecha_Final" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                            ToolTip="Seleccione la Fecha"/>                       
                    </td>                                                            
                </tr>   
            </table>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


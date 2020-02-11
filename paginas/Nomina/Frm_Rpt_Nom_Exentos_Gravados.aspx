<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Exentos_Gravados.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Exentos_Gravados" Title="Reporte de Exentos y Gravados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="SM_Rpt_Exentos_Gravados" runat="server"  EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
<asp:UpdatePanel ID="UPnl_Rpt_Exentos_Gravados" runat="server" UpdateMode="Always">
    <ContentTemplate>
    
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Rpt_Exentos_Gravados" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>  
        
        <div style="width:98%; background-color:White;">

        <table width="100%" title="Control_Errores"> 
            <tr>
                <td class="button_autorizar" style="width:100%; text-align:center; cursor:default; font-size:14px;">
                    Reporte Exentos y Gravados
                </td>               
            </tr>            
            <tr>
                <td style="width:100%; text-align:left; cursor:default;">
                    <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />
                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>        
                </td>               
            </tr>
        </table>
        
        <asp:Panel ID="Pnl_Datos" runat="server" Width="100%" GroupingText="Filtrar por Calendario">
            <table width="100%">
                <tr>
                    <td class="button_autorizar" style="width:20%;text-align:left;">
                        Nomina
                    </td>
                    <td class="button_autorizar" style="width:30%;text-align:left;">
                        <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" AutoPostBack="true" 
                            TabIndex="5" onselectedindexchanged="Cmb_Calendario_Nomina_SelectedIndexChanged"/>
                    </td>             
                    <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                    </td>
                    <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                        
                    </td>                                                                                        
                </tr>     
                <tr>
                    <td class="button_autorizar" style="width:20%;text-align:left;">
                        Periodo A
                    </td>
                    <td class="button_autorizar" style="width:30%;text-align:left;">
                        <asp:DropDownList ID="Cmb_Periodo_A" runat="server" 
                            Width="100%" TabIndex="6" />
                    </td>             
                    <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                        &nbsp;&nbsp;Periodo B
                    </td>
                    <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                        <asp:DropDownList ID="Cmb_Periodo_B" runat="server" 
                            Width="100%" TabIndex="6" />
                    </td>                                                                                        
                </tr>                                              
            </table>
        </asp:Panel>
        
        <table width="100%">                                    
            <tr>
                <td class="button_autorizar" style="width:100%; text-align:left; cursor:default;" colspan="4">
                   <hr />
                </td>                
            </tr>                
            <tr>
                <td style="width:100%; text-align:left; cursor:default;" colspan="4">
                    <asp:Button ID="Btn_Generar_Reporte" runat="server" Text="Generar Reporte" 
                        Width="100%" CssClass="button_autorizar" OnClick="Btn_Generar_Reporte_Click"/>
                </td>                
            </tr>   
            <tr>
                <td style="width:100%; text-align:left; cursor:default;" colspan="4">
                    <hr />
                </td>                
            </tr>       
        </table>        
        </div>
    </ContentTemplate>
</asp:UpdatePanel>            
</asp:Content>


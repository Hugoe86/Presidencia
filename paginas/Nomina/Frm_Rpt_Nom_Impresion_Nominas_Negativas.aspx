<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Impresion_Nominas_Negativas.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Impresion_Nominas_Negativas" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

<asp:ScriptManager ID="Sm_Reporte_Nominas_Negativas" runat="server"/>
<asp:UpdatePanel ID="UPnl_Nominas_Negativas" runat="server">
    <ContentTemplate>
    
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Nominas_Negativas" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>  
         
        <div style="width:98%; background-color:White;">
        
            <table width="100%" title="Control_Errores"> 
                <tr>
                    <td class="button_autorizar" style="width:100%; text-align:center; cursor:default; font-size:12px;">
                        Reporte Nominas Negativas
                    </td>               
                </tr>            
                <tr>
                    <td class="button_autorizar"  style="width:100%; text-align:left; cursor:default;">
                        <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" 
                            style="display:none;" Width="24px" Height="24px"/>
                        <asp:Label id="Lbl_Mensaje_Error" runat="server" CssClass="estilo_fuente_mensaje_error" />
                    </td>               
                </tr>   
            </table>        
        
            <table width="100%">
                <tr>
                    <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                        Nomina
                    </td>
                    <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                        <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" OnSelectedIndexChanged="Cmb_Calendario_Nomina_SelectedIndexChanged" AutoPostBack="true"/>
                    </td>             
                    <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                        Periodo
                    </td>
                    <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                        <asp:DropDownList ID="Cmb_Periodo" runat="server"
                            Width="100%" TabIndex="6"/>
                    </td>                                                                                    
                </tr>             
                <tr>
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        Tipo N&oacute;mina
                    </td>
                    <td class="button_autorizar" style="width:80%; text-align:left; cursor:default;" colspan="3">
                        <asp:DropDownList ID="Cmb_Tipo_Nominas" runat="server" Width="100%"/>
                    </td>                                                          
                </tr>                 
            </table> 
            
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


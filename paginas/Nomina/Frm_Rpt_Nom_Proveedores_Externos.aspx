<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Proveedores_Externos.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Proveedores_Externos" Title="Untitled Page" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="Sc_Rpt_Totales_Tipo_Nomina_Banco" runat="server"/>
<asp:UpdatePanel ID="UPnl_Rpt_Totales_Tipo_Nomina_Banco" runat="server">
    <ContentTemplate>
    
        <asp:UpdateProgress ID="UPgs_Rpt_Totales_Tipo_Nomina_Banco" runat="server" 
            AssociatedUpdatePanelID="UPnl_Rpt_Totales_Tipo_Nomina_Banco" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress">
                    <img alt="" src="../Imagenes/paginas/Updating.gif" />
                </div>                
            </ProgressTemplate>
        </asp:UpdateProgress>
        
        <div style="width:98%; background-color:White;">
        
            <table width="100%" title="Control_Errores"> 
                <tr>
                    <td class="button_autorizar" style="width:100%; text-align:center; cursor:default; font-size:12px;">
                        Reporte Pago a Proveedores Externos
                    </td>               
                </tr>            
                <tr>
                    <td class="button_autorizar"  style="width:100%; text-align:left; cursor:default;">
                        <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false"  />&nbsp;
                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                    </td>               
                </tr>   
            </table>          
        
            <table width="100%">
                <tr>
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        No Empleado
                    </td>
                    <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                        <asp:TextBox ID="Txt_Busqueda_No_Empleado" runat="server" Width="98%"/>
                        <cc1:TextBoxWatermarkExtender ID="Twe_No_Empleado" runat="server" WatermarkCssClass="watermarked"
                            WatermarkText="Número Empleado" TargetControlID="Txt_Busqueda_No_Empleado" /> 
                        <cc1:FilteredTextBoxExtender ID="Fte_No_Empleado" runat="server" TargetControlID="Txt_Busqueda_No_Empleado" 
                            FilterType="Numbers"/>                                                   
                    </td>            
                    <td  style="width:20%; text-align:left; cursor:default;">
                    </td>
                    <td  style="width:30%; text-align:left; cursor:default;">                  
                    </td>                                                
                </tr>             
                <tr>
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        Nomina
                    </td>
                    <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                        <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" AutoPostBack="true" 
                            TabIndex="5" onselectedindexchanged="Cmb_Calendario_Nomina_SelectedIndexChanged"/>
                    </td>            
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        Periodo
                    </td>
                    <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;">
                        <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" 
                            Width="100%" TabIndex="6" />                    
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


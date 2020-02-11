<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Totales_Generales_Nomina.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Totales_Generales_Nomina" Title="Reportes de Totales Generales de Nómina" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">    
    
    <link href="../../easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <script src="../../jquery/jquery-1.5.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../javascript/Js_General.js" type="text/javascript"></script>
    <link href="../../easyui/themes/icon.css" rel="stylesheet" type="text/css" />

    <script src="../../easyui/jquery.progressbar.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.progressbar.min.js" type="text/javascript"></script>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

<asp:ScriptManager ID="Sm_Reporte_Totales_Generales_Nomina" runat="server"/>
<asp:UpdatePanel ID="UPnl_Totales_Generales_Nomina" runat="server">
    <ContentTemplate>
         
        <div style="width:98%; background-color:White;">
        
            <table width="100%" title="Control_Errores"> 
                <tr>
                    <td class="button_autorizar" style="width:100%; text-align:center; cursor:default; font-size:12px;">
                        Reporte Totales Generales N&oacute;mina
                    </td>               
                </tr>            
                <tr>
                    <td class="button_autorizar"  style="width:100%; text-align:left; cursor:default;">
                        <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" 
                            style="display:none;" Width="24px" Height="24px"/>
                        <label id="Lbl_Mensaje_Error" class="estilo_fuente_mensaje_error" style="display:none;"/>
                    </td>               
                </tr>   
            </table>        
        
            <table width="100%">
                <tr>
                    <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                        Nomina
                    </td>
                    <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                        <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" />
                    </td>             
                    <td class="button_autorizar" style="width:50%;text-align:left; cursor:default;" colspan="2">
                    
                    </td>                                                                                      
                </tr>             
                <tr>
                    <td class="button_autorizar" style="width:20%; text-align:left; cursor:default;">
                        Tipo N&oacute;mina
                    </td>
                    <td class="button_autorizar" style="width:30%; text-align:left; cursor:default;" colspan="3">
                        <asp:DropDownList ID="Cmb_Tipo_Nominas" runat="server" Width="100%"/>
                    </td>                                                          
                </tr>    
                <tr>
                    <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                        Periodo A
                    </td>
                    <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                        <asp:DropDownList ID="Cmb_Periodo_A" runat="server"
                            Width="100%" TabIndex="6" Enabled="false"/>
                    </td>             
                    <td class="button_autorizar" style="width:20%;text-align:left; cursor:default;">
                        &nbsp;&nbsp;Periodo B
                    </td>
                    <td class="button_autorizar" style="width:30%;text-align:left; cursor:default;">
                        <asp:DropDownList ID="Cmb_Periodo_B" runat="server" 
                            Width="100%" TabIndex="6" Enabled="false"/>
                    </td>                                                                                        
                </tr>               
            </table> 
            
            <center>
                <div id="progressBackgroundFilter"  class="progressBackgroundFilter" style="display:none;"></div>
                <div id="Barra_Progreso" class="processMessage" style="display:none;"></div>
            </center>
            
            <table width="100%">                                    
                <tr>
                    <td class="button_autorizar" style="width:100%; text-align:left; cursor:default;" colspan="4">
                       <hr />
                    </td>                
                </tr>                
                <tr>
                    <td style="width:100%; text-align:left; cursor:default;" colspan="4">
                        <asp:Button ID="Btn_Generar_Reporte" runat="server" Text="Generar Reporte" 
                            Width="100%" CssClass="button_autorizar" />
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


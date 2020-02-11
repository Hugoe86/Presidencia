<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Nom_Movimientos_Empleados.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Nom_Movimientos_Empleados" Title="Reporte de Movimientos de los Empleados" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">

    <link href="../../easyui/themes/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    
    <script src="../../jquery/jquery-1.5.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
    <script src="../../javascript/Js_Rpt_Nom_Mov_Empleados.js" type="text/javascript"></script>

    <script src="../../easyui/jquery.progressbar.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.progressbar.min.js" type="text/javascript"></script>
    
    <link href="../../easyui/css/redmond/jquery-ui-1.7.1.custom.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/css/ui.daterangepicker.css" rel="stylesheet" type="text/css" />
    <script src="../../easyui/jquery-ui-1.7.1.custom.min.js" type="text/javascript"></script>
    <script src="../../easyui/daterangepicker.jQuery.js" type="text/javascript"></script>
    <script src="../../easyui/ui.datepicker-es.js" type="text/javascript"></script>
      
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

<asp:ScriptManager ID="Sm_Reporte_Mov_Empleasdos" runat="server"/>

<asp:UpdatePanel ID="UPnl_Reporte_Mov_Empleados" runat="server">    
    <ContentTemplate>
    
    
        <div id="Div_Contenedor_Principal" style="width:98%; background-color:White;">
            <table width="100%">
                <tr>
                    <td class="button_autorizar"  style="width:100%; cursor:default;" align="center">
                        Reporte de Movimiento de los Empleados
                    </td>
                </tr>
                <tr>
                    <td class="button_autorizar"  style="width:100%; cursor:default;">
                        <div id="Div_Contenedor_Errorres" style="display:none;">
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" 
                                style="display:none;" Width="24px" Height="24px"/>
                            <label id="Lbl_Mensaje_Error"  class="estilo_fuente_mensaje_error"/>
                        </div>
                    </td>
                </tr>                
            </table>
            
            <table width="100%">
                <tr>
                    <td class="button_autorizar" style="width:20%; cursor:default; font-size:11px;">
                        Tipo Movimiento
                    </td>
                    <td class="button_autorizar" style="width:30%; cursor:default;">
                        <asp:DropDownList ID="Cmb_Tipo_Movimiento" runat="server" Width="100%">
                            <asp:ListItem>&lt;- Seleccione -&gt;</asp:ListItem>
                            <asp:ListItem>ALTA</asp:ListItem>
                            <asp:ListItem>BAJA</asp:ListItem>
                            <asp:ListItem>REINGRESO</asp:ListItem>
                            <asp:ListItem>REACTIVACION</asp:ListItem>
                            <asp:ListItem>PROMOCION</asp:ListItem>
                            <asp:ListItem>ACTUALIZACION</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td  style="width:20%; cursor:default;">                        
                    </td>             
                    <td   style="width:30%; cursor:default;">                        
                    </td>                                               
                </tr>
                <tr>
                    <td class="button_autorizar"  style="width:20%; cursor:default; font-size:11px;">
                        Tipo Nómina
                    </td>
                    <td class="button_autorizar"  style="width:80%; cursor:default;" colspan="3">
                        <asp:DropDownList ID="Cmb_Tipos_Nominas" runat="server" Width="100%"/>
                    </td>                                               
                </tr>                
                <tr>
                    <td class="button_autorizar"  style="width:20%; cursor:default; font-size:11px;">
                        U. Responsable
                    </td>
                    <td class="button_autorizar"  style="width:80%; cursor:default;" colspan="3">
                        <asp:DropDownList ID="Cmb_Unidad_Responsable" runat="server" Width="100%"/>
                    </td>                                               
                </tr>     
                <tr>
                    <td class="button_autorizar"  style="width:20%; cursor:default; font-size:11px;">
                        Empleados
                    </td>
                    <td class="button_autorizar"  style="width:80%; cursor:default;" colspan="3">
                        <asp:DropDownList ID="Cmb_Empleados" runat="server" Width="100%"/>
                    </td>                                               
                </tr>    
                <tr>
                    <td class="button_autorizar"  style="width:20%; cursor:default; font-size:11px;">
                        Fecha Inicio
                    </td>
                    <td class="button_autorizar"  style="width:30%; cursor:default;">
                        <input type="text" id="Txt_Fecha_Inicio"  style="width: 160px; height: 1.1em; display:block;"/>
                    </td>
                    <td class="button_autorizar"  style="width:20%; cursor:default; font-size:11px;">                        
                        Fecha Fin
                    </td>             
                    <td class="button_autorizar"  style="width:30%; cursor:default;">
                        <input type="text" id="Txt_Fecha_Fin" style="width: 160px; height: 1.1em; display:block;"/>                        
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
                    <td style="width:100%; text-align:right; cursor:default;" colspan="4" class="button_autorizar" >
                        <asp:ImageButton ID="Btn_Generar_Reporte" runat="server" Text="Generar Reporte"  ToolTip="Reporte de movimientos de los empleados"
                            ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" Width="32px" Height="32px" Style="cursor: hand;" />
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


<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Ver_Documentos_Empleados.aspx.cs" Inherits="paginas_Nomina_Frm_Rpt_Ver_Documentos_Empleados" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script src="../../easyui/jquery-1.4.2.min.js" type="text/javascript"></script>   
    <link href="../../easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../../easyui/jquery.easyui.min.js" type="text/javascript"></script>

    <script src="../../javascript/Js_Descargar_Documentos_Empl.js" type="text/javascript"></script>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<div id="capa_contenedor" style="background-color:#ffffff; width:98%; height:100%; ">
   <table id="tabla_cabecera" width="830px"  border="0" cellspacing="0" class="estilo_fuente">
        <tr align="center">
            <td colspan="2" class="label_titulo">
                <br />Documentos de los Empleados<br /><br />
            </td>
        </tr>
        <tr id="fila_ctlr_errores" style="display:none;">
            <td colspan="2">
                <img id="Img_Error" src="../imagenes/paginas/sias_warning.png" alt="" style="width:32px;height:32px;"/>
                &nbsp;
                <label id="Lbl_Mensaje_Error" class="estilo_fuente_mensaje_error"/>
            </td>
        </tr>
        <tr class="barra_busqueda" align="right">
            <td align = "left">
                <input type="image" id="Btn_Limpiar" src="../imagenes/paginas/sias_clear.png" style="width:24px;height:24px; display:none;" title="Limpiar controles de la página."/>               
               <input type="image" id="Btn_Salir" src="../imagenes/paginas/icono_salir.png" style="width:24px;height:24px;" title="Salir de la página."/>
            </td>
            <td style="width:50%" id="busqueda_recibos_nomina">
                <input type="image" id="Btn_Busqueda_Empleados" src="../imagenes/paginas/Busqueda_00001.png" style="width:24px;height:24px;"/>
            </td>                   
        </tr>
    </table>
    
    <center>
    <asp:Panel ID="Pnl_Datos_Empleado" runat="server">
        <table width="600px">
            <tr>
                <td colspan="2" style="width:100%;">
                    <hr />
                </td>                   
            </tr>
            <tr>
                <td style="width:10%;vertical-align:top ;" align="center">
                    <asp:Image ID="Img_Foto_Empleado_Solicitante" runat="server"  Width="70px" Height="85px"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_No_Disponible.JPG" BorderWidth="1px" BorderStyle="Outset" style="color:White;"/>
                </td>
                <td style="width:90%; vertical-align:bottom;">                
                    <table width="100%">
                        <tr>
                            <td style="width:100%;text-align:left;">
                                <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" Width="98%" Enabled="false" 
                                    style="vertical-align:middle; border-style:outset; border-color:White; letter-spacing:.1em; 
                                        font-size:14px; height:28px; padding:  9px 3px 9px 3px;"/>
                            </td>                                                    
                        </tr>                                                        
                    </table>
                </td>                        
            </tr>
            <tr>
                <td colspan="2" style="width:100%;">
                    <hr />
                </td>                   
            </tr>
        </table>                        
    </asp:Panel>        
    
    <table id="Tbl_Documentos_Empleados"></table>
    </center>
    
    <div  id="Div_Busquedas" class="easyui-window" 
        style='border-style:outset;border-color:Silver;background-image:url(../imagenes/paginas/fondo1.jpg);background-repeat:repeat-y;'>
        <center></center>
    </div>    

    <input type="hidden" id="_Nomina_ID"/>
    <input type="hidden" id="_No_Nomina"/>
    <input type="hidden" id="_Tipo_Nomina"/>
    <input type="hidden" id="_No_Empleado"/>
    <input type="hidden" id="_UR"/>
    <input type="hidden" id="_RFC"/>
    <input type="hidden" id="_CURP"/>
    <input type="hidden" id="_Banco_ID"/>
</div>    
</asp:Content>


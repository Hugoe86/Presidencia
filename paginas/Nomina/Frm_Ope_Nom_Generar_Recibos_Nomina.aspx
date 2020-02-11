<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Generar_Recibos_Nomina.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Generar_Recibos_Nomina" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">

     
    <link href="../../easyui/themes/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/icon.css" rel="stylesheet" type="text/css" />   
    <link href="../../easyui/themes/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />
    
    <script src="../../jquery/jquery-1.5.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
    <script src="../../javascript/Js_Ope_Nom_Generacion_Recibos_Nomina.js" type="text/javascript"></script>   
    <script src="../../easyui/jquery.progressbar.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.progressbar.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

<div id="capa_contenedor" style="background-color:#ffffff; width:98%; height:100%; ">
   <table id="tabla_cabecera" width="830px"  border="0" cellspacing="0" class="estilo_fuente">
        <tr align="center">
            <td colspan="2" class="label_titulo">
                <br />Generaci&oacute;n de Recibos de N&oacute;mina<br /><br />
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
                <input type="image" id="Btn_Limpiar" src="../imagenes/paginas/sias_clear.png" style="width:24px;height:24px;" title="Limpiar controles de la página."/>
               <input type="image" id="Btn_Imprimir_Recibos_Nomina" src="../imagenes/gridview/grid_print.png" style="width:24px;height:24px;" title="Imprimir recibo (s) de nómina."/>
               <input type="image" id="Btn_Salir" src="../imagenes/paginas/icono_salir.png" style="width:24px;height:24px;" title="Salir de la página."/>
            </td>
            <td style="width:50%" id="busqueda_recibos_nomina">
                <input type="image" id="Btn_Busqueda_Avanzada_Recibos" src="../imagenes/paginas/Busqueda_00001.png" style="width:24px;height:24px;"/>
            </td>                   
        </tr>
    </table>
    <div  id="Div_Busquedas" class="easyui-window"><center></center></div>
    <table style="width:830px;">
        <tr>
            <td colspan="2">
            </td>
        </tr>
        <tr id="Recibos">
            <td colspan="2">
                <table id="tabla_recibo_nomina" width="830px"></table>
            </td>
        </tr>
        <tr id="conceptos">
            <td style="width:415px;">
                <table id="tabla_percepciones" width="414px"></table>
            </td>
            <td style="width:415px;">
                <table id="tabla_deducciones" width="414px"></table>
            </td>            
        </tr>
    </table>

    <center>
        <div id="progressBackgroundFilter"  class="progressBackgroundFilter" style="display:none;"></div>
        <div id="Barra_Progreso" class="processMessage" style="display:none;"></div>
    </center>

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


<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Creacion_Plazas.aspx.cs" Inherits="paginas_Frm_Cat_Nom_Creacion_Plazas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <link href="../../easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/css/redmond/jquery-ui-1.7.1.custom.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/css/ui.daterangepicker.css" rel="stylesheet" type="text/css" />
    
    <script src="../../easyui/jquery-1.4.2.min.js" type="text/javascript"></script>   
    <script src="../../easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery-ui-1.7.1.custom.min.js" type="text/javascript"></script>
    <script src="../../easyui/daterangepicker.jQuery.js" type="text/javascript"></script>
    <script src="../../easyui/ui.datepicker-es.js" type="text/javascript"></script>
    <script src="../../javascript/Js_Cat_Nom_Creacion_Plazas.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<center>

<table  style="width:90%; background-image: url(../imagenes/overlays/white_.png); background-repeat:no-repeat; padding:40px 30px;">
    <tr>
        <td style="width:20%; text-align:left; font-size:small;">
            Unidad Responsable
        </td>
        <td style="width:80%; text-align:left;">
            <select id="Cmb_Unidades_Reponsables" style="width:444px;" title="Este dato es requerido"></select>
            
        </td>
    </tr>
    <tr>
        <td style="width:20%; text-align:left; font-size:small;">
            Puesto
        </td>
        <td style="width:30%; text-align:left;">
            <select id="Cmb_Puestos" style="width:80%;"></select>
        </td>
    </tr>
    <tr>
        <td style="width:20%; text-align:left; font-size:small;">
            Tipo Plaza
        </td>
        <td style="width:80%; text-align:left;">
            <select id="Cmb_Tipo_Paza" style="width:80%;">
              <option value="">&lt;-- Seleccione -- &gt; </option>
              <option value="BASE-SUBSEMUN-SUBROGADOS">BASE-SUBSEMUN-SUBROGADOS</option>
              <option value="EVENTUAL">EVENTUAL</option>
              <option value="ASIMILABLE">ASIMILABLE</option>
              <option value="PENSIONADO">PENSIONADO</option>
              <option value="DIETA">DIETA</option>
            </select>
        </td>
    </tr>
    <tr>
        <td style="width:100%; text-align:left; font-size:x-large; text-align:right;" colspan="2">
            <span id="Span_SD" style="font-family:Lucida Grande, Tahoma, Arial, Verdana, sans-serif; 
                font-size:x-small; font-weight:bold; color:#565656; padding:0px 115px 0px 0px;"></span>
            <br />
            <span id="Span_S" style="font-family:Lucida Grande, Tahoma, Arial, Verdana, sans-serif; 
                font-size:x-small; font-weight:bold; color:#565656; padding:0px 115px 0px 0px;"></span>
            <br />
            <span id="Span_PSM_Diaria" style="font-family:Lucida Grande, Tahoma, Arial, Verdana, sans-serif; 
                font-size:x-small; font-weight:bold; color:#565656; padding:0px 115px 0px 0px;"></span>
        </td>
    </tr>
</table>

<table id="Tbl_Proyeccion">
    <tr>
        <td style="width:20%; text-align:left; 
                font-family:Lucida Grande, Tahoma, Arial, Verdana, sans-serif; 
                font-size:x-small; font-weight:bold; color:#565656; padding-left:15px;">
            Fecha Inicia Proyecci&oacute;n
        </td>
        <td style="width:30%; text-align:left; font-size:x-small;">
            <input type="text" id="Txt_Fecha"  style="width: 160px; height: 1.1em; display:block;text-align:center;"/>
        </td>
        <td style="width:8%; text-align:left; 
                font-family:Lucida Grande, Tahoma, Arial, Verdana, sans-serif; 
                font-size:x-small; font-weight:bold; color:#565656; padding-left:15px;">
                Sueldo
        </td>
        <td style="width:42%; text-align:left; font-size:x-small;text-align:left;">
            <input type="text" id="Txt_Proyeccion"  style="width: 56%;" disabled='disabled'/>
        </td>
    </tr>
    
    <tr>
        <td style="width:20%; text-align:left; 
                font-family:Lucida Grande, Tahoma, Arial, Verdana, sans-serif; 
                font-size:x-small; font-weight:bold; color:#565656; padding-left:15px;">
        </td>
        <td style="width:30%; text-align:right; font-size:x-small;">
            <input type="image" src="../imagenes/paginas/SIAS_Calc3.gif" id="Link_Proyeccion" 
                title='Calcular la proyección de Sueldo y PSM si aplica'/>
        </td>
        <td style="width:8%; text-align:left; 
                font-family:Lucida Grande, Tahoma, Arial, Verdana, sans-serif; 
                font-size:x-small; font-weight:bold; color:#565656; padding-left:15px;">
            PSM
        </td>
        <td style="width:42%; text-align:left; font-size:x-small;text-align:left;">
            <input type="text" id="Txt_Proyeccion_PSM"  style="width: 56%;" disabled='disabled'/>
        </td>
    </tr>
</table>

<table id="SAP_Codigos_Programaticos_Sueldo" style="width:90%; background-image: url(../imagenes/overlays/white_.png); background-repeat:no-repeat; padding:30px 30px;">
    <tr>
        <td style="width:100%; text-align:left; font-family:Lucida Grande, Tahoma, Arial, Verdana, sans-serif; font-size:small; font-weight:bold; color:#565656;" colspan="2">
            C&oacute;digo Program&aacute;tico Partida Sueldos
            <hr style='width:70%;'/>
        </td>
    </tr>
    <tr>
        <td style="width:20%; text-align:left; font-size:x-small;">
            (S) Fte Finaciamiento
        </td>
        <td style="width:80%; text-align:left;">
            <select id="Cmb_S_Fte_Finaciamiento" style="width:80%;"></select>
        </td>
    </tr>
    <tr>
        <td style="width:20%; text-align:left; font-size:x-small;">
            (S) Area Funcional
        </td>
        <td style="width:80%; text-align:left;">
            <select id="Cmb_S_Area_Funcional" style="width:80%;"></select>
        </td>
    </tr>
    <tr>
        <td style="width:20%; text-align:left; font-size:x-small;">
            (S) Programa
        </td>
        <td style="width:80%; text-align:left;">
            <select id="Cmb_S_Programa" style="width:80%;"></select>
        </td>
    </tr>
    <tr>
        <td style="width:20%; text-align:left; font-size:x-small;">
            (S) Unidad Responsable
        </td>
        <td style="width:80%; text-align:left; font-size:x-small;">
            <select id="Cmb_S_Dependencia" style="width:80%;"></select>
        </td>
    </tr>
    <tr>
        <td style="width:20%; text-align:left; font-size:x-small;">
            (S) Partida
        </td>
        <td style="width:80%; text-align:left;">
            <select id="Cmb_S_Partida" style="width:80%;"></select>
            <span id="Span_Disponible" style="font-family:Courier New; font-size:large; font-weight:bold; padding:0px 115px 0px 0px;"></span>
        </td>
    </tr>
</table>

<table id="SAP_Codigos_Programaticos_PSM" style="width:90%; background-image: url(../imagenes/overlays/white_.png); background-repeat:no-repeat; padding:30px 30px;">
    <tr>
        <td style="width:100%; text-align:left; font-family:Lucida Grande, Tahoma, Arial, Verdana, sans-serif; font-size:small; font-weight:bold; color:#565656;" colspan="2">
            C&oacute;digo Program&aacute;tico Partida PSM
            <hr style='width:70%;'/>
        </td>
    </tr>
    <tr>
        <td style="width:20%; text-align:left; font-size:x-small;">
            (PSM) Fte Finaciamiento
        </td>
        <td style="width:80%; text-align:left;">
            <select id="Cmb_PSM_Fte_Financiamiento" style="width:80%;"></select>
        </td>
    </tr>
    <tr>
        <td style="width:20%; text-align:left; font-size:x-small;">
            (PSM) Area Funcional
        </td>
        <td style="width:80%; text-align:left;">
            <select id="Cmb_PSM_Area_Funcional" style="width:80%;"></select>
        </td>
    </tr>
    <tr>
        <td style="width:20%; text-align:left; font-size:x-small;">
            (PSM) Programa
        </td>
        <td style="width:80%; text-align:left;">
            <select id="Cmb_PSM_Programa" style="width:80%;"></select>
        </td>
    </tr>
    <tr>
        <td style="width:20%; text-align:left; font-size:x-small;">
            (PSM) Unidad Responsable
        </td>
        <td style="width:80%; text-align:left;">
            <select id="Cmb_PSM_Dependencia" style="width:80%;"></select>
        </td>
    </tr>
    <tr>
        <td style="width:20%; text-align:left; font-size:x-small;">
            (PSM) Partida
        </td>
        <td style="width:80%; text-align:left;">
            <select id="Cmb_PSM_Partida" style="width:80%;"></select>
            <span id="Span_Disponible_PSM" style="font-family:Courier New; font-size:large; font-weight:bold; padding:0px 115px 0px 0px;"></span>
        </td>
    </tr>
</table>

<br />
</center>

<div align="left" style="padding-left:70px;">
    <input id="Link_Validar" type="image" src = "../imagenes/paginas/accept.png" style="width:24px; height.24px;" />
</div>

<div style="padding-left:70px;">
    <table id="Tbl_Plazas" ></table>
    <br /><br />
</div>

</asp:Content>


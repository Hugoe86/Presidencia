<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" 
    CodeFile="Frm_Cat_Nom_Reporteador.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Reporteador" Title="Untitled Page" 
    EnableEventValidation="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    
    <link href="../../easyui/themes/default/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/default/ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="../../easyui/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.bgiframe-2.1.2.js" type="text/javascript"></script>
    <script src="../../easyui/jquery-ui-i18n.min.js" type="text/javascript"></script>
    
    <link href="../../easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../../easyui/jquery.easyui.min.js" type="text/javascript"></script>    
    <script src="../../javascript/Js_Rpt_Nom_Reporteador.js" type="text/javascript"></script>
    
    <script src="../../easyui/jquery.progressbar.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.progressbar.min.js" type="text/javascript"></script>    
    
	<style type="text/css">
	    #sortable1, #sortable2 { list-style-type: none; margin: 0; padding: 0; float: left; margin-right: 10px; }
	    #sortable1 li, #sortable2 li { margin: 0 5px 5px 5px; padding: 5px; font-size: 1.2em; width: 120px; }
	</style>
	
	
	<script type="text/javascript">
	$(function() {
		$( "#sortable1, #sortable2" ).sortable({
			connectWith: ".connectedSortable"
		}).disableSelection();
	});
	</script>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

<center>
   <table id="tabla_cabecera" width="830px"  border="0" cellspacing="0" class="estilo_fuente">
        <tr align="center">
            <td colspan="2" class="label_titulo">
                <br />Reportes Cat&aacute;logos de Nomina<br /><br />
            </td>
        </tr>
        <tr class="barra_busqueda" align="right">
            <td align = "left">
                <input type="image" id="Btn_Limpiar" src="../imagenes/paginas/sias_clear.png" style="width:24px;height:24px;" title="Limpiar controles de la página."/>
               <input type="image" id="Btn_Salir" src="../imagenes/paginas/icono_salir.png" style="width:24px;height:24px;" title="Salir de la página."/>
            </td>
            <td style="width:50%" id="busqueda_recibos_nomina">
                
            </td>                   
        </tr>
    </table>
    <table style="width:78%;">
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>      
        <tr>
            <td class="button_autorizar" style="cursor:default; width:30%; text-align: left;">
                Cat&aacute;logos Nomina
            </td>
            <td class="button_autorizar" style="cursor:default; width:70%; text-align: left;">
                <asp:DropDownList ID="cmb_tablas_catalogos_nomina" runat="server" Width="100%"/>
            </td>        
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>    
    </table>
    
    <center>
        <table style="width:78%;">       
            <tr>
                <td class="datagrid-toolbar" 
                    style="width:100%; text-align:right; cursor:default; "

                        colspan="4">
                    <asp:ImageButton ID="Btn_Generar_Reporte" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                         OnClick="Btn_Generar_Reporte_Click" ToolTip="Generar Reporte en EXCEL" Width="32px" Height="32px" style="cursor:hand;"/>
                </td>                
            </tr>   
            <tr>
                <td style="width:100%; text-align:left; cursor:default;" colspan="4">
                    <hr />
                </td>                
            </tr>                                                                         
        </table>
    </center>
    
    <div class="demo" style="width:78%;">
        <table style="width:100%;">
            <tr>
                <td style="width:50%;  vertical-align:top;">   
                    <table style="width:99%;">
                        <tr>
                            <td style="width:100%;" align="center">
                                <div id="Contenedor_Titulo" style="color:White;font-size:12;font-weight:bold;border-style:outset;background:url(../imagenes/paginas/titleBackground.png) repeat-x top;background-color:Silver;">
                                    <table width="100%">        
                                        <tr>
                                            <td width="100%">
                                                <font style="color: Black; font-weight: bold;">Campos Disponibles</font>
                                            </td>    
                                        </tr>                                      
                                    </table>    
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td width="100%" style="text-align:center; ">
                                <ul id="sortable1" class="connectedSortable" style="border-top:outset 1px White; 
                                                                                    border-left:outset 1px White; 
                                                                                    border-right:outset 1px White; 
                                                                                    width:98%; color:White; 
                                                                                    overflow: hidden; height:1%;">
                                </ul>
                            </td>    
                        </tr>
                    </table>                                      
                </td>
                <td style="width:50%; vertical-align:top;">     
                    <table style="width:99%;">
                        <tr>
                            <td style="width:100%;" align="center">
                                <div id="Div1" style="color:White;font-size:12;font-weight:bold;border-style:outset;background:url(../imagenes/paginas/titleBackground.png) repeat-x top;background-color:Silver;">
                                    <table width="100%">        
                                        <tr>
                                            <td width="100%">
                                                <font style="color: Black; font-weight: bold;">Campos Reporte</font>
                                            </td>    
                                        </tr>                                      
                                    </table>    
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td width="100%" style="text-align:center; overflow: hidden; height:1%;">
                                <ul id="sortable2" class="connectedSortable" style="border-top:outset 1px White; 
                                                                                    border-left:outset 1px White; 
                                                                                    border-right:outset 1px White; 
                                                                                    width:98%; color:White; 
                                                                                    overflow: hidden; height:1%;">
                                </ul>
                            </td>    
                        </tr>
                    </table>                                                    
                </td>                
            </tr>
        </table>
        </ul>
    </div>
</center>

<asp:HiddenField ID="Txt_Campos_Reporte" runat="server" />
<asp:HiddenField ID="Txt_Tabla" runat="server" />

<center>
    <div id="progressBackgroundFilter"  class="progressBackgroundFilter" style="display:none;"></div>
    <div id="Barra_Progreso" class="processMessage" style="display:none;"></div>
</center>  
</asp:Content>


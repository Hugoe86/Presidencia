﻿///***************************************************************************************************************************************
///********************************************************* INICIO **********************************************************************
///***************************************************************************************************************************************
var DOM_Pagina = $(document);
DOM_Pagina.ready(function(){
    $('#Div_Busquedas').window('close');
    Configuracion_Inicial();
});

function StringBuilder(value)
{
    this.strings = new Array("");
    this.append(value);
}

StringBuilder.prototype.append = function (value)
{
    if (value)
    {
        this.strings.push(value);
    }
}

StringBuilder.prototype.clear = function ()
{
    this.strings.length = 1;
}

StringBuilder.prototype.toString = function ()
{
    return this.strings.join("");
}
///***************************************************************************************************************************************
///********************************************************* METODOS *********************************************************************
///***************************************************************************************************************************************

///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Lauch_Panel_Busqueda
///DESCRIPCIÓN          : Funcion para llenar ´llenar el panel de la busqueda de recibos
///PROPIEDADES          :
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : Noviembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function Lauch_Panel_Busqueda(){
    var Str_Panel_Busqueda = Crear_Panel_Busqueda_Recibos_Nomina();
    var left = (screen.width/2)-(540/2);
    var top = (screen.height/2)-(500/2);
    
    $('#Div_Busquedas').window({
        title: 'Busqueda Empleados',
        width: 540,
        modal: true,
        shadow: false,
        closed: false,
        height: 500,
        collapsible:false,  
        minimizable:false,  
        maximizable:false,
        top:top,
        left:left                    
    });
    
    var Div_Busquedas = $('#Div_Busquedas');
    $('#Div_Busquedas').empty();
    $(Str_Panel_Busqueda).appendTo(Div_Busquedas);
}
///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Configuracion_Inicial
///DESCRIPCIÓN          : Funcion de la configuración inicial de los eventos de los botones de la pagina
///PROPIEDADES          :
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : Noviembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function Configuracion_Inicial(){
    $('#Btn_Busqueda_Empleados').live("click", function(e){
        e.preventDefault();
        Lauch_Panel_Busqueda();
        Consultar_Tipos_Nomina();
        Consultar_Unidades_Responsables();
        Limpiar_Variables();
    });  
    
    $('#Btn_Limpiar').bind('click', function(evento){
        evento.preventDefault();
        location.reload();
    });
    
    $('#Btn_Salir').bind('click', function(evento){
        evento.preventDefault();
        window.location.replace("../Paginas_Generales/Frm_Apl_Principal.aspx");
    });    
}
///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Limpiar_Variables
///DESCRIPCIÓN          : Funcion para limpiar las variables de la pagina
///PROPIEDADES          :
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : 12/Diciembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function Limpiar_Variables(){
    $('#_Nomina_ID').val('');
    $('#_No_Nomina').val('');
    $('#_Tipo_Nomina').val('');
    $('#_UR').val('');
    $('#_No_Empleado').val('');
    $('#_RFC').val('');
    $('#_CURP').val('');
    $('#_Banco_ID').val('');
    $('img[id$=Img_Foto_Empleado_Solicitante]').attr('src', '../imagenes/paginas/Sias_No_Disponible.JPG');
    $('input[id$=Txt_Nombre_Empleado]').val('');
}
///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Crear_Panel_Busqueda_Recibos_Nomina
///DESCRIPCIÓN          : Funcion para crear la interfaz el panel de busqueda
///PROPIEDADES          :
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : Noviembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function Crear_Panel_Busqueda_Recibos_Nomina(){
  var HTML  =new StringBuilder();
    
    HTML.append("<center><table>");
    HTML.append("<tr>");
    HTML.append("<td style='width:100%;' align='center'>");         
        
        HTML.append("<table style='width:500px;' border='0px'>");
        
            HTML.append('<tr style="border-style:outset;">');
                HTML.append('<td style="text-align:left; width:20%; cursor:default;" colspan="4">'); 
                    HTML.append('<hr />');             
                HTML.append('</td>');
            HTML.append('</tr>');
            
           HTML.append('<tr">');      
                HTML.append('<td style="text-align:left; width:20%; cursor:default; font-size:11px;">'); 
                    HTML.append('No Empleado');             
                HTML.append('</td>');
                HTML.append('<td style="text-align:left; width:30%; cursor:default;">');  
                    HTML.append('<input type="text" id="Txt_No_Empleado" style="width:98%;" class="texto"/>');            
                HTML.append('</td>');
                HTML.append('<td style="text-align:left; width:20%; cursor:default; font-size:11px;">');         
                HTML.append('</td>');
                HTML.append('<td style="text-align:left; width:30%; cursor:default;">');     
                HTML.append('</td>');                
            HTML.append('</tr>');
            
           HTML.append('<tr">');
                HTML.append('<td style="text-align:left; width:20%; cursor:default; font-size:11px;">'); 
                    HTML.append('RFC');             
                HTML.append('</td>');
                HTML.append('<td style="text-align:left; width:30%; cursor:default;">');  
                    HTML.append('<input type="text" id="Txt_RFC" style="width:98%;" class="texto"/>');            
                HTML.append('</td>');
                HTML.append('<td style="text-align:left; width:20%; cursor:default; font-size:11px;">'); 
                    HTML.append('&nbsp;&nbsp;CURP');             
                HTML.append('</td>');
                HTML.append('<td style="text-align:left; width:30%; cursor:default;">');  
                    HTML.append('<input type="text" id="Txt_CURP" style="width:97%;" class="texto"/>');            
                HTML.append('</td>');
            HTML.append('</tr>');  
            
            HTML.append('<tr style="border-style:outset;">');
                HTML.append('<td style="text-align:left; width:20%; cursor:default; font-size:11px;">'); 
                    HTML.append('Tipos Nomina');             
                HTML.append('</td>');
                HTML.append('<td style="text-align:left; width:80%; cursor:default;" colspan="3">');  
                    HTML.append('<select id="Cmb_Tipos_Nomina" style="width:100%;"/>');            
                HTML.append('</td>');
            HTML.append('</tr>');                     
            
            HTML.append('<tr style="border-style:outset;">');
                HTML.append('<td style="text-align:left; width:20%; cursor:default; font-size:11px;">'); 
                    HTML.append('U.Responsable');             
                HTML.append('</td>');
                HTML.append('<td style="text-align:left; width:80%; cursor:default;" colspan="3">');  
                    HTML.append('<select id="Cmb_UR" style="width:100%;"/>');            
                HTML.append('</td>');
            HTML.append('</tr>');
            
            HTML.append('<tr style="border-style:outset;">');
                HTML.append('<td style="text-align:left; width:20%; cursor:default;" colspan="4">'); 
                    HTML.append('<hr />');             
                HTML.append('</td>');
            HTML.append('</tr>');            
            
            HTML.append('<tr style="border-style:outset;">');
                HTML.append('<td style="text-align:left; width:20%; cursor:default;" colspan="4">'); 
                    HTML.append('<input type="button" id="Btn_Ejecutar_Consulta" value="Buscar Documentos Empleado" class="button_autorizar" style="background-image:url(../imagenes/paginas/busqueda.png); background-repeat: no-repeat; background-position:right; cursor:hand;">');             
                HTML.append('</td>');
            HTML.append('</tr>');
            
            HTML.append('<tr style="border-style:outset;">');
                HTML.append('<td style="text-align:left; width:20%; cursor:default;" colspan="4">'); 
                    HTML.append('<hr />');             
                HTML.append('</td>');
            HTML.append('</tr>');       
            
            HTML.append('<tr style="border-style:outset;">');
                HTML.append('<td style="text-align:left; width:100%; cursor:default;" colspan="4">'); 
                    HTML.append('<table id="Tbl_Empleados" class="easyui-datagrid"></table>');             
                HTML.append('</td>');
            HTML.append('</tr>');        
            
        HTML.append("</table>");
        
                
    HTML.append("</td>");
    HTML.append("</tr>");
    HTML.append("</table></center>");

    $('input.texto').live('click', function(e){ e.preventDefault(); $(this).focus().select();});
    $('input[id$=Txt_No_Empleado]').live('keyup blur', function(){ if($(this).val().match(/[^0-9]/g)){ $(this).val($(this).val().replace(/[^0-9]/g, ''));}});
    $('input[id$=Txt_No_Empleado]').live("blur", function(){
        var Ceros = "";
        if($(this).val() != undefined){
            if($(this).val() != ''){
                for(i=0; i<(6-$(this).val().length); i++){
                    Ceros += '0';
                }
                $(this).val(Ceros + $(this).val());
                Ceros = "";
            }else $(this).val('');
        }
    });
    
    $('input[id$=Txt_RFC]').live('blur', function(){$(this).filter(function(){if(!this.value.match(/^[a-zA-Z]{3,4}(\d{6})((\D|\d){3})?$/))$(this).val('');});});
    $('input[id$=Txt_CURP]').live('blur', function(){$(this).filter(function(){if(!this.value.match(/^[a-zA-Z]{4}(\d{6})([a-zA-Z]{6})(\d{2})?$/))$(this).val('');});});    
    $('input.texto').live('keyup', function(){$(this).focus().val($(this).val().toUpperCase());});        
    $('#Btn_Ejecutar_Consulta').live('click', function(e){LLenar_Grid_Empleados();});         
    $('input[id$=Btn_Ejecutar_Consulta]').live("mouseover", function(e){e.preventDefault();$(this).css("background-color", "#2F4E7D");$(this).css("color", "#FFFFFF");});
    $('input[id$=Btn_Ejecutar_Consulta]').live("mouseout", function(e){e.preventDefault();$(this).css("background-color", "#f5f5f5");$(this).css("color", "#565656");});      
    
    return HTML.toString();
}
///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Consultar_Tipos_Nomina
///DESCRIPCIÓN          : Funcion para consultar los tipos de nominas
///PROPIEDADES          :
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : Noviembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function Consultar_Tipos_Nomina(){
    var Combo = $('#Cmb_Tipos_Nomina').get(0);
    
    $.ajax({
        url: 'Frm_Rpt_Ver_Documentos_Empl_Ctrl.aspx?tabla=TIPOS_NOMINAS&opcion=consultar_tipos_nominas',
        type: 'POST',
        async: false,
        cache: false,
        datatype: 'json',
        success: function (Datos){
            if(Datos != null){
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.TIPOS_NOMINAS, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.NOMINA, Elemento.TIPO_NOMINA_ID);                             
                 });
             }
             else {
                 $.messager.alert("Mensaje", "No hay tipos de nómina registrados en el sistema.");
             } 
        }
    });
}
///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Consultar_Unidades_Responsables
///DESCRIPCIÓN          : Funcion para consultar las unidades responsables y llenar el combo
///PROPIEDADES          :
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : Noviembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function Consultar_Unidades_Responsables(){
    var Combo = $('#Cmb_UR').get(0);
    
    $.ajax({
        url: 'Frm_Rpt_Ver_Documentos_Empl_Ctrl.aspx?tabla=UR&opcion=consultar_unidad_responsable',
        type: 'POST',
        async: false,
        cache: false,
        dataType: 'json',
        success: function(Datos){
            if(Datos != null){
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.UR, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option("[" + Elemento.CLAVE + "] -- " + Elemento.NOMBRE, Elemento.DEPENDENCIA_ID);                             
                 });
             }
             else {
                 $.messager.alert("Mensaje","No hay unidades responsables registradas en el sistema.");
             } 
        }
    });
}
///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : LLenar_Grid_Empleados
///DESCRIPCIÓN          : Funcion para llenar el grid de con los filtros de la busqueda
///PROPIEDADES          :
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : Noviembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
 function LLenar_Grid_Empleados(){
    $('#conceptos table > tbody > tr').remove();
 
    $('#_Nomina_ID').val($('#Cmb_Calendario_Nomina :selected').val());
    $('#_No_Nomina').val($('#Cmb_Periodo_Nomina :selected').val());
    $('#_Tipo_Nomina').val($('#Cmb_Tipos_Nomina :selected').val());
    $('#_UR').val($('#Cmb_UR :selected').val());
    $('#_No_Empleado').val($('#Txt_No_Empleado').val());
    $('#_RFC').val($('#Txt_RFC').val());
    $('#_CURP').val($('#Txt_CURP').val());
    $('#_Banco_ID').val($('#Cmb_Banco :selected').val());
    var Parametros = new StringBuilder();
    
    Parametros.append('tipo_nomina_id=');
    Parametros.append($('#_Tipo_Nomina').val());
    Parametros.append('&no_empleado=');
    Parametros.append($('#_No_Empleado').val());
    Parametros.append('&rfc=');
    Parametros.append($('#_RFC').val());
    Parametros.append('&curp=');
    Parametros.append($('#_CURP').val());
    Parametros.append('&ur=');
    Parametros.append($('#_UR').val());
    Parametros.append('&tabla=');
    Parametros.append("rows");
    Parametros.append('&opcion=');
    Parametros.append('consultar_empleados');
 
 //inicializamos el grid
    $('#Tbl_Empleados').datagrid({
        title:'EMPLEADOS',
        iconCls:'icon-save',
        width:500,
        height:240,
        collapsible:true,
        url:'Frm_Rpt_Ver_Documentos_Empl_Ctrl.aspx?' + Parametros.toString(),
        columns:
        [[
            {field:'NO_EMPLEADO', title: ' No Empleado', width:100, sortable: true , resizable:true,
                formatter:function(value){
                	return '<span style="font-size:x-small">'+value+'</span>';
                }
            },
            {field:'NOMBRE', title: 'Nombre', width:390, sortable: true , resizable:true,
                formatter:function(value){
                	return '<span style="font-size:x-small">'+value+'</span>';
                }
            },
            {field:'RUTA_FOTO', title: '', width:390, sortable: true , resizable:true, hidden:true,
                formatter:function(value){
                	return '<span style="font-size:x-small">'+value+'</span>';
                }
            }            
        ]],
        pagination: true,
        rownumbers: false,
        idField: 'NO_EMPLEADO',
        sortOrder: 'asc',
        remoteSort: false,
        singleSelect: false,
        loadMsg: "Cargando, espere ...",
        pageNumber: 1,
        pageSize: 20,
        fitColumns: false,
        striped: true,
        nowrap: false,
        pageList: [10, 20, 50, 100],
        remoteSort: false,
        showFooter: true,
        onClickRow: function (rowIndex, rowData) {
            $('input[id$=Txt_Nombre_Empleado]').val("[" + rowData.NO_EMPLEADO + "] -- " + rowData.NOMBRE);
            
            $('img[id$=Img_Foto_Empleado_Solicitante]').attr('src', function(){
                return (rowData.RUTA_FOTO != '') ? rowData.RUTA_FOTO : '../imagenes/paginas/Sias_No_Disponible.JPG';                
             });
            
            LLenar_Grid_Documentos_Empleado(rowData.NO_EMPLEADO);      
            $('#Div_Busquedas').window('close');         
        }
    });
}

///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : LLenar_Grid_Documentos_Empleado
///DESCRIPCIÓN          : Funcion para llenar el grid de con los filtros de la busqueda
///PROPIEDADES          :
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : Noviembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
 function LLenar_Grid_Documentos_Empleado(No_Empleado){
    var Parametros = new StringBuilder();
    
    Parametros.append('no_empleado=');
    Parametros.append(No_Empleado);
    Parametros.append('&tabla=');
    Parametros.append("rows");
    Parametros.append('&opcion=');
    Parametros.append('consultar_documentos_empleados');
 
 //inicializamos el grid
    $('#Tbl_Documentos_Empleados').datagrid({
        title:'Documentos Entregados por el Empleado',
        iconCls:'icon-save',
        width:600,
        height:300,
        collapsible:true,
        url:'Frm_Rpt_Ver_Documentos_Empl_Ctrl.aspx?' + Parametros.toString(),
        columns:
        [[
            {field:'EMPLEADO', title: 'Empleado', width:0, sortable: true , resizable:true, hidden:true,
                formatter:function(value){
                	return '<span style="font-size:x-small">' + value + '</span>';
                }
            },
            {field:'REQUISITO', title: 'Documentos', width:590, sortable: true , resizable:true, align:'center',
                formatter:function(value){
                	return '<span style="font-size:x-small; text-decoration:underline; color:blue; cursor:hand; text-align:center;">' + value + '</span>';
                }
            },
            {field:'RUTA_DOCUMENTO', title: 'Link', width:0, sortable: true , resizable:true, hidden:true,
                formatter:function(value){
                	return '<span style="font-size:x-small; ">' + value + '</span>';
                }
            }
        ]],
        pagination: true,
        rownumbers: false,
        idField: 'EMPLEADO',
        sortOrder: 'asc',
        remoteSort: false,
        singleSelect: false,
        loadMsg: "Cargando, espere ...",
        pageNumber: 1,
        pageSize: 20,
        fitColumns: false,
        striped: true,
        nowrap: false,
        pageList: [10, 20, 50, 100],
        remoteSort: false,
        showFooter: true,
        onClickRow: function (rowIndex, rowData) {
            window.location.href = 'Frm_Mostrar_Archivos.aspx?Documento=' + rowData.RUTA_DOCUMENTO;             
        }
    });
}


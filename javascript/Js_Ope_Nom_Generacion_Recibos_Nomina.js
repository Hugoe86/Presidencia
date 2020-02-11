///***************************************************************************************************************************************
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
        title: 'Recibos de Nomina',
        width: 540,
        modal: true,
        shadow: false,
        closed: false,
        height: 315,
        collapsible:false,  
        minimizable:false,  
        maximizable:false,        
        top:top,
        left:left 
    });
    
    var Div_Busquedas = $('#Div_Busquedas');
    $('#Div_Busquedas').empty();
    $(Str_Panel_Busqueda).appendTo(Div_Busquedas);
    
    $('#Btn_Limpiar').click(function(e){
        e.preventDefault();
        $('#conceptos table > tbody > tr').remove();
        $('#tabla_recibo_nomina').datagrid('unselectAll');
        $('#tabla_recibo_nomina').datagrid('reload');
        LLenar_Grid_Recibos_Nomina();
    });
    
     $("#Barra_Progreso").progressBar({
            steps:100,
            stepDuration:10,
            max:100,
            showText:false,
            textFormat:'percentage',
            width:120,
            height:12,
            callback:null,
            barImage:{
                    0:  '../../easyui/images/progressbg_green.gif',
                    20: '../../easyui/images/progressbg_green.gif',
                    40: '../../easyui/images/progressbg_green.gif',
                    60: '../../easyui/images/progressbg_green.gif',
                    80: '../../easyui/images/progressbg_green.gif',
                    100: '../../easyui/images/progressbg_green.gif'
                     }       
         }); 
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
    $('#Btn_Busqueda_Avanzada_Recibos').live("click", function(e){
        e.preventDefault();
        Lauch_Panel_Busqueda();
        Llenar_Combo_Calendario_Nomina();
        Consultar_Tipos_Nomina();
        Consultar_Unidades_Responsables();
        Consultar_Bancos();
        Limpiar_Variables();
    });  
    
    $('#Btn_Imprimir_Recibos_Nomina').live("click", function(e){
        e.preventDefault();
        if(Validar_Datos()){
            Generar_Recibo_Nomina();
        }
    });
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
    
    HTML.append("<table style='width:100%; height:200px; color:Black;font-size:10px;'>");
    HTML.append("<tr>");
    HTML.append("<td style='width:100%;' align='center'>");
         
        HTML.append("<table style='width:500px;border:outset 1px Silver;' border='0px'>");            
            HTML.append('<tr style="border-style:outset;">');
                HTML.append('<td class="button_autorizar" style="text-align:left; width:20%; cursor:default;">'); 
                    HTML.append('N&oacute;mina');             
                HTML.append('</td>');
                HTML.append('<td class="button_autorizar" style="text-align:left; width:30%; cursor:default;">');  
                    HTML.append('<select id="Cmb_Calendario_Nomina" style="width:100%;"/>');            
                HTML.append('</td>');
                HTML.append('<td class="button_autorizar" style="text-align:left; width:20%; cursor:default;">');   
                    HTML.append('&nbsp;&nbsp;Periodo');               
                HTML.append('</td>');
                HTML.append('<td class="button_autorizar" style="text-align:left; width:30%; cursor:default;">');  
                    HTML.append('<select id="Cmb_Periodo_Nomina" style="width:100%;"/>');               
                HTML.append('</td>');
            HTML.append('</tr>');            
        HTML.append("</table>");       
        
        HTML.append("<table style='width:500px;border:outset 1px Silver;' border='0px'>");
            HTML.append('<tr style="border-style:outset;">');
                HTML.append('<td  class="button_autorizar" style="text-align:left; width:20%; cursor:default; font-size:11px;" colspan="2">'); 
                    HTML.append('Tipos Nomina');             
                HTML.append('</td>');
                HTML.append('<td style="text-align:left; width:80%; cursor:default;" colspan="2">');  
                    HTML.append('<select id="Cmb_Tipos_Nomina" style="width:100%;"/>');            
                HTML.append('</td>');
            HTML.append('</tr>');
            
           HTML.append('<tr style="border-style:outset;">');
                HTML.append('<td class="button_autorizar" style="text-align:left; width:20%; cursor:default; font-size:11px;" colspan="2">'); 
                    HTML.append('No Empleado');             
                HTML.append('</td>');
                HTML.append('<td style="text-align:left; width:80%; cursor:default;" colspan="2">');  
                    HTML.append('<input type="text" id="Txt_No_Empleado" style="width:98%;" class="texto"/>');            
                HTML.append('</td>');
            HTML.append('</tr>');
            
           HTML.append('<tr style="border-style:outset;">');
                HTML.append('<td class="button_autorizar" style="text-align:left; width:20%; cursor:default; font-size:11px;" colspan="2">'); 
                    HTML.append('RFC');             
                HTML.append('</td>');
                HTML.append('<td style="text-align:left; width:80%; cursor:default;" colspan="2">');  
                    HTML.append('<input type="text" id="Txt_RFC" style="width:98%;" class="texto"/>');            
                HTML.append('</td>');
            HTML.append('</tr>');
            
           HTML.append('<tr style="border-style:outset;">');
                HTML.append('<td class="button_autorizar" style="text-align:left; width:20%; cursor:default; font-size:11px;" colspan="2">'); 
                    HTML.append('CURP');             
                HTML.append('</td>');
                HTML.append('<td style="text-align:left; width:80%; cursor:default;" colspan="2">');  
                    HTML.append('<input type="text" id="Txt_CURP" style="width:98%;" class="texto"/>');            
                HTML.append('</td>');
            HTML.append('</tr>');
            
            HTML.append('<tr style="border-style:outset;">');
                HTML.append('<td class="button_autorizar" style="text-align:left; width:20%; cursor:default; font-size:11px;" colspan="2">'); 
                    HTML.append('Banco');             
                HTML.append('</td>');
                HTML.append('<td style="text-align:left; width:80%; cursor:default;" colspan="2">');  
                    HTML.append('<select id="Cmb_Banco" style="width:100%;"/>');            
                HTML.append('</td>');
            HTML.append('</tr>');
            
            HTML.append('<tr style="border-style:outset;">');
                HTML.append('<td class="button_autorizar" style="text-align:left; width:20%; cursor:default; font-size:11px;" colspan="2">'); 
                    HTML.append('U.Responsable');             
                HTML.append('</td>');
                HTML.append('<td style="text-align:left; width:80%; cursor:default;" colspan="2">');  
                    HTML.append('<select id="Cmb_UR" style="width:100%;"/>');            
                HTML.append('</td>');
            HTML.append('</tr>');
            
            HTML.append('<tr style="border-style:outset;">');
                HTML.append('<td style="text-align:left; width:20%; cursor:default;" colspan="4">'); 
                    HTML.append('<input type="button" id="Btn_Busqueda_Recibos_Nomina" value="Buscar Recibos Nomina" class="button_autorizar" style="background-image:url(../imagenes/paginas/busqueda.png); background-repeat: no-repeat; background-position:right;">');             
                HTML.append('</td>');
            HTML.append('</tr>');
        HTML.append("</table>");        
                
    HTML.append("</td>");
    HTML.append("</tr>");
    HTML.append("</table>");

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
        
    $('#Btn_Busqueda_Recibos_Nomina').live('click', function(e){
        LLenar_Grid_Recibos_Nomina();
//        $.modaldialog.hide();
        $('#Div_Busquedas').window('close');
    }); 
    
    return HTML.toString();
}

///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Calendario_Nomina
///DESCRIPCIÓN          : Funcion para llenar el combo de calendario de nomina
///PROPIEDADES          :
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : Noviembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function Llenar_Combo_Calendario_Nomina() {
    var Combo = $("#Cmb_Calendario_Nomina").get(0);
   
    $.ajax({
        url: "Frm_Ope_Nom_Controlador_Gen_Recibos.aspx?tabla=CALENDARIO&opcion=consultar_calendario_nomina",
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {             
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.CALENDARIO, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.ANIO, Elemento.NOMINA_ID);                             
                 });
             }
             else {
                 $.messager.alert("Mensaje","No hay calendario de nómina en el sistema en el sistema");
             }      
        }        
    });
    
    $("#Cmb_Calendario_Nomina").bind('change', function(){
         var Calendario_Seleccionado = $("#Cmb_Calendario_Nomina :selected").val();
         
         if(Calendario_Seleccionado != ""){
            Llenar_Combo_Periodos_Calendario_Nomina($("#Cmb_Calendario_Nomina :selected").val());
         }
    });
}

///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Periodos_Calendario_Nomina
///DESCRIPCIÓN          : Funcion para llenar el combo de periodos de calendario de nomina
///PROPIEDADES          1 nomina_id: año del que obtendremos los peridos
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : Noviembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function Llenar_Combo_Periodos_Calendario_Nomina(nomina_id) {
    var Combo = $("#Cmb_Periodo_Nomina").get(0);
   
    $.ajax({
        url: "Frm_Ope_Nom_Controlador_Gen_Recibos.aspx?tabla=PERIODOS&opcion=consultar_periodos_nominales&nomina_id=" + nomina_id,
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {             
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.PERIODOS, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.NO_NOMINA, Elemento.NO_NOMINA);                             
                 });
             }
             else {
                 $.messager.alert("Mensaje","No hay periodos de nomina para el calendario seleccionado.");
             }      
        }        
    });
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
        url: 'Frm_Ope_Nom_Controlador_Gen_Recibos.aspx?tabla=TIPOS_NOMINAS&opcion=consultar_tipos_nominas',
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
        url: 'Frm_Ope_Nom_Controlador_Gen_Recibos.aspx?tabla=UR&opcion=consultar_unidad_responsable',
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
///NOMBRE DE LA FUNCIÓN : LLenar_Grid_Recibos_Nomina
///DESCRIPCIÓN          : Funcion para llenar el grid de con los filtros de la busqueda
///PROPIEDADES          :
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : Noviembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
 function LLenar_Grid_Recibos_Nomina(){
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
    
    Parametros.append('nomina_id=');
    Parametros.append($('#_Nomina_ID').val());
    Parametros.append('&no_nomina=');
    Parametros.append($('#_No_Nomina').val());
    Parametros.append('&tipo_nomina_id=');
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
    Parametros.append('consultar_recibos_nomina');
    Parametros.append('&banco_id=');
    Parametros.append($('#_Banco_ID').val());
 
 //inicializamos el grid
    $('#tabla_recibo_nomina').datagrid({
        title:'RECIBOS EMPLEADOS',
        iconCls:'icon-save',
        width:830,
        height:400,
        collapsible:true,
        url:'Frm_Ope_Nom_Controlador_Gen_Recibos.aspx?' + Parametros.toString(),
        frozenColumns:[[
            {field:'NO_RECIBO', title: 'No Recibo', width:75, sortable:true , resizable:true,
                formatter:function(value){
                	return '<span style="font-size:x-small">'+value+'</span>';
                }
            },   
            {field:'EMPLEADO', title: 'Nombre', width:280, sortable: true , resizable:true,
                formatter:function(value){
                	return '<span style="font-size:x-small">'+value+'</span>';
                }
            },
            {field:'NO_NOMINA', title: 'Periodo', width:50, sortable: true , resizable:true,
                formatter:function(value){
                	return '<span style="font-size:x-small">'+value+'</span>';
                }
            }            
        ]],
        columns:
        [[
            {field:'UNIDAD_RESPONSABLE', title: 'UR', width:240, sortable: true , resizable:true,
                formatter:function(value){
                	return '<span style="font-size:x-small">'+value+'</span>';
                }
            },
            {field:'DIAS_TRABAJADOS', title: 'Dias', width:35, sortable: true , resizable:true,
                formatter:function(value){
                	return '<span style="font-size:x-small">'+value+'</span>';
                }
            },
            {field:'TOTAL_NOMINA_FOR', title: 'Total Nomina', width:110, sortable: true , resizable:true, align: 'right',
                formatter:function(value){
                	return '<span style="font-size:x-small;">'+value+'</span>';
                }
            },
            {field:'TOTAL_PERCEPCIONES_FOR', title: 'Total Percepciones', width:120, sortable: true , resizable:true, align: 'right',
                formatter:function(value){
                	return '<span style="font-size:x-small;">'+value+'</span>';
                }
            },
            {field:'TOTAL_DEDUCCIONES_FOR', title: 'Total Deducciones', width:120, sortable: true , resizable:true, align: 'right',
                formatter:function(value){
                	return '<span style="font-size:x-small;">'+value+'</span>';
                }
            }
        ]],
        pagination: true,
        rownumbers: false,
        idField: 'NO_RECIBO',
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
            LLenar_Grid_Percepciones_Recibo_Nomina(rowData.NO_RECIBO);
            LLenar_Grid_Deducciones_Recibo_Nomina(rowData.NO_RECIBO);
        }
    });
}

///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : LLenar_Grid_Percepciones_Recibo_Nomina
///DESCRIPCIÓN          : Funcion para llenar el grid de percepciones
///PROPIEDADES          1 No_Recibo: número del recibo que consultaremos las percepciones
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : Noviembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function LLenar_Grid_Percepciones_Recibo_Nomina(No_Recibo){
    var Parametros = new StringBuilder();

    Parametros.append('no_recibo=');
    Parametros.append(No_Recibo);
    Parametros.append('&tabla=');
    Parametros.append("rows");
    Parametros.append('&opcion=');
    Parametros.append('consultar_percepciones_recibo_nomina');
 
    $('#tabla_percepciones').datagrid({
        title:'PERCEPCIONES',
        iconCls:'icon-save',
        width:414,
        height:250,
        collapsible:true,
        url:'Frm_Ope_Nom_Controlador_Gen_Recibos.aspx?' + Parametros.toString(),
        columns:
        [[
            {field:'CLAVE', title: 'Clave', width:100, sortable:true , resizable:true,
                formatter:function(value){
                	return '<span style="font-size:x-small;">'+value+'</span>';
                }
            },    
            {field:'NOMBRE', title: 'Nombre', width:400, sortable: true , resizable:true,
                formatter:function(value){
                	return '<span style="font-size:x-small;">'+value+'</span>';
                }
            },
            {field:'MONTO_FOR', title: 'Cantidad', width:100, sortable: true , resizable:true, align: 'right',
                formatter:function(value){
                	return '<span style="font-size:x-small;">'+value+'</span>';
                }
            }
        ]],
        pagination: true,
        rownumbers: false,
        idField: 'CLAVE',
        sortOrder: 'asc',
        remoteSort: false,
        singleSelect: true,
        loadMsg: "Cargando, espere ...",
        pageNumber: 1,
        pageSize: 10,
        fitColumns: true,
        striped: true,
        nowrap: false,
        pageList: [10, 20, 50, 100],
        remoteSort: false,
        showFooter: true,
        rowStyler: function(rowIndex, rowData){  
            return 'background:Red;';  
        }   
    });
}

///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : LLenar_Grid_Deducciones_Recibo_Nomina
///DESCRIPCIÓN          : Funcion para llenar el grid de deducciones
///PROPIEDADES          1 No_Recibo: número del recibo del que consultaremos las deducciones
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : Noviembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function LLenar_Grid_Deducciones_Recibo_Nomina(No_Recibo){
    var Parametros = new StringBuilder();

    Parametros.append('no_recibo=');
    Parametros.append(No_Recibo);
    Parametros.append('&tabla=');
    Parametros.append("rows");
    Parametros.append('&opcion=');
    Parametros.append('consultar_deducciones_recibo_nomina');
 
    $('#tabla_deducciones').datagrid({
        title:'DEDUCCIONES',
        iconCls:'icon-save',
        width:414,
        height:250,
        collapsible:true,
        url:'Frm_Ope_Nom_Controlador_Gen_Recibos.aspx?' + Parametros.toString(),
        columns:
        [[
            {field:'CLAVE', title: 'Clave', width:100, sortable:true , resizable:true,
                formatter:function(value){
                	return '<span style="font-size:x-small;">'+value+'</span>';
                }
            },    
            {field:'NOMBRE', title: 'Nombre', width:400, sortable: true , resizable:true,
                formatter:function(value){
                	return '<span style="font-size:x-small;">'+value+'</span>';
                }
            },
            {field:'MONTO_FOR', title: 'Cantidad', width:100, sortable: true , resizable:true, align: 'right',
                formatter:function(value){
                	return '<span style="font-size:x-small;">'+value+'</span>';
                }
            }
        ]],
        pagination: true,
        rownumbers: false,
        idField: 'CLAVE',
        sortOrder: 'asc',
        remoteSort: false,
        singleSelect: true,
        loadMsg: "Cargando, espere ...",
        pageNumber: 1,
        pageSize: 10,
        fitColumns: true,
        striped: true,
        nowrap: false,
        pageList: [10, 20, 50, 100],
        remoteSort: false,
        showFooter: true,
        rowStyler: function(rowIndex, rowData){   
            $.messager.alert('Mensaje','');
            return 'background-color:pink;color:blue;font-weight:bold;';  
        }
    });
}

///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Generar_Recibo_Nomina
///DESCRIPCIÓN          : Funcion para Generar el diseño del recibo de nomina
///PROPIEDADES          :
///CREO                 : Leslie González Vázquez
///FECHA_CREO           : 08/Diciembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function Generar_Recibo_Nomina(){
   
    var Parametros = new StringBuilder();
    Parametros.append('nomina_id=');
    Parametros.append($('#_Nomina_ID').val());
    Parametros.append('&no_nomina=');
    Parametros.append($('#_No_Nomina').val());
    Parametros.append('&tipo_nomina_id=');
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
    Parametros.append('obtener_recibo_nomina');
    Parametros.append('&banco_id=');
    Parametros.append($('#_Banco_ID').val());

    var Div_Contenedor = new StringBuilder();
    try{
        Div_Contenedor.append('<html><head>');
        Div_Contenedor.append('<link href="../estilos/Impresion_Nomina.css" rel="stylesheet" type="text/css" />');
        Div_Contenedor.append('</head><body>');
         
        Controla_Visibilidad_ProgressBar(true);
        
        $.ajax({
        url: 'Frm_Ope_Nom_Controlador_Gen_Recibos.aspx?' + Parametros.toString(),
        type: 'POST',
        async: false,
        cache: false,
        dataType: 'json',
        success: function(Datos){
            if(Datos.total != 0)
            {
                 $.each(Datos.rows, function (i, item) {
                    if($('#_Tipo_Nomina').val() == '00003')
                    {
                         Div_Contenedor.append('<div class="Div_Recibo_Asimilable">');
                          Div_Contenedor.append('<center>');
                            Div_Contenedor.append('<div class="Div_Espaciador"></div>');
                            Div_Contenedor.append('<div class="Div_Fecha">');
                                Div_Contenedor.append('<label class="Lbl_Fecha">'+item.FECHA+'</label>');
                            Div_Contenedor.append('</div>');
                            Div_Contenedor.append('<div class="Div_Espaciador"></div>');
                            Div_Contenedor.append('<div class="Div_Texto">');
                                Div_Contenedor.append('RECIBÍ DEL MUNICIPIO DE IRAPUATO, GTO., CON R.F.C. MIG-850101-IZ2 Y CON DOMICILIO ');
                                Div_Contenedor.append('EN EL PALACIO MUNICIPAL S/N DE IRAPUATO, GTO., LA CANTIDAD DE ' + item.TOTAL_NOMINA_FOR + ' ');
                                Div_Contenedor.append(item.CANTIDAD_LETRA + '.<br />');
                                Div_Contenedor.append('DE HONORARIOS CON TRATAMIENTO SIMILAR A SALARIOS EN TÉRMINOS DEL ART. 110 FRACCIÓN V ');
                                Div_Contenedor.append('DE LA LEY DE IMPUESTO SOBRE LA RENTA. ');
                                Div_Contenedor.append('&nbsp;<br />');
                                Div_Contenedor.append('POR CONCEPTO DE HONORARIOS SIMILAR A SALARIOS CORRESPONDIENTES DEL ' + item.FECHA_INI + ' ');
                                Div_Contenedor.append('AL ' + item.FECHA_FINAL + '.<br />');
                            Div_Contenedor.append('</div>');
                            Div_Contenedor.append('<div class="Div_Percepcion_Deduccion">');
                                Div_Contenedor.append('<label class="Lbl_Percepcion">PERCEPCIONES</label>');
                                Div_Contenedor.append('<label class="Lbl_Deduccion">DEDUCCIONES</label>');
                                Div_Contenedor.append('<Hr />');
                                Div_Contenedor.append('<div style="clear:both;"></div>');
                                Div_Contenedor.append(Generar_Detalles(item.NO_RECIBO));
                                Div_Contenedor.append('<div class="Div_Percepciones">');
                                    Div_Contenedor.append('<div class="Div_ClavePer_ConPer">');
                                        Div_Contenedor.append('<div class="Div_ClavePer">');
                                            Div_Contenedor.append('<label class="Lbl_Clave_Percepcion">&nbsp;</label>');
                                        Div_Contenedor.append('</div>');
                                        Div_Contenedor.append('<div class="Div_ConPer">');
                                            Div_Contenedor.append('<label class="Lbl_Concepto_Percepcion">&nbsp;</label>');
                                        Div_Contenedor.append('</div>');
                                    Div_Contenedor.append('</div>');
                                    Div_Contenedor.append('<div class="Div_ImpPer_ClaveDed">');
                                        Div_Contenedor.append('<div class="Div_ImpPer">');
                                            Div_Contenedor.append('<label class="Lbl_Importe_Percepcion">----------</label>');
                                        Div_Contenedor.append('</div>');
                                        Div_Contenedor.append('<div class="Div_ClaveDed">');
                                            Div_Contenedor.append('<label class="Lbl_Clave_Deduccion">&nbsp;</label>');
                                        Div_Contenedor.append('</div>');
                                    Div_Contenedor.append('</div>');
                                Div_Contenedor.append('</div>');
                                Div_Contenedor.append('<div class="Div_Deducciones">');
                                        Div_Contenedor.append('<div class="Div_ConDed">');
                                            Div_Contenedor.append('<label class="Lbl_Concepto_Deduccion">&nbsp;</label>');
                                        Div_Contenedor.append('</div>');
                                        Div_Contenedor.append('<div class="Div_ImpDed">');
                                            Div_Contenedor.append('<label class="Lbl_Importe_Deduccion">----------</label>');
                                        Div_Contenedor.append('</div>');
                                Div_Contenedor.append('</div>');
                                
                                Div_Contenedor.append('<div class="Div_Percepciones">');
                                    Div_Contenedor.append('<div class="Div_ClavePer_ConPer">');
                                        Div_Contenedor.append('<div class="Div_ClavePer">');
                                            Div_Contenedor.append('<label class="Lbl_Clave_Percepcion">&nbsp;</label>');
                                        Div_Contenedor.append('</div>');
                                        Div_Contenedor.append('<div class="Div_ConPer">');
                                            Div_Contenedor.append('<label class="Lbl_Concepto_Percepcion">TOTAL PERCEPCIONES:</label>');
                                        Div_Contenedor.append('</div>');
                                    Div_Contenedor.append('</div>');
                                    Div_Contenedor.append('<div class="Div_ImpPer_ClaveDed">');
                                        Div_Contenedor.append('<div class="Div_ImpPer">');
                                            Div_Contenedor.append('<label class="Lbl_Importe_Percepcion">'+item.TOTAL_PERCEPCIONES_FOR+'&nbsp;</label>');
                                        Div_Contenedor.append('</div>');
                                        Div_Contenedor.append('<div class="Div_ClaveDed">');
                                            Div_Contenedor.append('<label class="Lbl_Clave_Deduccion">&nbsp;</label>');
                                        Div_Contenedor.append('</div>');
                                    Div_Contenedor.append('</div>');
                                Div_Contenedor.append('</div>');
                                Div_Contenedor.append('<div class="Div_Deducciones">');
                                    Div_Contenedor.append('<div class="Div_ConDed">');
                                        Div_Contenedor.append('<label class="Lbl_Concepto_Deduccion">TOTAL DEDUCCIONES:</label>');
                                    Div_Contenedor.append('</div>');
                                    Div_Contenedor.append('<div class="Div_ImpDed">');
                                        Div_Contenedor.append('<label class="Lbl_Importe_Deduccion">'+item.TOTAL_DEDUCCIONES_FOR+'&nbsp;</label>');
                                    Div_Contenedor.append('</div>');
                                Div_Contenedor.append('</div>');
                                Div_Contenedor.append('<div style="padding:0.3cm 0 0 0;"></div>');
                                Div_Contenedor.append('<div class="Div_Percepciones">');
                                    Div_Contenedor.append('<div class="Div_ClavePer_ConPer">');
                                        Div_Contenedor.append('<div class="Div_ClavePer">');
                                            Div_Contenedor.append('<label class="Lbl_Clave_Percepcion">&nbsp;</label>');
                                        Div_Contenedor.append('</div>');
                                        Div_Contenedor.append('<div class="Div_ConPer">');
                                            Div_Contenedor.append('<label class="Lbl_Concepto_Percepcion">&nbsp;</label>');
                                        Div_Contenedor.append('</div>');
                                    Div_Contenedor.append('</div>');
                                    Div_Contenedor.append('<div class="Div_ImpPer_ClaveDed">');
                                        Div_Contenedor.append('<div class="Div_ImpPer">');
                                            Div_Contenedor.append('<label class="Lbl_Importe_Percepcion">&nbsp;</label>');
                                        Div_Contenedor.append('</div>');
                                        Div_Contenedor.append('<div class="Div_ClaveDed">');
                                            Div_Contenedor.append('<label class="Lbl_Clave_Deduccion">&nbsp;</label>');
                                        Div_Contenedor.append('</div>');
                                    Div_Contenedor.append('</div>');
                                   Div_Contenedor.append('</div>');
                                   Div_Contenedor.append('<div class="Div_Deducciones">');
                                        Div_Contenedor.append('<div class="Div_ConDed">');
                                            Div_Contenedor.append('<label class="Lbl_Concepto_Deduccion">NETO A PAGAR:</label>');
                                        Div_Contenedor.append('</div>');
                                        Div_Contenedor.append('<div class="Div_ImpDed">');
                                            Div_Contenedor.append('<label class="Lbl_Importe_Deduccion">' + item.TOTAL_NOMINA_FOR + '&nbsp;</label>');
                                    Div_Contenedor.append('</div>');
                                Div_Contenedor.append('</div>');
                            Div_Contenedor.append('</div>');
                            Div_Contenedor.append('<div class="Div_Pie_Pagina">');
                                Div_Contenedor.append('<label class="Lbl_Recibi">RECIBI DE CONFORMIDAD</label><br />');
                                Div_Contenedor.append('<label class="Lbl_Recibi">&nbsp;</label><br />');
                                Div_Contenedor.append('<label class="Lbl_Recibi">&nbsp;</label><br />');
                                Div_Contenedor.append('<label class="Lbl_Recibi">____________________________________</label><br />');
                                Div_Contenedor.append('<div style="padding:0.3cm 0 0 0;"></div>');
                                Div_Contenedor.append('<label class="Lbl_Recibi">'+item.RFC+'</label><br />');
                                Div_Contenedor.append('<label class="Lbl_Recibi">'+item.EMPLEADO+'</label><br />');
                                Div_Contenedor.append('<label class="Lbl_Recibi">'+item.UNIDAD_RESPONSABLE+'</label><br />');
                            Div_Contenedor.append('</div>');
                            Div_Contenedor.append('<div class="Div_Hora">');
                               Div_Contenedor.append('<label class="Lbl_Hora">&nbsp;&nbsp;'+item.HORA+'</label>');
                            Div_Contenedor.append('</div>');
                          Div_Contenedor.append('</center>');
                         Div_Contenedor.append('</div>');
                         Div_Contenedor.append('<br class="Salto_Pagina">');
                    }
                    else
                    {
                        Div_Contenedor.append('<div class="Div_Recibo">');
                           Div_Contenedor.append('<div class="Div_Encabezado_Empl_Recibo"></div>');
                           Div_Contenedor.append('<div class="Div_Encabezado_No_Empl_Recibo">');
                             Div_Contenedor.append('<div class="Div_No_Empl_Recibo">');
                               Div_Contenedor.append('<div class="Div_No_Empl">');
                                Div_Contenedor.append(item.NO_EMPLEADO);
                               Div_Contenedor.append('</div>');
                               Div_Contenedor.append('<div class="Div_No_Recibo">');
                                Div_Contenedor.append(item.NO_RECIBO_FOR);
                               Div_Contenedor.append('</div>');
                             Div_Contenedor.append('</div>');
                           Div_Contenedor.append('</div>');
                           
                           Div_Contenedor.append('<div class="Div_Encabezados"></div>');
                           Div_Contenedor.append('<div class="Div_Encabezado_RFC_Nom_Per_Cat">');
                             Div_Contenedor.append('<div class="Div_RFC_Nom">');
                                Div_Contenedor.append('<div class="Div_RFC">');
                                  Div_Contenedor.append('<label class="Lbl_RFC">'+item.RFC+'</label>');
                                Div_Contenedor.append('</div>');
                                Div_Contenedor.append('<div class="Div_Nom">');
                                  Div_Contenedor.append('<label class="Lbl_Nombre_Empleado">'+item.EMPLEADO+'</label>');
                                Div_Contenedor.append('</div>');
                             Div_Contenedor.append('</div>');
                             Div_Contenedor.append('<div class="Div_Per_Cat">');
                                Div_Contenedor.append('<div class="Div_Per">');
                                  Div_Contenedor.append('<label class="Lbl_Periodo">'+item.FECHA_FIN_FOR+'</label>');
                                Div_Contenedor.append('</div>');
                                Div_Contenedor.append('<div class="Div_Cat">');
                                  Div_Contenedor.append('<label class="Lbl_Categoria">'+item.PUESTO+'</label>');
                                Div_Contenedor.append('</div>');
                             Div_Contenedor.append('</div>');
                           Div_Contenedor.append('</div>');
                           
                           Div_Contenedor.append('<div class="Div_Encabezados"></div>');
                           Div_Contenedor.append('<div class="Div_Encabezado_CURP_Dep_Afi_Cod_Dias">');
                            Div_Contenedor.append('<div class="Div_CURP_Dep">');
                              Div_Contenedor.append('<div class="Div_CURP">');
                                Div_Contenedor.append('<label class="Lbl_CURP">'+item.CURP+'</label>');
                              Div_Contenedor.append('</div>');
                              Div_Contenedor.append('<div class="Div_Dep">');
                                Div_Contenedor.append('<label class="Lbl_Departamento">'+item.UNIDAD_RESPONSABLE+'</label>');
                              Div_Contenedor.append('</div>');
                            Div_Contenedor.append('</div>');
                            Div_Contenedor.append('<div class="Div_Afi_Cod_Dias">');
                              Div_Contenedor.append('<div class="Div_Afi">');
                                Div_Contenedor.append('<label class="Lbl_No_Afiliacion">'+item.NO_IMSS+'</label>');
                              Div_Contenedor.append('</div>');
                              Div_Contenedor.append('<div class="Div_Cod_Dias">');
                                 Div_Contenedor.append('<div class="Div_Cod">');
                                    Div_Contenedor.append('<label class="Lbl_Codigo_Programatico">'+item.CODIGO_PROGRAMATICO+'</label>');
                                 Div_Contenedor.append('</div>');
                                 Div_Contenedor.append('<div class="Div_Dias">');
                                    Div_Contenedor.append('<label class="Lbl_Dias">'+item.DIAS_TRABAJADOS+'</label>');
                                  Div_Contenedor.append('</div>');
                              Div_Contenedor.append('</div>');
                            Div_Contenedor.append('</div>');
                           Div_Contenedor.append('</div>');
                           
                           Div_Contenedor.append('<div class="Div_Encabezados"></div>');
                           Div_Contenedor.append('<div class="Div_Encabezado_Detalles"></div>');
                           Div_Contenedor.append('<div class="Div_Detalles">');
                             Div_Contenedor.append(Generar_Detalles(item.NO_RECIBO));
                           Div_Contenedor.append('</div>');
                          
                          Div_Contenedor.append('<div class="Div_Totales">');
                             Div_Contenedor.append('<div class="Div_Tot_Percepciones">');
                                Div_Contenedor.append('<div class="Div_TotPer">');
                                  Div_Contenedor.append('<label class="Lbl_Total_Percepciones">'+item.TOTAL_PERCEPCIONES_FOR+'&nbsp;</label>');
                                Div_Contenedor.append('</div>');
                             Div_Contenedor.append('</div>');
                             Div_Contenedor.append('<div class="Div_Tot_Deducciones">');
                                Div_Contenedor.append('<div class="Div_TotDed">');
                                  Div_Contenedor.append('<label class="Lbl_Total_Deducciones">'+item.TOTAL_DEDUCCIONES_FOR+'&nbsp;</label>');
                                Div_Contenedor.append('</div>');
                             Div_Contenedor.append('</div>');
                          Div_Contenedor.append('</div>');
                          
                          Div_Contenedor.append('<div class="Div_Total_Neto">');
                             Div_Contenedor.append('<div class="Div_Total">');
                               Div_Contenedor.append('<label class="Lbl_Total_Neto">'+item.TOTAL_NOMINA_FOR+'&nbsp;</label>');
                             Div_Contenedor.append('</div>');
                          Div_Contenedor.append('</div>');
                          
                         Div_Contenedor.append('<div class="Div_Banco">');
                           Div_Contenedor.append('<label class="Lbl_Banco">&nbsp; BANCO '+item.BANCO+'</label>');
                         Div_Contenedor.append('</div>');
                         Div_Contenedor.append('<div class="Div_Hora">');
                           Div_Contenedor.append('<label class="Lbl_Hora">&nbsp;&nbsp;'+item.HORA+'</label>');
                         Div_Contenedor.append('</div>');
                        Div_Contenedor.append('</div>');
                    }
                 });
            }
            else {
                $.messager.alert("Mensaje","No hay datos de recibos por imprimir.");
            } 
          }
        });
         
        setTimeout("Actualizar_Barra_Progreso_Eventos(100)", 250);
        Div_Contenedor.append('</body></html>');
        Div_Contenedor.toString();
        printme(Div_Contenedor);
        
    }catch(e){
        $.messager.alert('Mensaje','Error al generar el recibo de nomina. Error: [' + e + "]");
    }
}

///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : printme
///DESCRIPCIÓN          : Funcion para llenar el grid de deducciones
///PROPIEDADES          1 No_Recibo: número del recibo del que consultaremos las deducciones
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : Noviembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function printme(div_imprimir){
    var winPrint = window.open();

    winPrint.document.write(div_imprimir);
    winPrint.document.close();
    winPrint.focus();
    winPrint.print();
    winPrint.close();

}

///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Generar_Detalles
///DESCRIPCIÓN          : Funcion para obtener los detalles del recibo
///PROPIEDADES          1 No_Recibo: numero del recibo que generaremos los detalles
///CREO                 : Leslie Gonzalez Vazquez
///FECHA_CREO           : 09/Diciembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function Generar_Detalles(No_Recibo)
{
   var Div_Detalles = new StringBuilder();
   var Parametros = new StringBuilder();
    
   Parametros.append('no_recibo=');
   Parametros.append(No_Recibo);
   Parametros.append('&tabla=');
   Parametros.append("rows");
   Parametros.append('&opcion=');
   Parametros.append('obtener_detalles_recibo_nomina');
    
   $.ajax({
   url: 'Frm_Ope_Nom_Controlador_Gen_Recibos.aspx?' + Parametros.toString(),
   type: 'POST',
   async: false,
   cache: false,
   dataType: 'json',
   success: function(Datos){
      if(Datos.total != 0)
      {
         $.each(Datos.rows, function (i, item) {
           Div_Detalles.append('<div class="Div_Percepciones">');
            Div_Detalles.append('<div class="Div_ClavePer_ConPer">');
                Div_Detalles.append('<div class="Div_ClavePer">');
                    Div_Detalles.append(item.Clave_Percepcion.replace('-',''));
                Div_Detalles.append('</div>');
                Div_Detalles.append('<div class="Div_ConPer">');
                    Div_Detalles.append(item.Concepto_Percepcion);
                Div_Detalles.append('</div>');
            Div_Detalles.append('</div>');
            Div_Detalles.append('<div class="Div_ImpPer_ClaveDed">');
                Div_Detalles.append('<div class="Div_ImpPer">');
                    Div_Detalles.append(item.Importe_Percepcion+'&nbsp;');
                Div_Detalles.append('</div>');
                Div_Detalles.append('<div class="Div_ClaveDed">');
                    Div_Detalles.append(item.Clave_Deduccion.replace('-',''));
                Div_Detalles.append('</div>');
            Div_Detalles.append('</div>');
           Div_Detalles.append('</div>');
           Div_Detalles.append('<div class="Div_Deducciones">');
                Div_Detalles.append('<div class="Div_ConDed">');
                    Div_Detalles.append(item.Concepto_Deduccion);
                Div_Detalles.append('</div>');
                Div_Detalles.append('<div class="Div_ImpDed">');
                    Div_Detalles.append(item.Importe_Deduccion+'&nbsp;');
                Div_Detalles.append('</div>');
           Div_Detalles.append('</div>');
           
//           Div_Detalles.append('<div class="Div_Division_Detalles"></div>');
         });
      }
    }
   });
   return Div_Detalles.toString();
}

///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Consultar_Bancos
///DESCRIPCIÓN          : Funcion para consultar los bancos y llenar el combo
///PROPIEDADES          :
///CREO                 : Leslie Gonzalez Vazquez
///FECHA_CREO           : 12/Diciembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function Consultar_Bancos(){
    var Combo = $('#Cmb_Banco').get(0);
    
    $.ajax({
        url: 'Frm_Ope_Nom_Controlador_Gen_Recibos.aspx?tabla=Banco&opcion=consultar_tipos_banco',
        type: 'POST',
        async: false,
        cache: false,
        dataType: 'json',
        success: function(Datos){
            if(Datos != null){
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.Banco, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.NOMBRE, Elemento.BANCO_ID);                             
                 });
             }
             else {
                 $.messager.alert("Mensaje","No hay bancos registrados en el sistema.");
             } 
        }
    });
}

///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Limpiar_Variables
///DESCRIPCIÓN          : Funcion para limpiar las variables de la pagina
///PROPIEDADES          :
///CREO                 : Leslie Gonzalez Vazquez
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
}

///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Validar_Datos
///DESCRIPCIÓN          : Funcion para validar los datos de la impresion de recibos
///PROPIEDADES          :
///CREO                 : Leslie Gonzalez Vazquez
///FECHA_CREO           : 12/Diciembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function Validar_Datos(){
    try
    {
        var Datos_Validos = true;
        var Mensaje = '';
        if($('#_Nomina_ID').val() == '')
        {
            Datos_Validos = false;
            Mensaje += "* Seleccionar la nomina <br />";
        }
//        if($('#_Tipo_Nomina').val() == '')
//        {
//            Datos_Validos = false;
//            Mensaje += "* Seleccionar el tipo de nomina <br />"
//        }
        if($('#_Banco_ID').val() == '')
        {
            Datos_Validos = false;
            Mensaje += "* Seleccionar el banco <br />"
        }
        if($('#_No_Nomina').val() == '')
        {
            Datos_Validos = false;
            Mensaje += "* Seleccionar el periodo <br />"
        }
        
        if(Mensaje != ''){
            Lauch_Panel_Busqueda();
            Llenar_Combo_Calendario_Nomina();
            Consultar_Tipos_Nomina();
            Consultar_Unidades_Responsables();
            Consultar_Bancos();
            $.messager.alert('Mensaje','Favor de: <br /> ' + Mensaje);
        }
        
        return Datos_Validos;
    }catch(Ex)
    {
         $.messager.alert('Mensaje','Error al validar los datos de la página. Error: [' + Ex + ']');
    }
}


///********************************************************************************************
///NOMBRE: Actualizar_Barra_Progreso_Eventos
///
///DESCRIPCION: Función que se llama para actualizar la barra de progreso.
///
///PARÁMETROS: valor.- Nuevo valor con el que se actualizara la barra de progreso.
/// 
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 12/Mayo/2011 10:15 a.m.
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
function Actualizar_Barra_Progreso_Eventos(valor){
    var Nuevo_Valor = valor + 1;
    
    try{
        if(Nuevo_Valor <= 100){    
             $("#Barra_Progreso").progressBar(Nuevo_Valor);//Establecemos el enuevo valor de la barra de progreso.
             /**
             * LLamamos a el método que que actualiza la barra de progreso y pasamos como parámetro el nuevo valor 
             * valor de la barra de progreso         
             **/     
             setTimeout("Actualizar_Barra_Progreso_Eventos(" + Nuevo_Valor + ")", 15);
         }else{         
             Controla_Visibilidad_ProgressBar(false);
             $("#Barra_Progreso").progressBar(0);//Asignamos a la barra de progreso un valor de cero. 
         }
     }catch(e){
        alert('Error al actualizar la barra de progreso. Error: [' + e + ']');
     }
} 
///********************************************************************************************
///NOMBRE: Controla_Visibilidad_ProgressBar
///
///DESCRIPCION: Controla si se muestran la barra en la página.
///
///PARÁMETROS: No Áplica.
/// 
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 12/Mayo/2011 10:15 a.m.
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
function Controla_Visibilidad_ProgressBar(Mostrar){
    try{
        if(Mostrar){
            $("#Barra_Progreso").css("display", "block");//Mostramos la capa que bloqueara la pantalla.
            $("#progressBackgroundFilter").css("display", "block");//Mostramos la barra de progreso.    
        }else{
            $("#Barra_Progreso").css("display", "none");//Ocultamos el div que bloquea la pantalla.
            $("#progressBackgroundFilter").css("display", "none");//Ocultamos el div que muestra la barra de progreso.    
        }
    }catch(e){
        alert('Error al mostrar o ocultar la barra de progreso. Error: [' + e + ']');
    }
}
///***************************************************************************************************************************************
///********************************************************* INICIO **********************************************************************
///***************************************************************************************************************************************
var DOM_Pagina = $(document);
DOM_Pagina.ready(function(){
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
///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Configuracion_Inicial
///DESCRIPCIÓN          : Funcion de la configuración inicial de los eventos de los botones de la pagina
///PROPIEDADES          :
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : Marzo/2012
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function Configuracion_Inicial(){

   Llenar_Combo_Cuentas_Contables();
   Llenar_Combo_Conceptos();
   LLenar_Tabla();
   
   $('input[id$=Txt_Busqueda]').bind('blur', function(){
        $.ajax({
            url: "Frm_Cat_Nom_Perc_Dedu_CC_Deta_Ctrl.aspx?Opcion=buscar&percepcion_deduccion_id=" + $(this).val(),
            type:'POST',
            async: false,
            cache: false,
            dataType:'text',
            success: function (Clave) {                 
               $('select[id$=Cmb_Percepcion_Deduccion]').val(Clave);    
               LLenar_Tabla();   
            }        
        });
   });
   $('input[id$=Txt_Cuenta_Contable]').bind('blur', function(){
        $.ajax({
            url: "Frm_Cat_Nom_Perc_Dedu_CC_Deta_Ctrl.aspx?Opcion=buscar_cuenta&cuenta_contable_id=" + $(this).val(),
            type:'POST',
            async: false,
            cache: false,
            dataType:'text',
            success: function (Clave) {                 
               $('select[id$=Cmb_Cuenta_Contable]').val(Clave);    
               LLenar_Tabla();   
            }        
        });
   });   
}
///********************************************************************************************
///NOMBRE: isNumber
///
///DESCRIPCION: Valida que el parámetro sea un dato numerico. 
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: Marzo/2012
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///******************************************************************************************** 
function isNumber(n) {   
    return !isNaN(parseFloat(n)) && isFinite(n); 
}  
///********************************************************************************************
///NOMBRE: Llenar_Combo_Cuentas_Contables
///
///DESCRIPCION: Consulta las cuentas contables en el sistema.
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 16/Mayo/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************  
function Llenar_Combo_Cuentas_Contables() {
    var Combo = $("select[id$=Cmb_Cuenta_Contable]").get(0);
   
    $.ajax({
        url: "Frm_Cat_Nom_Perc_Dedu_CC_Deta_Ctrl.aspx?Tabla=CUENTAS_CONTABLES&Opcion=cmb_cuentas_contables",
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {             
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.CUENTAS_CONTABLES, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.CUENTA, Elemento.CUENTA_CONTABLE_ID);                             
                 });
             }
             else {
                 alert("No hay cuentas contables en el sistema en el sistema");
             }      
        }        
    });
}
///********************************************************************************************
///NOMBRE: Llenar_Combo_Conceptos
///
///DESCRIPCION: Consulta los conceptos en el sistema.
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 16/Mayo/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************  
function Llenar_Combo_Conceptos() {
    var Combo = $("select[id$=Cmb_Percepcion_Deduccion]").get(0);
   
    $.ajax({
        url: "Frm_Cat_Nom_Perc_Dedu_CC_Deta_Ctrl.aspx?Tabla=CONCEPTOS&Opcion=cmb_percepciones_deducciones",
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {             
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.CONCEPTOS, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.CONCEPTO, Elemento.PERCEPCION_DEDUCCION_ID);                             
                 });
             }
             else {
                 alert("No hay cuentas conceptos en el sistema en el sistema");
             }      
        }        
    });
}
///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Agregar
///DESCRIPCIÓN          : Funcion agregar un registro.
///PROPIEDADES          :
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : Marzo/2012 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function Agregar(){
    var Parametros = new StringBuilder();
    
    Parametros.append('percepcion_deduccion_id=');
    Parametros.append($('select[id$=Cmb_Percepcion_Deduccion] :selected').val());
    Parametros.append('&cuenta_contable_id=');
    Parametros.append($('select[id$=Cmb_Cuenta_Contable] :selected').val());
    Parametros.append('&opcion=');
    Parametros.append('agregar');
    
    $.ajax({
        url: "Frm_Cat_Nom_Perc_Dedu_CC_Deta_Ctrl.aspx?" + Parametros.toString(),
        type:'POST',
        async: false,
        cache: false,
        dataType:'text',
        success: function (RESPUESTA) {}        
    });
}
///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Eliminar
///DESCRIPCIÓN          : Funcion eliminar un registro.
///PROPIEDADES          :
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : Marzo/2012
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function Eliminar(Percepcion_Deduccion_ID, Cuenta_Contable_ID){
    var Parametros = new StringBuilder();
    
    Parametros.append('percepcion_deduccion_id=');
    Parametros.append(Percepcion_Deduccion_ID);
    Parametros.append('&cuenta_contable_id=');
    Parametros.append(Cuenta_Contable_ID);
    Parametros.append('&opcion=');
    Parametros.append('eliminar');
    
    $.ajax({
        url: "Frm_Cat_Nom_Perc_Dedu_CC_Deta_Ctrl.aspx?" + Parametros.toString(),
        type:'POST',
        async: false,
        cache: false,
        dataType:'text',
        success: function (RESPUESTA) {}        
    });
}
///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : LLenar_Tabla
///DESCRIPCIÓN          : Funcion para llenar el grid.
///PROPIEDADES          :
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : Marzo/2012
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
 function LLenar_Tabla(){ 
    var Parametros = new StringBuilder();
    
    Parametros.append('percepcion_deduccion_id=');
    Parametros.append($('select[id$=Cmb_Percepcion_Deduccion] :selected').val());
    Parametros.append('&cuenta_contable_id=');
    Parametros.append($('select[id$=Cmb_Cuenta_Contable] :selected').val());

    Parametros.append('&tabla=');
    Parametros.append("rows");
    Parametros.append('&opcion=');
    Parametros.append('consultar_conceptos_x_cuenta_contable');
 
    //inicializamos el grid
    $('#Tbl_Detalle_Percepciones_Deducciones_CC').datagrid({
        title:'Relación Conceptos - Cuentas Contables',
        iconCls:'icon-save',
        width:800,
        height:400,
        collapsible:true,
        url:'Frm_Cat_Nom_Perc_Dedu_CC_Deta_Ctrl.aspx?' + Parametros.toString(),
        toolbar: [{
            text: 'Eliminar',
            iconCls: 'icon-remove',
            handler: function () {
                var row = $('#Tbl_Detalle_Percepciones_Deducciones_CC').datagrid('getSelected');
                if (row) {
                    $.messager.confirm('Confirmar', 'Desea eliminar el registro?', function (r) {
                        if (r) {
                            var percepcion_Deduccion_ID = row.PERCEPCION_DEDUCCION_ID;
                            var cuenta_contable_Id = row.CUENTA_CONTABLE_ID;
                            Eliminar(percepcion_Deduccion_ID, cuenta_contable_Id);
                            LLenar_Tabla();
                        }
                    });
                }
            }
        },
        {
            text: 'Agregar',
            iconCls: 'icon-add',
            handler: function () {
                $.messager.confirm('Confirmar', 'Desea agregar el registro?', function (r) {
                    if (r) {
                        Agregar();
                        LLenar_Tabla();
                    }
                });                
            }
        }],                  
        columns:
        [[
            {field:'PERCEPCION_DEDUCCION_ID', title: '', hidden:true, width:0, sortable: true , resizable:true,
                formatter:function(value){
                	return '<span style="font-size:x-small">'+value+'</span>';
                }
            },
            {field:'CUENTA_CONTABLE_ID', title: '', hidden:true, width:0, sortable: true , resizable:true,
                formatter:function(value){
                	return '<span style="font-size:x-small">'+value+'</span>';
                }
            }, 
            {field:'CONCEPTO', title: '', width:0, sortable: true , resizable:true,
                formatter:function(value){
                	return '<span style="font-size:x-small">'+value+'</span>';
                }
            }, 
            {field:'CUENTA', title: '', width:0, sortable: true , resizable:true,
                formatter:function(value){
                	return '<span style="font-size:x-small">'+value+'</span>';
                }
            }                        
        ]],
        pagination: true,
        rownumbers: false,
        idField: 'PERCEPCION_DEDUCCION_ID',
        sortOrder: 'asc',
        remoteSort: false,
        singleSelect: true,
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
        }
    });    
}
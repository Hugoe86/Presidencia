/// <reference path="../easyui/jquery-1.4.2.js" />

var Pagina;
Pagina=$(document);
Pagina.ready(Page_Load_Catalogo_Empleados);

///********************************************************************************************
///NOMBRE: Page_Load_Catalogo_Empleados
///
///DESCRIPCION: Carga la configuracion inicial de los controles de la pagina.
///
///PARÁMETROS: No Áplica
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 13/Junio/2011 09:27 a.m.
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
function Page_Load_Catalogo_Empleados() {
    Configuracion_Inicial_Pagina();
}
///********************************************************************************************
///NOMBRE: Configuracion_Inicial_Pagina
///
///DESCRIPCION: Método que habilita la configuración inicial de la página.
///
///PARÁMETROS: No Áplica
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 16/Mayo/2011 10:15 a.m.
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
function Configuracion_Inicial_Pagina(){
    try{
    
        Crear_Calendario();
        Llenar_Combo_Roles();
        Llenar_Combo_Tipo_Contratos();
        Llenar_Combo_Escolaridad();
        Llenar_Combo_Sindicatos();
        Llenar_Combo_Turnos();
        Llenar_Combo_Tipo_Trabajador();
        Llenar_Combo_Zona_Economica(); 
    
        //Establece la configuración inical de la barra de progreso. 
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
     }catch(Ex){
        alert('Error generado al cargar la configuración inicial de la pagina. Error: [' + + ']');
     } 
}
///********************************************************************************************************
///NOMBRE: Crear_Calendario
///
///DESCRIPCION: Método que crea los calendarios para la fecha de inicio y fecha fin de la búsqueda.
///
///PARÁMETROS: No Áplica.
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 16/Mayo/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///*********************************************************************************************************
function Crear_Calendario(){
    try{
        //Prepara el DatePicker para seleccionar la fecha
        $("#Txt_Fecha_Inicio").daterangepicker();
        $("#Txt_Fecha_Inicio").daterangepicker({arrows:true}); 
    }catch(e){
        alert('Error al crear los calendarios de la página. Error: [' + e + ']');
    }
}
///********************************************************************************************
///NOMBRE: Llenar_Combo_Empleados
///
///DESCRIPCION: Consulta los empleados que pertenecen a la unidad responsable seleccionada.
///
///PARÁMETROS: Unidad_Responsable_ID.- Identificador único de la unidad responsable.
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 16/Mayo/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************  
function Llenar_Combo_Roles() {
    var Combo = $("select[id$=Cmb_Rol_Empleado]").get(0);
   
    $.ajax({
        url: "Frm_Cat_Empleados_Controlador.aspx?Tabla=ROLES&Opcion=cmb_roles",
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {             
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.ROLES, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.NOMBRE, Elemento.ROL_ID);                             
                 });
             }
             else {
                 alert("No hay roles en el sistema");
             }      
        }        
    });
}
///********************************************************************************************
///NOMBRE: Llenar_Combo_Empleados
///
///DESCRIPCION: Consulta los empleados que pertenecen a la unidad responsable seleccionada.
///
///PARÁMETROS: Unidad_Responsable_ID.- Identificador único de la unidad responsable.
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 16/Mayo/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************  
function Llenar_Combo_Tipo_Contratos() {
    var Combo = $("select[id$=Cmb_Tipo_Contratos]").get(0);
   
    $.ajax({
        url: "Frm_Cat_Empleados_Controlador.aspx?Tabla=TIPO_CONTRATOS&Opcion=cmb_tipo_contratos",
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {             
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.TIPO_CONTRATOS, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.DESCRIPCION, Elemento.TIPO_CONTRATO_ID);                             
                 });
             }
             else {
                 alert("No hay roles en el sistema");
             }      
        }        
    });
}
///********************************************************************************************
///NOMBRE: Llenar_Combo_Empleados
///
///DESCRIPCION: Consulta los empleados que pertenecen a la unidad responsable seleccionada.
///
///PARÁMETROS: Unidad_Responsable_ID.- Identificador único de la unidad responsable.
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 16/Mayo/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************  
function Llenar_Combo_Escolaridad() {
    var Combo = $("select[id$=Cmb_Escolaridad]").get(0);
   
    $.ajax({
        url: "Frm_Cat_Empleados_Controlador.aspx?Tabla=ESCOLARIDAD&Opcion=cmb_escolaridad",
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {             
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.ESCOLARIDAD, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.ESCOLARIDAD, Elemento.ESCOLARIDAD_ID);                             
                 });
             }
             else {
                 alert("No hay roles en el sistema");
             }      
        }        
    });
}
///********************************************************************************************
///NOMBRE: Llenar_Combo_Empleados
///
///DESCRIPCION: Consulta los empleados que pertenecen a la unidad responsable seleccionada.
///
///PARÁMETROS: Unidad_Responsable_ID.- Identificador único de la unidad responsable.
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 16/Mayo/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************  
function Llenar_Combo_Sindicatos() {
    var Combo = $("select[id$=Cmb_Sindicato]").get(0);
   
    $.ajax({
        url: "Frm_Cat_Empleados_Controlador.aspx?Tabla=SINDICATOS&Opcion=cmb_sindicatos",
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {             
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.SINDICATOS, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.NOMBRE, Elemento.SINDICATO_ID);                             
                 });
             }
             else {
                 alert("No hay roles en el sistema");
             }      
        }        
    });
}
///********************************************************************************************
///NOMBRE: Llenar_Combo_Empleados
///
///DESCRIPCION: Consulta los empleados que pertenecen a la unidad responsable seleccionada.
///
///PARÁMETROS: Unidad_Responsable_ID.- Identificador único de la unidad responsable.
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 16/Mayo/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************  
function Llenar_Combo_Turnos() {
    var Combo = $("select[id$=Cmb_Turno]").get(0);
   
    $.ajax({
        url: "Frm_Cat_Empleados_Controlador.aspx?Tabla=TURNOS&Opcion=cmb_turnos",
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {             
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.TURNOS, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.DESCRIPCION, Elemento.TURNO_ID);                             
                 });
             }
             else {
                 alert("No hay roles en el sistema");
             }      
        }        
    });
}
///********************************************************************************************
///NOMBRE: Llenar_Combo_Empleados
///
///DESCRIPCION: Consulta los empleados que pertenecen a la unidad responsable seleccionada.
///
///PARÁMETROS: Unidad_Responsable_ID.- Identificador único de la unidad responsable.
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 16/Mayo/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************  
function Llenar_Combo_Tipo_Trabajador() {
    var Combo = $("select[id$=Cmb_Tipo_Trabajador]").get(0);
   
    $.ajax({
        url: "Frm_Cat_Empleados_Controlador.aspx?Tabla=TIPO_TRABAJADOR&Opcion=cmb_tipo_trabajador",
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {             
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.TIPO_TRABAJADOR, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.DESCRIPCION, Elemento.TIPO_TRABAJADOR_ID);                             
                 });
             }
             else {
                 alert("No hay roles en el sistema");
             }      
        }        
    });
}
///********************************************************************************************
///NOMBRE: Llenar_Combo_Empleados
///
///DESCRIPCION: Consulta los empleados que pertenecen a la unidad responsable seleccionada.
///
///PARÁMETROS: Unidad_Responsable_ID.- Identificador único de la unidad responsable.
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 16/Mayo/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************  
function Llenar_Combo_Zona_Economica() {
    var Combo = $("select[id$=Cmb_Zona_Economica]").get(0);
   
    $.ajax({
        url: "Frm_Cat_Empleados_Controlador.aspx?Tabla=ZONA_ECONOMICA&Opcion=cmb_zona_economica",
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {             
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.ZONA_ECONOMICA, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.ZONA_ECONOMICA, Elemento.ZONA_ID);                             
                 });
             }
             else {
                 alert("No hay roles en el sistema");
             }      
        }        
    });
}





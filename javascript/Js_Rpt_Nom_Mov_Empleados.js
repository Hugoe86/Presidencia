var Pagina;
Pagina=$(document);
Pagina.ready(Inicializar_Eventos_Pag_Rpt_Mov_Empleados);

///********************************************************************************************
///NOMBRE: Inicializar_Eventos_JQUERY
///
///DESCRIPCION: Carga la configuracion inicial de los controles de la pagina.
///
///PARÁMETROS: No Áplica
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 16/Mayo/2011 10:15 a.m.
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
function Inicializar_Eventos_Pag_Rpt_Mov_Empleados(){
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
        Llenar_Combo_Tipos_Nominas();     //Cargamos el combo de tipos de nóminas. 
        Llenar_Combo_Unidades_Responsables(); 
        Eventos_Pagina();//Inicializamos los eventos de la página.
        Crear_Calendario();
        
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
///********************************************************************************************
///NOMBRE: Llenar_Combo_Tipos_Nominas
///
///DESCRIPCION: Consulta los Tipos de Nominas activas en el sistema y las  carga en el 
///             combo.
///
///PARÁMETROS: No Áplica
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 16/Mayo/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
function Llenar_Combo_Tipos_Nominas() {
    var Combo = $("select[id$=Cmb_Tipos_Nominas]").get(0);
   
    $.ajax({
        url: "Frm_Rpt_Nom_Controlador_Mov_Empl.aspx?Tabla=TIPO_NOMINAS&Opcion=1",
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {             
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.TIPO_NOMINAS, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.NOMINA, Elemento.TIPO_NOMINA_ID);                             
                 });
             }
             else {
                 alert("No hay Tipos Nomina");
             }      
        }        
    });
 }
///********************************************************************************************
///NOMBRE: Llenar_Combo_Unidades_Responsables
///
///DESCRIPCION: Consulta las unidades responsables registradas en el sistema.
///
///PARÁMETROS: No Áplica
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 16/Mayo/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///******************************************************************************************** 
 function Llenar_Combo_Unidades_Responsables() {
    var Combo = $("select[id$=Cmb_Unidad_Responsable]").get(0);
   
    $.ajax({
        url: "Frm_Rpt_Nom_Controlador_Mov_Empl.aspx?Tabla=UNIDADES_RESPONSABLES&Opcion=2",
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {             
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.UNIDADES_RESPONSABLES, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.NOMBRE, Elemento.DEPENDENCIA_ID);                             
                 });
             }
             else {
                 alert("No hay Tipos Nomina");
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
function Llenar_Combo_Empleados(Unidad_Responsable_ID) {
    var Combo = $("select[id$=Cmb_Empleados]").get(0);
   
    $.ajax({
        url: "Frm_Rpt_Nom_Controlador_Mov_Empl.aspx?Tabla=EMPLEADOS&Unidad_Responsable=" + Unidad_Responsable_ID + "&Opcion=3",
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {             
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.EMPLEADOS, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.EMPLEADO, Elemento.EMPLEADO_ID);                             
                 });
             }
             else {
                 alert("No hay Tipos Nomina");
             }      
        }        
    });
}
///********************************************************************************************
///NOMBRE: Eventos_Pagina
///
///DESCRIPCION: Método que contiene los escuchadores de los eventos que se generan en la página.
///
///PARÁMETROS: No Áplica.
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 16/Mayo/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///******************************************************************************************** 
function Eventos_Pagina(){
    try{        
        /**
        * Evento generado al seleccionar un elemento del calendario de nómina.
        **/    
        $("select[id$=Cmb_Unidad_Responsable]").bind("change keyup", function() { 
            Controla_Visibilidad_ProgressBar(true);
            setTimeout("Actualizar_Barra_Progreso_Eventos(0)", 250);            
        });            

        /**
        * Evento generado al pulsar el botón de Generar Reporte.
        **/
        $('input[id$=Btn_Generar_Reporte]').click(Btn_Generar_Reporte_Click);                
    }catch(Ex){
        alert('Error al ejecutar los eventos de la página. Error: [' + Ex + ']');
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
        $("input[id$=Txt_Fecha_Inicio]").daterangepicker();
        $("input[id$=Txt_Fecha_Inicio]").daterangepicker({arrows:true}); 
            
        //Prepara el DatePicker para seleccionar la fecha
        $("#Txt_Fecha_Fin").daterangepicker();
        $("input[id$=Txt_Fecha_Fin]").daterangepicker({arrows:true}); 
    }catch(e){
        alert('Error al crear los calendarios de la página. Error: [' + e + ']');
    }
 }
///********************************************************************************************************
///NOMBRE: Btn_Generar_Reporte_Click
///
///DESCRIPCION: Evento del botón que genera el reporte de movimientos del empleado.
///
///PARÁMETROS: No Áplica.
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 16/Mayo/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************************* 
function Btn_Generar_Reporte_Click(e){ 
    try{
        e.preventDefault();//Evitamos que ocurra un postback en la página al dar click al botón que genera el reporte. 
        Control_Mensajes_Error(false);
        
        if(Validar_Datos()){
            var Tipo_Movimiento = $('select[id$=Cmb_Tipo_Movimiento] :selected').text();
            var Tipo_Nomina = $('select[id$=Cmb_Tipos_Nominas] :selected').val();
            var Unidad_Responsable = $('select[id$=Cmb_Unidad_Responsable] :selected').val();
            var Empleado = $('select[id$=Cmb_Empleados] :selected').val();
            var Fecha_Inicio = $('#Txt_Fecha_Inicio').val();
            var Fecha_Fin = $('#Txt_Fecha_Fin').val();                    
            
            /**
            * Construimos la cadena que enviara los datos al servidor para realizar la consulta de totales de nómina.
            **/
            var cadena="?Tipo_Movimiento=" + Tipo_Movimiento + "&Tipo_Nomina=" + Tipo_Nomina +
               "&Unidad_Responsable=" + Unidad_Responsable + "&Empleado=" + Empleado + 
               "&Fecha_Inicio=" + Fecha_Inicio + "&Fecha_Fin=" + Fecha_Fin + "&Opcion=4"; 
            
            Controla_Visibilidad_ProgressBar(true);   
              
            $.ajax({
                url: "Frm_Rpt_Nom_Controlador_Mov_Empl.aspx" + cadena,
                type: 'POST',
                async: false,
                cache: false,
                dataType:'text',
                success: function(URL_Pagina) {   
                    //Ejecutamos la función que mostrara el reporte y actualizara la barra de progreso.            
                    setTimeout("Actualizar_Barra_Progreso(0, '" + URL_Pagina + "')",250);                             
                }        
            }); 
        }else{
            Control_Mensajes_Error(true);
        }
    }catch(e){
        alert('Error al generar el reporte de movimientos del empleado. Error: [' + e + ']');
    }   
}
///********************************************************************************************
///NOMBRE: Actualizar_Barra_Progreso
///
///DESCRIPCION: Función que se llama para actualizar la barra de progreso.
///
///PARÁMETROS: valor.- Nuevo valor con el que se actualizara la barra de progreso.
///            URL_Pagina.- Ruta del reporte que se mostrara en pantalla.
/// 
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 12/Mayo/2011 10:15 a.m.
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
function Actualizar_Barra_Progreso(valor, URL_Pagina){
    var Nuevo_Valor = valor + 1;
    
    try{
        if(Nuevo_Valor <= 100){    
             $("#Barra_Progreso").progressBar(Nuevo_Valor);//Establecemos el enuevo valor de la barra de progreso.
             /**
             * LLamamos a el método que que actualiza la barra de progreso y pasamos como parámetro el nuevo valor 
             * valor de la barra de progreso         
             **/     
             setTimeout("Actualizar_Barra_Progreso(" + Nuevo_Valor + ", '" + URL_Pagina + "')",30);
         }else{         
             Controla_Visibilidad_ProgressBar(false);
             $("#Barra_Progreso").progressBar(0);//Asignamos a la barra de progreso un valor de cero.       
             
             /**
             * Mostramos el Reporte de Totales de Nómina.
             **/   
             window.open(URL_Pagina, 'Movimientos_Empleados',
                        'toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600');                      
         }
     }catch(e){
        alert('Error al actualizar la barra de progreso. Error: [' + e + ']');
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
                  
             $('select[id$=Cmb_Empleados]').empty();//Limpiamos el combo de periodo inicial.       
             Llenar_Combo_Empleados($('select[id$=Cmb_Unidad_Responsable] :selected').val()); 
             Control_Mensajes_Error(false);                             
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
///********************************************************************************************
///NOMBRE: Validar_Formato_Fechas
///
///DESCRIPCION: Método que valida que el formato de las fechas sea el correcto.
///
///PARÁMETROS: Fecha.- Fecha la cuál será evaluada para ver si su formato es correcto.
/// 
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 17/Mayo/2011 11:58 a.m.
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
function Validar_Formato_Fechas(Fecha){
    var PATRON = /^(([0-9])|([0-2][0-9])|([3][0-1]))\/(Ene|Feb|Mar|Abr|May|Jun|Jul|Ago|Sep|Oct|Nov|Dic)\/\d{4}$/;//patron con el cuál se evaluaran las fechas.

     try{
        if(Fecha != '')
            return Fecha.match(PATRON);            
    }catch(e){
        alert('Error generado al validar el formato de las fechas. Error: [' + e + ']');
    }                               
}
///********************************************************************************************
///NOMBRE: Validar_Datos
///
///DESCRIPCION: Válida los datos requeridos para realizar la consulta de los totales de nómina.
///
///PARÁMETROS: No Áplica.
/// 
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 17/Mayo/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
function Validar_Datos(){
    var Estado_Operacion = true;
    var Mensaje = "<ol style='font-size:10px; text-align:left;'>Es necesario introducir:";

    try {
        if($("select[id$=Cmb_Tipo_Movimiento] :selected").text() == '<- Seleccione ->'){
           Mensaje += "<li style='font-size:10px;'>Seleccione el tipo de movimiento a consultar de los empleados. </li>"; 
           Estado_Operacion = false;           
        }        

        if($('#Txt_Fecha_Inicio').val() != ''){
           if(!Validar_Formato_Fechas($('#Txt_Fecha_Inicio').val())){
                Mensaje += "<li style='font-size:10px;'>El formato de la fecha de inicio es incorrecto. </li>"; 
                Estado_Operacion = false; 
           }           
        }  
        
        if($('#Txt_Fecha_Fin').val() != ''){
           if(!Validar_Formato_Fechas($('#Txt_Fecha_Inicio').val())){
                Mensaje += "<li style='font-size:10px;'>El formato de la fecha de final es incorrecto. </li>"; 
                Estado_Operacion = false; 
           }           
        }         

        Mensaje += "</ol>";              
        $("#Lbl_Mensaje_Error").html(Mensaje); 
    }catch(Ex){
    
    }
    return Estado_Operacion;
}
///********************************************************************************************
///NOMBRE: Control_Mensajes_Error
///
///DESCRIPCION: Controla si se muestran los mensajes de validacion de la página.
///
///PARÁMETROS: No Áplica.
/// 
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 17/Mayo/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
function Control_Mensajes_Error(Limpiar){
    if(!Limpiar){
        $('#Div_Contenedor_Errorres').css("display", "none");
        $('img[id$=Img_Error]').css("display", "none");
        $("#Lbl_Mensaje_Error").css("display", "none"); 
        $("#Lbl_Mensaje_Error").html('');    
    }else{
        $('img[id$=Img_Error]').css("display", "block");
        $("#Lbl_Mensaje_Error").css("display", "block");     
        $('#Div_Contenedor_Errorres').css("display", "block");
    }
}
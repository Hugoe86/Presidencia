var Pagina;
Pagina=$(document);
Pagina.ready(Inicializar_Eventos_JQUERY);

///********************************************************************************************
///NOMBRE: Inicializar_Eventos_JQUERY
///
///DESCRIPCION: Carga la configuracion inicial de los controles de la pagina.
///
///PARÁMETROS: No Áplica
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 12/Mayo/2011 10:15 a.m.
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
function Inicializar_Eventos_JQUERY(){
    Llenar_Combo_Tipos_Nominas();     //Cargamos el combo de tipos de nóminas.
    Llenar_Combo_Calendario_Nominas();//Cargamos el combo de calendario de nominas.
    Configuracion_Inicial_Pagina();   //Habilitamos la configuración inicial de la página.    
}
///********************************************************************************************
///NOMBRE: Configuracion_Inicial_Pagina
///
///DESCRIPCION: Método que habilita la configuración inicial de la página.
///
///PARÁMETROS: No Áplica
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 12/Mayo/2011 10:15 a.m.
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
function Configuracion_Inicial_Pagina(){
    try{
        Eventos_Pagina();//Inicializamos los eventos de la página.
        
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
///FECHA CREÓ: 12/Mayo/2011 10:15 a.m.
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
function Llenar_Combo_Tipos_Nominas() {
    var Combo = $("select[id$=Cmb_Tipo_Nominas]").get(0);
   
    $.ajax({
        url: "Frm_Nom_Controlador.aspx?Tabla=TIPO_NOMINAS&Texto=NOMINA&Valor=TIPO_NOMINA_ID&Opcion=1",
        type:'POST',
        async: false,
        cache: false,
        success: function (Datos) {
             if (Datos != null) {             
                 Combo.options[0] = new Option('<-SELECCIONE->', ''); 
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
///NOMBRE: Llenar_Combo_Calendario_Nominas
///
///DESCRIPCION: Consulta los calendarios de nominas activas en el sistema y las  carga en el 
///             combo.
///
///PARÁMETROS: No Áplica
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 12/Mayo/2011 10:15 a.m.
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
function Llenar_Combo_Calendario_Nominas() {
    var Combo = $("select[id$=Cmb_Calendario_Nomina]").get(0);

    $.ajax({
        url: "Frm_Nom_Controlador.aspx?Tabla=CALENDARIO&Texto=ANIO&Valor=NOMINA_ID&Opcion=2",
        type:'POST',
        async: false,
        cache: false,
        success: function (Datos) {
             if (Datos != null) {             
                 Combo.options[0] = new Option('<-SELECCIONE->', ''); 
                 $.each(Datos.CALENDARIO, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.ANIO, Elemento.NOMINA_ID);                             
                 });
             }
             else {
                 alert("No hay Calendarios de Nómina Disponibles");
             }        
        }        
    });
 }
///********************************************************************************************
///NOMBRE: Llenar_Combo_Periodo
///
///DESCRIPCION: Consulta los calendarios de nominas activas en el sistema y las  carga en el 
///             combo.
///
///PARÁMETROS: Combo.- Control sobre el se cargaran los periodos nominales.
///            Nomina_ID.- Nomina de la cuál se consultaran los periodods nominales. 
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 12/Mayo/2011 10:15 a.m.
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
 function Llenar_Combo_Periodo(Combo, Nomina_ID) {
    var Combo = $("select[id$=" + Combo + "]").get(0);

    $.ajax({
        url: "Frm_Nom_Controlador.aspx?Tabla=PERIODO&Texto=NO_NOMINA&Valor=NO_NOMINA&Nomina_ID=" + Nomina_ID + "&Opcion=3",
        type:'POST',
        async: false,
        cache: false,
        success: function (Datos) {
             if (Datos != null) {                       
                 Combo.options[0] = new Option('<-SELECCIONE->', ''); 
                 $.each(Datos.PERIODO, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.NO_NOMINA, Elemento.NO_NOMINA);                             
                 });
                  Combo.disabled = false;
             }
             else {                
                 alert("No hay periodos catorcenales disponibles, para la nómina seleccionada.");
             }      
        }        
    });
 }
///********************************************************************************************
///NOMBRE: Eventos
///
///DESCRIPCION: Método que almacena los eventos generados en la página.
///
///PARÁMETROS: No Áplica
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 12/Mayo/2011 10:15 a.m.
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
 function Eventos_Pagina(){
    try{        
        /**
        * Evento generado al seleccionar un elemento del calendario de nómina.
        **/    
        $("select[id$=Cmb_Calendario_Nomina]").bind("change keyup", function() {
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
///********************************************************************************************
///NOMBRE: Btn_Generar_Reporte_Click
///
///DESCRIPCION: Genera el reporte de Totales Generales de la Nómina.
///
///PARÁMETROS: No Áplica
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 12/Mayo/2011 10:15 a.m.
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
 function Btn_Generar_Reporte_Click(e){
    try{
        e.preventDefault();//Evitamos que ocurra un postback en la página al dar click al botón que genera el reporte. 
        Control_Mensajes_Error(false);
        
        if(Validar_Datos()){
            var Nomina_ID = $('select[id$=Cmb_Calendario_Nomina] :selected').val();//Obtenemos el identificador del calendario de nomina.
            var Tipo_Nomina_ID = $('select[id$=Cmb_Tipo_Nominas] :selected').val();//Obtenemos el identificador del tipo de nómina.
            var No_Nomina = $('select[id$=Cmb_Periodo_A] :selected').val() + "," + $('select[id$=Cmb_Periodo_B] :selected').val();//Obtenemos los identificadores de los periodos seleccionados.
            
            /**
            * Construimos la cadena que enviara los datos al servidor para realizar la consulta de totales de nómina.
            **/
            var cadena="?NOMINA_ID=" + Nomina_ID + "&TIPO_NOMINA_ID=" + Tipo_Nomina_ID + "&NO_NOMINA=" + No_Nomina + "&Opcion=4";
            
            Controla_Visibilidad_ProgressBar(true);
             
             /**
             * Hacemos la petición al servidor para consultar los totales de nómina
             * y generar el reporte correspóndiente.
             **/    
            $.ajax({
                url: "Frm_Nom_Controlador.aspx" + cadena,
                type: 'POST',
                async: false,
                cache: false,
                success: function(URL_Pagina) {   
                    //Ejecutamos la función que mostrara el reporte y actualizara la barra de progreso.            
                    setTimeout("Actualizar_Barra_Progreso(0, '" + URL_Pagina + "')",250);                             
                }        
            });
        }else{
            Control_Mensajes_Error(true);
        }
    }catch(ex){
        alert('Error al consultar los Totales de Nomina. Error: [' + ex + ']');
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
         window.open(URL_Pagina, 'Totales_Generales_Nomina',
                    'toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600');                      
     }
}
///********************************************************************************************
///NOMBRE: Actualizar_Barra_Progreso_Eventos
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
function Actualizar_Barra_Progreso_Eventos(valor){
    var Nuevo_Valor = valor + 1;
    
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
         
        $('select[id$=Cmb_Periodo_A]').empty();//Limpiamos el combo de periodo inicial.
        $('select[id$=Cmb_Periodo_B]').empty();//Limpiamos el combo de periodo final.
        
        //Cargamos los combos de periodos nominales.
        Llenar_Combo_Periodo("Cmb_Periodo_A", $('select[id$=Cmb_Calendario_Nomina] :selected').val());
        Llenar_Combo_Periodo("Cmb_Periodo_B", $('select[id$=Cmb_Calendario_Nomina] :selected').val());
        
        Control_Mensajes_Error(false);                             
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
///FECHA CREÓ: 12/Mayo/2011 10:15 a.m.
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
function Validar_Datos(){
    var Estado_Operacion = true;
    var Mensaje = "<ol style='font-size:10px; text-align:left;'>Es necesario introducir:";

    try {
        if($("select[id$=Cmb_Calendario_Nomina] :selected").text() == '<-SELECCIONE->'){
           Mensaje += "<li style='font-size:10px;'>Seleccione un calendario de nomina. </li>"; 
           Estado_Operacion = false;           
        }        

        if($("select[id$=Cmb_Tipo_Nominas] :selected").text() == '<-SELECCIONE->'){
           Mensaje += "<li style='font-size:10px;'> Seleccione un Tipo de Nómina. </li>" ;
           Estado_Operacion = false;
        }  
        
        if($("select[id$=Cmb_Calendario_Nomina] :selected").text() != '<-SELECCIONE->'){
            if($("select[id$=Cmb_Periodo_A] :selected").text() != '' ){
                if($("select[id$=Cmb_Periodo_A] :selected").text() == '<-SELECCIONE->' ){
                   Mensaje += "<li style='font-size:10px;'> Seleccione el periodo inicial. </li>" ;
                   Estado_Operacion = false;
                }          
            }else{
               Mensaje += "<li style='font-size:10px;'> Seleccione el periodo inicial. </li>" ;
               Estado_Operacion = false;
            }
            
            if($("select[id$=Cmb_Periodo_B] :selected").text() != '' ){
                if($("select[id$=Cmb_Periodo_B] :selected").text() == '<-SELECCIONE->' ){
                   Mensaje += "<li style='font-size:10px;'> Seleccione el periodo final. </li>" ;
                   Estado_Operacion = false;
                }
            }else{
               Mensaje += "<li style='font-size:10px;'> Seleccione el periodo final. </li>" ;
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
///FECHA CREÓ: 12/Mayo/2011 10:15 a.m.
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************
function Control_Mensajes_Error(Limpiar){
    if(!Limpiar){
        $('img[id$=Img_Error]').css("display", "none");
        $("#Lbl_Mensaje_Error").css("display", "none"); 
        $("#Lbl_Mensaje_Error").html('');    
    }else{
        $('img[id$=Img_Error]').css("display", "block");
        $("#Lbl_Mensaje_Error").css("display", "block");     
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
    if(Mostrar){
        $("#Barra_Progreso").css("display", "block");//Mostramos la capa que bloqueara la pantalla.
        $("#progressBackgroundFilter").css("display", "block");//Mostramos la barra de progreso.    
    }else{
        $("#Barra_Progreso").css("display", "none");//Ocultamos el div que bloquea la pantalla.
        $("#progressBackgroundFilter").css("display", "none");//Ocultamos el div que muestra la barra de progreso.    
    }
}
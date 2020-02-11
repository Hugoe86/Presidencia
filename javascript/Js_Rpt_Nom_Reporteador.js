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
///FECHA_CREO           : Noviembre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function Configuracion_Inicial(){
    Consultar_Tablas_Nomina(); 
    
    $('input[id$=Btn_Generar_Reporte]').click(function(e){
        var tabla_seleccionada = $("select[id$=cmb_tablas_catalogos_nomina] :selected").val();
        
        $('input[id$=Txt_Campos_Reporte]').val($('#sortable2').sortable('toArray'));
        $('input[id$=Txt_Tabla]').val(tabla_seleccionada);
        
        if($('input[id$=Txt_Campos_Reporte]').val() == ''){
            e.preventDefault();
            $.messager.alert("Información","Es necesario seleccionar los campos que se mostraran en el reporte.");
        }
    });
    
    $('#Btn_Limpiar').click(function(e){
        e.preventDefault();
        Limpiar_Controles_All();
    });    
    
    $('#Btn_Salir').click(function(e){
        e.preventDefault();
        window.location.replace("../Paginas_Generales/Frm_Apl_Principal.aspx");
    });     
    
    //Establece la configuración inical de la barra de progreso. 
    $("#Barra_Progreso").progressBar({
        steps:20,
        stepDuration:75,
        max:100,
        showText:false,
        textFormat:'porcentage',
        width:120,
        height:12,
        boxImage:'../../easyui/images/progressbar.gif',
        callback:function(data){
        
         //Validamos que al finalizar el tiempo asignado la 
         //barra de progreso se oculte.
         if (data.running_value == data.value) {  
            Controla_Visibilidad_ProgressBar(false); 
            $("#Barra_Progreso").progressBar(0); 
         }
        },
        barImage:{
                0:  '../../easyui/images/progressbg_green.gif',
                20: '../../easyui/images/progressbg_green.gif',
                40: '../../easyui/images/progressbg_green.gif',
                60: '../../easyui/images/progressbg_green.gif',
                80: '../../easyui/images/progressbg_green.gif'
                 }       
     });                
}
/// *************************************************************************************************************************
/// Nombre Método: Consultar_Tablas_Nomina
/// 
/// Descripción: Método que consulta todas las tablas de nomina de tipo catalogo.
/// 
/// Parámetros: No Aplica.
/// 
/// Usuario Creo: Juan Alberto Hernández Negrete.
/// Fecha Creó: Diciembre/2011
/// Usuario Modifico:
/// Fecha Modifico:
/// Causa Modificación:
/// *************************************************************************************************************************
function Consultar_Tablas_Nomina() {
    var Combo = $("select[id$=cmb_tablas_catalogos_nomina]").get(0);
    var Aux = '';
   
    $.ajax({
        url: "Frm_Cat_Nom_Reporteador_Controlador.aspx?tabla=CAT_NOMINA&opcion=consultar_tablas_nomina",
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {             
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.CAT_NOMINA, function (Contador, Elemento) {
                    Aux = (Elemento.NOMBRE_CATALOGO + "").replace('CAT_', '');
                    Aux = Aux.replace('NOM_', '');
                    Aux = Aux.replace('TAB_', '');
                    Combo.options[++Contador] = new Option(Aux, Elemento.NOMBRE_CATALOGO);                             
                 });
             }
             else {
                 $.messager.alert("Mensaje","No catálogos de nomina en el sistema en el sistema");
             }      
        }        
    });
    
    $("select[id$=cmb_tablas_catalogos_nomina]").bind('change', function(){
         var tabla_seleccionada = $("select[id$=cmb_tablas_catalogos_nomina] :selected").val();
         
         if(tabla_seleccionada != ""){
            Limpiar_Controles();
            Consultar_Campos_Por_Tabla($("select[id$=cmb_tablas_catalogos_nomina] :selected").val());
         }
    });    
}
/// *************************************************************************************************************************
/// Nombre Método: Consultar_Campos_Por_Tabla
/// 
/// Descripción: Método que consulta todos los campos de la tabla seleccionada. Y crea a partir de esta información
///              una tabla con los campos que el usuario podra obtener en el reporte.
/// 
/// Parámetros: Tabla.- Tabla de la cuál se consultaran los campos.
/// 
/// Usuario Creo: Juan Alberto Hernández Negrete.
/// Fecha Creó: Diciembre/2011
/// Usuario Modifico:
/// Fecha Modifico:
/// Causa Modificación:
/// *************************************************************************************************************************
function Consultar_Campos_Por_Tabla(tabla_seleccionada) {    
    var HTML  = "";

    $.ajax({
        url: "Frm_Cat_Nom_Reporteador_Controlador.aspx?tabla=CAMPOS&opcion=consultar_campos_por_tabla&tabla_seleccionada=" + tabla_seleccionada,
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {
             
                Controla_Visibilidad_ProgressBar(true);
                $("#Barra_Progreso").progressBar(100);  
                
                 $.each(Datos.CAMPOS, function (Contador, Elemento) {                    
                    HTML = '<li class="ui-state-default" style="background: black url(../imagenes/paginas/glossyback.gif) repeat-x bottom left;font-family:Tahoma;color: White ;display: list-item;position: relative;width: auto;padding: 4px 0;padding-left: 10px;text-decoration:none;cursor:hand;border-style:outset; font-size:10px; width:210px; cursor:move;" id="' + Elemento.NOMBRE_CAMPO + '">' + Elemento.NOMBRE_CAMPO + '</li>';
                    $('#sortable1').append(HTML);
                    HTML = '';                                     
                 });
             }
             else {
                 $.messager.alert("Mensaje","Latabla consultada no tiene campos.");
             }      
        }        
    });       
}
/// *************************************************************************************************************************
/// Nombre Método: Limpiar_Controles
/// 
/// Descripción: Método que limpia los controles de la pagina.
/// 
/// Usuario Creo: Juan Alberto Hernández Negrete.
/// Fecha Creó: Diciembre/2011
/// Usuario Modifico:
/// Fecha Modifico:
/// Causa Modificación:
/// *************************************************************************************************************************
function Limpiar_Controles(){
    $('input[id$=Txt_Campos_Reporte]').val('');
    $('input[id$=Txt_Tabla]').val('');
    $('#sortable1').empty();
    $('#sortable2').empty();
}
/// *************************************************************************************************************************
/// Nombre Método: Limpiar_Controles_All
/// 
/// Descripción: Método que limpia los controles de la pagina.
/// 
/// Usuario Creo: Juan Alberto Hernández Negrete.
/// Fecha Creó: Diciembre/2011
/// Usuario Modifico:
/// Fecha Modifico:
/// Causa Modificación:
/// *************************************************************************************************************************
function Limpiar_Controles_All(){
    $("select[id$=cmb_tablas_catalogos_nomina]").val('');
    $('input[id$=Txt_Campos_Reporte]').val('');
    $('input[id$=Txt_Tabla]').val('');
    $('#sortable1').empty();
    $('#sortable2').empty();
}
///********************************************************************************************
///NOMBRE: Controla_Visibilidad_ProgressBar
///
///DESCRIPCION: Controla si se muestran la barra de progreso en la página.
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
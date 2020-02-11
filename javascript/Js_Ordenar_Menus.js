var Pagina;
Pagina=$(document);
Pagina.ready(Inicializar_Eventos_Ordenar_Menus);

///********************************************************************************************
///NOMBRE: Inicializar_Eventos_Ordenar_Menus
///
///DESCRIPCION: Configura el estado inicial del formulario.
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 20/Octubre/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************  
function Inicializar_Eventos_Ordenar_Menus()
{
     var Tipo = '';
     var Menu_Seleccionado = '';
     
     $("select[id$=Cmb_Menus]").attr('disabled', true);
    
     Llenar_Combo_Menus();
    
    //EVENTOS DE COMBO DE TIPO [MENUS-SUBMENUS]
     $("select[id$=Cmb_Tipo]").bind("change keyup", function() { 
        Tipo = $("select[id$=Cmb_Tipo] :selected").val();
        
         if(Tipo == "menu"){
            $("select[id$=Cmb_Menus]").attr('disabled', true);
            Cargar_Lista_Menus();
         }else if(Tipo == "submenu"){
             $("select[id$=Cmb_Menus]").attr('disabled', false);
             $('#Span_Orden_Menus').html('');
         }else if(Tipo == ""){
             Configuracion_Inicial();
         }         
     }); 
        
     //EVENTOS DEL COMBO DE MENUS.
     $("select[id$=Cmb_Menus]").bind("change keyup", function() { 
        Menu_Seleccionado = $("select[id$=Cmb_Menus] :selected").val();
         if(Menu_Seleccionado == ""){
            Configuracion_Inicial();
         }else{
           Cargar_Lista_Submenus();  
         } 
     }); 
     
     //EVENTO DEL CLICK DEL BOTON DE ACTUALIZAR MENUS Y SUBMENUS.
     $('input[id$=Btn_Actualizar_Menus]').bind("click", function(e){
        e.preventDefault();
        Actualizar_Orden_Menus();
     });
     
     //EVENTOS DE CONTROL VISUAL DEL BOTON DE ACTUALIZAR MENUS Y SUBMENUS.
     $('input[id$=Btn_Actualizar_Menus]').hover(
         function(e){
             e.preventDefault();
             $(this).css("background-color", "#2F4E7D");
             $(this).css("color", "#FFFFFF");
         },
        function(e){
             e.preventDefault();
             $(this).css("background-color", "Silver");
             $(this).css("color", "#5656560");
        }
     );
}
///********************************************************************************************
///NOMBRE: Llenar_Combo_Menus
///
///DESCRIPCION: Consulta los menus para cargar el combo de menus con los mismos.
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 20/Octubre/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************  
function Llenar_Combo_Menus() {
    var Combo = $("select[id$=Cmb_Menus]").get(0);
    
   
    $.ajax({
        url: "Frm_Controlador_Orden_Menus.aspx?Tabla=MENUS&Opcion=Consultar_Menus",
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {             
                 Combo.options[0] = new Option('<- Seleccione ->', ''); 
                 $.each(Datos.MENUS, function (Contador, Elemento) {
                    Combo.options[++Contador] = new Option(Elemento.MENU_DESCRIPCION, Elemento.MENU_ID);                             
                 });
             }
             else {
                 alert("No hay menus del sistema");
             }      
        }        
    });
}
///********************************************************************************************
///NOMBRE: Cargar_Lista_Menus
///
///DESCRIPCION: Consulta los menús del sistema.
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 20/Octubre/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************  
function Cargar_Lista_Menus(){
    var HTML_code = "";
    
    $.ajax({
        url: "Frm_Controlador_Orden_Menus.aspx?Tabla=MENUS&Opcion=Consultar_Sub_Menus&Parent_ID=0",
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {    
                 HTML_code = "<ol style='width:50%;'  id='sortable'>";         
                 $.each(Datos.MENUS, function (Contador, Elemento) {
                   HTML_code += '<li class="li_Orden_Menu" style="text-align:left;" id="' + Elemento.MENU_ID + '"><font style="color:White;font-size:10px;">' + Elemento.MENU_DESCRIPCION.toUpperCase() + '</font></li>';                               
                 });
                 HTML_code += "</ol> ";
                 $('#Span_Orden_Menus').html("<h3 style='color:#2F4E7D;font-size:18px; font-family:Comic Sans MS; font-weight:bold;'>Menús</h3>" + HTML_code);
                 
                 $("#sortable").sortable();
             }
             else {
                 alert("No hay menus del sistema");
             }      
        }        
    });
}
///********************************************************************************************
///NOMBRE: Cargar_Lista_Submenus
///
///DESCRIPCION: Consulta los sub menús del sistema.
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 20/Octubre/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************  
function Cargar_Lista_Submenus(){
    var HTML_code = "";
    var Parent_ID = $("select[id$=Cmb_Menus] :selected").val();
    
    $.ajax({
        url: "Frm_Controlador_Orden_Menus.aspx?Tabla=MENUS&Opcion=Consultar_Sub_Menus&Parent_ID=" + Parent_ID,
        type:'POST',
        async: false,
        cache: false,
        dataType:'json',
        success: function (Datos) {
             if (Datos != null) {    
                 HTML_code = "<ol style='width:50%;'  id='sortable'>";         
                 $.each(Datos.MENUS, function (Contador, Elemento) {
                   HTML_code += '<li class="li_Orden_Submenu" style="text-align:left;" id="' + Elemento.MENU_ID + '"><font style="color:Dark;font-size:10px;">' + Elemento.MENU_DESCRIPCION.toUpperCase() + '</font></li>';                               
                 });
                 HTML_code += "</ol> ";
                 $('#Span_Orden_Menus').html("<h3 style='color:#2F4E7D;font-size:18px; font-family:Comic Sans MS; font-weight:bold;'>Sub Menús</h3>" + HTML_code);
                 
                 $("#sortable").sortable();
             }
             else {
                 alert("No hay menus del sistema");
             }      
        }        
    });
}
///********************************************************************************************
///NOMBRE: Actualizar_Orden_Menus
///
///DESCRIPCION: Ejecuta la actualizacion del orden de los menus y submenus del sistema.
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 20/Octubre/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************  
function Actualizar_Orden_Menus(){
    var Nuevo_Orden_Menus = $("#sortable").sortable('toArray');
    if(Nuevo_Orden_Menus.length > 0){
    $.ajax({
        url: "Frm_Controlador_Orden_Menus.aspx?Opcion=Actualizar_Orden_Menus&Orden_Menus=" + Nuevo_Orden_Menus,
        type: 'POST',
        async: false,
        cache: false,
        dataType:'text',
        success: function(RESPUESTA) {
            if(RESPUESTA == "SI"){
                 Configuracion_Inicial();
                 alert("El orden de los menus se ha actualizado.");
                 location.reload();
            }else if(RESPUESTA == "NO"){
                alert("El orden de los menus no fue actualizado.");
            }
        }
    });
    
    }else{alert("El orden de los menus no fue actualizado.");}
}
///********************************************************************************************
///NOMBRE: Configuracion_Inicial
///
///DESCRIPCION: Configura el estado inicial de la página.
///
///USUARIO CREÓ: Juan Alberto Hernández Negrete.
///FECHA CREÓ: 20/Octubre/2011
///USUARIO MODIFICO:
///FECHA MODIFICO:
///CAUSA MODIFICACIÓN:
///********************************************************************************************  
function Configuracion_Inicial(){
     $("select[id$=Cmb_Menus]").attr('disabled', true);
     $('#Span_Orden_Menus').html('');
     $('select[id$=Cmb_Menus]').val('');
     $('select[id$=Cmb_Tipo]').val('');
}
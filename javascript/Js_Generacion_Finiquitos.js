var Pagina;
Pagina=$(document);
Pagina.ready(inicializarEventos);
jQuery.fx.interval = 100;

function inicializarEventos()
{
  var Pagina=$("#Btn_Ver_Detalles_Finiquito");
  Pagina.click(Mostrar_Detalles_Finiquito);
  
  Validar_Numero_Empleado();
  Estilo_Botones_Dinamicos();
  Validar_Boton_Cerrar_Finiquito();  
  //Validar_Copiar_Pegar_Cortar(); 
  
  $('input[id$=Btn_Actualizar_Tablas_Conceptos]').bind('click', function(e){
        var Texto = $('input[id$=Txt_Nombre_Empleado]').val();
        
        if(Texto == ''){
            alert('No existe ninguna tabla que actualziar!');
            e.preventDefault();
        }
  });
}


function Mostrar_Detalles_Finiquito()
{    
  $("#Contenedor").toggle(1000);
}


function Validar_Numero_Empleado(){
    $('input[id$=Txt_No_Empleado]').bind('keyup blur', function(){
        if(this.value.match(/[^0-9]/g)){
            this.value = this.value.replace(/[^0-9]/g, '');
        }
    });   
    
    $('input[id$=Txt_No_Empleado]').bind('keyup blur', function(e){
        if(this.value.length > 6){
            e.preventDefault();
            $(this).css("background-color", "Yellow");
            $(this).css("color", "Red");
            this.value = this.value.substring(0, 6);
        }else{
            $(this).css("background-color", "White");
            $(this).css("color", "Black");
        }
    });
}

function Validar_Boton_Cerrar_Finiquito(){
   $('input[id$=Btn_Cerrar_Finiquito]').click(function(e){
       if($('input[id$=Btn_Generar_Finiquito]').val() == 'Generar Finiquito'){
            alert('No es posible cerrar el finiquito si este previamente no a sido generado.');
            e.preventDefault();            
        }else{
            if(!confirm("¿Está seguro de cerrar el finiquito que se genero al empleado.\n Una vez cerrado no podra realizarse ningún cambio?")){
                alert('Operación de cierre de finiquito a sido cancelada');
                e.preventDefault(); 
            }
        }
    });
}


function Estilo_Botones_Dinamicos(){
   $('input[id$=Btn_Cerrar_Finiquito]').hover(
     function(e){
         e.preventDefault();
         $(this).css("background-color", "#2F4E7D");
         $(this).css("color", "#FFFFFF");
     },
    function(e){
         e.preventDefault();
             $(this).css("background-color", "#f5f5f5");
             $(this).css("color", "#565656");
    }
   );
   
    $('input[id$=Btn_Generar_Finiquito]').hover(
        function(e){
            e.preventDefault();
            $(this).css("background-color", "#2F4E7D");
            $(this).css("color", "#FFFFFF");
        },
        function(e){
            e.preventDefault();
             $(this).css("background-color", "#f5f5f5");
             $(this).css("color", "#565656");
        }
    );
  
    $('input[id$=Btn_Ver_Detalles_Finiquito]').hover(
        function(e){
            e.preventDefault();
            $(this).css("background-color", "#2F4E7D");
            $(this).css("color", "#FFFFFF");
        },
        function(e){
            e.preventDefault();
             $(this).css("background-color", "#f5f5f5");
             $(this).css("color", "#565656");
        }
    );
    
    $('input[id$=Btn_Actualizar_Tablas_Conceptos]').hover(
        function(e){
            e.preventDefault();
            $(this).css("background-color", "#2F4E7D");
            $(this).css("color", "#FFFFFF");
        },
        function(e){
            e.preventDefault();
             $(this).css("background-color", "#f5f5f5");
             $(this).css("color", "#565656");
        }
    );
}

function Validar_Copiar_Pegar_Cortar(){
    var mensaje = "";
    
    $('input[id$=Txt_No_Empleado]').bind('cut copy paste', function(e){
        e.preventDefault();
        
        switch(e.type){
            case "copy":
                mensaje = "copiar";
            break;
            case "cut":
                mensaje = "cortar";
            break;
            case "paste":
                mensaje = "pegar";
            break;                
            default:
            break;
        }
        alert('Tú no puedes ' + mensaje + ' texto. En este control.');
    });
}
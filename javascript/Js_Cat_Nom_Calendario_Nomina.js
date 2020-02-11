var Pagina;
Pagina=$(document);
Pagina.ready(Inicializar_Eventos_Calendario_Nomina);

function Inicializar_Eventos_Calendario_Nomina()
{
    //Validar_Copiar_Pegar_Cortar();
    Validar_Anio();
    
    $('input[id$=Btn_Agregar_Periodos]').click(function(e){
        $('input:text').each(function(){
           var valor = $(this).val();
           
           if(valor == '__/___/____'){
                e.preventDefault();
                $(this).css("background-color", "yellow");
                $(this).css("color", "red");
                alert('La Fecha de Fin del calendario de nómina es un dato\n requerido para generar los periodos nominales!');            
           }else{
                $(this).css("background-color", "white");
                $(this).css("color", "black");
           }
        });
    });
    
   $('input[id$=Btn_Agregar_Periodos]').hover(
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
   
   $('input[id$=Btn_Limpiar_Periodos]').hover(
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
    $('input:text').each(function(){
        $(this).bind('cut copy paste', function(e){
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
    });
}

function Validar_Anio(){
    $('input[id$=Txt_Anio_Calendario_Nomina]').blur(function(e){
    
        try {
            if($(this).val().length >= 4){
                var Anio =parseInt($(this).val());
                if(Anio < 2011 || Anio > 2100){
                    $(this).val('');
                    $(this).css("background-color", "Yellow");  
                    $('#Mensaje_Anio').text('Año Invalido');    
                }else{
                    $(this).css("background-color", "White");
                    $('#Mensaje_Anio').text('');
                }                
            }else{
                    $(this).css("background-color", "Yellow");
                    $('#Mensaje_Anio').text('Año Invalido');
                }   
        }catch(e){
            alert(e);
        }
    });
}

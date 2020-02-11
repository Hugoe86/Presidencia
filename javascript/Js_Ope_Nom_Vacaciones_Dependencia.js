var Pagina;
Pagina=$(document);
Pagina.ready(Inicializar_Eventos_Autoriazacion_Vacaciones_Dependencia);

function Inicializar_Eventos_Autoriazacion_Vacaciones_Dependencia()
{
    var Ceros = "";
    
    Contar_Caracteres_Comentarios_Autoriazacion_Vacaciones_Dependencia();
    
   $('input[id*=Txt_No_Empleado]').live("blur", function(){
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

     $('input[id*=Txt_No_Empleado]').css('background-image', 'url(../imagenes/paginas/empleado.png)');
     $('input[id*=Txt_No_Empleado]').css('background-repeat', 'no-repeat');
     $('input[id*=Txt_No_Empleado]').css('background-position', 'left');    
     $('input[id*=Txt_No_Empleado]').css('border-style', 'outset');
     $('input[id*=Txt_No_Empleado]').css('font-family', 'Tahoma');
     $('input[id*=Txt_No_Empleado]').css('font-size', '12px');
     $('input[id*=Txt_No_Empleado]').css('font-weight', 'bold');
     $('input[id*=Txt_No_Empleado]').css('color', '#2F4E7D');
     $('input[id*=Txt_No_Empleado]').css('height', '24px');
     $('input[id*=Txt_No_Empleado]').css('text-align', 'center');   
     $('input[id*=Txt_No_Empleado]').click(function(){ $(this).focus().select(); });
     $('input[id*=Txt_No_Empleado]').keyup(function(){$(this).css('background-color', '#FFFF99');});
     $('input[id*=Txt_No_Empleado]').blur(function(){$(this).css('background-color', 'White');});
     
     $('input[id$=Txt_No_Empleado]').bind('blur', function(){
        var $No_Empleado = $(this);
        
        if($No_Empleado.val() == '' || $No_Empleado.val() == 'No Empleado'){
            $No_Empleado.val('');
        }
     });
     
     $('input[id$=Btn_Consultar_Dias_Vacaciones]').bind('click', function(event){
        var $No_Empleado = $('input[id$=Txt_No_Empleado]');
        
        if($No_Empleado.val() == '' || $No_Empleado.val() == 'No Empleado'){
            $No_Empleado.val('');
            event.preventDefault();
        }
     });
}

function Contar_Caracteres_Comentarios_Autoriazacion_Vacaciones_Dependencia(){
    $('textarea[id$=Txt_Comentarios]').keyup(function() {
        var Caracteres =  $(this).val().length;
        
        if (Caracteres > 250) {
            this.value = this.value.substring(0, 250);
            $(this).css("background-color", "Yellow");
            $(this).css("color", "Red");
        }else{
            $(this).css("background-color", "White");
            $(this).css("color", "Black");
        }
        
        $('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');
    });
}

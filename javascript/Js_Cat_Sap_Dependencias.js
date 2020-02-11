var Pagina;
Pagina=$(document);
Pagina.ready(Inicializar_Eventos_Cat_Dependencias);

function Inicializar_Eventos_Cat_Dependencias()
{
    Contar_Caracteres_Comentarios_Dependencias();
    Validar_Clave();
    
    $('input[id$=Btn_Agregar_Fte_Financiamiento]').hover(
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
    $('input[id$=Btn_Agregar_Programa]').hover(
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

function Contar_Caracteres_Comentarios_Dependencias(){
    $('textarea[id$=Txt_Comentarios_Dependencia]').keyup(function() {
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

function Validar_Clave(){
    $('input[id$=Txt_Clave_Dependecia]').blur(function(e){    
        try {
            if($(this).val().length != 5){
                    $(this).val('');
                    $(this).css("background-color", "Yellow");  
                    $('#Mensaje').text('Clave Invalida');                  
            }else if($(this).val().length == 5){
                    $(this).css("background-color", "White");
                    $('#Mensaje').text('');
            }     
        }catch(e){
            alert(e);
        }
    });
}
var Pagina;
Pagina=$(document);
Pagina.ready(Inicializar_Eventos_Dias_Festivos);

function Inicializar_Eventos_Dias_Festivos()
{
    Contar_Caracteres_Comentarios_Dias_Festivos();
}

function Contar_Caracteres_Comentarios_Dias_Festivos(){
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
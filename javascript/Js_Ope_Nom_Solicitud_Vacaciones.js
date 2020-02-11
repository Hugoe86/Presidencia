var Pagina;
Pagina=$(document);
Pagina.ready(Inicializar_Eventos_Solicitud_Vacaciones);

function Inicializar_Eventos_Solicitud_Vacaciones()
{
    var Ceros = "";
    
    Contar_Caracteres_Comentarios_Solicitud_Vacacione();
    
    $("input[id$=Txt_No_Empleado]").bind("blur", function(){
        for(i=0;    i<(6-$(this).val().length);    i++){
            Ceros += '0';
        }
        $(this).val(Ceros + $(this).val());
        Ceros = "";
    });
}

function Contar_Caracteres_Comentarios_Solicitud_Vacacione(){
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

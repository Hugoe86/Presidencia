var Pagina;
Pagina=$(document);
Pagina.ready(Inicializar_Eventos_Faltas_Empleados);

function Inicializar_Eventos_Faltas_Empleados()
{
    Contar_Caracteres_Comentarios_Faltas_Empleados();
    
     $('input[id$=Txt_Cantidad_Descontar]').css('background-image', 'url(../imagenes/gridview/economico.png)');
     $('input[id$=Txt_Cantidad_Descontar]').css('background-repeat', 'no-repeat');
     $('input[id$=Txt_Cantidad_Descontar]').css('background-position', 'left');    
     $('input[id$=Txt_Cantidad_Descontar]').css('border-style', 'outset');
     $('input[id$=Txt_Cantidad_Descontar]').css('border-color', 'White');
     $('input[id$=Txt_Cantidad_Descontar]').css('font-family', 'Courier New');
     $('input[id$=Txt_Cantidad_Descontar]').css('font-size', '24px');
     $('input[id$=Txt_Cantidad_Descontar]').css('font-weight', 'bold');
     $('input[id$=Txt_Cantidad_Descontar]').css('color', '#2F4E7D');
     $('input[id$=Txt_Cantidad_Descontar]').css('height', '24px');   
     $('input[id$=Txt_Cantidad_Descontar]').css('width', '98%');   
     $('input[id$=Txt_Cantidad_Descontar]').css('text-align', 'center');   
}

function Contar_Caracteres_Comentarios_Faltas_Empleados(){
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
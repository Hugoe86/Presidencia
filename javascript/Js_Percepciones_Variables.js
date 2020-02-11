var Pagina;
Pagina=$(document);
Pagina.ready(Inicializar_Eventos_Percepciones_Variables);

function Inicializar_Eventos_Percepciones_Variables()
{
    Contar_Caracteres_Comentarios_Percepciones_Var();
    
    
     $('input[id$=Txt_Empleados]').css('background-image', 'url(../imagenes/paginas/empleado.png)');
     $('input[id$=Txt_Empleados]').css('background-repeat', 'no-repeat');
     $('input[id$=Txt_Empleados]').css('background-position', 'left');    
     $('input[id$=Txt_Empleados]').css('border-style', 'Solid');
     $('input[id$=Txt_Empleados]').css('border-color', '#2F4E7D');
     $('input[id$=Txt_Empleados]').css('font-family', 'Tahoma');
     $('input[id$=Txt_Empleados]').css('font-size', '12px');
     $('input[id$=Txt_Empleados]').css('font-weight', 'bold');
     $('input[id$=Txt_Empleados]').css('color', '#2F4E7D');
     $('input[id$=Txt_Empleados]').css('height', '24px');
     $('input[id$=Txt_Empleados]').css('text-align', 'center');   
     $('input[id$=Txt_Empleados]').click(function(){ $(this).focus().select(); });
     $('input[id$=Txt_Empleados]').keyup(function(){$(this).css('background-color', '#FFFF99');});
     $('input[id$=Txt_Empleados]').blur(function(){$(this).css('background-color', 'White');});
   
    
     $('input[id$=Txt_Cantidad]').css('background-image', 'url(../imagenes/gridview/economico.png)');
     $('input[id$=Txt_Cantidad]').css('background-repeat', 'no-repeat');
     $('input[id$=Txt_Cantidad]').css('background-position', 'left');    
     $('input[id$=Txt_Cantidad]').css('border-style', 'Solid');
     $('input[id$=Txt_Cantidad]').css('border-color', '#2F4E7D');
     $('input[id$=Txt_Cantidad]').css('font-family', 'Courier');
     $('input[id$=Txt_Cantidad]').css('font-size', '24px');
     $('input[id$=Txt_Cantidad]').css('font-weight', 'bold');
     $('input[id$=Txt_Cantidad]').css('color', '#2F4E7D');
     $('input[id$=Txt_Cantidad]').css('height', '24px');
     $('input[id$=Txt_Cantidad]').click(function(){ $(this).focus().select(); });
     $('input[id$=Txt_Cantidad]').keyup(function(){$(this).css('background-color', '#FFFF99');});
     $('input[id$=Txt_Cantidad]').blur(function(){$(this).css('background-color', 'White');});

    $('input[id$=Btn_Agregar_Empleado]').css('background', 'url(../imagenes/paginas/sias_add.png)');
    $('input[id$=Btn_Agregar_Empleado]').css('background-repeat', 'no-repeat');
    $('input[id$=Btn_Agregar_Empleado]').css('background-position', 'left');
    $('input[id$=Btn_Agregar_Empleado]').css('height', '27px');
    $('input[id$=Btn_Agregar_Empleado]').css('cursor', 'hand');
    $('input[id$=Btn_Agregar_Empleado]').css('font-family', 'Comic Sans MS');
    $('input[id$=Btn_Agregar_Empleado]').css('font-size', '13px');
    $('input[id$=Btn_Agregar_Empleado]').css('font-weight', 'bold');
    $('input[id$=Btn_Agregar_Empleado]').css('color', '#2F4E7D');
}

function Contar_Caracteres_Comentarios_Percepciones_Var(){
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
var Pagina;
Pagina=$(document);
Pagina.ready(inicializarEventos_Generacion_Nomina);

function inicializarEventos_Generacion_Nomina()
{
//  Estilo_Botones_Dinamicos();

  var Pagina=$("#Img_Irapuato");
  Pagina.hover(Cambiar_Imagen_In, Cambiar_Imagen_Out);
  Pagina.mousedown(presionaMouse);
  Pagina.mouseup(sueltaMouse);
  
     $('input[id$=Btn_Generar_Nomina]').hover(
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
     
     $('select[id$=Cmb_Tipo_Nomina]').bind('change', function(){
        Cmb_Tipo_Nomina_SelectedIndexChanged();
     });
     
     $('input[id*=Txt_Inicia_Catorcena]').css('background-image', 'url(../imagenes/gridview/grid_calendar.png)');
     $('input[id*=Txt_Inicia_Catorcena]').css('background-repeat', 'no-repeat');
     $('input[id*=Txt_Inicia_Catorcena]').css('background-position', 'left');    
     $('input[id*=Txt_Inicia_Catorcena]').css('border-style', 'outset');
     $('input[id*=Txt_Inicia_Catorcena]').css('font-family', 'Tahoma');
     $('input[id*=Txt_Inicia_Catorcena]').css('font-size', '12px');
     $('input[id*=Txt_Inicia_Catorcena]').css('font-weight', 'bold');
     $('input[id*=Txt_Inicia_Catorcena]').css('color', '#2F4E7D');
     $('input[id*=Txt_Inicia_Catorcena]').css('height', '24px');
     $('input[id*=Txt_Inicia_Catorcena]').css('text-align', 'center');   
     $('input[id*=Txt_Inicia_Catorcena]').click(function(){ $(this).focus().select(); });
     $('input[id*=Txt_Inicia_Catorcena]').keyup(function(){$(this).css('background-color', '#FFFF99');});
     $('input[id*=Txt_Inicia_Catorcena]').blur(function(){$(this).css('background-color', 'White');});     
     
     $('input[id*=Txt_Fin_Catorcena]').css('background-image', 'url(../imagenes/gridview/grid_calendar.png)');
     $('input[id*=Txt_Fin_Catorcena]').css('background-repeat', 'no-repeat');
     $('input[id*=Txt_Fin_Catorcena]').css('background-position', 'left');    
     $('input[id*=Txt_Fin_Catorcena]').css('border-style', 'outset');
     $('input[id*=Txt_Fin_Catorcena]').css('font-family', 'Tahoma');
     $('input[id*=Txt_Fin_Catorcena]').css('font-size', '12px');
     $('input[id*=Txt_Fin_Catorcena]').css('font-weight', 'bold');
     $('input[id*=Txt_Fin_Catorcena]').css('color', '#2F4E7D');
     $('input[id*=Txt_Fin_Catorcena]').css('height', '24px');
     $('input[id*=Txt_Fin_Catorcena]').css('text-align', 'center');   
     $('input[id*=Txt_Fin_Catorcena]').click(function(){ $(this).focus().select(); });
     $('input[id*=Txt_Fin_Catorcena]').keyup(function(){$(this).css('background-color', '#FFFF99');});
     $('input[id*=Txt_Fin_Catorcena]').blur(function(){$(this).css('background-color', 'White');});        
}

function Cambiar_Imagen_Out()
{
  var Pagina=$("#Img_Irapuato");
  Pagina.attr("src","../imagenes/paginas/nomina_personal.jpg");
  Pagina.attr("width", "350");
  Pagina.attr("height", "180");
  Pagina.css("background-color", "Silver");
  Pagina.css("border-style", "none");
}

function Cambiar_Imagen_In()
{
  var Pagina=$("#Img_Irapuato");
  Pagina.attr("src","../imagenes/paginas/nomina_personal.jpg");
  Pagina.attr("width", "350");
  Pagina.attr("height", "180");
  Pagina.css("background-color", "White");
  Pagina.css("border-style", "none");
}

function presionaMouse()
{
  var Pagina=$("#Img_Irapuato");
  Pagina.attr("src","../imagenes/paginas/nomina_personal.jpg");
  Pagina.attr("width", "350");
  Pagina.attr("height", "180");
  Pagina.css("background-color", "Silver");
  Pagina.css("border-style", "none");
  Pagina.css("vertical-align:", "middle");
}

function sueltaMouse()
{
  var Pagina=$("#Img_Irapuato");
  Pagina.attr("src","../imagenes/paginas/nomina_personal.jpg");
  Pagina.attr("width", "350");
  Pagina.attr("height", "180");
  Pagina.css("background-color", "White");
  Pagina.css("border-style", "none");
  Pagina.css("vertical-align:", "middle");
}


function OnSelectedIndexChanged(){
    var Fecha_Inicio = $('input[id$=Txt_Inicia_Catorcena]').val();
    var Fecha_Fin = $('input[id$=Txt_Fin_Catorcena]').val();
    
    if(Fecha_Inicio == '' || Fecha_Fin == ''){
        return false;
    }
}

function Cmb_Tipo_Nomina_SelectedIndexChanged(){
    if($('select[id$=Cmb_Tipo_Nomina]').val() == '') {
        $('input[id$=Txt_Inicia_Catorcena]').val('');
        $('input[id$=Txt_Fin_Catorcena]').val('');
        
        $('select[id$=Cmb_Calendario_Nomina]').val('');
        $("select[id$=Cmb_Periodo] option[value='']").attr("selected",true);
        $('select[id$=Cmb_Periodo]').children().remove();
        
        $('table.tbl :radio:checked').each(function (){
            $(this).attr('checked', false);
        });
        
        location.reload();
    }
}

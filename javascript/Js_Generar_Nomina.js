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
}

function Cambiar_Imagen_Out()
{
  var Pagina=$("#Img_Irapuato");
  Pagina.attr("src","../imagenes/paginas/escudo.jpg");
  Pagina.attr("width", "180");
  Pagina.attr("height", "200");
  Pagina.css("background-color", "Silver");
  Pagina.css("border-style", "none");
}

function Cambiar_Imagen_In()
{
  var Pagina=$("#Img_Irapuato");
  Pagina.attr("src","../imagenes/paginas/fundadores.png");
  Pagina.attr("width", "500");
  Pagina.attr("height", "200");
  Pagina.css("background-color", "White");
  Pagina.css("border-style", "none");
}

function presionaMouse()
{
  var Pagina=$("#Img_Irapuato");
  Pagina.attr("src","../imagenes/paginas/sias_logo_irapuato.jpg");
  Pagina.attr("width", "141");
  Pagina.attr("height", "50");
  Pagina.css("background-color", "Silver");
  Pagina.css("border-style", "none");
  Pagina.css("vertical-align:", "middle");
}

function sueltaMouse()
{
  var Pagina=$("#Img_Irapuato");
  Pagina.attr("src","../imagenes/paginas/fundadores.png");
  Pagina.attr("width", "500");
  Pagina.attr("height", "200");
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

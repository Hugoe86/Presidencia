
var renglon_agregar = null;
var estado;
$(function () {

    function format(item) {
        return item.nombre;

    }

    $("#Txt_Estado").autocomplete("../../Controlador/Controlador_Busquedas.aspx", {
        extraParams: { nivel: '5' },
        dataType: "json",
        parse: function (data) {
            return $.map(data, function (row) {
                return {
                    data: row,
                    value: row.estado_id,
                    result: row.nombre
                }
            });
        },
        formatItem: function (item) {
            return format(item);

        }
    }).result(function (e, item) {
        estado = item.estado_id;
        $("#Txt_Estado").attr('disabled', true);
        $("#Txt_Ciudad").val('');
        obtener_Ciudades();
   
    });



});

function obtener_Ciudades() {
    $.ajax({
        type: "POST",
        url: "../../Controlador/Controlador_Busquedas.aspx?Nivel=6&Estado=" + estado,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#Txt_Estado").attr('disabled', false);
            //$("#Txt_Ciudad").focus();
 
            escribir_Ciudades(response);
            
        }
    });
}


function escribir_Ciudades(Ciudades) {
    $("#Txt_Ciudad").autocomplete(Ciudades, {
        minChars: 0,
        width: 310,
        matchContains: "word",
        autoFill: false,
        formatItem: function (row, i, max) {
            return row.nombre;
        },
        formatMatch: function (row, i, max) {
            return row.nombre + " " + row.ciudad_id;
        },
        formatResult: function (row) {
            return row.nombre;
            
        }
    });
}

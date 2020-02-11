// <reference path="../easyui/jquery-1.4.2-vsdoc.js" />

$.extend({
    password: function(length, special) {
        var iteration = 0;
        var password = "";
        var randomNumber;
        if (special == undefined) {
            var special = false;
        }
        while (iteration < length && password.length < length) {
            randomNumber = (Math.floor((Math.random() * 100)) % 94) + 33;
            if (!special) {
                if ((randomNumber >= 33) && (randomNumber <= 47)) { continue; }
                if ((randomNumber >= 58) && (randomNumber <= 64)) { continue; }
                if ((randomNumber >= 91) && (randomNumber <= 96)) { continue; }
                if ((randomNumber >= 123) && (randomNumber <= 126)) { continue; }
            }
            iteration++;
            password += randomNumber;
        }

        var new_password = password.substring(0, 8);
        
        if (Existe_Clave(new_password)) {
            $.password(8);
        }
        return new_password;
    }
});

var PSM = 1.35685686;
///***************************************************************************************************************************************
///********************************************************* INICIO **********************************************************************
///***************************************************************************************************************************************
var DOM_Pagina = $(document);
DOM_Pagina.ready(function() {
    Configuracion_Inicial();
});

function StringBuilder(value) {
    this.strings = new Array("");
    this.append(value);
}

StringBuilder.prototype.append = function(value) {
    if (value) {
        this.strings.push(value);
    }
}

StringBuilder.prototype.clear = function() {
    this.strings.length = 1;
}

StringBuilder.prototype.toString = function() {
    return this.strings.join("");
}


function Configuracion_Inicial() {
    Cargar_Puestos();
    Cargar_Unidades_Responsables('Cmb_Unidades_Reponsables');
    Cargar_Unidades_Responsables_2('Cmb_S_Dependencia');
    Cargar_Unidades_Responsables_2('Cmb_PSM_Dependencia');

    Cargar_Area_Funcional('Cmb_S_Area_Funcional');
    Cargar_Area_Funcional('Cmb_PSM_Area_Funcional');
    Eventos();
    Estilos();

    Habilitar_Controles();
    Configuracion_Calendario();
}

function Cargar_Puestos() {
    var $Cmb_Puesto = $('#Cmb_Puestos').get(0);

    try {
        $.ajax({
            url: "Frm_Cat_Nom_Creacion_Plazas_Ctrl.aspx?Tabla=PUESTOS&Opcion=cmb_puestos",
            type: 'POST',
            async: false,
            cache: false,
            dataType: 'json',
            success: function(Datos) {
                if (Datos != null) {
                    $Cmb_Puesto.options[0] = new Option('<- Seleccione ->', '');
                    $.each(Datos.PUESTOS, function(Contador, Elemento) {
                        $Cmb_Puesto.options[++Contador] = new Option(Elemento.NOMBRE +
                    "    Salario Mensual: $" + parseFloat(Elemento.SALARIO_MENSUAL).toFixed(2) +
                         "    Aplica PSM: " + ((Elemento.APLICA_PSM == 'S') ? "SI" : "NO"), Elemento.PUESTO_ID);
                    });
                }
                else {
                    alert("No hay puestos en el sistema en el sistema");
                }
            }
        });
    } catch (e) {
        alert(e);
    }
}

function Aplica_Puesto_PSM(puesto_id) {
    try {
        var $puesto_id = $('#Cmb_Puestos :selected').val();

        if ($puesto_id != '' && $puesto_id != undefined && $puesto_id != NaN) {
            $.ajax({
                url: "Frm_Cat_Nom_Creacion_Plazas_Ctrl.aspx?Tabla=PUESTOS&Opcion=consultar_puesto&puesto_id=" + puesto_id,
                type: 'POST',
                async: false,
                cache: false,
                dataType: 'json',
                success: function(INF_PUESTO) {
                    if (INF_PUESTO != null) {
                        $.each(INF_PUESTO.PUESTOS, function(Contador, Elemento) {

                            $('#SAP_Codigos_Programaticos_Sueldo [id*=Cmb_]').removeAttr('disabled');

                            $('#Span_SD').text('Salario Mensual: $' + Formato_Moneda(parseFloat(Elemento.SALARIO_MENSUAL).toFixed(2)));
                            $('#Span_S').text('Salario Diario: $' + Formato_Moneda((parseFloat(Elemento.SALARIO_MENSUAL) / 30.42).toFixed(2)));

                            if (Elemento.APLICA_PSM == 'S') {
                                $('#SAP_Codigos_Programaticos_PSM [id*=Cmb_]').removeAttr('disabled');

                                $('#Span_PSM_Diaria').text('PSM Diario: $' + Formato_Moneda((parseFloat(parseFloat(Elemento.SALARIO_MENSUAL) * PSM) / 30.42).toFixed(2)));
                            } else {
                                $('#SAP_Codigos_Programaticos_PSM [id*=Cmb_]').attr('disabled', 'disabled');
                            }
                        });
                    }
                    else {
                        alert("No se encontraron resultados para el puesto buscado.");
                    }
                }
            });
        }
    } catch (e) {
        alert(e);
    }
}

function Cargar_Fte_Financiamiento(ctrl_id, unidad_responsable_id) {
    var $Cmb_Fte_Financiamiento = $('#' + ctrl_id).get(0);

    try {
        $.ajax({
            url: "Frm_Cat_Nom_Creacion_Plazas_Ctrl.aspx?Tabla=FTE_FIN&Opcion=cmb_fte_financiamiento&unidad_responsable_id=" + unidad_responsable_id,
            type: 'POST',
            async: false,
            cache: false,
            dataType: 'json',
            success: function(Datos) {
                if (Datos != null) {
                    $Cmb_Fte_Financiamiento.options[0] = new Option('<- Seleccione ->', '');
                    $.each(Datos.FTE_FIN, function(Contador, Elemento) {
                    $Cmb_Fte_Financiamiento.options[++Contador] = new Option(Elemento.FTE_FINANCIAMIENTO.replace('[', '').replace(']', '').replace('-', ').-'), Elemento.FUENTE_FINANCIAMIENTO_ID);
                    });
                }
                else {
                    alert("No hay fte financiamiento en el sistema");
                }
            }
        });
    } catch (e) {
        alert(e);
    }
}

function Cargar_Area_Funcional(ctrl_id) {
    var $Cmb_Area_Funcional = $('#' + ctrl_id).get(0);

    try {
        $.ajax({
            url: "Frm_Cat_Nom_Creacion_Plazas_Ctrl.aspx?Tabla=AREA_FUNCIONAL&Opcion=cmb_area_funcional",
            type: 'POST',
            async: false,
            cache: false,
            dataType: 'json',
            success: function(Datos) {
                if (Datos != null) {
                    $Cmb_Area_Funcional.options[0] = new Option('<- Seleccione ->', '');
                    $.each(Datos.AREA_FUNCIONAL, function(Contador, Elemento) {
                        $Cmb_Area_Funcional.options[++Contador] = new Option(Elemento.CLAVE_NOMBRE.replace('[', '').replace(']', '').replace('-', ').-'), Elemento.AREA_FUNCIONAL_ID);
                    });
                }
                else {
                    alert("No hay areas funcionales en el sistema");
                }
            }
        });
    } catch (e) {
        alert(e);
    }
}

function Seleccionar_Area_Funcional_UR(unidad_responsable_id) {
    try {
        $.ajax({
            url: "Frm_Cat_Nom_Creacion_Plazas_Ctrl.aspx?Opcion=buscar_area_funcional&unidad_responsable_id=" + unidad_responsable_id,
            type: 'POST',
            async: false,
            cache: false,
            dataType: 'text',
            success: function(AREA_FUNCIONAL_ID) {
                if (AREA_FUNCIONAL_ID != null) {
                    $('#Cmb_S_Area_Funcional').val(AREA_FUNCIONAL_ID);

                    $('#Cmb_PSM_Area_Funcional').val(AREA_FUNCIONAL_ID);
                }
                else {
                    alert("No hay un area funcional en el sistema para unidad responsable seleccionada.");
                }
            }
        });
    } catch (e) {
        alert(e);
    }
}

function Cargar_Programa(ctrl_id, unidad_responsable_id) {
    var $Cmb_Proyecto_Programa = $('#' + ctrl_id).get(0);

    try {
        $.ajax({
            url: "Frm_Cat_Nom_Creacion_Plazas_Ctrl.aspx?Tabla=PROGRAMA&Opcion=cmb_proyecto_programa&unidad_responsable_id=" + unidad_responsable_id,
            type: 'POST',
            async: false,
            cache: false,
            dataType: 'json',
            success: function(Datos) {
                if (Datos != null) {
                    $Cmb_Proyecto_Programa.options[0] = new Option('<- Seleccione ->', '');
                    $.each(Datos.PROGRAMA, function(Contador, Elemento) {
                        $Cmb_Proyecto_Programa.options[++Contador] = new Option(Elemento.PROYECTO_PROGRAMA.replace('[', '').replace(']', '').replace('-', ').-'), Elemento.PROYECTO_PROGRAMA_ID);
                    });
                }
                else {
                    alert("No hay programas en el sistema");
                }
            }
        });
    } catch (e) {
        alert(e);
    }
}

function Cargar_Unidades_Responsables(ctrl_id) {
    var $Cmb_UR = $('#' + ctrl_id).get(0);

    try {

        $('#' + ctrl_id).combobox({
            url: "Frm_Cat_Nom_Creacion_Plazas_Ctrl.aspx?Tabla=UR&Opcion=cmb_unidades_responsables",
            valueField: 'DEPENDENCIA_ID',
            textField: 'CLAVE_NOMBRE',
            mode: 'remote',
            method: 'post',
            onSelect: function(record) {
                if (record.DEPENDENCIA_ID != undefined) {
                    Limpiar_Controles();

                    Cargar_Fte_Financiamiento('Cmb_S_Fte_Finaciamiento', record.DEPENDENCIA_ID);
                    Cargar_Area_Funcional('Cmb_S_Area_Funcional');
                    Cargar_Programa('Cmb_S_Programa', record.DEPENDENCIA_ID);
                    Cargar_Unidades_Responsables_2('Cmb_S_Dependencia');

                    Cargar_Fte_Financiamiento('Cmb_PSM_Fte_Financiamiento', record.DEPENDENCIA_ID);
                    Cargar_Area_Funcional('Cmb_PSM_Area_Funcional');
                    Cargar_Programa('Cmb_PSM_Programa', record.DEPENDENCIA_ID);
                    Cargar_Unidades_Responsables_2('Cmb_PSM_Dependencia');

                    //Seleccionamos el area funcional que tiene asignada la unidad responsable.
                    Seleccionar_Area_Funcional_UR(record.DEPENDENCIA_ID);

                    $('#Cmb_S_Dependencia').val(record.DEPENDENCIA_ID);
                    $('#Cmb_PSM_Dependencia').val(record.DEPENDENCIA_ID);
                    LLenar_Tabla(record.DEPENDENCIA_ID);
                }
            }
        });
    } catch (e) {
        alert(e);
    }
}

function Cargar_Unidades_Responsables_2(ctrl_id) {
    var $Cmb_UR = $('#' + ctrl_id).get(0);

    try {
        $.ajax({
            url: "Frm_Cat_Nom_Creacion_Plazas_Ctrl.aspx?Tabla=UR&Opcion=cmb_unidades_responsables_2",
            type: 'POST',
            async: false,
            cache: false,
            dataType: 'json',
            success: function(Datos) {
                if (Datos != null) {
                    $Cmb_UR.options[0] = new Option('<- Seleccione ->', '');
                    $.each(Datos.UR, function(Contador, Elemento) {
                        $Cmb_UR.options[++Contador] = new Option(Elemento.CLAVE_NOMBRE, Elemento.DEPENDENCIA_ID);
                    });
                }
                else {
                    alert("No hay unidades responsables en el sistema en el sistema");
                }
            }
        });
    } catch (e) {
        alert(e);
    }
}

function Cargar_Partida(ctrl_id, proyecto_programa_id) {
    var $Cmb_Partidas = $('#' + ctrl_id).get(0);

    try {
        $.ajax({
            url: "Frm_Cat_Nom_Creacion_Plazas_Ctrl.aspx?Tabla=PARTIDAS&Opcion=cmb_partida&proyecto_programa_id=" + proyecto_programa_id,
            type: 'POST',
            async: false,
            cache: false,
            dataType: 'json',
            success: function(Datos) {
                if (Datos != null) {
                    $Cmb_Partidas.options[0] = new Option('<- Seleccione ->', '');
                    $.each(Datos.PARTIDAS, function(Contador, Elemento) {
                        $Cmb_Partidas.options[++Contador] = new Option(Elemento.PARTIDA.replace('[', '').replace(']', '').replace('-', ').-'), Elemento.PARTIDA_ID);
                    });
                }
                else {
                    alert("No hay partidas para el programa seleccionado");
                }
            }
        });
    } catch (e) {
        alert(e);
    }
}

///********************************************************************************************************
///NOMBRE DE LA FUNCIÓN : LLenar_Tabla
///DESCRIPCIÓN          : Funcion para llenar el grid.
///PROPIEDADES          :
///CREO                 : Juan Alberto Hernandez Negrete
///FECHA_CREO           : Marzo/2012
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN...:
///*********************************************************************************************************
function LLenar_Tabla(dependencia_id) {
    var Parametros = new StringBuilder();

    Parametros.append('unidad_responsable_id=');
    Parametros.append(dependencia_id);

    Parametros.append('&tabla=');
    Parametros.append("rows");
    Parametros.append('&opcion=');
    Parametros.append('consultar_plazas_x_dependencia');

    //inicializamos el grid
    $('#Tbl_Plazas').datagrid({
        title: 'Relación Unidades Responsables - Plazas',
        iconCls: 'icon-save',
        width: 600,
        height: 400,
        collapsible: true,
        url: 'Frm_Cat_Nom_Creacion_Plazas_Ctrl.aspx?' + Parametros.toString(),
        toolbar: [{
            text: 'Eliminar Plaza',
            iconCls: 'icon-remove',
            handler: function() {
                var row = $('#Tbl_Plazas').datagrid('getSelected');
                if (row) {
                    $.messager.confirm('Confirmar', 'Desea eliminar la plaza?', function(r) {
                        if (r) {
                            Eliminar_Plaza(row.DEPENDENCIA_ID, row.PUESTO_ID, row.CLAVE);
                            LLenar_Tabla($('#Cmb_Unidades_Reponsables').combobox('getValue'));
                        }
                    });
                }
            }
        },
        {
            text: 'Agregar Plaza',
            iconCls: 'icon-add',
            handler: function() {
                $.messager.confirm('Información Confirmación', 'Esta seguro de crear la plaza?', function(r) {
                    if (r) {
                        Validar_Puesto_Contra_Presupuesto();
                        LLenar_Tabla($('#Cmb_Unidades_Reponsables').combobox('getValue'));
                    }
                });
            }
}],
            columns:
        [[
            { field: 'DEPENDENCIA_ID', title: '', hidden: true, width: 0, sortable: true, resizable: true,
                formatter: function(value) {
                    return '<span style="font-size:x-small">' + value + '</span>';
                }
            },
            { field: 'PUESTO_ID', title: '', hidden: true, width: 0, sortable: true, resizable: true,
                formatter: function(value) {
                    return '<span style="font-size:x-small">' + value + '</span>';
                }
            },
            { field: 'NOMBRE', title: 'Puesto', width: 300, sortable: true, resizable: true,
                formatter: function(value) {
                    return '<span style="font-size:x-small">' + value + '</span>';
                }
            },
            { field: 'SALARIO_MENSUAL', title: 'Salario Mensual', width: 100, sortable: true, resizable: true,
                formatter: function(value) {

                    Number.prototype.formatMoney = function(c, d, t) {
                        var n = this, c = isNaN(c = Math.abs(c)) ? 2 : c, d = d == undefined ? "," : d, t = t == undefined ? "." : t, s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
                        return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
                    };

                    return '<span style="font-size:x-small">' + "$" + parseFloat(value).formatMoney(2, '.', ',') + '</span>';
                }
            },
            { field: 'APLICA_PSM', title: 'PSM', width: 50, sortable: true, resizable: true,
                formatter: function(value) {
                    return '<span style="font-size:x-small; text-align:center;">' + value + '</span>';
                }
            },
            { field: 'ESTATUS', title: 'Estatus', width: 80, sortable: true, resizable: true,
                formatter: function(value) {
                    return '<span style="font-size:x-small">' + value + '</span>';
                }
            },
            { field: 'CLAVE', title: 'Clave', width: 50, sortable: true, resizable: true,
                formatter: function(value) {
                    return '<span style="font-size:x-small">' + value + '</span>';
                }
            }
        ]],
            pagination: true,
            rownumbers: false,
            idField: 'DEPENDENCIA_ID',
            sortOrder: 'asc',
            remoteSort: false,
            singleSelect: true,
            loadMsg: "Cargando, espere ...",
            pageNumber: 1,
            pageSize: 20,
            fitColumns: false,
            striped: true,
            nowrap: false,
            pageList: [10, 20, 50, 100],
            remoteSort: false,
            showFooter: true,
            onClickRow: function(rowIndex, rowData) {
            }
        });
    }

    function Eventos() {
        //        $("#Cmb_Unidades_Reponsables").bind("select", function() {
        //            Limpiar_Controles();

        //            Cargar_Fte_Financiamiento('Cmb_S_Fte_Finaciamiento', $(this).val());
        //            Cargar_Area_Funcional('Cmb_S_Area_Funcional');
        //            Cargar_Programa('Cmb_S_Programa', $(this).val());
        //            Cargar_Unidades_Responsables('Cmb_S_Dependencia');

        //            Cargar_Fte_Financiamiento('Cmb_PSM_Fte_Financiamiento', $(this).val());
        //            Cargar_Area_Funcional('Cmb_PSM_Area_Funcional');
        //            Cargar_Programa('Cmb_PSM_Programa', $(this).val());
        //            Cargar_Unidades_Responsables('Cmb_PSM_Dependencia');

        //            //Seleccionamos el area funcional que tiene asignada la unidad responsable.
        //            Seleccionar_Area_Funcional_UR($(this).val());

        //            $('#Cmb_S_Dependencia').val($(this).val());
        //            $('#Cmb_PSM_Dependencia').val($(this).val());
        //            LLenar_Tabla($(this).val());
        //        });

        $("#Cmb_S_Programa").bind("change keyup", function() {
            $('#Cmb_S_Partida').find('option').remove();
            //Cargamos las partidas que tiene asignadas la unidad responsable.
            Cargar_Partida('Cmb_S_Partida', $(this).val());
        });

        $("#Cmb_PSM_Programa").bind("change keyup", function() {
            $('#Cmb_PSM_Partida').find('option').remove();
            //Cargamos las partidas que tiene asignadas la unidad responsable.
            Cargar_Partida('Cmb_PSM_Partida', $(this).val());
        });

        $("#Cmb_Puestos").bind("change keyup", function() {
            $('#Span_SD').empty();
            $('#Span_S').empty();
            $('#Span_PSM_Diaria').empty();

            $('#Txt_Proyeccion').val('0.00');
            $('#Txt_Proyeccion_PSM').val('0.00');

            //Cargamos las partidas que tiene asignadas la unidad responsable.
            Aplica_Puesto_PSM($(this).val());

            $('[id*=Area_Funcional]').attr('disabled', 'disabled');
            $('[id*=Dependencia]').attr('disabled', 'disabled');
        });

        $("#Cmb_S_Partida").bind("change keyup", function() {
            Consultar_Diponible_S();
        });

        $("#Cmb_PSM_Partida").bind("change keyup", function() {
            Consultar_Diponible_PSM();
        });

        $('#Link_Proyeccion').click(function(e) {
            var $puesto_id = $('#Span_SD').text();
            var $fecha = $('#Txt_Fecha').val();

            e.preventDefault();

            if (($puesto_id != '' && $puesto_id != undefined != $puesto_id != NaN) &&
            ($fecha != '' && $fecha != undefined && $fecha != NaN)) {
                Ejecutar_Proyeccion();
            } else {
                alert('Faltan ingresar algunos datos para poder realizar la operación.');
            }
        });

        //Temporal agregar plazas sin validar presupuestalmente.
        $('#Link_Validar').click(function(e) {
            e.preventDefault();
            Alta_Plaza();
            LLenar_Tabla($('#Cmb_Unidades_Reponsables').combobox('getValue'));
            //            LLenar_Tabla('');
            //            reset_all();
        });
    }

    function Estilos() {
        $('[id*=Cmb_]').each(function() {
            $(this).css('font-size', 'x-small');
            $(this).css('font-weight', 'bold');
            $(this).css('color', '#565656');
        });

        $('input[id$=Txt_Proyeccion]').css('background-image', 'url(../imagenes/gridview/economico.png)');
        $('input[id$=Txt_Proyeccion]').css('background-repeat', 'no-repeat');
        $('input[id$=Txt_Proyeccion]').css('background-position', 'left');
        $('input[id$=Txt_Proyeccion]').css('border-style', 'outset');
        $('input[id$=Txt_Proyeccion]').css('font-family', 'Courier');
        $('input[id$=Txt_Proyeccion]').css('font-size', '24px');
        $('input[id$=Txt_Proyeccion]').css('font-weight', 'bold');
        $('input[id$=Txt_Proyeccion]').css('color', '#2F4E7D');
        $('input[id$=Txt_Proyeccion]').css('height', '22px');
        $('input[id$=Txt_Proyeccion]').css('padding-left', '15px');
        $('input[id$=Txt_Proyeccion]').css('text-align', 'center');
        $('input[id$=Txt_Proyeccion]').click(function() { $(this).focus().select(); });
        $('input[id$=Txt_Proyeccion]').keyup(function() { $(this).css('background-color', '#FFFF99'); });
        $('input[id$=Txt_Proyeccion]').blur(function() { $(this).css('background-color', 'White'); });

        $('input[id$=Txt_Proyeccion_PSM]').css('background-image', 'url(../imagenes/gridview/economico.png)');
        $('input[id$=Txt_Proyeccion_PSM]').css('background-repeat', 'no-repeat');
        $('input[id$=Txt_Proyeccion_PSM]').css('background-position', 'left');
        $('input[id$=Txt_Proyeccion_PSM]').css('border-style', 'outset');
        $('input[id$=Txt_Proyeccion_PSM]').css('font-family', 'Courier');
        $('input[id$=Txt_Proyeccion_PSM]').css('font-size', '24px');
        $('input[id$=Txt_Proyeccion_PSM]').css('font-weight', 'bold');
        $('input[id$=Txt_Proyeccion_PSM]').css('color', '#2F4E7D');
        $('input[id$=Txt_Proyeccion_PSM]').css('height', '22px');
        $('input[id$=Txt_Proyeccion_PSM]').css('padding-left', '15px');
        $('input[id$=Txt_Proyeccion_PSM]').css('text-align', 'center');
        $('input[id$=Txt_Proyeccion_PSM]').click(function() { $(this).focus().select(); });
        $('input[id$=Txt_Proyeccion_PSM]').keyup(function() { $(this).css('background-color', '#FFFF99'); });
        $('input[id$=Txt_Proyeccion_PSM]').blur(function() { $(this).css('background-color', 'White'); });
    }

    function Habilitar_Controles() {
        $('[id*=SAP_Codigos_Programaticos] [id*=Cmb_]').each(function() {
            $(this).attr('disabled', 'disabled');
        });
    }

    function Limpiar_Controles() {
        $('[id*=SAP_Codigos_Programaticos] [id*=Cmb_]').each(function() {
            $(this).find('option').remove();
        });
    }

    function Consultar_Diponible_S() {
        var Parametros = new StringBuilder();

        Parametros.append('&unidad_responsable_id=');
        Parametros.append($('#Cmb_S_Dependencia :selected').val());
        Parametros.append('&fte_financiamiento=');
        Parametros.append($('#Cmb_S_Fte_Finaciamiento :selected').val());
        Parametros.append('&area_funcional=');
        Parametros.append($('#Cmb_S_Area_Funcional :selected').val());
        Parametros.append('&proyecto_programa_id=');
        Parametros.append($('#Cmb_S_Programa :selected').val());
        Parametros.append('&partida_id=');
        Parametros.append($('#Cmb_S_Partida :selected').val());

        try {
            $.ajax({
                url: "Frm_Cat_Nom_Creacion_Plazas_Ctrl.aspx?Opcion=consultar_disponible_codigo" + Parametros.toString(),
                type: 'POST',
                async: false,
                cache: false,
                dataType: 'text',
                success: function(DISPONIBLE) {
                    if (DISPONIBLE != null) {
                        $('#Span_Disponible').text(DISPONIBLE);
                    }
                    else {
                        alert("No se encontraron resultados en la consulta del disponible.");
                    }
                }
            });
        } catch (e) {
            alert(e);
        }
    }

    function Consultar_Diponible_PSM() {
        var Parametros = new StringBuilder();

        Parametros.append('&unidad_responsable_id=');
        Parametros.append($('#Cmb_PSM_Dependencia :selected').val());
        Parametros.append('&fte_financiamiento=');
        Parametros.append($('#Cmb_PSM_Fte_Financiamiento :selected').val());
        Parametros.append('&area_funcional=');
        Parametros.append($('#Cmb_PSM_Area_Funcional :selected').val());
        Parametros.append('&proyecto_programa_id=');
        Parametros.append($('#Cmb_PSM_Programa :selected').val());
        Parametros.append('&partida_id=');
        Parametros.append($('#Cmb_PSM_Partida :selected').val());

        try {
            $.ajax({
                url: "Frm_Cat_Nom_Creacion_Plazas_Ctrl.aspx?Opcion=consultar_disponible_codigo" + Parametros.toString(),
                type: 'POST',
                async: false,
                cache: false,
                dataType: 'text',
                success: function(DISPONIBLE) {
                    if (DISPONIBLE != null) {
                        $('#Span_Disponible_PSM').text(DISPONIBLE);
                    }
                    else {
                        alert("No se encontraron resultados en la consulta del disponible.");
                    }
                }
            });
        } catch (e) {
            alert(e);
        }
    }

    function Configuracion_Calendario() {
        try {
            //Prepara el DatePicker para seleccionar la fecha
            $("input[id$=Txt_Fecha]").daterangepicker();
            $("input[id$=Txt_Fecha]").daterangepicker({ arrows: true });
        } catch (e) {
            alert('Error al crear los calendarios de la página. Error: [' + e + ']');
        }
    }

    function Consultar_Dias_Restan_Anio() {
        var str_date = $('#Txt_Fecha').val().replace('/', '-').replace('/', '-');
        var fecha_inicio = customParse(str_date);
        var fecha_fin = new Date(fecha_inicio.getFullYear(), 11, 31);
        return days_between(fecha_inicio, fecha_fin);
    }

    function customParse(str) {
        var months = ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun',
                'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
      n = months.length, re = /(\d{2})-([a-z]{3})-(\d{4})/i, matches;

        while (n--) { months[months[n]] = n; } // map month names to their index :)

        matches = str.match(re); // extract date parts from string

        return new Date(matches[3], months[matches[2]], matches[1]);
    }

    function days_between(date1, date2) {
        // The number of milliseconds in one day
        var ONE_DAY = 1000 * 60 * 60 * 24;

        // Convert both dates to milliseconds
        var date1_ms = date1.getTime();
        var date2_ms = date2.getTime();

        // Calculate the difference in milliseconds
        var difference_ms = Math.abs(date1_ms - date2_ms);

        // Convert back to days and return
        return Math.round(difference_ms / ONE_DAY);
    }

    function Calculando_Proyeccion($salario_mensual, $dias_restan_anio) {
        var $dias_mes = 30.42;
        var $salario_diario = 0;
        var $proyeccion = 0;

        try {
            $salario_diario = parseFloat($salario_mensual / $dias_mes).toFixed(2);
            $proyeccion = ($salario_diario * $dias_restan_anio).toFixed(2);
            return $proyeccion;
        } catch (e) {
            alert(e);
        }
    }

    function Ejecutar_Proyeccion() {
        try {
            Number.prototype.formatMoney = function(c, d, t) {
                var n = this, c = isNaN(c = Math.abs(c)) ? 2 : c, d = d == undefined ? "," : d, t = t == undefined ? "." : t, s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
                return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
            };

            $('#Txt_Proyeccion').val('0.00');
            $('#Txt_Proyeccion_PSM').val('0.00');

            var $salario_mensual = $('#Span_SD').text().split(':')[1].replace('$', '');

            if ($salario_mensual != null && $salario_mensual != undefined) {
                $salario_mensual = $salario_mensual.replace(',', '');
                $salario_mensual = parseFloat($salario_mensual).toFixed(2);

                var $dias_restan_anio = Consultar_Dias_Restan_Anio();
                var $total_proyeccion = Calculando_Proyeccion($salario_mensual, $dias_restan_anio);
                var $proyeccion_psm = parseFloat(($total_proyeccion * PSM).toFixed(2)).formatMoney(2, '.', ',');

                //Asignas a la caja de texto el total de la proyección calculada de sueldo.
                $('#Txt_Proyeccion').val(parseFloat($total_proyeccion).formatMoney(2, '.', ','));

                //Valida si el puesto aplica para psm.
                if (Es_PSM()) {
                    //Asignas a la caja de texto el total de la proyección calculada de psm.
                    $('#Txt_Proyeccion_PSM').val($proyeccion_psm);
                }
            }
        } catch (e) {
            alert(e);
        }
    }

    function Formato_Moneda(cantidad) {
        try {
            Number.prototype.formatMoney = function(c, d, t) {
                var n = this, c = isNaN(c = Math.abs(c)) ? 2 : c, d = d == undefined ? "," : d, t = t == undefined ? "." : t, s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
                return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
            };
            return parseFloat(cantidad).formatMoney(2, '.', ',');
        } catch (e) { alert(e); } 
    }


    function Es_PSM() {
        var $puesto_id = $('#Cmb_Puestos :selected').val();
        var $aplica_psm = false;

        try {
            if ($puesto_id != '' && $puesto_id != undefined && $puesto_id != NaN) {
                $.ajax({
                    url: "Frm_Cat_Nom_Creacion_Plazas_Ctrl.aspx?Tabla=PUESTOS&Opcion=consultar_puesto&puesto_id=" + $puesto_id,
                    type: 'POST',
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function(INF_PUESTO) {
                        if (INF_PUESTO != null) {
                            $.each(INF_PUESTO.PUESTOS, function(Contador, Elemento) {
                                if (Elemento.APLICA_PSM == 'S') {
                                    $aplica_psm = true;
                                }
                            });
                        }
                        else {
                            alert("No se encontraron resultados para el puesto buscado.");
                        }
                    }
                });
            }
        } catch (e) {
            alert(e);
        } finally {return $aplica_psm;}
    }

    function Validar_Puesto_Contra_Presupuesto() {
        var Mensaje = new StringBuilder();
        var $proyeccion_sueldo = 0.0;
        var $proyeccion_psm = 0.0;
        var $presupuesto_sueldos = 0.0;
        var $presupuesto_psm = 0.0;

        Number.prototype.formatMoney = function(c, d, t) {
            var n = this, c = isNaN(c = Math.abs(c)) ? 2 : c, d = d == undefined ? "," : d, t = t == undefined ? "." : t, s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
        };

        try {
            if ($('#Txt_Proyeccion').val() != '' && $('#Txt_Proyeccion').val() != undefined) {
                $proyeccion_sueldo = parseFloat($('[id$=Txt_Proyeccion]').val().replace('$', '').replace(',', ''));
            }

            if ($('#Txt_Proyeccion_PSM').val() != '' && $('#Txt_Proyeccion_PSM').val() != undefined) {
                $proyeccion_psm = parseFloat($('[id$=Txt_Proyeccion_PSM]').val().replace('$', '').replace(',', ''));
            }

            if ($('#Span_Disponible').text() != '' && $('#Span_Disponible').text() != undefined) {
                $presupuesto_sueldos = parseFloat($('#Span_Disponible').text().replace('$', '').replace(',', ''));
            }

            if ($('#Span_Disponible_PSM').text() != '' && $('#Span_Disponible_PSM').text() != undefined) {
                $presupuesto_psm = parseFloat($('#Span_Disponible_PSM').text().replace('$', '').replace(',', ''));
            }

            if ($proyeccion_sueldo >= $presupuesto_sueldos) {
                Mensaje.append('Presupuesto disponible (SUELDOS) es insuficiente<br />Sueldo (Proyección): $' + parseFloat($proyeccion_sueldo).formatMoney(2, '.', ',') + '<br />Presupuesto (Sueldos): $' + parseFloat($presupuesto_sueldos).formatMoney(2, '.', ',') + "<br />");
            }

            if (Es_PSM()) {
                if ($proyeccion_psm >= $presupuesto_psm) {
                    Mensaje.append('_____________________________________<br/>');
                    Mensaje.append('El Presupuesto disponible (PSM) es insuficiente<br />PSM (Proyección): $' + parseFloat($proyeccion_psm).formatMoney(2, '.', ',') + '<br/>Presupuesto (PSM): $' + parseFloat($presupuesto_psm).formatMoney(2, '.', ','));
                }
            }

            if ($proyeccion_sueldo > 0 && ((Es_PSM()) ? ($proyeccion_psm > 0) : true)) {
                if (Mensaje != '' && Mensaje != undefined && Mensaje != NaN) {
                    $.messager.alert('Información Validación Presupuestal', Mensaje.toString());
                } else {
                    Alta_Plaza();
                }
            } else {
                $.messager.alert('Información:', 'No se ha realizado la proyección del sueldo ' + ((Es_PSM()) ? ' y de PSM ' : '') + ' de la plaza');
            }
        } catch (e) {

        }
    }

    function Alta_Plaza() {
        var $unidad_responsable = $('#Cmb_Unidades_Reponsables').combobox('getValue');
        var $puesto_id = $('#Cmb_Puestos :selected').val();
        var $tipo_plaza = $('#Cmb_Tipo_Paza :selected').text();
        var $estatus = 'DISPONIBLE';
        var $empleado_id = '';
        var $clave = $.password(8);
        var $s_fte_financiamiento = $('#Cmb_S_Fte_Finaciamiento :selected').val();
        var $s_area_funcional = $('#Cmb_S_Area_Funcional :selected').val();
        var $s_proyecto_programa = $('#Cmb_S_Programa :selected').val();
        var $s_unidad_responsable = $('#Cmb_S_Dependencia :selected').val();
        var $s_partida = $('#Cmb_S_Partida :selected').val();
        var $psm_fte_financiamiento = $('#Cmb_PSM_Fte_Financiamiento :selected').val();
        var $psm_area_funcional = $('#Cmb_PSM_Area_Funcional :selected').val();
        var $psm_proyecto_programa = $('#Cmb_PSM_Programa :selected').val();
        var $psm_unidad_responsable = $('#Cmb_PSM_Dependencia :selected').val();
        var $psm_partida = $('#Cmb_PSM_Partida :selected').val();

        var Parametros = new StringBuilder();

        Parametros.append('?unidad_responsable_id=');
        Parametros.append($unidad_responsable);
        Parametros.append('&puesto_id=');
        Parametros.append($puesto_id);
        Parametros.append('&tipo_plaza=');
        Parametros.append($tipo_plaza);
        Parametros.append('&estatus=');
        Parametros.append($estatus);
        Parametros.append('&empleado_id=');
        Parametros.append($empleado_id);
        Parametros.append('&clave=');
        Parametros.append($clave);
        Parametros.append('&s_fte_financiamiento_id=');
        Parametros.append($s_fte_financiamiento);
        Parametros.append('&s_area_funcional_id=');
        Parametros.append($s_area_funcional);
        Parametros.append('&s_proyecto_programa_id=');
        Parametros.append($s_proyecto_programa);
        Parametros.append('&s_unidad_responsable_id=');
        Parametros.append($s_unidad_responsable);
        Parametros.append('&s_partida_id=');
        Parametros.append($s_partida);
        Parametros.append('&psm_fte_financiamiento_id=');
        Parametros.append($psm_fte_financiamiento);
        Parametros.append('&psm_area_funcional_id=');
        Parametros.append($psm_area_funcional);
        Parametros.append('&psm_proyecto_programa_id=');
        Parametros.append($psm_proyecto_programa);
        Parametros.append('&psm_unidad_responsable_id=');
        Parametros.append($psm_unidad_responsable);
        Parametros.append('&psm_partida_id=');
        Parametros.append($psm_partida);
        Parametros.append('&Opcion=alta_plaza');

        try {
            if (!Existe_Clave($clave)) {
                $.ajax({
                    url: "Frm_Cat_Nom_Creacion_Plazas_Ctrl.aspx" + Parametros.toString(),
                    type: 'POST',
                    async: false,
                    cache: false,
                    dataType: 'text',
                    success: function(result) {
                        if (result != null) {
                            if (result == 'SI') {
                                $.messager.alert('Alta Plaza', 'Plaza creada con exito!!');
                            } else {
                                $.messager.alert('Alta Plaza', 'No operación no se completo');
                            }
                        }
                        else {
                            $.messager.alert('Alta Plaza', 'No operación no se completo!');
                        }
                    }
                });
            }
        } catch (e) {
            alert(e);
        }
    }

    function Eliminar_Plaza($unidad_responsable, $puesto_id, $clave) {
        var Parametros = new StringBuilder();
        Parametros.append('?unidad_responsable_id=');
        Parametros.append($unidad_responsable);
        Parametros.append('&puesto_id=');
        Parametros.append($puesto_id);
        Parametros.append('&clave=');
        Parametros.append($clave);
        Parametros.append('&Opcion=baja_plaza');

        try {
            $.ajax({
                url: "Frm_Cat_Nom_Creacion_Plazas_Ctrl.aspx" + Parametros.toString(),
                type: 'POST',
                async: false,
                cache: false,
                dataType: 'text',
                success: function(result) {
                    if (result != null) {
                        if (result == 'SI') {
                            $.messager.alert('Baja Plaza', 'Plaza eliminada con exito!!');
                        } else {
                            $.messager.alert('Baja Plaza', 'No operación no se completo');
                        }
                    }
                    else {
                        $.messager.alert('Alta Plaza', 'No operación no se completo!');
                    }
                }
            });
        } catch (e) {
            alert(e);
        }
    }

    function Existe_Clave(clave_generada) {
        var $unidad_responsable = $('#Cmb_Unidades_Reponsables').combobox('getValue');
        var Parametros = new StringBuilder();
        var $estatus = false;

        Parametros.append('?opcion=busqueda_plazas');
        Parametros.append('&tabla=PLAZAS');

        try {
            $.ajax({
                url: "Frm_Cat_Nom_Creacion_Plazas_Ctrl.aspx" + Parametros.toString(),
                type: 'POST',
                async: false,
                cache: false,
                dataType: 'json',
                success: function(DATOS) {
                    if (DATOS != null) {
                        $.each(DATOS.PLAZAS, function(Contador, Elemento) {
                            if (Elemento != undefined) {
                                if (Elemento.CLAVE == clave_generada) {
                                    $estatus = true;
                                    $.messager.alert('Información', 'La clave ya existe!');
                                }
                            }
                        });
                    }
                    else {
                        $.messager.alert('Existe Plaza', 'No se encontraron resultados para la busqueda realizada.');
                    }
                }
            });
        } catch (e) {
            alert(e);
        }
        return $estatus;
    }

    function reset_all() {
        try {
            $('#Cmb_Unidades_Reponsables').combobox('setValue', '');
            $('#Cmb_Puestos').val('');
            $('#Cmb_Tipo_Paza').val('');

            $('#Cmb_S_Fte_Finaciamiento').val('');
            $('#Cmb_S_Area_Funcional').val('');
            $('#Cmb_S_Programa').val('');
            $('#Cmb_S_Dependencia').val('');
            $('#Cmb_S_Partida').val('');

            $('#Span_SD').text('');
            $('#Span_S').text('');
            $('#Span_Disponible').text('');
            $('#Span_PSM_Diaria').text('');
            $('#Span_Disponible_PSM').text('');

            $('[id$=Txt_Fecha]').val('');
            $('[id$=Txt_Proyeccion]').val('0.00');
            $('[id$=Txt_Proyeccion_PSM]').val('0.00');

            $('#Cmb_PSM_Fte_Financiamiento').val('');
            $('#Cmb_PSM_Area_Funcional').val('');
            $('#Cmb_PSM_Programa').val('');
            $('#Cmb_PSM_Dependencia').val('');
            $('#Cmb_PSM_Partida').val('');
            
            Limpiar_Controles(); 
        } catch (e) { $.messager.alert('Información', 'Error al limpiar los datos cuando no aplica para previsión social múltiple.Error:[' + e + ']'); }
    }
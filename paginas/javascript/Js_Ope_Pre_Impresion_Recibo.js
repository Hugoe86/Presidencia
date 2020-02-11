var Referencia = "";
var No_Pago = "";
var Estatus = "";

///***********************************************************************************************************************
///NOMBRE DE LA FUNCIÓN : $(document).ready()
///DESCRIPCIÓN          : Funcion que indica cuando el documento ha sido cargado, para poder modificar los elemento DOM
///PARAMETROS           :   
///CREO                 : Leslie González Vázquez
///FECHA_CREO           : 28/Octubre/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN   :
///***********************************************************************************************************************
jQuery(document).ready(function() {
    Obtener_Parametros();
    Generar_Recibo_Oficial(Referencia);
    print();
    ventana=window.parent.self;
    ventana.opener=window.parent.self;
    ventana.close();
});

///***********************************************************************************************************************
///*********************************************** METODOS ***************************************************************
///***********************************************************************************************************************

var Cadena_HTML = "";
var Cadena_Encabezado = ""; // Aqui generaremos el diseño del encabezado
var Cadena_Nombre_Detalles = ""; // Aqui generaremos el diseño del detalle
var Cadena_Nombre_Calculo = ""; // Aqui generaremos el diseño del detalle
var Cadena_Signo = ""; // Aqui generaremos el diseño del detalle
var Cadena_Monto = ""; // Aqui generaremos el diseño del detalle
var Cadena_Pie_Pagina = ""; // Aqui generaremos el diseño del pie de pagina
var Cadena_Proteccion = ""; // Aqui generaremos el diseño de la proteccion
var Cadena_Titulo_Detalle ="";
var Cadena_Convenio="";
var Cadena_Cancelacion="";
var Cadena_Observacion="";
var Contribuyentes_HTML = '<div class="espaciador">&nbsp;</div><div class="Contribuyente_RFC">';
var Ubicacion_HTML = '<div class="Ubicacion_interior">';
var Exterior_HTML = '<div class="Exterior_Tasa">';
var cuota_HTML='<div class="Cuota_Efectos">';
var Valor_HTML = '<div class="Valor_CuotaBi">';
var Cadena_Clave_Ingreso = '<div class="Div_Clave_Ingreso">';

///***********************************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Generar_Recibo_Traslacion_Dominio
///DESCRIPCIÓN          : Funcion para generar el recibo de traslacion de dominio
///PARAMETROS           1 Referencia: con la que consultaremos los datos del pago para generar el recibo
///CREO                 : Leslie González Vázquez
///FECHA_CREO           : 28/Octubre/2011 
///MODIFICO             : 
///FECHA_MODIFICO       : 
///CAUSA_MODIFICACIÓN   : 
///***********************************************************************************************************************
function Generar_Recibo_Oficial(Referencia)
{
    //limpiamos el div por si contiene algo
    $('#Encabezado').empty();
    $('#Div_Importe').empty();
    $('#Div_Nombre_Detalles').empty();
    $('#Div_Signo').empty();
    $('#Div_Monto').empty();
    $('#Pie_Pagina').empty();
    $('#Proteccion').empty();
    $('#Div_Convenio').empty();
    $('#Div_Cancelacion').empty();
    $('#Div_Observaciones').empty();
    
    var Div_Encabezado = $('#Encabezado');
    var Div_Nombre_Detalles = $('#Div_Nombre_Detalles');
    var Div_Signo = $('#Div_Signo');
    var Div_Monto = $('#Div_Monto');
    var Div_Pie_Pagina = $('#Pie_Pagina');
    var Div_Proteccion = $('#Proteccion');
    var Div_Importe = $('#Div_Importe');
    var Div_Convenio = $('#Div_Convenio');
    var Div_Cancelacion = $('#Div_Cancelacion');
    var Div_Observaciones = $('#Div_Observaciones');
    
    //Hacemos la peticion al controlador
    $.ajax({
        url: "../Predial/Impresion_Recibos/Frm_Ope_Pre_Controlador_Impresion_Recibo.aspx?Referencia=" + Referencia + "&No_Pago=" + No_Pago + "&Estatus=" + Estatus,
        type: 'POST',
        contentType: 'JSON',
        async: false,
        cache: false,
        success: function (Datos) {
            if (Datos.TOTAL != 0) {
                $.each(Datos.Recibos, function (i, item) {
                    if (item.Tipo == "Encabezado") {
                        Generar_Encabezado("" + item.Nombre + "", "" + item.Descripcion + "");
                    }
                    if (item.Tipo == "Detalle") {
                        Generar_Detalles("" + item.Nombre + "", "" + item.Descripcion + "");
                    }
                    if (item.Tipo == "Detalle_Pago") {
                        Generar_Detalle_Pago("" + item.Nombre + "", "" + item.Descripcion + "");
                    }
                    if (item.Tipo == "Pie_Pagina") {
                        Generar_Pie_Pagina("" + item.Nombre + "", "" + item.Descripcion + "");
                    }
                    if (item.Tipo == "Convenio") {
                        Generar_Convenio("" + item.Nombre + "", "" + item.Descripcion + "");
                    }
                    if (item.Tipo == "Observaciones") {
                        Cadena_Observacion = '<label id="Lbl_Observacion">' + item.Nombre + '</label>';
                        Cadena_Observacion = '<label id="Lbl_Observacio_Descripcion">' + item.Descripcion + '</label>';
                    }
                    if (item.Tipo == "Proteccion") {
                        Cadena_Proteccion = '<label id="Lbl_Proteccion">' + item.Descripcion + '</label>';
                    }
                    if (item.Tipo == "Cancelacion") {
                        if (item.Nombre == "Cancela") {
                            Cadena_Cancelacion += '<label id="Lbl_Cancelacion">' + item.Descripcion + '</label><br />';
                        } else {
                            Cadena_Cancelacion += '<label id="Lbl_Motivo_Cancelacion">' + item.Descripcion + '</label>';
                        }
                    }
                });
            }
            else {
                Cadena_HTML = "Error al generar el recibo";
            }
        }
    });
    Cadena_Titulo_Detalle +='<label id="Lbl_Importes">IMPORTE</label>';
    $(Cadena_Encabezado).appendTo(Div_Encabezado);
    $(Cadena_Titulo_Detalle).appendTo(Div_Importe);
    $(Cadena_Nombre_Detalles).appendTo(Div_Nombre_Detalles);
    $(Cadena_Signo).appendTo(Div_Signo);
    $(Cadena_Monto).appendTo(Div_Monto);
    $(Cadena_Pie_Pagina).appendTo(Div_Pie_Pagina);
    $(Cadena_Convenio).appendTo(Div_Convenio);
    $(Cadena_Observacion).appendTo(Div_Observaciones);
    $(Cadena_Proteccion).appendTo(Div_Proteccion);
    if(Cadena_Cancelacion != ""){
        $('#Div_Importe').empty();
        $(Cadena_Cancelacion).appendTo(Div_Cancelacion);
    }
}

///***********************************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Generar_Encabezado
///DESCRIPCIÓN          : Funcion para generar el encabezado del recibo
///PARAMETROS           1 Nombre: Nombre del campo del recibo
///                     2 Descripcion: descripcion del campo del recibo
///CREO                 : Leslie González Vázquez
///FECHA_CREO           : 31/Octubre/2011 
///MODIFICO             : 
///FECHA_MODIFICO       : 
///CAUSA_MODIFICACIÓN   : 
///***********************************************************************************************************************
function Generar_Encabezado(Nombre, Descripcion)
{
    if(Nombre == "FECHA:")
    {
        Cadena_Encabezado += '<div class="Fecha">';
        Cadena_Encabezado += '<label id="Lbl_Fecha">' + Descripcion + '</label>';
        Cadena_Encabezado += '</div>';
    }
     if(Nombre == "TITULO:")
    {
        Cadena_Encabezado += '<div class="Titulo">';
        Cadena_Encabezado += '<label id="Lbl_Titulo">' + Descripcion + '</label>';
        Cadena_Encabezado += '</div>';
    }
     if(Nombre == "FOLIO:")
    {
        Cadena_Encabezado += '<div class="espaciador">&nbsp;</div><div class="Folio">';
         Cadena_Encabezado += '<label id="Lbl_Folio">' + Nombre + '</label>';
        Cadena_Encabezado += '<label id="Lbl_Folio_Nombre">' + Descripcion + '</label>';
        Cadena_Encabezado += '</div>';
    }
    if(Nombre == "CUENTA:")
    {
        Cadena_Encabezado += '<div class="espaciador">&nbsp;</div><div class="Cuenta">';
        Cadena_Encabezado += '<label id="Lbl_Cuenta">' + Nombre + '</label>';
        Cadena_Encabezado += '<label id="Lbl_Cuenta_Nombre">' + Descripcion + '</label>';
        Cadena_Encabezado += '</div>';
    }
    if(Nombre == "COLONIA:")
    {
        Cadena_Encabezado += '<div class="Colonia">';
        Cadena_Encabezado += '<label id="Lbl_Colonia">' + Nombre + '</label>';
        Cadena_Encabezado += '<label id="Lbl_Colonia_Nombre">' + Descripcion + '</label>';
        Cadena_Encabezado += '</div>';
    }
    
    if(Nombre == "UBICACION:" || Nombre == "INTERIOR:")
    {
         Cadena_Encabezado += Ubicacion_HTML ;
        if(Nombre == "UBICACION:")
        {
            Ubicacion_HTML ="";
            Cadena_Encabezado += '<div id="Div_Ubicacion">';
            Cadena_Encabezado += '<label id="Lbl_Ubicacion">' + Nombre + '</label>';
            Cadena_Encabezado += '<label id="Lbl_Ubicacion_Nombre">'+ Descripcion  + '</label>';
            Cadena_Encabezado += '</div>';
        }else
        {
            Ubicacion_HTML ="</div>";
            Cadena_Encabezado += '<div id="Div_Interior">';
            Cadena_Encabezado += '<label id="Lbl_Interior">' + Nombre +'</label>';
            Cadena_Encabezado += '<label id="Lbl_Interior_Nombre">' + Descripcion + '</label>';
            Cadena_Encabezado += '</div>';
        }
        Cadena_Encabezado += Ubicacion_HTML;
    }
    if(Nombre == "EXTERIOR:" || Nombre == "TASA:")
    {
         Cadena_Encabezado += Exterior_HTML ;
        if(Nombre == "EXTERIOR:")
        {
            Exterior_HTML ="";
            Cadena_Encabezado += '<div id="Div_Exterior">';
            Cadena_Encabezado += '<label id="Lbl_Exterior">' + Nombre + '</label>';
            Cadena_Encabezado += '<label id="Lbl_Exterior_Nombre">'+ Descripcion  + '</label>';
            Cadena_Encabezado += '</div>';
        }else
        {
            Exterior_HTML ="</div>";
            Cadena_Encabezado += '<div id="Div_Tasa">';
            Cadena_Encabezado += '<label id="Lbl_Tasa">' + Nombre +'</label>';
            Cadena_Encabezado += '<label id="Lbl_Tasa_Nombre">' + Descripcion + '</label>';
            Cadena_Encabezado += '</div>';
        }
        Cadena_Encabezado += Exterior_HTML;
    }
    if(Nombre == "VALOR FISCAL:" || Nombre == "CUOTA BIMESTRAL:")
    {
         Cadena_Encabezado += Valor_HTML ;
        if(Nombre == "VALOR FISCAL:")
        {
            Valor_HTML ="";
            Cadena_Encabezado += '<div id="Div_Valor_Fiscal">';
            Cadena_Encabezado += '<label id="Lbl_Valor_Fiscal">' + Nombre + '</label>';
            Cadena_Encabezado += '<label id="Lbl_Valor_Fiscal_Nombre">'+ Descripcion  + '</label>';
            Cadena_Encabezado += '</div>';
        }else
        {
            Valor_HTML ="</div>";
            Cadena_Encabezado += '<div id="Div_cuota_bimestral">';
            Cadena_Encabezado += '<label id="Lbl_cuota bimestral">' + Nombre +'</label>';
            Cadena_Encabezado += '<label id="Lbl_cuota bimestral_Nombre">' + Descripcion + '</label>';
            Cadena_Encabezado += '</div>';
        }
        Cadena_Encabezado += Valor_HTML;
    }
    if(Nombre == "CUOTA ANUAL:" || Nombre == "EFECTOS:")
    {
         Cadena_Encabezado += cuota_HTML ;
        if(Nombre == "CUOTA ANUAL:")
        {
            cuota_HTML ="";
            Cadena_Encabezado += '<div id="Div_cuota_Anual">';
            Cadena_Encabezado += '<label id="Lbl_Cuota_Anual">' + Nombre + '</label>';
            Cadena_Encabezado += '<label id="Lbl_Cuota_anual_Nombre">'+ Descripcion  + '</label>';
            Cadena_Encabezado += '</div>';
        }else
        {
            cuota_HTML ="</div>";
            Cadena_Encabezado += '<div id="Div_Efectos">';
            Cadena_Encabezado += '<label id="Lbl_Efectos">' + Nombre +'</label>';
            Cadena_Encabezado += '<label id="Lbl_Efectos_Nombre">' + Descripcion + '</label>';
            Cadena_Encabezado += '</div>';
        }
        Cadena_Encabezado += cuota_HTML;
    }
    if(Nombre == "CLAVE MOVIMIENTO:")
    {
        Cadena_Encabezado += '<div id="Clave_Movimiento">';
        Cadena_Encabezado += '<label id="Lbl_Clave_Movimiento">' + Nombre + '</label>';
        Cadena_Encabezado += '<label id="Lbl_Clave_Movimiento_Nombre">'+ Descripcion  + '</label>';
        Cadena_Encabezado += '</div>';
    }
    
    if(Nombre == "CONTRIBUYENTE:" || Nombre == "R.F.C.:")
    {
         Cadena_Encabezado += Contribuyentes_HTML ;
        if(Nombre == "CONTRIBUYENTE:")
        {
            Contribuyentes_HTML ="";
            Cadena_Encabezado += '<div id="Contribuyente">';
            Cadena_Encabezado += '<label id="Lbl_Contribuyente">' + Nombre + '</label>';
            Cadena_Encabezado += '<label id="Lbl_Contribuyente_Nombre">'+ Descripcion  + '</label>';
            Cadena_Encabezado += '</div>';
        }else
        {
            Contribuyentes_HTML ="</div>";
            Cadena_Encabezado += '<div id="RFC">';
            Cadena_Encabezado += '<label id="Lbl_RFC">' + Nombre +'</label>';
            Cadena_Encabezado += '<label id="Lbl_RFC_Nombre">' + Descripcion + '</label>';
            Cadena_Encabezado += '</div>';
        }
        Cadena_Encabezado += Contribuyentes_HTML;
    }
    if(Nombre == "DOMICILIO:")
    {
        Cadena_Encabezado += '<div id="Domicilio">';
        Cadena_Encabezado += '<label id="Lbl_Domicilio">' + Nombre + '</label>';
        Cadena_Encabezado += '<label id="Lbl_Domicilio_Nombre">'+ Descripcion  + '</label>';
        Cadena_Encabezado += '</div>';
    }
    if(Nombre == "CIUDAD:")
    {
        Cadena_Encabezado += '<div id="Ciudad">';
        Cadena_Encabezado += '<label id="Lbl_Ciudad">' + Nombre + '</label>';
        Cadena_Encabezado += '<label id="Lbl_Ciudad_Nombre">'+ Descripcion  + '</label>';
        Cadena_Encabezado += '</div>';
    }
    if(Nombre == "NOMBRE:")
    {
        Cadena_Encabezado += '<div id="Div_Nombre">';
        Cadena_Encabezado += '<label id="Lbl_Nombre">' + Nombre + '</label>';
        Cadena_Encabezado += '<label id="Lbl_Nombre_Descripcion">'+ Descripcion  + '</label>';
        Cadena_Encabezado += '</div>';
    }
}

///***********************************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Generar_Detalles
///DESCRIPCIÓN          : Funcion para generar el detalle del recibo
///PARAMETROS           1 Nombre: Nombre del campo del recibo
///                     2 Descripcion: descripcion del campo del recibo
///CREO                 : Leslie González Vázquez
///FECHA_CREO           : 31/Octubre/2011 
///MODIFICO             : 
///FECHA_MODIFICO       : 
///CAUSA_MODIFICACIÓN   : 
///***********************************************************************************************************************
function Generar_Detalles(Nombre, Descripcion)
{
    if(Nombre == "PERIODO"){
        if(Cadena_Titulo_Detalle == ""){
            Cadena_Titulo_Detalle += '<label class="Lbl_Titulo_Periodo">PERIODO</label>';
        }
        Cadena_Nombre_Detalles += '<label class="Lbl_Periodo">' + Descripcion + '</label>';
    }
    else if(Nombre == "CANTIDAD")
    {
        Cadena_Nombre_Detalles += Cadena_Clave_Ingreso;
        Cadena_Nombre_Detalles += '<label class="Lbl_Cantidad">' + Descripcion + '</label>&nbsp;&nbsp;&nbsp;';
        Cadena_Clave_Ingreso = "";
    }
    else{
        Cadena_Nombre_Detalles += Cadena_Clave_Ingreso;
        Cadena_Nombre_Detalles += '<label class="Lbl_Clave_Ingreso">' + Nombre + '</label></div>';
        if (Descripcion.toString().search("<BR/>") > -1){
            Cadena_Signo += Descripcion + '<br />';
        }
        else{
            Cadena_Signo += '<label class="Signo">$</label><br />';
        }
        Cadena_Monto += '<label class="Monto">' + Descripcion + '</label><br />';
        if(Cadena_Clave_Ingreso == ''){
            Cadena_Clave_Ingreso += '<div class="Div_Clave_Ingreso">';
        }
    }
}

///***********************************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Generar_Detalle_Pago
///DESCRIPCIÓN          : Funcion para generar el detalle del pago del recibo
///PARAMETROS           1 Nombre: Nombre del campo del recibo
///                     2 Descripcion: descripcion del campo del recibo
///CREO                 : Leslie González Vázquez
///FECHA_CREO           : 31/Octubre/2011 
///MODIFICO             : 
///FECHA_MODIFICO       : 
///CAUSA_MODIFICACIÓN   : 
///***********************************************************************************************************************
function Generar_Detalle_Pago(Nombre, Descripcion)
{
    Cadena_Nombre_Detalles += '<div class="Div_Nom_Calculo"><label class="Nom_Calculo">' + Nombre + '</label></div>';
    if (Descripcion.toString().search("<BR/>") > -1) {
        Cadena_Signo += '<label class="Signo">$</label><BR/><BR/>';
    }
    else{
        Cadena_Signo += '<label class="Signo">$</label><BR/>';
    }
    Cadena_Monto += '<label class="Monto">' + Descripcion + '</label><br />';
}

///***********************************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Generar_Pie_Pagina
///DESCRIPCIÓN          : Funcion para generar el detalle del recibo
///PARAMETROS           1 Nombre: Nombre del campo del recibo
///                     2 Descripcion: descripcion del campo del recibo
///CREO                 : Leslie González Vázquez
///FECHA_CREO           : 31/Octubre/2011 
///MODIFICO             : 
///FECHA_MODIFICO       : 
///CAUSA_MODIFICACIÓN   : 
///***********************************************************************************************************************
function Generar_Pie_Pagina(Nombre, Descripcion)
{
    if(Nombre =="BASE IMPUESTO:" || Nombre == "BASE GRAVABLE:"){
        Cadena_Pie_Pagina +='<div id="Div_Base_Impuesto">';
        Cadena_Pie_Pagina += '<label id="Lbl_Base_Imp_Nombre" Width="80px"  style="text-align:right">' + Nombre + '</label>';
        Cadena_Pie_Pagina += '<label id="Lbl_Base_Imp_Descricion" Width="200px" style="text-align:left">' + Descripcion + '</label> <\T>';
        Cadena_Pie_Pagina +='</div>';
    } else {
    if (Nombre == "CUOTA MINIMA:") {
        Cadena_Pie_Pagina += '<div id="Div_Cuota_Minima">';
        Cadena_Pie_Pagina += '<label id="Lbl_Cuota_Nombre" Width="80px"">' + Nombre + '</label>';
        Cadena_Pie_Pagina += '<label id="Lbl_Cuota_Minima" Width="700px">' + Descripcion + '</label>';
        Cadena_Pie_Pagina += '</div>';
    }
    else {
        Cadena_Pie_Pagina += '<div id="Div_Base_Gravable">';
        Cadena_Pie_Pagina += '<label id="Lbl_Base_Gra_Nombre" Width="80px"  style="text-align:right">' + Nombre + '</label>&nbsp;&nbsp;';
        Cadena_Pie_Pagina += '<label id="Lbl_Base_Gra_Descricion" Width="200px" style="text-align:left">' + Descripcion + '</label> <\T>';
        Cadena_Pie_Pagina += '</div>';
    }
    }
}

///*******************************************************************************
///NOMBRE DE LA FUNCIÓN : Obtener_Parametros
///DESCRIPCIÓN          : obtiene los parametros de la url
///PARAMETROS           : 
///CREO                 : Yañez Rodriguez Diego
///FECHA_CREO           : 19/Mayo/2011 
///MODIFICO             :
///FECHA_MODIFICO       :
///CAUSA_MODIFICACIÓN   :
///*******************************************************************************
function Obtener_Parametros() { 
    var No_Parametros = 0;
    var resultado = {};
    var url = window.location.href; //OBTENEMOS LAS URL DE LA PAGINA
    var parametros = url.slice(url.indexOf('?') + 1).split('&');
    for (var i = 0; i < parametros.length; i++) {
        var parametro = parametros[i].split('=');
        resultado[parametro[0]] = parametro[1];
        No_Parametros = No_Parametros + 1;
    }
    
    Referencia = resultado.Referencia;
    No_Pago = resultado.No_Pago;
    if (No_Parametros > 2){
         Estatus = resultado.Estatus;
    }
}

///***********************************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Generar_Convenio
///DESCRIPCIÓN          : Funcion para generar el detalle del convenio
///PARAMETROS           1 Nombre: Nombre del campo del recibo
///                     2 Descripcion: descripcion del campo del recibo
///CREO                 : Leslie González Vázquez
///FECHA_CREO           : 11/Noviembre/2011 
///MODIFICO             : 
///FECHA_MODIFICO       : 
///CAUSA_MODIFICACIÓN   : 
///***********************************************************************************************************************
function Generar_Convenio(Nombre, Descripcion)
{
    if (Nombre == "DATOS DEL CONVENIO:") {
        Cadena_Convenio += '<div id="Div_Datos_Convenio">';
        Cadena_Convenio += '<label id="Lbl_Datos_Nombre">' + Nombre + '</label>&nbsp;&nbsp;';
        Cadena_Convenio += '<label id="Lbl_Datos_Descricion">' + Descripcion + '</label>';
        Cadena_Convenio += '</div>';
    }
    else if (Nombre == " ") {
        Cadena_Convenio += '<div id="Div_Datos_Convenio" class="Div_Observaciones_Pago">';
        Cadena_Convenio += '<label id="Lbl_Datos_Nombre">' + Nombre + '</label>&nbsp;&nbsp;';
        Cadena_Convenio += '<label id="Lbl_Datos_Descricion" style="font-size:larger">' + Descripcion + '</label>';
        Cadena_Convenio += '</div>';
    }
    else {
        Cadena_Convenio += '<div id="Div_Pagos_Convenio">';
        Cadena_Convenio += '<label id="Lbl_Pago_Nombre">' + Nombre + '</label>&nbsp;&nbsp;';
        Cadena_Convenio += '<label id="Lbl_Pago_Descricion">' + Descripcion + '</label>';
        Cadena_Convenio += '</div>';
    }
}
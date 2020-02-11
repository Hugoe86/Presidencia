function StringBuilder(value)
{
    this.strings = new Array("");
    this.append(value);
}

StringBuilder.prototype.append = function (value)
{
    if (value)
    {
        this.strings.push(value);
    }
}

StringBuilder.prototype.clear = function ()
{
    this.strings.length = 1;
}

StringBuilder.prototype.toString = function ()
{
    return this.strings.join("");
}


function Btn_Cambio_Contrasena (){
    $.modaldialog.prompt(Crear_Tabla_Cambio_Password(), { title: '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Cambio de Password' });
    Estado_Inicial();
    
    $("#Btn_Actualizar_Password").bind("click", function(e){
        if(Validar_Datos()){
            Mostrar_Errores(true);
            Actualizar_Password($("#Txt_Repetir_Contrasena").val(), $("#Txt_No_Empleado").val());
        }else{
            Mostrar_Errores(false);
        }
    });
    return false;
}

function Crear_Tabla_Cambio_Password(){
  var HTML  =new StringBuilder();
    
    HTML.append("<table style='width:100%;color:Black;font-size:10px;'>");
    HTML.append("<tr>");
    HTML.append("<td style='width:100%;' align='center'>");
    
        HTML.append("<br /><br />");        
        HTML.append("<table style='width:60%;border:outset 1px Silver;' border='0px'>");
        HTML.append("<tr>");
        HTML.append("<td style='width:100%;' colspan='2'>");
        HTML.append("<label id='Lbl_Error' style='display:none;color:#FF3300;font-family: Verdana, Geneva, MS Sans Serif;font-size: 9px; '></label>");
        HTML.append("<hr />");
        HTML.append("</td>");
        HTML.append("</tr>");
        HTML.append("<tr>");
        HTML.append("<td style='width:25%;text-align:left;font-size:12px;font-weight:normal;font-family:Comic Sans MS;font-size:12px;color:Black;'>");
        HTML.append("&nbsp;&nbsp;N&uacute;mero Empleado: ");
        HTML.append("</td>");
        HTML.append("<td style='width:35%;' align='left'>");
        HTML.append("<input type='text' id='Txt_No_Empleado' style='width:50%; border-style:ouset; border-width:1px; border-color:Silver; font-family:Calibri;font-size:13px;background-color:Transparent; text-align:left; font-weight:bold;'/>");
        HTML.append("</td>");
        HTML.append("</tr>");
        HTML.append("<tr>");
        HTML.append("<td style='width:25%;text-align:left;font-size:12px;font-weight:normal;font-family:Comic Sans MS;font-size:12px;color:Black;'>");
        HTML.append("&nbsp;&nbsp;Contraseña Actual: ");
        HTML.append("</td>");
        HTML.append("<td style='width:35%;' align='left'>");
        HTML.append("<input type='password' id='Txt_Contrasena_Actual' style='width:80%; border-style:ouset; border-width:1px; border-color:Silver; font-family:Calibri;font-size:13px;background-color:Transparent; text-align:left; font-weight:bold;'/>");
        HTML.append("<span id='Span_Img1'></span>");
        HTML.append("</td>");
        HTML.append("</tr>");
        HTML.append("<tr>");
        HTML.append("<td style='width:25%;text-align:left;font-size:12px;font-family:Comic Sans MS;font-size:12px;color:Black;font-weight:normal;'>");
        HTML.append("&nbsp;&nbsp;Nueva Contraseña: ");
        HTML.append("</td>");
        HTML.append("<td style='width:35%;' align='left'>");
        HTML.append("<input type='password' id='Txt_Nueva_Contrasena' style='width:80%; border-style:ouset; border-width:1px; border-color:Silver; font-family:Calibri;font-size:13px;background-color:Transparent; text-align:left; font-weight:bold;'/>");
        HTML.append("<span id='Span_Img2'></span>");
        HTML.append("</td>");
        HTML.append("</tr>");
        HTML.append("<tr>");
        HTML.append("<td style='width:25%;text-align:left;font-size:12px;font-family:Comic Sans MS;font-size:12px;color:Black;font-weight:normal;'>");
        HTML.append("&nbsp;&nbsp;Confirmar Contraseña: ");
        HTML.append("</td>");
        HTML.append("<td style='width:35%;' align='left'>");
        HTML.append("<input type='password' id='Txt_Repetir_Contrasena' style='width:80%; border-style:ouset; border-width:1px; border-color:Silver; font-family:Calibri;font-size:13px;background-color:Transparent; text-align:left; font-weight:bold;'/>");
        HTML.append("<span id='Span_Img3'></span>");
        HTML.append("</td>");
        HTML.append("</tr>");
        HTML.append("<tr>");
        HTML.append("<td style='width:25%;text-align:left;font-size:12px;'>");
        HTML.append("</td>");
        HTML.append("<td style='width:35%;' align='right'>");
        HTML.append("<input type='image' id='Btn_Actualizar_Password' src='../imagenes/paginas/Sias_Actualizar.png' />");
        HTML.append("&nbsp;&nbsp;");
        HTML.append("</td>");
        HTML.append("</tr>");
        HTML.append("<tr>");
        HTML.append("<td style='width:100%;' colspan='2'>");
        HTML.append("<hr />");
        HTML.append("</td>");
        HTML.append("</tr>");
        HTML.append("</table>");
    HTML.append("</td>");
    HTML.append("</tr>");
    HTML.append("</table>");

    $("#Txt_No_Empleado").live("keyup", function(e){
        Limpiar_Controles_No_Empleado();
    });
    
    $("#Txt_No_Empleado").live("blur", function(e){
        Es_Empleado($("#Txt_No_Empleado").val());
    });
    
    $("#Txt_Contrasena_Actual").live("keyup", function(e){
        Limpiar_Controles_Contrasena_Actual();
    });
    
    $("#Txt_Contrasena_Actual").live("blur", function(e){
        Validar_Contrasena_Actual($(this).val(), $("#Txt_No_Empleado").val());
    });
    
    $("#Txt_Nueva_Contrasena").live("keyup", function(e){
        Limpiar_Controles_Nueva_Contraseña();
    });
    
    $("#Txt_Repetir_Contrasena").live("keyup", function(e){
        Limpiar_Controles_Repetir_Contraseña();
    });
    
    $("#Txt_Repetir_Contrasena").live("blur", function(e){
        Validar_Nuevo_Password();
    });
    


    return HTML.toString();
}

function Validar_Contrasena_Actual(Password, No_Empleado){
    if((No_Empleado != "") && (Password != "")){
        $.ajax({
            url: "Controlador_Password.aspx?Opcion=Cambio_Password&No_Empleado=" + No_Empleado + "&Password_Actual="+ Password,
            type: 'POST',
            async: false,
            cache: false,
            success: function(RESPUESTA) {
                if(RESPUESTA == "NO"){
                    $("#Span_Img1").html("<img id='Img_Estatus' src='../imagenes/paginas/Eliminar_Incidencia.png' alt=''/>");
                    $("#Txt_Contrasena_Actual").val("");
                    
                    $("#Txt_Nueva_Contrasena").attr("disabled","false");
                    $("#Txt_Repetir_Contrasena").attr("disabled","false");
                }else{
                    $("#Span_Img1").html("<img id='Img_Estatus' src='../imagenes/gridview/blue_button.png' alt=''/>");
                    
                    $("#Txt_Nueva_Contrasena").removeAttr("disabled");
                    $("#Txt_Repetir_Contrasena").removeAttr("disabled");
                }
            }
        });
    }
}

function Limpiar_Controles_No_Empleado(){
    $("#Txt_Contrasena_Actual").val("");
    $("#Txt_Nueva_Contrasena").val("");
    $("#Txt_Repetir_Contrasena").val("");
    
    $("#Txt_Nueva_Contrasena").attr("disabled","false");
    $("#Txt_Repetir_Contrasena").attr("disabled","false");
    
    $("#Span_Img1").empty();
    $("#Span_Img2").empty();
    $("#Span_Img3").empty();
    Mostrar_Errores(true);
}

function Limpiar_Controles_Contrasena_Actual(){
    $("#Txt_Nueva_Contrasena").val("");
    $("#Txt_Repetir_Contrasena").val("");
    
    $("#Txt_Nueva_Contrasena").attr("disabled","false");
    $("#Txt_Repetir_Contrasena").attr("disabled","false");
    
    $("#Span_Img1").empty();
    $("#Span_Img2").empty();
    $("#Span_Img3").empty();
    Mostrar_Errores(true);
}

function Limpiar_Controles_Nueva_Contraseña(){
    if($("#Txt_Repetir_Contrasena").val() != ""){
        $("#Txt_Repetir_Contrasena").val("");
        $("#Span_Img2").empty();
        $("#Span_Img3").empty();
    }else{
        $("#Span_Img2").empty();
        $("#Span_Img3").empty();
        Mostrar_Errores(true);
    }
}

function Validar_Nuevo_Password(){
    var Nuevo_Password = $("#Txt_Nueva_Contrasena").val();
    var Confirmar_Password = $("#Txt_Repetir_Contrasena").val();
    
    if((Nuevo_Password != "") && (Confirmar_Password != "")){
        if(Nuevo_Password != Confirmar_Password){
            $("#Txt_Nueva_Contrasena").val("");
            $("#Txt_Repetir_Contrasena").val("");
            
            $("#Span_Img2").html("<img id='Img_Estatus' src='../imagenes/paginas/Eliminar_Incidencia.png' alt=''/>");
            $("#Span_Img3").html("<img id='Img_Estatus' src='../imagenes/paginas/Eliminar_Incidencia.png' alt=''/>");
        }else{
            $("#Span_Img2").html("<img id='Img_Estatus' src='../imagenes/gridview/blue_button.png' alt=''/>");
            $("#Span_Img3").html("<img id='Img_Estatus' src='../imagenes/gridview/blue_button.png' alt=''/>");
        }
    }
}

function Limpiar_Controles_Repetir_Contraseña(){
    $("#Span_Img2").empty();
    $("#Span_Img3").empty();
    Mostrar_Errores(true);
}

function Actualizar_Password(Password, No_Empleado){
    if((No_Empleado != "") && (Password != "")){
        $.ajax({
            url: "Controlador_Password.aspx?Opcion=Actualizar_Password&No_Empleado=" + No_Empleado + "&Password_Actual="+ Password,
            type: 'POST',
            async: false,
            cache: false,
            success: function(RESPUESTA) {
                if(RESPUESTA == "NO"){
                    alert("El password no se ha actualizado.");
                }else{
                    $.modaldialog.hide();
                    Limpiar_Controles_No_Empleado();
                }
            }
        });
    }
}

function Es_Empleado(No_Empleado){
    if((No_Empleado != "")){
        $.ajax({
            url: "Controlador_Password.aspx?Opcion=Es_Empleado&No_Empleado=" + No_Empleado,
            type: 'POST',
            async: false,
            cache: false,
            success: function(RESPUESTA) {
                if(RESPUESTA == "NO"){
                    Estado_Inicial();
                    $("#Lbl_Error").html('No existe ning&uacute;n empleado con el n&uacute;mero de empleado ingresado.');
                    Mostrar_Errores(false);
                }else{
                    $("#Txt_Contrasena_Actual").removeAttr("disabled");
                }
            }
        });
    }
}

function Validar_Datos(){
    var Estatus = true;
    var HTML  =new StringBuilder();
    
    HTML.append("<div align='left'>");
    
    if($("#Txt_No_Empleado").val() == ""){
        HTML.append("<font style='font-size:10px;'>El n&uacute;mero de empleado es obligatorio. </font><br/>");
        Estatus = false;
    }
    
    if($("#Txt_Repetir_Contrasena").val() == ""){
        HTML.append("<font style='font-size:10px;'>El password es obligatorio. </font>");
        Estatus = false;
    }
    
    HTML.append("</div>");
    
    $("#Lbl_Error").html(HTML.toString());
    return Estatus;
}

function Mostrar_Errores(Estatus){
    if(Estatus){
        $("#Lbl_Error").empty('');
        $("#Lbl_Error").css("display", "none");
    }else{
        $("#Lbl_Error").css("display", "block");
    }
}

function Estado_Inicial(){
    $("#Txt_Contrasena_Actual").val("");
    $("#Txt_Nueva_Contrasena").val("");
    $("#Txt_Repetir_Contrasena").val("");
    
    $("#Txt_Contrasena_Actual").attr("disabled","false");
    $("#Txt_Nueva_Contrasena").attr("disabled","false");
    $("#Txt_Repetir_Contrasena").attr("disabled","false");
    
    $("#Span_Img1").empty();
    $("#Span_Img2").empty();
    $("#Span_Img3").empty();
    Mostrar_Errores(true);
}


///***********************************************************************************************************************
///NOMBRE DE LA FUNCIÓN :$(document).ready(function
///DESCRIPCIÓN          : Funcion para iniciar con los datos del formulario
///PARAMETROS           : 
///CREO                 : Leslie González Vázquez
///FECHA_CREO           : 30/Noviembre/2011 
///MODIFICO             : 
///FECHA_MODIFICO       : 
///CAUSA_MODIFICACIÓN   : 
///***********************************************************************************************************************
 $(document).ready(function() {
     try
     {
        Mensaje_Error('Ocultar');
        //Evento del boton generar clasificacion
        $("input[id$=Btn_Registrarse]").click(function (e) {
            e.preventDefault();
            if(Validar_Datos()){
                Registrar_Usuario();
            }else{
                 Mensaje_Error('Mostrar');
            }
        });
        
        $("input[id$=Btn_Consultar_Pass]").click(function (e) {
            e.preventDefault();
            if(Validar_Datos_Enviar_Password()){
                Consultar_Password_Usuario()
            }
            else{
                 Mensaje_Error_Password('Mostrar')
            }
           
//             if($("input[id$=Txt_Email_Pass]").val() == "" && $("input[id$=Txt_Email_Pass]").val().length <= 0){
//                $.messager.alert('Mensaje','Favor de instroducir un Email');
//            }else{
//                ;
//            }
        });
    }catch(Ex){
        $.messager.alert('Mensaje','Error al ejecutar los eventos de la página. Error: [' + Ex + ']');
    }
 });


///***********************************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Registro
///DESCRIPCIÓN          : Funcion para iniciar con los datos del formulario
///PARAMETROS           : 
///CREO                 : Leslie González Vázquez
///FECHA_CREO           : 01/Mayo/2012
///MODIFICO             : 
///FECHA_MODIFICO       : 
///CAUSA_MODIFICACIÓN   : 
///***********************************************************************************************************************
function Registro()
{
    Mensaje_Error('Ocultar');
    try
    {
        Limpiar_Controles();
        $('#Div_Registro').window('open');
    }catch(Ex){
        $.messager.alert('Mensaje','Error al ejecutar los eventos de la página. Error: [' + Ex + ']');
    }
}

///***********************************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Consultar_Password
///DESCRIPCIÓN          : Funcion para abrir la ventana de olvido de password
///PARAMETROS           : 
///CREO                 : Leslie González Vázquez
///FECHA_CREO           : 03/Mayo/2012
///MODIFICO             : 
///FECHA_MODIFICO       : 
///CAUSA_MODIFICACIÓN   : 
///***********************************************************************************************************************
function Consultar_Password()
{
    try
    {
        $("input[id$=Txt_Email_Pass]").val("");
        $('#Div_Pass').window('open');
    }catch(Ex){
        $.messager.alert('Mensaje','Error al ejecutar los eventos de la página. Error: [' + Ex + ']');
    }
}

///***********************************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Validar_Datos
///DESCRIPCIÓN          : Funcion para validar los datos del formulario
///PARAMETROS           : 
///CREO                 : Leslie González Vázquez
///FECHA_CREO           : 02/Mayo/2012
///MODIFICO             : 
///FECHA_MODIFICO       : 
///CAUSA_MODIFICACIÓN   : 
///***********************************************************************************************************************
function Validar_Datos()
{
    Mensaje_Error('Ocultar');
    var Datos_Validos = true;
    var Encabezado_Error = 'Es necesario Introducir: ';
    var Error = "";
    var Pregunta_Secreta = "";
    
    try
    {
        if($("input[id$=Txt_Nombre]").val() == "" && $("input[id$=Txt_Nombre]").val().length <= 0 )
        {
            Error += "Nombre, ";
            Datos_Validos = false;
        }
        if($("input[id$=Txt_Apellido_Paterno]").val() == "" && $("input[id$=Txt_Apellido_Paterno]").val().length <= 0 )
        {
            Error += "Apellido paterno, ";
            Datos_Validos = false;
        }
        if($("input[id$=Txt_Edad]").val() == "" && $("input[id$=Txt_Edad]").val().length <= 0 )
        {
            Error += "Edad, ";
            Datos_Validos = false;
        }
        if($("input[id$=Txt_Email]").val() == "" && $("input[id$=Txt_Email]").val().length <= 0 )
        {
            Error += "E-mail, ";
            Datos_Validos = false;
        }
        
        if($("input[id$=Txt_Confirmar_Email]").val() != $("input[id$=Txt_Email]").val())
        {
            Error += "Confirmar E-mail, ";
            Datos_Validos = false;
        }
        if($("input[id$=Txt_Calle]").val() == "" && $("input[id$=Txt_Calle]").val().length <= 0 )
        {
            Error += "Calle, ";
            Datos_Validos = false;
        }
        if($("input[id$=Txt_Colonia]").val() == "" && $("input[id$=Txt_Colonia]").val().length <= 0 )
        {
            Error += "Colonia, ";
            Datos_Validos = false;
        }
        if($("input[id$=Txt_Ciudad]").val() == "" && $("input[id$=Txt_Ciudad]").val().length <= 0 )
        {
            Error += "Ciudad, ";
            Datos_Validos = false;
        }
        if($("input[id$=Txt_Estado]").val() == "" && $("input[id$=Txt_Estado]").val().length <= 0 )
        {
            Error += "Estado, ";
            Datos_Validos = false;
        }
        if($("input[id$=Txt_Edad]").val() == "" && $("input[id$=Txt_Edad]").val().length <= 0 )
        {
            Error += "Edad, ";
            Datos_Validos = false;
        }
        if($("input[id$=Txt_Fecha_Nacimiento]").val() == "" && $("input[id$=Txt_Fecha_Nacimiento]").val().length <= 0 )
        {
            Error += "Fecha de Nacimiento, ";
            Datos_Validos = false;
        }
        
         Pregunta_Secreta = $("select[id$=Cmb_Pregunta_Secreta] option:selected").text();
        if(Pregunta_Secreta=="- Selecciona -")
        {
            Error += "Pregunta secreta, ";
            Datos_Validos = false;
        }
         if($("input[id$=Txt_Respuesta_Secreta]").val() == "" && $("input[id$=Txt_Respuesta_Secreta]").val().length <= 0 )
        {
            Error += "Respuesta secreta, ";
            Datos_Validos = false;
        }
        
        $("[id$=Lbl_Ecabezado_Mensaje]").text(Encabezado_Error);
        $("[id$=Lbl_Mensaje_Error]").text(Error);
        return Datos_Validos;
    }catch(Ex){
        $.messager.alert('Mensaje','Error al validar los datos de la pagina. Error: [' + Ex + ']');
    }
}




///***********************************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Validar_Datos
///DESCRIPCIÓN          : Funcion para validar los datos del formulario
///PARAMETROS           : 
///CREO                 : Hugo Enrique Ramírez Aguilera
///FECHA_CREO           : 22/Mayo/2012
///MODIFICO             : 
///FECHA_MODIFICO       : 
///CAUSA_MODIFICACIÓN   : 
///***********************************************************************************************************************
function Validar_Datos_Enviar_Password()
{
    Mensaje_Error_Password('Ocultar');
    var Datos_Validos = true;
    var Encabezado_Error = 'Es necesario Introducir: ';
    var Error = "";
    var Pregunta_Secreta = "";
    
    try
    {
//            Pregunta_Secreta = $("select[id$=Cmb_Pregunta_Email_Pass] option:selected").text();
//            if(Pregunta_Secreta=="- Selecciona -"){
//                Error += "la pregunta secreta, ";
//                Datos_Validos = false;
//            }
            if($("input[id$=Txt_Email_Pass]").val() == "" && $("input[id$=Txt_Email_Pass]").val().length >= 0) {
                Error += "Email, ";
                Datos_Validos = false;
            }
//            if( $("input[id$=Txt_Respuesta_Email_Pass]").val()== "" && $("input[id$=Txt_Respuesta_Email_Pass]").val().length <= 0){
//                 Error += "respuesta secreta ";
//                Datos_Validos = false;
//            }
                
        $("[id$=Lbl_Encabezado_Error_Pass]").text(Encabezado_Error);
        $("[id$=Lbl_Mensaje_Error_Pass]").text(Error);
        return Datos_Validos;
    }catch(Ex){
        $.messager.alert('Mensaje','Error al validar los datos de la pagina. Error: [' + Ex + ']');
    }
}

///***********************************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Mensaje_Error
///DESCRIPCIÓN          : Funcion para mostrar u ocultar los errores
///PARAMETROS           1 Opcion: para mostrar y ocultar los campos 
///CREO                 : Leslie González Vázquez
///FECHA_CREO           : 02/Mayo/2012
///MODIFICO             : 
///FECHA_MODIFICO       : 
///CAUSA_MODIFICACIÓN   : 
///***********************************************************************************************************************
function Mensaje_Error(Opcion)
{
    try
    {
        if(Opcion == 'Mostrar')
        {
            $('#Div_Contenedor_Msj_Error').show();
        }
        else
        {
            $('#Div_Contenedor_Msj_Error').hide();
        } 
    }catch(Ex){
        $.messager.alert('Mensaje','Error al mostrar el error. Error: [' + Ex + ']');
    }
}

///***********************************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Mensaje_Error_Password
///DESCRIPCIÓN          : Funcion para mostrar u ocultar los errores
///PARAMETROS           1 Opcion: para mostrar y ocultar los campos 
///CREO                 : Hugo Enrique Ramírez Aguilera
///FECHA_CREO           : 22/Mayo/2012
///MODIFICO             : 
///FECHA_MODIFICO       : 
///CAUSA_MODIFICACIÓN   : 
///***********************************************************************************************************************
function Mensaje_Error_Password(Opcion)
{
    try
    {
        if(Opcion == 'Mostrar')
        {
            $('#Div_Mensaje_Error_Pass').show();
        }
        else
        {
            $('#Div_Mensaje_Error_Pass').hide();
        } 
    }catch(Ex){
        $.messager.alert('Mensaje','Error al mostrar el error. Error: [' + Ex + ']');
    }
}

///***********************************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Registrar_Usuario
///DESCRIPCIÓN          : Funcion para registrar los datos del usuario
///PARAMETROS           :
///CREO                 : Leslie González Vázquez
///FECHA_CREO           : 02/Mayo/2012
///MODIFICO             : 
///FECHA_MODIFICO       : 
///CAUSA_MODIFICACIÓN   : 
///***********************************************************************************************************************
function Registrar_Usuario()
{
    var Nombre = "";
    var Paterno = "";
    var Materno = "";
    var Curp = "";
    var Rfc = "";
    var F_Nacimiento = "";
    var Edad = "";
    var Sexo = "";
    var Email = "";
    var Calle = "";
    var Colonia = "";
    var Cp = "";
    var Ciudad = "";
    var Estado = "";
    var T_Casa = "";
    var T_Celular = "";
    var Parametros = "";
    var Pregunta_Secreta = "";
    var Respuesta_Secreta = "";
    var Password = "";
    
    try
    {
       if($("input[id$=Txt_Nombre]").val() != "" && $("input[id$=Txt_Nombre]").val().length > 0 )
       {
            Nombre = $("input[id$=Txt_Nombre]").val();
       }
       if($("input[id$=Txt_Apellido_Paterno]").val() != "" && $("input[id$=Txt_Apellido_Paterno]").val().length > 0 )
       {
            Paterno = $("input[id$=Txt_Apellido_Paterno]").val();
       }
       if($("input[id$=Txt_Apellido_Materno]").val() != "" && $("input[id$=Txt_Apellido_Materno]").val().length > 0 )
       {
            Materno = $("input[id$=Txt_Apellido_Materno]").val();
       }
       if($("input[id$=Txt_Curp]").val() != "" && $("input[id$=Txt_Curp]").val().length > 0 )
       {
            Curp = $("input[id$=Txt_Curp]").val();
       }
       if($("input[id$=Txt_Rfc]").val() != "" && $("input[id$=Txt_Rfc]").val().length > 0 )
       {
            Rfc = $("input[id$=Txt_Rfc]").val();
       }
       if($("input[id$=Txt_Fecha_Nacimiento]").val() != "" && $("input[id$=Txt_Fecha_Nacimiento]").val().length > 0 )
       {
            F_Nacimiento = $("input[id$=Txt_Fecha_Nacimiento]").val();
       }
       if($("input[id$=Txt_Edad]").val() != "" && $("input[id$=Txt_Edad]").val().length > 0 )
       {
            Edad = $("input[id$=Txt_Edad]").val();
       }
       if($("input[id$=Txt_Email]").val() != "" && $("input[id$=Txt_Email]").val().length > 0 )
       {
            Email = $("input[id$=Txt_Email]").val();
       }
       
       Sexo = $("select[id$=Cmb_Sexo] option:selected").text();
       
       Pregunta_Secreta = $("select[id$=Cmb_Pregunta_Secreta] option:selected").text();
       
       if($("input[id$=Txt_Respuesta_Secreta]").val() != "" && $("input[id$=Txt_Respuesta_Secreta]").val().length > 0 )
       {
            Respuesta_Secreta = $("input[id$=Txt_Respuesta_Secreta]").val();
       }       
       if($("input[id$=Txt_Calle]").val() != "" && $("input[id$=Txt_Calle]").val().length > 0 )
       {
            Calle = $("input[id$=Txt_Calle]").val();
       }
       if($("input[id$=Txt_Colonia]").val() != "" && $("input[id$=Txt_Colonia]").val().length > 0 )
       {
            Colonia = $("input[id$=Txt_Colonia]").val();
       }
       if($("input[id$=Txt_CP]").val() != "" && $("input[id$=Txt_CP]").val().length > 0 )
       {
            Cp = $("input[id$=Txt_CP]").val();
       }
       if($("input[id$=Txt_Ciudad]").val() != "" && $("input[id$=Txt_Ciudad]").val().length > 0 )
       {
            Ciudad = $("input[id$=Txt_Ciudad]").val();
       }
       if($("input[id$=Txt_Estado]").val() != "" && $("input[id$=Txt_Estado]").val().length > 0 )
       {
            Estado = $("input[id$=Txt_Estado]").val();
       }
       if($("input[id$=Txt_Telefono_Casa]").val() != "" && $("input[id$=Txt_Telefono_Casa]").val().length > 0 )
       {
           T_Casa = $("input[id$=Txt_Telefono_Casa]").val();
       }
       if($("input[id$=Txt_Telefono_Celular]").val() != "" && $("input[id$=Txt_Telefono_Celular]").val().length > 0 )
       {
            T_Celular = $("input[id$=Txt_Telefono_Celular]").val();
       }
       
       //   para el password
        if($("input[id$=Txt_Registrar_Password]").val() != "" && $("input[id$=Txt_Registrar_Password]").val().length > 6 )
       {
            Password = $("input[id$=Txt_Registrar_Password]").val();
       }
       
       Parametros =  "&Nombre="+ Nombre + "&Paterno="+Paterno+ "&Materno="+ Materno + "&Curp="+Curp + "&Rfc="+Rfc;
       Parametros += "&F_Nacimiento="+F_Nacimiento + "&Edad="+Edad +"&Sexo="+Sexo + "&Email="+Email;
       Parametros += "&Calle="+Calle + "&Colonia="+Colonia +"&Cp="+Cp + "&Ciudad="+Ciudad;
       Parametros += "&Estado="+Estado + "&T_Casa="+T_Casa  +"&T_Celular="+T_Celular;
       Parametros += "&Respuesta_Secreta="+Respuesta_Secreta + "&Pregunta_Secreta="+Pregunta_Secreta;
       Parametros += "&Password="+Password ;
       
       $.ajax({
            url: "../Atencion_Ciudadana/Frm_Cat_Ven_Registro_Usuarios.aspx?Accion=Registrarse" +Parametros,
            type:'POST',
            async: false,
            cache: false,
            success: function(Datos) {
                 if (Datos == "Registrado") 
                 {
                    Limpiar_Controles();
                    $('#Div_Registro').window('close');
                     $.messager.alert('Mensaje',"Su Usuario y Contraseña para accesar al sistema fueron enviados a su correo.");
                 }
                 else if (Datos == "Iguales") 
                 {
                     Limpiar_Controles();
                     $('#Div_Registro').window('close');
                     $.messager.alert('Mensaje',"Su Usuario y Contraseña para accesar al sistema fueron enviados de nuevo a su correo, porque usted ya se encuentra registrado.");
                 }
                 else 
                 {
                      $.messager.alert('Mensaje',"No se pudo registrar, vuelva a intentar");
                 }
            }
        });
    }catch(Ex){
        $.messager.alert('Mensaje','Error al registrarse. Error: [' + Ex + ']');
    }
}

///***********************************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Limpiar_Controles
///DESCRIPCIÓN          : Funcion para limpiar los controles
///PARAMETROS           :
///CREO                 : Leslie González Vázquez
///FECHA_CREO           : 02/Mayo/2012
///MODIFICO             : 
///FECHA_MODIFICO       : 
///CAUSA_MODIFICACIÓN   : 
///***********************************************************************************************************************
function Limpiar_Controles()
{
     $("input[id$=Txt_Nombre]").val("");
     $("input[id$=Txt_Apellido_Paterno]").val("");
     $("input[id$=Txt_Apellido_Materno]").val("");
     $("input[id$=Txt_Curp]").val("");
     $("input[id$=Txt_Rfc]").val("");
     $("input[id$=Txt_Fecha_Nacimiento]").val("");
     $("input[id$=Txt_Edad]").val("");
     $("input[id$=Txt_Email]").val("");
     $("input[id$=Txt_Calle]").val("");
     $("input[id$=Txt_Colonia]").val("");
     $("input[id$=Txt_CP]").val("");
     $("input[id$=Txt_Ciudad]").val("");
     $("input[id$=Txt_Estado]").val("");
     $("input[id$=Txt_Telefono_Casa]").val("");
     $("input[id$=Txt_Telefono_Celular]").val("");
     $("input[id$=Txt_Confirmar_Email]").val("");
     $("input[id$=Txt_Respuesta_Calve]").val("");
     $("input[id$=Txt_Telefono_Casa]").val("");
     $("input[id$=Txt_Telefono_Celular]").val("");
     $("input[id$=Txt_Registrar_Password]").val("");
}

///***********************************************************************************************************************
///NOMBRE DE LA FUNCIÓN : Consultar_Password_Usuario
///DESCRIPCIÓN          : Funcion para registrar consultar los datos del usuario
///PARAMETROS           :
///CREO                 : Leslie González Vázquez
///FECHA_CREO           : 02/Mayo/2012
///MODIFICO             : 
///FECHA_MODIFICO       : 
///CAUSA_MODIFICACIÓN   : 
///***********************************************************************************************************************
function Consultar_Password_Usuario()
{
    var Email = "";
    var Pregunta_Secreta = "";
    var Respuesta_Secreta = "";
    
    try
    {
       if($("input[id$=Txt_Email_Pass]").val() != "" && $("input[id$=Txt_Email_Pass]").val().length > 0 )
       {
            Email = $("input[id$=Txt_Email_Pass]").val();
       }

       
       
       if($("input[id$=Txt_Respuesta_Email_Pass]").val() != "" && $("input[id$=Txt_Respuesta_Email_Pass]").val().length > 0 )
       {
            Respuesta_Secreta = $("input[id$=Txt_Respuesta_Email_Pass]").val();
            Pregunta_Secreta = $("select[id$=Cmb_Pregunta_Email_Pass] option:selected").text();
       }   
       
       Parametros = "&Email="+Email;
       Parametros += "&Respuesta_Secreta="+Respuesta_Secreta + "&Pregunta_Secreta="+Pregunta_Secreta;

       $.ajax({
            url: "../Atencion_Ciudadana/Frm_Cat_Ven_Registro_Usuarios.aspx?Accion=Consultar_Password" +Parametros,
            type:'POST',
            async: false,
            cache: false,
            success: function(Datos) {
                 $("input[id$=Txt_Email_Pass]").val("");
                 $('#Div_Pass').window('close');
                 if (Datos == "Listo") 
                 {
                    $.messager.alert('Mensaje',"Su Usuario y Contraseña para accesar al sistema fueron enviados a su correo.");
                 }
                 else 
                 {
                      $.messager.alert('Mensaje',"Usted no se encuentra registrado");
                 }
            }
        });
    }catch(Ex){
        $.messager.alert('Mensaje','Error consultar el password. Error: [' + Ex + ']');
    }
}
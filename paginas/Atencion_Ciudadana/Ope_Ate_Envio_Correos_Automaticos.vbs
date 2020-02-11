Call Envio_Correos()

Sub Envio_Correos()

        ' forzar al script a terminar si hay errores
        On Error Resume Next

        ' Variables
        Dim Obj_Peticion
        Dim Ruta_Pagina

        Set Obj_Peticion = CreateObject("Microsoft.XMLHTTP")

        'página para envio de correos
        Ruta_Pagina = "http://192.168.1.204/presidencia/paginas/Atencion_Ciudadana/Frm_Ope_Ate_Enviar_Correos.aspx"

        'abrir petición HTTP y pasar la URL al objeto Obj_Peticion
        Obj_Peticion.open "POST", Ruta_Pagina , false

        ' enviar peticion
        Obj_Peticion.Send

        Set Obj_Peticion = Nothing

End Sub
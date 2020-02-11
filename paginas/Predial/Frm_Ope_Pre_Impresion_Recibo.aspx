<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Impresion_Recibo.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Impresion_Recibo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Recibo Oficial</title>
    <meta http-equiv="X-UA-Compatible" content="IE=9" />
    <link href="Impresion_Recibos/Cs_Ope_Pre_Impresion_Recibo.css" rel="stylesheet" type="text/css" media="print"/>
    <link href="Impresion_Recibos/Cs_Ope_Pre_Impresion_Recibo.css" rel="stylesheet" type="text/css"/>
    <script src="../../javascript/jquery/jquery-1.5.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        function printpage() {
            window.print();
        }
    </script>

    <script src="../javascript/Js_Ope_Pre_Impresion_Recibo.js" type="text/javascript"></script>
</head>
<body>
    <form id="Frm_Ope_Pre_Impresion_Recibo" action="">
        <div id="Div_Recibo_Oficial" class="Div_Recibo_Oficial">
            <div id="Encabezado" class="Encabezado" >
                <%--Aqui se insertara el codigo del encabezado del recibo--%>
            </div>
            <div class="espaciador">&nbsp;</div>
            <div id="Detalles" class="Detalles">
                <div id="Div_Importe">
                        
                </div>
                <div class="Div_Descripcion">
                    <div id="Div_Nombre_Detalles">
                        <%--Aqui va la descripcion de las claves de ingresos--%>
                    </div>
                    <div id="Div_Calculos">
                        <div id="Div_Signo">
                            <%--Aqui va el $ del recibo--%>
                        </div>
                        <div id="Div_Monto">
                            <%--Aqui van las cantidades del recibo--%>
                        </div>
                    </div>
                </div>
                <div class="espaciador">&nbsp;</div>
                <div class="Pie_Pagina" id="Pie_Pagina">
                    <%--Aqui van la base impuesto y la base gravable del recibo o la cuota minima--%>
                </div>
                <div class="espaciador">&nbsp;</div>
                <div class="Div_Convenio" id="Div_Convenio">
                    <%--Aqui van los datos del convenio del recibo--%>
                </div>
            </div>
            <div class="espaciador">&nbsp;</div>
            <div id="Div_Observaciones" class="Div_Observaciones">
                <%--Aqui van las Observaciones del recibo--%>
            </div>
            <div id="Proteccion" class="Proteccion">
                <%--Aqui van la proteccion del recibo--%>
            </div>
            <div id="Div_Cancelacion" class="Div_Cancelacion">
                <%--Aqui van la proteccion del recibo--%>
            </div>
        </div>
    </form>
</body>
</html>




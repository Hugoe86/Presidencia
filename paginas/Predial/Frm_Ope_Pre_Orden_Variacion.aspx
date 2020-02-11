<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Orden_Variacion.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Orden_Variacion"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .Tabla_Comentarios
        {
            border-collapse: collapse;
            margin-left: 25px;
            color: #25406D;
            font-family: Verdana,Geneva,MS Sans Serif;
            font-size: small;
            text-align: left;
        }
        .Tabla_Comentarios, .Tabla_Comentarios th, .Tabla_Comentarios td
        {
            border: 1px solid #999999;
            padding: 2px 10px;
        }
        .style1
        {
            width: 20%;
            height: 48px;
        }
        .style2
        {
            width: 30%;
            height: 48px;
        }
        .style3
        {
            height: 48px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type="text/javascript">
            
            Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(initRequest);   
            function initRequest(sender, args)   
            {   
                if(Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack())   
                    args.set_cancel(true);                    
            }
            function myConfirm(text,button1,button2,answerFunc)
            {
	            var box = document.getElementById("confirmBox");
	            box.getElementsByTagName("p")[0].firstChild.nodeValue = text;
	            var button = box.getElementsByTagName("input");
	            button[0].value=button1;
	            button[1].value=button2;
	            answerFunction = answerFunc;
	            box.style.visibility="visible";
            }
        function confirmacion() {
        var Resultado;             
            Resultado = confirm("Se generaran adeudos en base a los efectos: " + document.getElementById('<%= Cmb_Efectos_Numero.ClientID %>').value + "/" + document.getElementById('<%= Cmb_Efectos.ClientID %>').value);
             document.getElementById('<%= Hdn_Respuesta_Confirmacion.ClientID %>').value = Resultado;
        }
            function Copiar_Texto() {
                var Beneficio = "";
                var Combo = "";
                var Diferencia = 0.00;
                var Origen = document.getElementById('<%= Txt_Superficie_Construida.ClientID %>').value;
                var Superficie = document.getElementById('<%= Hdn_Superficie_Construccion.ClientID %>').value;
                var Opcion_Orden = "";
                Combo = document.getElementById('<%= Cmb_Financiado.ClientID %>');
                Beneficio = Combo.options[Combo.selectedIndex].text.toUpperCase();
                Opcion_Orden = document.getElementById('<%= Hdn_Opcion_Tipo_Orden.ClientID %>').value;
                if (Superficie > 0)
                {
                //Validar opcion de predial
                    //Validar opcion de beneficio
                if (Opcion_Orden.indexOf("Predial") !=-1 )
                {
                    if(Beneficio.indexOf("ISSSTE") !=-1 || Beneficio.indexOf("INFONAVIT") !=-1 || Beneficio.indexOf("FOVISSSTE") !=-1 || Beneficio.indexOf("DIFERENCIAS") !=-1)
                    {   
                    
                            if (document.getElementById('<%= Hdn_Propietario_Validacion_Persona.ClientID %>').value == "FISICA") {
                                if (document.getElementById('<%= Hdn_Propietario_Validacion_Superficie.ClientID %>').value == document.getElementById('<%= Hdn_Propietario_ID.ClientID %>').value) {
                                    Diferencia = Origen - Superficie;
                                    document.getElementById('<%= Txt_Dif_Construccion.ClientID %>').value = Diferencia;
                                    document.getElementById('<%= Txt_Exedente_Construccion.ClientID %>').value = Diferencia;
                                }
                                else {
                                    alert("Se encontró un propietario diferente. La diferencia de superficie se reiniciará a cero");
                                    document.getElementById('<%= Txt_Dif_Construccion.ClientID %>').value = "0";
                                    document.getElementById('<%= Txt_Exedente_Construccion.ClientID %>').value = "0";
                                }
                            }
                            else {
                                document.getElementById('<%= Txt_Exedente_Construccion.ClientID %>').value = "0";
                            }
                        }
                    }
                    
                }
            }
            function Copiar_Diferencia() {
                
                var Diferencia = 0.00;
                var Origen = document.getElementById('<%= Txt_Dif_Construccion.ClientID %>').value;
                document.getElementById('<%= Txt_Exedente_Construccion.ClientID %>').value = Origen;
            }
            function Mismo_Domicilio(){
                var Combo = "";
                var Texto_Combo = ""; 
                var Valor_Combo = "";
                var Origen1 = false;
                if(document.getElementById('<%= Chk_Mismo_Domicilio.ClientID %>') != null)
                Origen1 = document.getElementById('<%= Chk_Mismo_Domicilio.ClientID %>').checked;                
                
            if(Origen1) //Mismo Domicilio
            {
                    document.getElementById('<%= Txt_Colonia_Propietario.ClientID %>').disabled = true;
                    document.getElementById('<%= Txt_Calle_Propietario.ClientID %>').disabled = true;
                    document.getElementById('<%= Txt_Numero_Exterior_Propietario.ClientID %>').disabled = true;
                    document.getElementById('<%= Txt_Numero_Interior_Propietario.ClientID %>').disabled = true;                    
                    document.getElementById('<%= Cmb_Domicilio_Foraneo.ClientID %>').disabled = true;
                    document.getElementById('<%= Txt_Estado.ClientID %>').disabled = true;
                    document.getElementById('<%= Txt_Ciudad.ClientID %>').disabled = true;                                        
                    document.getElementById('<%= Btn_Seleccionar_Calle.ClientID %>').style.display="none";
                    document.getElementById('<%= Cmb_Domicilio_Foraneo.ClientID %>').value = 'NO';                    
                    Combo = document.getElementById("<%=Txt_Calle_Cuenta.ClientID%>").value;
                    document.getElementById('<%= Txt_Calle_Propietario.ClientID %>').value = Combo;
                    Combo = document.getElementById("<%=Txt_Colonia_Cuenta.ClientID%>").value;
                    document.getElementById('<%= Txt_Colonia_Propietario.ClientID %>').value = Combo;                    
                    Texto_Combo = document.getElementById('<%= Txt_No_Exterior.ClientID %>').value;
                    document.getElementById('<%= Txt_Numero_Exterior_Propietario.ClientID %>').value = Texto_Combo;
                    Texto_Combo = document.getElementById('<%= Txt_No_Interior.ClientID %>').value;
                    document.getElementById('<%= Txt_Numero_Interior_Propietario.ClientID %>').value = Texto_Combo;
                    document.getElementById('<%= Txt_Estado.ClientID %>').value="GUANAJUATO";
                    document.getElementById('<%= Txt_Ciudad.ClientID %>').value="IRAPUATO";
                    document.getElementById('<%= Hdn_Colonia_ID_Notificacion.ClientID %>').value = document.getElementById('<%= Hdn_Colonia_ID.ClientID %>').value
                    document.getElementById('<%= Hdn_Calle_ID_Notificacion.ClientID %>').value = document.getElementById('<%= Hdn_Calle_ID.ClientID %>').value
                    
            }
                else {
                    //Otro Domicilio 
                    Combo = document.getElementById("<%=Cmb_Domicilio_Foraneo.ClientID%>");
                    Texto_Combo = Combo.options[Combo.selectedIndex].text;
                    document.getElementById('<%= Txt_Colonia_Propietario.ClientID %>').disabled = true;
                    document.getElementById('<%= Txt_Calle_Propietario.ClientID %>').disabled = true;
                    document.getElementById('<%= Btn_Seleccionar_Calle.ClientID %>').style.display = "inline";
                    if (Texto_Combo == 'SI') 
                    {
                        document.getElementById('<%= Txt_Colonia_Propietario.ClientID %>').disabled = false;
                        document.getElementById('<%= Txt_Calle_Propietario.ClientID %>').disabled = false;
                        document.getElementById('<%= Btn_Seleccionar_Calle.ClientID %>').style.display = "none";
                    }
                    
                    document.getElementById('<%= Txt_Numero_Exterior_Propietario.ClientID %>').disabled = false;
                    document.getElementById('<%= Txt_Numero_Interior_Propietario.ClientID %>').disabled = false;                    
                    document.getElementById('<%= Cmb_Domicilio_Foraneo.ClientID %>').disabled = false;
                    
                }
            }
            function formatCurrency(num) {
            var Combo = "";
            var Beneficio;
                num = num.toString().replace(/\$|\,/g,'');
                if(isNaN(num))
                num = "0";
                sign = (num == (num = Math.abs(num)));
                num = Math.floor(num*100+0.50000000001);
                cents = num%100;
                num = Math.floor(num/100).toString();
                if(cents<10)
                    cents = "0" + cents;
                    for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
                    num = num.substring(0,num.length-(4*i+3))+','+
                    num.substring(num.length-(4*i+3));
                    return (((sign)?'':'-') + num + '.' + cents);
            }
            function Cuota_Fija(){
                var Origen1 = document.getElementById('<%= Chk_Cuota_Fija.ClientID %>').checked;
                
            if(Origen1)
            {
                document.getElementById("<%=Pnl_Detalles_Cuota_Fija.ClientID%>").style.display="inline";                
            }
            else{            
                document.getElementById('<%= Pnl_Detalles_Cuota_Fija.ClientID %>').style.display="none";                
                }
            }
            function Quitar_Cuota_Fija() {
                var Origen1 = document.getElementById('<%= Chk_Cuota_Fija.ClientID %>').checked;
                if (!Origen1) {
                    Combo = document.getElementById('<%= Cmb_Solicitante.ClientID %>');
                    Beneficio = Combo.options[Combo.selectedIndex].text.toUpperCase();
                    if (Beneficio.indexOf("JUBILA") != -1 || Beneficio.indexOf("PENSION") != -1 || Beneficio.indexOf("TERCER") != -1) {
                        //confirmacion();
                        //document.getElementById('<%= Lbl_Defuncion.ClientID %>').style.display="inline";
                        //document.getElementById('<%= Txt_Fecha_Def.ClientID %>').style.display="inline";
                        //document.getElementById('<%= Btn_CE_Fecha_Defuncion.ClientID %>').style.display="inline";
                    }
                    else {
                        if (document.getElementById('<%= Hdn_Respuesta_Confirmacion.ClientID %>').value == "true") {
                            //document.getElementById('<%= Lbl_Defuncion.ClientID %>').style.display="none";
                            //document.getElementById('<%= Txt_Fecha_Def.ClientID %>').style.display="none";
                            //document.getElementById('<%= Btn_CE_Fecha_Defuncion.ClientID %>').style.display="none";
                        }
                    }
                }                
            }
            function Foraneo(){
                    Combo = document.getElementById("<%=Cmb_Domicilio_Foraneo.ClientID%>"); 
                    Texto_Combo = Combo.options[Combo.selectedIndex].text;
            if(Texto_Combo == 'SI')//Es foraneo
            {
                    document.getElementById('<%= Txt_Colonia_Propietario.ClientID %>').disabled = false;
                    document.getElementById('<%= Txt_Calle_Propietario.ClientID %>').disabled = false;
                    document.getElementById('<%= Txt_Colonia_Propietario.ClientID %>').value = "";
                    document.getElementById('<%= Txt_Calle_Propietario.ClientID %>').value = "";
                    document.getElementById('<%= Txt_Numero_Exterior_Propietario.ClientID %>').disabled = false;
                    document.getElementById('<%= Txt_Numero_Interior_Propietario.ClientID %>').disabled = false;                    
                    document.getElementById('<%= Cmb_Domicilio_Foraneo.ClientID %>').disabled = false;                    
                    document.getElementById('<%= Txt_Ciudad.ClientID %>').disabled = false;
                    document.getElementById('<%= Txt_Estado.ClientID %>').disabled = false;
                    document.getElementById('<%= Btn_Seleccionar_Calle.ClientID %>').style.display="none";                    
            }
                else {            //Es local
                    document.getElementById('<%= Txt_Colonia_Propietario.ClientID %>').value = "";
                    document.getElementById('<%= Txt_Calle_Propietario.ClientID %>').value = "";
                    document.getElementById('<%= Txt_Colonia_Propietario.ClientID %>').disabled = true;
                    document.getElementById('<%= Txt_Calle_Propietario.ClientID %>').disabled = true;
                    document.getElementById('<%= Txt_Numero_Exterior_Propietario.ClientID %>').disabled = false;
                    document.getElementById('<%= Txt_Numero_Interior_Propietario.ClientID %>').disabled = false;                    
                    document.getElementById('<%= Cmb_Domicilio_Foraneo.ClientID %>').disabled = false;
                    document.getElementById('<%= Txt_Ciudad.ClientID %>').disabled = true;
                    document.getElementById('<%= Txt_Estado.ClientID %>').disabled = true;
                    document.getElementById('<%= Btn_Seleccionar_Calle.ClientID %>').style.display="inline";                    
                    document.getElementById('<%= Txt_Estado.ClientID %>').value="GUANAJUATO";
                    document.getElementById('<%= Txt_Ciudad.ClientID %>').value="IRAPUATO";                    
                }
            }
            function Foraneo_Colonia() {
                    document.getElementById('<%= Cmb_Domicilio_Foraneo.ClientID %>').value = 'NO';
                    document.getElementById('<%= Txt_Colonia_Propietario.ClientID %>').value = "";
                    document.getElementById('<%= Txt_Calle_Propietario.ClientID %>').value = "";
                    document.getElementById('<%= Txt_Colonia_Propietario.ClientID %>').disabled = true;
                    document.getElementById('<%= Txt_Calle_Propietario.ClientID %>').disabled = true;
                    document.getElementById('<%= Txt_Numero_Exterior_Propietario.ClientID %>').disabled = false;
                    document.getElementById('<%= Txt_Numero_Interior_Propietario.ClientID %>').disabled = false;
                    document.getElementById('<%= Cmb_Domicilio_Foraneo.ClientID %>').disabled = false;
                    document.getElementById('<%= Txt_Ciudad.ClientID %>').disabled = true;
                    document.getElementById('<%= Txt_Estado.ClientID %>').disabled = true;                    
                    document.getElementById('<%= Txt_Estado.ClientID %>').value = "GUANAJUATO";
                    document.getElementById('<%= Txt_Ciudad.ClientID %>').value = "IRAPUATO";               
            }
            function Validar_Longitud_Texto(Text_Box, Max_Longitud){
                if (Text_Box.value.length > Max_Longitud){
                    Text_Box.value = Text_Box.value.substring(0, Max_Longitud);
                }
            }
            document.onkeydown = function () {
                //116->f5
                //122->f11
                if (window.event) {

                    if (window.event && (window.event.keyCode == 122 || window.event.keyCode == 116)) {
                        window.event.keyCode = 505;
                        alert('aaaaaaaaaaa');
                    }

                    if (window.event.keyCode == 505) {
                        return false;
                    }
                    if (window.event && (window.event.keyCode == 8)) {
                        valor = document.activeElement.value;
                        if (valor == undefined) { return false; } //Evita Back en página.
                        else {
                            if (document.activeElement.getAttribute('type') == 'select-one')
                            { return false; } //Evita Back en select.
                            if (document.activeElement.getAttribute('type') == 'button')
                            { return false; } //Evita Back en button.
                            if (document.activeElement.getAttribute('type') == 'radio')
                            { return false; } //Evita Back en radio.
                            if (document.activeElement.getAttribute('type') == 'checkbox')
                            { return false; } //Evita Back en checkbox.
                            if (document.activeElement.getAttribute('type') == 'file')
                            { return false; } //Evita Back en file.
                            if (document.activeElement.getAttribute('type') == 'reset')
                            { return false; } //Evita Back en reset.
                            if (document.activeElement.getAttribute('type') == 'submit')
                            { return false; } //Evita Back en submit.                        
                            else //Text, textarea o password
                            {
                                if (document.activeElement.type == 'select-one')
                                { return false; } //Evita Back en select.
                                if (document.activeElement.value.length == 0)
                                { return false; } //No realiza el backspace(largo igual a 0).
                                else
                                { document.activeElement.value.keyCode = 8; } //Realiza el backspace.
                            }
                        }
                    }
                }
                // keycode for firefox, safari and opera
                else {
                    var evt;
                    handleKeyPress(evt);
                }
            }

            function handleKeyPress(evt) {
                var nbr;
                var nbr = (window.event) ? event.keyCode : evt.which;
                if (nbr == 8) {
                    valor = document.activeElement.value;
                    if (valor == undefined) { return false; } //Evita Back en página.
                    else {
                        if (document.activeElement.getAttribute('type') == 'select-one')
                        { return false; } //Evita Back en select.
                        if (document.activeElement.getAttribute('type') == 'button')
                        { return false; } //Evita Back en button.
                        if (document.activeElement.getAttribute('type') == 'radio')
                        { return false; } //Evita Back en radio.
                        if (document.activeElement.getAttribute('type') == 'checkbox')
                        { return false; } //Evita Back en checkbox.
                        if (document.activeElement.getAttribute('type') == 'file')
                        { return false; } //Evita Back en file.
                        if (document.activeElement.getAttribute('type') == 'reset')
                        { return false; } //Evita Back en reset.
                        if (document.activeElement.getAttribute('type') == 'submit')
                        { return false; } //Evita Back en submit.                        
                        else //Text, textarea o password
                        {
                            if (document.activeElement.type == 'select-one')
                            { return false; } //Evita Back en select.
                            if (document.activeElement.value.length == 0)
                            { return false; } //No realiza el backspace(largo igual a 0).
                            else
                            { document.activeElement.value.keyCode = 8; } //Realiza el backspace.
                        }
                    }
                }
                return 0;
            }
            document.onkeydown = handleKeyPress
            
    </script>

    <script type="text/javascript" language="javascript">
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_sesiones.ashx";

        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion()
        {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }

        //Temporizador para matener la sesión activa
        setInterval('MantenSesion()', <%=(int)(0.9*(Session.Timeout * 60000))%>);

        window.onerror = new Function("return true");
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades)
        {
            window.showModalDialog(Url, null, Propiedades);
        }

        function Abrir_Resumen(Url, Propiedades)
        {
            window.open(Url, 'Resumen_Predio', Propiedades);
        }
        function Abrir_Vista_Adeudos(Url, Propiedades)
        {
            window.open(Url, 'Vista_Previa_Adeudos', Propiedades);
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" EnablePartialRendering="true" AsyncPostBackTimeout="9000">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Busqueda" runat="server" style="background-color: #ffffff; width: 100%;
                height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Orden de variación
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />
                            &nbsp;
                            <asp:Label ID="Lbl_Encabezado_Error" runat="server" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="98%" border="0" cellspacing="0">
                    <tr id="Tr1" align="center" runat="server">
                        <td>
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 40%;">
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click"
                                                Visible="false" />
                                            <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                                                TabIndex="4" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" OnClick="Btn_Imprimir_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td align="right" style="width: 28%;">
                                            <table style="width: 100%; height: 28px;">
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: right; width: 5%">
                                                        <asp:ImageButton ID="Btn_Busqueda_Cuentas" runat="server" ToolTip="Buscar Por Cuenta Predial"
                                                            CssClass="Img_Button" TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                                            OnClick="Btn_Busqueda_Cuentas_Click" />
                                                    </td>
                                                    <td style="vertical-align: middle; text-align: right; width: 90%">
                                                        Búsqueda:
                                                        <asp:TextBox ID="Txt_Buscar" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar"
                                                            Width="180px" AutoPostBack="true" OnTextChanged="Txt_Buscar_TextChanged" />
                                                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<No Orden>" TargetControlID="Txt_Buscar" />
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="Txt_Buscar"
                                                            FilterType="Numbers" />
                                                    </td>
                                                    <td style="vertical-align: middle; text-align: right; width: 5%">
                                                        <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                            ToolTip="Buscar" OnClick="Btn_Buscar_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
            </div>
            <div id="Div_Busqueda_Cuenta" runat="server" style="background-color: #ffffff; width: 100%;
                height: 100%;">
                <asp:Panel ID="Pnl_Busqueda_Cuenta" runat="server" GroupingText="Búsqueda por Cuenta">
                    <table width="100%" class="estilo_fuente">
                        <tr>
                            <td colspan="4" style="text-align: left" align="left">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%">
                                Cuenta Predial
                            </td>
                            <td colspan="3" style="width: 80%">
                                <asp:TextBox ID="Txt_Busqueda_Cuenta" runat="server" ToolTip="Buscar por Cuenta Predial"
                                    Width="98%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%">
                                <asp:Label ID="Lbl_Busqueda_Contrarecibo" runat="server" Text="No Contrarecibo"></asp:Label>
                            </td>
                            <td colspan="3" style="width: 80%">
                                <asp:TextBox ID="Txt_Busqueda_Contrarecibo" runat="server" ToolTip="Buscar por Contrarecibo"
                                    Width="98%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="width: 100%; text-align: right" align="right">
                                <asp:ImageButton ID="Btn_Consultar_Ordenes_Cuenta" runat="server" TabIndex="6" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png"
                                    ToolTip="Buscar por Cuenta Predial" OnClick="Btn_Consultar_Ordenes_Cuenta_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="width: 100%;">
                                <%--Grid de Busqueda por cuenta Predial--%>
                                <asp:GridView ID="Grid_Ordenes_Variacion" runat="server" Style="white-space: normal;
                                    width: 100%;" AutoGenerateColumns="False" PageSize="10" AllowPaging="true" OnSelectedIndexChanged="Grid_Ordenes_Variacion_SelectedIndexChanged"
                                    OnPageIndexChanging="Grid_Ordenes_Variacion_PageIndexChanging" DataKeyNames="NO_ORDEN_VARIACION,NO_CONTRARECIBO,ESTATUS_ORDEN,ANIO">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="NO_ORDEN_VARIACION" HeaderText="No. Movimiento">
                                            <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ANIO" HeaderText="Año">
                                            <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                            <ItemStyle HorizontalAlign="Center" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Identificador_Movimiento" HeaderText=" Movimiento">
                                            <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                            <ItemStyle HorizontalAlign="Center" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Descripcion_Movimiento" HeaderText=" Movimiento">
                                            <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                            <ItemStyle HorizontalAlign="Center" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Cuenta_Predial" HeaderText="Cuenta Predial">
                                            <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                            <ItemStyle HorizontalAlign="Center" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NO_CONTRARECIBO" HeaderText="Contrarecibo" NullDisplayText="Directa"
                                            HtmlEncode="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                            <ItemStyle HorizontalAlign="Center" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS_ORDEN" HeaderText="Estatus">
                                            <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                            <ItemStyle HorizontalAlign="Center" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA_ORDEN" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}"
                                            HtmlEncode="false">
                                            <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                            <ItemStyle HorizontalAlign="Center" Width="12%" />
                                        </asp:BoundField>
                                    </Columns>
                                    <HeaderStyle CssClass="GridHeader" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div id="Div_Contenido" runat="server" style="background-color: #ffffff; width: 100%;
                height: 100%;">
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align: left; width: 20%">
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Contrarecibo" runat="server" MaxLength="10" TabIndex="5" Visible="false"
                                ToolTip="Cargar Contrarecibo" Width="98%" AutoPostBack="true" OnTextChanged="Txt_Contrarecibo_TextChanged" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Contrarecibo>" TargetControlID="Txt_Contrarecibo" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Contrarecibo"
                                FilterType="Numbers" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            *Tipo de movimiento
                        </td>
                        <td style="text-align: left; width: 80%;">
                            <asp:DropDownList ID="Cmb_Tipos_Movimiento" runat="server" Width="99%" TabIndex="7">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Pnl_Datos_Generales" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <%------------------ Datos generales ------------------%>
                        <tr style="background-color: #3366CC">
                            <td id="Barra_Generales" style="text-align: left; font-size: 15px; color: #FFFFFF;"
                                colspan="4" runat="server">
                                Generales
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 12%;">
                                *Cuenta Predial
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="96.4%" Enable="false"
                                    TabIndex="9" MaxLength="12">                                 
                                </asp:TextBox>
                                <asp:ImageButton ID="Btn_Establecer_Cuenta_Predial" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_circle_green.png"
                                    OnClick="Btn_Establecer_Cuenta_Predial_Click" AlternateText="Registrar Cuenta Predial"
                                    ToolTip="Registrar Cuenta Predial" />
                                <asp:ImageButton ID="ImageButton12" runat="server" ToolTip="Listado de cuentas pendientes de aplicar"
                                    TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Listado.png" Height="22px"
                                    Width="22px" Style="float: left" Visible="false" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:ImageButton ID="Btn_Resumen_Cuenta" runat="server" ToolTip="Resumen de cuenta"
                                    TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Height="22px"
                                    Width="22px" Style="float: left" Visible="true" />
                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Cuentas" runat="server" ToolTip="Búsqueda Avanzada de Cuenta Predial"
                                    TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px"
                                    Width="22px" OnClick="Btn_Mostrar_Busqueda_Cuentas_Click" />
                                Cuenta origen
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Cta_Origen" runat="server" Width="96.4%" Style="text-transform: uppercase"
                                    MaxLength="12" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Tipo predio
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Tipos_Predio" runat="server" Width="99%" TabIndex="7">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                *Uso predio
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Usos_Predio" runat="server" Width="99%" TabIndex="7">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Estado de predio
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Estados_Predio" runat="server" Width="99%" TabIndex="7">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Estatus
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" TabIndex="7" Width="99%">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Superficie construida (m²)
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Superficie_Construida" runat="server" Width="96.4%" Text="0"
                                    onchange="Copiar_Texto();" onBlur="this.value=formatCurrency(this.value);" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="Txt_Superficie_Construida"
                                    FilterType="Numbers,Custom" ValidChars="." />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                *Superficie Total (m²)
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Superficie_Total" runat="server" Width="96.4%" Text="0" onBlur="this.value=formatCurrency(this.value);" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="Txt_Superficie_Total"
                                    FilterType="Numbers,Custom" ValidChars="." />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel1" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td style="text-align: left; width: 20%">
                                Calle
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Calle_Cuenta" runat="server" Width="96.4%" Enabled="false" onchange="javascript:Mismo_Domicilio();"></asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right">
                                <asp:ImageButton ID="Btn_Seleccionar_Colonia" runat="server" ToolTip="Seleccionar Calle y Colonia"
                                    TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px"
                                    Width="22px" OnClick="Btn_Buscar_Colonias_Click" />
                                *Colonia
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Colonia_Cuenta" runat="server" Width="96.4%" Enabled="false"
                                    onchange="javascript:Mismo_Domicilio();"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Número exterior
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_No_Exterior" runat="server" Width="96.4%" Style="text-transform: uppercase"
                                    MaxLength="20" onkeypress="javascript:Mismo_Domicilio();" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="Txt_No_Exterior"
                                    FilterType="Numbers,LowercaseLetters,UppercaseLetters,Custom" ValidChars="°ñÑ/- " />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Número interior
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_No_Interior" runat="server" Width="96.4%" Style="text-transform: uppercase"
                                    MaxLength="80" onkeypress="javascript:Mismo_Domicilio();" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="Txt_No_Interior"
                                    FilterType="Numbers,LowercaseLetters,UppercaseLetters,Custom" ValidChars="°ñÑ´./- " />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Clave catastral
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Catastral" runat="server" Width="96.4%" Style="text-transform: uppercase" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="Txt_Catastral"
                                    FilterType="Numbers,LowercaseLetters,UppercaseLetters,Custom" ValidChars="´./- "/>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                *Efectos
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Efectos" runat="server">
                                </asp:DropDownList>
                                <asp:DropDownList ID="Cmb_Efectos_Numero" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Ultimo movimiento
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%;">
                                &nbsp;
                                <asp:Label ID="Lbl_Ultimo_Movimiento" runat="server" Font-Bold="True" ToolTip="Número de Contrarecibo"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div id="Pnl_Propietario" runat="server">
                    <%------------------ Propietario ------------------%>
                    <table width="98%" class="estilo_fuente">
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Propietario
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Nombre
                            </td>
                            <td style="text-align: left;" colspan="3">
                                <asp:TextBox ID="Txt_Nombre_Propietario" runat="server" Width="94.4%" Style="float: left"
                                    TextMode="MultiLine" />
                                <asp:ImageButton ID="Btn_Busqueda_Propietarios" runat="server" Height="22px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                    Style="float: right" TabIndex="10" ToolTip="Búsqueda Avanzada" Width="22px" OnClick="Btn_Busqueda_Propietarios_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                RFC
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Rfc_Propietario" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                &nbsp; *Propietario/Poseedor
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Tipos_Propietario" runat="server" TabIndex="7" AutoPostBack="true"
                                    OnSelectedIndexChanged="Cmb_Tipos_Propietario_SelectedIndexChanged" Width="99%">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Mismo domicilio
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:CheckBox ID="Chk_Mismo_Domicilio" runat="server" onclick="javascript:Mismo_Domicilio();" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Domicilio foráneo
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Domicilio_Foraneo" runat="server" Width="99%" TabIndex="7"
                                    onChange="javascript:Foraneo();">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%">
                                Calle
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Calle_Propietario" runat="server" Width="96.4%" Style="text-transform: uppercase" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" TargetControlID="Txt_Calle_Propietario"
                                    FilterType="Numbers,LowercaseLetters,UppercaseLetters,Custom" ValidChars="°ñÑ´./- "/>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:ImageButton ID="Btn_Seleccionar_Calle" runat="server" ToolTip="Seeleccionar Calle y Colonia"
                                    TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px"
                                    Width="22px" OnClick="Btn_Buscar_Calles_Click" OnClientClick="javascript:Foraneo_Colonia();"/>
                                Colonia
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Colonia_Propietario" runat="server" Width="96.4%" Style="text-transform: uppercase" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="Txt_Colonia_Propietario"
                                    FilterType="Numbers,LowercaseLetters,UppercaseLetters,Custom" ValidChars="°ñÑ´./- "/>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Número exterior
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Numero_Exterior_Propietario" Style="text-transform: uppercase"
                                    runat="server" Width="96.4%" MaxLength="20" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="Txt_Numero_Exterior_Propietario"
                                    FilterType="Numbers,LowercaseLetters,UppercaseLetters,Custom" ValidChars="°´./- "/>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Número interior
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Numero_Interior_Propietario" Style="text-transform: uppercase"
                                    runat="server" Width="96.4%" MaxLength="80" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="Txt_Numero_Interior_Propietario"
                                    FilterType="Numbers,LowercaseLetters,UppercaseLetters,Custom" ValidChars="°ñÑ´./- "/>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Estado
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Estado" runat="server" Width="96.4%" Style="text-transform: uppercase"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="Txt_Estado"
                                    FilterType="Numbers,LowercaseLetters,UppercaseLetters,Custom" ValidChars="´./- "/>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Ciudad
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Ciudad" runat="server" Width="96.4%" Style="text-transform: uppercase"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" TargetControlID="Txt_Ciudad"
                                    FilterType="Numbers,LowercaseLetters,UppercaseLetters,Custom" ValidChars="´./- "/>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                C.P.
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_CP" runat="server" Width="96.4%" MaxLength="5" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="Txt_CP"
                                    FilterType="Numbers" />
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Panel ID="Pnl_Impuestos" runat="server">
                    <%------------------ Impuestos ------------------%>
                    <table id="tbl_imp" width="98%" class="estilo_fuente">
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Impuestos
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Valor fiscal
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Valor_Fiscal" runat="server" Width="96.4%" AutoPostBack="true" OnTextChanged="Txt_Valor_Fiscal_TextChanged"
                                    onBlur="this.value=formatCurrency(this.value);" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="Txt_Superficie_Construida"
                                    FilterType="Numbers,Custom" ValidChars=".," />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                &nbsp;
                            </td>
                            <td style="text-align: left; width: 30%;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Costo (m²)
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Costo_M2" runat="server" Width="96.4%" Text="0" onBlur="this.value=formatCurrency(this.value);" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="Txt_Costo_M2"
                                    FilterType="Numbers,Custom" ValidChars="." />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                &nbsp;
                            </td>
                            <td style="text-align: left; width: 30%;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Tasa
                            </td>
                            <td style="text-align: left;" colspan="3">
                                <asp:TextBox ID="Txt_Tasa_Descripcion" runat="server" Width="75%" Enabled="false" />
                                &nbsp;
                                <asp:TextBox ID="Txt_Tasa_Porcentaje" runat="server" Width="75px" Enabled="false"
                                    AutoPostBack="true" OnTextChanged="Txt_Tasa_Porcentaje_TextChanged"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="Btn_Busqueda_Tasas" runat="server" Height="22px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                    OnClick="Btn_Busqueda_Tasas_Click" TabIndex="10" ToolTip="Búsqueda Avanzada"
                                    Width="22px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Periodo corriente
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Periodo_Corriente" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Cuota anual
                            </td>
                            <td style="text-align: left; width: 30%; text-align: right;">
                                <asp:TextBox ID="Txt_Cuota_Anual" runat="server" Enabled="false" onBlur="this.value=formatCurrency(this.value);"
                                    Width="90%" Style="float: left"/><asp:ImageButton ID="Btn_Regresar_Anualidad" runat="server" TabIndex="6" ImageUrl="~/paginas/imagenes/paginas/actualizar_detalle.png"
                                    ToolTip="Restaurar Cuota Anual" Width="16px" Height="16px" Style="float: right" OnClick="Btn_Regresar_Anualidad_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                &nbsp;Término exención
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Height="18px" MaxLength="11" TabIndex="12"
                                    ToolTip="Dia/Mes/Año" Width="84%" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Inicial" runat="server" Enabled="True" Format="dd/MMM/yyyy"
                                    PopupButtonID="Btn_CE_Txt_Fecha_Inicial" TargetControlID="Txt_Fecha_Inicial" />
                                <asp:ImageButton ID="Btn_CE_Txt_Fecha_Inicial" runat="server" CausesValidation="false"
                                    Height="18px" ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:ImageButton ID="Btn_Calcular_Cuota" runat="server" ImageUrl="~/paginas/imagenes/paginas/SIAS_Calc3.gif"
                                    TabIndex="8" ToolTip="Calcular Cuotas" Style="float: left" OnClick="Btn_Calcular_Cuota_Click" />
                                Cuota bimestral
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Cuota_Bimestral" runat="server" Width="96.4%" Enabled="false"
                                    onBlur="this.value=formatCurrency(this.value);" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Fecha avalúo
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Avaluo" runat="server" Height="18px" MaxLength="11" TabIndex="12"
                                    ToolTip="Dia/Mes/Año" Width="84%" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Avaluo" runat="server" Enabled="True" Format="dd/MMM/yyyy"
                                    PopupButtonID="Btn_CE_Fecha_Avaluo" TargetControlID="Txt_Fecha_Avaluo" />
                                <asp:ImageButton ID="Btn_CE_Fecha_Avaluo" runat="server" CausesValidation="false"
                                    Height="18px" ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                % Exención
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Porcentaje_Excencion" runat="server" Width="96.4%" OnTextChanged="Txt_Porcentaje_Excencion_TextChanged"
                                    AutoPostBack="True" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Cuota fija
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:CheckBox ID="Chk_Cuota_Fija" runat="server" Text="" AutoPostBack="true" onClick="javascript:Cuota_Fija();Quitar_Cuota_Fija();"
                                    OnCheckedChanged="Chk_Cuota_Fija_CheckedChanged" />
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>--%>
                <div id="Pnl_Detalles_Cuota_Fija" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <%------------------ Detalles cuota fija ------------------%>
                        <asp:Label ID="Lbl_Error_Cuota_Fija" runat="server" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        <tr>
                            <td style="text-align: left; font-size: 14px; border-bottom: 1px solid #3366CC;"
                                colspan="4">
                                Detalles Cuota fija
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                El solicitante es:
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Solicitante" runat="server" Width="99%" TabIndex="7" AutoPostBack="true"
                                    OnSelectedIndexChanged="Cmb_Solicitante_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                o inmueble financiado:
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Financiado" runat="server" Width="99%" TabIndex="7" AutoPostBack="true"
                                    OnSelectedIndexChanged="Cmb_Financiado_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;" class="style1">
                                Plazo financiamiento
                            </td>
                            <td style="text-align: left;" class="style2">
                                <asp:TextBox ID="Txt_Plazo" runat="server" Width="96.4%" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="Txt_Plazo"
                                    FilterType="Numbers" />
                            </td>
                            <td class="style3">
                                Dif. de construcción
                            </td>
                            <td class="style3">
                                <asp:TextBox ID="Txt_Dif_Construccion" runat="server" AutoPostBack="true" onBlur="this.value=formatCurrency(this.value);Copiar_Diferencia();"
                                    OnTextChanged="Txt_Exedente_Construccion_TextChanged" Text="0" Width="96.4%" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" FilterType="Numbers,Custom"
                                    TargetControlID="Txt_Dif_Construccion" ValidChars="." />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Cuota mínima Anual
                            </td>
                            <td style="text-align: left; width: 20%;">
                                <asp:TextBox ID="Txt_Cuota_Minima" runat="server" Enabled="false" Width="96.4%" />
                                <asp:ImageButton ID="Btn_Busqueda_Cuota_Minima" runat="server" Height="22px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                    OnClick="Btn_Busqueda_Cuota_Minima_Click" TabIndex="10" ToolTip="Búsqueda Avanzada"
                                    Width="22px" />
                            </td>                            
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Cuota Minima a Aplicar
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Cuota_Minima_Aplicar" runat="server" Enabled="false" Width="30.4%" style="float:left" />
                                <asp:CheckBox ID="Chk_Beneficio_Completo" runat="server" Text="Completa" 
                                    style="float:right" AutoPostBack="true" OnCheckedChanged="Chk_Beneficio_Completo_CheckedChanged"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Excedente de construcción
                            </td>
                            <td style="text-align: left; width: 20%;" colspan="2">
                                <asp:TextBox ID="Txt_Exedente_Construccion" runat="server" TabIndex="10" MaxLength="10"
                                    TextMode="SingleLine" Width="70px" Enabled="false" OnTextChanged="Txt_Exedente_Construccion_TextChanged"
                                    AutoPostBack="true" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="Txt_Exedente_Construccion"
                                    FilterType="Numbers,Custom" ValidChars="." />
                                x
                                <asp:TextBox ID="Txt_Tasa_Exedente_Construccion" runat="server" TabIndex="10" MaxLength="10"
                                    TextMode="SingleLine" Width="50px" AutoPostBack="true" OnTextChanged="Txt_Tasa_Exedente_Construccion_TextChanged"
                                    Enabled="false" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="Txt_Tasa_Exedente_Construccion"
                                    WatermarkText="tasa" WatermarkCssClass="watermarked" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="Txt_Tasa_Exedente_Construccion"
                                    FilterType="Numbers,Custom" ValidChars="." />
                                =
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Excedente_Construccion_Total" runat="server" ReadOnly="True"
                                    Width="96.4%" AutoPostBack="true" OnTextChanged="Txt_Excedente_Construccion_Total_TextChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Excedente de valor
                            </td>
                            <td style="text-align: left; width: 20%;" colspan="2">
                                <asp:TextBox ID="Txt_Excedente_Valor" runat="server" TabIndex="10" MaxLength="10"
                                    TextMode="SingleLine" Width="70px" Enabled="false" />x
                                <asp:TextBox ID="Txt_Tasa_Excedente_Valor" runat="server" TabIndex="10" MaxLength="10"
                                    TextMode="SingleLine" Width="50px" Enabled="false" OnTextChanged="Txt_Tasa_Excedente_Valor_TextChanged" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="Txt_Tasa_Excedente_Valor"
                                    WatermarkText="tasa" WatermarkCssClass="watermarked" />
                                =
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Tasa_Valor_Total" runat="server" ReadOnly="True" Width="96.4%"
                                    AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:ImageButton ID="Btn_Calcular_Cuota_Fija" runat="server" ImageUrl="~/paginas/imagenes/paginas/SIAS_Calc3.gif"
                                    OnClick="Btn_Calcular_Cuota_Fija_Click" Style="float: left" TabIndex="8" ToolTip="Calcular Cuotas / Bajas Adeudos" />
                                Total cuota fija
                            </td>
                            <td style="text-align: left; width: 30%; text-align: right;">
                                <asp:TextBox ID="Txt_Total_Cuota_Fija" runat="server" ReadOnly="true" Enabled="false"
                                    Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                Fundamento legal
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%;">
                                <asp:TextBox ID="Txt_Fundamento" runat="server" TabIndex="10" MaxLength="250" TextMode="SingleLine"
                                    Width="98.6%" Enabled="false" AutoPostBack="True" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="Div_Datos_Defuncion" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td style="text-align: left; width: 20%">
                                <asp:Label ID="Lbl_Defuncion" runat="server" Text="Fecha de Defunción"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%">
                                <asp:TextBox ID="Txt_Fecha_Def" runat="server" Width="84%" TabIndex="12" MaxLength="11"
                                    Height="18px" AutoPostBack="true" ToolTip="Dia/Mes/Año" OnTextChanged="Txt_Fecha_Def_TextChanged" />
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="Txt_Fecha_Def"
                                    Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_CE_Fecha_Defuncion" />
                                <asp:ImageButton ID="Btn_CE_Fecha_Defuncion" runat="server" ImageUrl="../imagenes/paginas/SmallCalendar.gif"
                                    Style="vertical-align: top;" Height="18px" CausesValidation="false" OnClick="Btn_CE_Fecha_Defuncion_Click" />
                            </td>
                            <td style="text-align: left; width: 20%">
                            </td>
                            <td style="text-align: left; width: 30%">
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="Pnl_Copropietarios" runat="server">
                    <%------------------ Copropietarios ------------------%>
                    <table width="98%" class="estilo_fuente">
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Copropietarios
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Copropietario
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Co_Propietario" runat="server" Width="96.4%" />
                            </td>
                            <td colspan="2">
                                <asp:ImageButton ID="Btn_Busqueda_Co_Propietarios" runat="server" ToolTip="Búsqueda Avanzada"
                                    TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px"
                                    Width="22px" Style="float: left" OnClick="Btn_Busqueda_Co_Propietarios_Click" />
                                <asp:ImageButton ID="Btn_Agregar_Co_Propietarios" runat="server" ToolTip="Agregar"
                                    TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/sias_add.png" Height="22px"
                                    Width="22px" Style="float: left" OnClick="Btn_Agregar_Co_Propietarios_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView runat="server" AllowPaging="True" AutoGenerateColumns="False" CssClass="GridView_1"
                                    Width="100%" ID="Grid_Copropietarios" PageSize="5" HeaderStyle-CssClass="tblHead"
                                    OnPageIndexChanging="Grid_Copropietarios_PageIndexChanging" OnSelectedIndexChanged="Grid_Copropietarios_SelectedIndexChanged"
                                    DataKeyNames="CONTRIBUYENTE_ID">
                                    <AlternatingRowStyle CssClass="GridAltItem"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:BoundField DataField="CONTRIBUYENTE_ID" HeaderText="Contribuyente_ID" SortExpression="CONTRIBUYENTE_ID"
                                            Visible="false">
                                            <HeaderStyle Width="25%" HorizontalAlign="Left" />
                                            <ItemStyle Width="25%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_CONTRIBUYENTE" HeaderText="Nombre" SortExpression="NOMBRE">
                                            <HeaderStyle Width="75%" HorizontalAlign="Left" />
                                            <ItemStyle Width="75%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RFC" HeaderText="RFC" SortExpression="RFC" HtmlEncode="False">
                                            <HeaderStyle Width="75%" HorizontalAlign="Left" />
                                            <ItemStyle Width="75%" />
                                        </asp:BoundField>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/paginas/delete.png">
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:ButtonField>
                                    </Columns>
                                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                    <PagerStyle CssClass="GridHeader"></PagerStyle>
                                    <RowStyle CssClass="GridItem"></RowStyle>
                                    <SelectedRowStyle CssClass="GridSelected"></SelectedRowStyle>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
                <%------------------ Diferencias ------------------%>
                <div id="Div_Diferencias" style="background-color: #ffffff; width: 100%; height: 100%;">
                    <table width="98%" class="estilo_fuente">
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Diferencias
                            </td>
                        </tr>
                        <asp:Label ID="Lbl_Mensaje_Error_Diferencias" runat="server" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        <tr>
                            <td colspan="4" style="text-align: right; width: 100%;">
                                <asp:ImageButton ID="Btn_Mostrar_Tasas_Diferencias" runat="server" Height="22px"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" OnClick="Btn_Mostrar_Tasas_Diferencias_Click"
                                    TabIndex="10" ToolTip="Limpiar Análisis de Rezago" Width="22px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Periodo corriente
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%; vertical-align: middle;">
                                <asp:DropDownList ID="Cmb_P_C_Bimestre_Inicial" runat="server">
                                </asp:DropDownList>
                                -
                                <asp:DropDownList ID="Cmb_P_C_Bimestre_Final" runat="server">
                                </asp:DropDownList>
                                /
                                <asp:DropDownList ID="Cmb_P_C_Anio" runat="server">
                                </asp:DropDownList>
                                <asp:ImageButton ID="Btn_Agregar_P_Corriente" runat="server" Height="22px" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                    TabIndex="10" ToolTip="Agregar" Width="22px" OnClick="Btn_Agregar_P_Corriente_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Periodo rezago
                            </td>
                            <td style="text-align: left; width: 80%;" colspan="3">
                                <asp:DropDownList ID="Cmb_P_R_Bimestre_Inicial" runat="server">
                                </asp:DropDownList>
                                /
                                <asp:DropDownList ID="Cmb_P_R_Anio_Inicial" runat="server">
                                </asp:DropDownList>
                                -
                                <asp:DropDownList ID="Cmb_P_R_Bimestre_Final" runat="server">
                                </asp:DropDownList>
                                /
                                <asp:DropDownList ID="Cmb_P_R_Anio_Final" runat="server">
                                </asp:DropDownList>
                                <asp:ImageButton ID="Btn_Agregar_P_Regazo" runat="server" ToolTip="Agregar" TabIndex="10"
                                    ImageUrl="~/paginas/imagenes/paginas/sias_add.png" Height="22px" Width="22px"
                                    OnClick="Btn_Agregar_P_Regazo_Click" />
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="4">
                                <div id="Div_Grid_Diferencias" style="background-color: #ffffff; width: 100%;">
                                    <asp:GridView ID="Grid_Diferencias" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                                        Width="100%" HeaderStyle-CssClass="tblHead" Style="white-space: normal;" OnDataBound="Grid_Diferencias_DataBound"
                                        OnRowCommand="Grid_Diferencias_RowCommand" OnRowDataBound="Grid_Diferencias_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="PERIODO" HeaderText="Periodo" SortExpression="PERIODO">
                                                <HeaderStyle Width="15%" HorizontalAlign="Left" />
                                                <ItemStyle Width="15%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Alta/Baja" HeaderStyle-Width="12%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="Cmb_Tipo_Diferencias" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Tipo_Diferencias_SelectedIndexChanged">
                                                        <asp:ListItem Value="ALTA"> ALTA </asp:ListItem>
                                                        <asp:ListItem Value="BAJA"> BAJA </asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor fiscal" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Txt_Grid_Dif_Valor_Fiscal" runat="server" Style="width: 96%" AutoPostBack="true"
                                                        OnTextChanged="Txt_Grid_Dif_Valor_Fiscal_TextChanged" onChage="this.value=formatCurrency(this.value);"
                                                        onBlur="this.value=formatCurrency(this.value);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Grid_Dif_Valor_Fiscal"
                                                        FilterType=" Numbers,Custom" ValidChars=".,$" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="TASA" HeaderText="Tasa" SortExpression="TASA">
                                                <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                                <ItemStyle Width="10%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="..." HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Btn_Tasa_Seleccionar" runat="server" Height="22px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                                        TabIndex="10" ToolTip="Seleccionar Tasa" Width="22px" CommandName="Cmd_Tasa"
                                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IMPORTE" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Txt_Grid_Importe" runat="server" Style="width: 96%" AutoPostBack="true"
                                                        OnTextChanged="Txt_Grid_Importe_TextChanged" onBlur="this.value=formatCurrency(this.value);"
                                                        Text="0.00"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="Ftex_Grid_Importe" runat="server" TargetControlID="Txt_Grid_Importe"
                                                        FilterType=" Numbers,Custom" ValidChars=".,$" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CUOTA BIMESTRAL" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Txt_Grid_Cuota_Bimestral" runat="server" Style="width: 96%" AutoPostBack="true"
                                                        OnTextChanged="Txt_Grid_Importe_TextChanged" onBlur="this.value=formatCurrency(this.value);"
                                                        Text="0.00"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="Ftex_Grid_Cuota_Bim" runat="server" TargetControlID="Txt_Grid_Cuota_Bimestral"
                                                        FilterType=" Numbers,Custom" ValidChars=".,$" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Calcular" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:ImageButton ID="Btn_Calcular" runat="server" Height="22px" ImageUrl="~/paginas/imagenes/paginas/SIAS_Calc2.gif"
                                                            TabIndex="10" ToolTip="Calcular" Width="22px" CommandName="Cmd_Calcular" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <HeaderStyle CssClass="tblHead" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </div>
                                <br />
                            </td>
                        </tr>
                    </table>
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                Vista previa de adeudos
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%;">
                                <asp:ImageButton ID="Btn_Vista_Adeudos" runat="server" ToolTip="Vista Previa de adeudos"
                                    TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Height="22px"
                                    Width="22px" Style="float: left" OnClick="Btn_Vista_Adeudos_Click" />
                            </td>
                        </tr>
                        <%------------------ Detalles cuota fija ------------------%>
                        <tr>
                            <td style="text-align: left; font-size: 14px; border-bottom: 1px solid #3366CC;"
                                colspan="4">
                                Total periodo Corriente
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Desde periodo
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:Label ID="Txt_Desde_Periodo_Corriente" runat="server" Width="36.4%" Enabled="false"
                                    Font-Bold="True" />A&ntilde;o
                                <asp:Label ID="Txt_Desde_Anio_Periodo_Corriente" runat="server" Width="40%" Enabled="false"
                                    Font-Bold="True" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Hasta periodo
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:Label ID="Txt_Hasta_Periodo_Corriente" runat="server" Width="36.4%" Enabled="false"
                                    Font-Bold="True" />A&ntilde;o
                                <asp:Label ID="Txt_Hasta_Anio_Periodo_Corriente" runat="server" Width="40%" Enabled="false"
                                    Font-Bold="True" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Alta
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:Label ID="Txt_Alta_Periodo_Corriente" runat="server" Width="96.4%" Enabled="false"
                                    Font-Bold="True" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Baja
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:Label ID="Txt_Baja_Periodo_Corriente" runat="server" Width="96.4%" Enabled="false"
                                    Font-Bold="True" />
                            </td>
                        </tr>
                        <%------------------ Detalles cuota fija ------------------%>
                        <tr>
                            <td style="text-align: left; font-size: 14px; border-bottom: 1px solid #3366CC;"
                                colspan="4">
                                Total periodo Rezago
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Desde periodo
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:Label ID="Txt_Desde_Periodo_Regazo" runat="server" Width="36.4%" Enabled="false"
                                    Font-Bold="true" />A&ntilde;o
                                <asp:Label ID="Lbl_P_C_Anio_Inicio" runat="server" Enabled="false" Font-Bold="True"
                                    Width="40%" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Hasta periodo
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:Label ID="Txt_Hasta_Periodo_Regazo" runat="server" Width="36.4%" Enabled="false"
                                    Font-Bold="true" />
                                A&ntilde;o
                                <asp:Label ID="Lbl_P_C_Anio_Final" runat="server" Width="40%" Enabled="false" Font-Bold="True" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Alta
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:Label ID="Txt_Alta_Periodo_Regazo" runat="server" Width="96.4%" Enabled="false"
                                    Font-Bold="true" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Baja
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:Label ID="Txt_Baja_Periodo_Regazo" runat="server" Width="96.4%" Enabled="false"
                                    Font-Bold="true" />
                            </td>
                        </tr>
                        <%------------------ Observaciones ------------------%>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Observaciones de la cuenta
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                Observaciones de la cuenta
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%;">
                                <asp:TextBox ID="Txt_Observaciones_Cuenta" runat="server" TabIndex="10" 
                                    Style="text-transform: uppercase" TextMode="MultiLine" Width="98.6%" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="Txt_Comentarios"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="/*-+?¡¿}{_°|!#$%&=Ññ.,:;()áéíóúÁÉÍÓÚ " />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <%------------------ Observaciones ------------------%>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Observaciones de validación
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                Observaciones de validación
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%;">
                                <asp:TextBox ID="Txt_Comentarios" runat="server" TabIndex="10" Style="text-transform: uppercase"
                                    TextMode="MultiLine" Width="98.6%" enabled="false"/>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios" runat="server" TargetControlID="Txt_Comentarios"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="/*-+?¡¿}{_°|!#$%&=Ññ.,:;()áéíóúÁÉÍÓÚ " />
                            </td>
                        </tr>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Historial de Observaciones
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: left; vertical-align: top;">
                                <asp:GridView ID="Grid_Historial_Observaciones" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CssClass="Tabla_Comentarios" HeaderStyle-CssClass="tblHead"
                                    OnPageIndexChanging="Grid_Observaciones_PageIndexChanging" PageSize="5" Style="white-space: normal;"
                                    Width="97%">
                                    <Columns>
                                        <asp:BoundField DataField="OBSERVACIONES_ID" HeaderText="# Observación">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Width="15%" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPCION" HeaderText="Observaciones">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="85%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="USUARIO_CREO" HeaderText="Usuario">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="85%" />
                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
                <asp:HiddenField ID="Hdn_Propietario_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Cuenta_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Cuenta_ID_Temp" runat="server" />
                <asp:HiddenField ID="Hdn_Tasa_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Tasa_Dif" runat="server" />
                <asp:HiddenField ID="Hdn_Cuota_Minima" runat="server" />
                <asp:HiddenField ID="Hdn_Contrarecibo" runat="server" />
                <asp:HiddenField ID="Hdn_Excedente_Valor" runat="server" />
                <asp:HiddenField ID="Tope_Para_Excedente" runat="server" />
                <asp:HiddenField ID="Hdn_Orden_Variacion" runat="server" />
                <asp:HiddenField ID="Hdn_Orden_Variacion_Anio" runat="server" />
                <asp:HiddenField ID="Hdn_Tasa_General_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Colonia_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Calle_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Colonia_ID_Notificacion" runat="server" />
                <asp:HiddenField ID="Hdn_Calle_ID_Notificacion" runat="server" />
                <asp:HiddenField ID="Hdn_Respuesta_Confirmacion" runat="server" />
                <asp:HiddenField ID="Hdn_Superficie_Construccion" runat="server" />
                <asp:HiddenField ID="Hdn_Propietario_Validacion_Superficie" runat="server" />
                <asp:HiddenField ID="Hdn_Propietario_Validacion_Persona" runat="server" />
                <asp:HiddenField ID="Hdn_Cargar_Modulos" runat="server" />
                <asp:HiddenField ID="Hdn_Opcion_Tipo_Orden" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

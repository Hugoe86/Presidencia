<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Ventanilla.master"
    AutoEventWireup="true" CodeFile="Frm_Apl_Login_Ventanilla.aspx.cs" Inherits="paginas_Ventanilla_Frm_Apl_Login_Ventanilla"
    Title="Página sin título" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css" media="screen">
        .borde_difuminado_modulos
        {
            background: url('../imagenes/overlays/fondo_modulos_atencion.png') no-repeat 0 0 transparent;
            height: 90px;
            width: 370px;
            overflow: hidden;
            margin-bottom: 4px;
        }
        .contenedor_botones_modulos
        {
            width: 96%;
            margin: 6px 0 0 3px;
        }
        .contenedor_login_ciudadano
        {
            background: url('../imagenes/overlays/fondo_recuadro_superior_250.png') no-repeat 0 0 transparent;
        }
        .pie_contenedor_login_ciudadano
        {
            background: url('../imagenes/overlays/fondo_recuadro_inferior_250.png') no-repeat 0 100% transparent;
        }
        .pie_contenedor_login_ciudadano > table
        {
            width: 94%;
            margin-left: 8px;
        }
        .panel_boton_agrupado
        {
            margin-left: 12px;
            width: 242px;
            position: relative;
            top: -12px;
        }
        .contenedor_boton_enlace_ciudadano
        {
            background: url('../imagenes/overlays/fondo_recuadro_superior_250.png') no-repeat 0 0 transparent;
            text-align: center;
            height: 53px;
            margin-top: 5px;
        }
        .pie_contenedor_boton_enlace_ciudadano
        {
            background: url('../imagenes/overlays/fondo_recuadro_inferior_250.png') no-repeat 0 100% transparent;
            border: none;
            padding: 0 0 0 20px;
            position: relative;
            top: -4px;
            height: 60px;
            margin: 0;
        }
        input.estilo_boton
        {
            margin-top: 7px;
            border-top: solid 1px lightgray;
            border-left: solid 1px lightgray;
        }
        input.estilo_boton:active
        {
            border-top: solid 2px darkgray;
            border-left: solid 2px darkgray;
        }
    </style>
</asp:Content>
<asp:Content ID="Cph_Area_Trabajo2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
    <asp:ScriptManager ID="ScriptManager_Login" runat="server" />
    
    <link href="../../easyui/themes/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../easyui/themes/icon.css" rel="stylesheet" type="text/css" />

    <script src="../../easyui/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../easyui/jquery.easyui.min.js" type="text/javascript"></script>

    <script src="../../javascript/Js_Cat_Ven_Registro_Usuarios.js" type="text/javascript"></script>

    <div id="tabla" style="position: relative; width: 92%; height: auto; top: 115px;
        margin: auto;">
        <table width="100%">
            <%--style="background-image:url(../imagenes/master/site_map.PNG);"--%>
            <tr valign="top">
                <td style="width: 26%">
                    <asp:Panel ID="Pnl_Siag" runat="server">
                        <%--BorderStyle="Solid" BorderColor="DarkGray" >--%>
                        <table width="100%">
                            <tr valign="top">
                                <td align="center" valign="top">
                                    <asp:Image ID="Img_Siag" runat="server" Width="160px" Height="143px" ImageUrl="~/paginas/imagenes/overlays/escudo.jpg" />
                                </td>
                            </tr>
                            <tr>
                                <td style="font-family: Arial; font-size: 12px; text-align: justify;">
                                    <asp:Label ID="Lbl_Siag" runat="server" Text="Con la finalidad de agilizar los trámites de cualquier índole, evitar largas filas y múltiples procedimientos para realizar solicitudes, tramites, pagos y peticiones a las autoridades, tu Municipio ha creado el portal ciudadano a través del cual podrás realizar todas las operaciones antes mencionadas y dar seguimiento desde tu cuenta, esta cuenta del ciudadano será personalizada y eliminará el papeleo en cada tramite que realices gracias a que se conserva un historial de tus operaciones."></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td style="width: 1%">
                </td>
                <td style="width: 42%">
                    <asp:Panel ID="Pnl_Tramites" runat="server" CssClass="borde_difuminado_modulos">
                        <table class="contenedor_botones_modulos">
                            <tr>
                                <td style="width: 22%" align="center" rowspan="2">
                                    <asp:ImageButton ID="Btn_Tramites" runat="server" Width="82px" Height="60px" ImageUrl="~/paginas/imagenes/paginas/solicitud_tramites_Login.png"
                                        OnClick="Btn_Tramites_Click" />
                                </td>
                                <td style="width: 78%">
                                    <asp:Label ID="Lbl_Titulo_Tramites" runat="server" ForeColor="Blue" Text="TRAMITES"
                                        Style="font-family: Arial; font-size: 14px; width: 100%;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="justify">
                                    <asp:Label ID="Lbl_Tramites" runat="server" Text="Evite largas filas y realice sus trámites a través del portal del ciudadano."
                                        Style="font-family: Arial; font-size: 12px; width: 100%;"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="Pnl_Atencion_Ciudadana" runat="server" CssClass="borde_difuminado_modulos">
                        <table class="contenedor_botones_modulos">
                            <tr>
                                <td style="width: 22%" align="center" rowspan="2">
                                    <asp:ImageButton ID="Btn_Atencion_Ciudadana" runat="server" Width="63px" Height="72px"
                                        ImageUrl="~/paginas/imagenes/paginas/Atencion_Ciudadana_1.png" OnClick="Btn_Atencion_Ciudadana_Click" />
                                </td>
                                <td style="width: 78%" align="justify">
                                    <asp:Label ID="Lbl_Titulo_Atencion_Ciudadana" runat="server" ForeColor="Blue" Text="ATENCION CIUDADANA"
                                        Style="font-family: Arial; font-size: 14px; width: 100%;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-family: Arial; font-size: 12px; line-height: 13px; text-align: justify;
                                    position: relative; top: -2px;">
                                    <asp:Label ID="Lbl_Ayuda_Ciudadana" runat="server" Text="Para atender a las demandas de la ciudadanía de forma eficiente, usted registra su petición y de inmediato es canalizada a la dependencia correspondiente para dar solución."></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="Pnl_Bolsa_Trabajo" runat="server" CssClass="borde_difuminado_modulos">
                        <table class="contenedor_botones_modulos">
                            <tr>
                                <td style="width: 22%" align="center" rowspan="2">
                                    <asp:ImageButton ID="Btn_Listado_Vacantes" runat="server" Width="63px" Height="72px"
                                        ImageUrl="~/paginas/imagenes/paginas/curriculum.png" OnClick="Btn_Listado_Vacantes_Click" />
                                </td>
                                <td style="width: 78%" align="justify">
                                    <asp:Label ID="Lbl_Titulo_Bolsa_Trabajo" runat="server" ForeColor="Blue" Text="BOLSA DE TRABAJO"
                                        Style="font-family: Arial; font-size: 14px; width: 100%;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="justify">
                                    <asp:Label ID="Lbl_Ayuda_Bolsa_Trabajo" runat="server" Text="Un servicio que ofrece tu Municipio para facilitar la vinculación entre las personas que buscan un trabajo y las empresas que ofrecen empleos."
                                        Style="font-family: Arial; font-size: 12px; text-align: justify"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="Pnl_Pagos" runat="server" CssClass="borde_difuminado_modulos">
                        <table class="contenedor_botones_modulos">
                            <tr>
                                <td style="width: 22%" align="center" rowspan="2">
                                    <asp:ImageButton ID="Btn_Pagos" runat="server" Width="63px" Height="72px" 
                                        ImageUrl="~/paginas/imagenes/paginas/tarjetas-credito-debito.png"
                                        OnClick="Btn_Pagos_OnClick" />
                                </td>
                                <td style="width: 78%" align="justify">
                                    <asp:Label ID="Lbl_Titulo_Pagos" runat="server" ForeColor="Blue" Text="PAGOS" Style="font-family: Arial;
                                        font-size: 14px; width: 100%;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="justify">
                                    <asp:Label ID="Lbl_Ayuda_Pagos" runat="server" Text="Ahora puede realizar todos sus pagos de trámites, multas, entre otros, con su tarjeta de crédito."
                                        Style="font-family: Arial; font-size: 12px; text-align: justify"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="Pnl_Notarios" runat="server" CssClass="borde_difuminado_modulos">
                        <table class="contenedor_botones_modulos">
                            <tr>
                                <td style="width: 22%" align="center" rowspan="2">
                                    <asp:ImageButton ID="Btn_Notarios" runat="server" Width="63px" Height="72px" 
                                        ImageUrl="~/paginas/imagenes/paginas/Ventanilla_Tramties.png"
                                        OnClick="Btn_Notarios_OnClick" />
                                </td>
                                <td style="width: 78%" align="justify">
                                    <asp:Label ID="Lbl_Titulo_Notarios" runat="server" ForeColor="Blue" Text="NOTARIOS" Style="font-family: Arial;
                                        font-size: 14px; width: 100%;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="justify">
                                    <asp:Label ID="Lbl_Ayuda_Notarios" runat="server" Text="Acceso para consulta de Cuentas Predial para Notarios."
                                        Style="font-family: Arial; font-size: 12px; text-align: justify"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td style="width: 1%">
                </td>
                <td style="width: 30%">
                    <asp:Panel ID="Pnl_Login" runat="server" CssClass="contenedor_login_ciudadano">
                        <div class="pie_contenedor_login_ciudadano">
                            <table width="97%">
                                <tr>
                                    <td style="width: 25%">
                                    </td>
                                    <td style="width: 25%">
                                    </td>
                                    <td style="width: 25%">
                                    </td>
                                    <td style="width: 25%">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="center">
                                        <asp:Image ID="Img_Login" runat="server" Width="45px" Height="45px" ImageUrl="~/paginas/imagenes/master/sesion.png" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" align="right">
                                        <asp:Label ID="Lbl_Usuario" runat="server" Text="Usuario:" Width="100%" Style="font-family: Arial;
                                            font-size: 11px; height: 100%"></asp:Label>
                                    </td>
                                    <td style="width: 25%" colspan="2">
                                        <asp:TextBox ID="Txt_Usuario" runat="server" Width="85%" TabIndex="1" Style="font-family: Arial;
                                            font-size: 11px;"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Filt_Txt_Usuario" runat="server" TargetControlID="Txt_Usuario"
                                            FilterType="Custom, UppercaseLetters, LowercaseLetters" ValidChars="ÑñáéíóúÁÉÍÓÚ_1234567890@. ">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td rowspan="2">
                                        <asp:ImageButton ID="Btn_Auntentificar_Login" AlternateText="Iniciar sesión..." runat="server"
                                            ImageUrl="~/paginas/imagenes/paginas/Login_Ventanilla.png" TabIndex="3" Width="45px"
                                            Height="45px" OnClick="Btn_Auntentificar_Login_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" align="right">
                                        <asp:Label ID="Llb_Password" runat="server" Text="Password:" Width="100%" Style="font-family: Arial;
                                            font-size: 11px; height: 100%"></asp:Label>
                                    </td>
                                    <td style="width: 25%" colspan="2">
                                        <asp:TextBox ID="Txt_Password" runat="server" Width="85%" TextMode="Password" TabIndex="2"
                                            Style="font-family: Arial; font-size: 11px;"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="Rfv_Txt_Password" runat="server" ErrorMessage="*" ControlToValidate="Txt_Password" >
                                        </asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="left">
                                        <asp:ImageButton ID="Btn_Img_Mensaje" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
                                            Style="visibility: hidden" />
                                        <asp:Label ID="Lbl_Mensaje" runat="server" ForeColor="Red" Style="font-family: Arial;
                                            font-size: 11px;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%" colspan="4" align="center">
                                        <asp:HyperLink ID="Hyp_Link_Olvidar_Password" runat="server" ForeColor="Blue" Text="Olvidaste tu password"
                                            Width="100%" Style="font-family: Arial; font-size: 12px; cursor: hand;" NavigateUrl="javascript:Consultar_Password();"></asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="right">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="Pnl_Registrate" runat="server" GroupingText="¿No tienes cuenta?" BackColor="LightYellow"
                                Style="font-family: Arial; font-size: 15px;" CssClass="panel_boton_agrupado">
                                <table width="100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="Btn_Registrar_Ciudadano" runat="server" BackColor="Yellow" Text="Regístrate"
                                                OnClick="Btn_Registrar_Ciudadano_OnClick" />
                                            <%--  <asp:HyperLink ID="Hyp_Link_Regisrtate" runat="server" ForeColor="Blue" Text="Registrate" Width="100%"
                                            style="font-family:Arial;font-size:14px;cursor:hand;" 
                                            NavigateUrl="javascript:Registro();"></asp:HyperLink>--%>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                    <table width="100%">
                        <tr>
                            <td rowspan="20">
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="20">
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="Pnl_Busqueda_Tramite_Folio" runat="server" CssClass="contenedor_boton_enlace_ciudadano"
                        Style="margin-bottom: 15px;">
                        <fieldset class="pie_contenedor_boton_enlace_ciudadano">
                            <legend style="background-color: White;">Consultar trámite</legend>
                            <asp:Button ID="Btn_Buscar" runat="server" BackColor="LightYellow" Text="Ingresar clave"
                                OnClick="Btn_Buscar_OnClick" CssClass="estilo_boton" />
                        </fieldset>
                    </asp:Panel>
                    <asp:Panel ID="Pnl_Busqueda_Peticion_Folio" runat="server" CssClass="contenedor_boton_enlace_ciudadano"
                        Style="margin-top: 8px;">
                        <fieldset class="pie_contenedor_boton_enlace_ciudadano">
                            <legend style="background-color: White;">Consultar petición</legend>
                            <asp:Button ID="Btn_Buscar_Folio" runat="server" BackColor="LightYellow" Text="Ingresar folio"
                                OnClick="Btn_Buscar_Folio_OnClick" CssClass="estilo_boton" />
                        </fieldset>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        
        <%--   si se olvido de la contraseña --%>
        <div id="Div_Pass" class="easyui-window" closed="true" modal="true" title="Olvidaste tu Password"
            resizable="false" minimizable="false" collapsible="false" maximizable="false"
            closable="true" style="width: 600px; height: 280px;">
            <center>
                <asp:Panel ID="Pnl_Datos_Pass" runat="server" GroupingText="" Width="99%" Style="font-size: 8pt;
                    color: Navy; font-weight: bold;">
                    <div id="Div_Mensaje_Error_Pass" style="width: 98%;">
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:ImageButton ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
                                        Width="24px" Height="24px" />
                                    <asp:Label ID="Lbl_Encabezado_Error_Pass" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%;">
                                </td>
                                <td style="width: 90%; text-align: left;" valign="top">
                                    <asp:Label ID="Lbl_Mensaje_Error_Pass" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table style="width: 100%; text-align: center;">
                        <tr>
                            <td style="width: 100%; text-align: left; cursor: default" colspan="2" class="button_autorizar">
                                <asp:Label ID="Label1" Width="100%" runat="server" Style="width: 100%; font-size: 8pt;
                                    color: Navy; font-weight: bold; text-align: justify;" Text="Si olvidaste tu Password sólo introduce esta información y nosotros te haremos llegar tus datos."></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 22%; cursor: default" class="button_autorizar">
                                <asp:Label ID="Lbl_Email_Pass" runat="server" Text="* Email" Style="font-size: 8pt;
                                    color: Navy; font-weight: bold;"></asp:Label>
                            </td>
                            <td style="width: 78%; text-align: left; cursor: default" class="button_autorizar">
                                <asp:TextBox ID="Txt_Email_Pass" runat="server" Width="100%" MaxLength="100" Text=""
                                    onblur="this.value = (this.value.match(/^[A-Za-z]{1}([-\.]?\w)+@([A-Za-z]{1}[A-Za-z0-9_\-]{1,63})(\.[A-Za-z]{2,4}){1}((\.[A-Za-z]{2}){1})?$/))? this.value : '';" />
                            </td>
                        </tr>
                    </table>
                    <div id="Div_Pregunta_Secreta" runat="server" style="display: block">
                        <table style="width: 100%; text-align: center;">
                            <tr>
                                <td style="text-align: left; width: 22%; cursor: default" class="button_autorizar">
                                    <asp:Label ID="Label2" runat="server" Text="* Pregunta" Style="font-size: 8pt; color: Navy;
                                        font-weight: bold;"></asp:Label>
                                </td>
                                <td style="width: 78%; text-align: left; cursor: default" class="button_autorizar">
                                    <asp:DropDownList ID="Cmb_Pregunta_Email_Pass" runat="server" Width="99%" Enabled="true">
                                        <asp:ListItem> - Selecciona - </asp:ListItem>
                                        <asp:ListItem>Lugar de nacimiento de la madre</asp:ListItem>
                                        <asp:ListItem>Mejor amigo</asp:ListItem>
                                        <asp:ListItem>Nombre de tu primera mascota</asp:ListItem>
                                        <asp:ListItem>Libro favorito</asp:ListItem>
                                        <asp:ListItem>Equipo favorito</asp:ListItem>
                                        <asp:ListItem>Nombre de la primaria a la que asististe</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 22%; cursor: default" class="button_autorizar">
                                    <asp:Label ID="Lbl_Respuesta_Secreta_Pass" runat="server" Text="* Respuesta" Style="font-size: 8pt;
                                        color: Navy; font-weight: bold;"></asp:Label>
                                </td>
                                <td style="width: 78%; text-align: left; cursor: default" class="button_autorizar">
                                    <asp:TextBox ID="Txt_Respuesta_Email_Pass" runat="server" Width="99%" MaxLength="100" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <div id="Div_Consultar_Pass_Boton" runat="server" style="display: block">
                    <table style="width: 100%; text-align: center;">
                        <tr>
                            <td style="text-align: center; width: 99%; height: 0.2em;">
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; width: 99%;">
                                <asp:Button ID="Btn_Consultar_Pass" runat="server" CssClass="button" Text="Enviar" ToolTip="Enviar" />
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
    </div>
</asp:Content>

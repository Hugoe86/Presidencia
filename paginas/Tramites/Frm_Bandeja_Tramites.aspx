<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Bandeja_Tramites.aspx.cs" Inherits="paginas_Tramites_Ope_Bandeja_Tramites"
    Title="Bandeja de Tramites" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        fieldset
        {
            border: 1px solid #A4A4A4;
        }
        /*this is the border color*/legend
        {
            color: Black;
            font-weight: bold;
        }
        /* this is the GroupingText color */</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
    <!--SCRIPT PARA LA VALIDACION QUE NO EXPERE LA SESSION-->

    <script language="javascript" type="text/javascript">
    <!--
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_Session.ashx";
 
        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion() {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }
 
        //Temporizador para matener la sesión activa
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);
        
    //-->
    </script>

    <script type="text/javascript" language="javascript">
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }

        function Abrir_Resumen(Url, Propiedades) {
            window.open(Url, 'Resumen_Predio', Propiedades);
        }    
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server" AsyncPostBackTimeout="3600" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../imagenes/paginas/Updating.gif" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color: #ffffff; width: 100%; height: 100%;">
                <center>
                    <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                        <tr align="center">
                            <td class="label_titulo">
                                Bandeja de Trámites
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td colspan="2" align="left" style="font-size: 10px; width: 90%; text-align: left;"
                                                valign="top" class="estilo_fuente_mensaje_error">
                                                <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
                                                    Width="24px" Height="24px" />
                                                <%--<asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />--%>
                                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%;">
                                            </td>
                                            <td style="font-size: 10px; width: 90%; text-align: left;" valign="top" class="estilo_fuente_mensaje_error">
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div align="right" style="width: 99%; background-color: #2F4E7D; color: #FFFFFF;
                        font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy;
                        height: 32px">
                        <table style="width: 100%; height: 28px;">
                            <tr>
                                <td align="left">
                                    <asp:ImageButton ID="Btn_Evaluar" runat="server" OnClick="Btn_Evaluar_Click" ImageUrl="~/paginas/imagenes/paginas/save_accept.png"
                                        ToolTip="Evaluar" Width="24px" Height="24px" />
                                    <asp:ImageButton ID="Btn_Reporte_Orden_Pago" runat="server" OnClick="Btn_Reporte_Orden_Pago_Click"
                                        ToolTip="Pre visualizar orden de pago" Width="24px" Height="24px" Visible="false"
                                        ImageUrl="~/paginas/imagenes/paginas/sias_revisarplan.png" />
                                    <asp:ImageButton ID="Btn_Cancelar_Solicitud" runat="server" OnClick="Btn_Cancelar_Solicitud_Click"
                                        ToolTip="Cancelar" Width="24px" Height="24px" Visible="false" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" />
                                    <asp:ImageButton ID="Btn_Copiar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_copiar.jpeg"
                                        Width="24px" Height="24px" OnClientClick="return confirm('¿Esta seguro que desea generar una nueva solicitud a partir de esta?');"
                                        OnClick="Btn_Copiar_Click" Visible="false" />
                                    &nbsp;
                                </td>
                                <td align="right">
                                    Búsqueda por Estatus
                                    <asp:DropDownList ID="Cmb_Buscar_Solicitudes_Estatus" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="Cmb_Buscar_Solicitudes_Estatus_SelectedIndexChanged">
                                        <asp:ListItem Value="PENDIENTE_PROCESO" Text="PENDIENTES Y PROCESO" />
                                        <asp:ListItem Value="PENDIENTE" Text="PENDIENTES" />
                                        <asp:ListItem Value="PROCESO" Text="PROCESO" />
                                        <asp:ListItem Value="DETENIDO" Text="DETENIDOS" />
                                        <asp:ListItem Value="TERMINADO" Text="TERMINADOS" />
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="Btn_Buscar_Solicitudes_Estatus" runat="server" CausesValidation="false"
                                        ImageUrl="~/paginas/imagenes/paginas/Sias_Actualizar.png" ToolTip="Actualizar"
                                        Height="22px" Width="22px" OnClick="Btn_Buscar_Solicitudes_Estatus_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </div>
            <asp:Panel ID="Pnl_Filtro_Fechas" runat="server" GroupingText="" Style="width: 98%;
                display: none">
                <table style="width: 100%; height: 28px;">
                    <tr>
                        <td style="width: 100%" align="right">
                            <asp:CheckBox ID="Chk_Fechas" runat="server" Text="Filtro: Fecha de " />
                            <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="15%" Enabled="false"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fecha_Inicio" runat="server" TargetControlID="Txt_Fecha_Inicio"
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_" />
                            <cc1:CalendarExtender ID="Txt_Fecha_Inicio_CalendarExtender" runat="server" TargetControlID="Txt_Fecha_Inicio"
                                PopupButtonID="Btn_Fecha_Inicio" Format="dd/MMM/yyyy" />
                            <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            <asp:Label ID="Lbl_De" runat="server" Text=" A "></asp:Label>
                            <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="15%" Enabled="false"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Fecha_Fin" runat="server" TargetControlID="Txt_Fecha_Fin"
                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_" />
                            <cc1:CalendarExtender ID="Txt_Fecha_Fin_CalendarExtender1" runat="server" TargetControlID="Txt_Fecha_Fin"
                                PopupButtonID="Btn_Fecha_Fin" Format="dd/MMM/yyyy" />
                            <asp:ImageButton ID="Btn_Fecha_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <%-- <div id="Div_Dependencias" runat="server" style="display: none">
                    <table width="98%">
                        <tr>
                            <td style="width: 15%; text-align: left;">
                                <asp:Label ID="Lbl_Dependencia" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 85%" align="left">
                                <asp:DropDownList ID="Cmb_Dependencias" runat="server" Width="85%" DropDownStyle="DropDownList"
                                    AutoCompleteMode="SuggestAppend" CssClass="WindowsStyle" MaxLength="0" />
                                <asp:ImageButton ID="Btn_Buscar_Dependencia" runat="server" ToolTip="Seleccionar Unidad responsable"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                    OnClick="Btn_Buscar_Dependencia_Click" />
                            </td>
                        </tr>
                    </table>
                </div>--%>
            <div id="Div_Detalle_Bandeja_Tramite" runat="server" style="display: none; margin-top: 10px;">
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="98%" ActiveTabIndex="0">
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1" Width="96%">
                        <HeaderTemplate>
                            Bandeja de Trámites</HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td style="width: 15%">
                                        <asp:HiddenField ID="Hdf_Solicitud_ID" runat="server" />
                                        <asp:HiddenField ID="Hdf_Subproceso_ID" runat="server" />
                                        <asp:HiddenField ID="HDN_Solicitud_ID" runat="server" />
                                        <asp:HiddenField ID="Hdf_Condicion_Si" runat="server" />
                                        <asp:HiddenField ID="Hdf_Condicion_No" runat="server" />
                                        <asp:HiddenField ID="Hdf_Tramite_Id" runat="server" />
                                        <asp:HiddenField ID="Hdf_Costo_Total" runat="server" />
                                        <asp:HiddenField ID="Hdf_Habilitar_Evaluar" runat="server" />
                                        <asp:HiddenField ID="Hdf_Contribuyente_Id" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <table width="98%">
                                <tr>
                                    <td style="width: 16%">
                                        <asp:Label ID="Lbl_Clave_Solicitud" runat="server" Text="Clave Solicitud"></asp:Label>
                                    </td>
                                    <td style="width: 34%;">
                                        <asp:TextBox ID="Txt_Clave_Solicitud" runat="server" Width="84%" Enabled="False"
                                            MaxLength="20"></asp:TextBox>
                                    </td>
                                    <td style="width: 16%" align="left">
                                        <asp:Label ID="Lbl_Consecutivo" runat="server" Text="Consecutivo"></asp:Label>
                                    </td>
                                    <td style="width: 34%;">
                                        <asp:TextBox ID="Txt_Consecutivo" runat="server" Width="96%" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 16%">
                                        <asp:Label ID="Lbl_Porcentaje_Avance" runat="server" Text="% Fase"></asp:Label>
                                    </td>
                                    <td style="width: 34%">
                                        <asp:TextBox ID="Txt_Porcentaje_Actual_Proceso" runat="server" Width="84%" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td style="width: 16%" align="left">
                                        <asp:Label ID="Lbl_Porcentaje_Acumulado" runat="server" Text="% Acumulado"></asp:Label>
                                    </td>
                                    <td style="width: 34%">
                                        <asp:TextBox ID="Txt_Porcentaje_Avance" runat="server" Width="96%" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table width="98%">
                                <tr>
                                    <td style="width: 16%">
                                        <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus"></asp:Label>
                                    </td>
                                    <td style="width: 34%">
                                        <asp:TextBox ID="Txt_Estatus" runat="server" Width="85%" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td style="width: 16%" align="left">
                                        <asp:Label ID="Lbl_Fecha_Solicitud" runat="server" Text="Fecha Solicitud"></asp:Label>
                                    </td>
                                    <td style="width: 34%">
                                        <asp:TextBox ID="Txt_Fecha_Solicitud" runat="server" Width="98%" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 16%">
                                        <asp:Label ID="Lbl_Nombre_Tramite" runat="server" Text="Trámite"></asp:Label>
                                    </td>
                                    <td style="width: 34%">
                                        <asp:TextBox ID="Txt_Nombre_Tramite" runat="server" Width="85%" Enabled="False" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td style="width: 16%" align="left">
                                        <asp:Label ID="Lbl_Costo_Total" runat="server" Text="Costo unitario"></asp:Label>
                                    </td>
                                    <td style="width: 34%">
                                        <asp:TextBox ID="Txt_Costo_Total" runat="server" Width="98%" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table width="98%">
                                <tr>
                                    <td style="width: 16%">
                                        <asp:Label ID="Lbl_Solicito" runat="server" Text="Solicitó"></asp:Label>
                                    </td>
                                    <td style="width: 84%">
                                        <asp:TextBox ID="Txt_Solicito" runat="server" Width="99%" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table id="Tbl_Fechas_Vigencia" runat="server" visible="False" width="98%">
                                <tr runat="server">
                                    <td style="width: 100%" runat="server">
                                        <asp:Panel ID="Pnl_Vigencia" runat="server" GroupingText="Vigencia">
                                            <table width="98%">
                                                <tr>
                                                    <td style="width: 16%">
                                                        <asp:Label ID="Lbl_Fecha_Vigencia_Inicio" runat="server" Text="Inicio"></asp:Label>
                                                    </td>
                                                    <td style="width: 33%">
                                                        <asp:TextBox ID="Txt_Fecha_Vigencia_Inicio" runat="server" Width="75%" Enabled="False"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fecha_Vigencia_Inicio" runat="server" TargetControlID="Txt_Fecha_Vigencia_Inicio"
                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="/_"
                                                            Enabled="True" />
                                                        <cc1:CalendarExtender ID="Calendar_Txt_Fecha_Vigencia_Inicio" runat="server" TargetControlID="Txt_Fecha_Vigencia_Inicio"
                                                            PopupButtonID="Btn_Txt_Fecha_Vigencia_Inicio" Format="dd/MMM/yyyy" Enabled="True" />
                                                        <asp:ImageButton ID="Btn_Txt_Fecha_Vigencia_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                                    </td>
                                                    <td style="width: 18%">
                                                        <asp:Label ID="Lbl_Fecha_Vigencia_Fin" runat="server" Text="Fin"></asp:Label>
                                                    </td>
                                                    <td style="width: 33%">
                                                        <asp:TextBox ID="Txt_Fecha_Vigencia_Fin" runat="server" Width="75%" Enabled="False"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fecha_Vigencia_Fin" runat="server" TargetControlID="Txt_Fecha_Vigencia_Fin"
                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="/_"
                                                            Enabled="True" />
                                                        <cc1:CalendarExtender ID="Calendar_Txt_Fecha_Vigencia_Fin" runat="server" TargetControlID="Txt_Fecha_Vigencia_Fin"
                                                            PopupButtonID="Btn_Txt_Fecha_Vigencia_Fin" Format="dd/MMM/yyyy" Enabled="True" />
                                                        <asp:ImageButton ID="Btn_Txt_Fecha_Vigencia_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 16%">
                                                        <asp:Label ID="Lbl_Documento_Vigencia_Inicio" runat="server" Text="Documento inicio"></asp:Label>
                                                    </td>
                                                    <td style="width: 33%">
                                                        <asp:TextBox ID="Txt_Fecha_Doc_Inicio" runat="server" Width="75%" Enabled="False"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fecha_Doc_Inicio" runat="server" TargetControlID="Txt_Fecha_Doc_Inicio"
                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="/_"
                                                            Enabled="True" />
                                                        <cc1:CalendarExtender ID="Calendar_Txt_Fecha_Doc_Inicio" runat="server" TargetControlID="Txt_Fecha_Doc_Inicio"
                                                            PopupButtonID="Btn_Txt_Fecha_Doc_Inicio" Format="dd/MMM/yyyy" Enabled="True" />
                                                        <asp:ImageButton ID="Btn_Txt_Fecha_Doc_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                                    </td>
                                                    <td style="width: 18%">
                                                        <asp:Label ID="Label2" runat="server" Text="Fin"></asp:Label>
                                                    </td>
                                                    <td style="width: 33%">
                                                        <asp:TextBox ID="Txt_Fecha_Doc_Fin" runat="server" Width="75%" Enabled="False"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Fecha_Doc_Fin" runat="server" TargetControlID="Txt_Fecha_Doc_Fin"
                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="/_"
                                                            Enabled="True" />
                                                        <cc1:CalendarExtender ID="Calendar_Txt_Fecha_Doc_Fin" runat="server" TargetControlID="Txt_Fecha_Doc_Fin"
                                                            PopupButtonID="Btn_Txt_Fecha_Doc_Fin" Format="dd/MMM/yyyy" Enabled="True" />
                                                        <asp:ImageButton ID="Btn_Txt_Fecha_Doc_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <table width="98%">
                                <tr>
                                    <td style="width: 16%">
                                    </td>
                                    <td style="width: 33%">
                                        <asp:LinkButton ID="Btn_Link_Cedula_Visita" runat="server" ForeColor="Blue" Text="Revisar Cedula de visita"
                                            OnClick="Btn_Link_Cedula_Visita_Click"></asp:LinkButton>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:LinkButton ID="Btn_Link_Opiniones" runat="server" ForeColor="Blue" Text="Revisar opinion tecnica"
                                            OnClick="Btn_Link_Opiniones_Click"></asp:LinkButton>
                                    </td>
                                    <td style="width: 33%">
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="Pnl_Evaluar_Solicitud" runat="server" GroupingText="RESPUESTA" Style="display: block;
                                margin-top: 8px; background: #E6E6E6;">
                                <table width="98%">
                                    <tr>
                                        <td style="width: 16%;">
                                            <asp:Label ID="Lbl_Subproceso" runat="server" Text="Actividad Actual"></asp:Label>
                                        </td>
                                        <td style="width: 84%">
                                            <asp:TextBox ID="Txt_Subproceso" runat="server" Width="99%" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Panel ID="Tabla_Condicion" runat="server" Style="display: block">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 16%" align="left">
                                                            <asp:Label ID="Lbl_Condicion" runat="server"></asp:Label>
                                                        </td>
                                                        <td style="width: 34%">
                                                            <asp:DropDownList ID="Cmb_Condicion" runat="server" Width="85%" AutoPostBack="True"
                                                                OnSelectedIndexChanged="Cmb_Condicion_SelectedIndexChanged">
                                                                <asp:ListItem Text="SI" Value="SI" Selected="True" />
                                                                <asp:ListItem Text="NO" Value="NO" />
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 16%" align="left">
                                                            <asp:Label ID="Lbl_Nombre_Actividad_Condicion" runat="server" Text="Siguiente Actividad"></asp:Label>
                                                        </td>
                                                        <td style="width: 34%">
                                                            <asp:TextBox ID="Txt_Nombre_Actividad_Condicion" runat="server" Width="98%" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 16%" align="left">
                                            <asp:Label ID="Lbl_Evaluacion" runat="server" Text="Estatus de Actividad"></asp:Label>
                                        </td>
                                        <td style="width: 84%">
                                            <asp:DropDownList ID="Cmb_Evaluacion" runat="server" Width="100%">
                                                <asp:ListItem Text="ACEPTAR" Value="APROBAR" Selected="True" />
                                                <asp:ListItem Text="REGRESAR A LA ACTIVIDAD ANTERIOR" Value="REGRESAR" />
                                                <asp:ListItem Text="DETENER" Value="DETENER" />
                                                <asp:ListItem Text="CANCELAR" Value="CANCELAR" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table width="98%">
                                    <tr>
                                        <td style="width: 16%;">
                                            <asp:Label ID="Lbl_Siguiente_Subproceso" runat="server" Text="Siguiente Actividad"></asp:Label>
                                        </td>
                                        <td style="width: 34%">
                                            <asp:TextBox ID="Txt_Siguiente_Subproceso" runat="server" Width="95%" Style="font-size: 12px"
                                                Enabled="False"></asp:TextBox>
                                        </td>
                                        <td style="width: 16%;">
                                            <asp:Label ID="Lbl_Perfil" runat="server" Text="Responsable"></asp:Label>
                                        </td>
                                        <td style="width: 34%">
                                            <asp:TextBox ID="Txt_Perfil" runat="server" Width="98%" Enabled="False" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="Pnl_Contenedor_Zona" runat="server" Style="display: none">
                                    <table width="98%">
                                        <tr>
                                            <td style="width: 16%" align="left">
                                                <asp:Label ID="Lbl_Cmb_Zonas" runat="server" Text="Zona" Font-Bold="False"></asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:DropDownList ID="Cmb_Zonas" runat="server" Width="100%" AutoPostBack="True"
                                                    OnSelectedIndexChanged="Cmb_Zonas_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 4%" align="right">
                                                <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                                    Style="width: 24px; height: 24px" AlternateText="Modificar" OnClick="Btn_Modificar_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="98%">
                                        <tr>
                                            <td style="width: 16%" align="left">
                                                <asp:Label ID="Lbl_Cmb_Supervisor_Zona" runat="server" Width="100%" Text="Coordinador"
                                                    Font-Bold="False"></asp:Label>
                                            </td>
                                            <td style="width: 84%">
                                                <asp:DropDownList ID="Cmb_Supervisor_Zona" runat="server" Width="100%">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="98%">
                                        <tr>
                                            <td style="width: 16%" align="left">
                                                <asp:Label ID="Lbl_Inspector" runat="server" Width="100%" Text="Inspector" Font-Bold="False"></asp:Label>
                                            </td>
                                            <td style="width: 84%">
                                                <asp:DropDownList ID="Cmb_Inspector" runat="server" Width="100%">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <table id="Tbl_Ubicacion_Expediente" runat="server" visible="False" width="98%">
                                    <tr runat="server">
                                        <td style="width: 16%" align="left" runat="server">
                                            <asp:Label ID="Lbl_Ubicacion_Expediente" runat="server" Width="100%" Text="Ubicacion del expediente"></asp:Label>
                                        </td>
                                        <td style="width: 84%" runat="server">
                                            <asp:TextBox ID="Txt_Ubicacion_Expediente" runat="server" Width="99%" MaxLength="200"
                                                TextMode="MultiLine" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table width="98%">
                                    <tr>
                                        <td style="width: 16%" align="left">
                                            <asp:Label ID="Lbl_Comentarios_Internos" runat="server" Width="100%" Text="Comentario Interno"></asp:Label>
                                        </td>
                                        <td style="width: 84%">
                                            <asp:TextBox ID="Txt_Comentarios_Internos" runat="server" Width="99%" MaxLength="200"
                                                TextMode="MultiLine" Enabled="False" Rows="2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 16%" align="left">
                                            <asp:Label ID="Lbl_Comentarios_Evaluacion" runat="server" Width="100%" Text="Comentario al Ciudadano"></asp:Label>
                                        </td>
                                        <td style="width: 84%">
                                            <asp:TextBox ID="Txt_Comentarios_Evaluacion" runat="server" Width="99%" MaxLength="200"
                                                TextMode="MultiLine" Enabled="False" Rows="2"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TbP_Datos_Cuenta_Predial" ID="TabPanel4"
                        Width="96%">
                        <HeaderTemplate>
                            Datos adicionales
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server" GroupingText="" Visible="true" Style="margin-top: 8px;">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 15%">
                                            <asp:Label ID="Lbl_Cuenta_Predial" runat="server" Text="Cuenta Predial"></asp:Label>
                                            <asp:HiddenField ID="Hdf_Cuenta_Predial" runat="server" />
                                            <asp:HiddenField ID="Hdf_Dependencia_ID" runat="server" />
                                        </td>
                                        <td style="width: 35%">
                                            <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="83%" Enabled="False" AutoPostBack="True"
                                                MaxLength="20"></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Buscar_Cuenta_Predial" runat="server" ToolTip="Resumen de predio"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Height="22px" Width="22px" />
                                        </td>
                                        <td style="width: 15%">
                                            Propietario
                                        </td>
                                        <td style="width: 35%">
                                            <asp:TextBox ID="Txt_Propietario_Cuenta_Predial" runat="server" Width="84%" Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%">
                                            Colonia
                                        </td>
                                        <td style="width: 35%">
                                            <asp:TextBox ID="Txt_Direccion_Predio" runat="server" Width="84%" Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                        <td style="width: 15%">
                                            Calle
                                        </td>
                                        <td style="width: 35%">
                                            <asp:TextBox ID="Txt_Calle_Predio" runat="server" Width="84%" Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%">
                                            Numero
                                        </td>
                                        <td style="width: 35%">
                                            <asp:TextBox ID="Txt_Numero_Predio" runat="server" Width="84%" Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                        <td style="width: 15%">
                                            Manzana
                                        </td>
                                        <td style="width: 35%">
                                            <asp:TextBox ID="Txt_Manzana_Predio" runat="server" Width="84%" Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%">
                                            Lote
                                        </td>
                                        <td style="width: 35%">
                                            <asp:TextBox ID="Txt_Lote_Predio" runat="server" Width="84%" Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                        <td style="width: 15%">
                                            <asp:Label ID="Lbl_Perito" runat="server" Width="100%" Text="Perito" Font-Bold="false"></asp:Label>
                                        </td>
                                        <td style="width: 35%">
                                            <asp:DropDownList ID="Cmb_Perito" runat="server" Width="87%" Enabled="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%">
                                            Otros
                                        </td>
                                        <td style="width: 35%" colspan="3">
                                            <asp:TextBox ID="Txt_Otros_Predio" runat="server" Width="94%" Enabled="false" MaxLength="1000"
                                                TextMode="MultiLine" Rows="5">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel2" ID="TabPanel2" Width="96%">
                        <HeaderTemplate>
                            Información Requerida
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="Pnl_Datos_Iniciales" runat="server" GroupingText="Datos Solicitados">
                                <table style="width: 100%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                    font-style: normal; font-variant: normal; font-family: fantasy; height: 25px">
                                    <tr>
                                        <td style="width: 70%;" align="left">
                                            Dato Inicial
                                        </td>
                                        <td style="width: 30%;" align="left">
                                            Valor
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" Height="120px" ScrollBars="Auto">
                                    <asp:GridView ID="Grid_Datos_Tramite" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                                        Width="100%" GridLines="None" EmptyDataText="No hay datos para este Subproceso"
                                        ShowHeader="false" OnRowDataBound="Div_Grid_Datos_Tramite_RowDataBound">
                                        <RowStyle CssClass="GridItem" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <Columns>
                                            <%-- 0 --%>
                                            <asp:BoundField DataField="OPE_DATO_ID" />
                                            <%-- 1 --%>
                                            <asp:BoundField DataField="DATO_ID" />
                                            <%-- 2 --%>
                                            <asp:BoundField DataField="NOMBRE_DATO">
                                                <ItemStyle HorizontalAlign="Left" Font-Size="12px" Width="60%" />
                                            </asp:BoundField>
                                            <%-- 3 --%>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Txt_Valor_Dato" runat="server" Width="98%" ReadOnly="true" TextMode="MultiLine"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="40%" Font-Size="12px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                            <asp:Panel ID="Pnl_Grid_Datos_Finales" runat="server" GroupingText="Datos Dictaminados">
                                <asp:ImageButton ID="Btn_Guardar_Datos_Dictamen" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png"
                                    Width="24px" CssClass="Img_Button" OnClick="Btn_Guardar_Datos_Dictamen_Click"
                                    Visible="false" />
                                <table style="width: 100%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                    font-style: normal; font-variant: normal; font-family: fantasy; height: 25px">
                                    <tr>
                                        <td style="width: 70%;" align="left">
                                            Dato Final
                                        </td>
                                        <td style="width: 30%;" align="left">
                                            Valor
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="Panel3" runat="server" Height="120px" ScrollBars="Auto">
                                    <asp:GridView ID="Grid_Datos_Dictamen" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                        EmptyDataText="No se encuentran datos para dictaminar" CssClass="GridView_1"
                                        ShowHeader="false" OnRowDataBound="Grid_Datos_Dictamen_RowDataBound" GridLines="None"
                                        Width="100%">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:BoundField DataField="Nombre">
                                                <ItemStyle HorizontalAlign="Left" Width="60%" Wrap="True" />
                                            </asp:BoundField>
                                            <%-- 1 --%>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Txt_Descripcion_Datos" runat="server" Width="98%" TextMode="MultiLine"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="40%" Font-Size="12px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                    <asp:GridView ID="Grid_Datos_Dictamen_Modificar" runat="server" CssClass="GridView_1"
                                        AutoGenerateColumns="False" Width="100%" GridLines="None" EmptyDataText="No hay datos"
                                        OnRowDataBound="Grid_Datos_Dictamen_Modificar_RowDataBound" ShowHeader="false">
                                        <RowStyle CssClass="GridItem" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <Columns>
                                            <%-- 0 --%>
                                            <asp:BoundField DataField="OPE_DATO_ID" />
                                            <%-- 1 --%>
                                            <asp:BoundField DataField="DATO_ID" />
                                            <%-- 2 --%>
                                            <asp:BoundField DataField="NOMBRE_DATO">
                                                <ItemStyle HorizontalAlign="Left" Font-Size="12px" Width="60%" />
                                            </asp:BoundField>
                                            <%-- 3 --%>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Txt_Valor_Dato" runat="server" Width="98%" TextMode="MultiLine"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="40%" Font-Size="12px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel3" ID="TabPanel3" Width="96%">
                        <HeaderTemplate>
                            Requisitos
                        </HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <asp:Label ID="Lbl_Documentos_Anexos" runat="server" Text="Documentos Anexados" Width="100%"
                                    CssClass="label_titulo">
                                </asp:Label>
                                <asp:Panel runat="server" GroupingText=" ">
                                    <table style="width: 100%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                        font-style: normal; font-variant: normal; font-family: fantasy; height: 25px">
                                        <tr>
                                            <td style="width: 90%;" align="left">
                                                Documento
                                            </td>
                                            <td style="width: 10%;" align="right">
                                                Ver
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel runat="server" Height="120px" ScrollBars="Auto">
                                        <asp:GridView ID="Grid_Documentos_Tramite" runat="server" CssClass="GridView_1" OnRowDataBound="Grid_Documentos_Tramite_RowDataBound"
                                            ShowHeader="false" GridLines="None" AutoGenerateColumns="False" Width="100%"
                                            EmptyDataText="No hay documentos anexados">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:BoundField DataField="OPE_DOCUMENTO_ID" />
                                                <asp:BoundField DataField="DETALLE_DOCUMENTO_ID" />
                                                <asp:BoundField DataField="NOMBRE_DOCUMENTO">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="URL" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Btn_Ver_Documento" runat="server" AlternateText="Ver" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                            OnClick="Btn_Ver_Documento_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </asp:Panel>
                                <br />
                                <asp:Label ID="Lbl_Documentos_Seguimiento" runat="server" Text="Documentos Creados en el Seguimiento"
                                    Width="100%" CssClass="label_titulo"></asp:Label>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 90%" align="left">
                                            <cc1:AsyncFileUpload ID="AFU_Subir_Archivo" runat="server" Width="100%" Enabled="False"
                                                UploadingBackColor="Yellow" CompleteBackColor="LightGreen" ThrobberID="Throbber"
                                                OnUploadedComplete="AFU_Subir_Archivo_UploadedComplete" FailedValidation="False" />
                                        </td>
                                        <td style="width: 10%" align="left">
                                            <asp:ImageButton ID="Btn_Actualizar_Documentos" CausesValidation="False" runat="server"
                                                Style="cursor: pointer;" ToolTip="Subir archivo" Width="22px" Height="22px" ImageUrl="~/paginas/imagenes/paginas/subir.png"
                                                OnClick="Btn_Actualizar_Documentos_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" GroupingText=" ">
                                    <table style="width: 100%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                        font-style: normal; font-variant: normal; font-family: fantasy; height: 25px">
                                        <tr>
                                            <td style="width: 80%;" align="left">
                                                Documento
                                            </td>
                                            <td style="width: 10%;" align="right">
                                                Ver
                                            </td>
                                            <td style="width: 10%;" align="right">
                                                Eliminar
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel runat="server" Height="120px" ScrollBars="Auto">
                                        <asp:GridView ID="Grid_Documentos_Seguimiento" runat="server" CssClass="GridView_1"
                                            ShowHeader="false" OnRowDataBound="Grid_Documentos_Seguimiento_RowDataBound"
                                            AutoGenerateColumns="False" Width="100%" GridLines="None" EmptyDataText="No hay documentos generados durante el seguimiento">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:BoundField DataField="NOMBRE_DOCUMENTO">
                                                    <ItemStyle Font-Names="12px" HorizontalAlign="Left" Width="90%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="URL" />
                                                <asp:TemplateField HeaderText="Ver">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Btn_Ver_Documento_Seguimiento" runat="server" AlternateText="Ver"
                                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Ver_Documento_Seguimiento_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Btn_Quitar_Documentos" CausesValidation="false" runat="server"
                                                            Style="cursor: pointer;" ToolTip="Eliminar archivo" Width="20px" Height="20px"
                                                            OnClientClick="return confirm('¿Esta seguro que desea quitar el documento?');"
                                                            ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" OnClick="Btn_Quitar_Documentos_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle Font-Names="12px" HorizontalAlign="Center" Width="10%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </asp:Panel>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel3" ID="TabPanel5" Width="96%">
                        <HeaderTemplate>
                            Plantillas y/o Formato
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="Pnl_Detalles_Plantillas" runat="server" GroupingText="Plantillas">
                                <table style="width: 100%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                    font-style: normal; font-variant: normal; font-family: fantasy; height: 25px">
                                    <tr>
                                        <td style="width: 15%;" align="left">
                                            Realizados
                                        </td>
                                        <td style="width: 60%;" align="left">
                                            Plantilla
                                        </td>
                                        <td style="width: 25%;" align="right">
                                            Generar documento
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" Height="120px" ScrollBars="Auto">
                                    <asp:GridView ID="Grid_Plantillas" runat="server" CssClass="GridView_1" OnRowDataBound="Grid_Plantillas_RowDataBound"
                                        ShowHeader="false" AutoGenerateColumns="False" Width="100%" GridLines="None"
                                        EmptyDataText="No hay plantillas para este actividad">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chk_Realizado" runat="server" Enabled="false" />
                                                </ItemTemplate>
                                                <ItemStyle Font-Names="12px" HorizontalAlign="Left" Width="15%" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PLANTILLA_ID">
                                                <ItemStyle Font-Names="12px" HorizontalAlign="Left" Width="0%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NOMBRE">
                                                <ItemStyle Font-Names="12px" HorizontalAlign="Left" Width="80%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ARCHIVO">
                                                <ItemStyle Font-Names="12px" HorizontalAlign="Left" Width="0%" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Btn_Generar_Documento" runat="server" autoposback="true" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                        OnClick="Btn_Generar_Documento_Click" />
                                                </ItemTemplate>
                                                <ItemStyle Font-Names="12px" HorizontalAlign="Right" Width="5%" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                            <asp:Panel ID="Pnl_Detalles_Formatos" runat="server" GroupingText="Formatos">
                                <table style="width: 100%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                    font-style: normal; font-variant: normal; font-family: fantasy; height: 25px">
                                    <tr>
                                        <td style="width: 15%;" align="left">
                                            Realizados
                                        </td>
                                        <td style="width: 60%;" align="left">
                                            Formato
                                        </td>
                                        <td style="width: 25%;" align="right">
                                            Lenar Formato
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" Height="120px" ScrollBars="Auto">
                                    <asp:GridView ID="Grid_Detalles_Formatos" runat="server" CssClass="GridView_1" OnRowDataBound="Grid_Detalles_Formatos_RowDataBound"
                                        ShowHeader="false" AutoGenerateColumns="False" Width="100%" GridLines="None"
                                        EmptyDataText="No hay Formatos para esta actividad">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chk_Realizado" runat="server" Enabled="false" />
                                                </ItemTemplate>
                                                <ItemStyle Font-Names="12px" HorizontalAlign="Left" Width="15%" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="formato_id">
                                                <ItemStyle Width="0%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NOMBRE">
                                                <ItemStyle Font-Names="12px" HorizontalAlign="Left" Width="85%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ARCHIVO">
                                                <ItemStyle Font-Names="12px" HorizontalAlign="Left" Width="0%" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Btn_Llenar_Formato" runat="server" autoposback="true" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                                        OnClick="Btn_Llenar_Formato_Click" Style="width: 24px; height: 24px" />
                                                </ItemTemplate>
                                                <ItemStyle Font-Names="12px" HorizontalAlign="Right" Width="5%" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel3" ID="TabPanel6" Width="100%">
                        <HeaderTemplate>
                            Actividades realizadas
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel runat="server" GroupingText="Comentario Interno">
                                <table style="width: 100%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                    font-style: normal; font-variant: normal; font-family: fantasy; height: 25px">
                                    <tr>
                                        <td style="width: 20%;" align="center">
                                            Actividad
                                        </td>
                                        <td style="width: 30%;" align="center">
                                            Comentarios
                                        </td>
                                        <td style="width: 25%;" align="center">
                                            Usuario
                                        </td>
                                        <td style="width: 25%;" align="center">
                                            Fecha
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="Pnl_Actividades_Internas" runat="server" Height="120px" ScrollBars="Auto">
                                    <asp:GridView ID="Grid_Comentarios_Internos" runat="server" CssClass="GridView_1"
                                        ShowHeader="false" AutoGenerateColumns="False" Width="100%" GridLines="None"
                                        EmptyDataText="No hay comentarios">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:BoundField DataField="Actividad" HeaderText="Actividad" SortExpression="Actividad"
                                                ItemStyle-Font-Size="12px" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Comentarios" HeaderText="Comentarios" SortExpression="Comentarios"
                                                ItemStyle-Font-Size="12px" ItemStyle-Width="35%" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="USUARIO_CREO" HeaderText="Usuario" SortExpression="USUARIO_CREO"
                                                ItemStyle-Font-Size="11px" ItemStyle-Width="29%" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="{0:dd/MMM/yyyy hh:mm:ss tt}"
                                                ItemStyle-Font-Size="11px" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                            <br />
                            <asp:Panel runat="server" GroupingText="Comentario al Ciudadano">
                                <table style="width: 100%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                    font-style: normal; font-variant: normal; font-family: fantasy; height: 25px">
                                    <tr>
                                        <td style="width: 20%;" align="center">
                                            Actividad
                                        </td>
                                        <td style="width: 55%;" align="center">
                                            Comentarios
                                        </td>
                                        <td style="width: 25%;" align="center">
                                            Fecha
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="Pnl_Actividades_Realizadas" runat="server" Height="120px" ScrollBars="Auto">
                                    <asp:GridView ID="Grid_Actividades" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                                        Width="100%" GridLines="None" EmptyDataText="No hay actividades ralizadas" ShowHeader="false">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:BoundField DataField="Actividad" SortExpression="Actividad" ItemStyle-Font-Size="12px"
                                                ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Comentarios" SortExpression="Comentarios" ItemStyle-Font-Size="12px"
                                                ItemStyle-Width="40%" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Fecha" SortExpression="Fecha" DataFormatString="{0:dd/MMM/yyyy hh:mm:ss tt}"
                                                ItemStyle-Font-Size="12px" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel3" ID="TPnl_Agregar_Tramite" Width="96%">
                        <HeaderTemplate>
                            Agregar Tramite
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td style="width: 20%" align="left">
                                        Seleccione un Tramite
                                    </td>
                                    <td style="width: 70%" align="left">
                                        <asp:DropDownList ID="Cmb_Agregar_Solicitud" runat="server" Width="100%" />
                                    </td>
                                    <td style="width: 10%" align="left">
                                        <asp:ImageButton ID="Btn_Agregar_Solicitud" runat="server" Width="20px" Height="20px"
                                            ImageUrl="~/paginas/imagenes/gridview/add_grid.png" AlternateText="Agregar Solicitud"
                                            OnClick="Btn_Agregar_Solicitud_Click" />
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="Grid_Solicitudes_Complemetarias" runat="server" CssClass="GridView_1"
                                AutoGenerateColumns="False" Width="97%" GridLines="None" EmptyDataText="No hay solicitudes relaciondas">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:BoundField DataField="SOLICITUD_ID" HeaderText="SOLICITUD_ID" SortExpression="SOLICITUD_ID"
                                        Visible="false">
                                        <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle Font-Size="11px" HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <%--2--%>
                                    <asp:BoundField DataField="CLAVE_SOLICITUD" HeaderText="Clave Solicitud" SortExpression="CLAVE_SOLICITUD">
                                        <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle Font-Size="11px" HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <%--3--%>
                                    <asp:BoundField DataField="CONSECUTIVO" HeaderText="Consecutivo" SortExpression="CLAVE_SOLICITUD">
                                        <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle Font-Size="11px" HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <%--4--%>
                                    <asp:BoundField DataField="TRAMITE" HeaderText="Trámite" SortExpression="TRAMITE">
                                        <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle Font-Size="11px" HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    <%--5--%>
                                    <asp:BoundField DataField="NOMBRE_ACTIVIDAD" HeaderText="Actividad Actual" SortExpression="NOMBRE_ACTIVIDAD">
                                        <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="18%" />
                                        <ItemStyle Font-Size="11px" HorizontalAlign="Left" Width="18%" />
                                    </asp:BoundField>
                                    <%-- 6 --%>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS">
                                        <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle Font-Size="11px" HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <%--7--%>
                                    <asp:BoundField DataField="SOLICITO" HeaderText="Solicitó" SortExpression="SOLICITO">
                                        <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="22%" />
                                        <ItemStyle Font-Size="11px" HorizontalAlign="Left" Width="22%" />
                                    </asp:BoundField>
                                    <%--8--%>
                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy hh:mm:ss tt}"
                                        SortExpression="FECHA">
                                        <HeaderStyle Font-Size="13px" HorizontalAlign="Center" Width="22%" />
                                        <ItemStyle Font-Size="11px" HorizontalAlign="Right" Width="22%" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </div>
            <div id="Div_Tramite_id" runat="server" style="overflow: auto; height: 400px; width: 98%;
                vertical-align: top; border-style: hidden; margin-top: 5px; border-color: Silver;
                display: block">
                <table width="98%">
                    <tr>
                        <td>
                            <center>
                                <asp:GridView ID="Grid_Bandeja_Entrada" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                                    Width="97%" GridLines="None" DataKeyNames="SOLICITUD_ID" AllowSorting="True"
                                    OnRowDataBound="Grid_Bandeja_Entrada_RowDataBound" OnSorting="Grid_Bandeja_Entrada_Sorting"
                                    OnPageIndexChanging="Grid_Bandeja_Entrada_PageIndexChanging" OnSelectedIndexChanged="Grid_Bandeja_Entrada_SelectedIndexChanged"
                                    EmptyDataText="No se encuentra ningun tramite en espera">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <%--0--%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Solicitud" OnClick="Btn_Solicitud_Click" ButtonType="Image"
                                                    runat="server" Width="16px" Height="16px" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" Font-Size="12px" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" Font-Size="11px" />
                                        </asp:TemplateField>
                                        <%--1--%>
                                        <asp:BoundField DataField="SOLICITUD_ID" HeaderText="SOLICITUD_ID" SortExpression="SOLICITUD_ID">
                                            <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="0%" />
                                            <ItemStyle Font-Size="11px" HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <%--2--%>
                                        <asp:BoundField DataField="CLAVE_SOLICITUD" HeaderText="Clave Solicitud" SortExpression="CLAVE_SOLICITUD">
                                            <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle Font-Size="11px" HorizontalAlign="Left" Width="10%" />
                                        </asp:BoundField>
                                        <%--3--%>
                                        <asp:BoundField DataField="CONSECUTIVO" HeaderText="Consecutivo" SortExpression="CLAVE_SOLICITUD">
                                            <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle Font-Size="11px" HorizontalAlign="Left" Width="10%" />
                                        </asp:BoundField>
                                        <%--4--%>
                                        <asp:BoundField DataField="TRAMITE" HeaderText="Trámite" SortExpression="TRAMITE">
                                            <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="25%" />
                                            <ItemStyle Font-Size="11px" HorizontalAlign="Left" Width="25%" />
                                        </asp:BoundField>
                                        <%--5--%>
                                        <asp:BoundField DataField="NOMBRE_ACTIVIDAD" HeaderText="Actividad Actual" SortExpression="NOMBRE_ACTIVIDAD">
                                            <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="18%" />
                                            <ItemStyle Font-Size="11px" HorizontalAlign="Left" Width="18%" />
                                        </asp:BoundField>
                                        <%-- 6 --%>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS">
                                            <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="0%" />
                                            <ItemStyle Font-Size="11px" HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <%--7--%>
                                        <asp:BoundField DataField="SOLICITO" HeaderText="Solicitó" SortExpression="SOLICITO">
                                            <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="22%" />
                                            <ItemStyle Font-Size="11px" HorizontalAlign="Left" Width="22%" />
                                        </asp:BoundField>
                                        <%--8--%>
                                        <asp:BoundField DataField="FECHA" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy hh:mm:ss tt}"
                                            SortExpression="FECHA">
                                            <HeaderStyle Font-Size="13px" HorizontalAlign="Center" Width="22%" />
                                            <ItemStyle Font-Size="11px" HorizontalAlign="Right" Width="22%" />
                                        </asp:BoundField>
                                        <%-- 9 --%>
                                        <asp:BoundField DataField="COMPLEMENTO" HeaderText="COMPLEMENTO" SortExpression="COMPLEMENTO">
                                            <HeaderStyle Font-Size="13px" HorizontalAlign="Center" Width="0%" />
                                            <ItemStyle Font-Size="11px" HorizontalAlign="Right" Width="0%" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </center>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Button ID="Btn_Comodin_FGC" runat="server" Text="Comodin" Style="display: none;" />
            <cc1:ModalPopupExtender ID="MPE_Crear_Plantilla" runat="server" TargetControlID="Btn_Comodin_FGC"
                PopupControlID="Pnl_Crear_Plantilla" CancelControlID="Btn_Cancelar" PopupDragHandleControlID="Pnl_Interno"
                DropShadow="True" BackgroundCssClass="progressBackgroundFilter" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="Pnl_Crear_Plantilla" runat="server" CssClass="drag" HorizontalAlign="Center"
        Width="650px" Style="display: none; border-style: outset; border-color: Silver;
        background-repeat: repeat-y;">
        <asp:Panel ID="Pnl_Interno" runat="server" Style="cursor: move; background-color: Silver;
            color: Black; font-size: 12; font-weight: bold; border-style: outset;">
            <center>
                <asp:UpdatePanel ID="UpPnl_Plantilla" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="Hdf_Plantilla_Seleccionada" runat="server" />
                        <table width="95%">
                            <tr>
                                <td style="width: 100%">
                                    <asp:Label ID="Lbl_Error_MPE_Crear_Plantilla" runat="server" Text="" Width="98%" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%">
                                    <center>
                                        <%--<div style="border-style:outset; height:98%px; width:98%; overflow:auto;" >--%>
                                        <div id="Div_Grid_Marcadores_Plantilla" runat="server" style="overflow: auto; height: 200px;
                                            width: 99%; vertical-align: top; border-style: solid; border-color: Silver; display: block">
                                            <asp:GridView ID="Grid_Marcadores_Platilla" runat="server" AutoGenerateColumns="False"
                                                Width="97%" BackColor="White" BorderColor="#336666" BorderStyle="Solid" BorderWidth="3px"
                                                CellPadding="4" GridLines="Horizontal" Font-Size="X-Small" EmptyDataText="No hay datos para este Subproceso">
                                                <FooterStyle BackColor="White" ForeColor="#333333" />
                                                <RowStyle BackColor="White" ForeColor="#333333" HorizontalAlign="Left" />
                                                <Columns>
                                                    <asp:BoundField DataField="MARCADOR_ID" HeaderText="MARCADOR_ID" SortExpression="MARCADOR_ID" />
                                                    <asp:BoundField DataField="NOMBRE_MARCADOR" HeaderText="Insertar" SortExpression="NOMBRE_MARCADOR" />
                                                    <asp:TemplateField HeaderText="Valor">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="Txt_Valor_Marcador" runat="server" Width="98%"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle CssClass="GridHeader" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                        </div>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <table width="95%">
                    <tr>
                        <td style="width: 100%">
                            <center>
                                <asp:Button ID="Btn_Crear_Documento" runat="server" Text="Crear" TabIndex="202" Width="80px"
                                    Height="26px" OnClick="Btn_Crear_Documento_Click" />
                                &nbsp;&nbsp;
                                <asp:Button ID="Btn_Cancelar" runat="server" TabIndex="202" Text="Cancelar" Width="80px"
                                    Height="26px" />
                            </center>
                        </td>
                    </tr>
                </table>
            </center>
        </asp:Panel>
    </asp:Panel>
</asp:Content>

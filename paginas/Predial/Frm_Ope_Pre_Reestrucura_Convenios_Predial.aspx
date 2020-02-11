<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Reestrucura_Convenios_Predial.aspx.cs"
    Inherits="paginas_Predial_Frm_Ope_Pre_Reestrucura_Convenios_Predial" Title=""
    Culture="es-MX" UICulture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType TypeName="Paginas_Generales_paginas_MasterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type="text/javascript" language="javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10000005;
        }

        function getCookie(c_name) {
            if (document.cookie.length > 0) {
                c_start = document.cookie.indexOf(c_name + "=");
                if (c_start != -1) {
                    c_start = c_start + c_name.length + 1;
                    c_end = document.cookie.indexOf(";", c_start);
                    if (c_end == -1) c_end = document.cookie.length;
                    return unescape(document.cookie.substring(c_start, c_end));
                }
            }
            return "";
        }

        function setCookie(c_name, value, expiredays) {
            var exdate = new Date();
            exdate.setDate(exdate.getDate() + expiredays);
            document.cookie = c_name + "=" + escape(value) +
			((expiredays == null) ? "" : ";expires=" + exdate.toUTCString());
        }

        function Abrir_Ventana_Emergente(Url, Propiedades) {

            window.open(Url, name, Propiedades, false);
        }
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }

        function Validar_Tipo_Solicitante() {
            var Tipo_Solicitante_Activo;
            if (document.getElementById("<%=Cmb_Tipo_Solicitante.ClientID%>").value == "DEUDOR SOLIDARIO") {
                Tipo_Solicitante_Activo = true;
                document.getElementById("<%=Txt_Solicitante.ClientID%>").value =
		        document.getElementById("<%=Hdf_Solicitante.ClientID%>").value;
                document.getElementById("<%=Txt_RFC.ClientID%>").value =
		        document.getElementById("<%=Hdf_RFC_Solicitante.ClientID%>").value;
            }
            else {
                Tipo_Solicitante_Activo = false;
                document.getElementById("<%=Hdf_Solicitante.ClientID%>").value =
		        document.getElementById("<%=Txt_Solicitante.ClientID%>").value;
                document.getElementById("<%=Txt_Solicitante.ClientID%>").value =
	            document.getElementById("<%=Txt_Propietario.ClientID%>").value;
                document.getElementById("<%=Hdf_RFC_Solicitante.ClientID%>").value =
		        document.getElementById("<%=Txt_RFC.ClientID%>").value;
                document.getElementById("<%=Txt_RFC.ClientID%>").value =
	            document.getElementById("<%=Hdf_RFC_Propietario.ClientID%>").value;
            }
            document.getElementById("<%=Txt_Solicitante.ClientID%>").disabled = !Tipo_Solicitante_Activo;
            document.getElementById("<%=Txt_RFC.ClientID%>").disabled = !Tipo_Solicitante_Activo;
            if (document.getElementById("<%=Txt_Solicitante.ClientID%>").disabled == false) {
                document.getElementById("<%=Txt_Solicitante.ClientID%>").value =
	            document.getElementById("<%=Hdf_Solicitante.ClientID%>").value;
                document.getElementById("<%=Txt_Solicitante.ClientID%>").focus();
            }
            else {
                document.getElementById("<%=Cmb_Periodicidad_Pago.ClientID%>").focus();
            }
        }

        function cancelBack() {
            if (window.event) {
                if (event.keyCode == 8 && (event.srcElement.form == null || event.srcElement.isTextEdit == false)) {
                    event.cancelBubble = true;
                    event.returnValue = false;
                }
            }
        }
        
        function Replace_Modificado(Cadena_Origen, Cadena_Busqueda) {
            var Patron;

            Cadena_Busqueda || (Cadena_Busqueda = '\\s|\\&nbsp;');
            Patron = new RegExp('^(' + Cadena_Busqueda + ')*', 'g');
            Cadena_Origen = Cadena_Origen.replace(Patron, "");
            Patron = new RegExp('(' + Cadena_Busqueda + ')*$', 'g');
            Cadena_Origen = Cadena_Origen.replace(Patron, "");

            return Cadena_Origen;
        }

        function Replace_Patron(Cadena_Origen, Cadena_Busqueda) {
            var Patron;

            Cadena_Busqueda || (Cadena_Busqueda = '\\_|\\,');
            Patron = new RegExp('(' + Cadena_Busqueda + ')*', 'g');
            Cadena_Origen = Cadena_Origen.replace(Patron, "");

            return Cadena_Origen;
        }

        function Calcular_Total_Descuento() {
            var Descuento_Recargos_Ordinarios = 0.0;
            var Descuento_Recargos_Moratorios = 0.0;
            var Descuento_Recargos_Multa = 0.0;
            var Total_Descuento = 0.0;
            var Total_Adeudo = 0.0;
            var Total_Convenio = 0.0;

            if (Replace_Modificado(Replace_Patron(document.getElementById("<%=Txt_Descuento_Recargos_Ordinarios.ClientID%>").value)) != ""
            && Replace_Modificado(Replace_Patron(document.getElementById("<%=Txt_Descuento_Recargos_Ordinarios.ClientID%>").value)) != ".") {
                Descuento_Recargos_Ordinarios = parseFloat(Replace_Patron(document.getElementById("<%=Txt_Descuento_Recargos_Ordinarios.ClientID%>").value));
            }
            else {
                document.getElementById("<%=Txt_Descuento_Recargos_Ordinarios.ClientID%>").value = "0.0";
            }
            if (Replace_Modificado(Replace_Patron(document.getElementById("<%=Txt_Descuento_Recargos_Moratorios.ClientID%>").value)) != ""
            && Replace_Modificado(Replace_Patron(document.getElementById("<%=Txt_Descuento_Recargos_Moratorios.ClientID%>").value)) != ".") {
                Descuento_Recargos_Moratorios = parseFloat(Replace_Patron(document.getElementById("<%=Txt_Descuento_Recargos_Moratorios.ClientID%>").value));
            }
            else {
                document.getElementById("<%=Txt_Descuento_Recargos_Moratorios.ClientID%>").value = "0.0";
            }
            Total_Descuento = Descuento_Recargos_Ordinarios + Descuento_Recargos_Moratorios + Descuento_Recargos_Multa;
            document.getElementById("<%=Txt_Total_Descuento.ClientID%>").value = Total_Descuento.toFixed(2);

            if (document.getElementById("<%=Txt_Total_Adeudo.ClientID%>").value != "") {
                Total_Adeudo = parseFloat(document.getElementById("<%=Txt_Total_Adeudo.ClientID%>").value);
            }
            else {
                document.getElementById("<%=Txt_Total_Adeudo.ClientID%>").value = "0.0";
            }
            Total_Convenio = Total_Adeudo + Total_Descuento;
            document.getElementById("<%=Txt_Total_Convenio.ClientID%>").value = Total_Convenio.toFixed(2);

        }

        function Calcular_Parcialidades() {
            PageMethods.WM_Calcular_Parcialidades(OnSucceeded, OnFailed);
        }

        function OnSucceeded(result) {
            return true;
        }

        function OnFailed(error) {
            return false;
        }

        function Abrir_Resumen(Url, Propiedades) {
            window.open(Url, 'Resumen_Predio', Propiedades);
        }
    </script>

    <!--SCRIPT PARA LA VALIDACION QUE NO EXPIRE LA SESSION-->

    <script language="javascript" type="text/javascript">
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
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="ScriptManager" runat="server" AsyncPostBackTimeout="3600"
        EnableScriptGlobalization="true" />
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
            <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Reestructura de Convenios de Impuesto Predial
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td>
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" AlternateText="Nuevo"
                                                OnClick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" AlternateText="Modificar"
                                                OnClick="Btn_Modificar_Click" />
                                            <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                                                TabIndex="3" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" AlternateText="Imprimir"
                                                OnClick="Btn_Imprimir_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" AlternateText="Salir"
                                                OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            <table style="width: 100%; height: 28px;">
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                        Filtro:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="Cmb_Busqueda_General" runat="server" TabIndex="5">
                                                            <asp:ListItem Text="TODOS" Value="('ACTIVO','INCUMPLIDO','CANCELADO','PENDIENTE','TERMINADO')" />
                                                            <asp:ListItem Text="ACTIVO" Value="('ACTIVO')" />
                                                            <asp:ListItem Text="INCUMPLIDO" Value="('INCUMPLIDO')" />
                                                            <asp:ListItem Text="CANCELADO" Value="('CANCELADO')" />
                                                            <asp:ListItem Text="PENDIENTE" Value="('PENDIENTE')" />
                                                            <asp:ListItem Text="TERMINADO" Value="('TERMINADO')" />
                                                        </asp:DropDownList>
                                                        <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                            B&uacute;squeda:
                                                        </td>
                                                        <td style="width: 55%;">
                                                            <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="6" ToolTip="Buscar"
                                                                Width="180px" />
                                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                                WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. " />
                                                        </td>
                                                        <td style="vertical-align: middle; width: 5%;">
                                                            <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="7" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                                ToolTip="Buscar" AlternateText="Buscar" OnClick="Btn_Buscar_Click" />
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
                <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                <asp:HiddenField ID="Hdf_Propietario_ID" runat="server" />
                <asp:HiddenField ID="Hdf_RFC_Propietario" runat="server" />
                <asp:HiddenField ID="Hdf_RFC_Solicitante" runat="server" />
                <asp:HiddenField ID="Hdf_Solicitante" runat="server" />
                <asp:HiddenField ID="Hdf_No_Convenio" runat="server" />
                <asp:HiddenField ID="Hdn_Tasa_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Propietario_ID" runat="server" />
                <asp:HiddenField ID="Hdn_No_Descuento" runat="server" />
                <br />
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align: left;" colspan="4">
                            <asp:GridView ID="Grid_Convenios" runat="server" AutoGenerateColumns="False" CssClass="GridView_1"
                                DataKeyNames="CUENTA_PREDIAL_ID" HeaderStyle-CssClass="tblHead" OnPageIndexChanging="Grid_Convenios_PageIndexChanging"
                                OnSelectedIndexChanged="Grid_Convenios_SelectedIndexChanged" AllowPaging="true"
                                PageSize="5" Style="white-space: normal;" Width="100%">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png"
                                        Text="Button">
                                        <HeaderStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="NO_CONVENIO" HeaderText="No. Convenio" Visible="false">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="CUENTA_PREDIAL_ID" Visible="False">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Cuenta_Predial" HeaderText="Cuenta Predial">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CONTRIBUYENTE_ID" HeaderText="PROPIETARIO_ID" Visible="False">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Nombre_Propietario" HeaderText="Propietario" />
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA" DataFormatString="{0:dd/MMM/yyyy}" HeaderText="Fecha">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_REESTRUCTURA" HeaderText="Reestructura" Visible="false">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ANTICIPO_REESTRUCTURA" HeaderText="Anticipo">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <HeaderStyle CssClass="tblHead" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                            <br />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel_Datos" runat="server">
                    <table width="98%" class="estilo_fuente" style="table-layout: fixed;">
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Cuenta Predial
                            </td>
                            <td colspan="3" style="width: 82%; text-align: left;">
                                <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" AutoPostBack="true" MaxLength="20"
                                    TabIndex="8" Width="35.7%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cuenta_Predial" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers"
                                    TargetControlID="Txt_Cuenta_Predial" />
                                &nbsp;
                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial" runat="server"
                                    Height="24px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" OnClick="Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial_Click"
                                    TabIndex="9" ToolTip="Búsqueda Avanzada de Cuenta Predial" Width="24px" />
                                &nbsp;
                                <asp:ImageButton ID="Btn_Detalles_Cuenta_Predial" runat="server" Height="24px" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png"
                                    TabIndex="10" ToolTip="Resumen de predio" Width="24px" />
                                <asp:ImageButton ID="Btn_Convenio_Escaneado" runat="server" Height="24px" ImageUrl="~/paginas/imagenes/gridview/grid_docto.png"
                                    TabIndex="11" ToolTip="Subir convenio escaneado" Width="24px" />
                                <div id="Contenedor_Subir_Convenio" runat="server" style="display: none;">
                                    &nbsp; &nbsp; &nbsp;
                                    <asp:FileUpload ID="Fup_Subir_Convenio_Predial" runat="server" TabIndex="12" />
                                    <asp:ImageButton ID="Btn_Enviar_Archivo" runat="server" Height="24px" ImageUrl="~/paginas/imagenes/paginas/sias_upload.png"
                                        OnClick="Btn_Enviar_Archivo_Click" TabIndex="13" ToolTip="Enviar archivo" Width="24px" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Propietario
                            </td>
                            <td style="text-align: left; width: 20%;" colspan="3">
                                <asp:TextBox ID="Txt_Propietario" runat="server" ReadOnly="True" Width="96.4%" TabIndex="14" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Colonia
                            </td>
                            <td style="text-align: left; width: 20%;">
                                <asp:TextBox ID="Txt_Colonia" runat="server" ReadOnly="True" Width="96.4%" TabIndex="15" />
                            </td>
                            <td style="text-align: right; width: 20%;">
                                Calle
                            </td>
                            <td style="text-align: left; width: 20%;">
                                <asp:TextBox ID="Txt_Calle" runat="server" ReadOnly="True" Width="96.4%" TabIndex="16" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                No. exterior
                            </td>
                            <td style="text-align: left; width: 20%;">
                                <asp:TextBox ID="Txt_No_Exterior" runat="server" ReadOnly="True" Width="96.4%" TabIndex="17" />
                            </td>
                            <td style="text-align: right; width: 20%;">
                                No. interior
                            </td>
                            <td style="text-align: left; width: 20%;">
                                <asp:TextBox ID="Txt_No_Interior" runat="server" ReadOnly="True" Width="96.4%" TabIndex="18" />
                            </td>
                        </tr>
                        <!------------------------------ ADEUDOS ------------------------------>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Al periodo
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Hasta_Anio_Periodo" runat="server" OnSelectedIndexChanged="Cmb_Hasta_Periodo_SelectedIndexChanged"
                                    AutoPostBack="true" TabIndex="19" Style="width: 65px; text-align: center;">
                                    <asp:ListItem Value="2011">2011</asp:ListItem>
                                    <asp:ListItem Value="2010">2010</asp:ListItem>
                                    <asp:ListItem Value="2009">2009</asp:ListItem>
                                    <asp:ListItem Value="2008">2008</asp:ListItem>
                                    <asp:ListItem Value="2007">2007</asp:ListItem>
                                    <asp:ListItem Value="2006">2006</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="Cmb_Hasta_Bimestre_Periodo" runat="server" OnSelectedIndexChanged="Cmb_Hasta_Periodo_SelectedIndexChanged"
                                    AutoPostBack="true" TabIndex="20" Style="width: 40px; text-align: center;">
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Total adeudo
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Monto_Total_Adeudo" runat="server" ReadOnly="True" Width="80%"
                                    TabIndex="21" Style="text-align: right;" />
                                <asp:ImageButton ID="Btn_Desglose_Adeudos" runat="server" Height="24px" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png"
                                    TabIndex="22" ToolTip="Adeudos por bimestre" Width="24px" />
                                <asp:HiddenField ID="Hdn_Monto_Impuesto" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Adeudos rezago
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Adeudo_Rezago" runat="server" ReadOnly="True" Width="96.4%"
                                    TabIndex="23" Style="text-align: right;" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Adeudo corriente
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Adeudo_Corriente" runat="server" ReadOnly="True" Enabled="false"
                                    Width="96.4%" Style="text-align: right;" TabIndex="24" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Adeudo recargos
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Monto_Recargos" runat="server" ReadOnly="True" Width="96.4%"
                                    TabIndex="25" Style="text-align: right;" AutoPostBack="true" OnTextChanged="Txt_Monto_Recargos_TextChanged" />
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Monto_Recargos" runat="server" TargetControlID="Txt_Monto_Recargos"
                                    FilterType="Custom, Numbers" ValidChars=".," />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Recargos moratorios
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Monto_Moratorios" runat="server" ReadOnly="True" Enabled="false"
                                    Width="96.4%" Style="text-align: right;" TabIndex="26" AutoPostBack="true" OnTextChanged="Txt_Monto_Moratorios_TextChanged" />
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Monto_Moratorios" runat="server" TargetControlID="Txt_Monto_Moratorios"
                                    FilterType="Custom, Numbers" ValidChars=".," />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Adeudo honorarios
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Adeudo_Honorarios" runat="server" ReadOnly="True" Enabled="false"
                                    Width="96.4%" Style="text-align: right;" TabIndex="27" MaxLength="15" AutoPostBack="true"
                                    OnTextChanged="Txt_Adeudo_Honorarios_TextChanged" />
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Adeudo_Honorarios" runat="server" TargetControlID="Txt_Adeudo_Honorarios"
                                    FilterType="Custom, Numbers" ValidChars=".," />
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
                        <%------------------ Convenio ------------------%>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Número de reestructura
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Numero_Reestructura" runat="server" ReadOnly="True" Width="96.4%" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                &nbsp;
                            </td>
                            <td style="text-align: left; width: 30%;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr style="background-color: #3366CC">
                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                Convenio
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Número convenio
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Numero_Convenio" runat="server" ReadOnly="True" Width="96.4%"
                                    TabIndex="28" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Estatus
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%" TabIndex="29">
                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                    <asp:ListItem>ACTIVO</asp:ListItem>
                                    <asp:ListItem>PENDIENTE</asp:ListItem>
                                    <asp:ListItem>CANCELADO</asp:ListItem>
                                    <asp:ListItem>INCUMPLIDO</asp:ListItem>
                                    <asp:ListItem>TERMINADO</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Tipo solicitante
                            </td>
                            <td style="text-align: left; width: 30%;" colspan="3">
                                <asp:DropDownList ID="Cmb_Tipo_Solicitante" runat="server" Width="99.6%" onChange="Validar_Tipo_Solicitante();"
                                    TabIndex="30">
                                    <asp:ListItem>PROPIETARIO</asp:ListItem>
                                    <asp:ListItem>DEUDOR SOLIDARIO</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                *Solicitante
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%; vertical-align: top;">
                                <asp:TextBox ID="Txt_Solicitante" runat="server" TabIndex="31" TextMode="SingleLine"
                                    Width="98.6%" MaxLength="256" Style="text-transform: uppercase" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *RFC
                            </td>
                            <td style="text-align: left; width: 30%;" colspan="3">
                                <asp:TextBox ID="Txt_RFC" runat="server" Width="98.6%" TabIndex="32" MaxLength="15"
                                    Style="text-transform: uppercase" />
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_RFC" runat="server" TargetControlID="Txt_RFC"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ-" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Periodicidad de pago
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Periodicidad_Pago" runat="server" Width="99%" TabIndex="33"
                                    AutoPostBack="True" OnSelectedIndexChanged="Cmb_Periodicidad_Pago_SelectedIndexChanged">
                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                    <asp:ListItem Value="7">Semanal</asp:ListItem>
                                    <asp:ListItem Value="14">Catorcenal</asp:ListItem>
                                    <asp:ListItem Value="15">Quincenal</asp:ListItem>
                                    <asp:ListItem Value="30">1 Mes</asp:ListItem>
                                    <asp:ListItem Value="2MES">2 Meses</asp:ListItem>
                                    <asp:ListItem Value="3MES">3 Meses</asp:ListItem>
                                    <asp:ListItem Value="4MES">4 Meses</asp:ListItem>
                                    <asp:ListItem Value="5MES">5 Meses</asp:ListItem>
                                    <asp:ListItem Value="6MES">6 Meses</asp:ListItem>
                                    <asp:ListItem Value="7MES">7 Meses</asp:ListItem>
                                    <asp:ListItem Value="8MES">8 Meses</asp:ListItem>
                                    <asp:ListItem Value="9MES">9 Meses</asp:ListItem>
                                    <asp:ListItem Value="10MES">10 Meses</asp:ListItem>
                                    <asp:ListItem Value="11MES">11 Meses</asp:ListItem>
                                    <asp:ListItem Value="365">Anual</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                *Número de parcialidades
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Numero_Parcialidades" runat="server" Width="96.4%" AutoPostBack="True"
                                    TabIndex="34" MaxLength="3" OnTextChanged="Txt_Numero_Parcialidades_TextChanged" />
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Numero_Parcialidades" runat="server" TargetControlID="Txt_Numero_Parcialidades"
                                    FilterType="Numbers" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Realizó
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Realizo" runat="server" ReadOnly="True" Width="96.4%" TabIndex="35" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Fecha
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Convenio" runat="server" ReadOnly="True" Width="96.4%"
                                    TabIndex="36" AutoPostBack="true" OnTextChanged="Txt_Fecha_Convenio_TextChanged" />
                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Fecha" runat="server" TargetControlID="Txt_Fecha_Convenio"
                                    WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                <cc1:CalendarExtender ID="DtpTxt_Fecha" runat="server" TargetControlID="Txt_Fecha_Convenio"
                                    Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                &nbsp;
                            </td>
                            <td style="text-align: left; width: 30%;">
                                &nbsp;
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Fecha vencimiento
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Vencimiento" runat="server" ReadOnly="True" Width="96.4%"
                                    TabIndex="37" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                Observaciones
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%;">
                                <asp:TextBox ID="Txt_Observaciones" runat="server" TabIndex="38" MaxLength="250"
                                    TextMode="MultiLine" Width="98.6%" Style="text-transform: uppercase;" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios" runat="server" TargetControlID="Txt_Observaciones"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <%------------------ Descuentos ------------------%>
                        <tr style="background-color: #3366CC">
                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                Descuentos
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Descuento recargos ordinarios (%)
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Descuento_Recargos_Ordinarios" runat="server" Width="96.4%"
                                    MaxLength="5" OnTextChanged="Txt_Descuento_Recargos_Ordinarios_TextChanged" TabIndex="39"
                                    Style="text-align: right;" />
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Descuento_Recargos_Ordinarios" runat="server"
                                    TargetControlID="Txt_Descuento_Recargos_Ordinarios" FilterType="Custom, Numbers"
                                    ValidChars=".," />
                            </td>
                            <td style="text-align: right; width: 20%;">
                                Descuento recargos moratorios (%)
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Descuento_Recargos_Moratorios" runat="server" Width="96.4%"
                                    MaxLength="5" OnTextChanged="Txt_Descuento_Recargos_Moratorios_TextChanged" TabIndex="40"
                                    Style="text-align: right;" />
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Descuento_Recargos_Moratorios" runat="server"
                                    TargetControlID="Txt_Descuento_Recargos_Moratorios" FilterType="Custom, Numbers"
                                    ValidChars=".," />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                &nbsp;
                            </td>
                            <td style="text-align: right; width: 20%; border-top: solid 1px BLACK;" colspan="3">
                                Total Adeudo
                                <asp:TextBox ID="Txt_Total_Adeudo" runat="server" Width="100px" Style="text-align: right;"
                                    TabIndex="41" />
                                <cc1:MaskedEditExtender ID="Mee_Txt_Total_Adeudo" runat="server" TargetControlID="Txt_Total_Adeudo"
                                    Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                    CultureTimePlaceholder="" Enabled="True" />
                                - Total Descuento
                                <asp:TextBox ID="Txt_Total_Descuento" runat="server" Width="100px" Style="text-align: right;"
                                    TabIndex="42" />
                                <cc1:MaskedEditExtender ID="Mee_Txt_Total_Descuento" runat="server" TargetControlID="Txt_Total_Descuento"
                                    Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                    CultureTimePlaceholder="" Enabled="True" />
                                = Sub-Total
                                <asp:TextBox ID="Txt_Sub_Total" runat="server" Width="100px" Style="text-align: right;"
                                    TabIndex="43" />
                                <cc1:MaskedEditExtender ID="Mee_Txt_Sub_Total" runat="server" TargetControlID="Txt_Sub_Total"
                                    Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                    CultureTimePlaceholder="" Enabled="True" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <%------------------ Parcialidades ------------------%>
                        <tr style="background-color: #3366CC">
                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                Parcialidades
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;" colspan="4">
                                *Porcentaje Anticipo
                                <asp:TextBox ID="Txt_Porcentaje_Anticipo" runat="server" Width="60px" TabIndex="44"
                                    AutoPostBack="True" OnTextChanged="Txt_Porcentaje_Anticipo_TextChanged" Style="text-align: right;"
                                    MaxLength="5" />
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Porcentaje_Anticipo" runat="server" TargetControlID="Txt_Porcentaje_Anticipo"
                                    FilterType="Custom, Numbers" ValidChars=",." />
                                *Total Anticipo
                                <asp:TextBox ID="Txt_Total_Anticipo" runat="server" Width="200px" TabIndex="45" AutoPostBack="True"
                                    OnTextChanged="Txt_Total_Anticipo_TextChanged" Style="text-align: right;" MaxLength="20" />
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Total_Anticipo" runat="server" TargetControlID="Txt_Total_Anticipo"
                                    FilterType="Custom, Numbers" ValidChars=",." />
                                Saldo
                                <asp:TextBox ID="Txt_Total_Convenio" runat="server" TabIndex="46" Width="200px" Style="text-align: right;" />
                                <cc1:MaskedEditExtender ID="Mee_Txt_Total_Convenio" runat="server" TargetControlID="Txt_Total_Convenio"
                                    Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                    CultureTimePlaceholder="" Enabled="True" />
                            </td>
                            <td style="text-align: right; width: 20%;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;" colspan="4">
                                <asp:CheckBox ID="Chk_Parcialidades_Manuales" runat="server" AutoPostBack="true"
                                    Text="Establecer parcialidades de forma manual" Checked="false" OnCheckedChanged="Chk_Parcialidades_Manuales_CheckedChanged" />
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="4">
                                <br />
                                <asp:GridView ID="Grid_Parcialidades" runat="server" AutoGenerateColumns="False"
                                    CssClass="GridView_1" HeaderStyle-CssClass="tblHead" ShowFooter="True" Style="white-space: normal;"
                                    Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="NO_PAGO" FooterText="Total" HeaderText="No. Pago">
                                            <FooterStyle Font-Bold="True" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Periodo" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="12%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txt_Grid_Periodo" runat="server" AutoPostBack="true" OnTextChanged="Txt_Grid_Periodo_TextChanged"
                                                    Style="width: 96%; text-align: right;" Visible='<%# Grid_Parcialidades_Manuales %>'
                                                    Text='<%# Bind("PERIODO", "{0:#,##0.00}") %>' MaxLength="16">
                                                </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Periodo" runat="server" FilterType=" Numbers,Custom"
                                                    TargetControlID="Txt_Grid_Periodo" ValidChars="-/" />
                                                <asp:Label ID="Lbl_Txt_Grid_Periodo" runat="server" Style="width: 96%; text-align: center;"
                                                    Visible='<%# !(bool)Grid_Parcialidades_Manuales %>' Text='<%# Bind("PERIODO", "{0:c2}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Honorarios" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="12%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txt_Grid_Monto_Honorarios" runat="server" AutoPostBack="true" OnTextChanged="Txt_Grid_Monto_Honorarios_TextChanged"
                                                    Style="width: 96%; text-align: right;" Visible='<%# Grid_Parcialidades_Manuales %>'
                                                    Text='<%# Bind("MONTO_HONORARIOS", "{0:#,##0.00}") %>' MaxLength="16">
                                                </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Monto_Honorarios" runat="server" FilterType=" Numbers,Custom"
                                                    TargetControlID="Txt_Grid_Monto_Honorarios" ValidChars=".,$" />
                                                <asp:Label ID="Lbl_Txt_Grid_Monto_Honorarios" runat="server" Style="width: 96%; text-align: right;"
                                                    Visible='<%# !(bool)Grid_Parcialidades_Manuales %>' Text='<%# Bind("MONTO_HONORARIOS", "{0:c2}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Recargos Ordinarios" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="12%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txt_Grid_Monto_Recargos_Ordinarios" runat="server" AutoPostBack="true"
                                                    OnTextChanged="Txt_Grid_Monto_Recargos_Ordinarios_TextChanged" Style="width: 96%;
                                                    text-align: right;" Visible='<%# Grid_Parcialidades_Manuales %>' Text='<%# Bind("RECARGOS_ORDINARIOS", "{0:#,##0.00}") %>'
                                                    MaxLength="16">
                                                </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Monto_Recargos_Ordinarios" runat="server"
                                                    FilterType=" Numbers,Custom" TargetControlID="Txt_Grid_Monto_Recargos_Ordinarios"
                                                    ValidChars=".,$" />
                                                <asp:Label ID="Lbl_Txt_Grid_Monto_Recargos_Ordinarios" runat="server" Style="width: 96%;
                                                    text-align: right;" Visible='<%# !(bool)Grid_Parcialidades_Manuales %>' Text='<%# Bind("RECARGOS_ORDINARIOS", "{0:c2}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Recargos Moratorios" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="12%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txt_Grid_Monto_Recargos_Moratorios" runat="server" AutoPostBack="true"
                                                    OnTextChanged="Txt_Grid_Monto_Recargos_Moratorios_TextChanged" Style="width: 96%;
                                                    text-align: right;" Visible='<%# Grid_Parcialidades_Manuales %>' Text='<%# Bind("RECARGOS_MORATORIOS", "{0:#,##0.00}") %>'
                                                    MaxLength="16">
                                                </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Monto_Recargos_Moratorios" runat="server"
                                                    FilterType=" Numbers,Custom" TargetControlID="Txt_Grid_Monto_Recargos_Moratorios"
                                                    ValidChars=".,$" />
                                                <asp:Label ID="Lbl_Txt_Grid_Monto_Recargos_Moratorios" runat="server" Style="width: 96%;
                                                    text-align: right;" Visible='<%# !(bool)Grid_Parcialidades_Manuales %>' Text='<%# Bind("RECARGOS_MORATORIOS", "{0:c2}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MONTO_IMPUESTO" DataFormatString="{0:c2}" HeaderText="Impuesto">
                                            <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Importe" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txt_Grid_Monto_Importe" runat="server" AutoPostBack="true" OnTextChanged="Txt_Grid_Monto_Importe_TextChanged"
                                                    Style="width: 96%; text-align: right;" Visible='<%# Grid_Parcialidades_Editable %>'
                                                    Text='<%# Bind("MONTO_IMPORTE", "{0:#,##0.00}") %>'>
                                                </asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Monto_Importe" runat="server" FilterType=" Numbers,Custom"
                                                    TargetControlID="Txt_Grid_Monto_Importe" ValidChars=".,$" />
                                                <asp:Label ID="Lbl_Txt_Grid_Monto_Importe" runat="server" Style="width: 96%; text-align: right;"
                                                    Visible='<%# !(bool)Grid_Parcialidades_Editable %>' Text='<%# Bind("MONTO_IMPORTE", "{0:c2}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle Font-Bold="True" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FECHA_VENCIMIENTO" DataFormatString="{0:dd/MMM/yyyy}"
                                            HeaderText="Fecha Vencimiento">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                                <br />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

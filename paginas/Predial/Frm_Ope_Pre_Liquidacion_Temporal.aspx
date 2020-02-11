<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Liquidacion_Temporal.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Liquidacion_Temporal" %>

<%@ MasterType TypeName="Paginas_Generales_paginas_MasterPage" %>
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

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
        setInterval('MantenSesion()', "<%=(int)(0.9*(Session.Timeout * 60000))%>");

    //Metodos para abrir los Modal PopUp's de la página
    function Abrir_Busqueda_Cuentas_Predial() {
        $find('Busqueda_Cuentas_Predial').show();
        return false;
    }
    function Abrir_Detalle_Cuenta_Predial() {
        $find('Detalle_Cuenta_Predial').show();
        return false;
    }
    function Abrir_Ventana_Modal(Url, Propiedades)
		{
			window.showModalDialog(Url, null, Propiedades);
		}

	function cancelBack() {
	    if (window.event) {
	        if (event.keyCode == 8 && (event.srcElement.form == null || event.srcElement.isTextEdit == false)) {
	            event.cancelBubble = true;
	            event.returnValue = false;
	        }
	    }
	}
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(initRequest);
        function initRequest(sender, args) {
            if (Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack())
                args.set_cancel(true);
        }   
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server" />
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
                        <td class="label_titulo">
                            Liquidación temporal
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
                    <tr align="center">
                        <td>
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                                                TabIndex="4" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" Visible="false"
                                                OnClick="Btn_Imprimir_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            <table style="width: 100%; height: 28px;">
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                    </td>
                                                    <td style="width: 55%;">
                                                    </td>
                                                    <td style="vertical-align: middle; width: 5%;">
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
                <table width="98%" class="estilo_fuente">
                    <%--<tr style="background-color: #3366CC" id="Tr_Encabezado_Ordenenes" runat="server" >
                        <td style="text-align:left; font-size:15px; color:#FFFFFF;" >
                            Órdenes de Variación por aplicar
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;">
                            <asp:GridView ID="Grid_Ordenes_Variacion" runat="server" 
                                AutoGenerateColumns="False" CssClass="GridView_Nested" 
                                HeaderStyle-CssClass="tblHead" 
                                style="white-space:normal;" Width="100%" AllowPaging="True" 
                                onpageindexchanging="Grid_Ordenes_Variacion_PageIndexChanging" 
                                onselectedindexchanged="Grid_Ordenes_Variacion_SelectedIndexChanged" 
                                PageSize="5" DataKeyNames="NO_CONTRARECIBO">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png" Text="Button">
                                        <HeaderStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="NO_ORDEN_VARIACION" HeaderText="No. Orden">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="USUARIO_CREO" HeaderText="Realizó">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Identificador_Movimiento" 
                                        HeaderText="Clave Movimiento">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA_CREO" HeaderText="Fecha" 
                                        DataFormatString="{0:dd/MMM/yyyy}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_CONTRARECIBO" HeaderText="NO_CONTRARECIBO" 
                                        Visible="False" />
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <HeaderStyle CssClass="tblHead" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                            <br />
                        </td>
                    </tr>--%>
                </table>
                <asp:Panel ID="Panel" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Cuenta_Predial" runat="server">*Cuenta Predial</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Enable="false" MaxLength="20"
                                    OnTextChanged="Txt_Cuenta_Predial_TextChanged" TabIndex="9" Width="96.4%"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada" runat="server" ToolTip="Búsqueda Avanzada de Cuenta Predial"
                                    TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px"
                                    Width="24px" OnClick="Btn_Mostrar_Busqueda_Avanzada_Click" />
                            </td>
                        </tr>
                        <%------------------ Datos generales ------------------%>
                        <tr style="background-color: #3366CC">
                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                Generales
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Tipo_Predio" runat="server">Tipo predio</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Tipo_Predio" runat="server" Width="96.4%" />
                                <asp:HiddenField ID="Hdn_Tipo_Predio_ID" runat="server" />
                            </td>
                            <td style="width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Uso_Predio" runat="server">Uso predio</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Uso_Predio" runat="server" Width="96.4%" />
                                <asp:HiddenField ID="Hdn_Uso_Predio_ID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Colonia_Cuenta" runat="server">*Calle</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Calle_Cuenta" runat="server" Width="96.4%" />
                                <asp:HiddenField ID="Hdn_Ciudad_Cuenta" runat="server" />
                            </td>
                            <td style="width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Calle_Cuenta" runat="server" Text="*Colonia"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Colonia_Cuenta" runat="server" Width="96.4%" />
                                <asp:HiddenField ID="Hdn_Estado_Cuenta" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_No_Exterior" runat="server" Text="Número exterior"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_No_Exterior" runat="server" Width="99%" MaxLength="80" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_No_Interior" runat="server" Text="Número interior"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_No_Interior" runat="server" Width="99%" MaxLength="80" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Valor_Fiscal" runat="server">Valor fiscal</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Valor_Fiscal" runat="server" Width="96.4%" />
                            </td>
                            <td style="width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Efectos" runat="server">Efectos</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Efectos" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Tasa_Porcentaje" runat="server">Tasa</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Tasa_Porcentaje" runat="server" Width="96.4%" />
                            </td>
                            <td style="width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Cuota_Bimestral" runat="server">Cuota bimestral</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Cuota_Bimestral" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Ultimo_Movimiento" runat="server">Ultimo 
                            movimiento</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Ultimo_Movimiento" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <%------------------ Propietario ------------------%>
                    <table width="98%" class="estilo_fuente">
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Propietario
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Nombre_Propietario" runat="server" Text="Nombre"></asp:Label>
                            </td>
                            <td style="text-align: left;" colspan="3">
                                <asp:TextBox ID="Txt_Nombre_Propietario" runat="server" Width="99%" Style="float: left" />
                                <asp:HiddenField ID="Hdn_Propietario_ID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_RFC_Propietario" runat="server" Text="RFC"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_RFC_Propietario" runat="server" Width="96.4%" />
                            </td>
                            <td style="width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Domicilio_Foraneo" runat="server">Domicilio foráneo</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Domicilio_Foraneo" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Calle_Propietario" runat="server" Text="Calle"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Calle_Propietario" runat="server" Width="96.4%" />
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Numero_Exterior_Propietario" runat="server">Número exterior</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Numero_Exterior_Propietario" runat="server" Width="99%" MaxLength="80" />
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Numero_Interior_Propietario" runat="server">Número interior</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Numero_Interior_Propietario" runat="server" Width="99%" MaxLength="80" />
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Colonia_Propietario" runat="server" Text="Colonia"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Colonia_Propietario" runat="server" Width="96.4%" />
                            </td>
                            <td style="width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_CP" runat="server">C.P.</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_CP" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Ciudad_Propietario" runat="server" Text="Ciudad"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Ciudad_Propietario" runat="server" Width="96.4%" />
                            </td>
                            <td style="width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Estado_Propietario" runat="server" Text="Estado"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Estado_Propietario" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                    </table>
                    <table width="98%" class="estilo_fuente">
                        <%------------------ Adeudos ------------------%>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Adeudos
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Periodo_Inicial" runat="server">Periodo inicial</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Periodo_Inicial" runat="server" Width="96.4%"></asp:TextBox>
                            </td>
                            <td style="width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Periodo_Final" runat="server">Periodo final</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Hasta_Bimestre_Periodo" runat="server" OnSelectedIndexChanged="Cmb_Hasta_Periodo_SelectedIndexChanged"
                                    AutoPostBack="true" TabIndex="17" Style="width: 40px; text-align: center;">
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="Cmb_Hasta_Anio_Periodo" runat="server" OnSelectedIndexChanged="Cmb_Hasta_Periodo_SelectedIndexChanged"
                                    AutoPostBack="true" TabIndex="16" Style="width: 65px; text-align: center;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="4">
                                <asp:GridView ID="Grid_Adeudos" runat="server" CssClass="GridView_1" AutoGenerateColumns="false"
                                    PageSize="5" AllowPaging="false" Width="100%" HeaderStyle-CssClass="tblHead"
                                    Style="white-space: normal;">
                                    <Columns>
                                        <asp:BoundField DataField="ANIO" HeaderText="Año" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" />
                                        <asp:BoundField DataField="Bimestre1" HeaderText="Bimestre 1" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Right" NullDisplayText="0.00" />
                                        <asp:BoundField DataField="Bimestre2" HeaderText="Bimestre 2" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Right" NullDisplayText="0.00" />
                                        <asp:BoundField DataField="Bimestre3" HeaderText="Bimestre 3" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Right" NullDisplayText="0.00" />
                                        <asp:BoundField DataField="Bimestre4" HeaderText="Bimestre 4" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Right" NullDisplayText="0.00" />
                                        <asp:BoundField DataField="Bimestre5" HeaderText="Bimestre 5" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Right" NullDisplayText="0.00" />
                                        <asp:BoundField DataField="Bimestre6" HeaderText="Bimestre 6" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Right" NullDisplayText="0.00" />
                                        <asp:BoundField DataField="TOTAL" HeaderText="Total" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="14%" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp
                            </td>
                            <td style="width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Total_Impuesto" runat="server">Total impuesto</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Total_Impuesto" runat="server" Width="96.4%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp
                            </td>
                            <td style="width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Total_Recargos_Ordinarios" runat="server">Total recargos </asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Total_Recargos_Ordinarios" runat="server" Width="96.4%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                            <td style="text-align: right; width: 20%; border-top: solid 1px BLACK">
                                Total
                            </td>
                            <td style="text-align: left; width: 30%; border-top: solid 1px BLACK">
                                <asp:TextBox ID="Txt_Total" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <asp:HiddenField ID="Hdn_No_Orden_Variacion" runat="server" />
                <asp:HiddenField ID="Hdn_Cuenta_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Cuota_Minima" runat="server" />
                <asp:HiddenField ID="Hdn_Contrarecibo" runat="server" />
                <asp:HiddenField ID="Hdn_Tasa_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Cuota_Minima_ID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

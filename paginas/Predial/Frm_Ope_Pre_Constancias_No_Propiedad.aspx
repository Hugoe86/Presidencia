<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Constancias_No_Propiedad.aspx.cs"
    Inherits="paginas_Predial_Frm_Ope_Pre_Constancias_No_Propiedad" Culture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">

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
    function Abrir_Busqueda_Contribuyentes() {
        $find('Busqueda_Contribuyentes').show();
        return false;
    }

    function Limpiar_Ctlr() {
        document.getElementById("<%=Txt_Busqueda_Contribuyente_ID.ClientID%>").value = "";
        document.getElementById("<%=Txt_Busqueda_Propietatio.ClientID%>").value = "";
        document.getElementById("<%=Txt_Busqueda_RFC.ClientID%>").value = "";
        document.getElementById("<%=Txt_Busqueda_Domicilio.ClientID%>").value = "";
        alert('si');
        document.getElementById("<%=Grid_Contribuyentes.ClientID%>").value = null;
        alert('no');
        document.getElementById("<%=Grid_Contribuyentes.ClientID%>").databind();
        return false;
    }

    </script>

    <asp:ScriptManager ID="ScriptManager" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" />
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
                            Constancias de No propiedad
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
                                                Width="24px" Height="24px" />
                                            <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%;">
                                        </td>
                                        <td style="width: 90%; text-align: left;" valign="top">
                                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="Hdf_Proteccion_Pago" runat="server" />
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td>
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click"
                                                AlternateText="Nuevo" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click"
                                                AlternateText="Modificar" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click"
                                                AlternateText="Salir" />
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            <table style="width: 100%; height: 28px;">
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                        B&uacute;squeda:
                                                    </td>
                                                    <td style="width: 55%;">
                                                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar"
                                                            Width="180px" Style="text-transform: uppercase" />
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. " />
                                                    </td>
                                                    <td style="vertical-align: middle; width: 5%;">
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
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Búsqueda
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:ImageButton ID="Btn_Mostrar_Busqueda_Constancias_Avanzada" runat="server" ToolTip="Búsqueda Avanzada de Cuenta Predial"
                                TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px"
                                Width="24px" OnClientClick="javascript:return Abrir_Busqueda_Contribuyentes();" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            RFC
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_RFC" runat="server" Width="96.4%" Style="text-transform: uppercase" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            *Nombre solicitante
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="Txt_Nombre_Solicitante" runat="server" TabIndex="10" MaxLength="250"
                                TextMode="SingleLine" Width="98.6%" Style="text-transform: uppercase" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            *Domicilio
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="Txt_Domicilio" runat="server" TabIndex="11" MaxLength="250" TextMode="SingleLine"
                                Width="98.6%" Style="text-transform: uppercase" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Folio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Folio" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: right; width: 20%;">
                            Fecha
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Fecha" runat="server" Width="96.4%" Enabled="false" />
                            <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha" runat="server" TargetControlID="Txt_Fecha"
                                WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                            <cc1:CalendarExtender runat="server" ID="CE_Txt_Fecha" Format="dd/MM/yyyy" TargetControlID="Txt_Fecha"
                                Enabled="True" PopupButtonID="Img_Calendario" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            No. Recibo de pago
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_No_Recibo_Pago" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Estatus
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                <asp:ListItem Text="POR PAGAR" Value="POR PAGAR" />
                                <asp:ListItem Text="PAGADA" Value="PAGADA" />
                                <asp:ListItem Text="IMPRESA" Value="IMPRESA" />
                                <asp:ListItem Text="CANCELADA" Value="CANCELADA" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Fecha Vencimiento
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Fecha_Vencimiento" runat="server" Width="96.4%"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="Txt_Fecha_Vencimiento"
                                WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Confrontó
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="Txt_Confronto" runat="server" TabIndex="10" MaxLength="250" TextMode="SingleLine"
                                Width="98.6%" AutoPostBack="True" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Observaciones
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="Txt_Observaciones" runat="server" TabIndex="10" MaxLength="250"
                                TextMode="MultiLine" Width="98.6%" Style="text-transform: uppercase" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios" runat="server" TargetControlID="Txt_Observaciones"
                                WatermarkText="Límite de Caracteres 250" WatermarkCssClass="watermarked" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios" runat="server" TargetControlID="Txt_Observaciones"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                        </td>
                    </tr>
                    <caption>
                        <asp:HiddenField ID="Hdf_No_Constancia" runat="server" />
                        <asp:HiddenField ID="Hdf_Propietario_ID" runat="server" />
                        <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                        <tr align="center">
                            <td colspan="4">
                                <asp:GridView ID="Grid_Constancias_No_Propiedad" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CssClass="GridView_1" HeaderStyle-CssClass="tblHead"
                                    PageSize="5" Style="white-space: normal;" Width="100%" OnPageIndexChanging="Grid_Constancias_No_Propiedad_PageIndexChanging"
                                    OnSelectedIndexChanged="Grid_Constancias_No_Propiedad_SelectedIndexChanged" OnRowCommand="Grid_Constancias_No_Propiedad_RowCommand"
                                    OnRowDataBound="Grid_Constancias_No_Propiedad_RowDataBound">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="SOLICITANTE" HeaderText="Solicitante" SortExpression="Nombre_Propietario" />
                                        <asp:BoundField DataField="SOLICITANTE_RFC" HeaderStyle-Width="13%" HeaderText="RFC"
                                            SortExpression="RFC_Propietario">
                                            <HeaderStyle Width="13%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FOLIO" HeaderText="Folio" SortExpression="FOLIO" />
                                        <asp:BoundField DataField="FECHA" HeaderText="Fecha" SortExpression="FECHA" DataFormatString="{0:dd/MMM/yyyy}" />
                                        <asp:BoundField DataField="FECHA_VENCIMIENTO" HeaderText="Fecha Vencimiento" SortExpression="FECHA"
                                            DataFormatString="{0:dd/MMM/yyyy}" />
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS" />
                                        <asp:BoundField DataField="DOMICILIO" HeaderText="Domicilio" SortExpression="DOMICILIO" />
                                        <asp:ButtonField ButtonType="Image" CommandName="Print" ImageUrl="~/paginas/imagenes/gridview/grid_print.png">
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:ButtonField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </caption>
                </table>
                <%---------------------------------- Modal Popup Extender búsqueda de Contribuyentes ----------------------------------%>
                <cc1:ModalPopupExtender ID="Mpe_Busqueda_Contribuyentes" runat="server" TargetControlID="Btn_Comodin_Open_Busqueda_Notarios"
                    PopupControlID="Pnl_Busqueda_Contenedor_Contribuyentes" BackgroundCssClass="popUpStyle"
                    BehaviorID="Busqueda_Contribuyentes" CancelControlID="Btn_Comodin_Close_Busqueda_Notarios"
                    DropShadow="true" DynamicServicePath="" Enabled="True" />
                <asp:Button ID="Btn_Comodin_Close_Busqueda_Notarios" runat="server" Style="background-color: transparent;
                    border-style: none; display: none;" Text="" />
                <asp:Button ID="Btn_Comodin_Open_Busqueda_Notarios" runat="server" Style="background-color: transparent;
                    border-style: none; display: none;" Text="" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <%---------------------------------- Panel búsqueda de Contribuyentes ----------------------------------%>
    <asp:Panel ID="Pnl_Busqueda_Contenedor_Contribuyentes" runat="server" CssClass="drag"
        Style="display: none; width: 650px; border-style: outset; border-color: Silver;
        background-image: url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG); background-repeat: repeat-y;">
        <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" Style="cursor: move; background-color: Silver;
            color: Black; font-size: 12; font-weight: bold; border-style: outset;">
            <table width="99%">
                <tr>
                    <td style="color: Black; text-align: center; font-size: 12; font-weight: bold;">
                        <asp:Image ID="Img_Icono_Busqueda_Contribuyentes" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" />
                        B&uacute;squeda: Contribuyentes
                    </td>
                    <td align="right" style="width: 5%;">
                        <asp:ImageButton ID="Btn_Cerrar_Busqueda_Contribuyentes" runat="server" Style="cursor: pointer;"
                            ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"
                            OnClick="Btn_Cerrar_Busqueda_Contribuyentes_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:UpdatePanel ID="Upd_Panel_Busqueda_Contribuyentes" runat="server">
            <ContentTemplate>
                <div style="color: #5D7B9D">
                    <table width="100%">
                        <tr>
                            <td align="left" style="text-align: left;">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100%" colspan="4" align="right">
                                            <asp:ImageButton ID="Btn_Limpiar_Busqueda_Contribuyentes" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_clear.png"
                                                ToolTip="Limpiar Controles de Busqueda" OnClick="Btn_Limpiar_Busqueda_Contribuyentes_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left; font-size: 11px;">
                                            Contribuyente ID
                                        </td>
                                        <td style="width: 35%; text-align: left; font-size: 11px;">
                                            <asp:TextBox ID="Txt_Busqueda_Contribuyente_ID" runat="server" Width="98%" MaxLength="20" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Contribuyente_ID" runat="server"
                                                FilterType="Numbers, LowercaseLetters, UppercaseLetters" TargetControlID="Txt_Busqueda_Contribuyente_ID" />
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Contribuyente_ID" runat="server"
                                                TargetControlID="Txt_Busqueda_Contribuyente_ID" WatermarkText="Búsqueda por ID"
                                                WatermarkCssClass="watermarked" />
                                        </td>
                                        <td style="width: 15%; text-align: right; font-size: 11px;">
                                            <%--Estatus--%>
                                        </td>
                                        <td style="width: 35%; text-align: left;">
                                            <%--<asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="100%">
                                                <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>
                                                <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                                <asp:ListItem Value="OBSOLETO">OBSOLETO</asp:ListItem>
                                            </asp:DropDownList>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left; font-size: 11px;">
                                            Propietario
                                        </td>
                                        <td style="width: 85%; text-align: left;" colspan="3">
                                            <asp:TextBox ID="Txt_Busqueda_Propietatio" runat="server" Width="99.5%" MaxLength="80" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Propietatio" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom"
                                                ValidChars="ÑñáéíóúÁÉÍÓÚ " TargetControlID="Txt_Busqueda_Propietatio" />
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Propietatio" runat="server" TargetControlID="Txt_Busqueda_Propietatio"
                                                WatermarkText="Búsqueda por Nombre de Propietario o Copropietario" WatermarkCssClass="watermarked" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left; font-size: 11px;">
                                            RFC
                                        </td>
                                        <td style="width: 35%; text-align: left;">
                                            <asp:TextBox ID="Txt_Busqueda_RFC" runat="server" Width="99.5%" MaxLength="80" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_RFC" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom, Numbers"
                                                ValidChars="ÑñáéíóúÁÉÍÓÚ " TargetControlID="Txt_Busqueda_RFC" />
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_RFC" runat="server" TargetControlID="Txt_Busqueda_RFC"
                                                WatermarkText="Búsqueda por RFC" WatermarkCssClass="watermarked" />
                                        </td>
                                        <td style="width: 15%; text-align: right; font-size: 11px;">
                                            Ubicación
                                        </td>
                                        <td style="width: 35%; text-align: right;">
                                            <asp:TextBox ID="Txt_Busqueda_Domicilio" runat="server" Width="99.5%" MaxLength="80" />
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Domicilio" runat="server" TargetControlID="Txt_Busqueda_Domicilio"
                                                WatermarkText="Búsqueda por Domicilio" WatermarkCssClass="watermarked" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; text-align: left;" colspan="4">
                                            <center>
                                                <asp:Button ID="Btn_Busqueda_Contribuyentes" runat="server" Text="Buscar Contribuyente"
                                                    CssClass="button" CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Contribuyentes_Click" />
                                            </center>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="Grid_Contribuyentes" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                CssClass="GridView_1" GridLines="None" OnPageIndexChanging="Grid_Contribuyentes_PageIndexChanging"
                                                OnSelectedIndexChanged="Grid_Contribuyentes_SelectedIndexChanged" PageSize="5"
                                                Width="98%">
                                                <RowStyle CssClass="GridItem" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png"
                                                        Visible="false">
                                                        <ItemStyle Width="5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta_Predial">
                                                        <ItemStyle Width="65%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CONTRIBUYENTE_ID" HeaderText="Contribuyente ID">
                                                        <FooterStyle Font-Size="0pt" Width="0px" />
                                                        <HeaderStyle Font-Size="0pt" Width="0px" />
                                                        <ItemStyle Font-Size="0pt" Width="0px" />
                                                    </asp:BoundField>
                                                    <%--<asp:BoundField DataField="PROPIETARIO_ID" HeaderText="Propietario ID">
                                                        <FooterStyle Font-Size="0pt" Width="0px" />
                                                        <HeaderStyle Font-Size="0pt" Width="0px" />
                                                        <ItemStyle Font-Size="0pt" Width="0px" />
                                                    </asp:BoundField>--%>
                                                    <asp:BoundField DataField="NOMBRE_COMPLETO" HeaderText="Nombre">
                                                        <ItemStyle Width="65%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RFC" HeaderText="RFC">
                                                        <ItemStyle Width="65%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="UBICACION" HeaderText="UBICACION">
                                                        <ItemStyle Width="65%" />
                                                    </asp:BoundField>
                                                    <%--<asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                        <ItemStyle Width="5%" />
                                                    </asp:BoundField>--%>
                                                </Columns>
                                                <HeaderStyle CssClass="GridHeader" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                            </asp:GridView>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; text-align: left;" colspan="4">
                                            <center>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>

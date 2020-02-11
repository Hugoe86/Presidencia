<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Listado_Ordenes_Aceptadas.aspx.cs"
    Inherits="paginas_Predial_Frm_Ope_Pre_Listado_Ordenes_Aceptadas" %>

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
                        <td colspan="4" class="label_titulo">
                            Listado de órdenes de variación aceptadas
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
                        <td colspan="2">
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" />
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            <table style="width: 100%; height: 28px;">
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                        B&uacute;squeda:
                                                    </td>
                                                    <td style="width: 55%;">
                                                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar"
                                                            Width="180px" />
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. " />
                                                    </td>
                                                    <td style="vertical-align: middle; width: 5%;">
                                                        <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                            ToolTip="Buscar" />
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
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Aceptadas el día:
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox55" runat="server" Width="84%" TabIndex="12" MaxLength="11"
                                Height="18px" />
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="TextBox55"
                                WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Inicial" runat="server" TargetControlID="TextBox55"
                                Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Txt_Fecha_Inicial" />
                            <asp:ImageButton ID="Btn_Txt_Fecha_Inicial" runat="server" ImageUrl="../imagenes/paginas/SmallCalendar.gif"
                                Style="vertical-align: top;" Height="18px" CausesValidation="false" />
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <%------------------ Cuenta ------------------%>
                    <tr style="background-color: #36C;">
                        <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                            Ordenes de variación aceptadas
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid" runat="server" AllowPaging="True" CssClass="GridView_1" AutoGenerateColumns="False"
                                PageSize="4" Width="100%" AllowSorting="True" HeaderStyle-CssClass="tblHead"
                                Style="white-space: normal;">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="ORDEN" HeaderText="No. Orden" Visible="True" SortExpression="Area_ID">
                                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                        <ItemStyle HorizontalAlign="Left" Width="12%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="REALIZO" HeaderText="Realizó" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TIPO" HeaderText="Clave movimiento" Visible="True">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" Width="13%" />
                                        <ItemStyle HorizontalAlign="Left" Width="13%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha" Visible="True">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CUENTA" HeaderText="Cuenta" Visible="True">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="c2" HeaderText="No. Nota" Visible="True" SortExpression="Area_ID">
                                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                        <ItemStyle HorizontalAlign="Left" Width="12%" />
                                    </asp:BoundField>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/grid_print.png">
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:ButtonField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Tipo de movimiento
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox19" runat="server" Width="96.4%" ReadOnly="True" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Estatus
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox54" runat="server" ReadOnly="True" Width="96.4%" />
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
                            Cuenta predial
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox16" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Cuenta origen
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox42" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Tipo predio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox40" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Uso predio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox38" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Estado de predio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox1" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Estatus
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox9" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Supe. construida (m&sup2;)
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox2" runat="server" Width="96.4%" Text="0" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Superficie Total (m&sup2;)
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox3" runat="server" Width="96.4%" Text="0" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Ubicación
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox4" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Colonia
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox5" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Número exterior
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox6" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Número interior
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox8" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Clave catastral
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox7" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Efectos
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox39" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Ultimo movimiento
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox41" runat="server" Width="96.4%" />
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
                    <%------------------ Propietario ------------------%>
                    <tr style="background-color: #36C;">
                        <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                            Propietario
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Nombre
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox10" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Propietario/Poseedor
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox49" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            RFC
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox62" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Colonia
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox63" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Calle
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox14" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Número exterior
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox43" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Número interior
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox12" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Estado
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox13" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Ciudad
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox44" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            C.P.
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox45" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Copropietarios
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="TextBox11" runat="server" TabIndex="10" MaxLength="250" TextMode="MultiLine"
                                Width="98.6%" AutoPostBack="True" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <%------------------ Impuestos ------------------%>
                    <tr style="background-color: #36C;">
                        <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                            Impuestos
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Valor fiscal
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox15" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Tasa
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox46" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Periodo corriente
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox22" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Tipo predio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox47" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Cuota anual
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox23" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Cuota bimestral
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox17" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Dif. de construcción
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox24" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            % Exención
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox36" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Término exención
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="96.4%" TabIndex="12" MaxLength="11"
                                Height="18px" />
                            <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Inicial" runat="server" TargetControlID="Txt_Fecha_Inicial"
                                WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Fecha avalúo
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox37" runat="server" Width="96.4%" TabIndex="12" MaxLength="11"
                                Height="18px" />
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="TextBox37"
                                WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Cuota fija
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:CheckBox ID="CheckBox2" runat="server" Text="" />
                            <asp:TextBox ID="TextBox20" runat="server" Width="85%" />
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <%------------------ Detalles cuota fija ------------------%>
                    <tr>
                        <td style="text-align: left; font-size: 14px; border-bottom: 1px solid #3366CC;"
                            colspan="4">
                            Detalles Cuota fija
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Cuota fija por:
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox48" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Plazo financiamiento
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox18" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;" colspan="3">
                            Cuota mínima
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox21" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;" colspan="3">
                            Excedente de construcción
                            <asp:TextBox ID="TextBox25" runat="server" TabIndex="10" MaxLength="10" TextMode="SingleLine"
                                Width="50px" />
                            x
                            <asp:TextBox ID="TextBox26" runat="server" TabIndex="10" MaxLength="10" TextMode="SingleLine"
                                Width="50px" />
                            =
                            <asp:TextBox ID="TextBox27" runat="server" ReadOnly="True" Width="50px" />
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox28" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;" colspan="3">
                            Excedente de valor
                            <asp:TextBox ID="TextBox29" runat="server" TabIndex="10" MaxLength="10" TextMode="SingleLine"
                                Width="50px" />
                            x
                            <asp:TextBox ID="TextBox30" runat="server" TabIndex="10" MaxLength="10" TextMode="SingleLine"
                                Width="50px" />=
                            <asp:TextBox ID="TextBox31" runat="server" ReadOnly="True" Width="50px" />
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox32" runat="server" ReadOnly="True" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; text-align: right;" colspan="3">
                            Total impuesto
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox33" runat="server" Enabled="false" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Fundamento legal
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="TextBox34" runat="server" TabIndex="10" MaxLength="250" TextMode="SingleLine"
                                Width="98.6%" AutoPostBack="True" />
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="TextBox10"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <%------------------ Diferencias ------------------%>
                    <tr style="background-color: #36C;">
                        <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                            Diferencias
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="5" Width="100%" HeaderStyle-CssClass="tblHead"
                                Style="white-space: normal;">
                                <Columns>
                                    <asp:BoundField DataField="c2" HeaderText="Periodo" />
                                    <asp:BoundField DataField="c2" HeaderText="Valor fiscal" HeaderStyle-Width="15%" />
                                    <asp:BoundField DataField="c2" HeaderText="Tasa" HeaderStyle-Width="12%" />
                                    <asp:BoundField DataField="c2" HeaderText="Alta/Baja" HeaderStyle-Width="12%" />
                                    <asp:BoundField DataField="c2" HeaderText="Importe" />
                                    <asp:BoundField DataField="c2" HeaderText="Cuota bimestral" HeaderStyle-Width="15%">
                                        <HeaderStyle Width="15%" />
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
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Vista previa de adeudos
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:ImageButton ID="ImageButton10" runat="server" ToolTip="Agregar" TabIndex="10"
                                ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Height="22px" Width="22px"
                                Style="float: left" />
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
                            <asp:TextBox ID="TextBox50" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Hasta periodo
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox51" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Alta
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox52" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Baja
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox53" runat="server" Width="96.4%" />
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
                            <asp:TextBox ID="TextBox59" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Hasta periodo
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox56" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Alta
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox57" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Baja
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="TextBox58" runat="server" Width="96.4%" />
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
                            Observaciones de la cuenta
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Observaciones de la cuenta
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="TextBox35" runat="server" TabIndex="10" MaxLength="250" TextMode="MultiLine"
                                Width="98.6%" AutoPostBack="True" />
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="TextBox35"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
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
                </table>
                <table width="94%" border="1" cellspacing="0" class="Tabla_Comentarios">
                    <tr>
                        <th style="width: 25%;">
                            30/May/2011 12:00 PM
                        </th>
                        <th>
                            Autor observación
                        </th>
                    </tr>
                    <tr align="center">
                        <td style="text-align: justify;" colspan="2">
                            Sed porttitor arcu vitae ipsum fringilla et fringilla sapien faucibus. Proin iaculis
                            congue mauris sit amet egestas. Mauris mattis gravida elit, ut dignissim sem porttitor
                            eu.
                        </td>
                    </tr>
                </table>
                <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

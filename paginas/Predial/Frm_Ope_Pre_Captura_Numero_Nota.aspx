<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Captura_Numero_Nota.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Captura_Numero_Nota" %>

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

        //Abrir una ventana modal
		function Abrir_Ventana_Modal(Url, Propiedades)
		{
			window.showModalDialog(Url, null, Propiedades);
		}

        function Abrir_Ventana_Emergente(Url, Nombre_Ventana, Propiedades)
        {
            window.open(Url, Nombre_Ventana, Propiedades);
        }

        function Validar_Longitud_Texto(Text_Box, Max_Longitud)
        {
            if (Text_Box.value.length > Max_Longitud)
            {
                Text_Box.value = Text_Box.value.substring(0, Max_Longitud);
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"
        AsyncPostBackTimeout="3600">
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
            <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Captura de Número de Nota
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
                                        <td align="left" style="width: 40%;">
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td style="vertical-align: middle; text-align: right; width: 35%;">
                                            Búsqueda:
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar"
                                                Width="180px" AutoPostBack="true" OnTextChanged="Txt_Busqueda_TextChanged" />
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                WatermarkText="&lt;No. de Movimiento&gt;" TargetControlID="Txt_Busqueda" />
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                                FilterType="Numbers, LowercaseLetters, UppercaseLetters" />
                                        </td>
                                        <td style="text-align: left; vertical-align: middle; width: 10%; margin-left: 35%;">
                                            <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                ToolTip="Buscar" OnClick="Btn_Buscar_Click" />
                                            <asp:HiddenField ID="Hdn_Cuenta_Predial" runat="server" />
                                            <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Cuenta_Predial" runat="server"
                                                Height="24px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" OnClick="Btn_Mostrar_Busqueda_Avanzada_Cuenta_Predial_Click"
                                                TabIndex="10" ToolTip="Búsqueda Avanzada de Cuenta Predial" Width="24px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 40%;">
                                            &nbsp;
                                        </td>
                                        <td style="vertical-align: middle; text-align: right; width: 40%;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%;">
                                            &nbsp;
                                        </td>
                                        <td style="vertical-align: middle; width: 10%; margin-left: 30%;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel_Datos_Generales" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <tr style="background-color: #3366CC">
                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                Datos para Captura
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 25%">
                                Cuenta Predial
                            </td>
                            <td style="text-align: left; width: 25%">
                                <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="98%"></asp:TextBox>
                            </td>
                            <td style="text-align: right; width: 25%">
                                Año del Movimiento
                            </td>
                            <td style="text-align: left; width: 25%">
                                <asp:Label ID="Lbl_Año_Movimiento" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 25%">
                                No. Movimiento
                            </td>
                            <td style="text-align: left; width: 25%">
                                <asp:TextBox ID="Txt_No_Movimiento" runat="server" Width="98%"></asp:TextBox>
                            </td>
                            <td style="text-align: right; width: 25%">
                                Tipo Movimiento
                            </td>
                            <td style="text-align: left; width: 25%">
                                <asp:TextBox ID="Txt_Tipo_Movimiento" runat="server" Width="98%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 25%">
                                Grupo Movimiento
                            </td>
                            <td style="text-align: left; width: 25%">
                                <asp:DropDownList ID="Cmb_Grupos_Movimientos" runat="server" Width="98%" AutoPostBack="True"
                                    OnSelectedIndexChanged="Cmb_Grupos_Movimientos_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right; width: 25%">
                                Tipo Predio
                            </td>
                            <td style="text-align: left; width: 25%">
                                <asp:TextBox ID="Txt_Tipo_Predio" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 25%">
                                No. Nota
                            </td>
                            <td style="text-align: left; width: 25%">
                                <asp:TextBox ID="Txt_No_Nota" runat="server" Width="98%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Ftb_Txt_No_Nota" runat="server" Enabled="True" FilterType="Custom, Numbers"
                                    TargetControlID="Txt_No_Nota" ValidChars="0123456789">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="text-align: right; width: 25%">
                                Último No. Nota del Grupo
                            </td>
                            <td style="text-align: left; width: 25%">
                                <asp:Label ID="Lbl_Ultimo_No_Nota_Grupo" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <asp:Panel ID="Panel_Resumen" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <tr style="background-color: #3366CC">
                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                Órdenes de Variación Encontradas
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:GridView ID="Grid_Ordenes_Variacion" runat="server" AutoGenerateColumns="False"
                                    CssClass="GridView_Nested" HeaderStyle-CssClass="tblHead" Style="white-space: normal;"
                                    Width="100%" AllowPaging="True" OnPageIndexChanging="Grid_Ordenes_Variacion_PageIndexChanging"
                                    OnSelectedIndexChanged="Grid_Ordenes_Variacion_SelectedIndexChanged" DataKeyNames="CUENTA_PREDIAL_ID,MOVIMIENTO_ID,TIPO_PREDIO_ID,GRUPO_MOVIMIENTO_ID,ANIO,GRUPO_MOVIMIENTO,TIPO_PREDIO,FECHA_NOTA,FECHA_VALIDO,USUARIO_VALIDO">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png"
                                            Text="Button">
                                            <HeaderStyle Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="CUENTA_PREDIAL_ID" Visible="False">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MOVIMIENTO_ID" HeaderText="MOVIMIENTO_ID" Visible="False">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TIPO_PREDIO_ID" HeaderText="TIPO_PREDIO_ID" Visible="False">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GRUPO_MOVIMIENTO_ID" HeaderText="GRUPO_MOVIMIENTO_ID"
                                            Visible="False">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ANIO" HeaderText="ANIO" Visible="False">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NO_ORDEN_VARIACION" HeaderText="No. Movimiento">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MOVIMIENTO" HeaderText="Tipo Movimiento">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GRUPO_MOVIMIENTO" HeaderText="Grupo Movimiento" Visible="False">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TIPO_PREDIO" HeaderText="Tipo Predio" Visible="False">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NO_NOTA" HeaderText="No. Nota">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA_NOTA" HeaderText="FECHA_NOTA" DataFormatString="{0:dd/MMM/yyyy}"
                                            Visible="False">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="USUARIO_CREO" HeaderText="Elaboró">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA_VALIDO" HeaderText="FECHA_VALIDO" DataFormatString="{0:dd/MMM/yyyy}"
                                            Visible="False">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="USUARIO_VALIDO" HeaderText="USUARIO_VALIDO" Visible="False">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS_ORDEN" HeaderText="Estatus">
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
                <br />
                <asp:HiddenField ID="Hdn_Cuenta_Predial_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Tipo_Predio_ID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

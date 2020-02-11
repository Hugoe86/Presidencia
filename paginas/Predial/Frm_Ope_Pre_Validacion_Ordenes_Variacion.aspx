<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Validacion_Ordenes_Variacion.aspx.cs"
    Inherits="paginas_Predial_Frm_Ope_Pre_Validacion_Ordenes_Variacion" %>

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

        function Mismo_Domicilio()
        {
            var Combo;
            var Texto_Combo; 
            var Origen1 = document.getElementById('<%= Chk_Mismo_Domicilio.ClientID %>').checked;
            if(Origen1)
            {
                document.getElementById('<%= Txt_Colonia_Propietario.ClientID %>').disabled = true;
                document.getElementById('<%= Txt_Calle_Propietario.ClientID %>').disabled = true;
                document.getElementById('<%= Txt_Numero_Exterior_Propietario.ClientID %>').disabled = true;
                document.getElementById('<%= Txt_Numero_Interior_Propietario.ClientID %>').disabled = true;
                document.getElementById('<%= Txt_Estado_Propietario.ClientID %>').disabled = true;
                document.getElementById('<%= Txt_Ciudad_Propietario.ClientID %>').disabled = true;
                document.getElementById('<%= Txt_Domicilio_Foraneo.ClientID %>').disabled = true;
                document.getElementById('<%= Txt_Domicilio_Foraneo.ClientID %>').value = 'NO';

                document.getElementById('<%= Txt_Calle_Propietario.ClientID %>').value = document.getElementById("<%=Txt_Calle_Cuenta.ClientID%>").value;
                document.getElementById('<%= Txt_Colonia_Propietario.ClientID %>').value = document.getElementById("<%=Txt_Colonia_Cuenta.ClientID%>").value;
                document.getElementById('<%= Txt_Numero_Exterior_Propietario.ClientID %>').value = document.getElementById('<%= Txt_No_Exterior.ClientID %>').value;
                document.getElementById('<%= Txt_Numero_Interior_Propietario.ClientID %>').value = document.getElementById('<%= Txt_No_Interior.ClientID %>').value;

                document.getElementById('<%= Txt_Estado_Propietario.ClientID %>').value = '';
                document.getElementById('<%= Txt_Ciudad_Propietario.ClientID %>').value = '';
            }
            else
            {
                document.getElementById('<%= Txt_Colonia_Propietario.ClientID %>').disabled = false;
                document.getElementById('<%= Txt_Calle_Propietario.ClientID %>').disabled = false;
                document.getElementById('<%= Txt_Numero_Exterior_Propietario.ClientID %>').disabled = false;
                document.getElementById('<%= Txt_Numero_Interior_Propietario.ClientID %>').disabled = false;                    
                document.getElementById('<%= Txt_Domicilio_Foraneo.ClientID %>').disabled = false;

                document.getElementById('<%= Txt_Colonia_Propietario.ClientID %>').value = '';
                document.getElementById('<%= Txt_Calle_Propietario.ClientID %>').value = '';
                document.getElementById('<%= Txt_Numero_Exterior_Propietario.ClientID %>').value = '';
                document.getElementById('<%= Txt_Numero_Interior_Propietario.ClientID %>').value = '';
                document.getElementById('<%= Txt_Estado_Propietario.ClientID %>').value = '';
                document.getElementById('<%= Txt_Ciudad_Propietario.ClientID %>').value = '';
            }
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
                            Validar Órdenes de Variación
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
                                        <td align="left" style="width: 35%;">
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click" />
                                            <%--<asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                                                TabIndex="4" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" />--%>
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            <table style="width: 100%; height: 28px;">
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                        Búsqueda:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="Cmb_Busqueda_Estatus_Ordenes" runat="server">
                                                            <asp:ListItem>POR VALIDAR</asp:ListItem>
                                                            <asp:ListItem>ACEPTADA</asp:ListItem>
                                                            <asp:ListItem>RECHAZADA</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 20%;">
                                                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar"
                                                            Width="180px" AutoPostBack="true" OnTextChanged="Txt_Contrarecibo_TextChanged" />
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="&lt;No. de Movimiento&gt;" TargetControlID="Txt_Busqueda" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                                            FilterType="Numbers, LowercaseLetters, UppercaseLetters" />
                                                    </td>
                                                    <td style="vertical-align: middle; width: 20%;">
                                                        <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                            ToolTip="Buscar" OnClick="Btn_Buscar_Click" />
                                                        <asp:HiddenField ID="Hdn_Cuenta_Predial" runat="server" />
                                                        <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Cuenta_Predial" runat="server"
                                                            Height="24px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" OnClick="Btn_Mostrar_Busqueda_Avanzada_Cuenta_Predial_Click"
                                                            TabIndex="10" ToolTip="Búsqueda Avanzada de Cuenta Predial" Width="24px" />
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
                <asp:Panel ID="Panel_Resumen" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <tr style="background-color: #3366CC">
                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                Órdenes de Variación por Validar
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:GridView ID="Grid_Ordenes_Variacion" runat="server" AutoGenerateColumns="False"
                                    CssClass="GridView_Nested" HeaderStyle-CssClass="tblHead" Style="white-space: normal;"
                                    Width="100%" AllowPaging="True" OnPageIndexChanging="Grid_Ordenes_Variacion_PageIndexChanging"
                                    OnSelectedIndexChanged="Grid_Ordenes_Variacion_SelectedIndexChanged" DataKeyNames="NO_CONTRARECIBO,CUENTA_PREDIAL_ID">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png"
                                            Text="Button">
                                            <HeaderStyle Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="NO_ORDEN_VARIACION" HeaderText="No. Movimiento">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS_ORDEN" HeaderText="Estatus">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="USUARIO_CREO" HeaderText="Realizó">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Identificador_Movimiento" HeaderText="Clave Movimiento">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA_CREO" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NO_CONTRARECIBO" HeaderText="NO_CONTRARECIBO" Visible="False" />
                                        <asp:BoundField DataField="MOVIMIENTO_ID" HeaderText="Movimiento_ID" Visible="False">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="CUENTA_PREDIAL_ID" Visible="False">
                                            <HeaderStyle HorizontalAlign="Center" />
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
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="Label1" runat="server" Text="Total Movimientos" Font-Bold="True"></asp:Label>
                                <asp:TextBox ID="Txt_Total_Movimientos" runat="server" Font-Bold="True" Width="100px"
                                    ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel_Detalle" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Tipo_Movimiento" runat="server" Text="Tipo de movimiento"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 80%;">
                                <asp:TextBox ID="Txt_Tipo_Movimiento" runat="server" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <%------------------ Datos generales ------------------%>
                    <table width="98%" class="estilo_fuente">
                        <tr style="background-color: #3366CC">
                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                Generales
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Cuenta_Predial" runat="server" Text="Cuenta Predial"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:HiddenField ID="Hdn_Cuenta_ID" runat="server" />
                                <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Enable="false" MaxLength="20"
                                    TabIndex="9" Width="86%"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Resumen_Cuenta" runat="server" Height="22px" ImageAlign="AbsMiddle"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Width="22px" ToolTip="Resumen de Cuenta" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Cuenta_Origen" runat="server" Text="Label">Cuenta origen</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Cuenta_Origen" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Tipo_Predio" runat="server" Text="Label">Tipo predio</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Tipo_Predio" runat="server" Width="96.4%" />
                                <asp:HiddenField ID="Hdn_Tipo_Predio_ID" runat="server" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Uso_Predio" runat="server" Text="Label">Uso predio</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Uso_Predio" runat="server" Width="96.4%" />
                                <asp:HiddenField ID="Hdn_Uso_Predio_ID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Estado_Predio" runat="server" Text="Estado de predio"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Estado_Predio" runat="server" Width="96.4%" />
                                <asp:HiddenField ID="Hdn_Estado_Predio_ID" runat="server" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Estatus" runat="server" Text="Label">Estatus</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Estatus" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Superficie_Construida" runat="server" Text="Label">Superficie construida (m²)</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Superficie_Construida" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Superficie_Total" runat="server" Text="Superficie Total (m²)"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Superficie_Total" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Calle_Cuenta" runat="server" Text="Calle"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Calle_Cuenta" runat="server" Width="96.4%" />
                                <asp:HiddenField ID="Hdn_Calle_ID" runat="server" />
                                <asp:HiddenField ID="Hdn_Ciudad_Cuenta" runat="server" />
                            </td>
                            <td style="text-align: right; width: 20%;">
                                <asp:Label ID="Lbl_Colonia_Cuenta" runat="server" Text="Colonia"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Colonia_Cuenta" runat="server" Width="96.4%" />
                                <asp:HiddenField ID="Hdn_Colonia_ID" runat="server" />
                                <asp:HiddenField ID="Hdn_Estado_Cuenta" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_No_Exterior" runat="server" Text="Número exterior"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;" colspan="3">
                                <asp:TextBox ID="Txt_No_Exterior" runat="server" Width="98.6%" MaxLength="80" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_No_Interior" runat="server" Text="Número interior"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;" colspan="3">
                                <asp:TextBox ID="Txt_No_Interior" runat="server" Width="98.6%" MaxLength="80" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Catastral" runat="server" Text="Label">Clave catastral</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Catastral" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Efectos" runat="server" Text="Label">Efectos</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Efectos" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Ultimo_Movimiento" runat="server" Text="Label">Último 
                            movimiento</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;" colspan="3">
                                <asp:TextBox ID="Txt_Ultimo_Movimiento" runat="server" Width="98.6%" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <%------------------ Propietario ------------------%>
                    </table>
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
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Tipo_Propietario" runat="server" Text="Propietario/Poseedor"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Tipo_Propietario" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Mismo_Domicilio" runat="server" Text="Label">Mismo domicilio</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:CheckBox ID="Chk_Mismo_Domicilio" runat="server" Checked="true" Text="" onclick="javascript:Mismo_Domicilio();" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Domicilio_Foraneo" runat="server" Text="Label">Domicilio foráneo</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Domicilio_Foraneo" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Calle_Propietario" runat="server" Text="Calle"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:HiddenField ID="Hdn_Calle_ID_Notificacion" runat="server" />
                                <asp:HiddenField ID="Hdn_Calle_Notificacion_Anterior" runat="server" />
                                <asp:HiddenField ID="Hdn_Calle_Notificacion_Nuevo" runat="server" />
                                <asp:TextBox ID="Txt_Calle_Propietario" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: right; width: 20%;">
                                <asp:Label ID="Lbl_Colonia_Propietario" runat="server" Text="Colonia"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:HiddenField ID="Hdn_Colonia_ID_Notificacion" runat="server" />
                                <asp:HiddenField ID="Hdn_Colonia_Notificacion_Anterior" runat="server" />
                                <asp:HiddenField ID="Hdn_Colonia_Notificacion_Nuevo" runat="server" />
                                <asp:TextBox ID="Txt_Colonia_Propietario" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Numero_Exterior_Propietario" runat="server" Text="Label">Número exterior</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;" colspan="3">
                                <asp:HiddenField ID="Hdn_No_Exterior_Propietario_Anterior" runat="server" />
                                <asp:HiddenField ID="Hdn_No_Exterior_Propietario_Nuevo" runat="server" />
                                <asp:TextBox ID="Txt_Numero_Exterior_Propietario" runat="server" Width="98.6%" MaxLength="80" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Numero_Interior_Propietario" runat="server" Text="Label">Número interior</asp:Label>
                            </td>
                            <td style="width: 30%;" colspan="3">
                                <asp:HiddenField ID="Hdn_No_Interior_Propietario_Anterior" runat="server" />
                                <asp:HiddenField ID="Hdn_No_Interior_Propietario_Nuevo" runat="server" />
                                <asp:TextBox ID="Txt_Numero_Interior_Propietario" runat="server" Width="98.6%" MaxLength="80" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; text-align: left;">
                                <asp:Label ID="Lbl_Ciudad_Propietario" runat="server" Text="Ciudad"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:HiddenField ID="Hdn_Ciudad_ID_Notificacion" runat="server" />
                                <asp:HiddenField ID="Hdn_Ciudad_Notificacion_Anterior" runat="server" />
                                <asp:HiddenField ID="Hdn_Ciudad_Notificacion_Nuevo" runat="server" />
                                <asp:TextBox ID="Txt_Ciudad_Propietario" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: right; width: 20%;">
                                <asp:Label ID="Lbl_Estado_Propietario" runat="server" Text="Estado"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:HiddenField ID="Hdn_Estado_ID_Notificacion" runat="server" />
                                <asp:HiddenField ID="Hdn_Estado_Notificacion_Anterior" runat="server" />
                                <asp:HiddenField ID="Hdn_Estado_Notificacion_Nuevo" runat="server" />
                                <asp:TextBox ID="Txt_Estado_Propietario" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_CP" runat="server" Text="Label">C.P.</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_CP" runat="server" Width="96.4%" />
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
                        <%------------------ Impuestos ------------------%>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Impuestos
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Valor_Fiscal" runat="server" Text="Label">Valor fiscal</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Valor_Fiscal" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Costo_M2" runat="server" Text="Label">Costo (m²)</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Costo_M2" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Tasa_Descripcion" runat="server" Text="Tasa"></asp:Label>
                            </td>
                            <td style="text-align: left;" colspan="3">
                                <asp:TextBox ID="Txt_Tasa_Descripcion" runat="server" Width="85%" Enabled="false" />
                                &nbsp;
                                <asp:TextBox ID="Txt_Tasa_Porcentaje" runat="server" Width="75px" Enabled="false"></asp:TextBox>
                                <asp:HiddenField ID="Hdn_Tasa_Predial_ID" runat="server" />
                                <asp:HiddenField ID="Hdn_Tasa_ID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Periodo_Corriente" runat="server" Text="Label">Periodo corriente</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Periodo_Corriente" runat="server" Width="96.4%" />
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
                                <asp:Label ID="Lbl_Cuota_Anual" runat="server" Text="Label">Cuota anual</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Cuota_Anual" runat="server" Width="96.4%" Enabled="false" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Cuota_Bimestral" runat="server" Text="Label">Cuota bimestral</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Cuota_Bimestral" runat="server" Width="96.4%" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Diferencia_Construccion" runat="server" Text="Dif. de construcción"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Diferencia_Construccion" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Porcentaje_Exencion" runat="server" Text="% Exención"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Porcentaje_Exencion" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Termino_Exencion" runat="server" Text="Label">Término exención</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Termino_Exencion" runat="server" Width="96.4%" TabIndex="12"
                                    MaxLength="11" ReadOnly="true" />
                                <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Termino_Exencion" runat="server" TargetControlID="Txt_Termino_Exencion"
                                    WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                <cc1:CalendarExtender ID="CE_Txt_Termino_Exencion" runat="server" TargetControlID="Txt_Termino_Exencion"
                                    Format="dd/MMM/yyyy" Enabled="False" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Fecha_Avaluo" runat="server" Text="Label">Fecha avalúo</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Avaluo" runat="server" Width="96.4%" TabIndex="12" MaxLength="11"
                                    ReadOnly="true" />
                                <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Avaluo" runat="server" TargetControlID="Txt_Fecha_Avaluo"
                                    WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Avaluo" runat="server" TargetControlID="Txt_Fecha_Avaluo"
                                    Format="dd/MMM/yyyy" Enabled="False" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Cuota_Fija" runat="server" Text="Label">Cuota fija</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:CheckBox ID="Chk_Cuota_Fija" runat="server" Text="" AutoPostBack="True" OnCheckedChanged="Chk_Cuota_Fija_CheckedChanged" />
                                <asp:TextBox ID="Txt_Cuota_Fija" runat="server" Width="85%" Visible="false" />
                            </td>
                            <td colspan="2">
                                &nbsp;
                                <asp:HiddenField ID="Hdn_No_Cuota_Fija_Nuevo" runat="server" />
                                <asp:HiddenField ID="Hdn_No_Cuota_Fija_Anterior" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <div id="Div_Detalles_Cuota_Fija" runat="server">
                        <table width="98%" class="estilo_fuente">
                            <%------------------ Detalles cuota fija ------------------%>
                            <tr>
                                <td style="text-align: left; font-size: 14px; border-bottom: 1px solid #3366CC;"
                                    colspan="4">
                                    Detalles Cuota fija
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    <asp:Label ID="Lbl_Solicitante" runat="server" Text="Label">El solicitante es:</asp:Label>
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:HiddenField ID="Hdn_Caso_Especial_ID" runat="server" />
                                    <asp:TextBox ID="Txt_Solicitante" runat="server" Width="96.4%" />
                                </td>
                                <td style="text-align: left; width: 20%; text-align: right;">
                                    <asp:Label ID="Lbl_Inmueble" runat="server" Text="Label">o inmueble financiado:</asp:Label>
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Inmueble" runat="server" Width="96.4%" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    <asp:Label ID="Lbl_Plazo_Financiado" runat="server" Text="Plazo financiamiento"></asp:Label>
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Plazo_Financiamiento" runat="server" Width="96.4%" />
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="Txt_Plazo_Financiamiento"
                                        FilterType="Numbers" />
                                </td>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    <asp:Label ID="Lbl_Cuota_Minima_Anual" runat="server" Text="Cuota mínima Anual"></asp:Label>
                                </td>
                                <td style="text-align: left; width: 20%;">
                                    <asp:TextBox ID="Txt_Cuota_Minima_Anual" runat="server" Enabled="false" Width="96.4%" />
                                    <asp:HiddenField ID="Hdn_Cuota_Minima_ID" runat="server" />
                                </td>
                                <td style="text-align: left; width: 20%;">
                                    Cuota Mínima a Aplicar
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Cuota_Minima_Aplicar" runat="server" Enabled="false" Width="96.4%" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    <asp:Label ID="Lbl_Exedente_Construccion" runat="server" Text="Excedente de construcción"></asp:Label>
                                </td>
                                <td style="text-align: left; width: 20%;">
                                    <asp:TextBox ID="Txt_Excedente_Construccion" runat="server" TabIndex="10" MaxLength="10"
                                        TextMode="SingleLine" Width="60%" />
                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="Txt_Excedente_Construccion"
                                        WatermarkText="m&sup2;" WatermarkCssClass="watermarked" />
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="Txt_Excedente_Construccion"
                                        FilterType="Numbers,Custom" ValidChars="." />
                                    x
                                    <asp:TextBox ID="Txt_Tasa_Excedente_Construccion" runat="server" TabIndex="10" MaxLength="10"
                                        TextMode="SingleLine" Width="50px" Enabled="false" />
                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="Txt_Tasa_Excedente_Construccion"
                                        WatermarkText="tasa" WatermarkCssClass="watermarked" />
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="Txt_Tasa_Excedente_Construccion"
                                        FilterType="Numbers,Custom" ValidChars="." />
                                    =
                                </td>
                                <td style="text-align: left; width: 20%;">
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Excedente_Construccion_Total" runat="server" ReadOnly="True"
                                        Width="96.4%" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    <asp:Label ID="Lbl_Excedente_Valor" runat="server" Text="Label">Excedente de valor</asp:Label>
                                </td>
                                <td style="text-align: left; width: 20%;">
                                    <asp:TextBox ID="Txt_Excedente_Valor" runat="server" TabIndex="10" MaxLength="10"
                                        TextMode="SingleLine" Width="60%" Enabled="false" />
                                    x
                                    <asp:TextBox ID="Txt_Tasa_Excedente_Valor" runat="server" TabIndex="10" MaxLength="10"
                                        TextMode="SingleLine" Width="50px" Enabled="false" />
                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="Txt_Tasa_Excedente_Valor"
                                        WatermarkText="tasa" WatermarkCssClass="watermarked" />
                                    =
                                </td>
                                <td style="text-align: left; width: 20%;">
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Excedente_Valor_Total" runat="server" ReadOnly="True" Width="96.4%" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                </td>
                                <td style="text-align: left; width: 20%; text-align: right;">
                                </td>
                                <td style="text-align: left; width: 20%; text-align: right;">
                                    <asp:Label ID="Lbl_Total_Cuota_Fija" runat="server" Text="Label">Total cuota fija</asp:Label>
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:TextBox ID="Txt_Total_Cuota_Fija" runat="server" ReadOnly="true" Enabled="false"
                                        Width="96.4%" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%; vertical-align: top;">
                                    <asp:Label ID="Lbl_Fundamento_Legal" runat="server" Text="Label">Fundamento legal</asp:Label>
                                </td>
                                <td colspan="3" style="text-align: left; width: 80%;">
                                    <asp:TextBox ID="Txt_Fundamento_Legal" runat="server" TabIndex="10" MaxLength="250"
                                        TextMode="SingleLine" Width="98.6%" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <%------------------ Copropietarios ------------------%>
                    <table width="98%" class="estilo_fuente">
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Copropietarios
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView runat="server" AllowPaging="True" AutoGenerateColumns="False" CssClass="GridView_1"
                                    Width="100%" ID="Grid_Copropietarios" PageSize="5" HeaderStyle-CssClass="tblHead"
                                    OnPageIndexChanging="Grid_Copropietarios_PageIndexChanging" OnRowDataBound="Grid_Copropietarios_RowDataBound"
                                    DataKeyNames="CONTRIBUYENTE_ID,ESTATUS_VARIACION">
                                    <AlternatingRowStyle CssClass="GridAltItem"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:BoundField DataField="CONTRIBUYENTE_ID" HeaderText="Contribuyente_ID" SortExpression="CONTRIBUYENTE_ID"
                                            Visible="False">
                                            <HeaderStyle Width="20%" HorizontalAlign="Center" />
                                            <ItemStyle Width="20%" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RFC" HeaderText="RFC">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_CONTRIBUYENTE" HeaderText="Nombre" SortExpression="NOMBRE">
                                            <HeaderStyle Width="75%" HorizontalAlign="Left" />
                                            <ItemStyle Width="75%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS_VARIACION" HeaderText="ESTATUS_VARIACION" Visible="False" />
                                    </Columns>
                                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                    <PagerStyle CssClass="GridHeader"></PagerStyle>
                                    <RowStyle CssClass="GridItem"></RowStyle>
                                    <SelectedRowStyle CssClass="GridSelected"></SelectedRowStyle>
                                </asp:GridView>
                            </td>
                        </tr>
                        <%------------------ Diferencias ------------------%>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Diferencias
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="4">
                                <asp:GridView ID="Grid_Diferencias" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                                    PageSize="5" Width="100%" HeaderStyle-CssClass="tblHead" Style="white-space: normal;"
                                    DataKeyNames="TIPO_PERIODO,TASA_ID" OnPageIndexChanging="Grid_Diferencias_PageIndexChanging"
                                    OnRowDataBound="Grid_Diferencias_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="PERIODO" HeaderText="Periodo" SortExpression="PERIODO">
                                            <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TIPO_PERIODO" HeaderText="Tipo Periodo" Visible="False">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VALOR_FISCAL" HeaderText="Valor Fiscal" DataFormatString="{0:c2}"
                                            SortExpression="VALOR_FISCAL">
                                            <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                            <ItemStyle HorizontalAlign="Right" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TASA_ID" HeaderText="TASA_ID" Visible="False"></asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPCION_TASA" HeaderText="Descripción Tasa">
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TASA" HeaderText="Tasa" SortExpression="TASA">
                                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                            <ItemStyle Width="5%" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TIPO" HeaderText="Alta/Baja" SortExpression="TIPO">
                                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Importe" DataFormatString="{0:c2}" DataField="IMPORTE"
                                            SortExpression="IMPORTE">
                                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataFormatString="{0:c2}" HeaderText="Cuota Bimestral" DataField="CUOTA_BIMESTRAL">
                                            <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                            <ItemStyle HorizontalAlign="Right" Width="15%" />
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
                                <asp:ImageButton ID="Btn_Vista_Adeudos" runat="server" ToolTip="Agregar" TabIndex="10"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Height="22px" Width="22px"
                                    Style="float: left" OnClick="Btn_Vista_Adeudos_Click" />
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
                                <asp:Label ID="Lbl_Desde_Periodo_Corriente" runat="server" Text="Label">Desde periodo</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Desde_Periodo_Corriente" runat="server" Width="96.4%"></asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Hasta_Periodo_Corriente" runat="server" Text="Label">Hasta periodo</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Hasta_Periodo_Corriente" runat="server" Width="96.4%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Alta_Periodo_Corriente" runat="server" Text="Label">Alta</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Alta_Periodo_Corriente" runat="server" Width="96.4%"></asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Baja_Periodo_Corriente" runat="server" Text="Baja"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Baja_Periodo_Corriente" runat="server" Width="96.4%"></asp:TextBox>
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
                                <asp:Label ID="Lbl_Desde_Periodo_Rezago" runat="server" Text="Label">Desde periodo</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Desde_Periodo_Rezago" runat="server" Width="96.4%"></asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Hasta_Periodo_Rezago" runat="server" Text="Label">Hasta periodo</asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Hasta_Periodo_Rezago" runat="server" Width="96.4%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:Label ID="Lbl_Alta_Periodo_Rezago" runat="server" Text="Alta"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Alta_Periodo_Rezago" runat="server" Width="96.4%"></asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:Label ID="Lbl_Baja_Periodo_Rezago" runat="server" Text="Baja"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Baja_Periodo_Rezago" runat="server" Width="96.4%"></asp:TextBox>
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
                                <asp:Label ID="Lbl_Observaciones_Cuenta" runat="server" Text="Label">*Observaciones de la cuenta</asp:Label>
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%;">
                                <asp:TextBox ID="Txt_Observaciones_Cuenta" runat="server" TabIndex="10" MaxLength="250"
                                    TextMode="MultiLine" Width="98.6%" Style="text-transform: uppercase;" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="Txt_Observaciones_Validacion"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;@#$%&/!¡¿?-_()áéíóúÁÉÍÓÚ " />
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
                            <td colspan="4" style="text-align: left; vertical-align: top;">
                                &nbsp;
                                <asp:Image ID="Img_Error1" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />
                                <asp:Label ID="Lbl_Encabezado_Error1" runat="server" 
                                    CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: left; vertical-align: top;">
                                <asp:Label ID="Lbl_Mensaje_Error1" runat="server" 
                                    CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                *Estatus
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Estatus_Orden_Variacion" runat="server" Width="292px">
                                    <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                    <asp:ListItem>ACEPTADA</asp:ListItem>
                                    <asp:ListItem>RECHAZADA</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                            </td>
                            <td style="text-align: right; width: 30%;">
                                <asp:ImageButton ID="Btn_Validar" runat="server" ImageUrl="~/paginas/imagenes/paginas/accept.png"
                                    AlternateText="Validar" OnClick="Btn_Modificar_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                *Observaciones de validación
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%;">
                                <asp:TextBox ID="Txt_Observaciones_Validacion" runat="server" MaxLength="255" TabIndex="10"
                                    TextMode="MultiLine" Width="98.6%" Style="text-transform: uppercase;" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                    TargetControlID="Txt_Observaciones_Validacion" ValidChars="Ññ.,:;@#$%&/!¡¿?-_()áéíóúÁÉÍÓÚ " />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: left; vertical-align: top;">
                                &nbsp;
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
                                    OnPageIndexChanging="Grid_Historial_Observaciones_PageIndexChanging" PageSize="5"
                                    Style="white-space: normal;" Width="97%">
                                    <Columns>
                                        <asp:BoundField DataField="OBSERVACIONES_ID" HeaderText="# Observación">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Width="15%" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPCION" HeaderText="Observaciones">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="65%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="USUARIO_CREO" HeaderText="Usuario">
                                            <ItemStyle Width="20%" />
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
                </asp:Panel>
                <br />
                <asp:HiddenField ID="Hdn_Movimiento_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Cargar_Modulos" runat="server" />
                <asp:HiddenField ID="Hdn_Grupo_Movimiento_ID" runat="server" />
                <asp:HiddenField ID="Hdn_No_Orden_Variacion" runat="server" />
                <asp:HiddenField ID="Hdn_Cuota_Minima" runat="server" />
                <asp:HiddenField ID="Hdn_Contrarecibo" runat="server" />
                <asp:HiddenField ID="Hdn_Excedente_Valor" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

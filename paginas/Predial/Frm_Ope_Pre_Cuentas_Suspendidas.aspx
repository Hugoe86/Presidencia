<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Cuentas_Suspendidas.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Cuentas_Suspendidas" %>

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
			window.showModalDialog(Url, null, Propiedades);        }
        function Abrir_Ventana_Estado_Cuenta(Url, Propiedades) 
        {
            window.open(Url, 'Estado_Cuenta', Propiedades);
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" EnablePartialRendering="true" AsyncPostBackTimeout="9000" />
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
                            Cuentas suspendidas
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td colspan="2">
                            <div align="right" style="width: 99%; background-color: #2F4E7D; color: #FFFFFF;
                                font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy;
                                height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
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
                    <%------------------ Datos generales ------------------%>
                    <tr style="background-color: #3366CC">
                        <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                            Generales
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            *Cuenta Predial
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="75%" Enable="false" TabIndex="9"
                                MaxLength="20" AutoPostBack="true" OnTextChanged="Txt_Cuenta_Predial_TextChanged"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Mostrar_Busqueda_Cuentas" runat="server" ToolTip="Búsqueda Avanzada de Cuenta Predial"
                                TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px"
                                Width="22px" OnClick="Btn_Mostrar_Busqueda_Cuentas_Click" />
                            <asp:ImageButton ID="Btn_Estado_Cuenta" runat="server" ToolTip="Detalles Estado de Cuenta"
                                TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Listado.png" Height="24px"
                                Width="24px" />
                        </td>
                        <td style="text-align: right;">
                            Cuenta origen
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Cta_Origen" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Propietario
                        </td>
                        <td style="text-align: left;" colspan="3">
                            <asp:TextBox ID="Txt_Nombre_Propietario" runat="server" Style="float: left" Width="98.5%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Tipo predio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Tipos_Predio" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Uso predio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Usos_Predio" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Estado de predio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Estados_Predio" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Estatus
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Superficie construida (m²)
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Superficie_Construida" runat="server" Width="96.4%" Text="0" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            *Superficie Total (m²)
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Superficie_Total" runat="server" Width="96.4%" Text="0" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            *Colonia
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Colonia_Cuenta" runat="server" Width="96.4%" />
                            <td style="text-align: left; width: 20%; text-align: right;">
                                *Calle
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Calle_Cuenta" runat="server" Width="96.4%" />
                            </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Número exterior
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_No_Exterior" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Número interior
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_No_Interior" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Clave catastral
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Catastral" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Efectos
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Efectos" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            &nbsp;
                            <asp:Label ID="Lbl_Ultimo_Movimiento" runat="server" Font-Bold="True" ToolTip="Número de Contrarecibo"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Aplica en:
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:CheckBox ID="Chk_Aplica_Traslado" runat="server" Text="Operaciones de Traslado de Dominio" />
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
                            &nbsp;
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:CheckBox ID="Chk_Aplica_Predial" runat="server" Text="Operaciones de Predial" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            &nbsp;
                        </td>
                        <td style="text-align: left; width: 30%;">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <%------------------ Propietario ------------------%>
                <%--<table width="98%" class="estilo_fuente">
                    <tr style="background-color: #36C;">
                        <td style="text-align:left; font-size:15px; color:#FFF;" colspan="4" >
                            Propietario
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Nombre
                        </td>
                        <td style="text-align:left;" colspan="3">
                            <asp:TextBox ID="Txt_Nombre_Propietario" runat="server" Width="94.4%" style="float:left"/>                            
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            RFC
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Rfc_Propietario" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            &nbsp; *Tipo propietario</td>
                        <td>
                            <asp:TextBox ID="Txt_Tipo_Propietario" runat="server" Width="96.4%" />                            
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Domicilio foráneo
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Domicilio_Foraneo" runat="server" Width="96.4%" />    
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            
                        </td>
                        <td style="text-align:left;width:30%;">
                        
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Colonia
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Colonia_Propietario" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            Calle
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Calle_Propietario" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Número exterior
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Numero_Exterior_Propietario" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            Número interior
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Numero_Interior_Propietario" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Estado
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Estado_Propietario" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            Ciudad
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Ciudad_Propietario" runat="server" Width="96.4%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            C.P.
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_CP" runat="server" Width="96.4%" />
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr><td colspan="4">&nbsp;</td></tr>
                    </table>--%>
                <table width="98%" class="estilo_fuente">
                    <%------------------ Cuenta ------------------%>
                    <tr style="background-color: #36C;">
                        <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                            Cuentas suspendidas
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid" runat="server" AllowPaging="True" CssClass="GridView_1" AutoGenerateColumns="False"
                                PageSize="4" Width="100%" AllowSorting="True" HeaderStyle-CssClass="tblHead"
                                Style="white-space: normal;" OnPageIndexChanging="Grid_PageIndexChanging" OnSelectedIndexChanged="Grid_SelectedIndexChanged"
                                DataKeyNames="ID">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="7%" HorizontalAlign="Center" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                        <ItemStyle HorizontalAlign="Left" Width="12%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TIPO_SUSPENCION" HeaderText="Traslado/Predial">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="USUARIO_MODIFICO" HeaderText="Realizó">
                                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="c2" HeaderText="Adeudo total" Visible="false">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                        <ItemStyle HorizontalAlign="Left" Width="12%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA_MODIFICO" HeaderText="Fecha bloqueo">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                        <ItemStyle HorizontalAlign="Left" Width="12%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ID" HeaderText="Cuenta_Predial_Id" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                        <ItemStyle HorizontalAlign="Left" Width="12%" />
                                    </asp:BoundField>
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
                </table>
                <br />
                <asp:HiddenField ID="Hdn_Propietario_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Cuenta_ID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

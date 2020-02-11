<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Recepcion_Pagos_Predial.aspx.cs"
    Inherits="paginas_Predial_Frm_Ope_Pre_Recepcion_Pagos_Predial" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

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
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server" EnableScriptLocalization="true"
        EnableScriptGlobalization="true" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Adeudos Predial
                        </td>
                    </tr>
                    <tr>
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
                                            <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                                AlternateText="Regresar a Caja" ToolTip="Regresar a Caja" OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            &nbsp;&nbsp;
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
                            <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                            Cuenta Predial
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="98%" Enabled="false" BorderWidth="1" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            <asp:ImageButton ID="Btn_Resumen_Predio" runat="server" ToolTip="Resumen de predio"
                                ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Height="24px" Width="24px"
                                Style="float: left" />
                            Convenio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:HiddenField ID="Hdf_Convenio" runat="server" />
                            <asp:TextBox ID="Txt_Convenio" runat="server" Width="98%" Enabled="false" BorderWidth="1" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Propietario
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="Txt_Propietario" runat="server" Width="99%" Enabled="false" BorderWidth="1" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Ubicación
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="Txt_Ubicacion" runat="server" Width="99%" Enabled="false" BorderWidth="1" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Desde Periodo
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Bimestre_Inicial" runat="server" MaxLength="250" TextMode="SingleLine"
                                Width="30%" Enabled="false" Style="text-align: center;" BorderWidth="1" />
                            <asp:TextBox ID="Txt_Anio_Inicial" runat="server" MaxLength="250" TextMode="SingleLine"
                                Width="60%" Enabled="false" Style="text-align: center; float: right;" BorderWidth="1" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Hasta el Periodo
                        </td>
                        <td style="text-align: left; width: 30%;">
                            &nbsp;&nbsp;
                            <asp:DropDownList ID="Cmb_Bimestre_Final" runat="server" Width="30%" AutoPostBack="true"
                                OnSelectedIndexChanged="Cmb_Bimestre_Final_SelectedIndexChanged">
                                <asp:ListItem Value="1">01</asp:ListItem>
                                <asp:ListItem Value="2">02</asp:ListItem>
                                <asp:ListItem Value="3">03</asp:ListItem>
                                <asp:ListItem Value="4">04</asp:ListItem>
                                <asp:ListItem Value="5">05</asp:ListItem>
                                <asp:ListItem Value="6">06</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="Cmb_Anio_Final" runat="server" Width="55%" Style="float: right"
                                AutoPostBack="true" OnSelectedIndexChanged="Cmb_Anio_Final_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <%------------------ Adeudos ------------------%>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="Lbl_Leyenda_Calculos" runat="server" Text="Calculos para el Pago"
                                Style="color: Blue; font-weight: bolder; text-align: center; font-size: medium;"
                                Width="98%"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Periodo Rezago
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Periodo_Rezago" runat="server" Width="98%" Enabled="false" BorderWidth="1" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Adeudo Rezago
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Adeudo_Rezago" runat="server" Width="98%" Enabled="false" BorderWidth="1"
                                Style="text-align: right;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Periodo Corriente
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Periodo_Actual" runat="server" Width="98%" Enabled="false" BorderWidth="1" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Adeudo Corriente
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Adeudo_Actual" runat="server" Width="98%" Enabled="false" BorderWidth="1"
                                Style="text-align: right;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Total Recargos Ordinarios
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Total_Recargos_Ordinarios" runat="server" Width="98%" Enabled="false"
                                BorderWidth="1" Style="text-align: right;" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Honorarios
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Honorarios" runat="server" Width="98%" Enabled="false" BorderWidth="1"
                                Style="text-align: right;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Recargos Moratorios
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Recargos_Moratorios" runat="server" Width="98%" Enabled="false"
                                BorderWidth="1" Style="text-align: right;" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Gastos de Ejecución
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Gastos_Ejecucion" runat="server" Width="98%" Enabled="false"
                                BorderWidth="1" Style="text-align: right;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="border-width: thick; text-align: left; width: 50%;" colspan="2">
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right; border-top: solid 1px BLACK">
                            Subtotal
                        </td>
                        <td style="text-align: left; width: 30%; border-top: solid 1px BLACK">
                            <asp:TextBox ID="Txt_SubTotal" runat="server" Width="98%" Enabled="false" BorderWidth="1"
                                Style="text-align: right;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            &nbsp;
                        </td>
                        <td style="width: 20%; text-align: right;" colspan="2">
                            Descuento Corriente
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Descuento_Corriente" runat="server" Width="98%" Enabled="false"
                                BorderWidth="1" Style="text-align: right;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            &nbsp;
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;" colspan="2">
                            Descuento Recargos Ordinarios
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Descuento_Recargos_Ordinarios" runat="server" Width="98%" Enabled="false"
                                BorderWidth="1" Style="text-align: right;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            &nbsp;
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;" colspan="2">
                            Descuento Recargos Moratorios
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Descuento_Recargos_Moratorios" runat="server" Width="98%" Enabled="false"
                                BorderWidth="1" Style="text-align: right;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;" colspan="2">
                            &nbsp;
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Descuento Honorarios
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Descuento_Honorarios" runat="server" Width="98%" Enabled="false"
                                BorderWidth="1" Style="text-align: right;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;" colspan="2">
                            &nbsp;
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right; border-top: solid 1px BLACK;">
                            Total
                        </td>
                        <td style="text-align: left; width: 30%; border-top: solid 1px BLACK;">
                            <asp:TextBox ID="Txt_Total" runat="server" Width="98%" Enabled="false" BorderWidth="1"
                                Style="text-align: right;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;" colspan="2">
                            &nbsp;
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Ajuste Tarifario
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Ajuste_Tarifario" runat="server" Width="98%" Enabled="false"
                                BorderWidth="1" Style="text-align: right;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;" colspan="2">
                            &nbsp;
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right; border-top: solid 1px BLACK;">
                            <b>Total a Pagar</b>
                        </td>
                        <td style="text-align: left; width: 30%; border-top: solid 1px BLACK; border-style: outset;">
                            <asp:TextBox ID="Txt_Total_Pagar" runat="server" Width="98%" Style="font-weight: bolder;
                                font-size: medium; text-align: right;" ReadOnly="true" BorderWidth="1" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25%; vertical-align: middle;">
                            <asp:ImageButton ID="Btn_Icono_Efectivo" runat="server" Enabled="false" ImageUrl="~/paginas/imagenes/paginas/efectivo.jpg"
                                Width="64px" />
                        </td>
                        <td style="width: 25%; vertical-align: middle;">
                            &nbsp;
                        </td>
                        <td colspan="2" style="width: 50%">
                            <asp:Button ID="Btn_Ejecutar_Pago" runat="server" Text="EJECUTAR PAGO EN CAJA" Style="color: Black;
                                font-weight: bolder; text-align: center; font-size: larger;" Width="98%" Height="35px"
                                OnClick="Btn_Ejecutar_Pago_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr style="background-color: #36C;">
                        <td style="text-align: left; font-size: 15px; color: #FFF; font-weight: bolder;"
                            colspan="4">
                            Adeudos Pendientes
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="4">
                            <div id="Div_Listado_Adeudos_Predial" runat="server" style="width: 99%; height: 300px;
                                overflow: auto; border-width: thick; border-color: Black;">
                                <asp:GridView ID="Grid_Listado_Adeudos" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                                    Width="100%" AllowSorting="True" HeaderStyle-CssClass="tblHead" Style="white-space: normal;"
                                    OnRowDataBound="Grid_Listado_Adeudos_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chk_Seleccion_Adeudo" runat="server" Checked="true" AutoPostBack="True"
                                                    OnCheckedChanged="Chk_Seleccion_Adeudo_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="NO_ADEUDO" HeaderText="NO_ADEUDO">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CONCEPTO" HeaderText="Concepto">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ANIO" HeaderText="Año">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BIMESTRE" HeaderText="Bim.">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DESCUENTO" HeaderText="Desc." DataFormatString="{0:c}">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CORRIENTE" HeaderText="Corriente" DataFormatString="{0:c}">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="REZAGOS" HeaderText="Rezagos" DataFormatString="{0:c}">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RECARGOS_ORDINARIOS" HeaderText="Recargos Ordinarios"
                                            DataFormatString="{0:c}">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RECARGOS_MORATORIOS" HeaderText="Recargos Moratorios"
                                            DataFormatString="{0:c}">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="HONORARIOS" HeaderText="Honorarios" DataFormatString="{0:c}">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MULTAS" HeaderText="Multas" DataFormatString="{0:c}">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DESCUENTOS" HeaderText="Descuentos" DataFormatString="{0:c}">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MONTO" HeaderText="Monto" DataFormatString="{0:c}">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </div>
                            <div id="Div_Listado_Adeudos_Convenio" runat="server" style="width: 99%; height: 300px;
                                overflow: auto; border-width: thick; border-color: Black;">
                                <asp:GridView ID="Grid_Listado_Adeudos_Convenio" runat="server" CssClass="GridView_1"
                                    AutoGenerateColumns="False" Width="100%" AllowSorting="True" HeaderStyle-CssClass="tblHead"
                                    Style="white-space: normal;" OnRowDataBound="Grid_Listado_Adeudos_Convenio_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chk_Seleccion_Parcialidad" runat="server" Checked="false" AutoPostBack="True"
                                                    OnCheckedChanged="Chk_Seleccion_Parcialidad_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="NO_PAGO" HeaderText="NO_PAGO">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTADO_PAGO" HeaderText="ESTADO_PAGO">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PARCIALIDAD" HeaderText="Parcialidad">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ANIO" HeaderText="Año">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PERIODO" HeaderText="Bimestres">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CORRIENTE" HeaderText="Corriente" DataFormatString="{0:c}">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="REZAGOS" HeaderText="Rezagos" DataFormatString="{0:c}">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RECARGOS_ORDINARIOS" HeaderText="Recargos Ordinarios"
                                            DataFormatString="{0:c}">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RECARGOS_MORATORIOS" HeaderText="Recargos Moratorios"
                                            DataFormatString="{0:c}">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="HONORARIOS" HeaderText="Honorarios" DataFormatString="{0:c}">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MONTO" HeaderText="Monto" DataFormatString="{0:c}">
                                            <ItemStyle Font-Size="XX-Small" HorizontalAlign="Center" Wrap="false" />
                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;<asp:HiddenField ID="Hfd_No_Descuento_Recargos" runat="server" />
                            <asp:HiddenField ID="Hfd_Tipo_Predio" runat="server" />
                            <asp:HiddenField ID="Hfd_Cuota_Fija" runat="server" />
                        </td>
                    </tr>
                </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

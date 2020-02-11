<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Baja_Directa.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Baja_Directa" %>

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

        function Abrir_Resumen(Url, Propiedades)
        {
            window.open(Url, 'Resumen_Predio', Propiedades);
        }

		function Abrir_Ventana_Estado_Cuenta(Url, Propiedades)
		{
			window.open(Url, 'Estado_Cuenta', Propiedades);
		}

		function formatCurrency(num) {
                num = num.toString().replace(/\$|\,/g,'');
                if(isNaN(num))
                num = "0";
                sign = (num == (num = Math.abs(num)));
                num = Math.floor(num*100+0.50000000001);
                cents = num%100;
                num = Math.floor(num/100).toString();
                if(cents<10)
                    cents = "0" + cents;
                    for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
                    num = num.substring(0,num.length-(4*i+3))+','+
                    num.substring(num.length-(4*i+3));
                    return (((sign)?'':'-') + num + '.' + cents);
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
                            Bajas Directas
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
                                            <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                                                TabIndex="4" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" Visible="false" />
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
                <asp:Panel ID="Panel_Bajas" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <%------------------ Bajas Generadas ------------------%>
                        <tr style="background-color: #3366CC">
                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                Bajas
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="Grid_Bajas" runat="server" AutoGenerateColumns="False" CssClass="GridView_1"
                                    HeaderStyle-CssClass="tblHead" Style="white-space: normal;" Width="100%" AllowPaging="True"
                                    DataKeyNames="CUENTA_PREDIAL_ID,MOVIMIENTO_ID,OBSERVACIONES" OnPageIndexChanging="Grid_Bajas_PageIndexChanging"
                                    OnSelectedIndexChanged="Grid_Bajas_SelectedIndexChanged">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png"
                                            Text="Button">
                                            <FooterStyle Width="5%" />
                                            <HeaderStyle Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="NO_ORDEN_VARIACION" HeaderText="No. Movimiento">
                                            <FooterStyle Width="20%" />
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle Width="20%" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial">
                                            <FooterStyle Width="20%" />
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="CUENTA_PREDIAL_ID" Visible="False" />
                                        <asp:BoundField DataField="IDENTIFICADOR_DESCRIPCION" HeaderText="Descripción" />
                                        <asp:BoundField DataField="MOVIMIENTO_ID" HeaderText="MOVIMIENTO_ID" Visible="False" />
                                        <asp:BoundField DataField="FECHA_CREO" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Fecha Movimiento">
                                            <FooterStyle Width="20%" />
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OBSERVACIONES" HeaderText="OBSERVACIONES" Visible="False" />
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
                <asp:Panel ID="Panel_Datos" runat="server">
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
                                <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="84%" Enable="false" TabIndex="9"
                                    MaxLength="12" AutoPostBack="true" OnTextChanged="Txt_Cuenta_Predial_TextChanged"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Cuentas" runat="server" ToolTip="Búsqueda Avanzada de Cuenta Predial"
                                    TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px"
                                    Width="22px" OnClick="Btn_Mostrar_Busqueda_Cuentas_Click" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                <asp:ImageButton ID="Btn_Estado_Cuenta" runat="server" ToolTip="Estado de cuenta"
                                    TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/sias_revisarplan.png" Height="22px"
                                    Width="22px" Style="float: left" />
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
                                <asp:TextBox ID="Txt_Nombre_Propietario" runat="server" Style="float: left" Width="98.6%" />
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
                                <asp:TextBox ID="Txt_Estatus" runat="server" Width="96.4%" />
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
                                    *Ubicación
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
                                Costo(m²)
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Costo_M2" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Dif. de construcción
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Dif_Construccion" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                *Movimiento
                            </td>
                            <td colspan="3" style="text-align: left;">
                                <asp:DropDownList ID="Cmb_Movimientos" runat="server" Width="99.4%">
                                </asp:DropDownList>
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
                        <%------------------ Detalles cuota fija ------------------%>
                        <asp:Label ID="Lbl_Error_Cuota_Fija" runat="server" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        <tr>
                            <td style="text-align: left; font-size: 14px; border-bottom: 1px solid #3366CC;"
                                colspan="4">
                                Detalles Cuota fija
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                El solicitante es:
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Solicitante" runat="server" Width="99%" TabIndex="7" AutoPostBack="true"
                                    OnSelectedIndexChanged="Cmb_Solicitante_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                o inmueble financiado:
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Financiado" runat="server" Width="99%" TabIndex="7" AutoPostBack="true"
                                    OnSelectedIndexChanged="Cmb_Financiado_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                Fundamento legal
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%;">
                                <asp:TextBox ID="Txt_Fundamento" runat="server" TabIndex="10" MaxLength="250" TextMode="SingleLine"
                                    Width="98.6%" Enabled="false" AutoPostBack="True" />
                            </td>
                        </tr>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF; border-color: #FFF" colspan="4">
                                Impuestos
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: middle;">
                                Valor Fiscal
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%; color: #FFF; border-color: #FFF">
                                <asp:TextBox ID="Txt_Valor_Fiscal" runat="server" onBlur="this.value=formatCurrency(this.value);"
                                    Width="200px" Enabled="False" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; text-align: left;">
                                Tasa
                            </td>
                            <td colspan="3" style="text-align: left; text-align: right; width: 80%; color: #FFF;
                                border-color: #FFF">
                                <asp:TextBox ID="Txt_Tasa_Descripcion" runat="server" Enabled="false" Width="85%"
                                    Style="float: left" />
                                <asp:TextBox ID="Txt_Tasa_Porcentaje" runat="server" AutoPostBack="true" Style="float: right"
                                    Enabled="false" Width="50px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: middle;">
                                Excedente Construcción
                            </td>
                            <td style="text-align: left; width: 30%; color: #FFF; border-color: #FFF">
                                <asp:TextBox ID="Txt_Excedente_Construccion_Total" runat="server" ReadOnly="True"
                                    Width="96.4%" />
                            </td>
                            <td style="text-align: right; width: 20%; vertical-align: middle;">
                                Excedente Valor
                            </td>
                            <td style="text-align: left; width: 30%; color: #FFF; border-color: #FFF">
                                <asp:TextBox ID="Txt_Excedente_Valor_Total" runat="server" ReadOnly="True" Width="96.4%" />
                            </td>
                        </tr>
                    </table>
                    <div id="Div_Adeudos_Editable" style="background-color: #ffffff; width: 100%; height: 100%;">
                        <table width="98%" class="estilo_fuente" style="border-spacing: 0">
                            <tr style="background-color: #36C;">
                                <td style="text-align: left; font-size: 15px; color: #FFF; border-color: #FFF" colspan="3">
                                    Adeudos
                                </td>
                                <td style="text-align: right; width: 100%; color: #FFF; border-color: #FFF">
                                    <asp:ImageButton ID="Btn_Generar_Diferencias" runat="server" Height="22px" ImageUrl="~/paginas/imagenes/paginas/accept.png"
                                        TabIndex="10" ToolTip="Generar Diferencias" Width="22px" OnClick="Btn_Generar_Diferencias_Click" />
                                    <asp:ImageButton ID="Btn_Mostrar_Tasas_Diferencias" runat="server" Height="22px"
                                        ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" OnClick="Btn_Mostrar_Tasas_Diferencias_Click"
                                        TabIndex="10" ToolTip="Limpiar Análisis de Rezago" Width="22px" />
                                </td>
                            </tr>
                            <asp:Label ID="Lbl_Error_Adeudos" runat="server" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </table>
                        <%--Grid Editable de Adeudos--%>
                        <table width="98%" class="estilo_fuente">
                            <tr>
                                <td align="center" colspan="4">
                                    <tr align="center">
                                        <td colspan="4">
                                            <asp:GridView ID="Grid_Adeudos_Editable" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" CssClass="GridView_1" HeaderStyle-CssClass="tblHead"
                                                Style="white-space: normal;" OnRowCommand="Grid_Adeudos_Editable_RowCommand"
                                                Width="100%" PageSize="100">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="2%">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="Btn_Editar_Adeudos" runat="server" Height="20px" ImageUrl="~/paginas/imagenes/gridview/tecnico.png"
                                                                ToolTip="Editar Adeudos" Width="20px" CommandName="Cmd_Editar_Adeudos" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Anio" HeaderText="Año">
                                                        <HeaderStyle HorizontalAlign="center" Width="4%" />
                                                        <ItemStyle HorizontalAlign="center" Width="4%" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Bimestre 1" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="Txt_Grid_Bimestre_1" runat="server" Style="width: 96%; text-align: right;"
                                                                MaxLength="10" Visible='<%# Grid_Editable %>' onChage="this.value=formatCurrency(this.value);"
                                                                onBlur="this.value=formatCurrency(this.value);" Text='<%# Bind("ADEUDO_BIMESTRE_1", "{0:#,###,##0.00}") %>'>
                                                            </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Bimestre" runat="server" FilterType="Numbers,Custom"
                                                                TargetControlID="Txt_Grid_Bimestre_1" ValidChars="." />
                                                            <asp:Label ID="Lbl_Grid_Bimestre_1" runat="server" Style="width: 96%; text-align: right;"
                                                                Visible='<%# !(bool)Grid_Editable %>' OnChange="this.value=formatCurrency(this.value);"
                                                                Text='<%# Bind("ADEUDO_BIMESTRE_1", "{0:#,###,##0.00}") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Bimestre 2" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="Txt_Grid_Bimestre_2" runat="server" Style="width: 96%; text-align: right;"
                                                                MaxLength="7" Visible='<%# Grid_Editable %>' onChage="this.value=formatCurrency(this.value);"
                                                                onBlur="this.value=formatCurrency(this.value);" Text='<%# Bind("ADEUDO_BIMESTRE_2", "{0:#,###,##0.00}") %>'>
                                                            </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Bimestre_2" runat="server" FilterType="Numbers,Custom"
                                                                TargetControlID="Txt_Grid_Bimestre_2" ValidChars="." />
                                                            <asp:Label ID="Lbl_Grid_Bimestre_2" runat="server" Style="width: 90%; text-align: right;
                                                                padding-right: 3px;" Visible='<%# !(bool)Grid_Editable %>' Text='<%# Bind("ADEUDO_BIMESTRE_2", "{0:#,###,##0.00}") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Bimestre 3" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="Txt_Grid_Bimestre_3" runat="server" Style="width: 96%; text-align: right;"
                                                                MaxLength="7" Visible='<%# Grid_Editable %>' onChage="this.value=formatCurrency(this.value);"
                                                                onBlur="this.value=formatCurrency(this.value);" Text='<%# Bind("ADEUDO_BIMESTRE_3", "{0:#,###,##0.00}") %>'>
                                                            </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Bimestre_3" runat="server" FilterType="Numbers,Custom"
                                                                TargetControlID="Txt_Grid_Bimestre_3" ValidChars="." />
                                                            <asp:Label ID="Lbl_Grid_Bimestre_3" runat="server" Style="width: 90%; text-align: right;
                                                                padding-right: 3px;" Visible='<%# !(bool)Grid_Editable %>' Text='<%# Bind("ADEUDO_BIMESTRE_3", "{0:#,###,##0.00}") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Bimestre 4" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="Txt_Grid_Bimestre_4" runat="server" Style="width: 96%; text-align: right;"
                                                                MaxLength="7" Visible='<%# Grid_Editable %>' onChage="this.value=formatCurrency(this.value);"
                                                                onBlur="this.value=formatCurrency(this.value);" Text='<%# Bind("ADEUDO_BIMESTRE_4", "{0:#,###,##0.00}") %>'>
                                                            </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Bimestre_4" runat="server" FilterType="Numbers,Custom"
                                                                TargetControlID="Txt_Grid_Bimestre_4" ValidChars="." />
                                                            <asp:Label ID="Lbl_Bimestre_4" runat="server" Style="width: 90%; text-align: right;
                                                                padding-right: 3px;" Visible='<%# !(bool)Grid_Editable %>' Text='<%# Bind("ADEUDO_BIMESTRE_4", "{0:#,###,##0.00}") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Bimestre 5" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="Txt_Grid_Bimestre_5" runat="server" Style="width: 96%; text-align: right;"
                                                                MaxLength="7" Visible='<%# Grid_Editable %>' onChage="this.value=formatCurrency(this.value);"
                                                                onBlur="this.value=formatCurrency(this.value);" Text='<%# Bind("ADEUDO_BIMESTRE_5", "{0:#,###,##0.00}") %>'>
                                                            </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Bimestre_5" runat="server" FilterType="Numbers,Custom"
                                                                TargetControlID="Txt_Grid_Bimestre_5" ValidChars="." />
                                                            <asp:Label ID="Lbl_Grid_Bimestre_5" runat="server" Style="width: 90%; text-align: right;
                                                                padding-right: 3px;" Visible='<%# !(bool)Grid_Editable %>' Text='<%# Bind("ADEUDO_BIMESTRE_5", "{0:#,###,##0.00}") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Bimestre 6" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="Txt_Grid_Bimestre_6" runat="server" Style="width: 96%; text-align: right;"
                                                                MaxLength="7" Visible='<%# Grid_Editable %>' onChage="this.value=formatCurrency(this.value);"
                                                                onBlur="this.value=formatCurrency(this.value);" Text='<%# Bind("ADEUDO_BIMESTRE_6", "{0:#,###,##0.00}") %>'>
                                                            </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Grid_Bimestre_6" runat="server" FilterType="Numbers,Custom"
                                                                TargetControlID="Txt_Grid_Bimestre_6" ValidChars="." />
                                                            <asp:Label ID="Lbl_Grid_Bimestre_6" runat="server" Style="width: 90%; text-align: right;
                                                                padding-right: 3px;" Visible='<%# !(bool)Grid_Editable %>' Text='<%# Bind("ADEUDO_BIMESTRE_6", "{0:#,###,##0.00}") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Lbl_Total" runat="server" Style="width: 90%; text-align: right; padding-right: 3px;"
                                                                Text='<%# Bind("ADEUDO_TOTAL_ANIO", "{0:#,###,##0.00}") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="2%">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="Btn_Calcular_Totales" runat="server" Height="20px" ImageUrl="~/paginas/imagenes/paginas/SIAS_Calc2.gif"
                                                                ToolTip="Calcular" Width="20px" CommandName="Cmd_Calcular_Totales" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            <br />
                                        </td>
                                    </tr>
                                </td>
                                <tr>
                                    <td align="left" colspan="2" />
                                </tr>
                                <td colspan="2" align="left" />
                            </tr>
                        </table>
                    </div>
                    <%--Grid Editable de Adeudos--%>
                    <div id="Div_Diferencias" style="background-color: #ffffff; width: 100%; height: 100%;">
                        <table width="98%" class="estilo_fuente" style="border-spacing: 0">
                            <tr style="background-color: #36C;">
                                <td style="text-align: left; font-size: 15px; color: #FFF; border-color: #FFF" colspan="3">
                                    Diferencias
                                </td>
                                <td style="text-align: right; width: 100%; color: #FFF; border-color: #FFF">
                                </td>
                            </tr>
                            <asp:Label ID="Lbl_Mensaje_Error_Diferencias" runat="server" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            <%--<tr>
                                <td style="text-align: left; width: 20%;">
                                    Periodo corriente
                                </td>
                                <td colspan="3" style="text-align: left; width: 80%; vertical-align: middle;">
                                    <asp:DropDownList ID="Cmb_P_C_Bimestre_Inicial" runat="server">
                                    </asp:DropDownList>
                                    -
                                    <asp:DropDownList ID="Cmb_P_C_Bimestre_Final" runat="server">
                                    </asp:DropDownList>
                                    /
                                    <asp:DropDownList ID="Cmb_P_C_Anio" runat="server">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="Btn_Agregar_P_Corriente" runat="server" Height="22px" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                        TabIndex="10" ToolTip="Agregar" Width="22px" OnClick="Btn_Agregar_P_Corriente_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    Periodo rezago
                                </td>
                                <td style="text-align: left; width: 80%;" colspan="3">
                                    <asp:DropDownList ID="Cmb_P_R_Bimestre_Inicial" runat="server">
                                    </asp:DropDownList>
                                    /
                                    <asp:DropDownList ID="Cmb_P_R_Anio_Inicial" runat="server">
                                    </asp:DropDownList>
                                    -
                                    <asp:DropDownList ID="Cmb_P_R_Bimestre_Final" runat="server">
                                    </asp:DropDownList>
                                    /
                                    <asp:DropDownList ID="Cmb_P_R_Anio_Final" runat="server">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="Btn_Agregar_P_Regazo" runat="server" ToolTip="Agregar" TabIndex="10"
                                        ImageUrl="~/paginas/imagenes/paginas/sias_add.png" Height="22px" Width="22px"
                                        OnClick="Btn_Agregar_P_Regazo_Click" />
                                </td>
                            </tr>--%>
                            <tr align="center">
                                <td colspan="4">
                                    <div id="Div_Grid_Diferencias" style="background-color: #ffffff; width: 100%;">
                                        <asp:GridView ID="Grid_Diferencias" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                                            Width="100%" HeaderStyle-CssClass="tblHead" Style="white-space: normal;" OnDataBound="Grid_Diferencias_DataBound"
                                            OnRowCommand="Grid_Diferencias_RowCommand" OnRowDataBound="Grid_Diferencias_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="PERIODO" HeaderText="Periodo" SortExpression="PERIODO">
                                                    <HeaderStyle Width="15%" HorizontalAlign="Left" />
                                                    <ItemStyle Width="15%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TIPO" HeaderText="Alta/Baja" SortExpression="Alta/Baja">
                                                    <HeaderStyle Width="12%" HorizontalAlign="Left" />
                                                    <ItemStyle Width="12%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="VALOR_FISCAL" HeaderText="Valor fiscal" SortExpression="VALOR_FISCAL">
                                                    <HeaderStyle Width="15%" HorizontalAlign="Left" />
                                                    <ItemStyle Width="15%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TASA" HeaderText="Tasa" SortExpression="TASA">
                                                    <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                                    <ItemStyle Width="10%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="IMPORTE" HeaderText="Importe" SortExpression="IMPORTE">
                                                    <HeaderStyle Width="15%" HorizontalAlign="Left" />
                                                    <ItemStyle Width="15%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CUOTA_BIMESTRAL" HeaderText="Cuota Bimestral" SortExpression="CUOTA BIMESTRAL">
                                                    <HeaderStyle Width="15%" HorizontalAlign="Left" />
                                                    <ItemStyle Width="15%" />
                                                </asp:BoundField>
                                            </Columns>
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <HeaderStyle CssClass="tblHead" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </div>
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <table width="98%" class="estilo_fuente">
                            <tr>
                                <td style="text-align: left; width: 20%; vertical-align: top;">
                                    Vista previa de adeudos
                                </td>
                                <td colspan="3" style="text-align: left; width: 80%;">
                                    <asp:ImageButton ID="Btn_Vista_Adeudos" runat="server" ToolTip="Vista previa de Adeudos"
                                        TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Height="22px"
                                        Width="22px" Style="float: left" />
                                </td>
                            </tr>
                            <%------------------ Detalles Analis de Rezagos ------------------%>
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
                                    <asp:Label ID="Txt_Desde_Periodo_Corriente" runat="server" Width="36.4%" Enabled="false"
                                        Font-Bold="True" />A&ntilde;o
                                    <asp:Label ID="Txt_Desde_Anio_Periodo_Corriente" runat="server" Width="40%" Enabled="false"
                                        Font-Bold="True" />
                                </td>
                                <td style="text-align: left; width: 20%; text-align: right;">
                                    Hasta periodo
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:Label ID="Txt_Hasta_Periodo_Corriente" runat="server" Width="36.4%" Enabled="false"
                                        Font-Bold="True" />A&ntilde;o
                                    <asp:Label ID="Txt_Hasta_Anio_Periodo_Corriente" runat="server" Width="40%" Enabled="false"
                                        Font-Bold="True" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    Alta
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:Label ID="Txt_Alta_Periodo_Corriente" runat="server" Width="96.4%" Enabled="false"
                                        Font-Bold="True" />
                                </td>
                                <td style="text-align: left; width: 20%; text-align: right;">
                                    Baja
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:Label ID="Txt_Baja_Periodo_Corriente" runat="server" Width="96.4%" Enabled="false"
                                        Font-Bold="True" />
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
                                    <asp:Label ID="Txt_Desde_Periodo_Regazo" runat="server" Width="36.4%" Enabled="false"
                                        Font-Bold="true" />A&ntilde;o
                                    <asp:Label ID="Lbl_P_C_Anio_Inicio" runat="server" Enabled="false" Font-Bold="True"
                                        Width="40%" />
                                </td>
                                <td style="text-align: left; width: 20%; text-align: right;">
                                    Hasta periodo
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:Label ID="Txt_Hasta_Periodo_Regazo" runat="server" Width="36.4%" Enabled="false"
                                        Font-Bold="true" />
                                    A&ntilde;o
                                    <asp:Label ID="Lbl_P_C_Anio_Final" runat="server" Width="40%" Enabled="false" Font-Bold="True" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    Alta
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:Label ID="Txt_Alta_Periodo_Regazo" runat="server" Width="96.4%" Enabled="false"
                                        Font-Bold="true" />
                                </td>
                                <td style="text-align: left; width: 20%; text-align: right;">
                                    Baja
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    <asp:Label ID="Txt_Baja_Periodo_Regazo" runat="server" Width="96.4%" Enabled="false"
                                        Font-Bold="true" />
                                </td>
                            </tr>
                            <%------------------ Observaciones ------------------%>
                            <tr>
                                <td style="text-align: left; width: 20%;">
                                    &nbsp;
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    &nbsp;
                                </td>
                                <td style="text-align: left; width: 20%; text-align: right;">
                                    &nbsp;
                                </td>
                                <td style="text-align: left; width: 30%;">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr style="background-color: #36C;">
                                <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                    Observaciones de la cuenta
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 20%; vertical-align: top;">
                                    *Observaciones de la cuenta
                                </td>
                                <td colspan="3" style="text-align: left; width: 80%;">
                                    <asp:TextBox ID="Txt_Observaciones_Cuenta" runat="server" TabIndex="10" MaxLength="250"
                                        TextMode="MultiLine" Width="98.6%" Style="text-transform: uppercase" />
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="Txt_Observaciones_Cuenta"
                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <br />
                    </div>
                </asp:Panel>
                <asp:HiddenField ID="Hdn_Propietario_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Cuenta_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Tasa_ID" runat="server" />
                <asp:HiddenField ID="Hdn_Tasa_Dif" runat="server" />
                <asp:HiddenField ID="Hdn_Excedente_Valor" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

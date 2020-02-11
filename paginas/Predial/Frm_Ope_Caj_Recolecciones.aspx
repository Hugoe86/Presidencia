<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Caj_Recolecciones.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Caj_Recolecciones"
    Title="Recoleccion de Caja" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/paginas/Paginas_Generales/Pager.ascx" TagPrefix="custom" TagName="Pager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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

        //Metodo para mantener los calendarios en una capa mas alat.
        function calendarShown(sender, args)
        {
            sender._popupBehavior._element.style.zIndex = 10000005;
        }

        function Formatear_Numero(Numero) {
            //Propiedades
            this.valor = Numero || 0
            this.dec = -1;
            //Método 
            this.Formato = numFormat;
            //Definición de los métodos 
            function numFormat(dec, miles) {
                var num = this.valor, signo = 3, expr;
                var cad = "" + this.valor;
                var ceros = "", pos, pdec, i;
                for (i = 0; i < dec; i++)
                    ceros += '0';
                pos = cad.indexOf('.')
                if (pos < 0)
                    cad = cad + "." + ceros;
                else {
                    pdec = cad.length - pos - 1;
                    if (pdec <= dec) {
                        for (i = 0; i < (dec - pdec); i++)
                            cad += '0';
                    }
                    else {
                        num = num * Math.pow(10, dec);
                        num = Math.round(num);
                        num = num / Math.pow(10, dec);
                        cad = new String(num);
                    }
                }
                pos = cad.indexOf('.')
                if (pos < 0) pos = cad.lentgh
                if (cad.substr(0, 1) == '-' || cad.substr(0, 1) == '+')
                    signo = 4;
                if (miles && pos > signo)
                    do {
                        expr = /([+-]?\d)(\d{3}[\.\,]\d*)/
                        cad.match(expr)
                        cad = cad.replace(expr, RegExp.$1 + ',' + RegExp.$2)
                    }
                    while (cad.indexOf(',') > signo)
                    if (dec < 0) cad = cad.replace(/\./, '')
                    return cad;
                }
            } //Fin del objeto Formatear_Numero

            function Sumar(){
              var Denominacion =0;
              var Total = 0;
              var Monto = 0;
      
              $('#Div_Denominaciones :input').each(function(){
                Denominacion = parseFloat(($(this).attr('title') == "") ? "0" : $(this).attr('title'));
                Monto = parseFloat((($(this).val() == "")?"0":$(this).val()));
                Monto = Monto * Denominacion;
                Total = Total + Monto;
              });
                var Numero = new Formatear_Numero(Total); //obtenemos el objeto del total para poder formatearlo
                $('input[id$=Txt_Monto_Recolectado]').val(Numero.Formato(2, true));
                $("input[id$=Hdn_Efectivo_Caja]").val(Numero.Formato(2, true));
            }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager_Recolecciones_Empleados" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Recolecciones_Empleados" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Recolecciones_Empleados" style="background-color: #ffffff; width: 98%;
                height: 100%;">
                <table width="100%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Corte Parcial
                        </td>
                    </tr>
                    <tr>
                        <td>
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
                            <div align="right" class="barra_busqueda">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 50%;">
                                            <asp:UpdatePanel ID="Upnl_Botones_Operacion" runat="server" UpdateMode="Conditional"
                                                RenderMode="Inline">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                                        TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                                                    <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                                                        TabIndex="2" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" OnClick="Btn_Imprimir_Click" />
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                        TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                                    <asp:TextBox ID="Txt_Caja_ID" runat="server" Visible="false" ReadOnly="true"></asp:TextBox>
                                                    <asp:TextBox ID="Txt_No_Turno" runat="server" Visible="false" ReadOnly="true"></asp:TextBox>
                                                    <asp:HiddenField ID="Hdn_Efectivo_Caja" runat="server" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td style="width: 50%">
                                            Busqueda
                                            <asp:TextBox ID="Txt_Fecha_Recoleccion_Busqueda" runat="server" Width="60%" TabIndex="7"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Escolaridad" runat="server" WatermarkCssClass="watermarked"
                                                WatermarkText="<Ingrese la Fecha>" TargetControlID="Txt_Fecha_Recoleccion_Busqueda" />
                                            <cc1:FilteredTextBoxExtender ID="FTE_Fecha_Recoleccion_Busqueda" runat="server" TargetControlID="Txt_Fecha_Recoleccion_Busqueda"
                                                FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="/_" />
                                            <cc1:CalendarExtender ID="DTP_Fecha_Recoleccion_Busqueda" runat="server" PopupButtonID="Btn_Fecha_Recoleccion_Busqueda"
                                                TargetControlID="Txt_Fecha_Recoleccion_Busqueda" Format="dd/MMM/yyyy" />
                                            <asp:ImageButton ID="Btn_Fecha_Recoleccion_Busqueda" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                                ToolTip="Seleccione la Fecha de Busqueda" />
                                            <cc1:MaskedEditExtender ID="MEE_Fecha_Recoleccion_Busqueda" Mask="99/LLL/9999" runat="server"
                                                MaskType="None" UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/"
                                                TargetControlID="Txt_Fecha_Recoleccion_Busqueda" Enabled="True" ClearMaskOnLostFocus="false" />
                                            <asp:ImageButton ID="Btn_Buscar_Recolecciones" runat="server" ToolTip="Consultar"
                                                TabIndex="6" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Recolecciones_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="Upnl_Datos_Generales_Recoleccion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="Pnl_Datos_Generales" runat="server" GroupingText="Datos Generales"
                            Width="97%" BackColor="White">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        Cajero
                                    </td>
                                    <td colspan="3" style="width: 80%; text-align: left;">
                                        <asp:TextBox ID="Txt_Nombre_Cajero" runat="server" ReadOnly="true" Width="98%" BorderStyle="Solid"
                                            BorderWidth="1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        Unidad Responsable
                                    </td>
                                    <td colspan="3" style="width: 80%; text-align: left;">
                                        <asp:TextBox ID="Txt_Unidad_Responsable" runat="server" ReadOnly="true" Width="98%"
                                            BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        Modulo
                                    </td>
                                    <td colspan="3" style="width: 80%; text-align: left;">
                                        <asp:TextBox ID="Txt_Modulo" runat="server" ReadOnly="true" Width="98%" BorderStyle="Solid"
                                            BorderWidth="1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        Caja
                                    </td>
                                    <td style="width: 30%; text-align: left;">
                                        <asp:TextBox ID="Txt_Caja_Recoleccion" runat="server" ReadOnly="true" Width="98%"
                                            BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        No. Recolección
                                    </td>
                                    <td style="width: 30%; text-align: left;">
                                        <asp:TextBox ID="Txt_Numero_Recolecciones" runat="server" ReadOnly="true" Width="95%"
                                            BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        Fecha
                                    </td>
                                    <td style="width: 30%; text-align: left;">
                                        <asp:TextBox ID="Txt_Fecha_Recoleccion" runat="server" ReadOnly="true" Width="98%"
                                            BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        *Monto
                                    </td>
                                    <td style="width: 30%; text-align: left;">
                                        <asp:TextBox ID="Txt_Monto_Recolectado" runat="server" Width="95%" Style="text-align: right"
                                            Enabled="true" BorderStyle="Solid" BorderWidth="1">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        &nbsp;
                                    </td>
                                    <td style="width: 30%; text-align: left;">
                                        &nbsp;
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        Total Tarjeta
                                    </td>
                                    <td style="width: 30%; text-align: left;">
                                        <asp:TextBox ID="Txt_Conteo_Tarjeta" runat="server" Width="24%" Style="text-align: right"
                                            BorderStyle="Solid" BorderWidth="1" ReadOnly="true" />
                                        <asp:TextBox ID="Txt_Total_Tarjeta" runat="server" Width="68%" Style="text-align: right"
                                            BorderStyle="Solid" BorderWidth="1" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        &nbsp;
                                    </td>
                                    <td style="width: 30%; text-align: left;">
                                        &nbsp;
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        Total Cheques
                                    </td>
                                    <td style="width: 30%; text-align: left;">
                                        <asp:TextBox ID="Txt_Conteo_Cheques" runat="server" Width="24%" Style="text-align: right"
                                            BorderStyle="Solid" BorderWidth="1" ReadOnly="true" />
                                        <asp:TextBox ID="Txt_Total_Cheques" runat="server" Width="68%" Style="text-align: right"
                                            BorderStyle="Solid" BorderWidth="1" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        &nbsp;
                                    </td>
                                    <td style="width: 30%; text-align: left;">
                                        &nbsp;
                                    </td>
                                    <td style="width: 20%; text-align: left;">
                                        Total Transferencia
                                    </td>
                                    <td style="width: 30%; text-align: left;">
                                        <asp:TextBox ID="Txt_Conteo_Transferencias" runat="server" Width="24%" Style="text-align: right"
                                            BorderStyle="Solid" BorderWidth="1" ReadOnly="true" />
                                        <asp:TextBox ID="Txt_Total_Transferencias" runat="server" Width="68%" Style="text-align: right"
                                            BorderStyle="Solid" BorderWidth="1" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; text-align: left;">
                                        *Recibe Efectivo
                                    </td>
                                    <td colspan="3" style="width: 80%; text-align: left;">
                                        <asp:TextBox ID="Txt_Nombre_Recibe_Efectivo" runat="server" Width="98%" BorderStyle="Solid"
                                            BorderWidth="1"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div id="Div_Denominaciones" style="width: 98%;" visible="false">
                                <table width="100%" border="0" cellspacing="0" class="estilo_fuente">
                                    <tr>
                                        <td style="width: 50%; text-align: left; vertical-align: top;">
                                            <asp:UpdatePanel ID="Upnl_Datos_Generales_Billetes" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="Pnl_Billetes" runat="server" GroupingText="Billetes" Width="97%" BackColor="White">
                                                        <table width="98%" class="estilo_fuente">
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $1000
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Billete_1000" runat="server" Width="98%" MaxLength="3" TabIndex="4"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="1000" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Billete_1000" runat="server" TargetControlID="Txt_Billete_1000"
                                                                        FilterType="Custom, Numbers" ValidChars=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $500
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Billete_500" runat="server" Width="98%" MaxLength="3" TabIndex="5"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="500" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Billete_500" runat="server" TargetControlID="Txt_Billete_500"
                                                                        FilterType="Custom, Numbers" ValidChars=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $200
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Billetes_200" runat="server" Width="98%" MaxLength="3" TabIndex="6"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="200" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Billetes_200" runat="server" TargetControlID="Txt_Billetes_200"
                                                                        FilterType="Custom, Numbers" ValidChars=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $100
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Billetes_100" runat="server" Width="98%" MaxLength="3" TabIndex="7"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="100" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Billetes_100" runat="server" TargetControlID="Txt_Billetes_100"
                                                                        FilterType="Custom, Numbers" ValidChars=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $50
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Billetes_50" runat="server" Width="98%" MaxLength="3" TabIndex="8"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="50" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Billetes_50" runat="server" TargetControlID="Txt_Billetes_50"
                                                                        FilterType="Custom, Numbers" ValidChars=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $20
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Billetes_20" runat="server" Width="98%" MaxLength="3" TabIndex="9"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="20" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Billetes_20" runat="server" TargetControlID="Txt_Billetes_20"
                                                                        FilterType="Custom, Numbers" ValidChars=" " />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td style="width: 50%; text-align: left; vertical-align: top;">
                                            <asp:UpdatePanel ID="Upnl_Datos_Generales_Monedas" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="Pnl_Monedas" runat="server" GroupingText="Monedas" Width="97%" BackColor="White">
                                                        <table width="98%" class="estilo_fuente">
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $20
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Monedas_20" runat="server" Width="98%" MaxLength="3" TabIndex="10"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="20" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Monedas_20" runat="server" TargetControlID="Txt_Monedas_20"
                                                                        FilterType="Custom, Numbers" ValidChars=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $10
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Monedas_10" runat="server" Width="98%" MaxLength="3" TabIndex="11"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="10" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Monedas_10" runat="server" TargetControlID="Txt_Monedas_10"
                                                                        FilterType="Custom, Numbers" ValidChars=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $5
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Monedas_5" runat="server" Width="98%" MaxLength="3" TabIndex="12"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="5" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Monedas_5" runat="server" TargetControlID="Txt_Monedas_5"
                                                                        FilterType="Custom, Numbers" ValidChars=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $2
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Monedas_2" runat="server" Width="98%" MaxLength="3" TabIndex="13"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="2" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Monedas_2" runat="server" TargetControlID="Txt_Monedas_2"
                                                                        FilterType="Custom, Numbers" ValidChars=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $1
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Monedas_1" runat="server" Width="98%" MaxLength="8" TabIndex="14"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="1" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Monedas_1" runat="server" TargetControlID="Txt_Monedas_1"
                                                                        FilterType="Custom, Numbers" ValidChars=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $0.50
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Moneda_050" runat="server" Width="98%" MaxLength="3" TabIndex="15"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="0.50" />
                                                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_Moneda_050" runat="server" TargetControlID="Txt_Moneda_050"
                                                                        FilterType="Custom, Numbers" ValidChars=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $0.20
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Moneda_020" runat="server" Width="98%" MaxLength="10" TabIndex="16"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="0.20" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Moneda_020" runat="server" TargetControlID="Txt_Moneda_020"
                                                                        FilterType="Custom, Numbers" ValidChars=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $0.10
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Moneda_010" runat="server" Width="98%" MaxLength="11" TabIndex="17"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="0.10" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Moneda_010" runat="server" TargetControlID="Txt_Moneda_010"
                                                                        FilterType="Custom, Numbers" ValidChars=" " />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table width="100%" border="0" cellspacing="0" class="estilo_fuente">
                                <tr align="center">
                                    <td>
                                        <asp:GridView ID="Grid_Recolecciones_Caja" runat="server" AllowPaging="True" Width="100%"
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" PageSize="5"
                                            AllowSorting="True" HeaderStyle-CssClass="tblHead" OnPageIndexChanging="Grid_Recolecciones_Caja_PageIndexChanging"
                                            OnSelectedIndexChanged="Grid_Recolecciones_Caja_SelectedIndexChanged" EnableModelValidation="True">
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="7%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="Num_Recoleccion" HeaderText="No" Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" Width="7%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Caja" HeaderText="Caja" Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Modulo" HeaderText="Modulo" Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Fecha" HeaderText="Fecha Hora" Visible="True" SortExpression="Fecha"
                                                    DataFormatString="{0:dd/MMM/yyyy HH:mm:ss}">
                                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Monto_Recolectado" HeaderText="Monto" Visible="True">
                                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Right" Width="15%" CssClass="text_cantidades_grid" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="No_Recoleccion" HeaderText="No_Recoleccion" Visible="True">
                                                    <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Right" Width="0%" CssClass="text_cantidades_grid" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Recibe_Efectivo" HeaderText="Recibe Efectivo" />
                                                <asp:BoundField DataField="Conteo_Tarjeta" HeaderText="Conteo Tarjeta" />
                                                <asp:BoundField DataField="Monto_Tarjeta" HeaderText="Monto_Tarjeta" />
                                                <asp:BoundField DataField="Conteo_Cheque" HeaderText="Conteo_Cheques" />
                                                <asp:BoundField DataField="Monto_Cheque" HeaderText="Monto Cheques" />
                                                <asp:BoundField DataField="Conteo_Transferencia" HeaderText="Conteo Transferencia" />
                                                <asp:BoundField DataField="Monto_Transferencia" HeaderText="Monto Transferencia" />
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

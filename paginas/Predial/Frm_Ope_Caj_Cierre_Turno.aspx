<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Caj_Cierre_Turno.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Caj_Cierre_Turno"
    Title="Cierre de Turno" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type="text/javascript" language="javascript">
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
                $('input[id$=Txt_Total_Efectivo]').val(Numero.Formato(2, true));
                $("input[id$=Hdn_Efectivo_Caja]").val(Numero.Formato(2, true));
            }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
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
                        <td colspan="4" class="label_titulo">
                            Cierre de Turno
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
                                        <td align="left" style="width: 50%;">
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Guardar" CssClass="Img_Button"
                                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click" />
                                            <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" OnClick="Btn_Imprimir_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td align="right" style="width: 50%;">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:UpdatePanel ID="Upnl_Datos_Generales_Empleado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="Datos Generales del Cajero" Width="97%"
                            BackColor="White">
                            <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                                <tr>
                                    <td style="text-align: left; width: 20%;">
                                        No Empleado
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_No_Empleado" runat="server" ReadOnly="True" Width="98%" BorderStyle="Solid"
                                            BorderWidth="1" />
                                    </td>
                                    <td style="text-align: left; width: 50%;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 20%;">
                                        Nombre
                                    </td>
                                    <td colspan="2" style="text-align: left; width: 80%;">
                                        <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" ReadOnly="True" Width="99%"
                                            BorderStyle="Solid" BorderWidth="1" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:UpdatePanel ID="Upnl_Datos_Generales_Apertura_Turno" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="Pnl_Datos_Generales" runat="server" GroupingText="Datos Generales de Apertura"
                            Width="97%" BackColor="White">
                            <table width="98%" class="estilo_fuente">
                                <tr>
                                    <td style="text-align: left; width: 20%;">
                                        Módulo
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Modulo_Caja_Empleado" runat="server" Width="98%" ReadOnly="true"
                                            BorderStyle="Solid" BorderWidth="1" />
                                    </td>
                                    <td style="text-align: left; width: 20%;">
                                        Caja
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Caja_Empleado" runat="server" Width="98%" ReadOnly="true" BorderStyle="Solid"
                                            BorderWidth="1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 20%;">
                                        Fecha Apertura
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Fecha_Apertura_Turno" runat="server" Width="98%" ReadOnly="true"
                                            BorderStyle="Solid" BorderWidth="1" />
                                    </td>
                                    <td style="text-align: left; width: 20%;">
                                        Hora
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Hora_Apertura_Turno" runat="server" Width="98%" ReadOnly="true"
                                            BorderStyle="Solid" BorderWidth="1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 20%;">
                                        Fecha Aplicación
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Fecha_Aplicacion_Turno" runat="server" Width="98%" ReadOnly="true"
                                            BorderStyle="Solid" BorderWidth="1" />
                                    </td>
                                    <td style="text-align: left; width: 20%;">
                                        *Fondo Inicial
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Fondo_Inicial_Turno" runat="server" Width="98%" CssClass="text_cantidades_grid"
                                            ReadOnly="true" BorderStyle="Solid" BorderWidth="1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 20%;">
                                        Recibo Inicial
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Recibo_Inicial_Turno" runat="server" Width="98%" ReadOnly="true"
                                            BorderStyle="Solid" BorderWidth="1" />
                                    </td>
                                    <td style="text-align: left; width: 20%;">
                                        Recibo Final
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Recibo_Final_Turno" runat="server" Width="98%" ReadOnly="true"
                                            BorderStyle="Solid" BorderWidth="1" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:UpdatePanel ID="Upnl_Datos_Caja" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="Pnl_Datos_Caja" runat="server" GroupingText="Datos Generales del Cierre"
                            Width="97%" BackColor="White">
                            <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                                <tr>
                                    <td style="text-align: left; width: 20%;">
                                        No Turno
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_No_Turno" runat="server" Width="98%" ReadOnly="true" BorderStyle="Solid"
                                            BorderWidth="1" />
                                    </td>
                                    <td style="text-align: left; width: 20%;">
                                        Estatus
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Estatus_Turno" runat="server" Width="98%" ReadOnly="true" BorderStyle="Solid"
                                            BorderWidth="1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 20%;">
                                        Fecha Cierre
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Fecha_Cierre_Turno" runat="server" Width="98%" ReadOnly="true"
                                            BorderStyle="Solid" BorderWidth="1" />
                                    </td>
                                    <td style="text-align: left; width: 20%;">
                                        Hora Cierre
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Hora_Cierre_Turno" runat="server" Width="98%" ReadOnly="true"
                                            BorderStyle="Solid" BorderWidth="1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;text-align:left;">*Recibe Efectivo</td>
                                    <td colspan="3" style="width:80%;text-align:left;">
                                        <asp:TextBox ID="Txt_Nombre_Recibe_Efectivo" runat="server" Width="99%" BorderStyle="Solid" BorderWidth="1"></asp:TextBox> 
                                    </td>
                                </tr>  
                            </table>
                            <br />
                            <div id="Div_Denominaciones" style="width: 98%;" visible="false">
                                <table width="100%" border="0" cellspacing="0" class="estilo_fuente">
                                    <tr style="background-color: #36C;">
                                        <td style="width: 100%; text-align: left; font-size: 15px; color: #FFF;" colspan="2">
                                            Denominaciones
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%; text-align: left; vertical-align: top;">
                                            <asp:UpdatePanel ID="Upnl_Datos_Generales_Billetes" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="Pnl_Billetes" runat="server" GroupingText="Billetes" Width="97%" BackColor="White">
                                                        <table width="98%" class="estilo_fuente">
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $&nbsp;&nbsp;1000.00
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Billetes_1000" runat="server" Width="98%" MaxLength="3" TabIndex="4"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="1000" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Billetes_1000" runat="server" TargetControlID="Txt_Billetes_1000"
                                                                        FilterType="Custom, Numbers" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $&nbsp;&nbsp;&nbsp;500.00
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Billetes_500" runat="server" Width="98%" MaxLength="3" TabIndex="5"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="500" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Billetes_500" runat="server" TargetControlID="Txt_Billetes_500"
                                                                        FilterType="Custom, Numbers" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $&nbsp;&nbsp;&nbsp;200.00
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Billetes_200" runat="server" Width="98%" MaxLength="3" TabIndex="6"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="200" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Billetes_200" runat="server" TargetControlID="Txt_Billetes_200"
                                                                        FilterType="Custom, Numbers" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $&nbsp;&nbsp;&nbsp;100.00
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Billetes_100" runat="server" Width="98%" MaxLength="3" TabIndex="7"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="100" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Billetes_100" runat="server" TargetControlID="Txt_Billetes_100"
                                                                        FilterType="Custom, Numbers" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $&nbsp;&nbsp;&nbsp;&nbsp;50.00
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Billetes_50" runat="server" Width="98%" MaxLength="3" TabIndex="8"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="50" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Billetes_50" runat="server" TargetControlID="Txt_Billetes_50"
                                                                        FilterType="Custom, Numbers" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $&nbsp;&nbsp;&nbsp;&nbsp;20.00
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Billetes_20" runat="server" Width="98%" MaxLength="3" TabIndex="9"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="20" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Billetes_20" runat="server" TargetControlID="Txt_Billetes_20"
                                                                        FilterType="Custom, Numbers" />
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
                                                                    $&nbsp;&nbsp;&nbsp;&nbsp;20.00
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Monedas_20" runat="server" Width="98%" MaxLength="3" TabIndex="10"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="20" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Monedas_20" runat="server" TargetControlID="Txt_Monedas_20"
                                                                        FilterType="Custom, Numbers" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $&nbsp;&nbsp;&nbsp;&nbsp;10.00
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Monedas_10" runat="server" Width="98%" MaxLength="3" TabIndex="11"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="10" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Monedas_10" runat="server" TargetControlID="Txt_Monedas_10"
                                                                        FilterType="Custom, Numbers" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;5.00
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Monedas_5" runat="server" Width="98%" MaxLength="3" TabIndex="12"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="5" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Monedas_5" runat="server" TargetControlID="Txt_Monedas_5"
                                                                        FilterType="Custom, Numbers" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;2.00
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Monedas_2" runat="server" Width="98%" MaxLength="3" TabIndex="13"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="2" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Monedas_2" runat="server" TargetControlID="Txt_Monedas_2"
                                                                        FilterType="Custom, Numbers" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;1.00
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Monedas_1" runat="server" Width="98%" MaxLength="8" TabIndex="14"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="1" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Monedas_1" runat="server" TargetControlID="Txt_Monedas_1"
                                                                        FilterType="Custom, Numbers" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;0.50
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Monedas_050" runat="server" Width="98%" MaxLength="3" TabIndex="15"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="0.50" />
                                                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_Monedas_050" runat="server" TargetControlID="Txt_Monedas_050"
                                                                        FilterType="Custom, Numbers" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;0.20
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Monedas_020" runat="server" Width="98%" MaxLength="10" TabIndex="16"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="0.20" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Monedas_020" runat="server" TargetControlID="Txt_Monedas_020"
                                                                        FilterType="Custom, Numbers" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%;">
                                                                    $&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;0.10
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Monedas_010" runat="server" Width="98%" MaxLength="11" TabIndex="17"
                                                                        CssClass="text_cantidades_grid" Style="cursor: default;" onkeyup="javascript:Sumar();"
                                                                        title="0.10" />
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Monedas_010" runat="server" TargetControlID="Txt_Monedas_010"
                                                                        FilterType="Custom, Numbers" />
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
                            <div id="Div_Totales_Turno" style="width: 98%;" visible="false">
                                <table width="100%" border="0" cellspacing="0" class="estilo_fuente">
                                    <tr>
                                        <td style="width: 50%; text-align: left; vertical-align: top;">
                                        </td>
                                        <td style="width: 100%; background-color: #36C; text-align: left; font-size: 15px;
                                            color: #FFF;">
                                            Total Registrado
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%; text-align: left; vertical-align: top;">
                                        </td>
                                        <td style="width: 50%; text-align: left; vertical-align: top;">
                                            <asp:UpdatePanel ID="Upnl_Totales_Turno" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="Pnl_Totales_Turno" runat="server" GroupingText="Total Pagado" Width="97%"
                                                        BackColor="White">
                                                        <table width="98%" class="estilo_fuente">
                                                            <tr>
                                                                <td style="text-align: left; width: 50%; font-family: Tahoma; font-size: 13px; font-weight: bold;">
                                                                    Total Efectivo en Caja
                                                                </td>
                                                                <td style="text-align: right; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Total_Efectivo" runat="server" Width="98%" ReadOnly="true" Style="font-weight: bolder;
                                                                        font-size: medium; text-align: right;" BorderWidth="1" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%; font-family: Tahoma; font-size: 13px; font-weight: bold;">
                                                                    Cortes Parciales Efectivo
                                                                </td>
                                                                <td style="text-align: right; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Total_Cortes_Parciales" runat="server" Width="98%" ReadOnly="true"
                                                                        Style="font-weight: bolder; font-size: medium; text-align: right;" BorderWidth="1" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%; font-family: Tahoma; font-size: 13px; font-weight: bold;">
                                                                    Cortes Parciales Tarjeta
                                                                </td>
                                                                <td style="text-align: right; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Parcial_Conteo_Bancos" runat="server" Width="24%" ReadOnly="true"
                                                                        Style="font-weight: bolder; font-size: medium; text-align: right;" BorderWidth="1" />
                                                                    <asp:TextBox ID="Txt_Parcial_Total_Bancos" runat="server" Width="69%" ReadOnly="true"
                                                                        Style="font-weight: bolder; font-size: medium; text-align: right;" BorderWidth="1" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%; font-family: Tahoma; font-size: 13px; font-weight: bold;">
                                                                    Cortes Parciales Cheques
                                                                </td>
                                                                <td style="text-align: right; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Parcial_Conteo_Cheques" runat="server" Width="24%" ReadOnly="true"
                                                                        Style="font-weight: bolder; font-size: medium; text-align: right;" BorderWidth="1" />
                                                                    <asp:TextBox ID="Txt_Parcial_Total_Cheques" runat="server" Width="69%" ReadOnly="true"
                                                                        Style="font-weight: bolder; font-size: medium; text-align: right;" BorderWidth="1" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%; font-family: Tahoma; font-size: 13px; font-weight: bold;">
                                                                    Cortes Parciales Transferencias
                                                                </td>
                                                                <td style="text-align: right; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Parcial_Conteo_Transferencias" runat="server" Width="24%" ReadOnly="true"
                                                                        Style="font-weight: bolder; font-size: medium; text-align: right;" BorderWidth="1" />
                                                                    <asp:TextBox ID="Txt_Parcial_Total_Transferencias" runat="server" Width="69%" ReadOnly="true"
                                                                        Style="font-weight: bolder; font-size: medium; text-align: right;" BorderWidth="1" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%; font-family: Tahoma; font-size: 13px; font-weight: bold;">
                                                                    Total Bancos
                                                                </td>
                                                                <td style="text-align: right; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Conteo_Bancos" runat="server" Width="24%" ReadOnly="true" Style="font-weight: bolder;
                                                                        font-size: medium; text-align: right;" BorderWidth="1" />
                                                                    <asp:TextBox ID="Txt_Total_Bancos" runat="server" Width="69%" ReadOnly="true" Style="font-weight: bolder;
                                                                        font-size: medium; text-align: right;" BorderWidth="1" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%; font-family: Tahoma; font-size: 13px; font-weight: bold;">
                                                                    Total Cheques
                                                                </td>
                                                                <td style="text-align: right; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Conteo_Cheques" runat="server" Width="24%" ReadOnly="true" Style="font-weight: bolder;
                                                                        font-size: medium; text-align: right;" BorderWidth="1" />
                                                                    <asp:TextBox ID="Txt_Total_Cheques" runat="server" ReadOnly="true" Width="69%" Style="font-weight: bolder;
                                                                        font-size: medium; text-align: right;" BorderWidth="1" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%; font-family: Tahoma; font-size: 13px; font-weight: bold;">
                                                                    Total Transferencia
                                                                </td>
                                                                <td style="text-align: right; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Conteo_Transferencias" runat="server" Width="24%" ReadOnly="true"
                                                                        Style="font-weight: bolder; font-size: medium; text-align: right;" BorderWidth="1" />
                                                                    <asp:TextBox ID="Txt_Total_Transferencia" runat="server" ReadOnly="true" Width="69%"
                                                                        Style="font-weight: bolder; font-size: medium; text-align: right;" BorderWidth="1" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%; font-family: Tahoma; font-size: 13px; font-weight: bold;">
                                                                    Monto Total
                                                                </td>
                                                                <td style="text-align: right; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Monto_Total" runat="server" ReadOnly="true" Width="98%" Style="font-weight: bolder;
                                                                        font-size: medium; text-align: right;" BorderWidth="1" ForeColor="Red" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 100%;" colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left; width: 50%; font-family: Tahoma; font-size: 13px; font-weight: bold;">
                                                                    <asp:Label ID="Lbl_Sobrante_Faltante" runat="server" Text="Sobrante/Faltante"></asp:Label>
                                                                </td>
                                                                <td style="text-align: left; width: 50%;">
                                                                    <asp:TextBox ID="Txt_Sobrante_Faltante" runat="server" ReadOnly="true" Width="98%"
                                                                        Style="font-weight: bolder; font-size: medium; text-align: right;" BorderWidth="1" />
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
                            <asp:HiddenField ID="Hdn_Caja_ID" runat="server" />
                            <asp:HiddenField ID="Hdn_Efectivo_Sistema" runat="server" />
                            <asp:HiddenField ID="Hdn_No_Recolecciones" runat="server" />
                            <asp:HiddenField ID="Hdn_Efectivo_Caja" runat="server" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

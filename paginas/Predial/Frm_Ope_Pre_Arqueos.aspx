<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Arqueos.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Arqueos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type="text/javascript" language="javascript">

        function Abrir_Modal_Popup() {
            $find('Busqueda_Empleados').show();
            return false;
        }
        function Cerrar_Popup() {
             $find("Busqueda_Empleados").hide();
             return false;
         }
        function Limpiar_Ctlr(){
            return false;
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

        function Suma() {
            var Total_Caja = parseFloat($("input[id$=Txt_Total_Caja]").val().replace(',',''));
            var Total = 0.00;
            var Cantidad =  parseFloat($("input[id$=Txt_Denom_10_Cent]").val())* 0.1;
            Total = Total + Cantidad ;
            Cantidad = parseFloat($("input[id$=Txt_Denom_20_Cent]").val())*0.2;
            Total = Total + Cantidad ;
             Cantidad =  parseFloat($("input[id$=Txt_Denom_50_Cent]").val())*0.5;
            Total = Total + Cantidad ;
             Cantidad = parseFloat($("input[id$=Txt_Denom_1_Peso]").val())*1;
            Total = Total + Cantidad ;
             Cantidad = parseFloat($("input[id$=Txt_Denom_2_Pesos]").val())*2;
            Total = Total + Cantidad ;
             Cantidad = parseFloat($("input[id$=Txt_Denom_5_Pesos]").val())*5;
            Total = Total + Cantidad ;
            Cantidad =  parseFloat($("input[id$=Txt_Denom_10_Pesos]").val())*10;
            Total = Total + Cantidad ;
            Cantidad = parseFloat($("input[id$=Txt_Denom_20_Pesos]").val())*20;
            Total = Total + Cantidad ;
            Cantidad = parseFloat($("input[id$=Txt_Denom_50_Pesos]").val())*50;
            Total = Total + Cantidad ;
            Cantidad = parseFloat($("input[id$=Txt_Denom_100_Pesos]").val())*100;
            Total = Total + Cantidad ;
            Cantidad = parseFloat($("input[id$=Txt_Denom_200_Pesos]").val())*200;
            Total = Total + Cantidad ;
            Cantidad = parseFloat($("input[id$=Txt_Denom_500_Pesos]").val())*500;
            Total = Total + Cantidad ;
            Cantidad = parseFloat($("input[id$=Txt_Denom_1000_Pesos]").val())*1000;
            Total = Total + Cantidad ;

            var Total_Diferencia = Total - Total_Caja;
            var Diferencia = new Formatear_Numero(Total_Diferencia); //obtenemos el objeto del total para poder formatearlo

            var Numero = new Formatear_Numero(Total); //obtenemos el objeto del total para poder formatearlo
            $("input[id$=Txt_Total]").val(Numero.Formato(2, true));

            $("input[id$=Txt_Diferencia]").val(Diferencia.Formato(2, true));
            $("input[id$=Hfd_Diferencia]").val(Diferencia.Formato(2, true));
            
            if (Total_Diferencia < 0) {
                $('[id$=Lbl_Diferencia]').text("Faltante");
                $('[id$=Lbl_Diferencia]').css("color", "red")
                $("input[id$=Txt_Diferencia]").css("color", "red");
            }
            else {
                $('[id$=Lbl_Diferencia]').text("Sobrante");
                $('[id$=Lbl_Diferencia]').css("color", "green")
                $("input[id$=Txt_Diferencia]").css("color", "green");
            }
        }
        function Validar_Cantidad_Caracteres(){
            var limit = 250;
            $('textarea[id$=Txt_Comentarios]').keyup(function() {
                var len = $(this).val().length;
                if (len > limit) {
                    alert("LLego al limite de caracteres");
                    this.value = this.value.substring(0, limit);
                }
            });
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
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Arqueos
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                            <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:ImageButton ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
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
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td>
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                                TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click" />
                                            <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                                                TabIndex="4" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" OnClick ="Btn_Imprimir_Click" />
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
                <br />
                
                <div id="Div_Arqueos" runat="server">
                    <table width="98%" class="estilo_fuente">
                    <tr align="center">
                        <td>
                            <asp:GridView ID="Grid_Arqueos" runat="server" AllowPaging="True" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="5" Width="100%" EmptyDataText="&quot;No se encontraron registros&quot;"
                                AllowSorting="True" HeaderStyle-CssClass="tblHead" Style="white-space: normal;"
                                OnPageIndexChanging="Grid_Arqueos_PageIndexChanging" OnSelectedIndexChanged="Grid_Arqueos_SelectedIndexChanged">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="NO_ARQUEO" HeaderText="No. Arqueo">
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="REALIZO" HeaderText="Realizó">
                                        <HeaderStyle Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MODULO" HeaderText="Módulo">
                                        <HeaderStyle Width="4%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CAJA" HeaderText="Caja">
                                        <HeaderStyle Width="4%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CAJERO" HeaderText="Cajero">
                                        <HeaderStyle Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MONTO" HeaderText="Monto" DataFormatString="{0:c2}">
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha/Hora" DataFormatString="{0:dd/MMM/yyyy HH:mm:ss tt}">
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
                    </table>
                 </div>
                <div id="Div_Arqueos_Detalles" runat="server" >
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td colspan="4">
                                <asp:HiddenField ID="Hf_No_Turno" runat="server" /><asp:HiddenField ID="Hf_No_Arqueo" runat="server" />
                                 <asp:HiddenField ID="Hf_Realizo" runat="server" /><asp:HiddenField ID="Hf_No_Empleado" runat="server" />
                                 <asp:HiddenField ID="Hf_Caja_ID" runat="server" /><asp:HiddenField ID="Hf_No_Caja" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 15%;">Número de caja</td>
                            <td style="text-align: left; width: 35%;">
                                <asp:TextBox ID="Txt_No_Caja" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Fondo Inicial&nbsp;&nbsp;
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fondo_Inicial" runat="server" Width="96.4%" style="text-align:right "/>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;"> Cajero</td>
                            <td style="text-align: left; width: 30%;">        
                                <asp:TextBox ID="Txt_Cajero" runat="server" Width="96.4%" TabIndex="7">
                                </asp:TextBox>
                            </td>
                            <td style="text-align: right ; width: 20%;">
                                Total Efectivo &nbsp;&nbsp;
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Total_Cobrado" runat="server" Width="96.4%" style="text-align:right;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 15%;">Módulo</td>
                            <td style="text-align: left; width: 35%;">
                                <asp:TextBox ID="Txt_Modulo" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: right ; width: 20%;">
                                Total Parcial&nbsp;&nbsp;
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Total_Recolectado" runat="server" Width="96.4%" style="text-align:right "/>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 15%;">&nbsp;</td>
                            <td style="text-align: left; width: 35%;">
                                &nbsp;
                            </td>
                            <td style="text-align: right ; width: 20%;">
                                Total Tarjeta&nbsp;&nbsp;
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Total_Tarjeta" runat="server" Width="96.4%" style="text-align:right "/>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 15%;">&nbsp;</td>
                            <td style="text-align: left; width: 35%;">
                                &nbsp;
                            </td>
                            <td style="text-align: right ; width: 20%;">
                                Total Cheques&nbsp;&nbsp;
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Total_Cheques" runat="server" Width="96.4%" style="text-align:right "/>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 15%;">&nbsp;</td>
                            <td style="text-align: left; width: 35%;">
                                &nbsp;
                            </td>
                            <td style="text-align: right ; width: 20%;">
                                Total Transferencias&nbsp;&nbsp;
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Total_Transferencias" runat="server" Width="96.4%" style="text-align:right "/>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 15%;">
                                *Realizó
                            </td>
                            <td style="text-align:left; width: 30%; vertical-align:middle;">
                                <asp:TextBox ID="Txt_Realizo" runat="server" Width="84%" TabIndex="12" MaxLength="11"
                                    Height="18px" />
                                <asp:ImageButton ID="Btn_Realizo" runat="server" Height="24px" OnClientClick="javascript:return Abrir_Modal_Popup();"
                                    ImageUrl="../imagenes/paginas/sias_add.png" TabIndex="10" ToolTip="Autentificarse"  style="vertical-align:middle;"/>
                            </td>
                            <td style="text-align: right ; width: 20%;">
                                Total Caja&nbsp;&nbsp;
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Total_Caja" runat="server" Width="96.4%" 
                                    style="text-align:right "/>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 15%;">
                                &nbsp;
                            </td>
                            <td style="text-align:left; width: 30%; vertical-align:middle;">
                                &nbsp;
                            </td>
                            <td style="text-align: right ; width: 20%;">
                                Total General&nbsp;&nbsp;
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Total_General" runat="server" Width="96.4%" 
                                    style="text-align:right "/>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 15%; vertical-align: top;">
                                *Comentarios
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%; vertical-align: top;">
                                <asp:TextBox ID="Txt_Comentarios" runat="server" TabIndex="10" MaxLength="250" TextMode="MultiLine"
                                    Width="98.6%" onkeyup="javascript:Validar_Cantidad_Caracteres();"/>
                                <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Comentarios" runat="server" WatermarkCssClass="watermarked"
                                 WatermarkText="Límite de Caractes 250" TargetControlID="Txt_Comentarios" 
                                 Enabled="True">
                                </cc1:TextBoxWatermarkExtender>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios" runat="server" TargetControlID="Txt_Comentarios"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                            </td>
                        </tr>
                       
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <%------------------ Recolecciones ------------------%>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Entregas Parciales
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="4">
                                <asp:GridView ID="Grid_Recolecciones" runat="server" AllowPaging="True" CssClass="GridView_1"
                                    AutoGenerateColumns="False" PageSize="5" Width="100%" EmptyDataText="&quot;No se encontraron registros&quot;"
                                     AllowSorting="True"
                                    HeaderStyle-CssClass="tblHead" Style="white-space: normal;">
                                    <Columns>
                                        <asp:BoundField DataField="NO_RECOLECCION" HeaderText="No. Entrega">
                                            <HeaderStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RECOLECTO" HeaderText="Recolectó">
                                            <HeaderStyle Width="30%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MODULO" HeaderText="Módulo">
                                            <HeaderStyle Width="30%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CAJA" HeaderText="Caja">
                                            <HeaderStyle Width="7%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CAJERO" HeaderText="Cajero">
                                            <HeaderStyle Width="30%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MONTO" HeaderText="Monto" DataFormatString="{0:c2}">
                                            <HeaderStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}">
                                            <HeaderStyle Width="10%" />
                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <%------------------ Denominaciones ------------------%>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Denominaciones
                            </td>
                        </tr>
                    </table>
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td style="width:50%">
                                <asp:Panel GroupingText="Monedas" runat="server">
                                <table>
                                    <tr>
                                        <td style="text-align: left;">$ 10</td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="Txt_Denom_10_Pesos" runat="server" Text="0" Style="width: 40px;
                                                text-align: center;" onBlur="javascript:Suma();"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Denom_10_Pesos" runat="server" FilterType="Numbers"
                                                   ValidChars = "0123456789" TargetControlID="Txt_Denom_10_Pesos" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;"> $ 5</td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="Txt_Denom_5_Pesos" runat="server" Text="0" Style="width: 40px; text-align: center;"
                                                onBlur="javascript:Suma();"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Denom_5_Pesos" runat="server" FilterType="Numbers"
                                                   ValidChars = "0123456789" TargetControlID="Txt_Denom_5_Pesos" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">$ 2</td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="Txt_Denom_2_Pesos" runat="server" Text="0" Style="width: 40px; text-align: center;"
                                                onBlur="javascript:Suma();"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Denom_2_Pesos" runat="server" FilterType="Numbers"
                                                   ValidChars = "0123456789" TargetControlID="Txt_Denom_2_Pesos" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">$ 1</td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="Txt_Denom_1_Peso" runat="server" Text="0" Style="width: 40px; text-align: center;"
                                                onBlur="javascript:Suma();"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Denom_1_Peso" runat="server" FilterType="Numbers"
                                                   ValidChars = "0123456789" TargetControlID="Txt_Denom_1_Peso" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left;"> $ 0.50 </td>
                                        <td style="width: 16%; text-align: left;">
                                            <asp:TextBox ID="Txt_Denom_50_Cent" runat="server" Text="0" Style="width: 40px; text-align: center;"
                                                onBlur="javascript:Suma();"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Denom_50_Cent" runat="server" FilterType="Numbers"
                                                   ValidChars = "0123456789" TargetControlID="Txt_Denom_50_Cent" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left;">$ 0.20</td>
                                        <td style="width: 16%; text-align: left;">
                                            <asp:TextBox ID="Txt_Denom_20_Cent" runat="server" Text="0" Style="width: 40px; text-align: center;"
                                                onBlur="javascript:Suma();"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Denom_20_Cent" runat="server" FilterType="Numbers"
                                                   ValidChars = "0123456789" TargetControlID="Txt_Denom_20_Cent" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%; text-align: left;">$ 0.10</td>
                                        <td style="width: 16%; text-align: left;">
                                            <asp:TextBox ID="Txt_Denom_10_Cent" runat="server" Text="0" Style="width: 40px; text-align: center;"
                                               onBlur="javascript:Suma();"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Denom_10_Cent" runat="server" FilterType="Numbers"
                                                   ValidChars = "0123456789" TargetControlID="Txt_Denom_10_Cent" />
                                        </td>
                                    </tr>
                                </table>
                                </asp:Panel>
                            </td>
                            <td style="width:50%">
                                <asp:Panel runat="server" GroupingText="Billetes">
                                    <table>
                                        <tr>
                                            <td style="text-align: left; width: 15%;">$ 1000</td>
                                            <td style="text-align: left; width: 16%;">
                                                <asp:TextBox ID="Txt_Denom_1000_Pesos" runat="server" Text="0" Style="width: 40px;
                                                    text-align: center;" onBlur="javascript:Suma();"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Denom_1000_Pesos" runat="server" FilterType="Numbers"
                                                       ValidChars = "0123456789" TargetControlID="Txt_Denom_1000_Pesos" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left;">$ 500</td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="Txt_Denom_500_Pesos" runat="server" Text="0" Style="width: 40px;
                                                    text-align: center;" onBlur="javascript:Suma();"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Denom_500_Pesos" runat="server" FilterType="Numbers"
                                                       ValidChars = "0123456789" TargetControlID="Txt_Denom_500_Pesos" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left;">$ 200</td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="Txt_Denom_200_Pesos" runat="server" Text="0" Style="width: 40px;
                                                    text-align: center;" onBlur="javascript:Suma();"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Frbe_Txt_Denom_200_Pesos" runat="server" FilterType="Numbers"
                                                       ValidChars = "0123456789" TargetControlID="Txt_Denom_200_Pesos" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left;">$ 100</td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="Txt_Denom_100_Pesos" runat="server" Text="0" Style="width: 40px;
                                                    text-align: center;" onBlur="javascript:Suma();"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Denom_100_Pesos" runat="server" FilterType="Numbers"
                                                       ValidChars = "0123456789" TargetControlID="Txt_Denom_100_Pesos" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left;"> $ 50 </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="Txt_Denom_50_Pesos" runat="server" Text="0" Style="width: 40px;
                                                    text-align: center;" onBlur="javascript:Suma();"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Denom_50_Pesos" runat="server" FilterType="Numbers"
                                                       ValidChars = "0123456789" TargetControlID="Txt_Denom_50_Pesos" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left;">$ 20</td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="Txt_Denom_20_Pesos" runat="server" Text="0" Style="width: 40px;
                                                    text-align: center;" onBlur="javascript:Suma();"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Denom_20_Pesos" runat="server" FilterType="Numbers"
                                                       ValidChars = "0123456789" TargetControlID="Txt_Denom_20_Pesos" />
                                            </td>
                                        </tr>
                                        <tr><td colspan="2">&nbsp;</td></tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <table>
                                    <tr>
                                        <td style="text-align: right; font-size:medium">Total $</td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="Txt_Total" runat="server" Text="0.00" Width="96.4%" style="text-align:right; font-size:medium;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; font-size:medium">
                                            <asp:Label ID="Lbl_Diferencia" runat="server" Text="Sobrante" 
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="Txt_Diferencia" runat="server" Text="0.00" Width="96.4%" ReadOnly="true" style="text-align:right; font-size:medium;"></asp:TextBox>
                                            <asp:HiddenField ID="Hfd_Diferencia" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
              </div>
              <cc1:ModalPopupExtender 
                ID="Mpe_Autenticacion" runat="server"
                 BackgroundCssClass="popUpStyle"
                 BehaviorID="Busqueda_Empleados" PopupControlID="Pnl_Busqueda_Contenedor" 
                 TargetControlID="Btn_Comodin_Open"
                 PopupDragHandleControlID="Pnl_Busqueda_Cabecera" 
                 CancelControlID="Btn_Comodin_Close"
                 DropShadow="True" DynamicServicePath="" Enabled="True" />
                                <asp:Button Style="background-color: transparent;  border-style: none;" ID="Btn_Comodin_Close"
                                    runat="server" Text="" />
                                <asp:Button Style="background-color: transparent;  border-style: none;" ID="Btn_Comodin_Open"
                                    runat="server" Text="" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--Modal popup para la Autenticacion de Usuarios--%>
    <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag" HorizontalAlign="Center"
        Width="300px" Style="display:none; border-style:outset; border-color:Silver;
        background-image: url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG); background-repeat:repeat-y;">
        <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" Style="cursor:move;  background-color:Silver;
            color:Black; font-size:12; font-weight:bold; border-style:outset; ">
            <table width="99%">
                <tr>
                    <td style="color: Black; font-size: 12; font-weight: bold;">
                        <asp:Image ID="Img_Informatcion_Autorizacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                        Autenticación de Usuarios
                    </td>
                    <td align="right" style="width: 10%;">
                        <asp:ImageButton ID="Btn_Cerrar_Ventana" CausesValidation="false" runat="server"
                            Style="cursor: pointer;" ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"
                            OnClick="Btn_Cerrar_Ventana_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:UpdatePanel ID="Upnl_Filtros_Autenticacion" runat="server">
            <ContentTemplate>
                <div style="color: #5D7B9D">
                    <table width="100%">
                        <tr>
                            <td align="left" style="text-align: left;">
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Autenticacion" runat="server"
                                    AssociatedUpdatePanelID="Upnl_Filtros_Autenticacion" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter1" class="progressBackgroundFilter">
                                        </div>
                                        <div style="background-color: Transparent; position: fixed; top: 50%; left: 47%;
                                            padding: 10px; z-index: 1002;" id="div_progress1">
                                            <img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table width="100%">
                                    <tr>
                                       
                                        <td style="width: 100%" colspan="4" align="right">
                                            <asp:ImageButton ID="Btn_Limpiar_Ctlr_Busqueda" runat="server" OnClientClick="javascript:return Limpiar_Ctlr();"
                                                ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" ToolTip="Limpiar Controles de Busqueda" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 40%; text-align: right; font-size: 11px;" colspan="2"> 
                                           No Empleado:
                                        </td>
                                        <td style="width: 40%; text-align: right; font-size: 11px;">
                                            <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="98%" MaxLength="10" TextMode="Password" />
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_No_Empleado" runat="server" TargetControlID="Txt_No_Empleado"
                                                WatermarkText="No. Empleado" WatermarkCssClass="watermarked" />
                                        </td>
                                    <tr>
                                    <td style="width: 100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                       
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; text-align: left;" colspan="4">
                                            <center>
                                                <asp:Button ID="Btn_Autenticarse" runat="server" Text="Aceptar"
                                                    CssClass="button" OnClick = "Btn_Autenticarse_Click" Width="100px" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
            
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>

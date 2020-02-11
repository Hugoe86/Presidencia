<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Pagos_Predial_Internet.aspx.cs"
    Inherits="paginas_Predial_Frm_Ope_Pre_Pagos_Predial_Internet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pagos de Predial por Internet </title>
    <style type="text/css">
        *
        {
	        font-family:Arial;
	        font-size:small;
        }
        
        BODY
        {
            BORDER-BOTTOM: medium none;
            BORDER-LEFT: medium none;
            PADDING-BOTTOM: 0px;
            MARGIN: 0px;
            PADDING-LEFT: 0px;
            PADDING-RIGHT: 0px;
            BORDER-TOP: medium none;
            BORDER-RIGHT: medium none;
            PADDING-TOP: 0px
        }
         .progressBackgroundFilter { 
            z-index:1000;
	        background-color:Black;
	        width:3000px;
	        height:3000px; 
	        position:fixed;
	        top:0px;
	        left:0px;
	        filter:alpha(opacity=80);
	        -moz-opacity: 0.80;
	        opacity: 0.80;
        } 
        .processMessage {  
            position:fixed;  
            top:30%;  
            left:43%; 
            padding:10px; 
            width:14%; 
            z-index:1001;  
        } 
        .progressTemplate
        {
            background-color:Gray; 
            width:100%; height:100%;
            position:absolute; 
            top:0px; left:0px; 
            /*filter:alpha(opacity=50); */
            -moz-opacity: 0.75;
            /*opacity: 0.75; */
            elevation:above; 
            z-index:1000;
        }
        
        .estilo_fuente
        {
            text-align: left; 
            font-size: small; 
            font-family: Verdana, Geneva, MS Sans Serif;
            color: #25406D;
        }
        
        .watermarked {	            
	         background-color:#F0F8FF;
	        color:gray;
	        text-align:center;
	        font-size:12px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" method="post">
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
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <center>
            <table width="1024px" class="estilo_fuente">
                <tr>
                    <td colspan="4"><img alt="encabezado" src="../imagenes/master/encabezado.png" /></td>
                </tr>
                <tr>
                    <td colspan="4" align="center"><strong>PAGO DE IMPUESTO PREDIAL POR INTERNET</strong></td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 20%;">
                    <input type="button" style="display: none;" onclick="javascript:return false;" />
                        <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                        Cuenta Predial
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="98%" Enabled="false" BorderWidth="1"
                            OnTextChanged="Txt_Cuenta_Predial_TextChanged" AutoPostBack="true" MaxLength="12" />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Cuenta_Predial" runat="server"
                                FilterType="Numbers, LowercaseLetters, UppercaseLetters" TargetControlID="Txt_Cuenta_Predial" />
                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Cuenta_Predial" runat="server"
                                TargetControlID="Txt_Cuenta_Predial" WatermarkText="Proporcione su # de Cuenta Predial"
                                WatermarkCssClass="watermarked" />
                    </td>
                    <td style="text-align: left; width: 20%; text-align: right;">
                        Tipo de Pago<asp:DropDownList ID="Cmb_Tipo_Pago" runat="server" AutoPostBack="True" 
                            Width="140px" onselectedindexchanged="Cmb_Tipo_Pago_SelectedIndexChanged" 
                            Enabled="False">
                            <asp:ListItem>PAGO TOTAL</asp:ListItem>
                            <asp:ListItem>PAGO PARCIAL</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right; width: 30%;">
                        <asp:Button ID="Btn_Imprimir_Estado_Cuenta" runat="server" Text="Imprimir Estado de Cuenta"
                            ToolTip="Imprime el estado de cuenta" OnClick="Btn_Imprimir_Estado_Cuenta_Click"
                            TabIndex="10" />
                        <asp:HiddenField ID="Hdf_Convenio" runat="server" />
                        <asp:TextBox ID="Txt_Convenio" runat="server" Width="98%" Enabled="false" BorderWidth="1"
                            Visible="false" />
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
                            OnSelectedIndexChanged="Cmb_Bimestre_Final_SelectedIndexChanged" 
                            Enabled="False">
                            <asp:ListItem Value="1">01</asp:ListItem>
                            <asp:ListItem Value="2">02</asp:ListItem>
                            <asp:ListItem Value="3">03</asp:ListItem>
                            <asp:ListItem Value="4">04</asp:ListItem>
                            <asp:ListItem Value="5">05</asp:ListItem>
                            <asp:ListItem Value="6">06</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="Cmb_Anio_Final" runat="server" Width="55%" Style="float: right"
                            AutoPostBack="true" 
                            OnSelectedIndexChanged="Cmb_Anio_Final_SelectedIndexChanged" Enabled="False">
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
                        Descuento Pronto Pago
                    </td>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Descuento_Corriente" runat="server" Width="98%" Enabled="false"
                            BorderWidth="1" Style="text-align: right;" />
                    </td>
                </tr>
                <tr>
                        <td style="text-align:left;width:20%;" colspan="2">
                            &nbsp;
                        </td>
                        <td style="text-align:left;width:20%;text-align:right; border-top:solid 1px BLACK;">
                            Total
                        </td>
                        <td style="text-align:left;width:30%; border-top:solid 1px BLACK;">
                            <asp:TextBox ID="Txt_Total" runat="server" Width="98%" Enabled="false" BorderWidth="1" style="text-align:right;"  />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;" colspan="2">
                            &nbsp;
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            Ajuste Tarifario
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Ajuste_Tarifario" runat="server" Width="98%" Enabled="false" BorderWidth="1" style="text-align:right;" />
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
                            font-size:large; text-align: right;" ReadOnly="true" BorderWidth="1" ForeColor="Red" />
                    </td>
                </tr>
                <tr style="background-color: #36C;">
                    <td style="text-align: left; font-size: 15px; color: #FFF; font-weight: bolder;"
                        colspan="4">
                        Si desea efectuar el pago, proporcione la información de la Tarjeta de Crédito/Débito   
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp &nbsp &nbsp
                    </td>
                    <td colspan="3">
                        <table width="98%" class="estilo_fuente" border="1" align="center">
                            <tr>
                                <td>
                                    <asp:Label ID="Lbl_Titular_Tarjeta" Text="Titular de la Tarjeta de Crédito: " runat="server" />
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Titular_Tarjeta" Text="" runat="server" Width="95%" Style="text-transform: uppercase" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Lbl_No_Tarjeta" Text="Número de Tarjeta: " runat="server" />
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_No_Tarjeta" Text="" runat="server" MaxLength="16" Width="95%" />
                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Tarjeta" runat="server" TargetControlID="Txt_No_Tarjeta"
                                        FilterType="Numbers" ValidChars="0123456789" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Lbl_Codigo_Seguridad" Text="Código de Verificación de la Tarjeta: <br>(Ultimas tres cifras que se localizan en la parte <br>posterior de la tarjeta a un lado de la firma)"
                                        runat="server" />
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Codigo_Seguridad" runat="server" Text="" MaxLength="3" Width="95%"
                                        TextMode="Password" />
                                    <cc1:FilteredTextBoxExtender ID="Fte_Txt_Codigo_Seguridad" runat="server" TargetControlID="Txt_Codigo_Seguridad"
                                        FilterType="Numbers" ValidChars="0123456789" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Lbl_Valido_Hasta" Text="Válida hasta: " runat="server" />
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="Cmb_Validez_Mes" runat="server">
                                        <asp:ListItem Value="SELECCIONE" Text="<SELECCIONE>" />
                                        <asp:ListItem Value="01" Text="01" />
                                        <asp:ListItem Value="02" Text="02" />
                                        <asp:ListItem Value="03" Text="03" />
                                        <asp:ListItem Value="04" Text="04" />
                                        <asp:ListItem Value="05" Text="05" />
                                        <asp:ListItem Value="06" Text="06" />
                                        <asp:ListItem Value="07" Text="07" />
                                        <asp:ListItem Value="08" Text="08" />
                                        <asp:ListItem Value="09" Text="09" />
                                        <asp:ListItem Value="10" Text="10" />
                                        <asp:ListItem Value="11" Text="11" />
                                        <asp:ListItem Value="12" Text="12" />
                                    </asp:DropDownList>
                                    /
                                    <asp:DropDownList ID="Cmb_Valido_Anio" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <center>
                                        <asp:Label ID="Lbl_Nota1" Text="Si su tarjeta de crédito es rechazada, verifique el número, su límite disponible, fecha"
                                            runat="server" Width="95%" /></center>
                                    <center>
                                        <asp:Label ID="Lbl_Nota2" Text="de vencimiento o le sugerimos comunicarse con su banco."
                                            runat="server" Width="95%" /></center>
                                </td>
                            </tr>
                        </table>
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
                        <asp:Button ID="Btn_Ejecutar_Pago" runat="server" Text="REALIZAR PAGO" Style="color: Black;
                            font-weight: bolder; text-align: center; font-size: larger;" Width="98%" Height="35px"
                            OnClick="Btn_Ejecutar_Pago_Click" />
                        <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender_Btn_Ejecutar_Pago" runat="server" ConfirmText="¿Esta seguro de realizar el pago?"
                            TargetControlID="Btn_Ejecutar_Pago">
                        </cc1:ConfirmButtonExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
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
                        &nbsp;
                        <asp:HiddenField ID="Hfd_No_Descuento_Recargos" runat="server" />
                        <asp:HiddenField ID="Hfd_Tipo_Predio" runat="server" />
                        <asp:HiddenField ID="Hdf_Calle_Prop" runat="server" />
                        <asp:HiddenField ID="Hdf_Colonia_Prop" runat="server" />
                        <asp:HiddenField ID="Hdf_No_Int_Prop" runat="server" />
                        <asp:HiddenField ID="Hdf_No_Ext_Prop" runat="server" />
                        <asp:HiddenField ID="Hdf_RFC_Prop" runat="server" />
                        <asp:HiddenField ID="Hdf_Ciudad_Prop" runat="server" />
                        <asp:HiddenField ID="Hdf_Estado_Prop" runat="server" />
                        <asp:HiddenField ID="Hdf_Cod_Postal_Prop" runat="server" />
                        <asp:HiddenField ID="Hdf_Calle_Predio" runat="server" />
                        <asp:HiddenField ID="Hdf_Col_Predio" runat="server" />
                        <asp:HiddenField ID="Hdf_No_Ext_Predio" runat="server" />
                        <asp:HiddenField ID="Hdf_No_Int_Predio" runat="server" />
                        <asp:HiddenField ID="Hdf_Valor_Fiscal_Predio" runat="server" />
                        <asp:HiddenField ID="Hdf_efectos_Predio" runat="server" />
                        <asp:HiddenField ID="Hdf_Movimiento" runat="server" />
                        <asp:HiddenField ID="Hdf_Tasa_Predio" runat="server" />
                        <asp:HiddenField ID="Hdf_Cuota_Bimestral" runat="server" />
                        <asp:HiddenField ID="Hdf_Tasa_Id" runat="server" />
                        <asp:HiddenField ID="Hdf_Clave_Operacion" runat="server" />
                    </td>
                </tr>
            </table>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>

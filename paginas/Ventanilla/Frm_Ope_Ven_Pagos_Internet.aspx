<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Ven_Pagos_Internet.aspx.cs"
    Inherits="paginas_Atencion_Ciudadana_Frm_Ope_Ven_Pagos_Internet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pagos por Internet </title>
    <style type="text/css">
        *
        {
            font-family: Arial;
            font-size: small;
        }
        BODY
        {
            border-bottom: medium none;
            border-left: medium none;
            padding-bottom: 0px;
            margin: 0px;
            padding-left: 0px;
            padding-right: 0px;
            border-top: medium none;
            border-right: medium none;
            padding-top: 0px;
        }
        .progressBackgroundFilter
        {
            z-index: 1000;
            background-color: Black;
            width: 3000px;
            height: 3000px;
            position: fixed;
            top: 0px;
            left: 0px;
            filter: alpha(opacity=80);
            -moz-opacity: 0.80;
            opacity: 0.80;
        }
        .processMessage
        {
            position: fixed;
            top: 30%;
            left: 43%;
            padding: 10px;
            width: 14%;
            z-index: 1001;
        }
        .progressTemplate
        {
            background-color: Gray;
            width: 100%;
            height: 100%;
            position: absolute;
            top: 0px;
            left: 0px; /*filter:alpha(opacity=50); */
            -moz-opacity: 0.75; /*opacity: 0.75; */
            elevation: above;
            z-index: 1000;
        }
        .estilo_fuente
        {
            text-align: left;
            font-size: small;
            font-family: Verdana, Geneva, MS Sans Serif;
            color: #25406D;
        }
        .watermarked
        {
            background-color: #F0F8FF;
            color: gray;
            text-align: center;
            font-size: 12px;
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
                        <td colspan="4">
                            <img alt="encabezado" src="../imagenes/master/encabezado.png" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <strong>PAGOS POR INTERNET</strong>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            <input type="button" style="display: none;" onclick="javascript:return false;" />
                            Folio para pago
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Folio_Pago" runat="server" Width="98%" Enabled="false" BorderWidth="1"
                                OnTextChanged="Txt_Folio_Pago_TextChanged" AutoPostBack="true" MaxLength="20"
                                Style="font-size: 12px;" />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Folio_Pago" runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                TargetControlID="Txt_Folio_Pago" />
                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Folio_Pago" runat="server" TargetControlID="Txt_Folio_Pago"
                                WatermarkText="Proporcione el folio para pago" WatermarkCssClass="watermarked" />
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Solicitante
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="Txt_Solicitante" runat="server" Width="99%" Enabled="false" BorderWidth="1" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%; vertical-align: top;">
                            Concepto de pago
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="Txt_Concepto" runat="server" Width="99%" Enabled="false" BorderWidth="1" />
                        </td>
                    </tr>
                    <%------------------ Adeudos ------------------%>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;" colspan="2">
                            &nbsp;
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Importe
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Importe" runat="server" Width="98%" Enabled="false" BorderWidth="1"
                                Style="text-align: right;" />
                        </td>
                    </tr>
                    <tr id="Tr_Contenedor_Ajuste_Tarifario" runat="server">
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
                            Total a pagar
                        </td>
                        <td style="text-align: left; width: 30%; border-top: solid 1px BLACK;">
                            <asp:TextBox ID="Txt_Total_Pagar" runat="server" Width="98%" Enabled="false" BorderWidth="1"
                                Style="text-align: right;" />
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
                                        <asp:Label ID="Lbl_Tipo_Tarjeta" Text="Tipo Tarjeta Bancaria: " runat="server" />
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="Cmb_Tipo_Tarjeta" Width="96%" runat="server">
                                            <asp:ListItem Value="SELECCIONE" Text="<SELECCIONE>" />
                                            <asp:ListItem Value="VISA" Text="VISA" />
                                            <asp:ListItem Value="MC" Text="MASTER CARD" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Lbl_Titular_Tarjeta" Text="Titular de la Tarjeta de Bancaria: " runat="server" />
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Titular_Tarjeta" Text="" runat="server" Width="95%" Style="text-transform: uppercase" />
                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Titular_Tarjeta" runat="server" TargetControlID="Txt_Titular_Tarjeta"
                                            FilterType="UppercaseLetters, LowercaseLetters, Custom" ValidChars="ÑñáéíóúÁÉÍÓÚ. " />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Lbl_No_Tarjeta" Text="Número de Tarjeta Bancaria: " runat="server" />
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_No_Tarjeta" Text="" runat="server" MaxLength="16" Width="95%" />
                                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_No_Tarjeta" runat="server" TargetControlID="Txt_No_Tarjeta"
                                            FilterType="Numbers" />
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
                            &nbsp;
                        </td>
                        <td style="width: 25%; vertical-align: middle;">
                            &nbsp;
                        </td>
                        <%--<td colspan="2" style="width: 50%" align="right">
                            <asp:CheckBox ID="Chk_Requiere_Factura" runat="server" Text="REQUIERE FACTURA" TextAlign="Left"
                                Font-Bold="True" />
                        </td>--%>
                    </tr>
                    <tr>
                        <td style="width: 25%; vertical-align: middle;">
                            <asp:Image ID="Img_Icono_Efectivo" runat="server" Enabled="false" ImageUrl="~/paginas/imagenes/paginas/tarjetas-credito-debito.png"
                                Width="64px" />  
                                <asp:Button ID="Btn_Regresar" runat="server" Text="Regresar" Style="color: Black;
                                font-weight: bolder; text-align: center; font-size: larger;" Width="50%" Height="35px"
                                OnClick="Btn_Regresar_Click" />
                        </td>
                        <td style="width: 25%; vertical-align: middle;">
                          
                        </td>
                        <td colspan="2" style="width: 50%">
                            <asp:Button ID="Btn_Ejecutar_Pago" runat="server" Text="REALIZAR PAGO" Style="color: Black;
                                font-weight: bolder; text-align: center; font-size: larger;" Width="98%" Height="35px"
                                OnClick="Btn_Ejecutar_Pago_Click" />
                            <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender_Btn_Ejecutar_Pago" runat="server"
                                ConfirmText="Por su seguridad será redireccionado a un sistema de seguridad bancario, para verificar la autenticidad de la operación, ¿Esta seguro de realizar el pago?"
                                TargetControlID="Btn_Ejecutar_Pago">
                            </cc1:ConfirmButtonExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
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
                            <asp:HiddenField ID="Hdf_3D_Estatus" runat="server" />
                            <asp:HiddenField ID="Hdf_3D_ECI" runat="server" />
                            <asp:HiddenField ID="Hdf_3D_CardType" runat="server" />
                            <asp:HiddenField ID="Hdf_3D_XID" runat="server" />
                            <asp:HiddenField ID="Hdf_3D_CAVV" runat="server" />
                        </td>
                    </tr>
                </table>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>

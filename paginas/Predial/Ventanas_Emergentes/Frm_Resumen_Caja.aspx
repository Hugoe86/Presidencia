<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Resumen_Caja.aspx.cs"
    Inherits="paginas_Predial_Ventanas_Emergentes_Frm_Ope_Pre_Resumen_Caja" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Resumen de Predio - Cajas</title>
    <link href="../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../../estilos/estilo_ajax.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        *
        {
            font-size: small;
            font-family: Arial;
        }
    </style>

    <script type="text/javascript" language="javascript">
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }
        function Abrir_Ventana_Estado_Cuenta(Url, Propiedades) {
            window.open(Url, 'Estado_Cuenta', Propiedades);
        }
    </script>

</head>
<body>
    <form id="frm_resumen_caja" runat="server">
    <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
        <table width="100%" class="estilo_fuente">
            <tr align="center">
                <td colspan="4" class="label_titulo">
                    Resumen de Predio
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;
                    <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                        Visible="false" />&nbsp;
                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                    <asp:Label ID="Lbl_Encabezado_Error" runat="server" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                </td>
            </tr>
        </table>
        <table width="98%" class="estilo_fuente">
            <tr>
                <td>
                </td>
                <td>
                    <asp:Label ID="Lbl_Estatus" Style="color: Red" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                    Cuenta predial
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="96.4%" />
                </td>
                <td style="width: 20%; text-align: right;">
                    Cuenta origen
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Cuenta_Origen" runat="server" ReadOnly="True" Width="96.4%" />
                </td>
            </tr>
            <%------------------ Datos generales ------------------%>
            <tr style="background-color: #3366CC">
                <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                    Generales
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Estado de predio
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Estado_Predio_General" runat="server" Width="96.4%" />
                </td>
                <td style="width: 20%; text-align: right;">
                    Uso predio
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Uso_Predio_General" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Ultimo movimiento
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Ultimo_Movimiento_General" runat="server" Width="96.4%" />
                </td>
                <td style="width: 20%; text-align: right;">
                    Estatus
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Estatus_General" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Supe. construida (m&sup2;)
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Supe_Construida_General" runat="server" Width="96.4%" Text="0" />
                </td>
                <td style="width: 20%; text-align: right;">
                    Superficie Total (m&sup2;)
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Super_Total_General" runat="server" Width="96.4%" Text="0" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Calle
                </td>
                <td style="text-align: left; width: 30%;" colspan="3">
                    <asp:TextBox ID="Txt_Ubicacion_General" runat="server" Width="98.6%" />
                </td>
            </tr>
            <tr>
                <td style="width: 20%; text-align: left;">
                    Colonia
                </td>
                <td style="text-align: left; width: 30%;" colspan="3">
                    <asp:TextBox ID="Txt_Colonia_General" runat="server" Width="98.6%" />
                </td>
            </tr>
            <tr>
                <td style="width: 20%;">
                    Número exterior
                </td>
                <td style="width: 80%;" colspan="3">
                    <asp:TextBox ID="Txt_Numero_Exterior_General" runat="server" Width="98.6%" />
                </td>
            </tr>
            <tr>
                <td style="width: 20%;">
                    Número interior
                </td>
                <td style="width: 80%;" colspan="3">
                    <asp:TextBox ID="Txt_Numero_Interior_General" runat="server" Width="98.6%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Clave catastral
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Clave_Catastral_General" runat="server" Width="96.4%" />
                </td>
                <td style="width: 20%; text-align: right;">
                    Efectos
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Efectos_General" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                </td>
                <td style="text-align: left; width: 30%;">
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
            <%------------------ Propietario ------------------%>
            <tr style="background-color: #36C;">
                <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                    <asp:ImageButton ID="Img_Btn_Imprimir_Propietario" runat="server" ImageUrl="../imagenes/gridview/grid_print.png"
                        Style="vertical-align: top; float: right;" Height="18px" CausesValidation="false"
                        OnClick="Img_Btn_Imprimir_Propietario_Click" Visible="false" />
                    Propietario
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Nombre
                </td>
                <td style="text-align: left; width: 30%;" colspan="3">
                    <asp:TextBox ID="Txt_Nombre_Propietario" runat="server" Width="98.6%" TextMode="MultiLine" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    RFC
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Rfc_Propietario" runat="server" Width="96.4%" />
                </td>
                <td style="width: 20%; text-align: right;">
                    Propietario/Poseedor
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Propietario_Poseedor_Propietario" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Calle
                </td>
                <td style="text-align: left; width: 30%;" colspan="3">
                    <asp:TextBox ID="Txt_Calle_Propietario" runat="server" Width="98.6%" />
                </td>
            </tr>
            <tr>
                <td style="width: 20%;">
                    Número exterior
                </td>
                <td style="width: 80%;" colspan="3">
                    <asp:TextBox ID="Txt_Numero_Exterior_Propietario" runat="server" Width="98.6%" />
                </td>
            </tr>
            <tr>
                <td style="width: 20%;">
                    Número interior
                </td>
                <td style="width: 80%;" colspan="3">
                    <asp:TextBox ID="Txt_Numero_Interior_Propietario" runat="server" Width="98.6%" />
                </td>
            </tr>
            <tr>
                <td style="width: 20%; text-align: left;">
                    Colonia
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Colonia_Propietario" runat="server" Width="96.4%" />
                </td>
                <td style="width: 20%; text-align: right;">
                    C.P.
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Cod_Postal_Propietario" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Ciudad
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Ciudad_Propietario" runat="server" Width="96.4%" />
                </td>
                <td style="width: 20%; text-align: right;">
                    Estado
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Estado_Propietario" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%; vertical-align: top;">
                    Copropietarios
                </td>
                <td colspan="3" style="text-align: left; width: 80%;">
                    <asp:TextBox ID="Txt_Copropietarios_Propietario" runat="server" TabIndex="10" MaxLength="250"
                        TextMode="MultiLine" Width="98.6%" AutoPostBack="True" Enabled="true" ForeColor="Silver" />
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
                    <asp:ImageButton ID="Img_Btn_Imprimir_Impuestos" runat="server" ImageUrl="../imagenes/gridview/grid_print.png"
                        Style="vertical-align: top; float: right;" Height="18px" CausesValidation="false"
                        OnClick="Img_Btn_Imprimir_Impuestos_Click" Visible="false" />
                    Impuestos
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Valor fiscal
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Valor_Fiscal_Impuestos" runat="server" Width="96.4%" />
                </td>
                <td style="width: 20%; text-align: right;">
                    Tasa
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Tasa_Impuestos" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Periodo corriente
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Periodo_Corriente_Impuestos" runat="server" Width="96.4%" />
                </td>
                <td style="width: 20%; text-align: right;">
                    Tipo predio
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Tipo_Periodo_Impuestos" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Cuota anual
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Cuota_Anual_Impuestos" runat="server" Width="96.4%" />
                </td>
                <td style="width: 20%; text-align: right;">
                    Cuota bimestral
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Cuota_Bimestral_Impuestos" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Dif. de construcción
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Dif_Construccion_Impuestos" runat="server" Width="96.4%" />
                </td>
                <td style="text-align: left; width: 20%; text-align: right;">
                    % Exención
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Porciento_Exencion_Impuestos" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Término exención
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Fecha_Termino_Extencion" runat="server" Width="96.4%" TabIndex="12"
                        MaxLength="11" Height="18px" />
                </td>
                <td style="text-align: left; width: 20%; text-align: right;">
                    Fecha avalúo
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Fecha_Avaluo_Impuestos" runat="server" Width="96.4%" TabIndex="12"
                        MaxLength="11" Height="18px" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Cuota fija
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:CheckBox ID="Chk_Cuota_Fija" runat="server" Text="" />
                    <asp:TextBox ID="Txt_Cuota_Fija_Impuestos" runat="server" Width="85%" Visible="false" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Beneficio
                </td>
                <td colspan="3">
                    <asp:TextBox ID="Txt_Cuota_Fija" runat="server" Width="98.6%" />
                </td>
            </tr>
            <%------------------ Detalles cuota fija ------------------%>
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
            <%------------------ Detalles cuota fija ------------------%>
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
            <%------------------ Historial de pagos ------------------%>
            <tr style="background-color: #36C;">
                <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                    <asp:DropDownList ID="Cmb_Consultar_Tipo_Pago" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="Cmb_Consultar_Tipo_Pago_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="PREDIAL">PREDIAL</asp:ListItem>
                        <asp:ListItem Value="TRASLADO">TRASLADO</asp:ListItem>
                        <asp:ListItem Value="IMPUESTOS FRACCIONAMIENTO">IMPUESTOS FRACCIONAMIENTO</asp:ListItem>
                        <asp:ListItem Value="DERECHOS SUPERVISION">DERECHOS SUPERVISION</asp:ListItem>
                        <asp:ListItem Value="CONSTANCIAS">CONSTANCIAS</asp:ListItem>
                    </asp:DropDownList>
                    Historial de pagos
                    <br />
                    <asp:GridView ID="Grid_Historial_Pagos" runat="server" CssClass="GridView_1" AutoGenerateColumns="false"
                        Width="100%" OnPageIndexChanging="Grid_Historial_Pagos_PageIndexChanging" HeaderStyle-CssClass="tblHead"
                        Style="white-space: normal;" PageSize="5" AllowPaging="true">
                        <Columns>
                            <asp:BoundField DataField="PREDIAL" HeaderText="TIPO">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="No_Recibo" HeaderText="No. Recibo">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yy}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="No_Operacion" HeaderText="No. Operación">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Numero_Caja" HeaderText="No. Caja ">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Clave_Banco" HeaderText="No. Banco">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Periodo" HeaderText="Periodo">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Monto_Rezago" HeaderText="Monto Rezago" DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Monto_Corriente" HeaderText="Monto Corriente" DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Recargos_Ordinarios" HeaderText="Recargos Ordinarios"
                                DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Recargos_Moratorios" HeaderText="Recargos Moratorios"
                                DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Honorarios" HeaderText="Honorarios" DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Gastos_Ejecucion" HeaderText="G. Ejec." DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Descuento_Recargos" HeaderText="Desc. Rec." DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Descuento_Moratorios" HeaderText="Desc. Mor." DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Descuento_Honorarios" HeaderText="Desc. Hon." DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Descuento_Pronto_Pago" HeaderText="Desc. Pronto Pago"
                                DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                        </Columns>
                        <SelectedRowStyle CssClass="GridSelected" />
                        <PagerStyle CssClass="GridHeader" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                    </asp:GridView>
                    <asp:GridView ID="Grid_Traslado" runat="server" CssClass="GridView_1" AutoGenerateColumns="false"
                        Width="100%" OnPageIndexChanging="Grid_Traslado_PageIndexChanging" HeaderStyle-CssClass="tblHead"
                        Style="white-space: normal;" PageSize="5" AllowPaging="true" AllowSorting="true">
                        <Columns>
                            <asp:BoundField DataField="TIPO_PAGO" HeaderText="TIPO">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="No_Recibo" HeaderText="No. Recibo">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Numero_Caja" HeaderText="No. Caja ">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yy}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Impuesto_Traslado" HeaderText="Impuesto Traslado" DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Impuesto_Division" HeaderText="Impuesto Division" DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Constancia_No_Adeudo" HeaderText="Constancia no Adeudo"
                                DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Multas" HeaderText="Multas" DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Descuento_Multas" HeaderText="Desc. Multas" DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="recargos" HeaderText="Recargos" DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Descuento_Recargos" HeaderText="Desc. Recargos" DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:c}">
                                <HeaderStyle Font-Size="XX-Small" HorizontalAlign="center" />
                                <ItemStyle Font-Size="XX-Small" HorizontalAlign="center" />
                            </asp:BoundField>
                        </Columns>
                        <SelectedRowStyle CssClass="GridSelected" />
                        <PagerStyle CssClass="GridHeader" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                    </asp:GridView>
                    <asp:GridView ID="Grid_Impuesto_Fraccionamiento" runat="server" CssClass="GridView_1"
                        AutoGenerateColumns="false" Width="100%" OnPageIndexChanging="Grid_Impuesto_Fraccionamiento_PageIndexChanging"
                        HeaderStyle-CssClass="tblHead" Style="white-space: normal;" PageSize="5" AllowPaging="true">
                        <Columns>
                            <asp:BoundField DataField="TIPO_PAGO" HeaderText="FRACC.">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="No_Recibo" HeaderText="No. Recibo">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Numero_Caja" HeaderText="No. Caja ">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yy}">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Recargos" HeaderText="Recargos" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Descuento_Recargos" HeaderText="Desc. Recargos" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Multas" HeaderText="Multas" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Descuento_Multas" HeaderText="Desc. Multas" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                        </Columns>
                        <SelectedRowStyle CssClass="GridSelected" />
                        <PagerStyle CssClass="GridHeader" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                    </asp:GridView>
                    <asp:GridView ID="Grid_Derechos_Supervision" runat="server" CssClass="GridView_1"
                        AutoGenerateColumns="false" Width="100%" OnPageIndexChanging="Grid_Derechos_Supervision_PageIndexChanging"
                        HeaderStyle-CssClass="tblHead" Style="white-space: normal;" PageSize="5" AllowPaging="true">
                        <Columns>
                            <asp:BoundField DataField="TIPO_PAGO" HeaderText="DER. SUP.">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="No_Recibo" HeaderText="No. Recibo">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Numero_Caja" HeaderText="No. Caja ">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yy}">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Recargos" HeaderText="Recargos" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Descuento_Recargos" HeaderText="Desc. Recargos" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Multas" HeaderText="Multas" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Descuento_Multas" HeaderText="Desc. Multas" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" />
                                <ItemStyle HorizontalAlign="center" />
                            </asp:BoundField>
                        </Columns>
                        <SelectedRowStyle CssClass="GridSelected" />
                        <PagerStyle CssClass="GridHeader" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                    </asp:GridView>
                    <asp:GridView ID="Grid_Constancias" runat="server" CssClass="GridView_1" AutoGenerateColumns="false"
                        Width="100%" OnPageIndexChanging="Grid_Constancias_PageIndexChanging" HeaderStyle-CssClass="tblHead"
                        Style="white-space: normal;" PageSize="5" AllowPaging="true">
                        <Columns>
                            <asp:BoundField DataField="'CONSTANCIAS'" HeaderText="CONST">
                                <HeaderStyle HorizontalAlign="center" Width="5%" />
                                <ItemStyle HorizontalAlign="center" Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="No_Recibo" HeaderText="No. Recibo">
                                <HeaderStyle HorizontalAlign="center" Width="15%" />
                                <ItemStyle HorizontalAlign="center" Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Numero_Caja" HeaderText="No. Caja ">
                                <HeaderStyle HorizontalAlign="center" Width="15%" />
                                <ItemStyle HorizontalAlign="center" Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yy}">
                                <HeaderStyle HorizontalAlign="center" Width="15%" />
                                <ItemStyle HorizontalAlign="center" Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Tipo_Constancia" HeaderText="Tipo Constancia">
                                <HeaderStyle HorizontalAlign="center" Width="15%" />
                                <ItemStyle HorizontalAlign="center" Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" Width="15%" />
                                <ItemStyle HorizontalAlign="center" Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="No_Constancia" HeaderText="No Constancia" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" Width="15%" />
                                <ItemStyle HorizontalAlign="center" Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:c}">
                                <HeaderStyle HorizontalAlign="center" Width="15%" />
                                <ItemStyle HorizontalAlign="center" Width="15%" />
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
        <asp:HiddenField ID="Hdn_Tasa_ID" runat="server" />
    </div>
    </form>
</body>
</html>

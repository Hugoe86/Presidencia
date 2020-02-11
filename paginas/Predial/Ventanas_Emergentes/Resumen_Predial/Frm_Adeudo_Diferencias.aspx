<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Adeudo_Diferencias.aspx.cs"
    Inherits="paginas_Predial_Ventanas_Emergentes_Resumen_Predial_Frm_Adeudo_Diferencias" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<style type="text/css">
    *
    {
        font-size: small;
        font-family: Arial;
    }
</style>
<head id="Head1" runat="server">
    <link href="../../../estilos/estilo_ajax.css" rel="stylesheet" type="text/css" />
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <title>Vista Previa de Adeudos</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" class="estilo_fuente">
            <tr align="center">
                <td colspan="4" class="label_titulo">
                    Vista Previa de Aplicación de Adeudos
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
        <table width="98%" border="0" cellspacing="0">
        </table>
        <br />
        <table width="98%" class="estilo_fuente">
            <tr>
                <td style="text-align: left; width: 20%;">
                    Cuenta predial
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="96.4%" />
                </td>
                <td style="text-align: left; width: 20%;">
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
            <%------------------ Propietario ------------------%>
            <%--            <tr style="background-color: #36C;">
                <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                    Propietario
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;">
                    Nombre
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Nombre_Propietario" runat="server" Width="96.4%" />
                </td>
                <td style="text-align: right; width: 20%;">
                    RFC
                </td>
                <td style="text-align: left; width: 30%;">
                    <asp:TextBox ID="Txt_Rfc_Propietario" runat="server" Width="96.4%" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
--%>
            <%------------------ Bimestral ------------------%>
            <tr style="background-color: #36C;">
                <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                    Adeudos por bimestre
                </td>
            </tr>
            <tr align="center">
                <td colspan="4">
                    <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                    <asp:GridView ID="Grid_Estado_Cuenta" runat="server" AllowPaging="True" CssClass="GridView_1"
                        AutoGenerateColumns="False" PageSize="6" Width="100%" AllowSorting="True" HeaderStyle-CssClass="tblHead"
                        Style="white-space: normal;" OnPageIndexChanging="Grid_Estado_Cuenta_PageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="AÑO" HeaderText="Año">
                                <HeaderStyle HorizontalAlign="center" Width="10%" />
                                <ItemStyle HorizontalAlign="center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BIMESTRE_1" HeaderText="1" DataFormatString="{0:C2}">
                                <HeaderStyle HorizontalAlign="center" Width="10%" />
                                <ItemStyle HorizontalAlign="center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BIMESTRE_2" HeaderText="2" DataFormatString="{0:C2}">
                                <HeaderStyle HorizontalAlign="center" Width="10%" />
                                <ItemStyle HorizontalAlign="center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BIMESTRE_3" HeaderText="3" DataFormatString="{0:C2}">
                                <HeaderStyle HorizontalAlign="center" Width="10%" />
                                <ItemStyle HorizontalAlign="center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BIMESTRE_4" HeaderText="4" DataFormatString="{0:C2}">
                                <HeaderStyle HorizontalAlign="center" Width="10%" />
                                <ItemStyle HorizontalAlign="center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BIMESTRE_5" HeaderText="5" DataFormatString="{0:C2}">
                                <HeaderStyle HorizontalAlign="center" Width="10%" />
                                <ItemStyle HorizontalAlign="center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BIMESTRE_6" HeaderText="6" DataFormatString="{0:C2}">
                                <HeaderStyle HorizontalAlign="center" Width="10%" />
                                <ItemStyle HorizontalAlign="center" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ADEUDO_TOTAL_AÑO" HeaderText="Total" DataFormatString="{0:C2}">
                                <HeaderStyle HorizontalAlign="center" Width="25%" />
                                <ItemStyle HorizontalAlign="center" Width="25%" />
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
                    <hr />
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 20%;" colspan="2">
                    &nbsp;
                </td>
                <td style="text-align: right; width: 20%;">
                    Total
                </td>
                <td style="text-align: left; width: 30%; border-top: solid 1px BLACK">
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="Hdn_Propietario_ID" runat="server" />
        <asp:HiddenField ID="Hdn_Tasa_ID" runat="server" />
    </div>
    </form>
</body>
</html>

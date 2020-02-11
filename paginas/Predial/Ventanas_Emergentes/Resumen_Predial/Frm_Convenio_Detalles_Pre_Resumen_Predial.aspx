<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Convenio_Detalles_Pre_Resumen_Predial.aspx.cs" Inherits="paginas_Predial_Ventanas_Emergentes_Resumen_Predial_Frm_Convenio_Detalles_Pre_Resumen_Predial" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Convenios de la Cuenta</title>
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../../../estilos/estilo_masterpage.css" rel="stylesheet" type="text/css" />
    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../../../estilos/estilo_ajax.css" rel="stylesheet" type="text/css" />
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table >
            <tr>
                <td>
                    <asp:GridView ID="Grid_Convenios_Detalles" runat="server" AllowPaging="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                                PageSize="5" onpageindexchanging="Grid_Convenios_Detalles_PageIndexChanging" 
                                                Width="98%">
                                                <RowStyle CssClass="GridItem" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle Width="5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="No_Convenio" HeaderText="No.Convenio">
                                                        <ItemStyle Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="No_Pago" HeaderText="No. Pago">
                                                        <ItemStyle Width="15%" />
                                                    </asp:BoundField>
                                                    <%--<asp:BoundField DataField="Monto" HeaderText="Monto">
                                                        <ItemStyle Width="50%" />
                                                    </asp:BoundField>--%>
                                                    <asp:BoundField DataField="Fecha_Vencimiento" HeaderText="Fecha Vencimiento" 
                                                        NullDisplayText="-">
                                                        <ItemStyle Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" 
                                                        NullDisplayText="-">
                                                        <ItemStyle Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Monto_Impuesto" HeaderText="Monto Impuesto" 
                                                        NullDisplayText="-">
                                                        <ItemStyle Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Recargos_Ordinarios" HeaderText="Recargos Ordinarios" 
                                                        NullDisplayText="-">
                                                        <ItemStyle Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Recargos_Moratorios" HeaderText="Recargos Moratorios" 
                                                        NullDisplayText="-">
                                                        <ItemStyle Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Monto_Multas" HeaderText="Monto Multas" 
                                                        NullDisplayText="-">
                                                        <ItemStyle Width="5%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <HeaderStyle CssClass="GridHeader" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                            </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

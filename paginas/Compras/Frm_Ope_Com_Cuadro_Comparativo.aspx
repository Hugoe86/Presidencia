<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Com_Cuadro_Comparativo.aspx.cs"
    Inherits="paginas_Compras_Frm_Ope_Com_Cuadro_Comparativo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cuadro Comparativo</title>
    <link href="../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="overflow:auto;height:580px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;">
        <table style="width: 100%;">
            <tr>
                <td>
                    &nbsp;
                    <asp:GridView ID="Grid_Cuadro_Comparativo" runat="server" CssClass="GridView_1">
                    </asp:GridView>
                </td>
                <td>
                    &nbsp;<asp:Label ID="Label1" runat="server" Text="Label" ></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
<%--    <div style="overflow:auto;height:580px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" >
    <table>
        <tr>
            <td style="width: 99%" align="center" colspan="4">
                <asp:Panel ID="Pnl_Grid" runat="server">
                <asp:GridView ID="Grid_Requisiciones" runat="server" AutoGenerateColumns="False"
                    CssClass="GridView_1" GridLines="Both" Width="100%" AllowSorting="true" OnSorting="Grid_Requisiciones_Sorting" HeaderStyle-Height="0%">
                    <RowStyle CssClass="GridItem" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png">
                        <ItemStyle Width="3%" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="UR" HeaderText="" Visible="false" SortExpression="UR">
                            <ItemStyle HorizontalAlign="Left" Wrap="true" Font-Size="Smaller" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FUENTE" HeaderText="" Visible="true"
                            SortExpression="FUENTE">
                            <ItemStyle HorizontalAlign="Left" Width="18%" Wrap="true" Font-Size="Smaller" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PROGRAMA" HeaderText="" Visible="True" SortExpression="PROGRAMA">
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Wrap="true" Font-Size="Smaller" Width="19%"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="PARTIDA" HeaderText="" Visible="true" SortExpression="PARTIDA">
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="28%" Wrap="true" Font-Size="Smaller" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ASIGNADO" HeaderText="" Visible="true" SortExpression="ASIGNADO"
                            DataFormatString="{0:n}">
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Right" Width="8%" Font-Size="Smaller" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DISPONIBLE" HeaderText="" Visible="true" SortExpression="DISPONIBLE"
                            DataFormatString="{0:n}">
                            <ItemStyle HorizontalAlign="Right" Font-Size="Smaller" Width="8%"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="COMPROMETIDO" HeaderText="" Visible="true"
                            SortExpression="COMPROMETIDO" DataFormatString="{0:n}">
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Right" Width="8%" Font-Size="Smaller" />
                        </asp:BoundField>
                        <asp:BoundField DataField="EJERCIDO" HeaderText="" Visible="true" SortExpression="EJERCIDO"
                            DataFormatString="{0:n}">
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Right" Width="8" Font-Size="Smaller" />
                        </asp:BoundField>
                    </Columns>
                    <PagerStyle CssClass="GridHeader" />
                    <SelectedRowStyle CssClass="GridSelected" />
                    <HeaderStyle CssClass="GridHeader" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
        </table>
    </div>--%>

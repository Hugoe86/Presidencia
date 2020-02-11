<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pagina.aspx.cs" Inherits="paginas_Paginas_Generales_Pagina" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Presupuestos</title>
    <link href="../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../estilos/estilo_masterpage.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
    <div>
<%--         <table style="width: 99%">
            <tr class="barra_busqueda">
                <td style="width: 20%;">
                    Fte. Financiamiento
                </td>
                <td style="width: 20%;">
                    Programa
                </td>
                <td style="width: 28%;">
                    Partida
                </td>
                <td style="width: 8%;">
                    Asignado
                </td>
                <td style="width: 8%;">
                    Disponible
                </td>
                <td style="width: 8%;">
                    Comprometido
                </td>
                <td style="width: 8%;">
                    Ejercido
                </td>
            </tr>
        </table> --%>
    </div>
    <div style="overflow:auto;height:580px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" >
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
                            <ItemStyle HorizontalAlign="Left" Wrap="true" Font-Size="X-Small" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FUENTE" HeaderText="Fte.Financiamiiento" Visible="true"
                            SortExpression="FUENTE">
                            <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="true" Font-Size="X-Small" />
                        </asp:BoundField>
                        <%--<asp:BoundField DataField="AREA" HeaderText="Area Funcional" Visible="true"
                            SortExpression="AREA">
                            <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="true" Font-Size="X-Small" />
                        </asp:BoundField>--%>
                        <asp:BoundField DataField="PROGRAMA" HeaderText="Programa" Visible="True" SortExpression="PROGRAMA">
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Wrap="true" Font-Size="X-Small" Width="19%"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="PARTIDA" HeaderText="Partida" Visible="true" SortExpression="PARTIDA">
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="28%" Wrap="true" Font-Size="X-Small" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ASIGNADO" HeaderText="Asignado" Visible="true" SortExpression="ASIGNADO"
                            DataFormatString="{0:n}">
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Right" Width="8%" Font-Size="X-Small" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="AMPLIACION" HeaderText="Ampliacion" Visible="true" SortExpression="AMPLIACION"
                            DataFormatString="{0:n}">
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Right" Width="8%" Font-Size="X-Small" />
                        </asp:BoundField>
                        <asp:BoundField DataField="REDUCCION" HeaderText="Reducción" Visible="true" SortExpression="REDUCCION"
                            DataFormatString="{0:n}">
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Right" Width="8%" Font-Size="X-Small" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MODIFICADO" HeaderText="Modificado" Visible="true" SortExpression="MODIFICADO"
                            DataFormatString="{0:n}">
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Right" Width="8%" Font-Size="X-Small" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DEVENGADO" HeaderText="Devengado" Visible="true" SortExpression="DEVENGADO"
                            DataFormatString="{0:n}">
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Right" Width="8%" Font-Size="X-Small" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PAGADO" HeaderText="Pagado" Visible="true" SortExpression="PAGADO"
                            DataFormatString="{0:n}">
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Right" Width="8%" Font-Size="X-Small" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="DISPONIBLE" HeaderText="Disponible" Visible="true" SortExpression="DISPONIBLE"
                            DataFormatString="{0:n}">
                            <ItemStyle HorizontalAlign="Right" Font-Size="X-Small" Width="8%"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="COMPROMETIDO" HeaderText="Comprometido" Visible="true"
                            SortExpression="COMPROMETIDO" DataFormatString="{0:n}">
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Right" Width="8%" Font-Size="X-Small" />
                        </asp:BoundField>
                        <asp:BoundField DataField="EJERCIDO" HeaderText="Ejercido" Visible="true" SortExpression="EJERCIDO"
                            DataFormatString="{0:n}">
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Right" Width="8" Font-Size="X-Small" />
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
    </div>
    </form>
</body>
</html>

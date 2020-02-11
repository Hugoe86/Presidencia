<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Vista_Previa_Presupuestos.aspx.cs" Inherits="paginas_presupuestos_Frm_Vista_Previa_Presupuestos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vista Previa Presupuestos</title>
    <link href="../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <link href="../estilos/estilo_masterpage.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="overflow:auto;height:97%;width:97%;vertical-align:top;border-style:outset;border-color: Silver;">
        <table>
        <tr>
            <td colspan="4">
                <asp:ImageButton ID="Btn_Exportar_PDF" runat="server" CssClass="Img_Button" 
                ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" 
                ToolTip="Exportar PDF" AlternateText="Consultar" 
                    onclick="Btn_Exportar_PDF_Click"/>
                <asp:ImageButton ID="Btn_Exportar_Excel" runat="server" CssClass="Img_Button" 
                ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                ToolTip="Exportar PDF" AlternateText="Consultar" 
                    onclick="Btn_Exportar_Excel_Click"/>
            </td>
        </tr>
        <tr>
            <td style="width: 99%" align="center" colspan="4">
            <div style="overflow:auto;height:600px;width:97%;vertical-align:top;border-style:outset;border-color: Silver;" >
                    <asp:GridView ID="Grid_Presupuestos" runat="server" AllowSorting="true" 
                        AutoGenerateColumns="False" CssClass="GridView_1" GridLines="Both" 
                        HeaderStyle-Height="0%" Width="100%" OnSorting="Grid_Presupuestos_Sorting">
                        <RowStyle CssClass="GridItem" />
                        <Columns>
                            <asp:BoundField DataField="UR" HeaderText="UR" SortExpression="UR" 
                                Visible="true">
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Wrap="true" Width="9%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FUENTE" HeaderText="Fte.Financiamiiento" 
                                SortExpression="FUENTE" Visible="true">
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="9%" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PROGRAMA" HeaderText="Programa" 
                                SortExpression="PROGRAMA" Visible="True">
                                <FooterStyle HorizontalAlign="Left" />
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="9%" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PARTIDA_GENERICA" HeaderText="Partida Gen" 
                                SortExpression="PARTIDA_GENERICA" Visible="true">
                                <FooterStyle HorizontalAlign="Left" />
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="9%" Wrap="true" />
                            </asp:BoundField>
                             
                             <asp:BoundField DataField="PARTIDA" HeaderText="Partida Esp" 
                                SortExpression="PARTIDA" Visible="true">
                                <FooterStyle HorizontalAlign="Left" />
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="9%" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CAPITULO" HeaderText="Capitulo" 
                                SortExpression="CAPITULO" Visible="true">
                                <FooterStyle HorizontalAlign="Left" />
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="9%" Wrap="true" />
                            </asp:BoundField>
                            
                             <asp:BoundField DataField="CONCEPTO" HeaderText="Concepto" 
                                SortExpression="CONCEPTO" Visible="true">
                                <FooterStyle HorizontalAlign="Left" />
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="9%" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ASIGNADO" DataFormatString="{0:n}" 
                                HeaderText="Asignado" SortExpression="ASIGNADO" Visible="true">
                                <FooterStyle HorizontalAlign="Left" />
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="4%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AMPLIACION" DataFormatString="{0:n}" 
                                HeaderText="Ampliacion" SortExpression="AMPLIACION" Visible="false">
                                <FooterStyle HorizontalAlign="Left" />
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="4%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="REDUCCION" DataFormatString="{0:n}" 
                                HeaderText="Reducción" SortExpression="REDUCCION" Visible="false">
                                <FooterStyle HorizontalAlign="Left" />
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="4%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MODIFICADO" DataFormatString="{0:n}" 
                                HeaderText="Modificado" SortExpression="MODIFICADO" Visible="true">
                                <FooterStyle HorizontalAlign="Left" />
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="4%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DEVENGADO" DataFormatString="{0:n}" 
                                HeaderText="Devengado" SortExpression="DEVENGADO" Visible="true">
                                <FooterStyle HorizontalAlign="Left" />
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="4%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PAGADO" DataFormatString="{0:n}" HeaderText="Pagado" 
                                SortExpression="PAGADO" Visible="true">
                                <FooterStyle HorizontalAlign="Left" />
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="4%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DISPONIBLE" DataFormatString="{0:n}" 
                                HeaderText="Disponible" SortExpression="DISPONIBLE" Visible="true">
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="4%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="COMPROMETIDO" DataFormatString="{0:n}" 
                                HeaderText="Comprometido" SortExpression="COMPROMETIDO" Visible="true">
                                <FooterStyle HorizontalAlign="Left" />
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="4%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EJERCIDO" DataFormatString="{0:n}" 
                                HeaderText="Ejercido" SortExpression="EJERCIDO" Visible="true">
                                <FooterStyle HorizontalAlign="Left" />
                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="4" />
                            </asp:BoundField>
                        </Columns>
                        <PagerStyle CssClass="GridHeader" />
                        <SelectedRowStyle CssClass="GridSelected" />
                        <HeaderStyle CssClass="GridHeader" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                    </asp:GridView>
                   </div>
            </td>
        </tr>
        </table>
        </div>
    </form>
</body>
</html>

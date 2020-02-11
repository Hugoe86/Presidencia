<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Busqueda_Avanzada_Cuenta_Contable.aspx.cs" Inherits="paginas_Tramites_Ventanas_Emergente_Frm_Busqueda_Avanzada_Cuenta_Contable" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Tramites</title>
     <%--<link href="../../estilos/estilo_masterpage.css" rel="stylesheet" type="text/css" />--%>
    <link href="../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <%--<link href="../../estilos/estilo_ajax.css" rel="stylesheet" type="text/css" />--%>
    <base target="_self" />
    
    <style type="text/css">
        .Ventana
        {
            height: 420px;
            width: 700px;
        }
        .GridView_1
        {
            font-family: Verdana, Geneva, MS Sans Serif;
            font-size: 8px;
            color: #2F4E7D;
            font-weight: normal;
            padding: 3px 6px 3px 6px;
            vertical-align: middle;
            white-space: nowrap;
            background-color: White;
            border-style: none;
            border-width: 5px;
            width: 98%;
            text-align: left;
            margin-left: 0px;
        }
        *
        {
            font-family: Arial;
            font-size: small;
            text-align: left;
        }
        .GridHeader
        {
            font-weight: bold;
            background-color: #2F4E7D;
            color: #ffffff;
            text-align: left;
            position: relative;
            height: 23px;
        }
        .GridItem
        {
            background-color: white;
            color: #25406D;
            font-size: x-small;
        }
        .GridAltItem
        {
            background-color: #E6E6E6; /*#E6E6E6 #E0F8F7*/
            color: #25406D;
            font-size: x-small;
        }
        .GridSelected
        {
            background-color: #A4A4A4; /*#A9F5F2;*/
            color: #25406D;
            font-size: x-small;
        }
        .style2
        {
            width: 4px;
        }
    </style>
</head>

<body>
    <form id="Frm_Busqueda_Avanzada_Ciudadano" runat="server">
    <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
            <table style="width: 100%;">
                <tr>
                    <td colspan="2" align="left">
                        <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
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
        <div id="Div_Cuerpo" style="width: 98%;" runat="server" visible="true">
            <asp:ScriptManager ID="ScptM_Busqueda_Avanzada" runat="server" />
       
            <table style="width: 98%;" border="0" cellspacing="0" class="estilo_fuente">
                <tr>
                    <td colspan="5">
                        &nbsp;
                    </td>
                </tr>
                <tr class="barra_busqueda">
                    <td align="center" class="style2">
                        &nbsp;
                    </td>
                    <td align="center">
                        <asp:ImageButton ID="Btn_Buscar_Cuenta" runat="server"
                            CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar"
                            OnClick="Btn_Buscar_Cuenta_Click" />
                        <asp:ImageButton ID="Btn_Limpiar" runat="server" AlternateText="Limpiar" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" OnClick="Btn_Limpiar_Click"
                            Width="24px" />
                    </td>
                    <td align="center">
                        &nbsp;
                    </td>
                    <td style="text-align: right;">
                        <asp:ImageButton ID="Btn_Regresar" runat="server" AlternateText="Regresar" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Regresar_Click"
                            Width="24px" />
                    </td>
                    <td align="center" class="style2">
                        &nbsp;
                    </td>
                </tr>
            </table>
            
            <table width="100%">
                <tr>  
                    <td style="width:15%">
                        <asp:Label ID="Lbl_Filtro_Cuenta" runat="server" Text="Cuenta"></asp:Label>
                    </td>
                
                    <td style="width:85%">
                        <asp:TextBox ID="Txt_Filtro_Cuenta" runat="server"  Width="85%" ></asp:TextBox>
                    </td>
                    
                    
                </tr>     
                <tr>
                    <td style="width:15%">
                        <asp:Label ID="Lbl_Filtro_Nombre" runat="server" Text="Nombre(s)"></asp:Label>
                    </td>
                    <td style="width:85%">
                        <asp:TextBox ID="Txt_Filtro_Nombre" runat="server" Width="85%" MaxLength="50" ></asp:TextBox>
                        
                    </td> 
                </tr> 
            </table>
            
                <table class="estilo_fuente" width="100%">      
                    <tr>
                        <td style="width:100%;text-align:center;vertical-align:top;">
                            <center>
                                <div id="Div_Ciudadano" runat="server" style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;display:block">                              
                                    <asp:GridView ID="Grid_Cuentas" runat="server" CssClass="GridView_1"
                                        AutoGenerateColumns="False"  Width="96%"
                                        GridLines= "None"
                                        EmptyDataText="No se encuentra ningun registro"
                                        onselectedindexchanged="Grid_Cuentas_SelectedIndexChanged" >
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <%--0--%>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                            </asp:ButtonField>
                                            <%--1--%>
                                            <asp:BoundField DataField="CUENTA_CONTABLE_ID">
                                            </asp:BoundField>
                                            <%--2--%>
                                            <asp:BoundField DataField="CLAVE" HeaderText="Clave" >
                                                <HeaderStyle HorizontalAlign="Left" Width="15%" Font-Size="13px" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="12px" />
                                            </asp:BoundField>
                                            <%--3--%>
                                            <asp:BoundField DataField="DESCRIPCION" HeaderText="Nombre" 
                                                SortExpression="">
                                                <HeaderStyle HorizontalAlign="Left" Width="60%" Font-Size="13px" />
                                                <ItemStyle HorizontalAlign="Left" Width="60%" Font-Size="12px" />
                                            </asp:BoundField>
                                            <%--4--%>
                                            <asp:BoundField DataField="Clave_Nombre" HeaderText="" 
                                                SortExpression="">
                                                <HeaderStyle HorizontalAlign="Left" Width="0%" Font-Size="13px" />
                                                <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size="12px" />
                                            </asp:BoundField>
                                            <%--5--%>
                                            <asp:BoundField DataField="IMPORTE" HeaderText="Importe" 
                                                SortExpression="">
                                                <HeaderStyle HorizontalAlign="Left" Width="15%" Font-Size="13px" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="12px" />
                                            </asp:BoundField>
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <HeaderStyle CssClass="GridHeader" />                                
                                        <AlternatingRowStyle CssClass="GridAltItem" />       
                                </asp:GridView>
                            </div>
                        </center>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>


<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Busqueda_Avanzada_Formatos.aspx.cs" Inherits="paginas_Tramites_Ventanas_Emergente_Frm_Busqueda_Avanzada_Formatos" %>

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
    <form id="Frm_Busqueda_Avanzada_Tramites" runat="server">
        
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
                        <asp:ImageButton ID="Btn_Buscar" runat="server"
                            CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" ToolTip="Consultar"
                            OnClick="Btn_Buscar_Click" />
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
           
        
            <table width="99%">   
                <tr>
                    <td style="width:15%">
                        <asp:Label ID="Lbl_Nombre_Formato" runat="server"  Text="Nombre del formato" Width="100%"></asp:Label> 
                    </td>
                    <td style="width:80%">
                        <asp:TextBox ID="Txt_Nombre_Formato" runat="server" Width="98%"></asp:TextBox>
                    </td> 
                    <td style="width:5%" rowspan="2" align="center">
                      
                    </td>
                </tr>
            </table> 
            
            <table class="estilo_fuente" width="100%">      
                <tr>
                    <td style="width:100%;text-align:center;vertical-align:top;">
                        <center>
                            <div id="Div_Tramites_Generales" runat="server" 
                                style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block">                              
                                <asp:GridView ID="Grid_Tramites_Generales" runat="server" CssClass="GridView_1"
                                    AutoGenerateColumns="False"  Width="98%"
                                    GridLines= "None"
                                    EmptyDataText="No se encuentra ningun registro favor"
                                    onselectedindexchanged="Grid_Tramites_Generales_SelectedIndexChanged" >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="FORMATO_ID" HeaderText="formato ID"
                                            SortExpression="FORMATO_ID">
                                            <HeaderStyle HorizontalAlign="Left" Width="0%" Font-Size="13px" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size="12px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre formato" 
                                            SortExpression="NOMBRE">
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" Font-Size="13px" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="12px" />
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

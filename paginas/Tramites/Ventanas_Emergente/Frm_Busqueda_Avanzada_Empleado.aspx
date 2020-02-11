<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Busqueda_Avanzada_Empleado.aspx.cs" 
Inherits="paginas_Tramites_Ventanas_Emergente_Frm_Busqueda_Avanzada_Empleado" %>


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
                        <asp:ImageButton ID="Btn_Buscar_Solicitante" runat="server"
                            CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar"
                            OnClick="Btn_Buscar_Solicitante_Click" />
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
             
             
            
            <asp:Panel ID="Pnl_Filtro_Empleado" runat="server" GroupingText="Buscar Empleado" ForeColor="Blue">  
                <table width="100%">
                    <tr>
                        <td style="width:20%" >
                            Empleado
                        </td>
                        <td style="width:30%" >
                           <asp:TextBox id="Txt_Filtro_Nombre_Empleado" runat="server" Width="95%" />
                        </td>
                        <td style="width:20%" align="right">
                            No Empleado
                        </td>
                        <td style="width:30%" >
                           <asp:TextBox id="Txt_Filtro_Numero_Empleado" runat="server" Width="95%"/>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Filtro_Numero_Empleado" runat="server" FilterType="Numbers"
                                    TargetControlID="Txt_Filtro_Numero_Empleado"/>
                                <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Filtro_Numero_Empleado" runat="server" 
                                    TargetControlID ="Txt_Filtro_Numero_Empleado" WatermarkText="Busqueda por No Empleado" 
                                    WatermarkCssClass="watermarked"/>        
                        </td>
                        
                    </tr>
                    <tr>
                        <td style="width:20%" >
                            U. Responsable
                        </td>
                        <td style="width:30%" colspan="3" >
                          <asp:DropDownList id="Cmb_Filtro_Unidad_Responsable" runat="server" Width="500px" 
                            AutoPostBack="False" 
                            DropDownStyle="DropDownList" 
                            AutoCompleteMode="SuggestAppend" 
                            CaseSensitive="False" 
                            CssClass="WindowsStyle" 
                            ItemInsertLocation="Append"/>
                        </td>
                    </tr>
                </table>
                        
            </asp:Panel>
            
                <table class="estilo_fuente" width="100%">      
                    <tr>
                        <td style="width:100%;text-align:center;vertical-align:top;">
                            <center>
                                <div id="Div_Ciudadano" runat="server" style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;display:block">                              
                                    <asp:GridView ID="Grid_Empleado" runat="server" CssClass="GridView_1"
                                        AutoGenerateColumns="False"  Width="96%"
                                        GridLines= "None"
                                        EmptyDataText="No se encuentra ningun registro"
                                        onselectedindexchanged="Grid_Empleado_SelectedIndexChanged" >
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="EMPLEADO_ID" HeaderText="EMPLEADO_ID"
                                                SortExpression="EMPLEADO_ID">
                                                <HeaderStyle HorizontalAlign="Left" Width="0%" Font-Size="13px" />
                                                <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size="12px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Nombre_Empleado" HeaderText="NOMBRE" 
                                                SortExpression="Nombre_Empleado">
                                                <HeaderStyle HorizontalAlign="Left" Width="50%" Font-Size="12px" />
                                                <ItemStyle HorizontalAlign="Left" Width="50%" Font-Size="10px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus"
                                                  HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="10px">
                                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NOMBRE_DEPENDENCIA" HeaderText="Unidad Resp."
                                                  HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="10px">
                                                <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                <ItemStyle HorizontalAlign="Left" Width="30%" />
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
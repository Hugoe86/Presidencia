<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Busqueda_Avanzada_Colonias_Calles.aspx.cs"
    Inherits="paginas_Predial_Ventanas_Emergentes_Frm_Busqueda_Avanzada_Colonias_Calles" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TASAS</title>
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
        }
        .GridAltItem
        {
            background-color: #E6E6E6; /*#E6E6E6 #E0F8F7*/
            color: #25406D;
        }
        .GridSelected
        {
            background-color: #A4A4A4; /*#A9F5F2;*/
            color: #25406D;
        }
        .renglon_botones
        {
            vertical-align: middle;
            height: 40px;
        }
        .style2
        {
            width: 4px;
        }
        .style3
        {
            width: 25%;
        }
        .style4
        {
            width: 25%;
        }
        .style6
        {
            width: 25%;
            text-align: right;
        }
        .style7
        {
            width: 10%;
        }
        .style8
        {
        }
    </style>
</head>
<body>

    <script type="text/javascript" language="javascript">

    //Metodos para abrir los Modal PopUp's de la página
    function Abrir_Busqueda_Colonias() {
        $find('Busqueda_Colonias').show();
        Window_Resize();
        return false;
    }

    function Abrir_Busqueda_Calles() {
        $find('Busqueda_Calles').show();
        Window_Resize();
        return false;
    }
    
    function Window_Resize()
    {
        var myWidth = 0;
        var myHeight = 0;
        var yWithScroll = 0;
        var xWithScroll = 0;

        //Obtiene las dimensiones de la ventana emergente en su alto y ancho.
        if( typeof( window.innerWidth ) == 'number' ) 
        {
            //Non-IE
            myWidth = window.innerWidth+18;
            myHeight = window.innerHeight+77;
        }
        else if( document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight ) ) 
        {
            //IE 6+ in 'standards compliant mode'
            myWidth = document.documentElement.clientWidth;
            myHeight = document.documentElement.clientHeight;
        }
        else if( document.body && ( document.body.clientWidth || document.body.clientHeight ) ) 
        {
            //IE 4 compatible
            myWidth = document.body.clientWidth+18;
            myHeight = document.body.clientHeight+77;
        }
        //Aplica un alto más grande a la ventana
        window.resizeTo(myWidth,myHeight/2);

        //Obtiene los máximos en desplazamiento de scroll vertical y horizontal
        if( typeof( window.innerWidth ) == 'number' ) 
        {
            //Non-IE
            myWidth = window.innerWidth+18;
            myHeight = window.innerHeight+77;
        }
        else if( document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight ) ) 
        {
            //IE 6+ in 'standards compliant mode'
            myWidth = document.documentElement.clientWidth;
            myHeight = document.documentElement.clientHeight;
        }
        else if( document.body && ( document.body.clientWidth || document.body.clientHeight ) ) 
        {
            //IE 4 compatible
            myWidth = document.body.clientWidth+18;
            myHeight = document.body.clientHeight+77;
        }
        if (window.scrollMaxY > 0 || window.scrollMaxX > 0)
        {
            // Firefox 
            yWithScroll = window.scrollMaxY; 
            xWithScroll = window.scrollMaxX; 
        }
        else if (document.body.scrollHeight > document.body.offsetHeight
                || document.body.scrollWidth > document.body.offsetWidth)
        { 
            // all but Explorer Mac 
            yWithScroll = document.body.scrollHeight; 
            xWithScroll = document.body.scrollWidth; 
        }
        else if (document.documentElement.scrollHeight > document.documentElement.offsetHeight
                || document.documentElement.scrollWidth > document.documentElement.offsetWidth)
        { 
            // Explorer 
            yWithScroll = document.documentElement.scrollHeight; 
            xWithScroll = document.documentElement.scrollWidth; 
        }
        
        //Redimensiona el alto y ancho de la ventana para ajustar el contenido y no se visualice la barra del scroll
        window.resizeTo(myWidth+xWithScroll,myHeight+yWithScroll);
        
        var clientWidth = 0;
        var clientHeight = 0;
        //Obtiene el ancho y alto del panel de búsqueda
        if (document.getElementById("<%=Pnl_Busqueda_Contenedor_Colonias.ClientID%>").style.display != 'none')
        {
            clientWidth = document.getElementById("<%=Pnl_Busqueda_Contenedor_Colonias.ClientID%>").clientWidth;
            clientHeight = document.getElementById("<%=Pnl_Busqueda_Contenedor_Colonias.ClientID%>").clientHeight;
            //Ajusta el alto y/o ancho de la ventana al contenido del panel
            window.resizeTo((myWidth+xWithScroll) + (clientWidth - (myWidth+xWithScroll)) + 22,(myHeight+yWithScroll) + (clientHeight - (myHeight+yWithScroll)) + 80);
//            document.getElementsByTagName("html")[0].style.overflow = "hidden";
        }
//        else
//        {
//            document.getElementsByTagName("html")[0].style.overflow = "auto";
//        }
        //Obtiene el ancho y alto del panel de búsqueda
        if (document.getElementById("<%=Pnl_Busqueda_Contenedor_Calles.ClientID%>").style.display != 'none')
        {
            clientWidth = document.getElementById("<%=Pnl_Busqueda_Contenedor_Calles.ClientID%>").clientWidth;
            clientHeight = document.getElementById("<%=Pnl_Busqueda_Contenedor_Calles.ClientID%>").clientHeight;
            //Ajusta el alto y/o ancho de la ventana al contenido del panel
            window.resizeTo((myWidth+xWithScroll) + (clientWidth - (myWidth+xWithScroll)) + 22,(myHeight+yWithScroll) + (clientHeight - (myHeight+yWithScroll)) + 80);
//            document.getElementsByTagName("html")[0].style.overflow = "hidden";
        }
//        else
//        {
//            document.getElementsByTagName("html")[0].style.overflow = "auto";
//        }
    }

    </script>

    <form id="Frm_Busqueda_Avanzada_Cuentas_Predial" method="post" runat="server">
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
    <div>
        <asp:ScriptManager ID="ScptM_Busqueda_Avanzada" runat="server" />
        <%--    <asp:UpdatePanel ID="Upd_Parametros_Predial" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
--%>
        <table style="width: 98%;" border="0" cellspacing="0" class="estilo_fuente">
            <tr>
                <td align="center" class="style2">
                    &nbsp;
                </td>
                <td align="center" class="style8">
                    &nbsp;
                </td>
                <td align="center" class="style3">
                    &nbsp;
                </td>
                <td align="center" class="style7">
                    &nbsp;
                </td>
                <td align="center" class="style3">
                    &nbsp;
                </td>
                <td align="center" class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr class="barra_busqueda">
                <td align="center" class="style2">
                    &nbsp;
                </td>
                <td align="center" class="style8" colspan="2">
                    <asp:ImageButton ID="Btn_Regresar" runat="server" AlternateText="Regresar" CssClass="Img_Button"
                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Regresar_Click"
                        Width="24px" />
                </td>
                <td align="center" class="style7">
                    &nbsp;
                </td>
                <td align="center" class="style6">
                    <asp:ImageButton ID="Btn_Limpiar" runat="server" AlternateText="Limpiar" CssClass="Img_Button"
                        ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" OnClick="Btn_Limpiar_Click"
                        Width="24px" />
                </td>
                <td align="center" class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;
                </td>
                <td class="style8">
                    &nbsp;
                </td>
                <td class="style3">
                    &nbsp;
                </td>
                <td class="style7">
                    &nbsp;
                </td>
                <td class="style3">
                    &nbsp;
                </td>
                <td class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;
                </td>
                <td class="style8" style="text-align: left; font-size: 12px;">
                    Colonia
                </td>
                <td class="style3" style="width: 50%; text-align: left; font-size: 11px;">
                    <asp:HiddenField ID="Hdn_Colonia_ID" runat="server" />
                    <asp:TextBox ID="Txt_Busqueda_Colonia" runat="server" Width="80%" OnTextChanged="Txt_Busqueda_Colonia_TextChanged"
                        AutoPostBack="True" Style="text-transform: uppercase" />
                    <asp:ImageButton ID="Btn_Buscar_Colonias" runat="server" AlternateText="Buscar" CssClass="Img_Button"
                        ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Width="24px" OnClick="Btn_Buscar_Colonias_Click" />
                    <cc1:FilteredTextBoxExtender ID="Txt_Busqueda_Colonia_FilteredTextBoxExtender" runat="server"
                        FilterType="Numbers, LowercaseLetters, UppercaseLetters, CUSTOM" ValidChars=" ÁáÉéÍíÓóÚúÑñ"
                        TargetControlID="Txt_Busqueda_Colonia" />
                    <cc1:TextBoxWatermarkExtender ID="Txt_Busqueda_Colonia_TextBoxWatermarkExtender"
                        runat="server" TargetControlID="Txt_Busqueda_Colonia" WatermarkText="Búsqueda de Colonias por Aproximación"
                        WatermarkCssClass="watermarked" />
                </td>
                <td class="style7">
                    Clave
                </td>
                <td class="style3">
                    <asp:TextBox ID="Txt_Busqueda_Clave_Colonia" runat="server" Width="70%" OnTextChanged="Txt_Busqueda_Colonia_TextChanged"
                        AutoPostBack="True" Style="text-transform: uppercase" />
                    <asp:ImageButton ID="Btn_Buscar_Colonias_Clave" runat="server" AlternateText="Buscar"
                        CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                        Width="24px" OnClick="Btn_Buscar_Colonias_Click" />
                    <cc1:FilteredTextBoxExtender ID="Txt_Busqueda_Clave_Colonia_FilteredTextBoxExtender"
                        runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, CUSTOM"
                        ValidChars=" ÁáÉéÍíÓóÚúÑñ" TargetControlID="Txt_Busqueda_Clave_Colonia" />
                    <cc1:TextBoxWatermarkExtender ID="Txt_Busqueda_Clave_Colonia_TextBoxWatermarkExtender"
                        runat="server" TargetControlID="Txt_Busqueda_Clave_Colonia" WatermarkText="Búsqueda por Clave"
                        WatermarkCssClass="watermarked" />
                </td>
                <td class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;
                </td>
                <td class="style7" style="text-align: left; font-size: 12px;">
                    Calle
                </td>
                <td class="style3" style="width: 50%; text-align: left; font-size: 11px;">
                    <asp:HiddenField ID="Hdn_Calle_ID" runat="server" />
                    <asp:TextBox ID="Txt_Busqueda_Calle" runat="server" Width="80%" OnTextChanged="Txt_Busqueda_Calle_TextChanged"
                        AutoPostBack="True" Style="text-transform: uppercase" />
                    <asp:ImageButton ID="Btn_Buscar_Calles" runat="server" AlternateText="Buscar" CssClass="Img_Button"
                        ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Width="24px" OnClick="Btn_Buscar_Calles_Click" />
                    <cc1:FilteredTextBoxExtender ID="Txt_Busqueda_Calle_FilteredTextBoxExtender" runat="server"
                        FilterType="Numbers, LowercaseLetters, UppercaseLetters, CUSTOM" ValidChars=" ÁáÉéÍíÓóÚúÑñ"
                        TargetControlID="Txt_Busqueda_Calle" />
                    <cc1:TextBoxWatermarkExtender ID="Txt_Busqueda_Calle_TextBoxWatermarkExtender" runat="server"
                        TargetControlID="Txt_Busqueda_Calle" WatermarkText="Búsqueda de Calles por Aproximación"
                        WatermarkCssClass="watermarked" />
                </td>
                <td class="style7">
                    Clave
                </td>
                <td class="style3">
                    <asp:TextBox ID="Txt_Busqueda_Clave_Calle" runat="server" Width="70%" OnTextChanged="Txt_Busqueda_Calle_TextChanged"
                        AutoPostBack="True" Style="text-transform: uppercase" />
                    <asp:ImageButton ID="Btn_Buscar_Calles_Clave" runat="server" AlternateText="Buscar"
                        CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                        Width="24px" OnClick="Btn_Buscar_Calles_Click" />
                    <cc1:FilteredTextBoxExtender ID="Txt_Busqueda_Clave_Calle_FilteredTextBoxExtender"
                        runat="server" FilterType="Numbers, LowercaseLetters, UppercaseLetters, CUSTOM"
                        ValidChars=" ÁáÉéÍíÓóÚúÑñ" TargetControlID="Txt_Busqueda_Clave_Calle" />
                    <cc1:TextBoxWatermarkExtender ID="Txt_Busqueda_Clave_Calle_TextBoxWatermarkExtender"
                        runat="server" TargetControlID="Txt_Busqueda_Clave_Calle" WatermarkText="Búsqueda por Clave"
                        WatermarkCssClass="watermarked" />
                </td>
                <td class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;
                </td>
                <td class="style8" style="text-align: left; font-size: 12px;">
                    &nbsp;
                </td>
                <td class="style3" style="width: 40%; text-align: left; font-size: 11px;">
                    &nbsp;
                </td>
                <td class="style7" style="text-align: left; font-size: 12px;">
                    &nbsp;
                </td>
                <td class="style3" style="width: 40%; text-align: left; font-size: 11px;">
                    &nbsp;
                </td>
                <td class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;
                </td>
                <td class="style8">
                    &nbsp;
                </td>
                <td class="style3">
                    &nbsp;
                </td>
                <td class="style7">
                    &nbsp;
                </td>
                <td class="style3">
                    &nbsp;
                </td>
                <td class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;
                </td>
                <td colspan="4">
                    <asp:GridView ID="Grid_Colonias_Calles" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CssClass="GridView_1" GridLines="None" OnPageIndexChanging="Grid_Colonias_Calles_PageIndexChanging"
                        OnSelectedIndexChanged="Grid_Colonias_Calles_SelectedIndexChanged" PageSize="5"
                        Style="white-space: normal" Width="100%" DataKeyNames="COLONIA_ID,CALLE_ID">
                        <RowStyle CssClass="GridItem" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="COLONIA_ID" HeaderText="COLONIA_ID" Visible="False">
                                <FooterStyle Font-Size="0pt" Width="0px" />
                                <HeaderStyle Font-Size="0pt" Width="0px" />
                                <ItemStyle Font-Size="0pt" Width="0px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="COLONIA_NOMBRE" HeaderText="Colonia">
                                <HeaderStyle Width="30%" />
                                <ItemStyle Width="30%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="COLONIA_CLAVE" HeaderText="Clave Colonia">
                                <HeaderStyle Width="10%" />
                                <ItemStyle Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CALLE_ID" HeaderText="CALLE_ID" Visible="False">
                                <FooterStyle Font-Size="0pt" Width="0px" />
                                <HeaderStyle Font-Size="0pt" Width="0px" />
                                <ItemStyle Font-Size="0pt" Width="0px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CALLE_NOMBRE" HeaderText="Calle">
                                <HeaderStyle Width="35%" />
                                <ItemStyle Width="35%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CALLE_CLAVE" HeaderText="Clave Calle">
                                <HeaderStyle Width="10%" />
                                <ItemStyle Width="10%" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle CssClass="GridHeader" />
                        <PagerStyle CssClass="GridHeader" />
                        <SelectedRowStyle CssClass="GridSelected" />
                    </asp:GridView>
                </td>
                <td class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;
                </td>
                <td class="style8">
                    &nbsp;
                </td>
                <td class="style3">
                    &nbsp;
                </td>
                <td class="style7">
                    &nbsp;
                </td>
                <td class="style3">
                    &nbsp;
                </td>
                <td class="style2">
                    &nbsp;
                </td>
            </tr>
        </table>
        <%--<asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                <ItemStyle Width="10%" />
                            </asp:BoundField>--%>
        <%--<asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                <ItemStyle Width="10%" />
                            </asp:BoundField>--%>
        <cc1:ModalPopupExtender ID="Mpe_Busqueda_Colonias" runat="server" TargetControlID="Btn_Comodin_Open_Busqueda_Colonias"
            PopupControlID="Pnl_Busqueda_Contenedor_Colonias" BackgroundCssClass="popUpStyle"
            BehaviorID="Busqueda_Colonias" CancelControlID="Btn_Comodin_Close_Busqueda_Colonias"
            DropShadow="true" DynamicServicePath="" Enabled="True" />
        <asp:Button ID="Btn_Comodin_Close_Busqueda_Colonias" runat="server" Style="background-color: transparent;
            border-style: none; display: none;" Text="" />
        <asp:Button ID="Btn_Comodin_Open_Busqueda_Colonias" runat="server" Style="background-color: transparent;
            border-style: none; display: none;" Text="" />
        <%--<asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                <ItemStyle Width="10%" />
                            </asp:BoundField>--%>
        <cc1:ModalPopupExtender ID="Mpe_Busqueda_Calles" runat="server" TargetControlID="Btn_Comodin_Open_Busqueda_Calles"
            PopupControlID="Pnl_Busqueda_Contenedor_Calles" BackgroundCssClass="popUpStyle"
            BehaviorID="Busqueda_Calles" CancelControlID="Btn_Comodin_Close_Busqueda_Calles"
            DropShadow="true" DynamicServicePath="" Enabled="True" />
        <asp:Button ID="Btn_Comodin_Close_Busqueda_Calles" runat="server" Style="background-color: transparent;
            border-style: none; display: none;" Text="" />
        <asp:Button ID="Btn_Comodin_Open_Busqueda_Calles" runat="server" Style="background-color: transparent;
            border-style: none; display: none;" Text="" />
        <%--<asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                <ItemStyle Width="10%" />
                            </asp:BoundField>--%>
        <asp:Panel ID="Pnl_Busqueda_Contenedor_Colonias" runat="server" CssClass="drag" Style="display: none;
            width: 650px; border-style: outset; border-color: Silver; background-image: url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);
            background-repeat: repeat-y;">
            <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" Style="cursor: move; background-color: Silver;
                color: Black; font-size: 12; font-weight: bold; border-style: outset;">
                <table width="99%">
                    <tr>
                        <td style="color: Black; text-align: center; font-size: 12; font-weight: bold;">
                            <asp:Image ID="Img_Icono_Busqueda_Colonias" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" />
                            B&uacute;squeda: Colonias
                        </td>
                        <td align="right" style="width: 5%;">
                            <asp:ImageButton ID="Btn_Cerrar_Busqueda_Colonias" runat="server" Style="cursor: pointer;"
                                ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"
                                OnClick="Btn_Cerrar_Busqueda_Colonias_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:UpdatePanel ID="Upd_Panel_Busqueda_Colonias" runat="server">
                <ContentTemplate>
                    <div style="color: #5D7B9D">
                        <table width="100%">
                            <tr>
                                <td align="left" style="text-align: left;">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 100%" colspan="2" align="right">
                                                <asp:ImageButton ID="Btn_Limpiar_Busqueda_Colonias" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_clear.png"
                                                    ToolTip="Limpiar Controles de Busqueda" OnClick="Btn_Limpiar_Busqueda_Colonias_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%" colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%; text-align: left; font-size: 11px;">
                                                Colonia ID
                                            </td>
                                            <td style="width: 35%; text-align: left; font-size: 11px;">
                                                <asp:TextBox ID="Txt_Busqueda_Avanzada_Colonia_ID" runat="server" Width="40%" MaxLength="20"
                                                    OnTextChanged="Txt_Busqueda_Avanzada_Colonia_ID_TextChanged" />
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Avanzada_Colonia_ID" runat="server"
                                                    FilterType="Numbers, LowercaseLetters, UppercaseLetters" TargetControlID="Txt_Busqueda_Avanzada_Colonia_ID" />
                                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Avanzada_Colonia_ID" runat="server"
                                                    TargetControlID="Txt_Busqueda_Avanzada_Colonia_ID" WatermarkText="Búsqueda por ID"
                                                    WatermarkCssClass="watermarked" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%; text-align: left; font-size: 11px;">
                                                Nombre
                                            </td>
                                            <td style="width: 85%; text-align: left;">
                                                <asp:TextBox ID="Txt_Busqueda_Avanzada_Nombre_Colonia" runat="server" Width="99.5%"
                                                    MaxLength="80" OnTextChanged="Txt_Busqueda_Avanzada_Nombre_Colonia_TextChanged" />
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Avanzada_Nombre_Colonia" runat="server"
                                                    FilterType="LowercaseLetters, UppercaseLetters, Custom" ValidChars="ÑñáéíóúÁÉÍÓÚ "
                                                    TargetControlID="Txt_Busqueda_Avanzada_Nombre_Colonia" />
                                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Avanzada_Nombre_Colonia" runat="server"
                                                    TargetControlID="Txt_Busqueda_Avanzada_Nombre_Colonia" WatermarkText="Búsqueda por Nombre de Colonia"
                                                    WatermarkCssClass="watermarked" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%" colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%; text-align: center;" colspan="2">
                                                <%--<center>--%>
                                                <asp:Button ID="Btn_Busqueda_Colonias" runat="server" Text="Buscar Colonias" CssClass="button"
                                                    CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Colonias_Click" />
                                                <%--</center>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:GridView ID="Grid_Colonias" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    CssClass="GridView_1" GridLines="None" PageSize="5" Width="98%" OnPageIndexChanging="Grid_Colonias_PageIndexChanging"
                                                    OnSelectedIndexChanged="Grid_Colonias_SelectedIndexChanged">
                                                    <RowStyle CssClass="GridItem" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                    <Columns>
                                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                            <ItemStyle Width="5%" />
                                                        </asp:ButtonField>
                                                        <asp:BoundField DataField="COLONIA_ID" HeaderText="Colonia ID">
                                                            <ItemStyle Width="15%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" HtmlEncode="true">
                                                            <ItemStyle Width="80%" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                </asp:GridView>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%; text-align: left;" colspan="2">
                                                <center>
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <%--</center>--%>
        <asp:Panel ID="Pnl_Busqueda_Contenedor_Calles" runat="server" CssClass="drag" Style="display: none;
            width: 650px; border-style: outset; border-color: Silver; background-image: url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);
            background-repeat: repeat-y;">
            <asp:Panel ID="Panel2" runat="server" Style="cursor: move; background-color: Silver;
                color: Black; font-size: 12; font-weight: bold; border-style: outset;">
                <table width="99%">
                    <tr>
                        <td style="color: Black; text-align: center; font-size: 12; font-weight: bold;">
                            <asp:Image ID="Img_Icono_Busqueda_Calles" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" />
                            B&uacute;squeda: Calles
                        </td>
                        <td align="right" style="width: 5%;">
                            <asp:ImageButton ID="Btn_Cerrar_Busqueda_Calles" runat="server" Style="cursor: pointer;"
                                ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"
                                OnClick="Btn_Cerrar_Busqueda_Calles_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:UpdatePanel ID="Upd_Panel_Busqueda_Calles" runat="server">
                <ContentTemplate>
                    <div style="color: #5D7B9D">
                        <table width="100%">
                            <tr>
                                <td align="left" style="text-align: left;">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 100%" colspan="2" align="right">
                                                <asp:ImageButton ID="Btn_Limpiar_Busqueda_Calles" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_clear.png"
                                                    ToolTip="Limpiar Controles de Busqueda" OnClick="Btn_Limpiar_Busqueda_Calles_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%" colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%; text-align: left; font-size: 11px;">
                                                Calle ID
                                            </td>
                                            <td style="width: 35%; text-align: left; font-size: 11px;">
                                                <asp:TextBox ID="Txt_Busqueda_Avanzada_Calle_ID" runat="server" Width="40%" MaxLength="20"
                                                    OnTextChanged="Txt_Busqueda_Avanzada_Calle_ID_TextChanged" />
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Avanzada_Calle_ID" runat="server"
                                                    FilterType="Numbers, LowercaseLetters, UppercaseLetters" TargetControlID="Txt_Busqueda_Avanzada_Calle_ID" />
                                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Avanzada_Calle_ID" runat="server"
                                                    TargetControlID="Txt_Busqueda_Avanzada_Calle_ID" WatermarkText="Búsqueda por ID"
                                                    WatermarkCssClass="watermarked" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%; text-align: left; font-size: 11px;">
                                                Nombre
                                            </td>
                                            <td style="width: 85%; text-align: left;">
                                                <asp:TextBox ID="Txt_Busqueda_Avanzada_Nombre_Calle" runat="server" Width="99.5%"
                                                    MaxLength="80" OnTextChanged="Txt_Busqueda_Avanzada_Nombre_Calle_TextChanged" />
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Avanzada_Nombre_Calle" runat="server"
                                                    FilterType="LowercaseLetters, UppercaseLetters, Custom" ValidChars="ÑñáéíóúÁÉÍÓÚ "
                                                    TargetControlID="Txt_Busqueda_Avanzada_Nombre_Calle" />
                                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Avanzada_Nombre_Calle" runat="server"
                                                    TargetControlID="Txt_Busqueda_Avanzada_Nombre_Calle" WatermarkText="Búsqueda por Nombre de Calle"
                                                    WatermarkCssClass="watermarked" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%" colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%; text-align: center;" colspan="2">
                                                <%--<center>--%>
                                                <asp:Button ID="Btn_Busqueda_Calles" runat="server" Text="Buscar Calles" CssClass="button"
                                                    CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Calles_Click" />
                                                <%--</center>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:GridView ID="Grid_Calles" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    CssClass="GridView_1" GridLines="None" PageSize="5" Width="98%" OnPageIndexChanging="Grid_Calles_PageIndexChanging"
                                                    OnSelectedIndexChanged="Grid_Calles_SelectedIndexChanged">
                                                    <RowStyle CssClass="GridItem" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                    <Columns>
                                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                            <ItemStyle Width="5%" />
                                                        </asp:ButtonField>
                                                        <asp:BoundField DataField="CALLE_ID" HeaderText="Calle ID">
                                                            <ItemStyle Width="15%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                                            <ItemStyle Width="80%" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                </asp:GridView>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%; text-align: left;" colspan="2">
                                                <center>
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    </form>
</body>
</html>

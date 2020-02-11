<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Busqueda_Avanzada_Cuentas_Predial.aspx.cs"
    Inherits="paginas_Predial_Ventanas_Emergentes_Frm_Busqueda_Avanzada_Cuentas_Predial"
    Culture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cuentas Predial</title>

    <script src="../../../easyui/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
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
    window.onerror = new Function("return true");
	//Abrir una ventana modal
	function Abrir_Ventana_Modal(Url, Propiedades)
	{
		window.showModalDialog(Url, null, Propiedades);
	}

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
    }

    </script>

    <form id="Frm_Busqueda_Avanzada_Cuentas_Predial" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
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
                            <asp:ImageButton ID="Btn_Buscar" runat="server" AlternateText="Buscar" CssClass="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Width="24px" OnClick="Btn_Buscar_Click" />
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
                            <asp:HiddenField ID="Hdn_Estatus_Cuentas" runat="server" />
                        </td>
                        <td class="style7">
                            &nbsp;
                        </td>
                        <td class="style3">
                            <asp:HiddenField ID="Hdn_Tipo_Contribuyente" runat="server" />
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
                            Cuenta Predial
                        </td>
                        <td class="style3" style="width: 40%; text-align: left; font-size: 11px;">
                            <asp:TextBox ID="Txt_Busqueda_Cuenta_Predial" runat="server" Width="98%" MaxLength="12"
                                Style="text-transform: uppercase" />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Cuenta_Predial" runat="server"
                                FilterType="Numbers, LowercaseLetters, UppercaseLetters" TargetControlID="Txt_Busqueda_Cuenta_Predial" />
                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Cuenta_Predial" runat="server"
                                TargetControlID="Txt_Busqueda_Cuenta_Predial" WatermarkText="Búsqueda por Cuenta Predial"
                                WatermarkCssClass="watermarked" />
                        </td>
                        <td class="style7" style="text-align: left; font-size: 11px;">
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
                        <td class="style8" style="text-align: left; font-size: 12px;">
                            Propietario
                        </td>
                        <td class="style3" style="width: 35%; text-align: left; font-size: 11px;" colspan="3">
                            <asp:TextBox ID="Txt_Busqueda_Propietatio" runat="server" Width="99.5%" MaxLength="80"
                                Style="text-transform: uppercase" />
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Propietatio" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom"
                                ValidChars="ÑñáéíóúÁÉÍÓÚ " TargetControlID="Txt_Busqueda_Propietatio" />
                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Propietatio" runat="server" TargetControlID="Txt_Busqueda_Propietatio"
                                WatermarkText="Búsqueda por Nombre de Propietario o Copropietario" WatermarkCssClass="watermarked" />
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
                        <td class="style3" style="width: 40%; text-align: left; font-size: 11px;">
                            <asp:HiddenField ID="Hdn_Colonia_ID" runat="server" />
                            <asp:TextBox ID="Txt_Busqueda_Colonia" runat="server" Width="80%" ReadOnly="True"
                                Style="text-transform: uppercase" />
                            <asp:ImageButton ID="Btn_Busqueda_Avanzada_Colonias" runat="server" AlternateText="Buscar"
                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                Width="24px" OnClick="Btn_Buscar_Colonias_Click" />
                            <cc1:FilteredTextBoxExtender ID="Txt_Busqueda_Colonia_FilteredTextBoxExtender" runat="server"
                                FilterType="Numbers, LowercaseLetters, UppercaseLetters, CUSTOM" ValidChars=" "
                                TargetControlID="Txt_Busqueda_Colonia" />
                            <cc1:TextBoxWatermarkExtender ID="Txt_Busqueda_Colonia_TextBoxWatermarkExtender"
                                runat="server" TargetControlID="Txt_Busqueda_Colonia" WatermarkText="Búsqueda de Colonias por Aproximación"
                                WatermarkCssClass="watermarked" />
                        </td>
                        <td class="style7" style="text-align: left; font-size: 12px;">
                            Calle
                        </td>
                        <td class="style3" style="width: 40%; text-align: left; font-size: 11px;">
                            <asp:HiddenField ID="Hdn_Calle_ID" runat="server" />
                            <asp:TextBox ID="Txt_Busqueda_Calle" runat="server" Width="80%" ReadOnly="True" Style="text-transform: uppercase" />
                            <asp:ImageButton ID="Btn_Busqueda_Avanzada_Calles" runat="server" AlternateText="Buscar"
                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                Width="24px" OnClick="Btn_Buscar_Colonias_Click" />
                            <cc1:FilteredTextBoxExtender ID="Txt_Busqueda_Calle_FilteredTextBoxExtender" runat="server"
                                FilterType="Numbers, LowercaseLetters, UppercaseLetters, CUSTOM" ValidChars=" "
                                TargetControlID="Txt_Busqueda_Calle" />
                            <cc1:TextBoxWatermarkExtender ID="Txt_Busqueda_Calle_TextBoxWatermarkExtender" runat="server"
                                TargetControlID="Txt_Busqueda_Calle" WatermarkText="Búsqueda de Calles por Aproximación"
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
                            <asp:GridView ID="Grid_Cuentas_Predial" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CssClass="GridView_1" GridLines="None" OnPageIndexChanging="Grid_Cuentas_Predial_PageIndexChanging"
                                OnSelectedIndexChanged="Grid_Cuentas_Predial_SelectedIndexChanged" PageSize="5"
                                Style="white-space: normal" Width="100%" DataKeyNames="CUENTA_PREDIAL_ID,COLONIA_ID,CALLE_ID">
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="CUENTA_PREDIAL_ID" Visible="False">
                                        <FooterStyle Font-Size="0pt" Width="0px" />
                                        <HeaderStyle Font-Size="0pt" Width="0px" />
                                        <ItemStyle Font-Size="0pt" Width="0px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial">
                                        <ItemStyle Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE_PROPIETARIO" HeaderText="Propietario">
                                        <ItemStyle Width="30%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="COLONIA_ID" HeaderText="COLONIA_ID" Visible="False">
                                        <FooterStyle Font-Size="0pt" Width="0px" />
                                        <HeaderStyle Font-Size="0pt" Width="0px" />
                                        <ItemStyle Font-Size="0pt" Width="0px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE_COLONIA" HeaderText="Colonia">
                                        <ItemStyle Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CALLE_ID" HeaderText="CALLE_ID" Visible="False">
                                        <FooterStyle Font-Size="0pt" Width="0px" />
                                        <HeaderStyle Font-Size="0pt" Width="0px" />
                                        <ItemStyle Font-Size="0pt" Width="0px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE_CALLE" HeaderText="Calle">
                                        <ItemStyle Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_EXTERIOR" HeaderText="No. Exterior">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="20%" />
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Busqueda_Avanzada_Tabla_Valores_Avaluo_Rustico.aspx.cs" Inherits="paginas_Catastro_Ventanas_Emergentes_Frm_Busqueda_Avanzada_Tabla_Valores_Avaluo_Rustico" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Tabla de Valores por tramos de calle</title>

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
        function Abrir_Ventana_Modal(Url, Propiedades) {
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

        function Window_Resize() {
            var myWidth = 0;
            var myHeight = 0;
            var yWithScroll = 0;
            var xWithScroll = 0;

            //Obtiene las dimensiones de la ventana emergente en su alto y ancho.
            if (typeof (window.innerWidth) == 'number') {
                //Non-IE
                myWidth = window.innerWidth + 18;
                myHeight = window.innerHeight + 77;
            }
            else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
                //IE 6+ in 'standards compliant mode'
                myWidth = document.documentElement.clientWidth;
                myHeight = document.documentElement.clientHeight;
            }
            else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
                //IE 4 compatible
                myWidth = document.body.clientWidth + 18;
                myHeight = document.body.clientHeight + 77;
            }
            //Aplica un alto más grande a la ventana
            window.resizeTo(myWidth, myHeight / 2);

            //Obtiene los máximos en desplazamiento de scroll vertical y horizontal
            if (typeof (window.innerWidth) == 'number') {
                //Non-IE
                myWidth = window.innerWidth + 18;
                myHeight = window.innerHeight + 77;
            }
            else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
                //IE 6+ in 'standards compliant mode'
                myWidth = document.documentElement.clientWidth;
                myHeight = document.documentElement.clientHeight;
            }
            else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
                //IE 4 compatible
                myWidth = document.body.clientWidth + 18;
                myHeight = document.body.clientHeight + 77;
            }
            if (window.scrollMaxY > 0 || window.scrollMaxX > 0) {
                // Firefox 
                yWithScroll = window.scrollMaxY;
                xWithScroll = window.scrollMaxX;
            }
            else if (document.body.scrollHeight > document.body.offsetHeight
                || document.body.scrollWidth > document.body.offsetWidth) {
                // all but Explorer Mac 
                yWithScroll = document.body.scrollHeight;
                xWithScroll = document.body.scrollWidth;
            }
            else if (document.documentElement.scrollHeight > document.documentElement.offsetHeight
                || document.documentElement.scrollWidth > document.documentElement.offsetWidth) {
                // Explorer 
                yWithScroll = document.documentElement.scrollHeight;
                xWithScroll = document.documentElement.scrollWidth;
            }

            //Redimensiona el alto y ancho de la ventana para ajustar el contenido y no se visualice la barra del scroll
            window.resizeTo(myWidth + xWithScroll, myHeight + yWithScroll);

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
                            <asp:ImageButton ID="Btn_Salir" runat="server" AlternateText="Regresar" CssClass="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click"
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
                            
                        </td>
                        <td class="style7">
                            &nbsp;
                        </td>
                        <td class="style3">
                            
                        </td>
                        <td class="style2">
                            
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            &nbsp;
                        </td>
                        <td class="style8" style="text-align: left; font-size: 12px;">
                            Tipo Construccion
                        </td>
                        <td class="style3" style="width: 40%; text-align: left; font-size: 11px;">
                            <asp:TextBox ID="Txt_Tipo_Construccion" runat="server" Width="98%"
                                Style="text-transform: uppercase" />
                                <asp:HiddenField ID="Hdf_Tipo_Construccion" runat="server" />
                        </td>
                        <td class="style8" style="text-align: left; font-size: 12px;">
                        <%--Estatus--%>
                        </td>
                        <td class="style3" style="width: 40%; text-align: left; font-size: 11px;">
                        <%--<asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" TabIndex="12" Enabled="false">
                                <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE"></asp:ListItem>
                                <asp:ListItem Text="VIGENTE" Value="VIGENTE"></asp:ListItem>
                                <asp:ListItem Text="BAJA" Value="BAJA"></asp:ListItem> 
                            </asp:DropDownList>--%>
                        </td>
                        <td class="style2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                    <div id="Div_Grid_Tipos_Construccion" runat="server" visible="false">
                        <td class="style2">
                            &nbsp;
                        </td>
                        <td colspan="4">
                            <asp:GridView ID="Grid_Tipos_Construccion" runat="server" AllowPaging="True" AllowSorting="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="15" style="white-space:normal;" 
                                                Width="100%"
                                                OnSelectedIndexChanged = "Grid_Tipos_Construccion_SelectedIndexChanged"
                                                OnPageIndexChanging = "Grid_Tipos_Construccion_PageIndexChanging">
                                                <Columns>

                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="TIPO_CONSTRUCCION_ID" HeaderStyle-Width="15%" HeaderText="ID" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="IDENTIFICADOR" HeaderStyle-Width="50%" HeaderText="Tipo de Construcción">
                                                    <HeaderStyle HorizontalAlign="Center" Width="50%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="50%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                    <HeaderStyle HorizontalAlign="Center" Width="50%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="50%" />
                                                    </asp:BoundField>
                                                    
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                        </td>
                        <td class="style2">
                            &nbsp;
                        </td>
                        </div>
                    </tr>
                    <tr>
                    <td>
                    </td>
                    </tr>
                    <tr>
                    <div id="Div_Grid_Tabla_Valores" runat="server" visible="false">
                        <td class="style2">
                            &nbsp;
                        </td>
                        <td colspan="4">
                            <asp:GridView ID="Grid_Valores" runat="server" AllowPaging="True" AllowSorting="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="10" style="white-space:normal;" 
                                                Width="100%"
                                                OnSelectedIndexChanged = "Grid_Valores_SelectedIndexChanged"
                                                OnPageIndexChanging = "Grid_Valores_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    
                                                    </asp:ButtonField>

                                                    <asp:BoundField DataField="VALOR_CONSTRU_RUSTICO_ID" HeaderStyle-Width="15%" HeaderText="Id" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="ANIO" HeaderStyle-Width="20%" HeaderText="Año">
                                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="VALOR_M2" HeaderStyle-Width="30%" HeaderText="Valor X Hectárea" DataFormatString="{0:c2}">
                                                    <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="30%" />
                                                    </asp:BoundField>

                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                        </td>
                        <td class="style2">
                            &nbsp;
                        </td>
                        </div>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Cat_Avaluo_Urbano_Av.aspx.cs"
    Inherits="paginas_Catastro_Frm_Ope_Cat_Avaluo_Urbano_Av" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    Title="Avalúo Urbano Actualización de valor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        body
        {
            font: normal 12px auto "Trebuchet MS" , Verdana;
            background-color: #ffffff;
            color: #4f6b72;
        }
        .link
        {
            color: Black;
        }
        .Label
        {
            width: 163px;
        }
        .TextBox
        {
            text-align: right;
        }
        a.enlace_fotografia:link, a.enlace_fotografia:visited
        {
            color: #25406D;
            text-decoration: underline;
            font-weight: normal;
            padding: 0 5px 0 5px;
        }
        a.enlace_fotografia:hover
        {
            color: #25406D;
            text-decoration: underline;
            font-weight: bold;
            padding: 0 5px 0 5px;
        }
        .style1
        {
            width: 239px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type='text/javascript'>

    <!--
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_Session.ashx";
 
        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion() {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }
 
        //Temporizador para matener la sesión activa
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);
        
    //-->

        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }
        
        function Abrir_Resumen(Url, Propiedades) {
            window.open(Url, 'Resumen_Predio', Propiedades);
        }

        function ValidarCaracteres(textareaControl, maxlength) { if (textareaControl.value.length > maxlength) { textareaControl.value = textareaControl.value.substring(0, maxlength); alert("Debe ingresar hasta un máximo de " + maxlength + " caracteres"); } }
        
        function Validar_Tipo_Construccion()
		{
		    if (document.getElementById("<%=Cmb_Tipo_Construccion.ClientID%>").value == "OTRO")
		    {
		        document.getElementById("<%=Txt_Uso.ClientID%>").disabled = false;
		    }
		    else
		    {
		        document.getElementById("<%=Txt_Uso.ClientID%>").disabled = true;
		    }
		}
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server" AsyncPostBackTimeout="3600"
        EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <asp:UpdatePanel ID="Upd_Otros_Pagos" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Otros_Pagos"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Calles" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">
                            Avalúos Urbanos de Actualización de Valor
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:ImageButton ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
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
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="Btn_Comodin" runat="server" Style="background-color: Transparent;
                                border-style: none;" OnClientClick="javascript:return false;" />
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td style="vertical-align: middle; text-align: left; width: 55%;">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Nuevo" OnClick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Modificar" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Imprimir" runat="server" TabIndex="3" ImageUrl="~/paginas/imagenes/gridview/grid_print.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Imprimir" OnClick="Btn_Imprimir_Click" />
                            <asp:ImageButton ID="Btn_Imprimir_Cuentas" runat="server" TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Imprimir" OnClick="Btn_Imprimir_Cuentas_Click"
                                ToolTip="Emprimir cuentas asignadas" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click" />
                        </td>
                        <td align="right">
                            <table style="width: 100%; height: 28px;">
                                <tr>
                                    <td style="width: 20%;">
                                        Búsqueda Avalúo:
                                    </td>
                                    <td style="width: 55%;">
                                        <asp:TextBox ID="Txt_Busqueda" runat="server" Width="99%" TabIndex="5" Style="text-transform: uppercase"></asp:TextBox>
                                    </td>
                                    <td style="vertical-align: middle; width: 5%;">
                                        <asp:ImageButton ID="Btn_Buscar" runat="server" CausesValidation="false" TabIndex="6"
                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" OnClick="Btn_Buscar_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr class="barra_busqueda">
                        <td align="left" colspan="1">
                        </td>
                        <td align="right">
                            <table style="width: 100%; height: 28px;">
                                <tr>
                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                        B&uacute;squeda Estatus Avalúo:
                                    </td>
                                    <td style="width: 55%;">
                                        <asp:DropDownList ID="Cmb_Busqueda_Avaluo_Estatus" runat="server" Width="99%" OnSelectedIndexChanged="Cmb_Busqueda_Avaluo_Estatus_SelectedIndexChanged"
                                            AutoPostBack="true">
                                            <asp:ListItem Text="POR VALIDAR" Value="POR VALIDAR" />
                                            <asp:ListItem Text="RECHAZADO" Value="RECHAZADO" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="vertical-align: middle; width: 5%;">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr class="barra_busqueda">
                        <td align="left" colspan="1">
                        </td>
                        <td align="right">
                            <table style="width: 100%; height: 28px;">
                                <tr>
                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                        B&uacute;squeda Cuentas Asignadas:
                                    </td>
                                    <td style="width: 55%;">
                                        <asp:DropDownList ID="Cmb_Busqueda" runat="server" Width="45.5%">
                                            <asp:ListItem Text="COLONIA" Value="COLONIA" />
                                            <asp:ListItem Text="CALLE" Value="CALLE" />
                                            <asp:ListItem Text="PROPIETARIO" Value="PROPIETARIO" />
                                            <asp:ListItem Text="CUENTA PREDIAL" Value="CUENTA_PREDIAL" />
                                        </asp:DropDownList>
                                        <asp:TextBox ID="Txt_Busqueda_Asignadas" runat="server" MaxLength="100" TabIndex="5"
                                            ToolTip="Buscar" Width="49.5%" Style="text-transform: uppercase" />
                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                            WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda_Asignadas" />
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda_Asignadas"
                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. " />
                                    </td>
                                    <td style="vertical-align: middle; width: 5%;">
                                        <asp:ImageButton ID="Btn_Busqueda_Asignadas" runat="server" TabIndex="6" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                            ToolTip="Buscar" AlternateText="Buscar" OnClick="Btn_Busqueda_Asignadas_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <center>
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td>
                                <div id="Div_Grid_Avaluo" runat="server" visible="true">
                                    <tr style="background-color: #3366CC">
                                        <td style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                            Cuentas Asignadas
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">
                                            <asp:GridView ID="Grid_Cuentas_Asignadas" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="15" Style="white-space: normal;" Width="100%"
                                                OnSelectedIndexChanged="Grid_Cuentas_Asignadas_SelectedIndexChanged" OnPageIndexChanging="Grid_Cuentas_Asignadas_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="NO_ASIGNACION" HeaderStyle-Width="15%" HeaderText="ID"
                                                        Visible="false">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PERITO_INTERNO_ID" HeaderText="Perito Interno id" Visible="false">
                                                        <HeaderStyle HorizontalAlign="Center" Width="70%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="70%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NOMBRE_PROPIETARIO" HeaderText="Propietario">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="Cuenta Predial Id" Visible="false">
                                                        <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NOMBRE_COLONIA" HeaderText="Colonia">
                                                        <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NOMBRE_CALLE" HeaderText="Calle">
                                                        <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NO_EXTERIOR" HeaderText="No. Ext.">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NO_INTERIOR" HeaderText="No. Int">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr style="background-color: #3366CC">
                                        <td style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                            Avalúos Urbanos
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td>
                                            <asp:GridView ID="Grid_Avaluos_Urbanos" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="15" Style="white-space: normal;" Width="100%"
                                                OnSelectedIndexChanged="Grid_Avaluos_Urbanos_SelectedIndexChanged" OnPageIndexChanging="Grid_Avaluos_Urbanos_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="NO_AVALUO" HeaderStyle-Width="15%" HeaderText="ID" Visible="false">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ANIO_AVALUO" HeaderStyle-Width="15%" HeaderText="Anio"
                                                        Visible="false">
                                                        <HeaderStyle HorizontalAlign="Left" Width="70%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="70%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AVALUO" HeaderText="Avaluo">
                                                        <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FECHA_CREO" HeaderText="Fecha de Recepción" DataFormatString="{0:dd-MMM-yyyy}">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="VECES_RECHAZO" HeaderText="Veces Rechazado">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FECHA_RECHAZO" HeaderText="Fecha de Último Rechazo" DataFormatString="{0:dd-MMM-yyyy}">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial">
                                                        <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            <br />
                                        </td>
                                    </tr>
                                </div>
                                <div id="Div_Datos_Avaluo" runat="server" visible="false">
                                    <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                                        <tr style="background-color: #3366CC">
                                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                                DATOS GENERALES
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                <asp:HiddenField ID="Hdf_No_Avaluo" runat="server" />
                                                <asp:HiddenField ID="Hdf_Calle_Id" runat="server" />
                                                <asp:HiddenField ID="Hdf_Anio_Avaluo" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                                *Motivo del Avalúo
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:DropDownList ID="Cmb_Motivo_Avaluo" runat="server" Width="98%" TabIndex="7"
                                                    Enabled="false">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: Left;">
                                                No. Avalúo
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_No_Avaluo" runat="server" Width="96.4%" Enabled="false" Style="text-transform: uppercase" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                Cuenta Predial
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="80%" Enabled="false" Style="text-transform: uppercase"></asp:TextBox>
                                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial" runat="server"
                                                    Height="24px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" OnClick="Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial_Click"
                                                    ToolTip="Búsqueda Avanzada de Cuenta Predial" Width="24px" />
                                                <asp:ImageButton ID="Btn_Detalles_Cuenta_Predial" runat="server" Height="24px" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png"
                                                    ToolTip="Detalles de la cuenta" Width="24px" />
                                                <asp:HiddenField ID="Hdf_Cuenta_Predial_Id" runat="server" />
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: Left;">
                                                Clave Catastral
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                &nbsp; *Reg.<asp:TextBox ID="Txt_Region" runat="server" Width="10%" TabIndex="8"
                                                    Enabled="false" Style="text-transform: uppercase" MaxLength="20" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Region" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="Txt_Region" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>                                
                                                &nbsp; *Mzna.<asp:TextBox ID="Txt_Manzana" runat="server" Width="10%" TabIndex="9"
                                                    Enabled="false" Style="text-transform: uppercase" MaxLength="20" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Manzana" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="Txt_Manzana" ValidChars="0123456789"></cc1:FilteredTextBoxExtender> 
                                                &nbsp; *Lote<asp:TextBox ID="Txt_Lote" runat="server" Width="10%" TabIndex="10" Enabled="false"
                                                    Style="text-transform: uppercase" MaxLength="20" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Lote" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="Txt_Lote" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                Ubicación del Predio
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Ubicacion_Predio" runat="server" Width="98%" Enabled="false"
                                                    Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: Left;">
                                                Colonia
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Colonia" runat="server" Width="96.4%" Enabled="false" Style="text-transform: uppercase" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                Localidad
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Localidad" runat="server" Width="98%" Enabled="false" Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: Left;">
                                                Municipio
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Municipio" runat="server" Width="96.4%" Enabled="false" Style="text-transform: uppercase" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                Propietario
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Propietario" runat="server" Width="98%" Enabled="false" Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                            <td style="text-align: Left; width: 20%;">
                                                Estatus
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%">
                                                    <asp:ListItem Text="POR VALIDAR" Value="POR VALIDAR" />
                                                    <asp:ListItem Text="RECHAZADO" Value="RECHAZADO" />
                                                    <asp:ListItem Text="AUTORIZADO" Value="AUTORIZADO" />
                                                    <asp:ListItem Text="POR PAGAR" Value="POR PAGAR" />
                                                    <asp:ListItem Text="PAGADO" Value="PAGADO" />
                                                    <asp:ListItem Text="POR AUTORIZAR" Value="POR AUTORIZAR" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                Domicilio para Notificar
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Domicilio_Not" runat="server" Width="98%" Enabled="false" Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: Left;">
                                                Colonia
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Colonia_Not" runat="server" Width="96.4%" Enabled="false" Style="text-transform: uppercase" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                Localidad
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Localidad_Not" runat="server" Width="98%" Enabled="false" Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: Left;">
                                                Municipio
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Municipio_Not" runat="server" Width="96.4%" Enabled="false"
                                                    Style="text-transform: uppercase" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                *Solicitante
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Solicitante" runat="server" Width="98%" TabIndex="12" Enabled="false"
                                                    Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                                Habilitar para Revisión
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:DropDownList ID="Cmb_Revision" runat="server" Width="98%" TabIndex="12" Enabled="false">
                                                    <asp:ListItem Text="NO" Value="NO" />
                                                    <asp:ListItem Text="SI" Value="SI" />
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="Hdf_No_Asignacion" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                Avalúo por Fotogrametría
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:CheckBox ID="Chk_Fotogrametria" runat="server" TabIndex="13" Text="" OnCheckedChanged="Chk_Fotogrametria_CheckedChanged"
                                                    AutoPostBack="true" />
                                            </td>
                                        </tr>
                                        <div id="Div_Completo" runat="server" visible="true">
                                            <tr>
                                                <td>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr style="background-color: #3366CC">
                                                <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                                    CARACTERISTICAS DEL TERRENO
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; font-size: 15px; color: #FFFFFF; background-color: #555555;"
                                                    colspan="4">
                                                    Clasificación de la Zona
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:GridView ID="Grid_Clasificacion_Zona" runat="server" AllowPaging="False" AllowSorting="True"
                                                        AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                        HeaderStyle-CssClass="tblHead" OnDataBound="Grid_Clasificacion_Zona_DataBound"
                                                        PageSize="20" Style="white-space: normal;" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="COLUMNA_A" HeaderStyle-Width="20%">
                                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Columna_A" runat="server" Font-Size="X-Small" name="<%=Chk_Columna_A.ClientID %>"
                                                                        Width="98%" TabIndex="13"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="COLUMNA_B" HeaderStyle-Width="20%">
                                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Columna_B" runat="server" Font-Size="X-Small" name="<%=Chk_Columna_B.ClientID %>"
                                                                        Width="98%" TabIndex="13"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="COLUMNA_C" HeaderStyle-Width="20%">
                                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Columna_C" runat="server" Font-Size="X-Small" name="<%=Chk_Columna_C.ClientID %>"
                                                                        Width="98%" TabIndex="13"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="COLUMNA_D" HeaderStyle-Width="20%">
                                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Columna_D" runat="server" Font-Size="X-Small" name="<%=Chk_Columna_D.ClientID %>"
                                                                        Width="98%" TabIndex="13"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="GridSelected" />
                                                        <PagerStyle CssClass="GridHeader" />
                                                        <HeaderStyle CssClass="tblHead" />
                                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #3366CC">
                                                <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; font-size: 15px; color: #FFFFFF; background-color: #555555;"
                                                    colspan="4">
                                                    Servicios de la Zona
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:GridView ID="Grid_Servicios_Zona" runat="server" AllowPaging="False" AllowSorting="True"
                                                        AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                        HeaderStyle-CssClass="tblHead" OnDataBound="Grid_Servicios_Zona_DataBound" PageSize="20"
                                                        Style="white-space: normal;" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="COLUMNA_A" HeaderStyle-Width="20%">
                                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Columna_A" runat="server" Font-Size="X-Small" name="<%=Chk_Columna_A.ClientID %>"
                                                                        Width="98%" TabIndex="14"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="COLUMNA_B" HeaderStyle-Width="20%">
                                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Columna_B" runat="server" Font-Size="X-Small" name="<%=Chk_Columna_B.ClientID %>"
                                                                        Width="98%" TabIndex="14"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="COLUMNA_C" HeaderStyle-Width="20%">
                                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Columna_C" runat="server" Font-Size="X-Small" name="<%=Chk_Columna_C.ClientID %>"
                                                                        Width="98%" TabIndex="14"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="COLUMNA_D" HeaderStyle-Width="20%">
                                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Columna_D" runat="server" Font-Size="X-Small" name="<%=Chk_Columna_D.ClientID %>"
                                                                        Width="98%" TabIndex="14"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="GridSelected" />
                                                        <PagerStyle CssClass="GridHeader" />
                                                        <HeaderStyle CssClass="tblHead" />
                                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #3366CC">
                                                <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; font-size: 15px; color: #FFFFFF; background-color: #555555;"
                                                    colspan="4">
                                                    Construcción Dominante
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:GridView ID="Grid_Construccion_Dominante" runat="server" AllowPaging="False"
                                                        AllowSorting="True" AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                        HeaderStyle-CssClass="tblHead" OnDataBound="Grid_Construccion_Dominante_DataBound"
                                                        PageSize="20" Style="white-space: normal;" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="COLUMNA_A" HeaderStyle-Width="20%">
                                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <ItemTemplate>
                                                                    <asp:RadioButton ID="Rdb_Columna_A" runat="server" GroupName="Grp_Rbd_Constru_Dominante"
                                                                        name="<%=Rdb_Columna_A.ClientID %>" Width="98%" TabIndex="15"></asp:RadioButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="COLUMNA_B" HeaderStyle-Width="20%">
                                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <ItemTemplate>
                                                                    <asp:RadioButton ID="Rdb_Columna_B" runat="server" GroupName="Grp_Rbd_Constru_Dominante"
                                                                        name="<%=Rdb_Columna_B.ClientID %>" Width="98%" TabIndex="15"></asp:RadioButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="COLUMNA_C" HeaderStyle-Width="20%">
                                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <ItemTemplate>
                                                                    <asp:RadioButton ID="Rdb_Columna_C" runat="server" GroupName="Grp_Rbd_Constru_Dominante"
                                                                        name="<%=Rdb_Columna_C.ClientID %>" Width="98%" TabIndex="15"></asp:RadioButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="GridSelected" />
                                                        <PagerStyle CssClass="GridHeader" />
                                                        <HeaderStyle CssClass="tblHead" />
                                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #3366CC">
                                                <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; font-size: 15px; color: #FFFFFF; background-color: #555555;">
                                                    Vías de Acceso
                                                </td>
                                                <td style="text-align: right; width: 30%;">
                                                    <asp:RadioButton ID="Rdb_Buenas" runat="server" GroupName="Rdb_Vias_Acceso" Text="Buenas"
                                                        TabIndex="16" />
                                                </td>
                                                <td style="text-align: right; width: 20%;">
                                                    <asp:RadioButton ID="Rdb_Regulares" runat="server" GroupName="Rdb_Vias_Acceso" Text="Regulares"
                                                        TabIndex="17" />
                                                </td>
                                                <td style="text-align: right; width: 30%;">
                                                    <asp:RadioButton ID="Rdb_Malas" runat="server" GroupName="Rdb_Vias_Acceso" Text="Malas"
                                                        TabIndex="18" />
                                                </td>
                                            </tr>
                                            <tr style="background-color: #3366CC">
                                                <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; font-size: 15px; color: #FFFFFF; background-color: #555555;">
                                                    Topografía
                                                </td>
                                                <td style="text-align: right; width: 30%;">
                                                    <asp:RadioButton ID="Rdb_Plana" runat="server" GroupName="Rdb_Foto" Text="Plana"
                                                        TabIndex="19" />
                                                </td>
                                                <td style="text-align: right; width: 20%;">
                                                    <asp:RadioButton ID="Rdb_Pendiente" runat="server" GroupName="Rdb_Foto" Text="Pendiente"
                                                        TabIndex="20" />
                                                </td>
                                            </tr>
                                            <tr style="background-color: #3366CC">
                                                <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; font-size: 15px; color: #FFFFFF; background-color: #555555;">
                                                    Dens. de Construcción
                                                </td>
                                                <td style="text-align: right; width: 30%;">
                                                    <asp:TextBox ID="Txt_Dens_Construccion" runat="server" MaxLength="6" Style="text-transform: uppercase"
                                                        Width="98.6%" TabIndex="21" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Dens_Construccion" runat="server" Enabled="True"
                                                        FilterType="Custom, Numbers" TargetControlID="Txt_Dens_Construccion" ValidChars="0123456789.,">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: 20%;">
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                </td>
                                                <td style="text-align: left; width: 20%; text-align: right;">
                                                    <%--*Confirmar Contraseña--%>
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                </td>
                                            </tr>
                                            <tr style="background-color: #3366CC">
                                                <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                    CONSTRUCCIÓN
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; width: 20%;">
                                                    <asp:RadioButton ID="Rdb_Nueva" runat="server" GroupName="Rdb_Tipo_Construccion"
                                                        Text="Nueva" TabIndex="22" />
                                                </td>
                                                <td style="text-align: right; width: 30%;">
                                                    <asp:RadioButton ID="Rdb_Ampliacion" runat="server" GroupName="Rdb_Tipo_Construccion"
                                                        Text="Ampliación" TabIndex="23" />
                                                </td>
                                                <td style="text-align: right; width: 20%;">
                                                    <asp:RadioButton ID="Rdb_Remodelacion" runat="server" GroupName="Rdb_Tipo_Construccion"
                                                        Text="Remodelación" TabIndex="24" />
                                                </td>
                                                <td style="text-align: right; width: 30%;">
                                                    <asp:RadioButton ID="Rdb_Rentada" runat="server" GroupName="Rdb_Tipo_Construccion"
                                                        Text="Rentada" TabIndex="25" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; width: 20%;">
                                                    <asp:RadioButton ID="Rdb_Misma" runat="server" GroupName="Rdb_Tipo_Construccion"
                                                        Text="Misma Construcción" TabIndex="33" />
                                                </td>
                                                <td style="text-align: right; width: 30%;">
                                                </td>
                                                <td style="text-align: right; width: 20%;">
                                                </td>
                                                <td style="text-align: right; width: 30%;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; width: 20%;">
                                                    <asp:Label ID="Lbl_Calidad_Proy" runat="server" Text="Calidad del Proyecto" />
                                                </td>
                                                <td style="text-align: right; width: 30%;">
                                                    <asp:RadioButton ID="Rdb_Calidad_Buena" runat="server" GroupName="Rdb_Calidad_Proy"
                                                        Text="Buena" TabIndex="26" />
                                                </td>
                                                <td style="text-align: right; width: 20%;">
                                                    <asp:RadioButton ID="Rdb_Calidad_Mala" runat="server" GroupName="Rdb_Calidad_Proy"
                                                        Text="Mala" TabIndex="27" />
                                                </td>
                                                <td style="text-align: right; width: 30%;">
                                                    <asp:RadioButton ID="Rdb_Calidad_Regular" runat="server" GroupName="Rdb_Calidad_Proy"
                                                        Text="Regular" TabIndex="28" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: 20%;">
                                                    *Uso
                                                </td>
                                                <td style="text-align: right; width: 30%;">
                                                    <asp:DropDownList ID="Cmb_Tipo_Construccion" runat="server" Width="98%" Enabled="false"
                                                        onchange="Validar_Tipo_Construccion();" />
                                                </td>
                                                <td style="text-align: left; width: 20%;">
                                                </td>
                                                <td style="text-align: right; width: 30%;">
                                                    <asp:TextBox ID="Txt_Uso" runat="server" Style="float: left; text-transform: uppercase"
                                                        Width="98%" TabIndex="40" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: 20%;">
                                                    <%--Usuario--%>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #3366CC">
                                                <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                    ELEMENTOS DE CONTRUCCIÓN
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: 20%;">
                                                    Renglones y Columnas
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:TextBox ID="Txt_Renglones_Col" runat="server" MaxLength="2" Style="text-align: right"
                                                        Enabled="false" OnTextChanged="Txt_Renglones_Col_TextChanged" AutoPostBack="true"
                                                        TabIndex="30" />
                                                    <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Renglones_Col" TargetControlID="Txt_Renglones_Col"
                                                        FilterType="Numbers" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" style="text-align: center;">
                                                    <asp:GridView ID="Grid_Elementos_Construccion" runat="server" AllowPaging="False"
                                                        AllowSorting="True" AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                        HeaderStyle-CssClass="tblHead" OnDataBound="Grid_Elementos_Construccion_DataBound"
                                                        PageSize="18" Style="white-space: normal;" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="ELEMENTOS_CONSTRUCCION_ID" HeaderStyle-Width="5%" HeaderText="Id"
                                                                Visible="false">
                                                                <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                                <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ELEMENTOS_CONSTRUCCION" HeaderStyle-Width="10%" HeaderText="Referencia">
                                                                <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                                <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="A">
                                                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="Txt_A" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                        name="<%=Txt_A.ClientID %>" Style="text-transform: uppercase" Width="90%" TabIndex="30"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="B">
                                                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="Txt_B" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                        name="<%=Txt_B.ClientID %>" Style="text-transform: uppercase" Width="90%" TabIndex="31"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="C">
                                                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="Txt_C" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                        name="<%=Txt_C.ClientID %>" Style="text-transform: uppercase" Width="90%" TabIndex="32"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="D">
                                                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="Txt_D" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                        name="<%=Txt_D.ClientID %>" Style="text-transform: uppercase" Width="90%" TabIndex="33"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="E">
                                                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="Txt_E" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                        name="<%=Txt_E.ClientID %>" Style="text-transform: uppercase" Width="90%" TabIndex="34"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="F">
                                                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="Txt_F" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                        name="<%=Txt_F.ClientID %>" Style="text-transform: uppercase" Width="90%" TabIndex="35"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="G">
                                                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="Txt_G" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                        name="<%=Txt_G.ClientID %>" Style="text-transform: uppercase" Width="90%" TabIndex="36"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="H">
                                                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="Txt_H" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                        name="<%=Txt_H.ClientID %>" Style="text-transform: uppercase" Width="90%" TabIndex="37"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="I">
                                                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="Txt_I" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                        name="<%=Txt_I.ClientID %>" Style="text-transform: uppercase" Width="90%" TabIndex="38"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="J">
                                                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="Txt_J" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                        name="<%=Txt_J.ClientID %>" Style="text-transform: uppercase" Width="90%" TabIndex="39"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="K">
                                                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="Txt_K" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                        name="<%=Txt_K.ClientID %>" Style="text-transform: uppercase" Width="90%" TabIndex="40"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="L">
                                                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="Txt_L" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                        name="<%=Txt_L.ClientID %>" Style="text-transform: uppercase" Width="90%" TabIndex="41"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="M">
                                                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="Txt_M" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                        name="<%=Txt_M.ClientID %>" Style="text-transform: uppercase" Width="90%" TabIndex="42"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="N">
                                                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="Txt_N" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                        name="<%=Txt_N.ClientID %>" Style="text-transform: uppercase" Width="90%" TabIndex="43"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="O">
                                                                <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="Txt_O" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                        name="<%=Txt_O.ClientID %>" Style="text-transform: uppercase" Width="90%" TabIndex="44"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="GridSelected" />
                                                        <PagerStyle CssClass="GridHeader" />
                                                        <HeaderStyle CssClass="tblHead" />
                                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                </td>
                                            </tr>
                                        </div>
                                        <tr style="background-color: #3366CC">
                                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                OBSERVACIONES
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                                Observaciones
                                            </td>
                                            <td colspan="3" style="width: 98%; text-align: left;">
                                                <asp:TextBox ID="Txt_Observaciones" runat="server" Rows="3" Style="float: left; text-transform: uppercase"
                                                    TextMode="MultiLine" Width="96.4%" TabIndex="45" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr style="background-color: #3366CC">
                                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                CALCULOS DEL VALOR DEL TERRENO
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:GridView ID="Grid_Calculos" runat="server" AllowPaging="False" AllowSorting="True"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                    HeaderStyle-CssClass="tblHead" OnDataBound="Grid_Calculos_DataBound" OnRowCommand="Grid_Calculos_RowCommand"
                                                    PageSize="15" Style="white-space: normal;" Width="100%">
                                                    <Columns>
                                                        <asp:BoundField DataField="SECCION" HeaderStyle-Width="5%" HeaderText="Sección">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Superficie M2">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Superficie_M2" runat="server" AutoPostBack="true" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Txt_Superficie_M2.ClientID %>" OnTextChanged="Txt_Superficie_M2_Cal_TextChanged"
                                                                    Width="80%" TabIndex="46"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Superficie_M2" runat="server" Enabled="True"
                                                                    FilterType="Custom, Numbers" TargetControlID="Txt_Superficie_M2" ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor M2">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Valor_M2" runat="server" CssClass="text_cantidades_grid" Enabled="false"
                                                                    Font-Size="X-Small" name="<%=Txt_Valor_M2.ClientID %>" Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Valor_M2" runat="server" Enabled="True"
                                                                    FilterType="Custom, Numbers" TargetControlID="Txt_Valor_M2" ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor Tramo Id">
                                                            <HeaderStyle HorizontalAlign="right" Width="0%" />
                                                            <ItemStyle HorizontalAlign="right" Width="0%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Tramo_Id" runat="server" Font-Size="X-Small" name="<%=Txt_Tramo_Id.ClientID %>"
                                                                    Width="0%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Width="5%" HeaderText="">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="Btn_Valor_Tramo" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                    CommandName="Cmd_Valor_M2" Height="22px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                                                    TabIndex="46" ToolTip="Seleccionar Valor M2" Width="22px" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Factor">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Factor" runat="server" AutoPostBack="true" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Txt_Factor.ClientID %>" OnTextChanged="Txt_Factor_Cal_TextChanged"
                                                                    Width="80%" TabIndex="46"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Factor" runat="server" Enabled="True" FilterType="Custom, Numbers"
                                                                    TargetControlID="Txt_Factor" ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="F. de Ef.">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Factor_Ef" runat="server" AutoPostBack="true" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Txt_Factor_Ef.ClientID %>" OnTextChanged="Txt_Factor_Ef_Cal_TextChanged"
                                                                    Width="80%" TabIndex="46"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Factor_Ef" runat="server" Enabled="True"
                                                                    FilterType="Custom, Numbers" TargetControlID="Txt_Factor_Ef" ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="">
                                                            <HeaderStyle HorizontalAlign="right" Width="30%" />
                                                            <ItemStyle HorizontalAlign="right" Width="30%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Total" runat="server" CssClass="text_cantidades_grid" Enabled="false"
                                                                    Font-Size="X-Small" name="<%=Txt_Total.ClientID %>" Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Importe_Grid" runat="server" Enabled="True"
                                                                    FilterType="Custom, Numbers" TargetControlID="Txt_Total" ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <HeaderStyle CssClass="tblHead" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                Superficie Total
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Terreno_Superficie_Total" runat="server" Enabled="false" Style="text-transform: uppercase"
                                                    Width="98%"></asp:TextBox>
                                                <asp:HiddenField ID="Hdf_Valor_Terreno" runat="server" Value="0.00" />
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: left;">
                                                Valor Total
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Terreno_Valor_Total" runat="server" Enabled="false" Style="text-transform: uppercase"
                                                    Width="96.4%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr style="background-color: #3366CC">
                                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                CALCULOS DEL VALOR DE LA CONSTRUCCION
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:GridView ID="Grid_Valores_Construccion" runat="server" AllowPaging="False" AllowSorting="True"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                    HeaderStyle-CssClass="tblHead" OnDataBound="Grid_Valores_Construccion_DataBound"
                                                    PageSize="15" Style="white-space: normal;" Width="100%">
                                                    <Columns>
                                                        <asp:BoundField DataField="REFERENCIA" HeaderStyle-Width="5%" HeaderText="Ref.">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Tipo">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Tipo" runat="server" AutoPostBack="true" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Txt_Tipo.ClientID %>" OnTextChanged="Txt_Tipo_Constru_TextChanged"
                                                                    Width="80%" TabIndex="47"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Tipo" runat="server" Enabled="True" FilterType="Numbers"
                                                                    TargetControlID="Txt_Tipo">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Con Serv">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Con_Serv" runat="server" AutoPostBack="true" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Txt_Con_Serv.ClientID %>" TabIndex="47" OnTextChanged="Txt_Con_Serv_Constru_TextChanged"
                                                                    Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Con_Serv" runat="server" Enabled="True"
                                                                    FilterType="Numbers" TargetControlID="Txt_Con_Serv">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Superficie M2">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Superficie_M2" runat="server" AutoPostBack="true" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Txt_Superficie_M2.ClientID %>" TabIndex="47" OnTextChanged="Txt_Superficie_M2_Constru_TextChanged"
                                                                    Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Superficie_M2" runat="server" Enabled="True"
                                                                    FilterType="Custom, Numbers" TargetControlID="Txt_Superficie_M2" ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor X M2">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Valor_X_M2" runat="server" CssClass="text_cantidades_grid" Enabled="false"
                                                                    Font-Size="X-Small" name="<%=Txt_Valor_X_M2.ClientID %>" Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Valor_X_M2" runat="server" Enabled="True"
                                                                    FilterType="Custom, Numbers" TargetControlID="Txt_Valor_X_M2" ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor Construccion Id" Visible="false">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Valor_Construccion_Id" runat="server" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Txt_Valor_Construccion_Id.ClientID %>" Width="80%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Factor">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Factor" runat="server" AutoPostBack="true" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" TabIndex="47" name="<%=Txt_Factor.ClientID %>" OnTextChanged="Txt_Factor_Constru_TextChanged"
                                                                    Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Importe_Grid" runat="server" Enabled="True"
                                                                    FilterType="Custom, Numbers" TargetControlID="Txt_Factor" ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor Parcial">
                                                            <HeaderStyle HorizontalAlign="right" Width="30%" />
                                                            <ItemStyle HorizontalAlign="right" Width="30%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Total" runat="server" CssClass="text_cantidades_grid" Enabled="false"
                                                                    Font-Size="X-Small" name="<%=Txt_Total.ClientID %>" Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Total" runat="server" Enabled="True" FilterType="Custom, Numbers"
                                                                    TargetControlID="Txt_Total" ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <HeaderStyle CssClass="tblHead" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                Superficie Total
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Construccion_Superficie_Total" runat="server" Enabled="false"
                                                    Style="text-transform: uppercase" TabIndex="9" Width="98%"></asp:TextBox>
                                                <asp:HiddenField ID="Hdf_Valor_Construccion" runat="server" Value="0.00" />
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: left;">
                                                Valor Total
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Construccion_Valor_Total" runat="server" Enabled="false" Style="text-transform: uppercase"
                                                    TabIndex="11" Width="98%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr style="background-color: #3366CC">
                                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                VALOR TOTAL DEL PREDIO ACTUAL
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                Valor fiscal actual
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Valor_Total_Predio" runat="server" Enabled="false" Style="text-transform: uppercase"
                                                    TabIndex="9" Width="98%"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left; width: 20%;">
                                                Cuota anual actual
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Cuota_Anual_Actual" runat="server" Enabled="false" Style="text-transform: uppercase"
                                                    TabIndex="9" Width="98%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="background-color: #3366CC">
                                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                VALOR TOTAL ANTERIOR DEL PREDIO
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                Valor fiscal Anterior
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Valor_Total_Predio_Anterior" runat="server" Enabled="false"
                                                    Style="text-transform: uppercase" TabIndex="9" Width="98%"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: left;">
                                                Cuota Anual Anterior
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Cuota_Anual_Anterior" runat="server" Enabled="false" Style="text-transform: uppercase"
                                                    TabIndex="11" Width="98%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                Diferencia Anual
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Diferencia_Anual" runat="server" Enabled="false" Style="text-transform: uppercase"
                                                    TabIndex="11" Width="98%" />
                                            </td>
                                        </tr>
                                        <%--<tr style="background-color: #3366CC">
                                    <td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                        VIII. MEDIDAS Y COLINDANCIAS: SEGUN PLANO DE LOTIFICACION
                                    </td>
                                </tr>
                                <tr id="Tr1" runat="server">
                                            <td style="text-align: left; width: 20%" class="style1">
                                                Medidas y Colindancias
                                            </td>
                                            <td colspan="3" style="width: 80%">
                                                <asp:TextBox ID="Txt_Medida_Colindancia" Style="text-transform: uppercase" runat="server"
                                                    TextMode="MultiLine" Rows="3" Width="98%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%">
                                            </td>
                                            <td style="width: 30%">
                                            </td>
                                            <td style="width: 20%">
                                            </td>
                                            <td style="width: 30%">
                                                <asp:ImageButton ID="Btn_Agregar_Med_Col" runat="server" Height="20px" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                                    OnClick="Btn_Agregar_Med_Col_Click" ToolTip="Agregar Medidas y Colindancias"
                                                    Width="20px" TabIndex="17" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:GridView ID="Grid_Colindancias" runat="server" AllowPaging="True" AllowSorting="True"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                    HeaderStyle-CssClass="tblHead" PageSize="15" Style="white-space: normal;" Width="100%"
                                                    OnSelectedIndexChanged="Grid_Colindancias_SelectedIndexChanged" OnPageIndexChanging="Grid_Colindancias_PageIndexChanging">
                                                    <Columns>
                                                        <asp:BoundField DataField="NO_COLINDANCIA" HeaderStyle-Width="15%" HeaderText="ID"
                                                            Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="MEDIDA_COLINDANCIA" HeaderStyle-Width="96.5%" HeaderText="MEDIDAS Y COLINDANCIAS">
                                                            <HeaderStyle HorizontalAlign="Left" Width="70%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="70%" />
                                                        </asp:BoundField>
                                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/paginas/delete.png">
                                                            <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                        </asp:ButtonField>
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <HeaderStyle CssClass="tblHead" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>
                                                <br />
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <table id="Tab_Ocultar" runat="server" style="display: none" width="100%">
                                    <tr style="background-color: #3366CC">
                                        <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                            ARCHIVOS DEL AVALÚO
                                        </td>
                                    </tr>
                                    <tr id="Tr_Fila_Fotografias_Bien" runat="server">
                                        <td style="text-align: left; width: 20%" class="style1">
                                            Nombre del Documento
                                        </td>
                                        <td style="text-align: left; width: 30%" class="style1">
                                            <asp:DropDownList ID="Cmb_Documento" runat="server" Width="98%" TabIndex="48">
                                                <asp:ListItem Text="ARCHIVO AUTOCAD" Value="ARCHIVO" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: left; width: 20%" class="style2">
                                            Documento
                                        </td>
                                        <td style="width: 30%" class="style2">
                                            <%--<asp:FileUpload ID="Fup_Documento" runat="server" Width="98%" TabIndex="49" />--%>
                                            <cc1:AsyncFileUpload ID="Fup_Documento" runat="server" Width="270px" PersistFile="true"
                                                UploadingBackColor="Yellow" ErrorBackColor="Red" CompleteBackColor="LightGreen" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%">
                                        </td>
                                        <td style="width: 30%">
                                        </td>
                                        <td style="width: 20%">
                                        </td>
                                        <td style="width: 30%">
                                            <asp:ImageButton ID="Btn_Agregar_Documento" runat="server" Height="20px" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                                OnClick="Btn_Agregar_Documento_Click" ToolTip="Agregar Documento" Width="20px"
                                                TabIndex="50" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="text-align: left; width: 20%;">
                                            <asp:GridView ID="Grid_Documentos" runat="server" AllowPaging="False" AllowSorting="True"
                                                AutoGenerateColumns="False" HeaderStyle-CssClass="tblHead" OnSelectedIndexChanged="Grid_Documentos_SelectedIndexChanged"
                                                PageSize="20" OnDataBound="Grid_Documentos_DataBound" Style="white-space: normal;"
                                                Width="100%">
                                                <Columns>
                                                    <asp:BoundField DataField="NO_DOCUMENTO" HeaderText="No documento" Visible="false">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ANIO_DOCUMENTO" HeaderText="Anio Documento" Visible="false">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DOCUMENTO" HeaderText="Nombre Documento">
                                                        <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="50%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="Btn_Eliminar_Fotos" runat="server" CommandName="Select" Height="20px"
                                                                ImageUrl="~/paginas/imagenes/paginas/delete.png" ToolTip="Eliminar" Width="20px" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="2%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="tblHead" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr style="background-color: #3366CC">
                                        <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                            COMENTARIOS PARA EL REVISOR
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; width: 20%;" class="style1">
                                            Comentarios
                                        </td>
                                        <td colspan="3" style="width: 80%;">
                                            <asp:TextBox ID="Txt_Comentarios_Perito" runat="server" Rows="3" Style="text-transform: uppercase"
                                                TextMode="MultiLine" Width="98%" TabIndex="51"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <div id="Div_Observaciones" runat="server" visible="false">
                                    <table border="0" cellspacing="0" class="estilo_fuente" width="98%">
                                        <tr style="background-color: #3366CC">
                                            <td colspan="3" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                Observaciones: Motivos del rechazo
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:GridView ID="Grid_Observaciones" runat="server" AllowPaging="True" AllowSorting="True"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                    HeaderStyle-CssClass="tblHead" OnPageIndexChanging="Grid_Observaciones_PageIndexChanging"
                                                    PageSize="10" Style="white-space: normal;" Width="100%">
                                                    <Columns>
                                                        <asp:BoundField DataField="NO_SEGUIMIENTO" HeaderStyle-Width="5%" HeaderText="no seguimiento"
                                                            Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="MOTIVO_ID" HeaderStyle-Width="5%" HeaderText="Motivo Id"
                                                            Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="MOTIVO_DESCRIPCION" HeaderStyle-Width="93%" HeaderText="Motivo de Rechazo">
                                                            <HeaderStyle HorizontalAlign="Left" Width="93%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="93%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ESTATUS" HeaderStyle-Width="7%" HeaderText="Estatus">
                                                            <HeaderStyle HorizontalAlign="Left" Width="7%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <HeaderStyle CssClass="tblHead" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%">
                                                Observaciones
                                            </td>
                                            <td colspan="2" style="width: 80%">
                                                <asp:TextBox ID="Txt_Observaciones_Rechazo" runat="server" TextMode="MultiLine" Rows="3"
                                                    Width="98%" TabIndex="51"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="background-color: #3366CC">
                                            <td colspan="2" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                FIRMANTE:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="Cmb_Firmante" runat="server" Width="50%">
                                                    <asp:ListItem Selected="False" Text="SELECCIONE FIRMANTE"></asp:ListItem>
                                                    <asp:ListItem Text="FIRMATE_1" Value="Firmante_1"></asp:ListItem>
                                                    <asp:ListItem Text="FIRMATE_2" Value="Firmante_2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

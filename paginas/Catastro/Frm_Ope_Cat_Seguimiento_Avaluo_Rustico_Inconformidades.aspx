<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Cat_Seguimiento_Avaluo_Rustico_Inconformidades.aspx.cs" Inherits="paginas_Catastro_Frm_Ope_Cat_Seguimiento_Avaluo_Rustico_Inconformidades" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Title=" Seguimiento Avalúo Rústico Inconformidades" %>

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

        function ValidarCaracteres(textareaControl, maxlength) { if (textareaControl.value.length > maxlength) { textareaControl.value = textareaControl.value.substring(0, maxlength); alert("Debe ingresar hasta un máximo de " + maxlength + " caracteres"); } }
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
                        <td colspan="4" class="label_titulo">
                            Seguimiento Avalúos Rústico Inconformidades</td>
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
                        <td align="left" colspan="3">
                            <asp:ImageButton ID="Btn_Modificar" runat="server" TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Modificar" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Imprimir" runat="server" TabIndex="3" ImageUrl="~/paginas/imagenes/gridview/grid_print.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Imprimir" OnClick="Btn_Imprimir_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click" />
                        </td>
                        <td align="right">
                            Búsqueda:
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="130px" TabIndex="5" Style="text-transform: uppercase"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar" runat="server" CausesValidation="false" TabIndex="6"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" OnClick="Btn_Buscar_Click" />
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
                                        <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                            Avalúos Rústicos
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <tr align="center">
                                                <td colspan="4">
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
                                                            <asp:BoundField DataField="VECES_RECHAZO" HeaderText="Veces Rechazado">
                                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FECHA_CREO" HeaderText="Fecha de Recepción" DataFormatString="{0:dd-MMM-yyyy}">
                                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
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
                                        </td>
                                    </tr>
                                </div>
                                <div id="Div_Datos_Avaluo" runat="server" visible="false">
                                    <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                                        <tr style="background-color: #3366CC">
                                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                                I. DATOS GENERALES
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
                                                <asp:TextBox ID="Txt_No_Avaluo" runat="server" Width="96.4%" TabIndex="11" Enabled="false"
                                                    Style="text-transform: uppercase" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                *Cuenta Predial
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="98%" TabIndex="8" Enabled="false"
                                                    Style="text-transform: uppercase" OnTextChanged="Txt_Cuenta_Predial_TextChanged"
                                                    AutoPostBack="true"></asp:TextBox>
                                                <asp:HiddenField ID="Hdf_Cuenta_Predial_Id" runat="server" />
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: Left;">
                                                Clave Catastral
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Clave_Catastral" runat="server" Width="98%" TabIndex="9" Enabled="false"
                                                    Style="text-transform: uppercase" MaxLength="20" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                *Propietario
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Propietario" runat="server" Width="98%" TabIndex="16" Enabled="false"
                                                    Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                            <td style="text-align: Left; width: 20%;">
                                                *Estatus
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" TabIndex="17">
                                                    <asp:ListItem Text="POR VALIDAR" Value="POR VALIDAR" />
                                                    <asp:ListItem Text="RECHAZADO" Value="RECHAZADO" />
                                                    <asp:ListItem Text="AUTORIZADO" Value="AUTORIZADO" />
                                                    <asp:ListItem Text="POR PAGAR" Value="POR PAGAR" />
                                                    <asp:ListItem Text="PAGADO" Value="PAGADO" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                *Domicilio para Notificar
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Domicilio_Not" runat="server" Width="98%" TabIndex="18" Enabled="false"
                                                    Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: Left;">
                                                *Municipio
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Municipio_Notificar" runat="server" Width="96.4%" TabIndex="19"
                                                    Enabled="false" Style="text-transform: uppercase" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                *Ubicación
                                            </td>
                                            <td style="text-align: left; width: 30%;" colspan="3">
                                                <asp:TextBox ID="Txt_Ubicacion_Predio" runat="server" Width="98%" TabIndex="12" Enabled="false"
                                                    Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                *Localidad y Municipio
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Localidad" runat="server" Width="98%" TabIndex="14" Enabled="false"
                                                    Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: Left;">
                                                *Fecha
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Fecha" runat="server" Width="96.4%" TabIndex="15" Enabled="false"
                                                    Style="text-transform: uppercase" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                *Solicitante
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Solicitante" runat="server" Width="98%" TabIndex="22" Enabled="false"
                                                    Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left; width: 20%;">
                                                *Nombre del Predio
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Nombre_Predio" runat="server" Width="98%" TabIndex="22" Enabled="false"
                                                    Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr style="background-color: #3366CC">
                                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                                Coordenadas X
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                Grados (°)
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_X_Horas" runat="server" Width="96.4%" Style="text-transform: uppercase"
                                                    MaxLength="3" TabIndex="7" />
                                                <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_X_Horas" FilterType="Numbers" runat="server"
                                                    TargetControlID="Txt_X_Horas">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="text-align: left; width: 20%;">
                                                Minutos (')
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_X_Minutos" runat="server" Width="96.4%" Style="text-transform: uppercase"
                                                    MaxLength="3" TabIndex="8" />
                                                <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_X_Minutos" FilterType="Numbers" runat="server"
                                                    TargetControlID="Txt_X_Minutos">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                Segundos (")
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_X_Segundos" runat="server" Width="96.4%" Style="text-transform: uppercase"
                                                    MaxLength="3" TabIndex="9" />
                                                <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_X_Segundos" FilterType="Numbers" runat="server"
                                                    TargetControlID="Txt_X_Segundos">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="text-align: left; width: 20%;">
                                                Orientación
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:DropDownList ID="Cmb_Latitud" runat="server" Width="96.4%" TabIndex="10" Enabled="false">
                                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE"></asp:ListItem>
                                                    <asp:ListItem Text="ESTE" Value="E"></asp:ListItem>
                                                    <asp:ListItem Text="OESTE" Value="O"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="background-color: #3366CC">
                                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                                Coordenadas Y
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                Grados (°)
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Y_Horas" runat="server" Width="96.4%" Style="text-transform: uppercase"
                                                    MaxLength="3" TabIndex="10" />
                                                <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Y_Horas" FilterType="Numbers" runat="server"
                                                    TargetControlID="Txt_Y_Horas">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="text-align: left; width: 20%;">
                                                Minutos (')
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Y_Minutos" runat="server" Width="96.4%" Style="text-transform: uppercase"
                                                    MaxLength="3" TabIndex="11" />
                                                <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Y_Minutos" FilterType="Numbers" runat="server"
                                                    TargetControlID="Txt_Y_Minutos">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                Segundos (")
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Y_Segundos" runat="server" Width="96.4%" Style="text-transform: uppercase"
                                                    MaxLength="3" TabIndex="12" />
                                                <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Y_Segundos" FilterType="Numbers" runat="server"
                                                    TargetControlID="Txt_Y_Segundos">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="text-align: left; width: 20%;">
                                                Orientación
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:DropDownList ID="Cmb_Longitud" runat="server" Width="96.4%" TabIndex="13" Enabled="false">
                                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE"></asp:ListItem>
                                                    <asp:ListItem Text="NORTE" Value="N"></asp:ListItem>
                                                    <asp:ListItem Text="SUR" Value="S"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="background-color: #3366CC">
                                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                V. OBSERVACIONES
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                                *Observaciones
                                            </td>
                                            <td colspan="3" style="width: 98%; text-align: left;">
                                                <asp:TextBox ID="Txt_Observaciones" runat="server" Rows="3" Style="float: left; text-transform: uppercase"
                                                    TextMode="MultiLine" Width="96.4%" TabIndex="36" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr style="background-color: #3366CC">
                                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                III. CALCULOS DEL VALOR DEL TERRENO
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:GridView ID="Grid_Calculos" runat="server" AllowPaging="False" AllowSorting="True"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                    HeaderStyle-CssClass="tblHead" OnDataBound="Grid_Calculos_DataBound" OnRowCommand="Grid_Calculos_RowCommand"
                                                    PageSize="15" Style="white-space: normal;" Width="100%">
                                                    <Columns>
                                                        <asp:BoundField DataField="TIPO_CONSTRU_RUSTICO_ID" HeaderStyle-Width="5%" HeaderText="Id_Construccion"
                                                            Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="IDENTIFICADOR" HeaderStyle-Width="5%" HeaderText="CLASIFICACION">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="SUPERFICIE EN Ha.">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Superficie_M2" runat="server" AutoPostBack="true" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Txt_Superficie_M2.ClientID %>" OnTextChanged="Txt_Superficie_M2_Cal_TextChanged"
                                                                    Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Superficie_M2" runat="server" Enabled="True"
                                                                    FilterType="Custom, Numbers" TargetControlID="Txt_Superficie_M2" ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="VALOR_CONSTRU_RUSTICO_ID" HeaderStyle-Width="5%" HeaderText="Id_Valor"
                                                            Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="VALOR POR Ha.">
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
                                                        <asp:TemplateField HeaderText="Factor">
                                                            <HeaderStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemStyle HorizontalAlign="right" Width="15%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Factor" runat="server" AutoPostBack="true" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Txt_Factor.ClientID %>" OnTextChanged="Txt_Factor_Cal_TextChanged"
                                                                    Width="80%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Factor" runat="server" Enabled="True" FilterType="Custom, Numbers"
                                                                    TargetControlID="Txt_Factor" ValidChars="0123456789.,">
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
                                                    TabIndex="9" Width="98%"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: left;">
                                                Valor Total
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Terreno_Valor_Total" runat="server" Enabled="false" Style="text-transform: uppercase"
                                                    TabIndex="11" Width="96.4%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr style="background-color: #3366CC">
                                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                IV. CALCULOS DEL VALOR DE LA CONSTRUCCION
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:GridView ID="Grid_Valores_Construccion" runat="server" AllowPaging="False" AllowSorting="True"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                    HeaderStyle-CssClass="tblHead" OnDataBound="Grid_Valores_Construccion_DataBound"
                                                    PageSize="15" Style="white-space: normal;" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="CROQUIS">
                                                            <HeaderStyle HorizontalAlign="right" Width="10%" />
                                                            <ItemStyle HorizontalAlign="right" Width="10%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Croquis" runat="server" Style="text-transform: uppercase" Font-Size="X-Small"
                                                                    name="<%=Txt_Croquis.ClientID %>" Width="98%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="TIPO">
                                                            <HeaderStyle HorizontalAlign="right" Width="5%" />
                                                            <ItemStyle HorizontalAlign="right" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Tipo" runat="server" AutoPostBack="true" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Txt_Tipo.ClientID %>" OnTextChanged="Txt_Tipo_Constru_TextChanged"
                                                                    Width="98%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Tipo" runat="server" Enabled="True" FilterType="Numbers"
                                                                    TargetControlID="Txt_Tipo">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="EDO.">
                                                            <HeaderStyle HorizontalAlign="right" Width="5%" />
                                                            <ItemStyle HorizontalAlign="right" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Con_Serv" runat="server" AutoPostBack="true" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Txt_Con_Serv.ClientID %>" OnTextChanged="Txt_Con_Serv_Constru_TextChanged"
                                                                    Width="98%"></asp:TextBox>
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
                                                                    Font-Size="X-Small" name="<%=Txt_Superficie_M2.ClientID %>" OnTextChanged="Txt_Superficie_M2_Constru_TextChanged"
                                                                    Width="98%"></asp:TextBox>
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
                                                                    Font-Size="X-Small" name="<%=Txt_Valor_X_M2.ClientID %>" Width="98%"></asp:TextBox>
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
                                                        <asp:TemplateField HeaderText="Valor Parcial">
                                                            <HeaderStyle HorizontalAlign="right" Width="20%" />
                                                            <ItemStyle HorizontalAlign="right" Width="20%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Total" runat="server" CssClass="text_cantidades_grid" Enabled="false"
                                                                    Font-Size="X-Small" name="<%=Txt_Total.ClientID %>" Width="98%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Total" runat="server" Enabled="True" FilterType="Custom, Numbers"
                                                                    TargetControlID="Txt_Total" ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="EDAD">
                                                            <HeaderStyle HorizontalAlign="right" Width="10%" />
                                                            <ItemStyle HorizontalAlign="right" Width="10%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Edad" runat="server" AutoPostBack="true" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Txt_Edad.ClientID %>" OnTextChanged="Txt_Edad_TextChanged"
                                                                    Width="98%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Edad_Grid" runat="server" Enabled="True"
                                                                    FilterType="Custom, Numbers" TargetControlID="Txt_Edad" ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Factor">
                                                            <HeaderStyle HorizontalAlign="right" Width="10%" />
                                                            <ItemStyle HorizontalAlign="right" Width="10%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Factor" runat="server" AutoPostBack="true" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Txt_Factor.ClientID %>" OnTextChanged="Txt_Factor_Constru_TextChanged"
                                                                    Width="98%"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Importe_Grid" runat="server" Enabled="True"
                                                                    FilterType="Custom, Numbers" TargetControlID="Txt_Factor" ValidChars="0123456789.,">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="USO">
                                                            <HeaderStyle HorizontalAlign="right" Width="10%" />
                                                            <ItemStyle HorizontalAlign="right" Width="10%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Uso" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                    Style="text-transform: uppercase" name="<%=Txt_Uso.ClientID %>" Width="98%"></asp:TextBox>
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
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: left;">
                                                Valor Total
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Construccion_Valor_Total" runat="server" Enabled="false" Style="text-transform: uppercase"
                                                    TabIndex="11" Width="96.4%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr style="background-color: #3366CC">
                                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                VIII. VALOR TOTAL DEL PREDIO
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td style="text-align: left; width: 20%;">
                                                Base Gravable
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Base_Gravable" runat="server" Enabled="false" Style="text-transform: uppercase"
                                                    Width="98%"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left; width: 20%;">
                                                Impuesto Bimestral
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Impuesto_Bimestral" runat="server" Enabled="false" OnTextChanged="Txt_Impuesto_Bimestral_TextChanged"
                                                    AutoPostBack="true" MaxLength="15" Style="text-transform: uppercase" Width="98%"
                                                    TabIndex="37"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Inpa" TargetControlID="Txt_Impuesto_Bimestral"
                                                    FilterType="Custom, Numbers" ValidChars="1234567890.," runat="server" />
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                Valor Total del Predio
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Valor_Total_Predio" runat="server" Enabled="false" MaxLength="15"
                                                    Style="text-transform: uppercase" TabIndex="38" Width="98%"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Inpr" TargetControlID="Txt_Valor_Total_Predio"
                                                    FilterType="Custom, Numbers" ValidChars="1234567890.," runat="server" />
                                            </td>
                                        </tr>
                                        <tr style="background-color: #3366CC">
                                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                IV. CALCULOS DEL VALOR DE LA CONSTRUCCION
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:GridView ID="Grid_Descripcion_Terreno" runat="server" AllowPaging="False" AllowSorting="True"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                    HeaderStyle-CssClass="tblHead" OnDataBound="Grid_Descripcion_Terreno_DataBound"
                                                    PageSize="15" Style="white-space: normal;" Width="100%">
                                                    <Columns>
                                                        <asp:BoundField DataField="DESC_CONSTRU_RUSTICO_ID" HeaderStyle-Width="10%" HeaderText="Id"
                                                            Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="IDENTIFICADOR" HeaderStyle-Width="20%">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DESCRIPCION_RUSTICO_ID" HeaderStyle-Width="10%" HeaderText="Id_Detalles"
                                                            Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="VALOR_INDICE" HeaderStyle-Width="10%">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="INDICADOR_A" HeaderStyle-Width="10%">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="right" Width="5%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Indicador_Valor_A" runat="server" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Chk_Indicador_Valor_A.ClientID %>"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="INDICADOR_B" HeaderStyle-Width="10%">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="right" Width="5%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Indicador_Valor_B" runat="server" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Chk_Indicador_Valor_B.ClientID %>"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="INDICADOR_C" HeaderStyle-Width="10%">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="right" Width="5%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Indicador_Valor_C" runat="server" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Chk_Indicador_Valor_C.ClientID %>"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="INDICADOR_D" HeaderStyle-Width="10%">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Indicador_Valor_D" runat="server" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Chk_Indicador_Valor_D.ClientID %>"></asp:CheckBox>
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
                                                VIII. MEDIDAS Y COLINDANCIAS: SEGUN PLANO DE LOTIFICACION
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%; text-align: left">
                                                *Al Norte
                                            </td>
                                            <td style="width: 30%; text-align: left">
                                                <asp:TextBox ID="Txt_Norte" runat="server" Style="text-transform: uppercase" Width="98%"
                                                    TabIndex="39" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td style="width: 20%; text-align: left">
                                                *Al Sur
                                            </td>
                                            <td style="width: 30%; text-align: left">
                                                <asp:TextBox ID="Txt_Sur" runat="server" Style="text-transform: uppercase" Width="98%"
                                                    TabIndex="40" MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%; text-align: left">
                                                *Al Oriente
                                            </td>
                                            <td style="width: 30%; text-align: left">
                                                <asp:TextBox ID="Txt_Oriente" runat="server" Style="text-transform: uppercase" Width="98%"
                                                    TabIndex="41" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td style="width: 20%; text-align: left">
                                                *Al Poniente
                                            </td>
                                            <td style="width: 30%; text-align: left">
                                                <asp:TextBox ID="Txt_Poniente" runat="server" Style="text-transform: uppercase" Width="98%"
                                                    TabIndex="42" MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="background-color: #3366CC">
                                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                IMPORTE DEL AVALÚO
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td style="text-align: left; width: 20%">
                                                Importe del Avalúo
                                            </td>
                                            <td style="text-align: left; width: 30%">
                                                <asp:TextBox ID="Txt_Precio_Avaluo" runat="server" Enabled="false" Width="96.4%" />
                                                <asp:HiddenField ID="Hdf_Factor_Cobro1" runat="server" />
                                                <asp:HiddenField ID="Hdf_Factor_Cobro2" runat="server" />
                                                <asp:HiddenField ID="Hdf_Base_Cobro" runat="server" />
                                                <asp:HiddenField ID="Hdf_Porcentaje_Cobro" runat="server" />
                                                <asp:HiddenField ID="Hdf_Cobro_Anterior" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <div id="Div_Observaciones" runat="server" visible="false">
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr style="background-color: #3366CC">
                        <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                            Observaciones: Motivos del rechazo
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Motivos de Rechazo.
                        </td>
                        <td style="text-align: left; width: 70%;" colspan="3">
                            <asp:DropDownList ID="Cmb_Motivos_Rechazo" runat="server" Width="98%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%">
                            <asp:HiddenField ID="Hdf_No_Seguimiento" runat="server" />
                            <asp:HiddenField ID="Hdf_Motivo_Id" runat="server" />
                            <asp:HiddenField ID="Hdf_Estatus" runat="server" />
                        </td>
                        <td style="width: 20%; text-align: right">
                            &nbsp;
                        </td>
                        <td style="width: 30%; text-align: left">
                            <asp:ImageButton ID="Btn_Agregar_Observacion" runat="server" Height="24px" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                OnClick="Btn_Agregar_Observacion_Click" TabIndex="10" ToolTip="Agregar Observación"
                                Width="24px" />
                            &nbsp; &nbsp;
                            <%--<asp:ImageButton ID="Btn_Actualizar_Observacion" runat="server"
                                Height="24px" ImageUrl="~/paginas/imagenes/paginas/Actualizar_Detalle.png" 
                                OnClick="Btn_Actualizar_Observacion_Click"
                                TabIndex="10" ToolTip="Actualizar Observacion" Width="24px" />
                                &nbsp; &nbsp;--%>
                            <asp:ImageButton ID="Btn_Eliminar_Observacion" runat="server" Height="24px" ImageUrl="~/paginas/imagenes/paginas/quitar.png"
                                OnClick="Btn_Eliminar_Observacion_Click" TabIndex="10" ToolTip="Eliminar Observacion"
                                Width="24px" />
                        </td>
                        <td style="width: 20%; text-align: left">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:GridView ID="Grid_Observaciones" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                HeaderStyle-CssClass="tblHead" PageSize="10" Style="white-space: normal;" Width="100%"
                                OnPageIndexChanging="Grid_Observaciones_PageIndexChanging" OnSelectedIndexChanged="Grid_Observaciones_SelectedIndexChanged">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                    </asp:ButtonField>
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
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%">
                            Observaciones
                        </td>
                        <td colspan="3" style="width: 80%">
                            <asp:TextBox ID="Txt_Observaciones_Rechazo" runat="server" TextMode="MultiLine" Rows="3"
                                Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
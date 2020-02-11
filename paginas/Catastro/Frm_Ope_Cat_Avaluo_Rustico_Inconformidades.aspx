<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Cat_Avaluo_Rustico_Inconformidades.aspx.cs"
    Inherits="paginas_Catastro_Frm_Ope_Cat_Avaluo_Rustico_Inconformidades" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    Title="Aval�o R�stico Inconformidades" %>

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
        //El nombre del controlador que mantiene la sesi�n
        var CONTROLADOR = "../../Mantenedor_Session.ashx";
 
        //Ejecuta el script en segundo plano evitando as� que caduque la sesi�n de esta p�gina
        function MantenSesion() {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }
 
        //Temporizador para matener la sesi�n activa
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);
        
    //-->

        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }
        
         function Abrir_Busqueda_Oficios() {
            $find('Busqueda_Oficios').show();
            return false;
        }
        
        function Abrir_Busqueda_Avaluos() {
            $find('Busqueda_Avaluos').show();
            return false;
        }
         function Abrir_Busqueda_Solicitudes() {
        $find('Busqueda_Solicitudes').show();
        return false;
        }
        function ValidarCaracteres(textareaControl, maxlength) { if (textareaControl.value.length > maxlength) { textareaControl.value = textareaControl.value.substring(0, maxlength); alert("Debe ingresar hasta un m�ximo de " + maxlength + " caracteres"); } }
        
        function Validar_Tipo_Construccion()
		{
		    if (document.getElementById("<%=Cmb_Tipo_Construccion.ClientID%>").value == "OTRO")
		    {
		        document.getElementById("<%=Txt_Uso_Constru.ClientID%>").disabled = false;
		    }
		    else
		    {
		        document.getElementById("<%=Txt_Uso_Constru.ClientID%>").disabled = true;
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
                    <%--<div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>--%>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Calles" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">
                            Aval�os R�stico Inconformidades
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
                        <td>
                            <asp:Button ID="Btn_Comodin" runat="server" Style="background-color: Transparent;
                                border-style: none;" OnClientClick="javascript:return false;" />
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left">
                            <asp:ImageButton ID="Btn_Autorizar_Avaluo" runat="server" TabIndex="1" OnClientClick="return confirm('�Est� seguro de que desea autorizar el Avaluo?');"
                                ImageUrl="~/paginas/imagenes/paginas/accept.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Autorizar" OnClick="Bt_Autorizar_Click" ToolTip="Autorizado" Visible="false" />
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Nuevo" OnClick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Modificar" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Imprimir" runat="server" TabIndex="4" ImageUrl="~/paginas/imagenes/gridview/grid_print.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Imprimir" OnClick="Btn_Imprimir_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" TabIndex="5" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click" />
                        </td>
                        <td align="right">
                            B�squeda:
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="130px" TabIndex="6" Style="text-transform: uppercase"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar" runat="server" CausesValidation="false" TabIndex="7"
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
                                        <td style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                            Aval�os R�sticos Inconformidades
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <tr align="center">
                                                <td>
                                                    <asp:GridView ID="Grid_Avaluos_Urbanos_Inconformidades" runat="server" AllowPaging="True"
                                                        AllowSorting="True" AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                        HeaderStyle-CssClass="tblHead" PageSize="15" Style="white-space: normal;" Width="100%"
                                                        OnSelectedIndexChanged="Grid_Avaluos_Urbanos_Inconformidades_SelectedIndexChanged"
                                                        OnPageIndexChanging="Grid_Avaluos_Urbanos_Inconformidades_PageIndexChanging">
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
                                                            <asp:BoundField DataField="FECHA_CREO" HeaderText="Fecha de Recepci�n" DataFormatString="{0:dd-MMM-yyyy}">
                                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FECHA_RECHAZO" HeaderText="Fecha de �ltimo Rechazo" DataFormatString="{0:dd-MMM-yyyy}">
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
                                        
                                        <td style="text-align: left; width: 20%;">
                                                *Clave del Tramite
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Clave_Tramite" runat="server" Width="85%" TabIndex="10" Enabled="false"
                                                    Style="text-transform: uppercase" AutoPostBack="true"></asp:TextBox>
                                                <%--OnTextChanged="Txt_Cuenta_Predial_TextChanged"--%>
                                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Solicitudes" runat="server" Height="24px"
                                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" TabIndex="11" ToolTip="B�squeda Avanzada Solicitudes"
                                                    Width="24px" OnClientClick="javascript:return Abrir_Busqueda_Solicitudes();" />
                                                <asp:HiddenField ID="Hdf_Solicitud_Id" runat="server" />
                                                 </td>
                                           
                                            
                                        </tr>
                                        <tr>
                                             <td style="text-align: left; width: 20%;">
                                                No. Aval�o de A.V.
                                            </td>
                                            <td style="width: 30%; text-align: left;">
                                                <asp:TextBox runat="server" ID="Txt_Avaluo_Av" Width="85%" TabIndex="8" Style="float: left;
                                                    text-transform: uppercase" MaxLength="50" Enabled="false" />
                                                <asp:ImageButton ID="Btn_Busqueda_Avaluos_Av" runat="server" Height="24px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                                    OnClientClick="javascript:return Abrir_Busqueda_Avaluos();" TabIndex="9" ToolTip="B�squeda Avanzada de Aval�os de Actualizaci�n de Valor"
                                                    Width="24px" />
                                                <asp:HiddenField ID="Hdf_No_Avaluo_AV" runat="server" />
                                                <asp:HiddenField ID="Hdf_Anio_Avaluo_AV" runat="server" />
                                            </td>
                                                 <td style="text-align: left; width: 20%; text-align: Left;">
                                                Motivo de la inconformidad
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Motivo_Avaluo_Tramite" runat="server" Width="97%" Enabled="false" Style="text-transform: uppercase" />
                                            </td>
                                        </tr>
                                         <tr>
                                        
                                       <td style="text-align: left; width: 20%;">
                                                Oficio
                                            </td>
                                            <td style="width: 30%; text-align: left;">
                                                <asp:TextBox ID="Txt_Oficio" runat="server" Enabled="false" MaxLength="50" Style="float: left;
                                                    text-transform: uppercase" Width="85%" />
                                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Recepcion_Oficios" runat="server"
                                                    Height="24px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" OnClientClick="javascript:return Abrir_Busqueda_Oficios();"
                                                    TabIndex="8" ToolTip="B�squeda Avanzada Oficios" Width="24px" />
                                                    <asp:HiddenField ID="Hdf_No_Oficio" runat="server" />
                                            </td>
                                            
                                        
                                        <td style="text-align: left; width: 20%; text-align: Left;">
                                                Folio de Aval�o Tramite
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Folio_Avaluo_Tramite" runat="server" Width="97%" Enabled="false" Style="text-transform: uppercase" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                                *Motivo del Aval�o
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:DropDownList ID="Cmb_Motivo_Avaluo" runat="server" Width="98%" TabIndex="12"
                                                    Enabled="false">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: Left;">
                                                No. Aval�o
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_No_Avaluo" runat="server" Width="98%" Enabled="false" Style="text-transform: uppercase" />
                                            </td>
                                        </tr>
                                       
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                *Cuenta Predial
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="98%" Enabled="false" Style="text-transform: uppercase"
                                                    OnTextChanged="Txt_Cuenta_Predial_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <asp:HiddenField ID="Hdf_Cuenta_Predial_Id" runat="server" />
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: Left;">
                                                Clave Catastral
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Clave_Catastral" runat="server" Width="98%" Enabled="false"
                                                    Style="text-transform: uppercase" MaxLength="20" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                *Propietario
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Propietario" runat="server" Width="98%" Enabled="false" Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                            <td style="text-align: Left; width: 20%;">
                                                *Estatus
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%" TabIndex="13">
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
                                                <asp:TextBox ID="Txt_Domicilio_Not" runat="server" Width="98%" Enabled="false" Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: Left;">
                                                *Municipio
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Municipio_Notificar" runat="server" Width="98%" Enabled="false"
                                                    Style="text-transform: uppercase" MaxLength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                *Ubicaci�n
                                            </td>
                                            <td style="text-align: left; width: 30%;" colspan="3">
                                                <asp:TextBox ID="Txt_Ubicacion_Predio" runat="server" Width="98%" Enabled="false"
                                                    Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                *Localidad y Municipio
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Localidad" runat="server" Width="98%" Enabled="false" Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: Left;">
                                                *Fecha
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Fecha" runat="server" Width="98%" Enabled="false" Style="text-transform: uppercase" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                *Solicitante
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Solicitante" runat="server" Width="98%" TabIndex="14" Enabled="false"
                                                    Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left; width: 20%;">
                                                *Nombre del Predio
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Nombre_Predio" runat="server" Width="98%" TabIndex="15" Enabled="false"
                                                    Style="text-transform: uppercase"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                *Uso
                                            </td>
                                            <td style="text-align: left-; width: 30%;">
                                                <asp:DropDownList ID="Cmb_Tipo_Construccion" runat="server" Width="98%" Enabled="false"
                                                    onchange="Validar_Tipo_Construccion();" TabIndex="16" />
                                            </td>
                                            <td style="text-align: left; width: 20%;">
                                            </td>
                                            <td style="text-align: left; width: 80%;">
                                                <asp:TextBox ID="Txt_Uso_Constru" runat="server" Width="98%" Style="text-transform: uppercase"
                                                    TabIndex="16"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="background-color: #3366CC">
                                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                II. LOCALIZACION DEL PREDIO
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                Coordenadas Cartograficas o UTM
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:DropDownList ID="Cmb_Coordenadas" runat="server" Width="96.4%" TabIndex="17"
                                                    Enabled="false" OnSelectedIndexChanged="Cmb_Coordenadas_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE"></asp:ListItem>
                                                    <asp:ListItem Text="GEOGRAFICAS" Value="CART"></asp:ListItem>
                                                    <asp:ListItem Text="UTM" Value="UTM"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <div id="Div_Cartograficas" runat="server" visible="false" style="width: 98%;">
                                            <tr style="background-color: #3366CC">
                                                <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                                    Coordenadas X
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: 20%;">
                                                    Grados (�)
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:TextBox ID="Txt_X_Horas" runat="server" Width="96.4%" Style="text-transform: uppercase"
                                                        Enabled="false" MaxLength="3" TabIndex="18" />
                                                    <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_X_Horas" FilterType="Numbers" runat="server"
                                                        TargetControlID="Txt_X_Horas">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td style="text-align: left; width: 20%;">
                                                    Minutos (')
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:TextBox ID="Txt_X_Minutos" Enabled="false" runat="server" Width="96.4%" Style="text-transform: uppercase"
                                                        MaxLength="3" TabIndex="19" />
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
                                                    <asp:TextBox ID="Txt_X_Segundos" Enabled="false" runat="server" Width="96.4%" Style="text-transform: uppercase"
                                                        MaxLength="6" TabIndex="20" OnTextChanged="Txt_X_Segundos_TextChanged" AutoPostBack="true" />
                                                    <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_X_Segundos" FilterType="Numbers, Custom"
                                                        runat="server" TargetControlID="Txt_X_Segundos" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td style="text-align: left; width: 20%;">
                                                    Orientaci�n
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:DropDownList ID="Cmb_Latitud" runat="server" Width="96.4%" TabIndex="21" Enabled="false">
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
                                                    Grados (�)
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:TextBox ID="Txt_Y_Horas" Enabled="false" runat="server" Width="96.4%" Style="text-transform: uppercase"
                                                        MaxLength="3" TabIndex="22" />
                                                    <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Y_Horas" FilterType="Numbers" runat="server"
                                                        TargetControlID="Txt_Y_Horas">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td style="text-align: left; width: 20%;">
                                                    Minutos (')
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:TextBox ID="Txt_Y_Minutos" Enabled="false" runat="server" Width="96.4%" Style="text-transform: uppercase"
                                                        MaxLength="3" TabIndex="23" />
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
                                                        MaxLength="6" TabIndex="24" OnTextChanged="Txt_Y_Segundos_TextChanged" AutoPostBack="true" />
                                                    <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Y_Segundos" FilterType="Numbers, Custom"
                                                        runat="server" TargetControlID="Txt_Y_Segundos" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td style="text-align: left; width: 20%;">
                                                    Orientaci�n
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:DropDownList ID="Cmb_Longitud" runat="server" Width="96.4%" TabIndex="25" Enabled="false">
                                                        <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE"></asp:ListItem>
                                                        <asp:ListItem Text="NORTE" Value="N"></asp:ListItem>
                                                        <asp:ListItem Text="SUR" Value="S"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </div>
                                        <div id="Div_UTM" runat="server" visible="false" style="width: 98%;">
                                            <tr style="background-color: #3366CC">
                                                <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                                    Coordenadas UTM
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: 20%;">
                                                    Coordenadas X
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:TextBox ID="Txt_Coordenadas_UTM" Enabled="false" runat="server" Width="96.4%"
                                                        TabIndex="26" />
                                                </td>
                                                <td style="text-align: left; width: 20%;">
                                                    Coordenadas Y
                                                </td>
                                                <td style="text-align: left; width: 30%;">
                                                    <asp:TextBox ID="Txt_Coordenadas_UTM_Y" Enabled="false" runat="server" Width="96.4%"
                                                        TabIndex="27" />
                                                </td>
                                            </tr>
                                        </div>
                                        <tr style="background-color: #3366CC">
                                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                III. ELEMENTOS DE CONTRUCCI�N
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="text-align: center;">
                                                <asp:GridView ID="Grid_Elementos_Construccion" runat="server" AllowPaging="False"
                                                    AllowSorting="True" AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                    HeaderStyle-CssClass="tblHead" OnDataBound="Grid_Elementos_Construccion_DataBound"
                                                    PageSize="18" Style="white-space: normal;" Width="100%">
                                                    <Columns>
                                                        <asp:BoundField DataField="ELEMENTOS_CONSTRUCCION_ID" HeaderStyle-Width="0%" HeaderText="Id"
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
                                                                    name="<%=Txt_A.ClientID %>" TabIndex="28" Style="text-transform: uppercase" Width="90%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="B">
                                                            <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_B" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                    name="<%=Txt_B.ClientID %>" TabIndex="29" Style="text-transform: uppercase" Width="90%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="C">
                                                            <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_C" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                    name="<%=Txt_C.ClientID %>" TabIndex="30" Style="text-transform: uppercase" Width="90%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="D">
                                                            <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_D" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                    name="<%=Txt_D.ClientID %>" TabIndex="31" Style="text-transform: uppercase" Width="90%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="E">
                                                            <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_E" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                    name="<%=Txt_E.ClientID %>" TabIndex="32" Style="text-transform: uppercase" Width="90%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="F">
                                                            <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_F" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                    name="<%=Txt_F.ClientID %>" TabIndex="33" Style="text-transform: uppercase" Width="90%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="G">
                                                            <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_G" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                    name="<%=Txt_G.ClientID %>" TabIndex="34" Style="text-transform: uppercase" Width="90%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="H">
                                                            <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_H" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                    name="<%=Txt_H.ClientID %>" TabIndex="35" Style="text-transform: uppercase" Width="90%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="I">
                                                            <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_I" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                    name="<%=Txt_I.ClientID %>" TabIndex="36" Style="text-transform: uppercase" Width="90%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="J">
                                                            <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_J" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                    name="<%=Txt_J.ClientID %>" TabIndex="37" Style="text-transform: uppercase" Width="90%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="K">
                                                            <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_K" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                    name="<%=Txt_K.ClientID %>" TabIndex="38" Style="text-transform: uppercase" Width="90%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="L">
                                                            <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_L" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                    name="<%=Txt_L.ClientID %>" TabIndex="39" Style="text-transform: uppercase" Width="90%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="M">
                                                            <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_M" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                    name="<%=Txt_M.ClientID %>" TabIndex="40" Style="text-transform: uppercase" Width="90%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="N">
                                                            <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_N" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                    name="<%=Txt_N.ClientID %>" TabIndex="41" Style="text-transform: uppercase" Width="90%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="O">
                                                            <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_O" runat="server" CssClass="text_cantidades_grid" Font-Size="X-Small"
                                                                    name="<%=Txt_O.ClientID %>" TabIndex="42" Style="text-transform: uppercase" Width="90%"></asp:TextBox>
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
                                                IV. CARACTERISTICAS DEL TERRENO
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
                                                        <asp:BoundField DataField="DESCRIPCION_RUSTICO_ID1" HeaderStyle-Width="10%" HeaderText="Id_Detalles"
                                                            Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="VALOR_INDICE1" HeaderStyle-Width="10%">
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
                                                                    Font-Size="X-Small" name="<%=Chk_Indicador_Valor_A.ClientID %>" TabIndex="43"
                                                                    AutoPostBack="true" OnCheckedChanged="Chk_Indicador_Valor_CheckedChanged"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="DESCRIPCION_RUSTICO_ID2" HeaderStyle-Width="10%" HeaderText="Id_Detalles"
                                                            Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="VALOR_INDICE2" HeaderStyle-Width="10%">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="INDICADOR_B" HeaderStyle-Width="10%">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="right" Width="5%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Indicador_Valor_B" runat="server" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Chk_Indicador_Valor_B.ClientID %>" TabIndex="44"
                                                                    AutoPostBack="true" OnCheckedChanged="Chk_Indicador_Valor_CheckedChanged"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="DESCRIPCION_RUSTICO_ID3" HeaderStyle-Width="10%" HeaderText="Id_Detalles"
                                                            Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="VALOR_INDICE3" HeaderStyle-Width="10%">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="INDICADOR_C" HeaderStyle-Width="10%">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="right" Width="5%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Indicador_Valor_C" runat="server" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Chk_Indicador_Valor_C.ClientID %>" TabIndex="45"
                                                                    AutoPostBack="true" OnCheckedChanged="Chk_Indicador_Valor_CheckedChanged"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="DESCRIPCION_RUSTICO_ID4" HeaderStyle-Width="10%" HeaderText="Id_Detalles"
                                                            Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="VALOR_INDICE4" HeaderStyle-Width="10%">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="INDICADOR_D" HeaderStyle-Width="10%">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="right" Width="5%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Indicador_Valor_D" runat="server" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" name="<%=Chk_Indicador_Valor_D.ClientID %>" TabIndex="46"
                                                                    AutoPostBack="true" OnCheckedChanged="Chk_Indicador_Valor_CheckedChanged"></asp:CheckBox>
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
                                                V. CALCULOS DEL VALOR DEL TERRENO
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
                                                                    Font-Size="X-Small" name="<%=Txt_Superficie_M2.ClientID %>" TabIndex="47" OnTextChanged="Txt_Superficie_M2_Cal_TextChanged"
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
                                                                    Width="80%" TabIndex="47"></asp:TextBox>
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
                                                    Width="98%"></asp:TextBox>
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
                                                                <asp:TextBox ID="Txt_Croquis" runat="server" Font-Size="X-Small" TabIndex="48" name="<%=Txt_Croquis.ClientID %>"
                                                                    Style="text-transform: uppercase" Width="98%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="TIPO">
                                                            <HeaderStyle HorizontalAlign="right" Width="5%" />
                                                            <ItemStyle HorizontalAlign="right" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="Txt_Tipo" runat="server" AutoPostBack="true" CssClass="text_cantidades_grid"
                                                                    Font-Size="X-Small" TabIndex="48" name="<%=Txt_Tipo.ClientID %>" OnTextChanged="Txt_Tipo_Constru_TextChanged"
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
                                                                    Font-Size="X-Small" name="<%=Txt_Con_Serv.ClientID %>" TabIndex="48" OnTextChanged="Txt_Con_Serv_Constru_TextChanged"
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
                                                                    Font-Size="X-Small" name="<%=Txt_Superficie_M2.ClientID %>" TabIndex="48" OnTextChanged="Txt_Superficie_M2_Constru_TextChanged"
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
                                                                    Font-Size="X-Small" TabIndex="48" name="<%=Txt_Edad.ClientID %>" OnTextChanged="Txt_Edad_TextChanged"
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
                                                                    Font-Size="X-Small" TabIndex="48" name="<%=Txt_Factor.ClientID %>" OnTextChanged="Txt_Factor_Constru_TextChanged"
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
                                                                    name="<%=Txt_Uso.ClientID %>" Style="text-transform: uppercase" Width="98%"></asp:TextBox>
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
                                                    Style="text-transform: uppercase" Width="98%"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: left;">
                                                Valor Total
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Construccion_Valor_Total" runat="server" Enabled="false" Style="text-transform: uppercase"
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
                                                VII. VALOR TOTAL DEL PREDIO
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td style="text-align: left; width: 20%;">
                                                Valor Total del Predio
                                            </td>
                                            <td style="text-align: left; width: 30%;">
                                                <asp:TextBox ID="Txt_Valor_Total_Predio" runat="server" Enabled="false" MaxLength="15"
                                                    Style="text-transform: uppercase" Width="98%"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTB_Txt_Inpr" runat="server" FilterType="Custom, Numbers"
                                                    TargetControlID="Txt_Valor_Total_Predio" ValidChars="1234567890.," />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr style="background-color: #3366CC">
                                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                VIII. MEDIDAS Y COLINDANCIAS: SEGUN PLANO DE LOTIFICACION
                                            </td>
                                        </tr>
                                        <tr id="Tr1" runat="server">
                                            <td style="text-align: left; width: 20%" class="style1">
                                                Medidas y Colindancias
                                            </td>
                                            <td colspan="3" style="width: 80%">
                                                <asp:TextBox ID="Txt_Medida_Colindancia" Style="text-transform: uppercase" runat="server"
                                                    TextMode="MultiLine" Rows="3" Width="98%" TabIndex="49"></asp:TextBox>
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
                                                    Width="20px" TabIndex="50" />
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
                                        </tr>
                                        <tr style="background-color: #3366CC">
                                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                IX. DOCUMENTOS
                                            </td>
                                        </tr>
                                        <tr id="Tr_Fila_Fotografias_Bien" runat="server">
                                            <td style="text-align: left; width: 20%" class="style1">
                                                Nombre del Documento
                                            </td>
                                            <td style="text-align: left; width: 30%" class="style1">
                                                <asp:DropDownList ID="Cmb_Documento" runat="server" Width="98%" TabIndex="51">
                                                    <asp:ListItem Text="ANEXO 1" Value="ANEXO_1" />
                                                    <asp:ListItem Text="ANEXO 2" Value="ANEXO_2" />
                                                    <asp:ListItem Text="ARCHIVO AUTOCAD" Value="ARCHIVO" />
                                                </asp:DropDownList>
                                            </td>
                                            <td style="text-align: left; width: 20%" class="style2">
                                                Documento
                                            </td>
                                            <td style="width: 30%" class="style2">
                                                <asp:FileUpload ID="Fup_Documento" runat="server" Width="98%" TabIndex="52" />
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
                                                    TabIndex="53" />
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
                                                <br />
                                            </td>
                                        </tr>
                                        <tr style="background-color: #3366CC">
                                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                                IX. OBSERVACIONES
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                                *Observaciones
                                            </td>
                                            <td colspan="3" style="width: 98%; text-align: left;">
                                                <asp:TextBox ID="Txt_Observaciones" runat="server" Rows="3" Style="float: left; text-transform: uppercase"
                                                    TextMode="MultiLine" Width="96.4%" TabIndex="54" />
                                            </td>
                                        </tr>
                                        <tr style="background-color: #3366CC">
                                            <%--<td colspan="4" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                        X. IMPORTE DEL AVAL�O
                                    </td>--%>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td style="text-align: left; width: 20%">
                                                <%--Importe del Aval�o--%>
                                            </td>
                                            <td>
                                                <%--style="text-align:left; width:30%">
                                        <asp:TextBox ID="Txt_Precio_Avaluo" runat="server" Enabled="false" 
                                            Width="96.4%"--%>
                                                <asp:HiddenField ID="Hdf_Factor_Cobro" runat="server" />
                                                <asp:HiddenField ID="Hdf_Menos_Ha" runat="server" />
                                                <asp:HiddenField ID="Hdf_Mayor_Ha" runat="server" />
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
            <%-- <div ID="Div1" runat="server" visible="false">
                    <table border="0" cellspacing="0" class="estilo_fuente" width="98%">
                        <tr style="background-color: #3366CC">
                            <td colspan="2" style="text-align:left; font-size:15px; color:#FFFFFF;">
                                OBSERVACIONES: MOTIVOS DEL RECHAZO
                            </td>
                        </tr>
                        <tr>
                            <td>
                                </td>
                        </tr>
                        <tr>
                            <td style="width:20%">
                                </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="Grid_Observaciones" runat="server" AllowPaging="True" 
                                    AllowSorting="True" AutoGenerateColumns="False" CssClass="GridView_1" 
                                    EmptyDataText="&quot;No se encontraron registros&quot;" 
                                    HeaderStyle-CssClass="tblHead" 
                                    OnPageIndexChanging="Grid_Observaciones_PageIndexChanging" PageSize="10" 
                                    style="white-space:normal;" Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="NO_SEGUIMIENTO" HeaderStyle-Width="5%" 
                                            HeaderText="no seguimiento" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MOTIVO_ID" HeaderStyle-Width="5%" 
                                            HeaderText="Motivo Id" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MOTIVO_DESCRIPCION" HeaderStyle-Width="93%" 
                                            HeaderText="Motivo de Rechazo">
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
            <td></td>
            </tr>
            <tr>
                        <td style="width:20%">
                        Observaciones
                        </td>
                        <td style="width:80%">
                        <asp:TextBox ID="Txt_Observaciones_Rechazo" runat="server" TextMode="MultiLine" Rows="3" Width="98%"></asp:TextBox>
                        </td>
                        </tr>
                    </table>
            </div>
            </td>
            </tr>
                </table>
                </center>
            </div>--%>
            </table> </center> </div>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="Mpe_Avaluos" runat="server" TargetControlID="Btn_Comodin_Open_Avaluo"
        PopupControlID="Pnl_Avaluos" BackgroundCssClass="popUpStyle" BehaviorID="Busqueda_Avaluos"
        CancelControlID="Btn_Comodin_Close_Avaluo" DropShadow="true" DynamicServicePath=""
        Enabled="True" />
    <asp:Button ID="Btn_Comodin_Open_Avaluo" runat="server" Style="background-color: transparent;
        border-style: none; display: none;" Text="" />
    <asp:Button ID="Btn_Comodin_Close_Avaluo" runat="server" Style="background-color: transparent;
        border-style: none; display: none;" Text="" />
    <%--Ventana modal de b�squeda de peritos internos--%>
    <asp:Panel ID="Pnl_Avaluos" runat="server" CssClass="drag" HorizontalAlign="Center"
        Width="650px" Style="display: none; border-style: outset; border-color: Silver;
        background-image: url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG); background-repeat: repeat-y;">
        <asp:Panel ID="Pnl_Titulo_Avaluos" runat="server" Style="cursor: move; background-color: Silver;
            color: Black; font-size: 12; font-weight: bold; border-style: outset;">
            <table width="99%">
                <tr>
                    <td style="color: Black; font-size: 12; font-weight: bold;">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                        B&uacute;squeda: Aval�os Rusticos
                    </td>
                    <td align="right" style="width: 10%;">
                        <asp:ImageButton ID="Btn_Cerrar_Ventana_Avaluo" CausesValidation="false" runat="server"
                            Style="cursor: pointer;" ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"
                            OnClick="Btn_Cerrar_Ventana_Avaluo_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:UpdatePanel ID="Udp_Avaluos" runat="server">
            <ContentTemplate>
                <div style="color: #5D7B9D">
                    <table width="100%">
                        <tr>
                            <td align="left" style="text-align: left;">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Udp_Avaluos"
                                    DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter1" class="progressBackgroundFilter">
                                        </div>
                                        <div style="background-color: Transparent; position: fixed; top: 50%; left: 47%;
                                            padding: 10px; z-index: 1002;" id="div_progress1">
                                            <img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table width="100%">
                                    <%--                                   <tr>
                                        <td style="width:100%" colspan="4" align="right">
                                            <asp:ImageButton ID="Btn_Limpiar_Avaluo" runat="server" OnClientClick="javascript:return Limpiar_Ctlr();"
                                                ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" ToolTip="Limpiar Controles de Busqueda"/>                         
                                        </td>
                                    </tr>     --%>
                                    <tr>
                                        <td style="width: 100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left; font-size: 11px;">
                                            Folio Aval�o
                                        </td>
                                        <td style="width: 30%; text-align: right; font-size: 11px;">
                                            <asp:TextBox ID="Txt_Busqueda_Avaluo" runat="server" Width="98%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="Grid_Avaluos_Av" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="15" Style="white-space: normal;" Width="100%"
                                                OnSelectedIndexChanged="Grid_Avaluos_Av_SelectedIndexChanged" OnPageIndexChanging="Grid_Avaluos_Av_PageIndexChanging">
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
                                    <tr>
                                        <td style="width: 100%; text-align: left;" colspan="4">
                                            <center>
                                                <asp:Button ID="Btn_Busqueda_Avaluos" runat="server" Text="Busqueda de Aval�os" CssClass="button"
                                                    CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Avaluos_Click" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <cc1:ModalPopupExtender ID="Mpe_Busqueda_Solicitud" runat="server" TargetControlID="Btn_Comodin_Open_Busqueda_Solicitudes"
        PopupControlID="Pnl_Busqueda_Contenedor" BackgroundCssClass="popUpStyle" BehaviorID="Busqueda_Solicitudes"
        CancelControlID="Btn_Comodin_Close_Busqueda_Solicitudes" DropShadow="true" DynamicServicePath=""
        Enabled="True" />
    <asp:Button ID="Btn_Comodin_Close_Busqueda_Solicitudes" runat="server" Style="background-color: transparent;
        border-style: none; display: none;" Text="" />
    <asp:Button ID="Btn_Comodin_Open_Busqueda_Solicitudes" runat="server" Style="background-color: transparent;
        border-style: none; display: none;" Text="" />
    <%--Ventana modal de b�squeda de Solicitudes--%>
    <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag" HorizontalAlign="Center"
        Width="650px" Style="display: none; border-style: outset; border-color: Silver;
        background-image: url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG); background-repeat: repeat-y;">
        <asp:Panel ID="Panel1" runat="server" Style="cursor: move; background-color: Silver;
            color: Black; font-size: 12; font-weight: bold; border-style: outset;">
            <table width="99%">
                <tr>
                    <td style="color: Black; font-size: 12; font-weight: bold;">
                        <asp:Image ID="Img_Informatcion_Autorizacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                        B&uacute;squeda: Solicitudes
                    </td>
                    <td align="right" style="width: 10%;">
                        <asp:ImageButton ID="Btn_Cerrar_Ventana" CausesValidation="false" runat="server"
                            Style="cursor: pointer;" ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"
                            OnClick="Btn_Cerrar_Ventana_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Prestamos" runat="server">
            <ContentTemplate>
                <div style="color: #5D7B9D">
                    <table width="100%">
                        <tr>
                            <td align="left" style="text-align: left;">
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server"
                                    AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Prestamos" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter1" class="progressBackgroundFilter">
                                        </div>
                                        <div style="background-color: Transparent; position: fixed; top: 50%; left: 47%;
                                            padding: 10px; z-index: 1002;" id="div_progress1">
                                            <img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left; font-size: 11px;">
                                            Cuenta Predial
                                        </td>
                                        <td style="width: 30%; text-align: right; font-size: 11px;" colspan="3">
                                            <asp:TextBox ID="Txt_Cuenta_Predial_Bqd" runat="server" Width="98%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left; font-size: 11px;">
                                            Solicitante
                                        </td>
                                        <td style="width: 30%; text-align: right; font-size: 11px;" colspan="3">
                                            <asp:TextBox ID="Txt_Solicitante_Bqd" runat="server" Width="98%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="Grid_Solicitudes" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="15" Style="white-space: normal;" Width="100%"
                                                OnSelectedIndexChanged="Grid_Solicitudes_SelectedIndexChanged" OnPageIndexChanging="Grid_Solicitudes_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="SOLICITUD_ID" HeaderStyle-Width="15%" HeaderText="ID"
                                                        Visible="false">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CLAVE_SOLICITUD" HeaderStyle-Width="15%" HeaderText="Clave Solicitud">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NOMBRE_COMPLETO" HeaderStyle-Width="15%" HeaderText="Nombre">
                                                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="USUARIO_CREO" HeaderStyle-Width="15%" HeaderText="Solicitante">
                                                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="MOTIVO_AVALUO" HeaderText="Motivo de la inconformidad">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                     <asp:BoundField DataField="AVALUO_INCONFORMIDAD" HeaderText="folio del aval�o">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
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
                                        <td style="width: 100%; text-align: left;" colspan="4">
                                            <center>
                                                <asp:Button ID="Btn_Busqueda_Solicitudes" runat="server" Text="Busqueda Solicitudes"
                                                    CssClass="button" CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Solicitudes_Click" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    
     <cc1:ModalPopupExtender ID="Mpe_Busqueda_Oficios" runat="server" TargetControlID="Btn_Comodin_Open_Busqueda_Oficios"
        PopupControlID="Pnl_Busqueda_Oficios" BackgroundCssClass="popUpStyle" BehaviorID="Busqueda_Oficios"
        CancelControlID="Btn_Comodin_Close_Busqueda_Oficios" DropShadow="true" DynamicServicePath=""
        Enabled="True" />
    <asp:Button ID="Btn_Comodin_Close_Busqueda_Oficios" runat="server" Style="background-color: transparent;
        border-style: none; display: none;" Text="" />
    <asp:Button ID="Btn_Comodin_Open_Busqueda_Oficios" runat="server" Style="background-color: transparent;
        border-style: none; display: none;" Text="" />
        
        
    <%--Ventana modal de b�squeda de Oficios--%>
    <asp:Panel ID="Pnl_Busqueda_Oficios" runat="server" CssClass="drag" HorizontalAlign="Center"
        Width="650px" Style="display: none; border-style: outset; border-color: Silver;
        background-image: url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG); background-repeat: repeat-y;">
        <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: Silver;
            color: Black; font-size: 12; font-weight: bold; border-style: outset;">
            <table width="99%">
                <tr>
                    <td style="color: Black; font-size: 12; font-weight: bold;">
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                        B&uacute;squeda: Oficios
                    </td>
                    <td align="right" style="width: 10%;">
                        <asp:ImageButton ID="ImageButton1" CausesValidation="false" runat="server"
                            Style="cursor: pointer;" ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"
                            OnClick="Btn_Cerrar_Ventana_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="color: #5D7B9D">
                    <table width="100%">
                        <tr>
                            <td align="left" style="text-align: left;">
                                <asp:UpdateProgress ID="UpdateProgress2" runat="server"
                                    AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Prestamos" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter1" class="progressBackgroundFilter">
                                        </div>
                                        <div style="background-color: Transparent; position: fixed; top: 50%; left: 47%;
                                            padding: 10px; z-index: 1002;" id="div_progress1">
                                            <img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left; font-size: 11px;">
                                            Numero de Oficio
                                        </td>
                                        <td style="width: 30%; text-align: right; font-size: 11px;" colspan="3">
                                            <asp:TextBox ID="Txt_No_Oficio" runat="server" Width="98%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left; font-size: 11px;">
                                            Dependencia
                                        </td>
                                        <td style="width: 30%; text-align: right; font-size: 11px;" colspan="3">
                                            <asp:TextBox ID="Txt_Dependencia" runat="server" Width="98%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="Grid_Oficios" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="15" Style="white-space: normal;" Width="100%"
                                                OnSelectedIndexChanged="Grid_Oficios_SelectedIndexChanged" OnPageIndexChanging="Grid_Oficios_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="NO_OFICIO" HeaderStyle-Width="15%" HeaderText="ID" Visible="false">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DESCRIPCION" HeaderStyle-Width="15%" HeaderText="Descripcion">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DEPENDENCIA" HeaderStyle-Width="15%" HeaderText="Dependencia">
                                                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NO_OFICIO_RECEPCION" HeaderText="No oficio Recepcion">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FECHA_RECEPCION" HeaderText="Fecha Recepcion" DataFormatString="{0:dd-MMM-yyyy}">
                                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="20%" />
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
                                        <td style="width: 100%; text-align: left;" colspan="4">
                                            <center>
                                                <asp:Button ID="Btn_Busqueda_Oficios" runat="server" Text="Busqueda Oficios" CssClass="button"
                                                    CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Oficios_Click" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>   
</asp:Content>

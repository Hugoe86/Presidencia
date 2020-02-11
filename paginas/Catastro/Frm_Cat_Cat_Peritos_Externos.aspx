<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Cat_Cat_Peritos_Externos.aspx.cs"
    Inherits="paginas_Catastro_Frm_Cat_Cat_Peritos_Externos" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    Title="Catálogo de Peritos Externos" %>

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
        .style2
        {
            width: 39%;
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
                        <img alt="" src="../Imagenes/paginas/Updating.gif" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Calles" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Cat&aacute;logo de Peritos Externos
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
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" class="style1" colspan="3">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Nuevo" OnClick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Modificar" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Eliminar" OnClientClick="return confirm('¿Esta seguro que desea dar de Baja al Perito?');"
                                OnClick="Btn_Eliminar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click" />
                        </td>
                        <td align="right" class="style1">
                            Búsqueda:
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="128px" TabIndex="6" Style="text-transform: uppercase"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar" runat="server" CausesValidation="false" TabIndex="7"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" OnClick="Btn_Buscar_Click" Width="24px"/>
                        </td>
                    </tr>
                </table>
                <br />
                <center>
                    <table width="98%" class="estilo_fuente">
                        <div id="Div_Grid_Peritos" runat="server">
                        </div>
                        <div id="Div_Grid_Datos_Peritos" runat="server"></div>
                            <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                                <tr style="background-color: #3366CC">
                                    <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                        Perito Externo
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 20%;">
                                        <asp:HiddenField ID="Hdf_Perito_Externo" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 20%;">
                                        *Nombre
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Nombre" runat="server" Width="98%" TabIndex="9" Enabled="false"
                                            MaxLength="50" Style="text-transform: uppercase"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left; width: 20%; text-align: left;">
                                        *Apellido Paterno
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Apellido_Paterno" runat="server" Width="98%" TabIndex="10" Enabled="false"
                                            MaxLength="50" Style="text-transform: uppercase" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 20%;">
                                        *Apellido Materno
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Apellido_Materno" runat="server" Width="98%" TabIndex="11" Enabled="false"
                                            MaxLength="50" Style="text-transform: uppercase"></asp:TextBox>
                                    </td>
                                </tr>
                                <div id="Div_Seguridad_Peritos_Externos" runat="server" >
                                    <tr>
                                        <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                            Seguridad
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; width: 20%;">
                                            *Usuario
                                        </td>
                                        <td style="text-align: left; width: 30%;">
                                            <asp:TextBox ID="Txt_Usuario" runat="server" Enabled="false" MaxLength="100" TabIndex="12"
                                                Width="98%"></asp:TextBox>
                                        </td>
                                        <td style="text-align: left; width: 20%; text-align: left;">
                                            *Contraseña
                                        </td>
                                        <td style="text-align: left; width: 30%;">
                                            <asp:TextBox ID="Txt_Password" runat="server" Enabled="false" MaxLength="13" TabIndex="13"
                                                TextMode="Password" Width="98%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; width: 20%;">
                                            <%--Usuario--%>
                                        </td>
                                        <td style="text-align: left; width: 30%;">
                                            <%--<asp:TextBox ID="TextBox1" runat="server" Width="98%" 
                                TabIndex="9" Enabled="false"></asp:TextBox>--%>
                                        </td>
                                        <td style="text-align: left; width: 20%; text-align: left">
                                            *Confirmar Contraseña
                                        </td>
                                        <td style="text-align: left; width: 30%;">
                                            <asp:TextBox ID="Txt_Password_Confirma" runat="server" Enabled="false" MaxLength="20"
                                                TabIndex="14" TextMode="Password" Width="98%" />
                                        </td>
                                    </tr>
                                </div>
                                <tr style="background-color: #3366CC">
                                    <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                        Datos del Perito Externo
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 20%;">
                                        *Calle
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Calle" runat="server" Width="98%" TabIndex="15" Enabled="false"
                                            MaxLength="50" Style="text-transform: uppercase"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left; width: 20%; text-align: left;">
                                        *Colonia
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Colonia" runat="server" Width="98%" TabIndex="16" Enabled="false"
                                            MaxLength="50" Style="text-transform: uppercase" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 20%; text-align: left;">
                                        *Estado
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Estado" runat="server" Width="98%" TabIndex="17" Enabled="false"
                                            MaxLength="20" Style="text-transform: uppercase" />
                                    </td>
                                    <td style="text-align: left; width: 20%; text-align: left;">
                                        *Ciudad
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Ciudad" runat="server" Width="98%" TabIndex="18" Enabled="false"
                                            MaxLength="20" Style="text-transform: uppercase" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 20%; vertical-align: top;">
                                        *Teléfono
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Telefono" runat="server" TabIndex="19" MaxLength="10" Width="98%"
                                            Style="text-transform: uppercase;" Enabled="false" />
                                        <cc1:FilteredTextBoxExtender FilterType="Numbers" runat="server" ID="FTB_Txt_Anio"
                                            TargetControlID="Txt_Telefono" />
                                    </td>
                                    <td style="text-align: left; width: 20%; vertical-align: top;">
                                        Celular
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Celular" runat="server" TabIndex="20" MaxLength="10" Width="98%"
                                            Style="text-transform: uppercase;" Enabled="false" />
                                        <cc1:FilteredTextBoxExtender FilterType="Numbers" runat="server" ID="FilteredTextBoxExtender1"
                                            TargetControlID="Txt_Celular" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 20%; vertical-align: top;">
                                        *Estatus
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" TabIndex="12" Enabled="false">
                                            <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE"></asp:ListItem>
                                            <asp:ListItem Text="PENDIENTE" Value="PENDIENTE"></asp:ListItem>
                                            <asp:ListItem Text="VIGENTE" Value="VIGENTE"></asp:ListItem>
                                            <asp:ListItem Text="BAJA" Value="BAJA"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: left; width: 20%; vertical-align: top;">
                                        *Fecha Vencimiento
                                    </td>
                                    <td style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Fecha" runat="server" TabIndex="13" Width="98%" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 20%; vertical-align: top;">
                                        Observaciones
                                    </td>
                                    <td colspan="3" style="text-align: left; width: 30%;">
                                        <asp:TextBox ID="Txt_Observaciones" runat="server" TabIndex="21" MaxLength="250"
                                            Width="98%" Style="text-transform: uppercase;" Enabled="false" Rows="3" TextMode="MultiLine" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="text-align: left; width: 20%;">
                                        <asp:GridView ID="Grid_Documentos" runat="server" AllowPaging="False" AllowSorting="True"
                                            AutoGenerateColumns="False" HeaderStyle-CssClass="tblHead" PageSize="20" OnDataBound="Grid_Documentos_DataBound"
                                            Style="white-space: normal;" Width="100%">
                                            <Columns>
                                                <asp:BoundField DataField="NO_DOCUMENTO" HeaderText="No documento" Visible="false">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PERITO_EXTERNO_ID" HeaderText="Perito Id" Visible="false">
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
                                                <asp:BoundField DataField="NO_BIEN" Visible="false" />
                                            </Columns>
                                            <HeaderStyle CssClass="tblHead" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr style="background-color: #3366CC">
                                    <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                        Peritos Externos
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:GridView ID="Grid_Peritos_Externos" runat="server" AllowPaging="True" AllowSorting="True"
                                            AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                            HeaderStyle-CssClass="tblHead" PageSize="15" Style="white-space: normal;" Width="100%"
                                            OnSelectedIndexChanged="Grid_Peritos_Externos_SelectedIndexChanged" OnPageIndexChanging="Grid_Peritos_Externos_PageIndexChanging">
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="PERITO_EXTERNO_ID" HeaderStyle-Width="15%" HeaderText="ID"
                                                    Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PERITO_EXTERNO" HeaderStyle-Width="15%" HeaderText="Perito Externo">
                                                    <HeaderStyle HorizontalAlign="Left" Width="70%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="70%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FECHA" HeaderText="Fecha del próximo Refrendo" DataFormatString="{0:dd/MMM/yyyy}">
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
                            </table>
                    </table>
                </center>
            </div>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

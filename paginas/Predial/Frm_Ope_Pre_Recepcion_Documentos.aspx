<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Recepcion_Documentos.aspx.cs" Inherits="paginas_predial_Frm_Ope_Pre_Recepcion_Documentos"
    Title="Operación - Recepción de Documentos" UICulture="es-MX" Culture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type="text/javascript" language="javascript">
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_sesiones.ashx";

        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion()
        {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }

        //Temporizador para matener la sesión activa
        setInterval('MantenSesion()', "<%=(int)(0.9*(Session.Timeout * 60000))%>");

    window.onerror = new Function("return true");
    //Metodos para limpiar los controles de la busqueda.    
        //Abrir una ventana modal
		function Abrir_Ventana_Modal(Url, Propiedades)
		{
            window.showModalDialog(Url, null, Propiedades);
		}

        function UploadComplete(sender, args) {
            __doPostBack('tctl00$Cph_Area_Trabajo1$Upd_Panel', '');
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager runat="Server" ID="ScriptManager1" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" EnablePartialRendering="true" AsyncPostBackTimeout="3600" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server"
                AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color: #ffffff; width: 98%; height: 100%;
                margin-left: 10px;">
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">
                            Recepción de Documentos
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                                Visible="true" />
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
                    <%---------------------------------- Barra principal de botones y búsqueda ----------------------------------%>
                    <tr class="barra_busqueda" align="right">
                        <td style="width: 40%; text-align: left;">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                CssClass="Img_Button" ToolTip="Nuevo" TabIndex="1" OnClick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                CssClass="Img_Button" ToolTip="Modificar" TabIndex="2" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Modificar_Recepcion" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                CssClass="Img_Button" TabIndex="2" OnClick="Btn_Modificar_Recepcion_Click" />
                            <asp:ImageButton ID="Btn_Imprimir" runat="server" ImageUrl="~/paginas/imagenes/gridview/grid_print.png"
                                CssClass="Img_Button" ToolTip="Imprimir" TabIndex="2" OnClick="Btn_Imprimir_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                ToolTip="Inicio" TabIndex="3" OnClick="Btn_Salir_Click" />
                        </td>
                        <td style="text-align: right;">
                            Búsqueda por
                            <asp:DropDownList ID="Cmb_Busqueda_Por" runat="server" Width="150px">
                                <asp:ListItem Value="0"><-Seleccione-></asp:ListItem>
                                <asp:ListItem Value="FOLIO">FOLIO</asp:ListItem>
                                <asp:ListItem Value="NOTARIO">NOTARIO</asp:ListItem>
                                <asp:ListItem Value="NOTARIA">NOTAR&Iacute;A</asp:ListItem>
                                <asp:ListItem Value="CUENTA">CUENTA</asp:ListItem>
                                <asp:ListItem Value="ESCRITURA">ESCRITURA</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;
                            <asp:TextBox ID="Txt_Busqueda_Recep_Docs" runat="server" Width="160px" TabIndex="4"
                                Style="text-transform: uppercase"> </asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Recep_Docs" runat="server" WatermarkText="<- Recepción de documento ->"
                                TargetControlID="Txt_Busqueda_Recep_Docs" WatermarkCssClass="watermarked">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Recep_Docs" runat="server" TargetControlID="Txt_Busqueda_Recep_Docs"
                                InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                            <asp:ImageButton ID="Btn_Buscar_Recepciones_Notario" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" TabIndex="5"
                                OnClick="Btn_Buscar_Recepciones_Notario_Click" />
                        </td>
                    </tr>
                </table>
                <%---------------------------------- Datos Notario ----------------------------------%>
                <br />
                <table width="100%" class="estilo_fuente">
                    <tr>
                        <td style="width: 18%; text-align: left;">
                            <asp:Label ID="Lbl_Notario_ID" runat="server" Text="No. Notaria" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width: 82%; text-align: left;" colspan="3">
                            <asp:TextBox ID="Txt_Numero_Notaria" runat="server" Width="37.7%" Enabled="False"
                                OnTextChanged="Txt_Notario_ID_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Notario_ID" runat="server" TargetControlID="Txt_Numero_Notaria"
                                FilterType="Numbers" />
                            &nbsp; B&uacute;squeda
                            <asp:ImageButton ID="Btn_Mostrar_Busqueda_Notario" runat="server" ToolTip="Búsqueda Avanzada de Notarios"
                                TabIndex="7" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px"
                                Width="24px" CausesValidation="true" OnClick="Btn_Mostrar_Busqueda_Notario_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 18%; text-align: left;">
                            <asp:Label ID="Lnl_RFC_Notario" runat="server" Text="RFC Notario" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width: 32%; text-align: left;">
                            <asp:TextBox ID="Txt_RFC_Notario" runat="server" Width="98%" Enabled="False" TabIndex="8"
                                MaxLength="10"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_RFC_Notario" runat="server" TargetControlID="Txt_RFC_Notario"
                                FilterType="UppercaseLetters, LowercaseLetters, Numbers" />
                        </td>
                        <td style="width: 15%; text-align: right;">
                            <asp:Label ID="Lbl_Numero_Notaria" runat="server" Text="Notario ID" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width: 35%; text-align: left;">
                            <asp:TextBox ID="Txt_Notario_ID" runat="server" Width="92.7%" Enabled="False" TabIndex="6"
                                MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%; text-align: left;">
                            <asp:Label ID="Lbl_Nombre_Notario" runat="server" Text="Notario" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width: 85%; text-align: left;" colspan="3">
                            <asp:TextBox ID="Txt_Nombre_Notario" runat="server" Width="97%" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <br />
                            <asp:GridView ID="Grid_Recepciones_Notario" runat="server" Style="white-space: normal;"
                                AutoGenerateColumns="False" CssClass="GridView_1" PageSize="10" AllowPaging="true"
                                OnSelectedIndexChanged="Grid_Recepciones_Notario_SelectedIndexChanged" OnPageIndexChanging="Grid_Recepciones_Notario_PageIndexChanging">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <HeaderStyle HorizontalAlign="Left" Width="2%" />
                                        <ItemStyle HorizontalAlign="Left" Width="2%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="NO_RECEPCION_DOCUMENTO" HeaderText="No. Recepción">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}">
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE_NOTARIO" HeaderText="Notario" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TOTAL_MOVIMIENTOS" HeaderText="Recibidos">
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Center" Width="8%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TOTAL_PENDIENTES" HeaderText="Pendientes">
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Center" Width="8%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TOTAL_RECHAZADOS" HeaderText="Rechazados">
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Center" Width="8%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TOTAL_ACEPTADOS" HeaderText="Aceptados">
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Center" Width="8%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TOTAL_CANCELADOS" HeaderText="Cancelados">
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Center" Width="8%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOTARIO_ID" HeaderText="ID_Notario" Visible="false" />
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:GridView ID="Grid_Detalles_Recepcion" runat="server" Style="white-space: normal;
                                margin-left: 5%; width: 93%;" AutoGenerateColumns="False" PageSize="10" AllowPaging="true"
                                OnSelectedIndexChanged="Grid_Detalles_Recepcion_SelectedIndexChanged" OnPageIndexChanging="Grid_Detalles_Recepcion_PageIndexChanging"
                                OnRowCommand="Borrar_Registro">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="NO_MOVIMIENTO" HeaderText="No. Trámite">
                                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                        <ItemStyle HorizontalAlign="Left" Width="12%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DOCUMENTOS" HeaderText="Documentos recibidos" />
                                    <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial">
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                        <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                        <ItemStyle HorizontalAlign="Center" Width="12%" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="Btn_Eliminar_Seleccionar" runat="server" Height="20px" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png"
                                                TabIndex="10" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro?')"
                                                ToolTip="Borrar_Recepcion" Width="20px" CommandName="Erase" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                </table>
                <%---------------- Datos cuenta ----------------------%>
                <asp:Panel ID="Pnl_Contenedor_Datos_Cuenta_Predial" runat="server" Visible="false">
                    <table width="100%" class="estilo_fuente">
                        <tr>
                            <td style="width: 18%; text-align: left;">
                                <asp:HiddenField ID="Hdn_Cuenta_Predial_ID" runat="server" />
                                <asp:HiddenField ID="Hdn_No_Movimiento" runat="server" />
                                <asp:Label ID="Lbl_Cuenta_Predial" runat="server" Text="Cuenta Predial" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 82%; text-align: left;">
                                <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="37.7%" AutoPostBack="true"
                                    Style="text-transform: uppercase" Enabled="false" TabIndex="9" MaxLength="20"
                                    OnTextChanged="Txt_Cuenta_Predial_TextChanged"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cuenta_Predial" runat="server" TargetControlID="Txt_Cuenta_Predial"
                                    FilterType="UppercaseLetters, LowercaseLetters, Numbers" />
                                &nbsp; B&uacute;squeda
                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Cuentas" runat="server" ToolTip="Búsqueda Avanzada de Cuenta Predial"
                                    TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px"
                                    Width="24px" CausesValidation="false" OnClick="Btn_Mostrar_Busqueda_Cuentas_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%; text-align: left;">
                                <asp:Label ID="Lbl_Nombre_Propietario" runat="server" Text="Propietario" AssociatedControlID="Txt_Nombre_Propietario"
                                    CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 85%; text-align: left;">
                                <asp:TextBox ID="Txt_Nombre_Propietario" runat="server" Width="97%" Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%; text-align: left;">
                                <asp:Label ID="Lbl_Ubicacion_Cuenta" runat="server" Text="Ubicación" AssociatedControlID="Txt_Ubicacion_Cuenta"
                                    CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 85%; text-align: left;">
                                <asp:TextBox ID="Txt_Ubicacion_Cuenta" runat="server" Width="97%" Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <br />
                </asp:Panel>
                <%------------------------ Documentos ---------------------------%>
                <asp:Panel ID="Pnl_Contenedor_Datos_Recepcion_Documentos" runat="server" Style="display: none;">
                    <table style="width: 98%;" class="estilo_fuente" id="Tbl_Documentos_Controles">
                        <tr style="height: 40px; vertical-align: top;">
                            <td style="width: 18.5%; text-align: left;">
                                *No. Escritura
                            </td>
                            <td style="width: 31.5%; text-align: left;">
                                <asp:TextBox ID="Txt_Numero_Escritura" runat="server" Style="width: 98%" TabIndex="11"
                                    MaxLength="20"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Numero_Escritura" runat="server" FilterType="Numbers"
                                    TargetControlID="Txt_Numero_Escritura">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="width: 18.5%; text-align: right;">
                                *Fecha escritura
                            </td>
                            <td style="width: 31.5%; text-align: left;">
                                <asp:TextBox ID="Txt_Fecha_Escritura" runat="server" Width="85%" TabIndex="12" MaxLength="11"
                                    Height="18px" />
                                <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Escritura" runat="server" TargetControlID="Txt_Fecha_Escritura"
                                    WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Escritura" runat="server" TargetControlID="Txt_Fecha_Escritura"
                                    Format="dd/MMM/yyyy" Enabled="True" PopupButtonID="Btn_Fecha_Escritura" />
                                <asp:ImageButton ID="Btn_Fecha_Escritura" runat="server" ImageUrl="../imagenes/paginas/SmallCalendar.gif"
                                    Style="vertical-align: top;" Height="18px" CausesValidation="false" OnClick="Btn_Fecha_Escritura_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                Documentos recibidos:
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="Grid_Documentos" runat="server" AutoGenerateColumns="False" CssClass="GridView_1"
                                    OnSelectedIndexChanged="Grid_Documentos_SelectedIndexChanged" Style="white-space: normal"
                                    Width="100%">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chk_Documento_Recibido" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CLAVE_DOCUMENTO" HeaderText="Clave" Visible="false" />
                                        <asp:BoundField DataField="NOMBRE_DOCUMENTO" HeaderText="Documento">
                                            <HeaderStyle HorizontalAlign="Left" Width="350px" />
                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Adjuntar archivo">
                                            <ItemTemplate>
                                                <%--<cc1:AsyncFileUpload ID="Fup_Documento_Recibido" runat="server" OnClientUploadComplete="UploadComplete"
                                                    OnUploadedComplete="Archivo_UploadComplete" Width="200px" ThrobberID="Lbl_Progress_File" />--%>
                                                <asp:Label ID="Lbl_Progress_File" runat="server" Text="Label">
                                                    <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server"
                                                        AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                                                        <ProgressTemplate>
                                                            <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                                                            </div>
                                                            <div class="processMessage" id="div_progress">
                                                                <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="200px" />
                                            <ItemStyle Width="200px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Archivo">
                                            <ItemTemplate>
                                                <asp:Label ID="Lbl_Nombre_Archivo" runat="server" Text=""></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="300px" />
                                            <ItemStyle Width="300px" />
                                        </asp:TemplateField>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/paginas/delete.png" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; vertical-align: top;">
                                <asp:Label ID="Lbl_Comentarios" runat="server" Text="Observaciones" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 85%; text-align: left;" colspan="3">
                                <asp:TextBox ID="Txt_Comentarios" MaxLength="250" runat="server" Style="text-transform: uppercase"
                                    TextMode="MultiLine" TabIndex="13" Rows="2" Width="98%"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios" runat="server" WatermarkText="Límite de 250 Caracteres"
                                    TargetControlID="Txt_Comentarios" WatermarkCssClass="watermarked" Enabled="True">
                                </cc1:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: right;">
                                <asp:ImageButton ID="Btn_Actualizar_Recepcion" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_aceptarplan.png"
                                    CssClass="Img_Button" ToolTip="Actualizar datos de trámite" AlternateText="Actualizar datos de trámite"
                                    TabIndex="14" OnClick="Btn_Actualizar_Recepcion_Click" Visible="false" />
                                <asp:ImageButton ID="Btn_Agregar_Recepcion" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                    CssClass="Img_Button" ToolTip="Agregar datos de trámite" TabIndex="15" OnClick="Btn_Agregar_Recepcion_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <asp:Image ID="Img_Error_Tamite" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                    Visible="false" />
                                <asp:Label ID="Lbl_Error_Tramite" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="Grid_Tramites" runat="server" Style="white-space: normal;" AutoGenerateColumns="False"
                                    OnSelectedIndexChanged="Grid_Tramites_SelectedIndexChanged" OnRowDataBound="Grid_Tramites_RowDataBound"
                                    Width="100%">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="7%" HorizontalAlign="Center" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta predial" />
                                        <asp:BoundField DataField="NO_ESCRITURA" HeaderText="No. Escritura" />
                                        <asp:BoundField DataField="FECHA_ESCRITURA" HeaderText="Fecha escritura" />
                                        <asp:BoundField DataField="COMENTARIOS" HeaderText="Observaciones" />
                                        <asp:BoundField DataField="NOMBRES_DOCUMENTOS" HeaderText="Documentos" />
                                        <asp:BoundField DataField="NOMBRES_ARCHIVO" HeaderText="Nombre archivo" Visible="false" />
                                        <asp:BoundField DataField="TIPOS_DOCUMENTO" HeaderText="Tipos documento" Visible="false" />
                                        <asp:BoundField DataField="CHECKSUM" HeaderText="Checksum" Visible="false" />
                                        <asp:BoundField DataField="NO_MOVIMIENTO" HeaderText="Checksum" Visible="false" />
                                        <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="Checksum" Visible="false" />
                                        <asp:TemplateField HeaderText="Quitar">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Quitar_Tramite" runat="server" ImageUrl="~/paginas/imagenes/paginas/delete.png"
                                                    CssClass="Img_Button" ToolTip="Quitar trámite" OnClick="Btn_Quitar_Tramite_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Label ID="Lbl_Numero_Movimiento" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <asp:Label ID="Lbl_Observaciones" runat="server" Text="" />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="Hdn_Recep_Docs" runat="server" />
                    <asp:TextBox ID="Txt_Bandera_Modifica_tramite_o_Recepcion" runat="server" Style="width: 98%"
                        MaxLength="20" Visible="false"></asp:TextBox>
                </asp:Panel>
                <%---------------------------------- Modal Popup Extender búsqueda de Notarios ----------------------------------%>
                <%--<cc1:ModalPopupExtender ID="Mpe_Busqueda_Notarios" runat="server" 
                    TargetControlID="Btn_Comodin_Open_Busqueda_Notarios"
                    PopupControlID="Pnl_Busqueda_Contenedor_Notarios" 
                    BackgroundCssClass="popUpStyle"
                    BehaviorID="Busqueda_Notatrios"
                    CancelControlID="Btn_Comodin_Close_Busqueda_Notarios" 
                    DropShadow="true" 
                    DynamicServicePath="" 
                    Enabled="True" />
                <asp:Button ID="Btn_Comodin_Close_Busqueda_Notarios" runat="server"
                    Style="background-color: transparent; border-style:none;display:none;"  Text="" />
                <asp:Button  ID="Btn_Comodin_Open_Busqueda_Notarios" runat="server"
                    Style="background-color:transparent; border-style:none;display:none;" Text="" />--%>
            </div>
        </ContentTemplate>
        <%--<Triggers>
            <asp:AsyncPostBackTrigger  ControlID="Btn_Aceptar" EventName="Click"/>            
        </Triggers>--%>
    </asp:UpdatePanel>
    <%----------------------------- Mpe búsqueda de Cuentas Predial -----------------------------%>
    <%--<asp:UpdatePanel ID="UPnl_Busqueda_Cuentas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
                <cc1:ModalPopupExtender ID="Mpe_Busqueda_Cuentas" runat="server" 
                    TargetControlID="Btn_Comodin_Open_Busqueda_Cuentas_Predial"
                    PopupControlID="Pnl_Contenedor_Busqueda_Cuentas" 
                    BackgroundCssClass="popUpStyle"
                    BehaviorID="Busqueda_Cuentas_Predial"
                    CancelControlID="Btn_Comodin_Close_Busqueda_Cuentas_Predial"
                    DropShadow="true" 
                    DynamicServicePath="" 
                    Enabled="True" />
                <asp:Button ID="Btn_Comodin_Close_Busqueda_Cuentas_Predial" runat="server"
                    Style="background-color: transparent; border-style:none;display:none;"  Text="" />
                <asp:Button  ID="Btn_Comodin_Open_Busqueda_Cuentas_Predial" runat="server"
                    Style="background-color:transparent; border-style:none;display:none;" Text="" />
        </ContentTemplate>  
    </asp:UpdatePanel>--%>
    <%---------------------------------- Panel búsqueda de Notarios ----------------------------------%>
    <%--<asp:Panel ID="Pnl_Busqueda_Contenedor_Notarios" runat="server" CssClass="drag" 
        style="display:none;width:650px;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">
            <asp:Panel ID="Pnl_Busqueda_Cabecera" runat="server" 
                style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                <table width="99%">
                    <tr>
                        <td style="color:Black; text-align:center; font-size:12; font-weight:bold;">
                            <asp:Image ID="Img_Icono_Busqueda_Notarios" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" />
                            B&uacute;squeda: Notarios
                        </td>
                        <td align="right" style="width:5%;">
                           <asp:ImageButton ID="Btn_Cerrar_Busqueda_Notarios" runat="server" 
                                style="cursor:pointer;" ToolTip="Cerrar Ventana"
                                ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" 
                                onclick="Btn_Cerrar_Busqueda_Notarios_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:UpdatePanel ID="Upd_Panel_Busqueda_Notarios" runat="server">
                <ContentTemplate>
                    <div style="color: #5D7B9D">
                     <table width="100%">
                        <tr>
                            <td align="left" style="text-align: left;" >

                                <table width="100%">
                                    <tr>
                                        <td style="width:100%" colspan="4" align="right">
                                            <asp:ImageButton ID="Btn_Limpiar_Ctlr_Busqueda" runat="server" OnClientClick="javascript:return Limpiar_Controles_Busqueda_Notario();"
                                                ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" ToolTip="Limpiar Controles de Busqueda"/>
                                        </td>
                                    </tr>     
                                    <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td style="width:12%;text-align:left;font-size:11px;">
                                           No Notario 
                                        </td>
                                        <td style="width:38%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Busqueda_No_Notario" runat="server" Width="98%" MaxLength="10" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Notario" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Busqueda_No_Notario"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_No_Notario" runat="server" 
                                                TargetControlID ="Txt_Busqueda_No_Notario" WatermarkText="Búsqueda por No Notario" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                        <td style="width:12%; text-align:right; font-size:11px;">
                                            Notaria
                                        </td>
                                        <td style="width:38%;text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Numero_Notaria" runat="server" Width="98%" MaxLength="10" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Numero_Notaria" runat="server" 
                                                FilterType="Numbers, LowercaseLetters, UppercaseLetters" 
                                                TargetControlID="Txt_Busqueda_Numero_Notaria"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Numero_Notaria" runat="server" 
                                                TargetControlID ="Txt_Busqueda_Numero_Notaria" WatermarkText="Búsqueda por No Notaria" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:12%;text-align:left;font-size:11px;">
                                            RFC
                                        </td>
                                        <td style="width:38%;text-align:left;font-size:11px;">
                                            <asp:TextBox ID="Txt_Busqueda_RFC" runat="server" Width="98%" MaxLength="15" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_RFC" runat="server" 
                                                FilterType="Numbers, UppercaseLetters, LowercaseLetters"
                                                TargetControlID="Txt_Busqueda_RFC"/>  
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_RFC" runat="server" 
                                                TargetControlID ="Txt_Busqueda_RFC" WatermarkText="Búsqueda por RFC" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                        <td style="width:12%; text-align:right; font-size:11px;">
                                            Estatus
                                        </td>
                                        <td style="width:38%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="100%">
                                                <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>
                                                <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                                <asp:ListItem Value="OBSOLETO">OBSOLETO</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:12%;text-align:left;font-size:11px;">
                                            Nombre
                                        </td>     
                                        <td style="width:88%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Busqueda_Nombre_Notario" runat="server" Width="99.5%" MaxLength="95" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Nombre_Notario" runat="server" 
                                                FilterType="Custom, LowercaseLetters, Numbers, UppercaseLetters" 
                                                TargetControlID="Txt_Busqueda_Nombre_Notario" ValidChars="áéíóúÁÉÍÓÚ "/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Nombre_Notario" runat="server" 
                                                TargetControlID ="Txt_Busqueda_Nombre_Notario" WatermarkText="Búsqueda por Nombre" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                    </tr>
                                   <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <center>
                                               <asp:Button ID="Btn_Busqueda_Notarios" runat="server"  Text="Buscar Notarios" CssClass="button"  
                                                CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Notarios_Click" /> 
                                            </center>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan="4">
                                        <center>
                                            <asp:GridView ID="Grid_Notarios" runat="server" AllowPaging="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                                OnPageIndexChanging="Grid_Notarios_PageIndexChanging" 
                                                OnSelectedIndexChanged="Grid_Notarios_SelectedIndexChanged" PageSize="5" 
                                                Width="98%">
                                                <RowStyle CssClass="GridItem" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle Width="5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="NOTARIO_ID" HeaderText="No.">
                                                        <ItemStyle Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RFC" HeaderText="RFC">
                                                        <ItemStyle Width="15%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NOMBRE_COMPLETO" HeaderText="Nombre">
                                                        <ItemStyle Width="65%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NUMERO_NOTARIA" HeaderText="Notaría" 
                                                        NullDisplayText="-">
                                                        <ItemStyle Width="5%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <HeaderStyle CssClass="GridHeader" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                            </asp:GridView>
                                            </center>
                                            <br />
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
        </asp:Panel>--%>
    <%----------------------------- Panel búsqueda de Cuentas Predial -----------------------------%>
    <%--<asp:Panel ID="Pnl_Contenedor_Busqueda_Cuentas" runat="server" CssClass="drag" 
        style="display:none;width:650px;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">
            <asp:Panel ID="Pnl_Cuentas_Cabacera" runat="server" 
                style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                <table width="99%">
                    <tr>
                        <td style="color:Black; text-align:center; font-size:12; font-weight:bold;">
                            <asp:Image ID="Img_Icono_Busqueda_Cuentas" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" />
                            B&uacute;squeda: Cuentas Predial
                        </td>
                        <td align="right" style="width:5%;">
                           <asp:ImageButton ID="Btn_Cerrar_Busqueda_Cuentas" runat="server" 
                                style="cursor:pointer;" ToolTip="Cerrar Ventana"
                                ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" 
                                onclick="Btn_Cerrar_Busqueda_Cuentas_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:UpdatePanel ID="Upd_Panel_Busqueda_Cuentas_Predial" runat="server">
                <ContentTemplate>
                    <div style="color: #5D7B9D">
                     <table width="100%">
                        <tr>
                            <td align="left" style="text-align: left;" >
                               
                                <table width="100%">
                                <tr class="barra_busqueda">
                        <td align="left" colspan="3">
                            <asp:ImageButton ID="Btn_Aceptar" runat="server" 
                                AlternateText="Aceptar" CssClass="Img_Button" 
                                ImageUrl="~/paginas/imagenes/paginas/accept.png" 
                                Width="24px" OnClick="Btn_Aceptar_Click" />
                        </td>
                        <td style="width:100%" align="right">
                                            <asp:ImageButton ID="Btn_Limpiar_Busqueda_Cuentas" runat="server" OnClientClick="javascript:return Limpiar_Controles_Busqueda_Cuentas();"
                                                ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" ToolTip="Limpiar Controles de Busqueda"/>
                                        </td>
                        </tr>
                                    
                                    <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td style="width:15%;text-align:left;font-size:11px;">
                                           Cuenta Predial 
                                        </td>
                                        <td style="width:35%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Busqueda_No_Cuenta" runat="server" Width="98%" MaxLength="20" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Cuenta" runat="server" 
                                                FilterType="Numbers, LowercaseLetters, UppercaseLetters"
                                                TargetControlID="Txt_Busqueda_No_Cuenta"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_No_Cuenta" runat="server" 
                                                TargetControlID ="Txt_Busqueda_No_Cuenta" WatermarkText="Búsqueda por Cuenta Predial" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                        <td style="width:15%; text-align:right; font-size:11px;">
                                            Estatus
                                        </td>
                                        <td style="width:35%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Estatus_Cuenta" runat="server" Width="100%">
                                                <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>
                                                <asp:ListItem Value="ACTIVA">ACTIVA</asp:ListItem>
                                                <asp:ListItem Value="INACTIVA">INACTIVA</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%;text-align:left;font-size:11px;">
                                            Propietario
                                        </td>     
                                        <td style="width:85%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Busqueda_Propietatio_Cuenta" runat="server" Width="99.5%" MaxLength="80" />
                                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Propietatio_Cuenta" runat="server" 
                                                FilterType="LowercaseLetters, UppercaseLetters, Custom" 
                                                ValidChars="ÑñáéíóúÁÉÍÓÚ "
                                                TargetControlID="Txt_Busqueda_Propietatio_Cuenta" />
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda_Propietatio_Cuenta" runat="server" 
                                                TargetControlID ="Txt_Busqueda_Propietatio_Cuenta" 
                                                WatermarkText="Búsqueda por Nombre de Propietario o copropietario" 
                                                WatermarkCssClass="watermarked"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%; text-align:left; font-size:11px;">
                                            
                                        </td>
                                        <td style="width:35%;text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Colonia_Cuenta" runat="server" Width="100%">
                                                <asp:ListItem Value="">&lt;- Seleccione -&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="Lse_Cmb_Busqueda_Colonia_Cuenta" runat="server" 
                                                TargetControlID="Cmb_Busqueda_Colonia_Cuenta" IsSorted="true" ></cc1:ListSearchExtender>
                                        </td>
                                        <td style="width:15%; text-align:right; font-size:11px;">
                                            
                                        </td>
                                        <td style="width:35%;text-align:right;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Calle_Cuenta" runat="server" Width="100%">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="Lse_Cmb_Busqueda_Calle_Cuenta" runat="server" 
                                                TargetControlID="Cmb_Busqueda_Calle_Cuenta" IsSorted="true" ></cc1:ListSearchExtender>
                                        </td>
                                    </tr>
                                   <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <center>
                                               <asp:Button ID="Btn_Busqueda_Cuenta" runat="server"  Text="Buscar Cuentas Predial" CssClass="button"  
                                                CausesValidation="false" Width="200px" OnClick="Btn_Busqueda_Cuentas_Predial_Click" /> 
                                            </center>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="Grid_Cuentas_Predial" runat="server" AllowPaging="True" PageSize="5" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                                OnPageIndexChanging="Grid_Cuentas_Predial_PageIndexChanging" 
                                                OnSelectedIndexChanged="Grid_Cuentas_Predial_SelectedIndexChanged" 
                                                Width="98%" >
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle Width="5%" /></asp:ButtonField>
                                                    <asp:BoundField DataField="CUENTA_PREDIAL_ID" Visible="false" />
                                                    <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta" >
                                                        <ItemStyle Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NOMBRE_PROPIETARIO" HeaderText="Propietario" >
                                                        <ItemStyle Width="45%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DOMICILIO" HeaderText="Domicilio"  >
                                                        <ItemStyle Width="25%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <HeaderStyle CssClass="GridHeader" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                            </asp:GridView>
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
        </asp:Panel>--%>
</asp:Content>

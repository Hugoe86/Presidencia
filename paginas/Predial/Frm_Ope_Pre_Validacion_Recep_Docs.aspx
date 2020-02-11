<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Validacion_Recep_Docs.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Validacion_Recep_Docs"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/paginas/Paginas_Generales/Pager.ascx" TagPrefix="custom" TagName="Pager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .Tabla_Comentarios
        {
            border-collapse: collapse;
            margin-left: 25px;
            color: #25406D;
            font-family: Verdana,Geneva,MS Sans Serif;
            font-size: small;
            text-align: left;
        }
        .Tabla_Comentarios, .Tabla_Comentarios th, .Tabla_Comentarios td
        {
            border: 1px solid #999999;
            padding: 2px 10px;
        }
    </style>
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

        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }
        function Limpiar_Controles() {
            document.getElementById("<%=Lbl_Mensaje_Error.ClientID%>").innerHTML = "";
            if (document.getElementById("<%=Img_Error.ClientID%>") != null) {
                document.getElementById("<%=Img_Error.ClientID%>").style.visibility = "hidden";
            }
            document.getElementById("<%=Txt_Cuenta_Predial.ClientID%>").value = "";
            document.getElementById("<%=Txt_No_Escritura.ClientID%>").value = "";
            document.getElementById("<%=Txt_Fecha_Escritura.ClientID%>").value = "";
            document.getElementById("<%=Txt_Folio_Recepcion.ClientID%>").value = "";
            document.getElementById("<%=Txt_Comentarios_Area.ClientID%>").value = "";
            document.getElementById("<%=Txt_Busqueda_Recep_Docs.ClientID%>").value = "";
            document.getElementById("<%=Cmb_Busqueda_Por.ClientID%>").value = "";
            document.getElementById("<%=Cmb_Estatus.ClientID%>").value = "";

            document.getElementById("<%=Datos_Movimiento.ClientID%>").style.visibility = "hidden";


            return false;
        }
    </script>

    <script type="text/javascript" language="javascript">
        window.onerror = new Function("return true");
        //Metodos para limpiar los controles de la busqueda.    
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }
        function Abrir_Resumen(Url, Propiedades) {
            window.open(Url, 'Resumen_Predio', Propiedades);
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente" cellpadding="0" cellspacing="0">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">
                            Validación de recepción de documentos
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
                    <tr class="barra_busqueda">
                        <td align="left" style="width: 30%;">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" CssClass="Img_Button" TabIndex="1"
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" CssClass="Img_Button" TabIndex="2"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                        </td>
                        <td style="text-align: right; width: 70%">
                            Búsqueda por
                            <asp:DropDownList ID="Cmb_Busqueda_Por" runat="server" Width="150px" TabIndex="4">
                                <asp:ListItem Value="0"><-Seleccione-></asp:ListItem>
                                <asp:ListItem Value="FOLIO">FOLIO</asp:ListItem>
                                <asp:ListItem Value="NOTARIO">NOTARIO</asp:ListItem>
                                <asp:ListItem Value="NOTARIA">NOTAR&Iacute;A</asp:ListItem>
                                <asp:ListItem Value="CUENTA">CUENTA</asp:ListItem>
                                <asp:ListItem Value="ESCRITURA">ESCRITURA</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;
                            <asp:TextBox ID="Txt_Busqueda_Recep_Docs" runat="server" Width="160px" MaxLength="12"
                                TabIndex="5"> </asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Recep_Docs" runat="server" WatermarkText="<- Recepción de documento ->"
                                TargetControlID="Txt_Busqueda_Recep_Docs" WatermarkCssClass="watermarked">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Recep_Docs" runat="server" TargetControlID="Txt_Busqueda_Recep_Docs"
                                InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                            <asp:ImageButton ID="Btn_Buscar_Recepciones_Notario" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" TabIndex="6"
                                OnClick="Btn_Buscar_Recepciones_Notario_Click" />
                        </td>
                    </tr>
                </table>
                <%---------------------------------- Datos Notario ----------------------------------%>
                <div id="Div_Notarios" runat="server">
                    <br />
                    <table width="100%" class="estilo_fuente">
                        <tr>
                            <td style="width: 18%; text-align: left;">
                                <asp:Label ID="Lbl_Notario_ID" runat="server" Text="No. Notaria" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 82%; text-align: left;" colspan="3">
                                <asp:TextBox ID="Txt_Notario_ID" runat="server" Width="37.7%" Enabled="False" TabIndex="7"
                                    MaxLength="10"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Notario_ID" runat="server" TargetControlID="Txt_Notario_ID"
                                    FilterType="Numbers" />
                                &nbsp; B&uacute;squeda
                                <asp:ImageButton ID="Btn_Mostrar_Busqueda_Notario" runat="server" ToolTip="Búsqueda Avanzada de Notarios"
                                    TabIndex="8" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px"
                                    Width="24px" CausesValidation="true" OnClick="Btn_Mostrar_Busqueda_Notario_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 18%; text-align: left;">
                                <asp:Label ID="Lnl_RFC_Notario" runat="server" Text="RFC Notario" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 32%; text-align: left;">
                                <asp:TextBox ID="Txt_RFC_Notario" runat="server" Width="98%" Enabled="False" TabIndex="9"
                                    MaxLength="10"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_RFC_Notario" runat="server" TargetControlID="Txt_RFC_Notario"
                                    FilterType="UppercaseLetters, LowercaseLetters, Numbers" />
                            </td>
                            <td style="width: 15%; text-align: right;">
                                <asp:Label ID="Lbl_Numero_Notaria" runat="server" Text="No. Notario" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 35%; text-align: left;">
                                <asp:TextBox ID="Txt_Numero_Notaria" runat="server" Width="92.7%" Enabled="False"></asp:TextBox>
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
                    </table>
                    <hr />
                </div>
                <table width="100%" class="estilo_fuente">
                    <%--<tr>
                            <td colspan="4">
                                <br />
                                <asp:GridView ID="Grid_Recepciones_Notario" runat="server" 
                                    style="white-space:normal;" AutoGenerateColumns="False" 
                                    CssClass="GridView_1" PageSize="10" AllowPaging="true"
                                    OnSelectedIndexChanged="Grid_Recepciones_Notario_SelectedIndexChanged"
                                    OnPageIndexChanging="Grid_Recepciones_Notario_PageIndexChanging" >
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
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
                                    <asp:BoundField DataField="TOTAL_PENDIENTES" HeaderText="Pendientes" >
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemStyle HorizontalAlign="Center" Width="8%" />
                                    </asp:BoundField>
                                     <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>                                    
                                    <asp:BoundField DataField="NOTARIO_ID" HeaderText="ID_Notario" Visible="false" />
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                </asp:GridView>
                            </td>
                        </tr>--%>
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
                        </td>
                    </tr>
                </table>
                <br />
                <div id="Div_Bandeja" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <tr align="center">
                            <td>
                                <asp:GridView ID="Grid_Bandeja_Recepcion" runat="server" AllowPaging="True" CssClass="GridView_1"
                                    AutoGenerateColumns="False" PageSize="10" Width="100%" AllowSorting="True" HeaderStyle-CssClass="tblHead"
                                    Style="white-space: normal;" OnSelectedIndexChanged="Grid_Bandeja_Recepcion_SelectedIndexChanged"
                                    OnPageIndexChanging="Grid_Bandeja_Recepcion_PageIndexChanging" OnSorting="Grid_Bandeja_Recepcion_Sorting">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="3.5%" HorizontalAlign="Center" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="NO_RECEPCION_DOCUMENTO" HeaderText="Folio de Recepción"
                                            HeaderStyle-Width="15%" SortExpression="NO_RECEPCION_DOCUMENTO" />
                                        <asp:BoundField DataField="NO_MOVIMIENTO" HeaderText="Movimiento" HeaderStyle-Width="15%"
                                            SortExpression="NO_MOVIMIENTO" />
                                        <asp:BoundField DataField="ASIGNADO" HeaderText="Asignado" HeaderStyle-Width="30%"
                                            Visible="true" />
                                        <asp:BoundField DataField="FECHA" HeaderText="Fecha" SortExpression="FECHA" DataFormatString="{0:dd/MMM/yy}" />
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS" />
                                        <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta" SortExpression="CUENTA_PREDIAL">
                                            <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                            <ItemStyle HorizontalAlign="Center" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NUMERO_ESCRITURA" HeaderText="Escritura" SortExpression="NUMERO_ESCRITURA">
                                            <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                            <ItemStyle HorizontalAlign="Center" Width="12%" />
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
                </div>
                <div id="Datos_Movimiento" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Cuenta Predial
                            </td>
                            <td style="width: 82%; text-align: left;" colspan="3">
                                <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="35.7%" AutoPostBack="true"
                                    TabIndex="11" MaxLength="20"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Cuenta_Predial" runat="server" TargetControlID="Txt_Cuenta_Predial"
                                    FilterType="UppercaseLetters, LowercaseLetters, Numbers" />
                                &nbsp;
                                <asp:ImageButton ID="Btn_Detalles_Cuenta_Predial" runat="server" ToolTip="Resumen de predio"
                                    TabIndex="12" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Height="24px"
                                    Width="24px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                No. Escritura
                            </td>
                            <td style="text-align: left; width: 82%;">
                                <asp:TextBox ID="Txt_No_Escritura" runat="server" Width="96.4%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%">
                                Fecha escritura
                            </td>
                            <td style="width: 80%; text-align: left;">
                                <asp:TextBox ID="Txt_Fecha_Escritura" runat="server" Width="96.4%" TabIndex="13"
                                    MaxLength="11" />
                            </td>
                        </tr>
                    </tabla>
                </div>
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Folio recepción
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Folio_Recepcion" runat="server" Width="96.4%" />
                        </td>
                        <td style="text-align: left; width: 20%; text-align: right;">
                            Estatus
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%" TabIndex="14">
                                <asp:ListItem Value="0"><- SELECCIONE -></asp:ListItem>
                                <asp:ListItem Value="PENDIENTE">PENDIENTE</asp:ListItem>
                                <asp:ListItem Value="ACEPTADO">ACEPTADO</asp:ListItem>
                                <asp:ListItem Value="RECHAZADO">RECHAZADO</asp:ListItem>
                                <asp:ListItem Value="CANCELADO">CANCELADO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            *Observaciones
                        </td>
                        <td colspan="3" style="text-align: left; width: 80%;">
                            <asp:TextBox ID="Txt_Comentarios_Area" runat="server" TabIndex="15" MaxLength="250"
                                Style="text-transform: uppercase" TextMode="MultiLine" Width="98.6%" Height="60px" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Area" runat="server" TargetControlID="Txt_Comentarios_Area"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                        </td>
                    </tr>
                </table>
                <br />
                <div style="color: #25406D;">
                    Documentos recibidos:</div>
                <table id="Tbl_Documentos_Recibidos" runat="server" width="94%" border="1" cellspacing="0"
                    class="Tabla_Comentarios">
                </table>
                <br />
                <div style="color: #25406D;">
                    Historial de observaciones:</div>
                <table id="Tbl_Comentarios" runat="server" width="94%" border="1" cellspacing="0"
                    class="Tabla_Comentarios">
                </table>
            </div>
            <br />
            <asp:HiddenField ID="Hdn_Movimiento_ID" runat="server" />
            </div>
            <%------------------------ Documentos ---------------------------%>
            <asp:Panel ID="Pnl_Contenedor_Datos_Recepcion_Documentos" runat="server" Style="display: none;">
                <table style="width: 98%;" class="estilo_fuente" id="Tbl_Documentos_Controles">
                    <tr>
                        <td style="width: 18%; text-align: left;">
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                            <asp:HiddenField ID="HiddenField2" runat="server" />
                            <asp:Label ID="Lbl_Cuenta_Predial" runat="server" Text="Cuenta Predial" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width: 82%; text-align: left;">
                            <asp:TextBox ID="Txt_Cuenta_Predial_1" runat="server" Width="37.7%" AutoPostBack="true"
                                Enabled="false" TabIndex="16" MaxLength="20" OnTextChanged="Txt_Cuenta_Predial_TextChanged"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Cuenta_Predial"
                                FilterType="UppercaseLetters, LowercaseLetters, Numbers" />
                            &nbsp; B&uacute;squeda
                            <asp:ImageButton ID="Btn_Mostrar_Busqueda_Cuentas" runat="server" ToolTip="Búsqueda Avanzada de Cuenta Predial"
                                TabIndex="17" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="24px"
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
                <table style="width: 98%;" class="estilo_fuente" id="Table1">
                    <tr style="height: 40px; vertical-align: top;">
                        <td style="width: 18.5%; text-align: left;">
                            *No. Escritura
                        </td>
                        <td style="width: 31.5%; text-align: left;">
                            <asp:TextBox ID="Txt_Numero_Escritura" runat="server" Style="width: 98%" TabIndex="18"
                                MaxLength="20"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Numero_Escritura" runat="server" FilterType="Numbers"
                                TargetControlID="Txt_Numero_Escritura">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="width: 18.5%; text-align: left;">
                            *Fecha escritura
                        </td>
                        <td style="width: 31.5%; text-align: left;">
                            <asp:TextBox ID="Txt_Fecha_Escritura_Docs" runat="server" Width="85%" TabIndex="19"
                                MaxLength="11" Height="18px" />
                            <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Escritura" runat="server" TargetControlID="Txt_Fecha_Escritura_Docs"
                                WatermarkCssClass="watermarked" WatermarkText="Dia/Mes/Año" Enabled="True" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Escritura" runat="server" TargetControlID="Txt_Fecha_Escritura_Docs"
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
                                    <%--<asp:TemplateField HeaderText="Adjuntar archivo">
                                        <ItemTemplate>
                                            <cc1:AsyncFileUpload ID="Fup_Documento_Recibido" runat="server" OnUploadedComplete="Archivo_UploadComplete"
                                                Width="200px"/>
                                            <%--<asp:Label ID="Lbl_Progress_File" runat="server" Text="Label">                                                    
                                                    <ProgressTemplate>
                                                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                                                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                                                </ProgressTemplate>   
                                            </asp:Label>--%>
                                    <%--</ItemTemplate>
                                        <HeaderStyle Width="200px" />
                                        <ItemStyle Width="200px" />
                                    </asp:TemplateField>--%>
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
                            <asp:TextBox ID="Txt_Comentarios" MaxLength="250" runat="server" TextMode="MultiLine"
                                TabIndex="20" Rows="2" Width="98%"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios" runat="server" WatermarkText="Límite de 250 Caracteres"
                                TargetControlID="Txt_Comentarios" WatermarkCssClass="watermarked" Enabled="True">
                            </cc1:TextBoxWatermarkExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: right;">
                            <asp:ImageButton ID="Btn_Actualizar_Recepcion" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_aceptarplan.png"
                                CssClass="Img_Button" ToolTip="Actualizar datos de trámite" AlternateText="Actualizar datos de trámite"
                                TabIndex="21" OnClick="Btn_Actualizar_Recepcion_Click" Visible="false" />
                            <asp:ImageButton ID="Btn_Agregar_Recepcion" runat="server" ImageUrl="~/paginas/imagenes/paginas/accept.png"
                                CssClass="Img_Button" ToolTip="Agregar datos de trámite" TabIndex="22" OnClick="Btn_Agregar_Recepcion_Click" />
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
            <asp:HiddenField ID="Hdn_Cuenta_Predial_ID" runat="server" />
            <asp:HiddenField ID="Hdn_No_Movimiento" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

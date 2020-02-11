<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Ort_Administracion_Urbana.aspx.cs" Inherits="paginas_Ordenamiento_Territorial_Frm_Ope_Ort_Administracion_Urbana" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script language="javascript" type="text/javascript">
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
   </script>

    <script type="text/javascript" language="javascript">

        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }

        //  para buscar que el porcentaje no se pase del 100%
        function Banos_Hombres(ctrl) {
            var Valor_Bano_Hombres = 0;
            var Valor_Bano_Mujeres = 0;
            var Suma;

            if ($("input[id$=Txt_Bano_Hombres]").val().length <= 0) {
                Valor_Bano_Hombres = 0;
                $('input[id$=Txt_Bano_Hombres]').val('0');
            }
            else {
                Valor_Bano_Hombres = parseFloat(ctrl.value);
            }



            if (document.getElementById("<%=Txt_Bano_Mujeres.ClientID%>").value == "") {
                Valor_Bano_Mujeres = 0;
            }
            else {
                Valor_Bano_Mujeres = parseFloat(document.getElementById("<%=Txt_Bano_Mujeres.ClientID%>").value);
            }

            Suma = Valor_Bano_Hombres + Valor_Bano_Mujeres;
            $('input[id$=Txt_Total_Banos]').val(Suma);
        }

        function Banos_Mujeres(ctrl) {
            var Valor_Bano_Mujeres = 0;
            var Valor_Bano_Hombres = 0;
            var Suma;

            if ($("input[id$=Txt_Bano_Mujeres]").val().length <= 0) {
                Valor_Bano_Mujeres = 0;
                $('input[id$=Txt_Bano_Mujeres]').val('0');
            }
            else {
                Valor_Bano_Mujeres = parseFloat(ctrl.value);
            }

            if (document.getElementById("<%=Txt_Bano_Hombres.ClientID%>").value == "") {
                Valor_Bano_Hombres = 0;
            }
            else {
                Valor_Bano_Hombres = parseFloat(document.getElementById("<%=Txt_Bano_Hombres.ClientID%>").value);
            }

            Suma = Valor_Bano_Hombres + Valor_Bano_Mujeres;
            $('input[id$=Txt_Total_Banos]').val(Suma);
        }


        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }

        function Abrir_Resumen(Url, Propiedades) {
            window.open(Url, 'Resumen_Predio', Propiedades);
        }

        function imposeMaxLength(Object, MaxLen) {
            return (Object.value.length <= MaxLen);
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server"  AsyncPostBackTimeout="3600"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <table width="100%" border="0" cellspacing="0" class="estilo_fuente" frame="border">
                <tr align="center">
                    <td class="label_titulo">
                        Cedula de Visita (Unificada)
                    </td>
                </tr>
                <tr>
                    <!--Bloque del mensaje de error-->
                    <td>
                        <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                            Visible="false" />&nbsp;
                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="98%" border="0" cellspacing="0" class="estilo_fuente" frame="border">
                <tr class="barra_busqueda" align="right">
                    <td align="left" valign="middle">
                        <div>
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                CssClass="Img_Button" OnClick="Btn_Nuevo_Click" ToolTip="Nuevo" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Modificar" ToolTip="Modificar Ficha"
                                OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" OnClick="Btn_Salir_Click" CssClass="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" />
                            <%--  <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                        CssClass="Img_Button" onclick="Btn_Eliminar_Click"  
                                        OnClientClick="return confirm('Desea eliminar los emplados relacionados con el perfil. ¿Desea continuar?');"
                                        AlternateText="Eliminar" ToolTip="Eliminar"/>
                                    
                                </div>--%>
                    </td>
                    <%-- <td colspan="2">Búsqueda
                                <asp:TextBox ID="Txt_Busqueda" runat="server"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Click" />
                            </td>--%>
                </tr>
            </table>
            <div id="Div_Lista_Formatos" runat="server" style="display: block">
                <table width="98%">
                    <tr>
                        <td style="width: 100%">
                            <center>
                                <asp:Panel ID="Pnl_Captura_Cedula" runat="server" GroupingText="Llenado de la cedula"
                                    Style="display: block">
                                    <div id="Div_Grid_Formatos" runat="server" style="overflow: auto; height: 150px;
                                        width: 99%; vertical-align: top; border-style: solid; border-color: Silver; display: block">
                                        <asp:GridView ID="Grid_Formatos" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                                            Width="97%" EmptyDataText="No se encuentra ninguna solicitud en espera" OnRowDataBound="Grid_Formatos_RowDataBound"
                                            GridLines="None">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <%-- 0 --%>
                                                <asp:TemplateField HeaderText="Capturar" HeaderStyle-Font-Size="13px" ItemStyle-Font-Size="12px">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:ImageButton ID="Btn_Autorizar_Formato" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                                                Width="24px" Height="24px" OnClick="Btn_Autorizar_Formato_Click" CausesValidation="false" />
                                                        </center>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="center" Width="7%" />
                                                    <ItemStyle HorizontalAlign="center" Width="7%" />
                                                </asp:TemplateField>
                                                <%-- 1 --%>
                                                <asp:BoundField DataField="SOLICITUD_ID" HeaderText="SOLICITUD_ID ID" SortExpression="SOLICITUD_ID" />
                                                <%-- 2 --%>
                                                <asp:BoundField DataField="TRAMITE_ID" HeaderText="TRAMITE_ID" SortExpression="TRAMITE_ID" />
                                                <%-- 3 --%>
                                                <asp:BoundField DataField="SUBPROCESO_ID" HeaderText="SUBPROCESO_ID ID" SortExpression="SUBPROCESO_ID" />
                                                <%-- 4 --%>
                                                <asp:BoundField DataField="NOMBRE_TRAMITE" HeaderText="Tramite" SortExpression="NOMBRE_TRAMITE">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="40%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="40%" />
                                                </asp:BoundField>
                                                <%-- 5 --%>
                                                <asp:BoundField DataField="NOMBRE_ACTIVIDAD" HeaderText="Actividad" SortExpression="NOMBRE_ACTIVIDAD">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="40%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="40%" />
                                                </asp:BoundField>
                                                <%-- 6 --%>
                                                <asp:BoundField DataField="CONSECUTIVO" HeaderText="Consecutivo" SortExpression="CONSECUTIVO">
                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" Width="13%" />
                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" Width="13%" />
                                                </asp:BoundField>
                                            </Columns>
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                            <asp:Panel ID="Pnl_Modificar_Cedula" runat="server" GroupingText="Modificacion Cedulas existentes"
                                Style="display: block">
                                <div id="Div_Modificar_Cedula" runat="server" style="overflow: auto; height: 150px;
                                    width: 99%; vertical-align: top; border-style: solid; border-color: Silver; display: block">
                                    <asp:GridView ID="Grid_Listado" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                                        Width="97%" Style="display: block" GridLines="None" EmptyDataText="No hay registros."
                                        OnSelectedIndexChanged="Grid_Listado_SelectedIndexChanged">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="ADMINISTRACION_URBANA_ID" HeaderText="ADMINISTRACION_URBANA_ID"
                                                SortExpression="ADMINISTRACION_URBANA_ID" NullDisplayText="-">
                                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TRAMITE_ID" HeaderText="TRAMITE_ID" SortExpression="TRAMITE_ID"
                                                NullDisplayText="-">
                                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SOLICITUD_ID" HeaderText="SOLICITUD_ID" SortExpression="SOLICITUD_ID"
                                                NullDisplayText="-">
                                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SUBPROCESO_ID" HeaderText="SUBPROCESO_ID" SortExpression="SUBPROCESO_ID"
                                                NullDisplayText="-">
                                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CLAVE_TRAMITE" HeaderText="Clave del Tramite" SortExpression="CLAVE_TRAMITE"
                                                NullDisplayText="-">
                                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CLAVE_SOLICITUD" HeaderText="Clave de Solicitud" SortExpression="CLAVE_SOLICITUD"
                                                NullDisplayText="-">
                                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CONSECUTIVO" HeaderText="Consecutivo" SortExpression="CONSECUTIVO"
                                                NullDisplayText="-">
                                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NOMBRE" HeaderText="Subproceso" SortExpression="NOMBRE"
                                                NullDisplayText="-">
                                                <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                            </center>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="Div_Principal_Llenado_Formato" runat="server" style="display: block">
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="98%" ActiveTabIndex="0">
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel_Area" Width="100%">
                        <HeaderTemplate>
                            AREA</HeaderTemplate>
                        <ContentTemplate>
                            <div id="Div_Area_Inspeccion" runat="server" style="display: block">
                                <asp:Panel ID="Pnl_Area_Inspeccion" runat="server" GroupingText="Datos a llenar">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                <%--<b>Area de inspenccion</b>--%>
                                            </td>
                                            <td style="width: 35%">
                                                <asp:RadioButtonList ID="RBtn_Area_Inspeccion" runat="server" TabIndex="1" Visible="false">
                                                    <asp:ListItem>URBANISTICO</asp:ListItem>
                                                    <asp:ListItem>INMOBILIARIO</asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:HiddenField ID="Hdf_Administracion_Urbana_ID" runat="server" />
                                                <asp:HiddenField ID="Hdf_Tramite_ID" runat="server" />
                                                <asp:HiddenField ID="Hdf_Solicitud_ID" runat="server" />
                                                <asp:HiddenField ID="Hdf_Subproceso_ID" runat="server" />
                                                <asp:HiddenField ID="Hdf_Redireccionar" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                <asp:Label ID="Lbl_Evaluacion" runat="server" Text="Evaluar" Font-Bold="True"></asp:Label>
                                            </td>
                                            <td style="width: 35%">
                                                <asp:DropDownList ID="Cmb_Evaluacion" runat="server" Width="96%">
                                                    <asp:ListItem Text="ACTUALIZAR DATOS CEDULA DE VISITA" Value="ACTUALIZAR" />
                                                    <asp:ListItem Text="APROBAR CEDULA DE VISITA" Value="APROBAR" Selected="True" />
                                                    <asp:ListItem Text="REGRESAR A LA ACTIVIDAD ANTERIOR" Value="REGRESAR" />
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                <b>Uso solicitado o destinado</b>
                                            </td>
                                            <td style="width: 35%" colspan="2">
                                                <asp:TextBox ID="Txt_Destinado" runat="server" Enabled="False" Width="88%" TabIndex="8"
                                                    MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="Grid_Datos_Dictamen" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                        CssClass="GridView_1" GridLines="None" Width="97%">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <%-- <asp:BoundField DataField="OPE_DATO_ID" HeaderText="OPE_DATO_ID" SortExpression="OPE_DATO_ID" />
                                            <asp:BoundField DataField="DATO_ID" HeaderText="DATO_ID" SortExpression="DATO_ID" />--%>
                                            <asp:BoundField DataField="Nombre" HeaderText="Datos">
                                                <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                                <ItemStyle HorizontalAlign="Left" Width="25%" Wrap="True" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Descripción">
                                                <HeaderStyle HorizontalAlign="Left" Width="75%" />
                                                <ItemStyle HorizontalAlign="Left" Width="75%" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Txt_Descripcion_Datos" runat="server" Width="90%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <HeaderStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                    <asp:GridView ID="Grid_Datos_Dictamen_Modificar" runat="server" CssClass="GridView_1"
                                        AutoGenerateColumns="False" Width="97%" GridLines="None" EmptyDataText="No hay datos">
                                        <RowStyle CssClass="GridItem" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <Columns>
                                            <asp:BoundField DataField="OPE_DATO_ID" HeaderText="OPE_DATO_ID" SortExpression="OPE_DATO_ID" />
                                            <asp:BoundField DataField="DATO_ID" HeaderText="DATO_ID" SortExpression="DATO_ID" />
                                            <asp:BoundField DataField="NOMBRE_DATO" HeaderText="Dato a dictaminar" SortExpression="NOMBRE_DATO">
                                                <ItemStyle HorizontalAlign="Left" Font-Size="12px" Width="60%" />
                                                <HeaderStyle HorizontalAlign="Left" Font-Size="13px" Width="60%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Valor">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Txt_Valor_Dato" runat="server" Width="95%"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="40%" Font-Size="12px" />
                                                <HeaderStyle HorizontalAlign="Left" Width="40%" Font-Size="13px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="GridHeader" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <SelectedRowStyle CssClass="GridSelected" />
                                    </asp:GridView>
                                </asp:Panel>
                                <table class="estilo_fuente" width="100%">
                                    <tr>
                                        <td style="width: 15%">
                                            <asp:Label ID="Lbl_Cuenta_Predial" runat="server" Text="Cuenta Predial" />
                                        </td>
                                        <td style="width: 85%">
                                            <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="38%" Enabled="False"></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Buscar_Cuenta_Predial" runat="server" ToolTip="Resumen de predio"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Height="22px" Width="22px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%" align="left">
                                            * Tipo de supervision
                                        </td>
                                        <td style="width: 85%" align="left">
                                            <asp:DropDownList ID="Cmb_Tipo_Supervision" runat="server" Width="96%">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%" align="left">
                                            Inspector
                                        </td>
                                        <td style="width: 85%" align="left">
                                            <asp:DropDownList ID="Cmb_Inspector" runat="server" Width="96%">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table id="Tabla_Colonia" runat="server" class="estilo_fuente" width="100%" style="display: none">
                                    <tr runat="server">
                                        <td style="width: 15%" runat="server">
                                            Colonia, Fraccionamiento O Ejido
                                        </td>
                                        <td style="width: 85%" runat="server">
                                            <asp:DropDownList ID="Cmb_Colonias" runat="server" Width="95%" TabIndex="2" AutoPostBack="True"
                                                DropDownStyle="DropDownList" AutoCompleteMode="SuggestAppend" CaseSensitive="False"
                                                CssClass="WindowsStyle" OnSelectedIndexChanged="Cmb_Colonias_OnSelectedIndexChanged" />
                                            <asp:ImageButton ID="Btn_Buscar_Colonia" runat="server" ToolTip="Seleccionar Colonia"
                                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                                OnClick="Btn_Buscar_Colonia_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <table id="Tabla_Calle" runat="server" class="estilo_fuente" width="100%" style="display: none">
                                    <tr runat="server">
                                        <td style="width: 15%" runat="server">
                                            Calle
                                        </td>
                                        <td style="width: 85%" runat="server">
                                            <asp:DropDownList ID="Cmb_Calle" runat="server" Width="95%" TabIndex="3" DropDownStyle="DropDown"
                                                AutoCompleteMode="SuggestAppend" CaseSensitive="False" CssClass="WindowsStyle" />
                                            <asp:ImageButton ID="Btn_Buscar_Calle" runat="server" ToolTip="Seleccionar Calle"
                                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                                OnClick="Btn_Buscar_Calles_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <table id="Tabla_Datos_Calle" runat="server" class="estilo_fuente" width="100%" style="display: none">
                                    <tr runat="server">
                                        <td style="width: 15%" align="left" runat="server">
                                            No Fisico
                                        </td>
                                        <td style="width: 15%" runat="server">
                                            <asp:TextBox ID="Txt_Numero_Fisico" runat="server" Enabled="False" Width="95%" TabIndex="4"
                                                Style="text-align: right" MaxLength="10"></asp:TextBox>
                                            <%--  <cc1:FilteredTextBoxExtender ID="Fte_Txt_Numero_Fisico" runat="server" FilterType="Numbers"
                                                TargetControlID="Txt_Numero_Fisico" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td style="width: 15%" align="right" runat="server">
                                            Lote
                                        </td>
                                        <td style="width: 15%" runat="server">
                                            <asp:TextBox ID="Txt_Lote" runat="server" Enabled="False" Width="90%" TabIndex="5"
                                                MaxLength="50"></asp:TextBox>
                                        </td>
                                        <td style="width: 15%" align="right" runat="server">
                                            Manazana
                                        </td>
                                        <td style="width: 15%" runat="server">
                                            <asp:TextBox ID="Txt_Manzana" runat="server" Enabled="False" Width="90%" TabIndex="6"
                                                MaxLength="50"></asp:TextBox>
                                        </td>
                                        <td style="width: 10%" align="right" runat="server">
                                        </td>
                                    </tr>
                                </table>
                                <table class="estilo_fuente" width="100%">
                                    <tr>
                                        <td style="width: 15%">
                                            Zona
                                        </td>
                                        <td style="width: 35%">
                                            <asp:DropDownList ID="Cmb_Zona" runat="server" Width="95%" TabIndex="7">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 15%" align="right">
                                            Consecutivo No.
                                        </td>
                                        <td style="width: 35%">
                                            <asp:TextBox ID="Txt_Consecutivo_ID" runat="server" Enabled="False" Width="89%"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="Pnl_Datos_Solicitud" runat="server" GroupingText="Detalles de Solicitud"
                                    Style="">
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%">
                                                <asp:Label ID="Lbl_Nombre" runat="server" Text="Nombre Solicitante"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Txt_Nombre" runat="server" Enabled="False" Width="93%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                <asp:Label ID="Lbl_Estatus" runat="server" Text="Estado"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Txt_Estatus" runat="server" Enabled="False" Width="90%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%">
                                                <asp:Label ID="Lbl_Proceso" runat="server" Text="Proceso"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Txt_Proceso" runat="server" Enabled="False" Width="93%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                <asp:Label ID="Lbl_Avance" runat="server" Text="Avance"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Txt_Avance" runat="server" Enabled="False" Width="90%"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel_Condiciones" Width="100%">
                        <HeaderTemplate>
                            CONDICIONES</HeaderTemplate>
                        <ContentTemplate>
                            <div id="Div_Condiciones_Inmueble" runat="server" style="display: block">
                                <asp:Panel ID="Pnl_Condiciones_Inmueble" runat="server" GroupingText="CONDICIONES DEL INMUEBLE"
                                    ForeColor="Blue">
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Tipo
                                            </td>
                                            <td style="width: 85%" align="left">
                                                <asp:DropDownList ID="Cmb_Condiciones_Inmueble" runat="server" Width="95%">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <br />
                            <br />
                            <div id="Div_Respecto_Via_Publcia_Areas_Donacion" runat="server" style="display: block">
                                <asp:Panel ID="Pnl_Respecto_Via_Publcia_Areas_Donacion" runat="server" GroupingText="RESPECTO A LA VIA PUBLICA Y AREAS DE DONACION"
                                    ForeColor="Blue">
                                    <table class="estilo_fuente" width="100%">
                                        <tr align="center">
                                            <td style="width: 25%" align="center">
                                                <asp:CheckBox ID="Chk_Invasion_Material" runat="server" Text="Invasion con Material" />
                                            </td>
                                            <td style="width: 25%" align="center">
                                                <asp:CheckBox ID="Chk_Invasion_Areas_donacion" runat="server" Text="Invasion de Áreas de Donación" />
                                            </td>
                                            <td style="width: 25%" align="center">
                                                <asp:CheckBox ID="Chk_Construccion_Marquesina" runat="server" Text="Construcción Sobre Marquesina" />
                                            </td>
                                            <td style="width: 25%" align="center">
                                                <asp:CheckBox ID="Chk_Sobresale_Paramento" runat="server" Text="Sobresale del Paramento" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 30%" align="left">
                                                Especificar si se debe considerar alguna restriccion
                                            </td>
                                            <td style="width: 70%" align="left">
                                                <asp:TextBox ID="Txt_Especificacion_Restriccion" runat="server" Enabled="False" Width="93%"
                                                    MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <br />
                            <br />
                            <div id="Div_Usos_Predominantes_Zona" runat="server" style="display: block">
                                <asp:Panel ID="Pnl_Usos_Predominantes_Zona" runat="server" GroupingText="USO PREDOMINANTES EN LA ZONA"
                                    ForeColor="Blue">
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Uso Colindante(Distancia)
                                            </td>
                                            <td style="width: 85%" align="left">
                                                <asp:TextBox ID="Txt_Usos_Colindantes" runat="server" Enabled="False" Width="95%"
                                                    MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Uso Frente al inmueble
                                            </td>
                                            <td style="width: 85%" align="left">
                                                <asp:TextBox ID="Txt_Usos_Frente_Inmueble" runat="server" Enabled="False" Width="95%"
                                                    MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="4">
                                                Usos cercanos importantes o de impacto a considerar (ESCUELAS, IGLESIAS, BARES,
                                                HOSPITALES, CENTROS COMERCIALES, ETC). Especificar uso e indicar distancia aproximada.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="4">
                                                <asp:TextBox ID="Txt_Usos_Cercanos_Riesgo" runat="server" Enabled="False" Width="92%"
                                                    MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel_Avance" Width="100%">
                        <HeaderTemplate>
                            AVANCE</HeaderTemplate>
                        <ContentTemplate>
                            <div id="Div_Avance_Obra" runat="server" style="display: block">
                                <asp:Panel ID="Pnl_Avance_Obra" runat="server" GroupingText="AVANCE DE LA OBRA" ForeColor="Blue">
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Actividad
                                            </td>
                                            <td style="width: 85%" align="left">
                                                <asp:DropDownList ID="Cmb_Avance_Obra" runat="server" Width="96%">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Bardeo (INDICAR AVANCE APROX.)
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Avance_Obra_Aproximado" runat="server" Enabled="False" Width="95%"
                                                    MaxLength="50"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Avance_Obra_Aproximado" runat="server" FilterType="Numbers"
                                                    TargetControlID="Txt_Avance_Obra_Aproximado" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                Niveles actuales
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Avance_Obra_Niveles_Actuales" runat="server" Enabled="False"
                                                    Width="90%" MaxLength="50"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Avance_Obra_Niveles_Actuales" runat="server"
                                                    FilterType="Numbers" TargetControlID="Txt_Avance_Obra_Niveles_Actuales" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Niveles a construir
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Avance_Obra_Niveles_Construir" runat="server" Enabled="False"
                                                    Width="95%" MaxLength="50"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Avance_Obra_Niveles_Construir" runat="server"
                                                    FilterType="Numbers" TargetControlID="Txt_Avance_Obra_Niveles_Construir" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 35%" align="right">
                                                Proyecto acorde a lo solicitado
                                            </td>
                                            <td style="width: 15%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Avance_Obra_Acorde_Solicitado" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel_Regularizacion"
                        Width="100%">
                        <HeaderTemplate>
                            REGULARIZACION</HeaderTemplate>
                        <ContentTemplate>
                            <div id="Div_Inspecciones" runat="server" style="display: block">
                                <asp:Panel ID="Pnl_Inspecciones" runat="server" GroupingText="REFERENTE A LA INSPECCION SI EXISTE REQUERIMIENTO DE REGULARIZACION"
                                    ForeColor="Blue">
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 10%" align="left">
                                                Notificado
                                            </td>
                                            <td style="width: 10%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Notificado" runat="server" OnSelectedIndexChanged="RBtn_Notificado_OnSelectedIndexChanged"
                                                    AutoPostBack="True">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 10%" align="right">
                                                N° Folio
                                            </td>
                                            <td style="width: 10%" align="left">
                                                <asp:TextBox ID="Txt_Notificacion_Folio" runat="server" Enabled="False" Width="100%"
                                                    MaxLength="20"></asp:TextBox>
                                            </td>
                                            <td style="width: 10%" align="left">
                                            </td>
                                            <td style="width: 10%" align="right">
                                                Acta de inspeccion
                                            </td>
                                            <td style="width: 10%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Acta_Inspeccion" runat="server" OnSelectedIndexChanged="RBtn_Acta_Inspeccion_OnSelectedIndexChanged"
                                                    AutoPostBack="True">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 10%" align="right">
                                                N° Folio
                                            </td>
                                            <td style="width: 10%" align="left">
                                                <asp:TextBox ID="Txt_Acta_Inspeccion_Folio" runat="server" Enabled="False" Width="100%"
                                                    MaxLength="20"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%" align="left">
                                                Clausurado
                                            </td>
                                            <td style="width: 10%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Clausurado" runat="server" OnSelectedIndexChanged="RBtn_Clausurado_OnSelectedIndexChanged"
                                                    AutoPostBack="True">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 10%" align="right">
                                                N° Folio
                                            </td>
                                            <td style="width: 10%" align="left">
                                                <asp:TextBox ID="Txt_Clausurado_Folio" runat="server" Enabled="False" Width="100%"
                                                    MaxLength="20"></asp:TextBox>
                                            </td>
                                            <td style="width: 10%" align="left">
                                            </td>
                                            <td style="width: 10%" align="right">
                                                Multado
                                            </td>
                                            <td style="width: 10%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Multado" runat="server" OnSelectedIndexChanged="RBtn_Multado_OnSelectedIndexChanged"
                                                    AutoPostBack="True">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 10%" align="right">
                                                N° Folio
                                            </td>
                                            <td style="width: 10%" align="left">
                                                <asp:TextBox ID="Txt_Multa_Folio" runat="server" Enabled="False" Width="100%" MaxLength="20"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <table class="estilo_fuente" width="100%">
                                <tr>
                                    <td rowspan="5">
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel_Uso_Actual" Width="100%">
                        <HeaderTemplate>
                            USO ACTUAL</HeaderTemplate>
                        <ContentTemplate>
                            <div id="Div_Uso_Actual" runat="server" style="display: block">
                                <asp:Panel ID="Pnl_Uso_Actual" runat="server" ForeColor="Blue">
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Tipo
                                            </td>
                                            <td style="width: 85%" align="left">
                                                <asp:DropDownList ID="Cmb_Uso_Actual" runat="server" Width="95%">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 30%" align="left">
                                                Existe un uso diferente o adicional al solicitado
                                            </td>
                                            <td style="width: 10%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Uso_Diferente_Adicional" runat="server" OnSelectedIndexChanged="RBtn_Uso_Diferente_Adicional_OnSelectedIndexChanged"
                                                    AutoPostBack="True">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                                <td style="width: 30%" align="right">
                                                    Especificar que tipo de uso
                                                </td>
                                                <td style="width: 30%" align="left">
                                                    <asp:TextBox ID="Txt_Especificar_Tipo_Uso" runat="server" Enabled="False" Width="95%"
                                                        MaxLength="100"></asp:TextBox>
                                                </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 100%" align="center">
                                                Especificar si la actividad se realiza en local o parte del inmueble (SALA, COCHERA,
                                                ETC)
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%" align="left">
                                                <asp:TextBox ID="Txt_Area_Acticidad" runat="server" Enabled="False" Width="95%" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Superficie M2
                                            </td>
                                            <td style="width: 85%" align="left">
                                                <asp:TextBox ID="Txt_Superficie_Metros2" runat="server" Enabled="False" Width="50%"
                                                    MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 100%" align="left">
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 100%" align="center">
                                                Especificar si se utilizara algun tipo de maquinaria o equipo
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%" align="left" colspan="3">
                                                <asp:TextBox ID="Txt_Maquinaria_Utilizar" runat="server" Enabled="False" Width="95%"
                                                    MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="right">
                                            </td>
                                            <td style="width: 35%" align="left">
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Problemas
                                            </td>
                                            <td style="width: 85%" align="left">
                                                <asp:DropDownList ID="Cmb_Funcionamiento" runat="server" Width="95%">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 35%" align="left">
                                                N° de personas que laboran
                                            </td>
                                            <td style="width: 65%" align="left">
                                                <asp:TextBox ID="Txt_Cantidad_Personas_Laboran" runat="server" Enabled="False" Width="30%"
                                                    Style="text-align: right" MaxLength="50"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cantidad_Personas_Laboran" runat="server"
                                                    FilterType="Numbers" TargetControlID="Txt_Cantidad_Personas_Laboran" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 35%" align="left">
                                                Afluencia aprox. de clientes / dia
                                            </td>
                                            <td style="width: 65%" align="left">
                                                <asp:TextBox ID="Txt_Afluencia" runat="server" Enabled="False" Width="30%" Style="text-align: right"
                                                    MaxLength="50"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Afluencia" runat="server" FilterType="Numbers"
                                                    TargetControlID="Txt_Afluencia" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <table class="estilo_fuente" width="100%">
                                <tr>
                                    <td rowspan="5">
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel_Anuncios" Width="100%">
                        <HeaderTemplate>
                            ANUNCIOS</HeaderTemplate>
                        <ContentTemplate>
                            <div id="Div_Tipo_Anuncio" runat="server" style="display: block">
                                <asp:Panel ID="Pnl_Tipo_Anuncio" runat="server" GroupingText="SI EL INMUEBLE TIENE ALGUN TIPO DE ANUNCIO ESPECIFICAR"
                                    ForeColor="Blue">
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Tipo
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Tipo_Anuncio_1" runat="server" Enabled="false" Width="95%" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                Dimensiones
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Dimenciones_Anuncion_1" runat="server" Enabled="false" Width="95%"
                                                    MaxLength="50"></asp:TextBox>
                                                <%-- <cc1:FilteredTextBoxExtender ID="FTE_Txt_Dimenciones_Anuncion_1" runat="server" FilterType="Numbers"
                                                    TargetControlID="Txt_Dimenciones_Anuncion_1" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Tipo
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Tipo_Anuncio_2" runat="server" Enabled="false" Width="95%" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                Dimensiones
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Dimenciones_Anuncion_2" runat="server" Enabled="false" Width="95%"
                                                    MaxLength="50"></asp:TextBox>
                                                <%-- <cc1:FilteredTextBoxExtender ID="FTE_Txt_Dimenciones_Anuncion_2" runat="server" FilterType="Numbers"
                                                    TargetControlID="Txt_Dimenciones_Anuncion_2" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Tipo
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Tipo_Anuncio_3" runat="server" Enabled="false" Width="95%" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                Dimensiones
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Dimenciones_Anuncion_3" runat="server" Enabled="false" Width="95%"
                                                    MaxLength="50"></asp:TextBox>
                                                <%--<cc1:FilteredTextBoxExtender ID="FTE_Txt_Dimenciones_Anuncion_3" runat="server" FilterType="Numbers"
                                                    TargetControlID="Txt_Dimenciones_Anuncion_3" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Tipo
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Tipo_Anuncio_4" runat="server" Enabled="false" Width="95%" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                Dimensiones
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Dimenciones_Anuncion_4" runat="server" Enabled="false" Width="95%"
                                                    MaxLength="50"></asp:TextBox>
                                                <%--<cc1:FilteredTextBoxExtender ID="FTE_Txt_Dimenciones_Anuncion_4" runat="server" FilterType="Numbers"
                                                    TargetControlID="Txt_Dimenciones_Anuncion_4" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <table class="estilo_fuente" width="100%">
                                <tr>
                                    <td rowspan="5">
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel_Servicios" Width="100%">
                        <HeaderTemplate>
                            SERVICIOS</HeaderTemplate>
                        <ContentTemplate>
                            <div id="Div_Servicios" runat="server" style="display: block">
                                <asp:Panel ID="Pnl_Servicios" runat="server" ForeColor="Blue">
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 17%" align="left">
                                                Cuenta con servicios sanitario
                                            </td>
                                            <td style="width: 16%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Servicios_Sanitarios" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 17%" align="right">
                                                WC
                                            </td>
                                            <td style="width: 16%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Wc" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 17%" align="right">
                                                Lavabo
                                            </td>
                                            <td style="width: 17%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Lavabo" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 17%" align="left">
                                                Letrina
                                            </td>
                                            <td style="width: 16%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Letrina" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 17%" align="right">
                                                Mixto
                                            </td>
                                            <td style="width: 16%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Mixto" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 34%" align="left">
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 20%" align="left">
                                                N° sanitarios hombre
                                            </td>
                                            <td style="width: 15%" align="left">
                                                <asp:TextBox ID="Txt_Bano_Hombres" runat="server" Style="text-align: right" onkeyup='javascript:Banos_Hombres(this);'
                                                    Enabled="False" Width="95%" MaxLength="50"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Bano_Hombres" runat="server" FilterType="Numbers"
                                                    TargetControlID="Txt_Bano_Hombres" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 20%" align="right">
                                                N° sanitarios mujer
                                            </td>
                                            <td style="width: 15%" align="left">
                                                <asp:TextBox ID="Txt_Bano_Mujeres" runat="server" Style="text-align: right" onkeyup='javascript:Banos_Mujeres(this);'
                                                    Enabled="False" Width="95%" MaxLength="50"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Bano_Mujeres" runat="server" FilterType="Numbers"
                                                    TargetControlID="Txt_Bano_Mujeres" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                Total
                                            </td>
                                            <td style="width: 15%" align="left">
                                                <asp:TextBox ID="Txt_Total_Banos" runat="server" Enabled="False" Width="95%" Style="text-align: right"
                                                    MaxLength="50"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Total_Banos" runat="server" FilterType="Numbers"
                                                    TargetControlID="Txt_Total_Banos" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 17%" align="left">
                                                Agua potable
                                            </td>
                                            <td style="width: 16%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Agua_Potable" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 17%" align="right">
                                                Abast. por particular
                                            </td>
                                            <td style="width: 16%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Agua_Potable_Particular" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 17%" align="right">
                                                Abast. por JAPAMI
                                            </td>
                                            <td style="width: 17%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Agua_Potable_Japami" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 17%" align="left">
                                                Drenaje
                                            </td>
                                            <td style="width: 16%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Agua_Potable_Drenaje" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 17%" align="right">
                                                Fosa septica
                                            </td>
                                            <td style="width: 16%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Agua_Potable_Fosa_Septica" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 17%" align="right">
                                                Cuenta con estacionamiento
                                            </td>
                                            <td style="width: 17%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Cuenta_Estacionamiento" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 17%" align="left">
                                                Dentro del inmueble
                                            </td>
                                            <td style="width: 16%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Cuenta_Estacionamiento_Dentro_Inmueble" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 17%" align="right">
                                                Rentado o anexo
                                            </td>
                                            <td style="width: 16%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Cuenta_Estacionamiento_Rentado" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 17%" align="right">
                                                N° cajones
                                            </td>
                                            <td style="width: 17%" align="left">
                                                <asp:TextBox ID="Txt_Numero_Cajones" runat="server" Enabled="False" Width="95%" Style="text-align: right"
                                                    MaxLength="50"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Cajones" runat="server" FilterType="Numbers"
                                                    TargetControlID="Txt_Numero_Cajones" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 17%" align="left">
                                                Tiene area de carga y descarga
                                            </td>
                                            <td style="width: 16%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Cuenta_Estacionamiento_Area_Carga" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 17%" align="right">
                                            </td>
                                            <td style="width: 16%" align="left">
                                            </td>
                                            <td style="width: 17%" align="right">
                                            </td>
                                            <td style="width: 17%" align="left">
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 100%" align="center">
                                                En caso de que el estacionamiento se ubique fuera del inmueble especificar domicilio
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%" align="left">
                                                <asp:TextBox ID="Txt_Domicilio_Estacionamiento" runat="server" Enabled="False" Width="99%"
                                                    MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <table class="estilo_fuente" width="100%">
                                <tr>
                                    <td rowspan="5">
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel_ESTABLECIMIENTO"
                        Width="100%">
                        <HeaderTemplate>
                            ESTABLECIMIENTO</HeaderTemplate>
                        <ContentTemplate>
                            <div id="Div_Medidas_Seguridad" runat="server" style="display: block">
                                <asp:Panel ID="Pnl_Medidas_Seguridad" runat="server" ForeColor="Blue">
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 30%" align="left">
                                                Material empelado en muros
                                            </td>
                                            <td style="width: 70%" align="left">
                                                <asp:DropDownList ID="Cmb_Material_Muros" runat="server" Width="95%">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 30%" align="left">
                                                Material empelado en Techo o plafon
                                            </td>
                                            <td style="width: 70%" align="left">
                                                <asp:DropDownList ID="Cmb_Material_Techo" runat="server" Width="95%">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <table class="estilo_fuente" width="100%">
                                <tr>
                                    <td rowspan="5">
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel_SEGURIDAD" Width="100%">
                        <HeaderTemplate>
                            SEGURIDAD</HeaderTemplate>
                        <ContentTemplate>
                            <div id="Div_Establecimiento" runat="server" style="display: block">
                                <asp:Panel ID="Pnl_Establecimiento" runat="server" GroupingText="MEDIDAS DE SEGURIDAD"
                                    ForeColor="Blue">
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 17%" align="left">
                                                Cuenta con señalizacion
                                            </td>
                                            <td style="width: 16%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Medidas_Seguridad_Senalizacion" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 17%" align="right">
                                                Equipo de seguridad
                                            </td>
                                            <td style="width: 16%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Medidas_Seguridad_Equipo" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 17%" align="right">
                                                Material flamable a la vista
                                            </td>
                                            <td style="width: 17%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Medidas_Seguridad_Material_Flamable" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 17%" align="left">
                                                Especificar
                                            </td>
                                            <td style="width: 16%" align="left" colspan="5">
                                                <asp:TextBox ID="Txt_Medidas_Seguridad_Especificar" runat="server" Enabled="False"
                                                    Width="95%" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <table class="estilo_fuente" width="100%">
                                <tr>
                                    <td rowspan="5">
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel_Poda_Arbol" Width="100%">
                        <HeaderTemplate>
                            ARBOLES</HeaderTemplate>
                        <ContentTemplate>
                            <div id="Div_Podar_Arbol" runat="server" style="display: block">
                                <asp:Panel ID="Pnl_Podar_Arbol" runat="server" ForeColor="Blue">
                                    <asp:Panel ID="Pnl_Tipos_Arboles" runat="server" GroupingText="Tipo">
                                        <table class="estilo_fuente" width="100%">
                                            <tr>
                                                <td style="width: 15%" align="left">
                                                    <asp:RadioButton ID="RBtn_Arboles_Tipo_Poda" runat="server" Text="Poda:"></asp:RadioButton>
                                                </td>
                                                <td style="width: 15%" align="left">
                                                    Cantidad
                                                </td>
                                                <td style="width: 20%" align="left">
                                                    <asp:TextBox ID="Txt_Arboles_Cantidad_Poda" runat="server" Width="85%" Style="text-align: right"
                                                        MaxLength="20"></asp:TextBox>
                                                </td>
                                                <td style="width: 15%" align="left">
                                                    <asp:RadioButton ID="RBtn_Arboles_Tipo_Tala" runat="server" Text="Tala:"></asp:RadioButton>
                                                </td>
                                                <td style="width: 15%" align="left">
                                                    Cantidad
                                                </td>
                                                <td style="width: 20%" align="left">
                                                    <asp:TextBox ID="Txt_Arboles_Cantidad_Tala" runat="server" Width="85%" Style="text-align: right"
                                                        MaxLength="20"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%" align="left">
                                                    <asp:RadioButton ID="RBtn_Arboles_Tipo_Trasplante" runat="server" Text="Transplante:">
                                                    </asp:RadioButton>
                                                </td>
                                                <td style="width: 15%" align="left">
                                                    Cantidad
                                                </td>
                                                <td style="width: 20%" align="left">
                                                    <asp:TextBox ID="Txt_Arboles_Cantidad_Trasplante" runat="server" Width="85%" Style="text-align: right"
                                                        MaxLength="20" />
                                                </td>
                                                <td style="width: 15%" align="left">
                                                </td>
                                                <td style="width: 15%" align="left">
                                                </td>
                                                <td style="width: 20%" align="left">
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Altura
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Arbol_Altura" runat="server" Width="95%" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                Diametro de tronco
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Arbol_Diametro" runat="server" Width="95%" Style="text-align: right"
                                                    MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Diametro de fronda
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Arbol_Diametro_Fronda" runat="server" Width="95%" Style="text-align: right"
                                                    MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                Estado
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Arbol_Estado" runat="server" Width="95%" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Permiso del instituto de ecologia
                                            </td>
                                            <td style="width: 15%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Material_Permiso_Ecologia" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                Permiso de uso de suelo
                                            </td>
                                            <td style="width: 15%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Material_Permiso_Suelo" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                Superficie Total
                                            </td>
                                            <td style="width: 25%" align="left">
                                                <asp:TextBox ID="Txt_Materiales_Superficie_Total" runat="server" Width="95%" Style="text-align: right"
                                                    MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Profundidad
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Materiales_Profundidad" runat="server" Width="95%" Style="text-align: left"
                                                    MaxLength="100"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                Inclinacion
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Materiales_Inclinacion" runat="server" Width="95%" Style="text-align: left"
                                                    MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Especie
                                            </td>
                                            <td style="width: 35%" align="left" colspan="3">
                                                <asp:TextBox ID="Txt_Arboles_Especie" runat="server" Width="98%" onkeypress="return imposeMaxLength(this, 99);"
                                                    TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Flora
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Materiales_Flora" MaxLength="100" runat="server" Width="95%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                Tipo de material petreo
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Materiales_Petreo" MaxLength="100" runat="server" Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Accesibilidad de vehiculos/tolvas
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Material_Accesibilidad_Vehiculo" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 15%" align="right">
                                            </td>
                                            <td style="width: 35%" align="right">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <table class="estilo_fuente" width="100%">
                                <tr>
                                    <td rowspan="5">
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="TabPnl_Manifiesto_Impacto_Ambiental" Width="100%">
                        <HeaderTemplate>
                            IMPACTO AMBIENTAL</HeaderTemplate>
                        <ContentTemplate>
                            <div id="Div_Manifiesto_Impacto_Ambiental" runat="server" style="display: block">
                                <asp:Panel ID="Pnl_Manifiesto_Impacto_Ambiental" runat="server" GroupingText="Manifiesto de impacto ambiental"
                                    ForeColor="Blue">
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%">
                                                Afectaciones de arboles
                                            </td>
                                            <td style="width: 85%">
                                                <asp:TextBox ID="Txt_Manifiesto_Afectacion" runat="server" Enabled="true" Width="95%"
                                                    TextMode="MultiLine" Rows="2" onkeypress="return imposeMaxLength(this, 99);" ></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" align="left" colspan="2">
                                                Colindancias (distancia, centros de población, cuerpos de agua (lagos, ríos, canales),
                                                gasoductos, líneas de alta tensión, centros de concentración masiva)
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" colspan="2">
                                                <asp:TextBox ID="Txt_Manifiesto_Colindancia" runat="server" Enabled="true" Width="95%"
                                                    onkeypress="return imposeMaxLength(this, 99);" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Superficie Total
                                            </td>
                                            <td style="width: 85%">
                                                <asp:TextBox ID="Txt_Manifiesto_Superficie_Total" MaxLength="100" runat="server" Enabled="true" Width="95%"></asp:TextBox>
                                                <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                                    TargetControlID="Txt_Manifiesto_Superficie_Total" Enabled="True" ValidChars="0123456789.">
                                                </cc1:FilteredTextBoxExtender>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Tipo Proyecto
                                            </td>
                                            <td style="width: 85%">
                                                <asp:TextBox ID="Txt_Manifiesto_Tipo_Proyecto" runat="server" Enabled="true" Width="95%"
                                                    MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="TabPnl_Licencia_Ambiental" Width="100%">
                        <HeaderTemplate>
                            EMISIONES</HeaderTemplate>
                        <ContentTemplate>
                            <div id="Div_Licencia_Ambiental" runat="server" style="display: block">
                                <asp:Panel ID="Pnl_Licencia_Ambiental" runat="server" GroupingText="Licencia ambiental de funcionamiento"
                                    ForeColor="Blue">
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Tipo de equipo emisor
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Licencia_Equipo_Emisor" MaxLength="100" runat="server" Enabled="true" Width="95%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%" align="left">
                                                Tipo Emision
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Licencia_Tipo_Emision" MaxLength="100" runat="server" Enabled="true" Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Hora de funcionamiento
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Licencia_Hora_Funcionamiento" runat="server" Enabled="true"
                                                    Width="95%"></asp:TextBox>
                                                <cc1:MaskedEditExtender ID="MEE_Txt_Licencia_Hora_Funcionamiento" runat="server"
                                                    Mask="99:99:99" TargetControlID="Txt_Licencia_Hora_Funcionamiento" MaskType="Time"
                                                    AcceptNegative="None" AcceptAMPM="true" />
                                            </td>
                                            <td style="width: 15%" align="left">
                                                Tipo de combustible
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Licencia_Tipo_Conbustible" MaxLength="100" runat="server" Enabled="true" Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Gastos de combustible
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Licencia_Gastos_Combustible" MaxLength="100" runat="server" Enabled="true" Width="95%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%" align="left">
                                                Almacenaje de combustible
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Licencia_Almacenaje_Combustible" MaxLength="100" runat="server" Enabled="true"
                                                    Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Cantidad de combustible
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Licencia_Cantidad_Combustible" MaxLength="100" runat="server" Enabled="true"
                                                    Width="95%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%" align="left">
                                            </td>
                                            <td style="width: 35%" align="left">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="TabPnl_Aprovechamiento" Width="100%">
                        <HeaderTemplate>
                            AMBIENTAL</HeaderTemplate>
                        <ContentTemplate>
                            <div id="Div_Aprovechamiento" runat="server" style="display: block">
                                <asp:Panel ID="Pnl_Aprovechamiento" runat="server" GroupingText="Autorizacion de aprovechamiento ambiental"
                                    ForeColor="Blue">
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Uso de suelos
                                            </td>
                                            <td style="width: 15%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Aprovechamiento_Uso_Suelo" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                    <asp:ListItem Value="En tramite">En tramite</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                Area de almacen de residuos
                                            </td>
                                            <td style="width: 15%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Aprovechamiento_Almacen_Residuos" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                Existe separacion
                                            </td>
                                            <td style="width: 15%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Aprovechamiento_Existe_Separacion" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 10%" align="right">
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Residuos tipo
                                            </td>
                                            <td style="width: 75%" align="left">
                                                <asp:DropDownList ID="Cmb_Tipo_Residuo" runat="server" Width="90%" DropDownStyle="DropDownList"
                                                    AutoCompleteMode="SuggestAppend" CaseSensitive="False" CssClass="WindowsStyle" />
                                            </td>
                                            <td style="width: 15%" align="center">
                                                <asp:ImageButton ID="Btn_Agregar_Tipo_Residuo" runat="server" Width="20px" Height="20px"
                                                    ImageUrl="~/paginas/imagenes/gridview/add_grid.png" AlternateText="Agregar Documento"
                                                    OnClick="Btn_Agregar_Tipo_Residuo_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 100%; text-align: center; vertical-align: top;">
                                                <center>
                                                    <div id="Div2" runat="server" style="overflow: auto; width: 99%; vertical-align: top;
                                                        border-style: solid; border-color: Silver; display: block">
                                                        <asp:GridView ID="Grid_Tipos_Residuos" runat="server" Width="97%" CssClass="GridView_1"
                                                            GridLines="None" AutoGenerateColumns="False" EmptyDataText="No se encuentra ningun tipo de residuo">
                                                            <Columns>
                                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png"
                                                                    Visible="False">
                                                                    <ItemStyle Width="0%" />
                                                                </asp:ButtonField>
                                                                <asp:BoundField DataField="TIPO_RESIDUO_ID" HeaderText="TIPO_RESIDUO_ID" SortExpression="TIPO_RESIDUO_ID" />
                                                                <asp:BoundField DataField="NOMBRE" HeaderText="Residuo" SortExpression="NOMBRE">
                                                                    <HeaderStyle Font-Size="13px" HorizontalAlign="Left" />
                                                                    <ItemStyle Font-Size="12px" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Btn_Quitar_Tipo_Residuo" runat="server" Width="20px" Height="20px"
                                                                            ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" AlternateText="Quitar Documento"
                                                                            OnClick="Btn_Quitar_Tipo_Residuo_Click" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" Width="5%" Font-Size="13px" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" Font-Size="12px" />
                                                                </asp:TemplateField>
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
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Metodo de separacion de residuos
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Aprovechamiento_Metodo_Sepearacion" MaxLength="100" runat="server" Width="95%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                Quien presta el servicio de recoleccion
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Aprovechamiento_Servicio_Recoleccion" MaxLength="100" runat="server" Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 25%" align="left">
                                                Se revuelven liquidos con solidos
                                            </td>
                                            <td style="width: 25%" align="left">
                                                <asp:RadioButtonList ID="RBtn_Aprovechamiento_Revuelven_Liquidos" runat="server">
                                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                Tipo de contenedor
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Aprovechamiento_Tipo_Contenedor" MaxLength="100" runat="server" Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Tipo de ruido
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Aprovechamiento_Tipo_Ruido" MaxLength="100" runat="server" Width="95%"></asp:TextBox>
                                            </td>
                                            <td style="width: 15%" align="right">
                                                Nivel de ruido
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Aprovechamiento_Nivel_Ruido" MaxLength="100" runat="server" Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Horario de labores Inicio
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Aprovechamiento_Horario_Inicial" runat="server" Width="95%"></asp:TextBox>
                                                <cc1:MaskedEditExtender ID="Mee_Txt_Aprovechamiento_Horario_Inicial" runat="server"
                                                    Mask="99:99:99" TargetControlID="Txt_Aprovechamiento_Horario_Inicial" MaskType="Time"
                                                    AcceptAMPM="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                    CultureTimePlaceholder="" Enabled="True" />
                                            </td>
                                            <td style="width: 15%" align="right">
                                                Horario de labores Fin
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Aprovechamiento_Horario_Final" runat="server" Width="95%"></asp:TextBox>
                                                <cc1:MaskedEditExtender ID="Mee_Txt_Aprovechamiento_Horario_Final" runat="server"
                                                    Mask="99:99:99" TargetControlID="Txt_Aprovechamiento_Horario_Final" MaskType="Time"
                                                    AcceptAMPM="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                    CultureTimePlaceholder="" Enabled="True" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 16%" align="left">
                                                Dias
                                            </td>
                                            <td style="width: 12%" align="left">
                                                <asp:CheckBox ID="Chk_Dias_Labor_Lunes" runat="server" Text="Lunes" />
                                            </td>
                                            <td style="width: 12%" align="left">
                                                <asp:CheckBox ID="Chk_Dias_Labor_Martes" runat="server" Text="Martes" />
                                            </td>
                                            <td style="width: 12%" align="left">
                                                <asp:CheckBox ID="Chk_Dias_Labor_Miercoles" runat="server" Text="Miercoles" />
                                            </td>
                                            <td style="width: 12%" align="left">
                                                <asp:CheckBox ID="Chk_Dias_Labor_Jueves" runat="server" Text="Jueves" />
                                            </td>
                                            <td style="width: 12%" align="left">
                                                <asp:CheckBox ID="Chk_Dias_Labor_Viernes" runat="server" Text="Viernes" />
                                            </td>
                                            <td style="width: 12%" align="left">
                                                <asp:CheckBox ID="Chk_Dias_Labor_Sabado" runat="server" Text="Sabado" />
                                            </td>
                                            <td style="width: 12%" align="left">
                                                <asp:CheckBox ID="Chk_Dias_Labor_Domingo" runat="server" Text="Domingo" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 20%" align="left">
                                                Emisiones a la atmosfera(humo, polvo, vapor, etc)
                                            </td>
                                            <td style="width: 80%" align="left">
                                                <asp:TextBox ID="Txt_Aprovechamiento_Emisiones_Atmosfera" runat="server" Width="98%"
                                                    MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel_Generales" Width="100%">
                        <HeaderTemplate>
                            GENERALES</HeaderTemplate>
                        <ContentTemplate>
                            <div id="Div_Generales" runat="server" style="display: block">
                                <asp:Panel ID="Pnl_Generales" runat="server" GroupingText="" ForeColor="Blue">
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                * Recepcion de inspector
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Generales_Recepcion_Inspector_Fecha" runat="server" Enabled="false"
                                                    Width="85%"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Recepcion_Inspector_Fecha" runat="server"
                                                    TargetControlID="Txt_Generales_Recepcion_Inspector_Fecha" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                                    ValidChars="/_" />
                                                <cc1:CalendarExtender ID="Txt_Recepcion_Inspector_Fecha_CalendarExtender" runat="server"
                                                    TargetControlID="Txt_Generales_Recepcion_Inspector_Fecha" PopupButtonID="Btn_Generales_Recepcion_Inspector_Fecha"
                                                    Format="dd/MMM/yyyy" />
                                                <asp:ImageButton ID="Btn_Generales_Recepcion_Inspector_Fecha" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                            </td>
                                            <td style="width: 15%" align="right">
                                                * Realizado en campo
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Generales_Recepcion_Campo_Fecha" runat="server" Enabled="false"
                                                    Width="85%"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Generales_Recepcion_Campo_Fecha" runat="server"
                                                    TargetControlID="Txt_Generales_Recepcion_Campo_Fecha" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                                    ValidChars="/_" />
                                                <cc1:CalendarExtender ID="Txt_Generales_Recepcion_Campo_Fecha_CalendarExtender" runat="server"
                                                    TargetControlID="Txt_Generales_Recepcion_Campo_Fecha" PopupButtonID="Btn_Generales_Recepcion_Campo_Fecha"
                                                    Format="dd/MMM/yyyy" />
                                                <asp:ImageButton ID="Btn_Generales_Recepcion_Campo_Fecha" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                * Recibida por coordinación
                                            </td>
                                            <td style="width: 35%" align="left">
                                                <asp:TextBox ID="Txt_Generales_Recepcion_Coordinador_Fecha" runat="server" Enabled="false"
                                                    Width="85%"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Generales_Recepcion_Coordinador_Fecha" runat="server"
                                                    TargetControlID="Txt_Generales_Recepcion_Coordinador_Fecha" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                                    ValidChars="/_" />
                                                <cc1:CalendarExtender ID="Txt_Generales_Recepcion_Coordinador_Fecha_CalendarExtender"
                                                    runat="server" TargetControlID="Txt_Generales_Recepcion_Coordinador_Fecha" PopupButtonID="Btn_Generales_Recepcion_Coordinador_Fecha"
                                                    Format="dd/MMM/yyyy" />
                                                <asp:ImageButton ID="Btn_Generales_Recepcion_Coordinador_Fecha" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                            </td>
                                            <td style="width: 15%" align="right">
                                            </td>
                                            <td style="width: 35%" align="right">
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="estilo_fuente" width="100%">
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Observaciones para el inspector
                                            </td>
                                            <td style="width: 85%" align="left">
                                                <asp:TextBox ID="Txt_Generales_Observaciones_Para_Inspector" runat="server" Rows="3"
                                                    TextMode="MultiLine" Width="99%" onkeypress="return imposeMaxLength(this, 99);"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Generales_Observaciones_Para_Inspector"
                                                    runat="server" TargetControlID="Txt_Generales_Observaciones_Para_Inspector" WatermarkText="Límite de Caractes 100"
                                                    WatermarkCssClass="watermarked" Enabled="True">
                                                </cc1:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%" align="left">
                                                Observaciones del inspector
                                            </td>
                                            <td style="width: 85%" align="left">
                                                <asp:TextBox ID="Txt_Generales_Observaciones_Del_Inspector" runat="server" Rows="3"
                                                    TextMode="MultiLine" Width="99%" onkeypress="return imposeMaxLength(this, 99);"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Generales_Observaciones_Del_Inspector"
                                                    runat="server" TargetControlID="Txt_Generales_Observaciones_Del_Inspector" WatermarkText="Límite de Caractes 100"
                                                    WatermarkCssClass="watermarked" Enabled="True">
                                                </cc1:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

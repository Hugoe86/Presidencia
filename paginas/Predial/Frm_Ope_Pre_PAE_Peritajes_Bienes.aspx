<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_PAE_Peritajes_Bienes.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_PAE_Peritaje_Bienes"
    Culture="es-MX" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
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
            padding:0 5px 0 5px;
        }
        a.enlace_fotografia:hover
        {
            color: #25406D;
            text-decoration: underline;
            font-weight: bold;
            padding:0 5px 0 5px;
        }
    </style>
    <!--SCRIPT PARA LA VALIDACION QUE NO EXPIRE LA SESSION-->

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
        setInterval('MantenSesion()', <%=(int)(0.9*(Session.Timeout * 60000))%>);

        window.onerror = new Function("return true");
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades)
        {
            window.showModalDialog(Url, null, Propiedades);
        }

        function Abrir_Resumen(Url, Propiedades)
        {
            window.open(Url, 'Resumen_Predio', Propiedades);
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="Tool_ScriptManager" runat="server" EnableScriptGlobalization="true">
    </cc1:ToolkitScriptManager>
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
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Bienes
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Encabezado_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td>
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click" />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                        </td>
                                        <td align="right" style="width: 41%;">
                                            <table style="width: 100%; height: 28px;">
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: right; width: 20%;">
                                                        B&uacute;squeda:
                                                    </td>
                                                    <td style="width: 55%;">
                                                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" ToolTip="Buscar" Width="180px" />
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Folio>" TargetControlID="Txt_Busqueda" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                                            FilterType="Numbers"/>
                                                    </td>
                                                    <td style="vertical-align: middle; width: 5%;">
                                                        <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                            ToolTip="Buscar" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <div id="Div_Generadas" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <%---------------- filtros ----------------%>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Folio inicial
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Folio_Inicial" runat="server" Width="96.4%" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Folio_Inicial" runat="server" TargetControlID="Txt_Folio_Inicial"
                                    FilterType="Numbers" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Folio final
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Folio_Final" runat="server" Width="96.4%" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Folio_Final" runat="server" TargetControlID="Txt_Folio_Final"
                                    FilterType="Numbers" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Número de Cuenta
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Numero_Cuenta" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Contribuyente
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Contribuyente" runat="server" Width="85%" Enabled="False" />
                                <asp:ImageButton ID="Btn_Busca_Contribuyente" runat="server" Height="22px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                    Width="22px" OnClick="Btn_Busca_Contribuyente_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Fecha inicial
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" MaxLength="11" Width="84%" AutoPostBack="true" OnTextChanged="Txt_Fecha_Inicial_TextChanged" />
                                <cc1:TextBoxWatermarkExtender ID="TBWE_Txt_Fecha_Inicial" runat="server" Enabled="True"
                                    TargetControlID="Txt_Fecha_Inicial" WatermarkCssClass="watermarked" WatermarkText="Día/Mes/Año" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Inicial" runat="server" Enabled="True" Format="dd/MMM/yyyy"
                                    PopupButtonID="Btn_Txt_Fecha_Inicial" TargetControlID="Txt_Fecha_Inicial" />
                                <asp:ImageButton ID="Btn_Txt_Fecha_Inicial" runat="server" CausesValidation="false"
                                    Height="18px" ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Fecha final
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Final" runat="server" MaxLength="11" Width="84%" AutoPostBack="true" OnTextChanged="Txt_Fecha_Final_TextChanged" />
                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Fecha_Final" runat="server" Enabled="True"
                                    TargetControlID="Txt_Fecha_Final" WatermarkCssClass="watermarked" WatermarkText="Día/Mes/Año" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Final" runat="server" Enabled="True" Format="dd/MMM/yyyy"
                                    PopupButtonID="Btn_Txt_Fecha_Final" TargetControlID="Txt_Fecha_Final" />
                                <asp:ImageButton ID="Btn_Txt_Fecha_Final" runat="server" CausesValidation="false"
                                    Height="18px" ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Despacho
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Asignado_a" runat="server" Width="99%">
                                    <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE" />
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Estatus
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%">
                                    <asp:ListItem Text="&lt; SELECCIONE &gt;" Value="0" />
                                    <asp:ListItem Text="NOTIFICACION" />
                                    <asp:ListItem Text="NO DILIGENCIADO" />
                                    <asp:ListItem Text="ILOCALIZABLE" />
                                    <asp:ListItem Text="PENDIENTE" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: right;">
                                <asp:Label ID="Lbl_Busqueda" runat="server" Text="Buscar Bienes"></asp:Label>
                                <asp:ImageButton ID="Btn_Buscar_Bienes" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png"
                                    OnClick="Btn_Buscar_Bienes_Click" />
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="4">
                                <asp:GridView ID="Grid_Embargos_Generados" runat="server" AllowPaging="True" AllowSorting="True"
                                    AutoGenerateColumns="False" CssClass="GridView_1" HeaderStyle-CssClass="tblHead"
                                    PageSize="5" Style="white-space: normal;" Width="100%" OnSelectedIndexChanged="Grid_Embargos_Generados_SelectedIndexChanged"
                                    OnPageIndexChanging="Grid_Embargos_Generados_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Detalle_Cuenta" runat="server" Height="20px" ImageUrl="~/paginas/imagenes/gridview/blue_button_.png"
                                                    CommandName="Select" ToolTip="Detalles Cuenta" Width="20px" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta">
                                            <ItemStyle HorizontalAlign="right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TOTAL" HeaderText="Adeudo" DataFormatString="{0:C2}">
                                            <ItemStyle HorizontalAlign="right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FOLIO" HeaderText="Folio">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ASIGNADO" HeaderText="Asignado">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ENTREGA" HeaderText="Entrega">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA_NOTIFICACION" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CUENTA_PREDIAL_ID" Visible="false" />
                                        <%-- 8 --%>
                                        <asp:BoundField DataField="NO_DETALLE_ETAPA" Visible="false" />
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="Hdn_Cuenta_ID" runat="server" />
                    <asp:HiddenField ID="Hdn_Contribuyente_ID" runat="server" />
                    <asp:HiddenField ID="Hdn_Archivos_Bien" runat="server" />
                </div>
                <div id="Div_Depositarios" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;">
                                Depositario
                            </td>
                        </tr>
                        <%--              Grid Depositarios                  --%>
                        <tr>
                            <td>
                                <asp:GridView ID="Grid_Depositario" runat="server" AllowPaging="True" AllowSorting="True"
                                    AutoGenerateColumns="False" CssClass="GridView_1" HeaderStyle-CssClass="tblHead"
                                    PageSize="5" Style="white-space: normal;" Width="100%" EmptyDataText="NO SE HA ENCONTRADO DEPOSITARIO">
                                    <Columns>
                                        <asp:BoundField DataField="FIGURA" DataFormatString="{0:C2}" HeaderText="Figura">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" DataFormatString="{0:C2}" HeaderText="Nombre">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DOMICILIO" HeaderText="Domicilio">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA_REMOCION" HeaderText="Fecha remoción" DataFormatString="{0:dd/MMM/yyyy}">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="Div_Detalles" runat="server">
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Avaluo
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Avaluo" runat="server" Width="96.4%" AutoPostBack="true"
                                    OnSelectedIndexChanged="Cmb_Avaluo_SelectedIndexChanged" />
                            </td>
                            <td style="text-align: right; width: 20%;">
                                *Fecha peritaje
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Peritaje" runat="server" MaxLength="11" Width="84%" />
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Fecha_Peritaje" runat="server" Enabled="True"
                                    TargetControlID="Txt_Fecha_Peritaje" WatermarkCssClass="watermarked" WatermarkText="Día/Mes/Año" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Peritaje" runat="server" Enabled="True" Format="dd/MMM/yyyy"
                                    PopupButtonID="Btn_Txt_Fecha_Peritaje" TargetControlID="Txt_Fecha_Peritaje" />
                                <asp:ImageButton ID="Btn_Txt_Fecha_Peritaje" runat="server" CausesValidation="false"
                                    Height="18px" ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Perito
                            </td>
                            <td colspan="3" style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Perito" runat="server" Width="98.9%" Style="text-transform: uppercase;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Valor
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Valor_Peritaje" runat="server" Width="96.4%" Style="text-align: right;" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Valor_Peritaje" runat="server" TargetControlID="Txt_Valor_Peritaje"
                                    FilterType="Numbers, Custom" ValidChars=".," />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                Observaciones
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%; vertical-align: top;">
                                <asp:TextBox ID="Txt_Observaciones" runat="server" MaxLength="250" TextMode="MultiLine"
                                    Width="98.6%" Style="text-transform: uppercase;" />
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observaciones" runat="server" TargetControlID="Txt_Observaciones"
                                    WatermarkText="Límite de Caracteres 250" WatermarkCssClass="watermarked" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Observaciones" runat="server" TargetControlID="Txt_Observaciones"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                            </td>
                        </tr>
                        <tr style="background-color: #36C;">
                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFF;">
                                Bienes
                            </td>
                        </tr>
                        <tr id="Tr_Fila_Tipo_Bien" runat="server">
                            <td style="text-align: left; width: 20%;">
                                *Tipo de bien
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Tipo_de_bien" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: right; width: 20%;">
                                *Valor
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Valor_Bien" runat="server" MaxLength="11" Width="96.4%" Style="text-align: right;" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Valor_Bien" runat="server" FilterType="Custom, Numbers"
                                    TargetControlID="Txt_Valor_Bien" ValidChars=".," />
                            </td>
                        </tr>
                        <tr id="Tr_Fila_Descripcion_Bien" runat="server">
                            <td style="text-align: left; width: 20%; vertical-align: top;">
                                *Descripción
                            </td>
                            <td colspan="3" style="text-align: left; width: 80%; vertical-align: top;">
                                <asp:TextBox ID="Txt_Descripcion" runat="server" MaxLength="250" TextMode="MultiLine"
                                    Width="98.6%" Style="text-transform: uppercase;" />
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Descripcion" runat="server" TargetControlID="Txt_Descripcion"
                                    WatermarkCssClass="watermarked" WatermarkText="Límite de Caracteres 250" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Descripcion" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                    TargetControlID="Txt_Descripcion" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                            </td>
                        </tr>
                        <tr id="Tr_Fila_Fotografias_Bien" runat="server">
                            <td style="text-align: left; width: 20%;">
                                Fotografías del bien
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:FileUpload ID="Fup_Bien" runat="server" Width="235px" />
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <asp:ImageButton ID="Btn_Subir_Archivo" runat="server" ToolTip="Enviar archivo" Style="border: 0 none;
                                    width: 20px; height: 20px; padding: 0;" ImageUrl="~/paginas/imagenes/paginas/sias_upload.png"
                                    OnClick="Btn_Subir_Archivo_Click" />
                                <asp:TextBox ID="Txt_Fotografias" runat="server" Width="80%" Enabled="false" />
                                &nbsp; &nbsp;
                                <asp:ImageButton ID="Btn_Agregar_Bien" runat="server" Height="20px" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                    OnClick="Btn_Agregar_Bien_Click" ToolTip="Agregar Costo" Width="20px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: left; width: 20%;">
                                <asp:GridView ID="Grid_Bienes" runat="server" AllowPaging="True" AllowSorting="True"
                                    AutoGenerateColumns="False" HeaderStyle-CssClass="tblHead" OnPageIndexChanging="Grid_Bienes_PageIndexChanging"
                                    OnSelectedIndexChanged="Grid_Bienes_SelectedIndexChanged" PageSize="5" Style="white-space: normal;"
                                    Width="100%" OnRowDataBound="Grid_Bienes_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="TIPO_BIEN" HeaderText="Tipo de bien">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPCION" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Descripción">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VALOR" HeaderText="Valor" DataFormatString="{0:c}">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FOTOGRAFIAS" HeaderText="Fotografías">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Eliminar_Fotos" runat="server" CommandName="Select" Height="20px"
                                                    ImageUrl="~/paginas/imagenes/paginas/delete.png" ToolTip="Eliminar" Width="20px" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="NO_BIEN" Visible="false" />
                                    </Columns>
                                    <HeaderStyle CssClass="tblHead" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr style="background-color: #36C;">
                            <td colspan="4" style="text-align: left; font-size: 15px; color: #FFF;">
                                Almacenamiento
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="RBtn_Lugar" runat="server" OnSelectedIndexChanged="RBtn_Lugar_SelectedIndexChanged"
                                    AutoPostBack="True">
                                    <asp:ListItem>Lugar del  Predio</asp:ListItem>
                                    <asp:ListItem Value="Lugar Externo">Lugar Externo</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Lugar
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Lugar" runat="server" Width="96.4%" Style="text-transform: uppercase;" />
                            </td>
                            <td style="text-align: right; width: 20%;">
                                *Costo m2
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Costo" runat="server" Width="96.4%" Style="text-align: right;" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Costo" runat="server" TargetControlID="Txt_Costo"
                                    FilterType="Numbers, Custom" ValidChars=".," />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Dimensiones
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Dimensiones" runat="server" Width="96.4%" Style="text-transform: uppercase;" />
                            </td>
                            <td style="text-align: right; width: 20%;">
                                *Fecha de ingreso
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Ingreso" runat="server" MaxLength="11" Width="84%" />
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Fecha_Ingreso" runat="server" Enabled="True"
                                    TargetControlID="Txt_Fecha_Ingreso" WatermarkCssClass="watermarked" WatermarkText="Día/Mes/Año" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Ingreso" runat="server" Enabled="True" Format="dd/MMM/yyyy"
                                    PopupButtonID="Btn_Txt_Fecha_Ingreso" TargetControlID="Txt_Fecha_Ingreso" />
                                <asp:ImageButton ID="Btn_Txt_Fecha_Ingreso" runat="server" CausesValidation="false"
                                    Height="18px" ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                Tiempo transcurrido
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Tiempo_transcurrido" runat="server" Width="96.4%" />
                            </td>
                            <td style="text-align: right; width: 20%;">
                                Costo almacenamiento
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Costo_Almacenamiento" runat="server" Width="96.4%" Style="text-align: right;" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Costo_Almacenamiento" runat="server" TargetControlID="Txt_Costo_Almacenamiento"
                                    FilterType="Numbers, Custom" ValidChars=".," />
                            </td>
                        </tr>
                    </table>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

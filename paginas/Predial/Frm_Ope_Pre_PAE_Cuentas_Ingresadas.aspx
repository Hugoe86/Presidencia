<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_PAE_Cuentas_Ingresadas.aspx.cs"
    Inherits="paginas_Predial_Frm_Ope_Pre_PAE_Cuentas_Ingresadas" Culture="es-MX"
    MaintainScrollPositionOnPostback="true" %>

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
            padding: 0 5px 0 5px;
        }
        a.enlace_fotografia:hover
        {
            color: #25406D;
            text-decoration: underline;
            font-weight: bold;
            padding: 0 5px 0 5px;
        }
        a.Detalles_Gastos:link, a.Detalles_Gastos:visited, a.Detalles_Gastos:hover
        {
            color: #2F4E7D;
            text-decoration:underline;
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
        
        //registra los eventos para la página
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(PageLoaded);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);

        function PageLoaded(sender, args) { }
        function endRequestHandler(sender, args) {
            $(function() {
                $('.Detalles_Gastos').click(function() {
                    var id = $(this).attr('id_cuenta');
                    Abrir_Ventana_Modal('Ventanas_Emergentes/PAE/Frm_Detalles_Gastos_De_Ejecucion.aspx?Cuenta_Predial=' + id, 'center:yes;resizable:yes;status:no;dialogWidth:290px;dialogHeight:225px;dialogHide:true;help:no;scroll:no');
                });
            });        
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
                            Reporte de cuentas ingresadas en PAE
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
                                            <asp:ImageButton ID="Btn_Exportar_pdf" runat="server" ToolTip="Exportar a pdf" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" AlternateText="Exportar a pdf"
                                                OnClick="Btn_Exportar_pdf_Click" />
                                            <asp:ImageButton ID="Btn_Exportar_Excel" runat="server" ToolTip="Exportar a Excel"
                                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png"
                                                AlternateText="Exportar a Excel" OnClick="Btn_Exportar_Excel_Click" />
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
                                                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="20" ToolTip="Buscar" Width="180px" />
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Folio>" TargetControlID="Txt_Busqueda" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                                            FilterType="Numbers" />
                                                    </td>
                                                    <td style="vertical-align: middle; width: 5%;">
                                                        <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                            ToolTip="Buscar" OnClick="Btn_Buscar_Click" />
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
                                Etapa PAE
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Etapa_PAE" runat="server" Width="99%">
                                    <asp:ListItem Text="&lt;--SELECCIONE--&gt;" Value="0" />
                                    <asp:ListItem Text="DETERMINACI&Oacute;N" Value="DETERMINACION" />
                                    <asp:ListItem Text="REQUERIMIENTO" Value="REQUERIMIENTO" />
                                    <asp:ListItem Text="EMBARGO" Value="EMBARGO" />
                                    <asp:ListItem Text="REMOCI&Oacute;N" Value="REMOCION" />
                                    <asp:ListItem Text="ALMONEDA" />
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Fecha de ingreso
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Fecha_Ingreso" runat="server" Width="99%">
                                </asp:DropDownList>
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
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Fecha_Inicial" runat="server" TargetControlID="Txt_Fecha_Inicial"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ/-" />
                                <asp:ImageButton ID="Btn_Txt_Fecha_Inicial" runat="server" CausesValidation="false"
                                    Height="18px" ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" />
                            </td>
                            <td style="text-align: left; width: 20%; text-align: right;">
                                Fecha final
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Fecha_Final" runat="server" MaxLength="11" Width="84%" AutoPostBack="true" OnTextChanged="Txt_Fecha_Final_TextChanged"/>
                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Fecha_Final" runat="server" Enabled="True"
                                    TargetControlID="Txt_Fecha_Final" WatermarkCssClass="watermarked" WatermarkText="Día/Mes/Año" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Final" runat="server" Enabled="True" Format="dd/MMM/yyyy"
                                    PopupButtonID="Btn_Txt_Fecha_Final" TargetControlID="Txt_Fecha_Final" />
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Fecha_Final" runat="server" TargetControlID="Txt_Fecha_Final"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ/-" />
                                <asp:ImageButton ID="Btn_Txt_Fecha_Final" runat="server" CausesValidation="false"
                                    Height="18px" ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: right;">
                                <asp:Label ID="Lbl_Busqueda" runat="server" Text="Buscar"></asp:Label>
                                <asp:ImageButton ID="Btn_Buscar_Cuentas_PAE" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png"
                                    OnClick="Btn_Buscar_Cuentas_PAE_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <%---------------- Cuentas omitidas ----------------%>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Cuentas omitidas
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="4">
                                <asp:GridView ID="Grid_Cuentas_Omitidas" runat="server" AllowPaging="True" AllowSorting="True"
                                    AutoGenerateColumns="False" CssClass="GridView_1" HeaderStyle-CssClass="tblHead"
                                    PageSize="5" Style="white-space: normal;" Width="100%" OnSelectedIndexChanged="Grid_Cuentas_Omitidas_SelectedIndexChanged"
                                    OnPageIndexChanging="Grid_Cuentas_Omitidas_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Detalle_Cuenta" runat="server" Height="20px" ImageUrl="~/paginas/imagenes/gridview/blue_button_.png"
                                                    CommandName="Select" ToolTip="Detalles Cuenta" Width="20px" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cuenta">
                                            <ItemStyle HorizontalAlign="right" />
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="Btn_Link_Cuenta" class="Detalles_Gastos" id_cuenta='<%# Eval("CUENTA_PREDIAL") %>'
                                                    Text='<%# Eval("CUENTA_PREDIAL") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ADEUDO_TOTAL" HeaderText="Adeudo" DataFormatString="{0:C2}">
                                            <ItemStyle HorizontalAlign="right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FOLIO" HeaderText="Folio">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA_PROCESO_CAMBIO" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PROCESO_ACTUAL" HeaderText="Proceso actual">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MOTIVO_OMISION" HeaderText="Motivo omisión">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CUENTA_PREDIAL_ID" Visible="false" />
                                        <asp:BoundField DataField="NO_DETALLE_ETAPA" Visible="false" />
                                        <asp:BoundField DataField="DESPACHO" Visible="false" />
                                        <asp:BoundField DataField="NUMERO_ENTREGA" Visible="false" />
                                        <asp:BoundField DataField="PERIODO_CORRIENTE" Visible="false" />
                                        <asp:BoundField DataField="PERIODO_REZAGO" Visible="false" />
                                        <asp:BoundField DataField="ADEUDO_CORRIENTE" Visible="false" />
                                        <asp:BoundField DataField="ADEUDO_REZAGO" Visible="false" />
                                        <asp:BoundField DataField="ADEUDO_RECARGOS_ORDINARIOS" Visible="false" />
                                        <asp:BoundField DataField="ADEUDO_RECARGOS_MORATORIOS" Visible="false" />
                                        <asp:BoundField DataField="ADEUDO_HONORARIOS" Visible="false" />
                                        <asp:BoundField DataField="MULTAS" Visible="false" />
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                            <td style="text-align: right;">
                                Total omitidas
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="Txt_Total_Cuentas_Omitidas" runat="server" Width="96.4%" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <%---------------- Cuentas ingresadas ----------------%>
                        <tr style="background-color: #36C;">
                            <td style="text-align: left; font-size: 15px; color: #FFF;" colspan="4">
                                Cuentas ingresadas
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="4">
                                <asp:GridView ID="Grid_Cuentas_Ingresadas" runat="server" AllowPaging="True" AllowSorting="True"
                                    AutoGenerateColumns="False" CssClass="GridView_1" HeaderStyle-CssClass="tblHead"
                                    PageSize="5" Style="white-space: normal;" Width="100%" OnSelectedIndexChanged="Grid_Cuentas_Ingresadas_SelectedIndexChanged"
                                    OnPageIndexChanging="Grid_Cuentas_Ingresadas_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Detalle_Cuenta" runat="server" Height="20px" ImageUrl="~/paginas/imagenes/gridview/blue_button_.png"
                                                    CommandName="Select" ToolTip="Detalles Cuenta" Width="20px" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cuenta">
                                            <ItemStyle HorizontalAlign="right" />
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="Btn_Link_Cuenta" class="Detalles_Gastos" id_cuenta='<%# Eval("CUENTA_PREDIAL") %>'
                                                    Text='<%# Eval("CUENTA_PREDIAL") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ADEUDO_TOTAL" HeaderText="Adeudo" DataFormatString="{0:C2}">
                                            <ItemStyle HorizontalAlign="right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FOLIO" HeaderText="Folio">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA_PROCESO_CAMBIO" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PROCESO_ACTUAL" HeaderText="Proceso actual">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CUENTA_PREDIAL_ID" Visible="false" />
                                        <asp:BoundField DataField="NO_DETALLE_ETAPA" Visible="false" />
                                        <asp:BoundField DataField="DESPACHO" Visible="false" />
                                        <asp:BoundField DataField="NUMERO_ENTREGA" Visible="false" />
                                        <asp:BoundField DataField="PERIODO_CORRIENTE" Visible="false" />
                                        <asp:BoundField DataField="PERIODO_REZAGO" Visible="false" />
                                        <asp:BoundField DataField="ADEUDO_CORRIENTE" Visible="false" />
                                        <asp:BoundField DataField="ADEUDO_REZAGO" Visible="false" />
                                        <asp:BoundField DataField="ADEUDO_RECARGOS_ORDINARIOS" Visible="false" />
                                        <asp:BoundField DataField="ADEUDO_RECARGOS_MORATORIOS" Visible="false" />
                                        <asp:BoundField DataField="ADEUDO_HONORARIOS" Visible="false" />
                                        <asp:BoundField DataField="MULTAS" Visible="false" />
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                            <td style="text-align: right;">
                                Total omitidas
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="Txt_Total_Cuentas_Ingresadas" runat="server" Width="96.4%" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                Se asignó a
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="Txt_Despacho_Asignado" runat="server" Width="96.4%" Enabled="false" />
                            </td>
                            <td style="text-align: right;">
                                Numero de entrega
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="Txt_Numero_Entrega" runat="server" Width="96.4%" Enabled="false" />
                            </td>
                        </tr>
                    </table>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

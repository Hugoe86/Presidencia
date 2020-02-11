<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Listado_Adeudos_Predial.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Listado_Adeudos_Predial"
    Title="Listado de Adeudos de Predial" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">

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
        setInterval("MantenSesion()", "<%=(int)(0.9*(Session.Timeout * 60000))%>");
        
    //Abrir una ventana modal   
	function Abrir_Ventana_Modal(Url, Propiedades) {
		window.showModalDialog(Url, null, Propiedades);
    }
    function Abrir_Resumen(Url, Propiedades) {
        window.open(Url, 'Resumen_Predio_Caja', Propiedades);
    }
    function Abrir_Ventana_Estado_Cuenta(Url, Propiedades) {
        window.open(Url, 'Estado_Cuenta_Caja', Propiedades);
    }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScptM_Listado_Adeudos" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="99%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="2">
                            Caja
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
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
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" style="width: 50%">
                            &nbsp;
                            <asp:ImageButton ID="Btn_Resumen_Cuenta" runat="server" Height="24px" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png"
                                ToolTip="Resumen de Predio" Width="24px" />
                            <asp:ImageButton ID="Btn_Estado_Cuenta" runat="server" ToolTip="Estado de Cuenta"
                                ImageUrl="~/paginas/imagenes/paginas/Listado.png" Height="24px" Width="24px" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                TabIndex="3" CausesValidation="false" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                OnClick="Btn_Salir_Click" />
                        </td>
                        <td style="width: 50%">
                            <asp:ImageButton ID="Btn_Limpiar_Formulario" runat="server" AlternateText="Limpiar Todo"
                                ToolTip="Limpiar Todo" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png"
                                Width="24px" OnClick="Btn_Limpiar_Formulario_Click" />
                            &nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
                <br />
                <center>
                    <table width="98%">
                        <tr>
                            <td colspan="4">
                                <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                                <asp:HiddenField ID="Hdf_Tipo_Suspension" runat="server" />
                                <asp:Label ID="Lbl_Estatus" Style="color: Red" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 18%; text-align: left;">
                                <asp:Label ID="Lbl_Cuenta_Predial" runat="server" Text="Cuenta Predial" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 36%; text-align: left; vertical-align: middle;">
                                <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="80%" BorderWidth="1" MaxLength="12"
                                    Style="font-weight: bolder; text-transform: uppercase" AutoPostBack="true" OnTextChanged="Txt_Cuenta_Predial_TextChanged"
                                    Font-Bold="True"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Cuenta_Predial" runat="server" WatermarkText="<- Seleccionar ó Introducir ->"
                                    TargetControlID="Txt_Cuenta_Predial" WatermarkCssClass="watermarked">
                                </cc1:TextBoxWatermarkExtender>
                                <asp:ImageButton ID="Btn_Busqueda_Avanzada_Cuenta_Predial" runat="server" AlternateText="Busqueda Avanzada"
                                    ToolTip="Busqueda Avanzada" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png"
                                    Width="24px" OnClick="Btn_Busqueda_Avanzada_Cuenta_Predial_Click" />
                            </td>
                            <td style="width: 18%; text-align: left;">
                                <asp:Label ID="Lbl_No_Folio" runat="server" Text="No. Folio" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 36%; text-align: left; vertical-align: middle;">
                                <asp:TextBox ID="Txt_No_Folio" runat="server" Width="97.5%" BorderWidth="1" AutoPostBack="true"
                                    OnTextChanged="Txt_No_Folio_TextChanged" Style="font-weight: bolder; text-transform: uppercase"
                                    Font-Bold="True"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_No_Folio" runat="server" WatermarkText="<- Introducir ->"
                                    TargetControlID="Txt_No_Folio" WatermarkCssClass="watermarked">
                                </cc1:TextBoxWatermarkExtender>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Folio" runat="server" TargetControlID="Txt_No_Folio"
                                    InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_/" Enabled="True">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 18%; text-align: left;">
                                <asp:Label ID="Lbl_Clave_Ingreso" runat="server" Text="Clave Ingreso" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width: 36%; text-align: left; vertical-align: middle;">
                                <asp:TextBox ID="Txt_Clave_Ingreso" runat="server" Width="97.5%" BorderWidth="1"
                                    AutoPostBack="true" OnTextChanged="Txt_Clave_Ingreso_TextChanged" Style="font-weight: bolder;
                                    text-transform: uppercase" Font-Bold="True"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Clave_Ingreso" runat="server" WatermarkText="<- Introducir ->"
                                    TargetControlID="Txt_Clave_Ingreso" WatermarkCssClass="watermarked">
                                </cc1:TextBoxWatermarkExtender>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Clave_Ingreso" runat="server" TargetControlID="Txt_Clave_Ingreso"
                                    InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_/" Enabled="True">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div id="Div_Datos_Cuenta_Predial" runat="server" style="width: 99.5%;">
                                    <table width="100%">
                                        <tr style="background-color: #36C;">
                                            <td style="text-align: left; font-size: 15px; color: #FFF; font-weight: bolder;"
                                                colspan="2">
                                                Datos Generales
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 18%; text-align: left;">
                                                <asp:Label ID="Lbl_Propietario_Cuenta" runat="server" Text="Propietario" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="Txt_Propietario_Cuenta" runat="server" Width="99%" BorderWidth="1"
                                                    Font-Bold="True" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 18%; text-align: left;">
                                                <asp:Label ID="Lbl_Ubicacion_Cuenta" runat="server" Text="Ubicación" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="Txt_Ubicacion_Cuenta" runat="server" Width="99%" BorderWidth="1"
                                                    Font-Bold="True" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div id="Div_Detalles_Clave_Ingreso" runat="server" style="width: 99.5%;">
                                    <table width="100%">
                                        <tr style="background-color: #36C;">
                                            <td style="text-align: left; font-size: 15px; color: #FFF; font-weight: bolder;"
                                                colspan="2">
                                                <asp:HiddenField ID="Hdf_Clave_Ingreso_ID" runat="server" />
                                                <asp:HiddenField ID="Hdf_Dependencia_ID" runat="server" />
                                                Detalles de la Clave de Ingreso
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 18%; text-align: left;">
                                                <asp:Label ID="Lbl_Clave_Ingreso_Descripcion" runat="server" Text="Descripción" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="Txt_Clave_Ingreso_Descripcion" runat="server" Width="99%" BorderWidth="1"
                                                    Font-Bold="True" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 18%; text-align: left;">
                                                <asp:Label ID="Lbl_Fundamento" runat="server" Text="Fundamento" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="Txt_Clave_Ingreso_Fundamento" runat="server" Width="99%" BorderWidth="1"
                                                    Font-Bold="True" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div id="Div_Datos_Constribuyente" runat="server" style="width: 99.5%;">
                                    <table width="100%">
                                        <tr style="background-color: #36C;">
                                            <td style="text-align: left; font-size: 15px; color: #FFF; font-weight: bolder;"
                                                colspan="4">
                                                Datos del Contribuyente
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 18%; text-align: left;">
                                                <asp:Label ID="Lbl_Nombre_Contribuyente" runat="server" Text="* Nombre" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="text-align: left;" colspan="3">
                                                <asp:TextBox ID="Txt_Nombre_Contribuyente" runat="server" Width="99%" BorderWidth="1"
                                                    Enabled="true" Style="text-transform: uppercase"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Contribuyente" runat="server" TargetControlID="Txt_Nombre_Contribuyente"
                                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 18%; text-align: left;">
                                                <asp:Label ID="Lbl_Observaciones" runat="server" Text="Observaciones" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="text-align: left;" colspan="3">
                                                <asp:TextBox ID="Txt_Observaciones" runat="server" Width="99%" BorderWidth="1" Enabled="true"
                                                    TextMode="MultiLine" MaxLength="250" Style="text-transform: uppercase"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Observaciones" runat="server" TargetControlID="Txt_Observaciones"
                                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ$/# " />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div id="Div_Listado_Adeudos_A_Pagar" runat="server" style="width: 99.5%;">
                                    <table width="100%">
                                        <tr style="background-color: #36C;">
                                            <td style="text-align: left; font-size: 15px; color: #FFF; font-weight: bolder;">
                                                Listado de Conceptos con Opción a Pagar
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="Div_Listado_Adeudos" runat="server" style="width: 99%; height: 370px; overflow: auto;
                                                    border-width: thick; border-color: Black;">
                                                    <asp:GridView ID="Grid_Listado_Adeudos" runat="server" CssClass="GridView_1" AutoGenerateColumns="False"
                                                        Width="100%" EmptyDataText="No se han encontrado Registros a Pagar" AllowSorting="false"
                                                        HeaderStyle-CssClass="tblHead" Style="white-space: normal;" OnRowDataBound="Grid_Listado_Adeudos_RowDataBound">
                                                        <RowStyle Font-Size="Medium" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chk_Seleccion_Adeudo" runat="server" Checked="false" AutoPostBack="True"
                                                                        OnCheckedChanged="Chk_Seleccion_Adeudo_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle Font-Size="Medium" HorizontalAlign="Center" Width="15px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="IDENTIFICADOR" SortExpression="IDENTIFICADOR" HeaderText="IDENTIFICADOR">
                                                                <ItemStyle Font-Size="Medium" HorizontalAlign="Center" Width="5px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TIPO_CONCEPTO" SortExpression="TIPO_CONCEPTO" HeaderText="TIPO_CONCEPTO">
                                                                <ItemStyle Font-Size="Medium" HorizontalAlign="Center" Width="5px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="REFERENCIA" SortExpression="REFERENCIA" HeaderText="No. Folio">
                                                                <ItemStyle Font-Size="Medium" HorizontalAlign="Center" Width="140px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FECHA" SortExpression="FECHA" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}">
                                                                <ItemStyle Font-Size="Medium" HorizontalAlign="Center" Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DESCRIPCION" SortExpression="DESCRIPCION" HeaderText="Descripción">
                                                                <ItemStyle Font-Size="Medium" HorizontalAlign="Center" Wrap="false" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="IMPORTE" SortExpression="IMPORTE" HeaderText="Importe"
                                                                DataFormatString="{0:c}">
                                                                <ItemStyle Font-Size="Medium" HorizontalAlign="Center" Wrap="false" Width="90px" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="GridSelected" />
                                                        <PagerStyle CssClass="GridHeader" />
                                                        <HeaderStyle CssClass="tblHead" Font-Size="Medium" />
                                                        <AlternatingRowStyle CssClass="GridAltItem" Font-Size="Medium" />
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div id="Div_Detalles_Pago_Clave_Ingreso" runat="server" style="width: 99.5%;">
                                    <table width="100%">
                                        <tr style="background-color: #36C;">
                                            <td style="text-align: left; font-size: 15px; color: #FFF; font-weight: bolder;"
                                                colspan="7">
                                                Detalles de Pago
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%; text-align: left;" colspan="1">
                                                <asp:Label ID="Lbl_Cantidad" runat="server" Text="Cantidad" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 10px; text-align: left;" colspan="1">
                                            </td>
                                            <td style="width: 10%; text-align: left;" colspan="1">
                                                <asp:Label ID="Lbl_Costo_Unidad" runat="server" Text="Costo/U [$]" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 10px; text-align: left;" colspan="1">
                                            </td>
                                            <td style="width: 10%; text-align: left;" colspan="1">
                                                <asp:Label ID="Lbl_Total_Pago" runat="server" Text=" Total" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%; text-align: left;">
                                                <asp:TextBox ID="Txt_Cantidad" runat="server" Width="99%" BorderWidth="1" Style="text-align: center;
                                                    font-weight: bolder;" OnTextChanged="Txt_Cantidad_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cantidad" runat="server" TargetControlID="Txt_Cantidad"
                                                    FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 10px; text-align: left;" colspan="1">
                                            </td>
                                            <td style="width: 15%; text-align: left;">
                                                <asp:TextBox ID="Txt_Costo_Unidad" runat="server" Width="99%" BorderWidth="1" Style="text-align: right;
                                                    font-weight: bolder;" OnTextChanged="Txt_Costo_Unidad_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Costo_Unidad" runat="server" TargetControlID="Txt_Cantidad"
                                                    FilterType="Numbers, Custom" ValidChars=".">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 10px; text-align: left;" colspan="1">
                                            </td>
                                            <td style="width: 15%; text-align: left;">
                                                <asp:TextBox ID="Txt_Total_Pago" runat="server" Width="99%" BorderWidth="1" Enabled="false"
                                                    Style="text-align: right; font-weight: bolder; color: Red;"></asp:TextBox>
                                            </td>
                                            <td style="width: 10px; text-align: left;" colspan="1">
                                            </td>
                                            <td style="width: 25%; text-align: right;">
                                                &nbsp;&nbsp;
                                                <asp:Button ID="Btn_Pagar_Caja" runat="server" Text="Pagar" Width="85%" OnClick="Btn_Pagar_Caja_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>
                    </table>
                    <br />
                </center>
            </div>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

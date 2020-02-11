<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Registro_Peticion.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Ope_Registro_Peticion"
    Title="" Culture="es-MX" UICulture="es" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script type="text/javascript" language="javascript">
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades)
        {
            window.showModalDialog(Url, null, Propiedades);
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" AsyncPostBackTimeout="3600" />
    <div style="width: 95%;">
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table width="100%" cellspacing="0">
                    <tr>
                        <td colspan="4" align="center" class="label_titulo">
                            Registro de Peticiones Ciudadanas
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Image ID="Img_Informacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />
                            <asp:Label ID="Lbl_Mensaje" runat="server" Text="" Visible="false" CssClass="estilo_fuente_mensaje_error">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td colspan="2" align="left" valign="middle">
                            <div>
                                &nbsp;
                                <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                    CssClass="Img_Button" OnClick="Btn_Nuevo_Click" ToolTip="Nuevo" CausesValidation="False" />
                                <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                    CssClass="Img_Button" OnClick="Btn_Modificar_Click" AlternateText="Modificar"
                                    ToolTip="Modificar" />
                                <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir listado de vacantes"
                                    CssClass="Img_Button" ImageUrl="~/paginas/imagenes/gridview/grid_print.png" OnClick="Btn_Imprimir_Click" />
                                <asp:ImageButton ID="Btn_Formato_Consecutivo" runat="server" ToolTip="Generar formato Consecutivo"
                                    CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_rep_word.png"
                                    OnClick="Btn_Formato_Consecutivo_Click" Visible="false" />
                                <asp:ImageButton ID="Btn_Salir" runat="server" OnClick="Btn_Salir_Click" CssClass="Img_Button"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" />
                            </div>
                        </td>
                        <td colspan="2">
                            Búsqueda
                            <asp:TextBox ID="Txt_Busqueda_Registro_Peticion" runat="server" Width="180" MaxLength="20"
                                ToolTip="Buscar por Folio."></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Registro_Peticion" runat="server"
                                WatermarkCssClass="watermarked" WatermarkText="<Ingrese Folio de petición>" TargetControlID="Txt_Busqueda_Registro_Peticion" />
                            <cc1:FilteredTextBoxExtender ID="Ftbe_Busqueda_Registro_Petision" runat="server"
                                TargetControlID="Txt_Busqueda_Registro_Peticion" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                ValidChars="-">
                            </cc1:FilteredTextBoxExtender>
                            <asp:ImageButton ID="Btn_Buscar_Registro_Peticion" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                OnClick="Btn_Buscar_Registro_Peticion_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%;">
                            Folio
                        </td>
                        <td style="width: 35%;">
                            <asp:TextBox ID="Txt_Folio" runat="server" Width="90%" MaxLength="20" Enabled="false"></asp:TextBox>
                            <asp:HiddenField ID="Hdn_Estatus" runat="server" />
                        </td>
                        <td style="width: 15%; text-align: right;">
                            *Origen
                        </td>
                        <td style="width: 35%;">
                            <asp:DropDownList ID="Cmb_Origen" runat="server" Width="92%" AutoPostBack="true"
                                OnSelectedIndexChanged="Cmb_Origen_SelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%;">
                            Fecha
                        </td>
                        <td style="width: 35%;">
                            <asp:TextBox ID="Txt_Fecha_Peticion" runat="server" Width="90%" Enabled="false"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="Tr_Contenedor_Combo_Consecutivo" runat="server" visible="false">
                        <td style="width: 15%;">
                            *Seguimiento consecutivo
                        </td>
                        <td style="width: 35%;">
                            <asp:DropDownList ID="Cmb_Seguimiento_Consecutivo" runat="server" Style="width: 95%;
                                position: relative; right: 4px;">
                                <asp:ListItem Value=""> &lt;Seleccione&gt; </asp:ListItem>
                                <asp:ListItem Value="RESOLUCION">Analizar la petición y brindar resolución.</asp:ListItem>
                                <asp:ListItem Value="CONOCIMIENTO">Para conocimiento.</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr class="linea" />
                        </td>
                    </tr>
                </table>
                <fieldset>
                    <legend>Información personal</legend>
                    <table width="100%" cellspacing="0">
                        <tr>
                            <td style="width: 15%;">
                                *Apellido Paterno
                            </td>
                            <td style="width: 35%;">
                                <asp:TextBox ID="Txt_Apellido_Paterno" runat="server" MaxLength="50" Width="85%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Ftbe_Apellido_Paterno" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                    TargetControlID="Txt_Apellido_Paterno" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                </cc1:FilteredTextBoxExtender>
                                <asp:ImageButton ID="Btn_Buscar_Ciudadano" runat="server" ToolTip="Búsqueda de usuarios registrados"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                    OnClick="Btn_Buscar_Ciudadano_Click" />
                                <asp:HiddenField ID="Hdn_Ciudadano_Id" runat="server" />
                                <asp:HiddenField ID="Hdn_Atendio" runat="server" />
                                <asp:HiddenField ID="Hdn_Programa_Atendido_Direcciones" runat="server" />
                            </td>
                            <td style="width: 15%; text-align: right;">
                                Apellido Materno
                            </td>
                            <td style="width: 35%;">
                                <asp:TextBox ID="Txt_Apellido_Materno" runat="server" MaxLength="50" Width="90%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Ftbe_Apellido_Materno" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                    TargetControlID="Txt_Apellido_Materno" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;">
                                *Nombre
                            </td>
                            <td style="width: 35%;">
                                <asp:TextBox ID="Txt_Nombre" runat="server" MaxLength="50" Width="90%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Ftbe_Nombre" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                    TargetControlID="Txt_Nombre" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="width: 15%; text-align: right;">
                                *Sexo
                            </td>
                            <td style="width: 35%;">
                                <asp:DropDownList ID="Cmb_Sexo" runat="server" Width="93%">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;">
                                Edad
                            </td>
                            <td style="width: 35%;">
                                <asp:TextBox ID="Txt_Edad" runat="server" MaxLength="3" Width="90%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Ftbe_Edad" runat="server" FilterType="Numbers" TargetControlID="Txt_Edad">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="width: 15%; text-align: right;">
                                Fecha de nacimiento
                            </td>
                            <td style="width: 35%;">
                                <asp:TextBox ID="Txt_Fecha_Nacimiento" runat="server" MaxLength="15" Width="83%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Fecha_Nacimiento" runat="server" TargetControlID="Txt_Fecha_Nacimiento"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="/- ">
                                </cc1:FilteredTextBoxExtender>
                                <cc1:calendarextender id="Cln_Txt_Fecha_Nacimiento" runat="server" targetcontrolid="Txt_Fecha_Nacimiento"
                                    format="dd/MMM/yyyy" popupbuttonid="Btn_Cln_Txt_Fecha_Nacimiento">
                                </cc1:calendarextender>
                                <asp:ImageButton ID="Btn_Cln_Txt_Fecha_Nacimiento" runat="server" CausesValidation="false"
                                    Height="18px" ImageUrl="../imagenes/paginas/SmallCalendar.gif" Style="vertical-align: top;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;">
                                *Colonia
                            </td>
                            <td style="width: 35%;">
                                <asp:DropDownList ID="Cmb_Colonia" runat="server" Width="85%" OnSelectedIndexChanged="Cmb_Colonia_SelectedIndexChanged"
                                    AutoPostBack="true" />
                                <asp:ImageButton ID="Btn_Buscar_Colonia" runat="server" ToolTip="Seleccionar Colonia"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                    OnClick="Btn_Buscar_Colonia_Click" />
                            </td>
                            <td style="text-align: right;">
                                *Calle
                            </td>
                            <td style="width: 35%;">
                                <asp:DropDownList ID="Cmb_Calle" runat="server" Width="85%" />
                                <asp:ImageButton ID="Btn_Buscar_Calle" runat="server" ToolTip="Seleccionar Calle"
                                    ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                    OnClick="Btn_Buscar_Calles_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;">
                                Número exterior
                            </td>
                            <td style="width: 35%;">
                                <asp:TextBox ID="Txt_Numero_Exterior" runat="server" MaxLength="20" Width="90%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Numero_Exterior" runat="server" TargetControlID="Txt_Numero_Exterior"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ.,;:()#@- ">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="text-align: right;">
                                Número interior
                            </td>
                            <td style="width: 35%;">
                                <asp:TextBox ID="Txt_Numero_Interior" runat="server" MaxLength="20" Width="90%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Numero_Interior" runat="server" TargetControlID="Txt_Numero_Interior"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ.,;:()#@- ">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;">
                                Referencia
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Referencia" runat="server" MaxLength="100" Width="96%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Referencia" runat="server" TargetControlID="Txt_Referencia"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ.,;:()#@- ">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;">
                                Código Postal
                            </td>
                            <td style="width: 35%;">
                                <asp:TextBox ID="Txt_Codigo_Postal" runat="server" MaxLength="10" Width="90%"></asp:TextBox>
                            </td>
                            <td style="text-align: right;">
                                E-mail
                            </td>
                            <td style="width: 35%;">
                                <asp:TextBox ID="Txt_Email" runat="server" MaxLength="50" Width="90%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Ftbe_Email" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                    TargetControlID="Txt_Email" ValidChars="-_.@">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;">
                                Teléfono
                            </td>
                            <td style="width: 35%;">
                                <asp:TextBox ID="Txt_Telefono" runat="server" MaxLength="20" Width="90%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Ftbe_Telefono" runat="server" TargetControlID="Txt_Telefono"
                                    FilterType="Custom, Numbers" ValidChars="-()">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table width="100%" cellspacing="0">
                    <tr>
                        <td colspan="4">
                            <hr class="linea" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%;" valign="top">
                            *Petición
                        </td>
                        <td style="width: 85%;" colspan="3">
                            <asp:TextBox ID="Txt_Peticion" runat="server" TextMode="MultiLine" Width="96%" MaxLength="4000"
                                Height="65"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Peticion" runat="server" TargetControlID="Txt_Peticion"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ.,;:()#¿?!¡@$+-/* ">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Peticion" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Límite de Caracteres - 4000>" TargetControlID="Txt_Peticion" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%;">
                            *Asunto
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="Cmb_Asunto" runat="server" Width="94%" AutoPostBack="true"
                                OnSelectedIndexChanged="Cmb_Asunto_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:ImageButton ID="Btn_Buscar_Asunto" runat="server" ToolTip="Seleccionar Asunto"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                OnClick="Btn_Buscar_Asunto_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%;">
                            *Unidad Responsable
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="94%" AutoPostBack="true"
                                OnSelectedIndexChanged="Cmb_Dependencia_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:ImageButton ID="Btn_Buscar_Dependencia" runat="server" ToolTip="Seleccionar Unidad responsable"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                OnClick="Btn_Buscar_Dependencia_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%;">
                            *Acción
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="Cmb_Accion" runat="server" Width="94%" AutoPostBack="true"
                                OnSelectedIndexChanged="Cmb_Accion_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:ImageButton ID="Btn_Buscar_Accion" runat="server" ToolTip="Seleccionar Acción"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                OnClick="Btn_Buscar_Accion_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%;">
                            *Fecha de Solución
                        </td>
                        <td style="width: 35%;">
                            <asp:TextBox ID="Txt_Fecha_Solucion" runat="server" Width="90%"></asp:TextBox>
                            <cc1:CalendarExtender ID="TextBox1_CalendarExtender" runat="server" Format="dd/MMM/yyyy"
                                TargetControlID="Txt_Fecha_Solucion">
                            </cc1:CalendarExtender>
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:CheckBox ID="Chk_Terminar_Peticion" runat="server" Text="Terminar petición"
                                Enabled="false" AutoPostBack="true" OnCheckedChanged="Chk_Terminar_Peticion_CheckedChanged" />
                            <asp:DropDownList ID="Cmb_Tipo_Solucion" runat="server" Width="32%" Enabled="false">
                                <asp:ListItem Value=""> &lt;Seleccione&gt; </asp:ListItem>
                                <asp:ListItem Value="POSITIVA">POSITIVA</asp:ListItem>
                                <asp:ListItem Value="NEGATIVA">NEGATIVA</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%;" valign="top">
                            Solución
                        </td>
                        <td style="width: 85%;" colspan="3">
                            <asp:TextBox ID="Txt_Descripcion_Solucion" runat="server" TextMode="MultiLine" Width="97%"
                                MaxLength="3000" Height="65" Enabled="false"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Descripcion_Solucion" runat="server" TargetControlID="Txt_Descripcion_Solucion"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ.,;:()#¿?!¡@$+-/* ">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Descripcion_Solucion" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Límite de Caracteres - 4000>" TargetControlID="Txt_Descripcion_Solucion" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <hr class="linea" />
                        </td>
                    </tr>
                </table>
                <div id="Div_Contenido" runat="server" style="max-height: 200px; overflow-x: hidden;
                    overflow-y: auto;">
                    <table style="width: 99%;">
                        <tr>
                            <td align="center">
                                <asp:GridView ID="Grid_Peticiones" runat="server" AllowPaging="false" AutoGenerateColumns="false"
                                    GridLines="None" PageSize="3" Width="99%" OnSelectedIndexChanged="Grid_Peticiones_SelectedIndexChanged"
                                    AllowSorting="true" HeaderStyle-CssClass="tblHead" OnSorting="Grid_Peticiones_Sorting"
                                    Style="font-size: xx-small; white-space: normal;">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="NO_PETICION" Visible="false" />
                                        <asp:BoundField DataField="ANIO_PETICION" Visible="false" />
                                        <asp:BoundField DataField="PROGRAMA_ID" Visible="false" />
                                        <asp:BoundField DataField="ESTATUS" Visible="false" />
                                        <asp:BoundField DataField="FOLIO" HeaderText="Folio" SortExpression="FOLIO">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA_PETICION" HeaderText="Fecha petición" DataFormatString="{0:dd/MMM/yyyy}"
                                            SortExpression="FECHA_PETICION">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_COMPLETO_SOLICITANTE" HeaderText="Solicitante"
                                            SortExpression="NOMBRE_COMPLETO_SOLICITANTE">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPCION_PETICION" HeaderText="Petición" SortExpression="DESCRIPCION_PETICION">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ASUNTO" HeaderText="Asunto" SortExpression="ASUNTO">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
                <div runat="server" id="Div_Contenedor_Grid_Peticiones_Recientes" style="clear: both;
                    display: none;">
                    <table width="100%">
                        <tr>
                            <td class="label_titulo">
                                <asp:Label ID="Lbl_Titulo_Grid_Peticiones_Recientes" runat="server" Text="Peticiones recientes"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Lbl_Cmb_Cantidad_Peticiones_Mostrar" runat="server" Text="Mostrar"></asp:Label>
                                <asp:DropDownList ID="Cmb_Cantidad_Peticiones_Mostrar" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="Cmb_Cantidad_Peticiones_Mostrar_SelectedIndexChanged">
                                    <asp:ListItem Text="Últimas 10 peticiones" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="Últimas 15 peticiones" Value="15"></asp:ListItem>
                                    <asp:ListItem Text="Últimas 20 peticiones" Value="20"></asp:ListItem>
                                    <asp:ListItem Text="Últimas 25 peticiones" Value="25"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="label_titulo">
                                <asp:GridView ID="Grid_Peticiones_Recientes" runat="server" Height="100%" AutoGenerateColumns="False"
                                    CssClass="GridView_1" AllowPaging="false" GridLines="None" Font-Underline="False"
                                    EmptyDataText="No se encontraron registros" OnSelectedIndexChanged="Grid_Peticiones_Recientes_SelectedIndexChanged"
                                    Style="font-size: xx-small; white-space: normal">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="NO_PETICION" Visible="false" />
                                        <asp:BoundField DataField="ANIO_PETICION" Visible="false" />
                                        <asp:BoundField DataField="PROGRAMA_ID" Visible="false" />
                                        <asp:BoundField DataField="FECHA_PETICION" HeaderText="Fecha" Visible="True" DataFormatString="{0:dd/MMM/yyyy}">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FOLIO" HeaderText="Folio" Visible="True">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_COMPLETO_SOLICITANTE" HeaderText="Solicitante"
                                            Visible="True">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" Wrap="True" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <br />
</asp:Content>

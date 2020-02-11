<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Canalizar_Peticiones.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Ope_Canalizar_Peticiones"
    Title="" %>

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
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
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
                            Canalizar Peticiones Ciudadanas
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
                                    CssClass="Img_Button" OnClick="Btn_Nuevo_Click" ToolTip="Nuevo" CausesValidation="False" Visible="false" Enabled="false" />
                                <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                    CssClass="Img_Button" OnClick="Btn_Modificar_Click" AlternateText="Modificar"
                                    ToolTip="Modificar" />
                                <asp:ImageButton ID="Btn_Salir" runat="server" OnClick="Btn_Salir_Click" CssClass="Img_Button"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" />
                            </div>
                        </td>
                        <td colspan="2">
                            Búsqueda
                            <asp:TextBox ID="Txt_Busqueda_Registro_Peticion" runat="server" Width="180" MaxLength="13"
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
                        <td style="width: 18%;">
                            Folio
                        </td>
                        <td style="width: 32%;">
                            <asp:TextBox ID="Txt_Folio" runat="server" Width="90%" MaxLength="15" Enabled="false"></asp:TextBox>
                        </td>
                        <td style="width: 18%;">
                            *Origen
                        </td>
                        <td style="width: 32%;">
                            <asp:DropDownList ID="Cmb_Origen" runat="server" Width="92%" />
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
                                <asp:TextBox ID="Txt_Apellido_Paterno" runat="server" MaxLength="50" Width="90%"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Ftbe_Apellido_Paterno" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                    TargetControlID="Txt_Apellido_Paterno" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="width: 15%;">
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
                            <td style="width: 15%;">
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
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;">
                                *Colonia
                            </td>
                            <td style="width: 35%;">
                                <asp:DropDownList ID="Cmb_Colonia" runat="server" Width="93%" OnSelectedIndexChanged="Cmb_Colonia_SelectedIndexChanged"
                                    AutoPostBack="true" />
                            </td>
                            <td style="width: 15%;">
                                *Calle
                            </td>
                            <td style="width: 35%;">
                                <asp:DropDownList ID="Cmb_Calle" runat="server" Width="93%" />
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
                            <td style="width: 15%;">
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
                            <td style="width: 15%;">
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
                            <asp:TextBox ID="Txt_Peticion" runat="server" TextMode="MultiLine" Width="97%" MaxLength="4000"
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
                        <td colspan="4" align="center">
                            <hr class="linea" />
                        </td>
                    </tr>
                </table>
                <div id="Div_Contenido" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td align="center" colspan="4">
                                <asp:GridView ID="Grid_Peticiones" runat="server" AllowPaging="false" AutoGenerateColumns="false"
                                    GridLines="None" PageSize="3" Width="99%" OnSelectedIndexChanged="Grid_Peticiones_SelectedIndexChanged"
                                    AllowSorting="true" HeaderStyle-CssClass="tblHead" OnSorting="Grid_Peticiones_Sorting"
                                    Style="font-size: xx-small; white-space: normal">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="NO_PETICION" Visible="false" />
                                        <asp:BoundField DataField="ANIO_PETICION" Visible="false" />
                                        <asp:BoundField DataField="PROGRAMA_ID" Visible="false" />
                                        
                                        <asp:BoundField DataField="FOLIO" HeaderText="Folio"
                                            SortExpression="FOLIO">
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

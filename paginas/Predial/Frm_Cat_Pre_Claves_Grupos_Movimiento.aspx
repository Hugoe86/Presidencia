<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Claves_Grupos_Movimiento.aspx.cs"
    Inherits="paginas_predial_Frm_Cat_Pre_Claves_Grupos_Movimiento" %>

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
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="Upd_Sectores" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Sectores" runat="server" AssociatedUpdatePanelID="Upd_Sectores"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="General" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table id="Tbl_Comandos" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                    <tr>
                        <td class="label_titulo" colspan="2">
                            Catálogo de Claves Grupos Movimiento
                        </td>
                    </tr>
                    <tr>
                        <div id="Div_Contenedor_Msj_Error" runat="server">
                            <td colspan="2">
                                <asp:Image ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" />
                                <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                <br />
                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" TabIndex="0" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            </td>
                        </div>
                    </tr>
                    <tr class="barra_busqueda">
                        <td style="width: 50%">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                CssClass="Img_Button" OnClick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                CssClass="Img_Button" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                CssClass="Img_Button" OnClick="Btn_Eliminar_Click" OnClientClick="return confirm('¿Esta seguro de Eliminar el presente registro?');" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                CssClass="Img_Button" OnClick="Btn_Salir_Click" />
                        </td>
                        <td align="right" style="width: 50%">
                            Búsqueda:
                            <asp:TextBox ID="Txt_Busqueda_Clave_Grupo_Movimiento" runat="server" Width="180"
                                ToolTip="Buscar" TabIndex="1"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Clave_Grupo_Movimiento" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                TabIndex="2" OnClick="Btn_Buscar_Click" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkText="<Buscar por Clave>"
                                TargetControlID="Txt_Busqueda_Clave_Grupo_Movimiento" WatermarkCssClass="watermarked">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda_Clave_Grupo_Movimiento"
                                InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
                <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                    <tr>
                        <td colspan="5">
                            &nbsp;
                        </td>
                    </tr>
                    <tr style="background-color: #3366CC">
                        <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="5">
                            Claves de Grupos de Movimientos
                        </td>
                    </tr>
                    <tr>
                        <asp:HiddenField ID="Hf_Clave_Grupo_Movimiento_ID" runat="server" />
                        <td style="width: 18%" align="left">
                            <asp:Label Text="* Clave" runat="server"></asp:Label>
                        </td>
                        <td style="width: 30%">
                            <asp:TextBox ID="Txt_Clave_Grupo_Movimiento" runat="server" Width="96.4%" Visible="true"
                                Style="text-transform: uppercase" />
                        </td>
                        <td style="width: 18%" align="right">
                            * Estatus
                        </td>
                        <td style="width: 30%">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="94.4%">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE">
                                </asp:ListItem>
                                <asp:ListItem Text="VIGENTE" Value="VIGENTE">
                                </asp:ListItem>
                                <asp:ListItem Text="BAJA" Value="BAJA">
                                </asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 18%" align="left">
                            <asp:Label ID="Lbl_Nombre" Text="* Nombre" runat="server" Width="96.4%"></asp:Label>
                        </td>
                        <td style="width: 30%">
                            <asp:TextBox ID="Txt_Nombre" runat="server" Width="96.4%" Visible="true" Style="text-transform: uppercase" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center">
                            <asp:GridView ID="Grid_Grupos_Movimiento" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                GridLines="None" PageSize="5" Style="white-space: normal" Width="96%" OnPageIndexChanging="Grid_Grupos_Movimiento_PageIndexChanging"
                                OnSelectedIndexChanged="Grid_Grupos_Movimiento_SelectedIndexChanged">
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="GRUPO_MOVIMIENTO_ID" HeaderText="Grupo Movimiento ID">
                                        <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CLAVE" HeaderText="Clave">
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                        <HeaderStyle HorizontalAlign="Center" Width="60%" />
                                        <ItemStyle HorizontalAlign="Left" Width="60%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                    <tr style="background-color: #3366CC">
                        <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="5">
                            Detalle de la Clave seleccionada (Folios Iniciales por Año)
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%" align="left">
                            <asp:Label ID="Label1" Text="* Folio Inicial" runat="server" Width="30%"></asp:Label>
                            <asp:TextBox ID="Txt_Folio_Inicial" runat="server" Width="50%" Visible="true" Style="text-transform: uppercase" />
                        </td>
                        <td style="width: 30%" align="left">
                            <asp:Label ID="Label2" Text="* Año" runat="server" Width="30%"></asp:Label>
                            <asp:TextBox ID="Txt_Año" runat="server" Width="50%" Visible="true" Style="text-transform: uppercase" />
                        </td>
                        <td style="width: 30%" align="left">
                            <asp:Label ID="Label3" Text="* Tipo Predio" runat="server" Width="30%"></asp:Label>
                            <asp:DropDownList ID="Cmb_Tipos_Predio" runat="server" Width="50%">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 10%">
                            <asp:ImageButton ID="Btn_Agregar_Detalle" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                ToolTip="Agregar Detalle" OnClick="Btn_Agregar_Detalle_Click" />
                            <asp:ImageButton ID="Btn_Actualizar_Detalle" runat="server" ImageUrl="~/paginas/imagenes/paginas/actualizar_detalle.png"
                                ToolTip="Actualizar Detalle" OnClick="Btn_Actualizar_Detalle_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center">
                            <asp:GridView ID="Grid_Grupos_Movimiento_Detalle_Folios" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                GridLines="None" OnPageIndexChanging="Grid_Grupos_Movimiento_Detalle_Folios_PageIndexChanging"
                                OnSelectedIndexChanged="Grid_Grupos_Movimiento_Detalle_Folios_SelectedIndexChanged"
                                PageSize="5" Style="white-space: normal" Width="96%" DataKeyNames="GRUPO_MOVIMIENTO_ID,TIPO_PREDIO_ID"
                                OnRowCommand="Grid_Grupos_Movimiento_Detalle_Folios_RowCommand">
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="GRUPO_MOVIMIENTO_ID" HeaderText="GRUPO_MOVIMIENTO_ID"
                                        Visible="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TIPO_PREDIO_ID" HeaderText="TIPO_PREDIO_ID" Visible="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FOLIO_INICIAL" HeaderText="Folio Inicial">
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                        <ItemStyle HorizontalAlign="Right" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ANIO" HeaderText="Año">
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" Width="40%" />
                                        <ItemStyle HorizontalAlign="Center" Width="40%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TIPO_PREDIO" HeaderText="Tipo de Predio">
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" Width="40%" />
                                        <ItemStyle HorizontalAlign="Center" Width="40%" />
                                    </asp:BoundField>
                                    <asp:ButtonField ButtonType="Image" CommandName="Quitar_Detalle" ImageUrl="~/paginas/imagenes/paginas/delete.png"
                                        Text="Button">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

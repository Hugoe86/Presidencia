<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    CodeFile="Frm_Cat_Cat_Tabla_Valores_Construccion.aspx.cs" Inherits="paginas_Catastro_Frm_Cat_Cat_Tabla_Valores_Construccion"
    Title="Catálogo de Tabla de Valores para construcción" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Calles" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">
                            Cat&aacute;logo de Tabla de valores para construcción
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
                        <td align="left" class="style1">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Nuevo" OnClick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Modificar" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" TabIndex="3" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click" />
                        </td>
                        <td align="right" class="style1">
                            Búsqueda:
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="130px" TabIndex="4" Style="text-transform: uppercase"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar" runat="server" CausesValidation="false" TabIndex="5"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" OnClick="Btn_Buscar_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <cc1:TabContainer ActiveTabIndex="0" ID="Tc_Tabla_Valores" runat="server">
                    <cc1:TabPanel ID="Tp_Datos_Generales" runat="server" HeaderText="Datos Generales"
                        TabIndex="0">
                        <HeaderTemplate>
                            Datos Generales</HeaderTemplate>
                        <ContentTemplate>
                            <table width="98%" class="estilo_fuente">
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%">
                                        Tipo de Construcción
                                    </td>
                                    <td style="width: 30%">
                                        <asp:DropDownList ID="Cmb_Tipos_Construccion" runat="server" Width="98%" AutoPostBack="true"
                                            OnSelectedIndexChanged="Cmb_Tipos_Construccion_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 20%">
                                        Calidad de la Construcción
                                    </td>
                                    <td style="width: 30%">
                                        <asp:DropDownList ID="Cmb_Calidad_Construccion" runat="server" Width="98%" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="Tp_Tabla_Valores" runat="server" HeaderText="Tabla de Valores"
                        TabIndex="1">
                        <HeaderTemplate>
                            Tabla de Valores</HeaderTemplate>
                        <ContentTemplate>
                            <table width="98%" class="estilo_fuente">
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 20%; text-align: left;">
                                        *Año
                                    </td>
                                    <td style="text-align: left; width: 20%; text-align: left;">
                                        <asp:TextBox ID="Txt_Anio" runat="server" Width="94.4%" Style="float: left; text-transform: uppercase"
                                            TabIndex="8" MaxLength="4" />
                                        <cc1:FilteredTextBoxExtender FilterType="Numbers" runat="server" ID="FTB_Txt_Anio"
                                            TargetControlID="Txt_Anio" />
                                    </td>
                                    <td style="text-align: left; width: 20%; text-align: left;">
                                        *Clave Valor
                                    </td>
                                    <td style="text-align: left; width: 20%;">
                                        <asp:TextBox ID="Txt_Clave_Valor" runat="server" Width="98%" TabIndex="9" Enabled="false"
                                            Style="text-transform: uppercase" MaxLength="3"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTB_Txt_Clave_Valor" runat="server" FilterType="Numbers"
                                            TargetControlID="Txt_Clave_Valor" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 20%; text-align: left;">
                                        *Estado de Conservación
                                    </td>
                                    <td style="text-align: left; width: 20%;">
                                        <asp:DropDownList ID="Cmb_Estado" runat="server" Width="98%" TabIndex="10" Enabled="false">
                                            <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE"></asp:ListItem>
                                            <asp:ListItem Text="EXCELENTE" Value="EXCELENTE"></asp:ListItem>
                                            <asp:ListItem Text="BUENO" Value="BUENO"></asp:ListItem>
                                            <asp:ListItem Text="REGULAR" Value="REGULAR"></asp:ListItem>
                                            <asp:ListItem Text="MALO" Value="MALO"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right; width: 20%; text-align: left;">
                                        *Valor M2
                                    </td>
                                    <td style="text-align: right; width: 20%; text-align: right;">
                                        <asp:TextBox ID="Txt_Valor_M2" runat="server" Width="98%" Style="float: left; text-transform: uppercase"
                                            AutoPostBack="true" OnTextChanged="Txt_Valor_M2_TextChanged" MaxLength="12" TabIndex="11" />
                                        <cc1:FilteredTextBoxExtender FilterType="Numbers,Custom" runat="server" ID="FTB_Txt_Valor_M2"
                                            TargetControlID="Txt_Valor_M2" ValidChars="1234567890.," />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="Hdf_Valor_Contruccion_Id" runat="server" />
                                        <asp:HiddenField ID="Hdf_Anio" runat="server" />
                                        <asp:HiddenField ID="hdf_Valor_M2" runat="server" />
                                        <asp:HiddenField ID="Hdf_Estado_Conservacion" runat="server" />
                                        <asp:HiddenField ID="Hdf_Clave_Valor" runat="server" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="Btn_Agregar_Valor_Construccion" runat="server" Height="24px"
                                            ImageUrl="~/paginas/imagenes/paginas/sias_add.png" OnClick="Btn_Agregar_Valor_Click"
                                            TabIndex="12" ToolTip="Agregar Valor de Construcción" Width="24px" />
                                        &nbsp; &nbsp;
                                        <asp:ImageButton ID="Btn_Actualizar_Valor_Construccion" runat="server" Height="24px"
                                            ImageUrl="~/paginas/imagenes/paginas/Actualizar_Detalle.png" OnClick="Btn_Actualizar_Valor_Click"
                                            TabIndex="13" ToolTip="Actualizar Valor de Construcción" Width="24px" />
                                        &nbsp; &nbsp;
                                        <asp:ImageButton ID="Btn_Eliminar_Valor_Construccion" runat="server" Height="24px"
                                            ImageUrl="~/paginas/imagenes/paginas/quitar.png" OnClick="Btn_Eliminar_Valor_Click"
                                            TabIndex="14" ToolTip="Eliminar Valor de Construcción" Width="24px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:GridView ID="Grid_Valores_Construccion" runat="server" AllowPaging="True" AllowSorting="True"
                                            AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                            HeaderStyle-CssClass="tblHead" PageSize="10" Style="white-space: normal;" Width="100%"
                                            OnSelectedIndexChanged="Grid_Valores_Construccion_SelectedIndexChanged" OnPageIndexChanging="Grid_Valores_Construccion_PageIndexChanging">
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="VALOR_CONSTRUCCION_ID" HeaderStyle-Width="15%" HeaderText="Id"
                                                    Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ANIO" HeaderStyle-Width="20%" HeaderText="Año">
                                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CLAVE_VALOR" HeaderStyle-Width="20%" HeaderText="Clave">
                                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ESTADO_CONSERVACION" HeaderStyle-Width="30%" HeaderText="Estado de Conservación">
                                                    <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="VALOR_M2" HeaderStyle-Width="30%" HeaderText="Valor de M2"
                                                    DataFormatString="{0:c2}">
                                                    <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                                    <ItemStyle HorizontalAlign="Right" Width="30%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ACCION" HeaderStyle-Width="15%" HeaderText="Accion" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
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
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
                            <table width="98%" class="estilo_fuente">
                                <tr>
                                    <td style="text-align: left;">
                                        <asp:GridView ID="Grid_Calidad" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                            CssClass="GridView_1" HeaderStyle-CssClass="tblHead" Style="white-space: normal;"
                                            Width="100%" OnPageIndexChanging="Grid_Calidad_PageIndexChanging" OnSelectedIndexChanged="Grid_Calidad_SelectedIndexChanged"
                                            PageSize="10" AllowPaging="true">
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/paginas/Select_Grid_Inner.png"
                                                    Text="Button">
                                                    <HeaderStyle Width="5%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="CALIDAD_ID" HeaderText="Id" Visible="false">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TIPO_CONSTRUCCION_ID" HeaderText="Id" Visible="false">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="IDENTIFICADOR" HeaderText="Tipo de Construccion">
                                                    <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CALIDAD" HeaderText="Calidad">
                                                    <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CLAVE_CALIDAD" HeaderText="Clave">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
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
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

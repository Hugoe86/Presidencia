<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Cat_Cat_Valores_Inpa.aspx.cs"
    Inherits="paginas_Catastro_Frm_Cat_Cat_Valores_Inpa" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
    <script type='text/javascript' language="javascript">
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
                            Cat&aacute;logo de I.N.P.A.
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
                            <%--                            <asp:ImageButton ID="Btn_Eliminar" runat="server" TabIndex="3"
                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Eliminar" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro de la Base de Datos?');"
                                OnClick = "Btn_Eliminar_Click" />--%>
                            <asp:ImageButton ID="Btn_Salir" runat="server" TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click" />
                        </td>
                        <td align="right" class="style1">
                            Búsqueda:
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="130px" TabIndex="6" Style="text-transform: uppercase"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar" runat="server" CausesValidation="false" TabIndex="7"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" OnClick="Btn_Buscar_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <center>
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td colspan="4">
                                <div id="Div_Grid_Tab_Val_Const" runat="server">
                                    <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                                        <tr style="background-color: #3366CC">
                                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                                I.N.P.A.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%;">
                                                <asp:HiddenField ID="Hdf_Inpa_Id" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 20%; text-align: left;">
                                                *Año
                                            </td>
                                            <td style="text-align: left; width: 20%; text-align: left;">
                                                <asp:TextBox ID="Txt_Anio" runat="server" Width="94.4%" Style="float: left; text-transform: uppercase"
                                                    MaxLength="4" />
                                                <cc1:FilteredTextBoxExtender FilterType="Numbers" runat="server" ID="FTB_Txt_Anio"
                                                    TargetControlID="Txt_Anio" />
                                            </td>
                                            <td style="text-align: right; width: 20%; text-align: left">
                                                *I.N.P.A.
                                            </td>
                                            <td style="text-align: right; width: 20%; text-align: right;">
                                                <asp:TextBox ID="Txt_Valor_Inpa" runat="server" Width="94.4%" Style="float: left;
                                                    text-transform: uppercase" AutoPostBack="true" OnTextChanged="Txt_Valor_Inpa_TextChanged"
                                                    MaxLength="12" />
                                                <cc1:FilteredTextBoxExtender FilterType="Numbers,Custom" runat="server" ID="FTB_Txt_Valor_Inpa"
                                                    TargetControlID="Txt_Valor_Inpa" ValidChars="1234567890.," />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="Hdf_Anio" runat="server" />
                                                <asp:HiddenField ID="hdf_Valor_Inpa" runat="server" />
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="Btn_Agregar_Inpa" runat="server" Height="24px" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                                    OnClick="Btn_Agregar_Valor_Click" TabIndex="10" ToolTip="Agregar I.n.p.a." Width="24px" />
                                                &nbsp; &nbsp;
                                                <asp:ImageButton ID="Btn_Actualizar_Inpa" runat="server" Height="24px" ImageUrl="~/paginas/imagenes/paginas/Actualizar_Detalle.png"
                                                    OnClick="Btn_Actualizar_Valor_Click" TabIndex="10" ToolTip="Actualizar I.n.p.a."
                                                    Width="24px" />
                                                &nbsp; &nbsp;
                                                <asp:ImageButton ID="Btn_Eliminar_Inpa" runat="server" Height="24px" ImageUrl="~/paginas/imagenes/paginas/quitar.png"
                                                    OnClick="Btn_Eliminar_Valor_Click" TabIndex="10" ToolTip="Eliminar I.n.p.a."
                                                    Width="24px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:GridView ID="Grid_Inpa" runat="server" AllowPaging="True" AllowSorting="True"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                                    HeaderStyle-CssClass="tblHead" PageSize="10" Style="white-space: normal;" Width="100%"
                                                    OnSelectedIndexChanged="Grid_Inpa_SelectedIndexChanged" OnPageIndexChanging="Grid_Inpa_PageIndexChanging">
                                                    <Columns>
                                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                            <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                        </asp:ButtonField>
                                                        <asp:BoundField DataField="VALOR_INPA_ID" HeaderStyle-Width="15%" HeaderText="Id"
                                                            Visible="false">
                                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ANIO" HeaderStyle-Width="15%" HeaderText="Año">
                                                            <HeaderStyle HorizontalAlign="Center" Width="50%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="50%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="VALOR_INPA" HeaderStyle-Width="15%" HeaderText="I.N.P.A."
                                                            DataFormatString="{0:N2}">
                                                            <HeaderStyle HorizontalAlign="Center" Width="50%" />
                                                            <ItemStyle HorizontalAlign="Right" Width="50%" />
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
                                </div>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

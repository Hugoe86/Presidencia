<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Cat_Cat_Tabla_Valores_Catastrales.aspx.cs" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Inherits="paginas_Catastro_Frm_Cat_Cat_Tabla_Valores_Catastrales" Title="Catálogo de Tabla de Valores Catastrales" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
    <script type='text/javascript'>
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }

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
                            Cat&aacute;logo de Tabla de Valores Catastrales
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
                                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" Visible="true" />
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
                        
                    </tr>
                </table>
                <br />
                <center>
                    <table width="98%" class="estilo_fuente">
                       
                        
                       <%-- <tr style="background-color: #3366CC">
                            <td style="text-align: left; font-size: 15px; color: #FFFFFF;" colspan="4">
                                Tipos de Construcción
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                <asp:HiddenField ID="hdf_Tipo_Construccion_Id" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 20%;">
                                *Tipo de Construcción
                            </td>
                            
                            <td style="text-align: left; width: 30%;">
                                    <asp:DropDownList ID="Cmb_Tipos_Construccion" runat="server" Enabled="true" 
                                        onselectedindexchanged="Cmb_Tipos_Construccion_SelectedIndexChanged" AutoPostBack="true" 
                                        TabIndex="12" Width="98%">
                                    </asp:DropDownList>

                                    <td> 
                                    </td>
                                                                     
                                    </td>
                            </tr>
                            <div ID="Div_Grid_Tab_Val" runat="server" visible="false">
                            </div>
                            <table border="0" cellspacing="0" class="estilo_fuente" width="98%">
                                <tr>
                                    <td>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr style="background-color: #3366CC">
                                    <td colspan="4" style="text-align: left; font-size: 15px; color: #FFFFFF;">
                                        Tabla de valores
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td style="text-align: left; width: 20%; text-align: left;">
                                        *Año
                                    </td>
                                    <td style="text-align: left; width: 30%; text-align: left;">
                                        <asp:TextBox ID="Txt_Anio" runat="server" MaxLength="4" 
                                            Style="float: left; text-transform: uppercase" Width="98%" />
                                        <cc1:FilteredTextBoxExtender ID="FTB_Txt_Anio" runat="server" 
                                            FilterType="Numbers" TargetControlID="Txt_Anio" />
                                    </td>
                                    </tr>
                                    <tr>
                                    <td style="text-align: right; width: 20%; text-align: Left;">
                                        *Cantidad_1
                                    </td>
                                    <td style="text-align: right; width: 30%; text-align: right;">
                                        <asp:TextBox ID="Txt_Cantidad_1" runat="server" AutoPostBack="true" 
                                            MaxLength="12" OnTextChanged="Txt_Cantidad_1_TextChanged" 
                                            Style="float: left; text-transform: uppercase" Width="98%" />
                                        <cc1:FilteredTextBoxExtender ID="FTB_Txt_Cantidad_1" runat="server" 
                                            FilterType="Numbers,Custom" TargetControlID="Txt_Cantidad_1" 
                                            ValidChars="1234567890.," />
                                    </td>
                                    
                                    <td style="text-align: right; width: 20%; text-align: Left;">
                                        *Cantidad_2
                                    </td>
                                    <td style="text-align: right; width: 30%; text-align: right;">
                                        <asp:TextBox ID="Txt_Cantidad_2" runat="server" AutoPostBack="true" 
                                            MaxLength="12" OnTextChanged="Txt_Cantidad_2_TextChanged" 
                                            Style="float: left; text-transform: uppercase" Width="98%" />
                                        <cc1:FilteredTextBoxExtender ID="FTB_Txt_Cantidad_2" runat="server" 
                                            FilterType="Numbers,Custom" TargetControlID="Txt_Cantidad_2" 
                                            ValidChars="1234567890.," />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="Hdf_Valor_Contruccion_Id" runat="server" />
                                        <asp:HiddenField ID="Hdf_Anio" runat="server" />
                                        <asp:HiddenField ID="hdf_Txt_Cantidad_1" runat="server" />
                                         <asp:HiddenField ID="hdf_Txt_Cantidad_2" runat="server" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="Btn_Agregar_Valor" runat="server" Height="24px" 
                                            ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                            OnClick="Btn_Agregar_Valor_Click" TabIndex="10" ToolTip="Agregar Valor" 
                                            Width="24px" />
                                        &nbsp; &nbsp;
                                        <asp:ImageButton ID="Btn_Actualizar_Valor" runat="server" Height="24px" 
                                            ImageUrl="~/paginas/imagenes/paginas/Actualizar_Detalle.png" 
                                            OnClick="Btn_Actualizar_Valor_Click" TabIndex="10" ToolTip="Actualizar Valor" 
                                            Width="24px" />
                                        &nbsp; &nbsp;
                                        <asp:ImageButton ID="Btn_Eliminar_Valor" runat="server" Height="24px" 
                                            ImageUrl="~/paginas/imagenes/paginas/quitar.png" 
                                            OnClick="Btn_Eliminar_Valor_Click" TabIndex="10" ToolTip="Eliminar Valor" 
                                            Width="24px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:GridView ID="Grid_Valores" runat="server" AllowPaging="True" 
                                            AllowSorting="True" AutoGenerateColumns="False" CssClass="GridView_1" 
                                            EmptyDataText="&quot;No se encontraron registros&quot;" 
                                            HeaderStyle-CssClass="tblHead" 
                                            OnPageIndexChanging="Grid_Valores_PageIndexChanging" 
                                            OnSelectedIndexChanged="Grid_Valores_SelectedIndexChanged" PageSize="10" 
                                            Style="white-space: normal;" Width="100%">
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="VALOR_CATASTRAL_ID" HeaderStyle-Width="15%" 
                                                    HeaderText="Id" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ANIO" HeaderStyle-Width="20%" HeaderText="Año">
                                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CANTIDAD_1" DataFormatString="{0:c2}" 
                                                    HeaderStyle-Width="30%" HeaderText="Cantidad_1">
                                                    <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                                    <ItemStyle HorizontalAlign="Right" Width="30%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CANTIDAD_2" DataFormatString="{0:c2}" 
                                                    HeaderStyle-Width="30%" HeaderText="Cantidad_2">
                                                    <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                                    <ItemStyle HorizontalAlign="Right" Width="30%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ACCION" HeaderStyle-Width="15%" HeaderText="Accion" 
                                                    Visible="false">
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
                        </caption>
                    </table>
                </center>
            </div>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Cat_Cat_Tipos_Construccion_Rustico.aspx.cs"
    MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Inherits="paginas_Catastro_Frm_Cat_Cat_Tipos_Construccion_Rustico"
    Title="Catálogo de Tipos de Construcción Rústico" %>

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
                        <td colspan="3" class="label_titulo">
                            Cat&aacute;logo de Características Agrológicas
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
                        <td align="left" >                           
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Nuevo" OnClick="Btn_Nuevo_Click" />
                                <%--Btn_Modificar_Click(--%> 
                            <asp:ImageButton ID="Btn_Modificar" runat="server" TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Modificar" OnClick="Btn_Modificar_Click" />
                                
                           <%-- <asp:ImageButton ID="Btn_Eliminar" runat="server" TabIndex="3"
                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Eliminar" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro de la Base de Datos?');"
                                OnClick = "Btn_Eliminar_Click" />--%>
                                
                                <asp:ImageButton ID="Btn_Salir" runat="server" TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                Width="24px" CssClass="Img_Button" AlternateText="Salir" OnClick="Btn_Salir_Click" />
                                
                            
                        <td style="width: 55%;">
                        B&uacute;squeda:
                            <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar"
                                Width="180px" Style="text-transform: uppercase" />
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                    WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. " />
                         </td>
                         <td style="vertical-align: middle; width: 5%;">
                            <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                ToolTip="Buscar" AlternateText="Buscar" OnClick="Btn_Buscar_Click" />
                         </td>
                    </tr>
                </table>
                <br />
                <center>
                    <table width="98%" class="estilo_fuente">
                        <div id="Div_Grid_Tipos_Construccion" runat="server" visible="true">
                        </div>
                        <div id="Div_Datos_Construccion" runat="server" visible="false">
                        </div>
                        <tr style="background-color: #3366CC" align="center">
                            <td style="text-align: center; font-size: 15px; color: #FFFFFF;" colspan="4" >
                                Caracteristicas Agrológicas
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
                                *Tipo de Terreno
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:TextBox ID="Txt_Tipo_Contruccion" runat="server" Width="98%" TabIndex="9" Enabled="false"
                                    Style="text-transform: uppercase" MaxLength="50"></asp:TextBox>
                            </td>
                            <td style="text-align: left; width: 20%; text-align: Left;">
                                *Estatus
                            </td>
                            <td style="text-align: left; width: 30%;">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" TabIndex="12" Enabled="false">
                                    <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE"></asp:ListItem>
                                    <asp:ListItem Text="VIGENTE" Value="VIGENTE"></asp:ListItem>
                                    <asp:ListItem Text="BAJA" Value="BAJA"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                        <td>
                        <br />
                        </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="Grid_Tipos_Construccion_Rustico" runat="server" AllowPaging="True" AllowSorting="True"
                                    AutoGenerateColumns="False" CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                    HeaderStyle-CssClass="tblHead" PageSize="15" Style="white-space: normal;" Width="100%"
                                    OnSelectedIndexChanged="Grid_SelectedIndexChanged" OnPageIndexChanging="Grid_PageIndexChanging">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="TIPO_CONSTRU_RUSTICO_ID" HeaderStyle-Width="15%" HeaderText="ID"
                                            Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IDENTIFICADOR" HeaderStyle-Width="15%" HeaderText="Tipo de Terreno">
                                            <HeaderStyle HorizontalAlign="Center" Width="70%" />
                                            <ItemStyle HorizontalAlign="Left" Width="70%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                            <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                            <ItemStyle HorizontalAlign="Center" Width="30%" />
                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                                </td>
                        </tr>
                        <div id="Div_Grid_Tab_Val" runat="server" visible="false">
                        
                        </div>                        
                    </table>
                </center>
            </div>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

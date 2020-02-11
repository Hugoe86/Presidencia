<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Parametros.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Parametros" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script language="javascript" type="text/javascript">
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
    <asp:ScriptManager ID="ScriptManager_Parametros_Predial" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="Upd_Parametros_Predial" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <%--update progrees--%>
            <asp:UpdateProgress ID="Uprg_Progress" runat="server" AssociatedUpdatePanelID="Upd_Parametros_Predial"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Parametros_General" style="background-color: #ffffff; width: 100%; height: 100%">
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td colspan="4" class="label_titulo">
                            Parámetros
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" />
                            <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" CssClass="estilo_fuente_mensaje_error"
                                Text="" /><br />
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                    <tr class="barra_busqueda">
                        <td colspan="2" style="width: 50%">
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                CssClass="Img_Button" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                CssClass="Img_Button" OnClick="Btn_Salir_Click" />
                        </td>
                        <td colspan="2" align="right" style="width: 50%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 18%">
                        </td>
                        <td style="width: 32%">
                        </td>
                        <td style="width: 18%">
                        </td>
                        <td style="width: 32%">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <%--inicio de TabContainer--%>
                            <cc1:TabContainer ID="Tab_Contenedor_Parametros" runat="server" Width="98%" ActiveTabIndex="5">
                                <%--tab para la definicion de el porentaje de recargos de traslado--%>
                                <cc1:TabPanel ID="Tbp_Recargos" runat="server" HeaderText="Tbp_Recargos" Width="100%">
                                    <HeaderTemplate>
                                        Recargos de Traslado</HeaderTemplate>
                                    <ContentTemplate>
                                        <center>
                                            <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                                                <tr>
                                                    <td style="width: 18%">
                                                    </td>
                                                    <td style="width: 32%">
                                                    </td>
                                                    <td style="width: 18%">
                                                    </td>
                                                    <td style="width: 32%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%--<td style="width:18%">*Año</td>--%>
                                                    <%--<td style="width:32%"><asp:TextBox ID="Txt_Anio" Width="92%" runat="server" MaxLength="4"></asp:TextBox>
                        <%--<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"  TargetControlID="Txt_Anio" FilterType="Numbers"/></td>--%>
                                                    <td style="width: 18%">
                                                        *Porcentaje Moratorios
                                                    </td>
                                                    <td style="width: 32%">
                                                        <asp:TextBox ID="Txt_Cuota" Width="92%" runat="server"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="Txt_Cuota"
                                                            FilterType="Numbers, Custom" ValidChars="." />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 18%">
                                                    </td>
                                                    <td style="width: 32%">
                                                    </td>
                                                    <td style="width: 18%">
                                                    </td>
                                                    <td style="width: 32%">
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <%--tab para la constancia de no adeudo--%>
                                <cc1:TabPanel ID="Tbp_Constancia_Adeudo" runat="server" HeaderText="Tbp_Constancia_Adeudo"
                                    Width="100%">
                                    <HeaderTemplate>
                                        Constancia de No Adeudo</HeaderTemplate>
                                    <ContentTemplate>
                                        <center>
                                            <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                                                <tr>
                                                    <td style="width: 18%">
                                                    </td>
                                                    <td style="width: 32%">
                                                    </td>
                                                    <td style="width: 45%">
                                                    </td>
                                                    <td style="width: 5%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 18%">
                                                        ID de Constancia de no adeudo
                                                    </td>
                                                    <td style="width: 32%" colspan="2">
                                                        <asp:TextBox ID="Txt_Constancia_No_Adeudo" Width="98%" runat="server" ReadOnly="True"
                                                            AutoPostBack="true"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 32%" align="right">
                                                        <asp:ImageButton ID="Btn_Busqueda_Cosntancias" runat="server" CssClass="Img_Button"
                                                            ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" OnClick="Btn_Busqueda_Cosntancias_Click1" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 18%">
                                                    </td>
                                                    <td style="width: 32%">
                                                    </td>
                                                    <td style="width: 18%">
                                                    </td>
                                                    <td style="width: 32%">
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <%--tab para la definicion del respaldo de descuento de traslado--%>
                                <%--MODIFICACION QUITAR PESTAÑA DE RESPALDO--%>
                                <%--<cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Tbp_Constancia_Adeudo" Width="100%">
                        <HeaderTemplate>Respaldo de ley</HeaderTemplate><ContentTemplate><center>
                        <table width="98%"  border="0" cellspacing="0" class="estilo_fuente"><tr>
                        <td style="width:18%"></td><td style="width:32%"></td><td style="width:18%"></td><td style="width:32%"></td></tr><tr>
                        <td style="width:18%">Descuento de Traslado </td><td colspan="3">
                        <asp:TextBox ID="Txt_Respaldo_Adeudo" Width="96%" Height="60px" runat="server" TextMode="MultiLine" MaxLength="250" Wrap="true"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FTxt_Comentarios" runat="server"  TargetControlID="Txt_Respaldo_Adeudo" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ,"/>
                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios" runat="server" TargetControlID ="Txt_Respaldo_Adeudo" WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/></td></tr><tr><td style="width:18%">
                        <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>
                        </td><td style="width:32%"></td><td style="width:18%"></td><td style="width:32%"></td></tr></table></center></ContentTemplate></cc1:TabPanel>--%>--%>
                                <%--MODIFICACION QUITAR PESTAÑA DE HONORARIOS--%>
                                <%--tab para la definicion de el porentaje de cobro de honorarios--%>
                                <%--<cc1:TabPanel ID="Tbp_Porcentaje_Cobro" runat="server" HeaderText="Tbp_Porcentaje_Cobro" Width="100%">
                        <HeaderTemplate>Cobro de Honorarios</HeaderTemplate><ContentTemplate><center>
                        <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                        <tr><td style="width:18%"></td><td style="width:32%"></td>
                        <td style="width:18%"></td><td style="width:32%"></td>
                        </tr><tr><td style="width:18%">*Porcentaje de Cobro de Honorarios</td>
                        <td style="width:32%"><asp:TextBox ID="Txt_Cobro_Honorarios" Width="92%" runat="server"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"  TargetControlID="Txt_Cobro_Honorarios" FilterType="Numbers, Custom" ValidChars="."/></td>
                        <td style="width:18%"></td><td style="width:32%"></td></tr><tr><td style="width:18%"></td><td style="width:32%"></td>
                        <td style="width:18%"></td><td style="width:32%"></td></tr>
                        </table></center></ContentTemplate></cc1:TabPanel>--%>
                                <%--tab para la definicion de el Tope de salario minimo--%>
                                <cc1:TabPanel ID="Tbp_Panel_Tope_Salario" runat="server" HeaderText="Tbp_Panel_Tope_Salario"
                                    Width="100%">
                                    <HeaderTemplate>
                                        Tope de Salario Minimo</HeaderTemplate>
                                    <ContentTemplate>
                                        <center>
                                            <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                                                <tr>
                                                    <td style="width: 18%">
                                                    </td>
                                                    <td style="width: 32%">
                                                    </td>
                                                    <td style="width: 18%">
                                                    </td>
                                                    <td style="width: 32%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 18%">
                                                        *Tope de Salario Minimo
                                                    </td>
                                                    <td style="width: 32%">
                                                        <asp:TextBox ID="Txt_Tope_Salario" Width="92%" runat="server"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="Txt_Tope_Salario"
                                                            FilterType="Numbers, Custom" ValidChars="." />
                                                    </td>
                                                    <td style="width: 18%">
                                                    </td>
                                                    <td style="width: 32%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 18%">
                                                    </td>
                                                    <td style="width: 32%">
                                                    </td>
                                                    <td style="width: 18%">
                                                    </td>
                                                    <td style="width: 32%">
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <%--tab para la definicion del Tolerancia de pagos en otras instituciones.--%>
                                <%--<cc1:TabPanel ID="Tbp_Panel_Pagos" runat="server" HeaderText="Tbp_Panel_Pagos" Width="100%">
                        <HeaderTemplate>Pagos en Otras Instituciones</HeaderTemplate><ContentTemplate><center>
                        <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                        <tr><td style="width:18%"></td><td style="width:32%"></td>
                        <td style="width:18%"></td><td style="width:32%"></td>
                        </tr><tr><td style="width:18%">*Tolerancia Pagos otras Instituciones</td>
                        <td style="width:32%"><asp:TextBox ID="Txt_Pagos" Width="92%" runat="server"></asp:TextBox>                        
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="Txt_Pagos" FilterType="Numbers, Custom" ValidChars="."/></td>
                        <td style="width:18%"></td><td style="width:32%"></td></tr><tr><td style="width:18%"></td><td style="width:32%"></td>
                        <td style="width:18%"></td><td style="width:32%"></td></tr>
                        </table></center></ContentTemplate></cc1:TabPanel>--%>
                            </cc1:TabContainer>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 18%">
                            &nbsp;
                        </td>
                        <td style="width: 32%">
                            &nbsp;
                        </td>
                        <td style="width: 18%">
                            &nbsp;
                        </td>
                        <td style="width: 32%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 18%">
                            &nbsp;
                        </td>
                        <td style="width: 32%">
                            &nbsp;
                        </td>
                        <td style="width: 18%">
                            &nbsp;
                        </td>
                        <td style="width: 32%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 18%">
                            &nbsp;
                        </td>
                        <td style="width: 32%">
                            &nbsp;
                        </td>
                        <td style="width: 18%">
                            &nbsp;
                        </td>
                        <td style="width: 32%">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Btn_Aceptar_Busqueda_Av" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <%--Modal popup para la busqueda de Cosntancias--%>
    <asp:UpdatePanel ID="UPnl_Busqueda_Avanzada" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_Busqueda_Constancias" runat="server" Text="Button" Style="display: none;" />
            <cc1:ModalPopupExtender ID="Modal_Busqueda_Constancias" runat="server" TargetControlID="Btn_Comodin_Busqueda_Constancias"
                BehaviorID="Busqueda" PopupControlID="Pnl_Buscar_Avanzada" CancelControlID="Btn_Cerrar_Ventana_Constancias"
                PopupDragHandleControlID="Pnl_Buscar_Avanzada_Interno" DropShadow="True" BackgroundCssClass="progressBackgroundFilter" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="Pnl_Buscar_Avanzada" runat="server" CssClass="drag" HorizontalAlign="Center"
        Width="600px" Style="display: none; border-style: outset; border-color: Silver;
        background-image: url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG); background-repeat: repeat-y;">
        <asp:Panel ID="Pnl_Buscar_Avanzada_Interno" runat="server" Style="cursor: move; background-color: Silver;
            color: Black; font-size: 12; font-weight: bold; border-style: outset;">
            <table width="99%">
                <tr>
                    <td style="color: Black; font-size: 12; font-weight: bold;">
                        <asp:Image ID="Img_Informatcion_Autorizacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                        B&uacute;squeda: Constancias
                    </td>
                    <td align="right" style="width: 10%;">
                        <asp:ImageButton ID="Btn_Cerrar_Ventana_Constancias" CausesValidation="false" runat="server"
                            Style="cursor: pointer;" ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" />
                    </td>
                </tr>
            </table>
            <table>
            </table>
        </asp:Panel>
        <asp:UpdatePanel ID="UPnl_Busqueda_Recibos_Empleados" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server"
                    AssociatedUpdatePanelID="UPnl_Busqueda_Recibos_Empleados" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressTemplateInner">
                        </div>
                        <div style="background-color: Transparent; position: fixed; top: 50%; left: 47%;
                            padding: 10px; z-index: 1002;" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Sias_Roler.gif" /></div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table width="100%" class="estilo_fuente">
                    <tr>
                        <td colspan="4">
                            <asp:Image ID="Img_Error_Bus_Pro" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" />
                            <br />
                            <asp:Label ID="Lbl_Error_Bus_Pro" runat="server" Text="" TabIndex="0" CssClass="estilo_fuente_mensaje_error" />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <%--<td style="width:18%" align="left">Constancia ID</td>
            <td style="width:32%" align="left"><asp:TextBox ID="Txt_B_Constancia_ID" runat="server" Width="95%"></asp:TextBox></td>
            --%><td style="width: 18%" align="center">
                Nombre
                        </td>
                        <td style="width: 32%" align="left">
                            <asp:TextBox ID="Txt_B_Nombre" runat="server" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; text-align: right; cursor: default;" colspan="4" align="right">
                            <asp:ImageButton ID="Btn_Aceptar_Busqueda_Av" runat="server" ToolTip="Buscar Propietarios"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" Style="color: Black; border-style: outset;"
                                OnClick="Btn_Aceptar_Busqueda_Av_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:GridView ID="Grid_Cosntancias_Busqueda" runat="server" AllowPaging="true" AutoGenerateColumns="false"
                                CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                GridLines="none" PageSize="5" Style="white-space: normal" Width="96%" OnSelectedIndexChanged="Grid_Constancias_Busqueda_SelectedIndexChanged">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="10%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="ID" HeaderText="ID">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                        <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                        <ItemStyle HorizontalAlign="Left" Width="40%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="COSTO" HeaderText="Costo">
                                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                        <ItemStyle HorizontalAlign="Left" Width="30%" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>

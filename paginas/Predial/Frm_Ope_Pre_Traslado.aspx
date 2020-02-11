<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Traslado.aspx.cs" Inherits="paginas_predial_Frm_Ope_Pre_Traslado"
    Title="Operación - Recepción de Documentos" %>

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
    <asp:ScriptManager ID="ScptM_Traslado" runat="server" />
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
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">
                            Traslado
                        </td>
                    </tr>
                    <tr>
                        <td>
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
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <br />
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="98%" ActiveTabIndex="0">
                    <cc1:TabPanel runat="server" HeaderText="Tab_Panel_Contrarecibos" ID="Tab_Panel_Contrarecibos"
                        Width="100%">
                        <HeaderTemplate>
                            Contrarecibo</HeaderTemplate>
                        <ContentTemplate>
                            <div style="width: 98%; height: 150px;">
                                <table width="100%">
                                    <tr>
                                        <td style="text-align: left;" colspan="2">
                                            <asp:Label ID="Lbl_Titulo_Busqueda" runat="server" Text="Búsqueda"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left;">
                                            <asp:Label ID="Lbl_Cuenta_Predial" runat="server" Text="Cuenta Predial" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 80%; text-align: left;">
                                            <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Width="200px" Style="text-transform: uppercase"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                ID="FTE_Txt_Cuenta_Predial" runat="server" FilterType="Numbers, UppercaseLetters, LowercaseLetters"
                                                TargetControlID="Txt_Cuenta_Predial" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left;" class="estilo_fuente">
                                            Estatus
                                        </td>
                                        <td style="width: 80%; text-align: left;">
                                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left;">
                                            <asp:Label ID="Lbl_No_Contrarecibo" runat="server" Text="No. Contrarecibo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 80%; text-align: left;">
                                            <asp:TextBox ID="Txt_No_Contrarecibo" runat="server" Width="200px"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                ID="FTE_Txt_No_Contrarecibo" runat="server" FilterType="Numbers" TargetControlID="Txt_No_Contrarecibo"
                                                Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left;">
                                            <asp:Label ID="Lbl_Fecha_Escritura" runat="server" Text="Fecha Escritura" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 80%; text-align: left;">
                                            <asp:TextBox ID="Txt_Fecha_Escritura" runat="server" Width="200px" Enabled="False"></asp:TextBox><asp:ImageButton
                                                ID="Btn_Fecha_Escritura" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"
                                                AlternateText="Seleccione la Fecha de Escritura" /><cc1:CalendarExtender ID="CE_Txt_Fecha_Fecha_Escritura"
                                                    runat="server" TargetControlID="Txt_Fecha_Escritura" PopupButtonID="Btn_Fecha_Escritura"
                                                    Format="dd/MMM/yyyy" Enabled="True">
                                                </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; text-align: left;">
                                            <asp:Label ID="Lbl_No_Escritura" runat="server" Text="No. Escritura" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width: 80%; text-align: left;">
                                            <asp:TextBox ID="Txt_No_Escritura" runat="server" Width="200px"></asp:TextBox>
                                            &#160;&#160;&#160;&#160;&#160;&#160;&#160;
                                            <asp:ImageButton ID="Btn_Buscar_Contrarecibos" runat="server" CausesValidation="False"
                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar Contrarecibos"
                                                OnClick="Btn_Buscar_Contrarecibos_Click" /><asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Contrarecibos"
                                                    runat="server" CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png"
                                                    Width="20px" ToolTip="Limpiar Filtros" OnClick="Btn_Limpiar_Filtros_Buscar_Contrarecibos_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <%--QUITE LA PESTAÑA DE LISTADO PUES NO ES NECESARIA PARA EL PROCESO--%>
                    <%--<cc1:TabPanel  runat="server" HeaderText="Tab_Panel_Listados"  ID="Tab_Panel_Listados"  Width="100%"  ><HeaderTemplate>Listado</HeaderTemplate><ContentTemplate><div style="border-style:outset; width:98%; height:150px;" ><table width="100%"><tr><td style="text-align:left;" colspan="2"><asp:Label ID="Lbl_Busqueda_Listado" runat="server" Text="Búsqueda"></asp:Label></td></tr><tr><td style="width:20%; text-align:left; "><asp:Label ID="Lbl_No_Listado" runat="server" Text="No. Listado" CssClass="estilo_fuente"></asp:Label></td><td style="width:80%; text-align:left;"><asp:TextBox ID="Txt_No_Listado" runat="server" Width="200px"></asp:TextBox><cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Listado" runat="server" FilterType="Numbers" 
                                                TargetControlID="Txt_No_Listado" Enabled="True"></cc1:FilteredTextBoxExtender></td></tr><tr><td style="width:20%; text-align:left; "><asp:Label ID="Lbl_Fecha_Generacion" runat="server" Text="Fecha Generacion" CssClass="estilo_fuente"></asp:Label></td><td style="width:80%; text-align:left;"><asp:TextBox ID="Txt_Fecha_Generacion" runat="server" Width="200px" Enabled="False"></asp:TextBox><asp:ImageButton ID="Btn_Fecha_Generacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" AlternateText="Seleccione la Fecha de Generacion del Listado" /><cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                                                TargetControlID="Txt_Fecha_Generacion" PopupButtonID="Btn_Fecha_Generacion" 
                                                Format="dd/MMM/yyyy" Enabled="True"></cc1:CalendarExtender></td></tr><tr><td style="width:20%; text-align:left; "><asp:Label ID="Lbl_Notario" runat="server" Text="Notario" CssClass="estilo_fuente"></asp:Label></td><td style="width:80%; text-align:left;"><asp:DropDownList ID="Cmb_Notarios" runat="server" Width="80%" ><asp:ListItem Text="&lt; SELECCIONE &gt;" Value="SELECCIONE"></asp:ListItem></asp:DropDownList>&#160;&#160;&#160;&#160;&#160;&#160;&#160; <asp:ImageButton ID="Btn_Buscar_Listado" runat="server" 
                                                CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                ToolTip="Buscar Listados" OnClick="Btn_Buscar_Listado_Click"/><asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Listado" runat="server"  
                                                CausesValidation="False"  
                                                ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Width="20px"  
                                                ToolTip="Limpiar Filtros" 
                                                OnClick="Btn_Limpiar_Filtros_Buscar_Listado_Click"  /></td></tr></table></div></ContentTemplate></cc1:TabPanel>--%>
                </cc1:TabContainer>
                <br />
                <br />
                <div style="width: 98%">
                    <center>
                        <caption>
                            <br />
                            <asp:GridView ID="Grid_Contrarecibos" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CssClass="GridView_1" GridLines="None" Width="98%" OnRowDataBound="Grid_Contrarecibos_RowDataBound"
                                PageIndex="5" OnSelectedIndexChanged="Grid_Contrarecibos_SelectedIndexChanged"
                                OnPageIndexChanging="Grid_Contrarecibos_PageIndexChanging">
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="30px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="NO_CONTRARECIBO" HeaderText="Contrarecibo" SortExpression="NO_CONTRARECIBO" />
                                    <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="Cuenta_Predial_ID" SortExpression="CUENTA_PREDIAL_ID">
                                        <FooterStyle Font-Size="0pt" Width="0px" />
                                        <HeaderStyle Font-Size="0pt" Width="0px" />
                                        <ItemStyle Font-Size="0pt" Width="0px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta Predial" NullDisplayText="SIN REGISTRO"
                                        SortExpression="CUENTA_PREDIAL" />
                                    <asp:BoundField DataField="NO_ESCRITURA" HeaderText="No. Escritura" SortExpression="NO_ESCRITURA" />
                                    <asp:BoundField DataField="FECHA_ESCRITURA" HeaderText="Fecha Escritura" DataFormatString="{0:dd/MMM/yyyy}"
                                        SortExpression="FECHA_ESCRITURA" />
                                    <asp:BoundField DataField="FECHA_LIBERACION" HeaderText="Fecha Liberación" DataFormatString="{0:dd/MMM/yyyy}"
                                        NullDisplayText="//" SortExpression="FECHA_LIBERACION" />
                                    <asp:BoundField DataField="FECHA_PAGO" HeaderText="Fecha Pago" DataFormatString="{0:dd/MMM/yyyy}"
                                        NullDisplayText="//" Visible="false" SortExpression="FECHA_PAGO" />
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:TextBox ID="Txt_Establecer_Cuenta_Predial" runat="server" Width="60px" Style="text-transform: uppercase"
                                                MaxLength="12"></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Establecer_Cuenta_Predial" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_circle_green.png"
                                                OnClick="Btn_Establecer_Cuenta_Predial_Click" AlternateText="Registrar Cuenta Predial" />
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Establecer_Cuenta_Predial" runat="server"
                                                TargetControlID="Txt_Establecer_Cuenta_Predial" FilterType="UppercaseLetters,LowercaseLetters, Numbers">
                                            </cc1:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                            </asp:GridView>
                        </caption>
                    </center>
                </div>
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

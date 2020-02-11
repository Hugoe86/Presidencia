<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Quitar_Beneficios.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Quitar_Beneficios" %>

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
    <asp:ScriptManager ID="ScriptManager" runat="server" AsyncPostBackTimeout="9999" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Contenido" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Quitar beneficio
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="98%" border="0" cellspacing="0">
                    <tr align="center">
                        <td colspan="2">
                            <div style="width: 99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold;
                                font-style: normal; font-variant: normal; font-family: fantasy; height: 32px">
                                <table style="width: 100%; height: 28px;">
                                    <tr>
                                        <td align="left" style="width: 59%;">
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                                Width="24px" CssClass="Img_Button" AlternateText="Nuevo" OnClick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" AlternateText="Eliminar"
                                                CssClass="Img_Button" TabIndex="2" Visible="false" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                                OnClientClick="return confirm('¿Está seguro que desea Quitar el Beneficio?');" />
                                            <%--<asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button" TabIndex="4"
                                            ImageUrl="~/paginas/imagenes/gridview/grid_print.png" />--%>
                                            <asp:ImageButton ID="Btn_Salir" runat="server" AlternateText="Salir" CssClass="Img_Button"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" OnClick="Btn_Salir_Click" />
                                        </td>
                                        <%--  <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                <td style="width:55%;">
                                                    <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5"  ToolTip = "Buscar" Width="180px"/>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                        WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" 
                                                        runat="server" TargetControlID="Txt_Busqueda" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                </td>
                                                <td style="vertical-align:middle;width:5%;" >
                                                    <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6"
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" OnClick="Btn_Buscar_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>--%>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            *Beneficio
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:DropDownList ID="Cmb_Beneficio" runat="server" AutoPostBack="true" Width="400px"
                                TabIndex="7" OnSelectedIndexChanged="Cmb_Beneficio_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 20%;">
                            Justificación
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Justificacion" runat="server" TextMode="MultiLine" Width="99%"></asp:TextBox>
                        </td>
                        <td style="text-align: left; width: 30%;">
                            <asp:TextBox ID="Txt_Cuenta_Predial" runat="server" Visible="false" Width="99%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="4">
                            <div style="width: 95%; height: 500px; background-color: #FFFFFF; color: #FFFFFF;
                                font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy;
                                overflow: auto;">
                                <asp:GridView ID="Grid_Quitar_Beneficios" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    CssClass="GridView_1" GridLines="None" PageSize="50" Width="97%" HeaderStyle-CssClass="tblHead"
                                    OnPageIndexChanging="Grid_Quitar_Beneficios_PageIndexChanging" Style="white-space: normal;">
                                    <Columns>
                                        <%--QUITE PALOMITA--%>
                                        <%-- <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" HorizontalAlign="Center"/>
                                    </asp:ButtonField>--%>
                                        <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="Cuenta predial" />
                                        <%--<asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="Cuenta predial ID" />--%>
                                        <asp:BoundField DataField="BENEFICIO" HeaderText="Beneficio" />
                                        <%--<asp:BoundField DataField="PORCENTAJE" HeaderText="Porcentaje" HeaderStyle-Width="15%" />--%>
                                        <%--<asp:BoundField DataField="CUOTA_FIJA" HeaderText="Cuota_Fija" HeaderStyle-Width="15%" />--%>
                                        <asp:BoundField DataField="FECHA_TRAMITE" HeaderText="Fecha tramitó" HeaderStyle-Width="15%"
                                            SortExpression="FECHA" DataFormatString="{0:dd/MMM/yyyy}">
                                            <HeaderStyle Width="15%" />
                                        </asp:BoundField>
                                        <%--<asp:BoundField DataField="PROPIEARIO" HeaderText="Propietario" HeaderStyle-Width="15%" />--%>
                                        <%-- <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" HeaderStyle-Width="15%" />--%>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="tblHead" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                                <br />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <td style="text-align: left; width: 30%;">
                        <asp:TextBox ID="Txt_Fecha" runat="server" Visible="false"></asp:TextBox>
                    </td>
                </table>
                <asp:TextBox ID="Txt_Usuario" runat="server" Visible="false"></asp:TextBox>
                <asp:HiddenField ID="Hdn_Cuenta_ID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Cat_Ate_Programas_Empleado.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Cat_Ate_Programas_Empleado" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script src="../jquery/jquery-1.5.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
     //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades)
        {
            window.showModalDialog(Url, null, Propiedades);
        }
               
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="True" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_General" runat="server" style="background-color: #ffffff; width: 98%;
                height: 100%;">
                <%--Fin del div General--%>
                <table width="100%" border="0" cellspacing="0" class="estilo_fuente" frame="border">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">
                            Asignar programas a empleados
                        </td>
                    </tr>
                    <tr>
                        <!--Bloque del mensaje de error-->
                        <td colspan="2">
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="100%" border="0" cellspacing="0" class="estilo_fuente" frame="border">
                    <tr class="barra_busqueda" align="right">
                        <td align="left" valign="middle" colspan="2">
                            <%--<div>--%>
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                CssClass="Img_Button" OnClick="Btn_Nuevo_Click" ToolTip="Nuevo" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                CssClass="Img_Button" OnClick="Btn_Modificar_Click" AlternateText="Modificar"
                                ToolTip="Modificar" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                CssClass="Img_Button" OnClick="Btn_Eliminar_Click" OnClientClick="return confirm('El registro seleccionado será eliminado. ¿Desea continuar?');"
                                AlternateText="Eliminar" ToolTip="Eliminar" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" OnClick="Btn_Salir_Click" CssClass="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" />
                            <%--</div>--%>
                        </td>
                        <td colspan="2">
                            Búsqueda
                            <asp:TextBox ID="Txt_Busqueda" runat="server"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                            WatermarkText="<Nombre empleado>" TargetControlID="Txt_Busqueda" />
                            <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                OnClick="Btn_Buscar_Click" />
                        </td>
                    </tr>
                </table>
                
                <table width="100%">
                    <tr>
                        <td style="width: 15%">
                            * Empleado
                        </td>
                        <td style="width: 55%">
                            <asp:DropDownList ID="Cmb_Empleado" runat="server" Width="95%" AutoPostBack="False"
                                DropDownStyle="DropDownList" AutoCompleteMode="SuggestAppend" CaseSensitive="False"
                                CssClass="WindowsStyle" ItemInsertLocation="Append" />
                        </td>
                        <td style="width: 15%">
                            <asp:LinkButton ID="Btn_Buscar_Empleado" runat="server" ForeColor="Blue" OnClick="Btn_Buscar_Empleado_Click">Búsqueda Empleado</asp:LinkButton>
                        </td>
                        <td style="width: 15%" align="right">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%">
                            * Programa (origen)
                        </td>
                        <td style="width: 55%">
                            <asp:DropDownList ID="Cmb_Programa" runat="server" Width="95%" AutoPostBack="False"
                                DropDownStyle="DropDownList" AutoCompleteMode="SuggestAppend" CaseSensitive="False"
                                CssClass="WindowsStyle" ItemInsertLocation="Append" />
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            * Estatus
                        </td>
                        <td>
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="35%" 
                                DropDownStyle="DropDownList" AutoCompleteMode="SuggestAppend" CaseSensitive="False"
                                CssClass="WindowsStyle" ItemInsertLocation="Append">
                                <asp:ListItem Value="0">&lt; SELECCIONE &gt;</asp:ListItem>
                                <asp:ListItem Value="ACTIVO">ACTIVO</asp:ListItem>
                                <asp:ListItem Value="INACTIVO">INACTIVO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                
                <div id="Div_Buscar_Programa_Empleado" runat="server" style="display: block;">
                        <table class="estilo_fuente" width="100%">
                            <tr>
                                <td style="width: 100%; text-align: center; vertical-align: top;">
                                    <center>
                                        <div id="Div1" runat="server" style="overflow: auto; height: 200px; width: 99%; vertical-align: top;
                                            border-style: solid; border-color: Silver; display: block">
                                            <asp:GridView ID="Grid_Programa_Empleado" runat="server" Width="100%" CssClass="GridView_1"
                                                HeaderStyle-CssClass="tblHead" GridLines="None" AllowPaging="false" AutoGenerateColumns="False"
                                                OnSelectedIndexChanged="Grid_Programa_Empleado_OnSelectedIndexChanged" EmptyDataText="No se encuentra ningun registro.">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="Empleado_Id" Visible="false" />
                                                    <asp:BoundField DataField="Programa_Id" Visible="false" />
                                                    <asp:BoundField DataField="Programa_Empleado_Id" Visible="false" />
                                                    <asp:BoundField DataField="NOMBRE_EMPLEADO" HeaderText="Nombre" HeaderStyle-Font-Size="12px"
                                                        ItemStyle-Font-Size="12px">
                                                        <HeaderStyle HorizontalAlign="Left" Width="56%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="56%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PROGRAMA" HeaderText="Programa" HeaderStyle-Font-Size="12px"
                                                        ItemStyle-Font-Size="12px">
                                                        <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" HeaderStyle-Font-Size="12px"
                                                        ItemStyle-Font-Size="12px">
                                                        <HeaderStyle HorizontalAlign="Center" Width="14%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="14%" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                        </div>
                                    </center>
                                </td>
                            </tr>
                        </table>
                </div>
                <div id="Div_Filtro_Empleados" runat="server" style="display: none">
                    <asp:Panel ID="Pnl_Filtro_Empleado" runat="server" GroupingText="Buscar Empleado"
                        ForeColor="Blue">
                        <table width="100%">
                            <tr style="background-color: Silver; color: Black; font-size: 12; font-weight: bold;
                                border-style: outset;">
                                <td style="width: 100%" align="right">
                                    <asp:ImageButton ID="Btn_Buscar_Empleado_Filtro" runat="server" CssClass="Img_Button"
                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar" OnClick="Btn_Buscar_Empleado_Filtro_Click" />
                                    <asp:ImageButton ID="Btn_Cerrar_Busqueda_Empleado" runat="server" ToolTip="Cerrar"
                                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClick="Btn_Cerrar_Busqueda_Empleado_Click" />
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td style="width: 15%">
                                    Empleado
                                </td>
                                <td style="width: 30%">
                                    <asp:TextBox ID="Txt_Filtro_Nombre_Empleado" runat="server" Width="95%" />
                                </td>
                                <td style="width: 15%" align="right">
                                    No Empleado
                                </td>
                                <td style="width: 30%">
                                    <asp:TextBox ID="Txt_Filtro_Numero_Empleado" runat="server" Width="95%" />
                                </td>
                                <td style="width: 10%" align="right" rowspan="2">
                                    <%--<asp:ImageButton ID="Btn_Buscar_Empleado" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                    OnClick="Btn_Buscar_Empleado_Click" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    U. Responsable
                                </td>
                                <td style="width: 30%" colspan="4">
                                    <cc1:ComboBox ID="Cmb_Filtro_Unidad_Responsable" runat="server" Width="550px" AutoPostBack="False"
                                        DropDownStyle="DropDownList" AutoCompleteMode="SuggestAppend" CaseSensitive="False"
                                        CssClass="WindowsStyle" ItemInsertLocation="Append" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <table width="100%">
                    <tr>
                        <td rowspan="5">
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

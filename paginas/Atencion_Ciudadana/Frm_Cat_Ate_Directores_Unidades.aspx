<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Cat_Ate_Directores_Unidades.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Cat_Ate_Directores_Unidades" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">

    <script src="../../jquery/jquery-1.5.js" type="text/javascript"></script>

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
                            Catálogo Director-Unidad Responsable
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
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Style="width: 200px;"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Nombre empleado>" TargetControlID="Txt_Busqueda" />
                            <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                OnClick="Btn_Buscar_Click" />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            *&nbsp;Unidad responsable
                        </td>
                        <td colspan="4">
                            <asp:DropDownList ID="Cmb_Unidad_Responsable" runat="server" Width="96%">
                            </asp:DropDownList>
                            <asp:ImageButton ID="Btn_Buscar_Dependencia" runat="server" ToolTip="Seleccionar Unidad responsable"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                OnClick="Btn_Buscar_Dependencia_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%">
                            * Empleado
                        </td>
                        <td style="width: 55%" colspan="4">
                            <asp:DropDownList ID="Cmb_Empleado" runat="server" Width="96%" AutoPostBack="False"
                                DropDownStyle="DropDownList" AutoCompleteMode="SuggestAppend" CaseSensitive="False"
                                CssClass="WindowsStyle" ItemInsertLocation="Append" />
                            <asp:ImageButton ID="Btn_Buscar_Empleado" runat="server" OnClick="Btn_Buscar_Empleado_Click"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" ToolTip="Seleccionar Empleado"
                                Height="22px" Width="22px" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%">
                            * Nombre
                        </td>
                        <td style="width: 55%" colspan="4">
                            <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" Width="96%" MaxLength="200" AutoPostBack="true" OnTextChanged="Txt_Nombre_Empleado_TextChanged" />
                            <cc1:FilteredTextBoxExtender ID="Ftbe_Txt_Nombre_Empleado" runat="server"
                                TargetControlID="Txt_Nombre_Empleado" FilterType="Custom, UppercaseLetters, LowercaseLetters"
                                ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
                <table class="estilo_fuente" width="100%">
                    <tr>
                        <td style="width: 100%; text-align: center; vertical-align: top;">
                            <center>
                                <div id="Div1" runat="server" style="height: 200px; width: 99%; vertical-align: top;
                                    border-style: solid; border-color: Silver; display: block; overflow-x: hidden;
                                    overflow-y: auto;">
                                    <asp:GridView ID="Grid_Organigrama" runat="server" Width="99%" CssClass="GridView_1"
                                        HeaderStyle-CssClass="tblHead" GridLines="None" AllowPaging="false" AutoGenerateColumns="False"
                                        Style="white-space: normal;" OnSelectedIndexChanged="Grid_Organigrama_OnSelectedIndexChanged"
                                        EmptyDataText="No se encontró ningún registro.">
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="Parametro_Id" Visible="false" />
                                            <asp:BoundField DataField="Empleado_Id" Visible="false" />
                                            <asp:BoundField DataField="Dependencia_Id" Visible="false" />
                                            <asp:BoundField DataField="NOMBRE_EMPLEADO" Visible="false" />
                                            <asp:BoundField DataField="EMPLEADO" HeaderText="Nombre" HeaderStyle-Font-Size="12px"
                                                ItemStyle-Font-Size="12px">
                                                <HeaderStyle HorizontalAlign="Left" Width="56%" />
                                                <ItemStyle HorizontalAlign="Left" Width="56%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UR" HeaderText="Unidad reponsable" HeaderStyle-Font-Size="12px"
                                                ItemStyle-Font-Size="12px">
                                                <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                                <ItemStyle HorizontalAlign="Left" Width="40%" />
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

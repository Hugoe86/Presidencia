<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Cat_Ate_Colonias.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Cat_Ate_Colonias" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="Upd_Panel_colonias" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Upr_colonias" runat="server" AssociatedUpdatePanelID="Upd_Panel_colonias"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Tabla" style="background-color: #ffffff; width: 98%; height: 100%;">
                <center>
                    <table width="97%" border="0" cellspacing="0" class="estilo_fuente">
                        <tr align="center">
                            <td colspan="4" class="label_titulo">
                                Colonias
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <%--Contenedor del Mensaje de Error--%>
                                <div id="Div_Contenedor_Msj_Error" style="width: 100%; font-size: 9px;" runat="server"
                                    visible="false">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td colspan="2" align="left" style="font-size: 12px; color: Red; font-family: Tahoma;
                                                text-align: left;">
                                                <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
                                                    Width="24px" Height="24px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%;">
                                            </td>
                                            <td style="font-size: 9px; width: 90%; text-align: left;" valign="top">
                                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </div>
                            </td>
                        </tr>
                        <%--Manejo de la barra de busqueda--%>
                        <tr class="barra_busqueda">
                            <td colspan="2" align="left">
                                <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                                <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click" />
                                <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" OnClick="Btn_Eliminar_Click"
                                    OnClientClick="return confirm('¿Está seguro de eliminar el asunto seleccionado?');" />
                                <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                            </td>
                            <td colspan="2" align="right">
                                Busqueda
                                <asp:TextBox ID="Txt_Busqueda_colonias" runat="server"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Buscar_colonia" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                    OnClick="Btn_Buscar_colonia_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                        </tr>
                        <%--Manejo de las cajas TextBox--%>
                        <tr align="left">
                            <td colspan="1">
                                Colonia ID
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txt_Colonia_ID" runat="server" Width="198px" Enabled="False" TabIndex="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                        </tr>
                        <tr align="left">
                            <td colspan="1" style="width: 100px">
                                Nombre
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txt_Nombre" runat="server" Width="600px" TabIndex="2"></asp:TextBox>
                            </td>
                        </tr>
                        <tr align="left">
                            <td colspan="1" style="width: 100px">
                                Tipo de Colonia
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="Cmb_Tipo_Colonia" runat="server" Width="600px" TabIndex="3">
                                </asp:DropDownList>
                            </td>
                        </tr>                        
                        <tr>
                            <td colspan="4">
                        </tr>
                        <tr align="left">
                            <td colspan="1" style="width: 100px">
                                Descripcion
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txt_Descripcion" runat="server" TabIndex="4" MaxLength="250" TextMode="MultiLine"
                                    Width="600px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1">
                            </td>
                            <td colspan="3" class="estilo_fuente_comentarios">
                                Limite de 100 caracteres
                            </td>
                        </tr>
                        <%--Creacion y configuracion del gridView Grid_Colonias--%>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:GridView ID="Grid_Colonias" runat="server" AutoGenerateColumns="False" CssClass="GridView_1"
                                    Width="721px" AllowPaging="True" PageSize="5" OnPageIndexChanging="Grid_Colonias_PageIndexChanging"
                                    OnSelectedIndexChanged="Grid_Colonias_SelectedIndexChanged" GridLines="None">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="Colonia_ID" HeaderText="Colonia ID" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Nombre" HeaderText="Colonia" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" Width="55%" />
                                            <ItemStyle HorizontalAlign="Left" Width="55%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" Width="55%" />
                                            <ItemStyle HorizontalAlign="Left" Width="55%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TIPO_COLONIA_ID" HeaderText="Tipo_Colonia" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td style="width: 238px;">
                                &nbsp;
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Cph_Area_Trabajo2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="head">

</asp:Content>

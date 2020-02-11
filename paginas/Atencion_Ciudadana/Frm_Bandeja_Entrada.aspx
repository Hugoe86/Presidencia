<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Bandeja_Entrada.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Bandeja_Entrada"
    Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager_Asuntos" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressTemplate"></div>
                <div  class="processMessage" id="div_progress">
                    <img alt="" src="../Imagenes/paginas/Updating.gif" />
                </div>
            </ProgressTemplate>
            </asp:UpdateProgress> 
            <div id="Div_Requisitos" style="background-color: #ffffff; width: 97%; height: 100%;">
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td align="center" class="label_titulo">
                            Bandeja de Pendientes
                        </td>
                    </tr>
                    <tr>
                        <td class="barra_busqueda">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            Usted tiene los siguientes pendientes.
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            Para resolver o dar seguimiento a las peticiones pulse
                            <asp:LinkButton ID="LBtn_Ir_a" runat="server" OnClick="LBtn_Ir_a_Click">aquí</asp:LinkButton>
                            .
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="Lbl_Informacion" runat="server" Enabled="False" Width="100%"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:GridView ID="Grid_Cantidad_Peticiones" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                OnPageIndexChanging="Grid_Peticiones_Pendientes_PageIndexChanging" Width="60%">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/grid_info.png">
                                        <ItemStyle Width="10%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Peticiones" HeaderText="Peticiones" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%"/>
                                        <ItemStyle HorizontalAlign="Left" Width="20%"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Area" HeaderText="Área" 
                                        Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:GridView ID="Grid_Peticiones_Pendientes" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CssClass="GridView_1" GridLines="None" OnPageIndexChanging="Grid_Peticiones_Pendientes_PageIndexChanging" Width = "80%">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/grid_info.png">
                                        <ItemStyle Width="10%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descripcion_Peticion" HeaderText="Petición" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Nivel_Importancia" HeaderText="Nivel de Importancia" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                    </td>
                    <tr>
                        <td align="center">
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



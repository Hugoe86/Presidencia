<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Presupuesto_UR.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Ope_Presupuesto_UR" 
    Title="Presupuesto UR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true">
    </asp:ScriptManager>
    
    <div id="Div_General" style="width: 98%;" visible="true" runat="server">
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
            <ContentTemplate>
            
                        
                <%--               <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>                    
                </asp:UpdateProgress>--%>
                <%--Div Encabezado--%>
                
                
                <div id="Div_Encabezado" runat="server">
                    <table style="width: 100%;" border="0" cellspacing="0">
                        <tr align="center">
                            <td colspan="4" class="label_titulo">
                                Consulta de Presupuestos 
                            </td>
                        </tr>
                        <tr align="left">
                            <td colspan="4">
                                <asp:Image ID="Img_Warning" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />
                                <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="#990000"></asp:Label>
                            </td>
                        </tr>
                        <tr class="barra_busqueda" align="right">
                            <td align="left" valign="middle" colspan="2">
<%--                                <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" CssClass="Img_Button" ToolTip="Nuevo" OnClick="Btn_Nuevo_Click" />
                                <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" CssClass="Img_Button" AlternateText="Modificar" ToolTip="Modificar" OnClick="Btn_Modificar_Click" />
                                <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" CssClass="Img_Button" AlternateText="Eliminar" ToolTip="Eliminar" />
                                <asp:ImageButton ID="Btn_Listar_Requisiciones" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Listar Requisiciones" OnClick="Btn_Listar_Requisiciones_Click" />
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" OnClick="Btn_Salir_Click" />--%>
                            </td>
                            <td colspan="2">
<%--                                <asp:Button ID="Btn_Comodin_Busqueda_Productos_Srv" runat="server" Text="Button" Style="display: none" />
                                <cc1:ModalPopupExtender ID="Modal_Busqueda_Prod_Serv" runat="server" TargetControlID="Btn_Comodin_Busqueda_Productos_Srv" PopupControlID="Modal_Productos_Servicios" CancelControlID="Btn_Cerrar"
                                    DropShadow="True" BackgroundCssClass="progressBackgroundFilter" />--%>
                            </td>
                        </tr>
                    </table>
                </div>
                <%--Div listado de requisiciones--%>
                <div id="Div_Listado_Requisiciones" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 15%;">
                                Unidad Responsable
                            </td>
                            <td style="width: 35%;">
                                <asp:DropDownList ID="Cmb_Dependencia_Panel" runat="server" Width="98%" />
                                
                            </td>
                            <td style="width: 45%;" align="right" visible="false" >
                                <asp:Button ID="Button1" runat="server" Text="Consultar presupuestos" onclick="Button1_Click" CssClass="button" Width="60%"/>
                            </td>
                            <td >
                            </td>
                        </tr>                    
                        <tr>
                            <td style="width: 15%;">
                            </td>
                            <td style="width: 35%;">
                            </td>
                            <td align="right" visible="false">
                            </td>
                            <td visible="false">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 99%" align="center" colspan="4">
                                <asp:GridView ID="Grid_Requisiciones" Visible="false" runat="server" AllowPaging="True" AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" Width="100%" OnPageIndexChanging="Grid_Requisiciones_PageIndexChanging"
                                    
                                    AllowSorting="true" HeaderStyle-CssClass="tblHead" OnSorting="Grid_Requisiciones_Sorting">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                    
                                        <asp:BoundField DataField="UR" HeaderText="U. Responsable" Visible="false" SortExpression="UR" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" Wrap="true" Font-Size="Smaller"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FUENTE" HeaderText="Fte. Financiamiento" 
                                            Visible="false" SortExpression="FUENTE">
                                            <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" Wrap="true" Font-Size="Smaller"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PROGRAMA" HeaderText="Programa" Visible="True" SortExpression="PROGRAMA">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="true" Font-Size="Smaller"/>
                                        </asp:BoundField>                                                                             
                                        <asp:BoundField DataField="PARTIDA" HeaderText="Partida" Visible="true" SortExpression="PARTIDA">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" Wrap="true" Font-Size="Smaller"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ASIGNADO" HeaderText="Asignado" Visible="true" SortExpression="ASIGNADO" DataFormatString="{0:n}">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Right" Width="12%" Font-Size="Smaller"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DISPONIBLE" HeaderText="Disponible" Visible="true" SortExpression="DISPONIBLE" DataFormatString="{0:n}">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Right" Font-Size="Smaller"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="COMPROMETIDO" HeaderText="Comprometido" Visible="true" SortExpression="COMPROMETIDO" DataFormatString="{0:n}">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Right" Width="12%" Font-Size="Smaller"/>
                                        </asp:BoundField>                                        
                                        <asp:BoundField DataField="DISPONIBLE" HeaderText="Disponible" Visible="true" SortExpression="DISPONIBLE" DataFormatString="{0:n}">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Right" Width="12%" Font-Size="Smaller"/>
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
                </div>
                <%--Div Contenido--%>
                <div id="Div_Contenido" runat="server">
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>    
</asp:Content>


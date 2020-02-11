<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Psw.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Ope_Psw" Title="Asignacíon Rol y Password" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true">
    </asp:ScriptManager>
    <div id="Div_General" style="width: 98%;" visible="true" runat="server">
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>                    
                </asp:UpdateProgress>
                <%--Div Encabezado--%>
                <div id="Div_Encabezado" runat="server">
                    <table style="width: 100%;" border="0" cellspacing="0">
                        <tr align="center">
                            <td colspan="4" class="label_titulo">
                               Asignar Rol y Password
                            </td>
                        </tr>
                        <tr align="left">
                            <td colspan="4">
                                <asp:Image ID="Img_Warning" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                    Visible="false" />
                                <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="#990000"></asp:Label>
                            </td>
                        </tr>
                        <tr class="barra_busqueda" align="right">
                            <td align="left" valign="middle" colspan="2">
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                    ToolTip="Inicio" onclick="Btn_Salir_Click"  />
                                <asp:ImageButton ID="Btn_Actualizar_Datos" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                    ToolTip="Modificar" onclick="Btn_Actualizar_Datos_Click"  />                                    
                            </td>
                            <td colspan="2">
                            </td>
                        </tr>
                    </table>
                </div>
                <%--Div listado de requisiciones--%>
                <div id="Div_Contenido" runat="server">
                 <asp:Panel ID="Pnl_Datos_Generales" runat="server"  GroupingText="Datos del Empleado" Width="99%" >
                    <table style="width: 100%;">
                        <tr class="button_autorizar">
                            <td style="width:25%;" class="button_autorizar">No. Empleado</td>
                            <td>
                                <asp:TextBox ID="Txt_No_Empleado" runat="server"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                 ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                 ToolTip="Buscar" onclick="Btn_Buscar_Click"/>
                            </td>
                        </tr>
                        <tr class="button_autorizar">
                            <td>Nombre</td>
                            <td>
                                <asp:TextBox ID="Txt_Nombre" runat="server" Width="70%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="button_autorizar">
                            <td>U.Responsable del Empleado</td>
                            <td>
                                <asp:DropDownList ID="Cmb_UR_Empleado" runat="server"  Width="70%">
                                </asp:DropDownList>
                            </td>
                        </tr>                        
                        <tr class="button_autorizar">
                            <td>Rol</td>
                            <td>
                                <asp:DropDownList ID="Cmb_Rol_Empleado" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="button_autorizar">
                            <td>Password</td>
                            <td>
                                <asp:TextBox ID="Txt_Password" runat="server" TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="button_autorizar">    
                            <td>Confirmar</td>
                            <td>
                                <asp:TextBox ID="Txt_Confirmar_Password" runat="server" TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="button_autorizar">
                            <td>U. Responsable</td>
                            <td>
                                <asp:DropDownList ID="Cmb_Dependencias" runat="server"  Width="70%">
                                </asp:DropDownList>
                                <asp:ImageButton ID="Btn_Agregar_Dependencia" runat="server" 
                                 ImageUrl="~/paginas/imagenes/paginas/accept.png" 
                                 ToolTip="Agregar" onclick="Btn_Agregar_Dependencia_Click"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="Grid_Dependencias" runat="server"
                                     AutoGenerateColumns="False" CssClass="GridView_1" 
                                    Enabled="false" GridLines="None" HeaderStyle-CssClass="tblHead" 
                                     Width="100%" 
                                    onselectedindexchanged="Grid_Dependencias_SelectedIndexChanged">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/paginas/delete.png" Text="Quitar">
                                            <ItemStyle Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="Dependencia_ID" HeaderText="Dependencia_ID" 
                                            Visible="true" >
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%"/>
                                        </asp:BoundField>
                                        
                                        <asp:BoundField DataField="Dependencia" HeaderText="Dependencia" 
                                            SortExpression="Descripcion" Visible="True">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="75%" />
                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Programas.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Programas" Title="Catálogo de Programas" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Programas" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Programas" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">Catálogo de Programas</td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td colspan="2" align = "left">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="1"
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="2"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" TabIndex="3"
                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                OnClientClick="return confirm('¿Está seguro de eliminar el Área seleccionada?');"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="4"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                        </td>
                        <td colspan="2">Busqueda
                            <asp:TextBox ID="Txt_Busqueda_Programa" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar por Nombre"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Programa" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese Nombre>" TargetControlID="Txt_Busqueda_Programa" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Programa" runat="server" 
                                TargetControlID="Txt_Busqueda_Programa" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </cc1:FilteredTextBoxExtender>
                            <asp:ImageButton ID="Btn_Buscar_Programa" runat="server" ToolTip="Consultar" TabIndex="6"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Programa_Click" />
                        </td>                        
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>
                    <tr>
                        <td>Programa ID</td>
                        <td>
                            <asp:TextBox ID="Txt_Programa_ID" runat="server" ReadOnly="True" Width="150px"></asp:TextBox></td>
                        <td>Estatus</td>
                        <td>
                            <asp:DropDownList ID="Cmb_Estatus_Programa" runat="server" Width="150px" TabIndex="7">
                                <asp:ListItem>ACTIVO</asp:ListItem>
                                <asp:ListItem>INACTIVO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>*Nombre</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Nombre_Programa" runat="server" MaxLength="100" TabIndex="8" Width="600px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Programa" runat="server" 
                                TargetControlID="Txt_Nombre_Programa" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>*U. Responsable</td>
                        <td colspan="3">
                            <asp:DropDownList ID="Cmb_Dependencia_Programa" runat="server" TabIndex="9" Width="600px"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Comentarios</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Comentarios_Programa" runat="server" TabIndex="10" MaxLength="250"
                                TextMode="MultiLine" Width="600px" AutoPostBack="True"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Programa" runat="server" WatermarkCssClass="watermarked"
                                TargetControlID ="Txt_Comentarios_Programa" WatermarkText="Límite de Caractes 250">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Programa" runat="server" TargetControlID="Txt_Comentarios_Programa" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>                    
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_Programa" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                PageSize="5" onpageindexchanging="Grid_Programa_PageIndexChanging" 
                                onselectedindexchanged="Grid_Programa_SelectedIndexChanged"
                                AllowSorting="True" OnSorting="Grid_Programa_Sorting" HeaderStyle-CssClass="tblHead">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="7%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Programa_ID" HeaderText="Programa ID" 
                                        Visible="True" SortExpression="Programa_ID">
                                        <HeaderStyle HorizontalAlign="Left" Width="18%" />
                                        <ItemStyle HorizontalAlign="Left" Width="18%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Nombre" HeaderText="Programa" Visible="True" SortExpression="Nombre">
                                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DEPENDENCIA" HeaderText="U. Responsable" Visible="True" SortExpression="DEPENDENCIA">
                                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


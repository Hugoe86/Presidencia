<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Sectores.aspx.cs" Inherits="paginas_predial_Frm_Cat_Pre_Sectores" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="Upd_Sectores" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Sectores" runat="server" AssociatedUpdatePanelID="Upd_Sectores"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="General" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table id="Tbl_Comandos" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                    <tr>
                        <td class="label_titulo" colspan="2">
                            Catálogo de Sectores
                        </td>
                    </tr>
                    <tr>
                        <div id="Div_Contenedor_Msj_Error" runat="server">
                            <td colspan="2">
                                <asp:Image ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" />
                                <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                <br />
                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" TabIndex="0" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            </td>
                        </div>
                    </tr>
                    <tr class="barra_busqueda">
                        <td style="width: 50%">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                CssClass="Img_Button" OnClick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                CssClass="Img_Button" OnClick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                CssClass="Img_Button" OnClientClick="return confirm('¿Esta seguro de Eliminar el presente registro?');"
                                OnClick="Btn_Eliminar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                CssClass="Img_Button" OnClick="Btn_Salir_Click" />
                        </td>
                        <td align="right" style="width: 50%">
                            Búsqueda:
                            <asp:TextBox ID="Txt_Busqueda_Sector" runat="server" Width="180" ToolTip="Buscar" TabIndex="1"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Sector" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                TabIndex="2" OnClick="Btn_Buscar_Sector_Click" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkText="<Buscar por Clave>"
                                TargetControlID="Txt_Busqueda_Sector" WatermarkCssClass="watermarked">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda_Sector"
                                InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>
                <cc1:TabContainer ID="Tab_Catalogo_Sectores" runat="server" Width="98%" ActiveTabIndex="0" 
                    >
                    <cc1:TabPanel ID="Tab_Sectores" runat="server" HeaderText="Claves de Ingreso" ActiveTabIndex="0">
                        <HeaderTemplate>
                            Sectores</HeaderTemplate>
                        <ContentTemplate>
                            <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                             
                                <tr>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="Txt_ID_Sector" runat="server" ReadOnly="True" Visible="False" Width="92%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        * Clave
                                    </td>
                                    <td style="width: 32%">
                                        <asp:TextBox ID="Txt_Clave_Sector" runat="server" Width="94%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        Nombre
                                    </td>
                                    <td style="width: 32%">
                                        <asp:TextBox ID="Txt_Nombre" runat="server" Width="94%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        Comentarios
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Comentarios" runat="server" Width="97%" Height="60px" TextMode="MultiLine"></asp:TextBox>
                                        <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Comentarios" runat="server" WatermarkCssClass="watermarked"
                                            WatermarkText="Límite de Caractes 250" TargetControlID="Txt_Comentarios" Enabled="True">
                                        </cc1:TextBoxWatermarkExtender>
                                        <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" runat="server"
                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="Txt_Comentarios"
                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="Tab_Sectores_Colonias" runat="server" HeaderText="Grupos de Movimientos">
                        <HeaderTemplate>
                            Sectores - Colonias</HeaderTemplate>
                        <ContentTemplate>
                            <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                                <tr>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="Txt_Clave_Colonia" runat="server" Width="94%" 
                                            Visible = "False" />
                                </tr>
                                
                                <tr>
                                    <td style="width: 18%">
                                        Colonia
                                    </td>
                                    <td style="width: 77%">
                                        <asp:DropDownList ID="Cmb_Colonia" runat="server" Width="94%" AutoPostBack="True"
                                        OnSelectedIndexChanged = "Llenar_Clave_Colonia">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5%">
                                        <asp:ImageButton ID="Img_Agregar" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                        OnClick = "Btn_Agregar_Click"    />
                                    </td>    
                                </tr>
                                <tr>
                                    <td>
                                     &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:GridView ID="Grid_Colonias_Sectores" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                            GridLines="None" PageSize="5" Style="white-space: normal" Width="96%"
                                            OnPageIndexChanging = "Grid_Sectores_Colonias_PageIndexChanging" onrowcommand="Borrar_Registro"
                                            >
                                            <RowStyle CssClass="GridItem" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="5%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="COLONIA_ID" HeaderText="Colonia ID">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CLAVE" HeaderText="Clave">
                                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                                    <HeaderStyle HorizontalAlign="Center" Width="80%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="80%" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Btn_Eliminar_Sector_Colonia" runat="server" Height="20px" 
                                                            ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png"  TabIndex="10" 
                                                            OnClientClick = "return confirm('¿Esta seguro que desea eliminar el registro?')"
                                                            ToolTip="Borrar_Recepcion" Width="20px"  CommandName="Erase" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="GridHeader" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 18%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 77%">
                                        &nbsp;
                                    </td>
                                    <td style="width: 5%">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    
          
           
            
        </cc1:TabContainer>
        <center>
         <table id="Tbl_Sectores" border="0" cellspacing="0" class="estilo_fuente" style="width: 100%;">
             <tr>
                                    <td colspan="4" align="center">
                                        <asp:GridView ID="Grid_Sectores" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CssClass="GridView_1" EmptyDataText="&quot;No se encontraron registros&quot;"
                                            GridLines="None" PageSize="5" Style="white-space: normal" Width="96%" OnSelectedIndexChanged="Grid_Sectores_SelectedIndexChanged"
                                            OnPageIndexChanging="Grid_Sectores_PageIndexChanging">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="5%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="SECTOR_ID" HeaderText="Clave Ingreso ID">
                                                    <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CLAVE" HeaderText="Clave">
                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                                    <HeaderStyle HorizontalAlign="Center" Width="80%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="80%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="COMENTARIOS" HeaderText="Comentarios">
                                                    <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
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
            </center>
        
        
         </div> </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Ope_Com_Comite_Compras.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Comite_Compras" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
        <ContentTemplate> 
        <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <%--Div de Contenido --%>
            <div id="Div_Contenido" style="width:97%;height:100%;">
            <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
                <tr>
                    <td colspan ="4" class="label_titulo">Comite de Compras</td>
                </tr>
                 <%--Fila de div de Mensaje de Error --%>
                <tr>
                    <td colspan ="4">
                        <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                        <table style="width:100%;">
                            <tr>
                                <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                                <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                Width="24px" Height="24px"/>
                                </td>            
                                <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />
                                </td>
                            </tr> 
                        </table>                   
                        </div>
                    </td>
                </tr>
                 <%--Fila de Busqueda y Botones Generales --%>
                <tr class="barra_busqueda">
                    <td style="width:20%;">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                            ToolTip="Nuevo" onclick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" 
                            ToolTip="Modificar" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                            onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                            OnClick="Btn_Salir_Click"/>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td align="right" colspan="3" style="width:80%;">
                        <div id="Div_Busqueda" runat="server">
                        <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                        onclick="Btn_Avanzada_Click" ToolTip="Busqueda Avanzada">Busqueda</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese un Folio>"
                                TargetControlID="Txt_Busqueda" />
                        <asp:ImageButton ID="Btn_Buscar" runat="server" 
                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                        onclick="Btn_Buscar_Click" ToolTip="Consultar"/>
                        </div>
                        <asp:Button ID="Btn_Comodin_2" runat="server" Text="Button" style="display:none;"/>
                       <%-- <cc1:ModalPopupExtender ID="Modal_Busqueda" runat="server"
                        TargetControlID="Btn_Comodin_2"
                        PopupControlID="Pnl_Busqueda"                      
                        CancelControlID="Btn_Cancelar"
                        DropShadow="True"
                        BackgroundCssClass="progressBackgroundFilter"/>--%>
                    </td> 
                </tr>
                <%--Fila de Listado de Requisiciones de Comite de Compras --%>
                <tr>
                    <td colspan="4">
                        <div id="Div_Listado_Comite" runat="server" style="width:100%;">
                        <asp:GridView ID="Grid_Comite_Compras" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" CssClass="GridView_1" DataKeyNames="No_Comite_Compras" 
                            OnSelectedIndexChanged = "Grid_Comite_Compras_SelectedIndexChanged"
                            GridLines="None"  onpageindexchanging="Grid_Comite_Compras_PageIndexChanging" PageSize="10">
                            <Columns>
                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                    <ItemStyle Width="5%" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="No_Comite_Compras" HeaderText="No_Comite_Compras" 
                                    Visible="false">
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="True">
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True">
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" Visible="True">
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Monto_Total" HeaderText="Monto Total" 
                                    Visible="True">
                                    <HeaderStyle HorizontalAlign="Right" Width="20%" />
                                    <ItemStyle HorizontalAlign="Right" Width="20%" />
                                </asp:BoundField>
                            </Columns>
                            <PagerStyle CssClass="GridHeader" />
                            <SelectedRowStyle CssClass="GridSelected" />
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                 <%--Fila de Datos Generales del Proceso de Comite de Compras --%>
                <tr>
                    <td>
                        <div id="Div_Comite_Compras" runat="server" style="width:100%;">
                        <tabla style="width:98%;">
                            <tr>
                                <td colspan="4" align="center">
                                    Datos Generales
                                </td>
                            </tr>  
                            <tr>
                                <td style="width:15%;">
                                    Folio
                                </td>
                                <td style="width:35%;">
                                    <asp:TextBox ID="Txt_Folio" runat="server" Width="97%" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width:15%;">
                                    Fecha
                                </td>
                                <td style="width:35%;">
                                    <asp:TextBox ID="Txt_Fecha" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tipo
                                </td>
                                <td>
                                    <asp:DropDownList ID="Cmb_Tipo" runat="server" Width="99%" Enabled="false"></asp:DropDownList>
                                </td>
                                <td style="width:15%;">
                                    Estatus
                                </td>
                                <td style="width:35%;">
                                    <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%" Enabled="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Justificacion
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Justificacion" runat="server" TabIndex="10" Enabled="false"     
                                    TextMode="MultiLine" Width="99%"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" WatermarkCssClass="watermarked"
                                    WatermarkText="<Indica el motivo de realizar la requisición>" TargetControlID="Txt_Justificacion" />
                                </td>                            
                            </tr>
                            <tr>
                                <td>
                                    Comentarios
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Comentario" runat="server" TabIndex="10" MaxLength="250"
                                    TextMode="MultiLine" Width="99%"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkCssClass="watermarked"
                                    WatermarkText="<Límite de Caracteres 250>" 
                                    TargetControlID="Txt_Comentario" />
                                </td>                            
                            </tr>
                            <tr>
                                <td colspan="4" class="barra_delgada">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    Agregar Requisiciones 
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                     <cc1:TabContainer ID="Tab_Requisiciones_Consolidaciones" runat="server" 
                                            ActiveTabIndex="1" Width="98%">
                                            <cc1:TabPanel ID="TabPnl_Buscar_Requisiciones" runat="server" Visible="true" Width="99%">
                                            <HeaderTemplate>Agregar Requisiciones/Consolidaciones</HeaderTemplate>
                                        <ContentTemplate>
                                        <table style="width:99%">
                                            <tr>
                                                <td>Seleccione una opción:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:CheckBox ID="Chk_Requisiciones" runat="server" AutoPostBack="True"
                                                        Text="Agregar Requisiciones a Licitacion" 
                                                        OnCheckedChanged="Chk_Requisiciones_CheckedChanged">
                                                    </asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:CheckBox ID="Chk_Consolidaciones" runat="server" AutoPostBack="True"
                                                        Text="Agregar Consolidaciones a Licitacion" 
                                                        oncheckedchanged="Chk_Consolidaciones_CheckedChanged">
                                                    </asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                 <div id="Div_Requisiciones_Busqueda" runat="server" 
                                                        style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" 
                                                        visible="False">
                                                 <asp:GridView ID="Grid_Requisiciones_Busqueda" runat="server" AllowPaging="True" 
                                                         AutoGenerateColumns="False" CssClass="GridView_1" DataKeyNames="No_Requisicion" 
                                                         GridLines="None" Width="96%">
                                                    <Columns>
                                                    <asp:TemplateField HeaderText="Selecciona">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:CheckBox ID="Chk_Requisicion_Seleccionada" runat="server" AutoPostBack="True"/>
                                                            </center>
                                                        </ItemTemplate>
                                                        <controlstyle width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="No_Requisicion" HeaderText="ID">
                                                            <FooterStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Folio" HeaderText="Folio">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Dependencia" HeaderText="Dependencia">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Area" HeaderText="Area">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Total" HeaderText="Total">
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                     </Columns>  
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    
                                                     </asp:GridView>  
                                                  </div>
                                                  <div id="Div_Consolidaciones_Busqueda" runat="server" 
                                                        style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" 
                                                        visible="False">
                                                    <asp:GridView ID="Grid_Consolidaciones_Busqueda" runat="server" AllowPaging="True" 
                                                         AutoGenerateColumns="False" CssClass="GridView_1" DataKeyNames="No_Consolidacion" 
                                                         GridLines="None" Width="96%">
                                                    <Columns>                       
                                                    <asp:TemplateField HeaderText="Selecciona">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:CheckBox ID="Chk_Consolidacion_Seleccionada" runat="server" AutoPostBack="True"/>
                                                            </center>
                                                        </ItemTemplate>
                                                        <controlstyle width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="No_Consolidacion" HeaderText="ID">
                                                            <FooterStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Folio" HeaderText="Folio">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Total" HeaderText="Total">
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    
                                                        </Columns><AlternatingRowStyle CssClass="GridAltItem" />
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    
                                                      </asp:GridView>
                                                   </div>
                                            </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4">
                                                <asp:Button ID="Btn_Agregar_Requisicion_Consolidaciones" runat="server" 
                                                Text="Agregar Consolidacion/Requisicion" Width="250px" 
                                                onclick="Btn_Agregar_Requisicion_Consolidaciones_Click" CssClass="button"/>
                                                </td>
                                            </tr>
                                        </table>
                                        
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel ID="TabPnl_Requisiciones" runat="server" Visible="true">
                                            <HeaderTemplate>Requisiciones y Consolidaciones</HeaderTemplate>
                                        <ContentTemplate>
                                        <table style="width:99%">
                                        
                                        <tr>
                                            <td>
                                                <center>Requisiciones</center>
                                                <asp:GridView ID="Grid_Requisiciones" runat="server" AllowPaging="True"  PageSize="5"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" DataKeyNames="No_Requisicion" 
                                                    GridLines="None" onpageindexchanging="Grid_Requisiciones_PageIndexChanging" 
                                                    onselectedindexchanged="Grid_Requisiciones_SelectedIndexChanged" Width="99%">
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/paginas/delete.png">
                                                    <ItemStyle Width="5%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="No_Requisicion" HeaderText="No_Requisicion" 
                                                        Visible="False">
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Folio" HeaderText="Folio">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Dependencia" HeaderText="Dependencia">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Area" HeaderText="Area">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Total" HeaderText="Total">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                
                                                    </Columns>
                                                <HeaderStyle CssClass="GridHeader" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                </asp:GridView>  
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="barra_delgada">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                Consolidaciones
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="Grid_Consolidaciones" runat="server" AllowPaging="True" 
                                                    AutoGenerateColumns="False" CssClass="GridView_1" DataKeyNames="No_Consolidacion" 
                                                    GridLines="None" Width="99%" PageSize="5" 
                                                    onselectedindexchanged=" Grid_Consolidaciones_SelectedIndexChanged">
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                    <Columns>                       
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/paginas/delete.png">
                                                    <ItemStyle Width="5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="No_Consolidacion" HeaderText="No_Consolidacion" 
                                                            Visible="False">
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Folio" HeaderText="Folio">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Total" HeaderText="Total">
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Lista_Requisiciones" HeaderText="Consolidacion" 
                                                            Visible="False">
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" colspan="4">
                                                Total&nbsp;
                                                <asp:TextBox ID="Txt_Total" runat="server" Enabled="False" 
                                                style="text-align:right" Width="200px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        </table>
                                        
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        </cc1:TabContainer>
                                </td>
                            </tr>
                            
                            <tr>
                                <td colspan="4">
                                </td>
                            </tr>
                        </tabla>
                        </div>
                    </td>
                </tr>
            </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    
</asp:Content>

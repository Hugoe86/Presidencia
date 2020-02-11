<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Caj_Empleados_Folios.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Caj_Empleados_Folios" Title="Asignación de Folios a Empleados" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Empleados_Folios" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <%--<asp:UpdateProgress ID="Uprg_Empleados_Folios" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress"><img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                
            </asp:UpdateProgress>--%>
            <div id="Div_IMSS" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Asignacion de Folios a Empleados</td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="98%"  border="0" cellspacing="0">
                    <tr align="center">
                        <td>                
                            <div align="right" class="barra_busqueda">                        
                                <table style="width:100%;height:28px;">
                                    <tr>
                                        <td align="left" style="width:59%;"> 
                                            <asp:UpdatePanel ID="Upnl_Botones_Operacion" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
                                                <ContentTemplate> 
                                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" 
                                                        TabIndex="1" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                                        onclick="Btn_Nuevo_Click" />
                                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" 
                                                        TabIndex="2" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                                        onclick="Btn_Modificar_Click" />
                                                     <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" 
                                                        CssClass="Img_Button" TabIndex="3"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                                        
                                                        OnClientClick="return confirm('¿Está seguro de eliminar el Registro seleccionado?');" 
                                                        onclick="Btn_Eliminar_Click"/>
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" 
                                                        TabIndex="4" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                                        onclick="Btn_Salir_Click"/>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>                                                
                                        </td>
                                        <td style="width:50%">Busqueda
                                            <asp:TextBox ID="Txt_Busqueda_Empleado" runat="server" MaxLength="10" TabIndex="5" 
                                                ToolTip="Buscar por Empleado" Width="200px"/>
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Empleado" runat="server" WatermarkCssClass="watermarked"
                                                WatermarkText="<Ingrese el  No Empleado>" TargetControlID="Txt_Busqueda_Empleado" />
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Empleado" runat="server" 
                                                TargetControlID="Txt_Busqueda_Empleado" FilterType="Custom, Numbers">
                                            </cc1:FilteredTextBoxExtender>
                                            <asp:ImageButton ID="Btn_Buscar_Empleado" runat="server" ToolTip="Consultar" TabIndex="6"
                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                onclick="Btn_Buscar_Empleado_Click"/>
                                        </td>       
                                    </tr>         
                                </table>                   
                            </div>
                        </td>
                    </tr>
                </table>
                <br /> 
                <asp:UpdatePanel ID="Upnl_Datos_Generales_Empleado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>          
                        <asp:Panel ID="Pnl_Datos_Generales" runat="server" GroupingText="Datos Generales del Empleado" Width="97%" BackColor="White">              
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">  
                                <tr>
                                    <td style="text-align:left;width:20%;">No Empleado</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_No_Empleado" runat="server" ReadOnly="True" Width="98%" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                    <td colspan="2" style="text-align:left;width:50%;">
                                        <asp:TextBox ID="Txt_Empleado_ID" runat="server" ReadOnly="True" Visible="false"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">Nombre</td>
                                    <td colspan="3" style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Nombre_Empleado" runat="server" ReadOnly="True" Width="99%" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:UpdatePanel ID="Upnl_Datos_Caja" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="Pnl_Datos_Caja" runat="server" GroupingText="Datos de los Folios" Width="97%" BackColor="White">
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                                <tr>
                                    <td style="text-align:left;width:20%;">Número</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_No_Folio" runat="server" Width="99%" ReadOnly="true" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                    <td style="text-align:left;width:20%;">Ultimo Folio Utilizado</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Ultimo_Folio_Utilizado" runat="server" Width="99%" ReadOnly="true" BorderStyle="Solid" BorderWidth="1"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">*Folio Inicial</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Folio_Inicial" runat="server" MaxLength="10" TabIndex="7" Width="99%"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Folio_Inicial" runat="server" TargetControlID="Txt_Folio_Inicial"
                                            FilterType="Custom, Numbers"/>
                                    </td>
                                    <td style="text-align:left;width:20%;">&nbsp;&nbsp;*Folio Final</td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Folio_Final" runat="server" MaxLength="10" TabIndex="8" Width="99%"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Folio_Final" runat="server" TargetControlID="Txt_Folio_Final"
                                            FilterType="Custom, Numbers"/>
                                    </td>
                                </tr>
                            </table>
                         </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2">
                            <asp:GridView ID="Grid_Empleados_Folios" runat="server" AllowPaging="True" Width="100%"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                PageSize="5" AllowSorting="True" HeaderStyle-CssClass="tblHead" 
                                onpageindexchanging="Grid_Empleados_Folios_PageIndexChanging" 
                                onselectedindexchanged="Grid_Empleados_Folios_SelectedIndexChanged" 
                                onsorting="Grid_Empleados_Folios_Sorting">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="No_Folio" HeaderText="No Folio" 
                                        Visible="True" SortExpression="No_Folio">
                                        <HeaderStyle HorizontalAlign="Left" Width="22%" />
                                        <ItemStyle HorizontalAlign="Left" Width="22%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Folio_Inicial" HeaderText="Folio Inicial" Visible="True" SortExpression="Folio_Inicial">
                                        <HeaderStyle HorizontalAlign="Left" Width="22%" />
                                        <ItemStyle HorizontalAlign="Left" Width="22%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Folio_Final" HeaderText="Folio Final" Visible="True" SortExpression="Folio_Final">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" Width="22%" />
                                        <ItemStyle HorizontalAlign="Left" Width="22%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Ultimo_Folio_Utilizado" HeaderText="Ultimo Folio Utilizado" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="22%" />
                                        <ItemStyle HorizontalAlign="Left" Width="22%" />
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


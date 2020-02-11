<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Com_Listado_Ordenes_Compra.aspx.cs"
    Inherits="paginas_Compras_Frm_Ope_Com_Listado_Ordenes_Compra" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="True" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
        <ContentTemplate>
            
            <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
              <!--      <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../Imagenes/paginas/Updating.gif" />
                    </div>-->
                </ProgressTemplate>
            </asp:UpdateProgress>
            <!--Div de Contenido -->
            <div id="Div_Contenido" style="width: 97%; height: 100%;">
                <table width="97%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td class="label_titulo">
                            Imprimir Orden de Compra
                        </td>
                    </tr>
                    <tr>
                        <!--Bloque del mensaje de error-->
                        <td>
                            <div id="Div_Contenedor_Msj_Error" style="width: 95%; font-size: 9px;" runat="server"
                                visible="false">
                                <table style="width: 100%;">
                                    <tr>
                                        <td align="left" style="font-size: 12px; color: Red; font-family: Tahoma; text-align: left;">
                                            <asp:Image ID="Img_Warning" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
                                                Width="24px" Height="24px" />
                                        </td>
                                        <td style="font-size: 9px; width: 90%; text-align: left;" valign="top">
                                            <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="Red" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <!--Bloque de la busqueda-->
                    <tr class="barra_busqueda">
                        <td style="width: 90%;">
                            <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                OnClick="Btn_Salir_Click" />
                        </td>
                        <td>
                            <asp:LinkButton ID="Lnk_Reimprimir" runat="server" Text="Reimprimir" 
                                ForeColor="White" onclick="Lnk_Reimprimir_Click"></asp:LinkButton>
                        </td>
                    </tr>
                </table>

                <table width="100%">
                    <tr id="Tr_Reimprimir" runat="server" visible="false">
                        <td style="width: 15%;">
                            Fecha
                        </td>
                        <td style="width: 35%;">
                            <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="85px" Enabled="false"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Inicial_FilteredTextBoxExtender" runat="server" TargetControlID="Txt_Fecha_Inicial" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                ValidChars="/_" />
                            <cc1:CalendarExtender ID="Txt_Fecha_Inicial_CalendarExtender" runat="server" TargetControlID="Txt_Fecha_Inicial" PopupButtonID="Btn_Fecha_Inicial" Format="dd/MMM/yyyy" />
                            <asp:ImageButton ID="Btn_Fecha_Inicial" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                            :&nbsp;&nbsp;
                            <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="85px" Enabled="false"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="Txt_Fecha_Final" PopupButtonID="Btn_Fecha_Final" Format="dd/MMM/yyyy" />
                            <asp:ImageButton ID="Btn_Fecha_Final" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Final" />
                        </td>
                        <td align="right" visible="false">
                            Folio
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="85%" MaxLength="13"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Click" ToolTip="Consultar"/>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="Txt_Busqueda" WatermarkText="&lt;OC-0&gt;" WatermarkCssClass="watermarked">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTBE_Txt_Busqueda" runat="server" FilterType="Custom" 
                                TargetControlID="Txt_Busqueda" ValidChars="OoCc-0123456789">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>                
                    <tr>
                        <td style="width: 10%;">
                            Cotizador(a)
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="Cmb_Cotizadores" runat="server" Width="100%" OnSelectedIndexChanged="Cmb_Cotizadores_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>                    
                    <tr>
                        <td colspan="4">
                            <div style="overflow: auto; height: 320px; width: 99%; vertical-align: top; border-style: outset;
                                border-color: Silver;" id="Div_Ordenes_Compra" runat="server">
                                <asp:GridView ID="Grid_Ordenes_Compra" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                    CssClass="GridView_1" GridLines="None" Width="100%" OnPageIndexChanging="Grid_Ordenes_Compra_PageIndexChanging"
                                    EmptyDataText="No se encontraron datos" OnRowDataBound="Grid_Ordenes_Compra_RowDataBound">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                    
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Info" runat="server" ToolTip="ImprimirX"
                                                    CommandArgument='<%# Eval("NO_ORDEN_COMPRA") %>' CommandName="Mostrar_Info"
                                                    ImageUrl="~/paginas/imagenes/gridview/circle_black.png"  OnClick="Btn_Info_Click"/>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="35px" />
                                        </asp:TemplateField>                                      
                                    
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Imprimir_Orden_Compra" runat="server" ToolTip="Imprimir"
                                                    CommandArgument='<%# Eval("NO_ORDEN_COMPRA") %>' CommandName="Imprimir_Orden_Compra"
                                                    ImageUrl="~/paginas/imagenes/gridview/grid_print.png" OnClick="Btn_Imprimir_Orden_Compra_Click" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="35px" />
                                        </asp:TemplateField>
                                        
                                     
                                        
                                        <asp:BoundField DataField="FOLIO" HeaderText="Folio">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NO_REQUISICION" HeaderText="RQ">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="9%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA" DataFormatString="{0:dd/MMM/yyyy}" HeaderStyle-HorizontalAlign="Left"
                                            HeaderText="Fecha" ItemStyle-HorizontalAlign="Center" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VIGENCIA" DataFormatString="{0:dd/MMM/yyyy}" HeaderText="Vigencia">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TOTAL_COTIZADO" HeaderText="Monto Total" DataFormatString="{0:C}">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PROVEEDOR" HeaderText="Proveedor">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NO_ORDEN_COMPRA" HeaderText="No_Orden_Compra" Visible="False">
                                            <HeaderStyle HorizontalAlign="Left" />
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
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

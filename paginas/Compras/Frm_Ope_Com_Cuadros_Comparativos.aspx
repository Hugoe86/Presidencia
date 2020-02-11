<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Com_Cuadros_Comparativos.aspx.cs" Inherits="paginas_Almacen_Frm_Ope_Com_Filtrar_Requisiciones_Transitorias"
    Title="Filtrado de Requisiciones Transitorias" Culture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true">
    </asp:ScriptManager>
    <div id="Div_General" style="width: 98%;" visible="true" runat="server">
        <%--<asp:UpdatePanel ID="Upl_Contenedor" runat="server">--%>
            <ContentTemplate>
<%--                <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
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
                               Cuadro Comparativo
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
                                    ToolTip="Inicio" OnClick="Btn_Salir_Click" />
                            </td>
                            <td colspan="2">
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="Div_Principal" runat="server">
                <%--Div listado de requisiciones--%>
                <div id="Div_Filtros_Busqueda" runat="server">                
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 15%;">
                                Cotizador(a)
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="Cmb_Cotizadores" runat="server" Width="100%" OnSelectedIndexChanged="Cmb_Cotizadores_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>  
                    </table>
                </div>
                <div id="Div1" runat="server">                
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 15%;">
                                No. Requisición
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Busqueda" runat="server" Width="15%" MaxLength="12"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Click" ToolTip="Consultar"/>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="Txt_Busqueda" WatermarkText="&lt;0&gt;" WatermarkCssClass="watermarked">
                                </cc1:TextBoxWatermarkExtender>
                                <cc1:FilteredTextBoxExtender ID="FTBE_Txt_Busqueda" runat="server" FilterType="Custom" 
                                    TargetControlID="Txt_Busqueda" ValidChars="0123456789">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>  
                        <tr>
                            <td style="width: 100%;" colspan="4" >
                                <b>
                                    Nota: Se muestran en el tabla las requisiciones con estatus: PROVEEDOR, COTIZADA, CONFIRMADA y COMPRA
                                </b>    
                            </td>
                        </tr>                          
                    </table>
                </div>                
                <div id="Div_Listado_Requisiciones" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 99%" align="center" colspan="4">
                                <asp:GridView ID="Grid_Requisiciones" runat="server" AutoGenerateColumns="False"
                                    CssClass="GridView_1" GridLines="None" Width="100%" AllowPaging="false" DataKeyNames="No_Requisicion"
                                    AllowSorting="true" HeaderStyle-CssClass="tblHead" OnSorting="Grid_Requisiciones_Sorting"
                                    EmptyDataText="No se encontraron requisiciones">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Seleccionar" runat="server" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png"
                                                    OnClick="Btn_Seleccionar_Click" 
                                                    ToolTip = "Ver detalle"
                                                    CommandArgument='<%# Eval("No_Requisicion") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:TemplateField>                                    
                                        <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="True" SortExpression="No_Requisicion">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Fecha_Creo" HeaderText="Fecha Inicial" DataFormatString="{0:dd/MMM/yyyy}"
                                            Visible="True" SortExpression="Fecha_Creo">
                                            <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_DEPENDENCIA" HeaderText="Unidad Responsable" Visible="True"
                                            SortExpression="NOMBRE_DEPENDENCIA">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" Visible="True" SortExpression="Tipo">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="No_Requisicion" HeaderText="ID" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ALERTA" HeaderText="Alerta" Visible="false" SortExpression="Estatus">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" />
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
                </div>
                <div id="Div_Secundario" runat="server">
                    <table style="width:100%;">
                        <tr>
                            <td style="width: 100%;" colspan="2">
                                <asp:Label ID="Lbl_Requisicion_Dividida" runat="server" ForeColor = "Blue" Visible = "false"
                                    Text="Requisición dividiva, se muestran datos de requisición origen">
                                </asp:Label>
                            </td>
                        </tr>                                                                                                                              
                        <tr style="width:100%;">
                            <td style="width:15%;">
                                Requisición
                            </td>
                            <td style="width:85%;">
                                <asp:TextBox ID="Txt_No_Requisicion" runat="server" Width="20%" Enabled="false"></asp:TextBox>
                            </td>                            
                        </tr>
                        <tr style="width:100%;">
                            <td style="width:15%;">
                                Cuadro comparativo
                            </td>
                            <td style="width:85%;">
                                <asp:ImageButton ID="Btn_Ver_Cuadro_Comparativo" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/icono_xls.png"
                                    OnClick="Btn_Mostrar_Cuadro_Economico_Click" >
                                </asp:ImageButton>
                            </td>                          
                        </tr>   
                        <tr style="width:100%;">
                            <td style="width:50%; height:10px;" colspan="2" >
                                
                            </td>
                                      
                        </tr>                                             
                        <tr style="width:100%;">
                            <td style="width:100%;" colspan="2">
                               <asp:Panel ID="Pnl_Datos_Generales" runat="server" GroupingText="Cotizaciones de proveedores" Width="100%">
                                <asp:GridView ID="Grid_Proveedores" runat="server" AutoGenerateColumns="False"
                                    CssClass="GridView_1" GridLines="None" Width="100%" AllowPaging="false" 
                                    AllowSorting="true" HeaderStyle-CssClass="tblHead" 
                                    EmptyDataText="No se encontraron proveedores"> 
                                    <RowStyle CssClass="GridItem" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />                                    
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Imprimir_Cotizacion" runat="server" 
                                                    ImageUrl="~/paginas/imagenes/gridview/grid_print.png"
                                                    OnClick="Btn_Imprimir_Cotizacion_Click" 
                                                    ToolTip = "Imprimir"
                                                    CommandArgument='<%# Eval("Proveedor_ID") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:TemplateField>                                    
         
                                        <asp:BoundField DataField="PROVEEDOR_ID" HeaderText="No. Padrón" 
                                            Visible="True" SortExpression="Fecha_Creo">
                                            <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Razón Social" 
                                            Visible="True" >
                                            <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Left" Width="41%" Font-Size="X-Small"/>
                                        </asp:BoundField>   
                                        <asp:BoundField DataField="COMPANIA" HeaderText="Nombre Comercial" 
                                            Visible="True" >
                                            <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Left" Width="42%" Font-Size="X-Small"/>
                                        </asp:BoundField>                                                                             
                                   </Columns>                                                                   
                                </asp:GridView>
                              </asp:Panel>  
                            </td>
                        </tr>                           
                    </table>
                </div>
            </ContentTemplate>
        <!--</asp:UpdatePanel> -->
    </div>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Ope_Com_Comite_Compras_Proveedores.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Comite_Compras_Proveedores" %>

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
        <div id="Div_Contenido_Compras_Proveedor" style="width:97%;height:100%;">
            <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
                <tr>
                    <td colspan ="4" class="label_titulo">Asignar Proveedores Comite de Compras</td>
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
                            <asp:ImageButton ID="Btn_Modificar" runat="server" 
                            ToolTip="Modificar" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                            onclick="Btn_Modificar_Click"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click"/>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td align="right" colspan="3" style="width:80%;">
                        <div id="Div_Busqueda" runat="server">
                        <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                        ToolTip="Avanzada">Busqueda</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese un Folio>"
                                TargetControlID="Txt_Busqueda" />
                        <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Consultar"
                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Click"/>
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
                 <tr>
                    <td colspan="4">
                        <div id="Div_Listado_Comite" runat="server" style="width:100%;">
                        <asp:GridView ID="Grid_Comite_Compras" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" CssClass="GridView_1" DataKeyNames="No_Comite_Compras" 
                            OnSelectedIndexChanged="Grid_Comite_Compras_SelectedIndexChanged"
                            OnPageIndexChanging="Grid_Comite_Compras_PageIndexChanging" PageSize="10"
                            GridLines="None">
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
                                <asp:BoundField DataField="Total_Cotizado" HeaderText="Total Cotizado" 
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
                <tr>
                    <td colspan ="4">
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
                                        <asp:TextBox ID="Txt_Folio" runat="server" Enabled="false" Width="97%"></asp:TextBox>
                                </td>
                                <td style="width:15%;">
                                        Fecha
                                </td>
                                <td style="width:35%;">
                                        <asp:TextBox ID="Txt_Fecha" runat="server" Enabled="false" Width="97%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tipo
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Tipo" runat="server" Enabled="false" Width="97%"></asp:TextBox>
                                </td>
                                <td style="width:15%;">
                                    Estatus
                                </td>
                                <td style="width:35%;">
                                    <asp:DropDownList ID="Cmb_Estatus" runat="server" Enabled="false" Width="99%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Justificacion
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Justificacion" runat="server" Enabled="false" 
                                        TabIndex="10" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" 
                                        TargetControlID="Txt_Justificacion" WatermarkCssClass="watermarked" 
                                        WatermarkText="&lt;Indica el motivo de realizar la requisición&gt;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Comentarios
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Comentario" runat="server" Enabled="false" MaxLength="250" 
                                        TabIndex="10" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                                        TargetControlID="Txt_Comentario" WatermarkCssClass="watermarked" 
                                        WatermarkText="&lt;Límite de Caracteres 250&gt;" />
                                </td>
                            </tr>
                            <tr>
                                <td class="barra_delgada" colspan="4">
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    Manejo de Requisiciones
                                </td>
                            </tr>
                         </tabla>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div id="Div_Tab_Requisiciones" runat="server">
                        <cc1:TabContainer ID="Tab_Requisiciones_Seleccionadas" runat="server" 
                            ActiveTabIndex="1" Width="98%">  
                             <cc1:TabPanel ID="TabPnl_Requisiciones_Consolidaciones" runat="server" Visible="true" Width="99%">
                             <HeaderTemplate>Requisiciones/Consolidaciones</HeaderTemplate>
                             <ContentTemplate>
                             <table style="width:99%">
                                <tr>
                                    <td>
                                        <div id="Div_Requisiciones" runat="server" visible="false">
                                            <center>Requisiciones</center>
                                            <div id="Div_Grid_Requisiciones" runat="server"
                                            style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">
                                            <asp:GridView ID="Grid_Requisiciones" runat="server" 
                                            AutoGenerateColumns="False" CssClass="GridView_1" DataKeyNames="No_Requisicion" 
                                            GridLines="None" Width="99%">
                                                <Columns>
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
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                </asp:GridView>
                                                </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="Div_Consolidaciones"  runat="server" visible="false">
                                            <center>Consolidaciones</center>
                                            <div id="Div_Grid_Consolidaciones" runat="server"
                                            style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">
                                            <asp:GridView ID="Grid_Consolidaciones" runat="server" 
                                                    AutoGenerateColumns="False" CssClass="GridView_1"
                                                    GridLines="None" Width="99%">
                                                    <Columns>                       
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
                                                    <asp:BoundField DataField="Monto" HeaderText="Total">
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
                                                    </Columns><AlternatingRowStyle CssClass="GridAltItem" />
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    </asp:GridView>
                                                    </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Total&nbsp;
                                        <asp:TextBox ID="Txt_Total" runat="server" style="text-align:right" Width="200px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                             </table>
                             </ContentTemplate>
                             </cc1:TabPanel>
                             <cc1:TabPanel ID ="TabPnl_Productos" runat="server" Width="99%">
                             <HeaderTemplate>Asignar Proveedores</HeaderTemplate>
                             <ContentTemplate>
                             <table width="99%">
                                <tr>
                                    <td style="width:15%">
                                        Concepto
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Cmb_Concepto"  runat="server" Width="250px"  Enabled="False"
                                            onselectedindexchanged="Cmb_Concepto_SelectedIndexChanged" 
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:Button ID="Btn_Add_Proveedor" runat="server" text="Agregar Proveedor" 
                                            CssClass="button" Width="200px" onclick="Btn_Add_Proveedor_Click"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Proveedor    
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Cmb_Proveedores" runat="server" Width="450px" Enabled="False">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                    <div id="Div_Grid_Proveedores" runat="server" visible="False"> 
                                        <center>Proveedores Asignados</center>
                                            <div id="Div1" runat="server"
                                            style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">                             
                                            <asp:GridView ID="Grid_Concepto_Proveedores" runat="server" 
                                            AutoGenerateColumns="False" CssClass="GridView_1" DataKeyNames="Concepto_ID"
                                            GridLines="None" Width="99%" OnSelectedIndexChanged="Grid_Concepto_Proveedores_SelectedIndexChanged">
                                            <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                ImageUrl="~/paginas/imagenes/paginas/delete.png">
                                                <ItemStyle Width="5%" />
                                            </asp:ButtonField>                 
                                            <asp:BoundField DataField="Concepto_ID" HeaderText="Concepto_ID" Visible="False">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Clave" HeaderText="Clave">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Descripcion_Concepto" HeaderText="Descripcion Concepto">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Nombre_Proveedor" HeaderText="Proveedor">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Proveedor_ID" HeaderText="Proveedor_ID" Visible="False">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            </Columns><AlternatingRowStyle CssClass="GridAltItem" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            </asp:GridView>
                                            </div>
                                            </div>
                                    </td>
                                </tr>
                             </table>
                             </ContentTemplate>
                             </cc1:TabPanel>
                             <cc1:TabPanel ID ="TabPnl_Productos_Proveedor" runat="server" Width="99%">
                             <HeaderTemplate>Asignar Precios Productos</HeaderTemplate>
                             <ContentTemplate>
                             <table width="99%">
                                <tr>
                                    <td style="width:15%">Nombre Producto</td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Nombre_Producto" runat="server" Width="98%" 
                                            Enabled="False" ></asp:TextBox>
                                    </td>
                                    <td style="width:15%">
                                        Costo Unitario
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Costo_Producto" runat="server" Width="70%"></asp:TextBox>
                                        <asp:ImageButton ID="Btn_Agregar" runat="server" 
                                            ImageUrl="~/paginas/imagenes/paginas/accept.png" Width="24px" 
                                            onclick="Btn_Agregar_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Nombre Proveedor
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="Txt_Nombre_Proveedor" runat="server" Width="97%" 
                                            Enabled="False"></asp:TextBox>
                                    </td>
                                    <td>
                                    <cc1:MaskedEditExtender ID="MEE_Txt_Costo_Producto" runat="server" 
                                            TargetControlID="Txt_Costo_Producto"                                          
                                            Mask="9,999,999.99"  
                                            MaskType="Number"    
                                            InputDirection="RightToLeft"    
                                            AcceptNegative="Left"    
                                            DisplayMoney="Left"  
                                            ErrorTooltipEnabled="True"
                                            AutoCompleteValue="0"
                                            ClearTextOnInvalid ="True" CultureAMPMPlaceholder="" 
                                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True"/> 
                                        <cc1:MaskedEditValidator
                                            ID="MEV_Txt_Costo_Producto" 
                                            runat="server"
                                            ControlExtender="MEE_Txt_Costo_Producto"
                                            ControlToValidate="Txt_Costo_Producto" 
                                            MaximumValue="1000000" 
                                            EmptyValueMessage="El precio del producto no puede ser $0.00 Pestaña 2/2"
                                            InvalidValueMessage="Formato del salario diario es inválido. Pestaña 2/2"
                                            MaximumValueMessage="Cantidad > $1,000,000.00"
                                            MinimumValueMessage="Cantidad < $0.00"
                                            MinimumValue="0" 
                                            EmptyValueBlurredText="Cantidad Requerida" 
                                            InvalidValueBlurredMessage="Formato Incorrecto" 
                                            MaximumValueBlurredMessage="Cantidad > $1,000,000.00" 
                                            MinimumValueBlurredText="Cantidad < $0.00"
                                            Display="Dynamic" 
                                            TooltipMessage="Monto entre $0.00 y $1,000,000.00"
                                            style="font-size:9px;" ErrorMessage="MEV_Txt_Costo_Producto"/> 
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <div  id="Div_Grid_Productos" runat="server" visible="False"
                                        style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">
                                        <asp:GridView ID="Grid_Productos" runat="server" Width="100%"
                                        AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                        DataKeyNames="Ope_Com_Req_Producto_ID" 
                                                onselectedindexchanged="Grid_Productos_SelectedIndexChanged">
                                        <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%"/>
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="Ope_Com_Req_Producto_ID" HeaderText="Producto_Req_ID" 
                                        Visible="False">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Prod_Serv_ID" HeaderText="No_Prod_Serv" 
                                        Visible="False">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Nombre_Prod_Serv" HeaderText="Producto/Servicio">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Precio_U_Sin_Imp_Cotizado" HeaderText="Costo S/I">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Precio_U_Con_Imp_Cotizado" HeaderText="Costo C/I">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IVA_Cotizado" HeaderText="Monto_IVA" Visible="False">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IEPS_Cotizado" HeaderText="Monto_IVA" Visible="False">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>                                                        
                                        <asp:BoundField DataField="Concepto_ID" HeaderText="Concepto_ID" Visible="False">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Total_Cotizado" HeaderText="Importe Total">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Subtotal_Cotizado" HeaderText="SubTotal" Visible="False">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Proveedor_ID" HeaderText="Proveedor_ID" Visible="False">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Nombre_Proveedor" HeaderText="Proveedor">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
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
                                <tr>
                                    <td colspan="4" align="right">
                                        Total Cotizado
                                        <asp:TextBox ID="Txt_Total_Cotizado" runat="server" style="text-align:right" 
                                            Width="200px" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                
                             </table>
                             </ContentTemplate>
                             </cc1:TabPanel>
                        </cc1:TabContainer> 
                        </div> 
                    </td>
                </tr>
                </table>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
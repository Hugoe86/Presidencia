<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Ope_Com_Licitacion_Proveedores.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Licitacion_Proveedores" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript" language="javascript">
        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
    </script> 
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
           <%--Table de Contenido --%>
           <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                <tr>
                    <td colspan ="4" class="label_titulo">Asignar Proveedores a Licitaciones</td>
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
                <tr class="barra_busqueda">
                    <td style="width:20%;">
                       <asp:ImageButton ID="Btn_Modificar" runat="server" 
                            ToolTip="Modificar" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                            onclick="Btn_Modificar_Click"/>
                       <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                            CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                            onclick="Btn_Salir_Click"/>
                    </td>
                    <td align="right" colspan="3" style="width:80%;">
                       <div id="Div_Busqueda" runat="server">
                        <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                            ToolTip="Avanzada" onclick="Btn_Busqueda_Avanzada_Click">Busqueda</asp:LinkButton>
                            &nbsp;&nbsp;
                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                    WatermarkCssClass="watermarked"
                                    WatermarkText="<Ingrese un Folio>"
                                    TargetControlID="Txt_Busqueda" />
                         <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Consultar"
                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                               onclick="Btn_Buscar_Click"/>
                         </div>
                         <asp:Button ID="Btn_Comodin_2" runat="server" Text="Button" style="display:none;"/>
                        <cc1:ModalPopupExtender ID="Modal_Busqueda" runat="server"
                        TargetControlID="Btn_Comodin_2"
                        PopupControlID="Pnl_Busqueda"                      
                        CancelControlID="Btn_Cancelar"
                        DropShadow="True"
                        BackgroundCssClass="progressBackgroundFilter"/>
                         
                    </td>
                </tr>
                <%-- Div que contiene el listado de licitaciones --%>
                <tr>
                    <td colspan="4">
                        <div id="Div_Licitaciones" runat="server" style="width:100%;">
                            <asp:GridView ID="Grid_Licitaciones" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" CssClass="GridView_1" DataKeyNames="No_Licitacion" 
                                GridLines="None" 
                                onselectedindexchanged="Grid_Licitaciones_SelectedIndexChanged"
                                OnPageIndexChanging = "Grid_Licitaciones_PageIndexChanging">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="No_Licitacion" HeaderText="No_Licitacion" 
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
                                    <asp:BoundField DataField="Clasificacion" HeaderText="Clasificacion" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha_Inicio" HeaderText="Fecha de Inicio" 
                                        Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha_Fin" HeaderText="Fecha de Termino" 
                                        Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
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
                <%--Div Contenido General--%>
                <tr>
                    <td colspan="4">
                        <div id="Div_Datos_Licitacion" runat="server">
                            <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                                 <tr>
                                    <td align="center">Datos Generales
                                    </td>
                                 </tr>
                                 <tr>
                                    <td style="width:15%;">
                                        Folio
                                    </td>
                                    <td style="width:35%;">
                                        <asp:TextBox ID="Txt_Folio" runat="server" Enabled="false" Width="98%"></asp:TextBox>
                                    </td>
                                    <td style="width:15%;">
                                        Fecha Inicio
                                    </td>
                                    <td style="width:35%;">
                                        <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td>
                                        Estatus</td>
                                    <td>
                                        <asp:DropDownList ID="Cmb_Estatus" runat="server" AutoPostBack="true" Width="99%">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Fecha Fin
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td>
                                        Tipo  
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Tipo" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        Clasificacion
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Clasificacion" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td>
                                        Justificacion
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Justificacion" runat="server" TabIndex="10"        
                                        TextMode="MultiLine" Width="99%" Enabled="false"></asp:TextBox>
                                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" WatermarkCssClass="watermarked"
                                        WatermarkText="<Justificacion>" 
                                        TargetControlID="Txt_Justificacion" />
                                    </td>                    
                                 </tr>
                                 <tr>
                                    <td>
                                        Comentarios
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Comentario" runat="server" TabIndex="10" MaxLength="250"
                                        TextMode="MultiLine" Width="99%" Enabled="false"></asp:TextBox>
                                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkCssClass="watermarked"
                                        WatermarkText="<Comentarios>" 
                                        TargetControlID="Txt_Comentario" />
                                    </td>                            
                                 </tr>
                                 <tr>
                                    <td colspan="4" class="barra_delgada">
                                    </td>
                                 </tr>
                                 <tr>
                                    <td colspan="4">
                                    <cc1:TabContainer ID="Tab_Container_Licitacion" runat="server" Width="99%" 
                                            ActiveTabIndex="1">
                                    <cc1:TabPanel ID = "Tab_General" runat = "server" Visible="true">
                                    <HeaderTemplate>Requisiciones</HeaderTemplate>
                                    <ContentTemplate>
                                        <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                                            <tr>
                                                <td colspan="4">Listado de Requisiciones pertenecientes a la requisicion</td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:GridView ID="Grid_Requisiciones" runat="server" AllowPaging="True" 
                                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" DataKeyNames="No_Requisicion"
                                                    PageSize="5" OnPageIndexChanging="Grid_Requisiciones_PageIndexChanging" 
                                                    Width="100%">
                                                    <Columns>
                                                    <asp:BoundField DataField="No_Requisicion" HeaderText="No_Requisicion" 
                                                            Visible="false">
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="true">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" Visible="true">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Dependencia" HeaderText="Dependencia" Visible="true">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Area" HeaderText="Area" Visible="true">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Total" HeaderText="Total" Visible="true">
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="No_Consolidacion" HeaderText="Consolidacion" 
                                                            Visible="false">
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <ItemStyle HorizontalAlign="Right" />
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
                                                <td colspan ="4" align="right">
                                                    Total&nbsp; 
                                                    <asp:TextBox ID="Txt_Total" runat="server" Enabled="False" Width="200px" 
                                                    style="text-align:right"></asp:TextBox>
                                                </td>
                                            </tr>
                                            
                                        </table>
                                        </ContentTemplate>
                                    </cc1:TabPanel>    
                            <cc1:TabPanel ID = "Tab_Productos" runat = "server">
                            <HeaderTemplate>Productos</HeaderTemplate>
                            <ContentTemplate>
                                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                                    <tr>
                                        <td style="width:15%;">Nombre Producto
                                        </td>
                                        <td style="width:35%;">
                                            <asp:TextBox ID="Txt_Nombre_Producto" runat="server" Width="98%" 
                                                Enabled="False"></asp:TextBox>
                                        </td>
                                        <td style="width:15%;">Costo
                                        </td>
                                        <td style="width:35%;">
                                            <asp:TextBox ID="Txt_Costo_Producto" runat="server" Width="98%"></asp:TextBox>
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
                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True"
                                            /> 
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
                                        <td>
                                            Proveedor
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="Cmb_Proveedor" runat="server" Width="95%">
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="Btn_Agregar" runat="server" 
                                            ImageUrl="~/paginas/imagenes/paginas/accept.png" OnClick="Btn_Agregar_Click"
                                            Width="24px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="Grid_Productos" runat="server" AllowPaging="True" 
                                                        AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                                        DataKeyNames="Ope_Com_Req_Producto_ID" 
                                                        onselectedindexchanged="Grid_Productos_SelectedIndexChanged" 
                                                        OnPageIndexChanging = "Grid_Productos_PageIndexChanging"                                                       
                                                        PageSize="5" Width="100%">
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
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan ="4" align="right">
                                            Total&nbsp; 
                                            <asp:TextBox ID="Txt_Total_Cotizado" runat="server" Enabled="False" Width="200px" 
                                            style="text-align:right"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                              </ContentTemplate>
                            </cc1:TabPanel>    
                        </cc1:TabContainer>
                        </td>
                        </tr>
                        </table>
                        </div>
                    </td>
                </tr>
                </table>
                <%--Fin del Table Contenido--%>           
        </ContentTemplate>
        
    </asp:UpdatePanel>  
    
    <%-- Panel del ModalPopUp Busqueda Avanzada--%>
    <asp:Panel ID="Pnl_Busqueda" runat="server" Width="60%" 
        style="border-style:outset;border-color:Silver;background-repeat:repeat-y;background-color:White;color:White;">
        <center>
        <asp:UpdatePanel ID="pnlPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <table class="estilo_fuente" width="100%">
              <tr>
                    <td colspan="4">
                        <asp:Label ID="Lbl_Error_Busqueda" runat="server" ForeColor="Red" Width="100%" />
                    </td>
              </tr>
              <tr>
                    <td colspan="4" class="barra_busqueda" align="center"> Busqueda Avanzada</td>
              </tr>
              <tr>
                    <td colspan="4"></td>
              </tr>
              <tr>
                    <td align="left" style="width:20%;">
                        Fecha&nbsp;De</td>
                    <td align="left">
                        <asp:TextBox ID="Txt_Fecha_Avanzada_1" runat="server" Width="150px" 
                        Enabled="true"></asp:TextBox>
                        <asp:ImageButton ID="Btn_Fecha_Avanzada_1" runat="server" 
                            ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                            ToolTip="Seleccione la Fecha" />
                        <cc1:CalendarExtender ID="Txt_Fecha_Avanzada_1_CalendarExtender" runat="server" OnClientShown="calendarShown"
                        TargetControlID="Txt_Fecha_Avanzada_1" Format ="dd/MMM/yyyy" PopupButtonID="Btn_Fecha_Avanzada_1">
                        </cc1:CalendarExtender>
                    </td>
                    <td align="left">
                        Al</td>
                    <td align="left">
                        <asp:TextBox ID="Txt_Fecha_Avanzada_2" runat="server" Width="150px" Enabled="true"></asp:TextBox>
                        <asp:ImageButton ID="Btn_Fecha_Avanzada_2" runat="server" 
                            ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                            ToolTip="Seleccione la Fecha" />
                        <cc1:CalendarExtender ID="Txt_Fecha_Avanzada_2_CalendarExtender" runat="server" OnClientShown="calendarShown"
                        TargetControlID="Txt_Fecha_Avanzada_2" Format ="dd/MMM/yyyy" PopupButtonID="Btn_Fecha_Avanzada_2">
                        </cc1:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Estatus</td>
                    <td align="left" colspan="3">
                        <asp:DropDownList ID="Cmb_Estatus_Busqueda" runat="server" Width="355px" 
                        Enabled="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                    <center>
                        <asp:Button ID="Btn_Aceptar" runat="server" Text="Aceptar" Width="100px" 
                        onclick="Btn_Aceptar_Click" CssClass="button"/>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Btn_Cancelar" runat="server" Text="Cancelar" Width="100px" 
                        CssClass="button"/>
                    </center>
                    </td>
                </tr>
                </table>
                </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Btn_Busqueda_Avanzada" EventName="Click"/>
          </Triggers>    
        </asp:UpdatePanel> 
        </center>
        </asp:Panel>       
</asp:Content>

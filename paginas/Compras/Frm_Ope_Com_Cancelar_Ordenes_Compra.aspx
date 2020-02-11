<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Ope_Com_Cancelar_Ordenes_Compra.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Cancelar_Ordenes_Compra" %>

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
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
           <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                 <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                 <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
           </asp:UpdateProgress>
           
           <div id="Div_Contenido" style="width:97%;height:100%;">
           <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
            <tr>
                <td colspan ="4" class="label_titulo">Cancelar Ordenes de Compra</td>
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
             <%--Fila 3 Renglon de barra de Busqueda--%>
            <tr class="barra_busqueda" >
                <td style="width:20%;" colspan ="4">
                    
                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                        CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" 
                        onclick="Btn_Modificar_Click"/>
                    <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" 
                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" 
                        onclick="Btn_Salir_Click"/>
                </td>
             
            </tr>
            <tr>
                    <td colspan="4">
                        <div ID="Div_Busquedas_Avanzadas" runat="server" visible="true">
                            <table border="0" cellspacing="0" style="width: 100%;">
                                <tr>
                                    <td width="50%">
                                        &nbsp;&nbsp;De
                                        <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Enabled="false" Width="30%"></asp:TextBox>
                                         <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server" 
                                            ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                            ToolTip="Seleccione la Fecha Inicial"/>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <cc1:CalendarExtender ID="CalendarExtender" runat="server" 
                                            Format="dd/MMM/yyyy" PopupButtonID="Btn_Fecha_Inicio" 
                                            TargetControlID="Txt_Fecha_Inicio" />
                                       
                                   
                                        &nbsp;&nbsp;Al
                                        <asp:TextBox ID="Txt_Fecha_Final" runat="server" Enabled="false" Width="30%"></asp:TextBox>
                                        <asp:ImageButton ID="Btn_Fecha_Final" runat="server" 
                                            ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                            ToolTip="Seleccione la Fecha Final" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                                            Format="dd/MMM/yyyy" PopupButtonID="Btn_Fecha_Final" 
                                            TargetControlID="Txt_Fecha_Final" />
                                        
                                    </td>
                                    <td width="40%">
                                        &nbsp;&nbsp;Estatus
                                        <asp:DropDownList ID="Cmb_Estatus_Busqueda" runat="server" Width="50%">
                                            <asp:ListItem Selected="True">SELECCIONAR</asp:ListItem>
                                            <asp:ListItem Selected="False" Value="GENERADA">GENERADA</asp:ListItem>
                                            <asp:ListItem Selected="False" Value="AUTORIZADA">AUTORIZADA</asp:ListItem>
                                            <asp:ListItem Selected="False" Value="RECHAZADA">RECHAZADA</asp:ListItem>
                                            <asp:ListItem Selected="False" Value="CANCELACION PARCIAL">CANCELADA</asp:ListItem>
                                            
                                        </asp:DropDownList>
                                    </td>
                                    <td width="40%" align="right">
                                        <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Buscar"
                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                            onclick="Btn_Buscar_Click" />
                                    </td>
                                 </tr>
                                 <tr >
                                    <td width="50%" colspan ="3">
                                        &nbsp;&nbsp;Folio
                                        <asp:TextBox ID="Txt_Orden_Compra_Busqueda" runat="server" Width="20%" 
                                            AutoPostBack="true" ontextchanged="Txt_Orden_Compra_Busqueda_TextChanged"></asp:TextBox>
                                       
                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" 
                                            TargetControlID="Txt_Orden_Compra_Busqueda" WatermarkCssClass="watermarked" 
                                            WatermarkText="&lt;OC-0&gt;" />
                                        &nbsp;&nbsp;&nbsp;Req<asp:TextBox ID="Txt_Req_Busqueda" runat="server" 
                                            Width="20%" AutoPostBack="true" ontextchanged="Txt_Req_Busqueda_TextChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" 
                                        TargetControlID="Txt_Req_Busqueda"  
                                        FilterType="Custom" ValidChars="0,1,2,3,4,5,6,7,8,9"
                                        Enabled="True" InvalidChars="<,>,&,',!,">   
                                        </cc1:FilteredTextBoxExtender>
                                        
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <%--Div Grid Listado--%>
                        <div ID="Div_Grid_Ordenes_Compra" runat="server" 
                            style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">
                            <asp:GridView ID="Grid_Ordenes_Compra" runat="server" Width="100%" 
                                AutoGenerateColumns="False" DataKeyNames="NO_ORDEN_COMPRA"
                                CssClass="GridView_1" GridLines="None" AllowSorting="True" OnSorting="Grid_Ordenes_Compra_Sorting"
                                HeaderStyle-CssClass="tblHead" OnSelectedIndexChanged="Grid_Ordenes_Compra_SelectedIndexChanged">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        HeaderText="" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle HorizontalAlign="Center" Width="5%" Font-Size="X-Small"/>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="NO_ORDEN_COMPRA" HeaderText="NO_ORDEN_COMPRA" Visible="false" SortExpression="NO_ORDEN_COMPRA">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FOLIO" HeaderText="Folio" Visible="True" SortExpression="FOLIO">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LISTA_REQUISICIONES" HeaderText="Req" Visible="True" SortExpression="Estatus">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha_Creo" HeaderText="Fecha" Visible="True" SortExpression="Fecha_Creo">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Tipo_Articulo" HeaderText="Tipo" Visible="True" SortExpression="Tipo_Articulo">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Total" HeaderText="Total" Visible="True" DataFormatString="{0:C}" SortExpression="Total">
                                        <HeaderStyle HorizontalAlign="Right" Width="15%" />
                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Right" Width="15%" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div ID="Div_Contenido_Orden_Compra" runat="server">
                            <table style="width:100%;">
                                <tr>
                                    <td align="left" style="width:15%">
                                        No. Orden Compra
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="Txt_No_Orden_Compra" runat="server" Enabled="false" 
                                            Visible="true" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width:15%">
                                        No. Requisición
                                    </td>
                                    <td align="left" style="width:35%">
                                        <asp:TextBox ID="Txt_No_Requisicion" runat="server" ReadOnly="true" 
                                            Width="95%" Wrap="true"></asp:TextBox>
                                    </td>
                                    <td align="left" style="width:15%;">
                                        <asp:Label ID="Lbl_No_Reserva" runat="server" Text="No. de Reserva"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="Txt_No_Reserva" runat="server" Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width:15%">
                                        <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus"></asp:Label>
                                    </td>
                                    <td align="left" style="width:35%">
                                        <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" AutoPostBack="true"
                                            onselectedindexchanged="Cmb_Estatus_SelectedIndexChanged">
                                            <asp:ListItem Selected="True">SELECCIONAR</asp:ListItem>
                                            <asp:ListItem Selected="False" Value="CANCELACION PARCIAL">CANCELACION PARCIAL</asp:ListItem>
                                            <asp:ListItem Selected="False" Value="CANCELACION TOTAL">CANCELACION TOTAL</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Fecha Cancelación
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Fecha_Cancelacion" runat="server" Enabled="false" 
                                            Width="95%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Proveedor
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Proveedor" runat="server" Width="98%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        Código Programatico
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:TextBox ID="Txt_Codigo_Programatico" runat="server" ReadOnly="true" 
                                            Width="98%" Wrap="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        Justificación
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:TextBox ID="Txt_Justificacion" runat="server" Height="52px" 
                                            ReadOnly="true" TextMode="MultiLine" Width="98%" Wrap="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        Motivo Cancelación
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:TextBox ID="Txt_Motivo_Cancelacion" runat="server" Height="52px" 
                                            TextMode="MultiLine" Width="98%" Wrap="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4" style="width:99%">
                                     <asp:GridView ID="Grid_Detalles_Compra" runat="server" Width="100%" 
                                        AutoGenerateColumns="False"
                                        CssClass="GridView_1" GridLines="None"
                                        HeaderStyle-CssClass="tblHead">
                                           <Columns>
                                                <asp:BoundField DataField="CLAVE" HeaderText="Clave" Visible="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" Visible="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" Visible="true">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PRECIO_U_SIN_IMP_COTIZADO" HeaderText="Precio Unitario" DataFormatString="{0:C}" Visible="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SUBTOTAL_COTIZADO" HeaderText="Subtotal" DataFormatString="{0:C}" Visible="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="IVA_COTIZADO" HeaderText="IVA" DataFormatString="{0:C}" Visible="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TOTAL_COTIZADO" HeaderText="Total" DataFormatString="{0:C}" Visible="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                </asp:BoundField>
                                            </Columns>
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="right">
                                        Subtotal 
                                        <asp:TextBox ID="Txt_Subtotal" runat="server" Width="20%" Enabled="false" style="text-align:right"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                     <td colspan="4" align="right">
                                        IVA<asp:TextBox ID="Txt_Total_IVA" runat="server" Width="20%" Enabled="false" style="text-align:right"></asp:TextBox>
                                    </td>
                                
                                </tr>
                                <tr>
                                     <td colspan="4" align="right">
                                        Total<asp:TextBox ID="Txt_Total" runat="server" Width="20%" Enabled="false" style="text-align:right"></asp:TextBox>
                                    </td>
                                </tr>
                                
                            </table>
                        </div>
                    </td>
                </tr>
              
           </table><%--Fin del TAble inicial --%>
           </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
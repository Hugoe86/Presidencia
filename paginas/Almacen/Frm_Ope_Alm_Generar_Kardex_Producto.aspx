<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Alm_Generar_Kardex_Producto.aspx.cs" 
Inherits="paginas_Almacen_Frm_Ope_Alm_Generar_Kardex_Producto" Title="Generar Kardex Productos" Culture="es-MX"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Generar_Kardex_Productos" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true">
    </asp:ScriptManager>    
    <asp:UpdatePanel ID="Upd_Panel_Generar_Kardex_Productos" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Panel_Generar_Kardex_Productos" runat="server" AssociatedUpdatePanelID="Upd_Panel_Generar_Kardex_Productos" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Generales" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="2">Kardex de Productos</td>
                    </tr>
                    <tr>
                        <td>
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                    
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top">
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>                        
                    </tr>
                </table>
                </div>     
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>                                
                    <div>
                        <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                            <tr class="barra_busqueda" align="right">
                                <td align="left" style="width: 30%">
                                    <asp:ImageButton ID="Btn_Ver_Kardex" runat="server" ImageUrl="~/paginas/imagenes/paginas/Listado.png"
                                        Width="24px" CssClass="Img_Button" AlternateText="Ver Kardex Producto" ToolTip="Ver Kardex Producto"
                                        OnClick="Btn_Ver_Kardex_Click" Visible="false" />
                                    <asp:ImageButton ID="Btn_Generar_Kardex_Pdf" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png"
                                        Width="24px" CssClass="Img_Button" AlternateText="Generar Kardex Producto [Pdf]"
                                        ToolTip="Generar Kardex Producto [Pdf]" OnClick="Btn_Generar_Kardex_Pdf_Click"
                                        Visible="false" />
                                    <asp:ImageButton ID="Btn_Generar_Kardex_Excel" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png"
                                        Width="24px" CssClass="Img_Button" AlternateText="Generar Kardex Producto [Excel]"
                                        ToolTip="Generar Kardex Producto [Excel]" OnClick="Btn_Generar_Kardex_Excel_Click" />
                                    <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                        Width="24px" CssClass="Img_Button" AlternateText="Salir" ToolTip="Salir" OnClick="Btn_Salir_Click" />
                                </td>
                                <td style="width: 70%">
                                </td>
                            </tr>
                        </table>
                        <br />                                                                       
                    </div>                 
                   </ContentTemplate>
                  </asp:UpdatePanel>
                <div>                
                <center>
                    <div id="Div_Datos_Producto" style="background-color:#ffffff; width:100%; height:100%;">
                        <table width="98%" cellspacing="0">
                            <tr>
                                <td colspan="4">
                                    <asp:HiddenField ID="Hdf_Producto_ID" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Label1" runat="server" Text="Búsqueda" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td colspan="3" style="width:18%; text-align:left;">
                                    <asp:TextBox ID="Txt_Busqueda" runat="server" Width="250px"></asp:TextBox>
                                    <asp:ImageButton ID="Btn_Buscar" runat="server" CausesValidation="false" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                        ToolTip="Buscar" AlternateText="Buscar" OnClick="Btn_Buscar_Click" />
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkText="<- Clave Producto ->"
                                        TargetControlID="Txt_Busqueda" WatermarkCssClass="watermarked">
                                    </cc1:TextBoxWatermarkExtender>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda"
                                        InvalidChars="<,>,&,',!," FilterType="Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Fecha_Inicial" runat="server" Text="Fecha Inicial" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="85%" MaxLength="20" Enabled="true"></asp:TextBox>
                                    <asp:ImageButton ID="Btn_Fecha_Inicial" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                    <cc1:CalendarExtender ID="CE_Txt_Fecha_Inicial" runat="server" TargetControlID="Txt_Fecha_Inicial" PopupButtonID="Btn_Fecha_Inicial" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEE_Txt_Fecha_Inicio" Mask="99/LLL/9999" runat="server" MaskType="None" 
                                            UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Inicial" 
                                            Enabled="True" ClearMaskOnLostFocus="false"/>  
                                    <cc1:MaskedEditValidator ID="MEV_MEE_Txt_Fecha_Inicio" runat="server" ControlToValidate="Txt_Fecha_Inicial" ControlExtender="MEE_Txt_Fecha_Inicio" 
                                            EmptyValueMessage="Fecha Requerida" InvalidValueMessage="Fecha no valida" IsValidEmpty="false" 
                                            TooltipMessage="Ingresar Fecha" Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                                </td>
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Fecha_Final" runat="server" Text="Fecha Final" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:32%" style="text-align:left;">
                                    <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="85%" MaxLength="20" Enabled="true"></asp:TextBox>
                                    <asp:ImageButton ID="Btn_Fecha_Final" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                    <cc1:CalendarExtender ID="CE_Txt_Fecha_Final" runat="server" TargetControlID="Txt_Fecha_Final" PopupButtonID="Btn_Fecha_Final" Format="dd/MMM/yyyy">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MaskedEditExtender1" Mask="99/LLL/9999" runat="server" MaskType="None" 
                                            UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Final" 
                                            Enabled="True" ClearMaskOnLostFocus="false"/>  
                                    <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="Txt_Fecha_Final" ControlExtender="MEE_Txt_Fecha_Inicio" 
                                            EmptyValueMessage="Fecha Requerida" InvalidValueMessage="Fecha no valida" IsValidEmpty="false" 
                                            TooltipMessage="Ingresar Fecha" Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:left;">
                                    <asp:Label ID="Lbl_Partida" runat="server" Text="Partida" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td colspan="3" style="text-align:left;">
                                    <asp:DropDownList ID="Cmb_Partida_Especifica" runat="server" Width="99%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="background-color: #3366CC">
                                <td id="Barra_Generales" style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" runat="server" >
                                    Datos Generales
                                </td>
                            </tr>
                            <tr>
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Clave" runat="server" Text="Clave" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_Clave" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Descripcion" runat="server" Text="Descripción" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Descripcion" runat="server" Width="99%" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="display:none;">
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Modelo" runat="server" Text="Modelo" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_Modelo" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Marca" runat="server" Text="Marca" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_Marca" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Unidad" runat="server" Text="Unidad(s)" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_Unidad" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_Estatus" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color: #3366CC">
                                <td id="Td1" style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" runat="server" >
                                    Datos Númericos Totales
                                </td>
                            </tr>
                            <tr>
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Inicial" runat="server" Text="Inicial" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_Inicial" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Existencias" runat="server" Text="Existencias" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_Existencias" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>                                
                                
                            </tr>
                            
                            <tr>
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Disponible" runat="server" Text="Disponible" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_Disponible" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>

                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Total_Comprometido" runat="server" Text="Comprometido" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_Total_Comprometido" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td  colspan="4"> <hr /> </td>
                            </tr>                         
                                                       
                            <tr>
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Entradas" runat="server" Text="Entradas [O. C.]" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_Entradas" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Salidas" runat="server" Text="Salidas [O. S.]" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_Salidas" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Ajuste_Entrada" runat="server" Text="Entradas [Ajuste]" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_Ajuste_Entrada" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width:18%; text-align:left;">
                                    <asp:Label ID="Lbl_Ajuste_Salidas" runat="server" Text="Salidas [Ajuste]" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_Ajuste_Salidas" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>

                            <tr style="background-color: #3366CC">
                                <td id="Td2" style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="2" runat="server" >
                                    Detalle de Movimientos
                                </td>
                                <td id="Td4" style="text-align:right; font-size:15px; color:#FFFFFF;" runat="server" >
                                   
                                </td>                                
                                <td id="Td5" style="text-align:right; font-size:15px; color:#FFFFFF;" runat="server" >
                                 Mostrar detalles
                                    <asp:DropDownList ID="Cmb_Mostrar_Detalles" runat="server">
                                    </asp:DropDownList>                                    
                                </td>                                

                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="100%">
                                        <tr>
                                            <td style="width:31%; vertical-align:top;">                                                                                            
                                            
                                                <asp:Panel ID="Pnl_Listado_Entradas" runat="server" Width="100%" style="overflow:auto; border-width:thin;" GroupingText="Entradas">                              
                                                    <asp:GridView ID="Grid_Entradas" runat="server" CssClass="GridView_1"
                                                        AutoGenerateColumns="False" Width="96%" EmptyDataText="No hay Entradas"
                                                        GridLines= "Vertical">
                                                        <RowStyle CssClass="GridItem" />
                                                        <Columns>
                                                            <asp:BoundField DataField="FECHA" HeaderText="Fecha" SortExpression="FECHA" DataFormatString="{0:dd/MMM/yyyy}">
                                                                <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" SortExpression="CANTIDAD">
                                                                <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="NO_OPERACION" HeaderText="Orden Compra" SortExpression="NO_ENTRADA">
                                                                <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <PagerStyle CssClass="GridHeader" />
                                                        <SelectedRowStyle CssClass="GridSelected" />
                                                        <HeaderStyle CssClass="GridHeader" />                                
                                                        <AlternatingRowStyle CssClass="GridAltItem" />       
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                            <td style="width:31%; vertical-align:top;">
                                                <asp:Panel ID="Pnl_Listado_Salidas" runat="server" Width="100%" style="overflow:auto; border-width:thin;" GroupingText="Salidas">                          
                                                    <asp:GridView ID="Grid_Salidas" runat="server" CssClass="GridView_1"
                                                        AutoGenerateColumns="False" Width="96%" EmptyDataText="No hay Salidas"
                                                        GridLines= "Vertical">
                                                        <RowStyle CssClass="GridItem" />
                                                        <Columns>
                                                            <asp:BoundField DataField="FECHA" HeaderText="Fecha" SortExpression="FECHA" DataFormatString="{0:dd/MMM/yyyy}">
                                                                <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" SortExpression="CANTIDAD" >
                                                                <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="NO_OPERACION" HeaderText="Orden Salida" SortExpression="NO_SALIDA" >
                                                                <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <PagerStyle CssClass="GridHeader" />
                                                        <SelectedRowStyle CssClass="GridSelected" />
                                                        <HeaderStyle CssClass="GridHeader" />                                
                                                        <AlternatingRowStyle CssClass="GridAltItem" />       
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                            <td style="width:31%; vertical-align:top;">
                                                <asp:Panel ID="Pnl_Listado_Comprometido" runat="server" Width="100%" style="overflow:auto; border-width:thin;" GroupingText="Comprometidos">                       
                                                    <asp:GridView ID="Grid_Comprometido" runat="server" CssClass="GridView_1"
                                                        AutoGenerateColumns="False" Width="96%" EmptyDataText="No hay Comprometidos"
                                                        GridLines= "Vertical">
                                                        <RowStyle CssClass="GridItem" />
                                                        <Columns>
                                                            <asp:BoundField DataField="FECHA" HeaderText="FECHA ID" SortExpression="FECHA" DataFormatString="{0:dd/MMM/yyyy}">
                                                                <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" SortExpression="CANTIDAD" >
                                                                <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="NO_OPERACION" HeaderText="Requisición" SortExpression="FOLIO" >
                                                                <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <PagerStyle CssClass="GridHeader" />
                                                        <SelectedRowStyle CssClass="GridSelected" />
                                                        <HeaderStyle CssClass="GridHeader" />                                
                                                        <AlternatingRowStyle CssClass="GridAltItem" />       
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="background-color: #3366CC">
                                <td id="Td3" style="text-align:left; font-size:15px; color:#FFFFFF;" colspan="4" runat="server" >
                                    Detalle de Ajustes de Inventario
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="100%">
                                        <tr>
                                            <td style="width:50%; vertical-align:top;">
                                                <asp:Panel ID="Pnl_Listado_Entradas_Ajuste" runat="server" Width="100%" style="height:300px; overflow:auto; border-width:thin;" GroupingText="Entradas de Ajuste">                              
                                                    <asp:GridView ID="Grid_Entradas_Ajuste" runat="server" CssClass="GridView_1"
                                                        AutoGenerateColumns="False" Width="96%" EmptyDataText="No hay Entradas de Ajuste"
                                                        GridLines= "Vertical">
                                                        <RowStyle CssClass="GridItem" />
                                                        <Columns>
                                                            <asp:BoundField DataField="FECHA" HeaderText="Fecha" SortExpression="FECHA" DataFormatString="{0:dd/MMM/yyyy}">
                                                                <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" SortExpression="CANTIDAD">
                                                                <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="NO_OPERACION" HeaderText="No. Ajuste" SortExpression="NO_AJUSTE">
                                                                <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <PagerStyle CssClass="GridHeader" />
                                                        <SelectedRowStyle CssClass="GridSelected" />
                                                        <HeaderStyle CssClass="GridHeader" />                                
                                                        <AlternatingRowStyle CssClass="GridAltItem" />       
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                            <td style="width:50%; vertical-align:top;">
                                                <asp:Panel ID="Pnl_Listado_Salidas_Ajuste" runat="server" Width="100%" style=" height:300px; overflow:auto; border-width:thin;" GroupingText="Salidas de Ajuste">                          
                                                    <asp:GridView ID="Grid_Salidas_Ajuste" runat="server" CssClass="GridView_1"
                                                        AutoGenerateColumns="False" Width="96%" EmptyDataText="No hay Salidas de Ajuste"
                                                        GridLines= "Vertical">
                                                        <RowStyle CssClass="GridItem" />
                                                        <Columns>
                                                            <asp:BoundField DataField="FECHA" HeaderText="Fecha" SortExpression="FECHA" DataFormatString="{0:dd/MMM/yyyy}">
                                                                <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" SortExpression="CANTIDAD" >
                                                                <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="NO_OPERACION" HeaderText="No. Ajuste" SortExpression="NO_AJUSTE">
                                                                <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <PagerStyle CssClass="GridHeader" />
                                                        <SelectedRowStyle CssClass="GridSelected" />
                                                        <HeaderStyle CssClass="GridHeader" />                                
                                                        <AlternatingRowStyle CssClass="GridAltItem" />       
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                     </div>
                    <br />                                
                </center>        
            </div>
            <div>                                              

            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger  ControlID="Btn_Generar_Kardex_Excel"/>
        </Triggers>                 
    </asp:UpdatePanel>
    
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Ope_Alm_Requisicion_Listado_Stock.aspx.cs"  MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" Inherits="paginas_Almacen_Frm_Ope_Alm_Requisicion_Listado_Stock" %>

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
                <%--Div Contenido--%>        
                <div id="Div_Contenido" style="width:99%;">
                <table border="0" cellspacing="0" class="estilo_fuente" width="99%">
                    <tr>
                        <td colspan ="2" class="label_titulo">Generar Requisiciones de Listado de Almacen</td>
                    </tr>
                    <%--Fila de div de Mensaje de Error --%>
                    <tr>
                        <td colspan ="2">
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
                        <td style="width:20%" colspan="2">
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" OnClick="Btn_Modificar_Click" />
                             <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/gridview/grid_print.png" 
                                onclick="Btn_Imprimir_Click"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                            onclick="Btn_Salir_Click"/>
                        </td>
                        <%--<td align="right" style="width:80%;">
                            <div id="Div_Busqueda" runat="server">
                                <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                                    onclick="Btn_Avanzada_Click" ToolTip="Avanzada">Busqueda</asp:LinkButton>
                                    &nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                    WatermarkCssClass="watermarked"
                                    WatermarkText="<Ingrese un Folio>"
                                    TargetControlID="Txt_Busqueda" />
                                <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Consultar"
                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                    onclick="Btn_Buscar_Click" />
                            </div></td>--%>
                        
                    </tr> 
                    <tr>
                        <td colspan="2">
                            <%--Div Grid Listado--%>
                            <div id="Div_Grid_Listado" runat="server" style="width:99%;">
                            <asp:GridView ID="Grid_Listado" runat="server" Width="99%" 
                                AllowPaging="true" AutoGenerateColumns="False" DataKeyNames="Listado_ID"
                                CssClass="GridView_1" GridLines="None" PageSize="10"
                                OnSelectedIndexChanged="Grid_Listado_SelectedIndexChanged"
                                AllowSorting="True" OnSorting="Grid_Listado_Sorting" HeaderStyle-CssClass="tblHead">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" Text="Ver"
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%"/>
                                        </asp:ButtonField>
                                    <asp:BoundField DataField="Listado_ID" HeaderText="Listado_ID" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left"/>
                                        <ItemStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="True" SortExpression="Folio">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha_Creo" HeaderText="Fecha" Visible="True" SortExpression="Fecha_Creo">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" Visible="True" SortExpression="Tipo">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Total" HeaderText="Total" Visible="True" SortExpression="Total">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Right" Width="20%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <%--Div Datos_Generales--%>
                    <div id="Div_Datos_Generales" runat="server" style="width:99%;">
                        <table  border="0" cellspacing="0" class="estilo_fuente" width="99%"> 
                            <tr >
                                <td colspan="4">
                                    &nbsp;</td>
                            </tr>
                            <tr >
                                <td colspan="4" align="center">Datos Generales</td>
                            </tr>
                            <tr align="right" class="barra_delgada">
                                <td colspan="4" align="center"></td>
                            </tr>
                            <tr>
                                <td style="width:10%;">Folio</td>
                                <td style="width:40%;">
                                    <asp:TextBox ID="Txt_Folio" runat="server" Width="97%" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width:10%;">Fecha</td>
                                <td style="width:40%;">
                                    <asp:TextBox ID="Txt_Fecha" runat="server" Width="97%" Enabled="false"></asp:TextBox>
                                </td>
                                
                            </tr>
                            <tr>
                                <td>Estatus</td>
                                <td>
                                    <asp:DropDownList ID="Cmb_Estatus" runat="server" Enabled="false" Width="99%">
                                         <asp:ListItem Value="AUTORIZADA">AUTORIZADA</asp:ListItem>
                                         <asp:ListItem Value="FILTRADA">FILTRADA</asp:ListItem>
                                    </asp:DropDownList>
                                    
                                   
                                </td>
                                <td>Tipo</td>
                                <td>
                                    <asp:TextBox ID="Txt_Tipo" runat="server" Width="97%" Enabled="false" ></asp:TextBox>
                                </td>
                                
                            </tr>
                           
                            <tr>
                                <td>Comentarios</td>
                                <td colspan="3"><asp:TextBox ID="Txt_Comentario" runat="server" TabIndex="10" MaxLength="250"
                                    TextMode="MultiLine" Width="100%" Enabled="false"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkCssClass="watermarked"
                                    WatermarkText="<Límite de Caracteres 250>" 
                                    TargetControlID="Txt_Comentario" /> </td>
                                    <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                    runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    TargetControlID="Txt_Comentario" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/ ">
                                    </cc1:FilteredTextBoxExtender>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">Productos</td>
                            </tr>
                            <tr align="right" class="barra_delgada">
                                <td colspan="4" align="center">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center"> 
                                    <asp:GridView ID="Grid_Productos" runat="server"
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                            Width="99%" Enabled ="false" 
                                            AllowSorting="True" OnSorting="Grid_Productos_Sorting" HeaderStyle-CssClass="tblHead">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ControlStyle-Width="35px">
                                                <ItemTemplate >
                                                <center>
                                                <asp:CheckBox ID="Chk_Producto" runat="server"/>
                                                </center>                                                
                                                </ItemTemplate >                                                
                                                <ControlStyle Width="35px" />
                                            </asp:TemplateField>
                                                <asp:BoundField DataField="Producto_ID" HeaderText="Producto_ID" Visible="false">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Partida_ID" HeaderText="Producto_ID" Visible="false">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Clave" HeaderText="Clave" Visible="true" SortExpression="Clave">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Producto_Nombre" HeaderText="Producto" SortExpression="Producto_Nombre"
                                                    Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="25%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion"
                                                    Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="25%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" Visible="true" SortExpression="Cantidad">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Precio_Unitario" HeaderText="Precio Unitario" SortExpression="Precio_Unitario"
                                                    Visible="false">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Costo_Compra" HeaderText="Costo Compra" Visible="True" >
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Importe" HeaderText="Importe Total" Visible="True" SortExpression="Importe">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Width="10%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Monto_IVA" HeaderText="Monto_IVA" Visible="false">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Monto_IEPS" HeaderText="Monto_IEPS" Visible="false">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Porcentaje_IVA" HeaderText="Porcentaje_IVA" Visible="false">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Porcentaje_IEPS" HeaderText="Porcentaje_IEPS" Visible="false">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Font-Size="X-Small"/>
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
                                    <%--Total--%>
                                    <asp:TextBox ID="Txt_Total" runat="server" Enabled="false" Visible="false" style="text-align:right;display:none"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%--Realizar--%>
                                </td>
                                <td>
                                    <asp:DropDownList ID="Cmb_Operacion_Realizar" runat="server" Width="98%" Visible="false" style="display:none">
                                    <asp:ListItem Selected ="True">-SELECCIONAR-</asp:ListItem>
                                    <asp:ListItem Value="REQUISICION">REQUISICION</asp:ListItem>
                                    <asp:ListItem Value="BORRADO">BORRADO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%--Motivo de Borrado--%>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Motivo_Borrado" runat="server" TextMode="MultiLine" Visible="false" Width="99%" MaxLength="200" style="display:none"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                                    TargetControlID="Txt_Motivo_Borrado"
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/ "
                                Enabled="True" InvalidChars="'"></cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <asp:Button ID="Btn_Realizar_Operacion" runat="server" 
                                        Text="Generar Requisicion" CssClass="button" Width="35%" 
                                        onclick="Btn_Realizar_Operacion_Click"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                 <div id="Div_Grid_Requisiciones_Listados" runat="server" visible="false" style="width:99%" >
                                     <asp:GridView ID="Grid_Requisiciones_Listados" runat="server" HeaderStyle-CssClass="tblHead"
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                            Width="99%">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:BoundField DataField="Folio" HeaderText="No. Requisicion" Visible="true">
                                                     <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="50%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Total" HeaderText="Total" Visible="true">
                                                     <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="50%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                              
                                                <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="true">
                                                     <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="50%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                             </Columns>                                                
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                 </div>
                                </td>
                            </tr>
                            
                        </table>
                    </div>
                </table>
                </div>     
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

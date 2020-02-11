<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Ope_Alm_Seguimiento_Listado_Almacen.aspx.cs" Inherits="paginas_Almacen_Frm_Ope_Alm_Seguimiento_Listado_Almacen" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

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
            <%--Div de Contenido --%>
           <div id="Div_Contenido" style="width:97%;height:100%;">
           <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
           
            <tr>
                <td colspan ="4" class="label_titulo">Seguimiento de Listado Stock</td>
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
                <td style="width:20%;" colspan ="2">
                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                        CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                        onclick="Btn_Salir_Click"/>
                        <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/gridview/grid_print.png" 
                        onclick="Btn_Imprimir_Click" />
                </td>
                
                <td style="width:20%;" colspan ="2" align="right">
                    <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" 
                        onclick="Btn_Busqueda_Avanzada_Click">Busqueda Avanzada</asp:LinkButton>
                </td>
                
            </tr>
            
            <tr>
                <td colspan="4">
                    <div ID="Div_Busqueda_Avanzada" runat="server" style="overflow:auto;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" 
                        visible="true">
                        <table width="99%" class="estilo_fuente">
                            
                            <tr style="background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                                
                                <td colspan="4" align="right">
                                        <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Consultar"
                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Click"
                                            />
                                        <asp:ImageButton ID="Btn_Limpiar_Busqueda_Avanzada" runat="server" ToolTip="Limpiar"
                                            ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" onclick="Btn_Limpiar_Busqueda_Avanzada_Click" 
                                           />
                                        <asp:ImageButton ID="Btn_Cerrar_Busqueda_Avanzada" runat="server" ToolTip="Cerrar"
                                            ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" onclick="Btn_Cerrar_Busqueda_Avanzada_Click" 
                                            />
                                         
                                            
                                    </td>
                           
                            </tr>
                            
                            <tr>
                                <td style="text-align:left;width:20%;">
                                Folio
                                </td>
                                <td style="text-align:left;width:30%;">
                                    <asp:TextBox ID="Txt_Folio_Busqueda" runat="server" Width="98%"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" 
                                    TargetControlID="Txt_Folio_Busqueda" WatermarkCssClass="watermarked" 
                                    WatermarkText="&lt;Folio Listado&gt;" />
                                </td>
                                <td style="text-align:left;width:20%;">
                                Tipo
                                </td>
                                <td style="text-align:left;width:30%;">
                                    <asp:DropDownList ID="Cmb_Tipo" runat="server" Width="98%">
                                        <asp:ListItem Selected="True">SELECCIONAR</asp:ListItem>
                                        <asp:ListItem Selected="False" Value="MANUAL">MANUAL</asp:ListItem>
                                        <asp:ListItem Selected="False" Value="AUTOMATICO">AUTOMATICO</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:CheckBox ID="Chk_Fecha_Busqueda" runat="server" 
                                        oncheckedchanged="Chk_Fecha_Busqueda_CheckedChanged" />
                                    Fecha De&nbsp;&nbsp;&nbsp;
                                    <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="20%"></asp:TextBox>
                                    <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                    ToolTip="Seleccione la Fecha" />
                                    <cc1:CalendarExtender ID="Calendar_Inicio" runat="server" 
                                    Format="dd/MMM/yyyy" OnClientShown="calendarShown" 
                                    PopupButtonID="Btn_Fecha_Inicio" TargetControlID="Txt_Fecha_Inicio"/>
                                    &nbsp;&nbsp;&nbsp; A&nbsp;&nbsp;&nbsp;
                                    <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="20%"></asp:TextBox>
                                    <asp:ImageButton ID="Btn_Fecha_Fin" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                    ToolTip="Seleccione la Fecha" />
                                    <cc1:CalendarExtender ID="Calendar_Fin" runat="server" 
                                    Format="dd/MMM/yyyy" OnClientShown="calendarShown" 
                                    PopupButtonID="Btn_Fecha_Fin" TargetControlID="Txt_Fecha_Fin"/>
                                </td>
                               
                            </tr>
                            
                        </table>
                        
                    </div>
                </td>

            </tr>
            
            <tr>
                <td colspan="4"> 
                    <%--Div Grid Listado--%>
                    <div id="Div_Grid_Listado" runat="server" style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">
                            <asp:GridView ID="Grid_Listado" runat="server" Width="100%" 
                                AutoGenerateColumns="False" DataKeyNames="Listado_ID"
                                CssClass="GridView_1" GridLines="None"
                                HeaderStyle-CssClass="tblHead" 
                                onselectedindexchanged="Grid_Listado_SelectedIndexChanged">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" Text="Ver"
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%"/>
                                    </asp:ButtonField>
                                     <asp:BoundField DataField="Listado_ID" HeaderText="Listado_ID" Visible="false">
                                                <FooterStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                    <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="True" SortExpression="Folio">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha_Creo" HeaderText="Fecha" Visible="True" SortExpression="Fecha_Creo">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" Visible="True" SortExpression="Tipo">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Total" HeaderText="Total" Visible="True" SortExpression="Total" DataFormatString="{0:C}">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Right" Width="15%" Font-Size="X-Small"/>
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
                    <div ID="Div_Listados" runat="server" style="width:100%;font-size:9px;" 
                        visible="true">
                        <table style="width:100%;">
                            <tr>
                                <td style="width:10%;">Folio</td>
                                <td style="width:40%;">
                                    <asp:TextBox ID="Txt_Folio" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width:10%;">&nbsp;</td>
                                <td style="width:40%;">
                                    &nbsp;</td>
                                
                            </tr>
                            <tr>
                                <td>Estatus</td>
                                <td>
                                   
                                    <asp:TextBox ID="Txt_Estatus" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                   
                                </td>
                                <td>Tipo</td>
                                <td>
                                    
                                    <asp:TextBox ID="Txt_Tipo" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                    
                                </td>
                                
                            </tr>
                            <tr>
                                <td>
                                    Partida Especifica</td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Partida_Especifica" runat="server" 
                                         Width="100%" ReadOnly="true"></asp:TextBox>
                                   
                                </td>
                             </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td colspan="3">
                                    &nbsp;</td>
                             </tr>
                             <tr>
                                <td>
                                    Fecha Construccion
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Fecha_Construccion" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    Empleado Construccion
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Empleado_Construccion" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                             
                             </tr>
                             <tr>
                                <td>
                                    Fecha Generacion
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Fecha_Generacion" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    Empleado Genero
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Empleado_Genero" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                             
                             </tr>
                             <tr>
                                <td>
                                    Fecha Autorizo
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Fecha_Autorizo" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    Empleado Autorizo
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Empleado_Autorizo" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                             
                             </tr>
                             <tr>
                                <td>
                                    Fecha Cancelacion
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Fecha_Cancelacion" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    Empleado Cancelacion
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Empleado_Cancelacion" runat="server" Width="97%" ReadOnly="true"></asp:TextBox>
                                </td>
                             
                             </tr>
                             
                             <tr align="right" class="barra_delgada">
                                <td align="center" colspan="4">
                                </td>
                             </tr>
                             <tr>
                                 <td colspan="4" align="center">
                                 Requisiciones
                                 </td>
                             </tr>
                             
                             <tr>
                                <td colspan="4">
                                    <asp:GridView ID="Grid_Requisicion" runat="server"
                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" Width="99%"
                                    AllowSorting="True" HeaderStyle-CssClass="tblHead">
                                    <Columns>
                                        <asp:BoundField DataField="FOLIO" HeaderText="Folio" Visible="true">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left"/>
                                            <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="true">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left"/>
                                            <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="COTIZADOR" HeaderText="Cotizador" Visible="true">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left"/>
                                            <ItemStyle HorizontalAlign="Left" Width="70%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        
                                        
                                    
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                             </td>
                           </tr>
                             <tr align="right" class="barra_delgada">
                                <td align="center" colspan="4">
                                </td>
                             </tr>
                             <tr align="right" >
                                <td align="center" colspan="4">
                                Productos
                                </td>
                             </tr>
                             <tr>
                                <td colspan="100%">
                                 <div id="Div_Grid_Productos" runat="server" style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">
                                    <asp:GridView ID="Grid_Productos" runat="server" Width="99%"
                                    AutoGenerateColumns="False" GridLines="None" CssClass="GridView_1"
                                    HeaderStyle-CssClass="tblHead">
                                    <Columns>
                                        <asp:BoundField DataField="CLAVE" HeaderText="Clave" Visible="true" >
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_PRODUCTO" HeaderText="Producto" Visible="true" >
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                            <ItemStyle HorizontalAlign="Left" Width="35%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UNIDAD" HeaderText="Unidad" Visible="true" >
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CANTIDAD" HeaderText="Cantidad" Visible="true" >
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="No_Requisicion" HeaderText="No. Requisición"
                                            Visible="true">
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small"/>
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
                </td>

            </tr>
            
           </table><%-- Tabla principal--%>
           </div>
           
           
           
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
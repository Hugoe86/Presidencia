<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Ope_Alm_Autorizar_Ajuste_Inventario.aspx.cs" Inherits="paginas_Almacen_Frm_Ope_Alm_Autorizar_Ajuste_Inventario" %>

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
                    <td colspan ="2" class="label_titulo">Autorizar Ajuste Inventario Stock</td>
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
             <%--Fila 3 Renglon de barra de Busqueda--%>
           <tr class="barra_busqueda">
                    <td style="width:20%">
                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                        ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                            onclick="Btn_Modificar_Click"/>
                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                            CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"
                        />
                         <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/gridview/grid_print.png" 
                            onclick="Btn_Imprimir_Click"/>
                    </td>
                <td style="width:20%;" align="right">
                    <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" style="display:none">Busqueda Avanzada</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div ID="Div_Busqueda_Avanzada" runat="server" style="overflow:auto;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" 
                        visible="true">
                        <table width="99%" class="estilo_fuente">
                        <tr>
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
                            <td visible="false">
                                <asp:TextBox ID="Txt_Busqueda" runat="server" Width="85%" MaxLength="13"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar" 
                                    onclick="Btn_Buscar_Click"/>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="Txt_Busqueda" WatermarkText="&lt;NUM&gt;" WatermarkCssClass="watermarked">
                                </cc1:TextBoxWatermarkExtender>
                                <cc1:FilteredTextBoxExtender ID="FTBE_Txt_Busqueda" runat="server" FilterType="Custom" 
                                    TargetControlID="Txt_Busqueda" ValidChars="rRqQ-0123456789">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        
                        </table>
                        
                    </div>
                </td>

            </tr>
            
             <tr>
                <td colspan="2"> 
                    <%--Div Grid Listado--%>
                    <div id="Div_Grid_Ajustes_Inv" runat="server" style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">
                            <asp:GridView ID="Grid_Ajustes_Inv" runat="server" Width="100%" 
                                AutoGenerateColumns="False" DataKeyNames="No_Ajuste"
                                CssClass="GridView_1" GridLines="None"
                                HeaderStyle-CssClass="tblHead" 
                                onselectedindexchanged="Grid_Ajustes_Inv_SelectedIndexChanged">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" Text="Ver"
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%"/>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="No_Ajuste" HeaderText="No. Ajuste" Visible="true">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left"  Width="15%" />
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha_Hora" HeaderText="Fecha Hora" Visible="True" SortExpression="Fecha_Hora">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Empleado_Elaboro" HeaderText="Empleado Elaboro" Visible="True" SortExpression="Empleado_Elaboro">
                                        <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                        <ItemStyle HorizontalAlign="Left" Width="50%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
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
                <td colspan="2">
                    <div ID="Div_Datos_Ajuste" runat="server" visible=true>
                        <table width="99%" >
                            <tr>
                                <td width="15%">No. Ajuste</td>
                                <td width="35%"> 
                                    <asp:TextBox ID="Txt_No_Ajuste" runat="server" Width="99%" Enabled="false"></asp:TextBox></td>
                                <td width="15%">Fecha y Hora</td>
                                <td width="35%"> 
                                    <asp:TextBox ID="Txt_Fecha_Hora" runat="server" Width="99%" Enabled="false"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Empleado Solicita</td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Empleado_Genero" runat="server" Width="99%" Enabled="false" ></asp:TextBox> </td>
                                
                            </tr>
                            <tr>
                                <td>Motivo Solicitud</td>
                                <td colspan="3"> 
                                    <asp:TextBox ID="Txt_Motivo_Ajuste_Coordinador" runat="server" TextMode="MultiLine" Width="99%" Height="100px" Enabled="false"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Estatus</td>
                                <td>
                                    <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" Enabled="false" AutoPostBack="true"
                                        onselectedindexchanged="Cmb_Estatus_SelectedIndexChanged">
                                        <asp:ListItem Selected="True">&#60;&#60;SELECCIONAR&#62;&#62;</asp:ListItem>
                                        <asp:ListItem Selected="False" Value="GENERADO">GENERADO</asp:ListItem>
                                        <asp:ListItem Selected="False" Value="AUTORIZADO">AUTORIZADO</asp:ListItem>
                                        <asp:ListItem Selected="False" Value="CANCELADO">CANCELADO</asp:ListItem>
                                    </asp:DropDownList>
                                    </td>
                            </tr>
                            <tr>
                                <td>
                                    Fecha Autorizacion</td>
                                <td>
                                    <asp:TextBox ID="Txt_Fecha_Autorizacion" runat="server" Width="99%" 
                                        Enabled="false"></asp:TextBox>
                                    </td>
                                <td>
                                    Fecha Cancelo</td>
                                <td>
                                    <asp:TextBox ID="Txt_Fecha_Rechazo" runat="server" Width="99%" Enabled="false" ></asp:TextBox>
                                    </td>
                            
                            </tr>
                            <tr>
                                <td>
                                    Empleado Autorizo
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Empleado_Autorizo" runat="server" Width="99%" Enabled="false"></asp:TextBox>
                                </td>
                                <td>
                                    Empleado Cancelo
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Empleado_Rechazo" runat="server" Width="99%" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    Comentarios de quien Autoriza/Cancela</td>
                                <td colspan="3"> 
                                    <asp:TextBox ID="Txt_Motivo_Ajuste_Dir" runat="server" TextMode="MultiLine" 
                                        Width="99%" Height="100px" Enabled="false"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <div id="Div1" runat="server" style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">
                                    <asp:GridView ID="Grid_Productos" runat="server" Width="100%" 
                                    AutoGenerateColumns="False" 
                                    CssClass="GridView_1" GridLines="None"
                                    HeaderStyle-CssClass="tblHead">
                                    <Columns>
                                       
                                        <asp:BoundField DataField="Clave" HeaderText="Clave" Visible="True" SortExpression="Clave">
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Nombre_Descripcion" HeaderText="Producto" Visible="True" SortExpression="Nombre_Descripcion">
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Existencia_Sistema" HeaderText="Existencia Sistema" Visible="True" SortExpression="Existencia_Sistema">
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Conteo_Fisico" HeaderText="Conteo Fisico" Visible="True" SortExpression="Conteo_Fisico">
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Diferencia" HeaderText="Diferencia" Visible="True" SortExpression="Diferencia">
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Tipo_Movimiento" HeaderText="Tipo Movimiento" Visible="True" SortExpression="Tipo_Movimiento">
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        
                                        <asp:BoundField DataField="Importe_Diferencia" HeaderText="Importe Diferencia" Visible="True" SortExpression="Importe_Diferencia">
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                        </asp:BoundField>
                                        
                                        <asp:BoundField DataField="Precio_Promedio" HeaderText="Precio Promedio" Visible="True" SortExpression="Precio_Promedio">
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
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
                                    <table width="100%">
                                     <tr>
                                        <td>
                                        
                                        </td>
                                        <td>
                                        
                                        </td>
                                        <td>
                                            Entradas
                                        </td>
                                        <td>
                                            Salidas
                                        </td> 
                                        <td>
                                            Total
                                        </td>                                                                                                               
                                    </tr>
                                    <tr>
                                        <td style="width: 16%" valign="top">
                                            
                                        </td>
                                        <td>
                                            Por Producto
                                        </td>    
                                        <td style="width:20%" valign="top">
                                            <asp:Label ID="Lbl_Entradas_Producto" runat="server" Text="0" BorderColor="GrayText" 
                                               BorderWidth="1px" Height="18px" Width="133px" Style="text-align:right;"></asp:Label>
                                               -</td>
                                        <td>
                                            <asp:Label ID="Lbl_Salidas_Producto" runat="server" Text="0" BorderColor="GrayText" 
                                               BorderWidth="1px" Height="18px" Width="133px" Style="text-align:right;"></asp:Label> 
                                               =                                                             
                                        </td>   
                                        <td>
                                            <asp:Label ID="Lbl_Producto_Ajustado" runat="server" Text="0" BorderColor="GrayText" 
                                               BorderWidth="1px" Height="18px" Width="133px" Style="text-align:right;"></asp:Label> 
                                                                           

                                        </td>                                                                                                                                       
                                    </tr> 
                                    <tr>
                                        <td style="width: 16%" valign="top">
                                            
                                        </td>
                                        <td>
                                            Por Unidades
                                        </td>                         
                                        <td style="width: 20%" valign="top">
                                            <asp:Label ID="Lbl_Entradas_Unidad" runat="server" Text="0" BorderColor="GrayText" 
                                               BorderWidth="1px" Height="18px" Width="133px" Style="text-align:right;"></asp:Label>
                                               -</td>
                                        <td>
                                            <asp:Label ID="Lbl_Salidas_Unidad" runat="server" Text="0" BorderColor="GrayText" 
                                               BorderWidth="1px" Height="18px" Width="133px" Style="text-align:right;"></asp:Label>
                                               =
                                        </td>                                                                                                            
                                        <td>
                                            <asp:Label ID="Lbl_Unidades_Ajustadas" runat="server" Text="0" BorderColor="GrayText" 
                                               BorderWidth="1px" Height="18px" Width="133px" Style="text-align:right;"></asp:Label>                                    
                                        

                                        </td>  
                                    </tr>                    
                                    <tr>
                                        <td style="width: 16%" valign="top">
                                       
                                        </td>
                                        <td>
                                            Importe
                                        </td>    
                                        <td style="width: 20%" valign="top">
                                            <asp:Label ID="Lbl_Importe_Entradas" runat="server" Text="0.00" BorderColor="GrayText" 
                                               BorderWidth="1px" Height="18px" Width="133px" Style="text-align:right;"></asp:Label>
                                               -</td>
                                        <td>
                                            <asp:Label ID="Lbl_Importe_Salidas" runat="server" Text="0.00" BorderColor="GrayText" 
                                               BorderWidth="1px" Height="18px" Width="133px" Style="text-align:right;"></asp:Label> 
                                               =                             
                                        </td>   
                                        <td>
                                            <asp:Label ID="Lbl_Importe_Saldo" runat="server" Text="0.00" BorderColor="GrayText" 
                                               BorderWidth="1px" Height="18px" Width="133px" Style="text-align:right;"></asp:Label>
                                        </td>                                                                                                          
                                    </tr>   
                                    
                                    
                                    </table>
                                
                                </td>
                            
                            </tr>
                            
                        </table>
                    </div>
                
                </td>
             
             
             </tr>
            </table>
            </div>
           
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
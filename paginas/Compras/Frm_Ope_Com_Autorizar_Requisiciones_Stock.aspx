<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Com_Autorizar_Requisiciones_Stock.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Autorizar_Requisiciones_Stock" 
Title="Seguimiento a Requisiciones" Culture="es-MX"%>                                                                 
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true">
    </asp:ScriptManager>
    
    <div id="Div_General" style="width: 98%;" visible="true" runat="server">
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
            <ContentTemplate>                                   
            
                <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">              
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>                            
                </asp:UpdateProgress>
                <%--Div Encabezado--%>                                
                <div id="Div_Encabezado" runat="server">
                    <table style="width: 100%;" border="0" cellspacing="0">
                        <tr align="center">
                            <td colspan="4" class="label_titulo">
                                Asignar reserva a requisición de Stock
                            </td>
                        </tr>
                        <tr align="left">
                            <td colspan="4">
                                <asp:Image ID="Img_Warning" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />
                                <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="#990000"></asp:Label>
                            </td>
                        </tr>
                        <tr class="barra_busqueda" align="right">
                            <td align="left" valign="middle" colspan="2">
                                <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" CssClass="Img_Button" ToolTip="Nuevo" />
                                <asp:ImageButton ID="Btn_Guardar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" CssClass="Img_Button" ToolTip="Guardar" OnClick="Btn_Guardar_Click"/>                                
                                <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" CssClass="Img_Button" AlternateText="Modificar" ToolTip="Modificar"  Visible="false"/>
                                <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" CssClass="Img_Button" AlternateText="Eliminar" ToolTip="Eliminar" />
                                <asp:ImageButton ID="Btn_Listar_Requisiciones" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Listar Requisiciones" OnClick="Btn_Listar_Requisiciones_Click" />
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" OnClick="Btn_Salir_Click" />
                            </td>
                            <td colspan="2">
<%--                                <asp:Button ID="Btn_Comodin_Busqueda_Productos_Srv" runat="server" Text="Button" Style="display: none" />
                                <cc1:ModalPopupExtender ID="Modal_Busqueda_Prod_Serv" runat="server" TargetControlID="Btn_Comodin_Busqueda_Productos_Srv" PopupControlID="Modal_Productos_Servicios" CancelControlID="Btn_Cerrar"
                                    DropShadow="True" BackgroundCssClass="progressBackgroundFilter" />--%>
                            </td>
                        </tr>
                    </table>
                </div>
                <%--Div listado de requisiciones--%>
                <div id="Div_Listado_Requisiciones" runat="server">
                    <table style="width: 100%;">
                        <tr visible="false">
                            <td style="width: 15%;" visible="false">
                        
                            </td>
                            <td style="width: 43%;">
                                <asp:DropDownList ID="Cmb_Dependencia_Panel" runat="server" Width="98%" Visible="false"/>
                            </td>
                            <td style="width: 12%;" align="right" visible="false">
        
                            </td>
                            <td visible="false">
                                <asp:DropDownList ID="Cmb_Tipo_Busqueda" runat="server" Width="98%" Visible="false"/>
                            </td>
                        </tr>                    
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
                                <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Click" ToolTip="Consultar"/>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="Txt_Busqueda" WatermarkText="&lt;RQ-000000&gt;" WatermarkCssClass="watermarked">
                                </cc1:TextBoxWatermarkExtender>
                                <cc1:FilteredTextBoxExtender ID="FTBE_Txt_Busqueda" runat="server" FilterType="Custom" 
                                    TargetControlID="Txt_Busqueda" ValidChars="rRqQ-0123456789">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 99%" align="center" colspan="4">
                              <div style="overflow:auto;height:320px;width:99%;vertical-align:top;
                                   border-style:outset;border-color: Silver;" > 
                                <asp:GridView ID="Grid_Requisiciones" runat="server" AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" Width="100%"
                                    DataKeyNames="No_Requisicion"
                                    AllowSorting="true" HeaderStyle-CssClass="tblHead" OnSorting="Grid_Requisiciones_Sorting" 
                                    onrowdatabound="Grid_Requisiciones_RowDataBound"
                                    EmptyDataText="No se encontraron requisiciones"
                                    >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Seleccionar_Requisicion" runat="server" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png" 
                                                    OnClick="Btn_Seleccionar_Requisicion_Click"                                                    
                                                    CommandArgument='<%# Eval("No_Requisicion") %>'/>
                                            </ItemTemplate >
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:TemplateField>  
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Imprimir_Requisicion" runat="server" 
                                                    ImageUrl="~/paginas/imagenes/gridview/grid_print.png" 
                                                    OnClick="Btn_Imprimir_Requisicion_Click"                                                    
                                                    CommandArgument='<%# Eval("No_Requisicion") %>'/>
                                            </ItemTemplate >
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:TemplateField>                                                                           
                                        <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="True" SortExpression="No_Requisicion">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Fecha_Creo" HeaderText="Fecha" 
                                            DataFormatString="{0:dd/MMM/yyyy}" Visible="True" SortExpression="Fecha_Creo">
                                            <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_DEPENDENCIA" HeaderText="Unidad Responsable" Visible="True" SortExpression="NOMBRE_DEPENDENCIA">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>                                                                             
                                        <asp:TemplateField HeaderText="" Visible="false">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Alerta" runat="server" 
                                                    ImageUrl="~/paginas/imagenes/gridview/circle_grey.png"                                                                                                        
                                                    CommandArgument='<%# Eval("No_Requisicion") %>'/>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="4%" />
                                            <ItemStyle HorizontalAlign="Center" Width="4%" />
                                        </asp:TemplateField>                                                                                   
                                        <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="No_Requisicion" HeaderText="ID" Visible="false" >
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
                              </div>  
                            </td>
                        </tr>
                    </table>
                </div>
                <%--Div Contenido--%>
                <div id="Div_Contenido" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 15%;" align="left">
                                Folio
                            </td>
                            <td style="width: 35%;" align="left">
                                <asp:TextBox ID="Txt_Folio" runat="server" Width="96%"></asp:TextBox>
                            </td>
                            <td style="width: 15%;" align="left">
                                <asp:Label ID="Lbl_No_Reserva" runat="server" Text="*No. Reserva" ForeColor="Blue"></asp:Label>
                            </td>
                            <td style="width: 35%;" align="left">
                                <asp:TextBox ID="Txt_No_Reserva" runat="server" Width="96%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;" align="left">
                                Unidad Responsable
                            </td>
                            <td style="width: 35%;" align="left">
                                <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="98%"  AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 15%;" align="left">
                                <asp:Label ID="Lbl_Estatus" runat="server" Text="*Estatus" ForeColor="Blue"></asp:Label>
                            </td>
                            <td style="width: 35%;" align="left">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" 
                                    onselectedindexchanged="Cmb_Estatus_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>                            
                        </tr>
                        <tr>
                            <td>
                                Código Programatico
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Codigo_Programatico" runat="server" Width="98%"></asp:TextBox>
                            </td>
                            <td>
                                Importe
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Importe" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                            </td>

                        </tr> 
                        <tr>
                            <td colspan="4">
                                Justificación
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:TextBox ID="Txt_Justificación" runat="server" ReadOnly="true" Height="90px" Width="100%"></asp:TextBox>
                            </td>
                        </tr> 
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="Lbl_Motivo_Rechazo" runat="server" Text="*Ingrese el motivo por el cual rechaza la requisición" ForeColor="Blue" Visible="false"></asp:Label>
                            </td>
                        </tr>   
                        <tr>
                            <td colspan="4">
                                <asp:TextBox ID="Txt_Motivo_Rechazo" runat="server" Height="90px" Width="100%" Visible="false" BorderStyle="Solid"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="Txt_Motivo_Rechazo"
                                    WatermarkText="&lt;Límite de Caracteres 3000&gt;" WatermarkCssClass="watermarked">
                                </cc1:TextBoxWatermarkExtender>
                                <cc1:FilteredTextBoxExtender ID="FTE_Justificacion" runat="server" TargetControlID="Txt_Motivo_Rechazo" 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ./*-!$%&()=,[]{}+<>@?¡?¿# ">
                                </cc1:FilteredTextBoxExtender>                            
                            </td>
                        </tr>                                            
                        <tr>
                            <td align="center" colspan="4">
                                <asp:GridView ID="Grid_Productos_Servicios" runat="server" 
                                    AutoGenerateColumns="False" CssClass="GridView_1" Visible="false" 
                                    GridLines="None" Width="100%"                                     
                                    style="white-space:normal" >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" Visible="True" DataFormatString="{0:dd/MMM/yyyy}">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="11%"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Fecha" HeaderText="Hora" Visible="True" DataFormatString="{0:hh:mm:ss tt}">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="11%"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Empleado" HeaderText="Modificó" Visible="True">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left"  />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>    
</asp:Content>


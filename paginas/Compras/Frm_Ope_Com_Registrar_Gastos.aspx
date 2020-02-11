<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Com_Registrar_Gastos.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Registrar_Gastos"
    Title="Registrar Gasto" Culture="es-MX"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
        function calendarShown(sender, args){
            sender._popupBehavior._element.style.zIndex = 10000005;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true">
    </asp:ScriptManager>
    <div id="Div_General" style="width: 98%;" visible="true" runat="server" >
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
                                Registrar Gastos
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
                                <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" CssClass="Img_Button" ToolTip="Nuevo" OnClick="Btn_Nuevo_Click" />
                                <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" CssClass="Img_Button" AlternateText="Modificar" ToolTip="Modificar" OnClick="Btn_Modificar_Click" />
                                <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" CssClass="Img_Button" AlternateText="Eliminar" ToolTip="Eliminar" />
                                <asp:ImageButton ID="Btn_Listar_Requisiciones" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Listar Requisiciones" OnClick="Btn_Listar_Requisiciones_Click" />
                                <asp:ImageButton ID="Btn_Salir" runat="server" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" OnClick="Btn_Salir_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <%--Div listado de requisiciones--%>
                <div id="Div_Listado_Requisiciones" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 15%;">
                                Dependencia
                            </td>
                            <td style="width: 35%;">
                                <asp:DropDownList ID="Cmb_Dependencia_Panel" runat="server" Width="98%" />
                            </td>
                            <td style="width: 15%;">
                                Estatus
                            </td>
                            <td style="width: 35%;">
                                <asp:DropDownList ID="Cmb_Estatus_Busqueda" runat="server" Width="98%" />
                            </td>
                            
                        </tr>
                        <tr>
                            <td style="width: 15%;">
                                Fecha
                            </td>
                            <td style="width: 35%;">
                                <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="90px" Enabled="false"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Txt_Fecha_Inicial_FilteredTextBoxExtender" runat="server" TargetControlID="Txt_Fecha_Inicial" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters"
                                    ValidChars="/_" />
                                <cc1:CalendarExtender ID="Txt_Fecha_Inicial_CalendarExtender" runat="server" TargetControlID="Txt_Fecha_Inicial" PopupButtonID="Btn_Fecha_Inicial" Format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Fecha_Inicial" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Inicial" />
                                :&nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="90px" Enabled="false"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="Txt_Fecha_Final" PopupButtonID="Btn_Fecha_Final" Format="dd/MMM/yyyy" />
                                <asp:ImageButton ID="Btn_Fecha_Final" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" ToolTip="Seleccione la Fecha Final" />
                            </td>
                            <td style="width: 15%;">
                                Folio
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Busqueda" runat="server" Width="85%"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Click" ToolTip="Consultar"/>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="Txt_Busqueda" WatermarkText="&lt;GT-000000&gt;" WatermarkCssClass="watermarked">
                                </cc1:TextBoxWatermarkExtender>                                
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 99%" align="center" colspan="4">
                                <asp:GridView ID="Grid_Requisiciones" runat="server" AllowPaging="True" AutoGenerateColumns="False" 
                                    CssClass="GridView_1" GridLines="None" Width="100%" 
                                    OnPageIndexChanging="Grid_Requisiciones_PageIndexChanging"
                                    DataKeyNames="Gasto_ID,Partida_ID,PROYECTO_PROGRAMA_ID,FUENTE_FINANCIAMIENTO_ID"
                                    AllowSorting="true" OnSorting="Grid_Requisiciones_Sorting">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Seleccionar_Requisicion" runat="server" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png" 
                                                    OnClick="Btn_Seleccionar_Gasto_Click"                                                    
                                                    CommandArgument='<%# Eval("Gasto_ID") %>'/>
                                            </ItemTemplate >
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:TemplateField>                       
                                                                                             
                                        <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="True" SortExpression="Gasto_ID">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Fecha_Creo" HeaderText="Fecha" DataFormatString="{0:dd/MMM/yyyy}" Visible="True"
                                            SortExpression="Fecha_Creo">
                                            <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_DEPENDENCIA" HeaderText="Unidad Responsable" Visible="True" SortExpression="NOMBRE_DEPENDENCIA">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Partida_ID" HeaderText="Tipo" Visible="false" >
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Gasto_ID" HeaderText="ID" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FUENTE_FINANCIAMIENTO_ID" HeaderText="ID" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PROYECTO_PROGRAMA_ID" HeaderText="ID" Visible="false">
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
                                Fecha
                            </td>
                            <td style="width: 35%;" align="left">
                                <asp:TextBox ID="Txt_Fecha" runat="server" Width="98%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;" align="left">
                                Dependencia
                            </td>
                            <td style="width: 35%;" align="left">
                                <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="98%" OnSelectedIndexChanged="Cmb_Dependencia_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 15%;" align="left">
                                Estatus
                            </td>
                            <td style="width: 35%;" align="left">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;" align="left">
                                Fuente de Financiamiento
                            </td>
                            <td style="width: 35%;" align="left">
                                <asp:DropDownList ID="Cmb_Fte_Financiamiento" runat="server" Width="98%" OnSelectedIndexChanged="Cmb_Fte_Financiamiento_SelectedIndexChanged" AutoPostBack="true" />
                            </td>
                            <td style="width: 15%;" align="left">
                                Programa
                            </td>
                            <td style="width: 35%;" align="left">
                                <asp:DropDownList ID="Cmb_Programa" runat="server" Width="99%" OnSelectedIndexChanged="Cmb_Programa_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;" align="left" valign="top">
                                Partida
                            </td>
                            <td style="width: 35%;" align="left" valign="top">
                                <asp:DropDownList ID="Cmb_Partida" runat="server" Width="99%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Partida_SelectedIndexChanged1">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 15%;" align="left">
                                Disponible
                            </td>
                            <td style="width: 35%;" align="left">
                                <asp:Label ID="Lbl_Disponible_Partida" runat="server" Text=" $ 0.00" ForeColor="Blue" BorderColor="Blue" BorderWidth="2px" Width="98%">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;" align="left">
                            </td>
                            <td style="width: 85%;" align="left" colspan="3">
                                <div id="Div_Presupuesto" runat="server" visible="false">
                                    <table border="2" width="99%">
                                        <tr style="background-color: #D8D8D8; height: 3px;">
                                            <td>
                                                Partida
                                            </td>
                                            <td>
                                                Clave
                                            </td>
                                            <td>
                                                Disponible
                                            </td>
                                            <td>
                                                Fecha Asignación
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 45%">
                                                <asp:Label ID="Lbl_Partida" runat="server" Text="Label" ForeColor="Green"></asp:Label>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:Label ID="Lbl_Clave" runat="server" Text="Label" ForeColor="Green"></asp:Label>
                                            </td>
                                            <td style="width: 20%">
                                                <asp:Label ID="Lbl_Disponible" runat="server" Text="Label" ForeColor="Green"></asp:Label>
                                            </td>
                                            <td style="width: 20%">
                                                <asp:Label ID="Lbl_Fecha_Asignacion" runat="server" Text="Label" ForeColor="Green"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="height: 4px;">
                                <hr class="linea" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td colspan="4">
                                <table style="width:100%;">
                                <tr>
                                    <td style="width: 65%;" align="left">
                                        Producto/Servicio
                                    </td>
                                    <td style="width: 5%;" align="left">
                                        IEPS
                                    </td>
                                    <td style="width: 5%;" align="left">
                                        IVA
                                    </td>                                    
                                    <td style="width: 10%;" align="left">
                                        Ctd.
                                    </td>
                                    <td style="width: 10%;" align="left">
                                        Precio U.
                                    </td>
                                    <td style="width: 5%;" align="left">
                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 65%;" align="left">
                                        <asp:TextBox ID="Txt_Producto_Servicio" runat="server" Width="98%"></asp:TextBox>
                                    </td>
                                    <td style="width: 5%;" align="left">
                                        <asp:DropDownList ID="Cmb_IEPS" runat="server">
                                        </asp:DropDownList>
                                    </td>                                                                        
                                    <td style="width: 5%;" align="left">
                                        <asp:DropDownList ID="Cmb_IVA" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 10%;" align="left">
                                        <asp:TextBox ID="Txt_Cantidad" runat="server" Width="95%" MaxLength="8"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTB_Txt_Cantidad" runat="server" 
                                            FilterType="Custom,Numbers"
                                            TargetControlID="Txt_Cantidad">
                                        </cc1:FilteredTextBoxExtender>                                        
                                    </td>
                                    <td style="width: 10%;" align="left">                                        
                                        <asp:TextBox ID="Txt_Precio_Unitario" runat="server" Width="95%" MaxLength="10"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Ftb_Txt_Precio" runat="server" 
                                            FilterType="Custom,Numbers"
                                            TargetControlID="Txt_Precio_Unitario" ValidChars=",.">
                                        </cc1:FilteredTextBoxExtender>                                        
                                        
                                    </td>
                                    <td style="width: 5%;" align="left">
                                       <asp:ImageButton ID="Ibtn_Agregar_Producto" runat="server" 
                                            OnClick="Ibtn_Agregar_Producto_Click" ToolTip="Agregar producto ó servicio" 
                                            ImageUrl="~/paginas/imagenes/paginas/accept.png" />
                                    </td>                                    
                                </tr>
                                </table>
                               </td>
                            <tr>
                                                                                                                      
                        </tr> </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <asp:GridView ID="Grid_Productos_Servicios" runat="server" AutoGenerateColumns="False" 
                                     CssClass="GridView_1" AllowPaging="true" PageSize="5" 
                                     GridLines="None" Width="100%" 
                                     OnPageIndexChanging="Grid_Productos_Servicios_PageIndexChanging"
                                     DataKeyNames="IDENTIFICADOR" style="white-space:normal">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Grid_Eliminar" runat="server" 
                                                    ImageUrl="~/paginas/imagenes/paginas/delete.png" 
                                                    OnClick="Btn_Grid_Gastos_Eliminar_Prod_Serv_Click"  
                                                    CommandArgument='<%# Eval("IDENTIFICADOR") %>'/>
                                            </ItemTemplate >
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:TemplateField>                                                                        
                                        <asp:BoundField DataField="PRODUCTO_SERVICIO" HeaderText="Producto/Servicio" Visible="True" >
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CANTIDAD" HeaderText="Ctd." Visible="True">
                                            <FooterStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="10%" Wrap="true"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="COSTO" HeaderText="$ Unitario S/I" Visible="True">
                                            <FooterStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IDENTIFICADOR" HeaderText="ID" Visible="false">
                                            <FooterStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="true"/>
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
                            <td align="right" colspan="4" style="width: 35%;">
                                Subtotal
                                <asp:TextBox ID="Txt_Subtotal" runat="server" 
                                Style="text-align: right" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4" style="width: 35%;">
                                IEPS
                                <asp:TextBox ID="Txt_IEPS" runat="server" 
                                Style="text-align: right" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>    
                            <td align="right" colspan="4" style="width: 35%;">
                                IVA
                                <asp:TextBox ID="Txt_IVA" runat="server" 
                                Style="text-align: right" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4" style="width: 35%;">
                                Total
                                <asp:TextBox ID="Txt_Total" runat="server" 
                                Style="text-align: right" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <hr class="linea" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <%--<asp:FileUpload ID="FileUpload1" runat="server" />--%>
                                <%--                                    <asp:UpdatePanel ID="Upnl_Botones_Operacion" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                        <ContentTemplate>
                                            <cc1:AsyncFileUpload ID="FUP_Anexo" runat="server" ErrorBackColor="Red" CompleteBackColor="Lime" UploadingBackColor="Silver" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </td>
                                <td align="left" colspan="4">
                                    <%--<asp:CheckBox ID="Chk_Verificar" runat="server" Text="Verificar características de productos" ToolTip="Verificar las características, garantía y pólizas de mantenimiento de la mercancía cuando se reciba del proveedor" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    Justificación del Gasto
                                </td>
                                <td align="left" colspan="3">
                                    <asp:TextBox ID="Txt_Justificacion" runat="server" MaxLength="1500" TabIndex="10" TextMode="MultiLine" Width="100%" Height="35px"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="Txt_Justificacion" WatermarkText="&lt;Límite de Caracteres 1500&gt;" WatermarkCssClass="watermarked">
                                    </cc1:TextBoxWatermarkExtender>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Justificacion" runat="server" TargetControlID="Txt_Justificacion" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    Comentarios
                                    <asp:LinkButton ID="Lnk_Observaciones" runat="server" OnClick="Lnk_Observaciones_Click">Mostrar</asp:LinkButton>
                                </td>
                                <td align="left" colspan="3">
                                    <asp:TextBox ID="Txt_Comentario" runat="server" MaxLength="1500" TabIndex="10" TextMode="MultiLine" Width="100%" Height="35px"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="Txt_Comentario_TextBoxWatermarkExtender" runat="server" TargetControlID="Txt_Comentario" WatermarkText="&lt;Límite de Caracteres 1500&gt;" WatermarkCssClass="watermarked">
                                    </cc1:TextBoxWatermarkExtender>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Comentario" runat="server" TargetControlID="Txt_Comentario" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <%--Div Observaciones--%>
                            <div id="Div_Comentarios" runat="server">
                                <tr>
                                    <td align="left" valign="top" colspan="4">
                                        <asp:GridView ID="Grid_Comentarios" runat="server" AllowPaging="true" AutoGenerateColumns="false" GridLines="None"
                                         PageSize="3" Width="100%" OnSelectedIndexChanged="Grid_Comentarios_SelectedIndexChanged" Visible="false"
                                            Style="font-size: xx-small" OnPageIndexChanging="Grid_Comentarios_PageIndexChanging">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="5%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="Comentario" HeaderText="Comentarios" Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" Width="68%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="68%" Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Fecha_Creo" HeaderText="Fecha" Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" Width="8%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="8%" Font-Size="X-Small" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Usuario_Creo" HeaderText="Usuario" Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" Width="24%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="24%" Font-Size="X-Small" />
                                                </asp:BoundField>
                                            </Columns>
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </div>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%-- Panel del ModalPopUp--%>
    </div>
    <asp:Panel ID="Modal_Productos_ServiciosX" runat="server" CssClass="drag" HorizontalAlign="Center" Style="display: none; border-style: outset; border-color: Silver; width: 760px;">
        <asp:Button ID="Btn_Realizar_Busqueda" runat="server" Text="Buscar" CssClass="button" />
        <asp:Button ID="Btn_Cerrar" runat="server" Text="Cerrar" CssClass="button" />
    </asp:Panel>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Com_Filtrar_Requisiciones_Transitorias.aspx.cs" Inherits="paginas_Almacen_Frm_Ope_Com_Filtrar_Requisiciones_Transitorias" 
Title="Filtrado de Requisiciones Transitorias" Culture="es-MX"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
   <!--SCRIPT PARA LA VALIDACION QUE NO EXPIRE LA SESSION-->  
   <script language="javascript" type="text/javascript">
    <!--
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_sesiones.ashx";

        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion() {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }
        //Temporizador para matener la sesión activa
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);
    //-->
   </script>
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
                                Filtrar Requisiciones
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
                            <td colspan="2">
                            </td>
                        </tr>
                    </table>
                </div>
                <%--Div listado de requisiciones--%>
                <div id="Div_Listado_Requisiciones" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 15%;">
                                Unidad Responsable
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Dependencia_Panel" runat="server" Width="98%" />
                            </td>
                            <td style="width: 15%;" align="right" visible="false">
                                Tipo
                            </td>
                            <td visible="false">
                                <asp:DropDownList ID="Cmb_Tipo_Busqueda" runat="server" Width="98%" />
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


                        </tr>
                        <tr>
                            <td style="width: 99%" align="center" colspan="4">
                              <div style="overflow:auto;height:320px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" > 
                                <asp:GridView ID="Grid_Requisiciones" runat="server" AllowPaging="false" AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" Width="100%" OnPageIndexChanging="Grid_Requisiciones_PageIndexChanging"
                                    DataKeyNames="No_Requisicion"
                                    AllowSorting="true" HeaderStyle-CssClass="tblHead" OnSorting="Grid_Requisiciones_Sorting" 
                                    onrowdatabound="Grid_Requisiciones_RowDataBound" EmptyDataText="No se encontraron requisiciones">
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
                                        <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="True" SortExpression="No_Requisicion">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Fecha_Creo" HeaderText="Fecha Inicial" 
                                            DataFormatString="{0:dd/MMM/yyyy}" Visible="True" SortExpression="Fecha_Creo">
                                            <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_DEPENDENCIA" HeaderText="Unidad Responsable" Visible="True" SortExpression="NOMBRE_DEPENDENCIA">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>                                                                             
                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" Visible="True" SortExpression="Tipo">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="12%" />
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
                                            <ItemStyle HorizontalAlign="Left" Width="12%" />
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
                                *Estatus
                            </td>
                            <td style="width: 35%;" align="left">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="58%" />
                                <asp:TextBox ID="Txt_Fecha" runat="server" Width="38%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;" align="left">
                                *Unidad Responsable
                            </td>
                            <td style="width: 35%;" align="left">
                                <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="98%" OnSelectedIndexChanged="Cmb_Dependencia_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 15%;" align="left">
                                *Tipo Requisición
                            </td>
                            <td style="width: 35%;" align="left">
                                <asp:DropDownList ID="Cmb_Tipo" runat="server" Width="58%" AutoPostBack="True" >
                                </asp:DropDownList>
                                <asp:DropDownList ID="Cmb_Producto_Servicio" runat="server" Width="40%"  AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        

 
                        <tr>
                            <td align="center" colspan="4">
                                <asp:GridView ID="Grid_Productos_Servicios" runat="server" 
                                    AutoGenerateColumns="False" CssClass="GridView_1" AllowPaging="false" 
                                    PageSize="5" GridLines="None" Width="100%" OnRowCommand="Grid_Productos_Servicios_RowCommand"
                                    DataKeyNames="Tipo,Prod_Serv_ID,Monto_IVA,Monto_IEPS" 
                                    OnPageIndexChanging="Grid_Productos_Servicios_PageIndexChanging" 
                                    style="white-space:normal" >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:BoundField DataField="Nombre_Producto_Servicio" HeaderText="Producto/Servicio" Visible="True">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Cantidad" HeaderText="Ctd." Visible="True">
                                            <FooterStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="6%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UNIDAD" HeaderText="Unidad" Visible="true">
                                            <FooterStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="6%" />
                                        </asp:BoundField>                                        
                                        <asp:BoundField DataField="Precio_Unitario" HeaderText="$ Unitario" Visible="True">
                                            <FooterStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Importe" HeaderText="Acumulado" Visible="false">
                                            <FooterStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Right" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Monto_IVA" HeaderText="IVA" Visible="False">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Monto_IEPS" HeaderText="IEPS" Visible="False">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Porcentaje_IEPS" HeaderText="Porcentaje_IEPS" Visible="False">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Porcentaje_IVA" HeaderText="Porcentaje_IVA" Visible="False">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Monto" HeaderText="$ Importe" Visible="true">
                                            <FooterStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Prod_Serv_ID" HeaderText="Producto_Servicio_ID" Visible="False">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Partida" HeaderText="Partida" Visible="False">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Proyecto_Programa" HeaderText="Proyecto_Programa" Visible="False">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" Visible="false">
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
                            <td align="right" colspan="4">
                                Subtotal
                                <asp:TextBox ID="Txt_Subtotal" runat="server" Style="text-align: right" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4">
                                IEPS
                                <asp:TextBox ID="Txt_IEPS" runat="server" Style="text-align: right" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4">
                                IVA
                                <asp:TextBox ID="Txt_IVA" runat="server" Style="text-align: right" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>

                            <td align="right" colspan="4" >
                                Total
                                <asp:TextBox ID="Txt_Total" runat="server"  Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <hr class="linea" />
                            </td>
                        </tr>
                            <tr>

                                <td align="left" colspan="4">
                                    <asp:CheckBox ID="Chk_Verificar" runat="server" Text="Verificar características de productos" ToolTip="Verificar las características, garantía y pólizas de mantenimiento de la mercancía cuando se reciba del proveedor" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    *Justificación de la compra
                                </td>
                                <td align="left" colspan="3">
                                    <asp:TextBox ID="Txt_Justificacion" runat="server" MaxLength="1500" TabIndex="10" TextMode="MultiLine" Width="100%" Height="60px" ReadOnly="true"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="Txt_Justificacion" 
                                        WatermarkText="&lt;Límite de Caracteres 1500&gt;" WatermarkCssClass="watermarked">
                                    </cc1:TextBoxWatermarkExtender>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Justificacion" runat="server" TargetControlID="Txt_Justificacion" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr style="display:none;">
                                <td align="left">
                                    Especificaciones adicionales
                                </td>
                                <td align="left" colspan="3">
                                    <asp:TextBox ID="Txt_Especificaciones" runat="server" MaxLength="1500" TabIndex="10" TextMode="MultiLine" Width="100%" Height="35px"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="Txt_Especificaciones"
                                        WatermarkText="&lt;Límite de Caracteres 1500&gt;" WatermarkCssClass="watermarked">
                                    </cc1:TextBoxWatermarkExtender>                       
                                    <cc1:FilteredTextBoxExtender ID="FTE_Especificaciones" runat="server" TargetControlID="Txt_Especificaciones" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                    </cc1:FilteredTextBoxExtender>
                                </td>                                
                            </tr>
                            <div id="Div_Comentarios" runat="server">
                            <tr>
                                <td align="left">
                                    Comentarios
                                    <asp:LinkButton ID="Lnk_Observaciones" runat="server" OnClick="Lnk_Observaciones_Click" Visible="false">Mostrar</asp:LinkButton>
                                </td>
                                <td align="left" colspan="3">
                                    <asp:TextBox ID="Txt_Comentario" runat="server" MaxLength="1500" TabIndex="10" TextMode="MultiLine" Width="100%" Height="65px"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender 
                                        ID="Txt_Comentario_TextBoxWatermarkExtender" runat="server" 
                                        TargetControlID="Txt_Comentario"
                                        WatermarkText="&lt;Límite de Caracteres 1500&gt;" WatermarkCssClass="watermarked">
                                    </cc1:TextBoxWatermarkExtender>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Comentario" runat="server" TargetControlID="Txt_Comentario" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ.,;: ">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            </tr>
                            
                            
                                <tr>
                                    <td align="left" valign="top" colspan="4">
                                        <asp:GridView ID="Grid_Comentarios" runat="server" AllowPaging="true" AutoGenerateColumns="false" GridLines="None" PageSize="3" Width="100%" OnSelectedIndexChanged="Grid_Comentarios_SelectedIndexChanged"
                                            Style="font-size: xx-small; white-space:normal" OnPageIndexChanging="Grid_Comentarios_PageIndexChanging" >
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
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

    </div>    
</asp:Content>


<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Rpt_Com_Reporte_Requisiciones.aspx.cs" Inherits="paginas_Compras_Frm_Rpt_Com_Reporte_Requisiciones" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<script runat="server">  
   
</script>

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
        <%--Contenido del Formulario--%>
        <div id="Div_Contenido" style="width:99%;">
            <table border="0" cellspacing="0" class="estilo_fuente" width="100%">
                <tr>
                    <td colspan ="4" class="label_titulo">Reporte Requisicion</td>
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
                <%--Renglon de barra de Busqueda--%>
                <tr class="barra_busqueda">
                    <td style="width:20%">
                    
                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                            CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                            onclick="Btn_Salir_Click"/>
                        <asp:ImageButton ID="Btn_Exportar_PDF" runat="server" CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" 
                            ToolTip="Exportar PDF" onclick="Btn_Generar_Reporte_Click" AlternateText="Consultar"/>
                        <asp:ImageButton ID="Btn_Exportar_Excel" runat="server" CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                            ToolTip="Exportar Excel" onclick="Btn_Exportar_Excel_Click" AlternateText="Consultar"/>
                            
                    </td>
                    <td align="right" colspan="3" style="width:80%;">
                        <div id="Div_Busqueda" runat="server">
                            <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                                onclick="Btn_Avanzada_Click" ToolTip="Avanzada">Busqueda</asp:LinkButton>
                                &nbsp;&nbsp;
                            <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese un Folio>"
                                TargetControlID="Txt_Busqueda" />
                            <asp:ImageButton ID="Btn_Buscar" runat="server" AlternateText="Consultar"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Click"/>
                            
                        </div>
                    </td>
                </tr> 
                <tr>
                    <td colspan="4">
                       <%--Div Grid_Requisiciones--%>
                        <div id="Div_Grid_Requisiciones" runat="server" style="width:99%;">
                            <asp:GridView ID="Grid_Requisiciones" runat="server" AllowPaging="True" DataKeyNames="NO_REQUISICION"
                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                            PageSize="10" 
                            Width="100%" onselectedindexchanged="Grid_Requisiciones_SelectedIndexChanged"
                            OnPageIndexChanging="Grid_Requisiciones_PageIndexChanging">
                            <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                            HeaderText="Selecciona" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="NO_REQUISICION" HeaderText="NO_REQUISICION" Visible="False">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FOLIO" HeaderText="Requisicion" Visible="True">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FECHA_SURTIDO" HeaderText="Fecha Surtido" 
                            Visible="True">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TIPO" HeaderText="Tipo" Visible="True">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TIPO_ARTICULO" HeaderText="Tipo Articulo" 
                            Visible="True">
                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" Visible="True">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TOTAL_COTIZADO" HeaderText="Monto Total" Visible="True">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle HorizontalAlign="Right" Width="15%" />
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
                    <div ID="Div_Datos_Generales" runat="server" style="width:100%;font-size:9px;" visible="false">
                        <table style="width:100%;">
                            <tr>
                                <td align="center" colspan="4">
                                    Datos Generales</td>
                            </tr>
                            <tr align="right" class="barra_delgada">
                                <td align="center" colspan="4">
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Dependencia</td>
                                <td>
                                    <asp:TextBox ID="Txt_Dependencia" runat="server" Enabled="False" Width="250px"></asp:TextBox>
                                </td>
                                <td>
                                    Folio</td>
                                <td>
                                       <asp:TextBox ID="Txt_Folio" runat="server" Enabled="False" Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Area</td>
                                <td>
                                    <asp:TextBox ID="Txt_Area" runat="server" Enabled="False" Width="250px"></asp:TextBox>
                                </td>
                                <td>
                                    Estatus</td>
                                <td>
                                    <asp:TextBox ID="Txt_Estatus" runat="server" Enabled="False" 
                                        Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tipo</td>
                                <td>
                                    <asp:TextBox ID="Txt_Tipo" runat="server" Enabled="False" Width="250px"></asp:TextBox>
                                </td>
                                <td>
                                    Tipo Articulo
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Tipo_Articulo" runat="server" Enabled="false" 
                                        Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox ID="Chk_Verificacion" runat="server" Enabled="false" 
                                        Text="Verificar las características, garantías y pólizas" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Justificación
                                <br />
                                    de la Compra</td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Justificacion" runat="server" Enabled="False" 
                                           TabIndex="10" TextMode="MultiLine" Width="95%"></asp:TextBox>
                                       <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" 
                                           TargetControlID="Txt_Justificacion" WatermarkCssClass="watermarked" 
                                           WatermarkText="&lt;Indica el motivo de realizar la requisición&gt;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Especificaciones
                                    <br />
                                    Adicionales</td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Especificacion" runat="server" Enabled="False" 
                                    TabIndex="10" TextMode="MultiLine" Width="95%"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" 
                                        TargetControlID="Txt_Especificacion" WatermarkCssClass="watermarked" 
                                        WatermarkText="&lt;Especificaciones de los productos&gt;" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <cc1:TabContainer ID="TabContainer_Detalles_Requisicion" runat="server" 
                                        ActiveTabIndex="2" Width="98%">
                                        <cc1:TabPanel ID="TabPnl_Historial_Estatus" runat="server" Visible="true" Width="98%"><HeaderTemplate>Historial Estatus</HeaderTemplate><ContentTemplate><table style="width:98%"><tr><td style="width:15%"><b>Estatus</b> </td><td style="width:60%"><b>Empleado Asigno Estatus</b> </td><td style="width:25%"><b>Fecha</b> </td></tr><tr><td>Construción </td><td><asp:TextBox ID="Txt_Empleado_Construccion" runat="server" Width="97%" Enabled="false"></asp:TextBox></td><td><asp:TextBox ID="Txt_Fecha_Construccion" runat="server" Width="97%" Enabled="false"></asp:TextBox></td></tr><tr><td>Generación </td><td><asp:TextBox ID="Txt_Empleado_Genero" runat="server" Width="97%" Enabled="false"></asp:TextBox></td><td><asp:TextBox ID="Txt_Fecha_Genero" runat="server" Width="97%" Enabled="false"></asp:TextBox></td></tr><tr><td>Autorización </td><td><asp:TextBox ID="Txt_Empleado_Autorizo" runat="server" Width="97%" Enabled="false"></asp:TextBox></td><td><asp:TextBox ID="Txt_Fecha_Autorizo" runat="server" Width="97%" Enabled="false"></asp:TextBox></td></tr><tr><td>Filtrado </td><td><asp:TextBox ID="Txt_Empleado_Filtrado" runat="server" Width="97%" Enabled="false"></asp:TextBox></td><td><asp:TextBox ID="Txt_Fecha_Filtrado" runat="server" Width="97%" Enabled="false"></asp:TextBox></td></tr><tr><td>Cotización </td><td><asp:TextBox ID="Txt_Empleado_Cotizo" runat="server" Width="97%" Enabled="false"></asp:TextBox></td><td><asp:TextBox ID="Txt_Fecha_Cotizo" runat="server" Width="97%" Enabled="false"></asp:TextBox></td></tr><tr><td>Confirmación </td><td><asp:TextBox ID="Txt_Empleado_Confirmo" runat="server" Width="97%" Enabled="false"></asp:TextBox></td><td><asp:TextBox ID="Txt_Fecha_Confirmo" runat="server" Width="97%" Enabled="false"></asp:TextBox></td></tr><tr><td>Surtido </td><td><asp:TextBox ID="Txt_Empleado_Surtido" runat="server" Width="97%" Enabled="false"></asp:TextBox></td><td><asp:TextBox ID="Txt_Fecha_Surtido" runat="server" Width="97%" Enabled="false"></asp:TextBox></td></tr><tr><td>Distribución </td><td><asp:TextBox ID="Txt_Empleado_Distribucion" runat="server" Width="97%" Enabled="false"></asp:TextBox></td><td><asp:TextBox ID="Txt_Fecha_Distribucion" runat="server" Width="97%" Enabled="false"></asp:TextBox></td></tr></table></ContentTemplate></cc1:TabPanel>
                                        <cc1:TabPanel ID="TabPnl_Productos" runat="server" Visible="true" Width="98%"><HeaderTemplate>Detalle Productos</HeaderTemplate><ContentTemplate><table style="width:98%"><tr><td colspan="2"><div id="Div_Grid_Productos" runat="server" 
                                                        style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" 
                                                        visible="False"><asp:GridView ID="Grid_Productos" runat="server" AllowPaging="True" 
                                                        AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                                        Width="100%"><Columns><asp:BoundField DataField="Nombre_Prod_Serv" HeaderText="Producto/Servicio"><FooterStyle HorizontalAlign="Left" /><HeaderStyle HorizontalAlign="Left" /><ItemStyle HorizontalAlign="Left" /></asp:BoundField><asp:BoundField DataField="Cantidad" HeaderText="Cantidad"><FooterStyle HorizontalAlign="Left" /><HeaderStyle HorizontalAlign="Left" /><ItemStyle HorizontalAlign="Left" /></asp:BoundField><asp:BoundField DataField="Nombre_Proveedor" HeaderText="Proveedor"><FooterStyle HorizontalAlign="Left" /><HeaderStyle HorizontalAlign="Left" /><ItemStyle HorizontalAlign="Left" /></asp:BoundField><asp:BoundField DataField="Nombre_Cotizador" HeaderText="Cotizador"><FooterStyle HorizontalAlign="Left" /><HeaderStyle HorizontalAlign="Left" /><ItemStyle HorizontalAlign="Left" /></asp:BoundField><asp:BoundField DataField="Monto_Total" HeaderText="Primer Cotizacion"><FooterStyle HorizontalAlign="Left" /><HeaderStyle HorizontalAlign="Left" /><ItemStyle HorizontalAlign="Left" /></asp:BoundField><asp:BoundField DataField="Total_Cotizado" HeaderText="Cotizacion Final"><FooterStyle HorizontalAlign="Left" /><HeaderStyle HorizontalAlign="Left" /><ItemStyle HorizontalAlign="Left" /></asp:BoundField></Columns><PagerStyle CssClass="GridHeader" /><SelectedRowStyle CssClass="GridSelected" /><HeaderStyle CssClass="GridHeader" /><AlternatingRowStyle CssClass="GridAltItem" />
                                                    </asp:GridView></div></td></tr><tr><td align="right">Primera Cotizacion </td><td style="width:20%"><asp:TextBox ID="Txt_Total" runat="server" Width="98%" Enabled="False"></asp:TextBox></td></tr><tr><td align="right">Cotizacion Final </td><td style="width:20%"><asp:TextBox ID="Txt_Total_Cotizado" runat="server" Width="98%" Enabled="False"></asp:TextBox></td></tr></table></ContentTemplate></cc1:TabPanel>
                                        <cc1:TabPanel ID="TabPanel_Detalle_Compra" runat="server" Visible="true" Width="99%" Enabled="false"><HeaderTemplate>Detalle Compra</HeaderTemplate><ContentTemplate><table style="width:100%"><tr><td style="width:25%">Tipo Compra </td><td style="width:25%"><asp:TextBox 
                                            ID="Txt_Tipo_Compra" runat="server" Enabled="False"></asp:TextBox></td><td style="width:25%">Clave</td><td style="width:25%"><asp:TextBox 
                                            ID="Txt_Clave_Compra" runat="server" Enabled="False"></asp:TextBox></td></tr><tr><td>Requisicion Consolidada </td><td><asp:TextBox 
                                                ID="Txt_Requisicion_Consolidada" runat="server" Enabled="False"></asp:TextBox></td><td>Clave Consolidación </td><td><asp:TextBox 
                                                ID="Txt_Clave_Consolidacion" runat="server" Enabled="False"></asp:TextBox></td></tr></table></ContentTemplate></cc1:TabPanel>
                                        <cc1:TabPanel ID="TabPanel_Historial_Comentarios" runat="server" Visible="true" Width="99%" Enabled="false"><HeaderTemplate>Historial Comentarios</HeaderTemplate><ContentTemplate><table style="width:99%"><tr><td><div id="Div_Grid_Comentarios" runat="server" 
                                                        style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" 
                                                        visible="False"><asp:GridView ID="Grid_Comentarios" runat="server" AllowPaging="True" 
                                                        AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                                        Width="95%"><RowStyle CssClass="GridItem" /><Columns><asp:BoundField DataField="Comentario" HeaderText="Comentarios"><HeaderStyle HorizontalAlign="Left" Width="50%" /><ItemStyle HorizontalAlign="Left" Width="50%" /></asp:BoundField><asp:BoundField DataField="Estatus" HeaderText="Estatus"><HeaderStyle HorizontalAlign="Left" Width="15%" /><ItemStyle HorizontalAlign="Left" Width="15%" /></asp:BoundField><asp:BoundField DataField="Fecha_Creo" HeaderText="Fecha"><HeaderStyle HorizontalAlign="Left" Width="15%" /><ItemStyle HorizontalAlign="Left" Width="15%" /></asp:BoundField><asp:BoundField DataField="Usuario_Creo" HeaderText="Usuario"><HeaderStyle HorizontalAlign="Left" Width="15%" /><ItemStyle HorizontalAlign="Left" Width="15%" /></asp:BoundField>
                                                        </Columns><PagerStyle CssClass="GridHeader" /><SelectedRowStyle CssClass="GridSelected" /><HeaderStyle CssClass="GridHeader" /><AlternatingRowStyle CssClass="GridAltItem" />
                                                        </asp:GridView></div></td></tr></table></ContentTemplate></cc1:TabPanel>
                                    </cc1:TabContainer>
                                    
                                </td>
                            </tr>
                            <%--<tr>
                                <td colspan="2" align="center">
                                    <asp:Button ID="Btn_Generar_Reporte" runat="server" Text="Exportar PDF" 
                                        Width="200px" CssClass="button" onclick="Btn_Generar_Reporte_Click"/>
                                </td>
                                <td colspan="2" align="center">
                                    <asp:Button ID="Btn_Exportar_Excel" runat="server" Text="Exportar Excel" 
                                        Width="200px" CssClass="button" onclick="Btn_Exportar_Excel_Click"/>
                                </td>
                            </tr>--%>
                        </table>
                  </div>
                </tr>     
            </table>
        </div>   
</ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger  ControlID="Btn_Aceptar" EventName="Click"/>
    </Triggers>
</asp:UpdatePanel>

        <asp:UpdatePanel ID="UPnl_Busqueda" runat ="server" UpdateMode="Conditional" >
            <ContentTemplate>   
                <asp:Button ID="Button" runat="server" Text="Button" style="display:none;"/>
                <cc1:ModalPopupExtender ID="Modal_Busqueda" runat="server"
                    TargetControlID="Btn_Comodin_1"
                    PopupControlID="Pnl_Busqueda"                      
                    CancelControlID="Btn_Comodin_Close"
                    PopupDragHandleControlID="Pnl_Cabecera_Bus_Avanzada" 
                    DynamicServicePath="" 
                    DropShadow="True"
                    BackgroundCssClass="progressBackgroundFilter"/>
                    <asp:Button ID="Btn_Comodin_1" runat="server" Text="Button" style="display:none;" />   
                    <asp:Button ID="Btn_Comodin_Close" runat="server" Text="Button" style="display:none;" />
            </ContentTemplate>
        </asp:UpdatePanel>
        
        <%-- Panel del ModalPopUp display:none;--%>
           <asp:Panel ID="Pnl_Busqueda" runat="server" Width="60%" 
            style="border-style:outset;border-color:Silver;background-repeat:repeat-y;background-color:White;color:White;">
            <asp:Panel ID="Pnl_Cabecera_Bus_Avanzada" runat="server" 
                style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                <table width="99%">
                    <tr>
                        <td style="color:Black;font-size:12;font-weight:bold;">
                        <asp:Image ID="Image1" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                            Busqueda Avanzada
                        </td>
                        <td align="right">
                        <asp:ImageButton ID="ImageButton1" CausesValidation="false" runat="server" style="cursor:pointer;" 
                        ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClick="Btn_Cerrar_Click"/>  
                        </td>
                    </tr>
                </table>
            </asp:Panel>            
           <center>
           <asp:UpdatePanel ID="pnlPanel" runat="server">
           <ContentTemplate>
              <table width="100%" class="estilo_fuente">
              <tr>
                    <td colspan="4">
                        <asp:ImageButton ID="Img_Error_Busqueda" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                        Width="24px" Height="24px"/>
                        <asp:Label ID="Lbl_Error_Busqueda" runat="server" ForeColor="Red" Width="100%" />
                    </td>
              </tr>
              <tr>
                    <td colspan="4"></td>
              </tr>
              <tr>
                    <td align="left">
                        <asp:CheckBox ID="Chk_Fecha" runat="server" Text="Fecha" 
                        oncheckedchanged="Chk_Fecha_CheckedChanged" AutoPostBack="true"/>
                        &nbsp;&nbsp; &nbsp;De</td>
                    <td align="left">
                        <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="150px" 
                        Enabled="False"></asp:TextBox>
                        <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server" 
                            ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                            ToolTip="Seleccione la Fecha" Enabled="false" />
                        <cc1:CalendarExtender ID="Txt_Fecha_Inicial_CalendarExtender" runat="server" 
                            Format="dd/MMM/yyyy" OnClientShown="calendarShown" 
                            PopupButtonID="Btn_Fecha_Inicio" TargetControlID="Txt_Fecha_Inicial" />
                    </td>
                    <td align="left">
                        Al</td>
                    <td align="left">
                        <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="150px" Enabled="False"></asp:TextBox>
                        <asp:ImageButton ID="Btn_Fecha_Fin" runat="server" 
                            ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                            ToolTip="Seleccione la Fecha" Enabled="false" />
                        <cc1:CalendarExtender ID="Txt_Fecha_Final_CalendarExtender" runat="server" 
                            Format="dd/MMM/yyyy" OnClientShown="calendarShown" 
                            PopupButtonID="Btn_Fecha_Fin" TargetControlID="Txt_Fecha_Final" />
                        
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:CheckBox ID="Chk_Estatus" runat="server" Text="Estatus"/></td>
                    <td align="left" colspan="3">
                        <asp:DropDownList ID="Cmb_Estatus_Busqueda" runat="server" Width="355px" 
                        Enabled="False">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:CheckBox ID="Chk_Dependencia" runat="server" Text="Dependencia" AutoPostBack="true" 
                        oncheckedchanged="Chk_Dependencia_CheckedChanged" />
                    </td>
                    <td align="left" colspan="3">
                        <asp:DropDownList ID="Cmb_Dependencia" runat="server" Width="355px" AutoPostBack="true" 
                            Enabled="False" onselectedindexchanged="Cmb_Dependencia_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    
                </tr>
                <tr>
                    <td align="left">
                        <asp:CheckBox ID="Chk_Area" runat="server" Text="Area" AutoPostBack="true" 
                        oncheckedchanged="Chk_Area_CheckedChanged" />
                    </td>
                    <td align="left" colspan="3">
                        <asp:DropDownList ID="Cmb_Area" runat="server" Width="355px"  Enabled="False">
                        </asp:DropDownList>
                    </td>
                    
                </tr>
                <tr>
                    <td colspan="4">
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                    <center>
                        <asp:Button ID="Btn_Aceptar" runat="server" Text="Aceptar" Width="100px" 
                        onclick="Btn_Aceptar_Click" CssClass="button"/>
                        &nbsp;&nbsp;&nbsp;
                        </center>
                    </td>
                </tr>
                </table>
                </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger  ControlID="Btn_Busqueda_Avanzada" EventName="Click"/>            
          </Triggers>    
        </asp:UpdatePanel> 
        </center>
        </asp:Panel>    


</asp:Content>
<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Alm_Com_Administrar_Listado.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Alm_Com_Administrar_Listado" %>

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
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always" >
    <ContentTemplate>
     <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        
        <%-- Div General --%>
        <div id="Div_Contenido" style="width:100%;">
            <table border="0" cellspacing="0" class="estilo_fuente" width="99%">
                <tr>
                    <td colspan ="4" class="label_titulo">
                        Autorizar Listado de Almacen</td>
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
                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                        onclick="Btn_Salir_Click"/>
                    </td>
                    <td align="right" colspan="3" style="width:80%; ">
                        <div id="Div_Busqueda" runat="server">
                            <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                                onclick="Btn_Avanzada_Click" ToolTip="Avanzada" style="display:none">Busqueda</asp:LinkButton>
                                &nbsp;&nbsp;Busqueda
                            <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese un Folio>"
                                TargetControlID="Txt_Busqueda" />
                            <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                onclick="Btn_Buscar_Click" ToolTip="Consultar"/>
                        </div>
                    </td>
                </tr> 
                <tr>
                    <td colspan="4"> 
                        <%--Div Grid Listado--%>
                        <div id="Div_Grid_Listado" runat="server" style="width:100%;">
                        <asp:GridView ID="Grid_Listado" runat="server" 
                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" PageSize="10"
                            OnSelectedIndexChanged="Grid_Listado_SelectedIndexChanged" 
                            AllowSorting="True" OnSorting="Grid_Listado_Sorting" HeaderStyle-CssClass="tblHead"
                            Width="100%">
                            <Columns>
                                <asp:ButtonField ButtonType="Image" CommandName="Select" Text="Ver"
                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                    <ItemStyle Width="5%" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="True" SortExpression="Folio">
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small" />
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
                                <asp:BoundField DataField="Total" HeaderText="Total" Visible="True" SortExpression="Total">
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                    <ItemStyle HorizontalAlign="Right" Width="20%" Font-Size="X-Small"/>
                                </asp:BoundField>
                            </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                        </asp:GridView>
                        </div> <%-- Fin de Div Grid Listado--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="4"> 
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                    <%--Div Datos_Generales--%>
                        <div id="Div_Datos_Generales" runat="server" style="width:100%;">
                        <table  border="0" width="100%" cellspacing="0" class="estilo_fuente"> 
                            <tr >
                                <td colspan="4"></td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">Datos Generales</td>
                            </tr>
                            <tr align="right" class="barra_delgada">
                                <td colspan="4" align="center"></td>
                            </tr>
                            <tr>
                                <td style="width:10%;">Folio</td>
                                <td style="width:40%;">
                                    <asp:TextBox ID="Txt_Folio" runat="server" Width="97%"></asp:TextBox>
                                </td>
                                <td style="width:10%;">Fecha</td>
                                <td style="width:40%;">
                                    <asp:TextBox ID="Txt_Fecha" runat="server" Width="97%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Tipo</td>
                                <td>
                                    <asp:TextBox ID="Txt_Tipo" runat="server" Width="97%"></asp:TextBox>
                                </td>
                            </tr>
                            
                            <tr>
                                <td colspan="4"></td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">Productos</td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                <div id="Div1" runat="server" style="overflow:auto;height:400px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;">
                                    <asp:GridView ID="Grid_Productos" runat="server" 
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                            PageSize="5" Width="100%" Enabled ="false"
                                            AllowSorting="True" OnSorting="Grid_Productos_Sorting" HeaderStyle-CssClass="tblHead">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:BoundField DataField="Producto_ID" HeaderText="Producto_ID" Visible="false">
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
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion"
                                                    Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="20%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad"
                                                    Visible="True">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Existencia" HeaderText="Existencia" Visible="false" SortExpression="Existencia">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Reorden" HeaderText="Punto de Reorden" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad Solicitada" Visible="true" SortExpression="Cantidad">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" Font-Size="X-Small"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Precio_Unitario" HeaderText="Precio Unitario" SortExpression="Precio_Unitario"
                                                    Visible="True">
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
                                                    <ItemStyle HorizontalAlign="Right"  Font-Size="X-Small"/>
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
                                    </div>
                                </td>  
                            </tr>
                            <tr>
                                <td colspan="4" align="right">Total
                                    <asp:TextBox ID="Txt_Total" runat="server" Width="150px" style="text-align:right"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    Observaciones
                                </td>
                            </tr>
                            <tr align="right" class="barra_delgada">
                                <td colspan="4" align="center"></td>
                            </tr>
                            <tr>
                                <td align="right" colspan="4">
                                    <asp:ImageButton ID="Btn_Alta_Observacion" runat="server" Height="24px" 
                                    ImageUrl="../imagenes/paginas/sias_add.png" 
                                    OnClick="Btn_Alta_Observacion_Click" ToolTip="Nuevo" Width="24px" />
                                    <asp:ImageButton ID="Btn_Cancelar_Observacion" runat="server" Height="24px" 
                                    ImageUrl="../imagenes/paginas/icono_cancelar.png" 
                                    OnClick="Btn_Cancelar_Observacion_Click" Width="24px" ToolTip="Cancelar"/>
                                </td>
                            </tr>
                            <tr>
                                <td > Estatus</td>
                                <td >
                                    <asp:DropDownList ID="Cmb_Estatus" runat="server" Enabled="False" Width="97%">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Comentario&nbsp;
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Comentario" runat="server" TabIndex="10" MaxLength="250"
                                    TextMode="MultiLine" Width="95%"></asp:TextBox>
                                    
                                    <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                    runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    TargetControlID="Txt_Comentario" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/ ">
                                    </cc1:FilteredTextBoxExtender>
                                    
                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkCssClass="watermarked"
                                    WatermarkText="<Límite de Caracteres 250>" 
                                    TargetControlID="Txt_Comentario" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <asp:GridView ID="Grid_Comentarios" runat="server" AllowPaging="true" 
                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                    OnSelectedIndexChanged="Grid_Comentarios_SelectedIndexChanged" PageSize="3"
                                    onpageindexchanging="Grid_Comentarios_PageIndexChanging"
                                    AllowSorting="True" OnSorting="Grid_Comentarios_Sorting" HeaderStyle-CssClass="tblHead"
                                    Width="98%">
                                    <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" Text="Ver"
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Comentario" HeaderText="Comentario" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fecha_Creo" HeaderText="Fecha" Visible="True" SortExpression="Fecha_Creo">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Usuario_Creo" HeaderText="Usuario" Visible="True" SortExpression="Usuario_Creo">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" Font-Size="X-Small"/>
                                    </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                
                                </td>
                            </tr>
                        </table><%-- Fin tabla de Datos_Generales--%>
                        </div> <%-- Fin Div Datos_Generales--%>
                    </td>
                </tr>
            </table> <%-- Fin de Tabla general--%>
        </div> <%--  Fin del Div General --%>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger  ControlID="Btn_Aceptar" EventName="Click"/>
        </Triggers>   
        </asp:UpdatePanel>
        
        <asp:UpdatePanel ID="UPnl_Busqueda" runat ="server" UpdateMode="Conditional" >
            <ContentTemplate>      
                <cc1:ModalPopupExtender ID="Modal_Busqueda" runat="server"
                    TargetControlID="Btn_Comodin"
                    PopupControlID="Pnl_Busqueda"                      
                    CancelControlID="Btn_Comodin_Close"
                    PopupDragHandleControlID="Pnl_Cabecera_Bus_Avanzada" 
                    DynamicServicePath="" 
                    DropShadow="True"
                    BackgroundCssClass="progressBackgroundFilter"/>
                <asp:Button ID="Btn_Comodin" runat="server" Text="Button" style="display:none;"/>
                <asp:Button ID="Btn_Comodin_Close" runat="server" Text="Button" style="display:none;"/>
            </ContentTemplate>
        </asp:UpdatePanel>
        
         <%-- Panel del ModalPopUp display:none;--%>
            <asp:Panel ID="Pnl_Busqueda" runat="server" Width="100%" 
             style="border-style:outset;border-color:Silver;background-repeat:repeat-y;background-color:White;color:White;display:none;">
            <asp:Panel ID="Pnl_Cabecera_Bus_Avanzada" runat="server" 
                style="background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                <table width="99%">
                    <tr>
                        <td style="color:Black;font-size:12;font-weight:bold;">
                        <asp:Image ID="Image1" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                            Busqueda Avanzada
                        </td>
                        <td align="right" style="width:10%;">
                        <asp:ImageButton ID="ImageButton1" CausesValidation="false" runat="server" ToolTip="Cerrar Ventana"
                        ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClick="Btn_Cerrar_Click"/>  
                        </td>
                    </tr>
                </table>
            </asp:Panel>
           <center>
           <asp:UpdatePanel ID="pnlPanel" runat="server">
           <ContentTemplate>
           
              <table class="estilo_fuente" width="100%">
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
                    <td align="left" style="width:20%;">
                        <asp:CheckBox ID="Chk_Fecha" runat="server" Text="Fecha" 
                        oncheckedchanged="Chk_Fecha_CheckedChanged" AutoPostBack="true"/>
                        &nbsp;De</td>
                    <td align="left">
                        <asp:TextBox ID="Txt_Fecha_Inicial" runat="server" Width="150px" 
                        Enabled="False"></asp:TextBox>
                        <cc1:CalendarExtender ID="Txt_Fecha_Inicial_CalendarExtender" runat="server" OnClientShown="calendarShown"
                        TargetControlID="Txt_Fecha_Inicial" Format ="dd/MMM/yyyy">
                        </cc1:CalendarExtender>
                    </td>
                    <td align="left">
                        Al</td>
                    <td align="left">
                        <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="150px" Enabled="False"></asp:TextBox>
                        <cc1:CalendarExtender ID="Txt_Fecha_Final_CalendarExtender" runat="server" OnClientShown="calendarShown"
                        TargetControlID="Txt_Fecha_Final" Format ="dd/MMM/yyyy">
                        </cc1:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:CheckBox ID="Chk_Estatus" runat="server" Text="Estatus" AutoPostBack="true"
                        oncheckedchanged="Chk_Estatus_CheckedChanged"/></td>
                    <td align="left" colspan="3">
                        <asp:DropDownList ID="Cmb_Estatus_Busqueda" runat="server" Width="355px" 
                        Enabled="False" >
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        &nbsp;</td>
                    <td align="left" colspan="3">
                        <br />
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
        
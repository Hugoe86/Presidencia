<%@ Page Language="C#"  MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Com_Licitaciones.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Com_Licitaciones" %>

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
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
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
                <td colspan ="4" class="label_titulo">Licitaciones</td>
            </tr>
             <%--Fila de div de Mensaje de Error --%>
            <tr>
                <td colspan ="4">
                    <div id="Div_Contenedor_Msj_Error" style="width:100%;font-size:9px;" runat="server" visible="false">
                    <table style="width:100%;">
                    <tr>
                        <td align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                        <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                        Width="24px" Height="24px"/>
                        </td>            
                        <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />
                        </td>
                        <td>
                        <asp:ImageButton ID="Btn_Cerrar_Mensaje" runat="server" 
                            ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" 
                            ToolTip="Cerrar Mensaje" OnClick="Btn_Cerrar_Mensaje_Click"/>
                        </td>
                    </tr> 
                    </table>                   
                    </div>
                </td>
            </tr>
            <%--Fila 3 Renglon de barra de Busqueda--%>
            <tr class="barra_busqueda">
                <td style="width:20%;">
                        <asp:ImageButton ID="Btn_Nuevo" runat="server" CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                            ToolTip="Nuevo" onclick="Btn_Nuevo_Click" />
                        <asp:ImageButton ID="Btn_Modificar" runat="server" 
                            ToolTip="Modificar" CssClass="Img_Button"
                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                            onclick="Btn_Modificar_Click" />
                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                        OnClick="Btn_Salir_Click"/>
                </td>
                <td align="right" colspan="3" style="width:80%;">
                        <div id="Div_Busqueda" runat="server">
                        <%--<asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                        onclick="Btn_Avanzada_Click" ToolTip="Busqueda Avanzada">Busqueda</asp:LinkButton>--%>
                        Busqueda&nbsp;&nbsp;
                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese un Folio>"
                                TargetControlID="Txt_Busqueda" />
                        <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Consultar"
                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                        onclick="Btn_Buscar_Click" />
                        </div>
                        <%--<asp:Button ID="Btn_Comodin_2" runat="server" Text="Button" style="display:none;"/>
                        <cc1:ModalPopupExtender ID="Modal_Busqueda" runat="server"
                        TargetControlID="Btn_Comodin_2"
                        PopupControlID="Pnl_Busqueda"                      
                        CancelControlID="Btn_Cancelar"
                        DropShadow="True"
                        BackgroundCssClass="progressBackgroundFilter"/>--%>
                </td> 
            </tr>
            <tr>
                <td colspan="4">
                    <div id="Div_Licitaciones" runat="server" style="width:100%;">
                        <asp:GridView ID="Grid_Licitaciones" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" CssClass="GridView_1" DataKeyNames="No_Licitacion" 
                            GridLines="None" 
                            OnSelectedIndexChanged="Grid_Licitaciones_SelectedIndexChanged">
                            <Columns>
                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                    <ItemStyle Width="5%" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="No_Licitacion" HeaderText="No_Licitacion" 
                                    Visible="false">
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Folio" HeaderText="Folio" Visible="True">
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True">
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Fecha_Inicio" HeaderText="Fecha de Inicio" 
                                    Visible="True">
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Fecha_Fin" HeaderText="Fecha de Termino" 
                                    Visible="True">
                                    <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
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
            <%-- Div Contenido General de Licitaciones --%>
            <tr>
                <td colspan="4">
                    <%-- Tabla de Licitaciones --%>
                    <div id="Div_Datos" runat="server">
                    <tabla style="width:98%;">
                        <tr>
                            <td colspan="4" align="center">
                            Datos Generales
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Estatus</td>
                            <td>
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" AutoPostBack="true" 
                                    Width="100%">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Fecha Fin
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Fecha_Fin" runat="server" Width="85%"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Fecha_Fin" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                    ToolTip="Seleccione la Fecha" />
                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" 
                                    Format="dd/MMM/yyyy" OnClientShown="calendarShown" 
                                    PopupButtonID="Btn_Fecha_Fin" TargetControlID="Txt_Fecha_Fin" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%;">
                                Folio
                                </td>                            
                            <td style="width:35%;">
                                <asp:TextBox ID="Txt_Folio" runat="server" Enabled="false" Width="98%"></asp:TextBox>
                                </td>
                            <td style="width:15%;">
                                Fecha Inicio
                            </td>
                            <td style="width:35%;">
                                <asp:TextBox ID="Txt_Fecha_Inicio" runat="server" Width="85%"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Fecha_Inicio" runat="server" 
                                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                                    ToolTip="Seleccione la Fecha" />
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" 
                                    Format="dd/MMM/yyyy" OnClientShown="calendarShown" 
                                    PopupButtonID="Btn_Fecha_Inicio" TargetControlID="Txt_Fecha_Inicio" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Tipo
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Tipo" runat="server" Width="100%"></asp:DropDownList>
                            </td>
                            <td>
                                Clasificacion
                            </td>
                            <td>
                                <asp:DropDownList ID="Cmb_Clasificacion" runat="server" Width="100%"></asp:DropDownList>
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                Justificacion
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Justificacion" runat="server" TabIndex="10" 
                                    TextMode="MultiLine" Width="100%"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" 
                                    TargetControlID="Txt_Justificacion" WatermarkCssClass="watermarked" 
                                    WatermarkText="&lt;Indica el motivo de realizar la requisición&gt;" />
                            </td>
                        </tr>
                            <tr>
                                <td>
                                    Comentarios
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="Txt_Comentario" runat="server" MaxLength="250" TabIndex="10" 
                                        TextMode="MultiLine" Width="100%"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
                                        TargetControlID="Txt_Comentario" WatermarkCssClass="watermarked" 
                                        WatermarkText="&lt;Límite de Caracteres 250&gt;" />
                                </td>
                            </tr>
                                <tr>
                                    <td class="barra_delgada" colspan="4">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4">
                                        Agregar Requisiciones
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <cc1:TabContainer ID="Tab_Requisiciones_Consolidaciones" runat="server" 
                                            ActiveTabIndex="0" Width="98%"><cc1:TabPanel 
                                            ID="TabPnl_Buscar_Requisiciones" runat="server" Visible="true" Width="99%">
                                            <HeaderTemplate>Agregar Requisiciones/Consolidaciones</HeaderTemplate>
                                        <ContentTemplate>
                                        <table style="width:99%">
                                            <tr>
                                                <td>Seleccione una opción:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:CheckBox ID="Chk_Requisiciones" runat="server" AutoPostBack="True"
                                                        Text="Agregar Requisiciones a Licitacion" 
                                                        oncheckedchanged="Chk_Requisiciones_CheckedChanged">
                                                    </asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:CheckBox ID="Chk_Consolidaciones" runat="server" AutoPostBack="True"
                                                        Text="Agregar Consolidaciones a Licitacion" 
                                                        oncheckedchanged="Chk_Consolidaciones_CheckedChanged">
                                                    </asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                 <div id="Div_Requisiciones_Busqueda" runat="server" 
                                                        style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" 
                                                        visible="False">
                                                 <asp:GridView ID="Grid_Requisiciones_Busqueda" runat="server" AllowPaging="True" 
                                                         AutoGenerateColumns="False" CssClass="GridView_1" DataKeyNames="No_Requisicion" 
                                                         GridLines="None" Width="96%">
                                                    <Columns>
                                                    <asp:TemplateField HeaderText="Selecciona">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:CheckBox ID="Chk_Requisicion_Seleccionada" runat="server" AutoPostBack="True"/>
                                                            </center>
                                                        </ItemTemplate>
                                                        <controlstyle width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="No_Requisicion" HeaderText="ID">
                                                            <FooterStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Folio" HeaderText="Folio">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Dependencia" HeaderText="Dependencia">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Area" HeaderText="Area">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Total" HeaderText="Total">
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                     </Columns>  
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    
                                                     </asp:GridView>  
                                                  </div>
                                                  <div id="Div_Consolidaciones_Busqueda" runat="server" 
                                                        style="overflow:auto;height:200px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" 
                                                        visible="False">
                                                    <asp:GridView ID="Grid_Consolidaciones_Busqueda" runat="server" AllowPaging="True" 
                                                         AutoGenerateColumns="False" CssClass="GridView_1" DataKeyNames="No_Consolidacion" 
                                                         GridLines="None" Width="96%">
                                                    <Columns>                       
                                                    <asp:TemplateField HeaderText="Selecciona">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:CheckBox ID="Chk_Consolidacion_Seleccionada" runat="server" AutoPostBack="True"/>
                                                            </center>
                                                        </ItemTemplate>
                                                        <controlstyle width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="No_Consolidacion" HeaderText="ID">
                                                            <FooterStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Folio" HeaderText="Folio">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Total" HeaderText="Total">
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    
                                                        </Columns><AlternatingRowStyle CssClass="GridAltItem" />
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    
                                                      </asp:GridView>
                                                   </div>
                                            </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4">
                                                <asp:Button ID="Btn_Agregar_Requisicion_Consolidaciones" runat="server" 
                                                Text="Agregar Consolidacion/Requisicion" Width="250px" 
                                                onclick="Btn_Agregar_Requisicion_Consolidaciones_Click" CssClass="button"/>
                                                </td>
                                            </tr>
                                        </table>
                                        
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel ID="TabPnl_Requisiciones" runat="server" Visible="true">
                                            <HeaderTemplate>Requisiciones y Consolidaciones</HeaderTemplate>
                                        <ContentTemplate>
                                        <table style="width:99%">
                                        <tr>
                                            <td align="center">
                                                Requisiciones
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="Grid_Requisiciones" runat="server" AllowPaging="True"  PageSize="5"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" DataKeyNames="No_Requisicion" 
                                                    GridLines="None" onpageindexchanging="Grid_Requisiciones_PageIndexChanging" 
                                                    onselectedindexchanged="Grid_Requisiciones_SelectedIndexChanged" Width="99%">
                                                <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/paginas/delete.png">
                                                    <ItemStyle Width="5%" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="No_Requisicion" HeaderText="No_Requisicion" 
                                                        Visible="False">
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Folio" HeaderText="Folio">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Dependencia" HeaderText="Dependencia">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Area" HeaderText="Area">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Total" HeaderText="Total">
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                
                                                    </Columns>
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                </asp:GridView>  
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="barra_delgada">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                Consolidaciones
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="Grid_Consolidaciones" runat="server" AllowPaging="True" 
                                                    AutoGenerateColumns="False" CssClass="GridView_1" DataKeyNames="No_Consolidacion" 
                                                    onpageindexchanging="Grid_Consolidaciones_PageIndexChanging" 
                                                    onselectedindexchanged="Grid_Consolidaciones_SelectedIndexChanged"
                                                    GridLines="None" Width="99%" PageSize="5">
                                                    <Columns>                       
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/paginas/delete.png">
                                                    <ItemStyle Width="5%" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="No_Consolidacion" HeaderText="No_Consolidacion" 
                                                            Visible="False">
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Folio" HeaderText="Folio">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                                                        <FooterStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Total" HeaderText="Total">
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Lista_Requisiciones" HeaderText="Consolidacion" 
                                                            Visible="False">
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    </Columns><AlternatingRowStyle CssClass="GridAltItem" />
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" colspan="4">
                                                Total&nbsp;
                                                <asp:TextBox ID="Txt_Total" runat="server" Enabled="False" 
                                                style="text-align:right" Width="200px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        </table>
                                        
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        </cc1:TabContainer>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        </td>
                                    <td colspan="3">
                                       </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                    </td>
                                </tr>
                               
                    </tabla>
                    </div>
                </td>
            </tr>
            
            </table><%--fin del table contenedor principal  --%>
           </div> <%--Fin del div contenido--%>
        </ContentTemplate>
        <Triggers>
            
        </Triggers>
    </asp:UpdatePanel>
    
    <%-- Panel del ModalPopUp Busqueda Avanzada--%>
    <asp:Panel ID="Pnl_Busqueda" runat="server" Width="60%" 
        style="border-style:outset;border-color:Silver;background-repeat:repeat-y;background-color:White;color:White;display:none;">
        <center>
        <asp:UpdatePanel ID="pnlPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <table class="estilo_fuente" width="100%">
              <tr>
                    <td colspan="4">
                        <asp:Label ID="Lbl_Error_Busqueda" runat="server" ForeColor="Red" Width="100%" />
                    </td>
              </tr>
              <tr>
                    <td colspan="4" class="barra_busqueda" align="center"> Busqueda Avanzada</td>
              </tr>
              <tr>
                    <td colspan="4"></td>
              </tr>
              <tr>
                    <td align="left" style="width:20%;">
                        Fecha&nbsp;De</td>
                    <td align="left">
                        <asp:TextBox ID="Txt_Fecha_Avanzada_1" runat="server" Width="150px" 
                        Enabled="true"></asp:TextBox>
                        <asp:ImageButton ID="Btn_Fecha_Avanzada_1" runat="server" 
                            ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                            ToolTip="Seleccione la Fecha" />
                        <cc1:CalendarExtender ID="Txt_Fecha_Avanzada_1_CalendarExtender" runat="server" OnClientShown="calendarShown"
                        TargetControlID="Txt_Fecha_Avanzada_1" Format ="dd/MMM/yyyy" PopupButtonID="Btn_Fecha_Avanzada_1">
                        </cc1:CalendarExtender>
                    </td>
                    <td align="left">
                        Al</td>
                    <td align="left">
                        <asp:TextBox ID="Txt_Fecha_Avanzada_2" runat="server" Width="150px" Enabled="true"></asp:TextBox>
                        <asp:ImageButton ID="Btn_Fecha_Avanzada_2" runat="server" 
                            ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                            ToolTip="Seleccione la Fecha" />
                        <cc1:CalendarExtender ID="Txt_Fecha_Avanzada_2_CalendarExtender" runat="server" OnClientShown="calendarShown"
                        TargetControlID="Txt_Fecha_Avanzada_2" Format ="dd/MMM/yyyy" PopupButtonID="Btn_Fecha_Avanzada_2">
                        </cc1:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Estatus</td>
                    <td align="left" colspan="3">
                        <asp:DropDownList ID="Cmb_Estatus_Busqueda" runat="server" Width="355px" 
                        Enabled="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                    <center>
                        <asp:Button ID="Btn_Aceptar" runat="server" Text="Aceptar" Width="100px" 
                        onclick="Btn_Aceptar_Click" CssClass="button"/>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Btn_Cancelar" runat="server" Text="Cancelar" Width="100px" 
                        CssClass="button"/>
                    </center>
                    </td>
                </tr>
                </table>
                </ContentTemplate>
        <%--  <Triggers>
            <asp:AsyncPostBackTrigger  ControlID="Btn_Busqueda_Avanzada" EventName="Click"/>
          </Triggers>  --%>  
        </asp:UpdatePanel> 
        </center>
        </asp:Panel>

</asp:Content>
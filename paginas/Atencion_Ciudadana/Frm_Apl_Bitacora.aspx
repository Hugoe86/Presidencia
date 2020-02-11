<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Apl_Bitacora.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Apl_Bitacora" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
            <%--Fila 1--%>
            <tr> 
                <td colspan ="7" class="label_titulo"> Bitácora de Eventos</td>
            </tr>
            <%--Fila 2--%>
            <%--Fila de div de Mensaje de Error --%>
            <tr>
                <td colspan ="7">
                          <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;" runat="server" visible="false">
                                <table style="width:100%;">
                                    <tr>
                                        <td colspan="2" align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                                        <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                        Width="24px" Height="24px"/>
                                        </td>            
                                    </tr>
                                    <tr>
                                        <td style="width:10%;">              
                                        </td>            
                                        <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />
                                        </td>
                                    </tr>          
                                   </table>                   
                          </div>
                 </td>
            </tr>
            <%--Fila 3 
            Renglon de barra de Busqueda--%>
            <tr class="barra_busqueda" align="left">
                <td colspan ="7">
                    &nbsp;
                    <asp:ImageButton ID="Btn_Reporte" runat="server" CssClass ="Img_Button"
                        ImageUrl="~/paginas/imagenes/paginas/report.png" ToolTip="Generar Reporte" 
                        onclick="Btn_Reporte_Click" />
                    <asp:ImageButton ID="Btn_Limpiar" runat="server" CssClass ="Img_Button"
                        ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" 
                        ToolTip="Limpiar Formulario" onclick="Btn_Limpiar_Click"  />
                    <asp:ImageButton ID="Btn_Salir" runat="server" CssClass ="Img_Button"
                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" 
                        onclick="Btn_Salir_Click" />
                </td>
            </tr>
            <%--Fila 4 --%>
            <tr>
                <td colspan ="7" style="height:350px;">
                    <cc1:TabContainer ID="Tab_Container" runat="server" ActiveTabIndex="0" 
                        Width="99%" Height="330px">
                    <%--pestaña 1 vertical-align:top;--%>
                    <cc1:TabPanel ID="Tab_Catalogo" runat="server" HeaderText="TabPanel1" style="height:330px;vertical-align:top;">
                        <HeaderTemplate>Paginas</HeaderTemplate>
                        <ContentTemplate>
                        <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                        <tr>
                            <td colspan = "2">
                            </td>
                        </tr>
                        <tr>
                            <td style ="width:12%" >Mostrar </td>
                            <td>
                            <asp:CheckBox ID="Chk_Altas" runat="server" text="Altas"  />
                            &#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; 
                            <asp:CheckBox ID="Chk_Modificar" runat="server" Text="Modificaciones" />
                            <br />
                            <asp:CheckBox ID="Chk_Bajas" runat="server" Text = "Bajas"/>
                            &#160;&#160;&#160;&#160;&#160;&#160;&#160; 
                            <asp:CheckBox ID="Chk_Consultas" runat="server" Text = "Consultas"/>
                            </td>
                        </tr>
                        <tr align="right" class="barra_delgada">
                            <td colspan="2" align="center">
                            </td>
                        </tr>   
                        <tr>
                            <td colspan = "2">
                            <asp:CheckBox ID="Chk_Todos_Catalogos" Text= "Todos los Catalogos" runat="server" 
                                    AutoPostBack="True" OnCheckedChanged="Chk_Todos_Catalogos_CheckedChanged" />
                            </td>
                        </tr>
                        <tr style="height:200px;vertical-align:top;clear:both">
                            <td colspan = "2">
                            <div style="overflow:auto;height:202px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;" >
                            <asp:GridView ID="Grid_Catalogos" runat="server" Width="95%" Height="200px" CssClass="GridView_1" GridLines="None" AutoGenerateColumns="False">
                                <Columns>
                                <asp:TemplateField HeaderText="Selecciona" >
                                    <ItemTemplate>
                                        <center>
                                        <asp:CheckBox ID="Chk_catalogo" runat="server" AutoPostBack="True" oncheckedchanged="Chk_catalogo_CheckedChanged"/>
                                        </center>
                                    </ItemTemplate>
                                <ControlStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="MENU_DESCRIPCION" HeaderText="Pagina">
                                    <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                    <ItemStyle HorizontalAlign="Left" Width="50%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CLASIFICACION" HeaderText="Clasificacion">
                                    <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                    <ItemStyle HorizontalAlign="Left" Width="35%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PAGINA">
                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                    <ItemStyle HorizontalAlign="Left" Width="0%" Font-Size = "0pt" ForeColor ="White"/>
                                </asp:BoundField>
                                </Columns>
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <HeaderStyle CssClass="GridHeader" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                            </asp:GridView>
                            </div>
                            </td>
                        </tr>
                         </table>
                         </ContentTemplate>
                         </cc1:TabPanel>  
                    <%--pestaña 2 --%>
                    <cc1:TabPanel ID="Tab_Otros" runat="server" HeaderText="TabPanel2" style="height:200px;">
                    <HeaderTemplate>Otros</HeaderTemplate>
                    <ContentTemplate>
                    <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            <asp:CheckBox ID="Chk_Impresiones" runat="server" Text ="Impresiones" /><br />
                            <asp:CheckBox ID="Chk_Accesos" runat="server" Text = "Accesos al Sistema"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        </table>
                    </ContentTemplate>
                    </cc1:TabPanel>
                    </cc1:TabContainer>
                    </td>
            </tr>
         
            <tr>
                <td colspan = "7">
                </td>
            </tr>
            <%--Fila 5 --%>
            <tr>
                <td ></td>
                <td>
                    <asp:CheckBox ID="Chk_Usuario" runat="server" Text = "Usuario" 
                        AutoPostBack="True" oncheckedchanged="Chk_Usuario_CheckedChanged" /></td>
                <td ></td>    
                <td colspan = "3">
                    <asp:TextBox ID="Txt_Usuario" runat="server" Width="443px" Enabled="False" >
                    </asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <%--Fila 6 --%>
            <tr>
                <td></td>
                <td> 
                    Fecha</td>
                <td align = "right" > De</td>
                <td style="width:230px"> 
                    <asp:TextBox ID="Txt_Fecha_inicial" runat="server" Width="200px"></asp:TextBox>
                    <asp:ImageButton ID="Btn_Fecha_1" runat="server" 
                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                    ToolTip="Seleccione la Fecha" />
                    <cc1:CalendarExtender ID="CalendarExtender_inicial" runat="server"  TargetControlID="Txt_Fecha_inicial"
                    Format ="dd/MMM/yyyy" PopupButtonID="Btn_Fecha_1">
                    </cc1:CalendarExtender>
                    
                    </td>
                <td> Al</td>
                <td>
                    <asp:TextBox ID="Txt_Fecha_final" runat="server" Width="200px"></asp:TextBox>
                    <asp:ImageButton ID="Btn_Fecha_2" runat="server" 
                    ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" 
                    ToolTip="Seleccione la Fecha" />
                    <cc1:CalendarExtender ID="CalendarExtender_final" runat="server" TargetControlID="Txt_Fecha_final" 
                    Format ="dd/MMM/yyyy" PopupButtonID="Btn_Fecha_2">
                    </cc1:CalendarExtender> 
                   
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                </td>
                <td><cc1:MaskedEditExtender 
                        ID="Mee_Txt_Fecha_inicial" 
                        Mask="99/LLL/9999" 
                        runat="server"
                        MaskType="None" 
                        UserDateFormat="DayMonthYear" 
                        UserTimeFormat="None" Filtered="/"
                        TargetControlID="Txt_Fecha_inicial" 
                        Enabled="True" 
                        ClearMaskOnLostFocus="false"/>  
                    <cc1:MaskedEditValidator 
                        ID="Mev_Txt_Fecha_inicial" 
                        runat="server" 
                        ControlToValidate="Txt_Fecha_inicial"
                        ControlExtender="Mee_Txt_Fecha_inicial" 
                        EmptyValueMessage="Fecha Requerida"
                        InvalidValueMessage="Fecha Invalida" 
                        IsValidEmpty="false" 
                        TooltipMessage="Ingrese o Seleccione la Fecha de Inicio"
                        Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/></td>
                <td></td>
                <td> <cc1:MaskedEditExtender 
                        ID="Mee_Txt_Fecha_final" 
                        Mask="99/LLL/9999" 
                        runat="server"
                        MaskType="None" 
                        UserDateFormat="DayMonthYear" 
                        UserTimeFormat="None" Filtered="/"
                        TargetControlID="Txt_Fecha_final" 
                        Enabled="True" 
                        ClearMaskOnLostFocus="false"/>  
                    <cc1:MaskedEditValidator 
                        ID="Mev_Txt_Fecha_final" 
                        runat="server" 
                        ControlToValidate="Txt_Fecha_final"
                        ControlExtender="Mee_Txt_Fecha_final" 
                        EmptyValueMessage="Fecha Requerida"
                        InvalidValueMessage="Fecha Invalida" 
                        IsValidEmpty="false" 
                        TooltipMessage="Ingrese o Seleccione la Fecha Final"
                        Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/></td>
            </tr>
            <%--Fila 7 --%>
            <tr>
                <td > </td>
                <td colspan = "2" > Ordenar reporte por</td>
                <td colspan = "4">
                    <asp:CheckBox ID="Chk_Ordenar_Usuario" runat="server" Text = "Usuario"/>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox ID="Chk_Ordenar_Fecha" runat="server" Text = "Fecha"/>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox ID="Chk_Ordenar_Accion" runat="server" Text = "Accion"/>
                </td>
            </tr>
          </table>


        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

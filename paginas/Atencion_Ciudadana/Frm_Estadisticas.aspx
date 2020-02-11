<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Estadisticas.aspx.cs" Inherits="paginas_Atencion_Ciudadana_Frm_Estadisticas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">       
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <asp:UpdatePanel ID="Upd_Panel" runat="server">    
     <ContentTemplate>
           <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressTemplate"></div>
                <div  class="processMessage" id="div_progress">
                    <img alt="" src="../Imagenes/paginas/Updating.gif" />
                </div>
            </ProgressTemplate>
            </asp:UpdateProgress>
         <div id="Estadisticas" style="background-color:#ffffff; width:100%;">
          
            <table width="97%"  border="0" cellspacing="0" class="estilo_fuente">
                <%--Fila 1 --%>
                <tr> 
                    <td class="label_titulo" colspan = "6"> Estadisticas</td>
                </tr>
                <%--Fila 2 --%>
                <%--Fila de div de Mensaje de Error --%>
                <tr>
                    <td colspan = "6">
                        <div id="Div_Contenedor_Msj_Error" style="width:98%;font-size:9px;" runat="server" visible="false">
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
                <%--Fila 3 Renglon de barra de Busqueda--%>
                <tr class="barra_busqueda" align="right">
                    <td colspan = "6" align="left">
                        <asp:ImageButton ID="Btn_Graficar" runat="server" ToolTip="Graficar" 
                            CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_graficar.png" 
                            onclick="Btn_Graficar_Click"/>
                        <asp:ImageButton ID="Btn_Limpiar" runat="server" ToolTip="Limpiar formulario" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png"
                            onclick="Btn_Limpiar_Click"/>
                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                            onclick="Btn_Salir_Click"/>
                                                                       
                    </td>
                </tr>
                <%--Fila 4 --%>
                <tr>
                    <td colspan = "6">
                        <cc1:TabContainer ID="TabContainer1" runat="server" Width = "100%" 
                            ActiveTabIndex="2" AutoPostBack ="false" >
                            <cc1:TabPanel ID = "Tab_General" runat = "server">
                            <HeaderTemplate>Graficas Generales</HeaderTemplate>
                            <ContentTemplate>
                                <table width = "100%">
                                    <tr>
                                        <td colspan = "2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="Chk_Estatus_Global" runat="server" Text = "Grafica de Peticiones por Estatus"/>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan = "2">
                                        </td>
                                    </tr>
                                    </table>
                            </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID ="Tab_Estatus" runat="server" HeaderText="TabPanel1"><HeaderTemplate>
                                Grafica por Estatus</HeaderTemplate><ContentTemplate><table width="97%">
                            
                            <tr>
                            <td>Dependencias </td>
                            </tr>
                            <tr>
                            <td> <asp:CheckBox ID="Chk_Todas_Dependencias_Estatus" runat="server" 
                                    Text = "Todos" AutoPostBack = "True" 
                                    oncheckedchanged="Chk_Todas_Dependencias_Estatus_CheckedChanged"/></td>
                            </tr>
                            <tr>                                
                            <td>
                            <table width = "99%">
                            <tr>
                                <td class="barra_delgada">
                                    </td>
                            </tr>
                            <tr>
                            <td>
                            <div style="overflow:auto; width:100%; height:150px">
                                <asp:GridView ID="Grid_Dependencias_Estatus" runat="server" Width="95%" 
                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None">
                                    <Columns>
                                       <asp:TemplateField HeaderText="Selecciona">
                                           <ItemTemplate>
                                                 <center>
                                                 <asp:CheckBox ID="Chk_Dependencias_Estatus" runat="server" AutoPostBack="True"
                                                    oncheckedchanged="Chk_Dependencias_Estatus_CheckedChanged" />
                                                 </center>
                                           </ItemTemplate>
                                           <ControlStyle Width="5%" />
                                       </asp:TemplateField>
                                       <asp:BoundField DataField="DEPENDENCIA_ID" HeaderText="Clave">
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                       </asp:BoundField> 
                                       <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                            <HeaderStyle HorizontalAlign="Left" Width="80%" />
                                            <ItemStyle HorizontalAlign="Left" Width="80%" />
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
                                   <td class="barra_delgada">
                                       </td>
                                   </tr>
                                   </table>
                                   </td>
                               </tr>
                               <tr>
                               <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                               </tr>
                               <tr>
                               <td>
                               <asp:CheckBox ID="Chk_Areas" runat="server" Text="Areas" 
                               oncheckedchanged="Chk_Areas_CheckedChanged" AutoPostBack="True" />
                               </td>
                               </tr>
                               
                               <tr>
                               <td>
                               <div id="Div_Areas" runat="server" visible="False" > 
                               
                               <table width = "99%">
                               <tr>
                                   <td class="barra_delgada">
                                       </td>
                                   </tr>
                               <tr>
                               <td>
                               <asp:CheckBox ID="Chk_Todos_Areas" runat="server" Text="Todas las Areas" 
                                AutoPostBack="True" oncheckedchanged="Chk_Todos_Areas_CheckedChanged" />
                                   &nbsp;</td>
                               </tr>
                                    <tr>
                                    <td class="barra_delgada">
                                        </td>
                                    </tr>
                                    <tr>
                                    <td>
                                    <div style="overflow:auto; width:100%; height:150px">
                                    <asp:GridView ID="Grid_Areas" runat="server" Width="95%" 
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Selecciona">
                                        <ItemTemplate>
                                            <center>
                                            <asp:CheckBox ID="Chk_Areas_Estatus" runat="server" AutoPostBack="True"
                                                oncheckedchanged="Chk_Areas_Estatus_CheckedChanged"/>
                                            </center>
                                        </ItemTemplate>
                                        <ControlStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="AREA_ID" HeaderText="Clave">
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField> 
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                            <HeaderStyle HorizontalAlign="Left" Width="80%" />
                                            <ItemStyle HorizontalAlign="Left" Width="80%" />
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
                                    <td class="barra_delgada">
                                        </td>
                                    </tr>
                               </table>
                               
                               </div>
                               </td>
                               </tr>
                               
                               </table>
                            </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID = "Tab_Tiempos" runat="server" HeaderText="TabPanel2"><HeaderTemplate>
                                Grafica por Tiempos</HeaderTemplate>
                            <ContentTemplate>
                            <table width = "100%">
                            
                            <tr>
                            <td colspan = "6">
                                Dependencias
                                </td>
                            </tr>
                            <tr>
                            <td colspan = "6">
                                <asp:CheckBox ID="Chk_Todos_Tiempo" runat="server" Text = "Todos" AutoPostBack="True"
                                    oncheckedchanged="Chk_Todos_Tiempo_CheckedChanged"/>
                                </td>
                            </tr>
                            <tr>
                            <td colspan = "6">
                            
                            <table width = "99%">
                            <tr><td colspan = "2" class="barra_delgada"></td>
                            </tr>
                            <tr>
                            <td>
                            <div style="overflow:auto; width:100%; height:150px"  >
                                <asp:GridView ID="Grid_Dependencias_Tiempos" runat="server" 
                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                    Width="95%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Selecciona">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:CheckBox ID="Chk_Dependencias_Tiempos" runat="server" 
                                                        AutoPostBack="True" OnCheckedChanged = "Chk_Dependencias_Tiempos_CheckedChanged" />
                                                </center>
                                            </ItemTemplate>
                                            <ControlStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DEPENDENCIA_ID" HeaderText="Clave">
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                            <HeaderStyle HorizontalAlign="Left" Width="80%" />
                                            <ItemStyle HorizontalAlign="Left" Width="80%" />
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
                                <tr><td colspan = "2" class="barra_delgada"></td></tr>
                                </table>
                                </td>
                            </tr>
                            
                                <tr><td>
                                <asp:CheckBox ID="Chk_Asuntos_Tiempos" runat="server" Text = "Asuntos" 
                                    AutoPostBack="True" OnCheckedChanged="Chk_Asuntos_CheckedChanged"/>
                                </td></tr></table></ContentTemplate></cc1:TabPanel>
                            
                            
                        </cc1:TabContainer>
                    </td>        
                </tr>
                <tr>
                    <td colspan = "6">
                    </td>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    <td align="right">
                        De
                    </td>
                    <td style="width:183px;">
                        <asp:TextBox ID="Txt_Fecha_Inicia" runat="server" Width="176px"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender1" 
                        runat="server" TargetControlID = "Txt_Fecha_Inicia" Format ="dd/MMM/yyyy">
                        </cc1:CalendarExtender>
                    </td>
                    <td >
                        Al</td>
                    <td>
                        <asp:TextBox ID="Txt_Fecha_Final" runat="server" Width="176px"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID = "Txt_Fecha_Final" Format ="dd/MMM/yyyy">
                        </cc1:CalendarExtender>
                    </td>
                    <td>
                        
                    </td>
                 </tr>
                 <tr>
                    <td colspan = "6">
                        
                    </td>
                 </tr>
                 <tr>
                    <td colspan = "6" align = "center">
<%--                        <asp:Button ID="Btn_Graficar" runat="server" CausesValidation="False" 
                            Text="Generar Grafica" CssClass="button" Width="150px" 
                            onclick="Btn_Graficar_Click"/>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Btn_Limpiar" runat="server" CausesValidation="False" 
                            Text="Limpiar Formulario" onclick="Btn_Limpiar_Click" CssClass="button" 
                            Width="150px"  />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Btn_Salir" runat="server" CausesValidation="False" 
                            Text="Salir" onclick="Btn_Salir_Click" CssClass="button" 
                            Width="150px"/>                   --%> 
                    </td>                    
                 </tr>
                 <tr style="height:280px;">
                    <td colspan = "6" align = "center">
                    </td>                    
                 </tr>                 
            </table>
          </div>                   
     </ContentTemplate> 
   </asp:UpdatePanel>
</asp:Content>
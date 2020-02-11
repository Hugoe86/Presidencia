<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Ope_Nom_Actualizar_Salario.aspx.cs" Inherits="paginas_Nomina_Ope_Nom_Actualizar_Salario" Title="Actualizar Salario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Actualizar_Salario" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpPnl_Actualizar_Salario" runat="server">
        <ContentTemplate>
            <%--<asp:UpdateProgress ID="Uprg_Actualizar_Salario" runat="server" AssociatedUpdatePanelID="UpPnl_Actualizar_Salario" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                     
            </asp:UpdateProgress>--%>
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td>
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                    <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>  
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top">
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>      
                            </table>                   
                          </div>                
                        </td>
                    </tr>
                </table>   
                <br />   
                <table style="width:100%;">
                    <tr>
                        <td style="width:100%;" align="center">
                            <div id="Contenedor_Titulo" style="background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;">
                                <table width="100%">
                                    <tr>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td width="100%">
                                            <font style="color: Black; font-weight: bold;">Actualizar Salario</font>
                                        </td>    
                                    </tr>  
                                    <tr>
                                        <td></td>
                                    </tr>                                      
                                </table>    
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="Pnl_Selección" runat="server" GroupingText="Selección" Width="98%">   
                    <table style="width:100%;">
                        <tr>
                            <td width="20%">
                                <asp:Label ID="Lbl_Tipo_Nomina" runat="server" Text="* Tipo Nomina"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="Cmb_Tipo_Nomina" runat="server" Width="98%" 
                                    AutoPostBack="True" 
                                    onselectedindexchanged="Cmb_Tipo_Nomina_SelectedIndexChanged">
                                    <asp:ListItem>&lt;-- SELECCIONE --&gt;</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%" style="text-align:left;">
                               <asp:Label ID="Lbl_Nomina" runat="server" Text="* Nomina"></asp:Label>
                            </td>
                            <td width="30%" style="text-align:left;">
                                <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Calendario_Nomina_SelectedIndexChanged">
                                    <asp:ListItem>&lt;-- SELECCIONE --&gt;</asp:ListItem>
                                </asp:DropDownList>
                            </td>             
                            <td width="20%" style="text-align:left;">
                                <asp:Label ID="Lbl_Periodo" runat="server" Text="* Periodo"></asp:Label>
                            </td>
                            <td width="30%" style="text-align:left;">
                                <asp:DropDownList ID="Cmb_Periodos_Catorcenales_Nomina" runat="server" Width="95%" Enabled="false">
                                    <asp:ListItem>&lt;-- SELECCIONE --&gt;</asp:ListItem>
                                </asp:DropDownList>
                            </td>                                                                                        
                        </tr>                     
                        <tr>
                            <td width="20%">
                                <asp:Label ID="Lbl_Sindicato" runat="server" Text="Sindicato"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="Cmb_Sindicato" runat="server" Width="98%">
                                    <asp:ListItem>&lt;-- NINGUNO --&gt;</asp:ListItem>
                                </asp:DropDownList>
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2" width="50%">&nbsp;</td>
                            <td colspan="2" style="text-align:center;" >
                                <asp:Button ID="Btn_Agregar_Sindicato" runat="server" Text="Agregar" 
                                    Width="45%" CssClass="button_autorizar" onclick="Btn_Agregar_Sindicato_Click" style="background-color:#A9D0F5; color:Black; border-style:outset;" />
                                <asp:Button ID="Btn_Limpiar_Sindicatos" runat="server" Text="Limpiar Listado" 
                                    CssClass="button_autorizar" onclick="Btn_Limpiar_Sindicatos_Click" Width="45%" style="background-color:#A9D0F5; color:Black; border-style:outset;" />
                            </td>
                        </tr>
                    </table>
                 </asp:Panel>    
                <br />
                <asp:Panel ID="Pnl_Listado_Sindicatos" runat="server" GroupingText="Listado de Sindicatos" Width="98%">   
                    <table style="width:100%;">
                        <tr>
                            <td style="text-align:center;">         
                                <asp:GridView ID="Grid_Listado_Sindicatos" runat="server" CssClass="GridView_1"
                                    AutoGenerateColumns="False" AllowPaging="True" PageSize="5" Width="99%"
                                    GridLines= "None" 
                                    onpageindexchanging="Grid_Listado_Sindicatos_PageIndexChanging" 
                                    onrowdatabound="Grid_Listado_Sindicatos_RowDataBound">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:BoundField DataField="SINDICATO_ID" HeaderText="Sindicato ID" SortExpression="SINDICATO_ID" >
                                            <ItemStyle Width="90px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE" />
                                        <asp:TemplateField HeaderText="Quitar">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Btn_Quitar_Sindicato" runat="server" 
                                                    AlternateText="Quitar Sindicato de Listado" 
                                                    ImageUrl="~/paginas/imagenes/paginas/quitar.png" 
                                                    onclick="Btn_Quitar_Sindicato_Click" />
                                            </ItemTemplate>
                                            <ItemStyle Width="30px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />                                
                                    <AlternatingRowStyle CssClass="GridAltItem" />       
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                 </asp:Panel>   
                 <br />      
                <table  width="98%">    
                    <tr>
                        <td colspan="2" style="text-align:center;" width="50%">
                            <asp:Button ID="Btn_Actualizar_Salario" runat="server" 
                                Text="Actualizar" Width="98%" CssClass="button_autorizar" 
                                onclick="Btn_Actualizar_Salario_Click" />
                        </td>
                        <td colspan="2" style="text-align:center;" width="50%">
                            <asp:Button ID="Btn_Salir" runat="server" Text="Salir" 
                                CssClass="button_autorizar" onclick="Btn_Salir_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>    
    </asp:UpdatePanel>
</asp:Content>


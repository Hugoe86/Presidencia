<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Cajeros.aspx.cs" Inherits="paginas_Predial_Frm_Cat_Pre_Cajeros" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Cajeros" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            </asp:UpdateProgress>
            <div id="General" style="background-color:#ffffff; width:100%; height:100%;">
                <table id="Tbl_Comandos" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                            <tr>
                                <td class="label_titulo" colspan="2">
                                    Catálogo de Cajeros</td>
                            </tr>
                            
                            <tr>
                                <div id="Div_Contenedor_Msj_Error" runat="server">
                                <td colspan="2">
                                    <asp:Image ID="Img_Error" runat="server" ImageUrl = "../imagenes/paginas/sias_warning.png"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                    <br />
                                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" TabIndex="0" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                                    
                                </td>
                                </div>
                            </tr>
                            <tr class="barra_busqueda">
                                <td style="width:50%">
                                    <asp:ImageButton ID="Btn_Guardar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_actualizar.png"
                                    AlternateText="Guardar" CssClass="Img_Button" onclick="Btn_Guardar_Click"/>
                                    <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                    AlternateText="Salir" CssClass="Img_Button" onclick="Btn_Salir_Click"/>
                                </td>
                                <td align="right" style="width:50%">
                                    Búsqueda:
                                    <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"
                                    ToolTip="Buscar" TabIndex="1" ></asp:TextBox>
                                    <asp:ImageButton ID="Btn_Busqueda" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                        OnClick ="Btn_Busqueda_Click"
                                        TabIndex="2"/>
                                        <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                        WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda"/>
                                </td>                        
                            </tr>
                        </table>
                 <cc1:TabContainer ID="Tab_Contenedor_Parametros" runat="server" Width="98%" 
                    ActiveTabIndex="1">
                        <%--TAB PARA LOS CAJEROS--%>
                        <cc1:TabPanel ID="Tbp_Cajeros" runat="server" HeaderText="Tbp_Cajeros" Width="100%"
                        ActiveTabIndex="1" >
                        <HeaderTemplate>
                        Cajeros
                        </HeaderTemplate>
                        <ContentTemplate>
                        <center>
                        <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                        <tr>
                        <td style="width:18%">&nbsp;</td>
                        <td style="width:32%">&nbsp;</td>
                        <td style="width:18%">&nbsp;</td>
                        <td style="width:27%">&nbsp;</td>
                        </tr>
                        <tr>
                        <td style="width:18%">Empleado ID</td>
                        <td style="width:32%"><asp:TextBox ID="Txt_Empleado_ID1" runat="server" Width="94%" ReadOnly="True"></asp:TextBox></td>
                        <td style="width:18%"></td>
                        <td style="width:27%"></td>
                        </tr>
                        <tr>
                        <td style="width:18%">Nombre</td>
                        <td colspan="3"><asp:TextBox ID="Txt_Cajero" runat="server" Width="97.65%" ReadOnly="True"></asp:TextBox></td>
                        </tr>
                        <tr>
                        <td style="width:18%">Estatus</td>
                        <td style="width:32%"><asp:TextBox ID="Txt_Estatus" runat="server" Width="94%" ReadOnly="True"></asp:TextBox></td>
                        <td style="width:18%">Tipo</td>
                        <td style="width:27%"><asp:TextBox ID="Txt_Tipo" runat="server" Width="94%" ReadOnly="True"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width:18%">
                                </td>
                                <td style="width:32%">
                                </td>
                                <td style="width:18%">
                                </td>
                                <td style="width:27%">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                <asp:GridView ID="Grid_Cajeros" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                onpageindexchanging="Grid_Cajeros_PageIndexChanging"
                                onselectedindexchanged="Grid_Cajeros_SelectedIndexChanged" 
                                GridLines="None"                                 
                                PageSize="5" 
                                Style="white-space:normal" Width="96%">
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                <ItemStyle Width="5%" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="NO_EMPLEADO" HeaderText="Empleado ID">
                                <HeaderStyle HorizontalAlign="Left" Width="20%" />

                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NOMBRE_EMPLEADO" HeaderText="Nombre">
                                <HeaderStyle HorizontalAlign="Left" Width="35%" />

                                <ItemStyle HorizontalAlign="Left" Width="35%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                <HeaderStyle HorizontalAlign="Left" Width="20%" />

                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NOMBRE" HeaderText="Tipo">
                                <HeaderStyle HorizontalAlign="Left" Width="20%" />

                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                
                                    </Columns>

                                <HeaderStyle CssClass="GridHeader" />

                                <PagerStyle CssClass="GridHeader" />

                                <RowStyle CssClass="GridItem" />

                                <SelectedRowStyle CssClass="GridSelected" />
                                
                                    </asp:GridView>

                                </td>
                            </tr>
                            <tr>
                                <td style="width:18%">
                                    &nbsp;</td>
                                <td style="width:32%">
                                    &nbsp;</td>
                                <td style="width:18%">
                                    &nbsp;</td>
                                <td style="width:32%">
                                    &nbsp;</td>
                            </tr>
                        </table>
                        </center>                        
                        </ContentTemplate>
                        
                        </cc1:TabPanel>
                        <%--TAB PARA LA ASIGNACION DE TURNOS Y CAJEROS--%>
                        <cc1:TabPanel ID="Tbp_Asignacion" runat="server" HeaderText="Tbp_Asignacion" Width="100%"
                        ActiveTabIndex = "2">
                        <HeaderTemplate>
                        Asignacion de Turnos
                        </HeaderTemplate>
                        
                        <ContentTemplate>
                        <center>
                        <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                        <tr>
                        <td style="width:18%"></td>
                        <td style="width:32%"></td>
                        <td style="width:18%"></td>
                        <td style="width:32%"></td>
                        </tr>
                        <tr>
                        <td colspan="4" class="button_autorizar">Asignar Caja y Turno</td>
                        </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp;</td>
                            </tr>
                        <tr>
                        <td style="width:18%">Empleado ID</td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Empleado_ID2" runat="server" Width="91%" 
                                ReadOnly = "True" ></asp:TextBox>
                            </td>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Empleado_ID" runat="server" Width="91%" visible ="False" ></asp:TextBox>
                        </td>
                        </tr>
                            <tr>
                                <td style="width:18%">Modulo</td>
                                <td style="width:32%">
                                    <asp:DropDownList ID="Cmb_Modulo" runat="server" Width="94%"
                                    AutoPostBack = "True" 
                                    OnSelectedIndexChanged = "Cmb_Modulo_SelectedIndexChanged">
                                    </asp:DropDownList></td>
                                <td style="width:18%">Caja</td>
                                <td style="width:32%">
                                    <asp:DropDownList ID="Cmb_Caja" runat="server" Width="95%">
                                    
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:18%">
                                    Turno</td>
                                <td style="width:32%">
                                    <asp:DropDownList ID="Cmb_Turno" runat="server" Width="94%">
                                    </asp:DropDownList>
                                </td>
                                <td style="width:18%">
                                </td>
                                <td style="width:32%">
                                </td>
                            </tr>
                            <tr>
                                <td style="width:18%">
                                </td>
                                <td style="width:32%">
                                </td>
                                <td style="width:18%">
                                </td>
                                <td style="width:32%;" align=right >
                                    <asp:ImageButton ID="Btn_Agregar" runat="server" CssClass="Img_Button" 
                                        ImageUrl="~/paginas/imagenes/gridview/add_grid.png" 
                                        OnClick="Btn_Agregar_Click" />
                                </td>
                                <td style="width:5%">
                                    &nbsp;</td>
                                    
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                <asp:GridView ID="Grid_Asignacion_Turnos" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                GridLines="None"                                 
                                PageSize="5" 
                                OnSelectedIndexChanged="Grid_Asignacion_Turnos_SelectedIndexChanged"
                                OnPageIndexChanging= "Grid_Asignacion_Turnos_PageIndexChanging"
                                Style="white-space:normal" Width="96%">
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                
                                <asp:BoundField DataField="EMPLEADO_ID">
                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                               
                                <asp:BoundField DataField="NO_EMPLEADO" HeaderText="Empleado ID">
                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                
                                <asp:BoundField DataField="MODULO" HeaderText="Modulo">
                                <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                <ItemStyle HorizontalAlign="Left" Width="35%" />
                                </asp:BoundField>
                                
                                <asp:BoundField DataField="CAJA_ID">
                                <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                <ItemStyle HorizontalAlign="Left" Width="35%" />
                                </asp:BoundField>
                                
                                <asp:BoundField DataField="CAJA" HeaderText="Caja">
                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                
                                <asp:BoundField DataField="TURNO_ID">
                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                
                                <asp:BoundField DataField="TURNO" HeaderText="Turno">
                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                </asp:BoundField>
                                
                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png">
                                <ItemStyle Width="5%" />
                                </asp:ButtonField> 
                                
                                </Columns>

                                <HeaderStyle CssClass="GridHeader" />

                                <PagerStyle CssClass="GridHeader" />

                                <RowStyle CssClass="GridItem" />

                                <SelectedRowStyle CssClass="GridSelected" />
                                
                                    </asp:GridView>

                                </td>
                            </tr>
                            <tr>
                                <td style="width:18%">
                                    &nbsp;</td>
                                <td style="width:32%">
                                    &nbsp;</td>
                                <td style="width:18%">
                                    &nbsp;</td>
                                <td style="width:32%">
                                    &nbsp;</td>
                            </tr>
                            
                           
                            
                        </table>
                        </center>
                        
                        
                        </ContentTemplate>
                        
                        </cc1:TabPanel>
                       </cc1:TabContainer>                
            </div>
                        
            </ContentTemplate>                        
    </asp:UpdatePanel>
</asp:Content>
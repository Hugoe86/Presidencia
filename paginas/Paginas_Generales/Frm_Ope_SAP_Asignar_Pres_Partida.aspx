<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_SAP_Asignar_Pres_Partida.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Ope_SAP_Asignar_Pres_Partida" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
    
        <ContentTemplate>
        <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressTemplate">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>
        </asp:UpdateProgress>
                
            <div id="Div_SAP_Pres_Proy_Progr_Busqueda" style="background-color:#ffffff; width:100%; height:100%;">                
                <table id="Tbl_Comandos" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">                
                    <tr>
                        <td colspan="4" class="label_titulo">
                            Asignaci&oacute;n de Presupuesto a Partidas
                        </td>                        
                    </tr>
                    
                    <tr>
                        <td colspan="4">
                            <asp:Image ID="Img_Error" runat="server" ImageUrl = "../imagenes/paginas/sias_warning.png"/>
                            <br />
                            <asp:Label ID="Lbl_Error" runat="server" ForeColor="Red" Text="" TabIndex="0"></asp:Label>
                        </td> 
                    </tr>
                    
                    <tr class="barra_busqueda">
                        <td colspan="2" style="width:50%">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                            CssClass="Img_Button" onclick="Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                            CssClass="Img_Button" onclick="Btn_Modificar_Click"/>                            
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                            CssClass="Img_Button" onclick="Btn_Salir_Click"/>
                        </td>
                        <td colspan="2" align="right" style="width:50%">
                            Búsqueda
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"
                            ToolTip="Buscar" TabIndex="1" ></asp:TextBox>
                            <asp:ImageButton ID="Btn_Busqueda" runat="server" ToolTip="Consultar"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                TabIndex="2" onclick="Btn_Busqueda_Click"/>
                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                        </td>                        
                    </tr>
                    
                </table>
            </div>
            
            <div id="Div_SAP_Pres_Proy_Progr" runat="server" style="background-color:#ffffff; width:100%; height:100%;">        
               <table id="Datos Generales_Inner" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                    
                    <tr>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Anio" runat="server" Text="*Año del Presupuesto"></asp:Label>&nbsp;
                        </td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Anio" runat="server" MaxLength="4" Width="90%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" 
                                FilterType="Numbers"
                                TargetControlID="Txt_Anio" ValidChars="0123456789">
                                 </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="width:18%">
                            &nbsp;
                        </td>
                        <td style="width:32%">
                            &nbsp;
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Nombre" runat="server" Text="*Proyecto"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="Cmb_Proyectos" runat="server" Width="92%" 
                                onselectedindexchanged="Cmb_Proyectos_SelectedIndexChanged" 
                                AutoPostBack="True">
                            </asp:DropDownList>
                            </td>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Partida" runat="server" Text="*Partidas"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="Cmb_Partidas" runat="server" Width="92%" 
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    
                    <tr>                        
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Presupuesto" runat="server" Text="*Monto Presupuestal"></asp:Label>
                        </td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Presupuesto" runat="server" Width="90%" 
                                AutoPostBack="True" ontextchanged="Txt_Presupuesto_TextChanged"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                                FilterType="Custom, Numbers"
                                TargetControlID="Txt_Presupuesto" ValidChars="0123456789.">
                                 </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="width:18%">
                        <asp:Label ID="Lbl_Monto_Ejercido" runat="server" Text="*Monto Ejercido"></asp:Label>
                        </td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Monto_Ejercido" runat="server" MaxLength="10" Width="90%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                FilterType="Numbers,Custom" 
                                TargetControlID="Txt_Monto_Ejercido" ValidChars="0123456789.">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Monto_Disponible" runat="server" Text="*Monto Disponible"></asp:Label>
                            </td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Monto_Disponible" runat="server" MaxLength="10" Width="90%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                                FilterType="Numbers,Custom" 
                                TargetControlID="Txt_Monto_Disponible" ValidChars="0123456789.">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Monto_Comprometido" runat="server" Text="*Monto Comprometido"></asp:Label>                            
                            </td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Monto_Comprometido" runat="server" MaxLength="10" Width="90%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" 
                                FilterType="Numbers,Custom" 
                                TargetControlID="Txt_Monto_Comprometido" ValidChars="0123456789.">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    
               </table>
                    
            </div>
                    
            <div id="Div_Grid_Programas_Proyectos" runat="server" style="background-color:#ffffff; width:100%; height:100%;">
                    <table id="Tbl_Grid_Proyectos" border="0" cellspacing="0" class="estilo_fuente" style="width:98%;">                        
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:GridView ID="Grid_Proyectos_Programas" runat="server" AllowPaging="true" 
                                        AutoGenerateColumns="False" CssClass="GridView_1" 
                                        EmptyDataText="&quot;No se encontraron registros&quot;" GridLines="none"                                        
                                        PageSize="5" Style="white-space:normal" Width="96%" 
                                        onpageindexchanging="Grid_Proyectos_Programas_PageIndexChanging" 
                                        onselectedindexchanged="Grid_Proyectos_Programas_SelectedIndexChanged">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                <ItemStyle Width="3%" />
                                            </asp:ButtonField>                                            
                                            <asp:BoundField DataField="PROYECTO_NOMBRE" 
                                                HeaderText="Proyecto" SortExpression="PROYECTO_NOMBRE">
                                                <ItemStyle Width="30%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ANIO_PRESUPUESTO"
                                                HeaderText="Año del Presupuesto" SortExpression="ANIO_PRESUPUESTO">
                                                <ItemStyle Width="7%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PARTIDA_NOMBRE" HeaderText="Partida" SortExpression="PARTIDA_NOMBRE">
                                                <ItemStyle Width="30%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MONTO_PRESUPUESTAL" HeaderText="Presupuesto" SortExpression="MONTO_PRESUPUESTAL">
                                                <ItemStyle Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MONTO_DISPONIBLE" HeaderText="Disponible" 
                                                SortExpression="MONTO_DISPONIBLE">
                                                <ItemStyle Width="10%" />
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
    
</asp:Content>


<%@ Page Title="Catálogo Cotizadores" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Com_Cotizadores.aspx.cs" Inherits="paginas_Compras_Frm_Cat_Com_Cotizadores" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
    
        <ContentTemplate>
        
        <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <%--<div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>--%></ProgressTemplate>
            </asp:UpdateProgress>
        
                <div id="Div_Cat_Com_Cotizadores_Botones" style="background-color:#ffffff; width:100%; height:100%;">                
                        <table id="Tbl_Comandos" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">                        
                            <tr>
                                <td colspan="4" class="label_titulo">
                                    Cat&aacute;logo de Cotizadores
                                </td>
                            </tr>
                            
                            <tr>
                                <div id="Div_Contenedor_error" runat="server">
                                <td colspan="4">
                                    <asp:Image ID="Img_Error" runat="server" ImageUrl = "../imagenes/paginas/sias_warning.png"/>
                                    <br />
                                    <asp:Label ID="Lbl_Error" runat="server" ForeColor="Red" Text="" TabIndex="0"></asp:Label>
                                </td>
                                </div>
                            </tr>
                            
                            <tr class="barra_busqueda">
                                <td colspan="2" style="width:50%">
                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                    CssClass="Img_Button" onclick="Btn_Nuevo_Click"/>
                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                    CssClass="Img_Button" onclick="Btn_Modificar_Click"/>
                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                    CssClass="Img_Button"                                         
                                        OnClientClick="return confirm('¿Esta seguro de Eliminar el presente registro?');" onclick="Btn_Eliminar_Click" 
                                        />
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
                                        WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda"/>
                                </td>                        
                            </tr>
                            
                        </table>
                    </div>

            <div id="Div_Datos_Cotizador" runat="server">
                 
                <div id="Div_Cat_Com_Cotizadores_Controles" runat="server" style="background-color:#ffffff; width:100%; height:100%;">        
                       <table id="Datos Generales_Inner" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                            
                            <tr>
                                <td style="width:18%">
                                    &nbsp;
                                </td>
                                <td style="width:32%">
                                    &nbsp;
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
                                    <asp:Label ID="Lbl_Empleado_ID" runat="server" Text="No. Empleado"></asp:Label></td>
                                <td style="width:32%">
                                    <asp:TextBox ID="Txt_No_Empleado" runat="server" Width="90%" Enabled="true"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" 
                                    TargetControlID="Txt_No_Empleado" 
                                    FilterType="Custom" ValidChars="0,1,2,3,4,5,6,7,8,9"
                                    Enabled="True" InvalidChars="<,>,&,',!,">
                                    </cc1:FilteredTextBoxExtender>
                                    <%-- <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" WatermarkCssClass="watermarked"
                                    WatermarkText="<No Empleado>" TargetControlID="Txt_No_Empleado" />--%>
                                    
                                </td>
                                <td style="width:18%">
                                        <asp:ImageButton ID="Btn_Buscar_Empleado" runat="server" ToolTip="Consultar"
                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Empleado_Click" 
                                        />
                                        </td>
                                <td style="width:32%">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width:18%">
                                    <asp:Label ID="Lbl_Nombre" runat="server" Text="Nombre"></asp:Label>
                                </td>
                                <td style="width:32%" colspan="2">
                                    <asp:TextBox ID="Txt_Nombre_Cotizador" runat="server" Width="99%" ReadOnly="true"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                        runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                        TargetControlID="Txt_Nombre_Cotizador" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:18%">
                                    
                                    <asp:Label ID="Lbl_Correo" runat="server" Text="Correo"></asp:Label>
                                    
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Correo" runat="server" Width="99%"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td style="width:18%">
                                    
                                    <asp:Label ID="Lbl_Password" runat="server" Text="Password"></asp:Label>
                                    
                                </td>
                                 <td>
                                    <asp:TextBox ID="Txt_Password" runat="server" Width="99%" TextMode="Password"></asp:TextBox>
                                </td>
                               
                            </tr>
                            <tr>
                                <td style="width:18%">
                                    <asp:Label ID="Lbl_Direccion_IP" runat="server" Text="Direccion IP Correo"></asp:Label>
                                </td>
                                
                                 <td>
                                    <asp:TextBox ID="Txt_Direccion_IP" runat="server" Width="99%" ></asp:TextBox>
                                      <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                      TargetControlID="Txt_Direccion_IP" ValidChars="1234567890.">
                                      </cc1:FilteredTextBoxExtender>
                                </td>
                               
                               
                            </tr>
                           
                            </table>
                            
                    </div>
            </div>
            <div id="Div_Listado_Cotizadores" runat="server" style="background-color:#ffffff; width:100%; height:100%;">
                    <table id="Tbl_Grid_Cotizadores" border="0" cellspacing="0" class="estilo_fuente" style="width:98%;">                        
                            <tr>
                                <td align="center" colspan="4">
                                &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:GridView ID="Grid_Cotizadores" runat="server" AllowPaging="true" 
                                        AutoGenerateColumns="False" CssClass="GridView_1" 
                                        EmptyDataText="&quot;No se encontraron registros&quot;" GridLines="none"                                         
                                        PageSize="5" Style="white-space:normal" Width="96%" 
                                        DataKeyNames="Empleado_ID"
                                        onselectedindexchanged="Grid_Cotizadores_SelectedIndexChanged">
                                        <RowStyle CssClass="GridItem" />
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                <ItemStyle Width="5%" />
                                            </asp:ButtonField>
                                                <asp:BoundField DataField="Empleado_ID">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" ForeColor="Transparent" Font-Size="0px"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="NO_EMPLEADO" HeaderText="No Empleado" >
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="NOMBRE_COMPLETO" HeaderText="Nombre">
                                                    <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CORREO" HeaderText="Correo">
                                                    <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="40%" />
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
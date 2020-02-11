<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Despachos_Externos.aspx.cs" Inherits="paginas_Predial_Frm_Cat_Pre_Despachos_Externos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Contenido" style="background-color:#ffffff; width:100%; height:100%;">
                    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Despachos Externos
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
               </table>             
            
               <table width="98%"  border="0" cellspacing="0">
                <tr align="center">
                    <td colspan="2">                
                        <div style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td align="left" style="width:59%;">
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                            onclick="Btn_Nuevo_Click" />
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" 
                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                            onclick="Btn_Modificar_Click" />
                                        <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" 
                                            ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" 
                                            onclick="Btn_Eliminar_Click" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" 
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            onclick="Btn_Salir_Click" />
                                    </td>
                                    <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                <td style="width:55%;">
                                                    <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100"   ToolTip = "Buscar" Width="180px"/>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                        WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" 
                                                        runat="server" TargetControlID="Txt_Busqueda" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                </td>
                                                <td style="vertical-align:middle;width:5%;" >
                                                    <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                                        onclick="Btn_Buscar_Despachos_Externos_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                </table>  
                    
                <br />
                    
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Despacho
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Despacho" runat="server" Width="96.4%" MaxLength="120"/>
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            *Estatus
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%" >
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                <asp:ListItem Text="BAJA" Value="BAJA" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Contacto
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Contacto" runat="server" Width="96.4%" MaxLength="120"/>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Contacto" 
                                runat="server" Enabled="True" TargetControlID="Txt_Contacto"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ/">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            *Correo electrónico
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Correo" runat="server" Width="96.4%" MaxLength="50"/>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Correo" 
                                runat="server" Enabled="True" TargetControlID="Txt_Correo" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="-_.@">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Colonia
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Colonias" runat="server" Width="99%" 
                                onselectedindexchanged="Cmb_Colonias_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            *Calle
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Calles" runat="server" Width="99%" >
                            <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Número exterior
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_No_Exterior" runat="server" Width="96.4%" MaxLength="50"/>
                            <cc1:FilteredTextBoxExtender ID="Txt_No_Exterior_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" TargetControlID="Txt_No_Exterior" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ/-#">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            Número interior
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_No_interior" runat="server" Width="96.4%" MaxLength="50"/>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                runat="server" Enabled="True" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                TargetControlID="Txt_No_interior" ValidChars="ÑñáéíóúÁÉÍÓÚ/-#">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Teléfono
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Telefono" runat="server" Width="96.4%" MaxLength="20"/>
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            Fax
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Fax" runat="server" Width="96.4%" MaxLength="100"/>
                        </td>
                    </tr>
                    <tr>
                    <td><asp:TextBox ID="Txt_Id" runat="server" Width="96.4%" Visible="false"/></td>
                    </tr>
                    </tr>
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_Despachos" runat="server" AllowPaging="True" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="5" Width="100%"
                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                AllowSorting="True" HeaderStyle-CssClass="tblHead" 
                                style="white-space:normal;" 
                                onpageindexchanging="Grid_Despachos_Externos_PageIndexChanging" 
                                onselectedindexchanged="Grid_Despachos_Externos_SelectedIndexChanged">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="3.5%" HorizontalAlign="Center"/>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="DESPACHO_ID" HeaderText="Id" HeaderStyle-Width="25%" Visible="false"/>
                                    <asp:BoundField DataField="DESPACHO" HeaderText="Despacho" HeaderStyle-Width="25%" />
                                    <asp:BoundField DataField="CONTACTO" HeaderText="Contacto" />
                                    <asp:BoundField DataField="DOMICILIO" HeaderText="Domicilio" HeaderStyle-Width="30%" />
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" HeaderStyle-Width="10%" />
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <HeaderStyle CssClass="tblHead" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                            <br />
                        </td>
                    </tr>
                </table>
                
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


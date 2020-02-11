<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Ciudadanos.master"
    AutoEventWireup="true" CodeFile="Frm_Ope_Tra_Solicitud_Ciudadano.aspx.cs" Inherits="paginas_Tramites_Frm_Ope_Tra_Solicitud"
    Title="Solicitud de Trámite" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div style="width: 95%;">       
         <ContentTemplate>
           <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
         <ContentTemplate>
            <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressTemplate">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />                            
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                </ContentTemplate>
                </asp:UpdatePanel>
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td align="center" class="label_titulo" colspan="4">
                            Solicitud de Tr&aacute;mite
                        </td>
                    </tr>
                    <tr align="left">
                        <td colspan="4">
                            <asp:Image ID="Img_Warning" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Visible="false" />
                            <asp:Label ID="Lbl_Informacion" runat="server" ForeColor="#990000"></asp:Label>
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td colspan="4" align="left" valign="middle">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" OnClick="Btn_Nuevo_Click" />
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                            OnClick="Btn_Modificar_Click" Visible="False" />
                                        <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                            
                                            OnClientClick="return confirm('¿Está seguro de eliminar el asunto seleccionado?');" 
                                            Visible="False" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Salir_Click" />
                                    </td>
                                    <td align="right">
                                        Búsqueda por:
                                        <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"></asp:TextBox>
                                       <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Solicitud" runat="server" WatermarkCssClass="watermarked"
                                            WatermarkText="Ingrese folio de solicitud" TargetControlID="Txt_Busqueda">
                                        </cc1:TextBoxWatermarkExtender>                                                                                                                                     
                                        <asp:ImageButton ID="Btn_Buscar" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                            TabIndex="1" OnClick="Btn_Buscar_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Folio
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Folio" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                            Estatus
                        </td>
                        <td>
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="205px" Enabled="False">
                            </asp:DropDownList>
                        </td>
                    </tr>

                    
                    <tr>
                        <td>
                            Tr&aacute;amite
                        </td>
                        <td>
                            <asp:DropDownList ID="Cmb_Tramite" runat="server" Width="205px" OnSelectedIndexChanged="Cmb_Tramite_SelectedIndexChanged"
                                AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Avance
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Avance" runat="server" Enabled="False" Width="200px"></asp:TextBox>
                            &nbsp;%
                            
                        </td>
                        
                    </tr>
                    
                    <tr>
                    
                        <td>
                            Costo
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Costo" runat="server" Enabled="False" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            Tiempo Estimado
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Tiempo_Estimado" runat="server" Enabled="False" 
                                Width="200px"></asp:TextBox>&nbsp;días
                        </td>
                        </tr>
                    
                    
                    <tr>
                        <td colspan="4">
                            <hr class="linea" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Nombre
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Nombre" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                        </td> 
                        <td>
                            Apellido Paterno
                        </td>
                        <td>
                             <asp:TextBox ID="Txt_Apellido_Paterno" runat="server" Width="200px" 
                                 Enabled="False"></asp:TextBox>
                        </td>              
                    </tr>
                 
                    <tr>
                    
                        <td>
                            Apellido Materno
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Apellido_Materno" runat="server" Width="200px" 
                                Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                            E-mail
                        </td>
                        <td>
                             <asp:TextBox ID="Txt_Email" runat="server" Width="200px" 
                                 Enabled="False"></asp:TextBox>
                                 <cc1:FilteredTextBoxExtender ID="Ftbe_Email" runat="server" 
                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                    TargetControlID="Txt_Email" ValidChars=".@">
                </cc1:FilteredTextBoxExtender>
                        </td>
                                              
                    </tr>
                    
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center"  colspan="4">
                            
                            <asp:Label ID="Lbl_Datos_Requeridos" runat="server" Font-Bold="True" 
                                Font-Size="Small" Text="Datos Requeridos" Visible="False" 
                                ForeColor="#006600"></asp:Label>
                            
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="4" align="center">
                            <asp:GridView ID="Grid_Datos" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" PageSize="5" 
                                Width="97%">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" 
                                        ImageUrl="~/paginas/imagenes/gridview/grid_info.png">
                                        <ItemStyle Width="10%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Nombre" HeaderText="Datos" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Descripción">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="Txt_Descripcion_Datos" runat="server" Width="<%# 400 %>"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                            </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                         
                            <asp:Label ID="Lbl_Documentos_Requeridos" runat="server" Font-Bold="True" 
                                Font-Size="Small" Text="Documentos Requeridos" Visible="False" 
                                ForeColor="#006600"></asp:Label>
                         
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="4" align="center">
                            <asp:GridView ID="Grid_Documentos" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CssClass="GridView_1" GridLines="None" PageSize="5" Width="97%">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" 
                                        ImageUrl="~/paginas/imagenes/gridview/grid_info.png">
                                        <ItemStyle Width="10%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Documento" HeaderText="Documento" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Width="25%" Wrap = "true"/>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="URL">
                                        <ItemTemplate>
                                            <cc1:AsyncFileUpload ID="FileUp" runat="server" ErrorBackColor="Red"
                                                 CompleteBackColor="Lime" UploadingBackColor="Silver"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                              
                                
                            </asp:GridView>
                            <asp:GridView ID="Grid_Documentos_Modificar" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CssClass="GridView_1" GridLines="None" PageSize="5" Width="50%">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" 
                                        ImageUrl="~/paginas/imagenes/gridview/grid_info.png">
                                        <ItemStyle Width="10%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Documento" HeaderText="Documento" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left"/>
                                        <ItemStyle HorizontalAlign="Left" Wrap = "true"/>
                                    </asp:BoundField>
                                                                       
                                    
                                    
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>                                               
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                </table>
          </ContentTemplate>
     
    </div>
</asp:Content>

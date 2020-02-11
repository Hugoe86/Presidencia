<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Areas.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Areas" Title="Catálogo de Áreas" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Areas" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <%--<div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>--%>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Areas" style="background-color:#ffffff; width:100%; height:100%;">          
                    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Catálogo de Áreas
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
               </table>             
            
               <table width="98%"  border="0" cellspacing="0">
                         <tr align="center">
                             <td> 
                                 <div align="right" class="barra_busqueda">                        
                                      <table style="width:100%;height:28px;">
                                        <tr>
                                          <td align="left" style="width:59%;">  
                                                <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="1"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                                <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="2"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" /> 
                                                <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" TabIndex="2"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                                    OnClientClick="return confirm('¿Está seguro de eliminar el Área seleccionada?');"/>
                                                <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="4"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                          </td>
                                          <td align="right" style="width:41%;">
                                            <table style="width:100%;height:28px;">
                                                <tr>
                                                    <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                    <td style="width:55%;">
                                                        <asp:TextBox ID="Txt_Busqueda_Area" runat="server" MaxLength="100" TabIndex="5"  ToolTip = "Buscar por Nombre" Width="180px"/>
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Area" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Ingrese Nombre>" TargetControlID="Txt_Busqueda_Area" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Area" 
                                                            runat="server" TargetControlID="Txt_Busqueda_Area" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                    </td>
                                                    <td style="vertical-align:middle;width:5%;" >
                                                        <asp:ImageButton ID="Btn_Buscar_Area" runat="server" TabIndex="6"
                                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar" 
                                                            onclick="Btn_Buscar_Area_Click" />
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
                            Area ID
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Area_ID" runat="server" ReadOnly="True" Width="68%"/>
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            Estatus
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus_Area" runat="server" Width="100%" TabIndex="7">
                                <asp:ListItem>ACTIVO</asp:ListItem>
                                <asp:ListItem>INACTIVO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Nombre
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Nombre_Area" runat="server" MaxLength="100" TabIndex="8" Width="99%"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Area" 
                                runat="server" TargetControlID="Txt_Nombre_Area" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Unidad Responsable</td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:DropDownList ID="Cmb_Dependencias_Area" runat="server" TabIndex="9" Width="100%"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Comentarios
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Comentarios_Area" runat="server" TabIndex="10" MaxLength="250"
                                TextMode="MultiLine" Width="99.5%" AutoPostBack="True"/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Area" runat="server" TargetControlID ="Txt_Comentarios_Area" 
                                WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Area" 
                                runat="server" TargetControlID="Txt_Comentarios_Area" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ "/>
                        </td>
                    </tr>  
                </table>  
                
                <br />
                
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">                                      
                    <tr align="center">
                        <td>
                            <asp:GridView ID="Grid_Areas" runat="server" AllowPaging="True" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="5" GridLines="None" Width="100%"
                                onselectedindexchanged="Grid_Areas_SelectedIndexChanged" 
                                onpageindexchanging="Grid_Areas_PageIndexChanging"
                                AllowSorting="True" OnSorting="Grid_Areas_Sorting" HeaderStyle-CssClass="tblHead">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="7%" HorizontalAlign="Center"/>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Area_ID" HeaderText="Area ID" 
                                        Visible="True" SortExpression="Area_ID">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Nombre" HeaderText="Area" Visible="True" SortExpression="Nombre">
                                        <HeaderStyle HorizontalAlign="Left" Width="33%" />
                                        <ItemStyle HorizontalAlign="Left" Width="33%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Dependencia" HeaderText="Unidad Responsable" Visible="True" SortExpression="Dependencia">
                                        <HeaderStyle HorizontalAlign="Left" Width="33%" />
                                        <ItemStyle HorizontalAlign="Left" Width="33%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


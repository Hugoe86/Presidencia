<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_SAP_Fuente_Financiamiento.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Cat_SAP_Fuente_Financiamiento" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Fuentes_Financiamiento" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
          <%--  <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>--%>
                
            <div id="Div_Fuentes_Financiamiento" style="background-color:#ffffff; width:100%; height:100%;">          
                    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">
                            Catálogo de Fuentes de financiamiento
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
                                 <div align="right" style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >                        
                                      <table style="width:100%;height:28px;">
                                        <tr>
                                          <td align="left" style="width:59%;">  
                                                <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="1"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                                <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="2"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                                <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Inactivar" CssClass="Img_Button" TabIndex="3"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                                    OnClientClick="return confirm('Sólo se cambiará el estatus de la Fuente de financiamiento a INACTIVO. ¿Confirma que desea proceder?');"/>
                                                <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="4"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                          </td>
                                          <td align="right" style="width:41%;">
                                            <table style="width:100%;height:28px;">
                                                <tr>
                                                    <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                    <td style="width:55%;">
                                                        <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5"  ToolTip = "Buscar por Nombre" Width="180px"/>
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Ingrese Nombre>" TargetControlID="Txt_Busqueda" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" 
                                                            runat="server" TargetControlID="Txt_Busqueda" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                    </td>
                                                    <td style="vertical-align:middle;width:5%;" >
                                                        <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6"
                                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar"
                                                            onclick="Btn_Buscar_Click" />
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
                
                            <asp:HiddenField ID="Txt_Fuentes_Financiamiento_ID" runat="server" />
                <table width="98%" class="estilo_fuente">
                                                            
                    <tr>
                        <td style="text-align:left;width:20%;">
                            <asp:Label ID="Lbl_Txt_Clave" runat="server" AssociatedControlID="Txt_Clave" Text="*Clave" ></asp:Label>
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Clave" runat="server" MaxLength="5" style="width:98%;" TabIndex="7" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Clave" 
                                runat="server" TargetControlID="Txt_Clave" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ-"/>
                        </td>
                        <td style="text-align:right;width:20%;">
                            <asp:Label ID="Lbl_Cmb_Estatus" runat="server" AssociatedControlID="Cmb_Estatus" Text="Estatus" ></asp:Label>
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" style="width:100%;" TabIndex="8" >
                                <asp:ListItem Value="ACTIVO">ACTIVO</asp:ListItem>
                                <asp:ListItem Value="INACTIVO">INACTIVO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="text-align:left;width:20%; vertical-align:top;">
                            <asp:Label ID="Lbl_Txt_Descripcion" runat="server" AssociatedControlID="Txt_Descripcion" Text="*Descripción" ></asp:Label>
                        </td>
                        <td style="text-align:left;width:30%;" colspan="3" >
                            <asp:TextBox ID="Txt_Descripcion" runat="server" MaxLength="255" 
                                style="width:99%;" TabIndex="9" TextMode="MultiLine" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Descripcion" 
                                runat="server" TargetControlID="Txt_Descripcion" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ-_., "/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%; vertical-align:top;">
                            <asp:Label ID="Lbl_Especiales" runat="server" AssociatedControlID="Cmb_Especiales_Ramo_33" Text="*Programas especiales" ></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="Cmb_Especiales_Ramo_33" runat="server" style="width:100%;" TabIndex="8" >
                                <asp:ListItem Value="NO">NO</asp:ListItem>
                                <asp:ListItem Value="SI">SI</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                   <tr>
                        <td style="text-align:left;width:20%; vertical-align:top;">
                            <asp:Label ID="Lbl_Anio" runat="server" AssociatedControlID="Txt_Anio" Text="Año" ></asp:Label>
                        </td>
                        <td style="text-align:left;width:30%;"  >
                            <asp:TextBox ID="Txt_Anio" runat="server" MaxLength="4" 
                            style="width:99%;" TabIndex="10" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Anio" 
                            runat="server" TargetControlID="Txt_Anio" FilterType="Numbers" 
                        />
                        </td>
                   </tr>
                </table>  
                
                <br />
                
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td>
                            <div style="overflow:auto;height:320px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" >
                                <asp:GridView ID="Grid_Fuentes_Financiamiento" runat="server" 
                                     AllowSorting="True" CssClass="GridView_1" HeaderStyle-CssClass="tblHead" 
                                    AutoGenerateColumns="False"  GridLines="None" Width="100%" 
                                    onpageindexchanging="Grid_Fuentes_Financiamiento_PageIndexChanging"
                                    onsorting="Grid_Fuentes_Financiamiento_Sorting"  
                                    onselectedindexchanged="Grid_Fuentes_Financiamiento_SelectedIndexChanged">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="7%" HorizontalAlign="Center"/>
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="Clave" HeaderText="Clave" Visible="True" SortExpression="Clave">
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" Visible="True" SortExpression="Descripcion">
                                            <HeaderStyle HorizontalAlign="Left" Width="63%" />
                                            <ItemStyle HorizontalAlign="Left" Width="63%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Anio" HeaderText="Año" Visible="True" SortExpression="Anio">
                                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <PagerStyle CssClass="GridHeader" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


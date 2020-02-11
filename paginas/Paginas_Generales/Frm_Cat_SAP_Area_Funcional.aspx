<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_SAP_Area_Funcional.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Cat_SAP_Area_Funcional_"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Giro_Proveedor" runat="server" />
     <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
              <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress">
                    <img alt="" src="../Imagenes/paginas/Updating.gif" />
                </div>
            </ProgressTemplate>
           </asp:UpdateProgress>    

        <div id="Div_Requisitos" style="background-color:#ffffff; width:99%; height:100%;">      
            <table width="100%" class="estilo_fuente">
                <tr align="center">
                    <td class="label_titulo">
                        &Aacute;reas Funcionales
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                    </td>
                </tr>
           </table>          
           
            <table width="100%"  border="0" cellspacing="0">
                     <tr align="center">
                         <td>                
                             <div style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px">                        
                                  <table style="width:100%;height:28px;">
                                    <tr>
                                      <td align="left" style="width:59%;">  
                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                        CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                         OnClick = "Btn_Nuevo_Click"/>
                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                         OnClick = "Btn_Modificar_Click"/>
                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Desactivar Area" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                        OnClick = "Btn_Eliminar_Click" OnClientClick="return confirm('El estatus del Area cambiara a Inactivo. ¿Desea continuar?');"/>
                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                        OnClick="Btn_Salir_Click" />
                                      </td>
                                      <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                <td style="width:55%;">
                                                    <asp:TextBox ID="Txt_Busqueda" runat="server" Width="200px"></asp:TextBox>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Rol_ID" runat="server" WatermarkCssClass="watermarked"
                                                          WatermarkText="<Ingrese nombre del área funcional>" TargetControlID="Txt_Busqueda" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" 
                                                        runat="server" TargetControlID="Txt_Busqueda" 
                                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                        ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                </td>
                                                <td style="vertical-align:middle;width:5%;" >
                                                    <asp:ImageButton ID="Btn_Img_buscar" runat="server" ToolTip="Consultar"
                                                          ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                         OnClick = "Btn_Img_buscar_Click"/>                                       
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
          
            <table width="100%">
                <tr>
                    <td colspan="4">    
                        <hr />
                    </td>
                </tr>            
                 <tr>
                    <td style="width:10%;text-align:left;">
                        Clave
                    </td>
                    <td style="width:30%;text-align:left;">
                        <asp:TextBox ID="Txt_Clave" runat="server" MaxLength="5" Width="100%"></asp:TextBox>
                    </td>
                    <td style="width:10%; text-align:left;">
                        &nbsp;&nbsp;&nbsp;Estatus
                    </td>
                     <td style="width:30%;text-align:left;">
                       <asp:DropDownList ID="Cmb_Estatus" runat="server" Enabled="False" Width="100%" /> 
                    </td>
                </tr>
                <tr>
                    <td style="width:10%;text-align:left;">
                        Descripci&oacute;n
                    </td>
                    <td style="width:90%;text-align:left;" colspan="3">
                        <asp:TextBox ID="Txt_Descripcion" runat="server" Width="100%" MaxLength="250" TextMode="MultiLine"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="Txt_Descripcion_TextBoxWatermarkExtender" runat="server" 
                                TargetControlID="Txt_Descripcion" WatermarkCssClass="watermarked" 
                                WatermarkText="&lt;Límite de Caracteres 250&gt;" />
                            <cc1:FilteredTextBoxExtender ID="Txt_Descripcion_FilteredTextBoxExtender" 
                                runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                TargetControlID="Txt_Descripcion" ValidChars="Ññ.,:;/()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>       
                    </td>
                </tr>
                <tr> 
                    <td>
                        <asp:Label ID="Lbl_Anio" runat="server" AssociatedControlID="Txt_Anio" Text="Año" ></asp:Label>
                    </td>
                    <td style="text-align:left;width:30%;"  >
                            <asp:TextBox ID="Txt_Anio" runat="server" MaxLength="4" 
                            style="width:20%;" TabIndex="10" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Anio" 
                            runat="server" TargetControlID="Txt_Anio" 
                            FilterType="Numbers"/>
                </tr>
                <tr>
                    <td colspan="4">    
                        <hr />
                    </td>
                </tr>                
            </table>
            
            <table width="100%">    
                <tr>
                    <td align = "center" style="width:100%;">
                        <div style="overflow:auto;height:320px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" >
                            <asp:GridView ID="Grid_Areas_Funcionales" runat="server" AutoGenerateColumns="false" Width="100%"
                                     onselectedindexchanged="Grid_Areas_Funcionales_SelectedIndexChanged" style="white-space:normal"
                                    CssClass="GridView_1" onpageindexchanging="Grid_Areas_Funcionales_PageIndexChanging" 
                                     GridLines="None"
                                    OnSorting="Grid_Areas_Funcionales_Sorting" AllowSorting="true">                        
                                <Columns>
                                     <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                         ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                         <ItemStyle Width="5%" />
                                     </asp:ButtonField>
                                    <asp:BoundField DataField="Clave" HeaderText="Clave" Visible="True" SortExpression="Clave">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" Visible="True" SortExpression="Descripcion">
                                         <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                         <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                       <asp:BoundField DataField="Anio" HeaderText="Año" Visible="True" SortExpression="Anio">
                                         <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                </Columns>
                                <RowStyle CssClass="GridItem" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
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


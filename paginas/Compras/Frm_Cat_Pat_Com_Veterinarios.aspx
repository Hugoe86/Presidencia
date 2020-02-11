<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pat_Com_Veterinarios.aspx.cs" Inherits="paginas_predial_Frm_Cat_Pat_Com_Veterinarios" Title="Catalogo de Veterinarios" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Veterinarios" runat="server" />  
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="2">Catálogo de Veterinarios</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
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
                    <tr>
                        <td>&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" style="width:50%;">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Nuevo" OnClick="Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Modificar" onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Eliminar" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro de la Base de Datos?');"
                                onclick="Btn_Eliminar_Click"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" onclick="Btn_Salir_Click"/>
                        </td>
                        <td style="width:50%;">Busqueda
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="240px"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                onclick="Btn_Buscar_Click" AlternateText="Consultar" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" 
                                WatermarkText="<- Nombre ->" TargetControlID="Txt_Busqueda" 
                                WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>    
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" 
                                TargetControlID="Txt_Busqueda" InvalidChars="<,>,&,',!," 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>                                  
                        </td>                        
                    </tr>
                </table>   
                <br />
                <center>
                    <table width="98%" class="estilo_fuente">
                        <tr>
                            <td colspan="4">
                                <asp:HiddenField ID="Hdf_Veterinario_ID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:18%; text-align:left;">
                                <asp:Label ID="Lbl_Veterinario_ID" runat="server" Text="Veterinario ID" ></asp:Label>
                            </td>
                            <td style="width:32%; text-align:left;">
                                <asp:TextBox ID="Txt_Veterinario_ID" runat="server" Width="50%" MaxLength="5" Enabled="False"></asp:TextBox>
                            </td>
                        </tr>                                    
                        <tr>
                            <td style="width:18%; text-align:left; ">
                               <asp:Label ID="Lbl_Nombre" runat="server" Text="* Nombre" ></asp:Label>
                            </td>
                            <td colspan="3" style="width:82%; text-align:left; ">
                                <asp:TextBox ID="Txt_Nombre" runat="server" Width="99.5%" MaxLength="50"></asp:TextBox>   
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre" runat="server" 
                                    TargetControlID="Txt_Nombre" InvalidChars="<,>,&,',!," 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                </cc1:FilteredTextBoxExtender> 
                            </td>
                        </tr>          
                        <tr>
                            <td style="width:18%; text-align:left;">
                                <asp:Label ID="Lbl_Apellido_Paterno" runat="server" Text="* Apellido Paterno" ></asp:Label>
                            </td>
                            <td style="width:32%; text-align:left;">
                                <asp:TextBox ID="Txt_Apellido_Paterno" runat="server" Width="98%" MaxLength="50" Enabled="False"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Apellido_Paterno" runat="server" 
                                    TargetControlID="Txt_Apellido_Paterno" InvalidChars="<,>,&,',!," 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                </cc1:FilteredTextBoxExtender> 
                            </td>
                            <td style="width:18%; text-align:left;">
                                <asp:Label ID="Lbl_Apellido_Materno" runat="server" Text="* Apellido Materno" ></asp:Label>
                            </td>
                            <td style="width:32%; text-align:left;">
                                <asp:TextBox ID="Txt_Apellido_Materno" runat="server" Width="98%" MaxLength="50" Enabled="False"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Apellido_Materno" runat="server" 
                                    TargetControlID="Txt_Apellido_Materno" InvalidChars="<,>,&,',!," 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                </cc1:FilteredTextBoxExtender> 
                            </td>
                        </tr>     
                       <tr>
                            <td style="width:18%; text-align:left;">
                                <asp:Label ID="Lbl_Direccion" runat="server" Text="* Dirección" ></asp:Label>
                            </td>
                            <td colspan="3" style="width:82%; text-align:left;">
                                <asp:TextBox ID="Txt_Direccion" runat="server" Width="99.5%" MaxLength="150"></asp:TextBox>    
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Direccion" runat="server" 
                                    TargetControlID="Txt_Direccion" InvalidChars="<,>,&,',!," 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                </cc1:FilteredTextBoxExtender> 
                            </td>
                        </tr>          
                        <tr>
                            <td style="width:18%; text-align:left;">
                                <asp:Label ID="Lbl_Ciudad" runat="server" Text="* Ciudad" ></asp:Label>
                            </td>
                            <td style="width:32%; text-align:left;">
                                <asp:TextBox ID="Txt_Ciudad" runat="server" Width="98%" MaxLength="50" Enabled="False"></asp:TextBox>    
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Ciudad" runat="server" 
                                    TargetControlID="Txt_Ciudad" InvalidChars="<,>,&,',!," 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="width:18%; text-align:left;">
                                <asp:Label ID="Lbl_Estado" runat="server" Text="* Estado" ></asp:Label>
                            </td>
                            <td style="width:32%; text-align:left;">
                                <asp:TextBox ID="Txt_Estado" runat="server" Width="98%" MaxLength="50" Enabled="False"></asp:TextBox>    
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Estado" runat="server" 
                                    TargetControlID="Txt_Estado" InvalidChars="<,>,&,',!," 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>        
                        <tr>
                            <td style="width:18%; text-align:left;">
                                <asp:Label ID="Lbl_Telefono" runat="server" Text="* Telefono" ></asp:Label>
                            </td>
                            <td style="width:32%; text-align:left;">
                                <asp:TextBox ID="Txt_Telefono" runat="server" Width="98%" MaxLength="20" Enabled="False"></asp:TextBox>  
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Telefono" runat="server" 
                                    TargetControlID="Txt_Telefono" InvalidChars="<,>,&,',!," 
                                    FilterType="Custom, Numbers" 
                                    ValidChars="()-">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="width:18%; text-align:left;">
                                <asp:Label ID="Lbl_Celular" runat="server" Text="* Celular" ></asp:Label>
                            </td>
                            <td style="width:32%; text-align:left;">
                                <asp:TextBox ID="Txt_Celular" runat="server" Width="98%" MaxLength="20" Enabled="False"></asp:TextBox> 
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Celular" runat="server" 
                                    TargetControlID="Txt_Celular" InvalidChars="<,>,&,',!," 
                                    FilterType="Custom, Numbers" 
                                    ValidChars="()-">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>         
                        <tr>
                            <td style="width:18%; text-align:left;">
                                <asp:Label ID="Lbl_CURP" runat="server" Text="* CURP" ></asp:Label>
                                </td>
                            <td style="width:32%; text-align:left;">
                                <asp:TextBox ID="Txt_CURP" runat="server" Width="98%" MaxLength="18" Enabled="False"></asp:TextBox>   
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_CURP" runat="server" 
                                    TargetControlID="Txt_CURP" InvalidChars="<,>,&,',!," 
                                    FilterType="Custom, UppercaseLetters, Numbers" 
                                    ValidChars="Ññ">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="width:18%; text-align:left;">
                                <asp:Label ID="Lbl_RFC" runat="server" Text="* RFC" ></asp:Label>
                            </td>
                            <td style="width:32%; text-align:left;">
                                <asp:TextBox ID="txt_RFC" runat="server" Width="98%" MaxLength="15" Enabled="False"></asp:TextBox>  
                                <cc1:FilteredTextBoxExtender ID="FTE_txt_RFC" runat="server" 
                                    TargetControlID="txt_RFC" InvalidChars="<,>,&,',!," 
                                    FilterType="Custom, UppercaseLetters, Numbers" 
                                    ValidChars="Ññ">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>           
                        <tr>
                            <td style="width:18%; text-align:left;">
                                <asp:Label ID="Lbl_Cedula_Profesional" runat="server" Text="* Cedula Profesional" ></asp:Label>
                            </td>
                            <td style="width:32%; text-align:left;">
                                <asp:TextBox ID="Txt_Cedula_Profesional" runat="server" Width="98%" MaxLength="20" Enabled="False"></asp:TextBox>   
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cedula_Profesional" runat="server" 
                                    TargetControlID="Txt_Cedula_Profesional" InvalidChars="<,>,&,',!," 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="width:18%; text-align:left;">
                               <asp:Label ID="Lbl_Estatus" runat="server" Text="* Estatus" ></asp:Label>
                            </td>
                            <td style="width:32%; text-align:left; ">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                    <asp:ListItem Text ="&lt;SELECCIONE&gt;" Value="SELECCIONE"></asp:ListItem>
                                    <asp:ListItem Text ="VIGENTE" Value="VIGENTE"></asp:ListItem>
                                    <asp:ListItem Text ="OBSOLETO" Value="OBSOLETO"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>         
                    </table>
                    <br />                                
                    <asp:GridView ID="Grid_Veterinarios" runat="server" CssClass="GridView_1"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="5" Width="99%"
                        GridLines= "None"
                        onselectedindexchanged="Grid_Veterinarios_SelectedIndexChanged" 
                        onpageindexchanging="Grid_Veterinarios_PageIndexChanging">
                        <RowStyle CssClass="GridItem" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png"  >
                                <ItemStyle Width="30px" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="VETERINARIO_ID" HeaderText="Veterinario ID" 
                                SortExpression="VETERINARIO_ID" >
                                <ItemStyle Width="90px" HorizontalAlign="Center" Font-Size="X-Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NOMBRE_COMPLETO" HeaderText="Nombre" 
                                SortExpression="NOMBRE_COMPLETO">
                                <ItemStyle Font-Size="X-Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" 
                                SortExpression="ESTATUS" >
                                <ItemStyle Width="70px" HorizontalAlign="Center" Font-Size="X-Small" />
                            </asp:BoundField>
                        </Columns>
                        <PagerStyle CssClass="GridHeader" />
                        <SelectedRowStyle CssClass="GridSelected" />
                        <HeaderStyle CssClass="GridHeader" />                                
                        <AlternatingRowStyle CssClass="GridAltItem" />       
                    </asp:GridView>
                </center>        
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
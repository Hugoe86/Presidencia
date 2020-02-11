<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Puestos.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Puestos" Title="Catálogo de Puestos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" language="javascript">
    function pageLoad() { $('[id*=Txt_Comen').keyup(function() {var Caracteres =  $(this).val().length;if (Caracteres > 250) {this.value = this.value.substring(0, 250);$(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});}
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Puestos" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
             <asp:Button ID="Btn_Comodin" runat="server" style="background-color:Transparent;border-style:none;" OnClientClick="javascript:return false;"/>
            
            <div id="Div_Puestos" style="background-color:#ffffff; width:100%; height:100%;">                                            
                
              <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Catálogo de Puestos
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                        </td>
                    </tr>
               </table>             
            
               <table width="98%"  border="0" cellspacing="0">
                         <tr align="center">
                             <td colspan="2">                
                                 <div align="right" class="barra_busqueda">                        
                                      <table style="width:100%;height:28px;">
                                        <tr>
                                          <td align="left" style="width:59%;">  
                                               <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="1"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                                <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="2"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                                <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" TabIndex="3"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                                    OnClientClick="return confirm('¿Está seguro de eliminar el Puesto seleccionada?');"/>
                                                <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="4"
                                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                          </td>
                                          <td align="right" style="width:41%;">
                                            <table style="width:100%;height:28px;">
                                                <tr>
                                                    <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                    <td style="width:55%;">
                                                        <asp:TextBox ID="Txt_Busqueda_Puesto" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar por Nombre" Width="200px"/>
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Puesto" runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="<Ingrese el Nombre>" TargetControlID="Txt_Busqueda_Puesto" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Puesto" runat="server" TargetControlID="Txt_Busqueda_Puesto"
                                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                    </td>
                                                    <td style="vertical-align:middle;width:5%;" >
                                                        <asp:ImageButton ID="Btn_Buscar_Puesto" runat="server" ToolTip="Consultar" TabIndex="6"
                                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Puesto_Click" />                                       
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
                
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="98%" ActiveTabIndex="0" CssClass="Tab">
                    <cc1:TabPanel  runat="server" HeaderText="TabPanel1"  ID="TabPanel1"  Width="100%">
                        <HeaderTemplate> Generales </HeaderTemplate>
                        <ContentTemplate>
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                                <tr>
                                    <td style="width:100%" colspan="4">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        Puesto ID
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Puesto_ID" runat="server" ReadOnly="True" Width="98%"/>
                                    </td>
                                    <td style="text-align:left;width:20%;">
                                       &nbsp;*Aplica PSM
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:DropDownList ID="Cmb_Aplica_PSM" runat="server" Width="100%" TabIndex="7">
                                            <asp:ListItem>N</asp:ListItem>
                                            <asp:ListItem>S</asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        *Aplica Fondo Retiro
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:DropDownList ID="Cmb_Aplica_Fondo_Retiro" runat="server" Width="100%" TabIndex="7">
                                            <asp:ListItem>NO</asp:ListItem>
                                            <asp:ListItem>SI</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align:left;width:20%;">
                                        &nbsp;*Estatus
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:DropDownList ID="Cmb_Estatus_Puesto" runat="server" Width="100%" TabIndex="7">
                                            <asp:ListItem>ACTIVO</asp:ListItem>
                                            <asp:ListItem>INACTIVO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        *Nombre
                                    </td>
                                    <td style="text-align:left;" colspan="3">
                                        <asp:TextBox ID="Txt_Nombre_Puesto" runat="server" MaxLength="100" TabIndex="8" Width="98%"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Puesto" runat="server" TargetControlID="Txt_Nombre_Puesto" 
                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                            ValidChars="-ÑñáéíóúÁÉÍÓÚ. " Enabled="True"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;">
                                        &nbsp;* Plaza
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:DropDownList ID="Cmb_Plaza" runat="server" Width="100%" TabIndex="7">
                                            <asp:ListItem>&lt;- SELECCIONE - &gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align:left;width:20%;">
                                        &nbsp;*Salario Mensual
                                    </td>
                                    <td style="text-align:left;width:30%;">
                                        <asp:TextBox ID="Txt_Salario_Mensual" runat="server" Width="96%"/>
                                        <cc1:MaskedEditExtender ID="MEE_Txt_Salario_Mensual" runat="server" 
                                            TargetControlID="Txt_Salario_Mensual" Mask="9,999,999.99" MaskType="Number" 
                                            InputDirection="RightToLeft" AcceptNegative="Left" DisplayMoney="Left" 
                                            ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" 
                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                            CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                    </td>                                    
                                </tr>
                                <tr>
                                    <td style="text-align:left;width:20%;vertical-align:top;">
                                        Comentarios
                                    </td>
                                    <td colspan="3" style="text-align:left;width:80%;">
                                        <asp:TextBox ID="Txt_Comentarios_Puesto" runat="server" TabIndex="10" MaxLength="250"
                                            TextMode="MultiLine" Width="99.5%" AutoPostBack="True"/>
                                        <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>
                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Puesto" runat="server" TargetControlID ="Txt_Comentarios_Puesto" 
                                            WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked" 
                                            Enabled="True"/>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Puesto" runat="server" TargetControlID="Txt_Comentarios_Puesto" 
                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100%" colspan="4">
                                        <hr />
                                    </td>
                                </tr>
                            </table>    
                            
                            <br />
                            
                            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">                        
                                <tr align="center">
                                    <td colspan="4">
                                        <asp:GridView ID="Grid_Puesto" runat="server" AllowPaging="True" Width="100%" 
                                            AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                            onselectedindexchanged="Grid_Puesto_SelectedIndexChanged" 
                                            PageSize="5"  onpageindexchanging="Grid_Puesto_PageIndexChanging">
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="7%" HorizontalAlign="Center"/>
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="Puesto_ID" HeaderText="Puesto ID">
                                                    <HeaderStyle HorizontalAlign="Left" Width="18%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="18%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Nombre" HeaderText="Puesto">
                                                    <HeaderStyle HorizontalAlign="Left" Width="55%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="55%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Estatus" HeaderText="Estatus">
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                </asp:BoundField>
                                            </Columns>
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <PagerStyle CssClass="GridHeader" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>                            
                                    </td>
                                </tr>
                            </table> 
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel  runat="server" HeaderText="TabPanel1"  ID="TabPanel2"  Width="100%"  >
                        <HeaderTemplate> Perfiles </HeaderTemplate>
                        <ContentTemplate>
                            <table width="98%">   
                                    <tr>
                                        <td colspan="4">
                                            <asp:HiddenField ID="Hdf_Detalle_Autorizar_ID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%; text-align:left; vertical-align:top;">
                                            <asp:Label ID="Lbl_Perfil" runat="server" Text="Perfiles" CssClass="estilo_fuente"/>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="Cmb_Perfil_Puesto" runat="server" Width="99%"/>
                                        </td>   
                                        <td style="width:20%;">
                                            <asp:Button ID="Btn_Agregar_Perfil" runat="server" Text="Agregar Perfil"  ToolTip="Agregar perfiles al puesto"
                                                Width="100%" AlternateText="Agregar Perfil" onclick="Btn_Agregar_Perfil_Click" />
                                        </td>                            
                                    </tr>                                    
                            </table>     
                                           
                            <asp:GridView ID="Grid_Perfiles" runat="server" AllowPaging="True"  Width="100%" 
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                PageSize="10" onpageindexchanging="Grid_Perfiles_PageIndexChanging" OnRowDataBound="Btn_Eliminar_Perfil_RowDataBound"
                                >
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Eliminar">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="Btn_Eliminar_Perfil" runat="server" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" 
                                               OnClick="Btn_Eliminar_Perfil_Click" OnClientClick="return confirm('¿Está seguro de eliminar el perfil seleccionado seleccionado?');"/>
                                        </ItemTemplate>    
                                        <HeaderStyle Width="15" HorizontalAlign="Center"/>
                                        <ItemStyle Width="15" HorizontalAlign="Center"/>                                
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PERFIL_ID" HeaderText="Perfil ID">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre del Perfil">
                                        <HeaderStyle HorizontalAlign="Left" Width="65%" />
                                        <ItemStyle HorizontalAlign="Left" Width="65%" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>   
                                                   
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Proveedores.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Proveedores" Title="Catálogo de Proveedores" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" language="javascript">
    function pageLoad() { $('[id*=Txt_Comen').keyup(function() {var Caracteres =  $(this).val().length;if (Caracteres > 250) {this.value = this.value.substring(0, 250);$(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});}
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Proveedores" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Calendario_Nominas" style="background-color:#ffffff; width:99%; height:100%;">
            
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Catálogo de Proveedores
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                        </td>
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td colspan = "2" align = "left">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" 
                                TabIndex="17"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" 
                                TabIndex="18"/> 
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" 
                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"                                                                 
                                TabIndex="19"
                                OnClientClick="return confirm('¿Está seguro de eliminar el Proveedor seleccionado?');" onclick="Btn_Eliminar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click" 
                                TabIndex="20"/>
                        </td>
                        <td colspan="2">
                            Busqueda
                            <asp:TextBox ID="Txt_Busqueda_Proveedor" runat="server" MaxLength="100"  TabIndex="21"
                                ToolTip = "Buscar por Nombre del Proveedor" Width="180px"/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Proveedor" 
                                runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese Nombre>" 
                                TargetControlID="Txt_Busqueda_Proveedor" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Proveedoro" 
                                runat="server" TargetControlID="Txt_Busqueda_Proveedor" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ. ,.()"/>
                            <asp:ImageButton ID="Btn_Buscar_Proveedor" runat="server" TabIndex="22"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar" onclick="Btn_Buscar_Proveedor_Click"
                                 />
                        </td>                        
                    </tr>
                </table>
                                
                <table width="100%">
                    <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Proveedor ID
                        </td>
                        <td  style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Proveedor_ID" runat="server" Width="99%"/>
                        </td>
                        <td style="text-align:left;width:20%;">
                            *Aval                                                       
                        </td>
                        <td  style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Aval" runat="server" Width="100%" TabIndex="0">
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                <asp:ListItem>SI</asp:ListItem>
                                <asp:ListItem>NO</asp:ListItem>
                            </asp:DropDownList>                            
                        </td>                        
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Nombre
                        </td>
                        <td style="text-align:left;width:80%;" colspan="3">     
                            <asp:TextBox ID="Txt_Nombre_Proveedor" runat="server" Width="99%" MaxLength="100" TabIndex="1"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Nombre_Proveedor" runat="server"  TargetControlID="Txt_Nombre_Proveedor"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters" ValidChars="áéíóúÁÉÍÓÚ ñÑ . ,.()"/>
                        </td>
                    </tr>
                   <tr>
                        <td style="text-align:left;width:20%;">                          
                            *RFC
                        </td>
                        <td style="text-align:left;width:30%;">     
                            <asp:TextBox ID="Txt_RFC_Proveedor" runat="server" Width="98%" MaxLength="15" TabIndex="2"
                                onkeyup='this.value = this.value.toUpperCase();'/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_RFC_Proveedor" runat="server"  TargetControlID="Txt_RFC_Proveedor"
                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ñ"/>
                        </td>
                        <td style="text-align:left;width:20%;">
                            *Estatus
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus_Proveedor" runat="server" Width="100%" TabIndex="3">
                                <asp:ListItem>&lt; Seleccione &gt;</asp:ListItem>
                                <asp:ListItem>ACTIVO</asp:ListItem>
                                <asp:ListItem>INACTIVO</asp:ListItem>
                            </asp:DropDownList>
                        </td>       
                    </tr>          
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Calle
                        </td>
                        <td  style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Calle_Proveedor" runat="server" Width="98%" MaxLength="100" TabIndex="4"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Calle_Proveedor" runat="server"  TargetControlID="Txt_Calle_Proveedor"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>                            
                        </td>
                        <td style="text-align:left;width:20%;">
                            *Numero                           
                        </td>
                        <td  style="text-align:left;width:30%;">                            
                            <asp:TextBox ID="Txt_Numero_Proveedor" runat="server" Width="98%" TabIndex="5"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Numero_Proveedor" runat="server"  TargetControlID="Txt_Numero_Proveedor"
                                FilterType="Numbers"/>                              
                        </td>                        
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Colonia
                        </td>
                        <td  style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Colonia_Proveedor" runat="server" Width="99%" MaxLength="100" TabIndex="6"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Colonia_Proveedor" runat="server"  TargetControlID="Txt_Colonia_Proveedor"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>                             
                        </td>
                        <td style="text-align:left;width:20%;">   
                            *Codigo Postal                        
                        </td>
                        <td  style="text-align:left;width:30%;">                            
                            <asp:TextBox ID="Txt_Codigo_Postal_Proveedor" runat="server" Width="98%" MaxLength="5" TabIndex="7"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Codigo_Postal_Proveedor" runat="server"  TargetControlID="Txt_Codigo_Postal_Proveedor"
                                FilterType="Numbers" />                              
                        </td>                        
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Ciudad
                        </td>
                        <td  style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Ciudad_Proveedor" runat="server" Width="98%" MaxLength="100" TabIndex="8"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Ciudad_Proveedor" runat="server"  TargetControlID="Txt_Ciudad_Proveedor"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>                             
                        </td>
                        <td style="text-align:left;width:20%;">                           
                            *Estado
                        </td>
                        <td  style="text-align:left;width:30%;"> 
                             <asp:TextBox ID="Txt_Estado_Proveedor" runat="server" Width="98%" MaxLength="100" TabIndex="9"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Estado_Proveedor" runat="server"  TargetControlID="Txt_Estado_Proveedor"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>                               
                        </td>                        
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Telefono
                        </td>
                        <td  style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Telefono_Proveedor" runat="server" Width="98%" MaxLength="13" TabIndex="10"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Telefono_Proveedor" runat="server"  TargetControlID="Txt_Telefono_Proveedor"
                                FilterType="Custom, Numbers" ValidChars="-"/>                              
                        </td>
                        <td style="text-align:left;width:20%;">    
                            Fax                       
                        </td>
                        <td  style="text-align:left;width:30%;">                            
                            <asp:TextBox ID="Txt_Fax_Proveedor" runat="server" Width="98%" MaxLength="13" TabIndex="11"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Fax_Proveedor" runat="server"  TargetControlID="Txt_Fax_Proveedor"
                                FilterType="Custom, Numbers" ValidChars="-"/>                                
                        </td>                        
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            E-mail
                        </td>
                        <td  style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Email_Proveedor" runat="server" Width="98%" MaxLength="100" TabIndex="12"/>                             
                        </td>
                        <td style="text-align:left;width:20%;"> 
                            *Contacto                          
                        </td>
                        <td  style="text-align:left;width:30%;">                            
                            <asp:TextBox ID="Txt_Contacto_Proveedor" runat="server" Width="98%" MaxLength="100" TabIndex="13"/>
                        </td>                        
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;vertical-align:top;">
                            Comentarios
                        </td>
                        <td  style="text-align:left;width:80%;" colspan="3">
                            <asp:TextBox ID="Txt_Comentarios_Proveedor" runat="server" Width="99.5%" TextMode="MultiLine" MaxLength="255" TabIndex="14"/>
                            <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>                                                                                                                                  
                </table>
                <cc1:TabContainer ID="TPnl_Contenedor" runat="server" ActiveTabIndex="0" Width="99.5%">
                    <cc1:TabPanel ID="Pnl_Proveedores" runat="server" HeaderText="Proveedores">
                        <HeaderTemplate>
                            Proveedores
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%">
                                <tr align="center">
                                    <td colspan="4">
                                        <asp:GridView ID="Grid_Proveedores" runat="server" CssClass="GridView_1"
                                             AutoGenerateColumns="False"  GridLines="None" AllowPaging="True" PageSize="100"
                                            onpageindexchanging="Grid_Proveedores_PageIndexChanging" 
                                            onselectedindexchanged="Grid_Proveedores_SelectedIndexChanged"
                                            AllowSorting="True" OnSorting="Grid_Proveedores_Sorting">
                                                 <Columns>
                                                     <asp:ButtonField ButtonType="Image" CommandName="Select"  HeaderText="Seleccionar"
                                                         ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                         <ItemStyle Width="15%" />
                                                         </asp:ButtonField>
                                                     <asp:BoundField DataField="PROVEEDOR_ID" HeaderText="Proveedor ID" SortExpression="PROVEEDOR_ID">
                                                         <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                         <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                     </asp:BoundField>
                                                     <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" 
                                                          DataFormatString="{0:dd/MM/yyyy}" SortExpression="NOMBRE">
                                                          <HeaderStyle HorizontalAlign="Left" Width="33%" />
                                                          <ItemStyle HorizontalAlign="Left" Width="33%" />
                                                     </asp:BoundField>
                                                     <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" 
                                                          DataFormatString="{0:dd/MM/yyyy}" SortExpression="ESTATUS">
                                                          <FooterStyle HorizontalAlign="Left" />
                                                          <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                          <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                     </asp:BoundField>
                                                 </Columns>
                                                 <SelectedRowStyle CssClass="GridSelected" />
                                                 <HeaderStyle CssClass="tblHead" />
                                                 <PagerStyle CssClass="GridHeader" />
                                                 <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>  
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TPnl_Deducciones" runat="server" HeaderText="Deducciones">
                        <HeaderTemplate>Deducciones</HeaderTemplate>
                        <ContentTemplate>
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:20%;text-align:left;vertical-align:top;">
                                        Deducciones
                                    </td>
                                    <td style="width:30%;text-align:left;vertical-align:top;">
                                        <asp:DropDownList ID="Cmb_Deducciones" runat="server" Width="100%" TabIndex="15" />
                                    </td>
                                    <td style="width:50%;text-align:left;vertical-align:top;">
                                        <asp:Button ID="Btn_Agregar_Deducciones" runat="server" Text="Agregar Deducción" TabIndex="16"
                                            OnClick="Btn_Agregar_Deduccion" CausesValidation="false"  Width="150px" Height="22px" style="font-size:12px;"/>
                                    </td>                                                                                                                                            
                                </tr>
                                <tr>
                                    <td style="width:99.5%;text-align:center;vertical-align:top;" colspan="3">
                                        <center>
                                        <div style="overflow:auto;height:250px;width:99.5%;vertical-align:top;border-style:outset;border-color:Silver;" >
                                        <asp:GridView ID="Grid_Deducciones" runat="server"
                                                    AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                                    Width="100%" OnRowDataBound="Grid_Deducciones_RowDataBound">
                                                    
                                                    <Columns>
                                                       <asp:BoundField DataField="PERCEPCION_DEDUCCION_ID" HeaderText="Percepción">
                                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="15%" />                                                       
                                                       </asp:BoundField>
                                                       <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" >
                                                            <HeaderStyle HorizontalAlign="Left" Width="45%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="45%" />                                                       
                                                       </asp:BoundField>                                                                                                    
                                                       <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <center>
                                                                <asp:ImageButton ID="Btn_Delete_Deduccion" runat="server" 
                                                                       ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png"  CausesValidation ="false" 
                                                                       OnClick="Btn_Delete_Deduccion" 
                                                                       OnClientClick="return confirm('¿Está seguro de eliminar de la tabla la Deduccion seleccionada?');"/>                                                        
                                                                </center>                
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />                                                                                   
                                                       </asp:TemplateField>                                                                                                                                                                                                                                                 
                                                    </Columns>
                                                   <SelectedRowStyle CssClass="GridSelected" />
                                                   <PagerStyle CssClass="GridHeader" />
                                                   <HeaderStyle CssClass="GridHeader" />
                                                   <AlternatingRowStyle CssClass="GridAltItem" />  
                                        </asp:GridView>
                                        </div>
                                        </center>
                                    </td>                                                                                                                                            
                                </tr>                                
                            </table>                          
                        </ContentTemplate>
                    </cc1:TabPanel>                                        
                </cc1:TabContainer>                                                           
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>    
</asp:Content>


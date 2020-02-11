<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" 
    CodeFile="Frm_Cat_Nom_Clave_Cargo_Abono.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Clave_Cargo_Abono" Title="Claves Cargo-Abono" %>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
 <script type="text/javascript" language="javascript">
    function pageLoad() {      
        Contar_Caracteres();
    }
  
    function Contar_Caracteres(){
        $('textarea[id$=Txt_Descripcion]').keyup(function() {
            var Caracteres =  $(this).val().length;
            
            if (Caracteres > 250) {
                this.value = this.value.substring(0, 250);
                $(this).css("background-color", "Yellow");
                $(this).css("color", "Red");
            }else{
                $(this).css("background-color", "White");
                $(this).css("color", "Black");
            }
            
            $('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');
        });
    }
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="Sm_Clave_Cargo_Abono" runat="server" />

<asp:UpdatePanel ID="Upnl_Clave_Cargo_Abono" runat="server">
    <ContentTemplate>
    
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upnl_Clave_Cargo_Abono" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Clave_Cargo_Abono" style="background-color:#ffffff; width:100%; height:100%;">
            
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Claves Cargo-Abono</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false"  />&nbsp;
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
                                           <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="3" 
                                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="4"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" TabIndex="5"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                                OnClientClick="return confirm('¿Está seguro de eliminar la clave cargo-abono seleccionada?');"/>
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="6"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                      </td>
                                      <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="width:60%;vertical-align:top;">
                                                     B&uacute;squeda
                                                    <asp:TextBox ID="Txt_Busqueda_Clave" runat="server" MaxLength="10"  TabIndex="21"
                                                        ToolTip = "Busquedad de Claves de Cargo-Abono" Width="180px"/>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Clave" 
                                                        runat="server" WatermarkCssClass="watermarked"
                                                        WatermarkText="<Clave>" 
                                                        TargetControlID="Txt_Busqueda_Clave" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Clave" 
                                                        runat="server" TargetControlID="Txt_Busqueda_Clave" 
                                                        FilterType="Numbers" />
                                                    <asp:ImageButton ID="Btn_Busqueda_Claves_Cargo_Abono" runat="server" TabIndex="22"
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar" 
                                                        onclick="Btn_Busqueda_Claves_Cargo_Abono_Click"
                                                         />                                      
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
                
                <center>
                    <table id="Tbl_Contenido" style="width:88%;">
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>                
                        <tr>
                            <td style="cursor:default; text-align:left; width:20%; ">
                                Clave
                            </td>
                            <td style="cursor:default; text-align:left; width:30%; ">
                                <asp:TextBox runat="server" ID="Txt_Clave_Cargo_Abono" Width="98%" ToolTip="Clave de Cargo/Abono" MaxLength="10"/>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Clave_Cargo_Abono" runat="server" TargetControlID="Txt_Clave_Cargo_Abono" 
                                    FilterType="Numbers" />                                
                            </td>                        
                            <td style="cursor:default; text-align:left; width:20%; ">
                            </td>
                            <td style="cursor:default; text-align:left; width:30%; ">                        
                            </td>                                                
                        </tr>
                        <tr>
                            <td style="cursor:default; text-align:left; width:20%; vertical-align:top;">
                                Descripci&oacute;n                        
                            </td>
                            <td style="cursor:default; text-align:left; width:80%; " colspan="3">
                                <asp:TextBox runat="server" ID="Txt_Descripcion" Width="99.5%" ToolTip="Descripción de la clave de CARGO-ABONO"
                                    TextMode="MultiLine" Height="40px" onkeyup='this.value = this.value.toUpperCase();'/>
                                <cc1:FilteredTextBoxExtender ID="FTxt_Descripcion" runat="server"  TargetControlID="Txt_Descripcion"
                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ/-"/>    
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Descripcion" runat="server" TargetControlID ="Txt_Descripcion" 
                                    WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/>     
                                <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>                                
                            </td>                        
                        </tr>                    
                        <tr>
                            <td colspan="4">
                                <asp:HiddenField runat="server" ID="HTxt_Clave_Cargo_Abono_ID" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>                             
                    </table> 
                    
                    <table style="width:90%;">
                        <tr>
                            <td style="width:100%;">
                                <asp:GridView ID="Grid_Claves_Cargo_Abono" runat="server" 
                                    CssClass="GridView_1" 
                                    Width="98%"
                                    AutoGenerateColumns="False"  
                                    GridLines="None" 
                                    AllowPaging="true" 
                                    PageSize="10"
                                    onpageindexchanging="Grid_Claves_Cargo_Abono_PageIndexChanging"
                                    OnSelectedIndexChanged="Grid_Claves_Cargo_Abono_SelectedIndexChanged"
                                    >                                
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select"  
                                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                <ItemStyle Width="10%"  HorizontalAlign="Left"/>
                                                <HeaderStyle HorizontalAlign="Left" Width="10%"/>
                                            </asp:ButtonField>    
                                                                                        
                                            <asp:BoundField DataField="CARGO_ABONO_ID"/>     
                                                                                          
                                            <asp:BoundField DataField="CLAVE" HeaderText="Clave Cargo/Abono">
                                                <HeaderStyle HorizontalAlign="Left" Width="10%"  Font-Bold="true" Font-Size="X-Small"/>
                                                <ItemStyle HorizontalAlign="Left" Width="10%"  Font-Bold="true" Font-Size="X-Small"/>
                                            </asp:BoundField>  
                                            
                                            <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción">
                                                <HeaderStyle HorizontalAlign="Left" Width="80%" Font-Bold="true" Font-Size="X-Small" />
                                                <ItemStyle HorizontalAlign="Left" Width="80%"  Font-Bold="true" Font-Size="X-Small"/>
                                            </asp:BoundField>                                                                                                                                          
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <HeaderStyle CssClass="GridHeader" />          
                                        <FooterStyle CssClass="GridHeader" />
                                        <RowStyle CssClass="GridItem"/>                                    
                                </asp:GridView>                            
                            </td>
                        </tr>
                    </table>
                </center>
            </div>    
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>


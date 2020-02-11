<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Antiguedad_Sindicatos.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Antiguedad_Sindicatos" Title="Catálogo Antiguedad Sindicatos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" language="javascript">
    function pageLoad() { $('[id*=Txt_Comen').keyup(function() {var Caracteres =  $(this).val().length;if (Caracteres > 250) {this.value = this.value.substring(0, 250);$(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});}
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Antiguedad_Sindicato" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>  
               
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <asp:Button ID="Btn_Comodin_Perder_Foco" runat="server" style="background-color:Transparent;border-style:none;" OnClientClick="javascript:return false;"/>
            
             <div id="Div_Antiguedad_Sindicato" style="background-color:#ffffff; width:100%; height:100%;">
            
                    <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                        <tr align="center">
                            <td class="label_titulo">Antiguedad Sindicato</td>
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
                                                        OnClientClick="return confirm('¿Está seguro de eliminar la Antiguedad Sindicato seleccionado?');"/>
                                                    <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="6"
                                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                              </td>
                                              <td align="right" style="width:41%;">
                                                <table style="width:100%;height:28px;">
                                                    <tr>
                                                        <td style="width:60%;vertical-align:top;">
                                                             B&uacute;squeda
                                                            <asp:TextBox ID="Txt_Busqueda_Antiguedad_Sindicatos" runat="server" MaxLength="100"  TabIndex="21"
                                                                ToolTip = "Busquedad de Antiguedad Sindicatos" Width="180px"/>
                                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Antiguedad_Sindicatos" 
                                                                runat="server" WatermarkCssClass="watermarked"
                                                                WatermarkText="<Antiguedad_ID ó Años>" 
                                                                TargetControlID="Txt_Busqueda_Antiguedad_Sindicatos" />
                                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Antiguedad_Sindicatos" 
                                                                runat="server" TargetControlID="Txt_Busqueda_Antiguedad_Sindicatos" 
                                                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                                ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                            <asp:ImageButton ID="Btn_Busqueda_Antiguedad_Sindicatos" runat="server" TabIndex="22"
                                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar"
                                                                onclick="Btn_Busqueda_Antiguedad_Sindicatos_Click"
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

                <br />
       
                <table width="98%">
                    <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Identificador
                        </td>
                        <td  style="text-align:left;width:30%;" >
                            <asp:TextBox ID="Txt_Clabe_Antiguedad" runat="server" Width="98%" TabIndex="0"/>
                        </td> 
                        <td style="text-align:left;width:20%;">   
                            &nbsp;&nbsp;*Años                      
                        </td>
                        <td  style="text-align:left;width:30%;">  
                            <asp:TextBox ID="Txt_Anyos_Antiguedad" runat="server" Width="98%" TabIndex="1" onblur="if((this.value%5) != 0)this.value='';"/>   
                            <cc1:FilteredTextBoxExtender ID="FTxt_Anyos_Antiguedad" runat="server"  TargetControlID="Txt_Anyos_Antiguedad"
                                FilterType="Numbers"/>  
                            <asp:CustomValidator ID="Cv_Txt_Anyos_Antiguedad" runat="server"  Display="None"
                                 EnableClientScript="true" ErrorMessage="Los registros de antiguedad solo pueden ser multiplos de 5 años"
                                 Enabled="true"
                                 ClientValidationFunction="TextBox_Txt_Anyos_Antiguedad"
                                 HighlightCssClass="highlight" 
                                 ControlToValidate="Txt_Anyos_Antiguedad"/>
                            <cc1:ValidatorCalloutExtender ID="Vce_Txt_Anyos_Antiguedad" runat="server" TargetControlID="Cv_Txt_Anyos_Antiguedad" PopupPosition="TopRight"/>    
                            <script type="text/javascript" >
                                function TextBox_Txt_Anyos_Antiguedad(sender, args) {     
                                     var v = document.getElementById("<%=Txt_Anyos_Antiguedad.ClientID%>").value;   
                                     if ((v%5)!=0){  
                                        document.getElementById("<%=Txt_Anyos_Antiguedad.ClientID%>").value ="";       
                                        args.IsValid = false;     
                                     }
                                  } 
                            </script>                                                                                                                                                              
                        </td>                                                                       
                    </tr>  
                    <tr>
                        <td style="text-align:left;width:20%;vertical-align:top;">
                           *Comentarios
                        </td>
                        <td  style="text-align:left;width:30%;" colspan="3">
                            <asp:TextBox ID="Txt_Comentarios_Antiguedad" runat="server" Width="99.5%" MaxLength="100" TextMode="MultiLine" 
                                TabIndex="2"/>
                            <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Comentarios_Antiguedad" runat="server"  TargetControlID="Txt_Comentarios_Antiguedad"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>    
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Antiguedad" runat="server" TargetControlID ="Txt_Comentarios_Antiguedad" 
                                WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/>                                
                        </td>                                                                     
                    </tr>
                    <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>                                                                                                                                                                                                                                         
                </table>

                <asp:GridView ID="Grid_Antiguedad_Sindicatos" runat="server" CssClass="GridView_1" Width="98%"
                     AutoGenerateColumns="False"  GridLines="None" AllowPaging="true" PageSize="5"
                     onpageindexchanging="Grid_Antiguedad_Sindicatos_PageIndexChanging"
                     OnSelectedIndexChanged="Grid_Antiguedad_Sindicatos_SelectedIndexChanged"
                     AllowSorting="True" OnSorting="Grid_Antiguedad_Sindicatos_Sorting" HeaderStyle-CssClass="tblHead">
                        <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select"  HeaderText=""
                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                <ItemStyle Width="15%"  HorizontalAlign="Left"/>
                                <HeaderStyle HorizontalAlign="Left" Width="15%"/>
                            </asp:ButtonField>                                                
                            <asp:BoundField DataField="ANTIGUEDAD_SINDICATO_ID" HeaderText="Clave" SortExpression="ANTIGUEDAD_SINDICATO_ID">
                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </asp:BoundField>                                                   
                            <asp:BoundField DataField="ANIOS" HeaderText="Años" SortExpression="ANIOS">
                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </asp:BoundField>  
                            <asp:BoundField DataField="COMENTARIOS" HeaderText="Comentarios" SortExpression="COMENTARIOS">
                                <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                <ItemStyle HorizontalAlign="Left" Width="40%" />
                            </asp:BoundField>                                                                                                                                          
                        </Columns>
                        <SelectedRowStyle CssClass="GridSelected" />
                        <PagerStyle CssClass="GridHeader" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                    </asp:GridView>   
                    <br /><br /><br /><br />                                                                                                   
             </div>
        </ContentTemplate>         
    </asp:UpdatePanel>   
</asp:Content>


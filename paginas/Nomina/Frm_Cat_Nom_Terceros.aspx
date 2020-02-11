<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Terceros.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Terceros" Title="Catálogo Terceros" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<link href="../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />

<script type="text/javascript" language="javascript">
    function pageLoad() { $('[id*=Txt_Comen').keyup(function() {var Caracteres =  $(this).val().length;if (Caracteres > 250) {this.value = this.value.substring(0, 250);$(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});}
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager_Terceros" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <asp:Button ID="Btn_Comodin_Perder_Foco" runat="server" style="background-color:Transparent;border-style:none;" OnClientClick="javascript:return false;"/>
            
            <div id="Div_Calendario_Nominas" style="background-color:#ffffff; width:99%; height:100%;">
            
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Catálogo de Terceros
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
                                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                                CssClass="Img_Button" TabIndex="1"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" 
                                                />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                                CssClass="Img_Button" TabIndex="2"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" 
                                                /> 
                                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" 
                                                CssClass="Img_Button" TabIndex="2"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"                                                                                                 
                                                OnClientClick="return confirm('¿Está seguro de eliminar el registro seleccionado?');" onclick="Btn_Eliminar_Click" 
                                                />
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                                CssClass="Img_Button" TabIndex="4"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click" 
                                                />
                                          </td>
                                          <td align="right" style="width:41%;">
                                            <table style="width:100%;height:28px;">
                                                <tr>
                                                    <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                    <td style="width:55%;">
                                                        <asp:TextBox ID="Txt_Busqueda_Tercero" runat="server" MaxLength="100" 
                                                            TabIndex="5"  ToolTip = "Buscar Por Nombre del Terceero" Width="200px"/>
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Tercero" 
                                                            runat="server" WatermarkCssClass="watermarked"
                                                            WatermarkText="Nombre o Tercero ID" 
                                                            TargetControlID="Txt_Busqueda_Tercero" />
                                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Tercero" 
                                                            runat="server" TargetControlID="Txt_Busqueda_Tercero" 
                                                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                            ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                    </td>
                                                    <td style="vertical-align:middle;width:5%;" >
                                                        <asp:ImageButton ID="Btn_Buscar_Tercero" runat="server" TabIndex="6"
                                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar"
                                                            OnClick="Btn_Busqueda_Click"
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
                
                                
                <table width="100%">
                    <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Tercero ID
                        </td>
                        <td  style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Tercero_ID" runat="server" Width="99%"/>
                        </td>
                        <td style="text-align:left;width:20%;">    
                            Deducci&oacute;n                                                                               
                        </td>
                        <td  style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Deducciones_Calculadas" runat="server" Width="100%"/>
                        </td>                        
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Nombre
                        </td>
                        <td style="text-align:left;width:30%;">     
                            <asp:TextBox ID="Txt_Nombre_Tercero" runat="server" Width="99%" MaxLength="100"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Txt_Nombre_Tercero" runat="server"  TargetControlID="Txt_Nombre_Tercero"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters" ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>                               
                        </td>
                        <td style="text-align:left;width:20%;">
                            *Contacto
                        </td>
                        <td style="text-align:left;width:30%;"> 
                            <asp:TextBox ID="Txt_Contacto_Tercero" runat="server" Width="99%" MaxLength="15"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Contacto_Tercero" runat="server"  TargetControlID="Txt_Contacto_Tercero"
                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ñ"/>
                        </td>
                    </tr>
                   <tr>
                        <td style="text-align:left;width:20%;">                          
                            *Tel&eacute;fono
                        </td>
                        <td style="text-align:left;width:30%;">     
                            <asp:TextBox ID="Txt_Telefono" runat="server" Width="99%" MaxLength="15"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Telefono" runat="server"  TargetControlID="Txt_Telefono"
                                FilterType="Custom, Numbers" ValidChars="-"/>
                        </td>
                        <td style="text-align:left;width:20%;">
                            *Porcentaje Retenci&oacute;n
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Porcentaje_Retencion" runat="server" Width="99%" MaxLength="10"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Porcentaje_Retencion" runat="server"  TargetControlID="Txt_Porcentaje_Retencion"
                                FilterType="Custom, Numbers" ValidChars="."/>  
                            <asp:CustomValidator ID="Cv_Txt_Porcentaje_Retencion" runat="server"  Display="None"
                                 EnableClientScript="true" ErrorMessage="El Porcentaje de Retención es de [0-100]."
                                 Enabled="true"
                                 ClientValidationFunction="TextBox_Txt_Porcentaje_Retencion"
                                 HighlightCssClass="highlight" 
                                 ControlToValidate="Txt_Porcentaje_Retencion"/>
                            <cc1:ValidatorCalloutExtender ID="Vce_Txt_Porcentaje_Retencion" runat="server" TargetControlID="Cv_Txt_Porcentaje_Retencion" PopupPosition="TopRight"/>    
                            <script type="text/javascript" >
                                function TextBox_Txt_Porcentaje_Retencion(sender, args) {     
                                     var v = document.getElementById("<%=Txt_Porcentaje_Retencion.ClientID%>").value;   
                                     if ( (v < 0) || (v > 100) ){  
                                        document.getElementById("<%=Txt_Porcentaje_Retencion.ClientID%>").value ="";       
                                        args.IsValid = false;     
                                     }
                                  } 
                            </script>                                 
                        </td>       
                    </tr>          
                    <tr>
                        <td style="text-align:left;width:20%;vertical-align:top;" >
                            *Comentarios   
                        </td>
                        <td  style="text-align:left;width:30%;" colspan="3">
                              <asp:TextBox ID="Txt_Comentarios" runat="server" Width="99%" TextMode="MultiLine" Height="25px" MaxLength="250"/>                          
                              <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>
                              <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios" runat="server" 
                                    TargetControlID ="Txt_Comentarios" WatermarkText="Límite de Caractes 250" 
                                    WatermarkCssClass="watermarked"/>
                              <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios" runat="server" 
                                    TargetControlID="Txt_Comentarios" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ "/>
                            </td>                              
                        </td>                     
                    </tr>
                    <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>                                                                                                                                
                </table>

                <table width="100%">
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_Terceros" runat="server" CssClass="GridView_1" Width="100%"
                                 AutoGenerateColumns="False"  GridLines="None" AllowPaging="true" PageSize="5"
                                 onpageindexchanging="Grid_Terceros_PageIndexChanging" 
                                 onselectedindexchanged="Grid_Terceros_SelectedIndexChanged"
                                 AllowSorting="True" OnSorting="Grid_Terceros_Sorting" HeaderStyle-CssClass="tblHead">
                                     <Columns>
                                         <asp:ButtonField ButtonType="Image" CommandName="Select"  HeaderText="Seleccionar"
                                             ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                             <ItemStyle Width="15%" />
                                         </asp:ButtonField>
                                         <asp:BoundField DataField="TERCERO_ID" HeaderText="Tercero ID" SortExpression="TERCERO_ID">
                                             <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                             <ItemStyle HorizontalAlign="Left" Width="15%" />
                                         </asp:BoundField>
                                         <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE">
                                              <HeaderStyle HorizontalAlign="Left" Width="33%" />
                                              <ItemStyle HorizontalAlign="Left" Width="33%" />
                                         </asp:BoundField>
                                         <asp:BoundField DataField="TELEFONO" HeaderText="Teléfono" SortExpression="TELEFONO">
                                              <FooterStyle HorizontalAlign="Left" />
                                              <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                              <ItemStyle HorizontalAlign="Left" Width="10%" />
                                         </asp:BoundField>                                         
                                         <asp:BoundField DataField="CONTACTO" HeaderText="CONTACTO" SortExpression="CONTACTO">
                                              <FooterStyle HorizontalAlign="Left" />
                                              <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                              <ItemStyle HorizontalAlign="Left" Width="10%" />
                                         </asp:BoundField>
                                         <asp:BoundField DataField="PORCENTAJE_RETENCION" HeaderText="PORCENTAJE_RETENCION" SortExpression="PORCENTAJE_RETENCION">
                                              <FooterStyle HorizontalAlign="Left" />
                                              <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                              <ItemStyle HorizontalAlign="Left" Width="10%" />
                                         </asp:BoundField>
                                         <asp:BoundField DataField="COMENTARIOS" HeaderText="COMENTARIOS" SortExpression="COMENTARIOS">
                                              <FooterStyle HorizontalAlign="Left" />
                                              <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                              <ItemStyle HorizontalAlign="Left" Width="10%" />
                                         </asp:BoundField>        
                                         <asp:BoundField DataField="PERCEPCION_DEDUCCION_ID" HeaderText="Deducción" SortExpression="PERCEPCION_DEDUCCION_ID">
                                              <FooterStyle HorizontalAlign="Left" />
                                              <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                              <ItemStyle HorizontalAlign="Left" Width="0%" />
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


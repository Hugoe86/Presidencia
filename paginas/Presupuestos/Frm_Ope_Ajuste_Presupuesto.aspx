<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Ajuste_Presupuesto.aspx.cs" Inherits="paginas_Presupuestos_Frm_Ope_Ajuste_Presupuesto" Title="Catalogo de Ajuste de Presupuesto"%>
<%@ Register Assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
        .style1
        {
            width: 426px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" language="javascript">
    function pageLoad() { $('[id*=Txt_Comen').keyup(function() {var Caracteres =  $(this).val().length;if (Caracteres > 250) {this.value = this.value.substring(0, 250);$(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});}
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                        </div>
                        <div id="div_progress" class="processMessage" >
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />                           
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
             
                
             <div id="Div_Fuentes_Financiamiento" style="background-color:#ffffff; width:100%; height:100%;">          
                    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Catálogo de Ajuste de Presupuesto
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
               </table>             
            
               <table width="100%"  border="0" cellspacing="0">
                         <tr align="center">
                             <td colspan="2">                
                                 <div  align="right" style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >                        
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
                
                <cc1:TabContainer ID="Contenedor_Grid_Movimiento" runat="server" Width="98%" CssClass="Tab">
                    <cc1:TabPanel ID="Tab_Grid_Movimiento" HeaderText="Grid Movimiento" runat="server" Width="70%">
                        <HeaderTemplate>
                        Historial de Movimientos
                        </HeaderTemplate>
                        <ContentTemplate>
                             
                                <div style="overflow:auto;height:320px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" >
                                    <asp:GridView ID="Grid_Movimiento" runat="server"  CssClass="GridView_1" Width="100%" 
                                        AutoGenerateColumns="False" GridLines="None"
                                        AllowSorting="True" 
                                        EmptyDataText="&quot;No se encuentra ningun registros&quot;"
                                        onselectedindexchanged="Grid_Movimiento_SelectedIndexChanged" 
                                        onsorting="Grid_Movimiento_Sorting"  >
                                         
                                        <Columns>
                                            <asp:ButtonField ButtonType="Image" CommandName="Select"
                                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                <ItemStyle Width="3%" />
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="NO_SOLICITUD" HeaderText="Solicitud" 
                                                SortExpression="NO_SOLICITUD">
                                                <HeaderStyle HorizontalAlign="Right" Width="7%" />
                                                <ItemStyle HorizontalAlign="Right" Width="7%" />
                                            </asp:BoundField>
                                            
                                           <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" 
                                                SortExpression="ESTATUS" >
                                                <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                                <ItemStyle HorizontalAlign="Center" Width="20%" />
                                            </asp:BoundField> 
                                            <asp:BoundField DataField="FECHA_CREO" HeaderText="Fecha de Creación" 
                                                DataFormatString="{0:F}" SortExpression="FECHA_CREO">
                                                <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                                <ItemStyle HorizontalAlign="Left" Width="40%" />
                                            </asp:BoundField>
                                             <asp:BoundField DataField="USUARIO_CREO" HeaderText="Usuario que lo creo" 
                                                SortExpression="USUARIO_CREO">
                                                <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                <ItemStyle HorizontalAlign="Left" Width="30%" />
                                            </asp:BoundField>
                                            
                                        </Columns>
                                        <SelectedRowStyle CssClass="GridSelected" />
                                        <HeaderStyle CssClass="tblHead" />
                                        <PagerStyle CssClass="GridHeader" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView>
                                </div>
                        </ContentTemplate>
                    </cc1:TabPanel>        
                  </cc1:TabContainer>  
                  
                <cc1:TabContainer ID="Contenedor_Datos_Generales" runat="server" Width="98%"  CssClass="Tab">
                    <cc1:TabPanel ID="Tab_Datos_Generales" HeaderText="Datos Generales" runat="server" Width="70%">
                        <HeaderTemplate>
                            Datos Generales
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%">
                            
                                <tr>
                                    <td width="20%"> 
                                        <asp:Label ID="Lbl_Fuente_Financiamiento_Datos_General" runat="server" Text="Fuente de Financiamiento"   Width="100%"></asp:Label>
                                    </td>
                                    <td width="40%">
                                        <asp:DropDownList ID="Cmb_Fuente_Financiamiento" runat="server" Width="95%" AutoPostBack="True" 
                                         OnSelectedIndexChanged="Cmb_Fuente_Financiamiento_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                   
                                    <td width="40%"></td>
                                </tr>
                                
                                <tr>
                                    <td> 
                                        <asp:Label ID="Lbl_Programa_Datos_General" runat="server" Text="Programa" Width="100%"  ></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Cmb_Programa" runat="server" Width="95%" AutoPostBack="True" 
                                         OnSelectedIndexChanged="Cmb_Programa_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                
                                <tr>
                                     <td >
                                        <asp:Label ID="Lbl_Unidad_Responsable_Datos_General" runat="server" 
                                            Text="Unidad Responsable" Width="100%" ></asp:Label>
                                    </td>
                                    <td >
                                        <asp:DropDownList ID="Cmb_Unidad_Responsable" runat="server" Width="95%"  
                                            Visible="False" AutoPostBack="True" 
                                         OnSelectedIndexChanged="Cmb_Unidad_Responsable_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td > 
                                        <asp:Label ID="Lbl_Partida_Datos_General" runat="server" Text="Partida" Width="98%"  ></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Cmb_Partidas_Datos_Generales" runat="server" Width="95%" AutoPostBack="True" 
                                         OnSelectedIndexChanged="Cmb_Partidas_Datos_Generales_SelectedIndexChanged" >
                                        </asp:DropDownList>
                                    </td>
                                </tr>  
                                <tr>
                                    <td> 
                                        <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus" Width="100%"  ></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXt_Estatus_Movimiento" runat="server" Text="GENERADA" 
                                            ReadOnly="True"  Width="40%"></asp:TextBox>
                                    </td>
                                </tr>  
                                
                                <tr>
                                    <td> 
                                        <asp:Label ID="Lbl_Descripcion_Datos_General" runat="server" Text="Descripción" Width="95%"  ></asp:Label>
                                    </td>
                                    <td colspan="2" >
                                         <asp:TextBox ID="Txt_Descripcion_Datos_General" runat="server" 
                                            TextMode="MultiLine" Width="85%"></asp:TextBox>
                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Descripcion_Datos_General" 
                                             runat="server" WatermarkCssClass="watermarked"
                                            TargetControlID ="Txt_Descripcion_Datos_General" 
                                             WatermarkText="Límite de Caractes 250" Enabled="True">
                                        </cc1:TextBoxWatermarkExtender>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Descripcion_Datos_General" runat="server" 
                                            TargetControlID="Txt_Descripcion_Datos_General" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">
                                        </cc1:FilteredTextBoxExtender> 
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>        
                  </cc1:TabContainer> 
                   
              <table>
                  <tr>
                      <td>
                      </td>
                  </tr>
              </table>
              
                 <cc1:TabContainer ID="Contenedor_Movimiento" runat="server" Width="98%"  CssClass="Tab">
                    <cc1:TabPanel ID="Tab_Movimiento" HeaderText="Movimiento" runat="server"  >
                        <HeaderTemplate>
                            Movimiento
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table style="width:100%;"> 
                                <tr>
                                    <td width="18%" align="center">
                                        <asp:Label ID="Lbl_Partida_Movimiento"  runat="server" Text="Partida" Width="100%"  ></asp:Label>
                                    </td>
                                    <td width="18%" align="center">
                                        <asp:Label ID="Lbl_Disponible_Movimiento" runat="server" Text="Disponible" Width="100%"  ></asp:Label>
                                    </td>
                                    <td width="18%" align="center">
                                        <asp:Label ID="Lbl_Ampliar_Movimiento" runat="server" Text="Ampliar" Width="100%"  ></asp:Label>
                                    </td>
                                    <td width="18%" align="center">
                                        <asp:Label ID="Lbl_Reducir_Movimiento" runat="server" Text="Reducir" Width="100%"  ></asp:Label>
                                    </td>
                                    <td width="18%" align="center">
                                        <asp:Label ID="Lbl_Incrementar_Movimiento" runat="server" Text="Incrementar" Width="100%" ></asp:Label>
                                    </td>
                                    <td width="20%" align="center" >
                                        <asp:ImageButton ID="Btn_Agregar"  runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/sias_add.png"
                                        CssClass="Img_Button"  onclick="Btn_Agregar_Click" ToolTip="Agregar Partida"/>
                                       
                                        <asp:ImageButton ID="Btn_Delete_Partida"  runat="server" 
                                       ImageUrl="~/paginas/imagenes/paginas/quitar.png"  
                                        CssClass="Img_Button"  onclick="Delete_Partida_Click" ToolTip="Eliminar Partida"/>
                                      
                                    </td> 
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="Txt_Partida_Movimiento" runat="server" ReadOnly="True" 
                                            Width="95%"  ></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Disponible_Movimiento" runat="server" ReadOnly="True" 
                                            Width="95%"  ></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Ampliar_Movimiento" runat="server" Width="95%" AutoPostBack="True"
                                            ontextchanged="Txt_Ampliar_Movimiento_TextChanged"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="Txt_Ampliar_Movimiento_FilteredTextBoxExtender" 
                                                runat="server" FilterType="Numbers" 
                                            TargetControlID="Txt_Ampliar_Movimiento" Enabled="True" >
                                            </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Reducir_Movimiento" runat="server"  Width="95%" AutoPostBack="True" 
                                        ontextchanged="Txt_Reducir_Movimiento_TextChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Txt_Reducir_Movimiento_FilteredTextBoxExtender1" 
                                            runat="server" FilterType="Numbers" 
                                            TargetControlID="Txt_Reducir_Movimiento" Enabled="True" >
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Incrementar_Movimiento" runat="server"   Width="95%" AutoPostBack="True"
                                        ontextchanged="Txt_Incrementar_Movimiento_TextChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="Txt_Incrementar_Movimietno_FilteredTextBoxExtender1" 
                                            runat="server" FilterType="Numbers" 
                                            TargetControlID="Txt_Incrementar_Movimiento" Enabled="True" >
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    
                                    <td align="center">
                                      
                                        <asp:ImageButton ID="Btn_Limpiar" runat="server"   
                                        ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" 
                                        CssClass="Img_Button"  onclick="Btn_Limpiar_Click" ToolTip="Limpiar"/>
                                    </td>
                                </tr> 
                               
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>        
                 </cc1:TabContainer> 
                  
                <table>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
                
                 <cc1:TabContainer ID="Contendor_Partidas_Movimiento" runat="server" Width="98%" CssClass="Tab" >
                    <cc1:TabPanel ID="Tab_Partidas_Movimiento" HeaderText="Partidas En Movimiento" runat="server" Width="70%" >
                        <HeaderTemplate>
                            Partidas En Movimiento
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table style="width:100%;">
                            <tr>
                                <td>
                                    <div style="overflow:auto;height:230px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" >
                                        <asp:GridView ID="Grid_Partidas_Movimiento" runat="server" 
                                                    AutoGenerateColumns="false" CssClass="GridView_1" GridLines="None"
                                                    Width="100%"  
                                                    OnSelectedIndexChanged="Grid_Partidas_Movimiento_SelectedIndexChanged"
                                                    EmptyDataText="&quot;No se encuentra ningun registros&quot;"> 
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                    ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="7%" />
                                                </asp:ButtonField>
                                                
                                                <asp:TemplateField  HeaderText="Eliminar">
                                                    <ItemTemplate>
                                                        <center>
                                                           
                                                        </center>                
                                                    </ItemTemplate>                                                    
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="10%"  />
                                                </asp:TemplateField>
                                                
                                                 <asp:BoundField DataField="Partida" HeaderText="Partida">
                                                        <HeaderStyle HorizontalAlign="Center" Width="10%"  />
                                                        <ItemStyle HorizontalAlign="Center" Width="10%"  />
                                                </asp:BoundField>
                                                
                                                <asp:BoundField DataField="Ampliacion" HeaderText="Ampliación" DataFormatString="{0:n}" >
                                                        <HeaderStyle HorizontalAlign="Right" Width="15%"  />
                                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                </asp:BoundField>
                                                
                                                <asp:BoundField DataField="Reduccion" HeaderText="Reducción" DataFormatString="{0:n}">
                                                        <HeaderStyle HorizontalAlign="Right" Width="15%"  />
                                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                </asp:BoundField>
                                                
                                                <asp:BoundField DataField="Incremento" HeaderText="Incremento" DataFormatString="{0:n}" >
                                                        <HeaderStyle HorizontalAlign="Right" Width="15%"  />
                                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                </asp:BoundField>
                                                
                                                <asp:BoundField DataField="Disponible" HeaderText="Disponible" DataFormatString="{0:n}" >
                                                        <HeaderStyle HorizontalAlign="Right" Width="10%"  />
                                                        <ItemStyle HorizontalAlign="Right" Width="10%"  />
                                                </asp:BoundField>
                                               
                                               <asp:BoundField DataField="Fuente_Financiamiento" HeaderText="Fuente de Financiamiento"  >
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center"   />
                                                </asp:BoundField>
                                                
                                                <asp:BoundField DataField="Programa" HeaderText="Programa"  >
                                                        <HeaderStyle HorizontalAlign="Center"  />
                                                        <ItemStyle HorizontalAlign="Center"   />
                                                </asp:BoundField>
                                                
                                                <asp:BoundField DataField="Unidad_Responsable" HeaderText="Unidad Responsable" >
                                                        <HeaderStyle HorizontalAlign="Center"  />
                                                        <ItemStyle HorizontalAlign="Center"   />
                                                </asp:BoundField>
                                            </Columns>
                                                   
                                                   <AlternatingRowStyle CssClass="GridAltItem" />
                                                   <HeaderStyle CssClass="GridHeader" />
                                                   <PagerStyle CssClass="GridHeader" />
                                                   <SelectedRowStyle CssClass="GridSelected" />
                                                   
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                            
                        </ContentTemplate>
                 </cc1:TabPanel>        
             </cc1:TabContainer> 
             <table width="100%">
                 <tr>
                     <td  width="18%"  align="right">
                     </td>
                    
                    <td  width="18%"  align="right">
                        <asp:Label ID="Lbl_Sumas" runat="server" Text="Suma" Width="100%"  ></asp:Label> 
                    </td>
                    <td  width="18%" align="right">
                        <asp:TextBox ID="Txt_Suma_Ampliacion"  runat="server" Text="0" ReadOnly="true" ToolTip="Suma de Ampliación" Width="95%"  ></asp:TextBox>
                         
                    </td>
                    <td  width="18%" align="right">
                        <asp:TextBox ID="Txt_Suma_Reduccion"  runat="server" Text="0" ReadOnly="true" ToolTip="Suma de Reducción" Width="95%" ></asp:TextBox>
                        
                    </td>
                    <td width="18%"></td>
                    <td width="18%"></td>
                </tr>
             </table>
              
         </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

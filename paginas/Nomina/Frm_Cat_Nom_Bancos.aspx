<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Bancos.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Bancos" Title="Catálogo Bancos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
    <script src="../../javascript/Js_Cat_Nom_Bancos.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="SM_Bancos" runat="server" />
    <asp:UpdatePanel ID="UPnl_Bancos" runat="server">
        <ContentTemplate>        
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Bancos" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Antiguedad_Sindicato" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Bancos</td>
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
                                                OnClientClick="return confirm('¿Está seguro de eliminar el Banco seleccionado?');"/>
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="6"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                      </td>
                                      <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="width:60%;vertical-align:top;">
                                                     B&uacute;squeda
                                                    <asp:TextBox ID="Txt_Busqueda_Bancos" runat="server" MaxLength="100"  TabIndex="21"
                                                        ToolTip = "Busquedad de Bancos" Width="180px"/>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Bancos" 
                                                        runat="server" WatermarkCssClass="watermarked"
                                                        WatermarkText="<Banco_ID ó Nombre>" 
                                                        TargetControlID="Txt_Busqueda_Bancos" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Bancos" 
                                                        runat="server" TargetControlID="Txt_Busqueda_Bancos" 
                                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                        ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                    <asp:ImageButton ID="Btn_Busqueda_Bancos" runat="server" TabIndex="22"
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar" 
                                                        onclick="Btn_Busqueda_Bancos_Click"
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
                            Banco ID
                        </td>
                        <td  style="text-align:left;width:30%;" >
                            <asp:TextBox ID="Txt_Banco_ID" runat="server" Width="98%" TabIndex="0"/>
                        </td> 
                         <td style="text-align:left;width:20%;">
                        </td>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            <b>*</b>No Cuenta
                        </td>
                        <td  style="text-align:left;width:30%;" >
                            <asp:TextBox ID="Txt_No_Cuenta" runat="server" Width="98%" TabIndex="1" MaxLength="20"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Cuenta" 
                                runat="server" TargetControlID="Txt_No_Cuenta" FilterType="Numbers" />
                        </td> 
                         <td style="text-align:left;width:20%;">
                            &nbsp;&nbsp;<b>*</b>Tipo
                        </td>
                        <td  style="text-align:left;width:30%;" >
                            <asp:DropDownList ID="Cmb_Tipo" runat="server" Width="100%">
                                <asp:ListItem Value="">&lt;-- Seleccione -- &gt;</asp:ListItem>
                                <asp:ListItem Value="NOMINA">NOMINA</asp:ListItem>
                                <asp:ListItem Value="INGRESOS">INGRESOS</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr> 
                    <tr>
                        <td style="text-align:left;width:20%;">
                            <b>*</b>Nombre
                        </td>
                        <td  style="text-align:left;width:80%;" colspan="3">
                            <asp:TextBox ID="Txt_Nombre_Banco" runat="server" Width="99.5%" TabIndex="2" MaxLength="100"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Banco" 
                                runat="server" TargetControlID="Txt_Nombre_Banco" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ "/>                            
                        </td>                                                                      
                    </tr>            
                    <tr>
                        <td style="text-align:left;width:20%;">    
                            <b>*</b>Sucursal                   
                        </td>
                        <td  style="text-align:left;width:30%;"> 
                            <asp:TextBox ID="Txt_Sucursal" runat="server" Width="98%" TabIndex="3" MaxLength="100"/>    
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Sucursal" 
                                runat="server" TargetControlID="Txt_Sucursal" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ "/>                                                                                                                                                                                  
                        </td>  
                        <td style="text-align:left;width:20%;">    
                            &nbsp;&nbsp;Referencia                   
                        </td>
                        <td  style="text-align:left;width:30%;"> 
                            <asp:TextBox ID="Txt_Referencia" runat="server" Width="98%" TabIndex="4" MaxLength="30"/>    
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Referencia" 
                                runat="server" TargetControlID="Txt_Referencia" FilterType="Numbers" />                                                                                                                                                                                  
                        </td>                                                                       
                    </tr>                               
                    <tr>
                        <td style="text-align:left;width:20%;vertical-align:top;">
                           Comentarios
                        </td>
                        <td  style="text-align:left;width:30%;" colspan="3">
                            <asp:TextBox ID="Txt_Comentarios" runat="server" Width="99.5%" MaxLength="100" TextMode="MultiLine" TabIndex="5" Height="45px"
                                Wrap="true"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Comentarios" runat="server"  TargetControlID="Txt_Comentarios"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>    
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios" runat="server" TargetControlID ="Txt_Comentarios" 
                                WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/>     
                            <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>
                </table>
                
                <table style="width:98%;" id="Tbl_Plan_Pagos" runat="server">
                    <tr>
                        <td class="button_autorizar" style="text-align:left; width:20%;cursor:default;">
                            Plan Pagos
                        </td>
                        <td  class="button_autorizar" style="text-align:left; width:30%;">
                            <asp:DropDownList ID="Cmb_Plan_Pagos" runat="server" Width="100%">
                                <asp:ListItem Value="">&lt;-- Seleccione -- &gt;</asp:ListItem>
                                <asp:ListItem Value="NORMAL">NORMAL</asp:ListItem>
                                <asp:ListItem Value="MESES_SIN_INTERESES">MESES SIN INTERESES</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td  class="button_autorizar" style="text-align:left; width:20%;cursor:default;">
                            &nbsp;&nbsp;No Meses
                        </td>
                        <td  class="button_autorizar" style="text-align:left; width:30%;">
                            <asp:DropDownList ID="Cmb_No_Meses" runat="server" Width="100%"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>
                </table>
                        
                <asp:GridView ID="Grid_Bancos" runat="server" CssClass="GridView_1" Width="98%"
                     AutoGenerateColumns="False"  GridLines="None" AllowPaging="true" PageSize="5"
                     onpageindexchanging="Grid_Bancos_PageIndexChanging"
                     OnSelectedIndexChanged="Grid_Bancos_SelectedIndexChanged"
                     AllowSorting="True" OnSorting="Grid_Bancos_Sorting" HeaderStyle-CssClass="tblHead">
                        <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select"  HeaderText="Seleccionar"
                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                <ItemStyle Width="15%"  HorizontalAlign="Left"/>
                                <HeaderStyle HorizontalAlign="Left" Width="15%"/>
                            </asp:ButtonField>                                                
                            <asp:BoundField DataField="BANCO_ID" HeaderText="Identificador" SortExpression="BANCO_ID">
                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </asp:BoundField>                                                   
                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE">
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


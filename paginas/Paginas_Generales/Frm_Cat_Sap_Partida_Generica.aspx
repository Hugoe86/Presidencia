<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Sap_Partida_Generica.aspx.cs" Inherits="paginas_Paginas_Generales_Frm_Cat_Sap_Partida_Generica" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="SM_Sap_Partidas_Genericas" runat="server" />
    <asp:UpdatePanel ID="UPnl_Sap_Partidas_Genericas" runat="server">
        <ContentTemplate>        
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Sap_Partidas_Genericas" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Sap_Partidas_Genericas" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Partidas Genericas</td>
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
                             <div align="right" style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >                        
                                  <table style="width:100%;height:28px;">
                                    <tr>
                                      <td align="left" style="width:59%;">                                                  
                                           <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="3" 
                                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="4"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" TabIndex="5"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                                OnClientClick="return confirm('¿Está seguro de eliminar la partida generica seleccionada?');"/>
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="6"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                                      </td>
                                      <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="width:60%;vertical-align:top;">
                                                     B&uacute;squeda
                                                    <asp:TextBox ID="Txt_Busqueda_Sap_Partidas_Genericas" runat="server" MaxLength="100"  TabIndex="21"
                                                        ToolTip = "Busquedad de Sap Partidas Genericas" Width="180px"/>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Sap_Partidas_Genericas" 
                                                        runat="server" WatermarkCssClass="watermarked"
                                                        WatermarkText="<Partida_Genrica_ID ó Clave>" 
                                                        TargetControlID="Txt_Busqueda_Sap_Partidas_Genericas" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Sap_Partidas_Genericas" 
                                                        runat="server" TargetControlID="Txt_Busqueda_Sap_Partidas_Genericas" 
                                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                        ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                    <asp:ImageButton ID="Btn_Busqueda_Sap_Partidas_Genericas" runat="server" TabIndex="22"
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar" 
                                                        onclick="Btn_Busqueda_Sap_Partidas_Genericas_Click"
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
                        <td style="text-align:left;width:20%;">
                            Partida Generica ID
                        </td>
                        <td  style="text-align:left;width:30%;" >
                            <asp:TextBox ID="Txt_Partida_Generica_ID" runat="server" Width="98%" TabIndex="0"/>
                        </td> 
                         <td style="text-align:left;width:20%;">
                        </td>
                        <td  style="text-align:left;width:30%;" >                                                
                        </td>                                                                       
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            <b>*</b>Cap&iacute;tulo
                        </td>
                        <td  style="text-align:left;width:30%;"> 
                            <asp:DropDownList ID="Cmb_Sap_Capitulo" runat="server" Width="100%" AutoPostBack="true" 
                                OnSelectedIndexChanged="Cmb_Sap_Capitulo_SelectedIndexChanged"/>                                                                                                                    
                        </td>  
                        <td style="text-align:left;width:20%;">    
                            &nbsp;&nbsp;<b>*</b>Concepto               
                        </td>
                        <td  style="text-align:left;width:30%;"> 
                            <asp:DropDownList ID="Cmb_Sap_Conceptos" runat="server" Width="100%" AutoPostBack="true" 
                                OnSelectedIndexChanged="Cmb_Sap_Conceptos_SelectedIndexChanged" />                                                                                                                             
                        </td>                                                                      
                    </tr>                       
                    <tr>
                        <td style="text-align:left;width:20%;">
                            <b>*</b>Clave
                        </td>
                        <td  style="text-align:left;width:30%;" >
                            <asp:TextBox ID="Txt_Clave" runat="server" Width="98%" TabIndex="1" MaxLength="20" AutoPostBack="true"
                                OnTextChanged="Txt_Clave_TextChanged"/>  
                            <cc1:FilteredTextBoxExtender ID="FTxt_Clave" runat="server"  TargetControlID="Txt_Clave"
                                FilterType="Numbers"/>                                  
                        </td> 
                         <td style="text-align:left;width:20%;">
                            &nbsp;&nbsp;<b>*</b>Estatus
                        </td>
                        <td  style="text-align:left;width:30%;" >
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                <asp:ListItem>&lt;<-- Seleccione -->&gt;</asp:ListItem>
                                <asp:ListItem>ACTIVO</asp:ListItem>
                                <asp:ListItem>INACTIVO</asp:ListItem>
                            </asp:DropDownList>                                                   
                        </td>                                                                       
                    </tr>                                                              
                    <tr>
                        <td style="text-align:left;width:20%;vertical-align:top;">
                           Descripci&oacute;n
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
                </table>
                <asp:HiddenField ID="Txt_Clave_Oculta" runat="server" />  
                <br />
                <div style="overflow:auto;height:320px;width:98%;vertical-align:top;border-style:outset;border-color: Silver;" >        
                <asp:GridView ID="Grid_Sap_Partidas_Genericas" runat="server" CssClass="GridView_1" Width="100%"
                     AutoGenerateColumns="False"  GridLines="None" AllowPaging="false" PageSize="5"
                     onpageindexchanging="Grid_Sap_Partidas_Genericas_PageIndexChanging"
                     OnSelectedIndexChanged="Grid_Sap_Partidas_Genericas_SelectedIndexChanged"
                     AllowSorting="true" OnSorting="Grid_Sap_Partidas_Genericas_Sorting" >
                        <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select"  HeaderText=""
                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                <ItemStyle HorizontalAlign="Left"/>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"/>
                            </asp:ButtonField>                                                
                            <asp:BoundField DataField="PARTIDA_GENERICA_ID" HeaderText="Identificador" SortExpression="PARTIDA_GENERICA_ID">
                                <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                <ItemStyle HorizontalAlign="Left" Width="0%" />
                            </asp:BoundField>                                                   
                            <asp:BoundField DataField="CLAVE" HeaderText="Clave" SortExpression="CLAVE">
                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </asp:BoundField>  
                            <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" SortExpression="DESCRIPCION">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>                             
                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS">
                                <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                <ItemStyle HorizontalAlign="Left" Width="12%" />
                            </asp:BoundField>                                                                                                                                          
                        </Columns>
                        <SelectedRowStyle CssClass="GridSelected" />
                        <PagerStyle CssClass="GridHeader" />
                        <HeaderStyle CssClass="GridHeader" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                </asp:GridView>   
                </div>
                    <br /><br /><br /><br />                                                                
            </div>            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


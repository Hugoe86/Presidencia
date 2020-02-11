<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Estados_Predio.aspx.cs" Inherits="paginas_predial_Frm_Cat_Pre_Estados_Predio" Title="Catalogo de Estados Predio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Estados_Predio" runat="server" />  
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
                        <td class="label_titulo" colspan="2">Catálogo de Estados Predio</td>
                    </tr>
                    <tr>
                        <td>
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
                        <td align="left">
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
                        <td>Búsqueda:
                            <asp:TextBox ID="Txt_Busqueda_Estado_Predio" runat="server" Width="130px"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Estado_Predio" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                onclick="Btn_Buscar_Estado_Predio_Click" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Estado_Predio" runat="server" 
                                WatermarkText="<Descripcion>" TargetControlID="Txt_Busqueda_Estado_Predio" 
                                WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>  
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Estado_Predio" runat="server" TargetControlID="Txt_Busqueda_Estado_Predio" InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>      
                              
                        </td>                        
                    </tr>
                </table>   
                <br />
                <center>
                    <table width="98%">
                        <tr>
                            <td colspan="4">
                                <asp:HiddenField ID="Hdf_Estado_Predio_ID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:18%; text-align:left;">
                                <asp:Label ID="Lbl_ID_Estado_Predio" runat="server" Text="Estado Predio ID" CssClass="estilo_fuente"></asp:Label></td>
                            <td style="width:32%; text-align:left;">
                                <asp:TextBox ID="Txt_ID_Estado_Predio" runat="server" Width="150px" MaxLength="10" Enabled="False"></asp:TextBox>
                            </td>
                            <td colspan="2">&nbsp;</td>
                        </tr>                                    
                        <tr>
                            <td style="width:18%; text-align:left; ">
                               <asp:Label ID="Lbl_Descripcion_Estado_Predio" runat="server" 
                                    Text="* Descripción" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3" style="text-align:left; ">
                                <asp:TextBox ID="Txt_Descripcion_Estado_Predio" runat="server" Width="98%" MaxLength="20"  style="text-transform:uppercase"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />                                
                    <asp:GridView ID="Grid_Estados_Predio" runat="server" CssClass="GridView_1"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="5" Width="96%"
                        GridLines= "None"
                        onselectedindexchanged="Grid_Estados_Predio_SelectedIndexChanged" 
                        onpageindexchanging="Grid_Estados_Predio_PageIndexChanging">
                        <RowStyle CssClass="GridItem" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png"  >
                                <ItemStyle Width="30px" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="ESTADO_PREDIO_ID" HeaderText="Estado Predio ID" 
                                SortExpression="ESTADO_PREDIO_ID" />
                            <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" 
                                SortExpression="DESCRIPCION" />
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
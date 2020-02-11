<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Otros_Pagos.aspx.cs" Inherits="paginas_predial_Frm_Cat_Pre_Otros_Pagos" Title="Catalogo de Otros Pagos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 838px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Otros_Pagos" runat="server" />
    <asp:UpdatePanel ID="Upd_Otros_Pagos" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Otros_Pagos" DisplayAfter="0">
            <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Otros_Pagos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Cat&aacute;logo de Otros Pagos</td>
                    </tr>
                    <tr>
                        <td class="style1">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top" >
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text=""  CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                          
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left" class="style1">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Nuevo" OnClick = "Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Modificar" OnClick ="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Eliminar" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro de la Base de Datos?');"
                                onclick = "Btn_Eliminar_Click"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" OnClick = "Btn_Salir_Click"/>
                        </td>
                        <td align="right" class="style1">Búsqueda:
                            <asp:TextBox ID="Txt_Busqueda_Otros_Pagos" runat="server" Width="130px"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Otros_Pagos" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                OnClick = "Btn_Buscar_Otros_Pagos_Click" /> 
                        </td>                        
                    </tr>
                </table>   
                <br />
                        <center>
                                <table width="100%">
                                    <tr>
                                        <td style="width:18%; text-align:left;">
                                            <asp:Label ID="Lbl_Otros_Pagos_ID" runat="server" CssClass="estilo_fuente" 
                                                Text="Otro Pago ID"></asp:Label>
                                        </td>
                                        <td style="width:32%">
                                            <asp:TextBox ID="Txt_Otros_Pagos_ID" runat="server" Enabled="False" 
                                                MaxLength="10" Width="98%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Concepto" runat="server" 
                                                Text="* Concepto" CssClass="estilo_fuente"></asp:Label></td>
                                        <td style="width:35%">
                                            <asp:TextBox ID="Txt_Concepto" runat="server" Width="98%" MaxLength="10"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Estatus" runat="server" CssClass="estilo_fuente" 
                                                Text="* Estatus"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE"></asp:ListItem>
                                                <asp:ListItem Text="VIGENTE" Value="VIGENTE"></asp:ListItem>
                                                <asp:ListItem Text="BAJA" Value="BAJA"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td style="width:15%; text-align:left; vertical-align: top;">
                                            <asp:Label ID="Lbl_Descripcion" runat="server" Text="* Descripción" 
                                                CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Descripcion" runat="server" Width="98%" Rows="3" 
                                                TextMode="MultiLine"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Descripcion" runat="server" 
                                                Enabled="True" TargetControlID="Txt_Descripcion" 
                                                WatermarkCssClass="watermarked" WatermarkText="Límite de Caractes 250">
                                            </cc1:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                </table>
                                <br />                                
                                <asp:GridView ID="Grid_Otros_Pagos" runat="server" CssClass="GridView_1"
                                    AutoGenerateColumns="False" AllowPaging="True" PageSize="5" Width="98%"
                                    EmptyDataText="&quot;No se encontraron registros&quot;"
                                    onselectedindexchanged="Grid_Otros_Pagos_SelectedIndexChanged" 
                                    GridLines= "None"
                                    onpageindexchanging="Grid_Otros_Pagos_PageIndexChanging">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="30px" />
                                        </asp:ButtonField>
                                        
                                        <asp:BoundField DataField="PAGO_ID" HeaderText="Pago ID" 
                                            SortExpression="PAGO_ID">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>  
                                        
                                        <asp:BoundField DataField="CONCEPTO" HeaderText="Concepto" 
                                            SortExpression="CONCEPTO">
                                        <HeaderStyle HorizontalAlign="Left" Width="24%" />
                                        <ItemStyle HorizontalAlign="Left" Width="24%" />
                                        </asp:BoundField>  
    
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" 
                                            SortExpression="ESTATUS">
                                        <HeaderStyle HorizontalAlign="Left" Width="24%" />
                                        <ItemStyle HorizontalAlign="Left" Width="24%" />
                                        </asp:BoundField>  

                                        <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion" 
                                            SortExpression="DESCRIPCION">
                                        <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                        <ItemStyle HorizontalAlign="Left" Width="50%" />
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
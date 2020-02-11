<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Ciudades.aspx.cs" Inherits="paginas_predial_Frm_Cat_Pre_Ciudades" Title="Catalogo de Ciudades" %>

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
    <asp:ScriptManager ID="ScptM_Ciudades" runat="server" />
    <asp:UpdatePanel ID="Upd_Ciudades" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Ciudades" DisplayAfter="0">
            <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Ciudades" style="background-color:#ffffff; width:100%; height:100%;">
                <table  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Cat&aacute;logo de Ciudades</td>
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
                                AlternateText="Modificar" onclick = "Btn_Modificar_Click"  />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Eliminar" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro de la Base de Datos?');"
                                OnClick = "Btn_Eliminar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" OnClick = "Btn_Salir_Click"/>
                        </td>
                        <td align="right" class="style1">Búsqueda:
                            <asp:TextBox ID="Txt_Busqueda_Ciudades" runat="server" Width="130px"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Ciudades" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                OnClick = "Btn_Buscar_Click" /> 
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Ciudades" runat="server" WatermarkText="<Ciudad>" TargetControlID="Txt_Busqueda_Ciudades" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>                                
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Ciudades" runat="server" TargetControlID="Txt_Busqueda_Ciudades" InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender> 
                        </td>                        
                    </tr>
                </table>   
                <br />
                        <center>
                        <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align:left;width:10%;">
                            <asp:TextBox ID="Txt_Ciudad_ID" runat="server" Width="40%"
                            visible = "false" />
                        </td>
                    
                    <tr>
                       <td style="text-align:left;width:10%;text-align:left;">
                            *Estado
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estados" runat="server" Width="99%" TabIndex="7"
                            AutoPostBack = "true">
                            </asp:DropDownList>
                        </td>
                         <td style="text-align:left;width:20%;text-align:right;">
                            *Clave
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Clave" runat="server" Width="96.4%"/>
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="text-align:left;width:20%;text-align:left;">
                            *Nombre
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Nombre" runat="server" Width="96.4%"/>
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            *Estatus
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%" TabIndex="7">
                                <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE"></asp:ListItem>
                                <asp:ListItem Text="VIGENTE" Value="VIGENTE"></asp:ListItem>
                                <asp:ListItem Text="BAJA" Value="BAJA"></asp:ListItem> 
                            </asp:DropDownList>
                        </td>
                    </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <tr align="center">
                                        <td colspan="4">
                                            <asp:GridView ID="Grid_Ciudades" runat="server" AllowPaging="True" AllowSorting="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                                HeaderStyle-CssClass="tblHead" PageSize="5" style="white-space:normal;" 
                                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                                Width="100%"
                                                OnSelectedIndexChanged = "Grid_Ciudades_SelectedIndexChanged"
                                                OnPageIndexChanging = "Grid_Ciudades_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="CIUDAD_ID" HeaderStyle-Width="15%" HeaderText="Ciudad ID">
                                                    <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="ESTADO_ID" HeaderStyle-Width="15%" HeaderText="Estado ID">
                                                    <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="CLAVE" HeaderStyle-Width="15%" HeaderText="Clave">
                                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                                    <HeaderStyle HorizontalAlign="Center" Width="40%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="ESTATUS" HeaderStyle-Width="15%" HeaderText="Estatus">
                                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="ESTADO" HeaderText="Estado">
                                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:BoundField>
                                                                                                      
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="tblHead" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                            <br />
                                        </td>
                                    </tr>
                                </td>
                            </tr>
                </table>

                        </center>
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
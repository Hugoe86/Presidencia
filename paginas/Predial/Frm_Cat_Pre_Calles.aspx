<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Calles.aspx.cs" Inherits="paginas_predial_Frm_Cat_Pre_Calles" Title="Catalogo de Calles" %>
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
    <script type='text/javascript' >
        function Abrir_Ventana_Modal(Url, Propiedades) {
            window.showModalDialog(Url, null, Propiedades);
        }
    </script>
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
            
            <div id="Div_Calles" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Cat&aacute;logo de Calles</td>
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
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" TabIndex="1" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Nuevo" OnClick = "Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server" TabIndex="2"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Modificar" onclick = "Btn_Modificar_Click"  />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" TabIndex="3"
                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Eliminar" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro de la Base de Datos?');"
                                OnClick = "Btn_Eliminar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" TabIndex="4"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" OnClick = "Btn_Salir_Click"/>
                        </td>
                        <td align="right" class="style1">Búsqueda:
                            <asp:HiddenField ID="Hf_Filtro_Busqueda" runat="server" />
                            <asp:HiddenField ID="Hf_Busqueda" runat="server" />
                            <asp:DropDownList ID="Cmb_Filtro_Busqueda" runat="server" Width="25%" TabIndex="5">
                                <asp:ListItem Text="CALLE" Value="CALLE"></asp:ListItem>
                                <asp:ListItem Text="COLONIA" Value="COLONIA"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="Txt_Busqueda_Calles" runat="server"
                                Width="130px" TabIndex="6" ></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Calles" runat="server" 
                                CausesValidation="false" TabIndex="7"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                OnClick = "Btn_Buscar_Click" /> 
                        </td>                        
                    </tr>
                </table>   
                <br />
                        <center>
                        <table width="98%" class="estilo_fuente">
                    <tr>
                        <td style="text-align:left;width:20%;">
                            <asp:TextBox ID="Txt_Calle_ID" runat="server" Width="96.4%" TabIndex="8"
                            visible = "false" />
                        </td>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Colonia
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Colonia" runat="server" Width="98%" 
                                TabIndex="9" ></asp:TextBox>
                            <asp:HiddenField ID="Hdn_Colonia_ID" runat="server" />
                            <asp:HiddenField ID="Hdn_Colonia_ID_Anterior" runat="server" />
                        </td>
                        <td colspan="2">
                            <asp:ImageButton ID="Btn_Mostrar_Busqueda_Avanzada_Colonias" runat="server"
                                Height="24px" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" 
                                OnClick="Btn_Mostrar_Busqueda_Avanzada_Colonias_Click"
                                TabIndex="10" ToolTip="Búsqueda avanzada de colonias" Width="24px" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;text-align:left;">
                            Clave
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Clave" runat="server" Width="96.4%" TabIndex="11"/>
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            *Estatus
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" TabIndex="12">
                                <asp:ListItem Text="&lt;SELECCIONE&gt;" Value="SELECCIONE"></asp:ListItem>
                                <asp:ListItem Text="VIGENTE" Value="VIGENTE"></asp:ListItem>
                                <asp:ListItem Text="BAJA" Value="BAJA"></asp:ListItem> 
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%; vertical-align:top;">
                            *Nombre
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Nombre" runat="server" TabIndex="13" MaxLength="250"
                                Width="98.6%" style="text-transform:uppercase;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%; vertical-align:top;">
                            Comentarios
                        </td>
                        <td style="text-align:left;width:80%; vertical-align:top;" colspan="3" >
                            <asp:TextBox ID="Txt_Comentarios" runat="server" TabIndex="14" MaxLength="250"
                                TextMode="MultiLine" Width="98.6%" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios" runat="server" TargetControlID ="Txt_Comentarios" 
                                WatermarkText="Límite de Caracteres 250" WatermarkCssClass="watermarked"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios" 
                                runat="server" TargetControlID="Txt_Comentarios" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ "/>
                        </td>
                    </tr>
                            <tr>
                                <td align="center" >
                                    <tr align="center">
                                        <td colspan="4">
                                            <asp:GridView ID="Grid_Calles" runat="server" AllowPaging="True" AllowSorting="True" 
                                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                                HeaderStyle-CssClass="tblHead" PageSize="5" style="white-space:normal;" 
                                                Width="100%"
                                                OnSelectedIndexChanged = "Grid_Calles_SelectedIndexChanged"
                                                OnPageIndexChanging = "Grid_Calles_PageIndexChanging">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                        <ItemStyle HorizontalAlign="Center" Width="3.5%" />
                                                    
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="CALLE_ID" HeaderStyle-Width="15%" HeaderText="Calle ID">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="CLAVE" HeaderStyle-Width="15%" HeaderText="Clave">
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre Calle">
                                                    <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="COLONIA" HeaderText="Colonia">
                                                    <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="ESTATUS" HeaderStyle-Width="15%" HeaderText="Estatus">
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="COMENTARIOS" HeaderStyle-Width="15%" HeaderText="Comentarios">
                                                    <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    
                                                    <asp:BoundField DataField="COLONIA_ID" HeaderText="Colonia_ID" Visible="false" />
                                                    
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
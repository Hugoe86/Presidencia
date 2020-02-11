<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" CodeFile="Frm_Cat_Pre_Gastos_Ejecucion.aspx.cs" Inherits="paginas_Predial_Frm_Cat_Pre_Gastos_Ejecucion" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel_Padron_Predios" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Padrones" runat="server" AssociatedUpdatePanelID="Upd_Panel_Padron_Predios" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="General" style="background-color:#ffffff; width:100%; height:100%;">
                <table id="Tbl_Comandos" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                            <tr>
                                <td class="label_titulo" colspan="2">
                                    Catálogo de Gastos de Ejecución</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div ID="Div_Contenedor_Error" runat="server">
                                        <tr>
                                            <asp:Image ID="Img_Error" runat="server" 
                                                ImageUrl="../imagenes/paginas/sias_warning.png" />
                                            <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" 
                                                CssClass="estilo_fuente_mensaje_error" Text="" />
                                            <caption>
                                                <br />
                                                <asp:Label ID="Lbl_Error" runat="server" CssClass="estilo_fuente_mensaje_error" 
                                                    TabIndex="0" Text=""></asp:Label>
                                            </caption>
                                        </tr>
                                    </div>
                                </td>
                            </tr>
                                </div>
                            </tr>
                            <tr class="barra_busqueda">
                                <td style="width:50%">
                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                    CssClass="Img_Button" onclick="Btn_Nuevo_Click" />
                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                    CssClass="Img_Button" onclick="Btn_Modificar_Click" />
                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                    CssClass="Img_Button" 
                                        OnClientClick="return confirm('¿Esta seguro de Eliminar el presente registro?');" 
                                        onclick="Btn_Eliminar_Click" ToolTip="Eliminar" />
                                    <asp:ImageButton ID="Btn_Salir" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" CssClass="Img_Button" 
                                        onclick="Btn_Salir_Click1"/>
                                </td>
                                <td align="right" style="width:50%">
                                    Búsqueda:
                                    <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"
                                    ToolTip="Buscar" TabIndex="1" ></asp:TextBox>
                                    <asp:ImageButton ID="Btn_Busqueda" runat="server" 
                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                        TabIndex="2" onclick="Btn_Buscar_Gastos_Ejecucion_Click"/>
                                        <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                        WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda"/>
                                </td>                        
                            </tr>
                        </table>
                <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                    <tr>
                        <td style="width:18%">&nbsp;</td>
                        <td style="width:32%">&nbsp;</td>
                        <td style="width:18%">&nbsp;</td>
                        <td style="width:32%">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                            </td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Id" runat="server" Width="92%" Visible="false"></asp:TextBox>
                        </td>
                        <td style="width:18%">
                        </td>
                        <td style="width:32%">
                        </td>
                    </tr>
                    <tr>
                        <td style="width:18%">*Clave</td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Clave" runat="server" Width="92%" MaxLength="5"></asp:TextBox>
                        </td>
                        <td style="width:18%">*Estatus</td>
                        <td style="width:32%">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="94%">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                <asp:ListItem Text="BAJA" Value="BAJA" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                            *Nombre</td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Nombre" runat="server" Width="92%" MaxLength="50"></asp:TextBox></td>
                        <td style="width:18%">
                            </td>
                        <td style="width:32%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width:18%">*Descripción</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Descripcion" runat="server" Height="60px" 
                                TextMode="MultiLine" Width="97%" MaxLength="250"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Descripcion" runat="server" 
                                TargetControlID="Txt_Descripcion" WatermarkCssClass="watermarked" 
                                WatermarkText="Límite de Caractes 250">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="Txt_Descripcion_FilteredTextBoxExtender" 
                                runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                TargetControlID="Txt_Descripcion" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td style="width:32%">
                            &nbsp;</td>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td style="width:32%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            
                            <asp:GridView ID="Grid_Gastos" runat="server" AllowPaging="true" 
                                AutoGenerateColumns="False" CssClass="GridView_1" 
                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                GridLines="none"                                 
                                PageSize="5" Style="white-space:normal" Width="96%" 
                                onpageindexchanging="Grid_Gastos_PageIndexChanging" 
                                onselectedindexchanged="Grid_Gastos_SelectedIndexChanged" >
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>                                    
                                    <asp:BoundField DataField="GASTO_EJECUCION_ID" HeaderText="Id" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CLAVE" HeaderText="Clave">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>                                    
                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>                                    
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripcion">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                            
                        </td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td style="width:32%">
                            &nbsp;</td>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td style="width:32%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td style="width:32%">
                            &nbsp;</td>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td style="width:32%">
                            &nbsp;</td>
                    </tr>
                
                
                </table> 
            </div>
                        
            </ContentTemplate>                        
    </asp:UpdatePanel>
    


</asp:Content>
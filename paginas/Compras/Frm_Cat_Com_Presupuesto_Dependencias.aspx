<%@ Page Title="Catálogo de Dependencias Presupuesto" Language="VB" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="false" CodeFile="Frm_Cat_Com_Presupuesto_Dependencias.aspx.vb" Inherits="paginas_Compras_Frm_Cat_Com_Presupuesto_Dependencias" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div style="width: 95%;">
        <asp:UpdatePanel ID="Upl_Contenedor" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upl_Contenedor"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressTemplate">
                        </div>
                        <div class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />                           
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table width="98%" border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">Catálogo de Dependencias Presupuesto</td>
                    </tr>
                    <tr align="left">
                        <td colspan="4" >
                            <asp:Image ID="Img_Warning" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />
                            <asp:Label ID="Lbl_Informacion" runat="server" 
                                ForeColor="#990000"></asp:Label>                        
                        </td>
                    </tr>                    
                    <tr class="barra_busqueda" align="right">
                        <td colspan="4" align="left" valign="middle">
                           <table width = "100%">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                        CssClass="Img_Button" ToolTip="Nuevo" />
                                    <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                        CssClass="Img_Button" ToolTip="Modificar" />
                                    <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                        CssClass="Img_Button" ToolTip="Eliminar" 
                                            
                                            OnClientClick="return confirm('¿Está seguro de eliminar el impuesto seleccionado?');" />
                                    <asp:ImageButton ID="Btn_Salir" runat="server" 
                                        CssClass="Img_Button" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            ToolTip="Inicio" />
                                </td>
                                <td align = "right">
                                    Búsqueda por:
                                    <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Impuesto_ID" runat="server" WatermarkCssClass="watermarked"
                                        WatermarkText="<Ingrese nombre del impuesto>" TargetControlID="Txt_Busqueda" />
                                    <asp:ImageButton ID="Btn_Buscar" runat="server" ToolTip="Consultar"
                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" TabIndex="1" />
                                </td>
                            </tr>
                           </table>                           
                        </td>                        
                    </tr>
                    <tr>
                        <td width="145px">Dependiencia</td>
                        <td colspan="3">
                            <asp:DropDownList ID="Cmb_Dependencias" runat="server" Width="605px">
                            </asp:DropDownList>
                        </td>                        
                    </tr>
                    <tr>
                        <td>*Año Presupuesto</td>
                        <td>
                            <asp:DropDownList ID="Cmb_Año_Presupuesto" runat="server" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td width="145px">
                            *Monto Comprometido</td>
                        <td>
                            <asp:TextBox ID="Txt_Monto_Comprometido" runat="server" Enabled="false" 
                                ReadOnly="True" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>*Monto Presupuestal</td>
                        <td>
                            <asp:TextBox ID="Txt_Monto_Presupuestal" runat="server" Width="150px"></asp:TextBox>
                            <cc1:MaskedEditExtender ID="Txt_Monto_Presupuestal_MaskedEditExtender" runat="server" 
                                TargetControlID="Txt_Monto_Presupuestal" Mask="999.99" MaskType="Number" 
                                InputDirection="RightToLeft" AcceptNegative="None" DisplayMoney="None"  
                                ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" 
                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                        </td>
                        <td>
                            *Monto Disponible</td>
                        <td>
                            <asp:TextBox ID="Txt_Monto_Disponible" runat="server" Enabled="false" 
                                ReadOnly="True" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Comentarios</td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Comentarios" runat="server" Enabled="False" 
                                MaxLength="250" TabIndex="4" TextMode="MultiLine" Width="605px"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="Txt_Comentarios_TextBoxWatermarkExtender" runat="server" 
                                TargetControlID="Txt_Comentarios" WatermarkCssClass="watermarked" 
                                WatermarkText="&lt;Límite de Caracteress 250&gt;" />
                            <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                TargetControlID="Txt_Comentarios" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_Dependencias" runat="server" AutoGenerateColumns="False" 
                                CssClass="GridView_1" AllowPaging="True" PageSize="5" 
                                GridLines="None" Width = "98%">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="2px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="NOMBRE" HeaderText="Dependencia" 
                                        Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Año_Presupuesto" HeaderText="Año Presupuesto" 
                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Monto_Comprometido" HeaderText="Monto Comprometido" 
                                        Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Monto_Presupuestal" 
                                        HeaderText="Monto Presupuestal">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>                                        
                                    <asp:BoundField DataField="Monto_Disponible" HeaderText="Monto Disponible" />
                                    <asp:BoundField DataField="COMENTARIOS" HeaderText="Comentarios" Visible="True">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />                                
                                <AlternatingRowStyle CssClass="GridAltItem" />                                
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr style ="height:"50px">
                        <td colspan = "4"></td>
                    </tr>
                </table>
            </ContentTemplate>
    </asp:UpdatePanel>
  </div>
</asp:Content>
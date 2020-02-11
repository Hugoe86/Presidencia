<%@ Page Title="Catálogo de Unidades" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Com_Unidades.aspx.cs" Inherits="paginas_Compras_Frm_Cat_Com_Unidades" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        <asp:UpdateProgress ID="Uprg_Unidades" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Unidades" style="background-color:#ffffff; width:100%; height:100%;">
                <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                    <tr>
                        <td colspan="4" class="label_titulo">
                            Catálogo de Unidades
                        </td>                        
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Image ID="Img_Error" runat="server" ImageUrl = "../imagenes/paginas/sias_warning.png"/>
                            <br />
                            <asp:Label ID="Lbl_Error" runat="server" ForeColor="Red" Text="" TabIndex="0"></asp:Label>
                        </td> 
                    </tr>
                    <tr class="barra_busqueda">
                        <td colspan="2" style="width:50%">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                            CssClass="Img_Button" onclick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                            CssClass="Img_Button" onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                            CssClass="Img_Button" 
                                OnClientClick="return confirm('¿Esta seguro de Eliminar el presente registro?');" 
                                onclick="Btn_Eliminar_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                            CssClass="Img_Button" onclick="Btn_Salir_Click"  />
                        </td>
                        <td colspan="2" align="right" style="width:50%">
                            Búsqueda
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"
                            ToolTip="Buscar" TabIndex="1" ></asp:TextBox>
                            <asp:ImageButton ID="Btn_Busqueda" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                TabIndex="2" onclick="Btn_Busqueda_Click"/>
                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                        </td>                        
                    </tr>
                    <tr>
                        <td style="width:18%">
                            &nbsp;
                        </td>
                        <td style="width:32%">
                            &nbsp;
                        </td>
                        <td style="width:18%">
                            &nbsp;
                        </td>
                        <td style="width:32%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Lbl_ID" runat="server" Text="Unidades ID"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_ID" runat="server" Width="90%"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Nombre" runat="server" Text="*Nombre"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_Nombre" runat="server" Width="90%" MaxLength="100"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Txt_Nombre_FilteredTextBoxExtender" runat="server" 
                                TargetControlID="Txt_Nombre" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/[]$@&*+-!?=¿¡ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="width:18%">
                            &nbsp;
                            <asp:Label ID="Lbl_Abreviatura" runat="server" Text="*Abreviatura"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                            <asp:TextBox ID="Txt_Abreviatura" runat="server" Width="90%" MaxLength="5"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                            TargetControlID="Txt_Abreviatura" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                            ValidChars="ÑñáéíóúÁÉÍÓÚ. "></cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Comentarios" runat="server" Text="Comentarios"></asp:Label>
                            </td>
                            <td colspan="3" style="width:82%">
                            <asp:TextBox ID="Txt_Comentarios" runat="server" Width="97%" Height="65" 
                            TextMode="MultiLine"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Comentarios" runat="server" WatermarkCssClass="watermarked"
                                     WatermarkText="Límite de Caractes 250" TargetControlID="Txt_Comentarios">
                                </cc1:TextBoxWatermarkExtender>
                                <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                TargetControlID="Txt_Comentarios" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:GridView ID="Grid_Unidades" runat="server" CssClass="GridView_1"
                            AutoGenerateColumns="False" AllowPaging="true" PageSize="5" Width="96%" 
                            GridLines="none" onselectedindexchanged="Grid_Unidades_SelectedIndexChanged"
                            OnPageIndexChanging="Grid_Unidades_PageIndexChanging"
                            >
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png"  >
                                <ItemStyle Width="3%" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="UNIDAD_ID" HeaderText="Unidad ID"
                                SortExpression="UNIDAD_ID" Visible="false">
                                <HeaderStyle HorizontalAlign="Left" Width="14%"/>
                                <ItemStyle HorizontalAlign="Left" Width="14%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" 
                                SortExpression="NOMBRE" >
                                <HeaderStyle HorizontalAlign="Left" Width="30%"/>
                                <ItemStyle HorizontalAlign="Left" Width="30%" />
                            </asp:BoundField>                                
                            <asp:BoundField DataField="ABREVIATURA" HeaderText="Abreviatura" 
                                SortExpression="ABREVIATURA" >
                                <HeaderStyle HorizontalAlign="Left" Width="20%"/>
                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                            </asp:BoundField>                            
                            <asp:BoundField DataField="COMENTARIOS" HeaderText="Comentarios" 
                                SortExpression="COMENTARIOS" >
                                <HeaderStyle HorizontalAlign="Left" Width="30%"/>
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
                </table>
            </div>        
        </ContentTemplate>        
    </asp:UpdatePanel>
</asp:Content>


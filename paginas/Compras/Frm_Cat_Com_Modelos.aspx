<%@ Page Title="Catálogo de Modelos" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Com_Modelos.aspx.cs" Inherits="paginas_Compras_Frm_Cat_Com_Modelos" %>
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
        <asp:UpdateProgress ID="Uprg_Modelos" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Modelos" style="background-color:#ffffff; width:100%; height:100%;">
                <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                    <tr>
                        <td colspan="4" class="label_titulo">
                            Catálogo de Modelos</td>                        
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
                            CssClass="Img_Button" onclick="Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                            CssClass="Img_Button" onclick="Btn_Modificar_Click"/>
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                            CssClass="Img_Button" 
                                
                                
                                OnClientClick="return confirm('¿Esta seguro de Eliminar el presente registro?');" onclick="Btn_Eliminar_Click" 
                                />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                            CssClass="Img_Button" onclick="Btn_Salir_Click"/>
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
                            <asp:Label ID="Lbl_ID" runat="server" Text="Modelo ID"></asp:Label>
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
                            ValidChars="ÑñáéíóúÁÉÍÓÚ. "></cc1:FilteredTextBoxExtender>
                        </td>                        
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Estatus" runat="server" Text="*Estatus"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="92%">
                            </asp:DropDownList>
                        </td>                        
                    </tr>
                    <tr>
                    <td style="width:18%">
                        <asp:Label ID="Lbl_Sub_Familia" runat="server" Text="Subfamilia" Visible="false"></asp:Label></td>
                    <td style="width:32%">
                        <asp:DropDownList ID="Cmb_Sub_Familias" runat="server" Width="92%" Visible="false">
                        </asp:DropDownList>
                    </td>
                    <td style="width:18%">&nbsp;</td>
                    <td style="width:32%">&nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Comentarios" runat="server" Text="Comentarios"></asp:Label>
                            </td>
                            <td colspan="3" style="width:82%">
                            <asp:TextBox ID="Txt_Comentarios" runat="server" Width="96%" Height="65" 
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
                            <asp:GridView ID="Grid_Modelos" runat="server" CssClass="GridView_1" Style="white-space:normal"
                            AutoGenerateColumns="False" AllowPaging="true" PageSize="5" Width="96%" 
                            GridLines="none" onpageindexchanging="Grid_Modelos_PageIndexChanging" 
                                onselectedindexchanged="Grid_Modelos_SelectedIndexChanged">
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png"  >
                                <ItemStyle Width="3%" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="MODELO_ID" HeaderText="Modelo ID"
                                SortExpression="MODELO_ID" >
                                <ItemStyle Width="14%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MODELO" HeaderText="Nombre" 
                                SortExpression="MODELO" >
                                <ItemStyle Width="30%" />                                                        
                            </asp:BoundField>
                            <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" 
                                SortExpression="ESTATUS" >
                                <ItemStyle Width="15%" />
                            </asp:BoundField> 
                            <asp:BoundField DataField="SUBFAMILIA" HeaderText="Subfamilia" 
                                SortExpression="SUBFAMILIA" >
                                <ItemStyle Width="15%" />
                            </asp:BoundField> 
                            <asp:BoundField DataField="COMENTARIOS" HeaderText="Comentarios" 
                                SortExpression="COMENTARIOS" >                                
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


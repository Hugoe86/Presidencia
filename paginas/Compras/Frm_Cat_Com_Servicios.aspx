<%@ Page Title="Catálogo de Servicios" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Com_Servicios.aspx.cs" Inherits="paginas_Compras_Frm_Cat_Com_Servicios" %>
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
            <asp:UpdateProgress ID="Uprg_Servicios" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Servicios" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="4">
                            Catálogo de Servicios
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
                        <td colspan="2"> 
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                            CssClass="Img_Button"  TabIndex="1"
                            ToolTip="Nuevo" onclick="Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                            CssClass="Img_Button"  TabIndex="2"
                            AlternateText="Modificar" ToolTip="Modificar" onclick="Btn_Modificar_Click"/>
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                            CssClass="Img_Button"  TabIndex="3"
                            AlternateText="Eliminar" ToolTip="Eliminar" onclick="Btn_Eliminar_Click" OnClientClick="return confirm('¿Esta seguro de Eliminar el presente registro?');" />
                            <asp:ImageButton ID="Btn_Exportar_Excel" runat="server" 
                            AlternateText="Consultar" CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                            onclick="Btn_Exportar_Excel_Click" ToolTip="Exportar Excel" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" 
                            CssClass="Img_Button"  TabIndex="4"
                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" ToolTip="Inicio" 
                                onclick="Btn_Salir_Click" />
                        </td>
                        <td colspan="2" align="right">
                            Búsqueda
                            <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"
                                ToolTip="Buscar" TabIndex="5" ></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda" 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ., " />
                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                            WatermarkText="<Clave o Nombre>" TargetControlID="Txt_Busqueda" />
                            <asp:ImageButton ID="Btn_Buscar_Servicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                            TabIndex="6" onclick="Btn_Buscar_Servicio_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                        </td>
                        <td style="width:32%">
                        </td>
                        <td style="width:18%">
                        </td>
                        <td style="width:32%">
                        </td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Clave" runat="server" Text="Clave" ></asp:Label>
                        </td>
                        <td style="width:32%">
                            <asp:HiddenField ID="Hdn_Servicio_ID" runat="server" />
                            <asp:TextBox ID="Txt_Clave" runat="server" Width="90%" TabIndex="7" ></asp:TextBox>
                        </td>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td style="width:32%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Nombre_Servicio" runat="server" Text="*Nombre"></asp:Label>
                        </td>
                        <td style="width:97%" colspan="3">
                            <asp:TextBox ID="Txt_Nombre_Servicio" runat="server" MaxLength="100" Width="96%" TabIndex="8"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:18%; vertical-align:top;">
                            <asp:Label ID="Lbl_Comentarios" runat="server" Text="*Descripción"></asp:Label>
                            </td>
                            <td colspan="3" style="width:82%">
                            <asp:TextBox ID="Txt_Comentarios" runat="server" Width="96%" Height="80px" 
                                TextMode="MultiLine" MaxLength="3600" TabIndex="9" ></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Fte_Txt_Comentarios" runat="server" 
                                    TargetControlID="Txt_Comentarios" 
                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                    ValidChars="ÑñáéíóúÁÉÍÓÚ., "></cc1:FilteredTextBoxExtender>
                                <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Comentarios" runat="server" WatermarkCssClass="watermarked"
                                     WatermarkText="Límite de Caractes 3600" TargetControlID="Txt_Comentarios">
                                </cc1:TextBoxWatermarkExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:18%;">
                            <asp:Label ID="Lbl_Costo" runat="server" Text="*Costo"></asp:Label>
                        </td>
                        <td style="width:32%">
                            <asp:TextBox ID="Txt_Costo" runat="server" Width="90%" 
                                MaxLength="15" TabIndex="14" AutoPostBack="true" 
                                OnTextChanged="Txt_Costo_TextChanged" ></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="Fte_Txt_Costo" 
                                runat="server" Enabled="True" TargetControlID="Txt_Costo"
                                InvalidChars="<,>,&,',!," 
                                FilterType="Custom, Numbers" 
                                ValidChars=".">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="width:18%; text-align:right;">
                            <asp:Label ID="Lbl_Impuesto" runat="server" Text="*Impuesto"></asp:Label>
                        </td>
                        <td style="width:32%">
                            <asp:DropDownList ID="Cmb_Impuestos" runat="server" Width="92%" TabIndex="15">
                            </asp:DropDownList>
                        </td>
                    </tr>                    
                    <tr>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Capitulo" runat="server" Text="*Capítulo"></asp:Label>
                        </td>
                        <td style="width:98%" colspan="3">
                            <asp:DropDownList ID="Cmb_Capitulo" runat="server" AutoPostBack="true" Width="97%" TabIndex="10"
                                OnSelectedIndexChanged="Cmb_Capitulo_SelectedIndexChanged" >
                            </asp:DropDownList>
                        </td>
                     </tr>
                     <tr>    
                        <td style="width:18%; text-align:left;">
                            <asp:Label ID="Lbl_Concepto" runat="server" Text="*Concepto"></asp:Label>
                        </td>
                        <td style="width:98%" colspan="3">
                            <asp:DropDownList ID="Cmb_Concepto" runat="server" AutoPostBack="true" Width="97%" TabIndex="11"
                                OnSelectedIndexChanged="Cmb_Concepto_SelectedIndexChanged" >
                            </asp:DropDownList>
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="width:18%">
                            <asp:Label ID="Lbl_Partida_Generica" runat="server" Text="*Partida genérica"></asp:Label>
                        </td>
                        <td style="width:98%" colspan="3">
                            <asp:DropDownList ID="Cmb_Partida_Generica" runat="server" AutoPostBack="true" Width="97%" TabIndex="12"
                                OnSelectedIndexChanged="Cmb_Partida_Generica_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>    
                    <tr>
                        <td style="width:18%; text-align:left;">
                            <asp:Label ID="Lbl_Partida_Especifica" runat="server" Text="*Partida específica"></asp:Label>
                        </td>
                        <td style="width:98%" colspan="3">
                            <asp:DropDownList ID="Cmb_Partida_Especifica" runat="server" Width="97%" TabIndex="13">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    

                    <tr>
                        <td style="width:18%">
                            </td>
                                <td style="width:32%">
                                    
                                <td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                         <div style="overflow:auto;height:320px;width:99%;vertical-align:top;border-style:outset;border-color: Silver;" > 
                            <asp:GridView ID="Grid_Servicios" runat="server" CssClass="GridView_1" Style="white-space:normal"
                                AutoGenerateColumns="False" AllowPaging="false" PageSize="5" Width="96%" 
                                GridLines="none" onpageindexchanging="Grid_Servicios_PageIndexChanging" 
                                onselectedindexchanged="Grid_Servicios_SelectedIndexChanged">
                            <RowStyle CssClass="GridItem" />
                            <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png"  >
                                <HeaderStyle HorizontalAlign="Center"/>
                                <ItemStyle Width="3%" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="CLAVE" HeaderText="Clave"
                                SortExpression="CLAVE" >
                                <HeaderStyle HorizontalAlign="Center"/>
                                <ItemStyle Width="8%" HorizontalAlign="Center"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" 
                                SortExpression="NOMBRE" >
                                <HeaderStyle HorizontalAlign="Left"/>
                                <ItemStyle HorizontalAlign="Left"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="COMENTARIOS" HeaderText="Descripción" Visible="false"
                                SortExpression="COMENTARIOS" >
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:BoundField DataField="COSTO" HeaderText="Costo" DataFormatString="{0:C}" 
                                SortExpression="COSTO" >
                                <HeaderStyle HorizontalAlign="Right"/>
                                <ItemStyle Width="10%" HorizontalAlign="Right"/>
                            </asp:BoundField>

                            </Columns>
                            <PagerStyle CssClass="GridHeader" />
                            <SelectedRowStyle CssClass="GridSelected" />
                            <HeaderStyle CssClass="GridHeader" />
                            <AlternatingRowStyle CssClass="GridAltItem" />                         
                        </asp:GridView>
                        </div>
                       </td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td style="width:32%">
                            &nbsp;<td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                                &nbsp;</td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:18%">
                            &nbsp;</td>
                        <td style="width:32%">
                            &nbsp;<td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                                &nbsp;</td>
                        </td>
                    </tr>
        </table>
            </div>
        </ContentTemplate>
         <Triggers>
            <asp:PostBackTrigger ControlID="Btn_Exportar_Excel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


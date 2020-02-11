<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Con_Tipo_Resultado.aspx.cs" Inherits="paginas_Contabilidad_Frm_Cat_Con_Tipo_Resultado" Title="Catálogo de Tipo de Resultado" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Tipo_Resultado" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Tipo_Resultado" >
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Catálogo de Tipos de Resultado</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>               
                    <tr class="barra_busqueda" align="right">
                        <td align="left">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" 
                                CssClass="Img_Button" TabIndex="1"
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                onclick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                CssClass="Img_Button" TabIndex="2"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" 
                                onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" 
                                CssClass="Img_Button" TabIndex="3"
                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png"
                                OnClientClick="return confirm('¿Está seguro de eliminar el Tipo de Resultado seleccionado?');" 
                                onclick="Btn_Eliminar_Click"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                CssClass="Img_Button" TabIndex="4"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                onclick="Btn_Salir_Click" />
                        </td>
                        <td style="width:50%">Busqueda
                            <asp:TextBox ID="Txt_Busqueda_Tipo_Resultado" runat="server" MaxLength="100" TabIndex="5" ToolTip="Buscar por Descripción"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Tipo_Resultado" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese Descripción>" TargetControlID="Txt_Busqueda_Tipo_Resultado" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Tipo_Resultado" runat="server" 
                                TargetControlID="Txt_Busqueda_Tipo_Resultado" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </cc1:FilteredTextBoxExtender>
                            <asp:ImageButton ID="Btn_Buscar_Descripcion_Tipo_Resultado" runat="server" 
                                ToolTip="Consultar" TabIndex="6" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                onclick="Btn_Buscar_Descripcion_Tipo_Resultado_Click"/>
                        </td> 
                    </tr>
                </table>
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="150px">Tipo Resultado ID</td>
                        <td>
                            <asp:TextBox ID="Txt_Tipo_Resultado_ID" runat="server" ReadOnly="True" Width="150px"></asp:TextBox>
                        </td>     
                    </tr>
                    <tr>
                        <td width="150px">*Descripcion</td>
                        <td>
                            <asp:TextBox ID="Txt_Descripcion_Tipo_Resultado" runat="server" MaxLength="100" TabIndex="7" Width="600px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Descripcion_Tipo_Resultado" runat="server" TargetControlID="Txt_Descripcion_Tipo_Resultado"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td width="150px">Comentarios</td>
                        <td>
                            <asp:TextBox ID="Txt_Comentarios_Tipo_Resultado" runat="server" TabIndex="8" MaxLength="250"
                                TextMode="MultiLine" Width="600px" AutoPostBack="True"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Tipo_Resultado" runat="server" WatermarkCssClass="watermarked"
                                TargetControlID ="Txt_Comentarios_Tipo_Resultado" WatermarkText="Límite de Caractes 250">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Tipo_Resultado" runat="server" 
                                TargetControlID="Txt_Comentarios_Tipo_Resultado" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>                    
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>                    
                    <tr align="center">
                        <td colspan="2">
                            <asp:GridView ID="Grid_Tipo_Resultado" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                PageSize="5" AllowSorting="True" 
                                HeaderStyle-CssClass="tblHead" 
                                onpageindexchanging="Grid_Tipo_Resultado_PageIndexChanging" 
                                onselectedindexchanged="Grid_Tipo_Resultado_SelectedIndexChanged" 
                                onsorting="Grid_Tipo_Resultado_Sorting">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="7%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Tipo_Resultado_ID" HeaderText="Tipo Resultado ID" Visible="True" SortExpression="Tipo_Resultado_ID">
                                        <HeaderStyle HorizontalAlign="Left" Width="18%" />
                                        <ItemStyle HorizontalAlign="Left" Width="18%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True" SortExpression="Descripcion">
                                        <HeaderStyle HorizontalAlign="Left" Width="73%" />
                                        <ItemStyle HorizontalAlign="Left" Width="73%" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>                            
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


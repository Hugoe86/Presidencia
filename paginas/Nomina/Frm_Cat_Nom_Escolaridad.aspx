<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Escolaridad.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Escolaridad" Title="Catálogo de Escolaridad" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" language="javascript">
    function pageLoad() { $('[id*=Txt_Comen').keyup(function() {var Caracteres =  $(this).val().length;if (Caracteres > 250) {this.value = this.value.substring(0, 250);$(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});}
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="ScriptManager_Escolaridad" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Escolaridad" style="background-color:#ffffff; width:98%; height:100%;">
            
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Catálogo de Escolaridad</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>

                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr class="barra_busqueda" align="right">
                        <td align="left">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="1"
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" onclick="Btn_Nuevo_Click" />
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="2"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server" ToolTip="Eliminar" CssClass="Img_Button" TabIndex="3"
                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" onclick="Btn_Eliminar_Click"
                                OnClientClick="return confirm('¿Está seguro de eliminar el Área seleccionada?');"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="4"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" onclick="Btn_Salir_Click"/>
                        </td>
                        <td style="width:50%">Busqueda
                            <asp:TextBox ID="Txt_Busqueda_Escolaridad" runat="server" MaxLength="100" TabIndex="5" 
                                ToolTip="Buscar por Escolaridad" Width="180px"/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Escolaridad" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese Escolaridad>" TargetControlID="Txt_Busqueda_Escolaridad" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Escolaridad" runat="server" 
                                TargetControlID="Txt_Busqueda_Escolaridad" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </cc1:FilteredTextBoxExtender>
                            <asp:ImageButton ID="Btn_Buscar_Escolaridad" runat="server" ToolTip="Consultar" TabIndex="6"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Escolaridad_Click" />
                        </td> 
                    </tr>
                </table>

                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Escolaridad ID
                        </td>
                        <td style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Escolaridad_ID" runat="server" Width="30%"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Escolaridad
                        </td>
                        <td style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Escolaridad" runat="server" MaxLength="100" TabIndex="7" Width="99%"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Escolarida" runat="server" TargetControlID="Txt_Escolaridad"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Comentarios
                        </td>
                        <td style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Comentarios_Escolaridad" runat="server" TabIndex="8" MaxLength="250"
                                TextMode="MultiLine" Width="99%"/>
                            <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Escolaridad" runat="server" WatermarkCssClass="watermarked"
                                TargetControlID ="Txt_Comentarios_Escolaridad" WatermarkText="Límite de Caractes 250"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Escolaridad" runat="server" 
                                TargetControlID="Txt_Comentarios_Escolaridad" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ "/>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%" colspan="4">
                            <hr />
                        </td>
                    </tr>
                </table>

                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2">
                            <asp:GridView ID="Grid_Escolaridad" runat="server" AllowPaging="True" Width="100%"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                PageSize="5" onpageindexchanging="Grid_Escolaridad_PageIndexChanging" 
                                onselectedindexchanged="Grid_Escolaridad_SelectedIndexChanged"
                                AllowSorting="True" OnSorting="Grid_Escolaridad_Sorting" HeaderStyle-CssClass="tblHead">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="7%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Escolaridad_ID" HeaderText="Escolaridad ID" 
                                        Visible="True" SortExpression="Escolaridad_ID">
                                        <HeaderStyle HorizontalAlign="Left" Width="18%" />
                                        <ItemStyle HorizontalAlign="Left" Width="18%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Escolaridad" HeaderText="Escolaridad" Visible="True" SortExpression="Escolaridad">
                                        <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                        <ItemStyle HorizontalAlign="Left" Width="60%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Comentarios" HeaderText="Comentarios" Visible="True" SortExpression="Comentarios">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
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


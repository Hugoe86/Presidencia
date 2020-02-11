<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Zona_Economica.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Zona_Economica" Title="Catálogo de Zonas Económicas" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">

    <script src="../../easyui/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency-1.4.0.min.js" type="text/javascript"></script>
    <script src="../../easyui/jquery.formatCurrency.all.js" type="text/javascript"></script>

<script type="text/javascript" language="javascript">
    function pageLoad() { $('[id*=Txt_Comen').keyup(function() {var Caracteres =  $(this).val().length;if (Caracteres > 250) {this.value = this.value.substring(0, 250);$(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});}
</script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="ScriptManager_Zona_Economica" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>

            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress"><img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <div id="Div_Zona_Economica" style="background-color:#ffffff; width:98%; height:100%;">

                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">
                            Catálogo de Zonas Económicas
                        </td>
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
                        <td align = "left">
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
                            <asp:TextBox ID="Txt_Busqueda_Zona_Economica" runat="server" MaxLength="100" TabIndex="5" 
                                ToolTip="Buscar por Zona" Width="180px"/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Zona_Economica" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese la Zona>" TargetControlID="Txt_Busqueda_Zona_Economica" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Zona_Economica" runat="server" 
                                TargetControlID="Txt_Busqueda_Zona_Economica" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                            <asp:ImageButton ID="Btn_Buscar_Zona_Economica" runat="server" ToolTip="Consultar" TabIndex="6" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Zona_Economica_Click" />
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
                            Zona ID
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Zona_Economica_ID" runat="server" Width="98%"/>
                        </td>
                        <td style="text-align:left;width:20%;">
                        </td>
                        <td style="text-align:left;width:30%;">
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Zona Económica
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Zona_Economica" runat="server" MaxLength="100" TabIndex="7" Width="99%"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Zona_Economica" runat="server" TargetControlID="Txt_Zona_Economica" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. _-."/>
                        </td>
                        <td style="text-align:left;width:20%;">
                            *Salario Diario
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Salario_Diario_Zona_Economica" runat="server" MaxLength="5" TabIndex="8" 
                                Width="98%" onblur="$('input[id$=Txt_Salario_Diario_Zona_Economica]').formatCurrency();"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Salario_Diario_Zona_Economica" runat="server" 
                                TargetControlID="Txt_Salario_Diario_Zona_Economica" FilterType="Custom, Numbers" ValidChars=",."/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;vertical-align:top;">
                            Comentarios
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Comentarios_Zona_Economica" runat="server" TabIndex="9" MaxLength="250"
                                TextMode="MultiLine" Width="99%">
                            </asp:TextBox>
                            <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Zona_Economica" runat="server" WatermarkCssClass="watermarked"
                                TargetControlID ="Txt_Comentarios_Zona_Economica" WatermarkText="Límite de Caractes 250"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Zona_Economicar" runat="server" 
                                TargetControlID="Txt_Comentarios_Zona_Economica" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
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
                        <td colspan="4">
                            <asp:GridView ID="Grid_Zona_Economica" runat="server" AllowPaging="True" Width="100%"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                PageSize="5" onpageindexchanging="Grid_Zona_Economica_PageIndexChanging" 
                                onselectedindexchanged="Grid_Zona_Economica_SelectedIndexChanged"
                                AllowSorting="True" OnSorting="Grid_Zona_Economica_Sorting" HeaderStyle-CssClass="tblHead">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Zona_ID" HeaderText="Zona ID" 
                                        Visible="True" SortExpression="Zona_ID">
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Zona_Economica" HeaderText="Zona Economica" Visible="True" SortExpression="Zona_Economica">
                                        <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                        <ItemStyle HorizontalAlign="Left" Width="40%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Salario_Diario" HeaderText="Salario" Visible="True" SortExpression="Salario_Diario">
                                        <HeaderStyle HorizontalAlign="Left" Width="18%" />
                                        <ItemStyle HorizontalAlign="Left" Width="18%" />
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


<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Nom_Tipo_Trabajador.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Nom_Tipo_Trabajador" Title="Catálogo de Tipos de Trabajador" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" language="javascript">
    function pageLoad() { $('[id*=Txt_Comen').keyup(function() {var Caracteres =  $(this).val().length;if (Caracteres > 250) {this.value = this.value.substring(0, 250);$(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});}
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="ScriptManager_Tipo_Trabajador" runat="server"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress"><img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                
            </asp:UpdateProgress>
            
            <div id="Div_Tipo_Trabajador" style="background-color:#ffffff; width:98%; height:100%;">
            
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">C&aacute;talogo de Tipos de Trabajadores</td>
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
                        <td colspan="2" align = "left">
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
                        <td colspan="2">B&uacute;squeda
                            <asp:TextBox ID="Txt_Busqueda_Tipo_Trabajador" runat="server" MaxLength="100" TabIndex="5" 
                                ToolTip="Buscar por Descripcion" Width="180px"/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Tipo_Trabajador" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese Descripcion>" TargetControlID="Txt_Busqueda_Tipo_Trabajador" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Tipo_Trabajador" runat="server" 
                                TargetControlID="Txt_Busqueda_Tipo_Trabajador" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="ÑñáéíóúÁÉÍÓÚ. "></cc1:FilteredTextBoxExtender>
                            <asp:ImageButton ID="Btn_Buscar_Tipo_Trabajador" runat="server" TabIndex="6" ToolTip="Consultar"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Buscar_Tipo_Trabajador_Click" />
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
                            Tipo Trabajador ID
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Tipo_Trabajador_ID" runat="server" Width="98%"/>
                        </td>
                        <td style="text-align:left;width:20%;">
                            Estatus
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus_Tipo_Trabajador" runat="server" Width="100%" TabIndex="7">
                                <asp:ListItem Value="ACTIVO">ACTIVO</asp:ListItem>
                                <asp:ListItem Value="INACTIVO">INACTIVO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Descripci&oacute;n
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Descripcion_Tipo_Trabajador" runat="server" MaxLength="100" 
                                TabIndex="8" Width="99%"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Descripcion_Tipo_Trabajador" runat="server" 
                                TargetControlID="Txt_Descripcion_Tipo_Trabajador" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "></cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;vertical-align:top;">
                            Comentarios
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Comentarios_Tipo_Trabajador" runat="server" TextMode="MultiLine" TabIndex="9" 
                                Width="99%"/>
                            <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Tipo_Trabajador" runat="server" TargetControlID="Txt_Comentarios_Tipo_Trabajador" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Tipo_Trabajador" runat="server" TargetControlID="Txt_Comentarios_Tipo_Trabajador" 
                                WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked"/>
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
                            <asp:GridView ID="Grid_Tipo_Trabajador" PageSize="5" CssClass="GridView_1" Width="100%"
                                runat="server" AllowPaging="True" AutoGenerateColumns="False" GridLines="None"
                                onpageindexchanging="Grid_Tipo_Trabajador_PageIndexChanging" 
                                onselectedindexchanged="Grid_Tipo_Trabajador_SelectedIndexChanged"
                                AllowSorting="True" OnSorting="Grid_Tipo_Trabajador_Sorting" HeaderStyle-CssClass="tblHead">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="7%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Tipo_Trabajador_ID" HeaderText="Identificador" SortExpression="Tipo_Trabajador_ID"/>
                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripci&oacute;n" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  SortExpression="Descripcion"/>
                                    <asp:BoundField DataField="Comentarios" HeaderText="Comentarios" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" SortExpression="Comentarios"/>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="Estatus"/>
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


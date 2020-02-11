<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Turnos.aspx.cs" Inherits="paginas_Nomina_Frm_Cat_Turnos" Title="Catálogo de Turnos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Src="~/paginas/Paginas_Generales/Pager.ascx" TagPrefix="custom" TagName="Pager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script type="text/javascript" language="javascript">
    function pageLoad() { $('[id*=Txt_Comen').keyup(function() {var Caracteres =  $(this).val().length;if (Caracteres > 250) {this.value = this.value.substring(0, 250);$(this).css("background-color", "Yellow");$(this).css("color", "Red");}else{$(this).css("background-color", "White");$(this).css("color", "Black");}$('#Contador_Caracteres_Comentarios').text('Carácteres Ingresados [ ' + Caracteres + ' ]/[ 250 ]');});}
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="ScriptManager_Turnos" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>

            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Turnos" style="background-color:#ffffff; width:98%; height:100%;">
            
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Catálogo de Turnos
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
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
                        <td colspan="2">Busqueda
                            <asp:TextBox ID="Txt_Busqueda_Turno" runat="server" MaxLength="100" TabIndex="5" ToolTip="Burcar por Descripcion"
                                Width="180px"/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Turno" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese la Descripcion>" TargetControlID="Txt_Busqueda_Turno" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Turno" runat="server" TargetControlID="Txt_Busqueda_Turno" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. ">
                            </cc1:FilteredTextBoxExtender>
                            <asp:ImageButton ID="Btn_Buscar_Turno" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                ToolTip="Consultar" onclick="Btn_Buscar_Turno_Click" TabIndex="6" />
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
                            Turno ID
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Turno_ID" runat="server" ReadOnly="True" Width="98%"/>
                        </td>
                        <td style="text-align:left;width:20%;">
                            Estatus
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Estatus_Turno" runat="server" Width="100%" TabIndex="7">
                                <asp:ListItem>ACTIVO</asp:ListItem>
                                <asp:ListItem>INACTIVO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Descripcion
                        </td>
                        <td colspan="3" style="text-align:left;width:20%;">
                            <asp:TextBox ID="Txt_Descripcion_Turno" runat="server" MaxLength="100" TabIndex="8" Width="99%"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Descripcion_Turno" runat="server" TargetControlID="Txt_Descripcion_Turno" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. -_[]{}"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Hora Entrada
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Hora_Entrada_Turno" runat="server" TabIndex="9"  Text="12:12:12 a.m."
                                ToolTip="[ Horas:0-23  Minutos:0-59  Segundos:0-59 ]"  Width="98%"/>
                            <cc1:MaskedEditExtender 
                                 AutoCompleteValue="true"
                                 ID="Txt_Hora_Entrada_Turno_MaskedEditExtender"
                                 runat="server" 
                                 TargetControlID="Txt_Hora_Entrada_Turno"
                                 PromptCharacter="_" 
                                 AcceptAMPM="true" 
                                 Mask="99:99:99" 
                                 MaskType="Time"
                                 Century="2000"
                                 UserTimeFormat="TwentyFourHour"
                                 InputDirection="LeftToRight"
                                 ClearMaskOnLostFocus="false"
                                 AcceptNegative="None"
                                 MessageValidatorTip="true"  AutoComplete="true" ClipboardEnabled="False"/>
                        </td>
                        <td style="text-align:left;width:20%;">
                            &nbsp;&nbsp;*Hora Salida
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Hora_Salida_Turno" runat="server" TabIndex="10"
                                ToolTip="[ Horas:0-23  Minutos:0-59  Segundos:0-59 ]" Width="97%"/>
                            <cc1:MaskedEditExtender 
                                 AutoCompleteValue="true"
                                 ID="Txt_Hora_Salida_Turno_MaskedEditExtender"
                                 runat="server" 
                                 TargetControlID="Txt_Hora_Salida_Turno"
                                 PromptCharacter="_" 
                                 AcceptAMPM="false" 
                                 Mask="99:99:99" 
                                 MaskType="Time"
                                 Century="2000"
                                 UserTimeFormat="TwentyFourHour"
                                 InputDirection="LeftToRight"
                                 ClearMaskOnLostFocus="false"
                                 AcceptNegative="None"
                                 MessageValidatorTip="true"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;vertical-align:top;">
                            Comentarios
                        </td>
                        <td colspan="3" style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Comentarios_Turno" runat="server" TabIndex="11" MaxLength="250"
                                TextMode="MultiLine" Width="99%" Rows="4"/>
                            <span id="Contador_Caracteres_Comentarios" class="watermarked"></span>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Turno" runat="server" TargetControlID ="Txt_Comentarios_Turno"
                                WatermarkText="Límite de Caractes 250" WatermarkCssClass="watermarked">
                            </cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Turno" runat="server" TargetControlID="Txt_Comentarios_Turno"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ "/>
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
                            <asp:GridView ID="Grid_Turno" runat="server" AllowPaging="True" Width="100%"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                PageSize="5" onpageindexchanging="Grid_Turno_PageIndexChanging" 
                                onselectedindexchanged="Grid_Turno_SelectedIndexChanged"
                                AllowSorting="true" OnSorting="Grid_Turno_Sorting" HeaderStyle-CssClass="tblHead">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="7%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Turno_ID" HeaderText="Turno ID" Visible="True" SortExpression="Turno_ID">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="True" SortExpression="Descripcion">
                                        <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                        <ItemStyle HorizontalAlign="Left" Width="50" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" Visible="True" SortExpression="Estatus">
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                </Columns>
                                <RowStyle CssClass="GridItem" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                            <custom:Pager ID="custPager" runat="server" OnPageChanged="custPager_PageChanged" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


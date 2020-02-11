<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Tab_Nom_Subsidio.aspx.cs" Inherits="paginas_Nomina_Frm_Tab_Nom_Subsidio" Title="Tabla de Subsidio" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="ScriptManager_Subsidio" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress"><img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Subsidio" style="background-color:#ffffff; width:98%; height:100%;">
            
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="6" class="label_titulo">Tabla de Subsidio</td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr class="barra_busqueda" align="right">
                        <td colspan="3" align = "left">
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
                        <td colspan="3">Busqueda
                            <asp:TextBox ID="Txt_Busqueda_Subsidio" runat="server" MaxLength="100" TabIndex="5" Width="180px" ToolTip="Buscar por Tipo Nómina"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Subsidio" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese el Tipo Nómina>" TargetControlID="Txt_Busqueda_Subsidio" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Subsidio" runat="server" 
                                TargetControlID="Txt_Busqueda_Subsidio" FilterType="LowercaseLetters, UppercaseLetters"/>
                            <asp:ImageButton ID="Btn_Busqueda_Subsidio" runat="server" ToolTip="Consultar" TabIndex="6" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Busqueda_Subsidio_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;</td>
                    </tr>
                </table>
                
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Subsidio ID
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Subsidio_ID" runat="server" ReadOnly="True" Width="98%"/>
                        </td>
                        <td style="text-align:left;width:20%;">
                            *Subsidio
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Subsidio" runat="server" Width="98%" TabIndex="5" MaxLength="9" 
                                ToolTip="[0.00]"/>
                            <cc1:MaskedEditExtender 
                                 ID="MEE_Txt_Subsidio"
                                 runat="server" 
                                 TargetControlID="Txt_Subsidio"
                                 Mask="9,999,999.99"
                                 OnFocusCssClass="MaskedEditFocus"
                                 OnInvalidCssClass="MaskedEditError"
                                 MaskType="Number"
                                 InputDirection="RightToLeft"
                                 DisplayMoney="Left"
                                 ErrorTooltipEnabled="true"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Tipo Nómina
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Tipo_Nomina_Subsidio" runat="server"  TabIndex="7" Width="100%">
                                <asp:ListItem><--SELECCIONAR--></asp:ListItem>
                                <asp:ListItem>SEMANAL</asp:ListItem>
                                <asp:ListItem>CATORCENAL</asp:ListItem>
                                <asp:ListItem>QUINCENAL</asp:ListItem>
                                <asp:ListItem>MENSUAL</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align:left;width:20%;">
                            *Limite Inferior
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Limite_Inferior_Subsidio" runat="server" Width="98%" TabIndex="8" 
                                MaxLength="10" ToolTip="[0.00]"/>
                            <cc1:MaskedEditExtender
                                 ID="MEE_Txt_Limite_Inferior_Subsidio"
                                 runat="server" 
                                 TargetControlID="Txt_Limite_Inferior_Subsidio"
                                 Mask="9,999,999.99"
                                 OnFocusCssClass="MaskedEditFocus"
                                 OnInvalidCssClass="MaskedEditError"
                                 MaskType="Number"
                                 InputDirection="RightToLeft"
                                 DisplayMoney="Left"
                                 ErrorTooltipEnabled="true"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Comentarios
                        </td>
                        <td colspan="3" style="text-align:left;width:80%;">
                            <asp:TextBox ID="Txt_Comentarios_Subsidio" runat="server" TabIndex="10" MaxLength="250"
                                TextMode="MultiLine" Width="99%"/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Subsidio" runat="server" WatermarkCssClass="watermarked"
                                TargetControlID ="Txt_Comentarios_Subsidio" WatermarkText="Límite de Caractes 250"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Subsidio" runat="server" TargetControlID="Txt_Comentarios_Subsidio" 
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ "/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>
                </table>
                
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="6">
                            <asp:GridView ID="Grid_Subsidio" runat="server" AllowPaging="True" Width="100%"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                PageSize="5" onpageindexchanging="Grid_Subsidio_PageIndexChanging" 
                                onselectedindexchanged="Grid_Subsidio_SelectedIndexChanged">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="7%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Subsidio_ID" HeaderText="Subsidio ID" 
                                        Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Tipo_Nomina" HeaderText="Nomina" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="23%" />
                                        <ItemStyle HorizontalAlign="Left" Width="23%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Limite_Inferior" HeaderText="Limite Inferior" 
                                        Visible="True" DataFormatString="{0:#,###,##0.00}">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Right" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Subsidio" HeaderText="Subsidio" Visible="True" 
                                        DataFormatString="{0:#,###,##0.00}">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle HorizontalAlign="Right" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Comentarios" HeaderText="Comentarios" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
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


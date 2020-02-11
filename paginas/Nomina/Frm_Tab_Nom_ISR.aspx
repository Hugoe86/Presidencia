<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Tab_Nom_ISR.aspx.cs" Inherits="paginas_Nomina_Frm_Tab_Nom_ISR" Title="Tabla de ISR" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">

    <asp:ScriptManager ID="ScriptManager_ISR" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_ISR" style="background-color:#ffffff; width:98%; height:100%;">
            
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="6" class="label_titulo">
                            Tabla de ISR
                        </td>
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
                            <asp:TextBox ID="Txt_Busqueda_ISR" runat="server" MaxLength="100" TabIndex="6" Width="150px" ToolTip="Buscar por Tipo Nómina"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_ISR" runat="server" WatermarkCssClass="watermarked"
                                WatermarkText="<Ingrese el Tipo Nómina>" TargetControlID="Txt_Busqueda_ISR" />
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_ISR" runat="server" 
                                TargetControlID="Txt_Busqueda_ISR" FilterType="LowercaseLetters, UppercaseLetters">
                            </cc1:FilteredTextBoxExtender>
                            <asp:ImageButton ID="Btn_Busqueda_ISR" runat="server" ToolTip="Consultar" TabIndex="6" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" onclick="Btn_Busqueda_ISR_Click" />
                        </td>
                    </tr>
                </table>
                
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            ISR ID
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_ISR_ID" runat="server" Width="98%"/>
                        </td>
                        <td style="text-align:left;width:20%;">
                        </td>
                        <td style="text-align:left;width:30%;">
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Tipo Nómina
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:DropDownList ID="Cmb_Tipo_Nomina_ISR" runat="server" TabIndex="7" Width="100%">
                                <asp:ListItem><--SELECCIONAR--></asp:ListItem>
                                <asp:ListItem>SEMANAL</asp:ListItem>
                                <asp:ListItem>CATORCENAL</asp:ListItem>
                                <asp:ListItem>QUINCENAL</asp:ListItem>
                                <asp:ListItem>MENSUAL</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align:left;width:20%;">
                            *Porcentaje
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Porcentaje_ISR" runat="server" Width="98%" TabIndex="10" MaxLength="6"/>
                            <cc1:FilteredTextBoxExtender ID="FTxt_Porcentaje_ISR" runat="server"  TargetControlID="Txt_Porcentaje_ISR"
                                FilterType="Custom, Numbers" ValidChars="." />
                            <asp:CustomValidator ID="Cv_Txt_Porcentaje_ISR" runat="server"  Display="None"
                                 EnableClientScript="true" ErrorMessage="Porcentaje ISR [0-100]"
                                 Enabled="true"
                                 ClientValidationFunction="TextBox_Txt_Porcentaje_ISR"
                                 HighlightCssClass="highlight" 
                                 ControlToValidate="Txt_Porcentaje_ISR"/>
                            <cc1:ValidatorCalloutExtender ID="Vce_Txt_Porcentaje_ISR" runat="server" TargetControlID="Cv_Txt_Porcentaje_ISR" PopupPosition="BottomLeft"/>    
                            <script type="text/javascript" >
                                function TextBox_Txt_Porcentaje_ISR(sender, args) {
                                     var Porcentaje_ISR = document.getElementById("<%=Txt_Porcentaje_ISR.ClientID%>").value;
                                     if ( (Porcentaje_ISR < 0) || (Porcentaje_ISR > 100) ){
                                        document.getElementById("<%=Txt_Porcentaje_ISR.ClientID%>").value ="";
                                        args.IsValid = false;
                                     }
                                  }
                            </script>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Limite Inferior
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Limite_Inferior_ISR" runat="server" Width="98%" TabIndex="8" MaxLength="10" 
                                ToolTip="[0.00]"/>
                            <cc1:MaskedEditExtender
                                 ID="MEE_Txt_Limite_Inferior_ISR"
                                 runat="server" 
                                 TargetControlID="Txt_Limite_Inferior_ISR"
                                 Mask="9,999,999.99"
                                 OnFocusCssClass="MaskedEditFocus"
                                 OnInvalidCssClass="MaskedEditError"
                                 MaskType="Number"
                                 InputDirection="RightToLeft"
                                 DisplayMoney="Left"
                                 ErrorTooltipEnabled="true"/>
                        </td>
                        <td style="text-align:left;width:20%;">
                            *Cuota Fija
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Couta_Fija_ISR" runat="server" Width="98%" TabIndex="9" MaxLength="10" 
                                ToolTip="[0.00]"/>
                            <cc1:MaskedEditExtender 
                                 ID="MEE_Txt_Couta_Fija_ISR"
                                 runat="server" 
                                 TargetControlID="Txt_Couta_Fija_ISR"
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
                            <asp:TextBox ID="Txt_Comentarios_ISR" runat="server" TabIndex="11" MaxLength="250"
                                TextMode="MultiLine" Width="99%"/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_ISR" runat="server" 
                                TargetControlID ="Txt_Comentarios_ISR" WatermarkText="Límite de Caractes 250" 
                                WatermarkCssClass="watermarked"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_ISR" runat="server" TargetControlID="Txt_Comentarios_ISR"
                                FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ "/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>
                </table>
                
                <table width="100%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_ISR" runat="server" AllowPaging="True" GridLines="None"
                                AutoGenerateColumns="False" CssClass="GridView_1" PageSize="5" Width="100%"
                                onpageindexchanging="Grid_ISR_PageIndexChanging" 
                                onselectedindexchanged="Grid_ISR_SelectedIndexChanged">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="7%" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="ISR_ID" HeaderText="ISR ID" 
                                        Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Tipo_Nomina" HeaderText="Nomina" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" Width="17%" />
                                        <ItemStyle HorizontalAlign="Left" Width="17%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Limite_Inferior" HeaderText="Limite Inferior" 
                                        Visible="True" DataFormatString="{0:#,###,##0.00}">
                                        <HeaderStyle HorizontalAlign="Left" Width="17%" />
                                        <ItemStyle HorizontalAlign="Right" Width="17%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Cuota_Fija" HeaderText="Cuota Fija" Visible="True" 
                                        DataFormatString="{0:#,###,##0.00}">
                                        <HeaderStyle HorizontalAlign="Left" Width="17%" />
                                        <ItemStyle HorizontalAlign="Right" Width="17%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Porcentaje" HeaderText="Porcentaje" Visible="True" 
                                        DataFormatString="{0:0.00}">
                                        <HeaderStyle HorizontalAlign="Left" Width="17%" />
                                        <ItemStyle HorizontalAlign="Right" Width="17%" />
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


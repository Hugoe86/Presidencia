<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Folios_Inutilizados.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Folios_Inutilizados" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript" >
    function Validar_Cantidad_Caracteres(){
        var limit = 250;
        $('textarea[id$=Txt_Observaciones]').keyup(function() {
            var len = $(this).val().length;
            if (len > limit) {
                alert("LLego al limite de caracteres");
                this.value = this.value.substring(0, limit);
            }
        });
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Contenido" style="background-color:#ffffff; width:100%; height:100%;">
                    
                <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Folios Inutilizados
                        </td>
                    </tr>
                    <tr>
                         <td class="style1">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top" >
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text=""  CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                          
                        </td>
                    </tr>
               </table>             
            
               <table width="98%"  border="0" cellspacing="0">
                <tr align="center">
                    <td colspan="2">                
                        <div style="width:99%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td align="left" style="width:59%;">
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Nuevo" CssClass="Img_Button" TabIndex="1"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" 
                                            OnClick = "Btn_Nuevo_Click"/>
                                        <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button" TabIndex="4"
                                            ImageUrl="~/paginas/imagenes/gridview/grid_print.png" OnClick="Btn_Imprimir_Click" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="5"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" AlternateText = "Salir"
                                            OnClick = "Btn_Salir_Click"/>
                                    </td>
                                    <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                <td style="width:25%;">
                                                    <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5"  ToolTip = "Buscar" Width="180px"/>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                        WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" 
                                                        runat="server" TargetControlID="Txt_Busqueda" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                </td>
                                                <td style="vertical-align:middle;width:5%;" >
                                                    <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6"
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar"
                                                        OnClick = "Btn_Buscar_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                </table>  
                    
                <br />
                    
                <table width="98%" class="estilo_fuente"> 
                    <tr>
                        <td style="text-align:left;width:20%;">
                            <asp:TextBox ID="Txt_Pago_ID" runat="server" Width="96.4%" 
                            MaxLength = "10" Visible = "false"/>
                        </td>
                        <td style="text-align:left;width:20%;">
                            <asp:TextBox ID="Txt_Caja_ID" runat="server" Width="96.4%" 
                            MaxLength = "10" Visible = "false"/>
                        </td>
                    </tr> 
                    <tr>
                        <td style="text-align:left;width:20%;text-align:left;">
                            Modulo
                        </td>
                        <td style="text-align:left;width:80%;" colspan = "3">
                            <asp:TextBox ID="Txt_Modulo" runat="server" Width="98.6%" enabled="false"
                            MaxLength = "100"/>
                        </td>
                    </tr>        
                    <tr>
                         <td style="text-align:left;width:20%;">
                            *Caja
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Caja" runat="server" Width="96.4%" Enabled = "false" />
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            *Fecha
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Fecha" runat="server" Width="96.4%" Enabled = "false" />
                        </td>
                    </tr> 
                    <tr>
                        <td style="text-align:left;width:20%;text-align:left;">
                            Cajero
                        </td>
                        <td style="text-align:left;width:80%;" colspan = "3">
                            <asp:TextBox ID="Txt_Cajero" runat="server" Width="98.6%" enabled="false"
                            MaxLength = "100"/>
                        </td>
                    </tr>        
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *No. Recibo Inicial
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Recibo" runat="server" Width="96.4%" 
                            MaxLength = "10"/>
                            <cc1:FilteredTextBoxExtender ID="FTBE_Txt_Recibo" runat="server" 
                              FilterType="Custom, Numbers" TargetControlID="Txt_Recibo" Enabled="True" /> 
                        </td>
                        <td style="text-align:right;width:20%;">
                            No. Recibo Final
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Folio_Fin" runat="server" Width="96.4%" 
                            MaxLength = "10"/>
                            <cc1:FilteredTextBoxExtender ID="FTBE_Txt_Folio_Fin" runat="server" 
                              FilterType="Custom, Numbers" TargetControlID="Txt_Folio_Fin" Enabled="True" /> 
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Motivo
                        </td>
                        <td style="text-align:left;width:80%;" colspan = "3">
                            <asp:DropDownList ID="Cmb_Motivo" runat="server" Width="98.6%">
                            </asp:DropDownList>  
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Observaciones
                        </td>
                        <td colspan="3" style="text-align:left;width:80%; vertical-align:top;">
                            <asp:TextBox ID="Txt_Observaciones" runat="server" TabIndex="10" MaxLength="250"
                                TextMode="MultiLine" Width="98.6%" onkeyup="javascript:Validar_Cantidad_Caracteres();"/>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkCssClass="watermarked"
                                 WatermarkText="Límite de Caractes 250" TargetControlID="Txt_Observaciones" 
                                 Enabled="True">
                                </cc1:TextBoxWatermarkExtender>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                 runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                 TargetControlID="Txt_Observaciones" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " 
                                 Enabled="True">
                                </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    
                    <tr><td colspan="4">&nbsp;</td></tr>
                    
                     <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_Folios_Inutilizados" runat="server" AllowPaging="True" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="20" Width="100%"
                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                AllowSorting="True" HeaderStyle-CssClass="tblHead" style="white-space:normal;"
                                OnSelectedIndexChanged = "Grid_Folios_Inutilizados_SelectedIndexChanged"
                                OnPageIndexChanging = "Grid_Folios_Inutilizados_PageIndexChanging">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" HorizontalAlign="Center"/>
                                    </asp:ButtonField>
                                    
                                    <asp:BoundField DataField="NO_PAGO" HeaderStyle-Width="0%" HeaderText="No. Pago">
                                    <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="NUM_RECIBO" HeaderStyle-Width="15%" HeaderText="No. Recibo">
                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="CAJA_ID" HeaderStyle-Width="0%" HeaderText="ID Caja">
                                    <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="CAJERO" HeaderStyle-Width="20%" HeaderText="Cajero">
                                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="FECHA" HeaderStyle-Width="15%" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" >
                                    <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="MOTIVO_ID" HeaderStyle-Width="0%" HeaderText="ID Motivo">
                                    <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="MOTIVO" HeaderStyle-Width="15%" HeaderText="Motivo">
                                    <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                    <ItemStyle HorizontalAlign="Left" Width="25%" />
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="OBSERVACIONES" HeaderStyle-Width="0%" HeaderText="Observaciones">
                                    <HeaderStyle HorizontalAlign="Center" Width="0%" />
                                    <ItemStyle HorizontalAlign="Left" Width="0%" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="GridSelected" />
                                <PagerStyle CssClass="GridHeader" />
                                <HeaderStyle CssClass="tblHead" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                            <br />
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="Hfd_No_Turno" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


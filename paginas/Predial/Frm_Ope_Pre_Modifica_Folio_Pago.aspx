<%@ Page Title="" Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Modifica_Folio_Pago.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Modifica_Folio_Pago" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
                            Modificación de Folio de Recibo de Pago
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
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ToolTip="Modificar" CssClass="Img_Button" TabIndex="1"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" AlternateText = "Nuevo"
                                            OnClick = "Btn_Nuevo_Click"/>
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" TabIndex="5"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" AlternateText = "Salir" OnClick = "Btn_Salir_Click" />
                                    </td>
                                    <td align="right" style="width:41%;">
                                        <table style="width:100%;height:28px;">
                                            <tr>
                                                <td style="vertical-align:middle;text-align:right;width:20%;">B&uacute;squeda:</td>
                                                <td style="width:15%;">
                                                    <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" TabIndex="5"  ToolTip = "Buscar" Width="180px"/>
                                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                                        WatermarkText="<Buscar por Recibo>" TargetControlID="Txt_Busqueda" />
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" 
                                                        runat="server" TargetControlID="Txt_Busqueda" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="ÑñáéíóúÁÉÍÓÚ. "/>
                                                </td>
                                                <td style="vertical-align:middle;width:5%;" >
                                                    <asp:ImageButton ID="Btn_Buscar" runat="server" TabIndex="6"
                                                        ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                                        OnClick = "Btn_Buscar_Click"/>
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
                            MaxLength = "10" Visible = "false" />
                        </td>
                    </tr> 
                                      
                    <tr>
                        <td style="text-align:left;width:20%;">
                            Folio actual
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Folio_Actual" runat="server" Width="96.4%" 
                            MaxLength = "10"/>
                        </td>
                        <td style="text-align:left;width:20%;text-align:right;">
                            *Folio nuevo
                        </td>
                        <td style="text-align:left;width:30%;">
                            <asp:TextBox ID="Txt_Folio_Nuevo" runat="server" Width="96.4%" 
                            MaxLength = "10"/>
                        </td>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                 runat="server" FilterType="Custom, Numbers" 
                                 TargetControlID="Txt_Folio_Nuevo" ValidChars="0123456789" 
                                 Enabled="True">
                        </cc1:FilteredTextBoxExtender>
                    </tr>
                    <tr>
                        <td style="text-align:left;width:20%;">
                            *Motivo
                        </td>
                        <td colspan="3" style="text-align:left;width:80%; vertical-align:top;">
                            <asp:TextBox ID="Txt_Motivo" runat="server" TabIndex="10" MaxLength="250"
                                TextMode="MultiLine" Width="98.6%" AutoPostBack="True"/>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Motivo" 
                                runat="server" TargetControlID="Txt_Motivo" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ "/>
                        </td>
                    </tr>
                    
                    <tr><td colspan="4">&nbsp;</td></tr>
                    
                     <tr align="center">
                        <td colspan="4">
                            <asp:GridView ID="Grid_Modifica_Folio_Pago" runat="server" AllowPaging="True" CssClass="GridView_1"
                                AutoGenerateColumns="False" PageSize="5" Width="98%"
                                EmptyDataText="&quot;No se encontraron registros&quot;"
                                AllowSorting="True" HeaderStyle-CssClass="tblHead" style="white-space:normal;"
                                OnSelectedIndexChanged = "Grid_Modifica_Folio_Pago_SelectedIndexChanged"
                                OnPageIndexChanging = "Grid_Modifica_Folio_Pago_PageIndexChanging" 
                                EnableModelValidation="True" >
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="5%" HorizontalAlign="Center"/>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="NO_PAGO" HeaderText="No. Pago" />
                                    <asp:BoundField DataField="FOLIO" HeaderText="Folio">
                                        <ItemStyle Width="20%" HorizontalAlign="Center"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DOCUMENTO" HeaderText="Documento" />
                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha" 
                                        DataFormatString="{0:dd/MMM/yyyy}">
                                    <ItemStyle Width="20%" HorizontalAlign="Center"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="OPERACION" HeaderText="# Ope." />
                                    <asp:BoundField DataField="CAJA" HeaderText="Caja" />
                                    <asp:BoundField DataField="CLAVE_BANCO" HeaderText="Cve. Banco" />
                                    <asp:BoundField DataField="CORRIENTE" HeaderText="Total" DataFormatString="{0:c}" HeaderStyle-HorizontalAlign="Right">
                                    <ItemStyle Width="20%" HorizontalAlign="Right"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="REZAGOS" HeaderText="Rezagos" />
                                    <asp:BoundField DataField="RECARGOS_ORDINARIOS" HeaderText="Recargos ordinarios" />
                                    <asp:BoundField DataField="RECARGOS_MORATORIOS" HeaderText="Recargos moratorios" />
                                    <asp:BoundField DataField="HONORARIOS" HeaderText="Honorarios" />
                                    <asp:BoundField DataField="MULTAS" HeaderText="Multas" />
                                    <asp:BoundField DataField="DESCUENTO" HeaderText="Descuento" />
                                    <%--<asp:BoundField DataField="MONTO" HeaderText="Monto" />--%>
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


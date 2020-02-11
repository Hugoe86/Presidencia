<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Descuentos_Generales.aspx.cs" Inherits="paginas_Predial_Frm_Cat_Pre_Descuentos_Generales" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" EnablePartialRendering="true"/>
    <asp:UpdatePanel ID="Upd_Panel_Padron_Predios" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Padrones" runat="server" AssociatedUpdatePanelID="Upd_Panel_Padron_Predios" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>
            </asp:UpdateProgress>
                <div id="General" style="background-color:#ffffff; width:100%; height:100%;">
                    <table id="Tbl_Comandos" border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                                <tr>
                                    <td class="label_titulo" colspan="2">
                                        Catálogo de Descuentos Generales</td>
                                </tr>
                                <tr>
                                    <div id="Div_Contenedor_error" runat="server">
                                    <td colspan="2">
                                        <asp:Image ID="Img_Error" runat="server" 
                                            ImageUrl = "../imagenes/paginas/sias_warning.png"/>
                                        <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                        <br />
                                        <asp:Label ID="Lbl_Error" runat="server" Text="" TabIndex="0" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                                    </td>
                                    </div>
                                </tr>
                                <tr class="barra_busqueda">
                                    <td style="width:50%">
                                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png"
                                        CssClass="Img_Button" onclick="Btn_Nuevo_Click"/>
                                        <asp:ImageButton ID="Btn_Modificar" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png"
                                        CssClass="Img_Button" onclick="Btn_Modificar_Click"/>
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                        CssClass="Img_Button" onclick="Btn_Salir_Click1"/>
                                    </td>
                                    <td align="right" style="width:50%">
                                        Búsqueda:
                                        <asp:TextBox ID="Txt_Busqueda" runat="server" Width="180"
                                        ToolTip="Buscar" TabIndex="1" ></asp:TextBox>
                                        <asp:ImageButton ID="Btn_Busqueda" runat="server" 
                                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                            TabIndex="2" onclick="Btn_Buscar_Descuentos_Generales_Click"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Busqueda" runat="server" WatermarkCssClass="watermarked"
                                            WatermarkText="<Buscar>" TargetControlID="Txt_Busqueda"/>
                                    </td>
                                </tr>
                            </table>
                    <table border="0" cellspacing="0" class="estilo_fuente" style="width: 98%;">
                        <tr>
                            <td style="width:18%">&nbsp;</td>
                            <td style="width:32%">&nbsp;</td>
                            <td style="width:18%">&nbsp;</td>
                            <td style="width:32%">&nbsp;</td>
                        </tr>
                        <tr>
                        <td style="width:18%">
                                *Tipo impuesto</td>
                            <td style="width:32%">
                                <asp:DropDownList ID="Cmb_Tipo_Descuento" Width="94%" runat="server">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                <asp:ListItem Text="PREDIAL" Value="PREDIAL" />
                                <asp:ListItem Text="RECARGOS ORDINARIOS" Value="RECARGOS ORDINARIOS" />
                                <asp:ListItem Text="RECARGOS MORATORIOS" Value="RECARGOS MORATORIOS" />
                                <asp:ListItem Text="HONORARIOS" Value="HONORARIOS" />
                                <asp:ListItem Text="MULTAS" Value="MULTAS" />
                                </asp:DropDownList>
                            </td>
                            <td style="width:18%">
                                *Estatus</td>
                            <td style="width:32%">
                                <asp:DropDownList ID="Cmb_Estatus" Width="94%" runat="server">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                <asp:ListItem Text="BAJA" Value="BAJA" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:18%">
                                *Vigencia desde</td>
                            <td style="width:32%">
                                <asp:TextBox ID="Txt_Vigencia_Desde" runat="server" Width="82%" Text="" ></asp:TextBox>
                                <cc1:CalendarExtender ID="Txt_Vigencia_Desde_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="Txt_Vigencia_Desde" PopupButtonID="Img_Calendario1" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkCssClass="watermarked"
                                     WatermarkText="Día/Mes/Año" TargetControlID="Txt_Vigencia_Desde">
                                </cc1:TextBoxWatermarkExtender>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                runat="server" FilterType="Numbers, Custom" 
                                TargetControlID="Txt_Vigencia_Desde" ValidChars="1234567890/">
                            </cc1:FilteredTextBoxExtender>
                                <asp:ImageButton ID="Img_Calendario1" runat="server" Width="20px" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"/>
                            </td>
                            <td style="width:18%">*Vigencia hasta</td>
                            <td style="width:32%">
                                <asp:TextBox ID="Txt_Vigencia_hasta" runat="server" Width="82%" Text=""></asp:TextBox>
                                <cc1:CalendarExtender ID="Txt_Vigencia_hasta_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="Txt_Vigencia_hasta" PopupButtonID="Img_Calendario2" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                runat="server" FilterType="Numbers, Custom" 
                                TargetControlID="Txt_Vigencia_hasta" ValidChars="1234567890/">
                            </cc1:FilteredTextBoxExtender>
                                <asp:ImageButton ID="Img_Calendario2" runat="server" Width="18px" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif"/>
                                        </td>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" WatermarkCssClass="watermarked"
                                     WatermarkText="Día/Mes/Año" TargetControlID="Txt_Vigencia_Hasta">
                                </cc1:TextBoxWatermarkExtender>
                        </tr>
                        <tr>
                            <td style="width:18%">
                                *Porcentaje descuento</td>
                            <td style="width:32%">
                                <asp:TextBox ID="Txt_Porcentaje_Descuento" runat="server" Width="82%" Text=""></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                                runat="server" FilterType="Numbers" 
                                TargetControlID="Txt_Porcentaje_Descuento" ValidChars="1234567890">
                            </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                         <tr>
                            <td style="width:18%">
                                *Motivo (se imprime)</td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Motivo" runat="server" Width="97%" Text="" Style="text-transform: uppercase"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:18%">
                                Comentarios</td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Comentarios" runat="server" Width="97%" Height="40px" Style="text-transform: uppercase"
                                    TextMode="MultiLine" ></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="Twe_Txt_Comentarios" runat="server" WatermarkCssClass="watermarked"
                                     WatermarkText="Límite de Caractes 250" TargetControlID="Txt_Comentarios">
                                </cc1:TextBoxWatermarkExtender>
                                <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                TargetControlID="Txt_Comentarios" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:18%">
                            </td>
                            <td style="width:32%">
                            </td>
                            <td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                            <asp:TextBox ID="Txt_id" runat="server" Width="92%" Text="" Visible="false"></asp:TextBox>
                                </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <asp:GridView ID="Grid_Descuentos_Generales" runat="server" AllowPaging="true" 
                                    AutoGenerateColumns="False" CssClass="GridView_1" 
                                    EmptyDataText="&quot;No se encontraron registros&quot;" GridLines="none" 
                                    PageSize="5" Style="white-space:normal" Width="96%" 
                                    onpageindexchanging="Grid_Descuentos_Generales_PageIndexChanging" 
                                    onselectedindexchanged="Grid_Descuentos_Generales_SelectedIndexChanged">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                            <ItemStyle Width="5%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="DESCUENTOS_GENERALES_ID" HeaderText="Id" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TIPO_DE_DESCUENTO" HeaderText="Tipo de Descuento">
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PORCENTAJE_DESCUENTO" HeaderText="Porcentaje Descuento">
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus">
                                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VIGENCIA_DESDE" HeaderText="Vigencia Desde" DataFormatString="{0:dd/MM/yyyy}">
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VIGENCIA_HASTA" HeaderText="Vigencia Hasta" DataFormatString="{0:dd/MM/yyyy}">
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="COMENTARIOS" HeaderText="Comentarios">
                                            <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                            <ItemStyle HorizontalAlign="Left" Width="0%" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                                                <tr>
                            <td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                                &nbsp;</td>
                            <td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                                &nbsp;</td>
                            <td style="width:18%">
                                &nbsp;</td>
                            <td style="width:32%">
                                &nbsp;</td>
                        </tr>
                    </table>
                    
                </div>
            </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
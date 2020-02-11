<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Recargos.aspx.cs" Inherits="paginas_predial_Frm_Cat_Pre_Recargos" Title="Catalogo de Recargos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Recargos" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="2" class="label_titulo">Catálogo de Recargos</td>
                    </tr>
                    <tr>
                        <td>
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top">
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                        
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Nuevo" OnClick="Btn_Nuevo_Click"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Modificar" onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Eliminar" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_eliminar.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Eliminar" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro de la Base de Datos?');"
                                onclick="Btn_Eliminar_Click"/>
                            <asp:ImageButton ID="Btn_Salir" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" onclick="Btn_Salir_Click"/>
                        </td>
                        <td>Busqueda:
                            <asp:TextBox ID="Txt_Busqueda_Recargo" runat="server" Width="130px"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Recargo" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                onclick="Btn_Buscar_Recargo_Click" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Recargo" runat="server" WatermarkText="<Identificador>" TargetControlID="Txt_Busqueda_Recargo" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>    
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Recargo" runat="server" TargetControlID="Txt_Busqueda_Recargo" InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>      
                        </td>                        
                    </tr>
                </table>   
                <br />
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="98%" ActiveTabIndex="0">
                    <cc1:TabPanel  runat="server" HeaderText="TabPanel1"  ID="TabPanel1"  Width="100%"  >
                        <HeaderTemplate>Recargos</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table width="100%">
                                    <tr>
                                        <td colspan="4">
                                            <asp:HiddenField ID="Hdf_Recargo_ID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:18%; text-align:left;">
                                            <asp:Label ID="Lbl_ID_Recargo" 
                                                runat="server" Text="Recargo ID" CssClass="estilo_fuente"></asp:Label></td>
                                        <td style="width:32%">
                                            <asp:TextBox ID="Txt_ID_Recargo" runat="server" Width="98%" 
                                                MaxLength="10" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Nombre" runat="server" Text="* Identificador" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:35%">
                                            <asp:TextBox ID="Txt_Identificador" runat="server" Width="98%" MaxLength="10"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;"><asp:Label ID="Lbl_Estatus" runat="server" Text="* Estatus" CssClass="estilo_fuente"></asp:Label></td>
                                        <td style="width:35%">
                                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                                <asp:ListItem Text="ACTIVO" Value="ACTIVO" />
                                                <asp:ListItem Text="INACTIVO" Value="INACTIVO" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%; text-align:left; vertical-align:top;"><asp:Label ID="Lbl_Descripcion" runat="server" Text="* Descripci&oacute;n" CssClass="estilo_fuente"></asp:Label></td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Descripcion" runat="server" Rows="3" TextMode="MultiLine" Width="98%"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Descripcion" runat="server" TargetControlID ="Txt_Descripcion" 
                                                WatermarkText="Límite de Caractes 100" WatermarkCssClass="watermarked" Enabled="True"/>          
                                        </td>
                                    </tr>
                                </table>
                                <br />                                
                                <asp:GridView ID="Grid_Recargos_Generales" runat="server" CssClass="GridView_1"
                                    AutoGenerateColumns="False" AllowPaging="True" PageSize="5" Width="98%"
                                    GridLines= "None"
                                    onselectedindexchanged="Grid_Recargos_Generales_SelectedIndexChanged" 
                                    onpageindexchanging="Grid_Recargos_Generales_PageIndexChanging">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png"  >
                                            <ItemStyle Width="30px" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="RECARGO_ID" HeaderText="Recargo ID" 
                                            SortExpression="RECARGO_ID" />
                                        <asp:BoundField DataField="IDENTIFICADOR" HeaderText="Identificador" 
                                            SortExpression="Identificador" />
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" 
                                            SortExpression="Estatus" />
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />                                
                                    <AlternatingRowStyle CssClass="GridAltItem" />       
                                </asp:GridView>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel  runat="server" HeaderText="TabPanel2"  ID="TabPanel2"  Width="100%"  >
                        <HeaderTemplate>Recargos - Tasas</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table width="100%">
                                    <tr>
                                        <td colspan="2">
                                            <asp:HiddenField ID="Hdf_Recargo_Tasa_ID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:18%; text-align:left;">
                                            <asp:Label ID="Lbl_Tasa_ID" runat="server" Text="Recargo Tasa ID" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="text-align:left;">
                                            <asp:TextBox ID="Txt_Tasa_ID" runat="server" MaxLength="5" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>    
                                    <tr>
                                        <td style="width:18%; text-align:left;">
                                            <asp:Label ID="Lbl_No_Bimestro" runat="server" Text="* No Bimestro" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="text-align:left;">
                                            <asp:TextBox ID="Txt_No_Bimestro" runat="server" MaxLength="4"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Bimestro" runat="server" TargetControlID="Txt_No_Bimestro" FilterType="Numbers" Enabled="True"></cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Panel ID="Pnl_Tasas__Por_Mes" runat="server" Width="98%" Style="border-style:outset;">
                                                <table width="99%">
                                                    <tr>
                                                        <td style="text-align:left; width:13%">
                                                            <asp:Label ID="Lbl_Enero" runat="server" Text="Enero" CssClass="estilo_fuente"></asp:Label>
                                                        </td>
                                                        <td style="text-align:left; width:20%">
                                                            <asp:TextBox ID="Txt_Enero" runat="server" Width="98%"></asp:TextBox>
                                                            <cc1:MaskedEditExtender ID="MEE_Txt_Enero" runat="server" 
                                                                TargetControlID="Txt_Enero" Mask="9,999,999.99" MaskType="Number" 
                                                                InputDirection="RightToLeft" AcceptNegative="Left" ErrorTooltipEnabled="True" 
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                                        </td>
                                                        <td style="text-align:left; width:13%">
                                                            <asp:Label ID="Lbl_Febrero" runat="server" Text="Febrero" CssClass="estilo_fuente"></asp:Label>
                                                        </td>
                                                        <td style="text-align:left; width:20%">
                                                            <asp:TextBox ID="Txt_Febrero" runat="server" Width="98%"></asp:TextBox>
                                                            <cc1:MaskedEditExtender ID="MEE_Txt_Febrero" runat="server" 
                                                                TargetControlID="Txt_Febrero" Mask="9,999,999.99" MaskType="Number" 
                                                                InputDirection="RightToLeft" AcceptNegative="Left" ErrorTooltipEnabled="True" 
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                                        </td>
                                                        <td style="text-align:left; width:13%">
                                                            <asp:Label ID="Lbl_Marzo" runat="server" Text="Marzo" CssClass="estilo_fuente"></asp:Label>
                                                        </td>
                                                        <td style="text-align:left; width:21%">
                                                            <asp:TextBox ID="Txt_Marzo" runat="server" Width="98%"></asp:TextBox>
                                                            <cc1:MaskedEditExtender ID="MEE_Txt_Marzo" runat="server" 
                                                                TargetControlID="Txt_Marzo" Mask="9,999,999.99" MaskType="Number" 
                                                                InputDirection="RightToLeft" AcceptNegative="Left" ErrorTooltipEnabled="True" 
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align:left; width:13%">
                                                            <asp:Label ID="Lbl_Abril" runat="server" Text="Abril" CssClass="estilo_fuente"></asp:Label>
                                                        </td>
                                                        <td style="text-align:left; width:20%">
                                                            <asp:TextBox ID="Txt_Abril" runat="server" Width="98%"></asp:TextBox>
                                                            <cc1:MaskedEditExtender ID="MEE_Txt_Abril" runat="server" 
                                                                TargetControlID="Txt_Abril" Mask="9,999,999.99" MaskType="Number" 
                                                                InputDirection="RightToLeft" AcceptNegative="Left" ErrorTooltipEnabled="True" 
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                                        </td>
                                                        <td style="text-align:left; width:13%">
                                                            <asp:Label ID="Lbl_Mayo" runat="server" Text="Mayo" CssClass="estilo_fuente"></asp:Label>
                                                        </td>
                                                        <td style="text-align:left; width:20%">
                                                            <asp:TextBox ID="Txt_Mayo" runat="server" Width="98%"></asp:TextBox>
                                                            <cc1:MaskedEditExtender ID="MEE_Txt_Mayo" runat="server" 
                                                                TargetControlID="Txt_Mayo" Mask="9,999,999.99" MaskType="Number" 
                                                                InputDirection="RightToLeft" AcceptNegative="Left" ErrorTooltipEnabled="True" 
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                                        </td>
                                                        <td style="text-align:left; width:13%">
                                                            <asp:Label ID="Lbl_Junio" runat="server" Text="Junio" CssClass="estilo_fuente"></asp:Label>
                                                        </td>
                                                        <td style="text-align:left; width:21%">
                                                            <asp:TextBox ID="Txt_Junio" runat="server" Width="98%"></asp:TextBox>
                                                            <cc1:MaskedEditExtender ID="MEE_Txt_Junio" runat="server" 
                                                                TargetControlID="Txt_Junio" Mask="9,999,999.99" MaskType="Number" 
                                                                InputDirection="RightToLeft" AcceptNegative="Left" ErrorTooltipEnabled="True" 
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align:left; width:13%">
                                                            <asp:Label ID="Lbl_Julio" runat="server" Text="Julio" CssClass="estilo_fuente"></asp:Label>
                                                        </td>
                                                        <td style="text-align:left; width:20%">
                                                            <asp:TextBox ID="Txt_Julio" runat="server" Width="98%"></asp:TextBox>
                                                            <cc1:MaskedEditExtender ID="MEE_Txt_Julio" runat="server" 
                                                                TargetControlID="Txt_Julio" Mask="9,999,999.99" MaskType="Number" 
                                                                InputDirection="RightToLeft" AcceptNegative="Left" ErrorTooltipEnabled="True" 
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                                        </td>
                                                        <td style="text-align:left; width:13%">
                                                            <asp:Label ID="Lbl_Agosto" runat="server" Text="Agosto" CssClass="estilo_fuente"></asp:Label>
                                                        </td>
                                                        <td style="text-align:left; width:20%">
                                                            <asp:TextBox ID="Txt_Agosto" runat="server" Width="98%"></asp:TextBox>
                                                            <cc1:MaskedEditExtender ID="MEE_Txt_Agosto" runat="server" 
                                                                TargetControlID="Txt_Agosto" Mask="9,999,999.99" MaskType="Number" 
                                                                InputDirection="RightToLeft" AcceptNegative="Left" ErrorTooltipEnabled="True" 
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                                        </td>
                                                        <td style="text-align:left; width:13%">
                                                            <asp:Label ID="Lbl_Septiembre" runat="server" Text="Septiembre" CssClass="estilo_fuente"></asp:Label>
                                                        </td>
                                                        <td style="text-align:left; width:21%">
                                                            <asp:TextBox ID="Txt_Septiembre" runat="server" Width="98%"></asp:TextBox>
                                                            <cc1:MaskedEditExtender ID="MEE_Txt_Septiembre" runat="server" 
                                                                TargetControlID="Txt_Septiembre" Mask="9,999,999.99" MaskType="Number" 
                                                                InputDirection="RightToLeft" AcceptNegative="Left" ErrorTooltipEnabled="True" 
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align:left; width:13%">
                                                            <asp:Label ID="Lbl_Octubre" runat="server" Text="Octubre" CssClass="estilo_fuente"></asp:Label>
                                                        </td>
                                                        <td style="text-align:left; width:20%">
                                                            <asp:TextBox ID="Txt_Octubre" runat="server" Width="98%"></asp:TextBox>
                                                            <cc1:MaskedEditExtender ID="MEE_Txt_Octubre" runat="server" 
                                                                TargetControlID="Txt_Octubre" Mask="9,999,999.99" MaskType="Number" 
                                                                InputDirection="RightToLeft" AcceptNegative="Left" ErrorTooltipEnabled="True" 
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                                        </td>
                                                        <td style="text-align:left; width:13%">
                                                            <asp:Label ID="Lbl_Noviembre" runat="server" Text="Noviembre" CssClass="estilo_fuente"></asp:Label>
                                                        </td>
                                                        <td style="text-align:left; width:20%">
                                                            <asp:TextBox ID="Txt_Noviembre" runat="server" Width="98%"></asp:TextBox>
                                                            <cc1:MaskedEditExtender ID="MEE_Txt_Noviembre" runat="server" 
                                                                TargetControlID="Txt_Noviembre" Mask="9,999,999.99" MaskType="Number" 
                                                                InputDirection="RightToLeft" AcceptNegative="Left" ErrorTooltipEnabled="True" 
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                                        </td>
                                                        <td style="text-align:left; width:13%">
                                                            <asp:Label ID="Lbl_Diciembre" runat="server" Text="Diciembre" CssClass="estilo_fuente"></asp:Label>
                                                        </td>
                                                        <td style="text-align:left; width:21%">
                                                            <asp:TextBox ID="Txt_Diciembre" runat="server" Width="98%"></asp:TextBox>
                                                            <cc1:MaskedEditExtender ID="MEE_Txt_Diciembre" runat="server" 
                                                                TargetControlID="Txt_Diciembre" Mask="9,999,999.99" MaskType="Number" 
                                                                InputDirection="RightToLeft" AcceptNegative="Left" ErrorTooltipEnabled="True" 
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>                                        
                                    <tr>
                                        <td colspan="3">&nbsp;</td>
                                        <td style="width:25%;">
                                            <asp:ImageButton ID="Btn_Ver_Tasas" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                AlternateText="Ver Tasas p/Mes" onclick="Btn_Ver_Tasas_Click" />                                                
                                            <asp:ImageButton ID="Btn_Agregar_Tasa" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                                AlternateText="Agregar" onclick="Btn_Agregar_Tasa_Click" />
                                            <asp:ImageButton ID="Btn_Modificar_Tasa" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/actualizar_detalle.png" 
                                                AlternateText="Modificar" onclick="Btn_Modificar_Tasa_Click" />
                                            <asp:ImageButton ID="Btn_Quitar_Tasa" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/quitar.png" 
                                                AlternateText="Quitar" onclick="Btn_Quitar_Tasa_Click" />
                                        </td>  
                                    </tr>
                                </table>
                                <br />
                                <asp:GridView ID="Grid_Recargos_Tasas" runat="server" 
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                    GridLines="None" AllowPaging="True" Width="98%"
                                    OnPageIndexChanging="Grid_Recargos_Tasas_PageIndexChanging" 
                                    PageSize="5" 
                                    onselectedindexchanged="Grid_Recargos_Tasas_SelectedIndexChanged">
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                            <ItemStyle Width="30px" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="RECARGO_TASA_ID" 
                                            HeaderText="RECARGO_TASA_ID" SortExpression="Id_Recargo" />
                                        <asp:BoundField DataField="NO_BIMESTRO" HeaderText="No. Bimestro" 
                                            SortExpression="No_Bimestro" />
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />                                
                                    <AlternatingRowStyle CssClass="GridAltItem" />       
                                </asp:GridView>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>      
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
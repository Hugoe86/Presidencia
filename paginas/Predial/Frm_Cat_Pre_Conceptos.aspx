<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Conceptos.aspx.cs" Inherits="paginas_predial_Frm_Cat_Pre_Conceptos" Title="Catalogo de Conceptos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Conceptos" runat="server" />
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
                        <td class="label_titulo" colspan="2">Catálogo de Conceptos</td>
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
                                <td style="font-size:10px;width:90%;text-align:left;" valign="top">
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error"/>
                                </td>
                              </tr>          
                            </table>                   
                          </div>                         
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>                        
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
                        <td>Búsqueda:
                            <asp:TextBox ID="Txt_Busqueda_Concepto" runat="server" Width="130px"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Concepto" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                onclick="Btn_Buscar_Concepto_Click" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Concepto" runat="server" WatermarkText="<Identificador>" TargetControlID="Txt_Busqueda_Concepto" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Concepto" runat="server" TargetControlID="Txt_Busqueda_Concepto" InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>                                  
                        </td>                        
                    </tr>
                </table>   
                <br />
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="98%" 
                    ActiveTabIndex="0">
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Impuestos"  ID="Tab_Impuestos"  Width="100%"  ><HeaderTemplate>Conceptos</HeaderTemplate><ContentTemplate><center><table width="100%"><tr>
                        <td colspan="4"><asp:HiddenField ID="Hdf_Concepto_ID" runat="server" /></td></tr><tr><td style="width:18%; text-align:left;"><asp:Label ID="Lbl_ID_Concepto" 
                                                runat="server" Text="Concepto ID" CssClass="estilo_fuente"></asp:Label></td><td style="width:32%"><asp:TextBox ID="Txt_ID_Concepto" runat="server" Width="98%" 
                                                MaxLength="10" Enabled="False"></asp:TextBox></td></tr><tr><td style="width:18%; text-align:left; "><asp:Label ID="Lbl_Identificador" runat="server" Text="* Identificador" CssClass="estilo_fuente"></asp:Label></td><td style="width:32%"><asp:TextBox ID="Txt_Identificador" runat="server" Width="98%" MaxLength="10"></asp:TextBox></td></tr><tr><td style="width:15%; text-align:left;"><asp:Label ID="Lbl_Estatus" runat="server" Text="* Estatus" CssClass="estilo_fuente"></asp:Label></td><td style="width:35%"><asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%"><asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" /><asp:ListItem Text="VIGENTE" Value="VIGENTE" /><asp:ListItem Text="BAJA" Value="BAJA" /></asp:DropDownList></td><td style="width:15%; text-align:left;"><asp:Label ID="Lbl_Tipo_Concepto" runat="server" Text="* Tipo Concepto" CssClass="estilo_fuente"></asp:Label></td><td style="width:25%"><asp:DropDownList ID="Cmb_Tipo_Concepto" runat="server" Width="98%"><asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" /><asp:ListItem Text="PREDIAL" Value="PREDIAL" /><asp:ListItem Text="TRASLADO DOMINIO" Value="TRASLADO DOMINIO" /><asp:ListItem Text="AMBOS" Value="AMBOS" /></asp:DropDownList></td></tr><tr><td style="width:15%; text-align:left; vertical-align:top;"><asp:Label ID="Lbl_Descripcion" runat="server" Text="* Descripci&oacute;n" CssClass="estilo_fuente"></asp:Label></td>
                        <td colspan="3"><asp:TextBox ID="Txt_Descripcion" runat="server" Rows="3" TextMode="MultiLine" Width="98%"></asp:TextBox><cc1:TextBoxWatermarkExtender ID="TWE_Txt_Descripcion" runat="server" TargetControlID ="Txt_Descripcion" 
                                                WatermarkText="Límite de Caractes 100" WatermarkCssClass="watermarked" Enabled="True"/></td></tr></table><br /></center></ContentTemplate></cc1:TabPanel>
                    
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Impuestos_Predial"  ID="Tab_Impuestos_Predial"  Width="100%"  ><HeaderTemplate>Conceptos - Impuestos Predial</HeaderTemplate><ContentTemplate><center><table width="100%"><tr><td colspan="4"><asp:HiddenField ID="Hdf_Concepto_Impuesto_Predial_ID" runat="server" /></td></tr><tr><td style="width:18%; text-align:left;"><asp:Label ID="Lbl_Impuesto_Predial_ID" 
                                                runat="server" Text="Impuesto Predial ID" CssClass="estilo_fuente"></asp:Label></td><td style="width:32%"><asp:TextBox ID="Txt_Impuesto_Predial_ID" runat="server" Width="98%" 
                                                MaxLength="10" Enabled="False"></asp:TextBox></td></tr><tr><td style="width:15%; text-align:left;"><asp:Label ID="Lbl_Imp_Predial_Anio" runat="server" Text="* Año" CssClass="estilo_fuente"></asp:Label></td><td style="width:35%; text-align:left;"><asp:TextBox ID="Txt_Imp_Predial_Anio" runat="server" Width="98%" MaxLength="4"></asp:TextBox><cc1:FilteredTextBoxExtender ID="FTE_Txt_Anio" runat="server" TargetControlID="Txt_Imp_Predial_Anio" FilterType="Numbers" Enabled="True"></cc1:FilteredTextBoxExtender></td><td style="width:15%; text-align:left;"><asp:Label ID="Lbl_Imp_Predial_Tasa" runat="server" Text="* Tasa" CssClass="estilo_fuente" ></asp:Label></td><td style="width:35%; text-align:left;"><asp:TextBox ID="Txt_Imp_Predial_Tasa" runat="server" Width="98%"></asp:TextBox>
                            <cc1:MaskedEditExtender ID="MEE_Txt_Imp_Predial_Tasa" runat="server" 
                                TargetControlID="Txt_Imp_Predial_Tasa" Mask="9,999,999.99" MaskType="Number" 
                                InputDirection="RightToLeft" AcceptNegative="Left" ErrorTooltipEnabled="True" 
                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" /></td></tr><tr><td colspan="3">&nbsp;</td><td style="width:15%;"><asp:ImageButton ID="Btn_Agregar_Impuesto_Predial" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                                AlternateText="Agregar" onclick="Btn_Agregar_Impuesto_Predial_Click" /><asp:ImageButton ID="Btn_Modificar_Impuesto_Predial" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/actualizar_detalle.png" 
                                                AlternateText="Modificar" onclick="Btn_Modificar_Impuesto_Predial_Click" /><asp:ImageButton ID="Btn_Quitar_Impuesto_Predial" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/quitar.png" 
                                                AlternateText="Quitar" onclick="Btn_Quitar_Impuesto_Predial_Click" /></td></tr></table><br /><asp:GridView ID="Grid_Conceptos_Impuesto_Predial" runat="server" 
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                    GridLines="None" AllowPaging="True" Width="98%" CssClass="GridView_1"
                                    OnPageIndexChanging="Grid_Conceptos_Impuesto_Predial_PageIndexChanging" 
                                    OnSelectedIndexChanged="Grid_Conceptos_Impuesto_Predial_SelectedIndexChanged"
                                    PageSize="5" ><RowStyle CssClass="GridItem" /><Columns><asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png" ><ItemStyle Width="30px" /></asp:ButtonField><asp:BoundField DataField="IMPUESTO_ID_PREDIAL" 
                                            HeaderText="IMPUESTO_ID_PREDIAL" SortExpression="Id_Impuesto_Predial" /><asp:BoundField DataField="ANIO" HeaderText="Año" 
                                            SortExpression="Identificador" /><asp:BoundField DataField="TASA" HeaderText="Tasa" 
                                            SortExpression="Estatus" /></Columns><PagerStyle CssClass="GridHeader" /><SelectedRowStyle CssClass="GridSelected" /><HeaderStyle CssClass="GridHeader" /><AlternatingRowStyle CssClass="GridAltItem" /></asp:GridView></center></ContentTemplate></cc1:TabPanel>
                    
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Impuestos_Traslacion"  ID="Tab_Impuestos_Traslacion"  Width="100%"  ><HeaderTemplate>Conceptos - Impuestos Traslaci&oacute;n</HeaderTemplate><ContentTemplate><center><table width="100%"><tr><td colspan="4"><asp:HiddenField ID="Hdf_Concepto_Impuesto_Traslacion_ID" runat="server" /></td></tr><tr><td style="width:18%; text-align:left;"><asp:Label ID="Lbl_Imp_Traslacion_ID" runat="server" Text="Impuesto Traslaci&oacute;n ID" CssClass="estilo_fuente"></asp:Label></td><td style="width:32%"><asp:TextBox ID="Txt_Imp_Traslacion_ID" runat="server" Width="98%" MaxLength="10" Enabled="False"></asp:TextBox></td></tr><tr><td style="width:15%; text-align:left;"><asp:Label ID="Lbl_Imp_Traslacion_Anio" runat="server" Text="Año" CssClass="estilo_fuente"></asp:Label></td><td style="width:35%; text-align:left;"><asp:TextBox ID="Txt_Imp_Traslacion_Anio" runat="server" Width="98%" MaxLength="4"></asp:TextBox><cc1:FilteredTextBoxExtender ID="FTE_Txt_Imp_Traslacion_Anio" runat="server" TargetControlID="Txt_Imp_Traslacion_Anio" FilterType="Numbers" Enabled="True"></cc1:FilteredTextBoxExtender></td><td style="width:15%; text-align:left;"><asp:Label ID="Lbl_Imp_Tralasion_Tasa" runat="server" Text="Tasa" CssClass="estilo_fuente" ></asp:Label></td><td style="width:35%; text-align:left;"><asp:TextBox ID="Txt_Imp_Traslacion_Tasa" runat="server" Width="98%"></asp:TextBox>
                        <cc1:MaskedEditExtender ID="MEE_Txt_Imp_Traslacion_Tasa" runat="server" 
                            TargetControlID="Txt_Imp_Traslacion_Tasa" Mask="9,999,999.99" MaskType="Number" 
                            InputDirection="RightToLeft" AcceptNegative="Left" ErrorTooltipEnabled="True" 
                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                            CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" /></td></tr><tr><td style="width:15%; text-align:left;"><asp:Label ID="Lbl_Imp_Trasl_Deducible_Normal" runat="server" Text="Deducible Normal" CssClass="estilo_fuente"></asp:Label></td><td style="width:35%; text-align:left;"><asp:TextBox ID="Txt_Imp_Trasl_Deducible_Normal" runat="server" Width="98%"></asp:TextBox>
                            <cc1:MaskedEditExtender ID="MEE_Txt_Imp_Trasl_Deducible_Normal" runat="server" 
                                TargetControlID="Txt_Imp_Trasl_Deducible_Normal" Mask="9,999,999.99" 
                                MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left" 
                                ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" 
                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" /></td><td style="width:15%; text-align:left;"><asp:Label ID="Lbl_Imp_Trasl_Deducible_Int_Social" runat="server" 
                                                Text="Deducible Interes Social" CssClass="estilo_fuente"></asp:Label></td><td style="width:35%; text-align:left;"><asp:TextBox ID="Txt_Imp_Trasl_Deducible_Int_Social" runat="server" Width="98%"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="MEE_Txt_Imp_Trasl_Deducible_Int_Social" 
                                    runat="server" TargetControlID="Txt_Imp_Trasl_Deducible_Int_Social" 
                                    Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" 
                                    AcceptNegative="Left" ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" 
                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                                    CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                    CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" /></td></tr><tr><td colspan="3">&nbsp;</td><td style="width:15%;"><asp:ImageButton ID="Btn_Agregar_Impuesto_Traslacion" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                                AlternateText="Agregar" onclick="Btn_Agregar_Impuesto_Traslacion_Click" /><asp:ImageButton ID="Btn_Modificar_Impuesto_Traslacion" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/actualizar_detalle.png" 
                                                AlternateText="Modificar" onclick="Btn_Modificar_Impuesto_Traslacion_Click" /><asp:ImageButton ID="Btn_Quitar_Impuesto_Traslacion" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/quitar.png" 
                                                AlternateText="Quitar" onclick="Btn_Quitar_Impuesto_Traslacion_Click" /></td></tr></table><br /><asp:GridView ID="Grid_Conceptos_Impuesto_Traslacion" runat="server" 
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                    GridLines="None" AllowPaging="True" Width="98%"
                                    PageSize="5"  CssClass="GridView_1"
                                    OnPageIndexChanging="Grid_Conceptos_Impuesto_Traslacion_PageIndexChanging" 
                                    OnSelectedIndexChanged="Grid_Conceptos_Impuesto_Traslacion_SelectedIndexChanged"><RowStyle CssClass="GridItem" /><Columns><asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png" ><ItemStyle Width="30px" /></asp:ButtonField><asp:BoundField DataField="IMPUESTO_ID_TRASLACION" 
                                            HeaderText="IMPUESTO_ID_TRASLACION" SortExpression="Impuesto_Id_Traslacion" /><asp:BoundField DataField="ANIO" HeaderText="Año" 
                                            SortExpression="Identificador" /><asp:BoundField DataField="TASA" HeaderText="Tasa" 
                                            SortExpression="Estatus" /><asp:BoundField DataField="DEDUCIBLE_NORMAL" HeaderText="Deducible Normal" 
                                            SortExpression="Deducible_Normal" /><asp:BoundField DataField="DEDUCIBLE_INTERES_SOCIAL" HeaderText="Deducible Interes Social" 
                                            SortExpression="Deducible_Interes_Social" /></Columns><PagerStyle CssClass="GridHeader" /><SelectedRowStyle CssClass="GridSelected" /><HeaderStyle CssClass="GridHeader" /><AlternatingRowStyle CssClass="GridAltItem" /></asp:GridView></center></ContentTemplate></cc1:TabPanel>
                </cc1:TabContainer>
                
                <table id="Tbl_Conceptos_Generales" border="0" cellspacing="0" class="estilo_fuente" style="width: 100%;">
                    <tr>
                <asp:GridView ID="Grid_Conceptos_Generales" runat="server" CssClass="GridView_1"
                                    GridLines= "None"
                                    AutoGenerateColumns="False" AllowPaging="True" PageSize="5" Width="98%"
                                    onselectedindexchanged="Grid_Conceptos_Generales_SelectedIndexChanged" 
                                    onpageindexchanging="Grid_Conceptos_Generales_PageIndexChanging"><AlternatingRowStyle CssClass="GridAltItem" /><Columns><asp:ButtonField ButtonType="Image" CommandName="Select" 
                                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png" ><ItemStyle Width="30px" /></asp:ButtonField><asp:BoundField DataField="CONCEPTO_PREDIAL_ID" HeaderText="Concepto ID" 
                                            SortExpression="CONCEPTO_ID" /><asp:BoundField DataField="IDENTIFICADOR" HeaderText="Identificador" 
                                            SortExpression="Identificador" /><asp:BoundField DataField="ESTATUS" HeaderText="Estatus" 
                                            SortExpression="Estatus" /></Columns><HeaderStyle CssClass="GridHeader" /><PagerStyle CssClass="GridHeader" /><RowStyle CssClass="GridItem" /><SelectedRowStyle CssClass="GridSelected" /></asp:GridView>
                
                </tr>
                </table>
            </div>
            <caption>
                <br />
            </caption>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
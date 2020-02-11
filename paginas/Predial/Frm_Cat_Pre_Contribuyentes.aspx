<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Cat_Pre_Contribuyentes.aspx.cs" Inherits="paginas_predial_Frm_Cat_Pre_Contribuyentes" Title="Catalogo de Contribuyentes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Contribuyentes" runat="server" />
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
                        <td class="label_titulo" colspan="2">Catálogo de Contribuyentes</td>
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
                        <td>Búsqueda:
                            <asp:TextBox ID="Txt_Busqueda_Contribuyente" runat="server" Width="150px"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Contribuyente" runat="server" CausesValidation="false"
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar" 
                                onclick="Btn_Buscar_Contribuyente_Click" />
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Contribuyente" runat="server" WatermarkText="<Nombre>" TargetControlID="Txt_Busqueda_Contribuyente" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>    
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Contribuyente" runat="server" TargetControlID="Txt_Busqueda_Contribuyente" InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </table>   
                <br />
                <center>
                    <table width="96%">
                        <tr>
                            <td colspan="4">
                                <asp:HiddenField ID="Hdf_Contribuyente" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:18%; text-align:left;">
                                <asp:Label ID="Lbl_ID_Contribuyente" 
                                    runat="server" Text="Contribuyente ID" CssClass="estilo_fuente"></asp:Label></td>
                            <td style="width:32%">
                                <asp:TextBox ID="Txt_ID_Contribuyente" runat="server" Width="98%" 
                                    MaxLength="10" Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left;"><asp:Label ID="Lbl_Tipo_Persona" runat="server" Text="* Tipo Persona" CssClass="estilo_fuente"></asp:Label></td>
                            <td style="width:35%">
                                <asp:DropDownList ID="Cmb_Tipo_Persona" runat="server" Width="100%" 
                                    onselectedindexchanged="Cmb_Tipo_Persona_SelectedIndexChanged" 
                                    AutoPostBack="True">
                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE"/>
                                    <asp:ListItem Text="FISICA" Value="FISICA" />
                                    <asp:ListItem Text="MORAL" Value="MORAL" />
                                </asp:DropDownList>
                            </td>
                            <td style="width:15%; text-align:left;"><asp:Label ID="Lbl_Estatus" runat="server" Text="* Estatus" CssClass="estilo_fuente"></asp:Label></td>
                            <td style="width:35%">
                                <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE"/>
                                    <asp:ListItem Text="VIGENTE" Value="VIGENTE" />
                                    <asp:ListItem Text="BAJA" Value="BAJA" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_Apellido_Paterno" runat="server" Text="* Apellido Paterno" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Apellido_Paterno" runat="server" Width="98%" 
                                    MaxLength="25" ontextchanged="Txt_Apellido_Paterno_TextChanged" AutoPostBack="True" ></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Apellido_Paterno" runat="server" WatermarkText=" "  TargetControlID="Txt_Apellido_Paterno" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            </td>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_Apellido_Materno" runat="server" Text="* Apellido Materno" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Apellido_Materno" runat="server" Width="98%" 
                                    MaxLength="25" ontextchanged="Txt_Apellido_Materno_TextChanged" AutoPostBack="True" ></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Apellido_Materno" runat="server" WatermarkText=" "  TargetControlID="Txt_Apellido_Materno" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_Nombre" runat="server" Text="* Nombre" CssClass="estilo_fuente"></asp:Label>
                                
                            </td>
                            <td colspan="3" style="width:35%">
                                <asp:TextBox ID="Txt_Nombre" runat="server" Width="98%" MaxLength="30" 
                                    ontextchanged="Txt_Nombre_TextChanged" AutoPostBack="True" ></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Nombre" runat="server" WatermarkText=" "  TargetControlID="Txt_Nombre" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            </td>
                        </tr> 
                        <tr>
                            <td style="width:15%; text-align:left;"><asp:Label ID="Lbl_Sexo" runat="server" Text="Sexo" CssClass="estilo_fuente"></asp:Label></td>
                            <td style="width:35%">
                                <asp:DropDownList ID="Cmb_Sexo" runat="server" Width="100%">
                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE"/>
                                    <asp:ListItem Text="FEMENINO" Value="FEMENINO" />
                                    <asp:ListItem Text="MASCULINO" Value="MASCULINO" />
                                </asp:DropDownList>
                            </td>
                            <td style="width:15%; text-align:left;"><asp:Label ID="Lbl_Estado_Civil" runat="server" Text="Estado Civil" CssClass="estilo_fuente"></asp:Label></td>
                            <td style="width:35%">
                                <asp:DropDownList ID="Cmb_Estado_Civil" runat="server" Width="100%">
                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE"/>
                                    <asp:ListItem Text="SOLTERO(A)" Value="SOLTERO" />
                                    <asp:ListItem Text="CASADO(A)" Value="CASADO" />
                                    <asp:ListItem Text="VIUDO(A)" Value="VIUDO" />
                                    <asp:ListItem Text="DIVORCIADO(A)" Value="DIVORCIADO" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_Fecha_Nacimiento" runat="server" Text="Fecha de Nacimiento" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%; text-align:left;">
                                <asp:TextBox ID="Txt_Fecha_Nacimiento" runat="server" Width="85%" 
                                    MaxLength="20" Enabled="false" ontextchanged="Txt_Fecha_Nacimiento_TextChanged" AutoPostBack="True" ></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Fecha_Nacimiento" runat="server" WatermarkText=" "  TargetControlID="Txt_Fecha_Nacimiento" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                                <asp:ImageButton ID="Btn_Fecha_Nacimiento" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                <cc1:CalendarExtender ID="CE_Fecha_Nacimiento" runat="server" TargetControlID="Txt_Fecha_Nacimiento" PopupButtonID="Btn_Fecha_Nacimiento" Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_RFC" runat="server" Text="* RFC" CssClass="estilo_fuente" ></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_RFC" runat="server" Width="98%" MaxLength="14"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="Txt_RFC_MaskedEditExtender" runat="server" 
                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                    CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                                    TargetControlID="Txt_RFC" Mask="CCCC999999">
                                </cc1:MaskedEditExtender>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_RFC" runat="server" WatermarkText=" " TargetControlID="Txt_RFC" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_CURP" runat="server" Text="CURP" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_CURP" runat="server" Width="98%" MaxLength="20"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="Txt_CURP_MaskedEditExtender" runat="server" 
                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                    CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                                    Mask="CCCC999999CCCCCC99" TargetControlID="Txt_CURP">
                                </cc1:MaskedEditExtender>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_CURP" runat="server" WatermarkText=" "  TargetControlID="Txt_CURP" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            </td>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_IFE" runat="server" Text="IFE" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_IFE" runat="server" Width="98%" MaxLength="20"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="Txt_IFE_MaskedEditExtender" runat="server" 
                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                    CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                                    Mask="9999999999999" TargetControlID="Txt_IFE">
                                </cc1:MaskedEditExtender>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_IFE" runat="server" WatermarkText=" "  TargetControlID="Txt_IFE" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left; vertical-align:top;">
                                <asp:Label ID="Lbl_Representante_Legal" runat="server" Text="Representante Legal" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Representante_Legal" runat="server" Rows="2" Width="98%"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Representante_Legal" runat="server" WatermarkText=" "  TargetControlID="Txt_Representante_Legal" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left; vertical-align:top;">
                                <asp:Label ID="Lbl_Domicilio" runat="server" Text="Domicilio" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Domicilio" runat="server" Width="98%" MaxLength="100"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Domicilio" runat="server" WatermarkText=" "  TargetControlID="Txt_Domicilio" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_Exterior" runat="server" Text="Exterior" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Exterior" runat="server" Width="98%" MaxLength="20"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Exterior" runat="server" WatermarkText=" "  TargetControlID="Txt_Exterior" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            </td>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_Interior" runat="server" Text="Interno" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Interior" runat="server" Width="98%" MaxLength="25"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Interior" runat="server" WatermarkText=" "  TargetControlID="Txt_Interior" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left; vertical-align:top;">
                                <asp:Label ID="Lbl_Colonia" runat="server" Text="Colonia" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Colonia" runat="server" Width="98%" MaxLength="100"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Colonia" runat="server" WatermarkText=" "  TargetControlID="Txt_Colonia" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_Ciudad" runat="server" Text="Ciudad" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Ciudad" runat="server" Width="98%" MaxLength="100"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Ciudad" runat="server" WatermarkText=" "  TargetControlID="Txt_Ciudad" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            </td>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_Codigo_Postal" runat="server" Text="Codigo Postal" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Codigo_Postal" runat="server" Width="98%" MaxLength="8"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="Txt_Codigo_Postal_FilteredTextBoxExtender" 
                                    runat="server" Enabled="True" FilterType="Numbers" 
                                    TargetControlID="Txt_Codigo_Postal">
                                </cc1:FilteredTextBoxExtender>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Codigo_Postal" runat="server" WatermarkText=" "  TargetControlID="Txt_Codigo_Postal" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%; text-align:left; ">
                                <asp:Label ID="Lbl_Estado" runat="server" Text="Estado" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:TextBox ID="Txt_Estado" runat="server" Width="98%" MaxLength="100"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Estado" runat="server" WatermarkText=" "  TargetControlID="Txt_Estado" WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>
                            </td>
                            <td style="width:15%; text-align:left;">
                                <asp:Label ID="Lbl_Tipo_Propietario" runat="server" Text="Tipo Propietario" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:35%">
                                <asp:DropDownList ID="Cmb_Tipo_Propietario" runat="server" Width="100%">
                                    <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE"/>
                                    <asp:ListItem Text="PROPIETARIO" Value="PROPIETARIO" />
                                    <asp:ListItem Text="COPROPIETARIO" Value="COPROPIETARIO" />
                                    <asp:ListItem Text="AMBOS" Value="AMBOS" />
                                </asp:DropDownList>
                            </td>
                        </tr>   
                    </table>
                    <br />
                    <asp:GridView ID="Grid_Contribuyentes" runat="server" CssClass="GridView_1"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="5" Width="96%"
                        EmptyDataText="&quot;No se encontraron registros&quot;"
                        onselectedindexchanged="Grid_Contribuyentes_SelectedIndexChanged" 
                        GridLines= "None"
                        onpageindexchanging="Grid_Contribuyentes_PageIndexChanging">
                        <RowStyle CssClass="GridItem" />
                        <Columns>
                            <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                ImageUrl="~/paginas/imagenes/gridview/blue_button.png" />
                            <asp:BoundField DataField="CONTRIBUYENTE_ID" HeaderText="Contribuyente ID" 
                                SortExpression="CONTRIBUYENTE_ID" />
                            <asp:BoundField DataField="RFC" HeaderText="RFC" 
                                SortExpression="rfc" />
                            <asp:BoundField DataField="NOMBRE_COMPLETO" HeaderText="NOMBRE COMPLETO" 
                                SortExpression="nombre" />
                        </Columns>
                        <PagerStyle CssClass="GridHeader" />
                        <SelectedRowStyle CssClass="GridSelected" />
                        <HeaderStyle CssClass="GridHeader" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                    </asp:GridView>
                </center>
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
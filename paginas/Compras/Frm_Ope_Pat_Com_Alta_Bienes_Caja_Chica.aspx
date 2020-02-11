<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pat_Com_Alta_Bienes_Caja_Chica.aspx.cs" Inherits="paginas_predial_Frm_Ope_Pat_Com_Alta_Bienes_Caja_Chica" Title="Caja Chica" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Caja_Chica" runat="server"  EnableScriptGlobalization ="true" EnableScriptLocalization = "True" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Conditional">
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
                        <td class="label_titulo" colspan="2">Caja Chica</td>
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
                            <asp:ImageButton ID="Btn_Salir" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" onclick="Btn_Salir_Click"/>
                        </td>
                        <td>&nbsp;</td>                                          
                </table>   
                <br /> 
                <table width="98%">         
                    <tr>
                        <td style="width:15%; text-align:left; ">
                            <asp:Label ID="Lbl_Nombre" runat="server" Text="Nombre" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Nombre" runat="server" Width="98%" Enabled="False"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre" runat="server" 
                                                            TargetControlID="Txt_Nombre" InvalidChars="<,>,&,',!," 
                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                            </cc1:FilteredTextBoxExtender>   
                        </td>
                    </tr> 
                    <tr>
                        <td style="width:15%; text-align:left; ">
                            <asp:Label ID="Lbl_Dependencia" runat="server" Text="Dependencia"  CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td colspan="3" style="width:35%">
                            <asp:DropDownList ID="Cmb_Dependencias" runat="server" Width="99%" onselectedindexchanged="Cmb_Dependencias_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%; text-align:left; ">
                            <asp:Label ID="Lbl_Numero_Inventario" runat="server" Text="No. Inventario" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width:35%">
                            <asp:TextBox ID="Txt_Numero_Inventario" runat="server" Width="98%" Enabled="False"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Inventario" runat="server" 
                                                            TargetControlID="Txt_Numero_Inventario" InvalidChars="<,>,&,',!," 
                                                            FilterType="Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                            </cc1:FilteredTextBoxExtender>                                           
                        </td>
                        <td style="width:15%; text-align:left;">
                            <asp:Label ID="Lbl_Cantidad" runat="server" Text="Cantidad" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width:35%">
                            <asp:TextBox ID="Txt_Cantidad" runat="server" Width="98%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cantidad" runat="server" TargetControlID="Txt_Cantidad" FilterType="Numbers" Enabled="True"></cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%; text-align:left; ">
                            <asp:Label ID="Lbl_Material" runat="server" Text="Material" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width:35%">
                            <asp:DropDownList ID="Cmb_Material" runat="server" Width="98%" Enabled="False">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:15%; text-align:left; ">
                            <asp:Label ID="Lbl_Color" runat="server" Text="Color" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width:35%">
                            <asp:DropDownList ID="Cmb_Color" runat="server" Width="98%" Enabled="False">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>   
                    <tr>
                        <td style="width:15%; text-align:left; ">
                            <asp:Label ID="Lbl_Marca" runat="server" Text="Marca" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width:35%">
                            <asp:DropDownList ID="Cmb_Marca" runat="server" Width="98%" Enabled="False">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:15%; text-align:left; ">
                            <asp:Label ID="Lbl_Modelo" runat="server" Text="Modelo" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width:35%">
                            <asp:DropDownList ID="Cmb_Modelo" runat="server" Width="98%" Enabled="False">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>                        
                    <tr>
                        <td style="width:15%; text-align:left; ">
                            <asp:Label ID="Lbl_Costo" runat="server" Text="Costo" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width:35%">
                            <asp:TextBox ID="Txt_Costo" runat="server" Width="98%" Enabled="False"></asp:TextBox>
                            <cc1:MaskedEditExtender ID="MEE_Txt_Costo" runat="server" 
                                TargetControlID="Txt_Costo" Mask="9,999,999.99" MaskType="Number" 
                                InputDirection="RightToLeft" AcceptNegative="Left" ErrorTooltipEnabled="True" 
                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                        </td>
                        <td style="width:15%; text-align:left; ">
                            <asp:Label ID="Lbl_Fecha_Adquisicion" runat="server" Text="Fecha Adquisición" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width:35%">
                            <asp:TextBox ID="Txt_Fecha_Adquisicion" runat="server" Width="85%" MaxLength="20" Enabled="false"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Fecha_Adquisicion" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Adquisicion" runat="server" TargetControlID="Txt_Fecha_Adquisicion" PopupButtonID="Btn_Fecha_Adquisicion" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>                           
                    <tr>
                        <td style="width:15%; text-align:left; ">
                            <asp:Label ID="Lbl_Estado" runat="server" Text="Estado" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width:35%">
                            <asp:DropDownList ID="Cmb_Estado" runat="server" Width="98%">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                <asp:ListItem Value="BUENO">BUENO</asp:ListItem>
                                <asp:ListItem Value="REGULAR">REGULAR</asp:ListItem>
                                <asp:ListItem Value="MALO">MALO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:15%; text-align:left; ">
                            <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width:35%">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" Enabled="False">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>                                      
                    <tr>
                        <td style="text-align:left; vertical-align:top;">
                            <asp:Label ID="Lbl_Comentarios_Generales" runat="server" 
                                Text="Comentarios Generales"  CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Comentarios_Generales" runat="server" Width="99%" Enabled="False" 
                                Rows="2" TextMode="MultiLine"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Observaciones" runat="server" 
                                                            TargetControlID="Txt_Comentarios_Generales" InvalidChars="<,>,&,',!," 
                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " 
                                Enabled="True">   
                            </cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observaciones" runat="server" TargetControlID ="Txt_Comentarios_Generales" 
                                WatermarkText="Límite de Caractes 500" WatermarkCssClass="watermarked" 
                                Enabled="True"/>     
                        </td>
                    </tr>    
                    <tr>
                        <td style="text-align:left; vertical-align:top; width:15%">
                            <asp:Label ID="Lbl_Archivo" runat="server" Text="Archivo"  CssClass="estilo_fuente"></asp:Label>
                            <asp:Label ID="Throbber" Text="wait" runat="server"  Width="30px">                                                                     
                               <div id="Div1" class="progressBackgroundFilter"></div>
                                   <div  class="processMessage" id="div2">
                                        <img alt="" src="../imagenes/paginas/Updating.gif" />
                                    </div>
                            </asp:Label>  
                        </td>
                        <td colspan="3">                      
                            <cc1:AsyncFileUpload ID="AFU_Archivo" runat="server"  Width="600px" ThrobberID="Throbber" ForeColor="White" Font-Bold="true" CompleteBackColor="LightBlue" 
                                ErrorBackColor="Red" UploadingBackColor="LightGray" />
                        </td>
                    </tr>                
                </table>
                <br />
                <table width="98%">
                    <tr>
                        <td class="label_titulo" colspan="3">Resguardos</td>
                    </tr>         
                    <tr align="right" class="barra_delgada">
                        <td colspan="3" align="center">
                        </td>
                    </tr>                            
                    <tr>
                        <td style="width:15%; text-align:left;">
                            <asp:Label ID="Lbl_Resguardantes" runat="server" Text="Empleados" 
                                CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td  colspan="2" style="width:85%; text-align:left;">
                            <asp:DropDownList ID="Cmb_Empleados" runat="server" Width="98%">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%; text-align:left; vertical-align:top;">
                            <asp:Label ID="Lbl_Comentarios" runat="server" Text="Comentarios" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td  colspan="2" style="width:85%; text-align:left;">
                            <asp:TextBox ID="Txt_Comentarios" runat="server" TextMode="MultiLine" Rows="3" 
                                Width="98%"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Cometarios" runat="server" TargetControlID ="Txt_Comentarios" 
                                WatermarkText="Límite de Caractes 150" WatermarkCssClass="watermarked" 
                                Enabled="True"/>                                                        
                        </td>
                    </tr>                                    
                    <tr>
                        <td colspan="2">&nbsp;</td>
                        <td style="width:15%; text-align:right;">
                            <asp:ImageButton ID="Btn_Agregar_Resguardante" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                AlternateText="Agregar" onclick="Btn_Agregar_Resguardante_Click"/>
                            <asp:ImageButton ID="Btn_Quitar_Resguardante" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/quitar.png" 
                                AlternateText="Quitar" onclick="Btn_Quitar_Resguardante_Click" />
                        </td>  
                    </tr>
                </table>
                <br />
                <asp:GridView ID="Grid_Resguardantes" runat="server" 
                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                    GridLines="None" AllowPaging="True" Width="98%"
                    OnPageIndexChanging="Grid_Resguardantes_PageIndexChanging" 
                    PageSize="5" >
                    <RowStyle CssClass="GridItem" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" CommandName="Select" 
                            ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                            <ItemStyle Width="30px" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="BIEN_RESGUARDO_ID" HeaderText="BIEN_RESGUARDO_ID" 
                            SortExpression="BIEN_RESGUARDO_ID">
                            <ItemStyle Width="110px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="EMPLEADO_ID" 
                            HeaderText="Empleado ID" SortExpression="EMPLEADO_ID">
                            <ItemStyle Width="110px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NOMBRE_EMPLEADO" HeaderText="Nombre" 
                            SortExpression="NOMBRE_EMPLEADO" />
                    </Columns>
                    <PagerStyle CssClass="GridHeader" />
                    <SelectedRowStyle CssClass="GridSelected" />
                    <HeaderStyle CssClass="GridHeader" />                                
                    <AlternatingRowStyle CssClass="GridAltItem" />                                
                </asp:GridView>
            </div>
            <br />          
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pat_Com_Actualizacion_Vehiculos.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Pat_Com_Actualizacion_Vehiculos" Title="Operación - Actualización de Vehículos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
 <script type="text/javascript" language="javascript">
        //Metodos para limpiar los controles de la busqueda.
        function Limpiar_Ctlr(){
            document.getElementById("<%=Txt_Busqueda_No_Empleado.ClientID%>").value="";
            document.getElementById("<%=Txt_Busqueda_RFC.ClientID%>").value="";  
            document.getElementById("<%=Cmb_Busqueda_Dependencia.ClientID%>").value="";  
            document.getElementById("<%=Txt_Busqueda_Nombre_Empleado.ClientID%>").value="";                            
            return false;
        }  
</script>  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    
    <asp:ScriptManager ID="ScptM_Vehiculos" runat="server"  EnableScriptGlobalization ="true" EnableScriptLocalization = "True" />

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
                        <td class="label_titulo" colspan="4">Actualización de Vehículos</td>
                    </tr>
                    <tr>
                        <td colspan="4">
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
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Modificar" onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Generar_Reporte" runat="server" ToolTip="Ver Resguardo"
                                ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Reporte" onclick="Btn_Generar_Reporte_Click" />    
                            <asp:ImageButton ID="Btn_Generar_Reporte_Completo" runat="server" ToolTip="Ver Reporte [Completo]"
                                ImageUrl="~/paginas/imagenes/paginas/report.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Reporte" onclick="Btn_Generar_Reporte_Completo_Click" />   
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Salir"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" onclick="Btn_Salir_Click" /> 
                        </td>
                        <td align="right" style="width:50%;">
                            <div id="Div_Busqueda" runat="server">
                                <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                                    onclick="Btn_Avanzada_Click" ToolTip="Avanzada">Busqueda</asp:LinkButton>
                                    &nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" 
                                    WatermarkCssClass="watermarked"
                                    WatermarkText="<Número de Inventario>"
                                    TargetControlID="Txt_Busqueda" />
                                <asp:ImageButton ID="Btn_Buscar" runat="server" OnClick="Btn_Buscar_Click " 
                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png" AlternateText="Consultar"/>
                            </div>
                        </td>
                </table>   
                <br />           
               <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="98%" ActiveTabIndex="0" CssClass="Tab" Style="visibility:visible;" >
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Bienes_Detalles"  ID="Tab_Bienes_Detalles"  Width="100%">
                        <HeaderTemplate>Generales</HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" class="estilo_fuente">
                                <tr>
                                    <td colspan="4">
                                        <asp:HiddenField ID="Hdf_Vehiculo_ID" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%; text-align:left;">
                                        <asp:Label ID="Lbl_Numero_Inventario" runat="server" Text="No. Inventario" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Numero_Inventario" runat="server" Width="97%" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td style="width:15%; text-align:left;">
                                        <asp:Label ID="Lbl_Nombre" runat="server" Text="Nombre Producto" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Nombre" runat="server" Width="97%" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>                        
                                <tr>
                                    <td width="15%">
                                        <asp:Label ID="Lbl_Clase_Activo" runat="server" Text="Clase Activo"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="Cmb_Clase_Activo" runat="server" Width="100%">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%">
                                        <asp:Label ID="Lbl_Tipo_Activo" runat="server" Text="Tipo Activo"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="Cmb_Tipo_Activo" runat="server" Width="100%">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>             
                                <tr>
                                    <td style="width:15%; text-align:left;">
                                        <asp:Label ID="Lbl_Dependencia" runat="server" Text="Unidad Responsable" ></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="Cmb_Dependencias" runat="server" Width="100%" OnSelectedIndexChanged="Cmb_Dependencias_SelectedIndexChanged" AutoPostBack="True">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr> 
                                <tr>
                                    <td style="width:15%; text-align:left; ">
                                        <asp:Label ID="Lbl_Marca" runat="server" Text="Marca" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:DropDownList ID="Cmb_Marca" runat="server" Width="100%">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:15%; text-align:left; ">
                                        <asp:Label ID="Lbl_Tipos_Vehiculos" runat="server" Text="Tipo" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:DropDownList ID="Cmb_Tipos_Vehiculos" runat="server" Width="100%">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%; text-align:left; ">
                                        <asp:Label ID="Lbl_Modelo" runat="server" Text="Modelo"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Modelo" runat="server" Width="99%" Enabled="False"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Modelo" runat="server" TargetControlID="Txt_Modelo"
                                            InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_" Enabled="True">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%; text-align:left;">
                                        <asp:Label ID="Lbl_Color" runat="server" Text="Color"></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:DropDownList ID="Cmb_Colores" runat="server" Width="100%">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:15%; text-align:left;">
                                        <asp:Label ID="Lbl_Tipo_Combustible" runat="server" Text=" Combustible"></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:DropDownList ID="Cmb_Tipo_Combustible" runat="server" Width="100%">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%; text-align:left;">
                                        <asp:Label ID="Lbl_Placas" runat="server" Text="Placas"></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Placas" runat="server" Width="97%" MaxLength="15"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Clave_Inventario" runat="server" TargetControlID="Txt_Placas" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_" Enabled="True"></cc1:FilteredTextBoxExtender>
                                    </td> 
                                    <td style="width:15%; text-align:left;">
                                        <asp:Label ID="Lbl_Numero_Economico" runat="server" Text="No. Económico"></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Numero_Economico" runat="server" Width="98%"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Economico" runat="server" TargetControlID="Txt_Numero_Economico"
                                            InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_/" Enabled="True">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; text-align: left;">
                                        <asp:Label ID="Lbl_Proveedor" runat="server" Text="Proveedor"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="Cmb_Proveedor" runat="server" Width="100%" Enabled="False">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%; text-align:left;">
                                        <asp:Label ID="Lbl_Zonas" runat="server" Text="Zona"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="Cmb_Zonas" runat="server" Width="100%">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr> 
                                <tr>
                                    <td style="width: 15%; text-align: left;">
                                        <asp:Label ID="Lbl_No_Factura" runat="server" Text="No Factura"></asp:Label>
                                    </td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="Txt_No_Factura" runat="server" Width="98%" MaxLength="20"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Factura" runat="server" TargetControlID="Txt_No_Factura"
                                            InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_" Enabled="True">
                                        </cc1:FilteredTextBoxExtender>   
                                    </td>
                                    <td style="width:15%; text-align:left;">
                                        <asp:Label ID="Lbl_Serie_Carroceria" runat="server" Text="Serie" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Serie_Carroceria" runat="server" Width="98%" ></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Serie" runat="server" TargetControlID="Txt_Serie_Carroceria"
                                            InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_/" Enabled="True">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%; text-align:left;">
                                        <asp:Label ID="Lbl_Costo_Inicial" runat="server" Text="Costo Inicial [$]" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Costo_Inicial" runat="server" Width="98%" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td style="width:15%; text-align:left;">
                                        <asp:Label ID="Lbl_Costo_Actual" runat="server" Text="Costo Actual [$]" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Costo_Actual" runat="server" Width="98%" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%; text-align:left;">
                                        <asp:Label ID="Lbl_Capacidad_Carga" runat="server" Text="Capacidad" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Capacidad_Carga" runat="server" Width="98%"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Capacidad_Carga" runat="server" TargetControlID="Txt_Capacidad_Carga" InvalidChars="<,>,&,',!," 
                                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True"></cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%; text-align:left; ">
                                        <asp:Label ID="Lbl_Anio_Fabricacion" runat="server" Text="Año Fabricación" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Anio_Fabricacion" runat="server" Width="98%" MaxLength="4"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Anio_Fabricacion" runat="server" TargetControlID="Txt_Anio_Fabricacion" FilterType="Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ "  Enabled="True"></cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td style="width:15%; text-align:left;">
                                        <asp:Label ID="Lbl_Numero_Cilindros" runat="server" Text="No. Cilindros"></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Numero_Cilindros" runat="server" Width="98%" ></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Cilindros" runat="server" TargetControlID="Txt_Numero_Cilindros" FilterType="Numbers" Enabled="True"></cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%; text-align:left;">
                                        <asp:Label ID="Lbl_Fecha_Adquisicion" runat="server" Text="Fecha Adquisición"></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Fecha_Adquisicion" runat="server" Width="85%" MaxLength="20"
                                            Enabled="False"></asp:TextBox>
                                        <asp:ImageButton ID="Btn_Fecha_Adquisicion" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                        <cc1:CalendarExtender ID="CE_Txt_Fecha_Adquisicion" runat="server" TargetControlID="Txt_Fecha_Adquisicion"
                                            PopupButtonID="Btn_Fecha_Adquisicion" Format="dd/MMM/yyyy" Enabled="True">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td style="width:15%; text-align:left;">
                                        <asp:Label ID="Lbl_Kilometraje" runat="server" Text="Kilometraje"></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Kilometraje" runat="server" Width="98%"></asp:TextBox>
                                        <cc1:MaskedEditExtender ID="MEE_Txt_Kilometraje" runat="server" TargetControlID="Txt_Kilometraje" Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left" ErrorTooltipEnabled="True" 
                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""  CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder=""  CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%; text-align:left;">
                                        <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus"></asp:Label>
                                    </td>
                                    <td style="width:35%;">
                                        <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                            <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                            <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:15%; text-align:left;">
                                        <asp:Label ID="Lbl_Estado" runat="server" Text="Odometro"></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:DropDownList ID="Cmb_Odometro" runat="server" Width="100%">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            <asp:ListItem Value="FUNCIONA">FUNCIONA</asp:ListItem>
                                            <asp:ListItem Value="NO_FUNCIONA">NO FUNCIONA</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left; vertical-align:top; width:15%;">
                                        <asp:Label ID="Lbl_Motivo_Baja" runat="server" Text="Motivo Baja"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Motivo_Baja" runat="server" Width="99%" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Motivo_Baja" runat="server" TargetControlID="Txt_Motivo_Baja" InvalidChars="<,>,&,',!," 
                                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True"></cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:left; vertical-align:top; width:15%;">
                                        <asp:Label ID="Lbl_Observaciones" runat="server" Text="Observaciones"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Observaciones" runat="server" Width="99%" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Observaciones" runat="server" TargetControlID="Txt_Observaciones" InvalidChars="<,>,&,',!," 
                                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True"></cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%" class="estilo_fuente">            
                                <tr>
                                    <td>
                                        <div runat="server" id="Div3">
                                            <table width="100%" class="estilo_fuente">
                                                <tr>
                                                    <td class="label_titulo" colspan="4">Resguardos</td>
                                                </tr>  
                                                <tr align="right" class="barra_delgada">
                                                    <td colspan="4" align="center">
                                                    </td>
                                                </tr>                                   
                                               <tr>
                                                    <td style="width:15%; text-align:left;">
                                                        <asp:Label ID="Lbl_Resguardantes" runat="server" Text="Empleados" ></asp:Label>
                                                    </td>
                                                    <td  style="width:85%; text-align:left;">
                                                        <asp:DropDownList ID="Cmb_Empleados" runat="server" Width="100%">
                                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:15%; text-align:left; vertical-align:top;">
                                                        <asp:Label ID="Lbl_Comentarios" runat="server" Text="Comentarios" ></asp:Label>
                                                    </td>
                                                    <td  style="width:85%; text-align:left;">
                                                        <asp:TextBox ID="Txt_Cometarios" runat="server" TextMode="MultiLine" Rows="3" Width="99%"></asp:TextBox>
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Cometarios" runat="server" TargetControlID ="Txt_Cometarios" 
                                                            WatermarkText="Límite de Caractes 150" WatermarkCssClass="watermarked" Enabled="True"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"><hr /></td>
                                                </tr>
                                                <tr>
                                                    <td style="width:15%; text-align:left; vertical-align:top;">
                                                        &nbsp;&nbsp;&nbsp;
                                                        <asp:ImageButton ID="Btn_Busqueda_Avanzada_Resguardante" runat="server" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Width="24px" ToolTip="Buscar" AlternateText="Buscar" OnClick="Btn_Busqueda_Avanzada_Resguardante_Click"/>
                                                    </td>
                                                    <td  style="width:85%; text-align:right;">
                                                        <asp:ImageButton ID="Btn_Agregar_Resguardante" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_add.png" ToolTip="Agregar" AlternateText="Agregar" OnClick="Btn_Agregar_Resguardante_Click"/>
                                                        <asp:ImageButton ID="Btn_Quitar_Resguardante" runat="server" ImageUrl="~/paginas/imagenes/paginas/quitar.png" ToolTip="Quitar" AlternateText="Quitar" OnClick="Btn_Quitar_Resguardante_Click"/>
                                                        &nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"><hr /></td>
                                                </tr>
                                            </table>
                                            <br />
                                            <asp:GridView ID="Grid_Resguardantes" runat="server" 
                                                AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                                GridLines="None" Width="98%" 
                                                onrowdatabound="Grid_Resguardantes_RowDataBound" >
                                                <RowStyle CssClass="GridItem" />
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                                        <ItemStyle Width="30px" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="BIEN_RESGUARDO_ID" HeaderText="BIEN_RESGUARDO_ID" SortExpression="BIEN_RESGUARDO_ID">
                                                        <ItemStyle Width="110px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EMPLEADO_ID" HeaderText="Empleado ID" SortExpression="EMPLEADO_ID">
                                                        <ItemStyle Width="90px" HorizontalAlign="Center" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NO_EMPLEADO" HeaderText="No. Empleado" SortExpression="NO_EMPLEADO">
                                                        <ItemStyle Width="90px" HorizontalAlign="Center" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="NOMBRE_EMPLEADO" HeaderText="Nombre  del Empleado" SortExpression="NOMBRE_EMPLEADO" >
                                                        <ItemStyle HorizontalAlign="Center" Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="Btn_Ver_Informacion_Resguardo" runat="server" Width="16px" ImageUrl="~/paginas/imagenes/gridview/grid_info.png" ToolTip="Comentarios del Resguardo" OnClick="Btn_Ver_Informacion_Resguardo_Click" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="50px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle CssClass="GridHeader" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Detalles_Vehiculo"  ID="Tab_Detalles_Vehiculo"  Width="100%">
                        <HeaderTemplate>Detalles de Vehículo</HeaderTemplate>
                        <ContentTemplate>
                            <div id="Div_Detalles_Vehiculos" runat="server" style="width:99%; height:500px; overflow:auto;">
                                <table class="estilo_fuente" width="99%">
                                    <tr>
                                        <td style="width:18%;">
                                            <asp:HiddenField ID="Hdf_Resguardo_Completo_Operador" runat="server" />
                                            <asp:Label ID="Lbl_Resguardo_Completo_Operador" runat="server" Text="Operador"></asp:Label>
                                        </td>
                                        <td style="width:82%;">
                                            <asp:TextBox ID="Txt_Resguardo_Completo_Operador" runat="server" Width="95%" Enabled="false"></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Resguardo_Completo_Operador" runat="server" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Width="16px" ToolTip="Buscar" AlternateText="Buscar" OnClick="Btn_Resguardo_Completo_Operador_Click"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:18%;">
                                            <asp:HiddenField ID="Hdf_Resguardo_Completo_Funcionario_Recibe" runat="server" />
                                            <asp:Label ID="Lbl_Resguardo_Completo_Funcionario_Recibe" runat="server" Text="Funcionario que Recibe"></asp:Label>
                                        </td>
                                        <td style="width:82%;">
                                            <asp:TextBox ID="Txt_Resguardo_Completo_Funcionario_Recibe" runat="server" Width="95%" Enabled="false"></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Resguardo_Completo_Funcionario_Recibe" runat="server" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Width="16px" ToolTip="Buscar" AlternateText="Buscar" OnClick="Btn_Resguardo_Completo_Funcionario_Recibe_Click"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:18%;">
                                            <asp:HiddenField ID="Hdf_Resguardo_Completo_Autorizo" runat="server" />
                                            <asp:Label ID="Lbl_Resguardo_Completo_Autorizo" runat="server" Text="Autorizó"></asp:Label>
                                        </td>
                                        <td style="width:82%;">
                                            <asp:TextBox ID="Txt_Resguardo_Completo_Autorizo" runat="server" Width="95%" Enabled="false"></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Resguardo_Completo_Autorizo" runat="server" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Width="16px" ToolTip="Buscar" AlternateText="Buscar" OnClick="Btn_Resguardo_Completo_Autorizo_Click"/>
                                        </td>
                                    </tr>
                                </table>
                                <asp:HiddenField ID="Hdf_Tipo_Busqueda" runat="server" />
                                <hr style="width:99%; text-align:center;" />
                                <asp:GridView ID="Grid_Detalles_Vehiculo" runat="server" AutoGenerateColumns="False" 
                                    CellPadding="4" ForeColor="#333333" 
                                    GridLines="None"  Width="99%" PageSize="5" >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:BoundField DataField="DETALLE_ID" HeaderText="DETALLE_ID" SortExpression="DETALLE_ID">
                                            <ItemStyle Width="110px" Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Parte de Vehículo" SortExpression="NOMBRE">
                                            <ItemStyle Font-Size="X-Small" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Estado">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="Cmb_Estado_Detalle" runat="server" Width="99%">
                                                    <asp:ListItem Value="N">NO TIENE (N)</asp:ListItem>
                                                    <asp:ListItem Value="B">BUENO (B)</asp:ListItem>
                                                    <asp:ListItem Value="R">REGULAR (R)</asp:ListItem>
                                                    <asp:ListItem Value="M">MALO (M)</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle Width="110px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                </asp:GridView> 
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Datos_Seguro"  ID="Tab_Datos_Seguro"  Width="100%">
                        <HeaderTemplate>Seguro</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table width="100%" class="estilo_fuente">  
                                    <tr>
                                        <td colspan="4">
                                            <asp:HiddenField ID="Hdf_Vehiculo_Aseduradora_ID" runat="server" />
                                        </td>
                                    </tr>                             
                                    <tr>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Aseguradoras" runat="server" Text="Aseguradora"></asp:Label>
                                        </td>
                                        <td style="width:85%; text-align:left; ">
                                            <asp:DropDownList ID="Cmb_Aseguradoras" runat="server" Width="100%">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>                                                       
                                    <tr>
                                        <td style="width:15%; text-align:left; ">
                                            <asp:Label ID="Lbl_Numero_Poliza_Seguro" runat="server" Text="Número Poliza"></asp:Label>
                                        </td>
                                        <td style="width:85%; text-align:left; ">
                                            <asp:TextBox ID="Txt_Numero_Poliza_Seguro" runat="server" Width="98%" MaxLength="30"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Poliza_Seguro" runat="server" TargetControlID="Txt_Numero_Poliza_Seguro" InvalidChars="<,>,&,',!," FilterType="Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                                            </cc1:FilteredTextBoxExtender>       
                                        </td>
                                    </tr>                 
                                    <tr>
                                        <td style="width:15%; text-align:left; vertical-align:top; ">
                                            <asp:Label ID="Lbl_Numero_Inciso" runat="server" Text="No. Inciso"></asp:Label>
                                        </td>
                                        <td style="width:85%; text-align:left; ">
                                            <asp:TextBox ID="Txt_Numero_Inciso" runat="server" Width="98%" MaxLength="150"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Inciso" runat="server" 
                                                                            TargetControlID="Txt_Numero_Inciso" InvalidChars="<,>,&,',!," 
                                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td style="width:15%; text-align:left; vertical-align:top; ">
                                            <asp:Label ID="Lbl_Cobertura_Seguro" runat="server" Text="Cobertura"></asp:Label>
                                        </td>
                                        <td style="width:85%; text-align:left; ">
                                            <asp:TextBox ID="Txt_Cobertura_Seguro" runat="server" Width="98%"  Rows="2" TextMode="MultiLine"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cobertura_Seguro" runat="server" 
                                                                            TargetControlID="Txt_Cobertura_Seguro" InvalidChars="<,>,&,',!," 
                                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                                            </cc1:FilteredTextBoxExtender>
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Cobertura_Seguro" runat="server" WatermarkText="Longitud Maxima de 150 Caracteres" TargetControlID="Txt_Cobertura_Seguro" 
                                                                            WatermarkCssClass="watermarked"></cc1:TextBoxWatermarkExtender>                                                
                                        </td>
                                    </tr>    
                                </table>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Historial_Resguardos"  ID="Tab_Historial_Resguardos"  Width="100%" >
                        <HeaderTemplate>Historial</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table width="100%" class="estilo_fuente"> 
                                    <tr>
                                        <td class="label_titulo" colspan="4">Modificaciones</td>
                                    </tr>   
                                    <tr align="right" class="barra_delgada">
                                        <td colspan="4" align="center">
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td style="text-align:left; vertical-align:top; width:15%">
                                            <asp:Label ID="Lbl_Usuario_Creo" runat="server" Text="Creación"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Usuario_creo" runat="server" Width="99%" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>     
                                    <tr>
                                        <td style="text-align:left; vertical-align:top; width:15%">
                                            <asp:Label ID="Lbl_Usuario_Modifico" runat="server" Text="Ultima Modificación"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Usuario_Modifico" runat="server" Width="99%" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>     
                                    <tr>
                                        <td colspan="4">&nbsp;</td>                        
                                    </tr>   
                                    <tr>
                                        <td class="label_titulo" colspan="4">Historial de Resguardos</td>
                                    </tr>  
                                    <tr align="right" class="barra_delgada">
                                        <td colspan="4" align="center">
                                        </td>
                                    </tr>                                       
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Historial_Empleado_Resguardo" runat="server" Text="Empleado"></asp:Label>
                                        </td>
                                        <td  colspan="3" style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Historial_Empleado_Resguardo" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>                                  
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Historial_Fecha_Inicial_Resguardo" runat="server" Text="Fecha Inicial"></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Historial_Fecha_Inicial_Resguardo" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Historial_Fecha_Final_Resguardo" runat="server" Text="Fecha Final"></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Historial_Fecha_Final_Resguardo" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                   <tr>
                                        <td style="width:15%; text-align:left; vertical-align:top;">
                                            <asp:Label ID="Lbl_Historial_Comentarios_Resguardo" runat="server" Text="Comentarios"></asp:Label>
                                        </td>
                                        <td  colspan="3" style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Historial_Comentarios_Resguardo" runat="server" TextMode="MultiLine" Rows="3" Width="98%" Enabled="false"></asp:TextBox>                                                     
                                        </td>
                                    </tr>             
                                </table>
                                <br />
                                <asp:GridView ID="Grid_Historial_Resguardantes" runat="server" 
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                    GridLines="None" AllowPaging="True" Width="98%"
                                    OnPageIndexChanging="Grid_Historial_Resguardantes_PageIndexChanging"
                                    OnSelectedIndexChanged="Grid_Historial_Resguardantes_SelectedIndexChanged"
                                    PageSize="5" >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                            <ItemStyle Width="20px" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="BIEN_RESGUARDO_ID" HeaderText="BIEN_RESGUARDO_ID" SortExpression="BIEN_RESGUARDO_ID"/>
                                        <asp:BoundField DataField="EMPLEADO_ID" HeaderText="Empleado ID" SortExpression="EMPLEADO_ID" />
                                        <asp:BoundField DataField="NO_EMPLEADO" HeaderText="Empleado ID" SortExpression="NO_EMPLEADO">
                                            <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE_EMPLEADO" HeaderText="Nombre" SortExpression="NOMBRE_EMPLEADO">
                                            <ItemStyle Font-Size="X-Small" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />                                
                                    <AlternatingRowStyle CssClass="GridAltItem" />                                
                                </asp:GridView>
                            </center>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="Tab_Archivos" runat="server" HeaderText="Tab_Archivos">
                        <HeaderTemplate>Archivos</HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" class="estilo_fuente">              
                                <tr>
                                    <td style="text-align:left; vertical-align:top; width:15%">
                                        <asp:Label ID="Lbl_Archivo" runat="server" Text="Archivo" ></asp:Label>
                                        <asp:Label ID="Throbber" Text="wait" runat="server"  Width="30px">                                                                     
                                            <div id="Div1" class="progressBackgroundFilter"></div>
                                            <div  class="processMessage" id="div2">
                                                <img alt="" src="../imagenes/paginas/Updating.gif" />
                                            </div>
                                        </asp:Label>  
                                    </td>
                                    <td colspan="3">                      
                                        <cc1:AsyncFileUpload ID="AFU_Archivo" runat="server"  Width="600px" 
                                            ThrobberID="Throbber" ForeColor="White" Font-Bold="True" 
                                            CompleteBackColor="LightBlue" UploadingBackColor="LightGray" 
                                            FailedValidation="False" />
                                    </td>
                                </tr>   
                            </table>
                            <br />
                            <asp:GridView ID="Grid_Archivos" runat="server" AutoGenerateColumns="False" 
                                CellPadding="4" ForeColor="#333333" 
                                GridLines="None" AllowPaging="True" Width="98%" PageSize="5" 
                                OnPageIndexChanging="Grid_Archivos_PageIndexChanging" onrowdatabound="Grid_Archivos_RowDataBound"
                                >
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:BoundField DataField="ARCHIVO_BIEN_ID" HeaderText="ARCHIVO_BIEN_ID" SortExpression="ARCHIVO_BIEN_ID">
                                        <ItemStyle Width="110px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ARCHIVO" HeaderText="ARCHIVO" SortExpression="ARCHIVO" />
                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha" SortExpression="FECHA" DataFormatString="{0:dd/MMM/yyyy}">
                                        <ItemStyle Width="110px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" SortExpression="DESCRIPCION" 
                                        NullDisplayText="ARCHIVO NORMAL" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="Btn_Ver_Archivo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" 
                                                Width="24px" CssClass="Img_Button" AlternateText="Ver Archivo" 
                                                OnClick="Btn_Ver_Archivo_Click"/>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView> 
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="Tab_Listado_Partes" runat="server" HeaderText="Tab_Listado_Partes">
                        <HeaderTemplate>Listado de Partes</HeaderTemplate>
                        <ContentTemplate>
                            <div id="Div_Partes_Vehiculos_Campos" runat="server">
                             <table width="98%">
                                  <tr>
                                    <td colspan="4">
                                        <asp:HiddenField ID="Hdf_Parte_ID" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; text-align: left;">
                                        <asp:Label ID="Lbl_Numero_Inventario_Parte" runat="server" Text="No. Inventario"
                                            CssClass="estilo_fuente"></asp:Label>
                                    </td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="Txt_Numero_Inventario_Parte" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Inventario_Parte" runat="server"
                                            TargetControlID="Txt_Numero_Inventario_Parte" InvalidChars="<,>,&,',!," FilterType="Numbers"
                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td style="width: 15%; text-align: left;">
                                        <asp:Label ID="Lbl_Cantidad_Parte" runat="server" Text="Cantidad" CssClass="estilo_fuente"></asp:Label>
                                    </td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="Txt_Cantidad_Parte" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="Txt_Cantidad_Parte"
                                            FilterType="Numbers" Enabled="True">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; text-align: left;">
                                        <asp:Label ID="Lbl_Material_Parte" runat="server" Text="Material" CssClass="estilo_fuente"></asp:Label>
                                    </td>
                                    <td style="width: 35%">
                                        <asp:DropDownList runat="server" ID="Cmb_Material_Parte" Width="99%" Enabled="false">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 15%; text-align: left;">
                                        <asp:Label ID="Lbl_Color_Parte" runat="server" Text="Color" CssClass="estilo_fuente"></asp:Label>
                                    </td>
                                    <td style="width: 35%">
                                        <asp:DropDownList runat="server" ID="Cmb_Color_Parte" Width="98%" Enabled="false">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; text-align: left;">
                                        <asp:Label ID="Lbl_Costo_Parte" runat="server" Text="Costo" CssClass="estilo_fuente"></asp:Label>
                                    </td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="Txt_Costo_Parte" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                        <cc1:MaskedEditExtender ID="MEE_Txt_Costo_Parte" runat="server" TargetControlID="Txt_Costo_Parte"
                                            Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft" AcceptNegative="Left"
                                            ErrorTooltipEnabled="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="" Enabled="True" />
                                    </td>
                                    <td style="width: 15%; text-align: left;">
                                        <asp:Label ID="Lbl_Fecha_Adquisicion_Parte" runat="server" Text="Adquisición" CssClass="estilo_fuente"></asp:Label>
                                    </td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="Txt_Fecha_Adquisicion_Parte" runat="server" Width="98%" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; text-align: left;">
                                        <asp:Label ID="Lbl_Estado_Parte" runat="server" Text="Estado" CssClass="estilo_fuente"></asp:Label>
                                    </td>
                                    <td style="width: 35%">
                                        <asp:DropDownList ID="Cmb_Estado_Parte" runat="server" Width="98%" Enabled="false">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            <asp:ListItem Value="BUENO">BUENO</asp:ListItem>
                                            <asp:ListItem Value="REGULAR">REGULAR</asp:ListItem>
                                            <asp:ListItem Value="MALO">MALO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 15%; text-align: left;">
                                        <asp:Label ID="Lbl_Estatus_Parte" runat="server" Text="Estatus" CssClass="estilo_fuente"></asp:Label>
                                    </td>
                                    <td style="width: 35%">
                                        <asp:DropDownList ID="Cmb_Estatus_Parte" runat="server" Width="98%" Enabled="False">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            <asp:ListItem Value="VIGENTE" Selected="True">VIGENTE</asp:ListItem>
                                            <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                            <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top;">
                                        <asp:Label ID="Lbl_Comentarios_Parte" runat="server" Text="Comentarios" CssClass="estilo_fuente"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Comentarios_Parte" runat="server" Width="99%" Rows="2" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Comentarios_Parte" runat="server" TargetControlID="Txt_Comentarios_Parte"
                                            InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Parte" runat="server" TargetControlID="Txt_Comentarios_Parte"
                                            WatermarkText="Límite de Caractes 500" WatermarkCssClass="watermarked" Enabled="True" />
                                    </td>
                                </tr>             
                            </table>
                            </div>
                            <asp:GridView ID="Grid_Partes" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                ForeColor="#333333" GridLines="None" AllowPaging="True" Width="98%" 
                                OnPageIndexChanging="Grid_Partes_PageIndexChanging"
                                OnSelectedIndexChanged="Grid_Partes_SelectedIndexChanged" 
                                PageSize="5">
                                <RowStyle CssClass="GridItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="30px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="BIEN_MUEBLE_ID" HeaderText="BIEN_MUEBLE_ID" SortExpression="BIEN_MUEBLE_ID">
                                        <ItemStyle Width="120px" HorizontalAlign="Center" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_INVENTARIO_ANTERIOR" HeaderText="No. Inventario" SortExpression="NO_INVENTARIO_ANTERIOR" >
                                        <ItemStyle Width="120px" HorizontalAlign="Center" Font-Size="X-Small"  />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_INVENTARIO_SIAS" HeaderText="No. Inventario [SIAS]" SortExpression="NO_INVENTARIO_SIAS" >
                                        <ItemStyle Width="120px" HorizontalAlign="Center" Font-Size="X-Small"  />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE" NullDisplayText="Nombre" >
                                        <ItemStyle Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MARCA" HeaderText="Marca" SortExpression="MARCA" NullDisplayText="Marca">
                                        <ItemStyle Width="120px" HorizontalAlign="Center" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MODELO" HeaderText="Modelo" SortExpression="MODELO" NullDisplayText="Modelo">
                                        <ItemStyle Width="120px" HorizontalAlign="Center" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTADO" HeaderText="Estado" SortExpression="ESTADO" NullDisplayText="Modelo">
                                        <ItemStyle Width="50px" HorizontalAlign="Center" Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS" NullDisplayText="Modelo">
                                        <ItemStyle Width="50px" HorizontalAlign="Center" Font-Size="X-Small" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                            </asp:GridView>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>       
            </div>            
            <br />
            <br />
        </ContentTemplate>           
    </asp:UpdatePanel>  
    
    <asp:UpdatePanel ID="UpPnl_MPE_Busqueda_Vehiculo" runat="server"  UpdateMode="Conditional"> 
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin" runat="server" Text="Button" style="display:none;"/> 
            <cc1:ModalPopupExtender ID="MPE_Busqueda_Vehiculo" runat="server" TargetControlID="Btn_Comodin" 
                PopupControlID="Pnl_Busqueda_Vehiculo" CancelControlID="Btn_Cerrar" PopupDragHandleControlID="Pnl_Interno"
                DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID="UpPnl_aux_Busqueda_Resguardante" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_MPE_Resguardante" runat="server" Text="" style="display:none;"/>
                <cc1:ModalPopupExtender ID="MPE_Resguardante" runat="server" 
                TargetControlID="Btn_Comodin_MPE_Resguardante" PopupControlID="Pnl_Busqueda_Contenedor" 
                CancelControlID="Btn_Cerrar_Ventana" PopupDragHandleControlID="Pnl_Busqueda_Resguardante_Cabecera"
                DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>  
        </ContentTemplate>
    </asp:UpdatePanel>   
    
    <asp:Panel ID="Pnl_Busqueda_Vehiculo" runat="server" HorizontalAlign="Center" Width="800px" style="display:none;border-style:outset;border-color:Silver;background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Interno" runat="server" style="background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">    
            <center>
            <asp:UpdatePanel ID="UpPnl_Busqueda" runat="server"> 
                <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpPnl_Busqueda" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas_Busqueda" runat="server" Width="98%" ActiveTabIndex="0">
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Panel_Datos_Generales" ID="Tab_Panel_Datos_Generales_Busqueda" Width="100%" Height="400px">
                        <HeaderTemplate>Generales</HeaderTemplate>
                            <ContentTemplate>
                            <div style="border-style:outset; width:98%; height:200px;" >
                                <table width="100%">
                                    <tr>
                                        <td style="text-align:left;" colspan="2">
                                            <asp:Label ID="Lbl_Titulo_Busqueda" runat="server" Text="Búsqueda"></asp:Label>
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Numero_Inventario" runat="server" Text="Número Inventario" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Numero_Inventario" runat="server" Width="97%" ></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Numero_Inventario" runat="server" TargetControlID="Txt_Busqueda_Numero_Inventario" FilterType="Numbers" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Numero_Economico" runat="server" Text="Número Económico"  CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Numero_Economico" runat="server" Width="97%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Numero_Economico" runat="server" TargetControlID="Txt_Busqueda_Numero_Economico" InvalidChars="<,>,&,',!,"  FilterType="Numbers"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                    </tr>                                
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Modelo" runat="server" Text="Modelo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Modelo_Busqueda" runat="server" Width="97%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Modelo_Busqueda" runat="server" TargetControlID="Txt_Modelo_Busqueda"
                                                InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>                               
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Marca" runat="server" Text="Marca" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Marca" runat="server"  Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>            
                                            </asp:DropDownList>                             
                                        </td>
                                    </tr>                       
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Tipo_Vehiculo" runat="server" Text="Tipo Vehículo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Tipo_Vehiculo" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="SELECCIONE"></asp:ListItem>
                                            </asp:DropDownList>                                 
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Tipo_Combustible" runat="server" Text="Combustible" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Tipo_Combustible" runat="server"  Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>            
                                            </asp:DropDownList>                             
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Anio_Fabricacion" runat="server" Text="Año Fabricación" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Anio_Fabricacion" runat="server" Width="97%" MaxLength="4"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Anio_Fabricacion" runat="server" TargetControlID="Txt_Busqueda_Anio_Fabricacion" FilterType="Numbers" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Color" runat="server" Text="Color" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Color" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                    </tr>             
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Zonas" runat="server" Text="Zonas" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Zonas" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Estatus" runat="server" Text="Estatus" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="100%" >
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                            <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                            <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                            <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                    </tr>    
                                    <tr>
                                        <td style="width:20%; text-align:left;">
                                            <asp:Label ID="Lbl_Busqueda_Dependencias" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:80%; text-align:left;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Busqueda_Dependencias" runat="server" Width="85%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                                            </asp:DropDownList>                                                 
                                            <asp:ImageButton ID="Btn_Buscar_Datos" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" CausesValidation="False" 
                                                ToolTip="Buscar Contrarecibos" OnClick="Btn_Buscar_Datos_Click" />
                                            <asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Datos" runat="server" 
                                                CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" 
                                                ToolTip="Limpiar Filtros" Width="20px" OnClick="Btn_Limpiar_Filtros_Buscar_Datos_Click" />                                      
                                        </td>
                                    </tr>                                     
                                </table>
                            </div>                                 
                        </ContentTemplate>
                    </cc1:TabPanel>   
                    
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Panel_Reguardantes" ID="Tab_Panel_Resguardantes_Busqueda" Width="100%" >
                        <HeaderTemplate>Resguardantes</HeaderTemplate>
                        <ContentTemplate>    
                            <div style="border-style:outset; width:98%; height:200px;" >
                                <table width="100%">
                                    <tr>
                                        <td style="text-align:left;" colspan="2">
                                            <asp:Label ID="Lbl_Busqueda_Listado" runat="server" Text="Búsqueda"></asp:Label>
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_RFC_Resguardante" runat="server" Text="RFC Reguardante" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:80%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_RFC_Resguardante" runat="server" Width="200px"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_RFC_Resguardante" runat="server" TargetControlID="Txt_Busqueda_RFC_Resguardante" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                    </tr>                          
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Resguardantes_Dependencias" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:80%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Resguardantes_Dependencias" runat="server" Width="100%" OnSelectedIndexChanged="Cmb_Busqueda_Resguardantes_Dependencias_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                                            </asp:DropDownList>                                   
                                        </td>
                                    </tr>                          
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Nombre_Resguardante" runat="server" Text="Resguardante" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:80%; text-align:left;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Busqueda_Nombre_Resguardante" runat="server" Width="100%" >
                                                <asp:ListItem Text="&lt;TODOS&gt;" Value="TODOS"></asp:ListItem>
                                            </asp:DropDownList>     
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td colspan="4" style="text-align:right">
                                            <asp:ImageButton ID="Btn_Buscar_Resguardante" runat="server" 
                                                CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                ToolTip="Buscar Listados" OnClick="Btn_Buscar_Resguardante_Click" />                                      
                                            <asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Resguardante" runat="server"  
                                                CausesValidation="False"  
                                                ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Width="20px"  
                                                ToolTip="Limpiar Filtros" OnClick="Btn_Limpiar_Filtros_Buscar_Resguardante_Click" />   
                                            &nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>                                      
                                </table>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>  
                <div style="width:97%; height:150px; overflow:auto; border-style:outset; background-color:White;">
                    <center>
                        <caption>
                            <asp:GridView ID="Grid_Listado_Vehiculos" runat="server" AllowPaging="true"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                OnPageIndexChanging="Grid_Listado_Vehiculos_PageIndexChanging"
                                OnSelectedIndexChanged="Grid_Listado_Vehiculos_SelectedIndexChanged"
                                Width="98%" PageSize="100" >
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="30px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="VEHICULO_ID" HeaderText="VEHICULO_ID" SortExpression="VEHICULO_ID" />
                                    <asp:BoundField DataField="NUMERO_INVENTARIO" HeaderText="No. Inven." SortExpression="NUMERO_INVENTARIO" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NUMERO_ECONOMICO" HeaderText="No. Eco." SortExpression="NUMERO_ECONOMICO" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VEHICULO" HeaderText="Vehículo" SortExpression="VEHICULO" >
                                        <ItemStyle Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MARCA" HeaderText="Marca" SortExpression="MARCA" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MODELO" HeaderText="Modelo" SortExpression="MODELO">
                                        <ItemStyle Width="150px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ANIO" HeaderText="Año" SortExpression="ANIO">
                                        <ItemStyle Width="70px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" SortExpression="ESTATUS" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                            </asp:GridView>
                        </caption>
                    </center>   
                </div>                                                  
                </ContentTemplate>
            </asp:UpdatePanel>
            <table width="95%">
                <tr>
                    <td style="width:100%">
                        <center>
                            <asp:Button ID="Btn_Cerrar" runat="server" TabIndex="202" Text="Cerrar" Width="80px"  Height="26px" />
                        </center>
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    </asp:Panel>     
    
    <asp:Panel ID="Pnl_Busqueda_Contenedor" runat="server" CssClass="drag"  HorizontalAlign="Center" Width="850px" 
            style="display:none;border-style:outset;border-color:Silver;background-image:url(~/paginas/imagenes/paginas/Sias_Fondo_Azul.PNG);background-repeat:repeat-y;">                         
            <asp:Panel ID="Pnl_Busqueda_Resguardante_Cabecera" runat="server" 
                style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                <table width="99%">
                    <tr>
                        <td style="color:Black;font-size:12;font-weight:bold;">
                           <asp:Image ID="Img_Informatcion_Autorizacion" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                             B&uacute;squeda: Empleados
                        </td>
                        <td align="right" style="width:10%;">
                           <asp:ImageButton ID="Btn_Cerrar_Ventana" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                                ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                        </td>
                    </tr>
                </table>            
            </asp:Panel>                                                                          
           <div style="color: #5D7B9D">
             <table width="100%">
                <tr>
                    <td align="left" style="text-align: left;" >                                    
                        <asp:UpdatePanel ID="Upnl_Filtros_Busqueda_Prestamos" runat="server">
                            <ContentTemplate>
                            
                                <asp:UpdateProgress ID="Progress_Upnl_Filtros_Busqueda_Prestamos" runat="server" AssociatedUpdatePanelID="Upnl_Filtros_Busqueda_Prestamos" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                                        <div  style="background-color:Transparent;position:fixed;top:50%;left:47%;padding:10px; z-index:1002;" id="div_progress"><img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress> 
                                                             
                                  <table width="100%">
                                   <tr>
                                        <td style="width:100%" colspan="4" align="right">
                                            <asp:ImageButton ID="Btn_Limpiar_Ctlr_Busqueda" runat="server" OnClientClick="javascript:return Limpiar_Ctlr();"
                                                ImageUrl="~/paginas/imagenes/paginas/sias_clear.png" ToolTip="Limpiar Controles de Busqueda"/>                         
                                        </td>
                                    </tr>     
                                   <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                           No Empleado 
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Busqueda_No_Empleado" runat="server" Width="98%" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_No_Empleado" runat="server" FilterType="Numbers" TargetControlID="Txt_Busqueda_No_Empleado"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_No_Empleado" runat="server" TargetControlID ="Txt_Busqueda_No_Empleado" WatermarkText="Busqueda por No Empleado" 
                                                WatermarkCssClass="watermarked"/>                                                                                                                                          
                                        </td> 
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            RFC
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;">
                                           <asp:TextBox ID="Txt_Busqueda_RFC" runat="server" Width="98%" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_RFC" runat="server" FilterType="Numbers, UppercaseLetters"
                                                TargetControlID="Txt_Busqueda_RFC"/>  
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Txt_Busqueda_RFC" runat="server" 
                                                TargetControlID ="Txt_Busqueda_RFC" WatermarkText="Busqueda por RFC" 
                                                WatermarkCssClass="watermarked"/>                                                                                                                                     
                                        </td>                               
                                    </tr>
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Nombre
                                        </td>              
                                        <td style="width:30%;text-align:left;" colspan="3">
                                            <asp:TextBox ID="Txt_Busqueda_Nombre_Empleado" runat="server" Width="99.5%" />
                                           <cc1:FilteredTextBoxExtender ID="Fte_Txt_Busqueda_Nombre_Empleado" runat="server" FilterType="Custom, LowercaseLetters, Numbers, UppercaseLetters"
                                                TargetControlID="Txt_Busqueda_Nombre_Empleado" ValidChars="áéíóúÁÉÍÓÚ ñÑ"/>
                                            <cc1:TextBoxWatermarkExtender ID="Twm_Nombre_Empleado" runat="server" 
                                                TargetControlID ="Txt_Busqueda_Nombre_Empleado" WatermarkText="Busqueda por Nombre" 
                                                WatermarkCssClass="watermarked"/>                                                                                               
                                        </td>                                         
                                    </tr>                   
                                    <tr>
                                        <td style="width:20%;text-align:left;font-size:11px;">
                                            Unidad Responsable
                                        </td>              
                                        <td style="width:30%;text-align:left;font-size:11px;" colspan="3">
                                           <asp:DropDownList ID="Cmb_Busqueda_Dependencia" runat="server" Width="100%" />                                          
                                        </td> 
                                    </tr>                                                                
                                   <tr>
                                        <td style="width:100%" colspan="4">
                                            <hr />
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td style="width:100%;text-align:left;" colspan="4">
                                            <center>
                                               <asp:Button ID="Btn_Busqueda_Empleados" runat="server"  Text="Busqueda de Empleados" CssClass="button"  
                                                CausesValidation="false"  Width="200px" OnClick="Btn_Busqueda_Empleados_Click" /> 
                                            </center>
                                        </td>                                                     
                                    </tr>                                                                        
                                  </table>   
                                  <br />
                                  <div id="Div_Resultados_Busqueda_Resguardantes" runat="server" style="border-style:outset; width:99%; height: 250px; overflow:auto;">
                                      <asp:GridView ID="Grid_Busqueda_Empleados_Resguardo" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            ForeColor="#333333" GridLines="None" AllowPaging="True" Width="100%" 
                                            PageSize="100" EmptyDataText="No se encontrarón resultados para los filtros de la busqueda" 
                                            OnSelectedIndexChanged="Grid_Busqueda_Empleados_Resguardo_SelectedIndexChanged"
                                            OnPageIndexChanging="Grid_Busqueda_Empleados_Resguardo_PageIndexChanging"
                                            >
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                    <ItemStyle Width="30px" Font-Size="X-Small" HorizontalAlign="Center" />
                                                    <HeaderStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="EMPLEADO_ID" HeaderText="EMPLEADO_ID" SortExpression="EMPLEADO_ID">
                                                    <ItemStyle Width="3px" Font-Size="X-Small" HorizontalAlign="Center" />
                                                    <HeaderStyle Width="3px" Font-Size="X-Small" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="NO_EMPLEADO" HeaderText="No. Empleado" SortExpression="NO_EMPLEADO" >
                                                    <ItemStyle Width="70px" Font-Size="X-Small" HorizontalAlign="Center" />
                                                    <HeaderStyle Width="70px" Font-Size="X-Small" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE" NullDisplayText="-" >
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" />
                                                    <HeaderStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DEPENDENCIA" HeaderText="Unidad Responsable" SortExpression="DEPENDENCIA" NullDisplayText="-" >
                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" />
                                                    <HeaderStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                            </Columns>
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                    </asp:GridView> 
                                </div>                                                                                                                                                          
                            </ContentTemplate>                                                                   
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>                                                      
                    </td>
                </tr>
             </table>                                                   
           </div>                 
    </asp:Panel>   
    
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pat_Com_Actualizacion_Bienes_Muebles.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Pat_Com_Actualizacion_Bienes_Muebles" Title="Operación - Actualización de Bienes Muebles" %>

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
    <asp:ScriptManager ID="ScptM_Bienes_Muebles" runat="server"  EnableScriptGlobalization ="true" EnableScriptLocalization = "True" />
   
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
                        <td class="label_titulo" colspan="4">Actualización de Bienes Muebles</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="4" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td colspan="3">
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
                            <asp:ImageButton ID="Btn_Modificar" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Modificar" OnClick="Btn_Modificar_Click"/>
                            <asp:ImageButton ID="Btn_Generar_Reporte" runat="server" ToolTip="Ver Resguardo"
                                ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Ver Resguardo" onclick="Btn_Generar_Reporte_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" OnClick="Btn_Salir_Click"/>
                           
                        </td>
                    <td align="right" colspan="3" style="width:80%;">
                        <div id="Div_Busqueda" runat="server">
                            <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                                onclick="Btn_Avanzada_Click" ToolTip="Busqueda Avanzada">Busqueda</asp:LinkButton>
                                &nbsp;&nbsp;
                            <asp:TextBox ID="Txt_Busqueda_Anterior" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda_Anterior" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="< - Inventario Anterior - >"
                                TargetControlID="Txt_Busqueda_Anterior" />
                            <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda" FilterType="Numbers">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" 
                                WatermarkCssClass="watermarked"
                                WatermarkText="< - Inventario SIAS - >"
                                TargetControlID="Txt_Busqueda" />
                            <asp:ImageButton ID="Btn_Buscar" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                onclick="Btn_Buscar_Click"  />
                        </div>
                    </td>                                      
                </table>  
                <br />
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="98%" ActiveTabIndex="0" CssClass="Tab" style="visibility:visible;">
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Bienes_Detalles"  ID="Tab_Bienes_Detalles"  Width="100%">
                        <HeaderTemplate>Datos Generales</HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" class="estilo_fuente">                                      
                                    <tr>
                                        <td colspan="4">
                                            <asp:HiddenField ID="Hdf_Bien_Mueble_ID" runat="server" />
                                        </td>
                                    </tr>                                      
                                    <tr>
                                        <td style="text-align:left; vertical-align:top; width:13%">
                                            <asp:Label ID="Lbl_Nombre_Producto" runat="server" Text="Nombre Producto" ></asp:Label>
                                        </td>
                                        <td style="width:37%;">
                                            <asp:TextBox ID="Txt_Nombre_Producto" runat="server" Width="97%" Enabled="False" ></asp:TextBox>
                                        </td>
                                        <td style="text-align:left; vertical-align:top; width:13%">
                                            <asp:Label ID="Lbl_Resguardo_Recibo" runat="server" Text="Operación"></asp:Label>
                                        </td>
                                        <td style=" width:37%;">
                                            <asp:TextBox ID="Txt_Resguardo_Recibo" runat="server" Width="97%" Enabled="False" ></asp:TextBox>
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:13%; text-align:left; ">
                                            <asp:Label ID="Lbl_Invenario_Anterior" runat="server" Text="No. Inventario"></asp:Label>
                                        </td>
                                        <td style="width:37%">
                                            <asp:TextBox ID="Txt_Invenario_Anterior" runat="server" Width="97%" Enabled="False"></asp:TextBox>                                         
                                        </td>
                                        <td style="width:13%; text-align:left; ">
                                            <asp:Label ID="Lbl_Numero_Inventario" runat="server" Text="Inventario Nuevo"></asp:Label>
                                        </td>
                                        <td style="width:37%">
                                            <asp:TextBox ID="Txt_Numero_Inventario" runat="server" Width="97%" Enabled="False"></asp:TextBox>                                         
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td width="13%">
                                            <asp:Label ID="Lbl_Clase_Activo" runat="server" Text="Clase Activo"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="Cmb_Clase_Activo" runat="server" Width="100%">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="13%">
                                            <asp:Label ID="Lbl_Tipo_Activo" runat="server" Text="Tipo Activo"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="Cmb_Tipo_Activo" runat="server" Width="100%">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>                   
                                    <tr>
                                        <td style="width:13%; text-align:left; ">
                                            <asp:Label ID="Lbl_Dependencia" runat="server" Text="U. Responsable"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                             <asp:DropDownList ID="Cmb_Dependencias" runat="server" Width="100%" 
                                                 OnSelectedIndexChanged="Cmb_Dependencias_SelectedIndexChanged" 
                                                 AutoPostBack="True">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>                                          
                                    <tr>
                                        <td style="width:13%; text-align:left; ">
                                            <asp:Label ID="Lbl_Zona" runat="server" Text="Zona"></asp:Label>
                                        </td>
                                        <td style="width:37%">
                                             <asp:DropDownList ID="Cmb_Zonas" runat="server" Width="99%" >
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align:left;width:13%;">
                                            <asp:Label ID="Lbl_Numero_Serie" runat="server" Text="No. Serie"></asp:Label>
                                        </td>
                                        <td style="width:37%">
                                            <asp:TextBox ID="Txt_Numero_Serie" runat="server" Width="97%" MaxLength="49"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Serie" runat="server" 
                                                                            TargetControlID="Txt_Numero_Serie" InvalidChars="<,>,&,',!," 
                                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ /*-+$%&" Enabled="True">   
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>                    
                                    <tr>
                                        <td style="width:13%; text-align:left; ">
                                            <asp:Label ID="Lbl_Marca" runat="server" Text="Marca"></asp:Label>
                                        </td>
                                        <td style="width:37%">
                                             <asp:DropDownList ID="Cmb_Marca" runat="server" Width="99%" >
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align:left;" width="13%">
                                            <asp:Label ID="Lbl_Procedencia_Bien" runat="server"  Text="Procedencia"></asp:Label>
                                        </td>
                                        <td style="width:37%">
                                            <asp:DropDownList ID="Cmb_Procedencia" runat="server" Width="99%">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>                                     
                                    <tr>
                                        <td  width="13%">
                                            <asp:Label ID="Lbl_Modelo" runat="server" Text="Modelo"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                             <asp:TextBox ID="Txt_Modelo" runat="server"  Width="99%" MaxLength="149"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                             runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                            TargetControlID="Txt_Modelo" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/$# " 
                                                 Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>                                  
                                    <tr>
                                        <td style="width:13%; text-align:left; ">
                                            <asp:Label ID="Lbl_Material" runat="server" Text="Material"></asp:Label>
                                        </td>
                                        <td style="width:37%">
                                            <asp:DropDownList ID="Cmb_Materiales" runat="server" Width="99%">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:13%; text-align:left; ">
                                            <asp:Label ID="Lbl_Color" runat="server" Text="Color"></asp:Label>
                                        </td>
                                        <td style="width:37%">
                                            <asp:DropDownList ID="Cmb_Colores" runat="server" Width="100%">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>                                          
                                    <tr>
                                        <td style="text-align:left; vertical-align:top; width:13%">
                                            <asp:Label ID="Lbl_Fecha_Adquisicion" runat="server" Text="F. Adquisición"></asp:Label>
                                        </td>
                                        <td style=" width:37%;">
                                            <asp:TextBox ID="Txt_Fecha_Adquisicion" runat="server" Width="80%" MaxLength="20" Enabled="false" ></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Fecha_Adquisicion" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Adquisicion" runat="server" TargetControlID="Txt_Fecha_Adquisicion" PopupButtonID="Btn_Fecha_Adquisicion" Format="dd/MMM/yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td style="width:13%; text-align:left; ">
                                            <asp:Label ID="Lbl_Factura" runat="server" Text="Factura"></asp:Label>
                                        </td>
                                        <td style="width:37%">
                                            <asp:TextBox ID="Txt_Factura" runat="server" Width="97%" MaxLength="49"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Factura" runat="server" 
                                                                            TargetControlID="Txt_Factura" InvalidChars="<,>,&,',!," 
                                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/$# " 
                                                Enabled="True">   
                                            </cc1:FilteredTextBoxExtender>      
                                        </td>
                                    </tr>   
                                    <tr>
                                       <td width="13%">
                                            <asp:HiddenField ID="Hdf_Proveedor_ID" runat="server" />
                                            <asp:Label ID="Lbl_Proveedor" runat="server" Text="Proveedor"></asp:Label>
                                        </td>
                                        <td  colspan="3">
                                            <asp:TextBox ID="Txt_Nombre_Proveedor" runat="server" Width="93%" Enabled="false"></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Lanzar_Mpe_Proveedores" runat="server" OnClick ="Btn_Lanzar_Mpe_Proveedores_Click" 
                                                 ImageUrl="~/paginas/imagenes/paginas/busqueda.png"  
                                                 ToolTip="Busqueda y Selección de Proveedor" AlternateText="Buscar" />
                                       </td>
                                    </tr>                            
                                    <tr>
                                        <td style="width:13%; text-align:left; ">
                                            <asp:Label ID="Lbl_Costo_Inicial" runat="server" Text="Costo Inicial [$]"></asp:Label>
                                        </td>
                                        <td style="width:37%">
                                            <asp:TextBox ID="Txt_Costo_Inicial" runat="server" Width="97%" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td style="width:13%; text-align:left; ">
                                            <asp:Label ID="Lbl_Costo_Actual" runat="server" Text="Costo Actual [$]"></asp:Label>
                                        </td>
                                        <td style="width:37%">
                                            <asp:TextBox ID="Txt_Costo_Actual" runat="server" Width="97%" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>                           
                                    <tr>
                                        <td style="width:13%; text-align:left; ">
                                            <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus"></asp:Label>
                                        </td>
                                        <td style="width:37%">
                                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="99%" >
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                                <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                                <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                                <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:13%; text-align:left; ">
                                            <asp:Label ID="Lbl_Estado" runat="server" Text="Estado"></asp:Label>
                                        </td>
                                        <td style="width:37%">
                                            <asp:DropDownList ID="Cmb_Estado" runat="server" Width="100%">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                                <asp:ListItem>BUENO</asp:ListItem>
                                                <asp:ListItem>REGULAR</asp:ListItem>
                                                <asp:ListItem>MALO</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr> 
                                    <tr>
                                         <td width="13%">
                                             <asp:Label ID="Lbl_Garantia" runat="server" Text="Garantía" ></asp:Label>
                                         </td>
                                         <td colspan="3">
                                            <asp:TextBox ID="Txt_Garantia" runat="server"  Width="99%" MaxLength="249" ></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Garantia" runat="server" 
                                                 Enabled="True" TargetControlID="Txt_Garantia" WatermarkCssClass="watermarked" 
                                                 WatermarkText="Límite de Caractes 250">
                                             </cc1:TextBoxWatermarkExtender>
                                             <cc1:FilteredTextBoxExtender ID="Txt_Comentarios_FilteredTextBoxExtender" 
                                                 runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                TargetControlID="Txt_Garantia" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/ " 
                                                 Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>                            
                                    <tr>
                                        <td style="text-align:left; vertical-align:top; width:13%">
                                            <asp:Label ID="Lbl_Motivo_Baja" runat="server" Text="Motivo Baja"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Motivo_Baja" runat="server" Width="99%" Rows="4" TextMode="MultiLine" Font-Size="X-Small"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Motivo_Baja" runat="server" 
                                                                            TargetControlID="Txt_Motivo_Baja" InvalidChars="<,>,&,',!," 
                                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ /*-+$%&" Enabled="True">   
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>           
                                    <tr>
                                        <td style="text-align:left; vertical-align:top; width:13%">
                                            <asp:Label ID="Lbl_Observaciones" runat="server" Text="Observaciones" ></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Observaciones" runat="server" Width="99%" Rows="4" TextMode="MultiLine" Font-Size="X-Small"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Observaciones" runat="server" 
                                                                            TargetControlID="Txt_Numero_Serie" InvalidChars="<,>,&,',!," 
                                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ /*-+$%&" 
                                                                            Enabled="True">   
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>  
                                </table>
                                <br />
                            <table width="100%">           
                                <tr>
                                    <td>
                                        <div runat="server" id="Div_Producto_Bien_Mueble_Padre" width="97%">
                                            <table width="100%" class="estilo_fuente">
                                                <tr>
                                                    <td class="label_titulo" colspan="4">Bien Mueble [Bien Principal]</td>
                                                </tr>
                                                 <tr align="right" class="barra_delgada">
                                                    <td colspan="4" align="center">
                                                    </td>
                                                </tr> 
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:HiddenField ID="Hdf_Bien_Padre_ID" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 18%; text-align: left;">
                                                        <asp:Label ID="Lbl_Nombre_Parent" runat="server" Text="Producto"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="Txt_Nombre_Parent" runat="server" Width="90%" Enabled="false"></asp:TextBox>
                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 18%; text-align: left;">
                                                        <asp:Label ID="Lbl_Numero_Inventario_Parent" runat="server" Text="No. Inventario" ></asp:Label>
                                                    </td>
                                                    <td style="width: 32%">
                                                        <asp:TextBox ID="Txt_Numero_Inventario_Parent" runat="server" Width="97%" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 18%; text-align: left;">
                                                        <asp:Label ID="Lbl_Inventario_SIAS" runat="server" Text="No. Inventario [SIAS]" ></asp:Label>
                                                    </td>
                                                    <td style="width: 32%">
                                                        <asp:TextBox ID="Txt_Inventario_SIAS" runat="server" Width="97%" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 18%; text-align: left;">
                                                        <asp:Label ID="Lbl_Marca_Padre" runat="server" Text="Marca" ></asp:Label>
                                                    </td>
                                                    <td style="width: 32%">
                                                        <asp:DropDownList ID="Cmb_Marca_Parent" runat="server" Width="100%" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 18%; text-align: left;">
                                                        <asp:Label ID="Lbl_Modelo_Parent" runat="server" Text="Modelo" ></asp:Label>
                                                    </td>
                                                    <td style="width: 32%">
                                                        <asp:TextBox ID="Txt_Modelo_Parent" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 18%; text-align: left;">
                                                        <asp:Label ID="Txt_Material_Parent" runat="server" Text="Material" ></asp:Label>
                                                    </td>
                                                    <td style="width: 32%">
                                                        <asp:DropDownList runat="server" ID="Cmb_Material_Parent" Width="100%" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 18%; text-align: left;">
                                                        <asp:Label ID="Lbl_Color_Parent" runat="server" Text="Color" ></asp:Label>
                                                    </td>
                                                    <td style="width: 32%">
                                                        <asp:DropDownList runat="server" ID="Cmb_Color_Parent" Width="100%" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 18%; text-align: left;">
                                                        <asp:Label ID="Lbl_Costo_Parent" runat="server" Text="Costo" CssClass="estilo_fuente"></asp:Label>
                                                    </td>
                                                    <td style="width: 32%">
                                                        <asp:TextBox ID="Txt_Costo_Parent" runat="server" Width="97%" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 18%; text-align: left;">
                                                        <asp:Label ID="Lbl_Fecha_Adquisicion_Parent" runat="server" Text="F. Adquisición"></asp:Label>
                                                    </td>
                                                    <td style="width: 32%">
                                                        <asp:TextBox ID="Txt_Fecha_Adquisicion_Parent" runat="server" Width="97%" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width:18%;">
                                                        <asp:Label ID="Lbl_Estado_Parent" runat="server" Text="Estado" ></asp:Label>
                                                    </td>
                                                    <td style="width: 32%">
                                                        <asp:DropDownList ID="Cmb_Estado_Parent" runat="server" Width="100%" Enabled="false">
                                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                                            <asp:ListItem Value="BUENO">BUENO</asp:ListItem>
                                                            <asp:ListItem Value="REGULAR">REGULAR</asp:ListItem>
                                                            <asp:ListItem Value="MALO">MALO</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="text-align: left; width:18%;">
                                                        <asp:Label ID="Lbl_Estatus_Parent" runat="server" Text="Estatus" ></asp:Label>
                                                    </td>
                                                    <td style="width: 32%">
                                                        <asp:DropDownList ID="Cmb_Estatus_Parent" runat="server" Width="100%" Enabled="False">
                                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                                            <asp:ListItem Value="VIGENTE" Selected="True">VIGENTE</asp:ListItem>
                                                            <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                                            <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; vertical-align: top; width:18%;">
                                                        <asp:Label ID="Lbl_Observaciones_Parent" runat="server" Text="Observaciones" ></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="Txt_Observaciones_Parent" runat="server" Width="99%" Rows="2" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>     
                                            </table>
                                        </div>
                                        <div id="Div_Vehiculo_Parent" runat="server" style="width:98%;">
                                            <table width="98%" class="estilo_fuente">
                                                <caption class="label_titulo">Vehículo [Bien Principal]</caption>
                                                <tr align="right" class="barra_delgada">
                                                    <td align="center" colspan="4">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:15%; text-align:left; ">
                                                        <asp:Label ID="Lbl_Vehiculo_Nombre" runat="server" Text="Nombre" ></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="Txt_Vehiculo_Nombre" runat="server" Width="92%" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:15%; text-align:left; ">
                                                        <asp:Label ID="Lbl_Vehiculo_No_Inventario" runat="server" Text="No. Inventario" ></asp:Label>
                                                    </td>
                                                    <td style="width:35%">
                                                        <asp:TextBox ID="Txt_Vehiculo_No_Inventario" runat="server" Width="99%" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width:15%; text-align:left;">
                                                        &nbsp;&nbsp;
                                                        <asp:Label ID="Lbl_Vehiculo_Numero_Serie" runat="server" Text="No. Serie" ></asp:Label>
                                                    </td>
                                                    <td style="width:35%">
                                                        <asp:TextBox ID="Txt_Vehiculo_Numero_Serie" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:15%; text-align:left;">
                                                        <asp:Label ID="Lbl_Vehiculo_Marca" runat="server" Text="Marca" ></asp:Label>
                                                    </td>
                                                    <td style="width:35%">
                                                        <asp:TextBox ID="Txt_Vehiculo_Marca" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width:15%; text-align:left; ">
                                                        &nbsp;&nbsp;
                                                        <asp:Label ID="Lbl_Vehiculo_Modelo" runat="server" Text="Modelo" ></asp:Label>
                                                    </td>
                                                    <td style="width:35%">
                                                        <asp:TextBox ID="Txt_Vehiculo_Modelo" runat="server" Width="100%" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:15%; text-align:left;">
                                                        <asp:Label ID="Lbl_Vehiculo_Tipo" runat="server" Text="Tipo" ></asp:Label>
                                                    </td>
                                                    <td style="width:35%">
                                                        <asp:TextBox ID="Txt_Vehiculo_Tipo" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width:15%; text-align:left;">
                                                        &nbsp;&nbsp;
                                                        <asp:Label ID="Lbl_Vehiculo_Color" runat="server" Text="Color" ></asp:Label>
                                                    </td>
                                                    <td style="width:35%">
                                                        <asp:TextBox ID="Txt_Vehiculo_Color" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:15%; text-align:left;">
                                                        <asp:Label ID="Lbl_Vehiculo_Numero_Economico" runat="server" Text="No. Economico" ></asp:Label>
                                                    </td>
                                                    <td style="width:35%">
                                                        <asp:TextBox ID="Txt_Vehiculo_Numero_Economico" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width:15%; text-align:left;">
                                                        &nbsp;&nbsp;
                                                        <asp:Label ID="Lbl_Vehiculo_Placas" runat="server" Text="Placas" ></asp:Label>
                                                    </td>
                                                    <td style="width:35%">
                                                        <asp:TextBox ID="Txt_Vehiculo_Placas" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>          
                                    </td>
                                </tr>
                            </table> 
                            <table width="100%">            
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
                                                        <asp:Label ID="Lbl_Resguardantes" runat="server" Text="Empleados"></asp:Label>
                                                    </td>
                                                    <td  style="width:85%; text-align:left;">
                                                        <asp:DropDownList ID="Cmb_Empleados" runat="server" Width="98%">
                                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:15%; text-align:left; vertical-align:top;">
                                                        <asp:Label ID="Lbl_Comentarios" runat="server" Text="Comentarios"></asp:Label>
                                                    </td>
                                                    <td  style="width:85%; text-align:left;">
                                                        <asp:TextBox ID="Txt_Cometarios" runat="server" TextMode="MultiLine" Rows="3" Width="98%"></asp:TextBox>
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
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Historial_Resguardos"  ID="Tab_Historial_Resguardos"  Width="100%" >
                        <HeaderTemplate>Historial</HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table width="100%">  
                                    <tr>
                                        <td class="label_titulo" colspan="4">Modificaciones</td>
                                    </tr>   
                                    <tr align="right" class="barra_delgada">
                                        <td colspan="4" align="center">
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td style="text-align:left; vertical-align:top; width:18%">
                                            <asp:Label ID="Lbl_Usuario_Creo" runat="server" Text="Creación"  CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Usuario_creo" runat="server" Width="99%" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>     
                                    <tr>
                                        <td style="text-align:left; vertical-align:top; width:18%">
                                            <asp:Label ID="Lbl_Usuario_Modifico" runat="server" Text="Ultima Modificación"  CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Usuario_Modifico" runat="server" Width="99%" 
                                                Enabled="False"></asp:TextBox>
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
                                        <td style="width:18%; text-align:left;">
                                            <asp:Label ID="Lbl_Historial_Empleado_Resguardo" runat="server" Text="Empleado" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td  colspan="3" style="text-align:left;">
                                            <asp:TextBox ID="Txt_Historial_Empleado_Resguardo" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>                                  
                                    <tr>
                                        <td style="width:18%; text-align:left;">
                                            <asp:Label ID="Lbl_Historial_Fecha_Inicial_Resguardo" runat="server" Text="Fecha Inicial" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:32%; text-align:left;">
                                            <asp:TextBox ID="Txt_Historial_Fecha_Inicial_Resguardo" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width:18%; text-align:left;">
                                            <asp:Label ID="Lbl_Historial_Fecha_Final_Resguardo" runat="server" Text="Fecha Final" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:32%; text-align:left;">
                                            <asp:TextBox ID="Txt_Historial_Fecha_Final_Resguardo" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                   <tr>
                                        <td style="width:18%; text-align:left; vertical-align:top;">
                                            <asp:Label ID="Lbl_Historial_Comentarios_Resguardo" runat="server" Text="Comentarios" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td  colspan="3" style="text-align:left;">
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
                                    PageSize="10" >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                            <ItemStyle Width="20px" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="BIEN_RESGUARDO_ID" HeaderText="BIEN_RESGUARDO_ID" SortExpression="BIEN_RESGUARDO_ID"/>
                                        <asp:BoundField DataField="EMPLEADO_ID" HeaderText="Empleado ID" SortExpression="EMPLEADO_ID" />
                                        <asp:BoundField DataField="NO_EMPLEADO" HeaderText="No. Empleado" SortExpression="NO_EMPLEADO">
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
                            <table width="100%">              
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
                                OnPageIndexChanging="Grid_Archivos_PageIndexChanging" 
                                onrowdatabound="Grid_Archivos_RowDataBound">
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
                                        NullDisplayText="ARCHIVO DEL BIEN..." />
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
                                        <asp:Label ID="Lbl_Nombre_Parte" runat="server" Text="Producto" CssClass="estilo_fuente"></asp:Label>
                                    </td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="Txt_Nombre_Parte" runat="server" Width="98%" Enabled="false"></asp:TextBox>
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
                                        <ItemStyle Width="120px" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_INVENTARIO_ANTERIOR" HeaderText="No. Inventario" SortExpression="NO_INVENTARIO_ANTERIOR" >
                                        <ItemStyle Width="120px" HorizontalAlign="Center"  />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_INVENTARIO_SIAS" HeaderText="No. Inventario [SIAS]" SortExpression="NO_INVENTARIO_SIAS" >
                                        <ItemStyle Width="120px" HorizontalAlign="Center"  />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE" NullDisplayText="Nombre" />
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
            <br />
            <br />
        </ContentTemplate>           
    </asp:UpdatePanel>  
    
    <asp:UpdatePanel ID="UpPnl_Modal" runat="server" UpdateMode="Conditional"> 
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin" runat="server" Text="Button" style="display:none;"/> 
            <cc1:ModalPopupExtender ID="MPE_Busqueda_Bien_Mueble" runat="server" TargetControlID="Btn_Comodin" 
                BackgroundCssClass="progressBackgroundFilter"
                PopupControlID="Pnl_Busqueda_Bien_Mueble" CancelControlID="Btn_Cerrar" PopupDragHandleControlID="Pnl_Interno"
                DropShadow="True" />
        </ContentTemplate>           
    </asp:UpdatePanel>  
        
    <asp:UpdatePanel ID="UpPnl_Aux_Mpe_Proveedores" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_Mpe_Proveedores" runat="server" Text="" style="display:none;"/>
                <cc1:ModalPopupExtender ID="Mpe_Proveedores_Cabecera" runat="server" 
                TargetControlID="Btn_Comodin_Mpe_Proveedores" PopupControlID="Pnl_Mpe_Proveedores" 
                CancelControlID="Btn_Cerrar_Mpe_Proveedores" PopupDragHandleControlID="Pnl_Mpe_Proveedores_Interno"
                DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>  
        </ContentTemplate>
    </asp:UpdatePanel>  
      
    <asp:UpdatePanel ID="UpPnl_Aux_Multiples_Resultados" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_Mpe_Multiples_Resultados" runat="server" Text="" style="display:none;"/>
                <cc1:ModalPopupExtender ID="Mpe_Multiples_Resultados" runat="server" 
                TargetControlID="Btn_Comodin_Mpe_Multiples_Resultados" PopupControlID="Pnl_Multiples_Resultados" 
                CancelControlID="Btn_Cerrar_Mpe_Multiples_Resultados" PopupDragHandleControlID="Pnl_Multiples_Resultados_Interno"
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
    
    <asp:Panel ID="Pnl_Busqueda_Bien_Mueble" runat="server" HorizontalAlign="Center" Width="800px" style="display:none;border-style:outset;border-color:Silver;background-repeat:repeat-y;">                         
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
                        <HeaderTemplate>Datos Generales</HeaderTemplate>
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
                                            <asp:Label ID="Lbl_Busqueda_Producto" runat="server" Text="Nombre Producto" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Producto" runat="server" Width="98%" ></asp:TextBox>   
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Producto" runat="server" 
                                                TargetControlID="Txt_Busqueda_Producto" InvalidChars="<,>,&,',!,"  
                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>                                                                                 
                                        </td>
                                    </tr>                                                 
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Dependencias" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Dependencias" runat="server" Width="99%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                                            </asp:DropDownList>                                   
                                        </td>
                                    </tr>                             
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Modelo" runat="server" Text="Modelo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Modelo" runat="server" Width="98%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Modelo" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                TargetControlID="Txt_Busqueda_Modelo" 
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/$# " Enabled="True">
                                            </cc1:FilteredTextBoxExtender>                               
                                        </td>
                                    </tr>                                                 
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Marca" runat="server" Text="Marca" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Marca" runat="server" Width="98%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>            
                                            </asp:DropDownList>                             
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Factura" runat="server" Text="No. Factura" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Factura" runat="server" Width="95%"></asp:TextBox>    
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Factura" runat="server" 
                                                TargetControlID="Txt_Busqueda_Factura" InvalidChars="<,>,&,',!,"  
                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>                                    
                                        </td>
                                    </tr>                   
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Estatus" runat="server" Text="Estatus" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="98%">
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                                <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                                <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                                <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                        <td style="width:20%; text-align:left;">
                                            <asp:Label ID="Lbl_Busqueda_Numero_Serie" runat="server" Text="No. Serie" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Numero_Serie" runat="server" Width="95%"></asp:TextBox>      
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Numero_Serie" runat="server" 
                                                TargetControlID="Txt_Busqueda_Numero_Serie" InvalidChars="<,>,&,',!,"  
                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ /*-+$%&" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>                   
                                                                              
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="text-align:right;">
                                            <asp:ImageButton ID="Btn_Buscar_Datos" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" CausesValidation="False" 
                                                OnClick="Btn_Buscar_Datos_Click" ToolTip="Buscar" />
                                            <asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Datos" runat="server" 
                                                CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" 
                                                ToolTip="Limpiar Filtros" 
                                                onclick="Btn_Limpiar_Filtros_Buscar_Datos_Click" Width="20px"/>  
                                            &nbsp;&nbsp;  
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
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_RFC_Resguardante" runat="server" Width="300px"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_RFC_Resguardante" runat="server" TargetControlID="Txt_Busqueda_RFC_Resguardante" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                    </tr>                          
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Resguardantes_Dependencias" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Resguardantes_Dependencias" runat="server" Width="98%" OnSelectedIndexChanged="Cmb_Busqueda_Resguardantes_Dependencias_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                                            </asp:DropDownList>                                   
                                        </td>
                                    </tr>                          
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Nombre_Resguardante" runat="server" Text="Resguardante" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Nombre_Resguardante" runat="server" Width="98%" >
                                                <asp:ListItem Text="&lt; SELECCIONE &gt;" Value="SELECCIONE"></asp:ListItem>
                                            </asp:DropDownList>     
                                        </td>
                                    </tr>    
                                     <tr>
                                        <td colspan="4" style="text-align:right;">
                                            <asp:ImageButton ID="Btn_Buscar_Resguardante" runat="server" 
                                                CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                ToolTip="Buscar Listados" OnClick="Btn_Buscar_Resguardante_Click"/>                                      
                                            <asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Resguardante" runat="server"  
                                                CausesValidation="False"  
                                                ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Width="20px"  
                                                ToolTip="Limpiar Filtros" 
                                                OnClick="Btn_Limpiar_Filtros_Buscar_Resguardante_Click"  />   
                                            &nbsp;&nbsp;                                 
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
                            <asp:GridView ID="Grid_Listado_Bienes" runat="server"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"  AllowPaging="true" PageSize="100"
                                OnSelectedIndexChanged="Grid_Listado_Bienes_SelectedIndexChanged"
                                Width="98%" OnPageIndexChanging="Grid_Listado_Bienes_PageIndexChanging">
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="30px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="BIEN_MUEBLE_ID" HeaderText="BIEN_MUEBLE_ID" SortExpression="BIEN_MUEBLE_ID" />
                                    <asp:BoundField DataField="NO_INVENTARIO_ANTERIOR" HeaderText="Inv. Anterior" SortExpression="NO_INVENTARIO_ANTERIOR" >
                                        <ItemStyle Width="150px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_INVENTARIO" HeaderText="Inv. SIAS" SortExpression="NO_INVENTARIO" >
                                        <ItemStyle Width="50px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE_PRODUCTO" HeaderText="Nombre" SortExpression="NOMBRE_PRODUCTO"  >
                                        <ItemStyle Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MARCA" HeaderText="Marca" SortExpression="MARCA" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MODELO" HeaderText="Modelo" SortExpression="MODELO" >
                                        <ItemStyle Width="150px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="COLOR" HeaderText="Color" SortExpression="COLOR"  >
                                        <ItemStyle Width="150px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTADO" HeaderText="Estado" SortExpression="ESTADO" >
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
    
    <asp:Panel ID="Pnl_Mpe_Proveedores" runat="server" CssClass="drag" HorizontalAlign="Center" 
    style="display:none;border-style:outset;border-color:Silver;width:760px;">                
    <asp:Panel ID="Pnl_Mpe_Proveedores_Interno" runat="server" CssClass="estilo_fuente"
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table class="estilo_fuente" width="100%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;width:90%">
                   <asp:Image ID="Img_Mpe_Proveedores" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Busqueda y Selección de Proveedores
                </td>
                <td align="right">
                   <asp:ImageButton ID="Btn_Cerrar_Mpe_Proveedores" CausesValidation="false" runat="server" style="cursor:pointer;" 
                        ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
     </asp:Panel>
       <center>
        <asp:UpdatePanel ID="UpPnl_Mpe_Proveedores" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpPgr_Mpe_Proveedores" runat="server" AssociatedUpdatePanelID="UpPnl_Mpe_Proveedores" DisplayAfter="0">
                        <ProgressTemplate>
                          <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                            <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                        </ProgressTemplate>                     
                    </asp:UpdateProgress>
                    <br />
                    <br />
                    <div style="border-style: outset; width: 95%; height: 380px; background-color: White;">
                        <table width="100%">
                            <tr>
                                <td style="width:15%; text-align:left;">
                                    <asp:Label ID="Lbl_Nombre_Proveedores_Buscar" runat="server" CssClass="estilo_fuente" Text="Introducir" style="font-weight:bolder;" ></asp:Label>
                                </td>
                                <td style="width:85%; text-align:left;">
                                    <asp:TextBox ID="Txt_Nombre_Proveedor_Buscar" runat="server" 
                                     Width="92%" AutoPostBack="true" 
                                     ontextchanged="Txt_Nombre_Proveedor_Buscar_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Proveedor_Buscar" runat="server" TargetControlID="Txt_Nombre_Proveedor_Buscar" InvalidChars="<,>,&,',!," 
                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                                    </cc1:FilteredTextBoxExtender>  
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Nombre_Proveedor_Buscar" runat="server" Enabled="True" TargetControlID="Txt_Nombre_Proveedor_Buscar" WatermarkCssClass="watermarked" 
                                                     WatermarkText="<- Razón Social ó Nombre Comercial ->">
                                    </cc1:TextBoxWatermarkExtender>
                                    <asp:ImageButton ID="Btn_Ejecutar_Busqueda_Proveedores" runat="server" OnClick ="Btn_Ejecutar_Busqueda_Proveedores_Click" 
                                         ImageUrl="~/paginas/imagenes/paginas/busqueda.png"  
                                         ToolTip="Buscar Productos" AlternateText="Buscar" />
                                </td>
                            </tr>
                        </table>
                        <br />
                            <asp:Panel ID="Pnl_Listado_Proveedores" runat="server" ScrollBars="Vertical" style="white-space:normal;"
                                Width="100%" BorderColor="#3366FF" Height="330px">
                                       <asp:GridView ID="Grid_Listado_Proveedores" runat="server" 
                                         AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                         GridLines="None"  PageSize="150" AllowPaging="true"
                                         OnSelectedIndexChanged="Grid_Listado_Proveedores_SelectedIndexChanged"
                                         OnPageIndexChanging="Grid_Listado_Proveedores_PageIndexChanging"
                                          Width="100%" CssClass="GridView_1">
                                         <AlternatingRowStyle CssClass="GridAltItem" />
                                         <Columns>
                                             <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                 ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                 <ItemStyle Width="30px" />
                                             </asp:ButtonField>
                                             <asp:BoundField DataField="PROVEEDOR_ID" HeaderText="Proveedor ID" SortExpression="PROVEEDOR_ID"  >
                                                 <ItemStyle Width="30px" Font-Size="X-Small" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="RFC" HeaderText="RFC" SortExpression="RFC" >
                                                 <ItemStyle Font-Size="X-Small" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="NOMBRE" HeaderText="Razón Social" SortExpression="NOMBRE">
                                                <ItemStyle Font-Size="X-Small"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="COMPANIA" HeaderText="Nombre Comercial" SortExpression="COMPANIA">
                                                 <ItemStyle Font-Size="X-Small" />
                                             </asp:BoundField>
                                         </Columns>
                                         <HeaderStyle CssClass="GridHeader" />
                                         <PagerStyle CssClass="GridHeader" />
                                         <RowStyle CssClass="GridItem" />
                                         <SelectedRowStyle CssClass="GridSelected" />
                                     </asp:GridView>
                           </asp:Panel>
                        <br />
                    </div>
                    <br />
                    <br />
                </ContentTemplate>
            </asp:UpdatePanel>
       </center>          
    </asp:Panel>
    
    <asp:Panel ID="Pnl_Multiples_Resultados" runat="server" CssClass="drag" HorizontalAlign="Center" style="display:none;border-style:outset;border-color:Silver;width:760px;">                
    <asp:Panel ID="Pnl_Multiples_Resultados_Interno" runat="server" CssClass="estilo_fuente"
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table class="estilo_fuente" width="100%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;width:90%">
                   <asp:Image ID="Image1" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                    Seleccione el Bien
                </td>
                <td align="right">
                   <asp:ImageButton ID="Btn_Cerrar_Mpe_Multiples_Resultados" CausesValidation="false" runat="server" style="cursor:pointer;" 
                        ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
     </asp:Panel>
       <center>
        <asp:UpdatePanel ID="UpPnl_Multiples_Resultados" runat="server"> 
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpPgr_Multiples_Resultados" runat="server" AssociatedUpdatePanelID="UpPnl_Multiples_Resultados" DisplayAfter="0">
                        <ProgressTemplate>
                            <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                            <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                        </ProgressTemplate>                     
                    </asp:UpdateProgress>
                    <br />
                    <div style="border-style: outset; width: 95%; height: 380px; background-color: White;">
                        <asp:Panel ID="Pnl_Grid_Resultados_Multiples" runat="server" ScrollBars="Vertical" style="white-space:normal;"
                            Width="100%" BorderColor="#3366FF" Height="370px"> 
                            <asp:GridView ID="Grid_Resultados_Multiples" runat="server" HeaderStyle-CssClass="tblHead"
                                EmptyDataText="No se encontrarón Registros"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"  AllowPaging="true" PageSize="100"
                                OnSelectedIndexChanged="Grid_Resultados_Multiples_SelectedIndexChanged" 
                                OnPageIndexChanging="Grid_Resultados_Multiples_PageIndexChanging"
                                Width="100%">
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="30px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="BIEN_MUEBLE_ID" HeaderText="BIEN_MUEBLE_ID" SortExpression="BIEN_MUEBLE_ID" />
                                    <asp:BoundField DataField="NO_INVENTARIO_ANTERIOR" HeaderText="Inv. Anterior" SortExpression="NO_INVENTARIO_ANTERIOR" >
                                        <ItemStyle Width="150px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NO_INVENTARIO" HeaderText="Inv. SIAS" SortExpression="NO_INVENTARIO" >
                                        <ItemStyle Width="50px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NOMBRE_PRODUCTO" HeaderText="Nombre" SortExpression="NOMBRE_PRODUCTO"  >
                                        <ItemStyle Font-Size="X-Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MARCA" HeaderText="Marca" SortExpression="MARCA" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MODELO" HeaderText="Modelo" SortExpression="MODELO" >
                                        <ItemStyle Width="150px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="COLOR" HeaderText="Color" SortExpression="COLOR"  >
                                        <ItemStyle Width="150px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ESTADO" HeaderText="Estado" SortExpression="ESTADO" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="OPERACION" HeaderText="Operación" SortExpression="OPERACION" >
                                        <ItemStyle Width="70px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="GridHeader" />
                                <PagerStyle CssClass="GridHeader" />
                                <SelectedRowStyle CssClass="GridSelected" />
                            </asp:GridView>
                       </asp:Panel>
                        <br />
                    </div>
                    <br />
                    <br />
                </ContentTemplate>
            </asp:UpdatePanel>
       </center>          
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


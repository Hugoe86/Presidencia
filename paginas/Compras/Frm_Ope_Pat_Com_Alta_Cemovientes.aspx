<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pat_Com_Alta_Cemovientes.aspx.cs" Inherits="paginas_predial_Frm_Ope_Pat_Com_Alta_Cemovientes" Title="Operación - Asignación de Cemovientes" %>

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
    //Metodos para limpiar los controles de la busqueda.
    function Limpiar_Ctlr_Donador(){
        document.getElementById("<%=Hdf_Donador_ID.ClientID%>").value="";
        document.getElementById("<%=Txt_Nombre_Donador.ClientID%>").value="";                         
        return false;
    }  
</script>  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScptM_Cemovientes" runat="server" EnableScriptGlobalization ="true" EnableScriptLocalization = "True"/>
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
                        <td class="label_titulo" colspan="2">Asignación de Animales</td>
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
                        <asp:ImageButton ID="Btn_Generar_Reporte" runat="server" ToolTip="Ver Resguardo" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" CssClass="Img_Button" AlternateText="Ver Resguardo" onclick="Btn_Generar_Reporte_Click" />
                        <asp:ImageButton ID="Btn_Salir" runat="server"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"  CausesValidation="false"
                                AlternateText="Salir" onclick="Btn_Salir_Click"/>
                        </td>
                        <td>&nbsp;</td>                                          
                </table>   
                <br /> 
                <table width="98%" class="estilo_fuente">
                    <tr>
                        <td class="label_titulo" colspan="4">Datos Generales</td>
                    </tr>           
                    <tr align="right" class="barra_delgada">
                        <td colspan="4" align="center"></td>
                    </tr>   
                    <tr>
                        <td colspan="4"> 
                            <asp:HiddenField ID="Hdf_Producto_ID" runat="server" /> 
                            <asp:HiddenField ID="Hdf_Animal_ID" runat="server" /> 
                        </td>
                    </tr>           
                    <tr> 
                        <td width="18%" align="left" >
                            <asp:Label ID="Lbl_Producto" runat="server" Text="* Producto"></asp:Label>
                        </td>
                        <td  colspan="3" align="left">
                            <asp:TextBox ID="Txt_Nombre_Producto_Donado" runat="server" Width="93%" Enabled="false"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Lanzar_Mpe_Productos" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Busqueda y Selección de Producto" AlternateText="Buscar" OnClick="Btn_Lanzar_Mpe_Productos_Click" />
                        </td>
                    </tr>        
                    <tr>
                        <td width="18%">
                            <asp:Label ID="Lbl_Clase_Activo" runat="server" Text="Clase Activo"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="Cmb_Clase_Activo" runat="server" Width="100%">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td width="18%">
                            <asp:Label ID="Lbl_Tipo_Activo" runat="server" Text="Tipo Activo"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="Cmb_Tipo_Activo" runat="server" Width="100%">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:18%;">
                            <asp:Label ID="Lbl_Proveedor" runat="server" Text="Proveedor"></asp:Label>
                                        <asp:HiddenField ID="Hdf_Proveedor_ID" runat="server" />
                        </td>
                        <td  colspan="3">
                            <asp:TextBox ID="Txt_Nombre_Proveedor" runat="server" Width="93%"  Enabled="false"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Lanzar_Mpe_Proveedores" runat="server" OnClick ="Btn_Lanzar_Mpe_Proveedores_Click" 
                                 ImageUrl="~/paginas/imagenes/paginas/busqueda.png"  
                                 ToolTip="Busqueda y Selección de Proveedor" AlternateText="Buscar" />
                       </td>
                    </tr> 
                    <tr>
                        <td style="width:18%; text-align:left; ">
                            <asp:Label ID="Lbl_Dependencia" runat="server" Text="* Unidad Responsable"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="Cmb_Dependencias" runat="server" Width="100%" onselectedindexchanged="Cmb_Dependencias_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>   
                    <tr>
                        <td style="width:18%; text-align:left;"><asp:Label ID="Lbl_Procedencia_Bien" runat="server"  Text="* Procedencia" ></asp:Label></td>
                        <td colspan="3" align="left">
                            <asp:DropDownList ID="Cmb_Procedencia" runat="server" Width="100%">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>       
                    <tr>
                        <td colspan="4"><hr /></td>
                    </tr>                           
                    <tr>
                        <td style="text-align:left; width:18%;">
                            <asp:Label ID="Lbl_RFC_Donador" runat="server" Text="Donador"></asp:Label>
                            <asp:HiddenField runat="server"  ID="Hdf_Donador_ID" />
                        </td>
                        <td style="text-align:left; width:32%;">
                            <asp:TextBox ID="Txt_RFC_Donador" runat="server" Width="70%" ></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_RFC_Donador" runat="server" Visible="false" ImageUrl="~/paginas/imagenes/paginas/busqueda.png"  AlternateText="Buscar" 
                                onclick="Btn_Buscar_RFC_Donador_Click" Width="16px" />
                            <asp:ImageButton ID="Btn_Agregar_Donador" runat="server"  Visible="false" AlternateText="Buscar" ImageUrl="~/paginas/imagenes/paginas/sias_add.png" 
                                onclick="Btn_Agregar_Donador_Click" Width="16px" />
                            <asp:ImageButton ID="Btn_Limpiar_Donador" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Height="16px" Width="16px" CssClass="Img_Button" AlternateText="Limpiar Donador" OnClientClick="javascript:return Limpiar_Ctlr_Donador();" ToolTip="Limpiar Donador"/>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_RFC_Donador" runat="server" 
                                Enabled="True" TargetControlID="Txt_RFC_Donador" WatermarkCssClass="watermarked" 
                                WatermarkText="RFC de Donador">
                            </cc1:TextBoxWatermarkExtender>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="Txt_Nombre_Donador" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>                               
                </table>
                <br /> 
                <table width="98%" class="estilo_fuente">  
                    <tr>
                        <td class="label_titulo" colspan="4">Detalles del Animal</td>
                    </tr>        
                    <tr align="right" class="barra_delgada">
                        <td colspan="4" align="center">
                        </td>
                    </tr>
                </table> 
                <table width="98%" class="estilo_fuente">  

                    <tr>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Numero_Inventario" runat="server" Text="No. Inventario" ></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:TextBox ID="Txt_Numero_Inventario" runat="server" Enabled="false" Width="97%" ForeColor="Green" Font-Bold="true"></asp:TextBox>                                          
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Numero_Inventario" runat="server" TargetControlID ="Txt_Numero_Inventario" WatermarkText="<AUTOMATICO>" WatermarkCssClass="watermarked" Enabled="True"/>                                                        
                        </td>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Inventario_Anterior" runat="server" Text="Inventario Anterior" ></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:TextBox ID="Txt_Inventario_Anterior" runat="server" Width="97%" ></asp:TextBox>  
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Inventario_Anterior" runat="server" TargetControlID="Txt_Inventario_Anterior" InvalidChars="<,>,&,',!," 
                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_/" Enabled="True">
                            </cc1:FilteredTextBoxExtender>                                         
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Tipo_Cemoviente" runat="server" Text="* Tipo Animal" ></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:DropDownList ID="Cmb_Tipo_Cemoviente" runat="server" Width="100%" AutoPostBack="True" onselectedindexchanged="Cmb_Tipo_Cemoviente_SelectedIndexChanged">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:20%; text-align:left;">
                            <asp:Label ID="Lbl_Nombre" runat="server" Text="* Nombre" ></asp:Label>
                        </td>
                        <td style="width:30%" align="left">
                            <asp:TextBox ID="Txt_Nombre" runat="server" Width="97%" Enabled="False"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre" runat="server" TargetControlID="Txt_Nombre" InvalidChars="<,>,&,',!," 
                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                            </cc1:FilteredTextBoxExtender> 
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Razas" runat="server" Text="* Raza" ></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:DropDownList ID="Cmb_Razas" runat="server" Width="100%">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Color" runat="server" Text="* Color" ></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:DropDownList ID="Cmb_Colores" runat="server" Width="100%">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>                          
                    <tr>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Tipo_Adiestramiento" runat="server" Text="* Adiestramiento" ></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:DropDownList ID="Cmb_Tipo_Adiestramiento" runat="server" Width="100%">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Funciones" runat="server" Text="* Función" ></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:DropDownList ID="Cmb_Funciones" runat="server" Width="100%">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>                              
                    <tr>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Fecha_Nacimiento" runat="server" Text="* Nacimiento" ></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:TextBox ID="Txt_Fecha_Nacimiento" runat="server" Width="85%"  Enabled="false"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Fecha_Nacimiento" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Nacimiento" runat="server" TargetControlID="Txt_Fecha_Nacimiento" PopupButtonID="Btn_Fecha_Nacimiento" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Fecha_Adquisicion" runat="server" Text="* Fecha Adquisición" ></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:TextBox ID="Txt_Fecha_Adquisicion" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Fecha_Adquisicion" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Adquisicion" runat="server" TargetControlID="Txt_Fecha_Adquisicion" PopupButtonID="Btn_Fecha_Adquisicion" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>                                
                    <tr>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Tipos_Alimentacion" runat="server" Text="* Alimentación" ></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:DropDownList ID="Cmb_Tipos_Alimentacion" runat="server" Width="100%">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Tipos_Ascendencia" runat="server" Text="* Ascendencia" ></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:DropDownList ID="Cmb_Tipos_Ascendencia" runat="server" Width="100%" AutoPostBack="True" onselectedindexchanged="Cmb_Tipos_Ascendencia_SelectedIndexChanged">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                <asp:ListItem Value="DESCONOCIDO">OTRA</asp:ListItem>
                                <asp:ListItem Value="GOBIERNO">GOBIERNO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>                              
                    <tr>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Padre" runat="server" Text="Padre" ></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:DropDownList ID="Cmb_Padre" runat="server" Width="100%">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Madre" runat="server" Text="Madre" ></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:DropDownList ID="Cmb_Madre" runat="server" Width="100%">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>                   
                    <tr>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Costo" runat="server" Text="* Costo [$]"></asp:Label>
                            <asp:RegularExpressionValidator ID="REV_Txt_Costo" runat="server" ErrorMessage="[No Valido. Verificar]" ControlToValidate="Txt_Costo" ValidationExpression="^\d+(\.\d\d)?$" Font-Size="X-Small" ></asp:RegularExpressionValidator>
                        </td>
                        <td style="width:30%">
                            <asp:TextBox ID="Txt_Costo" runat="server" Enabled="true" Width="96%"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Costo" runat="server" Enabled="True" FilterType="Numbers, Custom" ValidChars="." TargetControlID="Txt_Costo">
                            </cc1:FilteredTextBoxExtender>   
                        </td>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_No_Factura" runat="server" Text="No. Factura" ></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:TextBox ID="Txt_No_Factura" runat="server" Width="96%" Enabled="False"></asp:TextBox>
                           <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Factura" runat="server" TargetControlID="Txt_No_Factura"
                                InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_/" Enabled="True">   
                            </cc1:FilteredTextBoxExtender> 
                        </td>
                    </tr>            
                    <tr>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Sexo" runat="server" Text="* Sexo" ></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:DropDownList ID="Cmb_Sexo" runat="server" Width="100%">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                <asp:ListItem Value="HEMBRA">HEMBRA</asp:ListItem>
                                <asp:ListItem Value="MACHO">MACHO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:20%; text-align:left; ">
                            <asp:Label ID="Lbl_Estatus" runat="server" Text="* Estatus" ></asp:Label>
                        </td>
                        <td style="width:30%">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="100%" Enabled="False">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                <asp:ListItem>VIGENTE</asp:ListItem>
                                <asp:ListItem>BAJA</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>                            
                    <tr>
                        <td style="text-align:left; vertical-align:top; width:20%;">
                            <asp:Label ID="Lbl_Observaciones" runat="server" Text="Observaciones" ></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Observaciones" runat="server" Width="99%" Enabled="False" Rows="2" TextMode="MultiLine"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Observaciones" runat="server" 
                                                            TargetControlID="Txt_Observaciones" InvalidChars="<,>,&,',!," 
                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_/" Enabled="True">   
                            </cc1:FilteredTextBoxExtender>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observaciones" runat="server" TargetControlID ="Txt_Observaciones" 
                                WatermarkText="Límite de Caractes 255" WatermarkCssClass="watermarked" Enabled="True"/>     
                        </td>
                    </tr>                         
                    <tr>
                        <td style="text-align:left; width:20%">
                            <asp:Label ID="Lbl_Archivo" runat="server" Text="Archivo" ></asp:Label>
                            <asp:Label ID="Throbber" Text="wait" runat="server"  Width="30px">                                                                     
                               <div id="Div1" class="progressBackgroundFilter"></div>
                                   <div  class="processMessage" id="div2">
                                        <img alt="" src="../imagenes/paginas/Updating.gif" />
                                    </div>
                            </asp:Label>  
                        </td>
                        <td colspan="3">                      
                            <cc1:AsyncFileUpload ID="AFU_Archivo" runat="server" ThrobberID="Throbber" ForeColor="White" Font-Bold="true" CompleteBackColor="LightBlue" 
                                ErrorBackColor="Red" UploadingBackColor="LightGray" Width="600px" />
                            &nbsp;&nbsp;
                            <asp:ImageButton ID="Btn_Limpiar_FileUpload" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Width="24px" CssClass="Img_Button"
                                        AlternateText="Limpiar Archivo" OnClick="Btn_Limpiar_FileUpload_Click" ToolTip="Limpiar Archivo"/>
                        </td>
                    </tr>               
                </table>
                <br />
                <table width="98%">
                    <tr>
                        <td class="label_titulo" colspan="4">Vacunas</td>
                    </tr>         
                    <tr align="right" class="barra_delgada">
                        <td colspan="4" align="center">
                        </td>
                    </tr>                            
                    <tr>
                        <td style="width:20%; text-align:left;">
                            <asp:Label ID="Lbl_Vacunas" runat="server" Text="Vacunas" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width:30%; text-align:left;">
                            <asp:DropDownList ID="Cmb_Vacunas" runat="server" Width="98%">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:20%; text-align:left;">
                            <asp:Label ID="Lbl_Fecha_Aplicacion" runat="server" Text="Fecha de Aplicación" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td style="width:30%; text-align:left;">
                            <asp:TextBox ID="Txt_Fecha_Aplicacion" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Fecha_Aplicacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Aplicacion" runat="server" TargetControlID="Txt_Fecha_Aplicacion" PopupButtonID="Btn_Fecha_Aplicacion" Format="dd/MMM/yyyy">
                            </cc1:CalendarExtender>                   
                        </td>
                    </tr>                 
                    <tr>
                        <td style="width:20%; text-align:left;">
                            <asp:Label ID="Lbl_Veterinario_Vacuno" runat="server" Text="Aplicó" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td  colspan="3" style="text-align:left;">
                            <asp:DropDownList ID="Cmb_Veterinario_Vacuno" runat="server" Width="98%">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%; text-align:left; vertical-align:top;">
                            <asp:Label ID="Lbl_Comentarios_Vacuna" runat="server" Text="Comentarios" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td  colspan="3" style="text-align:left;">
                            <asp:TextBox ID="Txt_Comentarios_Vacuna" runat="server" TextMode="MultiLine" Rows="3" Width="98%"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Vacuna" runat="server" TargetControlID ="Txt_Comentarios_Vacuna" WatermarkText="Límite de Caractes 255" WatermarkCssClass="watermarked" Enabled="True"/>                                                        
                        </td>
                    </tr>                                    
                    <tr>
                        <td colspan="3">&nbsp;</td>
                        <td style="width:30%; text-align:right;">
                            <asp:ImageButton ID="Btn_Agregar_Vacuna" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_add.png"  AlternateText="Agregar" onclick="Btn_Agregar_Vacuna_Click" />
                            <asp:ImageButton ID="Btn_Quitar_Vacuna" runat="server" ImageUrl="~/paginas/imagenes/paginas/quitar.png"  AlternateText="Quitar" onclick="Btn_Quitar_Vacuna_Click"/>
                        </td>  
                    </tr>
                </table>
                <br />
                <asp:GridView ID="Grid_Vacunas" runat="server" 
                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                    GridLines="None" AllowPaging="True" Width="98%"
                    OnRowDataBound="Grid_Vacunas_RowDataBound"
                    PageSize="5" onpageindexchanging="Grid_Vacunas_PageIndexChanging" >
                    <RowStyle CssClass="GridItem" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                            <ItemStyle Width="30px" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="VACUNA_ID" HeaderText="VACUNA_ID" SortExpression="VACUNA_ID">
                            <ItemStyle Width="110px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="VETERINARIO_ID" HeaderText="VETERINARIO_ID"  SortExpression="VETERINARIO_ID">
                            <ItemStyle Width="110px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="VACUNA_NOMBRE" HeaderText="Vacuna" SortExpression="VACUNA_NOMBRE">
                            <ItemStyle HorizontalAlign="Center" Font-Size="X-Small" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FECHA_APLICACION" HeaderText="Aplicación" SortExpression="FECHA_APLICACION" DataFormatString="{0:dd/MMM/yyyy}" >
                            <ItemStyle HorizontalAlign="Center" Font-Size="X-Small" Width="90px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="VETERINARIO_NOMBRE" HeaderText="Aplicó" SortExpression="VETERINARIO_NOMBRE">
                            <ItemStyle HorizontalAlign="Center" Font-Size="X-Small" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="Btn_Ver_Informacion_Vacuna" runat="server" Width="16px" ImageUrl="~/paginas/imagenes/gridview/grid_info.png" ToolTip="Comentarios del Resguardo" OnClick="Btn_Ver_Informacion_Vacuna_Click" />
                            </ItemTemplate>
                            <ItemStyle Width="50px" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="GridHeader" />
                    <SelectedRowStyle CssClass="GridSelected" />
                    <HeaderStyle CssClass="GridHeader" />                                
                    <AlternatingRowStyle CssClass="GridAltItem" />                                
                </asp:GridView>
                <br />
                <table width="98%">
                    <tr>
                        <td class="label_titulo" colspan="4">Resguardos</td>
                    </tr>         
                    <tr align="right" class="barra_delgada">
                        <td colspan="4" align="center">
                        </td>
                    </tr>                            
                    <tr>
                        <td style="width:20%; text-align:left;">
                            <asp:Label ID="Lbl_Resguardantes" runat="server" Text="Empleados" 
                                CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td  colspan="3" style="text-align:left;">
                            <asp:DropDownList ID="Cmb_Empleados" runat="server" Width="98%">
                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%; text-align:left; vertical-align:top;">
                            <asp:Label ID="Lbl_Comentarios" runat="server" Text="Comentarios" CssClass="estilo_fuente"></asp:Label>
                        </td>
                        <td  colspan="3" style="text-align:left;">
                            <asp:TextBox ID="Txt_Comentarios" runat="server" TextMode="MultiLine" Rows="3" 
                                Width="98%"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Cometarios" runat="server" TargetControlID ="Txt_Comentarios" 
                                WatermarkText="Límite de Caractes 150" WatermarkCssClass="watermarked" 
                                Enabled="True"/>                                                        
                        </td>
                    </tr>    
                    <tr>
                        <td colspan="4"><hr /></td>                        
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
                        <td colspan="4"><hr /></td>                        
                    </tr>  
                </table>
                <br />
                <asp:GridView ID="Grid_Resguardantes" runat="server" AutoGenerateColumns="False"
                    CellPadding="4" ForeColor="#333333" GridLines="None" AllowPaging="false" Width="98%"
                    onrowdatabound="Grid_Resguardantes_RowDataBound" >
                    <RowStyle CssClass="GridItem" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                            <ItemStyle Width="30px" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="BIEN_RESGUARDO_ID" HeaderText="BIEN_RESGUARDO_ID" SortExpression="BIEN_RESGUARDO_ID">
                            <ItemStyle Width="110px" />
                        </asp:BoundField>
                         <asp:BoundField DataField="EMPLEADO_ID" HeaderText="EMPLEADO_ID" SortExpression="EMPLEADO_ID" >
                             <HeaderStyle HorizontalAlign="Center" />
                             <ItemStyle HorizontalAlign="Right" Width="40px" Font-Size="X-Small" />
                         </asp:BoundField>
                         <asp:BoundField DataField="NO_EMPLEADO" HeaderText="No. Empleado" SortExpression="NO_EMPLEADO">
                             <HeaderStyle HorizontalAlign="Center" />
                             <ItemStyle Width="100px" HorizontalAlign="Left" Font-Size="X-Small" />
                         </asp:BoundField>
                         <asp:BoundField DataField="NOMBRE_EMPLEADO" HeaderText="Nombre del Empleado" SortExpression="NOMBRE_EMPLEADO">
                             <HeaderStyle HorizontalAlign="Center" />
                             <ItemStyle HorizontalAlign="Left" Font-Size="X-Small" />
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
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    
     <asp:UpdatePanel ID="UpPnl_Pnl_Mpe_Productos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_Mpe_Productos_Cabecera" runat="server" Text="Button" style="display:none;"/> 
            <cc1:ModalPopupExtender ID="Mpe_Productos_Cabecera" runat="server" 
                TargetControlID="Btn_Comodin_Mpe_Productos_Cabecera" PopupControlID="Pnl_Mpe_Productos" 
                CancelControlID="Btn_Cerrar_Mpe_Productos" PopupDragHandleControlID="Pnl_Mpe_Productos_Interno"
                DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/> 
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID="UpPnl_Pnl_Mpe_Donador" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin" runat="server" Text="Button" style="display:none;"/> 
            <cc1:ModalPopupExtender ID="MPE_Donadores" runat="server" TargetControlID="Btn_Comodin"
                PopupControlID="Pnl_Donadores" CancelControlID="Btn_Cerrar_Ventana_Cancelacion"
                PopupDragHandleControlID="Pnl_Cabecera" DropShadow="True" BackgroundCssClass="progressBackgroundFilter" />   
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
    
    <asp:UpdatePanel ID="UpPnl_aux_Busqueda_Resguardante" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_MPE_Resguardante" runat="server" Text="" style="display:none;"/>
                <cc1:ModalPopupExtender ID="MPE_Resguardante" runat="server" 
                TargetControlID="Btn_Comodin_MPE_Resguardante" PopupControlID="Pnl_Busqueda_Contenedor" 
                CancelControlID="Btn_Cerrar_Ventana" PopupDragHandleControlID="Pnl_Busqueda_Resguardante_Cabecera"
                DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>  
        </ContentTemplate>
    </asp:UpdatePanel>   
    
    <asp:Panel ID="Pnl_Mpe_Productos" runat="server" CssClass="drag" HorizontalAlign="Center" style="display:none;border-style:outset;border-color:Silver;width:760px;">                         
    <asp:Panel ID="Pnl_Mpe_Productos_Interno" runat="server" CssClass="estilo_fuente"
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table class="estilo_fuente" width="100%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;width:90%">
                   <asp:Image ID="Img_Mpe_Productos" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Busqueda y Selección de Productos
                </td>
                <td align="right" colspan="3">
                   <asp:ImageButton ID="Btn_Cerrar_Mpe_Productos" CausesValidation="false" runat="server" style="cursor:pointer;" 
                        ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
     </asp:Panel>
       <center>
        <asp:UpdatePanel ID="UpPnl_Mpe_Productos" runat="server">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpPgr_Mpe_Productos" runat="server" AssociatedUpdatePanelID="UpPnl_Mpe_Productos" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
                </asp:UpdateProgress>
                    <br />
                    <br />
                    <div style="border-style: outset; width: 95%; height: 380px; background-color: White;">
                        <table width="98%">
                            <tr>
                                <td style="width:20%; text-align:left;">
                                    <asp:Label ID="Lbl_Nombre_Producto_Buscar" runat="server" CssClass="estilo_fuente" Text="Nombre" style="font-weight:bolder;" ></asp:Label>
                                </td>
                                <td style="width:85%; text-align:left;">
                                    <asp:TextBox ID="Txt_Nombre_Producto_Buscar" runat="server" Width="90%" AutoPostBack="true"  ontextchanged="Txt_Nombre_Producto_Buscar_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Producto_Buscar" runat="server" TargetControlID="Txt_Nombre_Producto_Buscar" InvalidChars="<,>,&,',!," 
                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                                    </cc1:FilteredTextBoxExtender>  
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Nombre_Producto_Buscar" runat="server" Enabled="True" TargetControlID="Txt_Nombre_Producto_Buscar" WatermarkCssClass="watermarked" 
                                                     WatermarkText="<-- TODOS -->">
                                    </cc1:TextBoxWatermarkExtender>
                                    <asp:ImageButton ID="Btn_Ejecutar_Busqueda_Productos" runat="server" OnClick ="Btn_Ejecutar_Busqueda_Productos_Click" 
                                         ImageUrl="~/paginas/imagenes/paginas/busqueda.png"  
                                         ToolTip="Buscar Productos" AlternateText="Buscar" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:GridView ID="Grid_Listado_Productos" runat="server" AllowPaging="True" 
                                 AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                 GridLines="None" OnPageIndexChanging="Grid_Listado_Productos_PageIndexChanging" 
                                 OnSelectedIndexChanged="Grid_Listado_Productos_SelectedIndexChanged"
                                 PageSize="5" Width="97.5%" CssClass="GridView_1">
                                 <AlternatingRowStyle CssClass="GridAltItem" />
                                 <Columns>
                                     <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                         ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                         <ItemStyle Width="30px" />
                                     </asp:ButtonField>
                                     <asp:BoundField DataField="PRODUCTO_ID" HeaderText="Producto ID" SortExpression="PRODUCTO_ID"  >
                                         <ItemStyle Width="30px" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="CLAVE_PRODUCTO" HeaderText="Clave" SortExpression="CLAVE_PRODUCTO">
                                        <ItemStyle Width="70px" HorizontalAlign="Center" Font-Size="X-Small"/>
                                     </asp:BoundField>
                                     <asp:BoundField DataField="NOMBRE_PRODUCTO" HeaderText="Nombre del Producto" SortExpression="NOMBRE_PRODUCTO" >
                                        <ItemStyle HorizontalAlign="Center" Font-Size="X-Small"/>
                                     </asp:BoundField>
                                     <asp:BoundField DataField="DESCRIPCION_PRODUCTO" HeaderText="Descripcion" SortExpression="DESCRIPCION_PRODUCTO">
                                        <ItemStyle HorizontalAlign="Center" Font-Size="X-Small"/>
                                     </asp:BoundField>
                                 </Columns>
                                 <HeaderStyle CssClass="GridHeader" />
                                 <PagerStyle CssClass="GridHeader" />
                                 <RowStyle CssClass="GridItem" />
                                 <SelectedRowStyle CssClass="GridSelected" />
                             </asp:GridView>
                        <br />
                    </div>
                    <br />
                    <br />
                </ContentTemplate>
            </asp:UpdatePanel>
       </center>          
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
                                    <asp:Label ID="Lbl_Nombre_Proveedores_Buscar" runat="server" CssClass="estilo_fuente" Text="Nombre" style="font-weight:bolder;" ></asp:Label>
                                </td>
                                <td style="width:85%; text-align:left;">
                                    <asp:TextBox ID="Txt_Nombre_Proveedor_Buscar" runat="server" Width="92%" AutoPostBack="true" ontextchanged="Txt_Nombre_Proveedor_Buscar_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Proveedor_Buscar" runat="server" TargetControlID="Txt_Nombre_Proveedor_Buscar" InvalidChars="<,>,&,',!," 
                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                                    </cc1:FilteredTextBoxExtender>  
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Nombre_Proveedor_Buscar" runat="server" Enabled="True" TargetControlID="Txt_Nombre_Proveedor_Buscar" WatermarkCssClass="watermarked" 
                                                     WatermarkText="<-- Razón Social ó Nombre Comercial -->">
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
                                       <asp:GridView ID="Grid_Listado_Proveedores" runat="server" AllowPaging="true"
                                         AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                         GridLines="None" OnPageIndexChanging="Grid_Listado_Proveedores_PageIndexChanging"
                                         OnSelectedIndexChanged="Grid_Listado_Proveedores_SelectedIndexChanged"
                                         PageSize="150" Width="100%" CssClass="GridView_1">
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
    
    <asp:Panel ID="Pnl_Donadores" runat="server" CssClass="drag" HorizontalAlign="Center" style="display:none;border-style:outset;border-color:Silver;width:760px;">                         
    <asp:Panel ID="Pnl_Cabecera" runat="server" CssClass="estilo_fuente"
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table class="estilo_fuente" width="100%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;width:90%">
                   <asp:Image ID="Image1" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Introducir los datos del Donador
                </td>
                <td align="right" colspan="3">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana_Cancelacion" CausesValidation="false" runat="server" style="cursor:pointer;" 
                        ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
     </asp:Panel>
       <center>
        <asp:UpdatePanel ID="Upl_Pnl_Donadores" runat="server">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpPrgr_Donadores" runat="server" AssociatedUpdatePanelID="Upl_Pnl_Donadores" DisplayAfter="0">
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
                                <td colspan="2">
                                    <table width="99%">
                                        <tr>
                                            <td style="width: 20%; text-align: left;">
                                                <asp:Label ID="Lbl_Nombre_Donador_MPE" runat="server" Text="* Nombre" CssClass="estilo_fuente" style="font-weight:bolder;"></asp:Label>
                                            </td>
                                            <td colspan="3" style="width: 82%; text-align: left;">
                                                <asp:TextBox ID="Txt_Nombre_Donador_MPE" runat="server" Width="99%" MaxLength="50"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Donador_MPE" runat="server" TargetControlID="Txt_Nombre_Donador_MPE"
                                                    InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%; text-align: left;">
                                                <asp:Label ID="Lbl_Apellido_Paterno_Donador" runat="server" Text="A. Paterno"
                                                    CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 30%; text-align: left;">
                                                <asp:TextBox ID="Txt_Apellido_Paterno_Donador" runat="server" Width="98%" MaxLength="50"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Apellido_Paterno_Donador" runat="server"
                                                    TargetControlID="Txt_Apellido_Paterno_Donador" InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 20%; text-align: left;">
                                                <asp:Label ID="Lbl_Apellido_Materno_Donador" runat="server" Text="A. Materno"
                                                    CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 30%; text-align: left;">
                                                <asp:TextBox ID="Txt_Apellido_Materno_Donador" runat="server" Width="98%" MaxLength="50"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Apellido_Materno_Donador" runat="server"
                                                    TargetControlID="Txt_Apellido_Materno_Donador" InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%; text-align: left;">
                                                <asp:Label ID="Lbl_Direccion_Donador" runat="server" Text="Dirección" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td colspan="3" style="width: 82%; text-align: left;">
                                                <asp:TextBox ID="Txt_Direccion_Donador" runat="server" Width="99%" MaxLength="150"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Direccion_Donador" runat="server" TargetControlID="Txt_Direccion_Donador"
                                                    InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%; text-align: left;">
                                                <asp:Label ID="Lbl_Ciudad_Donador" runat="server" Text="Ciudad" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 30%; text-align: left;">
                                                <asp:TextBox ID="Txt_Ciudad_Donador" runat="server" Width="98%" MaxLength="50"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Ciudad_Donador" runat="server" TargetControlID="Txt_Ciudad_Donador"
                                                    InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 20%; text-align: left;">
                                                <asp:Label ID="Lbl_Estado_Donador" runat="server" Text="Estado" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 30%; text-align: left;">
                                                <asp:TextBox ID="Txt_Estado_Donador" runat="server" Width="98%" MaxLength="50"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Estado_Donador" runat="server" TargetControlID="Txt_Estado_Donador"
                                                    InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%; text-align: left;">
                                                <asp:Label ID="Lbl_Telefono_Donador" runat="server" Text="Telefono" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 30%; text-align: left;">
                                                <asp:TextBox ID="Txt_Telefono_Donador" runat="server" Width="98%" MaxLength="20"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Telefono_Donador" runat="server" TargetControlID="Txt_Telefono_Donador"
                                                    InvalidChars="<,>,&,',!," FilterType="Custom, Numbers" ValidChars="()-">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 20%; text-align: left;">
                                                <asp:Label ID="Lbl_Celular_Donador" runat="server" Text="Celular" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 30%; text-align: left;">
                                                <asp:TextBox ID="Txt_Celular_Donador" runat="server" Width="98%" MaxLength="20"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Celular_Donador" runat="server" TargetControlID="Txt_Celular_Donador"
                                                    InvalidChars="<,>,&,',!," FilterType="Custom, Numbers" ValidChars="()-">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%; text-align: left;">
                                                <asp:Label ID="Lbl_CURP_Donador" runat="server" Text="CURP" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 30%; text-align: left;">
                                                <asp:TextBox ID="Txt_CURP_Donador" runat="server" Width="98%" MaxLength="18"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_CURP_Donador" runat="server" TargetControlID="Txt_CURP_Donador"
                                                    InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, Numbers" ValidChars="Ññ">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 20%; text-align: left;">
                                                <asp:Label ID="Lbl_RFC_Donador_MPE" runat="server" Text="* RFC" CssClass="estilo_fuente" style="font-weight:bolder;"></asp:Label>
                                            </td>
                                            <td style="width: 30%; text-align: left;">
                                                <asp:TextBox ID="Txt_RFC_Donador_MPE" runat="server" Width="98%" MaxLength="15"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_RFC_Donador_MPE" runat="server" TargetControlID="Txt_RFC_Donador_MPE"
                                                    InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, Numbers" ValidChars="Ññ">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; text-align: left;">
                                    &nbsp;
                                </td>
                                <td style="width: 80%; text-align: right;">
                                    <asp:ImageButton ID="Btn_MPE_Donador_Aceptar" runat="server" ImageUrl="~/paginas/imagenes/paginas/accept.png"
                                        CausesValidation="False" ToolTip="Aceptar" OnClick="Btn_MPE_Donador_Aceptar_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="Div_MPE_Donadores_Mensaje_Error" style="width: 98%;" runat="server" visible="false">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <asp:ImageButton ID="Btn_MPE_Donadores_Mensaje_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png"
                                                        Enabled="false" Width="24px" Height="24px" />
                                                    <asp:Label ID="Lbl_MPE_Donadores_Cabecera_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 10%;">
                                                </td>
                                                <td style="width: 90%; text-align: left;" valign="top">
                                                    <asp:Label ID="Lbl_MPE_Donadores_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <br />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Btn_Agregar_Donador" EventName="Click" />
                </Triggers>
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
                                                TargetControlID="Txt_Busqueda_Nombre_Empleado" ValidChars="áéíóúÁÉÍÓÚ "/>
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
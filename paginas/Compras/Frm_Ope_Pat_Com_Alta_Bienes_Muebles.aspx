<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" 
AutoEventWireup="true" CodeFile="Frm_Ope_Pat_Com_Alta_Bienes_Muebles.aspx.cs" 
Inherits="paginas_predial_Frm_Ope_Pat_Com_Alta_Bienes_Muebles" 
Title="Operación - Asignación de Bienes Muebles" %>

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
    <asp:ScriptManager ID="ScriptManager1" runat="server"  EnableScriptGlobalization ="true" EnableScriptLocalization = "True">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
            <ProgressTemplate>
                <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
            </ProgressTemplate>      
        </asp:UpdateProgress>
        <div id="Div_General" style="background-color:#ffffff; width:98%;"> <%--Div General--%>
            <table  border="0" cellspacing="0" class="estilo_fuente" frame="border" style="width:98%;">
                <tr align="center">
                    <td class="label_titulo">Asignaci&oacute;n de Bienes Muebles</td>
                </tr>
                <tr>
                    <td>
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px" onclick="IBtn_Imagen_Error_Click"/>
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
                    <td>&nbsp;</td>                        
                </tr>
                <tr class="barra_busqueda" style="width:98%;" >
                    <td align="left" style="Width:100%">
                        <asp:ImageButton ID="Btn_Nuevo" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" CssClass="Img_Button" AlternateText="Nuevo" OnClick="Btn_Nuevo_Click"/>
                        <asp:ImageButton ID="Btn_Generar_Reporte" runat="server" ToolTip="Ver Resguardo" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" CssClass="Img_Button" AlternateText="Ver Resguardo" onclick="Btn_Generar_Reporte_Click" />
                        <asp:ImageButton ID="Btn_Salir" runat="server" CausesValidation="false" ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button" AlternateText="Salir" onclick="Btn_Salir_Click"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="Div_Datos_Generales" runat="server" style="width:97%;"> <%--div Datos Generales--%>
                            <table width="100%">
                                <tr>
                                    <td class="label_titulo" colspan="4">Datos Generales<br /> </td>
                                </tr> 
                                <tr align="right" class="barra_delgada">
                                    <td align="center" colspan="4"></td>
                                </tr>
                                <tr>
                                    <td width="15%">
                                        <asp:HiddenField ID="Hdf_Producto_ID" runat="server" />
                                        <asp:HiddenField ID="Hdf_Proveedor_ID" runat="server" />
                                        <asp:Label ID="Lbl_Producto_ID" runat="server" Text="Producto ID"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Producto_ID" runat="server" Enabled="False" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Lbl_Nombre_Producto" runat="server" Text="Nombre"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Nombre_Producto" runat="server" Enabled="False" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Lbl_Marca_Producto" runat="server" Text="Marca"></asp:Label>
                                    </td>
                                    <td width="30%">
                                        <asp:TextBox ID="Txt_Marca_Producto" runat="server" Enabled="False" Width="98%"></asp:TextBox>
                                    </td>
                                    <td width="20%">
                                        <asp:Label ID="Lbl_Modelo_Producto" runat="server" Text="Modelo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Modelo_Producto" runat="server" Enabled="false" Width="98%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Lbl_Proveedor_Producto" runat="server" Text="Proveedor"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Proveedor_Producto" runat="server" Enabled="False" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td> 
                                        <asp:Label ID="Lbl_Clave_Producto" runat="server" Text="Clave"></asp:Label>
                                    </td>
                                    <td width="30%">
                                        <asp:TextBox ID="Txt_Clave_Producto" runat="server" Enabled="False" 
                                            Width="97.5%"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Lbl_Costo_Producto" runat="server" Text="Costo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_Costo_Producto" runat="server" Enabled="False" Height="22px" Width="98%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>   
        <br />
        <div id="Div_Generales_Otra_Procedencia" runat="server" style="width:97%;"> <%--Div Otra prosedencia--%> 
            <table width="100%" class="estilo_fuente" >  <%-- Tabla Otra procedencia--%>     
                <tr>
                    <td class="label_titulo" colspan="4">Generales<br /></td>
                </tr>  
                <tr align="right" class="barra_delgada">
                    <td colspan="4" align="center">
                    </td>
                </tr>
                <tr>
                    <td style="text-align:left; width:18%;">
                        <asp:Label ID="Lbl_Operación" runat="server" Text="* Operación" ></asp:Label>
                    </td>
                    <td style="width:32%">
                        <asp:DropDownList ID="Cmb_Operacion" runat="server" Width="100%" Enabled="false">
                            <asp:ListItem Value="RECIBO">RECIBO</asp:ListItem>
                            <asp:ListItem Value="RESGUARDO">RESGUARDO</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr> 
                    <td style="width:18%;">
                        <asp:HiddenField ID="Hdf_Bien_Mueble_ID" runat="server" />
                        <asp:Label ID="Lbl_Producto" runat="server" Text="* Producto"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="Txt_Nombre_Producto_Donado" runat="server" Width="93%" Enabled="false"></asp:TextBox>
                        <asp:ImageButton ID="Btn_Lanzar_Mpe_Productos" runat="server" OnClick="Btn_Lanzar_Mpe_Productos_Click"
                             ImageUrl="~/paginas/imagenes/paginas/busqueda.png"  ToolTip="Busqueda y Selección de Producto" 
                             AlternateText="Buscar" />
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
                     <td width="18%">
                        <asp:Label ID="Lbl_Dependencia" runat="server" Text="* Unidad Responsable"></asp:Label>
                    </td>
                     <td colspan="3">
                        <asp:TextBox ID="Txt_Dependencia" runat="server" visible="false" Width="98%"  Enabled="false"></asp:TextBox>
                        <asp:DropDownList ID="Cmb_Dependencias" runat="server" Width="100%" Visible="false"  onselectedindexchanged="Cmb_Dependencias_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:18%; text-align:left; ">
                        <asp:Label ID="Lbl_Zona" runat="server" Text="Zona" ></asp:Label>
                    </td>
                    <td colspan="3">
                         <asp:DropDownList ID="Cmb_Zonas" runat="server" Width="100%" >
                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="18%">
                        <asp:Label ID="Lbl_Numero_Serie" runat="server" Text="* No. Serie"></asp:Label>
                    </td>
                    <td style="width:32%;">
                        <asp:TextBox ID="Txt_Numero_Serie" runat="server" Enabled="true" Width="98%" MaxLength="49"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Serie" runat="server" 
                            Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                            InvalidChars="'" TargetControlID="Txt_Numero_Serie" 
                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ ">
                        </cc1:FilteredTextBoxExtender>
                    </td>
                    <td width="18%">
                        &nbsp;&nbsp;
                        <asp:Label ID="Lbl_Marca" runat="server" Text="* Marca"></asp:Label>
                    </td>
                    <td width="32%">
                         <asp:DropDownList ID="Cmb_Marca" runat="server" Width="100%">
                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="18%">
                        <asp:Label ID="Lbl_Modelo" runat="server" Text="* Modelo"></asp:Label>
                    </td>
                     <td colspan="3">
                        <asp:TextBox ID="Txt_Modelo" runat="server"  Width="99%" ></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Modelo" runat="server" 
                            FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                            TargetControlID="Txt_Modelo" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/$# ">
                        </cc1:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td width="18%">
                        <asp:Label ID="Lbl_Material" runat="server" Text="* Material"></asp:Label>
                    </td>
                    <td width="32%">
                        <asp:DropDownList ID="Cmb_Materiales" runat="server" Width="100%">
                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="18%">
                        &nbsp;&nbsp;
                        <asp:Label ID="Lbl_Color" runat="server" Text="* Color"></asp:Label>
                    </td>
                    <td width="32%">
                        <asp:DropDownList ID="Cmb_Colores" runat="server" Width="100%">
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
        </div>
        <br />
        <div id="Div_Detalles" runat="server" style="width:97%;"> 
            <table width="100%" class="estilo_fuente">
                <tr>
                    <td class="label_titulo" colspan="4">Detalles</td>
                </tr>    
               <tr align="right" class="barra_delgada">
                    <td colspan="4" align="center">
                    </td>
                </tr>
                <tr>
                    <td width="18%"> 
                        <asp:Label ID="Lbl_Numero_Inventario" runat="server" Text="No. Inventario"></asp:Label>
                    </td>
                    <td width="32%">
                       <asp:TextBox ID="Txt_Numero_Inventario" runat="server" Enabled="True" 
                            Width="98%" MaxLength="30"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Inventario" runat="server" 
                            Enabled="True" FilterType="Numbers" InvalidChars="&lt;,&gt;,&amp;,',!," 
                            TargetControlID="Txt_Numero_Inventario">
                        </cc1:FilteredTextBoxExtender>      
                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Numero_Inventario" runat="server" Enabled="True" TargetControlID="Txt_Numero_Inventario" WatermarkCssClass="watermarked" WatermarkText="<-- Automatico -->">
                        </cc1:TextBoxWatermarkExtender>                      
                    </td>
                    <td width="18%">
                        &nbsp;&nbsp;
                        <asp:Label ID="Lbl_Numero_Inventario_Anterior" runat="server" Text="* No. Inv. [Anterior]"></asp:Label>
                    </td>
                    <td width="32%">
                        <asp:TextBox ID="Txt_Numero_Inventario_Anterior" runat="server" Enabled="true" Width="98%" 
                            MaxLength="49"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Inventario_Anterior" runat="server" 
                            Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                            InvalidChars="'" TargetControlID="Txt_Numero_Inventario_Anterior" 
                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ ">
                        </cc1:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:left;" width="18%">
                        <asp:Label ID="Lbl_Procedencia_Bien" runat="server" Text="* Procedencia"></asp:Label>
                    </td>
                    <td  width="32%">
                        <asp:DropDownList ID="Cmb_Procedencia" runat="server" Width="99%">
                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="18%">
                        &nbsp;&nbsp;
                        <asp:Label ID="Lbl_Costo" runat="server"  Text="* Costo [$]"></asp:Label>
                    </td>
                    <td  width="32%">
                        <asp:TextBox ID="Txt_Costo" runat="server" Enabled="true" Width="50%"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Costo" runat="server" 
                            Enabled="True" FilterType="Numbers, Custom" ValidChars="."
                            TargetControlID="Txt_Costo">
                        </cc1:FilteredTextBoxExtender>   
                        <asp:RegularExpressionValidator ID="REV_Txt_Costo" runat="server" ErrorMessage="[Verificar]" ControlToValidate="Txt_Costo" ValidationExpression="^\d+(\.\d\d)?$" Font-Size="X-Small" ></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width:18%;">
                        <asp:Label ID="Lbl_Proveedor" runat="server" Text="Proveedor"></asp:Label>
                    </td>
                    <td  colspan="3">
                        <asp:TextBox ID="Txt_Nombre_Proveedor" runat="server" Width="93%"  Enabled="false"></asp:TextBox>
                        <asp:ImageButton ID="Btn_Lanzar_Mpe_Proveedores" runat="server" OnClick ="Btn_Lanzar_Mpe_Proveedores_Click" 
                             ImageUrl="~/paginas/imagenes/paginas/busqueda.png"  
                             ToolTip="Busqueda y Selección de Proveedor" AlternateText="Buscar" />
                   </td>
                </tr>
                <tr>
                    <td width="18%">
                        <asp:Label ID="Lbl_Factura" runat="server" Text="* No. de Factura"></asp:Label>
                    </td>
                    <td width="32%">
                        <asp:TextBox ID="Txt_Factura" runat="server" Enabled="true" Width="98%" 
                            MaxLength="49"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Factura" runat="server" Enabled="True" 
                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                            InvalidChars="'" TargetControlID="Txt_Factura" 
                            ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/_+-*@# ">
                        </cc1:FilteredTextBoxExtender>
                    </td>
                    <td width="18%">
                        &nbsp;&nbsp;
                        <asp:Label ID="Lbl_Fecha_Adquisicion" runat="server" Text="* Fecha Facturación"></asp:Label>
                    </td>
                    <td style="width:32%">
                        <asp:TextBox ID="Txt_Fecha_Adquisicion" runat="server" Width="85%" MaxLength="20" Enabled="false" ></asp:TextBox>
                        <asp:ImageButton ID="Btn_Fecha_Adquisicion" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                        <cc1:CalendarExtender ID="CE_Txt_Fecha_Adquisicion" runat="server" TargetControlID="Txt_Fecha_Adquisicion" PopupButtonID="Btn_Fecha_Adquisicion" Format="dd/MMM/yyyy">
                        </cc1:CalendarExtender>
                    </td>
                </tr>
                <tr>
                     <td width="18%">
                         <asp:Label ID="Lbl_Garantia" runat="server" Text="Garantía" ></asp:Label>
                    </td>
                     <td colspan="3">
                        <asp:TextBox ID="Txt_Garantia" runat="server"  Width="99%" MaxLength="249" ></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Garantia" runat="server" Enabled="True" TargetControlID="Txt_Garantia" WatermarkCssClass="watermarked" WatermarkText="Límite de Caractes 250">
                         </cc1:TextBoxWatermarkExtender>
                         <cc1:FilteredTextBoxExtender ID="FTE_Txt_Garantia" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" TargetControlID="Txt_Garantia" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/ ">
                        </cc1:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td width="18%">
                        <asp:Label ID="Lbl_Estatus" runat="server" Text="* Estatus"></asp:Label>
                    </td>
                    <td width="32%">
                        <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" Enabled="false">
                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                            <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                            <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="18%">
                        &nbsp;&nbsp;
                        <asp:Label ID="Lbl_Estado" runat="server" Text="* Estado"></asp:Label>
                    </td>
                    <td width="32%">
                        <asp:DropDownList ID="Cmb_Estado" runat="server" Width="100%">
                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                            <asp:ListItem>BUENO</asp:ListItem>
                            <asp:ListItem>REGULAR</asp:ListItem>
                            <asp:ListItem>MALO</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align:top; width:18%">
                            <asp:Label ID="Lbl_Observaciones" runat="server" Text="Observaciones"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="Txt_Observaciones" runat="server" Enabled="True" Rows="2" TextMode="MultiLine" Width="99%" MaxLength="100" ></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TBW_Txt_Observaciones" runat="server" Enabled="True" TargetControlID="Txt_Observaciones" WatermarkCssClass="watermarked" WatermarkText="Límite de Caractes 255">
                       </cc1:TextBoxWatermarkExtender>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:left; width:18%">
                        <asp:Label ID="Throbber" Text="wait" runat="server" Width="30px">                                                                     
                          <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                          <div  class="processMessage" id="div_progress">
                                <img alt=""  src="../imagenes/paginas/Updating.gif" />
                          </div>
                        </asp:Label>  
                        <asp:Label ID="Lbl_Seleccionar_Archivo" runat="server" Text="Archivo"></asp:Label>
                    </td>
                    <td colspan="3">                      
                        <cc1:AsyncFileUpload ID="AFU_Archivo" runat="server" Width="600px" CompleteBackColor="LightBlue" ErrorBackColor="Red" ThrobberID="Throbber" UploadingBackColor="LightGray" Enabled="true" />
                        <asp:ImageButton ID="Btn_Limpiar_FileUpload" runat="server" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Height="16px" Width="16px" CssClass="Img_Button" AlternateText="Limpiar Archivo" OnClick="Btn_Limpiar_FileUpload_Click" ToolTip="Liberar Archivo"/>
                    </td>
                </tr> 
            </table>
        </div>
        <br />
        <div id="Div_Region_Operacion" runat="server" style="width:97%;"> 
            <table width="100%" class="estilo_fuente">
                <tr>
                    <td colspan="4"><hr /></td>
                </tr>
                <tr>
                    <td colspan="4"> 
                       <asp:HiddenField ID="Hdf_Dependencia_ID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" width="50%"> 
                        <asp:Label ID="Lbl_Asignacion_Secundaria" runat="server" Text="* Asignar a Bien Mueble, Vehículo o Empleado"></asp:Label>
                    </td>
                    <td colspan="2" width="50%">
                        <asp:DropDownList ID="Cmb_Asignacion_Secundaria" runat="server" Width="100%" 
                            AutoPostBack="true" onselectedindexchanged="Cmb_Asignacion_Secundaria_SelectedIndexChanged" >
                            <asp:ListItem Value="SELECCIONE">&lt; - SELECCIONE - &gt;</asp:ListItem>
                            <asp:ListItem Value="NINGUNA">EMPLEADO</asp:ListItem>
                            <asp:ListItem Value="BIEN_MUEBLE">BIEN MUEBLE</asp:ListItem>
                            <asp:ListItem Value="VEHICULO">VEHÍCULO</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr> 
                <tr>
                    <td colspan="4"><hr /></td>
                </tr>
            </table>
        </div>  
        <br /> 
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
                        <asp:ImageButton ID="Btn_Lanzar_Buscar_Bien" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                            onclick="Btn_Lanzar_Buscar_Bien_Click"  />
                        
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
                        <asp:Label ID="Lbl_Fecha_Adquisicion_Parent" runat="server" Text="F. Adquisición" CssClass="estilo_fuente"></asp:Label>
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
                        <asp:Label ID="Lbl_Vehiculo_Nombre" runat="server" Text="Nombre"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="Txt_Vehiculo_Nombre" runat="server" Width="92%" Enabled="false"></asp:TextBox>
                        <asp:ImageButton ID="Btn_Buscar_Vehiculo" runat="server" 
                            AlternateText="Buscar Vehículo" 
                            ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                            onclick="Btn_Buscar_Vehiculo_Click"/>
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
                        <asp:Label ID="Lbl_Vehiculo_Marca" runat="server" Text="Marca"></asp:Label>
                    </td>
                    <td style="width:35%">
                        <asp:TextBox ID="Txt_Vehiculo_Marca" runat="server" Width="99.5%" Enabled="false"></asp:TextBox>
                    </td>
                    <td style="width:15%; text-align:left; ">
                        &nbsp;&nbsp;
                        <asp:Label ID="Lbl_Vehiculo_Modelo" runat="server" Text="Modelo"></asp:Label>
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
        <div id="Div_Resguardos" runat="server" style="width:97%;">
            <table width="100%" class="estilo_fuente">
                <tr>
                    <td class="label_titulo" colspan="4">Resguardos</td>
                </tr> 
                <tr align="right" class="barra_delgada">
                    <td colspan="4" align="center">
                    </td>
                </tr> 
                <tr align="left">
                    <td style="width:18%; text-align:left;">
                        <asp:Label ID="Lbl_Resguardantes" runat="server" Text="Empleados" ></asp:Label>
                    </td>
                    <td colspan="3" style="text-align:left;">
                         <asp:DropDownList ID="Cmb_Empleados" runat="server" Width="100%">
                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                         </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:18%; text-align:left; vertical-align:top;">
                        <asp:Label ID="Lbl_Comentarios" runat="server" Text="Comentarios"></asp:Label>
                    </td>
                     <td colspan="3" style="text-align:right;">
                         <asp:TextBox ID="Txt_Cometarios" runat="server" Rows="3" TextMode="MultiLine" Width="99%" MaxLength="100"></asp:TextBox>
                         <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Cometarios" runat="server" Enabled="True" TargetControlID="Txt_Cometarios" WatermarkCssClass="watermarked" WatermarkText="Límite de Caractes 255">
                         </cc1:TextBoxWatermarkExtender>
                     </td>
                </tr>                       
                <tr>
                    <td colspan="4"><hr /></td>
                </tr>
                <tr>
                    <td style="width:18%; text-align:left; vertical-align:top;">
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="Btn_Busqueda_Avanzada_Resguardante" runat="server" ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Width="24px" ToolTip="Buscar" AlternateText="Buscar" OnClick="Btn_Busqueda_Avanzada_Resguardante_Click"/>
                    </td>
                     <td colspan="3" style="text-align:right";>
                        <asp:ImageButton ID="Btn_Agregar_Resguardante" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_add.png" ToolTip="Agregar" AlternateText="Agregar" OnClick="Btn_Agregar_Resguardante_Click"/>
                        <asp:ImageButton ID="Btn_Quitar_Resguardante" runat="server" ImageUrl="~/paginas/imagenes/paginas/quitar.png" ToolTip="Quitar" AlternateText="Quitar" OnClick="Btn_Quitar_Resguardante_Click"/>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4"><hr /></td>
                </tr>
            </table>
        </div>
        <br />
        <div id="Div_Listado_Resguardantes" runat="server" style="width:97%;">
            <table width="100%" class="estilo_fuente">
                <tr>
                    <td align="center" colspan="4">
                        <br />
                        <asp:GridView ID="Grid_Resguardantes" runat="server" AllowPaging="True" 
                             AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                             GridLines="None" OnPageIndexChanging="Grid_Resguardantes_PageIndexChanging" 
                             PageSize="5" Width="97.5%" CssClass="GridView_1">
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <Columns>
                                <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                     ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                     <ItemStyle Width="30px" />
                                </asp:ButtonField>
                                <asp:BoundField DataField="EMPLEADO_ALMACEN_ID" HeaderText="EMPLEADO ALMACEN" 
                                     SortExpression="EMPLEADO_RESGUARDO_ID"  >
                                     <ItemStyle Width="30px" Font-Size="X-Small" />
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
                            </Columns>
                            <HeaderStyle CssClass="GridHeader" />
                            <PagerStyle CssClass="GridHeader" />
                            <RowStyle CssClass="GridItem" />
                            <SelectedRowStyle CssClass="GridSelected" />
                        </asp:GridView>
                        <br />
                    </td>
                </tr>
            </table>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel> 

    <asp:UpdatePanel ID="UpPnl_Aux_Mpe_Productos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_Mpe_Productos_Cabecera" runat="server" Text="" style="display:none;"/>
            <cc1:ModalPopupExtender ID="Mpe_Productos_Cabecera" runat="server" 
                                    TargetControlID="Btn_Comodin_Mpe_Productos_Cabecera" PopupControlID="Pnl_Mpe_Productos" 
                                    CancelControlID="Btn_Cerrar_Mpe_Productos" PopupDragHandleControlID="Pnl_Mpe_Productos_Interno"
                                    DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>
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
 
    <asp:UpdatePanel ID="UpPnl_Aux_Mpe_Bienes" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
               <asp:Button ID="Btn_Comodin_Buscar_Bien" runat="server" Text="Button" style="display:none;"/>   
                        <cc1:ModalPopupExtender ID="MPE_Busqueda_Bien_Mueble" runat="server" 
                        TargetControlID="Btn_Comodin_Buscar_Bien" PopupControlID="Pnl_Busqueda_Bien_Mueble" 
                        CancelControlID="Btn_Cerrar" PopupDragHandleControlID="Pnl_Interno"
                        DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID="UpPnl_Aux_Donadores" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
                <asp:Button ID="Btn_Comodin_Donadores" runat="server" Text="Button" style="display:none;"/>
                <cc1:ModalPopupExtender ID="MPE_Donadores" runat="server" TargetControlID="Btn_Comodin_Donadores"
                PopupControlID="Pnl_Donador" CancelControlID="Btn_Cerrar_Ventana_Cancelacion"
                PopupDragHandleControlID="Pnl_Cabecera" DropShadow="True" BackgroundCssClass="progressBackgroundFilter" />     
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
   
    <asp:Panel ID="Pnl_Mpe_Productos" runat="server" CssClass="drag" HorizontalAlign="Center" 
    style="display:none;border-style:outset;border-color:Silver;width:760px;" >                
    <asp:Panel ID="Pnl_Mpe_Productos_Interno" runat="server" CssClass="estilo_fuente"
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table class="estilo_fuente" width="100%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;width:90%">
                   <asp:Image ID="Img_Mpe_Productos" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Busqueda y Selección de Productos
                </td>
                <td align="right">
                   <asp:ImageButton ID="Btn_Cerrar_Mpe_Productos" CausesValidation="false" runat="server" style="cursor:pointer;" 
                        ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
     </asp:Panel>
       <center>
        <asp:UpdatePanel ID="UpPnl_Mpe_Productos" runat="server" UpdateMode="Conditional">
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
                        <table width="100%">
                            <tr>
                                <td style="width:15%; text-align:left;">
                                    <asp:Label ID="Lbl_Nombre_Producto_Buscar" runat="server" CssClass="estilo_fuente" Text="Introducir" style="font-weight:bolder;" ></asp:Label>
                                </td>
                                <td style="width:85%; text-align:left;">
                                    <asp:TextBox ID="Txt_Nombre_Producto_Buscar" runat="server" Width="92%" AutoPostBack="true"  ontextchanged="Txt_Nombre_Producto_Buscar_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Producto_Buscar" runat="server" TargetControlID="Txt_Nombre_Producto_Buscar" InvalidChars="<,>,&,',!," 
                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                                    </cc1:FilteredTextBoxExtender>  
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Nombre_Producto_Buscar" runat="server" Enabled="True" TargetControlID="Txt_Nombre_Producto_Buscar" WatermarkCssClass="watermarked" 
                                                     WatermarkText="<-- Nombre del Producto -->">
                                    </cc1:TextBoxWatermarkExtender>
                                    <asp:ImageButton ID="Btn_Ejecutar_Busqueda_Productos" runat="server" OnClick ="Btn_Ejecutar_Busqueda_Productos_Click" 
                                         ImageUrl="~/paginas/imagenes/paginas/busqueda.png"  
                                         ToolTip="Buscar Productos" AlternateText="Buscar" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        
                            <asp:Panel ID="Pnl_Listado_Bienes" runat="server" ScrollBars="Vertical" style="white-space:normal;"
                                Width="100%" BorderColor="#3366FF" Height="330px">
                                       <asp:GridView ID="Grid_Listado_Productos" runat="server" 
                                         AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                         GridLines="None" OnPageIndexChanging="Grid_Listado_Productos_PageIndexChanging" 
                                         OnSelectedIndexChanged="Grid_Listado_Productos_SelectedIndexChanged"
                                         PageSize="100" Width="100%" CssClass="GridView_1" AllowPaging="true">
                                         <AlternatingRowStyle CssClass="GridAltItem" />
                                         <Columns>
                                             <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                 ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                 <ItemStyle Width="30px" />
                                             </asp:ButtonField>
                                             <asp:BoundField DataField="PRODUCTO_ID" HeaderText="Producto ID" SortExpression="PRODUCTO_ID"  >
                                                 <ItemStyle Width="30px" Font-Size="X-Small" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="CLAVE_PRODUCTO" HeaderText="Clave" SortExpression="CLAVE_PRODUCTO">
                                                <ItemStyle Width="100px" Font-Size="X-Small"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="NOMBRE_PRODUCTO" HeaderText="Nombre del Producto" SortExpression="NOMBRE_PRODUCTO" >
                                                 <ItemStyle Font-Size="X-Small" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="DESCRIPCION_PRODUCTO" HeaderText="Descripcion" SortExpression="DESCRIPCION_PRODUCTO">
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

    <asp:Panel ID="Pnl_Donador" runat="server" CssClass="drag" HorizontalAlign="Center" 
    style="display:none;border-style:outset;border-color:Silver;width:760px;">                         
    <asp:Panel ID="Pnl_Cabecera" runat="server" CssClass="estilo_fuente"
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table class="estilo_fuente" width="100%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;width:90%">
                   <asp:Image ID="Image1" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Introducir los datos del Donador
                </td>
                <td align="right">
                   <asp:ImageButton ID="Btn_Cerrar_Ventana_Cancelacion" CausesValidation="false" runat="server" style="cursor:pointer;" 
                        ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
     </asp:Panel>
       <center>
        <asp:UpdatePanel ID="Upl_Pnl_Donadores" runat="server" UpdateMode="Conditional">
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
                                            <td style="width: 18%; text-align: left;">
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
                                            <td style="width: 18%; text-align: left;">
                                                <asp:Label ID="Lbl_Apellido_Paterno_Donador" runat="server" Text="A. Paterno"
                                                    CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 32%; text-align: left;">
                                                <asp:TextBox ID="Txt_Apellido_Paterno_Donador" runat="server" Width="98%" MaxLength="50"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Apellido_Paterno_Donador" runat="server"
                                                    TargetControlID="Txt_Apellido_Paterno_Donador" InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 18%; text-align: left;">
                                                <asp:Label ID="Lbl_Apellido_Materno_Donador" runat="server" Text="A. Materno"
                                                    CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 32%; text-align: left;">
                                                <asp:TextBox ID="Txt_Apellido_Materno_Donador" runat="server" Width="98%" MaxLength="50"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Apellido_Materno_Donador" runat="server"
                                                    TargetControlID="Txt_Apellido_Materno_Donador" InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 18%; text-align: left;">
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
                                            <td style="width: 18%; text-align: left;">
                                                <asp:Label ID="Lbl_Ciudad_Donador" runat="server" Text="Ciudad" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 32%; text-align: left;">
                                                <asp:TextBox ID="Txt_Ciudad_Donador" runat="server" Width="98%" MaxLength="50"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Ciudad_Donador" runat="server" TargetControlID="Txt_Ciudad_Donador"
                                                    InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 18%; text-align: left;">
                                                <asp:Label ID="Lbl_Estado_Donador" runat="server" Text="Estado" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 32%; text-align: left;">
                                                <asp:TextBox ID="Txt_Estado_Donador" runat="server" Width="98%" MaxLength="50"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Estado_Donador" runat="server" TargetControlID="Txt_Estado_Donador"
                                                    InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"
                                                    ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ ">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 18%; text-align: left;">
                                                <asp:Label ID="Lbl_Telefono_Donador" runat="server" Text="Telefono" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 32%; text-align: left;">
                                                <asp:TextBox ID="Txt_Telefono_Donador" runat="server" Width="98%" MaxLength="20"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Telefono_Donador" runat="server" TargetControlID="Txt_Telefono_Donador"
                                                    InvalidChars="<,>,&,',!," FilterType="Custom, Numbers" ValidChars="()-">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 18%; text-align: left;">
                                                <asp:Label ID="Lbl_Celular_Donador" runat="server" Text="Celular" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 32%; text-align: left;">
                                                <asp:TextBox ID="Txt_Celular_Donador" runat="server" Width="98%" MaxLength="20"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Celular_Donador" runat="server" TargetControlID="Txt_Celular_Donador"
                                                    InvalidChars="<,>,&,',!," FilterType="Custom, Numbers" ValidChars="()-">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 18%; text-align: left;">
                                                <asp:Label ID="Lbl_CURP_Donador" runat="server" Text="CURP" CssClass="estilo_fuente"></asp:Label>
                                            </td>
                                            <td style="width: 32%; text-align: left;">
                                                <asp:TextBox ID="Txt_CURP_Donador" runat="server" Width="98%" MaxLength="18"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_CURP_Donador" runat="server" TargetControlID="Txt_CURP_Donador"
                                                    InvalidChars="<,>,&,',!," FilterType="Custom, UppercaseLetters, Numbers" ValidChars="Ññ">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 18%; text-align: left;">
                                                <asp:Label ID="Lbl_RFC_Donador_MPE" runat="server" Text="* RFC" CssClass="estilo_fuente" style="font-weight:bolder;"></asp:Label>
                                            </td>
                                            <td style="width: 32%; text-align: left;">
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
    
    <asp:Panel ID="Pnl_Busqueda_Bien_Mueble" runat="server" HorizontalAlign="Center" Width="800px" style="display:none;border-style:outset;border-color:Silver;background-repeat:repeat-y;">                         
    <asp:Panel ID="Pnl_Interno" runat="server" style="background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">    
            <center>
            <asp:UpdatePanel ID="UpPnl_Busqueda" runat="server"  UpdateMode="Conditional"> 
                <ContentTemplate>
               <asp:UpdateProgress ID="UpdPgr_Busqueda" runat="server" AssociatedUpdatePanelID="UpPnl_Busqueda" DisplayAfter="0">
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
                                            <asp:Label ID="Lbl_Busqueda_Inventario_Anterior" runat="server" Text="No. Inventario" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Inventario_Anterior" runat="server" Width="98%" ></asp:TextBox>   
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Inventario_Anterior" runat="server" TargetControlID="Txt_Busqueda_Inventario_Anterior" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>                                                                                 
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Inventario_SIAS" runat="server" Text="No. Inventario SIAS" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Inventario_SIAS" runat="server" Width="98%" ></asp:TextBox>   
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Inventario_SIAS" runat="server" TargetControlID="Txt_Busqueda_Inventario_SIAS" InvalidChars="<,>,&,',!,"  FilterType="Numbers" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>                                                                                 
                                        </td>
                                    </tr>                                              
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Producto" runat="server" Text="Nombre Producto" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Producto" runat="server" Width="98%" ></asp:TextBox>   
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Producto" runat="server" TargetControlID="Txt_Busqueda_Producto" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
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
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Modelo" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" 
                                                TargetControlID="Txt_Busqueda_Modelo" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-_%/$# ">
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
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Factura" runat="server" TargetControlID="Txt_Busqueda_Factura" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
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
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Numero_Serie" runat="server" TargetControlID="Txt_Busqueda_Numero_Serie" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ /*-+$%&" Enabled="True">        
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
                            <asp:GridView ID="Grid_Listado_Bienes" runat="server" AllowPaging="true"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                OnSelectedIndexChanged="Grid_Listado_Bienes_SelectedIndexChanged" PageSize="100"
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
  
    <asp:UpdatePanel ID="UpPnl_MPE_Busqueda_Vehiculo" runat="server"  UpdateMode="Conditional"> 
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_Busqueda_Vehiculo" runat="server" Text="Button" style="display:none;"/> 
            <cc1:ModalPopupExtender ID="MPE_Busqueda_Vehiculo" runat="server" TargetControlID="Btn_Comodin_Busqueda_Vehiculo" 
                PopupControlID="Pnl_Busqueda_Vehiculo" CancelControlID="Btn_Cerrar_Busqueda_Vehiculo" PopupDragHandleControlID="Pnl_Busqueda_Vehiculo_Interno"
                DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>
        </ContentTemplate>
    </asp:UpdatePanel>
  
    <asp:Panel ID="Pnl_Busqueda_Vehiculo" runat="server" CssClass="drag" HorizontalAlign="Center" style="display:none;border-style:outset;border-color:Silver;width:760px;" >
        <center>
            <asp:Panel ID="Pnl_Busqueda_Vehiculo_Interno" runat="server" CssClass="estilo_fuente" style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
                <table class="estilo_fuente" width="100%">
                    <tr>
                        <td style="color:Black;font-size:12;font-weight:bold;width:90%">
                           <asp:Image ID="Img_Encabezado_Busqueda_Vehiculos" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                             Busqueda de Vehículos
                        </td>
                        <td align="right">
                           <asp:ImageButton ID="Btn_Cerrar_Busqueda_Vehiculo" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                        </td>
                    </tr>
                </table>   
            </asp:Panel>
            <asp:UpdatePanel ID="UpPnl_Busqueda_Vehiculo" runat="server"> 
                <ContentTemplate>
                <asp:UpdateProgress ID="UpPrg_Busqueda_Vehiculo" runat="server" AssociatedUpdatePanelID="UpPnl_Busqueda_Vehiculo" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
                </asp:UpdateProgress>
                <br />
                <cc1:TabContainer ID="Tab_Contenedor_Pestagnas_Busqueda_Vehiculo" runat="server" Width="100%" ActiveTabIndex="0" CssClass="Tab">
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Panel_Datos_Generales_Busqueda_Vehiculo" ID="Tab_Panel_Datos_Generales_Busqueda_Vehiculo" Width="100%" Height="400px" BackColor="White">
                        <HeaderTemplate>Generales</HeaderTemplate>
                            <ContentTemplate>
                            <div style="border-style:outset; width:99.5%; height:200px; background-color:White;" >
                                <table width="100%">
                                    <tr>
                                        <td style="text-align:left;" colspan="4">&nbsp;</td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Numero_Inventario" runat="server" Text="Número Inventario" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Vehiculo_Numero_Inventario" runat="server" Width="97%" ></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Vehiculo_Numero_Inventario" runat="server" TargetControlID="Txt_Busqueda_Vehiculo_Numero_Inventario" FilterType="Numbers" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Numero_Economico" runat="server" Text="Número Económico"  CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Vehiculo_Numero_Economico" runat="server" Width="97%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Vehiculo_Numero_Economico" runat="server" TargetControlID="Txt_Busqueda_Vehiculo_Numero_Economico" InvalidChars="<,>,&,',!,"  FilterType="Numbers"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                    </tr>                                
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Modelo" runat="server" Text="Modelo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Vehiculo_Modelo" runat="server" Width="97%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Vehiculo_Modelo" runat="server" TargetControlID="Txt_Busqueda_Vehiculo_Modelo"
                                                InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>                               
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Marca" runat="server" Text="Marca" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Marca" runat="server"  Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>            
                                            </asp:DropDownList>                             
                                        </td>
                                    </tr>                       
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Tipo_Vehiculo" runat="server" Text="Tipo Vehículo" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Tipo_Vehiculo" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="SELECCIONE"></asp:ListItem>
                                            </asp:DropDownList>                                 
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Tipo_Combustible" runat="server" Text="Combustible" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Tipo_Combustible" runat="server"  Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>            
                                            </asp:DropDownList>                             
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Anio_Fabricacion" runat="server" Text="Año Fabricación" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Vehiculo_Anio_Fabricacion" runat="server" Width="97%" MaxLength="4"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Vehiculo_Anio_Fabricacion" runat="server" TargetControlID="Txt_Busqueda_Vehiculo_Anio_Fabricacion" FilterType="Numbers" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Color" runat="server" Text="Color" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Color" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                    </tr>             
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Zonas" runat="server" Text="Zonas" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Zonas" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Estatus" runat="server" Text="Estatus" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Estatus" runat="server" Width="100%" >
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                            <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                            <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                            <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                                            </asp:DropDownList>                                                 
                                        </td>
                                    </tr>    
                                    <tr>
                                        <td style="width:20%; text-align:left;">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Dependencias" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:80%; text-align:left;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Dependencias" runat="server" Width="85%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                                            </asp:DropDownList>                                                 
                                            <asp:ImageButton ID="Btn_Buscar_Datos_Vehiculo" runat="server" 
                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" CausesValidation="False" 
                                                OnClick="Btn_Buscar_Datos_Vehiculo_Click"
                                                ToolTip="Buscar Contrarecibos"/>
                                            <asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Datos_Vehiculo" runat="server" 
                                                OnClick="Btn_Limpiar_Filtros_Buscar_Datos_Vehiculo_Click"
                                                CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" 
                                                ToolTip="Limpiar Filtros" Width="20px" />                                      
                                        </td>
                                    </tr>                                     
                                </table>
                            </div>                                 
                        </ContentTemplate>
                    </cc1:TabPanel>   
                    
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Panel_Reguardantes_Busqueda_Vehiculo" ID="Tab_Panel_Resguardantes_Busqueda_Vehiculo" Width="100%" BackColor="White">
                        <HeaderTemplate>Resguardantes</HeaderTemplate>
                        <ContentTemplate>    
                            <div style="border-style:outset; width:99.5%; height:200px; background-color:White;" >
                                <table width="100%">
                                    <tr>
                                        <td style="text-align:left;" colspan="2">&nbsp;</td>
                                    </tr>                                 
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_RFC_Resguardante" runat="server" Text="RFC Reguardante" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Vehiculo_RFC_Resguardante" runat="server" Width="200px"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Vehiculo_RFC_Resguardante" runat="server" TargetControlID="Txt_Busqueda_Vehiculo_RFC_Resguardante" InvalidChars="<,>,&,',!,"  FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_No_Empleado" runat="server" Text="No. Empleado" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Vehiculo_No_Empleado" runat="server" Width="97%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Vehiculo_No_Empleado" runat="server" TargetControlID="Txt_Busqueda_Vehiculo_No_Empleado" InvalidChars="<,>,&,',!,"  FilterType="Numbers" Enabled="True">        
                                            </cc1:FilteredTextBoxExtender>  
                                        </td>
                                    </tr>                          
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Resguardantes_Dependencias" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Busqueda_Vehiculo_Resguardantes_Dependencias_SelectedIndexChanged" >
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                                            </asp:DropDownList>                                   
                                        </td>
                                    </tr>                          
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Vehiculo_Nombre_Resguardante" runat="server" Text="Resguardante" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align:left;" colspan="3">
                                            <asp:DropDownList ID="Cmb_Busqueda_Vehiculo_Nombre_Resguardante" runat="server" Width="100%" >
                                                <asp:ListItem Text="&lt;TODOS&gt;" Value="TODOS"></asp:ListItem>
                                            </asp:DropDownList>     
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td colspan="4" style="text-align:right">
                                            <asp:ImageButton ID="Btn_Buscar_Resguardante_Vehiculo" runat="server" 
                                                CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" 
                                                OnClick="Btn_Buscar_Resguardante_Vehiculo_Click"
                                                ToolTip="Buscar Listados"/>                                      
                                            <asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Resguardante_Vehiculo" runat="server"  
                                                CausesValidation="False"  OnClick="Btn_Limpiar_Filtros_Buscar_Resguardante_Vehiculo_Click"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Width="20px"  
                                                ToolTip="Limpiar Filtros" />   
                                            &nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>                                      
                                </table>
                            </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>  
                <div style="width:99%; height:150px; overflow:auto; border-style:outset; background-color:White;">
                    <center>
                        <caption>
                            <asp:GridView ID="Grid_Listado_Busqueda_Vehiculo" runat="server" AllowPaging="true"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None"
                                OnPageIndexChanging="Grid_Listado_Busqueda_Vehiculo_PageIndexChanging"
                                OnSelectedIndexChanged="Grid_Listado_Busqueda_Vehiculo_SelectedIndexChanged"
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
        </center>
    </asp:Panel>    
  
  </asp:Content>  
    
    
    
    
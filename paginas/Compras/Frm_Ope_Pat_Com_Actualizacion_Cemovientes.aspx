<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pat_Com_Actualizacion_Cemovientes.aspx.cs" Inherits="paginas_Compras_Frm_Ope_Pat_Com_Actualizacion_Cemovientes" Title="Operación - Actualización de Animales" %>

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

    <asp:ScriptManager ID="ScptM_Cemovientes" runat="server"  EnableScriptGlobalization ="true" EnableScriptLocalization = "True" />
    
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
                        <td class="label_titulo" colspan="2">Actualización de Animales</td>
                    </tr>
                    <tr>
                        <td colspan="2">
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
                        <td align="left" style="width:25%;">
                            <asp:ImageButton ID="Btn_Modificar" runat="server" ToolTip="Modificar"
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Modificar" onclick="Btn_Modificar_Click" />
                            <asp:ImageButton ID="Btn_Generar_Reporte" runat="server" ToolTip="Ver Resguardo"
                                ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" CssClass="Img_Button" 
                                AlternateText="Reporte" onclick="Btn_Generar_Reporte_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Salir"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" CssClass="Img_Button"
                                AlternateText="Salir" onclick="Btn_Salir_Click"  />    
                        </td>
                        <td align="right" style="width:75%;">
                            <div id="Div_Busqueda" runat="server">
                                <asp:LinkButton ID="Btn_Busqueda_Avanzada" runat="server" ForeColor="White"
                                    onclick="Btn_Avanzada_Click" ToolTip="Avanzada">Busqueda</asp:LinkButton>
                                    &nbsp;&nbsp;
                                <asp:TextBox ID="Txt_Busqueda_Inventario_Anterior" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="FTE_Txt_Busqueda_Inventario_Anterior" runat="server" 
                                    WatermarkCssClass="watermarked"
                                    WatermarkText="< - Inventario Anterior - >"
                                    TargetControlID="Txt_Busqueda_Inventario_Anterior" />
                                <asp:TextBox ID="Txt_Busqueda" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda" runat="server" TargetControlID="Txt_Busqueda" FilterType="Numbers">
                            </cc1:FilteredTextBoxExtender>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Busqueda" runat="server" 
                                    WatermarkCssClass="watermarked"
                                    WatermarkText="< - Inventario SIAS  ->"
                                    TargetControlID="Txt_Busqueda" />
                                <asp:ImageButton ID="Btn_Buscar" runat="server" OnClick="Btn_Buscar_Click"
                                    ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Consultar"/>
                            </div>
                        </td>
                </table>   
                <br />           
               <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="98%" ActiveTabIndex="0" CssClass="Tab" Style="visibility:visible;" >
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Cemovientes_Detalles"  ID="Tab_Cemovientes_Detalles"  Width="100%">
                        <HeaderTemplate>Generales</HeaderTemplate>
                        <ContentTemplate>
                            <table width="98%" class="estilo_fuente">  
                                <tr>
                                    <td colspan="4">
                                        <asp:HiddenField ID="Hdf_Cemoviente_ID" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%; text-align:left; ">
                                        <asp:Label ID="Lbl_Nombre" runat="server" Text="Nombre" ></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Nombre" runat="server" Width="99.5%" Enabled="False"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre" runat="server" 
                                                                        TargetControlID="Txt_Nombre" InvalidChars="<,>,&,',!," 
                                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                                        ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                                        </cc1:FilteredTextBoxExtender>   
                                    </td>
                                </tr> 
                                <tr>
                                    <td style="width:15%; text-align:left; ">
                                        <asp:Label ID="Lbl_Numero_Inventario" runat="server" Text="No. Inventario" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Numero_Inventario" runat="server" Width="98%" Enabled="False"></asp:TextBox>                                       
                                    </td>
                                    <td style="width:15%; text-align:left; ">
                                        <asp:Label ID="Lbl_No_Inventario_Anterior" runat="server" Text="Inventario Anterior" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_No_Inventario_Anterior" runat="server" Width="98%" Enabled="False"></asp:TextBox>                                       
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
                                    <td style="width:15%; text-align:left; ">
                                        <asp:Label ID="Lbl_Dependencia" runat="server" Text="Unidad Responsable" ></asp:Label>
                                    </td>
                                    <td colspan="3" style="width:35%">
                                        <asp:DropDownList ID="Cmb_Dependencias" runat="server" Width="100%" onselectedindexchanged="Cmb_Dependencias_SelectedIndexChanged" AutoPostBack="True" Enabled="False">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:18%; text-align:left; ">
                                        <asp:Label ID="Lbl_Tipo_Cemoviente" runat="server" Text="Tipo Animal" ></asp:Label>
                                    </td>
                                    <td style="width:32%">
                                        <asp:DropDownList ID="Cmb_Tipo_Cemoviente" runat="server" Width="98%" Enabled="False" AutoPostBack="True" onselectedindexchanged="Cmb_Tipo_Cemoviente_SelectedIndexChanged">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:18%; text-align:left; ">
                                        <asp:Label ID="Lbl_Razas" runat="server" Text="Raza" ></asp:Label>
                                    </td> 
                                    <td style="width:32%">
                                        <asp:DropDownList ID="Cmb_Razas" runat="server" Width="98%" Enabled="False">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>                           
                                <tr>
                                    <td style="width:18%; text-align:left; ">
                                        <asp:Label ID="Lbl_Tipo_Adiestramiento" runat="server" Text="Adiestramiento" ></asp:Label>
                                    </td>
                                    <td style="width:32%">
                                        <asp:DropDownList ID="Cmb_Tipo_Adiestramiento" runat="server" Width="98%" Enabled="False">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:18%; text-align:left; ">
                                        <asp:Label ID="Lbl_Funciones" runat="server" Text="Función" ></asp:Label>
                                    </td>
                                    <td style="width:32%">
                                        <asp:DropDownList ID="Cmb_Funciones" runat="server" Width="98%" Enabled="False">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>                     
                                <tr>
                                    <td style="width:15%; text-align:left; ">
                                        <asp:Label ID="Lbl_Tipos_Alimentacion" runat="server" Text="Tipo Alimentación" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:DropDownList ID="Cmb_Tipos_Alimentacion" runat="server" Width="98%" Enabled="false">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:18%; text-align:left; ">
                                        <asp:Label ID="Lbl_Color" runat="server" Text="Color" ></asp:Label>
                                    </td>
                                    <td style="width:32%">
                                        <asp:DropDownList ID="Cmb_Colores" runat="server" Width="98%" Enabled="False">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>                           
                                <tr>
                                    <td style="width:15%; text-align:left; ">
                                        <asp:Label ID="Lbl_Fecha_Nacimiento" runat="server" Text="Fecha Nacimiento" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Fecha_Nacimiento" runat="server" Width="85%"  Enabled="false"></asp:TextBox>
                                        <asp:ImageButton ID="Btn_Fecha_Nacimiento" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                        <cc1:CalendarExtender ID="CE_Txt_Fecha_Nacimiento" runat="server" TargetControlID="Txt_Fecha_Nacimiento" PopupButtonID="Btn_Fecha_Nacimiento" Format="dd/MMM/yyyy">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td style="width:15%; text-align:left; ">
                                        <asp:Label ID="Lbl_Fecha_Adquisicion" runat="server" Text="Fecha Adquisición" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Fecha_Adquisicion" runat="server" Width="85%" Enabled="false"></asp:TextBox>
                                        <asp:ImageButton ID="Btn_Fecha_Adquisicion" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                        <cc1:CalendarExtender ID="CE_Txt_Fecha_Adquisicion" runat="server" TargetControlID="Txt_Fecha_Adquisicion" PopupButtonID="Btn_Fecha_Adquisicion" Format="dd/MMM/yyyy">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>        
                                <tr>
                                    <td style="width:18%; text-align:left; ">
                                        <asp:Label ID="Lbl_Sexo" runat="server" Text="Sexo" ></asp:Label>
                                    </td>
                                    <td style="width:32%">
                                        <asp:DropDownList ID="Cmb_Sexo" runat="server" Width="98%" Enabled="False">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            <asp:ListItem Value="HEMBRA">HEMBRA</asp:ListItem>
                                            <asp:ListItem Value="MACHO">MACHO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:18%; text-align:left; ">
                                        <asp:Label ID="Lbl_Tipos_Ascendencia" runat="server" Text="Ascendencia" ></asp:Label>
                                    </td>
                                    <td style="width:32%">
                                        <asp:DropDownList ID="Cmb_Tipos_Ascendencia" runat="server" Width="98%" Enabled="False" AutoPostBack="true"
                                            onselectedindexchanged="Cmb_Tipos_Ascendencia_SelectedIndexChanged">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            <asp:ListItem Value="DESCONOCIDO">OTRA</asp:ListItem>
                                            <asp:ListItem Value="GOBIERNO">GOBIERNO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>                                   
                                <tr>
                                    <td style="width:15%; text-align:left; ">
                                        <asp:Label ID="Lbl_Padre" runat="server" Text="Padre" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:DropDownList ID="Cmb_Padre" runat="server" Width="100%">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:15%; text-align:left; ">
                                        <asp:Label ID="Lbl_Madre" runat="server" Text="Madre"></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:DropDownList ID="Cmb_Madre" runat="server" Width="100%">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>                               
                                <tr>
                                    <td style="width:15%; text-align:left; ">
                                        <asp:Label ID="Lbl_Costo_Inicial" runat="server" Text="Costo Inicial [$]" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Costo_Inicial" runat="server" Width="98%" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td style="width:15%; text-align:left; ">
                                        <asp:Label ID="Lbl_Costo_Actual" runat="server" Text="Costo Actual [$]" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:TextBox ID="Txt_Costo_Actual" runat="server" Width="98%" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>    
                                <tr>
                                    <td style="width:18%; text-align:left; ">
                                        <asp:Label ID="Lbl_No_Factura" runat="server" Text="No. Factura" ></asp:Label>
                                    </td>
                                    <td style="width:32%">
                                        <asp:TextBox ID="Txt_No_Factura" runat="server" Width="98%" Enabled="False"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Factura" runat="server" TargetControlID="Txt_No_Factura" 
                                               InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ -_/" Enabled="True">   
                                        </cc1:FilteredTextBoxExtender> 
                                    </td>
                                    <td style="width:18%; text-align:left; ">
                                        <asp:Label ID="Lbl_Proveedores" runat="server" Text="Proveedor" ></asp:Label>
                                    </td>
                                    <td style="width:32%">
                                        <asp:DropDownList ID="Cmb_Proveedores" runat="server" Width="98%" Enabled="False">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>                            
                                <tr>
                                    <td style="width:15%; text-align:left; ">
                                        <asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:DropDownList ID="Cmb_Estatus" runat="server" Width="98%" Enabled="False">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                            <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                            <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:15%; text-align:left; ">
                                        <asp:Label ID="Lbl_Veterinario" runat="server" Text="Veterinario" ></asp:Label>
                                    </td>
                                    <td style="width:35%">
                                        <asp:DropDownList ID="Cmb_Veterinario" runat="server" Width="98%" Enabled="False">
                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>                            
                                <tr>
                                    <td style="text-align:left; vertical-align:top;">
                                        <asp:Label ID="Lbl_Motivo_Baja" runat="server" Text="Motivo Baja" ></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Motivo_Baja" runat="server" Width="99%" Enabled="False" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Motivo_Baja" runat="server" 
                                                                        TargetControlID="Txt_Motivo_Baja" InvalidChars="<,>,&,',!," 
                                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                                        ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ  -_/" Enabled="True">   
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Motivo_Baja" runat="server" TargetControlID ="Txt_Motivo_Baja" 
                                            WatermarkText="Límite de Caractes 255" WatermarkCssClass="watermarked" Enabled="True"/>     
                                    </td>
                                </tr>                               
                                <tr>
                                    <td style="text-align:left; vertical-align:top;">
                                        <asp:Label ID="Lbl_Observaciones" runat="server" Text="Observaciones" ></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Txt_Observaciones" runat="server" Width="99%" Enabled="False" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Observaciones" runat="server" 
                                                                        TargetControlID="Txt_Observaciones" InvalidChars="<,>,&,',!," 
                                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                                        ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ  -_/" Enabled="True">   
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observaciones" runat="server" TargetControlID ="Txt_Observaciones" 
                                            WatermarkText="Límite de Caractes 255" WatermarkCssClass="watermarked" Enabled="True"/>     
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
                                                    <td style="width:18%; text-align:left;">
                                                        <asp:Label ID="Lbl_Resguardantes" runat="server" Text="Empleados"></asp:Label>
                                                    </td>
                                                    <td  colspan="3" style="text-align:left;">
                                                        <asp:DropDownList ID="Cmb_Empleados" runat="server" Width="100%" Enabled="False">
                                                            <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:18%; text-align:left; vertical-align:top;">
                                                        <asp:Label ID="Lbl_Comentarios" runat="server" Text="Comentarios" ></asp:Label>
                                                    </td>
                                                    <td  colspan="3" style="text-align:left;">
                                                        <asp:TextBox ID="Txt_Comentarios" runat="server" TextMode="MultiLine" Rows="2" Width="99%" Enabled="False"></asp:TextBox>
                                                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios" runat="server" TargetControlID ="Txt_Comentarios" 
                                                            WatermarkText="Límite de Caractes 150" WatermarkCssClass="watermarked" Enabled="True"/>
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
                                            <asp:GridView ID="Grid_Resguardantes" runat="server" 
                                                AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                                GridLines="None" Width="98%" 
                                                onrowdatabound="Grid_Resguardantes_RowDataBound" >
                                                <RowStyle CssClass="GridItem" />
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                                        <ItemStyle Width="30px" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField DataField="BIEN_RESGUARDO_ID" HeaderText="BIEN_RESGUARDO_ID" SortExpression="BIEN_RESGUARDO_ID" >
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
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Vacunas"  ID="Tab_Vacunas"  Width="100%">
                        <HeaderTemplate>Vacunas</HeaderTemplate>
                        <ContentTemplate>
                                <table width="98%" class="estilo_fuente">                           
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Vacunas" runat="server" Text="Vacunas" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Vacunas" runat="server" Width="98%" Enabled="False">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Fecha_Aplicacion" runat="server" Text="Fecha de Aplicación" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Fecha_Aplicacion" runat="server" Width="85%" MaxLength="20" Enabled="false"></asp:TextBox>
                                            <asp:ImageButton ID="Btn_Fecha_Aplicacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Aplicacion" runat="server" TargetControlID="Txt_Fecha_Aplicacion" PopupButtonID="Btn_Fecha_Aplicacion" Format="dd/MMM/yyyy">
                                            </cc1:CalendarExtender>                     
                                        </td>
                                    </tr>                 
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Veterinario_Vacuno" runat="server" Text="Aplicó" ></asp:Label>
                                        </td>
                                        <td  colspan="3" style="width:35%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Veterinario_Vacuno" runat="server" Width="98%" Enabled="False">
                                                <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15%; text-align:left; vertical-align:top;">
                                            <asp:Label ID="Lbl_Comentarios_Vacuna" runat="server" Text="Comentarios" ></asp:Label>
                                        </td>
                                        <td  colspan="3" style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Comentarios_Vacuna" runat="server" TextMode="MultiLine" Rows="3" Width="98%" Enabled="False"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Comentarios_Vacuna" runat="server" TargetControlID ="Txt_Comentarios_Vacuna" WatermarkText="Límite de Caractes 255" WatermarkCssClass="watermarked" Enabled="True"/>                                                        
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td colspan="3">&nbsp;</td>
                                        <td style="width:15%; text-align:right;">
                                            <asp:ImageButton ID="Btn_Agregar_Vacuna" runat="server" OnClick="Btn_Agregar_Vacuna_Click"
                                                ImageUrl="~/paginas/imagenes/paginas/sias_add.png"  AlternateText="Agregar"   />
                                            <asp:ImageButton ID="Btn_Quitar_Vacuna" runat="server" OnClick="Btn_Quitar_Vacuna_Click"
                                                ImageUrl="~/paginas/imagenes/paginas/quitar.png"  AlternateText="Cancelar Vacuna" />
                                        </td>  
                                    </tr>
                                </table>
                                <br />
                                <asp:GridView ID="Grid_Vacunas" runat="server" 
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                    GridLines="None" AllowPaging="True" Width="98%"
                                    OnPageIndexChanging="Grid_Vacunas_PageIndexChanging"
                                    PageSize="5" >
                                    <RowStyle CssClass="GridItem" />
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png" >
                                            <ItemStyle Width="30px" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="VACUNA_ID" HeaderText="VACUNA_ID" 
                                            SortExpression="VACUNA_ID">
                                            <ItemStyle Width="110px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VETERINARIO_ID" HeaderText="VETERINARIO_ID" 
                                            SortExpression="VETERINARIO_ID">
                                            <ItemStyle Width="110px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VACUNA_NOMBRE" HeaderText="Vacuna" 
                                            SortExpression="VACUNA_NOMBRE">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FECHA_APLICACION" HeaderText="Fecha Aplicación" 
                                            SortExpression="FECHA_APLICACION" DataFormatString="{0:dd/MMM/yyyy}" />
                                        <asp:BoundField DataField="VETERINARIO_NOMBRE" HeaderText="Aplicó" 
                                            SortExpression="VETERINARIO_NOMBRE" />
                                        <asp:BoundField DataField="ESTATUS" HeaderText="Estatus" 
                                            SortExpression="ESTATUS" />
                                    </Columns>
                                    <PagerStyle CssClass="GridHeader" />
                                    <SelectedRowStyle CssClass="GridSelected" />
                                    <HeaderStyle CssClass="GridHeader" />                                
                                    <AlternatingRowStyle CssClass="GridAltItem" />                                
                                </asp:GridView>    
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel  runat="server" HeaderText="Tab_Historial_Resguardos"  ID="Tab_Historial_Resguardos"  Width="100%" >
                        <HeaderTemplate>Historial</HeaderTemplate>
                        <ContentTemplate>
                                <table width="100%" class="estilo_fuente"> 
                                    <tr>
                                        <td class="label_titulo" colspan="4">Modificaciones</td>
                                    </tr>   
                                    <tr align="right" class="barra_delgada">
                                        <td colspan="4" align="center">
                                        </td>
                                    </tr>   
                                    <tr>
                                        <td style="text-align:left; vertical-align:top; width:18%">
                                            <asp:Label ID="Lbl_Usuario_Creo" runat="server" Text="Creación" ></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="Txt_Usuario_creo" runat="server" Width="99%" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>     
                                    <tr>
                                        <td style="text-align:left; vertical-align:top; width:18%">
                                            <asp:Label ID="Lbl_Usuario_Modifico" runat="server" Text="Ultima Modificación" ></asp:Label>
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
                                            <asp:Label ID="Lbl_Historial_Empleado_Resguardo" runat="server" Text="Empleado" ></asp:Label>
                                        </td>
                                        <td  colspan="3" style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Historial_Empleado_Resguardo" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>                                  
                                    <tr>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Historial_Fecha_Inicial_Resguardo" runat="server" Text="Fecha Inicial" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Historial_Fecha_Inicial_Resguardo" runat="server" Width="97%" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width:15%; text-align:left;">
                                            <asp:Label ID="Lbl_Historial_Fecha_Final_Resguardo" runat="server" Text="Fecha Final" ></asp:Label>
                                        </td>
                                        <td style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Historial_Fecha_Final_Resguardo" runat="server" Width="97%" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                   <tr>
                                        <td style="width:15%; text-align:left; vertical-align:top;">
                                            <asp:Label ID="Lbl_Historial_Comentarios_Resguardo" runat="server" Text="Comentarios" ></asp:Label>
                                        </td>
                                        <td  colspan="3" style="width:35%; text-align:left;">
                                            <asp:TextBox ID="Txt_Historial_Comentarios_Resguardo" runat="server" TextMode="MultiLine" Rows="3" Width="98%" Enabled="false"></asp:TextBox>                                                     
                                        </td>
                                    </tr>             
                                </table>
                                <br />
                                <asp:GridView ID="Grid_Historial_Resguardantes" runat="server" 
                                    AutoGenerateColumns="False" CellPadding="5" ForeColor="#333333" 
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
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="Tab_Archivos" runat="server" HeaderText="Tab_Archivos">
                        <HeaderTemplate>Archivos</HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" class="estilo_fuente">              
                                <tr>
                                    <td style="text-align:left; vertical-align:top; width:15%">
                                        <asp:Label ID="Lbl_Archivo" runat="server" Text="Archivo"></asp:Label>
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
                                                Width="24px" CssClass="Img_Button" AlternateText="Ver Archivo"  ToolTip="Ver Archivo" 
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
                </cc1:TabContainer>       
            </div>            
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>     
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Btn_Aceptar_Cancelacion" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Grid_Listado_Semovientes" EventName="SelectedIndexChanged" />
        </Triggers>      
    </asp:UpdatePanel> 
     
    <asp:UpdatePanel ID="UpPnl_Modal_Motivo_Cancelacion_Vacuna" runat="server"  UpdateMode="Conditional"> 
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_2" runat="server" Text="Button" style="display:none;" /> 
            <cc1:ModalPopupExtender ID="MPE_Cancelar_Vacuna" runat="server" TargetControlID="Btn_Comodin_2" 
                PopupControlID="Pnl_Cancelar_Vacuna_Tmp" CancelControlID="Btn_Cerrar_Mpe_Vacuna_Tmp" PopupDragHandleControlID="Pnl_Interno_Pnl_Cancelar_Vacuna_Tmp"
                DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID="UpPnl_MPE_Busqueda_Cemoviente" runat="server"  UpdateMode="Conditional"> 
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin" runat="server" Text="Button" style="display:none;"/> 
            <cc1:ModalPopupExtender ID="MPE_Busqueda_Cemoviente" runat="server" TargetControlID="Btn_Comodin" 
                PopupControlID="Pnl_Busqueda_Cemoviente" CancelControlID="Btn_Cerrar" PopupDragHandleControlID="Pnl_Interno"
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
    
    <asp:Panel ID="Pnl_Busqueda_Cemoviente" runat="server" HorizontalAlign="Center" Width="800px" style="border-style:outset;border-color:Silver;background-repeat:repeat-y;">                         
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
                                        <td style="width:20%; text-align:left;">
                                            <asp:Label ID="Lbl_Busqueda_Dependencias" runat="server" Text="U. Responsable" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="Cmb_Busqueda_Dependencias" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Numero_Inventario" runat="server" Text="Número Inventario" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:TextBox ID="Txt_Busqueda_Numero_Inventario" runat="server" Width="97%"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Busqueda_Numero_Inventario" runat="server" TargetControlID="Txt_Busqueda_Numero_Inventario" FilterType="Numbers" Enabled="True"></cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Tipo_Ascendencia" runat="server" Text="Ascendencia" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Tipo_Ascendencia" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                                                <asp:ListItem Value="DESCONOCIDO">DESCONOCIDO</asp:ListItem>
                                                <asp:ListItem Value="GOBIERNO">GOBIERNO</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Tipo_Alimentacion" runat="server" Text="Alimentación" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Tipo_Alimentacion" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="SELECCIONE"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Tipo_Adiestramiento" runat="server" 
                                                Text="Adiestramiento" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Tipo_Adiestramiento" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Funciones" runat="server" Text="Función" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList  ID="Cmb_Busqueda_Funciones" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                                            </asp:DropDownList>
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
                                            <asp:Label ID="Lbl_Busqueda_Razas" runat="server" Text="Raza" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList  ID="Cmb_Busqueda_Razas" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:20%; text-align:left; ">
                                            <asp:Label ID="Lbl_Busqueda_Estatus" runat="server" Text="Estatus" CssClass="estilo_fuente"></asp:Label>
                                        </td>
                                        <td style="width:30%; text-align:left;">
                                            <asp:DropDownList ID="Cmb_Busqueda_Estatus" runat="server" Width="100%">
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="TODOS"></asp:ListItem>
                                                <asp:ListItem Value="VIGENTE">VIGENTE</asp:ListItem>
                                                <asp:ListItem Value="TEMPORAL">BAJA (TEMPORAL)</asp:ListItem>
                                                <asp:ListItem Value="DEFINITIVA">BAJA (DEFINITIVA)</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="text-align:right;">
                                            <asp:ImageButton ID="Btn_Buscar_Datos" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" CausesValidation="False" ToolTip="Buscar Contrarecibos" OnClick="Btn_Buscar_Datos_Click"/>
                                            <asp:ImageButton ID="Btn_Limpiar_Filtros_Buscar_Datos" runat="server" CausesValidation="False" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" ToolTip="Limpiar Filtros" Width="20px" OnClick="Btn_Limpiar_Filtros_Buscar_Datos_Click"  />
                                            &nbsp;&nbsp;&nbsp;
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
                                                <asp:ListItem Text="&lt; TODOS &gt;" Value="SELECCIONE"></asp:ListItem>
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
                            <asp:GridView ID="Grid_Listado_Semovientes" runat="server"
                                AutoGenerateColumns="False" CssClass="GridView_1" GridLines="None" 
                                Width="98%" AllowPaging="true" PageSize="100"
                                onpageindexchanging="Grid_Listado_Semovientes_PageIndexChanging" 
                                onselectedindexchanged="Grid_Listado_Semovientes_SelectedIndexChanged" >
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                        ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                        <ItemStyle Width="30px" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="CEMOVIENTE_ID" HeaderText="CEMOVIENTE_ID" SortExpression="CEMOVIENTE_ID" />
                                    <asp:BoundField DataField="NUMERO_INVENTARIO_ANTERIOR" HeaderText="No. Inv." SortExpression="NUMERO_INVENTARIO_ANTERIOR" >
                                        <ItemStyle Width="130px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NUMERO_INVENTARIO" HeaderText="Inv. SIAS" SortExpression="NUMERO_INVENTARIO" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TIPO_CEMOVIENTE" HeaderText="Tipo" SortExpression="TIPO_CEMOVIENTE" >
                                        <ItemStyle Width="90px" Font-Size="X-Small" HorizontalAlign="Center"  />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RAZA" HeaderText="Raza" SortExpression="RAZA">
                                        <ItemStyle Width="110px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SEXO" HeaderText="Sexo" SortExpression="SEXO" NullDisplayText="-" >
                                        <ItemStyle Width="60px" Font-Size="X-Small" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CEMOVIENTE" HeaderText="Nombre" SortExpression="CEMOVIENTE" >
                                        <ItemStyle Font-Size="X-Small" />
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
    
    <asp:Panel ID="Pnl_Cancelar_Vacuna_Tmp" runat="server" CssClass="drag" HorizontalAlign="Center" style="display:none;border-style:outset;border-color:Silver;width:760px;">                         
    <asp:Panel ID="Pnl_Interno_Pnl_Cancelar_Vacuna_Tmp" runat="server" CssClass="estilo_fuente"
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table class="estilo_fuente" width="100%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;width:90%">
                    <asp:Image ID="Img_Mpe_Productos" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                    Cancelación de Vacuna
                </td>
                <td align="right" colspan="3">
                    <asp:ImageButton ID="Btn_Cerrar_Mpe_Vacuna_Tmp" CausesValidation="false" runat="server" style="cursor:pointer;" 
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
                    <div style="border-style: outset; width: 95%; background-color: White;">
                        <table width="100%">
                            <tr>
                                <td colspan="2">
                                  <div id="Div_MPE_Cancelar_Vacuna_Error" style="width:98%;" runat="server" visible="false">
                                    <table style="width:100%;">
                                      <tr>
                                        <td colspan="2" align="left">
                                            <asp:Label ID="Lbl_MPE_Cancelar_Vacuna_Cabecera_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                        </td>            
                                      </tr>
                                      <tr>
                                        <td style="width:10%;">              
                                        </td>          
                                        <td style="width:90%;text-align:left;" valign="top">
                                          <asp:Label ID="Lbl_MPE_Cancelar_Vacuna_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                        </td>
                                      </tr>          
                                    </table>                   
                                  </div>   
                                    &nbsp;                       
                                </td>
                            </tr>  
                            <tr>
                                <td style="text-align:left;" colspan="2">
                                    <asp:HiddenField ID="Hdf_Vacuna_Cancelar" runat="server" />
                                </td>
                            </tr>                       
                            <tr>
                                <td style="width:20%; text-align:left;">
                                    <asp:Label ID="Lbl_MPE_Vacuna" runat="server" Text="Vacuna" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:80%; text-align:left;">
                                    <asp:TextBox ID="Txt_MPE_Vacuna" runat="server" Width="99%" Enabled="false"></asp:TextBox>                              
                                </td>
                            </tr>                   
                            <tr>
                                <td style="width:20%; text-align:left;">
                                    <asp:Label ID="Lbl_MPE_Fecha_Vacuna" runat="server" Text="Fecha Aplicacion" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:80%; text-align:left;">
                                    <asp:TextBox ID="Txt_MPE_Fecha_Vacuna" runat="server" Width="99%" Enabled="false"></asp:TextBox>                              
                                </td>
                            </tr>                   
                            <tr>
                                <td style="width:20%; text-align:left;">
                                    <asp:Label ID="Lbl_MPE_Aplico_Vacuna" runat="server" Text="Aplico Vacuna" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:80%; text-align:left;">
                                    <asp:TextBox ID="Txt_MPE_Aplico_Vacuna" runat="server" Width="99%" Enabled="false" ></asp:TextBox>                              
                                </td>
                            </tr>                        
                            <tr>
                                <td style="width:20%; text-align:left;">
                                    <asp:Label ID="Lbl_MPE_Motivo_Cancelacion" runat="server" Text="Motivo Cancelación" CssClass="estilo_fuente"></asp:Label>
                                </td>
                                <td style="width:80%; text-align:left;">
                                    <asp:TextBox ID="Txt_MPE_Motivo_Cancelacion" runat="server" Width="99%" Rows="3" TextMode="MultiLine" Height="40px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Motivo_Cancelacion" runat="server" 
                                                                        TargetControlID="Txt_MPE_Motivo_Cancelacion" InvalidChars="<,>,&,',!," 
                                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" 
                                                                        ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                                    </cc1:FilteredTextBoxExtender>
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Motivo_Cancelacion" runat="server" TargetControlID ="Txt_MPE_Motivo_Cancelacion" 
                                        WatermarkText="Motivo de la Cancelación de la Vacuna [Máximo Caráctes 255]" WatermarkCssClass="watermarked" Enabled="True"/>                                      
                                </td>
                            </tr>
                            <tr>
                                <td style="width:20%; text-align:left;">
                                    &nbsp;
                                </td>
                                <td style="width:80%; text-align:right;">
                                    <asp:ImageButton ID="Btn_Aceptar_Cancelacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/accept.png" CausesValidation="False" ToolTip="Aceptar" OnClick="Btn_Aceptar_Cancelacion_Click"/>
                                </td>
                            </tr>
                        </table>
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
                                                TargetControlID="Txt_Busqueda_Nombre_Empleado" ValidChars="áéíóúÁÉÍÓÚ Ññ"/>
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


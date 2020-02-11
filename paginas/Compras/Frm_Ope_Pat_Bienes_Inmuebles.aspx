<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pat_Bienes_Inmuebles.aspx.cs" Inherits="paginas_Control_Patrimonial_Frm_Ope_Pat_Bienes_Inmuebles" Title="Control de Bienes Inmuebles" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript" language="javascript">
    //Valida que los campos tengan el formato decimal correcto
    function Validar_Valores_Decimales(){  
        var regEx = /^[0-9]{1,50}(\.[0-9]{0,4})?$/;
        var Superficie = replaceAll(document.getElementById("<%=Txt_Superficie.ClientID%>").value, ",", "");
        var Valor_Comercial = replaceAll(document.getElementById("<%=Txt_Valor_Comercial.ClientID%>").value, ",", "");
        var Densidad = replaceAll(document.getElementById("<%=Txt_Densidad_Construccion.ClientID%>").value, ",", "");
        var Contruccion_Registrada = replaceAll(document.getElementById("<%=Txt_Construccion_Resgistrada.ClientID%>").value, ",", "");
        var Superficie_Contabilidad = replaceAll(document.getElementById("<%=Txt_Cont_Superficie.ClientID%>").value, ",", "");
        var Resultado = true;
        if(Contruccion_Registrada.length>0 && Resultado){
            Valido = Contruccion_Registrada.match(regEx);
            if(!Valido){
                alert('Formato Incorrecto para el Campo \"Contrucción Registrada\".'); 
                Resultado = false; 
            }
        }
        if(Superficie.length>0 && Resultado){
            Valido = Superficie.match(regEx);
            if(!Valido){
                alert('Formato Incorrecto para el Campo \"Superficie\".'); 
                Resultado = false; 
            }
        }
        if(Valor_Comercial.length>0 && Resultado){
            Valido = Valor_Comercial.match(regEx);
            if(!Valido){
                alert('Formato Incorrecto para el Campo \"Valor Comercial\".'); 
                Resultado = false; 
            }
        }
        if(Superficie_Contabilidad.length>0 && Resultado){
            Valido = Superficie_Contabilidad.match(regEx);
            if(!Valido){
                alert('Formato Incorrecto para el Campo \"Superficie (Contabilidad)\".'); 
                Resultado = false; 
            }
        }
        if(Densidad.length>0 && Resultado){
            Valido = Densidad.match(regEx);
            if(!Valido){
                alert('Formato Incorrecto para el Campo \"Densidad\".'); 
                Resultado = false; 
            }
        }
        if(Resultado){
            Resultado = Validar_Valores_Porcentuales();
        }
        return Resultado;
    } 
     
    //Valida que los campos tengan el formato porcentual correcto
    function Validar_Valores_Porcentuales(){  
        var regEx = /^([1]{1})?[0-9]{1,2}(\.[0-9]{1,2})?$/; 
        var Porcentaje_Ocupacion = document.getElementById("<%=Txt_Porcentaje_Ocupacion.ClientID%>").value;
        var Resultado = true;
        if(Porcentaje_Ocupacion.length>0 && Resultado){
            Valido = Porcentaje_Ocupacion.match(regEx);
            if(!Valido){
                alert('Formato Incorrecto para el Campo \"Porcentaje de Ocupación\".'); 
                Resultado = false; 
            } else {
                Valor = parseFloat(Porcentaje_Ocupacion);
                if(Valor>100){
                    alert('El valor del Campo \"Porcentaje de Ocupación\" no puede exceder el 100.'); 
                    Resultado = false; 
                }
            }
        }
        return Resultado;
      }  
      
	function replaceAll( text, busca, reemplaza ){
	  while (text.toString().indexOf(busca) != -1)
	      text = text.toString().replace(busca,reemplaza);
	  return text;
	}
</script>
   <!--SCRIPT PARA LA VALIDACION QUE NO EXPERE LA SESSION-->  
   <script language="javascript" type="text/javascript">
    <!--
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_Session.ashx";

        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion() {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }

        //Temporizador para matener la sesión activa
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);
        
    //-->
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"  AsyncPostBackTimeout="36000" EnableScriptLocalization="true" EnableScriptGlobalization="true"/>   
    
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_General" style="background-color:#ffffff; width:100%; height:100%;" runat="server">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="2">Control de Bienes Inmuebles</td>
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
                        <td align="left" style="width:50%;">
                            <asp:ImageButton ID="Btn_Nuevo" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_nuevo.png" Width="24px" 
                                CssClass="Img_Button" AlternateText="Nuevo" ToolTip="Nuevo" 
                                onclick="Btn_Nuevo_Click" OnClientClick="javascript:return Validar_Valores_Decimales();"/>
                            <asp:ImageButton ID="Btn_Modificar" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" 
                                CssClass="Img_Button" AlternateText="Modificar" ToolTip="Modificar" 
                                onclick="Btn_Modificar_Click" OnClientClick="javascript:return Validar_Valores_Decimales();" />
                            <asp:ImageButton ID="Btn_Ver_Ficha_Tecnica_PDF" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" 
                                CssClass="Img_Button" AlternateText="Ver Ficha Tenica de Bien Inmueble [PDF]" ToolTip="Ver Ficha Tenica de Bien Inmueble [PDF]" 
                                onclick="Btn_Ver_Ficha_Tecnica_PDF_Click" />
                            <asp:ImageButton ID="Btn_Ver_Ficha_Tecnica_Excel" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" Width="24px" 
                                CssClass="Img_Button" AlternateText="Ver Ficha Tenica de Bien Inmueble [EXCEL]" ToolTip="Ver Ficha Tenica de Bien Inmueble [EXCEL]" 
                                onclick="Btn_Ver_Ficha_Tecnica_Excel_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" Width="24px" 
                                CssClass="Img_Button" AlternateText="Salir" ToolTip="Salir" 
                                onclick="Btn_Salir_Click" />
                        </td>
                        <td style="width:50%;"> &nbsp; </td>                        
                    </tr>
                </table>   
                <br />
                <table border="0" width="98%" class="estilo_fuente">
                    <tr>
                        <td style="background-color:#4F81BD; color:White; font-weight:bolder; text-align:center;" colspan="4">DATOS GENERALES DEL BIEN INMUEBLE </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:HiddenField ID="Hdf_Bien_Inmueble_ID" runat="server" /> 
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;"><asp:Label ID="Lbl_Bien_Inmueble_ID" runat="server" Text="No. Inventario" style="font-weight:bolder;"></asp:Label></td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Bien_Inmueble_ID" runat="server" style="width:98%; font-weight:bolder;" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%; text-align:left;"><asp:Label ID="Lbl_Fecha_Alta_Cta_Pub" runat="server" Text="Alta Cta. Pública"></asp:Label></td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Fecha_Alta_Cta_Pub" runat="server" style="width:80%;" Enabled="false"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Fecha_Alta_Cta_Pub" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Alta_Cta_Pub" runat="server" 
                                TargetControlID="Txt_Fecha_Alta_Cta_Pub" 
                                PopupButtonID="Btn_Fecha_Alta_Cta_Pub" Format="dd/MMM/yyyy" Enabled="True">
                            </cc1:CalendarExtender>
                        </td>
                        <td style="width:15%;"><asp:Label ID="Lbl_Registro_Propiedad" runat="server" Text="Reg. Propiedad" ></asp:Label></td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Registro_Propiedad" runat="server" style="width:98%;"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Registro_Propiedad" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" InvalidChars="'" TargetControlID="Txt_Registro_Propiedad" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ "></cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;"><asp:HiddenField ID="Hdf_Calle_ID" runat="server" /><asp:Label ID="Lbl_Calle" runat="server" Text="Calle"></asp:Label></td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Calle" runat="server" style="width:93%;"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Calle" runat="server" ToolTip="Buscar Calle" AlternateText="Bucar Calle" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Calle_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;"><asp:Label ID="Lbl_Numero_Exterior" runat="server" Text="# Exterior"></asp:Label></td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Numero_Exterior" runat="server" style="width:98%;" MaxLength="10"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Numero_Exterior" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" InvalidChars="'" TargetControlID="Txt_Numero_Exterior" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ "></cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Numero_Interior" runat="server" Text="# Interior"></asp:Label></td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Numero_Interior" runat="server" style="width:98%;" MaxLength="10"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="fte_Txt_Numero_Interior" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" InvalidChars="'" TargetControlID="Txt_Numero_Interior" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ "></cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;"><asp:HiddenField ID="Hdf_Colonia_ID" runat="server" /><asp:Label ID="Lbl_Colonia" runat="server" Text="Colonia"></asp:Label></td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Colonia" runat="server" style="width:93%;"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Colonia" runat="server" ToolTip="Buscar Colonia" AlternateText="Bucar Colonia" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Colonia_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;"><asp:Label ID="Lbl_Uso" runat="server" Text="Uso"></asp:Label></td>
                        <td style="width:35%;"><asp:DropDownList ID="Cmb_Uso" runat="server" style="width:100%;"></asp:DropDownList></td>
                        <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Destino" runat="server" Text="Destino"></asp:Label></td>
                        <td style="width:35%;"><asp:DropDownList ID="Cmb_Destino" runat="server" style="width:100%;"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td style="width:15%;"><asp:Label ID="Lbl_Origen" runat="server" Text="Origen"></asp:Label></td>
                        <td style="width:35%;"><asp:DropDownList ID="Cmb_Origen" runat="server" style="width:100%;"></asp:DropDownList></td>
                        <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Estatus" runat="server" Text="Estatus"></asp:Label></td>
                        <td style="width:35%;">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" style="width:100%;">
                                <asp:ListItem Value="">&lt;SELECCIONE&gt;</asp:ListItem>
                                <asp:ListItem Value="DONADO">DONADO</asp:ListItem>
                                <asp:ListItem Value="INVADIDO">INVADIDO</asp:ListItem>
                                <asp:ListItem Value="PERMUTADO">PERMUTADO</asp:ListItem>
                                <asp:ListItem Value="PROCESO LEGAL">PROCESO LEGAL</asp:ListItem>
                                <asp:ListItem Value="PROCESO REGULARIZACION">PROCESO REGULARIZACION</asp:ListItem>
                                <asp:ListItem Value="VENDIDO">VENDIDO</asp:ListItem>
                                <asp:ListItem Value="OTRO">OTRO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;"><asp:Label ID="Lbl_Fecha_Registro" runat="server" Text="Fecha Registral"></asp:Label></td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Fecha_Registro" runat="server" style="width:80%;" Enabled="false"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Fecha_Registro" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                            <cc1:CalendarExtender ID="CE_Txt_Fecha_Registro" runat="server" 
                                TargetControlID="Txt_Fecha_Registro" 
                                PopupButtonID="Btn_Fecha_Registro" Format="dd/MMM/yyyy" Enabled="True">
                            </cc1:CalendarExtender>
                        </td>
                        <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Construccion_Registrada" runat="server" Text="Construcción Reg."></asp:Label></td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Construccion_Resgistrada" runat="server" style="width:80%;"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Construccion_Resgistrada" runat="server" Enabled="True" FilterType="Custom, Numbers" InvalidChars="'" TargetControlID="Txt_Construccion_Resgistrada" ValidChars="." ></cc1:FilteredTextBoxExtender>
                            <asp:Label ID="Lbl_Construccion_Registrada_M2" runat="server" Text="[m2]"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;"><asp:Label ID="Lbl_Superficie" runat="server" Text="Superficie"></asp:Label></td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Superficie" runat="server" style="width:80%;"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Superficie" runat="server" Enabled="True" FilterType="Custom, Numbers" InvalidChars="'" TargetControlID="Txt_Superficie" ValidChars=".," ></cc1:FilteredTextBoxExtender>
                            <asp:Label ID="Lbl_Superficie_M2" runat="server" Text="[m2]"></asp:Label>
                        </td>
                        <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Porcentaje_Ocupacion" runat="server" Text="Ocupación [%]"></asp:Label></td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Porcentaje_Ocupacion" runat="server" style="width:98%;"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Porcentaje_Ocupacion" runat="server" Enabled="True" FilterType="Custom, Numbers" InvalidChars="'" TargetControlID="Txt_Porcentaje_Ocupacion" ValidChars="." ></cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;"><asp:Label ID="Lbl_Manzana" runat="server" Text="Manzana"></asp:Label></td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Manzana" runat="server" style="width:98%;" MaxLength="4"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Manzana" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" InvalidChars="'" TargetControlID="Txt_Manzana" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ "></cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Lote" runat="server" Text="Lote"></asp:Label></td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Lote" runat="server" style="width:98%;" MaxLength="10"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Lote" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" InvalidChars="'" TargetControlID="Txt_Lote" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ "></cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;"><asp:Label ID="Lbl_Area_Donacion" runat="server" Text="Area de Donación"></asp:Label></td>
                        <td style="width:35%;"><asp:DropDownList ID="Cmb_Area_Donacion" runat="server" style="width:100%;"></asp:DropDownList></td>
                        <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Tipo_Bien" runat="server" Text="Tipo de Bien"></asp:Label></td>
                        <td style="width:35%;">
                            <asp:DropDownList ID="Cmb_Tipo_Bien" runat="server" style="width:100%;">
                                <asp:ListItem Value="">&lt;SELECCIONE&gt;</asp:ListItem>
                                <asp:ListItem Value="PROPIO">PROPIO</asp:ListItem>
                                <asp:ListItem Value="RENTADO">RENTADO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;"><asp:Label ID="Lbl_Numero_Cuenta_Predial" runat="server" Text="Cuenta Predial"></asp:Label></td>
                        <td style="width:35%;">
                            <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                            <asp:TextBox ID="Txt_Numero_Cuenta_Predial" runat="server" style="width:85%;"></asp:TextBox>
                            <asp:ImageButton ID="Btn_Buscar_Numero_Cuenta_Predial" runat="server" ToolTip="Buscar Cuenta Predial" AlternateText="Bucar Cuenta Predial" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Numero_Cuenta_Predial_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;"><asp:Label ID="Lbl_Propietario" runat="server" Text="Propietario"></asp:Label></td>
                        <td colspan="3"><asp:TextBox ID="Txt_Propietario" runat="server" style="width:99%;"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width:15%;"><asp:Label ID="Lbl_Valor_Catastral" runat="server" Text="Valor Catastral [$]"></asp:Label></td>
                        <td style="width:35%;"><asp:TextBox ID="Txt_Valor_Catastral" runat="server" style="width:98%;"></asp:TextBox></td>
                        <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Valor_Comercial" runat="server" Text="Valor Comercial [$]"></asp:Label></td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Valor_Comercial" runat="server" style="width:98%;"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Valor_Comercial" runat="server" Enabled="True" FilterType="Custom, Numbers" InvalidChars="'" TargetControlID="Txt_Valor_Comercial" ValidChars="." ></cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;"><asp:Label ID="Lbl_Efectos_Fiscales" runat="server" Text="Efectos Fiscales"></asp:Label></td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Efectos_Fiscales" runat="server" style="width:98%;" MaxLength="150"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Efectos_Fiscales" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" InvalidChars="'" TargetControlID="Txt_Efectos_Fiscales" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ "></cc1:FilteredTextBoxExtender>
                        </td>
                        <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Tipo_Predio" runat="server" Text="Tipo de Predio"></asp:Label></td>
                        <td style="width:35%;"><asp:DropDownList ID="Cmb_Tipo_Predio" runat="server" style="width:100%;"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td style="width:15%;"><asp:Label ID="Lbl_Sector" runat="server" Text="Sector"></asp:Label></td>
                        <td style="width:35%;"><asp:DropDownList ID="Cmb_Sector" runat="server" style="width:100%;"></asp:DropDownList></td>
                        <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Clasificacion_Zona" runat="server" Text="Clasif. Zona"></asp:Label></td>
                        <td style="width:35%;"><asp:DropDownList ID="Cmb_Clasificacion_Zona" runat="server" style="width:100%;"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td style="width:15%;"><asp:Label ID="Lbl_Estado" runat="server" Text="Estado"></asp:Label></td>
                        <td style="width:35%;">
                            <asp:DropDownList ID="Cmb_Estado" runat="server" style="width:100%;" OnSelectedIndexChanged="Cmb_Estado_SelectedIndexChanged" AutoPostBack="true" >
                                <asp:ListItem Value="ALTA">ALTA</asp:ListItem>
                                <asp:ListItem Value="BAJA">BAJA</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Densidad_Construccion" runat="server" Text="Densidad Const."></asp:Label></td>
                        <td style="width:35%;">
                            <asp:TextBox ID="Txt_Densidad_Construccion" runat="server" style="width:80%;"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Densidad_Construccion" runat="server" Enabled="True" FilterType="Custom, Numbers" InvalidChars="'" TargetControlID="Txt_Densidad_Construccion" ValidChars="." ></cc1:FilteredTextBoxExtender>
                            <asp:Label ID="Lbl_Densidad_Construccion_M2" runat="server" Text="[m2]"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;"><asp:Label ID="Lbl_Vias_Acceso" runat="server" Text="Vias de Acceso"></asp:Label></td>
                        <td colspan="3">
                            <asp:TextBox ID="Txt_Vias_Aceso" runat="server" style="width:99%;" Rows="3" TextMode="MultiLine"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FTE_Txt_Vias_Aceso" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" InvalidChars="'" TargetControlID="Txt_Vias_Aceso" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ "></cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:15%;"><asp:Label ID="Lbl_Observaciones" runat="server" Text="Observaciones"></asp:Label></td>
                        <td colspan="3"><asp:TextBox ID="Txt_Observaciones" runat="server" style="width:99%;" Rows="3" TextMode="MultiLine"></asp:TextBox></td>
                        <cc1:FilteredTextBoxExtender ID="FTE_Txt_Observaciones" runat="server" TargetControlID="Txt_Observaciones" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ " Enabled="True"></cc1:FilteredTextBoxExtender>
                        <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Observaciones" runat="server" TargetControlID ="Txt_Observaciones" WatermarkText="<< Para Agregar una Nueva Observación >>" WatermarkCssClass="watermarked" Enabled="True"/>    
                    </tr>
                    <tr>
                        <td colspan="4">
                             <cc1:TabContainer ID="Tab_Contenedor_Pestagnas" runat="server" Width="98%" ActiveTabIndex="0" CssClass="Tab">
                                <cc1:TabPanel  runat="server" HeaderText="Tab_Medidas_Colindancias"  ID="Tab_Medidas_Colindancias"  Width="100%">
                                    <HeaderTemplate>Medidas y Colindancias</HeaderTemplate>
                                    <ContentTemplate>
                                        <table width="98%" class="estilo_fuente" border="0">
                                            <tr>
                                                <td style="width:15%;"><asp:Label ID="Lbl_Orientacion" runat="server" Text="Orientacion" style="font-weight:bolder;"></asp:Label></td>
                                                <td style="width:35%;">
                                                    <asp:DropDownList ID="Cmb_Orientacion" runat="server" style="width:100%;">
                                                        <asp:ListItem Value="">&lt;SELECCIONE&gt;</asp:ListItem>
                                                        <asp:ListItem Value="NORTE">NORTE</asp:ListItem>
                                                        <asp:ListItem Value="NOR_ORIENTE">NOR ORIENTE</asp:ListItem>
                                                        <asp:ListItem Value="NOR_PONIENTE">NOR PONIENTE</asp:ListItem>
                                                        <asp:ListItem Value="SUR">SUR</asp:ListItem>
                                                        <asp:ListItem Value="SUR_ORIENTE">SUR ORIENTE</asp:ListItem>
                                                        <asp:ListItem Value="SUR_PONIENTE">SUR PONIENTE</asp:ListItem>
                                                        <asp:ListItem Value="ESTE">ESTE</asp:ListItem>
                                                        <asp:ListItem Value="OESTE">OESTE</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Medida" runat="server" Text="Medida" style="font-weight:bolder;"></asp:Label></td>
                                                <td style="width:35%;">
                                                    <asp:TextBox ID="Txt_Medida" runat="server" style="width:65%;"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Medida" runat="server" Enabled="True" FilterType="Custom, Numbers" InvalidChars="'" TargetControlID="Txt_Medida" ValidChars="." ></cc1:FilteredTextBoxExtender>
                                                    <asp:Label ID="Lbl_Medida_M2" runat="server" Text="[metros]"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:13%;"><asp:Label ID="Lbl_Colindancia" runat="server" Text="Colindancia"></asp:Label></td>
                                                <td colspan="3"><asp:TextBox ID="Txt_Colindancia" runat="server" style="width:100%;" Rows="2" TextMode="MultiLine"></asp:TextBox></td>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Colindancia" runat="server" TargetControlID="Txt_Colindancia" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ " Enabled="True"></cc1:FilteredTextBoxExtender>
                                            </tr>
                                            <tr>
                                                <td colspan="4"><hr /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" style="text-align:right;">
                                                    <asp:Button ID="Btn_Agregar_Medida_Colindancia" runat="server" Text="AGREGAR" style="border-style:outset; background-color:White; width:150px; font-weight:bolder;" OnClick="Btn_Agregar_Medida_Colindancia_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4"><hr /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <div runat="server" id="Div_Listado_Medidas_Colindancias" style="width:800px; overflow:auto; height: 280px;">
                                                        <asp:GridView ID="Grid_Listado_Medidas_Colindancias" runat="server" 
                                                            AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                                            GridLines="None" 
                                                            OnRowDataBound="Grid_Listado_Medidas_Colindancias_RowDataBound"
                                                            PageSize="100" Width="800px" CssClass="GridView_1" AllowPaging="false">
                                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ORIENTACION" HeaderText="Orientación" SortExpression="ORIENTACION">
                                                                    <ItemStyle Width="60px" Font-Size="X-Small" HorizontalAlign="Center"/>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="MEDIDA" HeaderText="Medida [m]" SortExpression="MEDIDA">
                                                                    <ItemStyle Width="80px" Font-Size="X-Small" HorizontalAlign="Center"/>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="COLINDANCIA" HeaderText="Colindancia" SortExpression="COLINDANCIA" >
                                                                    <HeaderStyle Width="620px" Wrap="true" />
                                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Left" Width="620px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Quitar">
                                                                    <ItemStyle Width="40px" HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Btn_Quitar_Medida_Colindancia" runat="server" ToolTip="Quitar Medida y Colindancia" AlternateText="Quitar Medida y Colindancia" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" OnClick="Btn_Quitar_Medida_Colindancia_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="GridHeader" />
                                                            <PagerStyle CssClass="GridHeader" />
                                                            <RowStyle CssClass="GridItem" />
                                                            <SelectedRowStyle CssClass="GridSelected" />
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel  runat="server" HeaderText="Tab_Sustento_Juridico"  ID="Tab_Sustento_Juridico"  Width="100%">
                                    <HeaderTemplate>Sustento Jurídico</HeaderTemplate>
                                    <ContentTemplate>
                                        <table width="98%" class="estilo_fuente" border="0">
                                            <tr>
                                                <td colspan="4"><asp:HiddenField ID="Hdf_No_Registro_Juridico_Alta" runat="server" /> </td>
                                            </tr>
                                            <tr>
                                                <td style="width:15%;"><asp:Label ID="Lbl_Escritura" runat="server" Text="Escritura"></asp:Label></td>
                                                <td style="width:35%;">
                                                    <asp:TextBox ID="Txt_Escritura" runat="server" style="width:98%;" MaxLength="150"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Escritura" runat="server" TargetControlID="Txt_Escritura" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ " Enabled="True"></cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Fecha_Escritura" runat="server" Text="Fecha de Escritura"></asp:Label></td>
                                                <td style="width:35%;">
                                                    <asp:TextBox ID="Txt_Fecha_Escritura" runat="server" style="width:80%;"></asp:TextBox>
                                                    <asp:ImageButton ID="Btn_Fecha_Escritura" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                                    <cc1:CalendarExtender ID="CE_Txt_Fecha_Escritura" runat="server" 
                                                        TargetControlID="Txt_Fecha_Escritura" 
                                                        PopupButtonID="Btn_Fecha_Escritura" Format="dd/MMM/yyyy" Enabled="True">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:15%;"><asp:Label ID="Lbl_No_Notario" runat="server" Text="No. Notario"></asp:Label></td>
                                                <td style="width:35%;">
                                                    <asp:TextBox ID="Txt_No_Notario" runat="server" style="width:98%;" MaxLength="20"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Notario" runat="server" TargetControlID="Txt_No_Notario" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ  -_/" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Constacia_Registral" runat="server" Text="Constancia Reg."></asp:Label></td>
                                                <td style="width:35%;">
                                                    <asp:TextBox ID="Txt_Constacia_Registral" runat="server" style="width:98%;" MaxLength="100"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Constacia_Registral" runat="server" TargetControlID="Txt_Constacia_Registral" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ  -_/" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:15%;"><asp:Label ID="Lbl_Nombre_Notario" runat="server" Text="Nombre Notario"></asp:Label></td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="Txt_Nombre_Notario" runat="server" style="width:99%;" MaxLength="150"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Notario" runat="server" TargetControlID="Txt_Nombre_Notario" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ  -_/" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:15%;"><asp:Label ID="Lbl_Folio_Real" runat="server" Text="Folio Real"></asp:Label></td>
                                                <td style="width:35%;">
                                                    <asp:TextBox ID="Txt_Folio_Real" runat="server" style="width:98%;" MaxLength="100"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Folio_Real" runat="server" TargetControlID="Txt_Folio_Real" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ " Enabled="True"></cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Libertad_Gravament" runat="server" Text="Libre Gravamen"></asp:Label></td>
                                                <td style="width:35%;">
                                                    <asp:DropDownList ID="Cmb_Libertad_Gravament" runat="server" Width="100%">
                                                        <asp:ListItem Value="NO">NO</asp:ListItem>
                                                        <asp:ListItem Value="SI">SI</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:15%;"><asp:Label ID="Lbl_Antecedente_Registral" runat="server" Text="Antecedente"></asp:Label></td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="Txt_Antecedente_Registral" runat="server" style="width:99%;" MaxLength="100" TextMode="MultiLine"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Antecedente_Registral" runat="server" TargetControlID="Txt_Antecedente_Registral" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ " Enabled="True"></cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:15%;"><asp:Label ID="Lbl_No_Contrato_Juridico" runat="server" Text="No. Contrato"></asp:Label></td>
                                                <td style="width:35%;">
                                                    <asp:TextBox ID="Txt_No_Contrato_Juridico" runat="server" style="width:98%;" MaxLength="100"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Contrato_Juridico" runat="server" TargetControlID="Txt_No_Contrato_Juridico" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ " Enabled="True"></cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:15%;"><asp:Label ID="Lbl_Proveedor" runat="server" Text="Proveedor"></asp:Label></td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="Txt_Proveedor" runat="server" style="width:99%;" MaxLength="150"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Proveedor" runat="server" TargetControlID="Txt_Proveedor" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ " Enabled="True"></cc1:FilteredTextBoxExtender>    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <div  runat="server" id="Div_Campos_Juridico_Baja" style="width:99%">
                                                        <table border="0" width="100%" class="estilo_fuente">
                                                            <tr>
                                                                <td style="background-color:#4F81BD; color:White; font-weight:bolder; text-align:center;" colspan="4">CAMPOS NECESARIOS PARA LA BAJA DE UN BIEN INMUEBLE MUNICIPAL</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4"><asp:HiddenField ID="Hdf_No_Registro_Juridico_Baja" runat="server" /> </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width:15%;"><asp:Label ID="Lbl_Fecha_Baja" runat="server" Text="Fecha de Baja"></asp:Label></td>
                                                                <td style="width:35%;">
                                                                    <asp:TextBox ID="Txt_Fecha_Baja" runat="server" style="width:80%;"></asp:TextBox>
                                                                    <asp:ImageButton ID="Btn_Fecha_Baja" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                                                    <cc1:CalendarExtender ID="CE_Txt_Fecha_Baja" runat="server" 
                                                                        TargetControlID="Txt_Fecha_Baja" 
                                                                        PopupButtonID="Btn_Fecha_Baja" Format="dd/MMM/yyyy" Enabled="True">
                                                                    </cc1:CalendarExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width:15%;"><asp:Label ID="Lbl_Baja_No_Escritura" runat="server" Text="Escritura"></asp:Label></td>
                                                                <td style="width:35%;">
                                                                    <asp:TextBox ID="Txt_Baja_No_Escritura" runat="server" style="width:98%;" MaxLength="150"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Baja_No_Escritura" runat="server" TargetControlID="Txt_Baja_No_Escritura" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ  -_/" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                                </td>
                                                                <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Baja_Fecha_Escritura" runat="server" Text="Fecha de Escritura"></asp:Label></td>
                                                                <td style="width:35%;">
                                                                    <asp:TextBox ID="Txt_Baja_Fecha_Escritura" runat="server" style="width:80%;"></asp:TextBox>
                                                                    <asp:ImageButton ID="Btn_Baja_Fecha_Escritura" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                                                    <cc1:CalendarExtender ID="CE_Txt_Baja_Fecha_Escritura" runat="server" 
                                                                        TargetControlID="Txt_Baja_Fecha_Escritura" 
                                                                        PopupButtonID="Btn_Baja_Fecha_Escritura" Format="dd/MMM/yyyy" Enabled="True">
                                                                    </cc1:CalendarExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width:15%;"><asp:Label ID="Lbl_Baja_No_Notario" runat="server" Text="No. Notario"></asp:Label></td>
                                                                <td style="width:35%;">
                                                                    <asp:TextBox ID="Txt_Baja_No_Notario" runat="server" style="width:98%;" MaxLength="20"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Baja_No_Notario" runat="server" TargetControlID="Txt_Baja_No_Notario" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ  -_/" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                                </td>
                                                                <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Baja_Constancia_Registral" runat="server" Text="Constancia Reg."></asp:Label></td>
                                                                <td style="width:35%;">
                                                                    <asp:TextBox ID="Txt_Baja_Constancia_Registral" runat="server" style="width:98%;" MaxLength="100"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Baja_Constancia_Registral" runat="server" TargetControlID="Txt_Baja_Constancia_Registral" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ  -_/" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width:15%;"><asp:Label ID="Lbl_Baja_Nombre_Notario" runat="server" Text="Nombre Notario"></asp:Label></td>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="Txt_Baja_Nombre_Notario" runat="server" style="width:99%;" MaxLength="150"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Baja_Nombre_Notario" runat="server" TargetControlID="Txt_Baja_Nombre_Notario" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ  -_/" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width:15%;"><asp:Label ID="Lbl_Baja_Folio_Real" runat="server" Text="Folio Real"></asp:Label></td>
                                                                <td style="width:35%;">
                                                                    <asp:TextBox ID="Txt_Baja_Folio_Real" runat="server" style="width:98%;" MaxLength="100"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Baja_Folio_Real" runat="server" TargetControlID="Txt_Baja_Folio_Real" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ " Enabled="True"></cc1:FilteredTextBoxExtender>
                                                                </td>
                                                                <td style="width:15%;"><asp:Label ID="Lbl_Baja_No_Contrato" runat="server" Text="No. Contrato"></asp:Label></td>
                                                                <td style="width:35%;">
                                                                    <asp:TextBox ID="Txt_Baja_No_Contrato" runat="server" style="width:98%;" MaxLength="100"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Baja_No_Contrato" runat="server" TargetControlID="Txt_Baja_No_Contrato" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ " Enabled="True"></cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width:15%;"><asp:Label ID="Lbl_Baja_Nuevo_Propietario" runat="server" Text="Nuevo Propietario"></asp:Label></td>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="Txt_Baja_Nuevo_Propietario" runat="server" style="width:99%;" MaxLength="150"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Baja_Nuevo_Propietario" runat="server" TargetControlID="Txt_Baja_Nuevo_Propietario" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ " Enabled="True"></cc1:FilteredTextBoxExtender>    
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>    
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel  runat="server" HeaderText="Tab_Afectaciones"  ID="Tab_Afectaciones"  Width="100%">
                                    <HeaderTemplate>Afectaciones</HeaderTemplate>
                                    <ContentTemplate>
                                        <table width="98%" class="estilo_fuente" border="0">
                                            <tr>
                                                <td style="width:15%;"><asp:Label ID="Lbl_No_Contrato" runat="server" Text="No. Contrato"></asp:Label></td>
                                                <td style="width:35%;">
                                                    <asp:TextBox ID="Txt_No_Contrato" runat="server" style="width:98%;" MaxLength="100"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_No_Contrato" runat="server" TargetControlID="Txt_No_Contrato" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ  -_/" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Fecha_Afectacion" runat="server" Text="Fecha Afectación"></asp:Label></td>
                                                <td style="width:35%;">
                                                    <asp:TextBox ID="Txt_Fecha_Afectacion" runat="server" style="width:80%;"></asp:TextBox>
                                                    <asp:ImageButton ID="Btn_Fecha_Afectacion" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                                    <cc1:CalendarExtender ID="CE_Txt_Fecha_Afectacion" runat="server" 
                                                        TargetControlID="Txt_Fecha_Afectacion" 
                                                        PopupButtonID="Btn_Fecha_Afectacion" Format="dd/MMM/yyyy" Enabled="True">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:15%;"><asp:Label ID="Lbl_Nuevo_Propietario" runat="server" Text="Propietario"></asp:Label></td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="Txt_Nuevo_Propietario" runat="server" style="width:99%;" MaxLength="250"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nuevo_Propietario" runat="server" TargetControlID="Txt_Nuevo_Propietario" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ  -_/" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:15%;"><asp:Label ID="Lbl_Session_Ayuntamiento" runat="server" Text="Sessión Ayunt."></asp:Label></td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="Txt_Session_Ayuntamiento" runat="server" style="width:99%;" MaxLength="50"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Session_Ayuntamiento" runat="server" TargetControlID="Txt_Session_Ayuntamiento" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ  -_/" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:15%;"><asp:Label ID="Lbl_Tramo" runat="server" Text="Tramo"></asp:Label></td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="Txt_Tramo" runat="server" style="width:99%;" MaxLength="100"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Tramo" runat="server" TargetControlID="Txt_Tramo" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ  -_/" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <div runat="server" id="Div_Grid_Afectaciones" style="width:99.5%; overflow:auto; height: 280px;">
                                                        <asp:GridView ID="Grid_Afectaciones" runat="server" 
                                                            AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                                            GridLines="None"
                                                            OnRowDataBound="Grid_Afectaciones_RowDataBound"
                                                            PageSize="100" Width="100%" CssClass="GridView_1" AllowPaging="true">
                                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                                            <Columns>
                                                                <asp:BoundField DataField="NO_REGISTRO" SortExpression="NO_REGISTRO" HeaderText="NO_REGISTRO">
                                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="NO_CONTRATO" HeaderText="No. Contrato" SortExpression="NO_CONTRATO">
                                                                    <ItemStyle Width="110px" Font-Size="X-Small" HorizontalAlign="Center"/>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="FECHA_AFECTACION" HeaderText="Fecha Afectación" SortExpression="FECHA_AFECTACION" DataFormatString="{0:dd/MMM/yyyy}">
                                                                    <ItemStyle Width="130px" Font-Size="X-Small" HorizontalAlign="Center"/>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="PROPIETARIO" HeaderText="Nuevo Propietario" SortExpression="PROPIETARIO">
                                                                    <ItemStyle Width="110px" Font-Size="X-Small" HorizontalAlign="Center"/>
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Ver">
                                                                    <ItemStyle Width="40px" HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Btn_Ver_Detalle_Afectaciones" runat="server" ToolTip="Ver Detalle" AlternateText="Ver Detalle" ImageUrl="~/paginas/imagenes/gridview/grid_info.png" OnClick="Btn_Ver_Detalle_Afectaciones_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="SESSION_AYUNTAMIENTO" SortExpression="SESSION_AYUNTAMIENTO" HeaderText="SESSION_AYUNTAMIENTO">
                                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="TRAMO" SortExpression="TRAMO" HeaderText="TRAMO">
                                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="USUARIO_CREO" SortExpression="USUARIO_CREO" HeaderText="USUARIO_CREO">
                                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="FECHA_CREO" SortExpression="FECHA_CREO" HeaderText="FECHA_CREO">
                                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="GridHeader" />
                                                            <PagerStyle CssClass="GridHeader" />
                                                            <RowStyle CssClass="GridItem" />
                                                            <SelectedRowStyle CssClass="GridSelected" />
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table> 
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" HeaderText="Tab_Contabilidad" ID="Tab_Contabilidad" Width="100%">
                                    <HeaderTemplate>Contabilidad</HeaderTemplate>
                                    <ContentTemplate>
                                        <table border="0" width="98%" class="estilo_fuente">
                                            <tr>
                                                <td style="width:15%;"><asp:Label ID="Lbl_Cont_Hoja" runat="server" Text="Hoja"></asp:Label></td>
                                                <td style="width:35%;">
                                                    <asp:TextBox ID="Txt_Cont_Hoja" runat="server" style="width:98%;" MaxLength="20"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cont_Hoja" runat="server" TargetControlID="Txt_Cont_Hoja" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ  -_/" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td style="width:15%;"><asp:Label ID="Lbl_Cont_Tomo" runat="server" Text="Tomo"></asp:Label></td>
                                                <td style="width:35%;">
                                                    <asp:TextBox ID="Txt_Cont_Tomo" runat="server" style="width:98%;" MaxLength="20"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cont_Tomo" runat="server" TargetControlID="Txt_Cont_Tomo" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ  -_/" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:15%;"><asp:Label ID="Lbl_Cont_Numero_Acta" runat="server" Text="No. Acta"></asp:Label></td>
                                                <td style="width:35%;">
                                                    <asp:TextBox ID="Txt_Cont_Numero_Acta" runat="server" style="width:98%;" MaxLength="20"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cont_Numero_Acta" runat="server" TargetControlID="Txt_Cont_Numero_Acta" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ  -_/" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td style="width:15%;"><asp:Label ID="Lbl_Cont_Cartilla_Parcelaria" runat="server" Text="Cartilla Parc."></asp:Label></td>
                                                <td style="width:35%;">
                                                    <asp:TextBox ID="Txt_Cont_Cartilla_Parcelaria" runat="server" style="width:98%;" MaxLength="20"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cont_Cartilla_Parcelaria" runat="server" TargetControlID="Txt_Cont_Cartilla_Parcelaria" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ  -_/" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:15%;"><asp:Label ID="Lbl_Cont_Superficie" runat="server" Text="Superficie"></asp:Label></td>
                                                <td style="width:35%;">
                                                    <asp:TextBox ID="Txt_Cont_Superficie" runat="server" style="width:98%;"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cont_Superficie" runat="server" TargetControlID="Txt_Cont_Superficie" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers" ValidChars="." Enabled="True"></cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td style="width:15%;"><asp:Label ID="Lbl_Cont_Unidad_Superficie" runat="server" Text="Unidad Superficie"></asp:Label></td>
                                                <td style="width:35%;">
                                                    <asp:TextBox ID="Txt_Cont_Unidad_Superficie" runat="server" style="width:98%;"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Cont_Unidad_Superficie" runat="server" TargetControlID="Txt_Cont_Unidad_Superficie" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ  -_/" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:15%;">
                                                    <asp:Label ID="Lbl_Clase_Activo" runat="server" Text="Clase Activo"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="Cmb_Clase_Activo" runat="server" Width="100%">
                                                        <asp:ListItem Value="SELECCIONE">&lt;SELECCIONE&gt;</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel  runat="server" HeaderText="Tab_Observaciones" ID="Tab_Observaciones"  Width="100%">
                                    <HeaderTemplate>Observaciones</HeaderTemplate>
                                    <ContentTemplate>
                                        <div runat="server" id="Div_Grid_Observaciones" style="width:98%; overflow:auto; height: 380px;">
                                            <asp:GridView ID="Grid_Observaciones" runat="server" 
                                                AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                                GridLines="None"
                                                PageSize="100" Width="100%" CssClass="GridView_1" AllowPaging="true">
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                <Columns>
                                                    <asp:BoundField DataField="NO_OBSERVACION" HeaderText="NO_OBSERVACION" SortExpression="NO_OBSERVACION"  >
                                                        <ItemStyle Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FECHA_OBSERVACION" HeaderText="Fecha / Hora" SortExpression="FECHA_OBSERVACION" DataFormatString="{0:dd-MMM-yyyy, HH:mm:ss tt}">
                                                        <ItemStyle Width="170px" Font-Size="X-Small" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="USUARIO_CREO" HeaderText="Autor" SortExpression="USUARIO_CREO">
                                                        <ItemStyle Width="240px" Font-Size="X-Small" HorizontalAlign="Center"/>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="OBSERVACION" HeaderText="Observaciones" SortExpression="OBSERVACION">
                                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Left"/>
                                                    </asp:BoundField>
                                                </Columns>
                                                <HeaderStyle CssClass="GridHeader" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <RowStyle CssClass="GridItem" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel  runat="server" HeaderText="Tab_Expropiaciones"  ID="Tab_Expropiaciones"  Width="100%">
                                    <HeaderTemplate>Expropiaciones</HeaderTemplate>
                                    <ContentTemplate>
                                        <table border="0" width="98%" class="estilo_fuente">
                                            <tr>
                                                <td style="width:15%;"><asp:Label ID="Lbl_Expropiacion" runat="server" Text="Descripción"></asp:Label></td>
                                                <td colspan="3"><asp:TextBox ID="Txt_Expropiacion" runat="server" style="width:99%;" Rows="3" TextMode="MultiLine"></asp:TextBox></td>
                                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Expropiacion" runat="server" TargetControlID="Txt_Expropiacion" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ " Enabled="True"></cc1:FilteredTextBoxExtender>
                                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Expropiacion" runat="server" TargetControlID ="Txt_Expropiacion" WatermarkText="<< Para Agregar una Nueva Expropiación >>" WatermarkCssClass="watermarked" Enabled="True"/>    
                                            </tr>
                                        </table>
                                        <div runat="server" id="Div_Expropiaciones" style="width:98%; overflow:auto; height: 350px;">
                                            <asp:GridView ID="Grid_Expropiaciones" runat="server" 
                                                AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                                GridLines="None"
                                                PageSize="100" Width="100%" CssClass="GridView_1" AllowPaging="true">
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                <Columns>
                                                    <asp:BoundField DataField="NO_EXPROPIACION" HeaderText="NO_EXPROPIACION" SortExpression="NO_EXPROPIACION"  >
                                                        <ItemStyle Font-Size="X-Small" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FECHA_EXPROPIACION" HeaderText="Fecha / Hora" SortExpression="FECHA_EXPROPIACION" DataFormatString="{0:dd-MMM-yyyy, HH:mm:ss tt}">
                                                        <ItemStyle Width="170px" Font-Size="X-Small" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="USUARIO_CREO" HeaderText="Autor" SortExpression="USUARIO_CREO">
                                                        <ItemStyle Width="240px" Font-Size="X-Small" HorizontalAlign="Center"/>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" SortExpression="DESCRIPCION">
                                                        <ItemStyle Font-Size="X-Small" HorizontalAlign="Left"/>
                                                    </asp:BoundField>
                                                </Columns>
                                                <HeaderStyle CssClass="GridHeader" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <RowStyle CssClass="GridItem" />
                                                <SelectedRowStyle CssClass="GridSelected" />
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel  runat="server" HeaderText="Tab_Anexos_Archivos"  ID="Tab_Anexos_Archivos"  Width="100%">
                                    <HeaderTemplate>Anexos</HeaderTemplate>
                                    <ContentTemplate>
                                        <table width="98%" class="estilo_fuente" border="0">
                                            <tr>
                                                <td style="width:15%;"><asp:Label ID="Lbl_Tipo_Archivo" runat="server" Text="Tipo" ></asp:Label></td>
                                                <td style="width:35%;">
                                                    <asp:DropDownList ID="Cmb_Tipo_Archivo" runat="server" style="width:100%;">
                                                        <asp:ListItem Value="">&lt;SELECCIONE&gt;</asp:ListItem>
                                                        <asp:ListItem Value="ESCRITURA">ESCRITURA</asp:ListItem>
                                                        <asp:ListItem Value="FOTOGRAFIA">FOTOGRAFÍA</asp:ListItem>
                                                        <asp:ListItem Value="LEVANTAMIENTO_TOPOGRAFICO">LEVANTAMIENTO TOPÓGRAFICO</asp:ListItem>
                                                        <asp:ListItem Value="MAPA">MAPA</asp:ListItem>
                                                        <asp:ListItem Value="OBRA_PUBLICA">OBRA PÚBLICA</asp:ListItem>
                                                        <asp:ListItem Value="OTRO">OTRO DOCUMENTO</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width:15%; text-align:right;">
                                                    <asp:Label ID="Lbl_Ruta_Archivo" runat="server" Text="Archivo" ></asp:Label>
                                                    <asp:Label ID="Throbber" Text="wait" runat="server"  Width="30px">                                                                     
                                                        <div id="Div1" class="progressBackgroundFilter"></div>
                                                        <div  class="processMessage" id="div2">
                                                            <img alt="" src="../imagenes/paginas/Updating.gif" />
                                                        </div>
                                                    </asp:Label>
                                                </td>
                                                <td style="width:35%;">
                                                    <cc1:AsyncFileUpload ID="AFU_Ruta_Archivo" runat="server" Width="300px" 
                                                        ThrobberID="Throbber" ForeColor="White" Font-Bold="True" 
                                                        CompleteBackColor="LightBlue" UploadingBackColor="LightGray" 
                                                        FailedValidation="False"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:13%;"><asp:Label ID="Lbl_Descripcion_Archivo" runat="server" Text="Descripción"></asp:Label></td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="Txt_Descripcion_Archivo" runat="server" style="width:100%;" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Descripcion_Archivo" runat="server" TargetControlID="Txt_Descripcion_Archivo" InvalidChars="<,>,&,',!," FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ-%/#@*+_ " Enabled="True"></cc1:FilteredTextBoxExtender>    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4"><hr /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" style="text-align:center;">
                                                    <asp:Label ID="Lbl_Nota_Archivos" runat="server" Text="*Nota: El Archivo se Agregará en cuanto se Guarde la Actualización del Bien Mueble." Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4"><hr /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <div runat="server" id="Div_Listado_Anexos" style="width:98%; overflow:auto; height: 280px;">
                                                        <asp:GridView ID="Grid_Listado_Anexos" runat="server" 
                                                            AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                                            GridLines="None"
                                                            OnRowDataBound="Grid_Listado_Anexos_RowDataBound"
                                                            PageSize="100" Width="100%" CssClass="GridView_1" AllowPaging="true">
                                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                                            <Columns>
                                                                <asp:BoundField DataField="NO_REGISTRO" SortExpression="SEGUN" HeaderText="Según">
                                                                    <ItemStyle Width="170px" Font-Size="X-Small" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RUTA_ARCHIVO" SortExpression="Ruta" HeaderText="RUTA_ARCHIVO">
                                                                    <ItemStyle Width="170px" Font-Size="X-Small" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Ver">
                                                                    <ItemStyle Width="40px" HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Btn_Ver_Anexo" runat="server" ToolTip="Ver Anexo" AlternateText="Ver Anexo" ImageUrl="~/paginas/imagenes/paginas/icono_consultar.png" Width="16px" OnClick="Btn_Ver_Anexo_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="TIPO_ARCHIVO" HeaderText="Tipo" SortExpression="TIPO_ARCHIVO">
                                                                    <ItemStyle Width="60px" Font-Size="X-Small" HorizontalAlign="Center"/>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="FECHA_CARGO" HeaderText="Fecha / Hora" SortExpression="FECHA_CARGO" DataFormatString="{0:dd-MMM-yyyy, HH:mm:ss tt}">
                                                                    <ItemStyle Width="150px" Font-Size="X-Small" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="USUARIO_CREO" HeaderText="Autor" SortExpression="USUARIO_CREO">
                                                                    <ItemStyle Width="220px" Font-Size="X-Small" HorizontalAlign="Center"/>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DESCRIPCION_ARCHIVO" HeaderText="Descripción" SortExpression="DESCRIPCION_ARCHIVO">
                                                                    <ItemStyle Font-Size="X-Small" HorizontalAlign="Left"/>
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Eliminar">
                                                                    <ItemStyle Width="40px" HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Btn_Eliminar_Anexo" runat="server" ToolTip="Eliminar Anexo" AlternateText="Eliminar Anexo" ImageUrl="~/paginas/imagenes/gridview/grid_garbage.png" OnClientClick="return confirm('¿Esta seguro que desea eliminar el registro de la Base de Datos?');" OnClick="Btn_Eliminar_Anexo_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="GridHeader" />
                                                            <PagerStyle CssClass="GridHeader" />
                                                            <RowStyle CssClass="GridItem" />
                                                            <SelectedRowStyle CssClass="GridSelected" />
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                             </cc1:TabContainer>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID="UpPnl_Aux_Mpe_Calles" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_Mpe_Calles_Cabecera" runat="server" Text="" style="display:none;"/>
            <cc1:ModalPopupExtender ID="Mpe_Calles_Cabecera" runat="server" 
                                    TargetControlID="Btn_Comodin_Mpe_Calles_Cabecera" PopupControlID="Pnl_Mpe_Calles" 
                                    CancelControlID="Btn_Cerrar_Mpe_Calles" PopupDragHandleControlID="Pnl_Mpe_Calles_Interno"
                                    DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:Panel ID="Pnl_Mpe_Calles" runat="server" CssClass="drag" HorizontalAlign="Center" style="display:none;border-style:outset;border-color:Silver;width:760px;" >                
    <asp:Panel ID="Pnl_Mpe_Calles_Interno" runat="server" CssClass="estilo_fuente" style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table class="estilo_fuente" width="100%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;width:90%">
                   <asp:Image ID="Img_Mpe_Productos" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Busqueda y Selección de Calle
                </td>
                <td align="right">
                   <asp:ImageButton ID="Btn_Cerrar_Mpe_Calles" CausesValidation="false" runat="server" style="cursor:pointer;" 
                        ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
     </asp:Panel>
       <center>
        <asp:UpdatePanel ID="UpPnl_Mpe_Calles" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:UpdateProgress ID="UpPgr_Mpe_Calles" runat="server" AssociatedUpdatePanelID="UpPnl_Mpe_Calles" DisplayAfter="0">
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
                                    <asp:Label ID="Lbl_Nombre_Calles_Buscar" runat="server" CssClass="estilo_fuente" Text="Introducir" style="font-weight:bolder;" ></asp:Label>
                                </td>
                                <td style="width:85%; text-align:left;">
                                    <asp:TextBox ID="Txt_Nombre_Calles_Buscar" runat="server" Width="92%" AutoPostBack="true" OnTextChanged="Txt_Nombre_Calles_Buscar_TextChanged" ></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Calles_Buscar" runat="server" TargetControlID="Txt_Nombre_Calles_Buscar" InvalidChars="<,>,&,',!," 
                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                                    </cc1:FilteredTextBoxExtender>  
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Nombre_Calles_Buscar" runat="server" Enabled="True" TargetControlID="Txt_Nombre_Calles_Buscar" WatermarkCssClass="watermarked" 
                                                     WatermarkText="<-- Nombre de la Calle ó Colonia -->">
                                    </cc1:TextBoxWatermarkExtender>
                                    <asp:ImageButton ID="Btn_Ejecutar_Busqueda_Calles" runat="server" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" ToolTip="Buscar Calles" AlternateText="Buscar" OnClick="Btn_Ejecutar_Busqueda_Calles_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        
                            <asp:Panel ID="Pnl_Listado_Calles" runat="server" ScrollBars="Vertical" style="white-space:normal;"
                                Width="100%" BorderColor="#3366FF" Height="330px">
                                       <asp:GridView ID="Grid_Listado_Calles" runat="server" 
                                         AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                         GridLines="None"
                                         OnPageIndexChanging="Grid_Listado_Calles_PageIndexChanging"
                                         OnSelectedIndexChanged="Grid_Listado_Calles_SelectedIndexChanged"
                                         PageSize="100" Width="100%" CssClass="GridView_1" AllowPaging="true">
                                         <AlternatingRowStyle CssClass="GridAltItem" />
                                         <Columns>
                                             <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                 ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                 <ItemStyle Width="30px" />
                                             </asp:ButtonField>
                                             <asp:BoundField DataField="CALLE_ID" HeaderText="Calle ID" SortExpression="CALLE_ID"  >
                                                 <ItemStyle Width="30px" Font-Size="X-Small" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="NOMBRE_CALLE" HeaderText="Calle" SortExpression="NOMBRE_CALLE">
                                                <ItemStyle Font-Size="X-Small"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="COLONIA" HeaderText="Colonia" SortExpression="COLONIA" >
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
    
    <asp:UpdatePanel ID="UpPnl_Aux_Mpe_Cuentas_Predial" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_Mpe_Cuentas_Predial_Cabecera" runat="server" Text="" style="display:none;"/>
            <cc1:ModalPopupExtender ID="Mpe_Cuentas_Predial_Cabecera" runat="server" 
                                    TargetControlID="Btn_Comodin_Mpe_Cuentas_Predial_Cabecera" PopupControlID="Pnl_Mpe_Cuentas_Predial" 
                                    CancelControlID="Btn_Cerrar_Mpe_Cuentas_Predial" PopupDragHandleControlID="Pnl_Mpe_Cuentas_Predial_Interno"
                                    DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>
        </ContentTemplate>
    </asp:UpdatePanel>
       
    <asp:Panel ID="Pnl_Mpe_Cuentas_Predial" runat="server" CssClass="drag" HorizontalAlign="Center" style="display:none;border-style:outset;border-color:Silver;width:760px;" >                
    <asp:Panel ID="Pnl_Mpe_Cuentas_Predial_Interno" runat="server" CssClass="estilo_fuente"
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table class="estilo_fuente" width="100%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;width:90%">
                   <asp:Image ID="Image1" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Busqueda y Selección de Cuentas Predial
                </td>
                <td align="right">
                   <asp:ImageButton ID="Btn_Cerrar_Mpe_Cuentas_Predial" CausesValidation="false" runat="server" style="cursor:pointer;" 
                        ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
     </asp:Panel>
       <center>
        <asp:UpdatePanel ID="UpPnl_Mpe_Cuentas_Predial" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                   <asp:UpdateProgress ID="UpPgr_Mpe_Productos" runat="server" AssociatedUpdatePanelID="UpPnl_Mpe_Cuentas_Predial" DisplayAfter="0">
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
                                    <asp:Label ID="Lbl_Nombre_Cuenta_Predial_Buscar" runat="server" CssClass="estilo_fuente" Text="Introducir" style="font-weight:bolder;" ></asp:Label>
                                </td>
                                <td style="width:85%; text-align:left;">
                                    <asp:TextBox ID="Txt_Nombre_Cuenta_Predial_Buscar" runat="server" Width="92%" AutoPostBack="true"  ontextchanged="Txt_Nombre_Cuenta_Predial_Buscar_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Cuenta_Predial_Buscar" runat="server" TargetControlID="Txt_Nombre_Cuenta_Predial_Buscar" InvalidChars="<,>,&,',!," 
                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                                    </cc1:FilteredTextBoxExtender>  
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Nombre_Cuenta_Predial_Buscar" runat="server" Enabled="True" TargetControlID="Txt_Nombre_Cuenta_Predial_Buscar" WatermarkCssClass="watermarked" 
                                                     WatermarkText="<-- No. Cuenta Predial -->">
                                    </cc1:TextBoxWatermarkExtender>
                                    <asp:ImageButton ID="Btn_Ejecutar_Busqueda_Cuenta_Predial" runat="server" OnClick="Btn_Ejecutar_Busqueda_Cuenta_Predial_Click"
                                         ImageUrl="~/paginas/imagenes/paginas/busqueda.png"  
                                         ToolTip="Buscar Cuenta_Predial" AlternateText="Buscar" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        
                            <asp:Panel ID="Pnl_Listado_Cuentas_Predial" runat="server" ScrollBars="Vertical" style="white-space:normal;"
                                Width="100%" BorderColor="#3366FF" Height="330px">
                                       <asp:GridView ID="Grid_Listado_Cuentas_Predial" runat="server" 
                                         AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                         GridLines="None" 
                                         OnPageIndexChanging="Grid_Listado_Cuentas_Predial_PageIndexChanging"
                                         OnSelectedIndexChanged="Grid_Listado_Cuentas_Predial_SelectedIndexChanged"
                                         PageSize="100" Width="100%" CssClass="GridView_1" AllowPaging="true">
                                         <AlternatingRowStyle CssClass="GridAltItem" />
                                         <Columns>
                                             <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                 ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                 <ItemStyle Width="30px" />
                                             </asp:ButtonField>
                                             <asp:BoundField DataField="CUENTA_PREDIAL_ID" HeaderText="Cuenta Predial ID" SortExpression="CUENTA_PREDIAL_ID"  >
                                                 <ItemStyle Width="30px" Font-Size="X-Small" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="CUENTA_PREDIAL" HeaderText="No. Cuenta Predial" SortExpression="CUENTA_PREDIAL">
                                                <ItemStyle Width="100px" Font-Size="X-Small"/>
                                             </asp:BoundField>
                                             <asp:BoundField DataField="NOMBRE_CALLE" HeaderText="Calle" SortExpression="NOMBRE_CALLE" >
                                                 <ItemStyle Font-Size="X-Small" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="NOMBRE_COLONIA" HeaderText="Colonia" SortExpression="NOMBRE_COLONIA">
                                                 <ItemStyle Font-Size="X-Small" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="NO_EXTERIOR" HeaderText="# Exterior" SortExpression="NO_EXTERIOR">
                                                 <ItemStyle Width="90px" Font-Size="X-Small" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="NO_INTERIOR" HeaderText="# Interior" SortExpression="NO_INTERIOR">
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
   
    <asp:UpdatePanel ID="UpPnl_Detalles_Afectaciones" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_Afectaciones" runat="server" Text="" style="display:none;"/>
            <cc1:ModalPopupExtender ID="Mpe_Detalles_Afectaciones" runat="server" 
                                    TargetControlID="Btn_Comodin_Afectaciones" PopupControlID="Pnl_Mpe_Detalles_Afectaciones" 
                                    CancelControlID="Btn_Cerrar_Mpe_Detalles_Afectaciones" PopupDragHandleControlID="Pnl_Mpe_Detalles_Afectaciones_Interno"
                                    DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:Panel ID="Pnl_Mpe_Detalles_Afectaciones" runat="server" CssClass="drag" HorizontalAlign="Center" style="display:none;border-style:outset;border-color:Silver;width:760px;" >                
    <asp:Panel ID="Pnl_Mpe_Detalles_Afectaciones_Interno" runat="server" CssClass="estilo_fuente" style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table class="estilo_fuente" width="100%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;width:90%">
                   <asp:Image ID="Image2" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Información de la Afectación
                </td>
                <td align="right">
                   <asp:ImageButton ID="Btn_Cerrar_Mpe_Detalles_Afectaciones" CausesValidation="false" runat="server" style="cursor:pointer;" 
                        ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
     </asp:Panel>
       <center>
        <asp:UpdatePanel ID="UpPnl_Mpe_Detalles_Afectaciones" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:UpdateProgress ID="UpPgr_Mpe_Detalles_Afectaciones" runat="server" AssociatedUpdatePanelID="UpPnl_Mpe_Detalles_Afectaciones" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                   </ProgressTemplate>                     
                </asp:UpdateProgress>
                <br />
                    <div style="border-style: outset; width: 95%; height: 150px; background-color: White;">
                        <table width="100%" class="estilo_fuente">
                            <tr>
                                <td style="width:15%;"><asp:Label ID="Lbl_Mpe_No_Contrato" runat="server" Text="No. Contrato"></asp:Label></td>
                                <td style="width:35%;"><asp:TextBox ID="Txt_Mpe_No_Contrato" runat="server" style="width:98%;" Enabled="false"></asp:TextBox></td>
                                <td style="width:15%;"><asp:Label ID="Lbl_Mpe_Fecha_Afectacion" runat="server" Text="Fecha Afectación"></asp:Label></td>
                                <td style="width:35%;"><asp:TextBox ID="Txt_Mpe_Fecha_Afectacion" runat="server" style="width:98%;" Enabled="false"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width:15%;"><asp:Label ID="Lbl_Mpe_Nuevo_Propietario" runat="server" Text="Nuevo Propietario"></asp:Label></td>
                                <td colspan="3"><asp:TextBox ID="Txt_Mpe_Nuevo_Propietario" runat="server" style="width:99%;" Enabled="false"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width:15%;"><asp:Label ID="Lbl_Mpe_Session_Ayuntamiento" runat="server" Text="Sessión Ayunt."></asp:Label></td>
                                <td colspan="3"><asp:TextBox ID="Txt_Mpe_Session_Ayuntamiento" runat="server" style="width:99%;" Enabled="false"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width:15%;"><asp:Label ID="Lbl_Mpe_Tramo" runat="server" Text="Tramo"></asp:Label></td>
                                <td colspan="3"><asp:TextBox ID="Txt_Mpe_Tramo" runat="server" style="width:99%;" Enabled="false"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td colspan="4"><hr /></td>
                            </tr>
                            <tr>
                                <td style="width:15%;"><asp:Label ID="Lbl_Mpe_Datos_Auditoria" runat="server" Text="Creación"></asp:Label></td>
                                <td colspan="3"><asp:TextBox ID="Txt_Mpe_Datos_Auditoria" runat="server" style="width:99%;" Enabled="false"></asp:TextBox></td>
                            </tr>
                        </table>
                    </div>
                <br />
                </ContentTemplate>
            </asp:UpdatePanel>
       </center>          
    </asp:Panel>
    
    <asp:UpdatePanel ID="UpPnl_Aux_Mpe_Colonias" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="Btn_Comodin_Mpe_Colonias" runat="server" Text="" style="display:none;"/>
            <cc1:ModalPopupExtender ID="Mpe_Colonias" runat="server" 
                                    TargetControlID="Btn_Comodin_Mpe_Colonias" PopupControlID="Pnl_Mpe_Colonias" 
                                    CancelControlID="Btn_Cerrar_Mpe_Colonias" PopupDragHandleControlID="Pnl_Mpe_Colonias_Interno"
                                    DropShadow="True" BackgroundCssClass="progressBackgroundFilter"/>
        </ContentTemplate>
    </asp:UpdatePanel>
       
    <asp:Panel ID="Pnl_Mpe_Colonias" runat="server" CssClass="drag" HorizontalAlign="Center" style="display:none;border-style:outset;border-color:Silver;width:760px;" >                
    <asp:Panel ID="Pnl_Mpe_Colonias_Interno" runat="server" CssClass="estilo_fuente"
        style="cursor: move;background-color:Silver;color:Black;font-size:12;font-weight:bold;border-style:outset;">
        <table class="estilo_fuente" width="100%">
            <tr>
                <td style="color:Black;font-size:12;font-weight:bold;width:90%">
                   <asp:Image ID="Image3" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
                     Busqueda y Selección de Cuentas Predial
                </td>
                <td align="right">
                   <asp:ImageButton ID="Btn_Cerrar_Mpe_Colonias" CausesValidation="false" runat="server" style="cursor:pointer;" 
                        ToolTip="Cerrar Ventana" ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png"/>  
                </td>
            </tr>
        </table>            
     </asp:Panel>
       <center>
        <asp:UpdatePanel ID="UpPnl_Mpe_Colonias" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                   <asp:UpdateProgress ID="UpPgr_Mpe_Colonias" runat="server" AssociatedUpdatePanelID="UpPnl_Mpe_Colonias" DisplayAfter="0">
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
                                    <asp:Label ID="Lbl_Nombre_Colonia_Buscar" runat="server" CssClass="estilo_fuente" Text="Introducir" style="font-weight:bolder;" ></asp:Label>
                                </td>
                                <td style="width:85%; text-align:left;">
                                    <asp:TextBox ID="Txt_Nombre_Colonia_Buscar" runat="server" Width="92%" AutoPostBack="true"  ontextchanged="Txt_Nombre_Colonia_Buscar_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FTE_Txt_Nombre_Colonia_Buscar" runat="server" TargetControlID="Txt_Nombre_Colonia_Buscar" InvalidChars="<,>,&,',!," 
                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"  ValidChars="Ññ.,:;()áéíóúÁÉÍÓÚ " Enabled="True">   
                                    </cc1:FilteredTextBoxExtender>  
                                    <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Nombre_Colonia_Buscar" runat="server" Enabled="True" TargetControlID="Txt_Nombre_Colonia_Buscar" WatermarkCssClass="watermarked" 
                                                     WatermarkText="<-- Nombre de la Colonia -->">
                                    </cc1:TextBoxWatermarkExtender>
                                    <asp:ImageButton ID="Btn_Ejecutar_Busqueda_Colonia" runat="server" OnClick="Btn_Ejecutar_Busqueda_Colonia_Click"
                                         ImageUrl="~/paginas/imagenes/paginas/busqueda.png"  
                                         ToolTip="Buscar Colonia" AlternateText="Buscar" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        
                            <asp:Panel ID="Pnl_Grid_Listado_Colonias" runat="server" ScrollBars="Vertical" style="white-space:normal;"
                                Width="100%" BorderColor="#3366FF" Height="330px">
                                       <asp:GridView ID="Grid_Listado_Colonias" runat="server" 
                                         AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                         GridLines="None" 
                                         OnPageIndexChanging="Grid_Listado_Colonias_PageIndexChanging"
                                         OnSelectedIndexChanged="Grid_Listado_Colonias_SelectedIndexChanged"
                                         PageSize="100" Width="100%" CssClass="GridView_1" AllowPaging="true">
                                         <AlternatingRowStyle CssClass="GridAltItem" />
                                         <Columns>
                                             <asp:ButtonField ButtonType="Image" CommandName="Select" 
                                                 ImageUrl="~/paginas/imagenes/gridview/blue_button.png">
                                                 <ItemStyle Width="30px" />
                                             </asp:ButtonField>
                                             <asp:BoundField DataField="COLONIA_ID" HeaderText="COLONIA_ID" SortExpression="COLONIA_ID"  >
                                                 <ItemStyle Width="30px" Font-Size="X-Small" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="NOMBRE_COLONIA" HeaderText="Colonia" SortExpression="NOMBRE_COLONIA">
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
    
</asp:Content>


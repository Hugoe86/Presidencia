<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Est_Pat_Bienes_Inmuebles.aspx.cs" Inherits="paginas_Control_Patrimonial_Frm_Est_Pat_Bienes_Inmuebles" Title="Estadisticas Bienes Inmuebles" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" language="javascript">
        //Metodos para limpiar los controles de la busqueda.
        function Limpiar_Ctlr_Campos(){ 
            document.getElementById("<%=Chk_Areas_Donacion.ClientID%>").checked = false;  
            document.getElementById("<%=Chk_Estatus.ClientID%>").checked = false;   
                document.getElementById("<%=Hdf_Calle_ID.ClientID%>").value=""; 
                document.getElementById("<%=Txt_Calle.ClientID%>").value=""; 
                document.getElementById("<%=Hdf_Colonia_ID.ClientID%>").value=""; 
                document.getElementById("<%=Txt_Colonia.ClientID%>").value=""; 
                document.getElementById("<%=Cmb_Uso.ClientID%>").value=""; 
                document.getElementById("<%=Cmb_Destino.ClientID%>").value=""; 
                document.getElementById("<%=Cmb_Origen.ClientID%>").value=""; 
                document.getElementById("<%=Cmb_Estatus.ClientID%>").value=""; 
                document.getElementById("<%=Cmb_Area_Donacion.ClientID%>").value=""; 
                document.getElementById("<%=Cmb_Tipo_Bien.ClientID%>").value=""; 
                document.getElementById("<%=Cmb_Sector.ClientID%>").value=""; 
                document.getElementById("<%=Cmb_Clasificacion_Zona.ClientID%>").value=""; 
                document.getElementById("<%=Cmb_Clase_Activo.ClientID%>").value=""; 
                document.getElementById("<%=Cmb_Estado.ClientID%>").value=""; 
                document.getElementById("<%=Cmb_Libertad_Gravament.ClientID%>").value=""; 
                document.getElementById("<%=Hdf_Cuenta_Predial_ID.ClientID%>").value=""; 
                document.getElementById("<%=Txt_Numero_Cuenta_Predial.ClientID%>").value=""; 
                document.getElementById("<%=Cmb_Tipo_Predio.ClientID%>").value=""; 
                document.getElementById("<%=Txt_Superficie_Hasta.ClientID%>").value=""; 
                document.getElementById("<%=Txt_Superficie_Desde.ClientID%>").value=""; 
                document.getElementById("<%=Txt_Valor_Comercial_Hasta.ClientID%>").value=""; 
                document.getElementById("<%=Txt_Valor_Comercial_Desde.ClientID%>").value=""; 
                document.getElementById("<%=Txt_Fecha_Registro_Inicio.ClientID%>").value="__/___/____"; 
                document.getElementById("<%=Txt_Fecha_Registro_Fin.ClientID%>").value="__/___/____"; 
                document.getElementById("<%=Txt_Fecha_Escritura_Inicio.ClientID%>").value="__/___/____"; 
                document.getElementById("<%=Txt_Fecha_Escritura_Fin.ClientID%>").value="__/___/____"; 
                document.getElementById("<%=Txt_Fecha_Baja_Inicio.ClientID%>").value="__/___/____"; 
                document.getElementById("<%=Txt_Fecha_Baja_Fin.ClientID%>").value="__/___/____"; 
            return false;
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
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <center>
                    <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">              
                        <tr align="center">
                            <td class="label_titulo" colspan="2">Estadisticas de Bienes Inmuebles</td>
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
                            <td>&nbsp;</td>                        
                        </tr>
                        <tr class="barra_busqueda" align="right">
                            <td align="left">
                                <asp:ImageButton ID="Btn_Generar_Reporte_PDF" runat="server" ToolTip="Generar (Pdf)" Width="24px" ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" OnClick="Btn_Generar_Reporte_PDF_Click" />
                                <asp:ImageButton ID="Btn_Limpiar_Filtros" runat="server" ToolTip="Limpiar Filtros" Width="24px" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" OnClientClick="javascript:return Limpiar_Ctlr_Campos();" />
                            </td>
                            <td>&nbsp;</td>                        
                        </tr>
                    </table>   
                </center>
                <br />
                <center>
                    <table width="98%" class="estilo_fuente">  
                        <tr>
                            <td style="width:15%">
                                <asp:HiddenField runat="server" ID="Hdf_Calle_ID" />
                                <asp:Label ID="Lbl_Calle" runat="server" Text="Calle"></asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="Txt_Calle" runat="server" style="width:93%;" Enabled="false"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Buscar_Calle" runat="server" ToolTip="Buscar Calle" AlternateText="Bucar Calle" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Calle_Click"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%">
                                <asp:HiddenField runat="server" ID="Hdf_Colonia_ID" />
                                <asp:Label ID="Lbl_Colonia" runat="server" Text="Colonia"></asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="Txt_Colonia" runat="server" style="width:93%;" Enabled="false"></asp:TextBox>
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
                                    <asp:ListItem Value="">&lt;TODOS&gt;</asp:ListItem>
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
                            <td style="width:15%;"><asp:Label ID="Lbl_Area_Donacion" runat="server" Text="Area de Donación"></asp:Label></td>
                            <td style="width:35%;"><asp:DropDownList ID="Cmb_Area_Donacion" runat="server" style="width:100%;"></asp:DropDownList></td>
                            <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Tipo_Bien" runat="server" Text="Tipo de Bien"></asp:Label></td>
                            <td style="width:35%;">
                                <asp:DropDownList ID="Cmb_Tipo_Bien" runat="server" style="width:100%;">
                                    <asp:ListItem Value="">&lt;TODOS&gt;</asp:ListItem>
                                    <asp:ListItem Value="PROPIO">PROPIO</asp:ListItem>
                                    <asp:ListItem Value="RENTADO">RENTADO</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%;"><asp:Label ID="Lbl_Sector" runat="server" Text="Sector"></asp:Label></td>
                            <td style="width:35%;"><asp:DropDownList ID="Cmb_Sector" runat="server" style="width:100%;"></asp:DropDownList></td>
                            <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Clasificacion_Zona" runat="server" Text="Clasif. Zona"></asp:Label></td>
                            <td style="width:35%;"><asp:DropDownList ID="Cmb_Clasificacion_Zona" runat="server" style="width:100%;"></asp:DropDownList></td>
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
                        <tr>
                            <td style="width:15%;"><asp:Label ID="Lbl_Estado" runat="server" Text="Estado"></asp:Label></td>
                            <td style="width:35%;">
                                <asp:DropDownList ID="Cmb_Estado" runat="server" style="width:100%;" >
                                    <asp:ListItem Value="">&lt;TODOS&gt;</asp:ListItem>
                                    <asp:ListItem Value="ALTA">ALTA</asp:ListItem>
                                    <asp:ListItem Value="BAJA">BAJA</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Libertad_Gravament" runat="server" Text="Libre Gravamen"></asp:Label></td>
                            <td style="width:35%;">
                                <asp:DropDownList ID="Cmb_Libertad_Gravament" runat="server" Width="100%">
                                    <asp:ListItem Value="">&lt;TODOS&gt;</asp:ListItem>
                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%;"><asp:Label ID="Lbl_Numero_Cuenta_Predial" runat="server" Text="Cuenta Predial"></asp:Label></td>
                            <td style="width:35%;">
                                <asp:HiddenField ID="Hdf_Cuenta_Predial_ID" runat="server" />
                                <asp:TextBox ID="Txt_Numero_Cuenta_Predial" runat="server" style="width:85%;" Enabled="false"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Buscar_Numero_Cuenta_Predial" runat="server" ToolTip="Buscar Cuenta Predial" AlternateText="Bucar Cuenta Predial" ImageUrl="~/paginas/imagenes/paginas/busqueda.png" OnClick="Btn_Buscar_Numero_Cuenta_Predial_Click" />
                            </td>
                            <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Tipo_Predio" runat="server" Text="Tipo de Predio"></asp:Label></td>
                            <td style="width:35%;"><asp:DropDownList ID="Cmb_Tipo_Predio" runat="server" style="width:100%;"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td style="width:15%;"><asp:Label ID="Lbl_Superficie_Desde" runat="server" Text="Superficie &#8805;"></asp:Label></td>
                            <td style="width:35%;">
                                <asp:TextBox ID="Txt_Superficie_Desde" runat="server" style="width:80%;"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Superficie_Desde" runat="server" Enabled="True" FilterType="Custom, Numbers" InvalidChars="'" TargetControlID="Txt_Superficie_Desde" ValidChars="." ></cc1:FilteredTextBoxExtender>
                                <asp:Label ID="Lbl_Construccion_Registrada_M2" runat="server" Text="[m2]"></asp:Label>
                            </td>
                            <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Superficie_Hasta" runat="server" Text="Superficie &#8804;"></asp:Label></td>
                            <td style="width:35%;">
                                <asp:TextBox ID="Txt_Superficie_Hasta" runat="server" style="width:80%;"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Superficie_Hasta" runat="server" Enabled="True" FilterType="Custom, Numbers" InvalidChars="'" TargetControlID="Txt_Superficie_Hasta" ValidChars="." ></cc1:FilteredTextBoxExtender>
                                <asp:Label ID="Lbl_Superficie_Hasta_M2" runat="server" Text="[m2]"></asp:Label>
                            </td>
                        </tr>  
                        <tr>
                            <td style="width:15%;"><asp:Label ID="Lbl_Valor_Comercial_Desde" runat="server" Text="Valor Comercial &#8805;"></asp:Label></td>
                            <td style="width:35%;">
                                <asp:Label ID="Lbl_Valor_Comercial_Desde_Pesos" runat="server" Text="$"></asp:Label>
                                <asp:TextBox ID="Txt_Valor_Comercial_Desde" runat="server" style="width:85%;"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Valor_Comercial_Desde" runat="server" Enabled="True" FilterType="Custom, Numbers" InvalidChars="'" TargetControlID="Txt_Valor_Comercial_Desde" ValidChars="." ></cc1:FilteredTextBoxExtender>
                            </td>
                            <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Valor_Comercial_Hasta" runat="server" Text="Valor Comercial &#8804;"></asp:Label></td>
                            <td style="width:35%; ">
                                <asp:Label ID="Lbl_Valor_Comercial_Hasta_Pesos" runat="server" Text="$"></asp:Label>
                                <asp:TextBox ID="Txt_Valor_Comercial_Hasta" runat="server" style="width:85%;"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FTE_Txt_Valor_Comercial_Hasta" runat="server" Enabled="True" FilterType="Custom, Numbers" InvalidChars="'" TargetControlID="Txt_Valor_Comercial_Hasta" ValidChars="." ></cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:15%;"><asp:Label ID="Lbl_Fecha_Registral_Inicio" runat="server" Text="Fecha Registral &#8805;"></asp:Label></td>
                            <td style="width:35%;">
                                <asp:TextBox ID="Txt_Fecha_Registro_Inicio" runat="server" style="width:80%;" AutoPostBack="true"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Fecha_Registro_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Registro_Inicio" runat="server" 
                                    TargetControlID="Txt_Fecha_Registro_Inicio" 
                                    PopupButtonID="Btn_Fecha_Registro_Inicio" Format="dd/MMM/yyyy" Enabled="True">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="Mee_Txt_Fecha_Registro_Inicio" Mask="99/LLL/9999" runat="server" MaskType="None" UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Registro_Inicio" Enabled="True" ClearMaskOnLostFocus="false"/>
                                <cc1:MaskedEditValidator  
                                ID="Mev_Mee_Txt_Fecha_Registro_Inicio" 
                                runat="server" 
                                ControlToValidate="Txt_Fecha_Registro_Inicio"
                                ControlExtender="Mee_Txt_Fecha_Registro_Inicio" 
                                EmptyValueMessage="La Fecha Inicial es obligatoria"
                                 InvalidValueMessage="Fecha Inicial Invalida" 
                                IsValidEmpty="true" 
                                TooltipMessage="Ingrese o Seleccione la Fecha Inicial"
                                Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                            </td>
                            <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Fecha_Registro_Fin" runat="server" Text="Fecha Registral &#8804;"></asp:Label></td>
                            <td style="width:35%;">
                                <asp:TextBox ID="Txt_Fecha_Registro_Fin" runat="server" style="width:80%;" AutoPostBack="true"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Fecha_Registro_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Registro_Fin" runat="server" 
                                    TargetControlID="Txt_Fecha_Registro_Fin" 
                                    PopupButtonID="Btn_Fecha_Registro_Fin" Format="dd/MMM/yyyy" Enabled="True">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="Mee_Txt_Fecha_Registro_Fin" Mask="99/LLL/9999" runat="server" MaskType="None" UserDateFormat="DayMonthYear" UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Registro_Fin" Enabled="True" ClearMaskOnLostFocus="false"/>
                                <cc1:MaskedEditValidator  
                                ID="Mev_Mee_Txt_Fecha_Registro_Fin" 
                                runat="server" 
                                ControlToValidate="Txt_Fecha_Registro_Fin"
                                ControlExtender="Mee_Txt_Fecha_Registro_Fin" 
                                EmptyValueMessage="La Fecha Final es obligatoria"
                                InvalidValueMessage="Fecha Final Invalida" 
                                IsValidEmpty="true" 
                                TooltipMessage="Ingrese o Seleccione la Fecha Final"
                                Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>
                            </td>
                        </tr>     
                        <tr>
                            <td style="width:15%;"><asp:Label ID="Lbl_Fecha_Escritura_Inicio" runat="server" Text="Fecha de Escritura &#8805;"></asp:Label></td>
                            <td style="width:35%;">
                                <asp:TextBox ID="Txt_Fecha_Escritura_Inicio" runat="server" style="width:80%;" AutoPostBack="true"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Fecha_Escritura_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Escritura_Inicio" runat="server" 
                                    TargetControlID="Txt_Fecha_Escritura_Inicio" 
                                    PopupButtonID="Btn_Fecha_Escritura_Inicio" Format="dd/MMM/yyyy" Enabled="True">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="Mee_Txt_Fecha_Escritura_Inicio" Mask="99/LLL/9999" runat="server" MaskType="None" UserDateFormat="DayMonthYear" 
                                UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Escritura_Inicio" Enabled="True" ClearMaskOnLostFocus="false"/>
                                <cc1:MaskedEditValidator  
                                ID="Mev_Mee_Txt_Fecha_Escritura_Inicio" 
                                runat="server" 
                                ControlToValidate="Txt_Fecha_Escritura_Inicio"
                                ControlExtender="Mee_Txt_Fecha_Escritura_Inicio" 
                                EmptyValueMessage="La Fecha Inicial es obligatoria"
                                 InvalidValueMessage="Fecha Inicial Invalida" 
                                IsValidEmpty="true" 
                                TooltipMessage="Ingrese o Seleccione la Fecha Inicial"
                                Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                              
                            </td>
                            <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Fecha_Escritura_Fin" runat="server" Text="Fecha de Escritura &#8804;"></asp:Label></td>
                            <td style="width:35%;">
                                <asp:TextBox ID="Txt_Fecha_Escritura_Fin" runat="server" style="width:80%;" AutoPostBack="true"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Fecha_Escritura_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Escritura_Fin" runat="server" 
                                    TargetControlID="Txt_Fecha_Escritura_Fin" 
                                    PopupButtonID="Btn_Fecha_Escritura_Fin" Format="dd/MMM/yyyy" Enabled="True">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="Mee_Txt_Fecha_Escritura_Fin" Mask="99/LLL/9999" runat="server" MaskType="None" UserDateFormat="DayMonthYear" 
                                UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Escritura_Fin" Enabled="True" ClearMaskOnLostFocus="false"/>
                                <cc1:MaskedEditValidator  
                                ID="Mev_Mee_Txt_Fecha_Escritura_Fin" 
                                runat="server" 
                                ControlToValidate="Txt_Fecha_Escritura_Fin"
                                ControlExtender="Mee_Txt_Fecha_Escritura_Fin" 
                                EmptyValueMessage="La Fecha Final es obligatoria"
                                 InvalidValueMessage="Fecha Final Invalida" 
                                IsValidEmpty="true" 
                                TooltipMessage="Ingrese o Seleccione la Fecha Final"
                                Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                                
                            </td>
                        </tr>     
                        <tr>
                            <td style="width:15%;"><asp:Label ID="Lbl_Fecha_Baja_Inicio" runat="server" Text="Fecha de Baja &#8805;"></asp:Label></td>
                            <td style="width:35%;">
                                <asp:TextBox ID="Txt_Fecha_Baja_Inicio" runat="server" style="width:80%;" AutoPostBack="true"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Fecha_Baja_Inicio" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Baja_Inicio" runat="server" 
                                    TargetControlID="Txt_Fecha_Baja_Inicio" 
                                    PopupButtonID="Btn_Fecha_Baja_Inicio" Format="dd/MMM/yyyy" Enabled="True">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="Mee_Txt_Fecha_Baja_Inicio" Mask="99/LLL/9999" runat="server" MaskType="None" UserDateFormat="DayMonthYear" 
                                UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Baja_Inicio" Enabled="True" ClearMaskOnLostFocus="false"/>
                                <cc1:MaskedEditValidator  
                                ID="Mev_Mee_Txt_Fecha_Baja_Inicio" 
                                runat="server" 
                                ControlToValidate="Txt_Fecha_Baja_Inicio"
                                ControlExtender="Mee_Txt_Fecha_Baja_Inicio" 
                                EmptyValueMessage="La Fecha Inicial es obligatoria"
                                 InvalidValueMessage="Fecha Inicial Invalida" 
                                IsValidEmpty="true" 
                                TooltipMessage="Ingrese o Seleccione la Fecha Inicial"
                                Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                                
                            </td>
                            <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Fecha_Baja_Fin" runat="server" Text="Fecha de Baja &#8804;"></asp:Label></td>
                            <td style="width:35%;">
                                <asp:TextBox ID="Txt_Fecha_Baja_Fin" runat="server" style="width:80%;" AutoPostBack="true"></asp:TextBox>
                                <asp:ImageButton ID="Btn_Fecha_Baja_Fin" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Baja_Fin" runat="server" 
                                    TargetControlID="Txt_Fecha_Baja_Fin" 
                                    PopupButtonID="Btn_Fecha_Baja_Fin" Format="dd/MMM/yyyy" Enabled="True">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="Mee_Txt_Fecha_Baja_Fin" Mask="99/LLL/9999" runat="server" MaskType="None" UserDateFormat="DayMonthYear" 
                                UserTimeFormat="None" Filtered="/" TargetControlID="Txt_Fecha_Baja_Fin" Enabled="True" ClearMaskOnLostFocus="false"/>
                                <cc1:MaskedEditValidator  
                                ID="Mev_Mee_Txt_Fecha_Baja_Fin" 
                                runat="server" 
                                ControlToValidate="Txt_Fecha_Baja_Fin"
                                ControlExtender="Mee_Txt_Fecha_Baja_Fin" 
                                EmptyValueMessage="La Fecha Final es obligatoria"
                                 InvalidValueMessage="Fecha Final Invalida" 
                                IsValidEmpty="true" 
                                TooltipMessage="Ingrese o Seleccione la Fecha Final"
                                Enabled="true" style="font-size:10px;background-color:#F0F8FF;color:Black;font-weight:bold;"/>                               
                            </td>
                        </tr>              
                    </table>   
                    <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                        <tr>
                            <td style="width:100%; text-align:center;">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:100%; text-align:left;">
                                <asp:CheckBox ID="Chk_Areas_Donacion" runat="server" Text="Áreas de Donación."></asp:CheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:100%; text-align:left;">
                                <asp:CheckBox ID="Chk_Estatus" runat="server" Text="Estatus."></asp:CheckBox>
                            </td>
                        </tr>
                    </table>  
                </center>                
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
                   <asp:Image ID="Image2" runat="server"  ImageUrl="~/paginas/imagenes/paginas/C_Tips_Md_N.png" />
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


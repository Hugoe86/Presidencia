<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Reporte_Activos_Fijos_Bienes_Inmuebles.aspx.cs" Inherits="paginas_Control_Patrimonial_Frm_Rpt_Reporte_Activos_Fijos_Bienes_Inmuebles" Title="Activos Fijos - Bienes Inmuebles" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

     <script type="text/javascript" language="javascript">
            function Limpiar_Ctlr_Campos(){
                document.getElementById("<%=Cmb_Tipo_Bien.ClientID%>").value=""; 
                document.getElementById("<%=Cmb_Origen.ClientID%>").value=""; 
                document.getElementById("<%=Cmb_Clase_Activo.ClientID%>").value=""; 
                document.getElementById("<%=Cmb_Estado.ClientID%>").value=""; 
                document.getElementById("<%=Cmb_Estatus.ClientID%>").value=""; 
                document.getElementById("<%=Txt_Fecha_Registro_Inicio.ClientID%>").value="__/___/____"; 
                document.getElementById("<%=Txt_Fecha_Registro_Fin.ClientID%>").value="__/___/____"; 
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
    <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
               <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                    
            </asp:UpdateProgress> 
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo" colspan="4">Activos Fijos [Bienes Inmuebles]</td>
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
                        <td align="left" style="width:50%;">&nbsp;</td>
                        <td align="right" style="width:50%;">
                            <asp:ImageButton ID="Btn_Limpiar_Campos" runat="server" ToolTip="Limpiar Campos" AlternateText="Limpiar Resguardante" ImageUrl="~/paginas/imagenes/paginas/icono_limpiar.png" Width="24px"  OnClientClick="javascript:return Limpiar_Ctlr_Campos();" />
                            &nbsp;
                        </td> 
                    </tr>                                     
                </table>  
                <br />  
                <table width="99%" class="estilo_fuente">                
                    <tr>
                        <td style="width:18%; text-align:left; ">
                            <asp:Label ID="Lbl_Tipo_Bienes" runat="server" Text="Tipo de Bien" ></asp:Label>
                        </td>
                        <td style="width:32%; text-align:left;">
                            <asp:DropDownList ID="Cmb_Tipo_Bien" runat="server" Width="100%">
                                <asp:ListItem Text="&lt; TODOS &gt;" Value=""></asp:ListItem>
                                <asp:ListItem Value="PROPIO">PROPIO</asp:ListItem>
                                <asp:ListItem Value="RENTADO">RENTADO</asp:ListItem>
                            </asp:DropDownList>                                                 
                        </td>
                        <td style="width:15%; text-align:right;"><asp:Label ID="Lbl_Origen" runat="server" Text="Origen"></asp:Label></td>
                        <td style="width:35%;"><asp:DropDownList ID="Cmb_Origen" runat="server" style="width:100%;"></asp:DropDownList></td>
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
                        <td style="width:15%;"><asp:Label ID="Lbl_Estado" runat="server" Text="Estado"></asp:Label></td>
                        <td style="width:35%;">
                            <asp:DropDownList ID="Cmb_Estado" runat="server" style="width:100%;" >
                                <asp:ListItem Value="">&lt;TODOS&gt;</asp:ListItem>
                                <asp:ListItem Value="ALTA">ALTA</asp:ListItem>
                                <asp:ListItem Value="BAJA">BAJA</asp:ListItem>
                            </asp:DropDownList>
                        </td>
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
            </div>    
        </ContentTemplate>        
    </asp:UpdatePanel>  
    <br />
    <table width="99%"  border="0" cellspacing="0" class="estilo_fuente">
        <tr align="center">
            <td style="text-align:right;">
                <asp:Button ID="Btn_Descargar_Excel" runat="server" OnClick="Btn_Descargar_Excel_Click" Text="Descargar Registros" style="border-style:outset; background-color:White; width:200px; height:50px; font-weight:bolder; font-size:medium; cursor:hand;" />   
                &nbsp;&nbsp;&nbsp;  
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />
</asp:Content>

